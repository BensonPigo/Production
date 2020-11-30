namespace Sci.Production.Shipping
{
    partial class P10_DeleteGarmentBookingHistory
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
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnSave = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridShipPlanDeleteGBHistory = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnClose = new Sci.Win.UI.Button();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridShipPlanDeleteGBHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 507);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(843, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 507);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(838, 5);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Controls.Add(this.btnSave);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 464);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(838, 43);
            this.panel4.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(665, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(84, 30);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Edit";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridShipPlanDeleteGBHistory);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 5);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(838, 459);
            this.panel5.TabIndex = 4;
            // 
            // gridShipPlanDeleteGBHistory
            // 
            this.gridShipPlanDeleteGBHistory.AllowUserToAddRows = false;
            this.gridShipPlanDeleteGBHistory.AllowUserToDeleteRows = false;
            this.gridShipPlanDeleteGBHistory.AllowUserToResizeRows = false;
            this.gridShipPlanDeleteGBHistory.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridShipPlanDeleteGBHistory.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridShipPlanDeleteGBHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridShipPlanDeleteGBHistory.DataSource = this.listControlBindingSource1;
            this.gridShipPlanDeleteGBHistory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridShipPlanDeleteGBHistory.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridShipPlanDeleteGBHistory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridShipPlanDeleteGBHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridShipPlanDeleteGBHistory.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridShipPlanDeleteGBHistory.Location = new System.Drawing.Point(0, 0);
            this.gridShipPlanDeleteGBHistory.Name = "gridShipPlanDeleteGBHistory";
            this.gridShipPlanDeleteGBHistory.RowHeadersVisible = false;
            this.gridShipPlanDeleteGBHistory.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridShipPlanDeleteGBHistory.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridShipPlanDeleteGBHistory.RowTemplate.Height = 24;
            this.gridShipPlanDeleteGBHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridShipPlanDeleteGBHistory.ShowCellToolTips = false;
            this.gridShipPlanDeleteGBHistory.Size = new System.Drawing.Size(838, 459);
            this.gridShipPlanDeleteGBHistory.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridShipPlanDeleteGBHistory.TabIndex = 0;
            this.gridShipPlanDeleteGBHistory.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(755, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // P10_DeleteGarmentBookingHistory
            // 
            this.AcceptButton = this.btnSave;
            this.ClientSize = new System.Drawing.Size(848, 507);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "P10_DeleteGarmentBookingHistory";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P10. Delete Garment Booking History";
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridShipPlanDeleteGBHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridShipPlanDeleteGBHistory;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnClose;
    }
}
