namespace Sci.Production.PPIC
{
    partial class P08_BatchConfirmRespDept
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.dateCreateDate = new Sci.Win.UI.DateRange();
            this.comboDropDownList1 = new Sci.Production.Class.comboDropDownList(this.components);
            this.label3 = new Sci.Win.UI.Label();
            this.dateLock = new Sci.Win.UI.DateBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grid1 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.grid2 = new Sci.Win.UI.Grid();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnQuery = new Sci.Win.UI.Button();
            this.btnReject = new Sci.Win.UI.Button();
            this.btnConfirm = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnReCalculate = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Create Date";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Responsibility";
            // 
            // dateCreateDate
            // 
            // 
            // 
            // 
            this.dateCreateDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCreateDate.DateBox1.Name = "";
            this.dateCreateDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateCreateDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCreateDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateCreateDate.DateBox2.Name = "";
            this.dateCreateDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateCreateDate.DateBox2.TabIndex = 1;
            this.dateCreateDate.IsSupportEditMode = false;
            this.dateCreateDate.Location = new System.Drawing.Point(104, 9);
            this.dateCreateDate.Name = "dateCreateDate";
            this.dateCreateDate.Size = new System.Drawing.Size(280, 23);
            this.dateCreateDate.TabIndex = 3;
            // 
            // comboDropDownList1
            // 
            this.comboDropDownList1.BackColor = System.Drawing.Color.White;
            this.comboDropDownList1.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboDropDownList1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDropDownList1.FormattingEnabled = true;
            this.comboDropDownList1.IsSupportUnselect = true;
            this.comboDropDownList1.Location = new System.Drawing.Point(104, 38);
            this.comboDropDownList1.Name = "comboDropDownList1";
            this.comboDropDownList1.OldText = "";
            this.comboDropDownList1.Size = new System.Drawing.Size(121, 24);
            this.comboDropDownList1.TabIndex = 4;
            this.comboDropDownList1.Type = "Replacement.R";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(387, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Lock Date";
            // 
            // dateLock
            // 
            this.dateLock.IsSupportEditMode = false;
            this.dateLock.Location = new System.Drawing.Point(482, 9);
            this.dateLock.Name = "dateLock";
            this.dateLock.Size = new System.Drawing.Size(130, 23);
            this.dateLock.TabIndex = 6;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(9, 68);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.grid1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grid2);
            this.splitContainer1.Size = new System.Drawing.Size(863, 344);
            this.splitContainer1.SplitterDistance = 501;
            this.splitContainer1.TabIndex = 8;
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(0, 0);
            this.grid1.Name = "grid1";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid1.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(501, 344);
            this.grid1.TabIndex = 0;
            this.grid1.SelectionChanged += new System.EventHandler(this.Grid1_SelectionChanged);
            // 
            // grid2
            // 
            this.grid2.AllowUserToAddRows = false;
            this.grid2.AllowUserToDeleteRows = false;
            this.grid2.AllowUserToResizeRows = false;
            this.grid2.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid2.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid2.DataSource = this.listControlBindingSource2;
            this.grid2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid2.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid2.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid2.Location = new System.Drawing.Point(0, 0);
            this.grid2.Name = "grid2";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid2.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grid2.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid2.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid2.RowTemplate.Height = 24;
            this.grid2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid2.ShowCellToolTips = false;
            this.grid2.Size = new System.Drawing.Size(358, 344);
            this.grid2.TabIndex = 0;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(792, 9);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 7;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // btnReject
            // 
            this.btnReject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReject.Location = new System.Drawing.Point(620, 418);
            this.btnReject.Name = "btnReject";
            this.btnReject.Size = new System.Drawing.Size(80, 30);
            this.btnReject.TabIndex = 9;
            this.btnReject.Text = "Reject";
            this.btnReject.UseVisualStyleBackColor = true;
            this.btnReject.Click += new System.EventHandler(this.BtnReject_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConfirm.Location = new System.Drawing.Point(706, 418);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(80, 30);
            this.btnConfirm.TabIndex = 10;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.BtnConfirm_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(792, 418);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnCose_Click);
            // 
            // btnReCalculate
            // 
            this.btnReCalculate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReCalculate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnReCalculate.Location = new System.Drawing.Point(620, 418);
            this.btnReCalculate.Name = "btnReCalculate";
            this.btnReCalculate.Size = new System.Drawing.Size(166, 30);
            this.btnReCalculate.TabIndex = 12;
            this.btnReCalculate.Text = "Re-Calculate";
            this.btnReCalculate.UseVisualStyleBackColor = true;
            this.btnReCalculate.Visible = false;
            this.btnReCalculate.Click += new System.EventHandler(this.BtnReCalculate_Click);
            // 
            // P08_BatchConfirmRespDept
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 454);
            this.Controls.Add(this.btnReCalculate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.btnReject);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.dateLock);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboDropDownList1);
            this.Controls.Add(this.dateCreateDate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "P08_BatchConfirmRespDept";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "Batch Confirm Responsibility Dept.";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.dateCreateDate, 0);
            this.Controls.SetChildIndex(this.comboDropDownList1, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.dateLock, 0);
            this.Controls.SetChildIndex(this.splitContainer1, 0);
            this.Controls.SetChildIndex(this.btnQuery, 0);
            this.Controls.SetChildIndex(this.btnReject, 0);
            this.Controls.SetChildIndex(this.btnConfirm, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnReCalculate, 0);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.DateRange dateCreateDate;
        private Class.comboDropDownList comboDropDownList1;
        private Win.UI.Label label3;
        private Win.UI.DateBox dateLock;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Button btnQuery;
        private Win.UI.Grid grid1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Grid grid2;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private Win.UI.Button btnReject;
        private Win.UI.Button btnConfirm;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnReCalculate;
    }
}