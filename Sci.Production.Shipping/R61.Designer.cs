namespace Sci.Production.Shipping
{
    partial class R61
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
            this.dateDecDate = new Sci.Win.UI.DateRange();
            this.dateETA = new Sci.Win.UI.DateRange();
            this.dateArrivWHDate = new Sci.Win.UI.DateRange();
            this.comboshipmode = new Sci.Production.Class.Txtshipmode();
            this.LabShippMode = new Sci.Win.UI.Label();
            this.txtDecNo1 = new Sci.Win.UI.TextBox();
            this.labDecNo = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.txtDecNo2 = new Sci.Win.UI.TextBox();
            this.labETA = new Sci.Win.UI.Label();
            this.labArrivWHDate = new Sci.Win.UI.Label();
            this.labDecDate = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(489, 96);
            this.print.TabIndex = 8;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(489, 17);
            this.toexcel.TabIndex = 6;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(489, 53);
            this.close.TabIndex = 7;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(443, 135);
            this.buttonCustomized.TabIndex = 17;
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(443, 144);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(443, 142);
            this.txtVersion.TabIndex = 15;
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
            this.dateDecDate.Location = new System.Drawing.Point(138, 14);
            this.dateDecDate.Name = "dateDecDate";
            this.dateDecDate.Size = new System.Drawing.Size(280, 23);
            this.dateDecDate.TabIndex = 0;
            // 
            // dateETA
            // 
            // 
            // 
            // 
            this.dateETA.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateETA.DateBox1.Name = "";
            this.dateETA.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateETA.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateETA.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateETA.DateBox2.Name = "";
            this.dateETA.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateETA.DateBox2.TabIndex = 1;
            this.dateETA.Location = new System.Drawing.Point(138, 43);
            this.dateETA.Name = "dateETA";
            this.dateETA.Size = new System.Drawing.Size(280, 23);
            this.dateETA.TabIndex = 1;
            // 
            // dateArrivWHDate
            // 
            // 
            // 
            // 
            this.dateArrivWHDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateArrivWHDate.DateBox1.Name = "";
            this.dateArrivWHDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateArrivWHDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateArrivWHDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateArrivWHDate.DateBox2.Name = "";
            this.dateArrivWHDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateArrivWHDate.DateBox2.TabIndex = 1;
            this.dateArrivWHDate.Location = new System.Drawing.Point(138, 72);
            this.dateArrivWHDate.Name = "dateArrivWHDate";
            this.dateArrivWHDate.Size = new System.Drawing.Size(280, 23);
            this.dateArrivWHDate.TabIndex = 2;
            // 
            // comboshipmode
            // 
            this.comboshipmode.BackColor = System.Drawing.Color.White;
            this.comboshipmode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboshipmode.FormattingEnabled = true;
            this.comboshipmode.IsSupportUnselect = true;
            this.comboshipmode.Location = new System.Drawing.Point(138, 135);
            this.comboshipmode.Name = "comboshipmode";
            this.comboshipmode.OldText = "";
            this.comboshipmode.Size = new System.Drawing.Size(170, 24);
            this.comboshipmode.TabIndex = 5;
            this.comboshipmode.UseFunction = null;
            // 
            // LabShippMode
            // 
            this.LabShippMode.Location = new System.Drawing.Point(9, 135);
            this.LabShippMode.Name = "LabShippMode";
            this.LabShippMode.Size = new System.Drawing.Size(124, 23);
            this.LabShippMode.TabIndex = 13;
            this.LabShippMode.Text = "Ship Mode";
            // 
            // txtDecNo1
            // 
            this.txtDecNo1.BackColor = System.Drawing.Color.White;
            this.txtDecNo1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDecNo1.Location = new System.Drawing.Point(138, 103);
            this.txtDecNo1.Name = "txtDecNo1";
            this.txtDecNo1.Size = new System.Drawing.Size(125, 23);
            this.txtDecNo1.TabIndex = 3;
            // 
            // labDecNo
            // 
            this.labDecNo.Location = new System.Drawing.Point(9, 103);
            this.labDecNo.Name = "labDecNo";
            this.labDecNo.Size = new System.Drawing.Size(124, 23);
            this.labDecNo.TabIndex = 12;
            this.labDecNo.Text = "Declaration#";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("標楷體", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(260, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 23);
            this.label1.TabIndex = 14;
            this.label1.Text = "~";
            this.label1.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtDecNo2
            // 
            this.txtDecNo2.BackColor = System.Drawing.Color.White;
            this.txtDecNo2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDecNo2.Location = new System.Drawing.Point(289, 103);
            this.txtDecNo2.Name = "txtDecNo2";
            this.txtDecNo2.Size = new System.Drawing.Size(125, 23);
            this.txtDecNo2.TabIndex = 4;
            // 
            // labETA
            // 
            this.labETA.Location = new System.Drawing.Point(9, 43);
            this.labETA.Name = "labETA";
            this.labETA.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labETA.RectStyle.BorderWidth = 1F;
            this.labETA.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labETA.RectStyle.ExtBorderWidth = 1F;
            this.labETA.Size = new System.Drawing.Size(124, 23);
            this.labETA.TabIndex = 99;
            this.labETA.Text = "ETA";
            this.labETA.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labETA.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labArrivWHDate
            // 
            this.labArrivWHDate.Location = new System.Drawing.Point(9, 72);
            this.labArrivWHDate.Name = "labArrivWHDate";
            this.labArrivWHDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labArrivWHDate.RectStyle.BorderWidth = 1F;
            this.labArrivWHDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labArrivWHDate.RectStyle.ExtBorderWidth = 1F;
            this.labArrivWHDate.Size = new System.Drawing.Size(124, 23);
            this.labArrivWHDate.TabIndex = 100;
            this.labArrivWHDate.Text = "Arrived WH Date";
            this.labArrivWHDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labArrivWHDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labDecDate
            // 
            this.labDecDate.Location = new System.Drawing.Point(9, 14);
            this.labDecDate.Name = "labDecDate";
            this.labDecDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labDecDate.RectStyle.BorderWidth = 1F;
            this.labDecDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labDecDate.RectStyle.ExtBorderWidth = 1F;
            this.labDecDate.Size = new System.Drawing.Size(124, 23);
            this.labDecDate.TabIndex = 101;
            this.labDecDate.Text = "Declaration Date";
            this.labDecDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labDecDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // R61
            // 
            this.ClientSize = new System.Drawing.Size(581, 193);
            this.Controls.Add(this.labDecDate);
            this.Controls.Add(this.labArrivWHDate);
            this.Controls.Add(this.labETA);
            this.Controls.Add(this.txtDecNo2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDecNo1);
            this.Controls.Add(this.labDecNo);
            this.Controls.Add(this.comboshipmode);
            this.Controls.Add(this.LabShippMode);
            this.Controls.Add(this.dateArrivWHDate);
            this.Controls.Add(this.dateETA);
            this.Controls.Add(this.dateDecDate);
            this.Name = "R61";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R61. KH Import Declaration Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.dateDecDate, 0);
            this.Controls.SetChildIndex(this.dateETA, 0);
            this.Controls.SetChildIndex(this.dateArrivWHDate, 0);
            this.Controls.SetChildIndex(this.LabShippMode, 0);
            this.Controls.SetChildIndex(this.comboshipmode, 0);
            this.Controls.SetChildIndex(this.labDecNo, 0);
            this.Controls.SetChildIndex(this.txtDecNo1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtDecNo2, 0);
            this.Controls.SetChildIndex(this.labETA, 0);
            this.Controls.SetChildIndex(this.labArrivWHDate, 0);
            this.Controls.SetChildIndex(this.labDecDate, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateRange dateDecDate;
        private Win.UI.DateRange dateETA;
        private Win.UI.DateRange dateArrivWHDate;
        private Class.Txtshipmode comboshipmode;
        private Win.UI.Label LabShippMode;
        private Win.UI.TextBox txtDecNo1;
        private Win.UI.Label labDecNo;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtDecNo2;
        private Win.UI.Label labETA;
        private Win.UI.Label labArrivWHDate;
        private Win.UI.Label labDecDate;
    }
}
