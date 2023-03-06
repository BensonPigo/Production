namespace Sci.Production.PPIC
{
    partial class P01_BatchShipmentFinished
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.lbMchandle = new Sci.Win.UI.Label();
            this.txtuser1 = new Sci.Production.Class.Txtuser();
            this.txtBuyer = new Sci.Win.UI.TextBox();
            this.txtStyle = new Sci.Win.UI.TextBox();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.labelBuyer = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnUpdate = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridBatchShipmentFinished = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchShipmentFinished)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 427);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(1027, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 427);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lbMchandle);
            this.panel3.Controls.Add(this.txtuser1);
            this.panel3.Controls.Add(this.txtBuyer);
            this.panel3.Controls.Add(this.txtStyle);
            this.panel3.Controls.Add(this.btnToExcel);
            this.panel3.Controls.Add(this.dateBuyerDelivery);
            this.panel3.Controls.Add(this.labelBuyerDelivery);
            this.panel3.Controls.Add(this.labelBuyer);
            this.panel3.Controls.Add(this.labelStyle);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1017, 49);
            this.panel3.TabIndex = 0;
            // 
            // lbMchandle
            // 
            this.lbMchandle.Location = new System.Drawing.Point(340, 13);
            this.lbMchandle.Name = "lbMchandle";
            this.lbMchandle.Size = new System.Drawing.Size(67, 23);
            this.lbMchandle.TabIndex = 6;
            this.lbMchandle.Text = "Mchandle";
            // 
            // txtuser1
            // 
            this.txtuser1.DisplayBox1Binding = "";
            this.txtuser1.Location = new System.Drawing.Point(410, 12);
            this.txtuser1.Name = "txtuser1";
            this.txtuser1.Size = new System.Drawing.Size(78, 23);
            this.txtuser1.TabIndex = 5;
            this.txtuser1.TextBox1Binding = "";
            this.txtuser1.TextBox1.Validated += new System.EventHandler(this.Txtuser1_Validated);
            // 
            // txtBuyer
            // 
            this.txtBuyer.BackColor = System.Drawing.Color.White;
            this.txtBuyer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBuyer.Location = new System.Drawing.Point(253, 13);
            this.txtBuyer.Name = "txtBuyer";
            this.txtBuyer.Size = new System.Drawing.Size(66, 23);
            this.txtBuyer.TabIndex = 1;
            this.txtBuyer.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtBuyer_PopUp);
            this.txtBuyer.Validated += new System.EventHandler(this.TxtBuyer_Validated);
            // 
            // txtStyle
            // 
            this.txtStyle.BackColor = System.Drawing.Color.White;
            this.txtStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtStyle.Location = new System.Drawing.Point(55, 13);
            this.txtStyle.Name = "txtStyle";
            this.txtStyle.Size = new System.Drawing.Size(130, 23);
            this.txtStyle.TabIndex = 0;
            this.txtStyle.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtStyle_PopUp);
            this.txtStyle.Validated += new System.EventHandler(this.TxtStyle_Validated);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Location = new System.Drawing.Point(909, 9);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(96, 30);
            this.btnToExcel.TabIndex = 3;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // dateBuyerDelivery
            // 
            this.dateBuyerDelivery.IsRequired = false;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(611, 13);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 2;
            this.dateBuyerDelivery.Validated += new System.EventHandler(this.DateBuyerDelivery_Validated);
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Lines = 0;
            this.labelBuyerDelivery.Location = new System.Drawing.Point(513, 13);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(94, 23);
            this.labelBuyerDelivery.TabIndex = 4;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            // 
            // labelBuyer
            // 
            this.labelBuyer.Lines = 0;
            this.labelBuyer.Location = new System.Drawing.Point(206, 13);
            this.labelBuyer.Name = "labelBuyer";
            this.labelBuyer.Size = new System.Drawing.Size(43, 23);
            this.labelBuyer.TabIndex = 2;
            this.labelBuyer.Text = "Buyer";
            // 
            // labelStyle
            // 
            this.labelStyle.Lines = 0;
            this.labelStyle.Location = new System.Drawing.Point(7, 13);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(44, 23);
            this.labelStyle.TabIndex = 0;
            this.labelStyle.Text = "Style#";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Controls.Add(this.btnUpdate);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(10, 382);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1017, 45);
            this.panel4.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(923, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Location = new System.Drawing.Point(824, 7);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(80, 30);
            this.btnUpdate.TabIndex = 0;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridBatchShipmentFinished);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 49);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1017, 333);
            this.panel5.TabIndex = 4;
            // 
            // gridBatchShipmentFinished
            // 
            this.gridBatchShipmentFinished.AllowUserToAddRows = false;
            this.gridBatchShipmentFinished.AllowUserToDeleteRows = false;
            this.gridBatchShipmentFinished.AllowUserToResizeRows = false;
            this.gridBatchShipmentFinished.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBatchShipmentFinished.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBatchShipmentFinished.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBatchShipmentFinished.DataSource = this.listControlBindingSource1;
            this.gridBatchShipmentFinished.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridBatchShipmentFinished.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBatchShipmentFinished.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBatchShipmentFinished.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBatchShipmentFinished.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBatchShipmentFinished.Location = new System.Drawing.Point(0, 0);
            this.gridBatchShipmentFinished.Name = "gridBatchShipmentFinished";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridBatchShipmentFinished.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridBatchShipmentFinished.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridBatchShipmentFinished.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridBatchShipmentFinished.RowTemplate.Height = 24;
            this.gridBatchShipmentFinished.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBatchShipmentFinished.Size = new System.Drawing.Size(1017, 333);
            this.gridBatchShipmentFinished.TabIndex = 0;
            this.gridBatchShipmentFinished.TabStop = false;
            // 
            // P01_BatchShipmentFinished
            // 
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(1037, 427);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "txtStyle";
            this.Name = "P01_BatchShipmentFinished";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Batch Shipment Finished";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchShipmentFinished)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Label labelStyle;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridBatchShipmentFinished;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Button btnToExcel;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.Label labelBuyer;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnUpdate;
        private Win.UI.TextBox txtBuyer;
        private Win.UI.TextBox txtStyle;
        private Win.UI.Label lbMchandle;
        private Class.Txtuser txtuser1;
    }
}
