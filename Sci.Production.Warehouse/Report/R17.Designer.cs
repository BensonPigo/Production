namespace Sci.Production.Warehouse
{
    partial class R17
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
            this.components = new System.ComponentModel.Container();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelSeqNo = new Sci.Win.UI.Label();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.labelLocation = new Sci.Win.UI.Label();
            this.labelStockType = new Sci.Win.UI.Label();
            this.checkBalanceQty = new Sci.Win.UI.CheckBox();
            this.comboStockType = new Sci.Win.UI.ComboBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.txtLocationEnd = new Sci.Production.Class.TxtMtlLocation(this.components);
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtSeq = new Sci.Production.Class.TxtSeq();
            this.txtMtlLocationStart = new Sci.Production.Class.TxtMtlLocation(this.components);
            this.label2 = new Sci.Win.UI.Label();
            this.dateETA = new Sci.Win.UI.DateRange();
            this.labelETA = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(440, 12);
            this.print.TabIndex = 7;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(440, 48);
            this.toexcel.TabIndex = 8;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(440, 84);
            this.close.TabIndex = 9;
            // 
            // dateSCIDelivery
            // 
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSCIDelivery.DateBox1.Name = "";
            this.dateSCIDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSCIDelivery.DateBox2.Name = "";
            this.dateSCIDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDelivery.DateBox2.TabIndex = 1;
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(100, 44);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 2;
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(100, 12);
            this.txtSPNo.MaxLength = 13;
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(118, 23);
            this.txtSPNo.TabIndex = 0;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(8, 12);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSPNo.Size = new System.Drawing.Size(89, 23);
            this.labelSPNo.TabIndex = 96;
            this.labelSPNo.Text = "SP#";
            this.labelSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelSeqNo
            // 
            this.labelSeqNo.Location = new System.Drawing.Point(251, 12);
            this.labelSeqNo.Name = "labelSeqNo";
            this.labelSeqNo.Size = new System.Drawing.Size(48, 23);
            this.labelSeqNo.TabIndex = 97;
            this.labelSeqNo.Text = "Seq#";
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.BackColor = System.Drawing.Color.PaleGreen;
            this.labelSCIDelivery.Location = new System.Drawing.Point(8, 44);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSCIDelivery.Size = new System.Drawing.Size(89, 23);
            this.labelSCIDelivery.TabIndex = 99;
            this.labelSCIDelivery.Text = "SCI Delivery";
            this.labelSCIDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelLocation
            // 
            this.labelLocation.BackColor = System.Drawing.Color.PaleGreen;
            this.labelLocation.Location = new System.Drawing.Point(8, 108);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.RectStyle.Color = System.Drawing.Color.LightSkyBlue;
            this.labelLocation.Size = new System.Drawing.Size(89, 23);
            this.labelLocation.TabIndex = 100;
            this.labelLocation.Text = "Location";
            this.labelLocation.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelStockType
            // 
            this.labelStockType.Location = new System.Drawing.Point(8, 172);
            this.labelStockType.Name = "labelStockType";
            this.labelStockType.Size = new System.Drawing.Size(89, 23);
            this.labelStockType.TabIndex = 104;
            this.labelStockType.Text = "Stock Type";
            // 
            // checkBalanceQty
            // 
            this.checkBalanceQty.AutoSize = true;
            this.checkBalanceQty.Checked = true;
            this.checkBalanceQty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBalanceQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBalanceQty.Location = new System.Drawing.Point(234, 174);
            this.checkBalanceQty.Name = "checkBalanceQty";
            this.checkBalanceQty.Size = new System.Drawing.Size(128, 21);
            this.checkBalanceQty.TabIndex = 8;
            this.checkBalanceQty.Text = "Balance Qty > 0";
            this.checkBalanceQty.UseVisualStyleBackColor = true;
            // 
            // comboStockType
            // 
            this.comboStockType.BackColor = System.Drawing.Color.White;
            this.comboStockType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStockType.FormattingEnabled = true;
            this.comboStockType.IsSupportUnselect = true;
            this.comboStockType.Items.AddRange(new object[] {
            "ALL",
            "Bulk",
            "Inventory"});
            this.comboStockType.Location = new System.Drawing.Point(99, 172);
            this.comboStockType.Name = "comboStockType";
            this.comboStockType.OldText = "";
            this.comboStockType.Size = new System.Drawing.Size(121, 24);
            this.comboStockType.TabIndex = 7;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(8, 140);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(89, 23);
            this.labelFactory.TabIndex = 106;
            this.labelFactory.Text = "Factory";
            // 
            // txtLocationEnd
            // 
            this.txtLocationEnd.BackColor = System.Drawing.Color.White;
            this.txtLocationEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocationEnd.Location = new System.Drawing.Point(228, 108);
            this.txtLocationEnd.Name = "txtLocationEnd";
            this.txtLocationEnd.Size = new System.Drawing.Size(100, 23);
            this.txtLocationEnd.StockTypeFilte = "";
            this.txtLocationEnd.TabIndex = 5;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(99, 140);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 6;
            // 
            // txtSeq
            // 
            this.txtSeq.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSeq.Location = new System.Drawing.Point(302, 12);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.Seq1 = "";
            this.txtSeq.Seq2 = "";
            this.txtSeq.Size = new System.Drawing.Size(61, 23);
            this.txtSeq.TabIndex = 1;
            // 
            // txtMtlLocationStart
            // 
            this.txtMtlLocationStart.BackColor = System.Drawing.Color.White;
            this.txtMtlLocationStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMtlLocationStart.Location = new System.Drawing.Point(100, 108);
            this.txtMtlLocationStart.Name = "txtMtlLocationStart";
            this.txtMtlLocationStart.Size = new System.Drawing.Size(100, 23);
            this.txtMtlLocationStart.StockTypeFilte = "";
            this.txtMtlLocationStart.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(203, 108);
            this.label2.Name = "label2";
            this.label2.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.RectStyle.Color = System.Drawing.Color.Transparent;
            this.label2.RectStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label2.Size = new System.Drawing.Size(22, 23);
            this.label2.TabIndex = 109;
            this.label2.Text = "～";
            this.label2.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
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
            this.dateETA.IsRequired = false;
            this.dateETA.Location = new System.Drawing.Point(100, 76);
            this.dateETA.Name = "dateETA";
            this.dateETA.Size = new System.Drawing.Size(280, 23);
            this.dateETA.TabIndex = 3;
            // 
            // labelETA
            // 
            this.labelETA.BackColor = System.Drawing.Color.PaleGreen;
            this.labelETA.Location = new System.Drawing.Point(8, 76);
            this.labelETA.Name = "labelETA";
            this.labelETA.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelETA.Size = new System.Drawing.Size(89, 23);
            this.labelETA.TabIndex = 111;
            this.labelETA.Text = "ETA";
            this.labelETA.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // R17
            // 
            this.ClientSize = new System.Drawing.Size(559, 237);
            this.Controls.Add(this.labelETA);
            this.Controls.Add(this.dateETA);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMtlLocationStart);
            this.Controls.Add(this.txtLocationEnd);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.txtSeq);
            this.Controls.Add(this.comboStockType);
            this.Controls.Add(this.checkBalanceQty);
            this.Controls.Add(this.labelStockType);
            this.Controls.Add(this.labelLocation);
            this.Controls.Add(this.labelSCIDelivery);
            this.Controls.Add(this.labelSeqNo);
            this.Controls.Add(this.labelSPNo);
            this.Controls.Add(this.txtSPNo);
            this.Controls.Add(this.dateSCIDelivery);
            this.Name = "R17";
            this.Text = "R17. Material Location Query";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.dateSCIDelivery, 0);
            this.Controls.SetChildIndex(this.txtSPNo, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.labelSeqNo, 0);
            this.Controls.SetChildIndex(this.labelSCIDelivery, 0);
            this.Controls.SetChildIndex(this.labelLocation, 0);
            this.Controls.SetChildIndex(this.labelStockType, 0);
            this.Controls.SetChildIndex(this.checkBalanceQty, 0);
            this.Controls.SetChildIndex(this.comboStockType, 0);
            this.Controls.SetChildIndex(this.txtSeq, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.txtLocationEnd, 0);
            this.Controls.SetChildIndex(this.txtMtlLocationStart, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.dateETA, 0);
            this.Controls.SetChildIndex(this.labelETA, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelSeqNo;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Label labelLocation;
        private Win.UI.Label labelStockType;
        private Win.UI.CheckBox checkBalanceQty;
        private Win.UI.ComboBox comboStockType;
        private Class.TxtSeq txtSeq;
        private Win.UI.Label labelFactory;
        private Class.Txtfactory txtfactory;
        private Class.TxtMtlLocation txtLocationEnd;
        private Class.TxtMtlLocation txtMtlLocationStart;
        private Win.UI.Label label2;
        private Win.UI.DateRange dateETA;
        private Win.UI.Label labelETA;
    }
}
