namespace UareUSampleCSharp
{
    partial class Verification
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnBack = new System.Windows.Forms.Button();
            this.txtVerify = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(276, 229);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 23);
            this.btnBack.TabIndex = 4;
            this.btnBack.Text = "Back";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // txtVerify
            // 
            this.txtVerify.Location = new System.Drawing.Point(12, 10);
            this.txtVerify.Multiline = true;
            this.txtVerify.Name = "txtVerify";
            this.txtVerify.Size = new System.Drawing.Size(339, 213);
            this.txtVerify.TabIndex = 3;
            this.txtVerify.TextChanged += new System.EventHandler(this.txtVerify_TextChanged);
            // 
            // Verification
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(358, 273);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.txtVerify);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(374, 312);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(374, 312);
            this.Name = "Verification";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Verification";
            this.Closed += new System.EventHandler(this.Verification_Closed);
            this.Load += new System.EventHandler(this.Verification_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button btnBack;
        internal System.Windows.Forms.TextBox txtVerify;
    }
}