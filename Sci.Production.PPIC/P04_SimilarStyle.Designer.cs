namespace Sci.Production.PPIC
{
    partial class P04_SimilarStyle
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
            this.displayMasterStyle = new Sci.Win.UI.DisplayBox();
            this.displayBrand = new Sci.Win.UI.DisplayBox();
            this.labelMasterStyle = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape4 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape3 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape2 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridChildrenStyle = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.labelSeason = new Sci.Win.UI.Label();
            this.displaySeason = new Sci.Win.UI.DisplayBox();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridChildrenStyle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 470);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(580, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 470);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.displaySeason);
            this.panel3.Controls.Add(this.labelSeason);
            this.panel3.Controls.Add(this.displayMasterStyle);
            this.panel3.Controls.Add(this.displayBrand);
            this.panel3.Controls.Add(this.labelMasterStyle);
            this.panel3.Controls.Add(this.labelBrand);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.shapeContainer1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(570, 98);
            this.panel3.TabIndex = 2;
            // 
            // displayMasterStyle
            // 
            this.displayMasterStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayMasterStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayMasterStyle.Location = new System.Drawing.Point(100, 27);
            this.displayMasterStyle.Name = "displayMasterStyle";
            this.displayMasterStyle.Size = new System.Drawing.Size(140, 23);
            this.displayMasterStyle.TabIndex = 4;
            // 
            // displayBrand
            // 
            this.displayBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBrand.Location = new System.Drawing.Point(338, 27);
            this.displayBrand.Name = "displayBrand";
            this.displayBrand.Size = new System.Drawing.Size(73, 23);
            this.displayBrand.TabIndex = 3;
            // 
            // labelMasterStyle
            // 
            this.labelMasterStyle.Location = new System.Drawing.Point(15, 27);
            this.labelMasterStyle.Name = "labelMasterStyle";
            this.labelMasterStyle.Size = new System.Drawing.Size(81, 23);
            this.labelMasterStyle.TabIndex = 2;
            this.labelMasterStyle.Text = "Master Style";
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(253, 27);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(81, 23);
            this.labelBrand.TabIndex = 1;
            this.labelBrand.Text = "Brand";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(15, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Master";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            this.label1.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label1.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape4,
            this.lineShape3,
            this.lineShape2,
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(570, 98);
            this.shapeContainer1.TabIndex = 7;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape4
            // 
            this.lineShape4.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.lineShape4.Name = "lineShape4";
            this.lineShape4.X1 = 4;
            this.lineShape4.X2 = 4;
            this.lineShape4.Y1 = 16;
            this.lineShape4.Y2 = 88;
            // 
            // lineShape3
            // 
            this.lineShape3.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.lineShape3.Name = "lineShape3";
            this.lineShape3.X1 = 565;
            this.lineShape3.X2 = 565;
            this.lineShape3.Y1 = 16;
            this.lineShape3.Y2 = 88;
            // 
            // lineShape2
            // 
            this.lineShape2.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.lineShape2.Name = "lineShape2";
            this.lineShape2.X1 = 4;
            this.lineShape2.X2 = 565;
            this.lineShape2.Y1 = 88;
            this.lineShape2.Y2 = 88;
            // 
            // lineShape1
            // 
            this.lineShape1.BorderColor = System.Drawing.SystemColors.ButtonShadow;
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 4;
            this.lineShape1.X2 = 565;
            this.lineShape1.Y1 = 16;
            this.lineShape1.Y2 = 16;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(10, 417);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(570, 53);
            this.panel4.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(464, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridChildrenStyle);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 98);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(570, 319);
            this.panel5.TabIndex = 4;
            // 
            // gridChildrenStyle
            // 
            this.gridChildrenStyle.AllowUserToAddRows = false;
            this.gridChildrenStyle.AllowUserToDeleteRows = false;
            this.gridChildrenStyle.AllowUserToResizeRows = false;
            this.gridChildrenStyle.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridChildrenStyle.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridChildrenStyle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridChildrenStyle.DataSource = this.listControlBindingSource1;
            this.gridChildrenStyle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridChildrenStyle.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridChildrenStyle.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridChildrenStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridChildrenStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridChildrenStyle.Location = new System.Drawing.Point(0, 0);
            this.gridChildrenStyle.Name = "gridChildrenStyle";
            this.gridChildrenStyle.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridChildrenStyle.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridChildrenStyle.RowTemplate.Height = 24;
            this.gridChildrenStyle.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridChildrenStyle.ShowCellToolTips = false;
            this.gridChildrenStyle.Size = new System.Drawing.Size(570, 319);
            this.gridChildrenStyle.TabIndex = 0;
            this.gridChildrenStyle.TabStop = false;
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(15, 57);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(81, 23);
            this.labelSeason.TabIndex = 5;
            this.labelSeason.Text = "Season";
            // 
            // displaySeason
            // 
            this.displaySeason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySeason.Location = new System.Drawing.Point(100, 57);
            this.displaySeason.Name = "displaySeason";
            this.displaySeason.Size = new System.Drawing.Size(311, 23);
            this.displaySeason.TabIndex = 6;
            // 
            // P04_SimilarStyle
            // 
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(590, 470);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "P04_SimilarStyle";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Similar Style";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridChildrenStyle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Win.UI.Button btnClose;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridChildrenStyle;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Label label1;
        private Win.UI.DisplayBox displayMasterStyle;
        private Win.UI.DisplayBox displayBrand;
        private Win.UI.Label labelMasterStyle;
        private Win.UI.Label labelBrand;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape4;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape3;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape2;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private Win.UI.DisplayBox displaySeason;
        private Win.UI.Label labelSeason;
    }
}
