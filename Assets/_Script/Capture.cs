using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DPUruNet;
using System;
using System.Text;
using UnityEngine.UI;

public class Capture : MonoBehaviour {
    public Renderer rend;    
    public Button close_reader;

    private ReaderCollection _readers;
    public Reader current_reader { get; set; }

    // 呈現指紋
    Fid.Fiv fiv;
    int count;

    // 指紋比對
    private const int PROBABILITY_ONE = 0x7fffffff;
    Constants.ResultCode captureResult;
    DataResult<Fmd> resultConversion;
    private Fmd firstFinger;
    private Fmd secondFinger;


    // Use this for initialization
    void Start () {
        fiv = null;
        count = 0;

        try
        {
            // 取得所有裝置
            _readers = ReaderCollection.GetReaders();

            // 設置第0個裝置為我的 current_reader(因為只有1台機器)
            current_reader = _readers[0];
        }
        catch (Exception e)
        {
            print("Error in Start: " + e.Message);
        }

        if (!OpenReader())
        {
            print("OpenReader failed.");
        }
        if (!StartCaptureAsync(OnCaptured))
        {
            print("StartCaptureAsync failed.");
        }

        close_reader.onClick.AddListener(() =>
        {
            CancelCaptureAndCloseReader(OnCaptured);
            print("CancelCaptureAndCloseReader");
        });
    }

