namespace Sci.Production.Planning
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
            this.radioBulk = new Sci.Win.UI.RadioButton();
            this.radioSample = new Sci.Win.UI.RadioButton();
            this.dateFactoryKPIDate = new Sci.Win.UI.DateRange();
            this.labelFactory = new Sci.Win.UI.Label();
            this.checkExportDetailData = new Sci.Win.UI.CheckBox();
            this.labelFactoryKPIDate = new Sci.Win.UI.Label();
            this.labelType = new Sci.Win.UI.Label();
            this.radioGarment = new Sci.Win.UI.RadioButton();
            this.txtFactory = new Sci.Production.Class.txtfactory();
            this.label1 = new Sci.Win.UI.Label();
            this.comboDropDownList1 = new Sci.Production.Class.comboDropDownList(this.components);
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(451, 12);
            this.print.TabIndex = 7;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(451, 48);
            this.toexcel.TabIndex = 8;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(451, 84);
            this.close.TabIndex = 9;
            // 
            // radioBulk
            // 
            this.radioBulk.AutoSize = true;
            this.radioBulk.Checked = true;
            this.radioBulk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBulk.Location = new System.Drawing.Point(109, 17);
            this.radioBulk.Name = "radioBulk";
            this.radioBulk.Size = new System.Drawing.Size(53, 21);
            this.radioBulk.TabIndex = 0;
            this.radioBulk.TabStop = true;
            this.radioBulk.Text = "Bulk";
            this.radioBulk.UseVisualStyleBackColor = true;
            // 
            // radioSample
            // 
            this.radioSample.AutoSize = true;
            this.radioSample.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSample.Location = new System.Drawing.Point(168, 17);
            this.radioSample.Name = "radioSample";
            this.radioSample.Size = new System.Drawing.Size(73, 21);
            this.radioSample.TabIndex = 1;
            this.radioSample.TabStop = true;
            this.radioSample.Text = "Sample";
            this.radioSample.UseVisualStyleBackColor = true;
            // 
            // dateFactoryKPIDate
            // 
            // 
            // 
            // 
            this.dateFactoryKPIDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateFactoryKPIDate.DateBox1.Name = "";
            this.dateFactoryKPIDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateFactoryKPIDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateFactoryKPIDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateFactoryKPIDate.DateBox2.Name = "";
            this.dateFactoryKPIDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateFactoryKPIDate.DateBox2.TabIndex = 1;
            this.dateFactoryKPIDate.IsRequired = false;
            this.dateFactoryKPIDate.Location = new System.Drawing.Point(96, 78);
            this.dateFactoryKPIDate.Name = "dateFactoryKPIDate";
            this.dateFactoryKPIDate.Size = new System.Drawing.Size(280, 23);
            this.dateFactoryKPIDate.TabIndex = 4;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(18, 107);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(75, 23);
            this.labelFactory.TabIndex = 98;
            this.labelFactory.Text = "Factory";
            // 
            // checkExportDetailData
            // 
            this.checkExportDetailData.AutoSize = true;
            this.checkExportDetailData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkExportDetailData.Location = new System.Drawing.Point(18, 136);
            this.checkExportDetailData.Name = "checkExportDetailData";
            this.checkExportDetailData.Size = new System.Drawing.Size(141, 21);
            this.checkExportDetailData.TabIndex = 6;
            this.checkExportDetailData.Text = "Export Detail Data";
            this.checkExportDetailData.UseVisualStyleBackColor = true;
            // 
            // labelFactoryKPIDate
            // 
            this.labelFactoryKPIDate.Location = new System.Drawing.Point(18, 78);
            this.labelFactoryKPIDate.Name = "labelFactoryKPIDate";
            this.labelFactoryKPIDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelFactoryKPIDate.RectStyle.BorderWidth = 1F;
            this.labelFactoryKPIDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelFactoryKPIDate.RectStyle.ExtBorderWidth = 1F;
            this.labelFactoryKPIDate.Size = new System.Drawing.Size(75, 23);
            this.labelFactoryKPIDate.TabIndex = 103;
            this.labelFactoryKPIDate.Text = "Date Range";
            this.labelFactoryKPIDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelFactoryKPIDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelType
            // 
            this.labelType.Location = new System.Drawing.Point(18, 19);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(75, 23);
            this.labelType.TabIndex = 105;
            this.labelType.Text = "Type";
            // 
            // radioGarment
            // 
            this.radioGarment.AutoSize = true;
            this.radioGarment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioGarment.Location = new System.Drawing.Point(247, 17);
            this.radioGarment.Name = "radioGarment";
            this.radioGarment.Size = new System.Drawing.Size(81, 21);
            this.radioGarment.TabIndex = 2;
            this.radioGarment.TabStop = true;
            this.radioGarment.Text = "Garment";
            this.radioGarment.UseVisualStyleBackColor = true;
            // 
            // txtFactory
            // 
            this.txtFactory.BackColor = System.Drawing.Color.White;
            this.txtFactory.boolFtyGroupList = true;
            this.txtFactory.FilteMDivision = false;
            this.txtFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactory.IsProduceFty = false;
            this.txtFactory.IssupportJunk = true;
            this.txtFactory.Location = new System.Drawing.Point(96, 107);
            this.txtFactory.MDivision = null;
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Size = new System.Drawing.Size(66, 23);
            this.txtFactory.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(18, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 107;
            this.label1.Text = "SDP";
            // 
            // comboDropDownList1
            // 
            this.comboDropDownList1.BackColor = System.Drawing.Color.White;
            this.comboDropDownList1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDropDownList1.FormattingEnabled = true;
            this.comboDropDownList1.IsSupportUnselect = true;
            this.comboDropDownList1.Location = new System.Drawing.Point(96, 48);
            this.comboDropDownList1.Name = "comboDropDownList1";
            this.comboDropDownList1.OldText = "";
            this.comboDropDownList1.Size = new System.Drawing.Size(121, 24);
            this.comboDropDownList1.TabIndex = 3;
            this.comboDropDownList1.Type = "SDP";
            // 
            // R17
            // 
            this.ClientSize = new System.Drawing.Size(543, 186);
            this.Controls.Add(this.comboDropDownList1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioGarment);
            this.Controls.Add(this.labelType);
            this.Controls.Add(this.labelFactoryKPIDate);
            this.Controls.Add(this.txtFactory);
            this.Controls.Add(this.checkExportDetailData);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.dateFactoryKPIDate);
            this.Controls.Add(this.radioSample);
            this.Controls.Add(this.radioBulk);
            this.DefaultControl = "dateFactoryKPIDate";
            this.DefaultControlForEdit = "dateFactoryKPIDate";
            this.Name = "R17";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R17. SDP Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.radioBulk, 0);
            this.Controls.SetChildIndex(this.radioSample, 0);
            this.Controls.SetChildIndex(this.dateFactoryKPIDate, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.checkExportDetailData, 0);
            this.Controls.SetChildIndex(this.txtFactory, 0);
            this.Controls.SetChildIndex(this.labelFactoryKPIDate, 0);
            this.Controls.SetChildIndex(this.labelType, 0);
            this.Controls.SetChildIndex(this.radioGarment, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.comboDropDownList1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioButton radioBulk;
        private Win.UI.RadioButton radioSample;
        private Win.UI.DateRange dateFactoryKPIDate;
        private Win.UI.Label labelFactory;
        private Win.UI.CheckBox checkExportDetailData;
        private Class.txtfactory txtFactory;
        private Win.UI.Label labelFactoryKPIDate;
        private Win.UI.Label labelType;
        private Win.UI.RadioButton radioGarment;
        private Win.UI.Label label1;
        private Class.comboDropDownList comboDropDownList1;
    }
}
