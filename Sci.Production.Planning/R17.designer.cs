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
            this.radioBulk = new Sci.Win.UI.RadioButton();
            this.radioSample = new Sci.Win.UI.RadioButton();
            this.dateFactoryKPIDate = new Sci.Win.UI.DateRange();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelCountry = new Sci.Win.UI.Label();
            this.checkExportDetailData = new Sci.Win.UI.CheckBox();
            this.labelFactoryKPIDate = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelType = new Sci.Win.UI.Label();
            this.txtMdivision = new Sci.Production.Class.txtMdivision();
            this.txtFactory = new Sci.Production.Class.txtfactory();
            this.txtCountry = new Sci.Production.Class.txtcountry();
            this.radioGarment = new Sci.Win.UI.RadioButton();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(445, 12);
            this.print.TabIndex = 7;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(445, 48);
            this.toexcel.TabIndex = 8;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(445, 84);
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
            this.dateFactoryKPIDate.Location = new System.Drawing.Point(139, 55);
            this.dateFactoryKPIDate.Name = "dateFactoryKPIDate";
            this.dateFactoryKPIDate.Size = new System.Drawing.Size(280, 23);
            this.dateFactoryKPIDate.TabIndex = 2;
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(18, 91);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(75, 23);
            this.labelFactory.TabIndex = 98;
            this.labelFactory.Text = "Factory";
            // 
            // labelCountry
            // 
            this.labelCountry.Lines = 0;
            this.labelCountry.Location = new System.Drawing.Point(18, 133);
            this.labelCountry.Name = "labelCountry";
            this.labelCountry.Size = new System.Drawing.Size(75, 23);
            this.labelCountry.TabIndex = 99;
            this.labelCountry.Text = "Country ";
            this.labelCountry.Visible = false;
            // 
            // checkExportDetailData
            // 
            this.checkExportDetailData.AutoSize = true;
            this.checkExportDetailData.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkExportDetailData.Location = new System.Drawing.Point(278, 90);
            this.checkExportDetailData.Name = "checkExportDetailData";
            this.checkExportDetailData.Size = new System.Drawing.Size(141, 21);
            this.checkExportDetailData.TabIndex = 6;
            this.checkExportDetailData.Text = "Export Detail Data";
            this.checkExportDetailData.UseVisualStyleBackColor = true;
            // 
            // labelFactoryKPIDate
            // 
            this.labelFactoryKPIDate.Lines = 0;
            this.labelFactoryKPIDate.Location = new System.Drawing.Point(18, 55);
            this.labelFactoryKPIDate.Name = "labelFactoryKPIDate";
            this.labelFactoryKPIDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelFactoryKPIDate.RectStyle.BorderWidth = 1F;
            this.labelFactoryKPIDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelFactoryKPIDate.RectStyle.ExtBorderWidth = 1F;
            this.labelFactoryKPIDate.Size = new System.Drawing.Size(118, 23);
            this.labelFactoryKPIDate.TabIndex = 103;
            this.labelFactoryKPIDate.Text = "Factory KPI Date";
            this.labelFactoryKPIDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelFactoryKPIDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(344, 133);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(75, 23);
            this.labelM.TabIndex = 104;
            this.labelM.Text = "M";
            this.labelM.Visible = false;
            // 
            // labelType
            // 
            this.labelType.Lines = 0;
            this.labelType.Location = new System.Drawing.Point(18, 19);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(75, 23);
            this.labelType.TabIndex = 105;
            this.labelType.Text = "Type";
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(422, 133);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 3;
            this.txtMdivision.Visible = false;
            // 
            // txtFactory
            // 
            this.txtFactory.BackColor = System.Drawing.Color.White;
            this.txtFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactory.Location = new System.Drawing.Point(96, 91);
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Size = new System.Drawing.Size(66, 23);
            this.txtFactory.TabIndex = 4;
            this.txtFactory.IssupportJunk = true;
            // 
            // txtCountry
            // 
            this.txtCountry.DisplayBox1Binding = "";
            this.txtCountry.Location = new System.Drawing.Point(96, 133);
            this.txtCountry.Name = "txtCountry";
            this.txtCountry.Size = new System.Drawing.Size(232, 38);
            this.txtCountry.TabIndex = 5;
            this.txtCountry.TextBox1Binding = "";
            this.txtCountry.Visible = false;
            // 
            // radioGarment
            // 
            this.radioGarment.AutoSize = true;
            this.radioGarment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioGarment.Location = new System.Drawing.Point(247, 17);
            this.radioGarment.Name = "radioGarment";
            this.radioGarment.Size = new System.Drawing.Size(81, 21);
            this.radioGarment.TabIndex = 106;
            this.radioGarment.TabStop = true;
            this.radioGarment.Text = "Garment";
            this.radioGarment.UseVisualStyleBackColor = true;
            // 
            // R17
            // 
            this.ClientSize = new System.Drawing.Size(537, 199);
            this.Controls.Add(this.radioGarment);
            this.Controls.Add(this.labelType);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelFactoryKPIDate);
            this.Controls.Add(this.txtFactory);
            this.Controls.Add(this.txtCountry);
            this.Controls.Add(this.checkExportDetailData);
            this.Controls.Add(this.labelCountry);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.dateFactoryKPIDate);
            this.Controls.Add(this.radioSample);
            this.Controls.Add(this.radioBulk);
            this.DefaultControl = "dateFactoryKPIDate";
            this.DefaultControlForEdit = "dateFactoryKPIDate";
            this.Name = "R17";
            this.Text = "R17. SDP Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.radioBulk, 0);
            this.Controls.SetChildIndex(this.radioSample, 0);
            this.Controls.SetChildIndex(this.dateFactoryKPIDate, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelCountry, 0);
            this.Controls.SetChildIndex(this.checkExportDetailData, 0);
            this.Controls.SetChildIndex(this.txtCountry, 0);
            this.Controls.SetChildIndex(this.txtFactory, 0);
            this.Controls.SetChildIndex(this.labelFactoryKPIDate, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.labelType, 0);
            this.Controls.SetChildIndex(this.radioGarment, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioButton radioBulk;
        private Win.UI.RadioButton radioSample;
        private Win.UI.DateRange dateFactoryKPIDate;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelCountry;
        private Win.UI.CheckBox checkExportDetailData;
        private Class.txtcountry txtCountry;
        private Class.txtfactory txtFactory;
        private Win.UI.Label labelFactoryKPIDate;
        private Win.UI.Label labelM;
        private Class.txtMdivision txtMdivision;
        private Win.UI.Label labelType;
        private Win.UI.RadioButton radioGarment;
    }
}
