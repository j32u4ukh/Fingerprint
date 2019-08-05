﻿using System;
using System.Windows.Forms;
using DPUruNet;
using System.Collections;
using System.Diagnostics;



namespace UareUSampleCSharp
{
    public partial class EnrollmentControl : Form
    {
        /// <summary>
    /// Holds the main form with many functions common to all of SDK actions.
    /// </summary>
        public Form_Main _sender;

        public int _int_Q;

        private DPCtlUruNet.EnrollmentControl _enrollmentControl;
        
        public EnrollmentControl()
        {
            InitializeComponent();
        }
        
        /// <summary>
    /// Initialize the form.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>
    /// *****
        private void EnrollmentControl_Load(object sender, EventArgs e)
        {
            Debug.WriteLine(_sender.Fmds.Count);

            if (_enrollmentControl != null)
            {
                _enrollmentControl.Reader = _sender.CurrentReader;

            }
            else
            {
                //產生的介面
                _enrollmentControl = new DPCtlUruNet.EnrollmentControl(_sender.CurrentReader, Constants.CapturePriority.DP_PRIORITY_COOPERATIVE);
                _enrollmentControl.BackColor = System.Drawing.SystemColors.Window;
                _enrollmentControl.Location = new System.Drawing.Point(3, 3);
                _enrollmentControl.Name = "ctlEnrollmentControl";
                _enrollmentControl.Size = new System.Drawing.Size(482, 346);
                _enrollmentControl.TabIndex = 0;
                _enrollmentControl.OnCancel += new DPCtlUruNet.EnrollmentControl.CancelEnrollment(this.enrollment_OnCancel);
                _enrollmentControl.OnCaptured += new DPCtlUruNet.EnrollmentControl.FingerprintCaptured(this.enrollment_OnCaptured);
                _enrollmentControl.OnDelete += new DPCtlUruNet.EnrollmentControl.DeleteEnrollment(this.enrollment_OnDelete);
                _enrollmentControl.OnEnroll += new DPCtlUruNet.EnrollmentControl.FinishEnrollment(this.enrollment_OnEnroll);
                _enrollmentControl.OnStartEnroll += new DPCtlUruNet.EnrollmentControl.StartEnrollment(this.enrollment_OnStartEnroll);
                Debug.WriteLine("EnrollmentControl_Load = else ");
            
            }
   
            this.Controls.Add(_enrollmentControl);
        }
        //取消註冊
        #region Enrollment Control Events
        private void enrollment_OnCancel(DPCtlUruNet.EnrollmentControl enrollmentControl, Constants.ResultCode result, int fingerPosition)
        {

            if (enrollmentControl.Reader != null)
            {
             
                SendMessage("OnCancel:  " + enrollmentControl.Reader.Description.Name + ", finger " + fingerPosition);
            }
            else
            {
                SendMessage("OnCancel:  No Reader Connected, finger " + fingerPosition);
            }

            btnCancel.Enabled = false;
        }
        //***** 按壓次數
        private void enrollment_OnCaptured(DPCtlUruNet.EnrollmentControl enrollmentControl, CaptureResult captureResult, int fingerPosition)
        {
           // Debug.WriteLine(_enrollmentControl.EnrolledFingerMask);

            if (enrollmentControl.Reader != null)
            {
                _int_Q += 1;
                Debug.WriteLine(captureResult.ResultCode);
                SendMessage("OnCaptured:  " + enrollmentControl.Reader.Description.Name + ", finger " + fingerPosition + ", quality " + captureResult.Quality.ToString());
            }
            else
            {
                SendMessage("OnCaptured:  No Reader Connected, finger " + fingerPosition);
            }
            //產生指紋圖
            if (captureResult.ResultCode != Constants.ResultCode.DP_SUCCESS)
            {
                Debug.WriteLine("captureResult.ResultCode != Constants.ResultCode.DP_SUCCESS" + captureResult.ResultCode);

                if (_sender.CurrentReader != null)
                {
                    Debug.WriteLine("_sender.CurrentReader != null" + _sender.CurrentReader);

                    _sender.CurrentReader.Dispose();
                    _sender.CurrentReader = null;
                }


                // Disconnect reader from enrollment control
                _enrollmentControl.Reader = null; 
                                
                MessageBox.Show("Error:  " + captureResult.ResultCode);
                btnCancel.Enabled = false;
            }
            else
            {
                if (captureResult.Data != null)
                {
                    Debug.WriteLine("captureResult.Data != null" + captureResult.Data);
                    foreach (Fid.Fiv fiv in captureResult.Data.Views)
                    {
                        pbFingerprint.Image = _sender.CreateBitmap(fiv.RawImage, fiv.Width, fiv.Height);
                    }
                }
            }
        }
        private void enrollment_OnDelete(DPCtlUruNet.EnrollmentControl enrollmentControl, Constants.ResultCode result, int fingerPosition)
        {
            if (enrollmentControl.Reader != null)
            {
                SendMessage("OnDelete:  " + enrollmentControl.Reader.Description.Name + ", finger " + fingerPosition);
                SendMessage("Enrolled Finger Mask: " + _enrollmentControl.EnrolledFingerMask);
                SendMessage("Disabled Finger Mask: " + _enrollmentControl.DisabledFingerMask);
            }
            else
            {
                SendMessage("OnDelete:  No Reader Connected, finger " + fingerPosition);
            }

            _sender.Fmds.Remove(fingerPosition);

            if (_sender.Fmds.Count == 0)
            {
                _sender.btnIdentificationControl.Enabled = false;
            }
        }
        private void enrollment_OnEnroll(DPCtlUruNet.EnrollmentControl enrollmentControl, DataResult<Fmd> result, int fingerPosition)
        {
            if (enrollmentControl.Reader != null)
            {
                SendMessage("OnEnroll:  " + enrollmentControl.Reader.Description.Name + ", finger " + fingerPosition);
                SendMessage("Enrolled Finger Mask: " + _enrollmentControl.EnrolledFingerMask);
                SendMessage("Disabled Finger Mask: " + _enrollmentControl.DisabledFingerMask);
            }
            else
            {
                SendMessage("OnEnroll:  No Reader Connected, finger " + fingerPosition);
            }

            if (result != null && result.Data != null)
            {
                _sender.Fmds.Add(fingerPosition, result.Data);
            }

            btnCancel.Enabled = false;

            _sender.btnIdentificationControl.Enabled = true;
        }
        //開始註冊
        private void enrollment_OnStartEnroll(DPCtlUruNet.EnrollmentControl enrollmentControl, Constants.ResultCode result, int fingerPosition)
        {
            if (enrollmentControl.Reader != null)
            {
                Debug.WriteLine("enrollment_OnStartEnroll = if");
                SendMessage("OnStartEnroll:  " + enrollmentControl.Reader.Description.Name + ", finger " + fingerPosition);
            }
            else
            {
                Debug.WriteLine("enrollment_OnStartEnroll = else");
                SendMessage("OnStartEnroll:  No Reader Connected, finger " + fingerPosition);
            }

            btnCancel.Enabled = true;
        }
        #endregion
        
        /// <summary>
    /// Cancel enrollment when window is closed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>
    /// 取消註冊
        private void btnCancel_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result;

            result = MessageBox.Show("Are you sure you want to cancel this enrollment?", "Are You Sure?", buttons, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                _enrollmentControl.Cancel();
            }
        }
        
        /// <summary>
    /// Close window.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        /// <summary>
    /// Cancel enrollment when window is closed.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <remarks></remarks>
        private void frmEnrollment_Closed(object sender, EventArgs e)
        {
            _enrollmentControl.Cancel();
        }

        private void SendMessage(string message)
        {
            txtMessage.Text += message + "\r\n\r\n";
            txtMessage.SelectionStart = txtMessage.TextLength;
            txtMessage.ScrollToCaret();
        }
    }
}
