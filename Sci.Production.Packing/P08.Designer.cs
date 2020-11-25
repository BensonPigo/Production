namespace Sci.Production.Packing
{
    partial class P08
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
            this.dateCartonEstArrived = new Sci.Win.UI.DateRange();
            this.dateCartonEstBooking = new Sci.Win.UI.DateRange();
            this.labelCartonEstArrived = new Sci.Win.UI.Label();
            this.labelSewingInlineDate = new Sci.Win.UI.Label();
            this.txtSPEnd = new Sci.Win.UI.TextBox();
            this.label6 = new Sci.Win.UI.Label();
            this.txtSPStart = new Sci.Win.UI.TextBox();
            this.dateSewingInlineDate = new Sci.Win.UI.DateRange();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.labelCartonEstBooking = new Sci.Win.UI.Label();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.labelSP = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnApprove = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 480);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(1003, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 480);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnToExcel);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.dateCartonEstArrived);
            this.panel3.Controls.Add(this.dateCartonEstBooking);
            this.panel3.Controls.Add(this.labelCartonEstArrived);
            this.panel3.Controls.Add(this.labelSewingInlineDate);
            this.panel3.Controls.Add(this.txtSPEnd);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.txtSPStart);
            this.panel3.Controls.Add(this.dateSewingInlineDate);
            this.panel3.Controls.Add(this.dateSCIDelivery);
            this.panel3.Controls.Add(this.labelCartonEstBooking);
            this.panel3.Controls.Add(this.labelSCIDelivery);
            this.panel3.Controls.Add(this.labelSP);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(998, 85);
            this.panel3.TabIndex = 3;
            // 
            // btnToExcel
            // 
            this.btnToExcel.Location = new System.Drawing.Point(863, 42);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(80, 30);
            this.btnToExcel.TabIndex = 19;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(863, 5);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 18;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // dateCartonEstArrived
            // 
            // 
            // 
            // 
            this.dateCartonEstArrived.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCartonEstArrived.DateBox1.Name = "";
            this.dateCartonEstArrived.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateCartonEstArrived.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCartonEstArrived.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateCartonEstArrived.DateBox2.Name = "";
            this.dateCartonEstArrived.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateCartonEstArrived.DateBox2.TabIndex = 1;
            this.dateCartonEstArrived.IsRequired = false;
            this.dateCartonEstArrived.Location = new System.Drawing.Point(522, 58);
            this.dateCartonEstArrived.Name = "dateCartonEstArrived";
            this.dateCartonEstArrived.Size = new System.Drawing.Size(280, 23);
            this.dateCartonEstArrived.TabIndex = 17;
            // 
            // dateCartonEstBooking
            // 
            // 
            // 
            // 
            this.dateCartonEstBooking.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateCartonEstBooking.DateBox1.Name = "";
            this.dateCartonEstBooking.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateCartonEstBooking.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateCartonEstBooking.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateCartonEstBooking.DateBox2.Name = "";
            this.dateCartonEstBooking.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateCartonEstBooking.DateBox2.TabIndex = 1;
            this.dateCartonEstBooking.IsRequired = false;
            this.dateCartonEstBooking.Location = new System.Drawing.Point(522, 32);
            this.dateCartonEstBooking.Name = "dateCartonEstBooking";
            this.dateCartonEstBooking.Size = new System.Drawing.Size(280, 23);
            this.dateCartonEstBooking.TabIndex = 16;
            // 
            // labelCartonEstArrived
            // 
            this.labelCartonEstArrived.Location = new System.Drawing.Point(391, 58);
            this.labelCartonEstArrived.Name = "labelCartonEstArrived";
            this.labelCartonEstArrived.Size = new System.Drawing.Size(127, 23);
            this.labelCartonEstArrived.TabIndex = 15;
            this.labelCartonEstArrived.Text = "Carton Est. Arrived";
            // 
            // labelSewingInlineDate
            // 
            this.labelSewingInlineDate.Location = new System.Drawing.Point(391, 5);
            this.labelSewingInlineDate.Name = "labelSewingInlineDate";
            this.labelSewingInlineDate.Size = new System.Drawing.Size(127, 23);
            this.labelSewingInlineDate.TabIndex = 14;
            this.labelSewingInlineDate.Text = "Sewing Inline Date";
            // 
            // txtSPEnd
            // 
            this.txtSPEnd.BackColor = System.Drawing.Color.White;
            this.txtSPEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPEnd.Location = new System.Drawing.Point(237, 5);
            this.txtSPEnd.Name = "txtSPEnd";
            this.txtSPEnd.Size = new System.Drawing.Size(120, 23);
            this.txtSPEnd.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(212, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(22, 23);
            this.label6.TabIndex = 12;
            this.label6.Text = "～";
            this.label6.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label6.TextStyle.Color = System.Drawing.Color.Black;
            this.label6.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label6.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtSPStart
            // 
            this.txtSPStart.BackColor = System.Drawing.Color.White;
            this.txtSPStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPStart.Location = new System.Drawing.Point(89, 5);
            this.txtSPStart.Name = "txtSPStart";
            this.txtSPStart.Size = new System.Drawing.Size(120, 23);
            this.txtSPStart.TabIndex = 5;
            // 
            // dateSewingInlineDate
            // 
            // 
            // 
            // 
            this.dateSewingInlineDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSewingInlineDate.DateBox1.Name = "";
            this.dateSewingInlineDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSewingInlineDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSewingInlineDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSewingInlineDate.DateBox2.Name = "";
            this.dateSewingInlineDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSewingInlineDate.DateBox2.TabIndex = 1;
            this.dateSewingInlineDate.IsRequired = false;
            this.dateSewingInlineDate.Location = new System.Drawing.Point(522, 5);
            this.dateSewingInlineDate.Name = "dateSewingInlineDate";
            this.dateSewingInlineDate.Size = new System.Drawing.Size(280, 23);
            this.dateSewingInlineDate.TabIndex = 4;
            // 
            // dateSCIDelivery
            // 
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSCIDelivery.DateBox1.Name = "";
            this.dateSCIDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSCIDelivery.DateBox2.Name = "";
            this.dateSCIDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDelivery.DateBox2.TabIndex = 1;
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(89, 32);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 3;
            // 
            // labelCartonEstBooking
            // 
            this.labelCartonEstBooking.Location = new System.Drawing.Point(391, 32);
            this.labelCartonEstBooking.Name = "labelCartonEstBooking";
            this.labelCartonEstBooking.Size = new System.Drawing.Size(127, 23);
            this.labelCartonEstBooking.TabIndex = 2;
            this.labelCartonEstBooking.Text = "Carton Est. Booking";
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(2, 32);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(82, 23);
            this.labelSCIDelivery.TabIndex = 1;
            this.labelSCIDelivery.Text = "SCI Delivery";
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(2, 5);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(82, 23);
            this.labelSP.TabIndex = 0;
            this.labelSP.Text = "SP#";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Controls.Add(this.btnApprove);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 436);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(998, 44);
            this.panel4.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(907, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnApprove
            // 
            this.btnApprove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApprove.Location = new System.Drawing.Point(810, 7);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(91, 30);
            this.btnApprove.TabIndex = 0;
            this.btnApprove.Text = "Approve";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.BtnApprove_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridDetail);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 85);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(998, 351);
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
            this.gridDetail.ShowCellToolTips = false;
            this.gridDetail.Size = new System.Drawing.Size(998, 351);
            this.gridDetail.TabIndex = 0;
            this.gridDetail.TabStop = false;
            // 
            // P08
            // 
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1008, 480);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.EditMode = true;
            this.Name = "P08";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P08. Batch Approve to Purchase Cartons";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.P08_FormClosed);
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
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.TextBox txtSPStart;
        private Win.UI.DateRange dateSewingInlineDate;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.Label labelCartonEstBooking;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Label labelSP;
        private Win.UI.Panel panel4;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel5;
        private Win.UI.TextBox txtSPEnd;
        private Win.UI.Label label6;
        private Win.UI.Button btnToExcel;
        private Win.UI.Button btnQuery;
        private Win.UI.DateRange dateCartonEstArrived;
        private Win.UI.DateRange dateCartonEstBooking;
        private Win.UI.Label labelCartonEstArrived;
        private Win.UI.Label labelSewingInlineDate;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnApprove;
        private Win.UI.Grid gridDetail;
    }
}
