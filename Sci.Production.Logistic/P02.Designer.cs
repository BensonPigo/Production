namespace Sci.Production.Logistic
{
    partial class P02
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
            this.gridImport = new Sci.Win.UI.Grid();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel1 = new Sci.Win.UI.Panel();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel4 = new Sci.Win.UI.Panel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.btnImportFromBarcode = new Sci.Win.UI.Button();
            this.btnFind = new Sci.Win.UI.Button();
            this.txtPackID = new Sci.Win.UI.TextBox();
            this.labelPackID = new Sci.Win.UI.Label();
            this.labelPONo = new Sci.Win.UI.Label();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.txtPONo = new Sci.Win.UI.TextBox();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.labelLocationNo = new Sci.Win.UI.Label();
            this.btnUpdateAllLocation = new Sci.Win.UI.Button();
            this.txtcloglocationLocationNo = new Sci.Production.Class.Txtcloglocation();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.lbFactory = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.label3 = new Sci.Win.UI.Label();
            this.txtTransferSlipNo = new Sci.Win.UI.TextBox();
            this.panel3 = new Sci.Win.UI.Panel();
            this.numTotalCTNQty = new Sci.Win.UI.NumericBox();
            this.numSelectedCTNQty = new Sci.Win.UI.NumericBox();
            this.lbTotalCTNQty = new Sci.Win.UI.Label();
            this.lbSelectedCTNQty = new Sci.Win.UI.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridImport);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 152);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(857, 350);
            this.panel5.TabIndex = 21;
            // 
            // gridImport
            // 
            this.gridImport.AllowUserToAddRows = false;
            this.gridImport.AllowUserToDeleteRows = false;
            this.gridImport.AllowUserToResizeRows = false;
            this.gridImport.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridImport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridImport.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridImport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridImport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridImport.Location = new System.Drawing.Point(0, 0);
            this.gridImport.Name = "gridImport";
            this.gridImport.RowHeadersVisible = false;
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridImport.RowTemplate.Height = 24;
            this.gridImport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridImport.ShowCellToolTips = false;
            this.gridImport.Size = new System.Drawing.Size(857, 350);
            this.gridImport.TabIndex = 10;
            this.gridImport.TabStop = false;
            this.gridImport.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridImport_ColumnHeaderMouseClick);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(867, 152);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 350);
            this.panel2.TabIndex = 18;
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 152);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 350);
            this.panel1.TabIndex = 17;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 502);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(877, 10);
            this.panel4.TabIndex = 20;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // shapeContainer2
            // 
            this.shapeContainer2.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer2.Name = "shapeContainer2";
            this.shapeContainer2.Size = new System.Drawing.Size(877, 152);
            this.shapeContainer2.TabIndex = 0;
            this.shapeContainer2.TabStop = false;
            // 
            // btnImportFromBarcode
            // 
            this.btnImportFromBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportFromBarcode.Location = new System.Drawing.Point(677, 3);
            this.btnImportFromBarcode.Name = "btnImportFromBarcode";
            this.btnImportFromBarcode.Size = new System.Drawing.Size(176, 30);
            this.btnImportFromBarcode.TabIndex = 7;
            this.btnImportFromBarcode.Text = "Import From Barcode";
            this.btnImportFromBarcode.UseVisualStyleBackColor = true;
            this.btnImportFromBarcode.Click += new System.EventHandler(this.ButtonImport_Click);
            // 
            // btnFind
            // 
            this.btnFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFind.Location = new System.Drawing.Point(773, 2);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 30);
            this.btnFind.TabIndex = 6;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.ButtonFind_Click);
            // 
            // txtPackID
            // 
            this.txtPackID.BackColor = System.Drawing.Color.White;
            this.txtPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackID.Location = new System.Drawing.Point(411, 5);
            this.txtPackID.Name = "txtPackID";
            this.txtPackID.Size = new System.Drawing.Size(120, 23);
            this.txtPackID.TabIndex = 5;
            // 
            // labelPackID
            // 
            this.labelPackID.Location = new System.Drawing.Point(355, 5);
            this.labelPackID.Name = "labelPackID";
            this.labelPackID.Size = new System.Drawing.Size(52, 23);
            this.labelPackID.TabIndex = 4;
            this.labelPackID.Text = "PackID";
            // 
            // labelPONo
            // 
            this.labelPONo.Location = new System.Drawing.Point(155, 5);
            this.labelPONo.Name = "labelPONo";
            this.labelPONo.Size = new System.Drawing.Size(40, 23);
            this.labelPONo.TabIndex = 2;
            this.labelPONo.Text = "PO#";
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(52, 5);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(100, 23);
            this.txtSPNo.TabIndex = 1;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(8, 5);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(40, 23);
            this.labelSPNo.TabIndex = 0;
            this.labelSPNo.Text = "SP#";
            // 
            // txtPONo
            // 
            this.txtPONo.BackColor = System.Drawing.Color.White;
            this.txtPONo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPONo.Location = new System.Drawing.Point(199, 5);
            this.txtPONo.Name = "txtPONo";
            this.txtPONo.Size = new System.Drawing.Size(153, 23);
            this.txtPONo.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(777, 117);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSave.Location = new System.Drawing.Point(691, 117);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // labelLocationNo
            // 
            this.labelLocationNo.Location = new System.Drawing.Point(12, 121);
            this.labelLocationNo.Name = "labelLocationNo";
            this.labelLocationNo.Size = new System.Drawing.Size(81, 23);
            this.labelLocationNo.TabIndex = 13;
            this.labelLocationNo.Text = "Location No";
            // 
            // btnUpdateAllLocation
            // 
            this.btnUpdateAllLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnUpdateAllLocation.Location = new System.Drawing.Point(193, 117);
            this.btnUpdateAllLocation.Name = "btnUpdateAllLocation";
            this.btnUpdateAllLocation.Size = new System.Drawing.Size(163, 30);
            this.btnUpdateAllLocation.TabIndex = 15;
            this.btnUpdateAllLocation.Text = "Update All Location";
            this.btnUpdateAllLocation.UseVisualStyleBackColor = true;
            this.btnUpdateAllLocation.Click += new System.EventHandler(this.ButtonUpdate_Click);
            // 
            // txtcloglocationLocationNo
            // 
            this.txtcloglocationLocationNo.BackColor = System.Drawing.Color.White;
            this.txtcloglocationLocationNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcloglocationLocationNo.IsSupportSytsemContextMenu = false;
            this.txtcloglocationLocationNo.Location = new System.Drawing.Point(97, 121);
            this.txtcloglocationLocationNo.MDivisionObjectName = null;
            this.txtcloglocationLocationNo.Name = "txtcloglocationLocationNo";
            this.txtcloglocationLocationNo.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.NonReadOnly;
            this.txtcloglocationLocationNo.Size = new System.Drawing.Size(80, 23);
            this.txtcloglocationLocationNo.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 23);
            this.label1.TabIndex = 18;
            this.label1.Text = "Transfer Date";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(251, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 17);
            this.label2.TabIndex = 27;
            this.label2.Text = "～";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(103, 34);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(146, 23);
            this.dateTimePicker1.TabIndex = 28;
            this.dateTimePicker1.Value = new System.DateTime(2017, 6, 1, 14, 42, 7, 0);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(275, 35);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(146, 23);
            this.dateTimePicker2.TabIndex = 29;
            this.dateTimePicker2.Value = new System.DateTime(2017, 6, 1, 14, 42, 7, 0);
            // 
            // lbFactory
            // 
            this.lbFactory.Location = new System.Drawing.Point(429, 34);
            this.lbFactory.Name = "lbFactory";
            this.lbFactory.Size = new System.Drawing.Size(52, 23);
            this.lbFactory.TabIndex = 35;
            this.lbFactory.Text = "Factory";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(484, 34);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 36;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(534, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 38;
            this.label3.Text = "TransferSlipNo";
            // 
            // txtTransferSlipNo
            // 
            this.txtTransferSlipNo.BackColor = System.Drawing.Color.White;
            this.txtTransferSlipNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTransferSlipNo.Location = new System.Drawing.Point(637, 6);
            this.txtTransferSlipNo.Name = "txtTransferSlipNo";
            this.txtTransferSlipNo.Size = new System.Drawing.Size(130, 23);
            this.txtTransferSlipNo.TabIndex = 37;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.numTotalCTNQty);
            this.panel3.Controls.Add(this.numSelectedCTNQty);
            this.panel3.Controls.Add(this.lbTotalCTNQty);
            this.panel3.Controls.Add(this.lbSelectedCTNQty);
            this.panel3.Controls.Add(this.panel7);
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.txtcloglocationLocationNo);
            this.panel3.Controls.Add(this.btnUpdateAllLocation);
            this.panel3.Controls.Add(this.labelLocationNo);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Controls.Add(this.shapeContainer2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(877, 152);
            this.panel3.TabIndex = 19;
            // 
            // numTotalCTNQty
            // 
            this.numTotalCTNQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numTotalCTNQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numTotalCTNQty.IsSupportEditMode = false;
            this.numTotalCTNQty.Location = new System.Drawing.Point(641, 121);
            this.numTotalCTNQty.Name = "numTotalCTNQty";
            this.numTotalCTNQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTotalCTNQty.ReadOnly = true;
            this.numTotalCTNQty.Size = new System.Drawing.Size(44, 23);
            this.numTotalCTNQty.TabIndex = 45;
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
            this.numSelectedCTNQty.Location = new System.Drawing.Point(482, 121);
            this.numSelectedCTNQty.Name = "numSelectedCTNQty";
            this.numSelectedCTNQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSelectedCTNQty.ReadOnly = true;
            this.numSelectedCTNQty.Size = new System.Drawing.Size(41, 23);
            this.numSelectedCTNQty.TabIndex = 44;
            this.numSelectedCTNQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // lbTotalCTNQty
            // 
            this.lbTotalCTNQty.Location = new System.Drawing.Point(538, 121);
            this.lbTotalCTNQty.Name = "lbTotalCTNQty";
            this.lbTotalCTNQty.Size = new System.Drawing.Size(100, 23);
            this.lbTotalCTNQty.TabIndex = 43;
            this.lbTotalCTNQty.Text = "Total CTN Qty:";
            // 
            // lbSelectedCTNQty
            // 
            this.lbSelectedCTNQty.Location = new System.Drawing.Point(359, 121);
            this.lbSelectedCTNQty.Name = "lbSelectedCTNQty";
            this.lbSelectedCTNQty.Size = new System.Drawing.Size(120, 23);
            this.lbSelectedCTNQty.TabIndex = 41;
            this.lbSelectedCTNQty.Text = "Selected CTN Qty:";
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.btnImportFromBarcode);
            this.panel7.Location = new System.Drawing.Point(3, 65);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(864, 42);
            this.panel7.TabIndex = 40;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.labelSPNo);
            this.panel6.Controls.Add(this.txtTransferSlipNo);
            this.panel6.Controls.Add(this.btnFind);
            this.panel6.Controls.Add(this.label3);
            this.panel6.Controls.Add(this.txtPackID);
            this.panel6.Controls.Add(this.txtfactory);
            this.panel6.Controls.Add(this.labelPackID);
            this.panel6.Controls.Add(this.lbFactory);
            this.panel6.Controls.Add(this.labelPONo);
            this.panel6.Controls.Add(this.dateTimePicker2);
            this.panel6.Controls.Add(this.txtSPNo);
            this.panel6.Controls.Add(this.dateTimePicker1);
            this.panel6.Controls.Add(this.txtPONo);
            this.panel6.Controls.Add(this.label2);
            this.panel6.Controls.Add(this.label1);
            this.panel6.Location = new System.Drawing.Point(3, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(864, 63);
            this.panel6.TabIndex = 39;
            // 
            // P02
            // 
            this.ClientSize = new System.Drawing.Size(877, 512);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.EditMode = true;
            this.Name = "P02";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P02. Clog Receive Carton Input";
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel5, 0);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel5;
        private Win.UI.Grid gridImport;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel4;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
        //private Microsoft.VisualBasic.PowerPacks.LineShape lineShape5;
        //private Microsoft.VisualBasic.PowerPacks.LineShape lineShape4;
        //private Microsoft.VisualBasic.PowerPacks.LineShape lineShape3;
        //private Microsoft.VisualBasic.PowerPacks.LineShape lineShape2;
        //private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private Win.UI.Button btnImportFromBarcode;
        private Win.UI.Button btnFind;
        private Win.UI.TextBox txtPackID;
        private Win.UI.Label labelPackID;
        private Win.UI.Label labelPONo;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.Label labelSPNo;
        private Win.UI.TextBox txtPONo;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnSave;
        private Win.UI.Label labelLocationNo;
        private Win.UI.Button btnUpdateAllLocation;
        private Class.Txtcloglocation txtcloglocationLocationNo;
        private Win.UI.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private Win.UI.Label lbFactory;
        private Class.Txtfactory txtfactory;
        private Win.UI.Label label3;
        private Win.UI.TextBox txtTransferSlipNo;
        private Win.UI.Panel panel3;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private Win.UI.NumericBox numTotalCTNQty;
        private Win.UI.NumericBox numSelectedCTNQty;
        private Win.UI.Label lbTotalCTNQty;
        private Win.UI.Label lbSelectedCTNQty;
    }
}
