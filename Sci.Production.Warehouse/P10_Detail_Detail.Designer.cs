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
            this.labelSciRefno = new Sci.Win.UI.Label();
            this.displaySciRefno = new Sci.Win.UI.DisplayBox();
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridRollNo = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
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
            this.btnCancel.Location = new System.Drawing.Point(912, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(816, 16);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 30);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.button2_Click);
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
            this.labelRequestVariance.Lines = 0;
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
            this.labelTotalQty.Lines = 0;
            this.labelTotalQty.Location = new System.Drawing.Point(439, 22);
            this.labelTotalQty.Name = "labelTotalQty";
            this.labelTotalQty.Size = new System.Drawing.Size(95, 23);
            this.labelTotalQty.TabIndex = 3;
            this.labelTotalQty.Text = "Total Qty";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.displaySizeSpec);
            this.groupBox1.Controls.Add(this.labelSizeSpec);
            this.groupBox1.Controls.Add(this.displayColorID);
            this.groupBox1.Controls.Add(this.displayDesc);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.labelSPNo);
            this.groupBox1.Controls.Add(this.labelDesc);
            this.groupBox1.Controls.Add(this.displaySPNo);
            this.groupBox1.Controls.Add(this.labelSciRefno);
            this.groupBox1.Controls.Add(this.displaySciRefno);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1008, 91);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // displaySizeSpec
            // 
            this.displaySizeSpec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySizeSpec.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySizeSpec.Location = new System.Drawing.Point(762, 19);
            this.displaySizeSpec.Name = "displaySizeSpec";
            this.displaySizeSpec.Size = new System.Drawing.Size(124, 23);
            this.displaySizeSpec.TabIndex = 120;
            this.displaySizeSpec.Visible = false;
            // 
            // labelSizeSpec
            // 
            this.labelSizeSpec.Lines = 0;
            this.labelSizeSpec.Location = new System.Drawing.Point(684, 19);
            this.labelSizeSpec.Name = "labelSizeSpec";
            this.labelSizeSpec.Size = new System.Drawing.Size(75, 23);
            this.labelSizeSpec.TabIndex = 119;
            this.labelSizeSpec.Text = "SizeSpec";
            this.labelSizeSpec.Visible = false;
            // 
            // displayColorID
            // 
            this.displayColorID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayColorID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayColorID.Location = new System.Drawing.Point(537, 19);
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
            this.displayDesc.Size = new System.Drawing.Size(802, 23);
            this.displayDesc.TabIndex = 118;
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(459, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 115;
            this.label4.Text = "ColorID";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Lines = 0;
            this.labelSPNo.Location = new System.Drawing.Point(6, 19);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(75, 23);
            this.labelSPNo.TabIndex = 111;
            this.labelSPNo.Text = "SP#";
            // 
            // labelDesc
            // 
            this.labelDesc.Lines = 0;
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
            // labelSciRefno
            // 
            this.labelSciRefno.Lines = 0;
            this.labelSciRefno.Location = new System.Drawing.Point(233, 19);
            this.labelSciRefno.Name = "labelSciRefno";
            this.labelSciRefno.Size = new System.Drawing.Size(75, 23);
            this.labelSciRefno.TabIndex = 113;
            this.labelSciRefno.Text = "SciRefno";
            // 
            // displaySciRefno
            // 
            this.displaySciRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySciRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySciRefno.Location = new System.Drawing.Point(311, 19);
            this.displaySciRefno.Name = "displaySciRefno";
            this.displaySciRefno.Size = new System.Drawing.Size(124, 23);
            this.displaySciRefno.TabIndex = 114;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridRollNo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 91);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 386);
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
            this.gridRollNo.Size = new System.Drawing.Size(1008, 386);
            this.gridRollNo.TabIndex = 0;
            this.gridRollNo.TabStop = false;
            // 
            // P10_Detail_Detail
            // 
            this.ClientSize = new System.Drawing.Size(1008, 530);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "P10_Detail_Detail";
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
        private Win.UI.Label labelSciRefno;
        private Win.UI.DisplayBox displaySciRefno;
        private Win.UI.NumericBox numRequestVariance;
        private Win.UI.Label labelRequestVariance;
    }
}
