namespace Sci.Production.PPIC
{
    partial class P02
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.btnQuery = new System.Windows.Forms.Button();
            this.txtbrand1 = new Sci.Production.Class.Txtbrand();
            this.labBrand = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.dateUpdatedDate = new Sci.Win.UI.DateBox();
            this.labelUpdatedDate = new Sci.Win.UI.Label();
            this.dateLastDate = new Sci.Win.UI.DateBox();
            this.labelLastDate = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnExcel = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridUpdateOrder = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridUpdateOrder)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 490);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(911, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 490);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.txtbrand1);
            this.panel3.Controls.Add(this.labBrand);
            this.panel3.Controls.Add(this.comboFactory);
            this.panel3.Controls.Add(this.labelFactory);
            this.panel3.Controls.Add(this.dateUpdatedDate);
            this.panel3.Controls.Add(this.labelUpdatedDate);
            this.panel3.Controls.Add(this.dateLastDate);
            this.panel3.Controls.Add(this.labelLastDate);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(906, 40);
            this.panel3.TabIndex = 1;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(779, 7);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(93, 28);
            this.btnQuery.TabIndex = 7;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // txtbrand1
            // 
            this.txtbrand1.BackColor = System.Drawing.Color.White;
            this.txtbrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand1.Location = new System.Drawing.Point(640, 11);
            this.txtbrand1.Name = "txtbrand1";
            this.txtbrand1.Size = new System.Drawing.Size(66, 23);
            this.txtbrand1.TabIndex = 6;
            // 
            // labBrand
            // 
            this.labBrand.Location = new System.Drawing.Point(582, 10);
            this.labBrand.Name = "labBrand";
            this.labBrand.Size = new System.Drawing.Size(55, 23);
            this.labBrand.TabIndex = 5;
            this.labBrand.Text = "Brand";
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(479, 9);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(87, 24);
            this.comboFactory.TabIndex = 2;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(421, 10);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(55, 23);
            this.labelFactory.TabIndex = 4;
            this.labelFactory.Text = "Factory";
            // 
            // dateUpdatedDate
            // 
            this.dateUpdatedDate.Location = new System.Drawing.Point(295, 10);
            this.dateUpdatedDate.Name = "dateUpdatedDate";
            this.dateUpdatedDate.Size = new System.Drawing.Size(110, 23);
            this.dateUpdatedDate.TabIndex = 1;
            // 
            // labelUpdatedDate
            // 
            this.labelUpdatedDate.Location = new System.Drawing.Point(201, 10);
            this.labelUpdatedDate.Name = "labelUpdatedDate";
            this.labelUpdatedDate.Size = new System.Drawing.Size(90, 23);
            this.labelUpdatedDate.TabIndex = 2;
            this.labelUpdatedDate.Text = "Updated Date";
            // 
            // dateLastDate
            // 
            this.dateLastDate.IsSupportEditMode = false;
            this.dateLastDate.Location = new System.Drawing.Point(76, 10);
            this.dateLastDate.Name = "dateLastDate";
            this.dateLastDate.ReadOnly = true;
            this.dateLastDate.Size = new System.Drawing.Size(110, 23);
            this.dateLastDate.TabIndex = 0;
            // 
            // labelLastDate
            // 
            this.labelLastDate.Location = new System.Drawing.Point(8, 10);
            this.labelLastDate.Name = "labelLastDate";
            this.labelLastDate.Size = new System.Drawing.Size(64, 23);
            this.labelLastDate.TabIndex = 0;
            this.labelLastDate.Text = "Last Date";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Controls.Add(this.btnExcel);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 444);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(906, 46);
            this.panel4.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(801, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExcel.Location = new System.Drawing.Point(706, 7);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(80, 30);
            this.btnExcel.TabIndex = 0;
            this.btnExcel.Text = "Excel";
            this.btnExcel.UseVisualStyleBackColor = true;
            this.btnExcel.Click += new System.EventHandler(this.BtnExcel_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridUpdateOrder);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 40);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(906, 404);
            this.panel5.TabIndex = 5;
            // 
            // gridUpdateOrder
            // 
            this.gridUpdateOrder.AllowUserToAddRows = false;
            this.gridUpdateOrder.AllowUserToDeleteRows = false;
            this.gridUpdateOrder.AllowUserToResizeRows = false;
            this.gridUpdateOrder.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridUpdateOrder.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridUpdateOrder.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridUpdateOrder.DataSource = this.listControlBindingSource1;
            this.gridUpdateOrder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridUpdateOrder.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridUpdateOrder.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridUpdateOrder.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridUpdateOrder.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridUpdateOrder.Location = new System.Drawing.Point(0, 0);
            this.gridUpdateOrder.Name = "gridUpdateOrder";
            this.gridUpdateOrder.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridUpdateOrder.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridUpdateOrder.RowTemplate.Height = 24;
            this.gridUpdateOrder.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridUpdateOrder.ShowCellToolTips = false;
            this.gridUpdateOrder.Size = new System.Drawing.Size(906, 404);
            this.gridUpdateOrder.TabIndex = 0;
            this.gridUpdateOrder.TabStop = false;
            // 
            // P02
            // 
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(916, 490);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "dateUpdatedDate";
            this.DefaultControlForEdit = "dateUpdatedDate";
            this.EditMode = true;
            this.Name = "P02";
            this.Text = "P02. Comparison List for updated order";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.panel5, 0);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridUpdateOrder)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.Label labelFactory;
        private Win.UI.DateBox dateUpdatedDate;
        private Win.UI.Label labelUpdatedDate;
        private Win.UI.DateBox dateLastDate;
        private Win.UI.Label labelLastDate;
        private Win.UI.Panel panel4;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnExcel;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridUpdateOrder;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private System.Windows.Forms.Button btnQuery;
        private Class.Txtbrand txtbrand1;
        private Win.UI.Label labBrand;
    }
}
