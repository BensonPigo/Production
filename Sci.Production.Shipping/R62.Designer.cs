namespace Sci.Production.Shipping
{
    partial class R62
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
            this.txtDecNo2 = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtDecNo1 = new Sci.Win.UI.TextBox();
            this.labDecNo = new Sci.Win.UI.Label();
            this.dateETD = new Sci.Win.UI.DateRange();
            this.labETD = new Sci.Win.UI.Label();
            this.dateDecDate = new Sci.Win.UI.DateRange();
            this.labDecDate = new Sci.Win.UI.Label();
            this.txtInvNo2 = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.txtInvNo1 = new Sci.Win.UI.TextBox();
            this.labInvNo = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(498, 80);
            this.print.TabIndex = 8;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(498, 9);
            this.toexcel.TabIndex = 6;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(498, 45);
            this.close.TabIndex = 7;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(452, 114);
            this.buttonCustomized.TabIndex = 13;
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(457, 123);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(457, 123);
            this.txtVersion.TabIndex = 14;
            // 
            // txtDecNo2
            // 
            this.txtDecNo2.BackColor = System.Drawing.Color.White;
            this.txtDecNo2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDecNo2.Location = new System.Drawing.Point(289, 70);
            this.txtDecNo2.Name = "txtDecNo2";
            this.txtDecNo2.Size = new System.Drawing.Size(125, 23);
            this.txtDecNo2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("標楷體", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(260, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "~";
            this.label1.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtDecNo1
            // 
            this.txtDecNo1.BackColor = System.Drawing.Color.White;
            this.txtDecNo1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDecNo1.Location = new System.Drawing.Point(138, 70);
            this.txtDecNo1.Name = "txtDecNo1";
            this.txtDecNo1.Size = new System.Drawing.Size(125, 23);
            this.txtDecNo1.TabIndex = 2;
            // 
            // labDecNo
            // 
            this.labDecNo.Location = new System.Drawing.Point(9, 70);
            this.labDecNo.Name = "labDecNo";
            this.labDecNo.Size = new System.Drawing.Size(124, 23);
            this.labDecNo.TabIndex = 11;
            this.labDecNo.Text = "Declaration#";
            // 
            // dateETD
            // 
            // 
            // 
            // 
            this.dateETD.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateETD.DateBox1.Name = "";
            this.dateETD.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateETD.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateETD.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateETD.DateBox2.Name = "";
            this.dateETD.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateETD.DateBox2.TabIndex = 1;
            this.dateETD.IsRequired = false;
            this.dateETD.Location = new System.Drawing.Point(138, 41);
            this.dateETD.Name = "dateETD";
            this.dateETD.Size = new System.Drawing.Size(280, 23);
            this.dateETD.TabIndex = 1;
            // 
            // labETD
            // 
            this.labETD.Location = new System.Drawing.Point(9, 41);
            this.labETD.Name = "labETD";
            this.labETD.Size = new System.Drawing.Size(124, 23);
            this.labETD.TabIndex = 10;
            this.labETD.Text = "ETA";
            // 
            // dateDecDate
            // 
            // 
            // 
            // 
            this.dateDecDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateDecDate.DateBox1.Name = "";
            this.dateDecDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateDecDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateDecDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateDecDate.DateBox2.Name = "";
            this.dateDecDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateDecDate.DateBox2.TabIndex = 1;
            this.dateDecDate.IsRequired = false;
            this.dateDecDate.Location = new System.Drawing.Point(138, 12);
            this.dateDecDate.Name = "dateDecDate";
            this.dateDecDate.Size = new System.Drawing.Size(280, 23);
            this.dateDecDate.TabIndex = 0;
            // 
            // labDecDate
            // 
            this.labDecDate.Location = new System.Drawing.Point(9, 12);
            this.labDecDate.Name = "labDecDate";
            this.labDecDate.Size = new System.Drawing.Size(124, 23);
            this.labDecDate.TabIndex = 9;
            this.labDecDate.Text = "Declaration Date";
            // 
            // txtInvNo2
            // 
            this.txtInvNo2.BackColor = System.Drawing.Color.White;
            this.txtInvNo2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInvNo2.Location = new System.Drawing.Point(289, 101);
            this.txtInvNo2.Name = "txtInvNo2";
            this.txtInvNo2.Size = new System.Drawing.Size(125, 23);
            this.txtInvNo2.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("標楷體", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label2.Location = new System.Drawing.Point(260, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "~";
            this.label2.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtInvNo1
            // 
            this.txtInvNo1.BackColor = System.Drawing.Color.White;
            this.txtInvNo1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInvNo1.Location = new System.Drawing.Point(138, 101);
            this.txtInvNo1.Name = "txtInvNo1";
            this.txtInvNo1.Size = new System.Drawing.Size(125, 23);
            this.txtInvNo1.TabIndex = 4;
            // 
            // labInvNo
            // 
            this.labInvNo.Location = new System.Drawing.Point(9, 101);
            this.labInvNo.Name = "labInvNo";
            this.labInvNo.Size = new System.Drawing.Size(124, 23);
            this.labInvNo.TabIndex = 12;
            this.labInvNo.Text = "Invoice No.";
            // 
            // R62
            // 
            this.ClientSize = new System.Drawing.Size(590, 172);
            this.Controls.Add(this.txtInvNo2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtInvNo1);
            this.Controls.Add(this.labInvNo);
            this.Controls.Add(this.txtDecNo2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDecNo1);
            this.Controls.Add(this.labDecNo);
            this.Controls.Add(this.dateETD);
            this.Controls.Add(this.labETD);
            this.Controls.Add(this.dateDecDate);
            this.Controls.Add(this.labDecDate);
            this.Name = "R62";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R62. KH Export Declaration Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.labDecDate, 0);
            this.Controls.SetChildIndex(this.dateDecDate, 0);
            this.Controls.SetChildIndex(this.labETD, 0);
            this.Controls.SetChildIndex(this.dateETD, 0);
            this.Controls.SetChildIndex(this.labDecNo, 0);
            this.Controls.SetChildIndex(this.txtDecNo1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtDecNo2, 0);
            this.Controls.SetChildIndex(this.labInvNo, 0);
            this.Controls.SetChildIndex(this.txtInvNo1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtInvNo2, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.TextBox txtDecNo2;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtDecNo1;
        private Win.UI.Label labDecNo;
        private Win.UI.DateRange dateETD;
        private Win.UI.Label labETD;
        private Win.UI.DateRange dateDecDate;
        private Win.UI.Label labDecDate;
        private Win.UI.TextBox txtInvNo2;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtInvNo1;
        private Win.UI.Label labInvNo;
    }
}
