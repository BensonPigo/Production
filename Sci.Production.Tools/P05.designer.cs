namespace Sci.Production.Tools
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel5 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.panel1 = new Sci.Win.UI.Panel();
            this.txtJSONContains = new Sci.Win.UI.TextBox();
            this.labJsonContains = new Sci.Win.UI.Label();
            this.dateCreateTime = new Sci.Win.UI.DateRange();
            this.labCreateTime = new Sci.Win.UI.Label();
            this.txtsupplier = new Sci.Production.Class.Txtsupplier();
            this.btnQuery = new Sci.Win.UI.Button();
            this.label1 = new Sci.Win.UI.Label();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.grid = new Sci.Win.UI.Grid();
            this.panel5.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.btnClose);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(0, 613);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1026, 44);
            this.panel5.TabIndex = 8;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(940, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtJSONContains);
            this.panel1.Controls.Add(this.labJsonContains);
            this.panel1.Controls.Add(this.dateCreateTime);
            this.panel1.Controls.Add(this.labCreateTime);
            this.panel1.Controls.Add(this.txtsupplier);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1026, 80);
            this.panel1.TabIndex = 7;
            // 
            // txtJSONContains
            // 
            this.txtJSONContains.BackColor = System.Drawing.Color.White;
            this.txtJSONContains.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtJSONContains.Location = new System.Drawing.Point(535, 16);
            this.txtJSONContains.Name = "txtJSONContains";
            this.txtJSONContains.Size = new System.Drawing.Size(309, 23);
            this.txtJSONContains.TabIndex = 2;
            // 
            // labJsonContains
            // 
            this.labJsonContains.Location = new System.Drawing.Point(415, 16);
            this.labJsonContains.Name = "labJsonContains";
            this.labJsonContains.Size = new System.Drawing.Size(117, 23);
            this.labJsonContains.TabIndex = 8;
            this.labJsonContains.Text = "JSON Contains";
            // 
            // dateCreateTime
            // 
            // 
            // 
            // 
            this.dateCreateTime.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCreateTime.DateBox1.Name = "";
            this.dateCreateTime.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateCreateTime.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCreateTime.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateCreateTime.DateBox2.Name = "";
            this.dateCreateTime.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateCreateTime.DateBox2.TabIndex = 1;
            this.dateCreateTime.IsRequired = false;
            this.dateCreateTime.Location = new System.Drawing.Point(117, 45);
            this.dateCreateTime.Name = "dateCreateTime";
            this.dateCreateTime.Size = new System.Drawing.Size(280, 23);
            this.dateCreateTime.TabIndex = 1;
            // 
            // labCreateTime
            // 
            this.labCreateTime.Location = new System.Drawing.Point(12, 45);
            this.labCreateTime.Name = "labCreateTime";
            this.labCreateTime.Size = new System.Drawing.Size(102, 23);
            this.labCreateTime.TabIndex = 7;
            this.labCreateTime.Text = "Create Time";
            // 
            // txtsupplier
            // 
            this.txtsupplier.DisplayBox1Binding = "";
            this.txtsupplier.Location = new System.Drawing.Point(117, 13);
            this.txtsupplier.Name = "txtsupplier";
            this.txtsupplier.Size = new System.Drawing.Size(147, 23);
            this.txtsupplier.TabIndex = 0;
            this.txtsupplier.TextBox1Binding = "";
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(913, 9);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(101, 30);
            this.btnQuery.TabIndex = 4;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Supp#";
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.DataSource = this.listControlBindingSource1;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(0, 80);
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
            this.grid.Size = new System.Drawing.Size(1026, 533);
            this.grid.TabIndex = 9;
            // 
            // P05
            // 
            this.ClientSize = new System.Drawing.Size(1026, 657);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel1);
            this.IsToolbarVisible = false;
            this.Name = "P05";
            this.OnLineHelpID = "Sci.Win.Forms.Base";
            this.Text = "P05 Receive from supplier";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel5, 0);
            this.Controls.SetChildIndex(this.grid, 0);
            this.panel5.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel5;
        private Win.UI.Button btnClose;
        private Win.UI.Panel panel1;
        private Win.UI.TextBox txtJSONContains;
        private Win.UI.Label labJsonContains;
        private Win.UI.DateRange dateCreateTime;
        private Win.UI.Label labCreateTime;
        private Class.Txtsupplier txtsupplier;
        private Win.UI.Button btnQuery;
        private Win.UI.Label label1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Grid grid;
    }
}
