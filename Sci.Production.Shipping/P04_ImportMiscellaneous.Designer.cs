namespace Sci.Production.Shipping
{
    partial class P04_ImportMiscellaneous
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
            this.panel3 = new Sci.Win.UI.Panel();
            this.dateDeliveryDate = new Sci.Win.UI.DateRange();
            this.labDeliveryDate = new Sci.Win.UI.Label();
            this.labLocalMiscPoNo = new Sci.Win.UI.Label();
            this.txtLocalMiscPoNo = new Sci.Win.UI.TextBox();
            this.btnQuery = new Sci.Win.UI.Button();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.gridImport = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dateDeliveryDate);
            this.panel3.Controls.Add(this.labDeliveryDate);
            this.panel3.Controls.Add(this.labLocalMiscPoNo);
            this.panel3.Controls.Add(this.txtLocalMiscPoNo);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(873, 45);
            this.panel3.TabIndex = 3;
            // 
            // dateDeliveryDate
            // 
            // 
            // 
            // 
            this.dateDeliveryDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateDeliveryDate.DateBox1.Name = "";
            this.dateDeliveryDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateDeliveryDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateDeliveryDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateDeliveryDate.DateBox2.Name = "";
            this.dateDeliveryDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateDeliveryDate.DateBox2.TabIndex = 1;
            this.dateDeliveryDate.IsRequired = false;
            this.dateDeliveryDate.Location = new System.Drawing.Point(384, 12);
            this.dateDeliveryDate.Name = "dateDeliveryDate";
            this.dateDeliveryDate.Size = new System.Drawing.Size(280, 23);
            this.dateDeliveryDate.TabIndex = 1;
            // 
            // labDeliveryDate
            // 
            this.labDeliveryDate.Location = new System.Drawing.Point(280, 12);
            this.labDeliveryDate.Name = "labDeliveryDate";
            this.labDeliveryDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labDeliveryDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labDeliveryDate.Size = new System.Drawing.Size(101, 23);
            this.labDeliveryDate.TabIndex = 4;
            this.labDeliveryDate.Text = "Delivery Date";
            this.labDeliveryDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labDeliveryDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labLocalMiscPoNo
            // 
            this.labLocalMiscPoNo.Location = new System.Drawing.Point(4, 12);
            this.labLocalMiscPoNo.Name = "labLocalMiscPoNo";
            this.labLocalMiscPoNo.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labLocalMiscPoNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labLocalMiscPoNo.Size = new System.Drawing.Size(118, 23);
            this.labLocalMiscPoNo.TabIndex = 3;
            this.labLocalMiscPoNo.Text = "Local Misc. PO#";
            this.labLocalMiscPoNo.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labLocalMiscPoNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtLocalMiscPoNo
            // 
            this.txtLocalMiscPoNo.BackColor = System.Drawing.Color.White;
            this.txtLocalMiscPoNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocalMiscPoNo.Location = new System.Drawing.Point(125, 12);
            this.txtLocalMiscPoNo.Name = "txtLocalMiscPoNo";
            this.txtLocalMiscPoNo.Size = new System.Drawing.Size(140, 23);
            this.txtLocalMiscPoNo.TabIndex = 0;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(781, 9);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 2;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Controls.Add(this.btnImport);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 430);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(873, 44);
            this.panel4.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(781, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(695, 6);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(80, 30);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // gridImport
            // 
            this.gridImport.AllowUserToAddRows = false;
            this.gridImport.AllowUserToDeleteRows = false;
            this.gridImport.AllowUserToResizeRows = false;
            this.gridImport.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridImport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridImport.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridImport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridImport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridImport.Location = new System.Drawing.Point(0, 45);
            this.gridImport.Name = "gridImport";
            this.gridImport.RowHeadersVisible = false;
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridImport.RowTemplate.Height = 24;
            this.gridImport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridImport.ShowCellToolTips = false;
            this.gridImport.Size = new System.Drawing.Size(873, 385);
            this.gridImport.TabIndex = 5;
            this.gridImport.TabStop = false;
            // 
            // P04_ImportMiscellaneous
            // 
            this.ClientSize = new System.Drawing.Size(873, 474);
            this.Controls.Add(this.gridImport);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Name = "P04_ImportMiscellaneous";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Import Data - Miscellaneous";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel3;
        private Win.UI.Label labLocalMiscPoNo;
        private Win.UI.TextBox txtLocalMiscPoNo;
        private Win.UI.Button btnQuery;
        private Win.UI.Panel panel4;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnImport;
        private Win.UI.Grid gridImport;
        private Win.UI.Label labDeliveryDate;
        private Win.UI.DateRange dateDeliveryDate;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}