    private void FixedUpdate()
    {
        #region 呈現指紋
        if (fiv != null)
        {
            // 呈現指紋
            rend.material.mainTexture = createFingerprint(fiv.RawImage, fiv.Width, fiv.Height);

            // 回復狀態，等待再次獲得指紋
            fiv = null;
        }
        #endregion

        //Fmd.DeserializeXml
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            print(string.Format("firstFinger: {0}", Fmd.SerializeXml(firstFinger)));
            
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            print(string.Format("resultConversion: {0}", resultConversion.Data.ToString()));
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            print(string.Format("captureResult: {0}", captureResult.ToString()));
        }
    }

    #region 開啟指紋機，並開始偵測
    /// <summary>
    /// Open a device and check result for errors.
    /// </summary>
    /// <returns>Returns true if successful; false if unsuccessful</returns>
    public bool OpenReader()
    {
        Constants.ResultCode result = Constants.ResultCode.DP_DEVICE_FAILURE;

        // Open reader
        result = current_reader.Open(Constants.CapturePriority.DP_PRIORITY_COOPERATIVE);

        if (result != Constants.ResultCode.DP_SUCCESS)
        {
            print("Error:  " + result);
            return false;
        }

        return true;
    }
        
    /// <summary>
    /// Hookup capture handler and start capture.
    /// </summary>
    /// <param name="OnCaptured">Delegate to hookup as handler of the On_Captured event</param>
    /// <returns>Returns true if successful; false if unsuccessful</returns>
    public bool StartCaptureAsync(Reader.CaptureCallback OnCaptured)
    {
        // Activate capture handler
        current_reader.On_Captured += new Reader.CaptureCallback(OnCaptured);

        // Call capture
        if (!CaptureFingerAsync())
        {
            return false;
        }

        return true;
    }
    
    /// <summary>
    /// Function to capture a finger. Always get status first and calibrate or wait if necessary.  Always check status and capture errors.
    /// </summary>
    /// <param name="fid"></param>
    /// <returns></returns>
    public bool CaptureFingerAsync()
    {
        try
        {
            //GetStatus();

            captureResult = current_reader.CaptureAsync(
                Constants.Formats.Fid.ANSI, 
                Constants.CaptureProcessing.DP_IMG_PROC_DEFAULT,
                current_reader.Capabilities.Resolutions[0]);

            if (captureResult != Constants.ResultCode.DP_SUCCESS)
            {
                throw new Exception("" + captureResult);
            }

            return true;
        }
        catch (Exception ex)
        {
            print("Error:  " + ex.Message);
            return false;
        }
    }
    #endregion

    #region 指紋機開啟狀態下執行
    /// <summary>
    /// Handler for when a fingerprint is captured.
    /// </summary>
    /// <param name="captureResult">contains info and data on the fingerprint capture</param>
    public void OnCaptured(CaptureResult captureResult)
    {
        try
        {
            // Check capture quality and throw an error if bad.
            if (!CheckCaptureResult(captureResult))
            {
                return;
            }

            resultConversion = FeatureExtraction.CreateFmdFromFid(
                captureResult.Data, 
                Constants.Formats.Fmd.ANSI);

            if (resultConversion.ResultCode != Constants.ResultCode.DP_SUCCESS)
            {
                throw new Exception(resultConversion.ResultCode.ToString());
            }

            if (count == 0)
            {
                firstFinger = resultConversion.Data;
                count++;
                print("Now place the same or a different finger on the reader.");
            }
            else if (count == 1)
            {
                secondFinger = resultConversion.Data;
                CompareResult compareResult = Comparison.Compare(firstFinger, 0, secondFinger, 0);
                if (compareResult.ResultCode != Constants.ResultCode.DP_SUCCESS)
                {
                    throw new Exception(compareResult.ResultCode.ToString());
                }

                print("Comparison resulted in a dissimilarity score of " + compareResult.Score.ToString() + (compareResult.Score < (PROBABILITY_ONE / 100000) ? " (fingerprints matched)" : " (fingerprints did not match)"));
                print("Place a finger on the reader.");
                count = 0;
            }

            // 成功取得指紋，則產生圖片
            foreach (Fid.Fiv fiv in captureResult.Data.Views)
            {
                this.fiv = fiv;
            }
        }
        catch (Exception e)
        {
            print("Error in OnCaptured: " + e.Message);
        }
    }

    /// <summary>
    /// Check quality of the resulting capture.
    /// </summary>
    public bool CheckCaptureResult(CaptureResult captureResult)
    {
        if (captureResult.Data == null || captureResult.ResultCode != Constants.ResultCode.DP_SUCCESS)
        {
            if (captureResult.ResultCode != Constants.ResultCode.DP_SUCCESS)
            {
                throw new Exception(captureResult.ResultCode.ToString());
            }

            // Send message if quality shows fake finger
            if ((captureResult.Quality != Constants.CaptureQuality.DP_QUALITY_CANCELED))
            {
                throw new Exception("Quality - " + captureResult.Quality);
            }
            return false;
        }
        return true;
    }
    
    // 產生指紋圖片
    public Texture2D createFingerprint(byte[] bytes, int width, int height)
    {
        Texture2D texture = new Texture2D(width, height/*, TextureFormat.RGB24, false*/);

        float b;
        for (int w = 0; w < width; w++) 
        {
            for (int h = 0; h < height; h++)
            {
                b = (bytes[width * w + h] / 255.0f);

                // bytes為單通道 gray_scale 數據，要轉換成通道的 rgb 數據，才能放入 texture
                Color color = new Color(b, b, b);
                texture.SetPixel(h, w, color);
            }
        }
        
        texture.Apply();

        return texture;
    }  

    // 查看 byte[] 數據
    void printByteData(byte[] bytes, int width, int height, int start_x, int start_y, int end_x, int end_y)
    {
        if(width < end_x)
        {
            end_x = width;
        }

        if (height < end_y)
        {
            end_y = height;
        }

        StringBuilder sb;
        int i, j;
        for(i = start_x; i < end_x; i++)
        {
            sb = new StringBuilder();

            for (j = start_y; j < end_y; j++)
            {
                sb.Append(string.Format("{0} ", bytes[width * i + j]));
            }

            print(sb.ToString());
        }
    }

    //IEnumerator display(Fid.Fiv fiv)
    //{
    //    print(string.Format("Width:{0}, Height:{1}", fiv.Width, fiv.Height));
    //    byte[] bytes = fiv.RawImage;
    //    print("bytes.Length: " + bytes.Length);
    //    print(string.Format("Width * Height: {0}", fiv.Width * fiv.Height));
    //    printByteData(fiv.RawImage, fiv.Width, fiv.Height, 200, 200);
    //    print("display");
    //    yield return new WaitForSeconds(Time.deltaTime);
    //}  
    #endregion

    #region 取消偵測，並關閉指紋機
    /// <summary>
    /// Cancel the capture and then close the reader.
    /// </summary>
    /// <param name="OnCaptured">Delegate to unhook as handler of the On_Captured event </param>
    public void CancelCaptureAndCloseReader(Reader.CaptureCallback OnCaptured)
    {
        if (current_reader != null)
        {
            current_reader.CancelCapture();

            // Dispose of reader handle and unhook reader events.
            current_reader.Dispose();

        }
    }
    #endregion
}
