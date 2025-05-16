namespace Sci.Production.Cutting
{
    partial class R01
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.txtFactory = new Sci.Production.Class.Txtfactory();
            this.txtM = new Sci.Production.Class.TxtMdivision();
            this.txtCuttingSP = new Sci.Win.UI.TextBox();
            this.labStatus = new Sci.Win.UI.Label();
            this.labCuttingSP = new Sci.Win.UI.Label();
            this.lbEstCutDate = new Sci.Win.UI.Label();
            this.comboDropDownList1 = new Sci.Production.Class.ComboDropDownList(this.components);
            this.labelFactory = new Sci.Win.UI.Label();
            this.radioByCutRef = new Sci.Win.UI.RadioButton();
            this.radioBySP = new Sci.Win.UI.RadioButton();
            this.dateEstCutDate = new Sci.Win.UI.DateRange();
            this.labReportType = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(446, 155);
            this.print.Size = new System.Drawing.Size(87, 30);
            this.print.TabIndex = 2;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(439, 12);
            this.toexcel.Size = new System.Drawing.Size(87, 30);
            this.toexcel.TabIndex = 0;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(439, 51);
            this.close.Size = new System.Drawing.Size(87, 30);
            this.close.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtFactory);
            this.panel1.Controls.Add(this.txtM);
            this.panel1.Controls.Add(this.txtCuttingSP);
            this.panel1.Controls.Add(this.labStatus);
            this.panel1.Controls.Add(this.labCuttingSP);
            this.panel1.Controls.Add(this.lbEstCutDate);
            this.panel1.Controls.Add(this.comboDropDownList1);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.radioByCutRef);
            this.panel1.Controls.Add(this.radioBySP);
            this.panel1.Controls.Add(this.dateEstCutDate);
            this.panel1.Controls.Add(this.labReportType);
            this.panel1.Controls.Add(this.labelM);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(408, 235);
            this.panel1.TabIndex = 0;
            // 
            // txtFactory
            // 
            this.txtFactory.BackColor = System.Drawing.Color.White;
            this.txtFactory.BoolFtyGroupList = true;
            this.txtFactory.FilteMDivision = true;
            this.txtFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactory.IsMultiselect = false;
            this.txtFactory.IsProduceFty = false;
            this.txtFactory.IssupportJunk = false;
            this.txtFactory.Location = new System.Drawing.Point(111, 100);
            this.txtFactory.MDivision = null;
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Size = new System.Drawing.Size(134, 23);
            this.txtFactory.TabIndex = 3;
            // 
            // txtM
            // 
            this.txtM.BackColor = System.Drawing.Color.White;
            this.txtM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtM.Location = new System.Drawing.Point(111, 70);
            this.txtM.Name = "txtM";
            this.txtM.Size = new System.Drawing.Size(134, 23);
            this.txtM.TabIndex = 2;
            // 
            // txtCuttingSP
            // 
            this.txtCuttingSP.BackColor = System.Drawing.Color.White;
            this.txtCuttingSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCuttingSP.Location = new System.Drawing.Point(111, 39);
            this.txtCuttingSP.MaxLength = 13;
            this.txtCuttingSP.Name = "txtCuttingSP";
            this.txtCuttingSP.Size = new System.Drawing.Size(134, 23);
            this.txtCuttingSP.TabIndex = 1;
            // 
            // labStatus
            // 
            this.labStatus.Location = new System.Drawing.Point(12, 131);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(96, 23);
            this.labStatus.TabIndex = 11;
            this.labStatus.Text = "Status";
            // 
            // labCuttingSP
            // 
            this.labCuttingSP.Location = new System.Drawing.Point(12, 39);
            this.labCuttingSP.Name = "labCuttingSP";
            this.labCuttingSP.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labCuttingSP.Size = new System.Drawing.Size(96, 23);
            this.labCuttingSP.TabIndex = 8;
            this.labCuttingSP.Text = "Cutting SP";
            this.labCuttingSP.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // lbEstCutDate
            // 
            this.lbEstCutDate.Location = new System.Drawing.Point(12, 7);
            this.lbEstCutDate.Name = "lbEstCutDate";
            this.lbEstCutDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbEstCutDate.Size = new System.Drawing.Size(96, 23);
            this.lbEstCutDate.TabIndex = 7;
            this.lbEstCutDate.Text = "Est. Cut Date";
            this.lbEstCutDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // comboDropDownList1
            // 
            this.comboDropDownList1.AddAllItem = true;
            this.comboDropDownList1.BackColor = System.Drawing.Color.White;
            this.comboDropDownList1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDropDownList1.FormattingEnabled = true;
            this.comboDropDownList1.IsSupportUnselect = true;
            this.comboDropDownList1.Location = new System.Drawing.Point(111, 131);
            this.comboDropDownList1.Name = "comboDropDownList1";
            this.comboDropDownList1.OldText = "";
            this.comboDropDownList1.Size = new System.Drawing.Size(134, 24);
            this.comboDropDownList1.TabIndex = 4;
            this.comboDropDownList1.Type = "PMS_SpreadingStatus";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(12, 100);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(96, 23);
            this.labelFactory.TabIndex = 10;
            this.labelFactory.Text = "Factory";
            // 
            // radioByCutRef
            // 
            this.radioByCutRef.AutoSize = true;
            this.radioByCutRef.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioByCutRef.Location = new System.Drawing.Point(124, 194);
            this.radioByCutRef.Name = "radioByCutRef";
            this.radioByCutRef.Size = new System.Drawing.Size(97, 21);
            this.radioByCutRef.TabIndex = 6;
            this.radioByCutRef.Text = "By CutRef#";
            this.radioByCutRef.UseVisualStyleBackColor = true;
            // 
            // radioBySP
            // 
            this.radioBySP.AutoSize = true;
            this.radioBySP.Checked = true;
            this.radioBySP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBySP.Location = new System.Drawing.Point(124, 167);
            this.radioBySP.Name = "radioBySP";
            this.radioBySP.Size = new System.Drawing.Size(64, 21);
            this.radioBySP.TabIndex = 5;
            this.radioBySP.TabStop = true;
            this.radioBySP.Text = "By SP";
            this.radioBySP.UseVisualStyleBackColor = true;
            // 
            // dateEstCutDate
            // 
            // 
            // 
            // 
            this.dateEstCutDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateEstCutDate.DateBox1.Name = "";
            this.dateEstCutDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateEstCutDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateEstCutDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateEstCutDate.DateBox2.Name = "";
            this.dateEstCutDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateEstCutDate.DateBox2.TabIndex = 1;
            this.dateEstCutDate.Location = new System.Drawing.Point(111, 7);
            this.dateEstCutDate.Name = "dateEstCutDate";
            this.dateEstCutDate.Size = new System.Drawing.Size(280, 23);
            this.dateEstCutDate.TabIndex = 0;
            // 
            // labReportType
            // 
            this.labReportType.Location = new System.Drawing.Point(12, 167);
            this.labReportType.Name = "labReportType";
            this.labReportType.Size = new System.Drawing.Size(96, 23);
            this.labReportType.TabIndex = 12;
            this.labReportType.Text = "Report Type";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(12, 70);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(96, 23);
            this.labelM.TabIndex = 9;
            this.labelM.Text = "M";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(442, 188);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 22);
            this.label4.TabIndex = 96;
            this.label4.Text = "Paper Size A4";
            // 
            // R01
            // 
            this.ClientSize = new System.Drawing.Size(545, 279);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "dateEstCutDate";
            this.DefaultControlForEdit = "dateEstCutDate";
            this.Name = "R01";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R01. Cutting Status";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Label label4;
        private Win.UI.Label labReportType;
        private Win.UI.Label labelM;
        private Win.UI.DateRange dateEstCutDate;
        private Win.UI.RadioButton radioByCutRef;
        private Win.UI.RadioButton radioBySP;
        private Win.UI.Label labelFactory;
        private Class.ComboDropDownList comboDropDownList1;
        private Win.UI.Label lbEstCutDate;
        private Win.UI.Label labCuttingSP;
        private Win.UI.Label labStatus;
        private Win.UI.TextBox txtCuttingSP;
        private Class.Txtfactory txtFactory;
        private Class.TxtMdivision txtM;
    }
}
