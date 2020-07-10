namespace Sci.Production.Quality
{
    partial class R08
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
            this.txtmulituser = new Sci.Production.Class.Txtmulituser();
            this.radioSummery = new Sci.Win.UI.RadioButton();
            this.radioDetail = new Sci.Win.UI.RadioButton();
            this.labReportType = new Sci.Win.UI.Label();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.labelBrand = new Sci.Win.UI.Label();
            this.dateInspectionDate = new Sci.Win.UI.DateRange();
            this.label9 = new Sci.Win.UI.Label();
            this.txtSPEnd = new Sci.Win.UI.TextBox();
            this.txtSPStart = new Sci.Win.UI.TextBox();
            this.labelInspected = new Sci.Win.UI.Label();
            this.labelInspectionDate = new Sci.Win.UI.Label();
            this.labelSP = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
            this.lbRefno = new Sci.Win.UI.Label();
            this.txtRefno1 = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtRefno2 = new Sci.Win.UI.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(465, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(465, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(465, 84);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtRefno2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtRefno1);
            this.panel1.Controls.Add(this.lbRefno);
            this.panel1.Controls.Add(this.txtmulituser);
            this.panel1.Controls.Add(this.radioSummery);
            this.panel1.Controls.Add(this.radioDetail);
            this.panel1.Controls.Add(this.labReportType);
            this.panel1.Controls.Add(this.txtbrand);
            this.panel1.Controls.Add(this.labelBrand);
            this.panel1.Controls.Add(this.dateInspectionDate);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtSPEnd);
            this.panel1.Controls.Add(this.txtSPStart);
            this.panel1.Controls.Add(this.labelInspected);
            this.panel1.Controls.Add(this.labelInspectionDate);
            this.panel1.Controls.Add(this.labelSP);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(426, 225);
            this.panel1.TabIndex = 94;
            // 
            // txtmulituser
            // 
            this.txtmulituser.DisplayBox1Binding = "";
            this.txtmulituser.Location = new System.Drawing.Point(123, 42);
            this.txtmulituser.Name = "txtmulituser";
            this.txtmulituser.Size = new System.Drawing.Size(300, 23);
            this.txtmulituser.TabIndex = 1;
            this.txtmulituser.TextBox1Binding = "";
            // 
            // radioSummery
            // 
            this.radioSummery.AutoSize = true;
            this.radioSummery.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioSummery.Location = new System.Drawing.Point(200, 186);
            this.radioSummery.Name = "radioSummery";
            this.radioSummery.Size = new System.Drawing.Size(85, 21);
            this.radioSummery.TabIndex = 8;
            this.radioSummery.Text = "Summery";
            this.radioSummery.UseVisualStyleBackColor = true;
            // 
            // radioDetail
            // 
            this.radioDetail.AutoSize = true;
            this.radioDetail.Checked = true;
            this.radioDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioDetail.Location = new System.Drawing.Point(132, 186);
            this.radioDetail.Name = "radioDetail";
            this.radioDetail.Size = new System.Drawing.Size(62, 21);
            this.radioDetail.TabIndex = 7;
            this.radioDetail.TabStop = true;
            this.radioDetail.Text = "Detail";
            this.radioDetail.UseVisualStyleBackColor = true;
            // 
            // labReportType
            // 
            this.labReportType.Location = new System.Drawing.Point(12, 181);
            this.labReportType.Name = "labReportType";
            this.labReportType.Size = new System.Drawing.Size(105, 23);
            this.labReportType.TabIndex = 115;
            this.labReportType.Text = "Report Type";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(120, 113);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(94, 23);
            this.txtbrand.TabIndex = 4;
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(12, 113);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(105, 23);
            this.labelBrand.TabIndex = 114;
            this.labelBrand.Text = "Brand";
            // 
            // dateInspectionDate
            // 
            // 
            // 
            // 
            this.dateInspectionDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateInspectionDate.DateBox1.Name = "";
            this.dateInspectionDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateInspectionDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateInspectionDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateInspectionDate.DateBox2.Name = "";
            this.dateInspectionDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateInspectionDate.DateBox2.TabIndex = 1;
            this.dateInspectionDate.IsRequired = false;
            this.dateInspectionDate.Location = new System.Drawing.Point(120, 7);
            this.dateInspectionDate.Name = "dateInspectionDate";
            this.dateInspectionDate.Size = new System.Drawing.Size(280, 23);
            this.dateInspectionDate.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(239, 79);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 23);
            this.label9.TabIndex = 110;
            this.label9.Text = "～";
            this.label9.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            this.label9.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtSPEnd
            // 
            this.txtSPEnd.BackColor = System.Drawing.Color.White;
            this.txtSPEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPEnd.Location = new System.Drawing.Point(262, 79);
            this.txtSPEnd.MaxLength = 13;
            this.txtSPEnd.Name = "txtSPEnd";
            this.txtSPEnd.Size = new System.Drawing.Size(116, 23);
            this.txtSPEnd.TabIndex = 3;
            // 
            // txtSPStart
            // 
            this.txtSPStart.BackColor = System.Drawing.Color.White;
            this.txtSPStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPStart.Location = new System.Drawing.Point(120, 79);
            this.txtSPStart.MaxLength = 13;
            this.txtSPStart.Name = "txtSPStart";
            this.txtSPStart.Size = new System.Drawing.Size(116, 23);
            this.txtSPStart.TabIndex = 2;
            // 
            // labelInspected
            // 
            this.labelInspected.Location = new System.Drawing.Point(12, 42);
            this.labelInspected.Name = "labelInspected";
            this.labelInspected.Size = new System.Drawing.Size(105, 23);
            this.labelInspected.TabIndex = 7;
            this.labelInspected.Text = "Inspected";
            // 
            // labelInspectionDate
            // 
            this.labelInspectionDate.Location = new System.Drawing.Point(12, 7);
            this.labelInspectionDate.Name = "labelInspectionDate";
            this.labelInspectionDate.Size = new System.Drawing.Size(105, 23);
            this.labelInspectionDate.TabIndex = 2;
            this.labelInspectionDate.Text = "Inspection Date";
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(12, 79);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(105, 23);
            this.labelSP.TabIndex = 0;
            this.labelSP.Text = "SP#";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(453, 126);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 22);
            this.label10.TabIndex = 97;
            this.label10.Text = "Paper Size A4";
            // 
            // lbRefno
            // 
            this.lbRefno.Location = new System.Drawing.Point(12, 147);
            this.lbRefno.Name = "lbRefno";
            this.lbRefno.Size = new System.Drawing.Size(105, 23);
            this.lbRefno.TabIndex = 116;
            this.lbRefno.Text = "Refno";
            // 
            // txtRefno1
            // 
            this.txtRefno1.BackColor = System.Drawing.Color.White;
            this.txtRefno1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefno1.Location = new System.Drawing.Point(120, 147);
            this.txtRefno1.MaxLength = 13;
            this.txtRefno1.Name = "txtRefno1";
            this.txtRefno1.Size = new System.Drawing.Size(116, 23);
            this.txtRefno1.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(239, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 23);
            this.label1.TabIndex = 118;
            this.label1.Text = "～";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            this.label1.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label1.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtRefno2
            // 
            this.txtRefno2.BackColor = System.Drawing.Color.White;
            this.txtRefno2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefno2.Location = new System.Drawing.Point(262, 147);
            this.txtRefno2.MaxLength = 13;
            this.txtRefno2.Name = "txtRefno2";
            this.txtRefno2.Size = new System.Drawing.Size(116, 23);
            this.txtRefno2.TabIndex = 6;
            // 
            // R08
            // 
            this.ClientSize = new System.Drawing.Size(557, 279);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.panel1);
            this.Name = "R08";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R08.Fabric Inspection Daily Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Label labelInspected;
        private Win.UI.Label labelInspectionDate;
        private Win.UI.Label labelSP;
        private Win.UI.TextBox txtSPEnd;
        private Win.UI.TextBox txtSPStart;
        private Win.UI.DateRange dateInspectionDate;
        private Win.UI.Label label9;
        private Win.UI.Label label10;
        private Class.Txtbrand txtbrand;
        private Win.UI.Label labelBrand;
        private Win.UI.RadioButton radioSummery;
        private Win.UI.RadioButton radioDetail;
        private Win.UI.Label labReportType;
        private Class.Txtmulituser txtmulituser;
        private Win.UI.Label lbRefno;
        private Win.UI.TextBox txtRefno2;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtRefno1;
    }
}
