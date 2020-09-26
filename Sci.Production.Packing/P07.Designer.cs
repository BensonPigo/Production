namespace Sci.Production.Packing
{
    partial class P07
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.radioGroup2 = new Sci.Win.UI.RadioGroup();
            this.btnToExcelCombo = new Sci.Win.UI.Button();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioFormB = new Sci.Win.UI.RadioButton();
            this.radioFormA = new Sci.Win.UI.RadioButton();
            this.radioGroup1 = new Sci.Win.UI.RadioGroup();
            this.label1 = new Sci.Win.UI.Label();
            this.txtSP_e = new Sci.Win.UI.TextBox();
            this.txtSP_s = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.dateFCRDate = new Sci.Win.UI.DateRange();
            this.label4 = new Sci.Win.UI.Label();
            this.txtGarmentBookingEnd = new Sci.Win.UI.TextBox();
            this.txtGarmentBookingStart = new Sci.Win.UI.TextBox();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelFCRDate = new Sci.Win.UI.Label();
            this.labelGarmentBooking = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel3.SuspendLayout();
            this.radioGroup2.SuspendLayout();
            this.radioPanel1.SuspendLayout();
            this.radioGroup1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 580);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(898, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 580);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.radioGroup2);
            this.panel3.Controls.Add(this.radioGroup1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(888, 201);
            this.panel3.TabIndex = 3;
            // 
            // radioGroup2
            // 
            this.radioGroup2.Controls.Add(this.btnToExcelCombo);
            this.radioGroup2.Controls.Add(this.btnToExcel);
            this.radioGroup2.Controls.Add(this.radioPanel1);
            this.radioGroup2.Location = new System.Drawing.Point(526, 4);
            this.radioGroup2.Name = "radioGroup2";
            this.radioGroup2.Size = new System.Drawing.Size(357, 191);
            this.radioGroup2.TabIndex = 1;
            this.radioGroup2.TabStop = false;
            // 
            // btnToExcelCombo
            // 
            this.btnToExcelCombo.Location = new System.Drawing.Point(207, 48);
            this.btnToExcelCombo.Name = "btnToExcelCombo";
            this.btnToExcelCombo.Size = new System.Drawing.Size(145, 30);
            this.btnToExcelCombo.TabIndex = 2;
            this.btnToExcelCombo.Text = "To Excel by combo";
            this.btnToExcelCombo.UseVisualStyleBackColor = true;
            this.btnToExcelCombo.Click += new System.EventHandler(this.BtnToExcelCombo_Click);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Location = new System.Drawing.Point(272, 15);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(80, 30);
            this.btnToExcel.TabIndex = 1;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioFormB);
            this.radioPanel1.Controls.Add(this.radioFormA);
            this.radioPanel1.Location = new System.Drawing.Point(7, 15);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(255, 63);
            this.radioPanel1.TabIndex = 0;
            // 
            // radioFormB
            // 
            this.radioFormB.AutoSize = true;
            this.radioFormB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioFormB.Location = new System.Drawing.Point(4, 36);
            this.radioFormB.Name = "radioFormB";
            this.radioFormB.Size = new System.Drawing.Size(157, 21);
            this.radioFormB.TabIndex = 1;
            this.radioFormB.TabStop = true;
            this.radioFormB.Text = "FormB (for LLL/TNF)";
            this.radioFormB.UseVisualStyleBackColor = true;
            // 
            // radioFormA
            // 
            this.radioFormA.AutoSize = true;
            this.radioFormA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioFormA.Location = new System.Drawing.Point(4, 8);
            this.radioFormA.Name = "radioFormA";
            this.radioFormA.Size = new System.Drawing.Size(250, 21);
            this.radioFormA.TabIndex = 0;
            this.radioFormA.TabStop = true;
            this.radioFormA.Text = "FormA (for Adidas/UA/Saucony/NB)";
            this.radioFormA.UseVisualStyleBackColor = true;
            // 
            // radioGroup1
            // 
            this.radioGroup1.Controls.Add(this.label1);
            this.radioGroup1.Controls.Add(this.txtSP_e);
            this.radioGroup1.Controls.Add(this.txtSP_s);
            this.radioGroup1.Controls.Add(this.label2);
            this.radioGroup1.Controls.Add(this.btnQuery);
            this.radioGroup1.Controls.Add(this.txtbrand);
            this.radioGroup1.Controls.Add(this.dateFCRDate);
            this.radioGroup1.Controls.Add(this.label4);
            this.radioGroup1.Controls.Add(this.txtGarmentBookingEnd);
            this.radioGroup1.Controls.Add(this.txtGarmentBookingStart);
            this.radioGroup1.Controls.Add(this.labelBrand);
            this.radioGroup1.Controls.Add(this.labelFCRDate);
            this.radioGroup1.Controls.Add(this.labelGarmentBooking);
            this.radioGroup1.Location = new System.Drawing.Point(6, 4);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Size = new System.Drawing.Size(501, 191);
            this.radioGroup1.TabIndex = 0;
            this.radioGroup1.TabStop = false;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(319, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 23);
            this.label1.TabIndex = 12;
            this.label1.Text = "～";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            this.label1.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label1.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtSP_e
            // 
            this.txtSP_e.BackColor = System.Drawing.Color.White;
            this.txtSP_e.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP_e.Location = new System.Drawing.Point(131, 102);
            this.txtSP_e.Name = "txtSP_e";
            this.txtSP_e.Size = new System.Drawing.Size(186, 23);
            this.txtSP_e.TabIndex = 11;
            // 
            // txtSP_s
            // 
            this.txtSP_s.BackColor = System.Drawing.Color.White;
            this.txtSP_s.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP_s.Location = new System.Drawing.Point(131, 73);
            this.txtSP_s.Name = "txtSP_s";
            this.txtSP_s.Size = new System.Drawing.Size(186, 23);
            this.txtSP_s.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(7, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 23);
            this.label2.TabIndex = 9;
            this.label2.Text = "SP#";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(405, 15);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 8;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(131, 162);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(98, 23);
            this.txtbrand.TabIndex = 7;
            // 
            // dateFCRDate
            // 
            // 
            // 
            // 
            this.dateFCRDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateFCRDate.DateBox1.Name = "";
            this.dateFCRDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateFCRDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateFCRDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateFCRDate.DateBox2.Name = "";
            this.dateFCRDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateFCRDate.DateBox2.TabIndex = 1;
            this.dateFCRDate.IsRequired = false;
            this.dateFCRDate.Location = new System.Drawing.Point(131, 129);
            this.dateFCRDate.Name = "dateFCRDate";
            this.dateFCRDate.Size = new System.Drawing.Size(280, 23);
            this.dateFCRDate.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(319, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 23);
            this.label4.TabIndex = 5;
            this.label4.Text = "～";
            this.label4.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            this.label4.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label4.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtGarmentBookingEnd
            // 
            this.txtGarmentBookingEnd.BackColor = System.Drawing.Color.White;
            this.txtGarmentBookingEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtGarmentBookingEnd.Location = new System.Drawing.Point(131, 44);
            this.txtGarmentBookingEnd.Name = "txtGarmentBookingEnd";
            this.txtGarmentBookingEnd.Size = new System.Drawing.Size(186, 23);
            this.txtGarmentBookingEnd.TabIndex = 4;
            // 
            // txtGarmentBookingStart
            // 
            this.txtGarmentBookingStart.BackColor = System.Drawing.Color.White;
            this.txtGarmentBookingStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtGarmentBookingStart.Location = new System.Drawing.Point(131, 15);
            this.txtGarmentBookingStart.Name = "txtGarmentBookingStart";
            this.txtGarmentBookingStart.Size = new System.Drawing.Size(186, 23);
            this.txtGarmentBookingStart.TabIndex = 3;
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(7, 162);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(120, 23);
            this.labelBrand.TabIndex = 2;
            this.labelBrand.Text = "Brand";
            // 
            // labelFCRDate
            // 
            this.labelFCRDate.Location = new System.Drawing.Point(7, 129);
            this.labelFCRDate.Name = "labelFCRDate";
            this.labelFCRDate.Size = new System.Drawing.Size(120, 23);
            this.labelFCRDate.TabIndex = 1;
            this.labelFCRDate.Text = "FCR Date";
            // 
            // labelGarmentBooking
            // 
            this.labelGarmentBooking.Location = new System.Drawing.Point(7, 15);
            this.labelGarmentBooking.Name = "labelGarmentBooking";
            this.labelGarmentBooking.Size = new System.Drawing.Size(120, 23);
            this.labelGarmentBooking.TabIndex = 0;
            this.labelGarmentBooking.Text = "Garment Booking#";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(10, 538);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(888, 42);
            this.panel4.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(803, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridDetail);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 201);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(888, 337);
            this.panel5.TabIndex = 5;
            // 
            // gridDetail
            // 
            this.gridDetail.AllowUserToAddRows = false;
            this.gridDetail.AllowUserToDeleteRows = false;
            this.gridDetail.AllowUserToResizeRows = false;
            this.gridDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetail.DataSource = this.listControlBindingSource1;
            this.gridDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDetail.Location = new System.Drawing.Point(0, 0);
            this.gridDetail.Name = "gridDetail";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridDetail.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.ShowCellToolTips = false;
            this.gridDetail.Size = new System.Drawing.Size(888, 337);
            this.gridDetail.TabIndex = 0;
            this.gridDetail.TabStop = false;
            // 
            // P07
            // 
            this.ClientSize = new System.Drawing.Size(908, 580);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.EditMode = true;
            this.Name = "P07";
            this.Text = "P07. Batch Print Packing List Report (Bulk)";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.panel5, 0);
            this.panel3.ResumeLayout(false);
            this.radioGroup2.ResumeLayout(false);
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.radioGroup1.ResumeLayout(false);
            this.radioGroup1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.RadioGroup radioGroup1;
        private Win.UI.TextBox txtGarmentBookingStart;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelFCRDate;
        private Win.UI.Label labelGarmentBooking;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridDetail;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.RadioGroup radioGroup2;
        private Win.UI.Button btnToExcel;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioFormB;
        private Win.UI.RadioButton radioFormA;
        private Win.UI.Button btnQuery;
        private Class.Txtbrand txtbrand;
        private Win.UI.DateRange dateFCRDate;
        private Win.UI.Label label4;
        private Win.UI.TextBox txtGarmentBookingEnd;
        private Win.UI.Button btnClose;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtSP_e;
        private Win.UI.TextBox txtSP_s;
        private Win.UI.Label label2;
        private Win.UI.Button btnToExcelCombo;
    }
}
