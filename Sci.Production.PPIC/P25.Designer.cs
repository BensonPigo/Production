namespace Sci.Production.PPIC
{
    partial class P25
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
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.grid = new Sci.Win.UI.Grid();
            this.panel3 = new Sci.Win.UI.Panel();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtHandle = new Sci.Production.Class.Txtuser();
            this.txtSMR = new Sci.Production.Class.Txtuser();
            this.labFactory = new Sci.Win.UI.Label();
            this.labHandle = new Sci.Win.UI.Label();
            this.labSMR = new Sci.Win.UI.Label();
            this.txtstyle = new Sci.Production.Class.Txtstyle();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.labStyle = new Sci.Win.UI.Label();
            this.labSciDel = new Sci.Win.UI.Label();
            this.labBrand = new Sci.Win.UI.Label();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.btnFindNow = new Sci.Win.UI.Button();
            this.dateSCIDel = new Sci.Win.UI.DateRange();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 596);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1005, 47);
            this.panel4.TabIndex = 7;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(913, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(0, 106);
            this.grid.Name = "grid";
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(1005, 537);
            this.grid.TabIndex = 0;
            this.grid.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtfactory);
            this.panel3.Controls.Add(this.txtHandle);
            this.panel3.Controls.Add(this.txtSMR);
            this.panel3.Controls.Add(this.labFactory);
            this.panel3.Controls.Add(this.labHandle);
            this.panel3.Controls.Add(this.labSMR);
            this.panel3.Controls.Add(this.txtstyle);
            this.panel3.Controls.Add(this.labStyle);
            this.panel3.Controls.Add(this.labSciDel);
            this.panel3.Controls.Add(this.txtbrand);
            this.panel3.Controls.Add(this.labBrand);
            this.panel3.Controls.Add(this.btnToExcel);
            this.panel3.Controls.Add(this.btnFindNow);
            this.panel3.Controls.Add(this.dateSCIDel);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1005, 106);
            this.panel3.TabIndex = 6;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(518, 64);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(79, 23);
            this.txtfactory.TabIndex = 5;
            // 
            // txtHandle
            // 
            this.txtHandle.DisplayBox1Binding = "";
            this.txtHandle.Location = new System.Drawing.Point(518, 36);
            this.txtHandle.Name = "txtHandle";
            this.txtHandle.Size = new System.Drawing.Size(300, 23);
            this.txtHandle.TabIndex = 4;
            this.txtHandle.TextBox1Binding = "";
            // 
            // txtSMR
            // 
            this.txtSMR.DisplayBox1Binding = "";
            this.txtSMR.Location = new System.Drawing.Point(518, 8);
            this.txtSMR.Name = "txtSMR";
            this.txtSMR.Size = new System.Drawing.Size(300, 23);
            this.txtSMR.TabIndex = 3;
            this.txtSMR.TextBox1Binding = "";
            // 
            // labFactory
            // 
            this.labFactory.Location = new System.Drawing.Point(432, 64);
            this.labFactory.Name = "labFactory";
            this.labFactory.Size = new System.Drawing.Size(83, 23);
            this.labFactory.TabIndex = 13;
            this.labFactory.Text = "Factory";
            // 
            // labHandle
            // 
            this.labHandle.Location = new System.Drawing.Point(432, 36);
            this.labHandle.Name = "labHandle";
            this.labHandle.Size = new System.Drawing.Size(83, 23);
            this.labHandle.TabIndex = 12;
            this.labHandle.Text = "Handle";
            // 
            // labSMR
            // 
            this.labSMR.Location = new System.Drawing.Point(432, 8);
            this.labSMR.Name = "labSMR";
            this.labSMR.Size = new System.Drawing.Size(83, 23);
            this.labSMR.TabIndex = 11;
            this.labSMR.Text = "SMR";
            // 
            // txtstyle
            // 
            this.txtstyle.BackColor = System.Drawing.Color.White;
            this.txtstyle.BrandObjectName = null;
            this.txtstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle.Location = new System.Drawing.Point(95, 64);
            this.txtstyle.Name = "txtstyle";
            this.txtstyle.Size = new System.Drawing.Size(130, 23);
            this.txtstyle.TabIndex = 2;
            this.txtstyle.TarBrand = this.txtbrand;
            this.txtstyle.TarSeason = null;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(95, 9);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(130, 23);
            this.txtbrand.TabIndex = 0;
            // 
            // labStyle
            // 
            this.labStyle.Location = new System.Drawing.Point(9, 64);
            this.labStyle.Name = "labStyle";
            this.labStyle.Size = new System.Drawing.Size(83, 23);
            this.labStyle.TabIndex = 10;
            this.labStyle.Text = "Style#";
            // 
            // labSciDel
            // 
            this.labSciDel.Location = new System.Drawing.Point(9, 36);
            this.labSciDel.Name = "labSciDel";
            this.labSciDel.Size = new System.Drawing.Size(83, 23);
            this.labSciDel.TabIndex = 9;
            this.labSciDel.Text = "SCI Del";
            // 
            // labBrand
            // 
            this.labBrand.Location = new System.Drawing.Point(9, 8);
            this.labBrand.Name = "labBrand";
            this.labBrand.Size = new System.Drawing.Size(83, 23);
            this.labBrand.TabIndex = 8;
            this.labBrand.Text = "Brand";
            // 
            // btnToExcel
            // 
            this.btnToExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToExcel.Location = new System.Drawing.Point(888, 65);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(105, 30);
            this.btnToExcel.TabIndex = 7;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // btnFindNow
            // 
            this.btnFindNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNow.Location = new System.Drawing.Point(888, 28);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(105, 30);
            this.btnFindNow.TabIndex = 6;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.btnFindNow_Click);
            // 
            // dateSCIDel
            // 
            // 
            // 
            // 
            this.dateSCIDel.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSCIDel.DateBox1.Name = "";
            this.dateSCIDel.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDel.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSCIDel.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSCIDel.DateBox2.Name = "";
            this.dateSCIDel.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDel.DateBox2.TabIndex = 1;
            this.dateSCIDel.Location = new System.Drawing.Point(95, 36);
            this.dateSCIDel.Name = "dateSCIDel";
            this.dateSCIDel.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDel.TabIndex = 1;
            // 
            // P25
            // 
            this.ClientSize = new System.Drawing.Size(1005, 643);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.panel3);
            this.DefaultControl = "txtbrand";
            this.Name = "P25";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P25. Order Used Pad Print List";
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.grid, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel4;
        private Win.UI.Button btnClose;
        private Win.UI.Grid grid;
        private Win.UI.Panel panel3;
        private Win.UI.Button btnToExcel;
        private Win.UI.Button btnFindNow;
        private Win.UI.DateRange dateSCIDel;
        private Win.UI.Label labFactory;
        private Win.UI.Label labHandle;
        private Win.UI.Label labSMR;
        private Class.Txtstyle txtstyle;
        private Win.UI.Label labStyle;
        private Win.UI.Label labSciDel;
        private Class.Txtbrand txtbrand;
        private Win.UI.Label labBrand;
        private Class.Txtfactory txtfactory;
        private Class.Txtuser txtHandle;
        private Class.Txtuser txtSMR;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}
