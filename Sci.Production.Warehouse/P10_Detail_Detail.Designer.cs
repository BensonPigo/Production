namespace Sci.Production.Warehouse
{
    partial class P10_Detail_Detail
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
            this.displaySizeSpec = new Sci.Win.UI.DisplayBox();
            this.labelSizeSpec = new Sci.Win.UI.Label();
            this.displayColorID = new Sci.Win.UI.DisplayBox();
            this.displayDesc = new Sci.Win.UI.DisplayBox();
            this.label4 = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelDesc = new Sci.Win.UI.Label();
            this.displaySPNo = new Sci.Win.UI.DisplayBox();
            this.labelRefno = new Sci.Win.UI.Label();
            this.displayRefno = new Sci.Win.UI.DisplayBox();
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridRollNo = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.label1 = new Sci.Win.UI.Label();
            this.displaySCIRefno = new Sci.Win.UI.DisplayBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridRollNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(897, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(801, 16);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 30);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
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
            this.groupBox2.Size = new System.Drawing.Size(993, 53);
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
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.displaySCIRefno);
            this.groupBox1.Controls.Add(this.displaySizeSpec);
            this.groupBox1.Controls.Add(this.labelSizeSpec);
            this.groupBox1.Controls.Add(this.displayColorID);
            this.groupBox1.Controls.Add(this.displayDesc);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.labelSPNo);
            this.groupBox1.Controls.Add(this.labelDesc);
            this.groupBox1.Controls.Add(this.displaySPNo);
            this.groupBox1.Controls.Add(this.labelRefno);
            this.groupBox1.Controls.Add(this.displayRefno);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(993, 91);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // displaySizeSpec
            // 
            this.displaySizeSpec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySizeSpec.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySizeSpec.Location = new System.Drawing.Point(971, 19);
            this.displaySizeSpec.Name = "displaySizeSpec";
            this.displaySizeSpec.Size = new System.Drawing.Size(10, 23);
            this.displaySizeSpec.TabIndex = 120;
            this.displaySizeSpec.Visible = false;
            // 
            // labelSizeSpec
            // 
            this.labelSizeSpec.Location = new System.Drawing.Point(958, 19);
            this.labelSizeSpec.Name = "labelSizeSpec";
            this.labelSizeSpec.Size = new System.Drawing.Size(10, 23);
            this.labelSizeSpec.TabIndex = 119;
            this.labelSizeSpec.Text = "SizeSpec";
            this.labelSizeSpec.Visible = false;
            // 
            // displayColorID
            // 
            this.displayColorID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayColorID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayColorID.Location = new System.Drawing.Point(816, 19);
            this.displayColorID.Name = "displayColorID";
            this.displayColorID.Size = new System.Drawing.Size(124, 23);
            this.displayColorID.TabIndex = 116;
            // 
            // displayDesc
            // 
            this.displayDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDesc.Location = new System.Drawing.Point(84, 51);
            this.displayDesc.Name = "displayDesc";
            this.displayDesc.Size = new System.Drawing.Size(856, 23);
            this.displayDesc.TabIndex = 118;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(738, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 115;
            this.label4.Text = "ColorID";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(6, 19);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(75, 23);
            this.labelSPNo.TabIndex = 111;
            this.labelSPNo.Text = "SP#";
            // 
            // labelDesc
            // 
            this.labelDesc.Location = new System.Drawing.Point(6, 51);
            this.labelDesc.Name = "labelDesc";
            this.labelDesc.Size = new System.Drawing.Size(75, 23);
            this.labelDesc.TabIndex = 117;
            this.labelDesc.Text = "Desc";
            // 
            // displaySPNo
            // 
            this.displaySPNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySPNo.Location = new System.Drawing.Point(84, 19);
            this.displaySPNo.Name = "displaySPNo";
            this.displaySPNo.Size = new System.Drawing.Size(124, 23);
            this.displaySPNo.TabIndex = 112;
            // 
            // labelRefno
            // 
            this.labelRefno.Location = new System.Drawing.Point(532, 19);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.Size = new System.Drawing.Size(75, 23);
            this.labelRefno.TabIndex = 113;
            this.labelRefno.Text = "Refno";
            // 
            // displayRefno
            // 
            this.displayRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayRefno.Location = new System.Drawing.Point(610, 19);
            this.displayRefno.Name = "displayRefno";
            this.displayRefno.Size = new System.Drawing.Size(124, 23);
            this.displayRefno.TabIndex = 114;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridRollNo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 91);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(993, 386);
            this.panel1.TabIndex = 20;
            // 
            // gridRollNo
            // 
            this.gridRollNo.AllowUserToAddRows = false;
            this.gridRollNo.AllowUserToDeleteRows = false;
            this.gridRollNo.AllowUserToResizeRows = false;
            this.gridRollNo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridRollNo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridRollNo.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridRollNo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridRollNo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridRollNo.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridRollNo.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridRollNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridRollNo.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridRollNo.Location = new System.Drawing.Point(0, 0);
            this.gridRollNo.Name = "gridRollNo";
            this.gridRollNo.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridRollNo.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridRollNo.RowTemplate.Height = 24;
            this.gridRollNo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridRollNo.ShowCellToolTips = false;
            this.gridRollNo.Size = new System.Drawing.Size(993, 386);
            this.gridRollNo.TabIndex = 0;
            this.gridRollNo.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(211, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 121;
            this.label1.Text = "SCIRefno";
            // 
            // displaySCIRefno
            // 
            this.displaySCIRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySCIRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySCIRefno.Location = new System.Drawing.Point(289, 19);
            this.displaySCIRefno.Name = "displaySCIRefno";
            this.displaySCIRefno.Size = new System.Drawing.Size(220, 23);
            this.displaySCIRefno.TabIndex = 122;
            // 
            // P10_Detail_Detail
            // 
            this.ClientSize = new System.Drawing.Size(993, 530);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "P10_Detail_Detail";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Roll#";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridRollNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnCancel;
        private Win.UI.Button btnImport;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Panel panel1;
        private Win.UI.Grid gridRollNo;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.DisplayBox displayTotalQty;
        private Win.UI.Label labelTotalQty;
        private Win.UI.DisplayBox displaySizeSpec;
        private Win.UI.Label labelSizeSpec;
        private Win.UI.DisplayBox displayColorID;
        private Win.UI.DisplayBox displayDesc;
        private Win.UI.Label label4;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelDesc;
        private Win.UI.DisplayBox displaySPNo;
        private Win.UI.Label labelRefno;
        private Win.UI.DisplayBox displayRefno;
        private Win.UI.NumericBox numRequestVariance;
        private Win.UI.Label labelRequestVariance;
        private Win.UI.Label label1;
        private Win.UI.DisplayBox displaySCIRefno;
    }
}
