using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DPUruNet;
using System.Diagnostics;

namespace UareUSampleCSharp
{
    public partial class Enrollment : Form
    {
        /// <summary>
        /// Holds the main form with many functions common to all of SDK actions.
        /// </summary>
        public Form_Main _sender;

        public String tempFingerPrint;

        public int ConversionFormate;

        List<Fmd> preenrollmentFmds;
        int count;

        public Enrollment()
        {

            InitializeComponent();
        }

        /// <summary>
        /// Initialize the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enrollment_Load(object sender, System.EventArgs e)
        {
            txtEnroll.Text = string.Empty;
            preenrollmentFmds = new List<Fmd>();
            count = 0;

            SendMessage(Action.SendMessage, "Place a finger on the reader.");

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
            //if (tempFingerPrint_Last == tempFingerPrint)
            //{
            //    tempFingerPrint_Last = tempFingerPrint;
            //    Debug.WriteLine(tempFingerPrint_Last);

            //}

            //Debug.WriteLine(tempFingerPrint);

            //Debug.WriteLine("00");

            try
            {
                // Check capture quality and throw an error if bad.
                if (!_sender.CheckCaptureResult(captureResult)) return;

                count++;
                Debug.WriteLine(count);
                //存取至數據庫的資料
                DataResult<Fmd> resultConversion = FeatureExtraction.CreateFmdFromFid(captureResult.Data, Constants.Formats.Fmd.ANSI);
                //tempFingerPrint = Fmd.SerializeXml(resultConversion.Data);
                tempFingerPrint = Fmd.SerializeXml(resultConversion.Data);

                //Fmd val = Fmd.DeserializeXml(tempFingerPrint);

                //Fmd fmd = resultConversion.Data;
                //Debug.WriteLine(tempFingerPrint);

                //Fmd.DeserializeXml(tempFingerPrint);
                
                Fmd fmd = resultConversion.Data;
                string base64Fmd = Convert.ToBase64String(fmd.Bytes);

                //Debug.WriteLine(fmd);
                Debug.WriteLine(base64Fmd);
                //byte resultFromDB = // get bytes from database
                // ConversionFormate = Convert.ToInt32(Constants.Formats.Fmd.ANSI);
                //Fmd obj = new Fmd(resultFromDB, ConversionFormate, Constants.WRAPPER_VERSION);
                SendMessage(Action.SendMessage, "A finger was captured.  \r\nCount:  " + (count));

                if (resultConversion.ResultCode != Constants.ResultCode.DP_SUCCESS)
                {
                    _sender.Reset = true;
                    throw new Exception(resultConversion.ResultCode.ToString());
                }
                //保存指紋進resultConversion.Data內 數據庫
                preenrollmentFmds.Add(resultConversion.Data);
               
                //tempFingerPrint = Fmd.DeserializeXml(resultConversion.Data);

                //Debug.WriteLine(tempFingerPrint);

                if (count >= 4)
                {
                    Debug.WriteLine("???");
                    //**註冊是否成功
                    DataResult<Fmd> resultEnrollment = DPUruNet.Enrollment.CreateEnrollmentFmd(Constants.Formats.Fmd.ANSI, preenrollmentFmds);
                    Debug.WriteLine(resultEnrollment.ResultCode);

                    //配對成功
                    if (resultEnrollment.ResultCode == Constants.ResultCode.DP_SUCCESS)
                    {
                        SendMessage(Action.SendMessage, "配對成功");
                        SendMessage(Action.SendMessage, "Place a finger on the reader.");
                        //刪除之前的指紋資料庫
                        preenrollmentFmds.Clear();
                        count = 0;
                        return;
                    }
                    //無法配對成功
                    else if (resultEnrollment.ResultCode == Constants.ResultCode.DP_ENROLLMENT_INVALID_SET)
                    {
                        SendMessage(Action.SendMessage, "配對失敗");
                        SendMessage(Action.SendMessage, "Place a finger on the reader.");
                        //刪除之前的指紋資料庫
                        preenrollmentFmds.Clear();
                        count = 0;
                        return;
                    }
                   // Debug.WriteLine(resultEnrollment.ResultCode);

                }

                SendMessage(Action.SendMessage, "Now place the same finger on the reader.");
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
        private void Enrollment_Closed(object sender, System.EventArgs e)
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
                if (this.txtEnroll.InvokeRequired)
                {
                    SendMessageCallback d = new SendMessageCallback(SendMessage);
                    this.Invoke(d, new object[] { action, payload });
                }
                else
                {
                    switch (action)
                    {
                        case Action.SendMessage:
                            txtEnroll.Text += payload + "\r\n\r\n";
                            txtEnroll.SelectionStart = txtEnroll.TextLength;
                            txtEnroll.ScrollToCaret();
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        #endregion

        private void txtEnroll_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
