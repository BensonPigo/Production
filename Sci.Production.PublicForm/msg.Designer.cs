namespace Sci.Production.PublicForm
{
    partial class Msg
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
            this.picInfo = new Sci.Win.UI.PictureBox();
            this.editBoxMsg = new System.Windows.Forms.TextBox();
            this.button1 = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // picInfo
            // 
            this.picInfo.Image = null;
            this.picInfo.Location = new System.Drawing.Point(12, 12);
            this.picInfo.Name = "picInfo";
            this.picInfo.Size = new System.Drawing.Size(56, 50);
            this.picInfo.TabIndex = 1;
            this.picInfo.TabStop = false;
            this.picInfo.WaitOnLoad = true;
            // 
            // editBoxMsg
            // 
            this.editBoxMsg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editBoxMsg.BackColor = System.Drawing.SystemColors.Control;
            this.editBoxMsg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.editBoxMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F);
            this.editBoxMsg.Location = new System.Drawing.Point(74, 12);
            this.editBoxMsg.Multiline = true;
            this.editBoxMsg.Name = "editBoxMsg";
            this.editBoxMsg.ReadOnly = true;
            this.editBoxMsg.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.editBoxMsg.Size = new System.Drawing.Size(284, 92);
            this.editBoxMsg.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.button1.Location = new System.Drawing.Point(140, 110);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 30);
            this.button1.TabIndex = 5;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // msg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 147);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.editBoxMsg);
            this.Controls.Add(this.picInfo);
            this.Name = "msg";
            this.Text = "Information";
            ((System.ComponentModel.ISupportInitialize)(this.picInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.PictureBox picInfo;
        private System.Windows.Forms.TextBox editBoxMsg;
        private Win.UI.Button button1;
    }
}