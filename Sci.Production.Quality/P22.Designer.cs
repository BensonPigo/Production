namespace Sci.Production.Quality
{
    partial class P22
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.dateRangeSCIDelivery = new Sci.Win.UI.DateRange();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.txtPoNo = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtPackID = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.panel1 = new Sci.Win.UI.Panel();
            this.checkCartonsInClog = new Sci.Win.UI.CheckBox();
            this.btnFind = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnColse = new Sci.Win.UI.Button();
            this.grid = new Sci.Win.UI.Grid();
            this.listControlBindingSource = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.IsSupportEditMode = false;
            this.txtSPNo.Location = new System.Drawing.Point(101, 14);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(114, 23);
            this.txtSPNo.TabIndex = 0;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(16, 14);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(82, 23);
            this.labelSPNo.TabIndex = 7;
            this.labelSPNo.Text = "SP#";
            // 
            // dateRangeSCIDelivery
            // 
            // 
            // 
            // 
            this.dateRangeSCIDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeSCIDelivery.DateBox1.Name = "";
            this.dateRangeSCIDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeSCIDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeSCIDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeSCIDelivery.DateBox2.Name = "";
            this.dateRangeSCIDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeSCIDelivery.DateBox2.TabIndex = 1;
            this.dateRangeSCIDelivery.IsSupportEditMode = false;
            this.dateRangeSCIDelivery.Location = new System.Drawing.Point(101, 46);
            this.dateRangeSCIDelivery.Name = "dateRangeSCIDelivery";
            this.dateRangeSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateRangeSCIDelivery.TabIndex = 3;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(16, 46);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(82, 23);
            this.labelSCIDelivery.TabIndex = 10;
            this.labelSCIDelivery.Text = "SCI Delivery";
            // 
            // txtPoNo
            // 
            this.txtPoNo.BackColor = System.Drawing.Color.White;
            this.txtPoNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPoNo.IsSupportEditMode = false;
            this.txtPoNo.Location = new System.Drawing.Point(315, 14);
            this.txtPoNo.Name = "txtPoNo";
            this.txtPoNo.Size = new System.Drawing.Size(114, 23);
            this.txtPoNo.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(252, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "PO#";
            // 
            // txtPackID
            // 
            this.txtPackID.BackColor = System.Drawing.Color.White;
            this.txtPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackID.IsSupportEditMode = false;
            this.txtPackID.Location = new System.Drawing.Point(502, 14);
            this.txtPackID.Name = "txtPackID";
            this.txtPackID.Size = new System.Drawing.Size(114, 23);
            this.txtPackID.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(439, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 23);
            this.label2.TabIndex = 9;
            this.label2.Text = "Pack ID";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.panel1.Controls.Add(this.checkCartonsInClog);
            this.panel1.Controls.Add(this.btnFind);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.labelSCIDelivery);
            this.panel1.Controls.Add(this.btnColse);
            this.panel1.Controls.Add(this.txtPackID);
            this.panel1.Controls.Add(this.labelSPNo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtSPNo);
            this.panel1.Controls.Add(this.txtPoNo);
            this.panel1.Controls.Add(this.dateRangeSCIDelivery);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(9, 6);
            this.panel1.Name = "panel1";
            this.panel1.RectStyle.BorderWidths.Bottom = 1F;
            this.panel1.RectStyle.BorderWidths.Left = 1F;
            this.panel1.RectStyle.BorderWidths.Right = 1F;
            this.panel1.RectStyle.BorderWidths.Top = 1F;
            this.panel1.Size = new System.Drawing.Size(725, 79);
            this.panel1.TabIndex = 0;
            // 
            // checkCartonsInClog
            // 
            this.checkCartonsInClog.AutoSize = true;
            this.checkCartonsInClog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkCartonsInClog.IsSupportEditMode = false;
            this.checkCartonsInClog.Location = new System.Drawing.Point(399, 49);
            this.checkCartonsInClog.Name = "checkCartonsInClog";
            this.checkCartonsInClog.Size = new System.Drawing.Size(123, 21);
            this.checkCartonsInClog.TabIndex = 53;
            this.checkCartonsInClog.Text = "Cartons in Clog";
            this.checkCartonsInClog.UseVisualStyleBackColor = true;
            this.checkCartonsInClog.CheckedChanged += new System.EventHandler(this.CheckCartonsInClog_CheckedChanged);
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(622, 10);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 30);
            this.btnFind.TabIndex = 4;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(536, 46);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnColse
            // 
            this.btnColse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnColse.Location = new System.Drawing.Point(622, 46);
            this.btnColse.Name = "btnColse";
            this.btnColse.Size = new System.Drawing.Size(80, 30);
            this.btnColse.TabIndex = 6;
            this.btnColse.Text = "Close";
            this.btnColse.UseVisualStyleBackColor = true;
            this.btnColse.Click += new System.EventHandler(this.BtnColse_Click);
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.DataSource = this.listControlBindingSource;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(9, 91);
            this.grid.Name = "grid";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(725, 452);
            this.grid.TabIndex = 0;
            // 
            // P22
            // 
            this.ClientSize = new System.Drawing.Size(744, 555);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "txtSPNo";
            this.Name = "P22";
            this.Text = "P22.CFA Select Need Inspection Cartons";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.grid, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.TextBox txtSPNo;
        private Win.UI.Label labelSPNo;
        private Win.UI.DateRange dateRangeSCIDelivery;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.TextBox txtPoNo;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtPackID;
        private Win.UI.Label label2;
        private Win.UI.Panel panel1;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnColse;
        private Win.UI.Button btnFind;
        private Win.UI.Grid grid;
        private Win.UI.ListControlBindingSource listControlBindingSource;
        private Win.UI.CheckBox checkCartonsInClog;
    }
}
