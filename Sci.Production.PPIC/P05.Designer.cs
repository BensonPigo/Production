namespace Sci.Production.PPIC
{
    partial class P05
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
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.btnQuitWithoutSave = new Sci.Win.UI.Button();
            this.btnSaveAndQuit = new Sci.Win.UI.Button();
            this.btnFindNow = new Sci.Win.UI.Button();
            this.txtLocateforSP = new Sci.Win.UI.TextBox();
            this.labelLocateforSP = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            this.dateUptoSCIDelivery = new Sci.Win.UI.DateBox();
            this.labelUptoSCIDelivery = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridProductionSchedule = new Sci.Win.UI.Grid();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductionSchedule)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 490);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(952, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 490);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnToExcel);
            this.panel3.Controls.Add(this.btnQuitWithoutSave);
            this.panel3.Controls.Add(this.btnSaveAndQuit);
            this.panel3.Controls.Add(this.btnFindNow);
            this.panel3.Controls.Add(this.txtLocateforSP);
            this.panel3.Controls.Add(this.labelLocateforSP);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.dateUptoSCIDelivery);
            this.panel3.Controls.Add(this.labelUptoSCIDelivery);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(947, 75);
            this.panel3.TabIndex = 0;
            // 
            // btnToExcel
            // 
            this.btnToExcel.Enabled = false;
            this.btnToExcel.Location = new System.Drawing.Point(402, 4);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(80, 30);
            this.btnToExcel.TabIndex = 2;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // btnQuitWithoutSave
            // 
            this.btnQuitWithoutSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuitWithoutSave.Location = new System.Drawing.Point(796, 40);
            this.btnQuitWithoutSave.Name = "btnQuitWithoutSave";
            this.btnQuitWithoutSave.Size = new System.Drawing.Size(147, 30);
            this.btnQuitWithoutSave.TabIndex = 6;
            this.btnQuitWithoutSave.Text = "Quit without Save";
            this.btnQuitWithoutSave.UseVisualStyleBackColor = true;
            this.btnQuitWithoutSave.Click += new System.EventHandler(this.BtnQuitWithoutSave_Click);
            // 
            // btnSaveAndQuit
            // 
            this.btnSaveAndQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAndQuit.Location = new System.Drawing.Point(796, 4);
            this.btnSaveAndQuit.Name = "btnSaveAndQuit";
            this.btnSaveAndQuit.Size = new System.Drawing.Size(147, 30);
            this.btnSaveAndQuit.TabIndex = 5;
            this.btnSaveAndQuit.Text = "Save and Quit";
            this.btnSaveAndQuit.UseVisualStyleBackColor = true;
            this.btnSaveAndQuit.Click += new System.EventHandler(this.BtnSaveAndQuit_Click);
            // 
            // btnFindNow
            // 
            this.btnFindNow.Location = new System.Drawing.Point(668, 40);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(80, 30);
            this.btnFindNow.TabIndex = 4;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.BtnFindNow_Click);
            // 
            // txtLocateforSP
            // 
            this.txtLocateforSP.BackColor = System.Drawing.Color.White;
            this.txtLocateforSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocateforSP.IsSupportEditMode = false;
            this.txtLocateforSP.Location = new System.Drawing.Point(542, 44);
            this.txtLocateforSP.Name = "txtLocateforSP";
            this.txtLocateforSP.Size = new System.Drawing.Size(120, 23);
            this.txtLocateforSP.TabIndex = 3;
            // 
            // labelLocateforSP
            // 
            this.labelLocateforSP.Lines = 0;
            this.labelLocateforSP.Location = new System.Drawing.Point(442, 44);
            this.labelLocateforSP.Name = "labelLocateforSP";
            this.labelLocateforSP.Size = new System.Drawing.Size(96, 23);
            this.labelLocateforSP.TabIndex = 4;
            this.labelLocateforSP.Text = "Locate for SP#";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(4, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(434, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "P.S. MTL contiguous delay is high light sp column in yellow back color.";
            this.label2.TextStyle.BorderColor = System.Drawing.Color.Red;
            this.label2.TextStyle.Color = System.Drawing.Color.Red;
            this.label2.TextStyle.ExtBorderColor = System.Drawing.Color.Red;
            this.label2.TextStyle.GradientColor = System.Drawing.Color.Red;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(252, 4);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 1;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // dateUptoSCIDelivery
            // 
            this.dateUptoSCIDelivery.IsSupportEditMode = false;
            this.dateUptoSCIDelivery.Location = new System.Drawing.Point(125, 8);
            this.dateUptoSCIDelivery.Name = "dateUptoSCIDelivery";
            this.dateUptoSCIDelivery.Size = new System.Drawing.Size(110, 23);
            this.dateUptoSCIDelivery.TabIndex = 0;
            // 
            // labelUptoSCIDelivery
            // 
            this.labelUptoSCIDelivery.Lines = 0;
            this.labelUptoSCIDelivery.Location = new System.Drawing.Point(4, 8);
            this.labelUptoSCIDelivery.Name = "labelUptoSCIDelivery";
            this.labelUptoSCIDelivery.Size = new System.Drawing.Size(117, 23);
            this.labelUptoSCIDelivery.TabIndex = 7;
            this.labelUptoSCIDelivery.Text = "Up to SCI Delivery";
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 480);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(947, 10);
            this.panel4.TabIndex = 4;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridProductionSchedule);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 75);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(947, 405);
            this.panel5.TabIndex = 5;
            // 
            // gridProductionSchedule
            // 
            this.gridProductionSchedule.AllowUserToAddRows = false;
            this.gridProductionSchedule.AllowUserToDeleteRows = false;
            this.gridProductionSchedule.AllowUserToResizeRows = false;
            this.gridProductionSchedule.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridProductionSchedule.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridProductionSchedule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridProductionSchedule.DataSource = this.listControlBindingSource1;
            this.gridProductionSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridProductionSchedule.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridProductionSchedule.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridProductionSchedule.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridProductionSchedule.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridProductionSchedule.Location = new System.Drawing.Point(0, 0);
            this.gridProductionSchedule.Name = "gridProductionSchedule";
            this.gridProductionSchedule.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridProductionSchedule.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridProductionSchedule.RowTemplate.Height = 24;
            this.gridProductionSchedule.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridProductionSchedule.Size = new System.Drawing.Size(947, 405);
            this.gridProductionSchedule.TabIndex = 0;
            this.gridProductionSchedule.TabStop = false;
            // 
            // P05
            // 
            this.ClientSize = new System.Drawing.Size(957, 490);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "dateUptoSCIDelivery";
            this.DefaultControlForEdit = "dateUptoSCIDelivery";
            this.Name = "P05";
            this.Text = "P05. Poduction Schedule";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.panel5, 0);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridProductionSchedule)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Button btnToExcel;
        private Win.UI.Button btnQuitWithoutSave;
        private Win.UI.Button btnSaveAndQuit;
        private Win.UI.Button btnFindNow;
        private Win.UI.TextBox txtLocateforSP;
        private Win.UI.Label labelLocateforSP;
        private Win.UI.Label label2;
        private Win.UI.Button btnQuery;
        private Win.UI.DateBox dateUptoSCIDelivery;
        private Win.UI.Label labelUptoSCIDelivery;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridProductionSchedule;
    }
}
