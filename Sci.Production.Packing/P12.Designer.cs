namespace Sci.Production.Packing
{
    partial class P12
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
            this.labelExpPoutDate = new Sci.Win.UI.Label();
            this.dateExpPoutDate = new Sci.Win.UI.DateBox();
            this.btnFindNow = new Sci.Win.UI.Button();
            this.labelCategory = new Sci.Win.UI.Label();
            this.txtLocateForSP = new Sci.Win.UI.TextBox();
            this.btnClose = new Sci.Win.UI.Button();
            this.labelLocateForSP = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel4 = new Sci.Win.UI.Panel();
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.panel3.SuspendLayout();
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
            this.panel3.Controls.Add(this.labelExpPoutDate);
            this.panel3.Controls.Add(this.dateExpPoutDate);
            this.panel3.Controls.Add(this.btnFindNow);
            this.panel3.Controls.Add(this.labelCategory);
            this.panel3.Controls.Add(this.txtLocateForSP);
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Controls.Add(this.labelLocateForSP);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.btnToExcel);
            this.panel3.Controls.Add(this.shapeContainer1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(846, 89);
            this.panel3.TabIndex = 3;
            // 
            // comboDropDownListCategory
            // 
            this.comboDropDownListCategory.BackColor = System.Drawing.Color.White;
            this.comboDropDownListCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDropDownListCategory.FormattingEnabled = true;
            this.comboDropDownListCategory.IsSupportUnselect = true;
            this.comboDropDownListCategory.Location = new System.Drawing.Point(343, 11);
            this.comboDropDownListCategory.Name = "comboDropDownListCategory";
            this.comboDropDownListCategory.Size = new System.Drawing.Size(160, 24);
            this.comboDropDownListCategory.TabIndex = 15;
            this.comboDropDownListCategory.Type = "Pms_GMT_Simple";
            // 
            // labelExpPoutDate
            // 
            this.labelExpPoutDate.Location = new System.Drawing.Point(4, 12);
            this.labelExpPoutDate.Name = "labelExpPoutDate";
            this.labelExpPoutDate.Size = new System.Drawing.Size(99, 23);
            this.labelExpPoutDate.TabIndex = 0;
            this.labelExpPoutDate.Text = "Exp P/out Date";
            // 
            // dateExpPoutDate
            // 
            this.dateExpPoutDate.Location = new System.Drawing.Point(107, 12);
            this.dateExpPoutDate.Name = "dateExpPoutDate";
            this.dateExpPoutDate.Size = new System.Drawing.Size(130, 23);
            this.dateExpPoutDate.TabIndex = 1;
            // 
            // btnFindNow
            // 
            this.btnFindNow.Location = new System.Drawing.Point(243, 53);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(80, 30);
            this.btnFindNow.TabIndex = 9;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.BtnFindNow_Click);
            // 
            // labelCategory
            // 
            this.labelCategory.Location = new System.Drawing.Point(278, 11);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(62, 23);
            this.labelCategory.TabIndex = 2;
            this.labelCategory.Text = "Category";
            // 
            // txtLocateForSP
            // 
            this.txtLocateForSP.BackColor = System.Drawing.Color.White;
            this.txtLocateForSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocateForSP.Location = new System.Drawing.Point(107, 57);
            this.txtLocateForSP.MaxLength = 13;
            this.txtLocateForSP.Name = "txtLocateForSP";
            this.txtLocateForSP.Size = new System.Drawing.Size(120, 23);
            this.txtLocateForSP.TabIndex = 8;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(756, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // labelLocateForSP
            // 
            this.labelLocateForSP.Location = new System.Drawing.Point(4, 57);
            this.labelLocateForSP.Name = "labelLocateForSP";
            this.labelLocateForSP.Size = new System.Drawing.Size(99, 23);
            this.labelLocateForSP.TabIndex = 7;
            this.labelLocateForSP.Text = "Locate for SP#";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(517, 8);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 4;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Location = new System.Drawing.Point(614, 8);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(113, 30);
            this.btnToExcel.TabIndex = 5;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(846, 89);
            this.shapeContainer1.TabIndex = 10;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 1;
            this.lineShape1.X2 = 841;
            this.lineShape1.Y1 = 44;
            this.lineShape1.Y2 = 44;
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
            this.panel5.Controls.Add(this.gridDetail);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 89);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(846, 379);
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
            this.gridDetail.Size = new System.Drawing.Size(846, 379);
            this.gridDetail.TabIndex = 0;
            this.gridDetail.TabStop = false;
            // 
            // P12
            // 
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(856, 483);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.EditMode = true;
            this.Name = "P12";
            this.Text = "P12. Shipment Schedule";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.panel5, 0);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Label labelCategory;
        private Win.UI.DateBox dateExpPoutDate;
        private Win.UI.Label labelExpPoutDate;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridDetail;
        private Win.UI.Button btnFindNow;
        private Win.UI.TextBox txtLocateForSP;
        private Win.UI.Button btnClose;
        private Win.UI.Label labelLocateForSP;
        private Win.UI.Button btnQuery;
        private Win.UI.Button btnToExcel;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private Class.ComboDropDownList comboDropDownListCategory;
    }
}
