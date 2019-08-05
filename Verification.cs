using System;
using System.Windows.Forms;
using DPUruNet;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace UareUSampleCSharp
{
    public partial class Verification : Form
    {
        /// <summary>
        /// Holds the main form with many functions common to all of SDK actions.
        /// </summary>
        public Form_Main _sender;

        private const int PROBABILITY_ONE = 0x7fffffff;
        private Fmd firstFinger;
        private Fmd MidFinger;
        private Fmd Mid2Finger;
        private Fmd secondFinger;
        private Fmd MySql_Fmd;
        private int count;
        MySqlConnection conn = null;

        public Verification()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Verification_Load(object sender, System.EventArgs e)
        {
            string cs = "Server=localhost;userid=root;password=123456789;database=flexcodesdk";
            conn = new MySqlConnection(cs);
            conn.Open();
            txtVerify.Text = string.Empty;
            firstFinger = null;
            secondFinger = null;
            count = 0;

            SendMessage(Action.SendMessage, "請按壓任意手指");

            if (!_sender.OpenReader())
            {
                this.Close();
            }

            if (!_sender.StartCaptureAsync(this.OnCaptured))
            {
                this.Close();
            }
        }

        /// <summary>
        /// Handler for when a fingerprint is captured.
        /// </summary>
        /// <param name="captureResult">contains info and data on the fingerprint capture</param>
        private void OnCaptured(CaptureResult captureResult)
        {
           

            //cmd.CommandText = "INSERT INTO exp_t(EmpID , EmpName , EmpTemplate) VALUES()";
            //
            try
            {
                // Check capture quality and throw an error if bad.
                if (!_sender.CheckCaptureResult(captureResult)) return;

                SendMessage(Action.SendMessage, "A finger was captured.");

                DataResult<Fmd> resultConversion = FeatureExtraction.CreateFmdFromFid(captureResult.Data, Constants.Formats.Fmd.ANSI);
                if (resultConversion.ResultCode != Constants.ResultCode.DP_SUCCESS)
                {
                    _sender.Reset = true;
                    throw new Exception(resultConversion.ResultCode.ToString());
                }

                if (count == 0)
                {
                    firstFinger = resultConversion.Data;
                    //指紋數據轉為Bytes型態數據
                    string base64Fmd = Fmd.SerializeXml(firstFinger);
                    //指紋數據上傳至數據庫
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "INSERT INTO exp_t(EmpTemplate) VALUES('" + base64Fmd + "')";
                    cmd.ExecuteNonQuery();

                    count += 1;
                    SendMessage(Action.SendMessage, "第二次按壓");
                  

                }
                else if (count == 1)
                {
                    Debug.WriteLine("--------------------------------------------");
                    MidFinger = resultConversion.Data;
                    //MySql回傳資料
                    string sql = "SELECT EmpTemplate FROM exp_t";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    string EmpTemplate = (string)cmd.ExecuteScalar();
                    Debug.WriteLine("EmpTemplate = " + EmpTemplate);

                    MySql_Fmd = Fmd.DeserializeXml(EmpTemplate);
                    string base64Fmd = Fmd.SerializeXml(MidFinger);
                  
                    SendMessage(Action.SendMessage, "第三次按壓");

                    //第一指和第二指的比較
                    CompareResult compareResult = Comparison.Compare(firstFinger, 0, MidFinger, 0);
                    count += 1;

                    Debug.WriteLine(" 第二次按壓和第一次按壓比較 = " + compareResult.Score + (compareResult.Score < (PROBABILITY_ONE / 100000) ? " (認證成功)" : " (認證失敗)"));
                    Debug.WriteLine("--------------------------------------------");

                    if (compareResult.ResultCode != Constants.ResultCode.DP_SUCCESS)
                    {
                        _sender.Reset = true;
                        throw new Exception(compareResult.ResultCode.ToString());
                    }
                    SendMessage(Action.SendMessage, "你指紋的誤差值為: " + compareResult.Score.ToString() + (compareResult.Score < (PROBABILITY_ONE / 100000) ? " (認證成功)" : " (認證失敗)"));
                }
                else if (count == 2)
                {
                    ///***MySql回傳資料未完成
                  
                    //MySql_Fmd = Convert.ToBase64String();
                    //byte[] MySql_Fmd = Convert.FromBase64String(EmpTemplate);
                    //MySql_Fmd
                    //string base64Fmd_2 = Convert.ToBase64String(MySql_Fmd);
                    //Debug.WriteLine(base64Fmd_2);

                    //查詢資料庫全部數據
                    //string sql = "SELECT * FROM exp_t EmpTemplate";
                    //MySqlCommand cmd = new MySqlCommand(sql, conn);

                    //MySqlDataReader data = cmd.ExecuteReader();

                    //Console.WriteLine("是否查到資料:{0}", data.HasRows);

                    //Console.WriteLine("欄位數:{0}", data.FieldCount);

                    Mid2Finger = resultConversion.Data;
                    SendMessage(Action.SendMessage, "第四次按壓");
                    //第一指和第三指比較
                    CompareResult compareResult_2 = Comparison.Compare(Mid2Finger, 0, firstFinger, 0);
                    //第二指和第三指比較
                    CompareResult compareResult_3 = Comparison.Compare(Mid2Finger, 0, MidFinger, 0);
                    
                    Debug.WriteLine(" 第三次按壓和第一次按壓比較 =" + compareResult_2.Score + (compareResult_2.Score < (PROBABILITY_ONE / 100000) ? " (認證成功)" : " (認證失敗)"));

                    Debug.WriteLine(" 第三次按壓和第二次按壓比較 =" + compareResult_3.Score + (compareResult_3.Score < (PROBABILITY_ONE / 100000) ? " (認證成功)" : " (認證失敗)"));
                    Debug.WriteLine("--------------------------------------------");

                    if (compareResult_2.ResultCode != Constants.ResultCode.DP_SUCCESS)
                    {
                        _sender.Reset = true;
                        throw new Exception(compareResult_2.ResultCode.ToString());
                    }
                    if (compareResult_3.ResultCode != Constants.ResultCode.DP_SUCCESS)
                    {
                        _sender.Reset = true;
                        throw new Exception(compareResult_3.ResultCode.ToString());
                    }
                    count += 1;

                    SendMessage(Action.SendMessage, "你指紋的誤差值為: " + compareResult_2.Score.ToString() + (compareResult_2.Score < (PROBABILITY_ONE / 100000) ? " (認證成功)" : " (認證失敗)"));
                    SendMessage(Action.SendMessage, "你指紋的誤差值為: " + compareResult_3.Score.ToString() + (compareResult_3.Score < (PROBABILITY_ONE / 100000) ? " (認證成功)" : " (認證失敗)"));
                }
                else if (count == 3)
                {
                    secondFinger = resultConversion.Data;
                    SendMessage(Action.SendMessage, "結果");
                    //告訴她比較結果
                    Fmd[] Test_fmd = new Fmd[3];
                    Test_fmd[0] = firstFinger;
                    Test_fmd[1] = MidFinger;
                    Test_fmd[2] = Mid2Finger;
                    //鑑定
                    IdentifyResult identifyResult = Comparison.Identify(firstFinger, 0, Test_fmd, PROBABILITY_ONE, 3);
                    int thresholdScore = PROBABILITY_ONE * 1 / 100000;
                    //Debug.WriteLine("compareResult.ResultCode = " + identifyResult.Indexes.Length.ToString());
                    //比較 按壓2次做一次比較前後正確性 給予評斷分數
                    //第四指和第一指比較
                    CompareResult compareResult_4 = Comparison.Compare(secondFinger, 0, firstFinger, 0);
                    
                    Debug.WriteLine(" 第四次按壓和第一次按壓比較 =" + compareResult_4.Score + (compareResult_4.Score < (PROBABILITY_ONE / 100000) ? " (認證成功)" : " (認證失敗)"));
                    //第四指和第二指比較
                    CompareResult compareResult_5 = Comparison.Compare(secondFinger, 0, MidFinger, 0);

                    Debug.WriteLine(" 第四次按壓和第二次按壓比較 =" + compareResult_5.Score + (compareResult_5.Score < (PROBABILITY_ONE / 100000) ? " (認證成功)" : " (認證失敗)"));
                    //第四指和第三指比較
                    CompareResult compareResult_6 = Comparison.Compare(secondFinger, 0, Mid2Finger, 0);

                    Debug.WriteLine(" 第四次按壓和第三次按壓比較 =" + compareResult_6.Score + (compareResult_6.Score < (PROBABILITY_ONE / 100000) ? " (認證成功)" : " (認證失敗)"));



                    if (compareResult_4.ResultCode != Constants.ResultCode.DP_SUCCESS)
                    {
                        _sender.Reset = true;
                        throw new Exception(compareResult_4.ResultCode.ToString());
                    }

                    if (compareResult_5.ResultCode != Constants.ResultCode.DP_SUCCESS)
                    {
                        _sender.Reset = true;
                        throw new Exception(compareResult_5.ResultCode.ToString());
                    }


                    if (compareResult_6.ResultCode != Constants.ResultCode.DP_SUCCESS)
                    {
                        _sender.Reset = true;
                        throw new Exception(compareResult_6.ResultCode.ToString());
                    }
                    //判斷數值越高 越不像
                    SendMessage(Action.SendMessage, "你指紋的誤差值為: " + compareResult_4.Score.ToString() + (compareResult_4.Score < (PROBABILITY_ONE / 100000) ? " (認證成功)" : " (認證失敗)"));
                    SendMessage(Action.SendMessage, "你指紋的誤差值為: " + compareResult_5.Score.ToString() + (compareResult_5.Score < (PROBABILITY_ONE / 100000) ? " (認證成功)" : " (認證失敗)"));
                    SendMessage(Action.SendMessage, "你指紋的誤差值為: " + compareResult_6.Score.ToString() + (compareResult_6.Score < (PROBABILITY_ONE / 100000) ? " (認證成功)" : " (認證失敗)"));

                    SendMessage(Action.SendMessage, "你過程中按壓的正確次數為: " + identifyResult.Indexes.Length.ToString());

                    SendMessage(Action.SendMessage, "請再次按壓重頭開始");
                    Debug.WriteLine("--------------------------------------------");

                    count = 0;
                }
            }
            catch (Exception ex)
            {
                // Send error message, then close form
                SendMessage(Action.SendMessage, "Error:  " + ex.Message);                
            }
        }

        /// <summary>
        /// Close window.
        /// </summary>
        private void btnBack_Click(System.Object sender, System.EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Close window.
        /// </summary>
        private void Verification_Closed(object sender, System.EventArgs e)
        {
            _sender.CancelCaptureAndCloseReader(this.OnCaptured);
        }

        #region SendMessage
        private enum Action
        {
            SendMessage
        }
        private delegate void SendMessageCallback(Action action, string payload);
        private void SendMessage(Action action, string payload)
        {
            try
            {
                if (this.txtVerify.InvokeRequired)
                {
                    SendMessageCallback d = new SendMessageCallback(SendMessage);
                    this.Invoke(d, new object[] { action, payload });
                }
                else
                {
                    switch (action)
                    {
                        case Action.SendMessage:
                            txtVerify.Text += payload + "\r\n\r\n";
                            txtVerify.SelectionStart = txtVerify.TextLength;
                            txtVerify.ScrollToCaret();
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        private void txtVerify_TextChanged(object sender, EventArgs e)
        {

        }
    }
}