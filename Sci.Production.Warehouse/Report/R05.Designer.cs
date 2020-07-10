namespace Sci.Production.Warehouse
{
    partial class R05
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.dateIssue = new Sci.Win.UI.DateRange();
            this.txtSP1 = new Sci.Win.UI.TextBox();
            this.label6 = new Sci.Win.UI.Label();
            this.txtSP2 = new Sci.Win.UI.TextBox();
            this.txtMdivision1 = new Sci.Production.Class.TxtMdivision();
            this.txtfactory1 = new Sci.Production.Class.Txtfactory();
            this.radioTransferIn = new Sci.Win.UI.RadioButton();
            this.radioTransferOut = new Sci.Win.UI.RadioButton();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(399, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(399, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(399, 84);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.SkyBlue;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 23);
            this.label1.TabIndex = 94;
            this.label1.Text = "Issue Date";
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 23);
            this.label2.TabIndex = 95;
            this.label2.Text = "SP#";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 23);
            this.label3.TabIndex = 96;
            this.label3.Text = "M";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 23);
            this.label4.TabIndex = 97;
            this.label4.Text = "Factory";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 149);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 23);
            this.label5.TabIndex = 98;
            this.label5.Text = "Transfer Type";
            // 
            // dateIssue
            // 
            // 
            // 
            // 
            this.dateIssue.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateIssue.DateBox1.Name = "";
            this.dateIssue.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateIssue.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateIssue.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateIssue.DateBox2.Name = "";
            this.dateIssue.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateIssue.DateBox2.TabIndex = 1;
            this.dateIssue.Location = new System.Drawing.Point(105, 9);
            this.dateIssue.Name = "dateIssue";
            this.dateIssue.Size = new System.Drawing.Size(280, 23);
            this.dateIssue.TabIndex = 99;
            // 
            // txtSP1
            // 
            this.txtSP1.BackColor = System.Drawing.Color.White;
            this.txtSP1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP1.Location = new System.Drawing.Point(105, 44);
            this.txtSP1.Name = "txtSP1";
            this.txtSP1.Size = new System.Drawing.Size(100, 23);
            this.txtSP1.TabIndex = 100;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(208, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 23);
            this.label6.TabIndex = 101;
            this.label6.Text = "～";
            // 
            // txtSP2
            // 
            this.txtSP2.BackColor = System.Drawing.Color.White;
            this.txtSP2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP2.Location = new System.Drawing.Point(234, 44);
            this.txtSP2.Name = "txtSP2";
            this.txtSP2.Size = new System.Drawing.Size(100, 23);
            this.txtSP2.TabIndex = 102;
            // 
            // txtMdivision1
            // 
            this.txtMdivision1.BackColor = System.Drawing.Color.White;
            this.txtMdivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision1.Location = new System.Drawing.Point(105, 79);
            this.txtMdivision1.Name = "txtMdivision1";
            this.txtMdivision1.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision1.TabIndex = 103;
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.FilteMDivision = false;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.IssupportJunk = false;
            this.txtfactory1.Location = new System.Drawing.Point(105, 114);
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.Size = new System.Drawing.Size(66, 23);
            this.txtfactory1.TabIndex = 104;
            // 
            // radioTransferIn
            // 
            this.radioTransferIn.AutoSize = true;
            this.radioTransferIn.Checked = true;
            this.radioTransferIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioTransferIn.Location = new System.Drawing.Point(105, 149);
            this.radioTransferIn.Name = "radioTransferIn";
            this.radioTransferIn.Size = new System.Drawing.Size(95, 21);
            this.radioTransferIn.TabIndex = 105;
            this.radioTransferIn.TabStop = true;
            this.radioTransferIn.Text = "Transfer In";
            this.radioTransferIn.UseVisualStyleBackColor = true;
            // 
            // radioTransferOut
            // 
            this.radioTransferOut.AutoSize = true;
            this.radioTransferOut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioTransferOut.Location = new System.Drawing.Point(218, 149);
            this.radioTransferOut.Name = "radioTransferOut";
            this.radioTransferOut.Size = new System.Drawing.Size(107, 21);
            this.radioTransferOut.TabIndex = 106;
            this.radioTransferOut.Text = "Transfer Out";
            this.radioTransferOut.UseVisualStyleBackColor = true;
            // 
            // R05
            // 
            this.ClientSize = new System.Drawing.Size(491, 206);
            this.Controls.Add(this.radioTransferOut);
            this.Controls.Add(this.radioTransferIn);
            this.Controls.Add(this.txtfactory1);
            this.Controls.Add(this.txtMdivision1);
            this.Controls.Add(this.txtSP2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtSP1);
            this.Controls.Add(this.dateIssue);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "R05";
            this.Text = "R05. Material Transfer In / Out Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.dateIssue, 0);
            this.Controls.SetChildIndex(this.txtSP1, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txtSP2, 0);
            this.Controls.SetChildIndex(this.txtMdivision1, 0);
            this.Controls.SetChildIndex(this.txtfactory1, 0);
            this.Controls.SetChildIndex(this.radioTransferIn, 0);
            this.Controls.SetChildIndex(this.radioTransferOut, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.DateRange dateIssue;
        private Win.UI.TextBox txtSP1;
        private Win.UI.Label label6;
        private Win.UI.TextBox txtSP2;
        private Class.TxtMdivision txtMdivision1;
        private Class.Txtfactory txtfactory1;
        private Win.UI.RadioButton radioTransferIn;
        private Win.UI.RadioButton radioTransferOut;
    }
}
