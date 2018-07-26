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
            this.txtbrand = new Sci.Production.Class.txtbrand();
            this.labelBrand = new Sci.Win.UI.Label();
            this.txtuser1 = new Sci.Production.Class.txtuser();
            this.dateInspectionDate = new Sci.Win.UI.DateRange();
            this.label9 = new Sci.Win.UI.Label();
            this.txtSPEnd = new Sci.Win.UI.TextBox();
            this.txtSPStart = new Sci.Win.UI.TextBox();
            this.labelInspected = new Sci.Win.UI.Label();
            this.labelInspectionDate = new Sci.Win.UI.Label();
            this.labelSP = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
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
            this.panel1.Controls.Add(this.txtbrand);
            this.panel1.Controls.Add(this.labelBrand);
            this.panel1.Controls.Add(this.txtuser1);
            this.panel1.Controls.Add(this.dateInspectionDate);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtSPEnd);
            this.panel1.Controls.Add(this.txtSPStart);
            this.panel1.Controls.Add(this.labelInspected);
            this.panel1.Controls.Add(this.labelInspectionDate);
            this.panel1.Controls.Add(this.labelSP);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(426, 146);
            this.panel1.TabIndex = 94;
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
            // txtuser1
            // 
            this.txtuser1.DisplayBox1Binding = "";
            this.txtuser1.Location = new System.Drawing.Point(120, 42);
            this.txtuser1.Name = "txtuser1";
            this.txtuser1.Size = new System.Drawing.Size(300, 23);
            this.txtuser1.TabIndex = 1;
            this.txtuser1.TextBox1Binding = "";
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
            // R08
            // 
            this.ClientSize = new System.Drawing.Size(557, 194);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.panel1);
            this.Name = "R08";
            this.Text = "R08.Fabric Inspection Daily Report";
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
        private Class.txtuser txtuser1;
        private Class.txtbrand txtbrand;
        private Win.UI.Label labelBrand;
    }
}
