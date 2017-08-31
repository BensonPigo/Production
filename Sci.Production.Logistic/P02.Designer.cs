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
            this.txtfactory = new Sci.Production.Class.txtfactory();
            this.lbFactory = new Sci.Win.UI.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.txtcloglocationLocationNo = new Sci.Production.Class.txtcloglocation();
            this.btnUpdateAllLocation = new Sci.Win.UI.Button();
            this.labelLocationNo = new Sci.Win.UI.Label();
            this.shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape5 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape4 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape3 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape2 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.txtTransferSlipNo = new Sci.Win.UI.TextBox();
            this.label3 = new Sci.Win.UI.Label();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridImport);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 152);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(849, 350);
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
            this.gridImport.Size = new System.Drawing.Size(849, 350);
            this.gridImport.TabIndex = 10;
            this.gridImport.TabStop = false;
            // 
            // txtPONo
            // 
            this.txtPONo.BackColor = System.Drawing.Color.White;
            this.txtPONo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPONo.Location = new System.Drawing.Point(203, 15);
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
            this.labelPONo.Location = new System.Drawing.Point(159, 15);
            this.labelPONo.Name = "labelPONo";
            this.labelPONo.Size = new System.Drawing.Size(40, 23);
            this.labelPONo.TabIndex = 2;
            this.labelPONo.Text = "PO#";
            // 
            // labelPackID
            // 
            this.labelPackID.Location = new System.Drawing.Point(359, 15);
            this.labelPackID.Name = "labelPackID";
            this.labelPackID.Size = new System.Drawing.Size(52, 23);
            this.labelPackID.TabIndex = 4;
            this.labelPackID.Text = "PackID";
            // 
            // txtPackID
            // 
            this.txtPackID.BackColor = System.Drawing.Color.White;
            this.txtPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackID.Location = new System.Drawing.Point(415, 15);
            this.txtPackID.Name = "txtPackID";
            this.txtPackID.Size = new System.Drawing.Size(120, 23);
            this.txtPackID.TabIndex = 5;
            // 
            // btnFind
            // 
            this.btnFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFind.Location = new System.Drawing.Point(777, 12);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 30);
            this.btnFind.TabIndex = 6;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.buttonFind_Click);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(859, 152);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 350);
            this.panel2.TabIndex = 18;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSave.Location = new System.Drawing.Point(693, 117);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(779, 117);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 152);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 350);
            this.panel1.TabIndex = 17;
            // 
            // btnImportFromBarcode
            // 
            this.btnImportFromBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportFromBarcode.Location = new System.Drawing.Point(681, 78);
            this.btnImportFromBarcode.Name = "btnImportFromBarcode";
            this.btnImportFromBarcode.Size = new System.Drawing.Size(176, 30);
            this.btnImportFromBarcode.TabIndex = 7;
            this.btnImportFromBarcode.Text = "Import From Barcode";
            this.btnImportFromBarcode.UseVisualStyleBackColor = true;
            this.btnImportFromBarcode.Click += new System.EventHandler(this.buttonImport_Click);
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 502);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(869, 10);
            this.panel4.TabIndex = 20;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtTransferSlipNo);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.txtfactory);
            this.panel3.Controls.Add(this.lbFactory);
            this.panel3.Controls.Add(this.dateTimePicker2);
            this.panel3.Controls.Add(this.dateTimePicker1);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.txtcloglocationLocationNo);
            this.panel3.Controls.Add(this.btnUpdateAllLocation);
            this.panel3.Controls.Add(this.labelLocationNo);
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
            this.panel3.Size = new System.Drawing.Size(869, 152);
            this.panel3.TabIndex = 19;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(488, 44);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 36;
            // 
            // lbFactory
            // 
            this.lbFactory.Location = new System.Drawing.Point(433, 44);
            this.lbFactory.Name = "lbFactory";
            this.lbFactory.Size = new System.Drawing.Size(52, 23);
            this.lbFactory.TabIndex = 35;
            this.lbFactory.Text = "Factory";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(279, 45);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(146, 23);
            this.dateTimePicker2.TabIndex = 29;
            this.dateTimePicker2.Value = new System.DateTime(2017, 6, 1, 14, 42, 7, 0);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(107, 44);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(146, 23);
            this.dateTimePicker1.TabIndex = 28;
            this.dateTimePicker1.Value = new System.DateTime(2017, 6, 1, 14, 42, 7, 0);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(255, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 17);
            this.label2.TabIndex = 27;
            this.label2.Text = "～";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 23);
            this.label1.TabIndex = 18;
            this.label1.Text = "Transfer Date";
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
            // btnUpdateAllLocation
            // 
            this.btnUpdateAllLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnUpdateAllLocation.Location = new System.Drawing.Point(193, 117);
            this.btnUpdateAllLocation.Name = "btnUpdateAllLocation";
            this.btnUpdateAllLocation.Size = new System.Drawing.Size(163, 30);
            this.btnUpdateAllLocation.TabIndex = 15;
            this.btnUpdateAllLocation.Text = "Update All Location";
            this.btnUpdateAllLocation.UseVisualStyleBackColor = true;
            this.btnUpdateAllLocation.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // labelLocationNo
            // 
            this.labelLocationNo.Location = new System.Drawing.Point(12, 121);
            this.labelLocationNo.Name = "labelLocationNo";
            this.labelLocationNo.Size = new System.Drawing.Size(81, 23);
            this.labelLocationNo.TabIndex = 13;
            this.labelLocationNo.Text = "Location No";
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
            this.shapeContainer2.Size = new System.Drawing.Size(819, 152);
            this.shapeContainer2.TabIndex = 0;
            this.shapeContainer2.TabStop = false;
            // 
            // lineShape5
            // 
            this.lineShape5.Name = "lineShape5";
            this.lineShape5.X1 = 7;
            this.lineShape5.X2 = 706;
            this.lineShape5.Y1 = 72;
            this.lineShape5.Y2 = 72;
            // 
            // lineShape4
            // 
            this.lineShape4.Name = "lineShape4";
            this.lineShape4.X1 = 7;
            this.lineShape4.X2 = 706;
            this.lineShape4.Y1 = 112;
            this.lineShape4.Y2 = 112;
            // 
            // lineShape3
            // 
            this.lineShape3.Name = "lineShape3";
            this.lineShape3.X1 = 706;
            this.lineShape3.X2 = 706;
            this.lineShape3.Y1 = 7;
            this.lineShape3.Y2 = 112;
            // 
            // lineShape2
            // 
            this.lineShape2.Name = "lineShape2";
            this.lineShape2.X1 = 6;
            this.lineShape2.X2 = 6;
            this.lineShape2.Y1 = 7;
            this.lineShape2.Y2 = 112;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 7;
            this.lineShape1.X2 = 706;
            this.lineShape1.Y1 = 7;
            this.lineShape1.Y2 = 7;
            // 
            // txtTransferSlipNo
            // 
            this.txtTransferSlipNo.BackColor = System.Drawing.Color.White;
            this.txtTransferSlipNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTransferSlipNo.Location = new System.Drawing.Point(641, 16);
            this.txtTransferSlipNo.Name = "txtTransferSlipNo";
            this.txtTransferSlipNo.Size = new System.Drawing.Size(130, 23);
            this.txtTransferSlipNo.TabIndex = 37;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(538, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 38;
            this.label3.Text = "TransferSlipNo";
            // 
            // P02
            // 
            this.ClientSize = new System.Drawing.Size(869, 512);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.EditMode = true;
            this.Name = "P02";
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
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel5;
        private Win.UI.Grid gridImport;
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
        private Win.UI.Button btnUpdateAllLocation;
        private Win.UI.Label labelLocationNo;
        private Class.txtcloglocation txtcloglocationLocationNo;
        private Win.UI.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private Class.txtfactory txtfactory;
        private Win.UI.Label lbFactory;
        private Win.UI.TextBox txtTransferSlipNo;
        private Win.UI.Label label3;
    }
}
