namespace Sci.Production.Warehouse
{
    partial class R13
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.dateAdjustDate = new Sci.Win.UI.DateRange();
            this.labelM = new Sci.Win.UI.Label();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.labelStockType = new Sci.Win.UI.Label();
            this.comboStockType = new System.Windows.Forms.ComboBox();
            this.labelReasonCode = new Sci.Win.UI.Label();
            this.txtReason = new Sci.Production.Class.TxtReason();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.labelFactory = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(547, 12);
            this.print.TabIndex = 4;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(547, 48);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(547, 84);
            this.close.TabIndex = 6;
            // 
            // dateAdjustDate
            // 
            // 
            // 
            // 
            this.dateAdjustDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateAdjustDate.DateBox1.Name = "";
            this.dateAdjustDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateAdjustDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateAdjustDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateAdjustDate.DateBox2.Name = "";
            this.dateAdjustDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateAdjustDate.DateBox2.TabIndex = 1;
            this.dateAdjustDate.IsRequired = false;
            this.dateAdjustDate.Location = new System.Drawing.Point(115, 12);
            this.dateAdjustDate.Name = "dateAdjustDate";
            this.dateAdjustDate.Size = new System.Drawing.Size(280, 23);
            this.dateAdjustDate.TabIndex = 0;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 84);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 103;
            this.labelM.Text = "M";
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(114, 84);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 2;
            // 
            // labelStockType
            // 
            this.labelStockType.Location = new System.Drawing.Point(13, 48);
            this.labelStockType.Name = "labelStockType";
            this.labelStockType.Size = new System.Drawing.Size(98, 23);
            this.labelStockType.TabIndex = 118;
            this.labelStockType.Text = "Stock Type";
            // 
            // comboStockType
            // 
            this.comboStockType.ForeColor = System.Drawing.Color.Red;
            this.comboStockType.FormattingEnabled = true;
            this.comboStockType.Location = new System.Drawing.Point(115, 47);
            this.comboStockType.Name = "comboStockType";
            this.comboStockType.Size = new System.Drawing.Size(121, 24);
            this.comboStockType.TabIndex = 1;
            // 
            // labelReasonCode
            // 
            this.labelReasonCode.Location = new System.Drawing.Point(12, 152);
            this.labelReasonCode.Name = "labelReasonCode";
            this.labelReasonCode.Size = new System.Drawing.Size(98, 23);
            this.labelReasonCode.TabIndex = 120;
            this.labelReasonCode.Text = "Reason Code";
            // 
            // txtReason
            // 
            this.txtReason.BackColor = System.Drawing.Color.White;
            this.txtReason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtReason.FormattingEnabled = true;
            this.txtReason.IsSupportUnselect = true;
            this.txtReason.Location = new System.Drawing.Point(114, 151);
            this.txtReason.Name = "txtReason";
            this.txtReason.ReasonTypeID = "Stock_Adjust";
            this.txtReason.Size = new System.Drawing.Size(388, 24);
            this.txtReason.TabIndex = 3;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(115, 119);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 123;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 119);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 124;
            this.labelFactory.Text = "Factory";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.SkyBlue;
            this.label1.Location = new System.Drawing.Point(14, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 23);
            this.label1.TabIndex = 125;
            this.label1.Text = "Adjust Date";
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // R13
            // 
            this.ClientSize = new System.Drawing.Size(639, 212);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtReason);
            this.Controls.Add(this.labelReasonCode);
            this.Controls.Add(this.comboStockType);
            this.Controls.Add(this.labelStockType);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.dateAdjustDate);
            this.IsSupportToPrint = false;
            this.Name = "R13";
            this.Text = "R13. Warehouse Adjust Report";
            this.Controls.SetChildIndex(this.dateAdjustDate, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelStockType, 0);
            this.Controls.SetChildIndex(this.comboStockType, 0);
            this.Controls.SetChildIndex(this.labelReasonCode, 0);
            this.Controls.SetChildIndex(this.txtReason, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateRange dateAdjustDate;
        private Class.TxtMdivision txtMdivision;
        private Win.UI.Label labelM;
        private Win.UI.Label labelStockType;
        private System.Windows.Forms.ComboBox comboStockType;
        private Win.UI.Label labelReasonCode;
        private Class.TxtReason txtReason;
        private Class.Txtfactory txtfactory;
        private Win.UI.Label labelFactory;
        private Win.UI.Label label1;
    }
}
