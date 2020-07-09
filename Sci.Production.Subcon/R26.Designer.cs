namespace Sci.Production.Subcon
{
    partial class R26
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.dateDeliveryDate = new Sci.Win.UI.DateRange();
            this.label1 = new Sci.Win.UI.Label();
            this.rdbtn_PandI = new Sci.Win.UI.RadioButton();
            this.rdbtn_incoming = new Sci.Win.UI.RadioButton();
            this.rdbtn_payment = new Sci.Win.UI.RadioButton();
            this.checkBoxNoClosed = new Sci.Win.UI.CheckBox();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.txtLocalPoidEnd = new Sci.Win.UI.TextBox();
            this.label10 = new Sci.Win.UI.Label();
            this.txtLocalPoidStart = new Sci.Win.UI.TextBox();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.comboReportType = new Sci.Win.UI.ComboBox();
            this.txtsubconSupplier = new Sci.Production.Class.TxtsubconNoConfirm();
            this.txtartworktype_ftyCategory = new Sci.Production.Class.Txtartworktype_fty();
            this.dateIssueDate = new Sci.Win.UI.DateRange();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.labelReportType = new Sci.Win.UI.Label();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelLocalPoid = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelIssueDate = new Sci.Win.UI.Label();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.checkShippingMark = new Sci.Win.UI.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(474, 24);
            this.print.TabIndex = 2;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(474, 60);
            this.toexcel.TabIndex = 3;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(474, 96);
            this.close.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dateDeliveryDate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.rdbtn_PandI);
            this.panel1.Controls.Add(this.rdbtn_incoming);
            this.panel1.Controls.Add(this.rdbtn_payment);
            this.panel1.Controls.Add(this.checkBoxNoClosed);
            this.panel1.Controls.Add(this.comboFactory);
            this.panel1.Controls.Add(this.txtLocalPoidEnd);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.txtLocalPoidStart);
            this.panel1.Controls.Add(this.txtSPNoEnd);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtSPNoStart);
            this.panel1.Controls.Add(this.comboReportType);
            this.panel1.Controls.Add(this.txtsubconSupplier);
            this.panel1.Controls.Add(this.txtartworktype_ftyCategory);
            this.panel1.Controls.Add(this.dateIssueDate);
            this.panel1.Controls.Add(this.dateSCIDelivery);
            this.panel1.Controls.Add(this.labelReportType);
            this.panel1.Controls.Add(this.labelSupplier);
            this.panel1.Controls.Add(this.labelCategory);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.labelLocalPoid);
            this.panel1.Controls.Add(this.labelSPNo);
            this.panel1.Controls.Add(this.labelIssueDate);
            this.panel1.Controls.Add(this.labelSCIDelivery);
            this.panel1.Location = new System.Drawing.Point(8, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(443, 381);
            this.panel1.TabIndex = 1;
            // 
            // dateDeliveryDate
            // 
            // 
            // 
            // 
            this.dateDeliveryDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateDeliveryDate.DateBox1.Name = "";
            this.dateDeliveryDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateDeliveryDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateDeliveryDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateDeliveryDate.DateBox2.Name = "";
            this.dateDeliveryDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateDeliveryDate.DateBox2.TabIndex = 1;
            this.dateDeliveryDate.IsRequired = false;
            this.dateDeliveryDate.Location = new System.Drawing.Point(114, 71);
            this.dateDeliveryDate.Name = "dateDeliveryDate";
            this.dateDeliveryDate.Size = new System.Drawing.Size(280, 23);
            this.dateDeliveryDate.TabIndex = 99;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(15, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 23);
            this.label1.TabIndex = 98;
            this.label1.Text = "Delivery";
            // 
            // rdbtn_PandI
            // 
            this.rdbtn_PandI.AutoSize = true;
            this.rdbtn_PandI.Enabled = false;
            this.rdbtn_PandI.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdbtn_PandI.Location = new System.Drawing.Point(119, 346);
            this.rdbtn_PandI.Name = "rdbtn_PandI";
            this.rdbtn_PandI.Size = new System.Drawing.Size(158, 21);
            this.rdbtn_PandI.TabIndex = 97;
            this.rdbtn_PandI.TabStop = true;
            this.rdbtn_PandI.Text = "Payment or Incoming";
            this.rdbtn_PandI.UseVisualStyleBackColor = true;
            // 
            // rdbtn_incoming
            // 
            this.rdbtn_incoming.AutoSize = true;
            this.rdbtn_incoming.Enabled = false;
            this.rdbtn_incoming.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdbtn_incoming.Location = new System.Drawing.Point(119, 319);
            this.rdbtn_incoming.Name = "rdbtn_incoming";
            this.rdbtn_incoming.Size = new System.Drawing.Size(82, 21);
            this.rdbtn_incoming.TabIndex = 96;
            this.rdbtn_incoming.TabStop = true;
            this.rdbtn_incoming.Text = "Incoming";
            this.rdbtn_incoming.UseVisualStyleBackColor = true;
            // 
            // rdbtn_payment
            // 
            this.rdbtn_payment.AutoSize = true;
            this.rdbtn_payment.Enabled = false;
            this.rdbtn_payment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rdbtn_payment.Location = new System.Drawing.Point(119, 292);
            this.rdbtn_payment.Name = "rdbtn_payment";
            this.rdbtn_payment.Size = new System.Drawing.Size(81, 21);
            this.rdbtn_payment.TabIndex = 95;
            this.rdbtn_payment.TabStop = true;
            this.rdbtn_payment.Text = "Payment";
            this.rdbtn_payment.UseVisualStyleBackColor = true;
            // 
            // checkBoxNoClosed
            // 
            this.checkBoxNoClosed.AutoSize = true;
            this.checkBoxNoClosed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBoxNoClosed.Location = new System.Drawing.Point(15, 292);
            this.checkBoxNoClosed.Name = "checkBoxNoClosed";
            this.checkBoxNoClosed.Size = new System.Drawing.Size(104, 21);
            this.checkBoxNoClosed.TabIndex = 94;
            this.checkBoxNoClosed.Text = "Outstanding";
            this.checkBoxNoClosed.UseVisualStyleBackColor = true;
            this.checkBoxNoClosed.CheckedChanged += new System.EventHandler(this.checkBoxNoClosed_CheckedChanged);
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(114, 162);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 6;
            // 
            // txtLocalPoidEnd
            // 
            this.txtLocalPoidEnd.BackColor = System.Drawing.Color.White;
            this.txtLocalPoidEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocalPoidEnd.Location = new System.Drawing.Point(272, 133);
            this.txtLocalPoidEnd.Name = "txtLocalPoidEnd";
            this.txtLocalPoidEnd.Size = new System.Drawing.Size(122, 23);
            this.txtLocalPoidEnd.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.SystemColors.Control;
            this.label10.Location = new System.Drawing.Point(245, 133);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(20, 23);
            this.label10.TabIndex = 19;
            this.label10.Text = "~";
            this.label10.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtLocalPoidStart
            // 
            this.txtLocalPoidStart.BackColor = System.Drawing.Color.White;
            this.txtLocalPoidStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocalPoidStart.Location = new System.Drawing.Point(114, 133);
            this.txtLocalPoidStart.Name = "txtLocalPoidStart";
            this.txtLocalPoidStart.Size = new System.Drawing.Size(121, 23);
            this.txtLocalPoidStart.TabIndex = 4;
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(272, 102);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(122, 23);
            this.txtSPNoEnd.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.Control;
            this.label9.Location = new System.Drawing.Point(245, 103);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(22, 23);
            this.label9.TabIndex = 18;
            this.label9.Text = "~";
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(114, 103);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(121, 23);
            this.txtSPNoStart.TabIndex = 2;
            // 
            // comboReportType
            // 
            this.comboReportType.BackColor = System.Drawing.Color.White;
            this.comboReportType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboReportType.FormattingEnabled = true;
            this.comboReportType.IsSupportUnselect = true;
            this.comboReportType.Items.AddRange(new object[] {
            "PO List",
            "PO Form",
            "PO Order"});
            this.comboReportType.Location = new System.Drawing.Point(114, 262);
            this.comboReportType.Name = "comboReportType";
            this.comboReportType.OldText = "";
            this.comboReportType.Size = new System.Drawing.Size(121, 24);
            this.comboReportType.TabIndex = 9;
            this.comboReportType.SelectedIndexChanged += new System.EventHandler(this.comboReportType_SelectedIndexChanged);
            // 
            // txtsubconSupplier
            // 
            this.txtsubconSupplier.DisplayBox1Binding = "";
            this.txtsubconSupplier.IsIncludeJunk = true;
            this.txtsubconSupplier.Location = new System.Drawing.Point(114, 232);
            this.txtsubconSupplier.Name = "txtsubconSupplier";
            this.txtsubconSupplier.Size = new System.Drawing.Size(214, 23);
            this.txtsubconSupplier.TabIndex = 8;
            this.txtsubconSupplier.TextBox1Binding = "";
            // 
            // txtartworktype_ftyCategory
            // 
            this.txtartworktype_ftyCategory.BackColor = System.Drawing.Color.White;
            this.txtartworktype_ftyCategory.CClassify = "\'P\'";
            this.txtartworktype_ftyCategory.CSubprocess = "";
            this.txtartworktype_ftyCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtartworktype_ftyCategory.Location = new System.Drawing.Point(114, 195);
            this.txtartworktype_ftyCategory.Name = "txtartworktype_ftyCategory";
            this.txtartworktype_ftyCategory.Size = new System.Drawing.Size(153, 23);
            this.txtartworktype_ftyCategory.TabIndex = 7;
            this.txtartworktype_ftyCategory.TextChanged += new System.EventHandler(this.txtartworktype_ftyCategory_TextChanged);
            // 
            // dateIssueDate
            // 
            // 
            // 
            // 
            this.dateIssueDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateIssueDate.DateBox1.Name = "";
            this.dateIssueDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateIssueDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateIssueDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateIssueDate.DateBox2.Name = "";
            this.dateIssueDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateIssueDate.DateBox2.TabIndex = 1;
            this.dateIssueDate.IsRequired = false;
            this.dateIssueDate.Location = new System.Drawing.Point(114, 41);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(280, 23);
            this.dateIssueDate.TabIndex = 1;
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
            this.dateSCIDelivery.Location = new System.Drawing.Point(114, 8);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 0;
            // 
            // labelReportType
            // 
            this.labelReportType.Location = new System.Drawing.Point(15, 263);
            this.labelReportType.Name = "labelReportType";
            this.labelReportType.Size = new System.Drawing.Size(94, 23);
            this.labelReportType.TabIndex = 17;
            this.labelReportType.Text = "Report Type";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Location = new System.Drawing.Point(15, 232);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(94, 23);
            this.labelSupplier.TabIndex = 16;
            this.labelSupplier.Text = "Supplier";
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(15, 195);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(94, 23);
            this.labelCategory.TabIndex = 15;
            this.labelCategory.Text = "Category";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(15, 163);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(94, 23);
            this.labelFactory.TabIndex = 14;
            this.labelFactory.Text = "Factory";
            // 
            // labelLocalPoid
            // 
            this.labelLocalPoid.Location = new System.Drawing.Point(15, 133);
            this.labelLocalPoid.Name = "labelLocalPoid";
            this.labelLocalPoid.Size = new System.Drawing.Size(94, 23);
            this.labelLocalPoid.TabIndex = 13;
            this.labelLocalPoid.Text = "Local Poid";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(15, 102);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(94, 23);
            this.labelSPNo.TabIndex = 12;
            this.labelSPNo.Text = "SP No";
            // 
            // labelIssueDate
            // 
            this.labelIssueDate.Location = new System.Drawing.Point(15, 41);
            this.labelIssueDate.Name = "labelIssueDate";
            this.labelIssueDate.Size = new System.Drawing.Size(94, 23);
            this.labelIssueDate.TabIndex = 11;
            this.labelIssueDate.Text = "Issue Date";
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(15, 8);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(94, 23);
            this.labelSCIDelivery.TabIndex = 10;
            this.labelSCIDelivery.Text = "SCI Delivery";
            // 
            // checkShippingMark
            // 
            this.checkShippingMark.AutoSize = true;
            this.checkShippingMark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkShippingMark.Location = new System.Drawing.Point(461, 147);
            this.checkShippingMark.Name = "checkShippingMark";
            this.checkShippingMark.Size = new System.Drawing.Size(117, 21);
            this.checkShippingMark.TabIndex = 0;
            this.checkShippingMark.Text = "Shipping Mark";
            this.checkShippingMark.UseVisualStyleBackColor = true;
            this.checkShippingMark.CheckedChanged += new System.EventHandler(this.checkShippingMark_CheckedChanged);
            // 
            // R26
            // 
            this.ClientSize = new System.Drawing.Size(582, 432);
            this.Controls.Add(this.checkShippingMark);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "dateSCIDelivery";
            this.DefaultControlForEdit = "dateSCIDelivery";
            this.Name = "R26";
            this.Text = "R26. Local PO Report";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.checkShippingMark, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Class.TxtsubconNoConfirm txtsubconSupplier;
        private Class.Txtartworktype_fty txtartworktype_ftyCategory;
        private Win.UI.DateRange dateIssueDate;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.Label labelReportType;
        private Win.UI.Label labelSupplier;
        private Win.UI.Label labelCategory;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelLocalPoid;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelIssueDate;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.ComboBox comboReportType;
        private Win.UI.TextBox txtLocalPoidEnd;
        private Win.UI.Label label10;
        private Win.UI.TextBox txtLocalPoidStart;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.Label label9;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.CheckBox checkShippingMark;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.CheckBox checkBoxNoClosed;
        private Win.UI.RadioButton rdbtn_PandI;
        private Win.UI.RadioButton rdbtn_incoming;
        private Win.UI.RadioButton rdbtn_payment;
        private Win.UI.DateRange dateDeliveryDate;
        private Win.UI.Label label1;
    }
}
