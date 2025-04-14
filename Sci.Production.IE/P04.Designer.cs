namespace Sci.Production.IE
{
    partial class P04
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
            this.gridLineMappingStatus = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel1 = new Sci.Win.UI.Panel();
            this.txtfty = new Sci.Production.Class.Txtfactory();
            this.label1 = new Sci.Win.UI.Label();
            this.dateInlineDate = new Sci.Win.UI.DateRange();
            this.labelInlineDate = new Sci.Win.UI.Label();
            this.chkLaster = new Sci.Win.UI.CheckBox();
            this.txtCategory = new Sci.Win.UI.TextBox();
            this.labelCAtegory = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridLineMappingStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridLineMappingStatus
            // 
            this.gridLineMappingStatus.AllowUserToAddRows = false;
            this.gridLineMappingStatus.AllowUserToDeleteRows = false;
            this.gridLineMappingStatus.AllowUserToResizeRows = false;
            this.gridLineMappingStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridLineMappingStatus.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridLineMappingStatus.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridLineMappingStatus.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridLineMappingStatus.DataSource = this.listControlBindingSource1;
            this.gridLineMappingStatus.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridLineMappingStatus.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridLineMappingStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridLineMappingStatus.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridLineMappingStatus.Location = new System.Drawing.Point(0, 94);
            this.gridLineMappingStatus.Name = "gridLineMappingStatus";
            this.gridLineMappingStatus.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridLineMappingStatus.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridLineMappingStatus.RowTemplate.Height = 24;
            this.gridLineMappingStatus.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridLineMappingStatus.ShowCellToolTips = false;
            this.gridLineMappingStatus.Size = new System.Drawing.Size(1008, 414);
            this.gridLineMappingStatus.TabIndex = 1;
            this.gridLineMappingStatus.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtfty);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dateInlineDate);
            this.panel1.Controls.Add(this.labelInlineDate);
            this.panel1.Controls.Add(this.chkLaster);
            this.panel1.Controls.Add(this.txtCategory);
            this.panel1.Controls.Add(this.labelCAtegory);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 88);
            this.panel1.TabIndex = 3;
            // 
            // txtfty
            // 
            this.txtfty.BackColor = System.Drawing.Color.White;
            this.txtfty.BoolFtyGroupList = true;
            this.txtfty.FilteMDivision = false;
            this.txtfty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfty.IsIE = false;
            this.txtfty.IsMultiselect = false;
            this.txtfty.IsProduceFty = false;
            this.txtfty.IssupportJunk = false;
            this.txtfty.Location = new System.Drawing.Point(679, 12);
            this.txtfty.MDivision = null;
            this.txtfty.Name = "txtfty";
            this.txtfty.Size = new System.Drawing.Size(66, 23);
            this.txtfty.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(611, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "Factory";
            // 
            // dateInlineDate
            // 
            // 
            // 
            // 
            this.dateInlineDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateInlineDate.DateBox1.Name = "";
            this.dateInlineDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateInlineDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateInlineDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateInlineDate.DateBox2.Name = "";
            this.dateInlineDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateInlineDate.DateBox2.TabIndex = 1;
            this.dateInlineDate.IsRequired = false;
            this.dateInlineDate.Location = new System.Drawing.Point(319, 12);
            this.dateInlineDate.Name = "dateInlineDate";
            this.dateInlineDate.Size = new System.Drawing.Size(280, 23);
            this.dateInlineDate.TabIndex = 0;
            // 
            // labelInlineDate
            // 
            this.labelInlineDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.labelInlineDate.Location = new System.Drawing.Point(211, 12);
            this.labelInlineDate.Name = "labelInlineDate";
            this.labelInlineDate.Size = new System.Drawing.Size(105, 23);
            this.labelInlineDate.TabIndex = 7;
            this.labelInlineDate.Text = "Sewing Inline";
            this.labelInlineDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // chkLaster
            // 
            this.chkLaster.AutoSize = true;
            this.chkLaster.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkLaster.Location = new System.Drawing.Point(12, 50);
            this.chkLaster.Name = "chkLaster";
            this.chkLaster.Size = new System.Drawing.Size(118, 21);
            this.chkLaster.TabIndex = 4;
            this.chkLaster.Text = "Latest Version";
            this.chkLaster.UseVisualStyleBackColor = true;
            this.chkLaster.CheckedChanged += new System.EventHandler(this.ChkLaster_CheckedChanged);
            // 
            // txtCategory
            // 
            this.txtCategory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtCategory.IsSupportEditMode = false;
            this.txtCategory.Location = new System.Drawing.Point(93, 12);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.ReadOnly = true;
            this.txtCategory.Size = new System.Drawing.Size(105, 23);
            this.txtCategory.TabIndex = 6;
            this.txtCategory.Text = "B-Bulk";
            // 
            // labelCAtegory
            // 
            this.labelCAtegory.Location = new System.Drawing.Point(9, 12);
            this.labelCAtegory.Name = "labelCAtegory";
            this.labelCAtegory.Size = new System.Drawing.Size(81, 23);
            this.labelCAtegory.TabIndex = 5;
            this.labelCAtegory.Text = "Category";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(806, 12);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 2;
            this.btnQuery.Text = "Find Now";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // P04
            // 
            this.ClientSize = new System.Drawing.Size(1008, 512);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gridLineMappingStatus);
            this.DefaultControl = "dateInlineDate";
            this.Name = "P04";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P04. Line Mapping Status";
            this.Controls.SetChildIndex(this.gridLineMappingStatus, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridLineMappingStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid gridLineMappingStatus;
        private Win.UI.Panel panel1;
        private Win.UI.CheckBox chkLaster;
        private Win.UI.TextBox txtCategory;
        private Win.UI.Label labelCAtegory;
        private Win.UI.Button btnQuery;
        private Win.UI.Label label1;
        private Win.UI.DateRange dateInlineDate;
        private Win.UI.Label labelInlineDate;
        private Class.Txtfactory txtfty;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}
