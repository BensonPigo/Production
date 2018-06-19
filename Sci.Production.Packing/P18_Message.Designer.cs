namespace Sci.Production.Packing
{
    partial class P18_Message
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
            this.txt_msg = new System.Windows.Forms.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // txt_msg
            // 
            this.txt_msg.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_msg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txt_msg.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_msg.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txt_msg.Location = new System.Drawing.Point(12, 12);
            this.txt_msg.Multiline = true;
            this.txt_msg.Name = "txt_msg";
            this.txt_msg.ReadOnly = true;
            this.txt_msg.Size = new System.Drawing.Size(260, 56);
            this.txt_msg.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoHeight = true;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 0);
            this.label1.TabIndex = 3;
            // 
            // P18_Message
            // 
            this.ClientSize = new System.Drawing.Size(284, 80);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_msg);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "P18_Message";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Warning";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txt_msg;
        private Win.UI.Label label1;
    }
}
