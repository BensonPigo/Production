namespace Sci.Production.Warehouse
{
    partial class P11_AutoPick_Detail
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
            this.btnSave = new Sci.Win.UI.Button();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.labelTotalIssueQty = new Sci.Win.UI.Label();
            this.displayTotalIssueQty = new Sci.Win.UI.DisplayBox();
            this.button1 = new Sci.Win.UI.Button();
            this.button2 = new Sci.Win.UI.Button();
            this.panel1 = new Sci.Win.UI.Panel();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.eb_desc = new Sci.Win.UI.EditBox();
            this.editOrderList = new Sci.Win.UI.EditBox();
            this.labelQty = new Sci.Win.UI.Label();
            this.labelSize = new Sci.Win.UI.Label();
            this.displyQty = new Sci.Win.UI.DisplayBox();
            this.displySize = new Sci.Win.UI.DisplayBox();
            this.labelOrderList = new Sci.Win.UI.Label();
            this.labelSeqNo = new Sci.Win.UI.Label();
            this.displySpecial = new Sci.Win.UI.DisplayBox();
            this.labelSpecial = new Sci.Win.UI.Label();
            this.labelUnit = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.displySeqNo = new Sci.Win.UI.DisplayBox();
            this.displyUnit = new Sci.Win.UI.DisplayBox();
            this.labelColor = new Sci.Win.UI.Label();
            this.displyColorid = new Sci.Win.UI.DisplayBox();
            this.gridBreakDown = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.gridAutoPickDetail = new Sci.Win.UI.Grid();
            this.label1 = new Sci.Win.UI.Label();
            this.displayDiffqty = new Sci.Win.UI.DisplayBox();
            this.groupBox2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBreakDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAutoPickDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(912, 16);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Undo";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(816, 16);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 30);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.displayDiffqty);
            this.groupBox2.Controls.Add(this.labelTotalIssueQty);
            this.groupBox2.Controls.Add(this.displayTotalIssueQty);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnSave);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 504);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1008, 53);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            // 
            // labelTotalIssueQty
            // 
            this.labelTotalIssueQty.Location = new System.Drawing.Point(478, 20);
            this.labelTotalIssueQty.Name = "labelTotalIssueQty";
            this.labelTotalIssueQty.Size = new System.Drawing.Size(99, 23);
            this.labelTotalIssueQty.TabIndex = 116;
            this.labelTotalIssueQty.Text = "Total Issue Qty";
            // 
            // displayTotalIssueQty
            // 
            this.displayTotalIssueQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayTotalIssueQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayTotalIssueQty.Location = new System.Drawing.Point(580, 20);
            this.displayTotalIssueQty.Name = "displayTotalIssueQty";
            this.displayTotalIssueQty.Size = new System.Drawing.Size(124, 23);
            this.displayTotalIssueQty.TabIndex = 115;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(763, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(50, 30);
            this.button1.TabIndex = 3;
            this.button1.Text = "Next";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(710, 16);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(50, 30);
            this.button2.TabIndex = 2;
            this.button2.Text = "Prev";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.Button2_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.gridBreakDown);
            this.panel1.Controls.Add(this.gridAutoPickDetail);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 504);
            this.panel1.TabIndex = 20;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.eb_desc);
            this.groupBox1.Controls.Add(this.editOrderList);
            this.groupBox1.Controls.Add(this.labelQty);
            this.groupBox1.Controls.Add(this.labelSize);
            this.groupBox1.Controls.Add(this.displyQty);
            this.groupBox1.Controls.Add(this.displySize);
            this.groupBox1.Controls.Add(this.labelOrderList);
            this.groupBox1.Controls.Add(this.labelSeqNo);
            this.groupBox1.Controls.Add(this.displySpecial);
            this.groupBox1.Controls.Add(this.labelSpecial);
            this.groupBox1.Controls.Add(this.labelUnit);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.displySeqNo);
            this.groupBox1.Controls.Add(this.displyUnit);
            this.groupBox1.Controls.Add(this.labelColor);
            this.groupBox1.Controls.Add(this.displyColorid);
            this.groupBox1.Location = new System.Drawing.Point(12, 217);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(542, 284);
            this.groupBox1.TabIndex = 113;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Issue Item";
            // 
            // eb_desc
            // 
            this.eb_desc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.eb_desc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.eb_desc.IsSupportEditMode = false;
            this.eb_desc.Location = new System.Drawing.Point(87, 220);
            this.eb_desc.Multiline = true;
            this.eb_desc.Name = "eb_desc";
            this.eb_desc.ReadOnly = true;
            this.eb_desc.Size = new System.Drawing.Size(449, 50);
            this.eb_desc.TabIndex = 116;
            // 
            // editOrderList
            // 
            this.editOrderList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editOrderList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editOrderList.IsSupportEditMode = false;
            this.editOrderList.Location = new System.Drawing.Point(87, 162);
            this.editOrderList.Multiline = true;
            this.editOrderList.Name = "editOrderList";
            this.editOrderList.ReadOnly = true;
            this.editOrderList.Size = new System.Drawing.Size(449, 50);
            this.editOrderList.TabIndex = 115;
            // 
            // labelQty
            // 
            this.labelQty.Location = new System.Drawing.Point(9, 57);
            this.labelQty.Name = "labelQty";
            this.labelQty.Size = new System.Drawing.Size(75, 23);
            this.labelQty.TabIndex = 111;
            this.labelQty.Text = "@Qty";
            // 
            // labelSize
            // 
            this.labelSize.Location = new System.Drawing.Point(252, 57);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(75, 23);
            this.labelSize.TabIndex = 112;
            this.labelSize.Text = "Size";
            // 
            // displyQty
            // 
            this.displyQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displyQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displyQty.Location = new System.Drawing.Point(87, 57);
            this.displyQty.Name = "displyQty";
            this.displyQty.Size = new System.Drawing.Size(124, 23);
            this.displyQty.TabIndex = 113;
            // 
            // displySize
            // 
            this.displySize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displySize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displySize.Location = new System.Drawing.Point(330, 57);
            this.displySize.Name = "displySize";
            this.displySize.Size = new System.Drawing.Size(124, 23);
            this.displySize.TabIndex = 114;
            // 
            // labelOrderList
            // 
            this.labelOrderList.Location = new System.Drawing.Point(9, 162);
            this.labelOrderList.Name = "labelOrderList";
            this.labelOrderList.Size = new System.Drawing.Size(75, 23);
            this.labelOrderList.TabIndex = 109;
            this.labelOrderList.Text = "Order List";
            // 
            // labelSeqNo
            // 
            this.labelSeqNo.Location = new System.Drawing.Point(9, 22);
            this.labelSeqNo.Name = "labelSeqNo";
            this.labelSeqNo.Size = new System.Drawing.Size(75, 23);
            this.labelSeqNo.TabIndex = 99;
            this.labelSeqNo.Text = "Seq#";
            // 
            // displySpecial
            // 
            this.displySpecial.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displySpecial.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displySpecial.Location = new System.Drawing.Point(87, 127);
            this.displySpecial.Name = "displySpecial";
            this.displySpecial.Size = new System.Drawing.Size(124, 23);
            this.displySpecial.TabIndex = 106;
            // 
            // labelSpecial
            // 
            this.labelSpecial.Location = new System.Drawing.Point(9, 127);
            this.labelSpecial.Name = "labelSpecial";
            this.labelSpecial.Size = new System.Drawing.Size(75, 23);
            this.labelSpecial.TabIndex = 105;
            this.labelSpecial.Text = "Special";
            // 
            // labelUnit
            // 
            this.labelUnit.Location = new System.Drawing.Point(252, 22);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(75, 23);
            this.labelUnit.TabIndex = 100;
            this.labelUnit.Text = "Unit";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 220);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 107;
            this.label5.Text = "Desc";
            // 
            // displySeqNo
            // 
            this.displySeqNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displySeqNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displySeqNo.Location = new System.Drawing.Point(87, 22);
            this.displySeqNo.Name = "displySeqNo";
            this.displySeqNo.Size = new System.Drawing.Size(124, 23);
            this.displySeqNo.TabIndex = 101;
            // 
            // displyUnit
            // 
            this.displyUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displyUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displyUnit.Location = new System.Drawing.Point(330, 22);
            this.displyUnit.Name = "displyUnit";
            this.displyUnit.Size = new System.Drawing.Size(124, 23);
            this.displyUnit.TabIndex = 102;
            // 
            // labelColor
            // 
            this.labelColor.Location = new System.Drawing.Point(9, 92);
            this.labelColor.Name = "labelColor";
            this.labelColor.Size = new System.Drawing.Size(75, 23);
            this.labelColor.TabIndex = 103;
            this.labelColor.Text = "Color";
            // 
            // displyColorid
            // 
            this.displyColorid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displyColorid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displyColorid.Location = new System.Drawing.Point(87, 92);
            this.displyColorid.Name = "displyColorid";
            this.displyColorid.Size = new System.Drawing.Size(124, 23);
            this.displyColorid.TabIndex = 104;
            // 
            // gridBreakDown
            // 
            this.gridBreakDown.AllowUserToAddRows = false;
            this.gridBreakDown.AllowUserToDeleteRows = false;
            this.gridBreakDown.AllowUserToResizeRows = false;
            this.gridBreakDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridBreakDown.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBreakDown.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBreakDown.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBreakDown.DataSource = this.listControlBindingSource1;
            this.gridBreakDown.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBreakDown.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBreakDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBreakDown.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBreakDown.Location = new System.Drawing.Point(10, 6);
            this.gridBreakDown.Name = "gridBreakDown";
            this.gridBreakDown.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridBreakDown.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridBreakDown.RowTemplate.Height = 24;
            this.gridBreakDown.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBreakDown.ShowCellToolTips = false;
            this.gridBreakDown.Size = new System.Drawing.Size(986, 205);
            this.gridBreakDown.TabIndex = 112;
            this.gridBreakDown.TabStop = false;
            // 
            // gridAutoPickDetail
            // 
            this.gridAutoPickDetail.AllowUserToAddRows = false;
            this.gridAutoPickDetail.AllowUserToDeleteRows = false;
            this.gridAutoPickDetail.AllowUserToResizeRows = false;
            this.gridAutoPickDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridAutoPickDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridAutoPickDetail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridAutoPickDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridAutoPickDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridAutoPickDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridAutoPickDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridAutoPickDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridAutoPickDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridAutoPickDetail.Location = new System.Drawing.Point(554, 217);
            this.gridAutoPickDetail.Name = "gridAutoPickDetail";
            this.gridAutoPickDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridAutoPickDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridAutoPickDetail.RowTemplate.Height = 24;
            this.gridAutoPickDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridAutoPickDetail.ShowCellToolTips = false;
            this.gridAutoPickDetail.Size = new System.Drawing.Size(442, 284);
            this.gridAutoPickDetail.TabIndex = 0;
            this.gridAutoPickDetail.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(242, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 23);
            this.label1.TabIndex = 118;
            this.label1.Text = "Diff with bal. qty";
            // 
            // displayDiffqty
            // 
            this.displayDiffqty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDiffqty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDiffqty.Location = new System.Drawing.Point(351, 20);
            this.displayDiffqty.Name = "displayDiffqty";
            this.displayDiffqty.Size = new System.Drawing.Size(124, 23);
            this.displayDiffqty.TabIndex = 117;
            // 
            // P11_AutoPick_Detail
            // 
            this.ClientSize = new System.Drawing.Size(1008, 557);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Name = "P11_AutoPick_Detail";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P11. Auto Pick Detail";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.P11_AutoPick_Detail_FormClosing);
            this.Load += new System.EventHandler(this.P11_AutoPick_Detail_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBreakDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAutoPickDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnCancel;
        private Win.UI.Button btnSave;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.Panel panel1;
        private Win.UI.Grid gridAutoPickDetail;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Grid gridBreakDown;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.EditBox eb_desc;
        private Win.UI.EditBox editOrderList;
        private Win.UI.Label labelQty;
        private Win.UI.Label labelSize;
        private Win.UI.DisplayBox displyQty;
        private Win.UI.DisplayBox displySize;
        private Win.UI.Label labelOrderList;
        private Win.UI.Label labelSeqNo;
        private Win.UI.DisplayBox displySpecial;
        private Win.UI.Label labelSpecial;
        private Win.UI.Label labelUnit;
        private Win.UI.Label label5;
        private Win.UI.DisplayBox displySeqNo;
        private Win.UI.DisplayBox displyUnit;
        private Win.UI.Label labelColor;
        private Win.UI.DisplayBox displyColorid;
        private Win.UI.Button button1;
        private Win.UI.Button button2;
        private Win.UI.Label labelTotalIssueQty;
        private Win.UI.DisplayBox displayTotalIssueQty;
        private Win.UI.Label label1;
        private Win.UI.DisplayBox displayDiffqty;
    }
}
