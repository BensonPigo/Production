namespace Sci.Production.Packing
{
    partial class P13
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
            this.btnToExcel = new Sci.Win.UI.Button();
            this.btnQuery = new Sci.Win.UI.Button();
            this.dateEstArrivedDate = new Sci.Win.UI.DateRange();
            this.dateEstBookingDate = new Sci.Win.UI.DateRange();
            this.labelEstArrivedDate = new Sci.Win.UI.Label();
            this.labelSewingInlineDate = new Sci.Win.UI.Label();
            this.dateSewingInlineDate = new Sci.Win.UI.DateRange();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.labelEstBookingDate = new Sci.Win.UI.Label();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 510);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(945, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 510);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnToExcel);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.dateEstArrivedDate);
            this.panel3.Controls.Add(this.dateEstBookingDate);
            this.panel3.Controls.Add(this.labelEstArrivedDate);
            this.panel3.Controls.Add(this.labelSewingInlineDate);
            this.panel3.Controls.Add(this.dateSewingInlineDate);
            this.panel3.Controls.Add(this.dateSCIDelivery);
            this.panel3.Controls.Add(this.labelEstBookingDate);
            this.panel3.Controls.Add(this.labelSCIDelivery);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(940, 73);
            this.panel3.TabIndex = 3;
            // 
            // btnToExcel
            // 
            this.btnToExcel.Location = new System.Drawing.Point(855, 39);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(80, 30);
            this.btnToExcel.TabIndex = 9;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(855, 5);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 8;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // dateEstArrivedDate
            // 
            this.dateEstArrivedDate.IsRequired = false;
            this.dateEstArrivedDate.Location = new System.Drawing.Point(537, 37);
            this.dateEstArrivedDate.Name = "dateEstArrivedDate";
            this.dateEstArrivedDate.Size = new System.Drawing.Size(280, 23);
            this.dateEstArrivedDate.TabIndex = 7;
            // 
            // dateEstBookingDate
            // 
            this.dateEstBookingDate.IsRequired = false;
            this.dateEstBookingDate.Location = new System.Drawing.Point(537, 9);
            this.dateEstBookingDate.Name = "dateEstBookingDate";
            this.dateEstBookingDate.Size = new System.Drawing.Size(280, 23);
            this.dateEstBookingDate.TabIndex = 6;
            // 
            // labelEstArrivedDate
            // 
            this.labelEstArrivedDate.Lines = 0;
            this.labelEstArrivedDate.Location = new System.Drawing.Point(421, 36);
            this.labelEstArrivedDate.Name = "labelEstArrivedDate";
            this.labelEstArrivedDate.Size = new System.Drawing.Size(114, 23);
            this.labelEstArrivedDate.TabIndex = 5;
            this.labelEstArrivedDate.Text = "Est. Arrived Date";
            // 
            // labelSewingInlineDate
            // 
            this.labelSewingInlineDate.Lines = 0;
            this.labelSewingInlineDate.Location = new System.Drawing.Point(2, 37);
            this.labelSewingInlineDate.Name = "labelSewingInlineDate";
            this.labelSewingInlineDate.Size = new System.Drawing.Size(120, 23);
            this.labelSewingInlineDate.TabIndex = 4;
            this.labelSewingInlineDate.Text = "Sewing Inline Date";
            // 
            // dateSewingInlineDate
            // 
            this.dateSewingInlineDate.IsRequired = false;
            this.dateSewingInlineDate.Location = new System.Drawing.Point(123, 37);
            this.dateSewingInlineDate.Name = "dateSewingInlineDate";
            this.dateSewingInlineDate.Size = new System.Drawing.Size(280, 23);
            this.dateSewingInlineDate.TabIndex = 3;
            // 
            // dateSCIDelivery
            // 
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(123, 10);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 2;
            // 
            // labelEstBookingDate
            // 
            this.labelEstBookingDate.Lines = 0;
            this.labelEstBookingDate.Location = new System.Drawing.Point(421, 9);
            this.labelEstBookingDate.Name = "labelEstBookingDate";
            this.labelEstBookingDate.Size = new System.Drawing.Size(114, 23);
            this.labelEstBookingDate.TabIndex = 1;
            this.labelEstBookingDate.Text = "Est. Booking Date";
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Lines = 0;
            this.labelSCIDelivery.Location = new System.Drawing.Point(2, 10);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(120, 23);
            this.labelSCIDelivery.TabIndex = 0;
            this.labelSCIDelivery.Text = "SCI Delivery";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 468);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(940, 42);
            this.panel4.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(855, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridDetail);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 73);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(940, 395);
            this.panel5.TabIndex = 5;
            // 
            // gridDetail
            // 
            this.gridDetail.AllowUserToAddRows = false;
            this.gridDetail.AllowUserToDeleteRows = false;
            this.gridDetail.AllowUserToResizeRows = false;
            this.gridDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetail.DataSource = this.listControlBindingSource1;
            this.gridDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDetail.Location = new System.Drawing.Point(0, 0);
            this.gridDetail.Name = "gridDetail";
            this.gridDetail.RowHeadersVisible = false;
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.Size = new System.Drawing.Size(940, 395);
            this.gridDetail.TabIndex = 0;
            this.gridDetail.TabStop = false;
            // 
            // P13
            // 
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(950, 510);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.EditMode = true;
            this.Name = "P13";
            this.Text = "P13. Carton Booking";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.panel5, 0);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Button btnToExcel;
        private Win.UI.Button btnQuery;
        private Win.UI.DateRange dateEstArrivedDate;
        private Win.UI.DateRange dateEstBookingDate;
        private Win.UI.Label labelEstArrivedDate;
        private Win.UI.Label labelSewingInlineDate;
        private Win.UI.DateRange dateSewingInlineDate;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.Label labelEstBookingDate;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Panel panel4;
        private Win.UI.Button btnClose;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridDetail;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}
