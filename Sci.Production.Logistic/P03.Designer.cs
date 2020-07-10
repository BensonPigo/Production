namespace Sci.Production.Logistic
{
    partial class P03
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
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridReceiveDate = new Sci.Win.UI.Grid();
            this.txtPONo = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.labelPONo = new Sci.Win.UI.Label();
            this.labelPackID = new Sci.Win.UI.Label();
            this.txtPackID = new Sci.Win.UI.TextBox();
            this.btnFind = new Sci.Win.UI.Button();
            this.panel2 = new Sci.Win.UI.Panel();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.panel1 = new Sci.Win.UI.Panel();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnImportFromBarcode = new Sci.Win.UI.Button();
            this.panel4 = new Sci.Win.UI.Panel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel3 = new Sci.Win.UI.Panel();
            this.chkOnlyReqCarton = new Sci.Win.UI.CheckBox();
            this.dateReqDate = new Sci.Win.UI.DateRange();
            this.label3 = new Sci.Win.UI.Label();
            this.numTotalCTNQty = new Sci.Win.UI.NumericBox();
            this.numSelectedCTNQty = new Sci.Win.UI.NumericBox();
            this.lbTotalCTNQty = new Sci.Win.UI.Label();
            this.lbSelectedCTNQty = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.lbFactory = new Sci.Win.UI.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new Sci.Win.UI.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape5 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape4 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape3 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape2 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridReceiveDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridReceiveDate);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 187);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(692, 348);
            this.panel5.TabIndex = 21;
            // 
            // gridReceiveDate
            // 
            this.gridReceiveDate.AllowUserToAddRows = false;
            this.gridReceiveDate.AllowUserToDeleteRows = false;
            this.gridReceiveDate.AllowUserToResizeRows = false;
            this.gridReceiveDate.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridReceiveDate.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridReceiveDate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridReceiveDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridReceiveDate.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridReceiveDate.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridReceiveDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridReceiveDate.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridReceiveDate.Location = new System.Drawing.Point(0, 0);
            this.gridReceiveDate.Name = "gridReceiveDate";
            this.gridReceiveDate.RowHeadersVisible = false;
            this.gridReceiveDate.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridReceiveDate.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridReceiveDate.RowTemplate.Height = 24;
            this.gridReceiveDate.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridReceiveDate.ShowCellToolTips = false;
            this.gridReceiveDate.Size = new System.Drawing.Size(692, 348);
            this.gridReceiveDate.TabIndex = 10;
            this.gridReceiveDate.TabStop = false;
            this.gridReceiveDate.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridReceiveDate_ColumnHeaderMouseClick);
            // 
            // txtPONo
            // 
            this.txtPONo.BackColor = System.Drawing.Color.White;
            this.txtPONo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPONo.Location = new System.Drawing.Point(240, 15);
            this.txtPONo.Name = "txtPONo";
            this.txtPONo.Size = new System.Drawing.Size(153, 23);
            this.txtPONo.TabIndex = 3;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(12, 15);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(40, 23);
            this.labelSPNo.TabIndex = 0;
            this.labelSPNo.Text = "SP#";
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(56, 15);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(100, 23);
            this.txtSPNo.TabIndex = 1;
            // 
            // labelPONo
            // 
            this.labelPONo.Location = new System.Drawing.Point(196, 15);
            this.labelPONo.Name = "labelPONo";
            this.labelPONo.Size = new System.Drawing.Size(40, 23);
            this.labelPONo.TabIndex = 2;
            this.labelPONo.Text = "PO#";
            // 
            // labelPackID
            // 
            this.labelPackID.Location = new System.Drawing.Point(433, 15);
            this.labelPackID.Name = "labelPackID";
            this.labelPackID.Size = new System.Drawing.Size(52, 23);
            this.labelPackID.TabIndex = 4;
            this.labelPackID.Text = "PackID";
            // 
            // txtPackID
            // 
            this.txtPackID.BackColor = System.Drawing.Color.White;
            this.txtPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackID.Location = new System.Drawing.Point(489, 15);
            this.txtPackID.Name = "txtPackID";
            this.txtPackID.Size = new System.Drawing.Size(120, 23);
            this.txtPackID.TabIndex = 5;
            // 
            // btnFind
            // 
            this.btnFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFind.Location = new System.Drawing.Point(623, 12);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 30);
            this.btnFind.TabIndex = 6;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(702, 187);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 348);
            this.panel2.TabIndex = 18;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSave.Location = new System.Drawing.Point(539, 151);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(625, 151);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 14;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 187);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 348);
            this.panel1.TabIndex = 17;
            // 
            // btnImportFromBarcode
            // 
            this.btnImportFromBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportFromBarcode.Location = new System.Drawing.Point(527, 111);
            this.btnImportFromBarcode.Name = "btnImportFromBarcode";
            this.btnImportFromBarcode.Size = new System.Drawing.Size(176, 30);
            this.btnImportFromBarcode.TabIndex = 12;
            this.btnImportFromBarcode.Text = "Import From Barcode";
            this.btnImportFromBarcode.UseVisualStyleBackColor = true;
            this.btnImportFromBarcode.Click += new System.EventHandler(this.BtnImportFromBarcode_Click);
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 535);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(712, 10);
            this.panel4.TabIndex = 20;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chkOnlyReqCarton);
            this.panel3.Controls.Add(this.dateReqDate);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.numTotalCTNQty);
            this.panel3.Controls.Add(this.numSelectedCTNQty);
            this.panel3.Controls.Add(this.lbTotalCTNQty);
            this.panel3.Controls.Add(this.lbSelectedCTNQty);
            this.panel3.Controls.Add(this.txtfactory);
            this.panel3.Controls.Add(this.lbFactory);
            this.panel3.Controls.Add(this.dateTimePicker2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.dateTimePicker1);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Controls.Add(this.txtPONo);
            this.panel3.Controls.Add(this.labelSPNo);
            this.panel3.Controls.Add(this.txtSPNo);
            this.panel3.Controls.Add(this.labelPONo);
            this.panel3.Controls.Add(this.labelPackID);
            this.panel3.Controls.Add(this.txtPackID);
            this.panel3.Controls.Add(this.btnFind);
            this.panel3.Controls.Add(this.btnImportFromBarcode);
            this.panel3.Controls.Add(this.shapeContainer2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(712, 187);
            this.panel3.TabIndex = 19;
            // 
            // chkOnlyReqCarton
            // 
            this.chkOnlyReqCarton.AutoSize = true;
            this.chkOnlyReqCarton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkOnlyReqCarton.Location = new System.Drawing.Point(12, 117);
            this.chkOnlyReqCarton.Name = "chkOnlyReqCarton";
            this.chkOnlyReqCarton.Size = new System.Drawing.Size(252, 21);
            this.chkOnlyReqCarton.TabIndex = 11;
            this.chkOnlyReqCarton.Text = "Only Show Factory Request Carton ";
            this.chkOnlyReqCarton.UseVisualStyleBackColor = true;
            this.chkOnlyReqCarton.CheckedChanged += new System.EventHandler(this.ChkOnlyReqCarton_CheckedChanged);
            // 
            // dateReqDate
            // 
            // 
            // 
            // 
            this.dateReqDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateReqDate.DateBox1.Name = "";
            this.dateReqDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateReqDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateReqDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateReqDate.DateBox2.Name = "";
            this.dateReqDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateReqDate.DateBox2.TabIndex = 1;
            this.dateReqDate.Location = new System.Drawing.Point(107, 76);
            this.dateReqDate.Name = "dateReqDate";
            this.dateReqDate.Size = new System.Drawing.Size(280, 23);
            this.dateReqDate.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 23);
            this.label3.TabIndex = 50;
            this.label3.Text = "Request Date";
            // 
            // numTotalCTNQty
            // 
            this.numTotalCTNQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotalCTNQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotalCTNQty.IsSupportEditMode = false;
            this.numTotalCTNQty.Location = new System.Drawing.Point(488, 155);
            this.numTotalCTNQty.Name = "numTotalCTNQty";
            this.numTotalCTNQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalCTNQty.ReadOnly = true;
            this.numTotalCTNQty.Size = new System.Drawing.Size(44, 23);
            this.numTotalCTNQty.TabIndex = 49;
            this.numTotalCTNQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numSelectedCTNQty
            // 
            this.numSelectedCTNQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numSelectedCTNQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numSelectedCTNQty.IsSupportEditMode = false;
            this.numSelectedCTNQty.Location = new System.Drawing.Point(329, 155);
            this.numSelectedCTNQty.Name = "numSelectedCTNQty";
            this.numSelectedCTNQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSelectedCTNQty.ReadOnly = true;
            this.numSelectedCTNQty.Size = new System.Drawing.Size(41, 23);
            this.numSelectedCTNQty.TabIndex = 48;
            this.numSelectedCTNQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // lbTotalCTNQty
            // 
            this.lbTotalCTNQty.Location = new System.Drawing.Point(385, 155);
            this.lbTotalCTNQty.Name = "lbTotalCTNQty";
            this.lbTotalCTNQty.Size = new System.Drawing.Size(100, 23);
            this.lbTotalCTNQty.TabIndex = 47;
            this.lbTotalCTNQty.Text = "Total CTN Qty:";
            // 
            // lbSelectedCTNQty
            // 
            this.lbSelectedCTNQty.Location = new System.Drawing.Point(206, 155);
            this.lbSelectedCTNQty.Name = "lbSelectedCTNQty";
            this.lbSelectedCTNQty.Size = new System.Drawing.Size(120, 23);
            this.lbSelectedCTNQty.TabIndex = 46;
            this.lbSelectedCTNQty.Text = "Selected CTN Qty:";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(488, 47);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 9;
            // 
            // lbFactory
            // 
            this.lbFactory.Location = new System.Drawing.Point(433, 46);
            this.lbFactory.Name = "lbFactory";
            this.lbFactory.Size = new System.Drawing.Size(52, 23);
            this.lbFactory.TabIndex = 34;
            this.lbFactory.Text = "Factory";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(279, 47);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(146, 23);
            this.dateTimePicker2.TabIndex = 8;
            this.dateTimePicker2.Value = new System.DateTime(2017, 6, 1, 14, 42, 7, 0);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 23);
            this.label1.TabIndex = 30;
            this.label1.Text = "Receive Date";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(107, 46);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(146, 23);
            this.dateTimePicker1.TabIndex = 7;
            this.dateTimePicker1.Value = new System.DateTime(2017, 6, 1, 14, 42, 7, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(255, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 17);
            this.label2.TabIndex = 31;
            this.label2.Text = "～";
            // 
            // shapeContainer2
            // 
            this.shapeContainer2.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer2.Name = "shapeContainer2";
            this.shapeContainer2.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape5,
            this.lineShape4,
            this.lineShape3,
            this.lineShape2,
            this.lineShape1});
            this.shapeContainer2.Size = new System.Drawing.Size(712, 187);
            this.shapeContainer2.TabIndex = 0;
            this.shapeContainer2.TabStop = false;
            // 
            // lineShape5
            // 
            this.lineShape5.Name = "lineShape5";
            this.lineShape5.X1 = 8;
            this.lineShape5.X2 = 707;
            this.lineShape5.Y1 = 144;
            this.lineShape5.Y2 = 144;
            // 
            // lineShape4
            // 
            this.lineShape4.Name = "lineShape4";
            this.lineShape4.X1 = 7;
            this.lineShape4.X2 = 706;
            this.lineShape4.Y1 = 104;
            this.lineShape4.Y2 = 104;
            // 
            // lineShape3
            // 
            this.lineShape3.Name = "lineShape3";
            this.lineShape3.X1 = 706;
            this.lineShape3.X2 = 706;
            this.lineShape3.Y1 = 7;
            this.lineShape3.Y2 = 143;
            // 
            // lineShape2
            // 
            this.lineShape2.Name = "lineShape2";
            this.lineShape2.X1 = 6;
            this.lineShape2.X2 = 7;
            this.lineShape2.Y1 = 7;
            this.lineShape2.Y2 = 142;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 7;
            this.lineShape1.X2 = 706;
            this.lineShape1.Y1 = 7;
            this.lineShape1.Y2 = 7;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Size = new System.Drawing.Size(877, 152);
            this.shapeContainer1.TabIndex = 0;
            this.shapeContainer1.TabStop = false;
            // 
            // P03
            // 
            this.ClientSize = new System.Drawing.Size(712, 545);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.EditMode = true;
            this.Name = "P03";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P03. Clog Return Carton Input";
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel5, 0);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridReceiveDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel5;
        private Win.UI.Grid gridReceiveDate;
        private Win.UI.TextBox txtPONo;
        private Win.UI.Label labelSPNo;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.Label labelPONo;
        private Win.UI.Label labelPackID;
        private Win.UI.TextBox txtPackID;
        private Win.UI.Button btnFind;
        private Win.UI.Panel panel2;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnClose;
        private Win.UI.Panel panel1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Button btnImportFromBarcode;
        private Win.UI.Panel panel4;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Win.UI.Panel panel3;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape5;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape4;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape3;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape2;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private Win.UI.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private Win.UI.Label lbFactory;
        private Class.Txtfactory txtfactory;
        private Win.UI.NumericBox numTotalCTNQty;
        private Win.UI.NumericBox numSelectedCTNQty;
        private Win.UI.Label lbTotalCTNQty;
        private Win.UI.Label lbSelectedCTNQty;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Win.UI.CheckBox chkOnlyReqCarton;
        private Win.UI.DateRange dateReqDate;
        private Win.UI.Label label3;
    }
}
