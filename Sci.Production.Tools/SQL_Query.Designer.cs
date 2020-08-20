namespace Sci.Production.Tools
{
    partial class SQL_Query
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.editSQL = new Sci.Win.UI.EditBox();
            this.btnQuery = new Sci.Win.UI.Button();
            this.gridSQLQuery = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnSqlUpdate = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.panel1 = new Sci.Win.UI.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSQLQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridSQLQuery);
            this.splitContainer1.Size = new System.Drawing.Size(801, 547);
            this.splitContainer1.SplitterDistance = 107;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.editSQL);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.btnQuery);
            this.splitContainer2.Size = new System.Drawing.Size(801, 107);
            this.splitContainer2.SplitterDistance = 723;
            this.splitContainer2.TabIndex = 0;
            // 
            // editSQL
            // 
            this.editSQL.BackColor = System.Drawing.Color.White;
            this.editSQL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.editSQL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editSQL.IsSupportEditMode = false;
            this.editSQL.Location = new System.Drawing.Point(0, 0);
            this.editSQL.Multiline = true;
            this.editSQL.Name = "editSQL";
            this.editSQL.Size = new System.Drawing.Size(723, 107);
            this.editSQL.TabIndex = 11;
            // 
            // btnQuery
            // 
            this.btnQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnQuery.Location = new System.Drawing.Point(0, 0);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(74, 107);
            this.btnQuery.TabIndex = 12;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // gridSQLQuery
            // 
            this.gridSQLQuery.AllowUserToAddRows = false;
            this.gridSQLQuery.AllowUserToDeleteRows = false;
            this.gridSQLQuery.AllowUserToResizeRows = false;
            this.gridSQLQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridSQLQuery.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSQLQuery.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSQLQuery.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSQLQuery.DataSource = this.listControlBindingSource1;
            this.gridSQLQuery.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSQLQuery.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSQLQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSQLQuery.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSQLQuery.Location = new System.Drawing.Point(0, 0);
            this.gridSQLQuery.Name = "gridSQLQuery";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridSQLQuery.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridSQLQuery.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSQLQuery.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSQLQuery.RowTemplate.Height = 24;
            this.gridSQLQuery.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSQLQuery.Size = new System.Drawing.Size(801, 399);
            this.gridSQLQuery.TabIndex = 1;
            this.gridSQLQuery.TabStop = false;
            // 
            // btnSqlUpdate
            // 
            this.btnSqlUpdate.Location = new System.Drawing.Point(58, 6);
            this.btnSqlUpdate.Name = "btnSqlUpdate";
            this.btnSqlUpdate.Size = new System.Drawing.Size(110, 28);
            this.btnSqlUpdate.TabIndex = 3;
            this.btnSqlUpdate.Text = "SQL Update";
            this.btnSqlUpdate.UseVisualStyleBackColor = true;
            this.btnSqlUpdate.Visible = false;
            this.btnSqlUpdate.Click += new System.EventHandler(this.BtnSqlUpdate_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(21, 14);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(15, 14);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.CheckBox1_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.btnSqlUpdate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 510);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(801, 37);
            this.panel1.TabIndex = 2;
            // 
            // SQL_Query
            // 
            this.ClientSize = new System.Drawing.Size(801, 547);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitContainer1);
            this.IsSupportEdit = false;
            this.Name = "SQL_Query";
            this.Text = "SQL_Query";
            this.Controls.SetChildIndex(this.splitContainer1, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSQLQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Win.UI.Grid gridSQLQuery;
        private Win.UI.Button btnQuery;
        private Win.UI.EditBox editSQL;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private System.Windows.Forms.Button btnSqlUpdate;
        private System.Windows.Forms.CheckBox checkBox1;
        private Win.UI.Panel panel1;

    }
}
