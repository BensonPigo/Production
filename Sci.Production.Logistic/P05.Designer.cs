namespace Sci.Production.Logistic
{
    partial class P05
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.btnQuery = new Sci.Win.UI.Button();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.txtPackID = new Sci.Win.UI.TextBox();
            this.labelPackID = new Sci.Win.UI.Label();
            this.dateReceiveDate = new Sci.Win.UI.DateRange();
            this.labelReceiveDate = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridReceiveDate = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnToDRExcel = new Sci.Win.UI.Button();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridReceiveDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 454);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(729, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 454);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnToDRExcel);
            this.panel3.Controls.Add(this.btnToExcel);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.txtSPNo);
            this.panel3.Controls.Add(this.labelSPNo);
            this.panel3.Controls.Add(this.txtPackID);
            this.panel3.Controls.Add(this.labelPackID);
            this.panel3.Controls.Add(this.dateReceiveDate);
            this.panel3.Controls.Add(this.labelReceiveDate);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(719, 80);
            this.panel3.TabIndex = 3;
            // 
            // btnToExcel
            // 
            this.btnToExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToExcel.Location = new System.Drawing.Point(626, 44);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(80, 30);
            this.btnToExcel.TabIndex = 7;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(626, 7);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 6;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(279, 49);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(130, 23);
            this.txtSPNo.TabIndex = 5;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(240, 49);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(35, 23);
            this.labelSPNo.TabIndex = 4;
            this.labelSPNo.Text = "SP#";
            // 
            // txtPackID
            // 
            this.txtPackID.BackColor = System.Drawing.Color.White;
            this.txtPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackID.Location = new System.Drawing.Point(67, 49);
            this.txtPackID.Name = "txtPackID";
            this.txtPackID.Size = new System.Drawing.Size(130, 23);
            this.txtPackID.TabIndex = 3;
            // 
            // labelPackID
            // 
            this.labelPackID.Location = new System.Drawing.Point(7, 49);
            this.labelPackID.Name = "labelPackID";
            this.labelPackID.Size = new System.Drawing.Size(56, 23);
            this.labelPackID.TabIndex = 2;
            this.labelPackID.Text = "Pack ID";
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
            this.dateReceiveDate.Location = new System.Drawing.Point(100, 13);
            this.dateReceiveDate.Name = "dateReceiveDate";
            this.dateReceiveDate.Size = new System.Drawing.Size(280, 23);
            this.dateReceiveDate.TabIndex = 1;
            // 
            // labelReceiveDate
            // 
            this.labelReceiveDate.Location = new System.Drawing.Point(7, 13);
            this.labelReceiveDate.Name = "labelReceiveDate";
            this.labelReceiveDate.Size = new System.Drawing.Size(89, 23);
            this.labelReceiveDate.TabIndex = 0;
            this.labelReceiveDate.Text = "Receive Date";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(10, 407);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(719, 47);
            this.panel4.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(627, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridReceiveDate);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 80);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(719, 327);
            this.panel5.TabIndex = 5;
            // 
            // gridReceiveDate
            // 
            this.gridReceiveDate.AllowUserToAddRows = false;
            this.gridReceiveDate.AllowUserToDeleteRows = false;
            this.gridReceiveDate.AllowUserToResizeRows = false;
            this.gridReceiveDate.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridReceiveDate.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridReceiveDate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridReceiveDate.DataSource = this.listControlBindingSource1;
            this.gridReceiveDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridReceiveDate.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridReceiveDate.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridReceiveDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridReceiveDate.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridReceiveDate.Location = new System.Drawing.Point(0, 0);
            this.gridReceiveDate.Name = "gridReceiveDate";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridReceiveDate.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridReceiveDate.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridReceiveDate.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridReceiveDate.RowTemplate.Height = 24;
            this.gridReceiveDate.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridReceiveDate.ShowCellToolTips = false;
            this.gridReceiveDate.Size = new System.Drawing.Size(719, 327);
            this.gridReceiveDate.TabIndex = 0;
            this.gridReceiveDate.TabStop = false;
            // 
            // btnToDRExcel
            // 
            this.btnToDRExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToDRExcel.Location = new System.Drawing.Point(500, 45);
            this.btnToDRExcel.Name = "btnToDRExcel";
            this.btnToDRExcel.Size = new System.Drawing.Size(120, 30);
            this.btnToDRExcel.TabIndex = 8;
            this.btnToDRExcel.Text = "To DR(For PH)";
            this.btnToDRExcel.UseVisualStyleBackColor = true;
            this.btnToDRExcel.Click += new System.EventHandler(this.BtnToDRExcel_Click);
            // 
            // P05
            // 
            this.ClientSize = new System.Drawing.Size(739, 454);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.EditMode = true;
            this.Name = "P05";
            this.Text = "P05. Query For Clog Receive Record";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.panel5, 0);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridReceiveDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Label labelPackID;
        private Win.UI.DateRange dateReceiveDate;
        private Win.UI.Label labelReceiveDate;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridReceiveDate;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Label labelSPNo;
        private Win.UI.TextBox txtPackID;
        private Win.UI.Button btnQuery;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnToExcel;
        private Win.UI.Button btnToDRExcel;
    }
}
