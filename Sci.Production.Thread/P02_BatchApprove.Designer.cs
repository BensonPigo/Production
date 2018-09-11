namespace Sci.Production.Thread
{
    partial class P02_BatchApprove
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
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridBatchApprove = new Sci.Win.UI.Grid();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.btntoExcel = new Sci.Win.UI.Button();
            this.dateEstArrived = new Sci.Win.UI.DateRange();
            this.dateEstBooking = new Sci.Win.UI.DateRange();
            this.dateSewingInLine = new Sci.Win.UI.DateRange();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.txtfactory1 = new Sci.Production.Class.txtfactory();
            this.txtstyle1 = new Sci.Production.Class.txtstyle();
            this.txtbrand1 = new Sci.Production.Class.txtbrand();
            this.label7 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnApprove = new Sci.Win.UI.Button();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchApprove)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridBatchApprove);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 117);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 391);
            this.panel1.TabIndex = 23;
            // 
            // gridBatchApprove
            // 
            this.gridBatchApprove.AllowUserToAddRows = false;
            this.gridBatchApprove.AllowUserToDeleteRows = false;
            this.gridBatchApprove.AllowUserToResizeRows = false;
            this.gridBatchApprove.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBatchApprove.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridBatchApprove.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBatchApprove.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBatchApprove.DataSource = this.listControlBindingSource1;
            this.gridBatchApprove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridBatchApprove.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBatchApprove.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBatchApprove.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBatchApprove.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBatchApprove.Location = new System.Drawing.Point(0, 0);
            this.gridBatchApprove.Name = "gridBatchApprove";
            this.gridBatchApprove.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridBatchApprove.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridBatchApprove.RowTemplate.Height = 24;
            this.gridBatchApprove.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBatchApprove.ShowCellToolTips = false;
            this.gridBatchApprove.Size = new System.Drawing.Size(1008, 391);
            this.gridBatchApprove.TabIndex = 0;
            this.gridBatchApprove.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btntoExcel);
            this.groupBox1.Controls.Add(this.dateEstArrived);
            this.groupBox1.Controls.Add(this.dateEstBooking);
            this.groupBox1.Controls.Add(this.dateSewingInLine);
            this.groupBox1.Controls.Add(this.dateSCIDelivery);
            this.groupBox1.Controls.Add(this.txtfactory1);
            this.groupBox1.Controls.Add(this.txtstyle1);
            this.groupBox1.Controls.Add(this.txtbrand1);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnQuery);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1008, 117);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            // 
            // btntoExcel
            // 
            this.btntoExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btntoExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btntoExcel.Location = new System.Drawing.Point(791, 77);
            this.btntoExcel.Name = "btntoExcel";
            this.btntoExcel.Size = new System.Drawing.Size(101, 30);
            this.btntoExcel.TabIndex = 24;
            this.btntoExcel.Text = "To Excel";
            this.btntoExcel.UseVisualStyleBackColor = true;
            this.btntoExcel.Click += new System.EventHandler(this.BtntoExcel_Click);
            // 
            // dateEstArrived
            // 
            // 
            // 
            // 
            this.dateEstArrived.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateEstArrived.DateBox1.Name = "";
            this.dateEstArrived.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateEstArrived.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateEstArrived.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateEstArrived.DateBox2.Name = "";
            this.dateEstArrived.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateEstArrived.DateBox2.TabIndex = 1;
            this.dateEstArrived.Location = new System.Drawing.Point(719, 48);
            this.dateEstArrived.Name = "dateEstArrived";
            this.dateEstArrived.Size = new System.Drawing.Size(280, 23);
            this.dateEstArrived.TabIndex = 23;
            // 
            // dateEstBooking
            // 
            // 
            // 
            // 
            this.dateEstBooking.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateEstBooking.DateBox1.Name = "";
            this.dateEstBooking.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateEstBooking.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateEstBooking.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateEstBooking.DateBox2.Name = "";
            this.dateEstBooking.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateEstBooking.DateBox2.TabIndex = 1;
            this.dateEstBooking.Location = new System.Drawing.Point(719, 19);
            this.dateEstBooking.Name = "dateEstBooking";
            this.dateEstBooking.Size = new System.Drawing.Size(280, 23);
            this.dateEstBooking.TabIndex = 22;
            // 
            // dateSewingInLine
            // 
            // 
            // 
            // 
            this.dateSewingInLine.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSewingInLine.DateBox1.Name = "";
            this.dateSewingInLine.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSewingInLine.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSewingInLine.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSewingInLine.DateBox2.Name = "";
            this.dateSewingInLine.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSewingInLine.DateBox2.TabIndex = 1;
            this.dateSewingInLine.Location = new System.Drawing.Point(344, 48);
            this.dateSewingInLine.Name = "dateSewingInLine";
            this.dateSewingInLine.Size = new System.Drawing.Size(280, 23);
            this.dateSewingInLine.TabIndex = 21;
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
            this.dateSCIDelivery.Location = new System.Drawing.Point(344, 19);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 20;
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.FilteMDivision = false;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.IssupportJunk = false;
            this.txtfactory1.Location = new System.Drawing.Point(98, 77);
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.Size = new System.Drawing.Size(66, 23);
            this.txtfactory1.TabIndex = 19;
            // 
            // txtstyle1
            // 
            this.txtstyle1.BackColor = System.Drawing.Color.White;
            this.txtstyle1.BrandObjectName = null;
            this.txtstyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle1.Location = new System.Drawing.Point(98, 48);
            this.txtstyle1.Name = "txtstyle1";
            this.txtstyle1.Size = new System.Drawing.Size(130, 23);
            this.txtstyle1.TabIndex = 18;
            this.txtstyle1.tarBrand = null;
            this.txtstyle1.tarSeason = null;
            // 
            // txtbrand1
            // 
            this.txtbrand1.BackColor = System.Drawing.Color.White;
            this.txtbrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand1.Location = new System.Drawing.Point(98, 19);
            this.txtbrand1.Name = "txtbrand1";
            this.txtbrand1.Size = new System.Drawing.Size(66, 23);
            this.txtbrand1.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(627, 48);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 23);
            this.label7.TabIndex = 16;
            this.label7.Text = "Est. Arrived";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(627, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 23);
            this.label6.TabIndex = 15;
            this.label6.Text = "Est.Booking";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(235, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 23);
            this.label5.TabIndex = 14;
            this.label5.Text = "Sewing In-Line";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(235, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 23);
            this.label4.TabIndex = 13;
            this.label4.Text = "SCI Delivery";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(20, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 12;
            this.label3.Text = "Factory";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(20, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 11;
            this.label2.Text = "Style#";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(20, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 10;
            this.label1.Text = "Brand";
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnQuery.Location = new System.Drawing.Point(898, 77);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(101, 30);
            this.btnQuery.TabIndex = 9;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(912, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnApprove
            // 
            this.btnApprove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApprove.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnApprove.Location = new System.Drawing.Point(809, 15);
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(97, 30);
            this.btnApprove.TabIndex = 0;
            this.btnApprove.Text = "Approve";
            this.btnApprove.UseVisualStyleBackColor = true;
            this.btnApprove.Click += new System.EventHandler(this.BtnApprove_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnApprove);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 508);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1008, 53);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            // 
            // P02_BatchApprove
            // 
            this.ClientSize = new System.Drawing.Size(1008, 561);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "P02_BatchApprove";
            this.Text = "Sub Process Batch Approve";
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchApprove)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel1;
        private Win.UI.Grid gridBatchApprove;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Button btnQuery;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnApprove;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.DateRange dateSCIDelivery;
        private Class.txtfactory txtfactory1;
        private Class.txtstyle txtstyle1;
        private Class.txtbrand txtbrand1;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.DateRange dateEstArrived;
        private Win.UI.DateRange dateEstBooking;
        private Win.UI.DateRange dateSewingInLine;
        private Win.UI.Button btntoExcel;
    }
}
