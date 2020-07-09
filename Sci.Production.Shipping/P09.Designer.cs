namespace Sci.Production.Shipping
{
    partial class P09
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
            this.btnQuery = new Sci.Win.UI.Button();
            this.comboDataType = new Sci.Win.UI.ComboBox();
            this.labelDataType = new Sci.Win.UI.Label();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.labelM = new Sci.Win.UI.Label();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.labelBrand = new Sci.Win.UI.Label();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridDelayShipmentOrderDetail = new Sci.Win.UI.Grid();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDelayShipmentOrderDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 492);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(957, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 492);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.comboDataType);
            this.panel3.Controls.Add(this.labelDataType);
            this.panel3.Controls.Add(this.comboM);
            this.panel3.Controls.Add(this.labelM);
            this.panel3.Controls.Add(this.txtbrand);
            this.panel3.Controls.Add(this.labelBrand);
            this.panel3.Controls.Add(this.dateBuyerDelivery);
            this.panel3.Controls.Add(this.labelBuyerDelivery);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(952, 45);
            this.panel3.TabIndex = 3;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(868, 8);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 8;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // comboDataType
            // 
            this.comboDataType.BackColor = System.Drawing.Color.White;
            this.comboDataType.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboDataType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDataType.FormattingEnabled = true;
            this.comboDataType.IsSupportUnselect = true;
            this.comboDataType.Location = new System.Drawing.Point(739, 11);
            this.comboDataType.Name = "comboDataType";
            this.comboDataType.Size = new System.Drawing.Size(103, 24);
            this.comboDataType.TabIndex = 7;
            // 
            // labelDataType
            // 
            this.labelDataType.Lines = 0;
            this.labelDataType.Location = new System.Drawing.Point(667, 11);
            this.labelDataType.Name = "labelDataType";
            this.labelDataType.Size = new System.Drawing.Size(68, 23);
            this.labelDataType.TabIndex = 6;
            this.labelDataType.Text = "Data Type";
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(561, 11);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(74, 24);
            this.comboM.TabIndex = 5;
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(537, 12);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(20, 23);
            this.labelM.TabIndex = 4;
            this.labelM.Text = "M";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.IsSupportEditMode = false;
            this.txtbrand.Location = new System.Drawing.Point(427, 12);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(88, 23);
            this.txtbrand.TabIndex = 3;
            // 
            // labelBrand
            // 
            this.labelBrand.Lines = 0;
            this.labelBrand.Location = new System.Drawing.Point(377, 12);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(46, 23);
            this.labelBrand.TabIndex = 2;
            this.labelBrand.Text = "Brand";
            // 
            // dateBuyerDelivery
            // 
            this.dateBuyerDelivery.IsRequired = false;
            this.dateBuyerDelivery.IsSupportEditMode = false;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(102, 12);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(253, 23);
            this.dateBuyerDelivery.TabIndex = 1;
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Lines = 0;
            this.labelBuyerDelivery.Location = new System.Drawing.Point(4, 12);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(94, 23);
            this.labelBuyerDelivery.TabIndex = 0;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Controls.Add(this.btnToExcel);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 446);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(952, 46);
            this.panel4.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(868, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToExcel.Location = new System.Drawing.Point(782, 7);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(80, 30);
            this.btnToExcel.TabIndex = 0;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridDelayShipmentOrderDetail);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 45);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(952, 401);
            this.panel5.TabIndex = 5;
            // 
            // gridDelayShipmentOrderDetail
            // 
            this.gridDelayShipmentOrderDetail.AllowUserToAddRows = false;
            this.gridDelayShipmentOrderDetail.AllowUserToDeleteRows = false;
            this.gridDelayShipmentOrderDetail.AllowUserToResizeRows = false;
            this.gridDelayShipmentOrderDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDelayShipmentOrderDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDelayShipmentOrderDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDelayShipmentOrderDetail.DataSource = this.listControlBindingSource1;
            this.gridDelayShipmentOrderDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDelayShipmentOrderDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDelayShipmentOrderDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDelayShipmentOrderDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDelayShipmentOrderDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDelayShipmentOrderDetail.Location = new System.Drawing.Point(0, 0);
            this.gridDelayShipmentOrderDetail.Name = "gridDelayShipmentOrderDetail";
            this.gridDelayShipmentOrderDetail.RowHeadersVisible = false;
            this.gridDelayShipmentOrderDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDelayShipmentOrderDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDelayShipmentOrderDetail.RowTemplate.Height = 24;
            this.gridDelayShipmentOrderDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDelayShipmentOrderDetail.Size = new System.Drawing.Size(952, 401);
            this.gridDelayShipmentOrderDetail.TabIndex = 0;
            this.gridDelayShipmentOrderDetail.TabStop = false;
            // 
            // P09
            // 
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(962, 492);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.EditMode = true;
            this.Name = "P09";
            this.Text = "P09. Delay shipment order detail";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.panel5, 0);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDelayShipmentOrderDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridDelayShipmentOrderDetail;
        private Win.UI.Label labelM;
        private Class.Txtbrand txtbrand;
        private Win.UI.Label labelBrand;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.Button btnQuery;
        private Win.UI.ComboBox comboDataType;
        private Win.UI.Label labelDataType;
        private Win.UI.ComboBox comboM;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnToExcel;
    }
}
