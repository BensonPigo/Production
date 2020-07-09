namespace Sci.Production.PPIC
{
    partial class P06
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
            this.comboDropDownListCategory = new Sci.Production.Class.ComboDropDownList(this.components);
            this.btnFindNow = new Sci.Win.UI.Button();
            this.txtLocateForSP = new Sci.Win.UI.TextBox();
            this.labelLocateForSP = new Sci.Win.UI.Label();
            this.btnQuitWithoutSave = new Sci.Win.UI.Button();
            this.btnSaveAndQuit = new Sci.Win.UI.Button();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.btnQuery = new Sci.Win.UI.Button();
            this.labelExpPoutDate = new Sci.Win.UI.Label();
            this.dateExpPoutDate = new Sci.Win.UI.DateBox();
            this.labelCategory = new Sci.Win.UI.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.panel4 = new Sci.Win.UI.Panel();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridShipmentSchedule = new Sci.Win.UI.Grid();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridShipmentSchedule)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 483);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(851, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 483);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.comboDropDownListCategory);
            this.panel3.Controls.Add(this.btnFindNow);
            this.panel3.Controls.Add(this.txtLocateForSP);
            this.panel3.Controls.Add(this.labelLocateForSP);
            this.panel3.Controls.Add(this.btnQuitWithoutSave);
            this.panel3.Controls.Add(this.btnSaveAndQuit);
            this.panel3.Controls.Add(this.btnToExcel);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.labelExpPoutDate);
            this.panel3.Controls.Add(this.dateExpPoutDate);
            this.panel3.Controls.Add(this.labelCategory);
            this.panel3.Controls.Add(this.shapeContainer1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(846, 108);
            this.panel3.TabIndex = 3;
            // 
            // comboDropDownListCategory
            // 
            this.comboDropDownListCategory.BackColor = System.Drawing.Color.White;
            this.comboDropDownListCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDropDownListCategory.FormattingEnabled = true;
            this.comboDropDownListCategory.IsSupportUnselect = true;
            this.comboDropDownListCategory.Location = new System.Drawing.Point(328, 23);
            this.comboDropDownListCategory.Name = "comboDropDownListCategory";
            this.comboDropDownListCategory.Size = new System.Drawing.Size(160, 24);
            this.comboDropDownListCategory.TabIndex = 14;
            this.comboDropDownListCategory.Type = "Pms_GMT_Simple";
            // 
            // btnFindNow
            // 
            this.btnFindNow.Location = new System.Drawing.Point(243, 73);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(80, 30);
            this.btnFindNow.TabIndex = 9;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.BtnFindNow_Click);
            // 
            // txtLocateForSP
            // 
            this.txtLocateForSP.BackColor = System.Drawing.Color.White;
            this.txtLocateForSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocateForSP.IsSupportEditMode = false;
            this.txtLocateForSP.Location = new System.Drawing.Point(107, 77);
            this.txtLocateForSP.MaxLength = 13;
            this.txtLocateForSP.Name = "txtLocateForSP";
            this.txtLocateForSP.Size = new System.Drawing.Size(120, 23);
            this.txtLocateForSP.TabIndex = 8;
            // 
            // labelLocateForSP
            // 
            this.labelLocateForSP.Location = new System.Drawing.Point(4, 77);
            this.labelLocateForSP.Name = "labelLocateForSP";
            this.labelLocateForSP.Size = new System.Drawing.Size(99, 23);
            this.labelLocateForSP.TabIndex = 13;
            this.labelLocateForSP.Text = "Locate for SP#";
            // 
            // btnQuitWithoutSave
            // 
            this.btnQuitWithoutSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuitWithoutSave.Location = new System.Drawing.Point(702, 34);
            this.btnQuitWithoutSave.Name = "btnQuitWithoutSave";
            this.btnQuitWithoutSave.Size = new System.Drawing.Size(139, 30);
            this.btnQuitWithoutSave.TabIndex = 7;
            this.btnQuitWithoutSave.Text = "Quit without Save";
            this.btnQuitWithoutSave.UseVisualStyleBackColor = true;
            this.btnQuitWithoutSave.Click += new System.EventHandler(this.BtnQuitWithoutSave_Click);
            // 
            // btnSaveAndQuit
            // 
            this.btnSaveAndQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAndQuit.Location = new System.Drawing.Point(702, 3);
            this.btnSaveAndQuit.Name = "btnSaveAndQuit";
            this.btnSaveAndQuit.Size = new System.Drawing.Size(139, 30);
            this.btnSaveAndQuit.TabIndex = 6;
            this.btnSaveAndQuit.Text = "Save and Quit";
            this.btnSaveAndQuit.UseVisualStyleBackColor = true;
            this.btnSaveAndQuit.Click += new System.EventHandler(this.BtnSaveAndQuit_Click);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Location = new System.Drawing.Point(602, 20);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(80, 30);
            this.btnToExcel.TabIndex = 5;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(509, 20);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 4;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // labelExpPoutDate
            // 
            this.labelExpPoutDate.Location = new System.Drawing.Point(4, 24);
            this.labelExpPoutDate.Name = "labelExpPoutDate";
            this.labelExpPoutDate.Size = new System.Drawing.Size(99, 23);
            this.labelExpPoutDate.TabIndex = 10;
            this.labelExpPoutDate.Text = "Exp P/out Date";
            // 
            // dateExpPoutDate
            // 
            this.dateExpPoutDate.IsSupportEditMode = false;
            this.dateExpPoutDate.Location = new System.Drawing.Point(107, 24);
            this.dateExpPoutDate.Name = "dateExpPoutDate";
            this.dateExpPoutDate.Size = new System.Drawing.Size(130, 23);
            this.dateExpPoutDate.TabIndex = 2;
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(263, 23);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(62, 23);
            this.labelCategory.TabIndex = 6;
            this.labelCategory.Text = "Category";
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(846, 108);
            this.shapeContainer1.TabIndex = 0;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 1;
            this.lineShape1.X2 = 845;
            this.lineShape1.Y1 = 69;
            this.lineShape1.Y2 = 69;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 468);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(846, 15);
            this.panel4.TabIndex = 4;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridShipmentSchedule);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 108);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(846, 360);
            this.panel5.TabIndex = 5;
            // 
            // gridShipmentSchedule
            // 
            this.gridShipmentSchedule.AllowUserToAddRows = false;
            this.gridShipmentSchedule.AllowUserToDeleteRows = false;
            this.gridShipmentSchedule.AllowUserToResizeRows = false;
            this.gridShipmentSchedule.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridShipmentSchedule.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridShipmentSchedule.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridShipmentSchedule.DataSource = this.listControlBindingSource1;
            this.gridShipmentSchedule.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridShipmentSchedule.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridShipmentSchedule.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridShipmentSchedule.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridShipmentSchedule.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridShipmentSchedule.Location = new System.Drawing.Point(0, 0);
            this.gridShipmentSchedule.Name = "gridShipmentSchedule";
            this.gridShipmentSchedule.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridShipmentSchedule.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridShipmentSchedule.RowTemplate.Height = 24;
            this.gridShipmentSchedule.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridShipmentSchedule.Size = new System.Drawing.Size(846, 360);
            this.gridShipmentSchedule.TabIndex = 0;
            this.gridShipmentSchedule.TabStop = false;
            // 
            // P06
            // 
            this.ClientSize = new System.Drawing.Size(856, 483);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "dateExpPoutDate";
            this.DefaultControlForEdit = "dateExpPoutDate";
            this.EditMode = true;
            this.Name = "P06";
            this.Text = "P06. Shipment Schedule";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.panel5, 0);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridShipmentSchedule)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridShipmentSchedule;
        private Win.UI.Label labelExpPoutDate;
        private Win.UI.DateBox dateExpPoutDate;
        private Win.UI.Label labelCategory;
        private Win.UI.Button btnToExcel;
        private Win.UI.Button btnQuery;
        private Win.UI.Button btnQuitWithoutSave;
        private Win.UI.Button btnSaveAndQuit;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private Win.UI.Button btnFindNow;
        private Win.UI.TextBox txtLocateForSP;
        private Win.UI.Label labelLocateForSP;
        private Class.ComboDropDownList comboDropDownListCategory;
    }
}
