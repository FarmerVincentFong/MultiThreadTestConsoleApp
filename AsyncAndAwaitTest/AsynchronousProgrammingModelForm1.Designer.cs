namespace AsyncAndAwaitTest
{
    partial class AsynchronousProgrammingModelForm1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbxUrl = new System.Windows.Forms.TextBox();
            this.btnDownload1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rtbState = new System.Windows.Forms.RichTextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "File Url:";
            // 
            // tbxUrl
            // 
            this.tbxUrl.Location = new System.Drawing.Point(70, 6);
            this.tbxUrl.Name = "tbxUrl";
            this.tbxUrl.Size = new System.Drawing.Size(515, 21);
            this.tbxUrl.TabIndex = 1;
            // 
            // btnDownload1
            // 
            this.btnDownload1.Location = new System.Drawing.Point(591, 4);
            this.btnDownload1.Name = "btnDownload1";
            this.btnDownload1.Size = new System.Drawing.Size(75, 23);
            this.btnDownload1.TabIndex = 2;
            this.btnDownload1.Text = "download";
            this.btnDownload1.UseVisualStyleBackColor = true;
            this.btnDownload1.Click += new System.EventHandler(this.dlbtn1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rtbState);
            this.groupBox1.Location = new System.Drawing.Point(13, 34);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(653, 233);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "State Info";
            // 
            // rtbState
            // 
            this.rtbState.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbState.Location = new System.Drawing.Point(3, 17);
            this.rtbState.Name = "rtbState";
            this.rtbState.Size = new System.Drawing.Size(647, 213);
            this.rtbState.TabIndex = 0;
            this.rtbState.Text = "";
            // 
            // AsynchronousProgrammingModelForm1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(678, 279);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnDownload1);
            this.Controls.Add(this.tbxUrl);
            this.Controls.Add(this.label1);
            this.Name = "AsynchronousProgrammingModelForm1";
            this.Text = "AsynchronousProgrammingModelForm1";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbxUrl;
        private System.Windows.Forms.Button btnDownload1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox rtbState;
    }
}