namespace Sci.Production.Warehouse
{
    partial class P53
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel3 = new Sci.Win.UI.Panel();
            this.btnSave = new Sci.Win.UI.Button();
            this.labFinished = new Sci.Win.UI.Label();
            this.displayFinished = new Sci.Win.UI.DisplayBox();
            this.labPreparing = new Sci.Win.UI.Label();
            this.displayPreparing = new Sci.Win.UI.DisplayBox();
            this.comboStatus = new Sci.Win.UI.ComboBox();
            this.labStatus = new Sci.Win.UI.Label();
            this.labFactory = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.labRequestNo = new Sci.Win.UI.Label();
            this.labSPNo = new Sci.Win.UI.Label();
            this.labRequestDate = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.txtRequestNo = new Sci.Win.UI.TextBox();
            this.dateRequestDate = new Sci.Win.UI.DateRange();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.detailbs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailbs)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.labFinished);
            this.panel3.Controls.Add(this.displayFinished);
            this.panel3.Controls.Add(this.labPreparing);
            this.panel3.Controls.Add(this.displayPreparing);
            this.panel3.Controls.Add(this.comboStatus);
            this.panel3.Controls.Add(this.labStatus);
            this.panel3.Controls.Add(this.labFactory);
            this.panel3.Controls.Add(this.txtfactory);
            this.panel3.Controls.Add(this.labRequestNo);
            this.panel3.Controls.Add(this.labSPNo);
            this.panel3.Controls.Add(this.labRequestDate);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.txtSPNo);
            this.panel3.Controls.Add(this.txtRequestNo);
            this.panel3.Controls.Add(this.dateRequestDate);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(984, 93);
            this.panel3.TabIndex = 4;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(891, 43);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // labFinished
            // 
            this.labFinished.BackColor = System.Drawing.Color.Transparent;
            this.labFinished.Location = new System.Drawing.Point(123, 65);
            this.labFinished.Name = "labFinished";
            this.labFinished.Size = new System.Drawing.Size(71, 23);
            this.labFinished.TabIndex = 15;
            this.labFinished.Text = "Finished";
            this.labFinished.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // displayFinished
            // 
            this.displayFinished.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFinished.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.displayFinished.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayFinished.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFinished.Location = new System.Drawing.Point(106, 69);
            this.displayFinished.Name = "displayFinished";
            this.displayFinished.Size = new System.Drawing.Size(14, 14);
            this.displayFinished.TabIndex = 14;
            // 
            // labPreparing
            // 
            this.labPreparing.BackColor = System.Drawing.Color.Transparent;
            this.labPreparing.Location = new System.Drawing.Point(24, 65);
            this.labPreparing.Name = "labPreparing";
            this.labPreparing.Size = new System.Drawing.Size(83, 23);
            this.labPreparing.TabIndex = 12;
            this.labPreparing.Text = "Preparing";
            this.labPreparing.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // displayPreparing
            // 
            this.displayPreparing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPreparing.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.displayPreparing.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayPreparing.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPreparing.Location = new System.Drawing.Point(7, 69);
            this.displayPreparing.Name = "displayPreparing";
            this.displayPreparing.Size = new System.Drawing.Size(14, 14);
            this.displayPreparing.TabIndex = 13;
            // 
            // comboStatus
            // 
            this.comboStatus.BackColor = System.Drawing.Color.White;
            this.comboStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStatus.FormattingEnabled = true;
            this.comboStatus.IsSupportUnselect = true;
            this.comboStatus.Items.AddRange(new object[] {
            "Issue Date",
            "Supplier"});
            this.comboStatus.Location = new System.Drawing.Point(674, 9);
            this.comboStatus.Name = "comboStatus";
            this.comboStatus.OldText = "";
            this.comboStatus.Size = new System.Drawing.Size(121, 24);
            this.comboStatus.TabIndex = 2;
            // 
            // labStatus
            // 
            this.labStatus.Location = new System.Drawing.Point(608, 10);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(63, 23);
            this.labStatus.TabIndex = 9;
            this.labStatus.Text = "Status";
            // 
            // labFactory
            // 
            this.labFactory.Location = new System.Drawing.Point(392, 39);
            this.labFactory.Name = "labFactory";
            this.labFactory.Size = new System.Drawing.Size(80, 23);
            this.labFactory.TabIndex = 11;
            this.labFactory.Text = "Factory";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = true;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(475, 39);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(74, 23);
            this.txtfactory.TabIndex = 4;
            // 
            // labRequestNo
            // 
            this.labRequestNo.Location = new System.Drawing.Point(392, 10);
            this.labRequestNo.Name = "labRequestNo";
            this.labRequestNo.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labRequestNo.RectStyle.BorderWidth = 1F;
            this.labRequestNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labRequestNo.RectStyle.ExtBorderWidth = 1F;
            this.labRequestNo.Size = new System.Drawing.Size(80, 23);
            this.labRequestNo.TabIndex = 8;
            this.labRequestNo.Text = "Request#";
            this.labRequestNo.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labRequestNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labSPNo
            // 
            this.labSPNo.Location = new System.Drawing.Point(9, 39);
            this.labSPNo.Name = "labSPNo";
            this.labSPNo.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labSPNo.RectStyle.BorderWidth = 1F;
            this.labSPNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labSPNo.RectStyle.ExtBorderWidth = 1F;
            this.labSPNo.Size = new System.Drawing.Size(98, 23);
            this.labSPNo.TabIndex = 10;
            this.labSPNo.Text = "SP#";
            this.labSPNo.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labRequestDate
            // 
            this.labRequestDate.Location = new System.Drawing.Point(9, 10);
            this.labRequestDate.Name = "labRequestDate";
            this.labRequestDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labRequestDate.RectStyle.BorderWidth = 1F;
            this.labRequestDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labRequestDate.RectStyle.ExtBorderWidth = 1F;
            this.labRequestDate.Size = new System.Drawing.Size(98, 23);
            this.labRequestDate.TabIndex = 7;
            this.labRequestDate.Text = "Request Date";
            this.labRequestDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labRequestDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(891, 7);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 5;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(110, 39);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(130, 23);
            this.txtSPNo.TabIndex = 3;
            // 
            // txtRequestNo
            // 
            this.txtRequestNo.BackColor = System.Drawing.Color.White;
            this.txtRequestNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRequestNo.Location = new System.Drawing.Point(475, 10);
            this.txtRequestNo.Name = "txtRequestNo";
            this.txtRequestNo.Size = new System.Drawing.Size(130, 23);
            this.txtRequestNo.TabIndex = 1;
            // 
            // dateRequestDate
            // 
            // 
            // 
            // 
            this.dateRequestDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRequestDate.DateBox1.Name = "";
            this.dateRequestDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRequestDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRequestDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRequestDate.DateBox2.Name = "";
            this.dateRequestDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRequestDate.DateBox2.TabIndex = 1;
            this.dateRequestDate.Location = new System.Drawing.Point(109, 10);
            this.dateRequestDate.Name = "dateRequestDate";
            this.dateRequestDate.Size = new System.Drawing.Size(280, 23);
            this.dateRequestDate.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 465);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(984, 47);
            this.panel4.TabIndex = 5;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(892, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
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
            this.gridDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDetail.Location = new System.Drawing.Point(0, 93);
            this.gridDetail.Name = "gridDetail";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridDetail.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.ShowCellToolTips = false;
            this.gridDetail.Size = new System.Drawing.Size(984, 372);
            this.gridDetail.TabIndex = 6;
            this.gridDetail.TabStop = false;
            // 
            // P53
            // 
            this.ClientSize = new System.Drawing.Size(984, 512);
            this.Controls.Add(this.gridDetail);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.DefaultControl = "dateRequestDate";
            this.DefaultControlForEdit = "dateRequestDate";
            this.Name = "P53";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P53. Issue Fabric Lacking & Replacement Tracking";
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.gridDetail, 0);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailbs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel3;
        private Win.UI.Button btnQuery;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.TextBox txtRequestNo;
        private Win.UI.DateRange dateRequestDate;
        private Win.UI.Panel panel4;
        private Win.UI.Button btnClose;
        private Win.UI.Grid gridDetail;
        private Win.UI.Label labRequestNo;
        private Win.UI.Label labSPNo;
        private Win.UI.Label labRequestDate;
        private Class.Txtfactory txtfactory;
        private Win.UI.Label labFactory;
        private Win.UI.ComboBox comboStatus;
        private Win.UI.Label labStatus;
        private Win.UI.Label labFinished;
        private Win.UI.DisplayBox displayFinished;
        private Win.UI.Label labPreparing;
        private Win.UI.DisplayBox displayPreparing;
        private Win.UI.Button btnSave;
        private Win.UI.ListControlBindingSource detailbs;
    }
}
