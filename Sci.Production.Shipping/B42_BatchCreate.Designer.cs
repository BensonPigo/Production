namespace Sci.Production.Shipping
{
    partial class B42_BatchCreate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(B42_BatchCreate));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.btnQuery = new Sci.Win.UI.Button();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.labelBrand = new Sci.Win.UI.Label();
            this.comboCategory = new Sci.Win.UI.ComboBox();
            this.labelCategory = new Sci.Win.UI.Label();
            this.txtstyle = new Sci.Production.Class.Txtstyle();
            this.labelStyle = new Sci.Win.UI.Label();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnEmptyNLCodetoExcel = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnCreate = new Sci.Win.UI.Button();
            this.pictureBox2 = new Sci.Win.UI.PictureBox();
            this.dateCdate = new Sci.Win.UI.DateBox();
            this.pictureBox1 = new Sci.Win.UI.PictureBox();
            this.txtVNContractID = new Sci.Win.UI.TextBox();
            this.btnAutoCustomSPNo = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridBatchCreate = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchCreate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 428);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(928, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 428);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.txtbrand);
            this.panel3.Controls.Add(this.labelBrand);
            this.panel3.Controls.Add(this.comboCategory);
            this.panel3.Controls.Add(this.labelCategory);
            this.panel3.Controls.Add(this.txtstyle);
            this.panel3.Controls.Add(this.labelStyle);
            this.panel3.Controls.Add(this.dateBuyerDelivery);
            this.panel3.Controls.Add(this.labelBuyerDelivery);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(918, 44);
            this.panel3.TabIndex = 2;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(827, 6);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 8;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(738, 10);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(82, 23);
            this.txtbrand.TabIndex = 7;
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(692, 10);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(42, 23);
            this.labelBrand.TabIndex = 6;
            this.labelBrand.Text = "Brand";
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(601, 9);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(74, 24);
            this.comboCategory.TabIndex = 5;
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(537, 10);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(60, 23);
            this.labelCategory.TabIndex = 4;
            this.labelCategory.Text = "Category";
            // 
            // txtstyle
            // 
            this.txtstyle.BackColor = System.Drawing.Color.White;
            this.txtstyle.BrandObjectName = null;
            this.txtstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle.Location = new System.Drawing.Point(390, 10);
            this.txtstyle.Name = "txtstyle";
            this.txtstyle.Size = new System.Drawing.Size(130, 23);
            this.txtstyle.TabIndex = 3;
            this.txtstyle.TarBrand = null;
            this.txtstyle.TarSeason = null;
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(349, 10);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(37, 23);
            this.labelStyle.TabIndex = 2;
            this.labelStyle.Text = "Style";
            // 
            // dateBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerDelivery.DateBox1.Name = "";
            this.dateBuyerDelivery.DateBox1.Size = new System.Drawing.Size(103, 23);
            this.dateBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox2.Location = new System.Drawing.Point(125, 0);
            this.dateBuyerDelivery.DateBox2.Name = "";
            this.dateBuyerDelivery.DateBox2.Size = new System.Drawing.Size(103, 23);
            this.dateBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateBuyerDelivery.IsRequired = false;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(102, 10);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(228, 23);
            this.dateBuyerDelivery.TabIndex = 1;
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Location = new System.Drawing.Point(4, 10);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(94, 23);
            this.labelBuyerDelivery.TabIndex = 0;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnEmptyNLCodetoExcel);
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Controls.Add(this.btnCreate);
            this.panel4.Controls.Add(this.pictureBox2);
            this.panel4.Controls.Add(this.dateCdate);
            this.panel4.Controls.Add(this.pictureBox1);
            this.panel4.Controls.Add(this.txtVNContractID);
            this.panel4.Controls.Add(this.btnAutoCustomSPNo);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(10, 387);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(918, 41);
            this.panel4.TabIndex = 3;
            // 
            // btnEmptyNLCodetoExcel
            // 
            this.btnEmptyNLCodetoExcel.Location = new System.Drawing.Point(662, 7);
            this.btnEmptyNLCodetoExcel.Name = "btnEmptyNLCodetoExcel";
            this.btnEmptyNLCodetoExcel.Size = new System.Drawing.Size(254, 30);
            this.btnEmptyNLCodetoExcel.TabIndex = 7;
            this.btnEmptyNLCodetoExcel.Text = "Empty Customs Code (to Excel)";
            this.btnEmptyNLCodetoExcel.UseVisualStyleBackColor = true;
            this.btnEmptyNLCodetoExcel.Click += new System.EventHandler(this.BtnEmptyNLCodetoExcel_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(576, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(490, 7);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(80, 30);
            this.btnCreate.TabIndex = 5;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(446, 7);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(22, 30);
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.WaitOnLoad = true;
            this.pictureBox2.Click += new System.EventHandler(this.PictureBox2_Click);
            // 
            // dateCdate
            // 
            this.dateCdate.Location = new System.Drawing.Point(341, 7);
            this.dateCdate.Name = "dateCdate";
            this.dateCdate.Size = new System.Drawing.Size(98, 23);
            this.dateCdate.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(310, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(24, 30);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.WaitOnLoad = true;
            this.pictureBox1.Click += new System.EventHandler(this.PictureBox1_Click);
            // 
            // txtVNContractID
            // 
            this.txtVNContractID.BackColor = System.Drawing.Color.White;
            this.txtVNContractID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtVNContractID.Location = new System.Drawing.Point(163, 7);
            this.txtVNContractID.Name = "txtVNContractID";
            this.txtVNContractID.Size = new System.Drawing.Size(140, 23);
            this.txtVNContractID.TabIndex = 1;
            this.txtVNContractID.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtVNContractID_PopUp);
            this.txtVNContractID.Validating += new System.ComponentModel.CancelEventHandler(this.TxtVNContractID_Validating);
            // 
            // btnAutoCustomSPNo
            // 
            this.btnAutoCustomSPNo.Location = new System.Drawing.Point(6, 5);
            this.btnAutoCustomSPNo.Name = "btnAutoCustomSPNo";
            this.btnAutoCustomSPNo.Size = new System.Drawing.Size(152, 30);
            this.btnAutoCustomSPNo.TabIndex = 0;
            this.btnAutoCustomSPNo.Text = "Auto Custom SP#";
            this.btnAutoCustomSPNo.UseVisualStyleBackColor = true;
            this.btnAutoCustomSPNo.Click += new System.EventHandler(this.BtnAutoCustomSPNo_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridBatchCreate);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 44);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(918, 343);
            this.panel5.TabIndex = 4;
            // 
            // gridBatchCreate
            // 
            this.gridBatchCreate.AllowUserToAddRows = false;
            this.gridBatchCreate.AllowUserToDeleteRows = false;
            this.gridBatchCreate.AllowUserToResizeRows = false;
            this.gridBatchCreate.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBatchCreate.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBatchCreate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBatchCreate.DataSource = this.listControlBindingSource1;
            this.gridBatchCreate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridBatchCreate.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBatchCreate.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBatchCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBatchCreate.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBatchCreate.Location = new System.Drawing.Point(0, 0);
            this.gridBatchCreate.Name = "gridBatchCreate";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridBatchCreate.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridBatchCreate.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridBatchCreate.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridBatchCreate.RowTemplate.Height = 24;
            this.gridBatchCreate.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBatchCreate.ShowCellToolTips = false;
            this.gridBatchCreate.Size = new System.Drawing.Size(918, 343);
            this.gridBatchCreate.TabIndex = 0;
            this.gridBatchCreate.TabStop = false;
            // 
            // B42_BatchCreate
            // 
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(938, 428);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "B42_BatchCreate";
            this.Text = "Batch Create";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchCreate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridBatchCreate;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Button btnQuery;
        private Class.Txtbrand txtbrand;
        private Win.UI.Label labelBrand;
        private Win.UI.ComboBox comboCategory;
        private Win.UI.Label labelCategory;
        private Class.Txtstyle txtstyle;
        private Win.UI.Label labelStyle;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.PictureBox pictureBox1;
        private Win.UI.TextBox txtVNContractID;
        private Win.UI.Button btnAutoCustomSPNo;
        private Win.UI.Button btnEmptyNLCodetoExcel;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnCreate;
        private Win.UI.PictureBox pictureBox2;
        private Win.UI.DateBox dateCdate;
    }
}
