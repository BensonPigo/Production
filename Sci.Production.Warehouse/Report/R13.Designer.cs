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
            this.txtMdivision = new Sci.Production.Class.txtMdivision();
            this.labelStockType = new Sci.Win.UI.Label();
            this.comboStockType = new System.Windows.Forms.ComboBox();
            this.labelReasonCode = new Sci.Win.UI.Label();
            this.txtReason = new Sci.Production.Class.txtReason();
            this.labelAdjustDate = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.txtfactory();
            this.labelFactory = new Sci.Win.UI.Label();
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
            this.dateAdjustDate.IsRequired = false;
            this.dateAdjustDate.Location = new System.Drawing.Point(115, 12);
            this.dateAdjustDate.Name = "dateAdjustDate";
            this.dateAdjustDate.Size = new System.Drawing.Size(280, 23);
            this.dateAdjustDate.TabIndex = 0;
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
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
            this.labelStockType.Lines = 0;
            this.labelStockType.Location = new System.Drawing.Point(13, 48);
            this.labelStockType.Name = "labelStockType";
            this.labelStockType.Size = new System.Drawing.Size(98, 23);
            this.labelStockType.TabIndex = 118;
            this.labelStockType.Text = "Stock Type";
            // 
            // comboStockType
            // 
            this.comboStockType.FormattingEnabled = true;
            this.comboStockType.Location = new System.Drawing.Point(115, 47);
            this.comboStockType.Name = "comboStockType";
            this.comboStockType.Size = new System.Drawing.Size(121, 24);
            this.comboStockType.TabIndex = 1;
            // 
            // labelReasonCode
            // 
            this.labelReasonCode.Lines = 0;
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
            // labelAdjustDate
            // 
            this.labelAdjustDate.Lines = 0;
            this.labelAdjustDate.Location = new System.Drawing.Point(14, 12);
            this.labelAdjustDate.Name = "labelAdjustDate";
            this.labelAdjustDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelAdjustDate.RectStyle.BorderWidth = 1F;
            this.labelAdjustDate.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.labelAdjustDate.RectStyle.ExtBorderWidth = 1F;
            this.labelAdjustDate.Size = new System.Drawing.Size(98, 23);
            this.labelAdjustDate.TabIndex = 122;
            this.labelAdjustDate.Text = "Adjust Date";
            this.labelAdjustDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelAdjustDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.Location = new System.Drawing.Point(115, 119);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 123;
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(13, 119);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 124;
            this.labelFactory.Text = "Factory";
            // 
            // R13
            // 
            this.ClientSize = new System.Drawing.Size(639, 212);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.labelAdjustDate);
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
            this.Controls.SetChildIndex(this.labelAdjustDate, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateRange dateAdjustDate;
        private Class.txtMdivision txtMdivision;
        private Win.UI.Label labelM;
        private Win.UI.Label labelStockType;
        private System.Windows.Forms.ComboBox comboStockType;
        private Win.UI.Label labelReasonCode;
        private Class.txtReason txtReason;
        private Win.UI.Label labelAdjustDate;
        private Class.txtfactory txtfactory;
        private Win.UI.Label labelFactory;
    }
}
