using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DPUruNet;
using System;
using UnityEngine.Events;

// create a event type
public class ErrorEvent : UnityEvent<string> { };
public class FingerEvent : UnityEvent<Fmd> { };

public class FingerprintCapture{
    private ReaderCollection _readers_;
    private Reader current_reader { get; set; }

    #region 指紋比對
    public const int PROBABILITY_ONE = 0x7fffffff;
    Constants.ResultCode captureResult;
    DataResult<Fmd> resultConversion;
    private Fmd firstFinger;
    private Fmd secondFinger;
    #endregion

    #region event
    public ErrorEvent error;
    public FingerEvent finger;
    #endregion

    // Constructor
    public FingerprintCapture()
    {
        error = new ErrorEvent();
        finger = new FingerEvent();

        try
        {
            // 取得所有裝置            
            _readers_ = ReaderCollection.GetReaders();

            // 設置第0個裝置為我的 current_reader(因為只有1台機器)
            current_reader = _readers_[0];
        }
        catch (Exception e)
        {
            error.Invoke(string.Format("Error in Start: {0}", e.Message));
        }

        if (!OpenReader())
        {
            error.Invoke("OpenReader failed.");
        }

        if (!StartCaptureAsync(OnCaptured))
        {
            error.Invoke("StartCaptureAsync failed.");
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
            error.Invoke("Error:  " + result);
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
            error.Invoke("Error:  " + ex.Message);
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

            finger.Invoke(resultConversion.Data);

        }
        catch (Exception e)
        {
            error.Invoke("Error in OnCaptured: " + e.Message);
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

    public void closeReader()
    {
        CancelCaptureAndCloseReader(OnCaptured);
    }
    #endregion
}
