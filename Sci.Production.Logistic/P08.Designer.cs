namespace Sci.Production.Logistic
{
    partial class P08
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel3 = new Sci.Win.UI.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnImportFromBarcode = new Sci.Win.UI.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.dateSciDelivery = new Sci.Win.UI.DateRange();
            this.dateReceiveDate = new Sci.Win.UI.DateRange();
            this.labelReceiveDate = new Sci.Win.UI.Label();
            this.labSCIDelivery = new Sci.Win.UI.Label();
            this.txtPO = new Sci.Win.UI.TextBox();
            this.labelSP = new Sci.Win.UI.Label();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.labelPO = new Sci.Win.UI.Label();
            this.labelPackID = new Sci.Win.UI.Label();
            this.txtPackID = new Sci.Win.UI.TextBox();
            this.BtnFind = new Sci.Win.UI.Button();
            this.chkCFA = new Sci.Win.UI.CheckBox();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel3.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel7);
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.chkCFA);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(873, 163);
            this.panel3.TabIndex = 20;
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.btnImportFromBarcode);
            this.panel7.Location = new System.Drawing.Point(5, 78);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(863, 42);
            this.panel7.TabIndex = 109;
            // 
            // btnImportFromBarcode
            // 
            this.btnImportFromBarcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportFromBarcode.Location = new System.Drawing.Point(614, 3);
            this.btnImportFromBarcode.Name = "btnImportFromBarcode";
            this.btnImportFromBarcode.Size = new System.Drawing.Size(236, 30);
            this.btnImportFromBarcode.TabIndex = 7;
            this.btnImportFromBarcode.Text = "Import From Barcode";
            this.btnImportFromBarcode.UseVisualStyleBackColor = true;
            this.btnImportFromBarcode.Click += new System.EventHandler(this.BtnImportFromBarcode_Click);
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.dateSciDelivery);
            this.panel6.Controls.Add(this.dateReceiveDate);
            this.panel6.Controls.Add(this.labelReceiveDate);
            this.panel6.Controls.Add(this.labSCIDelivery);
            this.panel6.Controls.Add(this.txtPO);
            this.panel6.Controls.Add(this.labelSP);
            this.panel6.Controls.Add(this.txtSP);
            this.panel6.Controls.Add(this.labelPO);
            this.panel6.Controls.Add(this.labelPackID);
            this.panel6.Controls.Add(this.txtPackID);
            this.panel6.Controls.Add(this.BtnFind);
            this.panel6.Location = new System.Drawing.Point(5, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(863, 77);
            this.panel6.TabIndex = 108;
            // 
            // dateSciDelivery
            // 
            // 
            // 
            // 
            this.dateSciDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSciDelivery.DateBox1.Name = "";
            this.dateSciDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSciDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSciDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSciDelivery.DateBox2.Name = "";
            this.dateSciDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSciDelivery.DateBox2.TabIndex = 1;
            this.dateSciDelivery.IsRequired = false;
            this.dateSciDelivery.IsSupportEditMode = false;
            this.dateSciDelivery.Location = new System.Drawing.Point(100, 45);
            this.dateSciDelivery.Name = "dateSciDelivery";
            this.dateSciDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSciDelivery.TabIndex = 117;
            // 
            // dateReceiveDate
            // 
            // 
            // 
            // 
            this.dateReceiveDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateReceiveDate.DateBox1.Name = "";
            this.dateReceiveDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateReceiveDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateReceiveDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateReceiveDate.DateBox2.Name = "";
            this.dateReceiveDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateReceiveDate.DateBox2.TabIndex = 1;
            this.dateReceiveDate.IsRequired = false;
            this.dateReceiveDate.IsSupportEditMode = false;
            this.dateReceiveDate.Location = new System.Drawing.Point(484, 45);
            this.dateReceiveDate.Name = "dateReceiveDate";
            this.dateReceiveDate.Size = new System.Drawing.Size(280, 23);
            this.dateReceiveDate.TabIndex = 116;
            // 
            // labelReceiveDate
            // 
            this.labelReceiveDate.Location = new System.Drawing.Point(383, 45);
            this.labelReceiveDate.Name = "labelReceiveDate";
            this.labelReceiveDate.Size = new System.Drawing.Size(98, 23);
            this.labelReceiveDate.TabIndex = 115;
            this.labelReceiveDate.Text = "Receive Date";
            // 
            // labSCIDelivery
            // 
            this.labSCIDelivery.Location = new System.Drawing.Point(12, 45);
            this.labSCIDelivery.Name = "labSCIDelivery";
            this.labSCIDelivery.Size = new System.Drawing.Size(85, 23);
            this.labSCIDelivery.TabIndex = 114;
            this.labSCIDelivery.Text = "SCI Delivery";
            // 
            // txtPO
            // 
            this.txtPO.BackColor = System.Drawing.Color.White;
            this.txtPO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPO.IsSupportEditMode = false;
            this.txtPO.Location = new System.Drawing.Point(310, 9);
            this.txtPO.Name = "txtPO";
            this.txtPO.Size = new System.Drawing.Size(171, 23);
            this.txtPO.TabIndex = 110;
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(12, 9);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(85, 23);
            this.labelSP.TabIndex = 107;
            this.labelSP.Text = "SP#";
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.IsSupportEditMode = false;
            this.txtSP.Location = new System.Drawing.Point(100, 9);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(129, 23);
            this.txtSP.TabIndex = 108;
            // 
            // labelPO
            // 
            this.labelPO.Location = new System.Drawing.Point(250, 9);
            this.labelPO.Name = "labelPO";
            this.labelPO.Size = new System.Drawing.Size(57, 23);
            this.labelPO.TabIndex = 109;
            this.labelPO.Text = "PO#";
            // 
            // labelPackID
            // 
            this.labelPackID.Location = new System.Drawing.Point(528, 9);
            this.labelPackID.Name = "labelPackID";
            this.labelPackID.Size = new System.Drawing.Size(83, 23);
            this.labelPackID.TabIndex = 111;
            this.labelPackID.Text = "PackID";
            // 
            // txtPackID
            // 
            this.txtPackID.BackColor = System.Drawing.Color.White;
            this.txtPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackID.IsSupportEditMode = false;
            this.txtPackID.Location = new System.Drawing.Point(614, 9);
            this.txtPackID.Name = "txtPackID";
            this.txtPackID.Size = new System.Drawing.Size(150, 23);
            this.txtPackID.TabIndex = 112;
            // 
            // BtnFind
            // 
            this.BtnFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.BtnFind.Location = new System.Drawing.Point(770, 6);
            this.BtnFind.Name = "BtnFind";
            this.BtnFind.Size = new System.Drawing.Size(80, 30);
            this.BtnFind.TabIndex = 113;
            this.BtnFind.Text = "Find";
            this.BtnFind.UseVisualStyleBackColor = true;
            this.BtnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // chkCFA
            // 
            this.chkCFA.AutoSize = true;
            this.chkCFA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkCFA.IsSupportEditMode = false;
            this.chkCFA.Location = new System.Drawing.Point(18, 132);
            this.chkCFA.Name = "chkCFA";
            this.chkCFA.Size = new System.Drawing.Size(229, 21);
            this.chkCFA.TabIndex = 107;
            this.chkCFA.Text = "Only Show CFA Selected Carton";
            this.chkCFA.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSave.Location = new System.Drawing.Point(690, 126);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(776, 126);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
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
            this.gridDetail.Location = new System.Drawing.Point(0, 163);
            this.gridDetail.Name = "gridDetail";
            this.gridDetail.RowHeadersVisible = false;
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.ShowCellToolTips = false;
            this.gridDetail.Size = new System.Drawing.Size(873, 275);
            this.gridDetail.TabIndex = 21;
            this.gridDetail.TabStop = false;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(9, 48);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(97, 23);
            this.labelSCIDelivery.TabIndex = 100;
            this.labelSCIDelivery.Text = "SCI Delivery";
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer2";
            this.shapeContainer1.Size = new System.Drawing.Size(862, 123);
            this.shapeContainer1.TabIndex = 0;
            this.shapeContainer1.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // P08
            // 
            this.ClientSize = new System.Drawing.Size(873, 438);
            this.Controls.Add(this.gridDetail);
            this.Controls.Add(this.panel3);
            this.Name = "P08";
            this.Text = "P08.Carton Transfer to CFA Input";
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.gridDetail, 0);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel3;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnImportFromBarcode;
        private Win.UI.Grid gridDetail;
        private Win.UI.ListControlBindingSource listControlBindingSource1;        
        private Win.UI.Label labelSCIDelivery;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Win.UI.CheckBox chkCFA;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel6;
        private Win.UI.DateRange dateSciDelivery;
        private Win.UI.DateRange dateReceiveDate;
        private Win.UI.Label labelReceiveDate;
        private Win.UI.Label labSCIDelivery;
        private Win.UI.TextBox txtPO;
        private Win.UI.Label labelSP;
        private Win.UI.TextBox txtSP;
        private Win.UI.Label labelPO;
        private Win.UI.Label labelPackID;
        private Win.UI.TextBox txtPackID;
        private Win.UI.Button BtnFind;
        private System.Windows.Forms.Panel panel7;
    }
}
