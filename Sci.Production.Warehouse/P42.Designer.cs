namespace Sci.Production.Warehouse
{
    partial class P42
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(P42));
            this.panel1 = new Sci.Win.UI.Panel();
            this.btnFind = new Sci.Win.UI.Button();
            this.btnQuery = new Sci.Win.UI.Button();
            this.txtLocateForSP = new Sci.Win.UI.TextBox();
            this.labelLocateForSP = new Sci.Win.UI.Label();
            this.checkEmptyMtlETA = new Sci.Win.UI.CheckBox();
            this.checkEmptyEachCons = new Sci.Win.UI.CheckBox();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.dateSewingInline = new Sci.Win.UI.DateRange();
            this.labelSewingInline = new Sci.Win.UI.Label();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.panel2 = new Sci.Win.UI.Panel();
            this.gridCuttingTapeQuickAdjust = new Sci.Win.UI.Grid();
            this.panel3 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.pictureBox2 = new Sci.Win.UI.PictureBox();
            this.pictureBox1 = new Sci.Win.UI.PictureBox();
            this.dateOffline = new Sci.Win.UI.DateBox();
            this.dateInline = new Sci.Win.UI.DateBox();
            this.labelOffline = new Sci.Win.UI.Label();
            this.labelInline = new Sci.Win.UI.Label();
            this.labelCheckedQty = new Sci.Win.UI.Label();
            this.displayCheckedQty = new Sci.Win.UI.DisplayBox();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCuttingTapeQuickAdjust)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnFind);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.txtLocateForSP);
            this.panel1.Controls.Add(this.labelLocateForSP);
            this.panel1.Controls.Add(this.checkEmptyMtlETA);
            this.panel1.Controls.Add(this.checkEmptyEachCons);
            this.panel1.Controls.Add(this.dateBuyerDelivery);
            this.panel1.Controls.Add(this.labelBuyerDelivery);
            this.panel1.Controls.Add(this.dateSewingInline);
            this.panel1.Controls.Add(this.labelSewingInline);
            this.panel1.Controls.Add(this.dateSCIDelivery);
            this.panel1.Controls.Add(this.labelSCIDelivery);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 106);
            this.panel1.TabIndex = 0;
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(272, 70);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(98, 30);
            this.btnFind.TabIndex = 7;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(867, 5);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 5;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // txtLocateForSP
            // 
            this.txtLocateForSP.BackColor = System.Drawing.Color.White;
            this.txtLocateForSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocateForSP.Location = new System.Drawing.Point(121, 74);
            this.txtLocateForSP.Name = "txtLocateForSP";
            this.txtLocateForSP.Size = new System.Drawing.Size(145, 23);
            this.txtLocateForSP.TabIndex = 6;
            // 
            // labelLocateForSP
            // 
            this.labelLocateForSP.Location = new System.Drawing.Point(9, 74);
            this.labelLocateForSP.Name = "labelLocateForSP";
            this.labelLocateForSP.Size = new System.Drawing.Size(109, 23);
            this.labelLocateForSP.TabIndex = 16;
            this.labelLocateForSP.Text = "Locate for SP#";
            // 
            // checkEmptyMtlETA
            // 
            this.checkEmptyMtlETA.AutoSize = true;
            this.checkEmptyMtlETA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkEmptyMtlETA.Location = new System.Drawing.Point(643, 40);
            this.checkEmptyMtlETA.Name = "checkEmptyMtlETA";
            this.checkEmptyMtlETA.Size = new System.Drawing.Size(157, 21);
            this.checkEmptyMtlETA.TabIndex = 4;
            this.checkEmptyMtlETA.Text = "Filter empty Mtl. ETA";
            this.checkEmptyMtlETA.UseVisualStyleBackColor = true;
            this.checkEmptyMtlETA.CheckedChanged += new System.EventHandler(this.CheckEmptyMtlETA_CheckedChanged);
            // 
            // checkEmptyEachCons
            // 
            this.checkEmptyEachCons.AutoSize = true;
            this.checkEmptyEachCons.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkEmptyEachCons.Location = new System.Drawing.Point(423, 38);
            this.checkEmptyEachCons.Name = "checkEmptyEachCons";
            this.checkEmptyEachCons.Size = new System.Drawing.Size(176, 21);
            this.checkEmptyEachCons.TabIndex = 3;
            this.checkEmptyEachCons.Text = "Filter empty Each Cons.";
            this.checkEmptyEachCons.UseVisualStyleBackColor = true;
            this.checkEmptyEachCons.CheckedChanged += new System.EventHandler(this.CheckEmptyEachCons_CheckedChanged);
            // 
            // dateBuyerDelivery
            // 
            this.dateBuyerDelivery.Location = new System.Drawing.Point(122, 40);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 1;
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Location = new System.Drawing.Point(9, 40);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(109, 23);
            this.labelBuyerDelivery.TabIndex = 13;
            this.labelBuyerDelivery.Text = "Buyer  Delivery";
            // 
            // dateSewingInline
            // 
            this.dateSewingInline.Location = new System.Drawing.Point(540, 9);
            this.dateSewingInline.Name = "dateSewingInline";
            this.dateSewingInline.Size = new System.Drawing.Size(280, 23);
            this.dateSewingInline.TabIndex = 2;
            // 
            // labelSewingInline
            // 
            this.labelSewingInline.Location = new System.Drawing.Point(423, 9);
            this.labelSewingInline.Name = "labelSewingInline";
            this.labelSewingInline.Size = new System.Drawing.Size(114, 23);
            this.labelSewingInline.TabIndex = 11;
            this.labelSewingInline.Text = "1st Sewing Inline";
            // 
            // dateSCIDelivery
            // 
            this.dateSCIDelivery.Location = new System.Drawing.Point(122, 9);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 0;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(9, 9);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(109, 23);
            this.labelSCIDelivery.TabIndex = 9;
            this.labelSCIDelivery.Text = "SCI  Delivery";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridCuttingTapeQuickAdjust);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 106);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 555);
            this.panel2.TabIndex = 1;
            // 
            // gridCuttingTapeQuickAdjust
            // 
            this.gridCuttingTapeQuickAdjust.AllowUserToAddRows = false;
            this.gridCuttingTapeQuickAdjust.AllowUserToDeleteRows = false;
            this.gridCuttingTapeQuickAdjust.AllowUserToResizeRows = false;
            this.gridCuttingTapeQuickAdjust.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridCuttingTapeQuickAdjust.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridCuttingTapeQuickAdjust.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridCuttingTapeQuickAdjust.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCuttingTapeQuickAdjust.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridCuttingTapeQuickAdjust.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridCuttingTapeQuickAdjust.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridCuttingTapeQuickAdjust.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridCuttingTapeQuickAdjust.Location = new System.Drawing.Point(3, 3);
            this.gridCuttingTapeQuickAdjust.Name = "gridCuttingTapeQuickAdjust";
            this.gridCuttingTapeQuickAdjust.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridCuttingTapeQuickAdjust.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridCuttingTapeQuickAdjust.RowTemplate.Height = 24;
            this.gridCuttingTapeQuickAdjust.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCuttingTapeQuickAdjust.Size = new System.Drawing.Size(1002, 486);
            this.gridCuttingTapeQuickAdjust.TabIndex = 0;
            this.gridCuttingTapeQuickAdjust.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.pictureBox2);
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Controls.Add(this.dateOffline);
            this.panel3.Controls.Add(this.dateInline);
            this.panel3.Controls.Add(this.labelOffline);
            this.panel3.Controls.Add(this.labelInline);
            this.panel3.Controls.Add(this.labelCheckedQty);
            this.panel3.Controls.Add(this.displayCheckedQty);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 601);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1008, 60);
            this.panel3.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(922, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(842, 15);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(791, 11);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(25, 31);
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.WaitOnLoad = true;
            this.pictureBox2.Click += new System.EventHandler(this.PictureBox2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(499, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(25, 31);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.WaitOnLoad = true;
            this.pictureBox1.Click += new System.EventHandler(this.PictureBox1_Click);
            // 
            // dateOffline
            // 
            this.dateOffline.Location = new System.Drawing.Point(655, 19);
            this.dateOffline.Name = "dateOffline";
            this.dateOffline.Size = new System.Drawing.Size(130, 23);
            this.dateOffline.TabIndex = 1;
            // 
            // dateInline
            // 
            this.dateInline.Location = new System.Drawing.Point(363, 19);
            this.dateInline.Name = "dateInline";
            this.dateInline.Size = new System.Drawing.Size(130, 23);
            this.dateInline.TabIndex = 0;
            // 
            // labelOffline
            // 
            this.labelOffline.Location = new System.Drawing.Point(603, 19);
            this.labelOffline.Name = "labelOffline";
            this.labelOffline.Size = new System.Drawing.Size(49, 23);
            this.labelOffline.TabIndex = 3;
            this.labelOffline.Text = "Offline";
            // 
            // labelInline
            // 
            this.labelInline.Location = new System.Drawing.Point(309, 19);
            this.labelInline.Name = "labelInline";
            this.labelInline.Size = new System.Drawing.Size(50, 23);
            this.labelInline.TabIndex = 2;
            this.labelInline.Text = "Inline";
            // 
            // labelCheckedQty
            // 
            this.labelCheckedQty.Location = new System.Drawing.Point(30, 19);
            this.labelCheckedQty.Name = "labelCheckedQty";
            this.labelCheckedQty.Size = new System.Drawing.Size(88, 23);
            this.labelCheckedQty.TabIndex = 1;
            this.labelCheckedQty.Text = "Checked Qty";
            // 
            // displayCheckedQty
            // 
            this.displayCheckedQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCheckedQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCheckedQty.Location = new System.Drawing.Point(121, 19);
            this.displayCheckedQty.Name = "displayCheckedQty";
            this.displayCheckedQty.Size = new System.Drawing.Size(100, 23);
            this.displayCheckedQty.TabIndex = 0;
            // 
            // P42
            // 
            this.ClientSize = new System.Drawing.Size(1008, 661);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "dateSCIDelivery";
            this.DefaultControlForEdit = "dateSCIDelivery";
            this.Name = "P42";
            this.Text = "P42. Cutting Tape Quick Adjust";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCuttingTapeQuickAdjust)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Button btnFind;
        private Win.UI.Button btnQuery;
        private Win.UI.TextBox txtLocateForSP;
        private Win.UI.Label labelLocateForSP;
        private Win.UI.CheckBox checkEmptyMtlETA;
        private Win.UI.CheckBox checkEmptyEachCons;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.DateRange dateSewingInline;
        private Win.UI.Label labelSewingInline;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Grid gridCuttingTapeQuickAdjust;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnSave;
        private Win.UI.PictureBox pictureBox2;
        private Win.UI.PictureBox pictureBox1;
        private Win.UI.DateBox dateOffline;
        private Win.UI.DateBox dateInline;
        private Win.UI.Label labelOffline;
        private Win.UI.Label labelInline;
        private Win.UI.Label labelCheckedQty;
        private Win.UI.DisplayBox displayCheckedQty;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}
