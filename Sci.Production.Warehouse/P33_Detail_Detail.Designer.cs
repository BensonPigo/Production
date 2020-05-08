namespace Sci.Production.Warehouse
{
    partial class P33_Detail_Detail
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
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.numRequestVariance = new Sci.Win.UI.NumericBox();
            this.labelRequestVariance = new Sci.Win.UI.Label();
            this.displayTotalQty = new Sci.Win.UI.DisplayBox();
            this.labelTotalQty = new Sci.Win.UI.Label();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.groupBox3 = new Sci.Win.UI.GroupBox();
            this.numIssueQty = new Sci.Win.UI.NumericBox();
            this.numAccuIssue = new Sci.Win.UI.NumericBox();
            this.numRequestQty = new Sci.Win.UI.NumericBox();
            this.labelRequestQty = new Sci.Win.UI.Label();
            this.labelIssueQty = new Sci.Win.UI.Label();
            this.labelAccuIssue = new Sci.Win.UI.Label();
            this.groupBox4 = new Sci.Win.UI.GroupBox();
            this.editDesc = new Sci.Win.UI.EditBox();
            this.displayColorID = new Sci.Win.UI.DisplayBox();
            this.labelColorID = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelDesc = new Sci.Win.UI.Label();
            this.displaySPNo = new Sci.Win.UI.DisplayBox();
            this.labelRefno = new Sci.Win.UI.Label();
            this.displayRefno = new Sci.Win.UI.DisplayBox();
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridSeq = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.label1 = new Sci.Win.UI.Label();
            this.displaySCIRefno = new Sci.Win.UI.DisplayBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSeq)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(912, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(816, 16);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 30);
            this.btnImport.TabIndex = 1;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numRequestVariance);
            this.groupBox2.Controls.Add(this.labelRequestVariance);
            this.groupBox2.Controls.Add(this.displayTotalQty);
            this.groupBox2.Controls.Add(this.labelTotalQty);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 477);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1008, 53);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            // 
            // numRequestVariance
            // 
            this.numRequestVariance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numRequestVariance.DecimalPlaces = 2;
            this.numRequestVariance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numRequestVariance.IsSupportEditMode = false;
            this.numRequestVariance.Location = new System.Drawing.Point(311, 21);
            this.numRequestVariance.Name = "numRequestVariance";
            this.numRequestVariance.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numRequestVariance.ReadOnly = true;
            this.numRequestVariance.Size = new System.Drawing.Size(111, 23);
            this.numRequestVariance.TabIndex = 115;
            this.numRequestVariance.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelRequestVariance
            // 
            this.labelRequestVariance.Location = new System.Drawing.Point(181, 21);
            this.labelRequestVariance.Name = "labelRequestVariance";
            this.labelRequestVariance.Size = new System.Drawing.Size(127, 23);
            this.labelRequestVariance.TabIndex = 114;
            this.labelRequestVariance.Text = "Request Variance";
            // 
            // displayTotalQty
            // 
            this.displayTotalQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayTotalQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayTotalQty.Location = new System.Drawing.Point(537, 22);
            this.displayTotalQty.Name = "displayTotalQty";
            this.displayTotalQty.Size = new System.Drawing.Size(100, 23);
            this.displayTotalQty.TabIndex = 4;
            // 
            // labelTotalQty
            // 
            this.labelTotalQty.Location = new System.Drawing.Point(439, 22);
            this.labelTotalQty.Name = "labelTotalQty";
            this.labelTotalQty.Size = new System.Drawing.Size(95, 23);
            this.labelTotalQty.TabIndex = 3;
            this.labelTotalQty.Text = "Total Qty";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1008, 202);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.numIssueQty);
            this.groupBox3.Controls.Add(this.numAccuIssue);
            this.groupBox3.Controls.Add(this.numRequestQty);
            this.groupBox3.Controls.Add(this.labelRequestQty);
            this.groupBox3.Controls.Add(this.labelIssueQty);
            this.groupBox3.Controls.Add(this.labelAccuIssue);
            this.groupBox3.Location = new System.Drawing.Point(11, 138);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(984, 68);
            this.groupBox3.TabIndex = 113;
            this.groupBox3.TabStop = false;
            // 
            // numIssueQty
            // 
            this.numIssueQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numIssueQty.DecimalPlaces = 2;
            this.numIssueQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numIssueQty.IsSupportEditMode = false;
            this.numIssueQty.Location = new System.Drawing.Point(569, 29);
            this.numIssueQty.Name = "numIssueQty";
            this.numIssueQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numIssueQty.ReadOnly = true;
            this.numIssueQty.Size = new System.Drawing.Size(111, 23);
            this.numIssueQty.TabIndex = 112;
            this.numIssueQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numAccuIssue
            // 
            this.numAccuIssue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numAccuIssue.DecimalPlaces = 2;
            this.numAccuIssue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numAccuIssue.IsSupportEditMode = false;
            this.numAccuIssue.Location = new System.Drawing.Point(107, 29);
            this.numAccuIssue.Name = "numAccuIssue";
            this.numAccuIssue.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numAccuIssue.ReadOnly = true;
            this.numAccuIssue.Size = new System.Drawing.Size(111, 23);
            this.numAccuIssue.TabIndex = 110;
            this.numAccuIssue.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numRequestQty
            // 
            this.numRequestQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numRequestQty.DecimalPlaces = 2;
            this.numRequestQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numRequestQty.IsSupportEditMode = false;
            this.numRequestQty.Location = new System.Drawing.Point(353, 29);
            this.numRequestQty.Name = "numRequestQty";
            this.numRequestQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numRequestQty.ReadOnly = true;
            this.numRequestQty.Size = new System.Drawing.Size(111, 23);
            this.numRequestQty.TabIndex = 109;
            this.numRequestQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelRequestQty
            // 
            this.labelRequestQty.Location = new System.Drawing.Point(241, 29);
            this.labelRequestQty.Name = "labelRequestQty";
            this.labelRequestQty.Size = new System.Drawing.Size(109, 23);
            this.labelRequestQty.TabIndex = 99;
            this.labelRequestQty.Text = "Issue By Output";
            // 
            // labelIssueQty
            // 
            this.labelIssueQty.Location = new System.Drawing.Point(476, 29);
            this.labelIssueQty.Name = "labelIssueQty";
            this.labelIssueQty.Size = new System.Drawing.Size(90, 23);
            this.labelIssueQty.TabIndex = 105;
            this.labelIssueQty.Text = "Issue Qty";
            // 
            // labelAccuIssue
            // 
            this.labelAccuIssue.Location = new System.Drawing.Point(14, 29);
            this.labelAccuIssue.Name = "labelAccuIssue";
            this.labelAccuIssue.Size = new System.Drawing.Size(90, 23);
            this.labelAccuIssue.TabIndex = 100;
            this.labelAccuIssue.Text = "Accu. Issued";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.displaySCIRefno);
            this.groupBox4.Controls.Add(this.editDesc);
            this.groupBox4.Controls.Add(this.displayColorID);
            this.groupBox4.Controls.Add(this.labelColorID);
            this.groupBox4.Controls.Add(this.labelSPNo);
            this.groupBox4.Controls.Add(this.labelDesc);
            this.groupBox4.Controls.Add(this.displaySPNo);
            this.groupBox4.Controls.Add(this.labelRefno);
            this.groupBox4.Controls.Add(this.displayRefno);
            this.groupBox4.Location = new System.Drawing.Point(13, -3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(984, 146);
            this.groupBox4.TabIndex = 112;
            this.groupBox4.TabStop = false;
            // 
            // editDesc
            // 
            this.editDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editDesc.IsSupportEditMode = false;
            this.editDesc.Location = new System.Drawing.Point(90, 48);
            this.editDesc.Multiline = true;
            this.editDesc.Name = "editDesc";
            this.editDesc.ReadOnly = true;
            this.editDesc.Size = new System.Drawing.Size(864, 94);
            this.editDesc.TabIndex = 109;
            // 
            // displayColorID
            // 
            this.displayColorID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayColorID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayColorID.Location = new System.Drawing.Point(830, 19);
            this.displayColorID.Name = "displayColorID";
            this.displayColorID.Size = new System.Drawing.Size(124, 23);
            this.displayColorID.TabIndex = 106;
            // 
            // labelColorID
            // 
            this.labelColorID.Location = new System.Drawing.Point(749, 19);
            this.labelColorID.Name = "labelColorID";
            this.labelColorID.Size = new System.Drawing.Size(75, 23);
            this.labelColorID.TabIndex = 105;
            this.labelColorID.Text = "Color";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(12, 19);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(75, 23);
            this.labelSPNo.TabIndex = 100;
            this.labelSPNo.Text = "PO#";
            // 
            // labelDesc
            // 
            this.labelDesc.Location = new System.Drawing.Point(12, 52);
            this.labelDesc.Name = "labelDesc";
            this.labelDesc.Size = new System.Drawing.Size(75, 23);
            this.labelDesc.TabIndex = 107;
            this.labelDesc.Text = "Desc";
            // 
            // displaySPNo
            // 
            this.displaySPNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySPNo.Location = new System.Drawing.Point(90, 19);
            this.displaySPNo.Name = "displaySPNo";
            this.displaySPNo.Size = new System.Drawing.Size(124, 23);
            this.displaySPNo.TabIndex = 102;
            // 
            // labelRefno
            // 
            this.labelRefno.Location = new System.Drawing.Point(521, 19);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.Size = new System.Drawing.Size(75, 23);
            this.labelRefno.TabIndex = 103;
            this.labelRefno.Text = "Refno";
            // 
            // displayRefno
            // 
            this.displayRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayRefno.Location = new System.Drawing.Point(599, 19);
            this.displayRefno.Name = "displayRefno";
            this.displayRefno.Size = new System.Drawing.Size(124, 23);
            this.displayRefno.TabIndex = 104;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridSeq);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 202);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 275);
            this.panel1.TabIndex = 20;
            // 
            // gridSeq
            // 
            this.gridSeq.AllowUserToAddRows = false;
            this.gridSeq.AllowUserToDeleteRows = false;
            this.gridSeq.AllowUserToResizeRows = false;
            this.gridSeq.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSeq.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridSeq.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSeq.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSeq.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSeq.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSeq.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSeq.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSeq.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSeq.Location = new System.Drawing.Point(0, 0);
            this.gridSeq.Name = "gridSeq";
            this.gridSeq.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSeq.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSeq.RowTemplate.Height = 24;
            this.gridSeq.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSeq.ShowCellToolTips = false;
            this.gridSeq.Size = new System.Drawing.Size(1008, 275);
            this.gridSeq.TabIndex = 0;
            this.gridSeq.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(239, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 112;
            this.label1.Text = "SCIRefno";
            // 
            // displaySCIRefno
            // 
            this.displaySCIRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySCIRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySCIRefno.Location = new System.Drawing.Point(317, 19);
            this.displaySCIRefno.Name = "displaySCIRefno";
            this.displaySCIRefno.Size = new System.Drawing.Size(183, 23);
            this.displaySCIRefno.TabIndex = 113;
            // 
            // P33_Detail_Detail
            // 
            this.ClientSize = new System.Drawing.Size(1008, 530);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "P33_Detail_Detail";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P33. Append Thread";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSeq)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnCancel;
        private Win.UI.Button btnImport;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Panel panel1;
        private Win.UI.Grid gridSeq;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.DisplayBox displayTotalQty;
        private Win.UI.Label labelTotalQty;
        private Win.UI.NumericBox numRequestVariance;
        private Win.UI.Label labelRequestVariance;
        private Win.UI.GroupBox groupBox3;
        private Win.UI.NumericBox numIssueQty;
        private Win.UI.NumericBox numAccuIssue;
        private Win.UI.NumericBox numRequestQty;
        private Win.UI.Label labelRequestQty;
        private Win.UI.Label labelIssueQty;
        private Win.UI.Label labelAccuIssue;
        private Win.UI.GroupBox groupBox4;
        private Win.UI.EditBox editDesc;
        private Win.UI.DisplayBox displayColorID;
        private Win.UI.Label labelColorID;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelDesc;
        private Win.UI.DisplayBox displaySPNo;
        private Win.UI.Label labelRefno;
        private Win.UI.DisplayBox displayRefno;
        private Win.UI.Label label1;
        private Win.UI.DisplayBox displaySCIRefno;
    }
}
