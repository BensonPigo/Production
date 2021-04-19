namespace Sci.Production.Warehouse
{
    partial class P01_BatchCloseRowMaterial
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
            this.btnBatchCloseRMTL = new Sci.Win.UI.Button();
            this.btnFindNow = new Sci.Win.UI.Button();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.btnToEexcel = new Sci.Win.UI.Button();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.txtstyle = new Sci.Production.Class.Txtstyle();
            this.txtmfactory = new Sci.Production.Class.Txtfactory();
            this.labelFactory = new Sci.Win.UI.Label();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.labelPullOutDate = new Sci.Win.UI.Label();
            this.datePullOutDate = new Sci.Win.UI.DateRange();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.comboCategory = new Sci.Win.UI.ComboBox();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridBatchCloseRowMaterial = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchCloseRowMaterial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(912, 17);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnBatchCloseRMTL
            // 
            this.btnBatchCloseRMTL.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBatchCloseRMTL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnBatchCloseRMTL.Location = new System.Drawing.Point(746, 17);
            this.btnBatchCloseRMTL.Name = "btnBatchCloseRMTL";
            this.btnBatchCloseRMTL.Size = new System.Drawing.Size(160, 30);
            this.btnBatchCloseRMTL.TabIndex = 4;
            this.btnBatchCloseRMTL.Text = "Batch Close R/MTL";
            this.btnBatchCloseRMTL.UseVisualStyleBackColor = true;
            this.btnBatchCloseRMTL.Click += new System.EventHandler(this.BtnBatchCloseRMTL_Click);
            // 
            // btnFindNow
            // 
            this.btnFindNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFindNow.Location = new System.Drawing.Point(895, 14);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(101, 30);
            this.btnFindNow.TabIndex = 8;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.BtnFindNow_Click);
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(124, 90);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(122, 23);
            this.txtSPNoStart.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnToEexcel);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnBatchCloseRMTL);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 477);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1008, 53);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            // 
            // btnToEexcel
            // 
            this.btnToEexcel.Location = new System.Drawing.Point(650, 17);
            this.btnToEexcel.Name = "btnToEexcel";
            this.btnToEexcel.Size = new System.Drawing.Size(90, 30);
            this.btnToEexcel.TabIndex = 3;
            this.btnToEexcel.Text = "To Excel";
            this.btnToEexcel.UseVisualStyleBackColor = true;
            this.btnToEexcel.Click += new System.EventHandler(this.BtnToEexcel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtstyle);
            this.groupBox1.Controls.Add(this.txtmfactory);
            this.groupBox1.Controls.Add(this.labelFactory);
            this.groupBox1.Controls.Add(this.txtbrand);
            this.groupBox1.Controls.Add(this.labelBrand);
            this.groupBox1.Controls.Add(this.labelStyle);
            this.groupBox1.Controls.Add(this.labelPullOutDate);
            this.groupBox1.Controls.Add(this.datePullOutDate);
            this.groupBox1.Controls.Add(this.dateBuyerDelivery);
            this.groupBox1.Controls.Add(this.labelBuyerDelivery);
            this.groupBox1.Controls.Add(this.labelSPNo);
            this.groupBox1.Controls.Add(this.labelCategory);
            this.groupBox1.Controls.Add(this.comboCategory);
            this.groupBox1.Controls.Add(this.txtSPNoEnd);
            this.groupBox1.Controls.Add(this.btnFindNow);
            this.groupBox1.Controls.Add(this.txtSPNoStart);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1008, 135);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // txtstyle
            // 
            this.txtstyle.BackColor = System.Drawing.Color.White;
            this.txtstyle.BrandObjectName = null;
            this.txtstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle.Location = new System.Drawing.Point(535, 54);
            this.txtstyle.Name = "txtstyle";
            this.txtstyle.Size = new System.Drawing.Size(130, 23);
            this.txtstyle.TabIndex = 5;
            this.txtstyle.TarBrand = null;
            this.txtstyle.TarSeason = null;
            // 
            // txtmfactory
            // 
            this.txtmfactory.BackColor = System.Drawing.Color.White;
            this.txtmfactory.BoolFtyGroupList = true;
            this.txtmfactory.FilteMDivision = true;
            this.txtmfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtmfactory.IsProduceFty = false;
            this.txtmfactory.IssupportJunk = false;
            this.txtmfactory.Location = new System.Drawing.Point(799, 18);
            this.txtmfactory.MDivision = null;
            this.txtmfactory.Name = "txtmfactory";
            this.txtmfactory.Size = new System.Drawing.Size(66, 23);
            this.txtmfactory.TabIndex = 7;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(684, 19);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(112, 23);
            this.labelFactory.TabIndex = 106;
            this.labelFactory.Text = "Factory";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(535, 90);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(66, 23);
            this.txtbrand.TabIndex = 6;
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(420, 91);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(112, 23);
            this.labelBrand.TabIndex = 104;
            this.labelBrand.Text = "Brand";
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(420, 54);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(112, 23);
            this.labelStyle.TabIndex = 103;
            this.labelStyle.Text = "Style";
            // 
            // labelPullOutDate
            // 
            this.labelPullOutDate.Location = new System.Drawing.Point(9, 19);
            this.labelPullOutDate.Name = "labelPullOutDate";
            this.labelPullOutDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelPullOutDate.Size = new System.Drawing.Size(112, 23);
            this.labelPullOutDate.TabIndex = 102;
            this.labelPullOutDate.Text = "PullOut Date";
            this.labelPullOutDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // datePullOutDate
            // 
            // 
            // 
            // 
            this.datePullOutDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.datePullOutDate.DateBox1.Name = "";
            this.datePullOutDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.datePullOutDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.datePullOutDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.datePullOutDate.DateBox2.Name = "";
            this.datePullOutDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.datePullOutDate.DateBox2.TabIndex = 1;
            this.datePullOutDate.Location = new System.Drawing.Point(124, 19);
            this.datePullOutDate.Name = "datePullOutDate";
            this.datePullOutDate.Size = new System.Drawing.Size(280, 23);
            this.datePullOutDate.TabIndex = 0;
            // 
            // dateBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerDelivery.DateBox1.Name = "";
            this.dateBuyerDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBuyerDelivery.DateBox2.Name = "";
            this.dateBuyerDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(124, 54);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 1;
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Location = new System.Drawing.Point(9, 54);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelBuyerDelivery.Size = new System.Drawing.Size(112, 23);
            this.labelBuyerDelivery.TabIndex = 99;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            this.labelBuyerDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(9, 90);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSPNo.Size = new System.Drawing.Size(112, 23);
            this.labelSPNo.TabIndex = 97;
            this.labelSPNo.Text = "SP#";
            this.labelSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(420, 18);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(112, 23);
            this.labelCategory.TabIndex = 12;
            this.labelCategory.Text = "Category";
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Items.AddRange(new object[] {
            "ALL",
            "Bulk",
            "Sample",
            "Material"});
            this.comboCategory.Location = new System.Drawing.Point(535, 18);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(121, 24);
            this.comboCategory.TabIndex = 4;
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(252, 90);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(116, 23);
            this.txtSPNoEnd.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridBatchCloseRowMaterial);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 135);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 342);
            this.panel1.TabIndex = 20;
            // 
            // gridBatchCloseRowMaterial
            // 
            this.gridBatchCloseRowMaterial.AllowUserToAddRows = false;
            this.gridBatchCloseRowMaterial.AllowUserToDeleteRows = false;
            this.gridBatchCloseRowMaterial.AllowUserToResizeRows = false;
            this.gridBatchCloseRowMaterial.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBatchCloseRowMaterial.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridBatchCloseRowMaterial.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBatchCloseRowMaterial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBatchCloseRowMaterial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridBatchCloseRowMaterial.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBatchCloseRowMaterial.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBatchCloseRowMaterial.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBatchCloseRowMaterial.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBatchCloseRowMaterial.Location = new System.Drawing.Point(0, 0);
            this.gridBatchCloseRowMaterial.Name = "gridBatchCloseRowMaterial";
            this.gridBatchCloseRowMaterial.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridBatchCloseRowMaterial.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridBatchCloseRowMaterial.RowTemplate.Height = 24;
            this.gridBatchCloseRowMaterial.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBatchCloseRowMaterial.ShowCellToolTips = false;
            this.gridBatchCloseRowMaterial.Size = new System.Drawing.Size(1008, 342);
            this.gridBatchCloseRowMaterial.TabIndex = 0;
            this.gridBatchCloseRowMaterial.TabStop = false;
            this.gridBatchCloseRowMaterial.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridBatchCloseRowMaterial_ColumnHeaderMouseClick);
            // 
            // P01_BatchCloseRowMaterial
            // 
            this.ClientSize = new System.Drawing.Size(1008, 530);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "P01_BatchCloseRowMaterial";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P01. Batch Close Row Material";
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchCloseRowMaterial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnCancel;
        private Win.UI.Button btnBatchCloseRMTL;
        private Win.UI.Button btnFindNow;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Panel panel1;
        private Win.UI.Grid gridBatchCloseRowMaterial;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.Label labelCategory;
        private Win.UI.ComboBox comboCategory;
        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.Label labelSPNo;
        private Class.Txtstyle txtstyle;
        private Class.Txtfactory txtmfactory;
        private Win.UI.Label labelFactory;
        private Class.Txtbrand txtbrand;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelStyle;
        private Win.UI.Label labelPullOutDate;
        private Win.UI.DateRange datePullOutDate;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.Button btnToEexcel;
    }
}
