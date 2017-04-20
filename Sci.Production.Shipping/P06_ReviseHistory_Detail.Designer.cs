namespace Sci.Production.Shipping
{
    partial class P06_ReviseHistory_Detail
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
            this.numShipQtyRevised = new Sci.Win.UI.NumericBox();
            this.displayStatusRevised = new Sci.Win.UI.DisplayBox();
            this.displayRevisedStatus = new Sci.Win.UI.DisplayBox();
            this.labelShipQtyRevised = new Sci.Win.UI.Label();
            this.labelStatusRevised = new Sci.Win.UI.Label();
            this.labelRevisedStatus = new Sci.Win.UI.Label();
            this.numShipQtyOld = new Sci.Win.UI.NumericBox();
            this.displayStatusOld = new Sci.Win.UI.DisplayBox();
            this.displaySPNo = new Sci.Win.UI.DisplayBox();
            this.labelShipQtyOld = new Sci.Win.UI.Label();
            this.labelStatusOld = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridReviseHistoryDetail = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridReviseHistoryDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 411);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(498, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 411);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.numShipQtyRevised);
            this.panel3.Controls.Add(this.displayStatusRevised);
            this.panel3.Controls.Add(this.displayRevisedStatus);
            this.panel3.Controls.Add(this.labelShipQtyRevised);
            this.panel3.Controls.Add(this.labelStatusRevised);
            this.panel3.Controls.Add(this.labelRevisedStatus);
            this.panel3.Controls.Add(this.numShipQtyOld);
            this.panel3.Controls.Add(this.displayStatusOld);
            this.panel3.Controls.Add(this.displaySPNo);
            this.panel3.Controls.Add(this.labelShipQtyOld);
            this.panel3.Controls.Add(this.labelStatusOld);
            this.panel3.Controls.Add(this.labelSPNo);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(488, 90);
            this.panel3.TabIndex = 2;
            // 
            // numShipQtyRevised
            // 
            this.numShipQtyRevised.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numShipQtyRevised.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numShipQtyRevised.IsSupportEditMode = false;
            this.numShipQtyRevised.Location = new System.Drawing.Point(386, 61);
            this.numShipQtyRevised.Name = "numShipQtyRevised";
            this.numShipQtyRevised.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numShipQtyRevised.ReadOnly = true;
            this.numShipQtyRevised.Size = new System.Drawing.Size(65, 23);
            this.numShipQtyRevised.TabIndex = 11;
            this.numShipQtyRevised.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // displayStatusRevised
            // 
            this.displayStatusRevised.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStatusRevised.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStatusRevised.Location = new System.Drawing.Point(386, 34);
            this.displayStatusRevised.Name = "displayStatusRevised";
            this.displayStatusRevised.Size = new System.Drawing.Size(75, 23);
            this.displayStatusRevised.TabIndex = 10;
            // 
            // displayRevisedStatus
            // 
            this.displayRevisedStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayRevisedStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayRevisedStatus.Location = new System.Drawing.Point(386, 7);
            this.displayRevisedStatus.Name = "displayRevisedStatus";
            this.displayRevisedStatus.Size = new System.Drawing.Size(73, 23);
            this.displayRevisedStatus.TabIndex = 9;
            // 
            // labelShipQtyRevised
            // 
            this.labelShipQtyRevised.Lines = 0;
            this.labelShipQtyRevised.Location = new System.Drawing.Point(260, 61);
            this.labelShipQtyRevised.Name = "labelShipQtyRevised";
            this.labelShipQtyRevised.Size = new System.Drawing.Size(121, 23);
            this.labelShipQtyRevised.TabIndex = 8;
            this.labelShipQtyRevised.Text = "Ship Q\'ty (Revised)";
            // 
            // labelStatusRevised
            // 
            this.labelStatusRevised.Lines = 0;
            this.labelStatusRevised.Location = new System.Drawing.Point(259, 34);
            this.labelStatusRevised.Name = "labelStatusRevised";
            this.labelStatusRevised.Size = new System.Drawing.Size(122, 23);
            this.labelStatusRevised.TabIndex = 7;
            this.labelStatusRevised.Text = "Status (Revised)";
            // 
            // labelRevisedStatus
            // 
            this.labelRevisedStatus.Lines = 0;
            this.labelRevisedStatus.Location = new System.Drawing.Point(259, 7);
            this.labelRevisedStatus.Name = "labelRevisedStatus";
            this.labelRevisedStatus.Size = new System.Drawing.Size(122, 23);
            this.labelRevisedStatus.TabIndex = 6;
            this.labelRevisedStatus.Text = "Revised Status";
            // 
            // numShipQtyOld
            // 
            this.numShipQtyOld.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numShipQtyOld.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numShipQtyOld.IsSupportEditMode = false;
            this.numShipQtyOld.Location = new System.Drawing.Point(101, 61);
            this.numShipQtyOld.Name = "numShipQtyOld";
            this.numShipQtyOld.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numShipQtyOld.ReadOnly = true;
            this.numShipQtyOld.Size = new System.Drawing.Size(65, 23);
            this.numShipQtyOld.TabIndex = 5;
            this.numShipQtyOld.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // displayStatusOld
            // 
            this.displayStatusOld.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStatusOld.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStatusOld.Location = new System.Drawing.Point(101, 34);
            this.displayStatusOld.Name = "displayStatusOld";
            this.displayStatusOld.Size = new System.Drawing.Size(75, 23);
            this.displayStatusOld.TabIndex = 4;
            // 
            // displaySPNo
            // 
            this.displaySPNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySPNo.Location = new System.Drawing.Point(101, 7);
            this.displaySPNo.Name = "displaySPNo";
            this.displaySPNo.Size = new System.Drawing.Size(130, 23);
            this.displaySPNo.TabIndex = 3;
            // 
            // labelShipQtyOld
            // 
            this.labelShipQtyOld.Lines = 0;
            this.labelShipQtyOld.Location = new System.Drawing.Point(4, 61);
            this.labelShipQtyOld.Name = "labelShipQtyOld";
            this.labelShipQtyOld.Size = new System.Drawing.Size(93, 23);
            this.labelShipQtyOld.TabIndex = 2;
            this.labelShipQtyOld.Text = "Ship Q\'ty (Old)";
            // 
            // labelStatusOld
            // 
            this.labelStatusOld.Lines = 0;
            this.labelStatusOld.Location = new System.Drawing.Point(3, 34);
            this.labelStatusOld.Name = "labelStatusOld";
            this.labelStatusOld.Size = new System.Drawing.Size(94, 23);
            this.labelStatusOld.TabIndex = 1;
            this.labelStatusOld.Text = "Status (Old)";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Lines = 0;
            this.labelSPNo.Location = new System.Drawing.Point(3, 7);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(94, 23);
            this.labelSPNo.TabIndex = 0;
            this.labelSPNo.Text = "SP No.";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(10, 368);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(488, 43);
            this.panel4.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(399, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridReviseHistoryDetail);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 90);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(488, 278);
            this.panel5.TabIndex = 4;
            // 
            // gridReviseHistoryDetail
            // 
            this.gridReviseHistoryDetail.AllowUserToAddRows = false;
            this.gridReviseHistoryDetail.AllowUserToDeleteRows = false;
            this.gridReviseHistoryDetail.AllowUserToResizeRows = false;
            this.gridReviseHistoryDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridReviseHistoryDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridReviseHistoryDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridReviseHistoryDetail.DataSource = this.listControlBindingSource1;
            this.gridReviseHistoryDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridReviseHistoryDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridReviseHistoryDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridReviseHistoryDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridReviseHistoryDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridReviseHistoryDetail.Location = new System.Drawing.Point(0, 0);
            this.gridReviseHistoryDetail.Name = "gridReviseHistoryDetail";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridReviseHistoryDetail.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridReviseHistoryDetail.RowHeadersVisible = false;
            this.gridReviseHistoryDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridReviseHistoryDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridReviseHistoryDetail.RowTemplate.Height = 24;
            this.gridReviseHistoryDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridReviseHistoryDetail.Size = new System.Drawing.Size(488, 278);
            this.gridReviseHistoryDetail.TabIndex = 0;
            this.gridReviseHistoryDetail.TabStop = false;
            // 
            // P06_ReviseHistory_Detail
            // 
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(508, 411);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "P06_ReviseHistory_Detail";
            this.Text = "Revised Detail";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridReviseHistoryDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridReviseHistoryDetail;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.NumericBox numShipQtyRevised;
        private Win.UI.DisplayBox displayStatusRevised;
        private Win.UI.DisplayBox displayRevisedStatus;
        private Win.UI.Label labelShipQtyRevised;
        private Win.UI.Label labelStatusRevised;
        private Win.UI.Label labelRevisedStatus;
        private Win.UI.NumericBox numShipQtyOld;
        private Win.UI.DisplayBox displayStatusOld;
        private Win.UI.DisplayBox displaySPNo;
        private Win.UI.Label labelShipQtyOld;
        private Win.UI.Label labelStatusOld;
        private Win.UI.Label labelSPNo;
        private Win.UI.Button btnClose;
    }
}
