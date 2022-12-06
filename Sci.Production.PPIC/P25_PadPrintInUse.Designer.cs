namespace Sci.Production.PPIC
{
    partial class P25_PadPrintInUse
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.panel3 = new Sci.Win.UI.Panel();
            this.chkCustOrderSize = new Sci.Win.UI.CheckBox();
            this.chkPart = new Sci.Win.UI.CheckBox();
            this.chkAgeGroup = new Sci.Win.UI.CheckBox();
            this.chkSizePage = new Sci.Win.UI.CheckBox();
            this.chkGender = new Sci.Win.UI.CheckBox();
            this.chkSourceSize = new Sci.Win.UI.CheckBox();
            this.lab1 = new Sci.Win.UI.Label();
            this.btnFindNow = new Sci.Win.UI.Button();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.grid = new Sci.Win.UI.Grid();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 399);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1064, 47);
            this.panel4.TabIndex = 10;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(972, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chkCustOrderSize);
            this.panel3.Controls.Add(this.chkPart);
            this.panel3.Controls.Add(this.chkAgeGroup);
            this.panel3.Controls.Add(this.chkSizePage);
            this.panel3.Controls.Add(this.chkGender);
            this.panel3.Controls.Add(this.chkSourceSize);
            this.panel3.Controls.Add(this.lab1);
            this.panel3.Controls.Add(this.btnFindNow);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1064, 57);
            this.panel3.TabIndex = 9;
            // 
            // chkCustOrderSize
            // 
            this.chkCustOrderSize.AutoSize = true;
            this.chkCustOrderSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkCustOrderSize.IsSupportEditMode = false;
            this.chkCustOrderSize.Location = new System.Drawing.Point(609, 17);
            this.chkCustOrderSize.Name = "chkCustOrderSize";
            this.chkCustOrderSize.Size = new System.Drawing.Size(119, 21);
            this.chkCustOrderSize.TabIndex = 246;
            this.chkCustOrderSize.Text = "CustOrderSize";
            this.chkCustOrderSize.UseVisualStyleBackColor = true;
            // 
            // chkPart
            // 
            this.chkPart.AutoSize = true;
            this.chkPart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkPart.IsSupportEditMode = false;
            this.chkPart.Location = new System.Drawing.Point(550, 17);
            this.chkPart.Name = "chkPart";
            this.chkPart.Size = new System.Drawing.Size(53, 21);
            this.chkPart.TabIndex = 245;
            this.chkPart.Text = "Part";
            this.chkPart.UseVisualStyleBackColor = true;
            // 
            // chkAgeGroup
            // 
            this.chkAgeGroup.AutoSize = true;
            this.chkAgeGroup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkAgeGroup.IsSupportEditMode = false;
            this.chkAgeGroup.Location = new System.Drawing.Point(452, 17);
            this.chkAgeGroup.Name = "chkAgeGroup";
            this.chkAgeGroup.Size = new System.Drawing.Size(92, 21);
            this.chkAgeGroup.TabIndex = 244;
            this.chkAgeGroup.Text = "AgeGroup";
            this.chkAgeGroup.UseVisualStyleBackColor = true;
            // 
            // chkSizePage
            // 
            this.chkSizePage.AutoSize = true;
            this.chkSizePage.Checked = true;
            this.chkSizePage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSizePage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkSizePage.IsSupportEditMode = false;
            this.chkSizePage.Location = new System.Drawing.Point(151, 17);
            this.chkSizePage.Name = "chkSizePage";
            this.chkSizePage.Size = new System.Drawing.Size(87, 21);
            this.chkSizePage.TabIndex = 243;
            this.chkSizePage.Text = "SizePage";
            this.chkSizePage.UseVisualStyleBackColor = true;
            // 
            // chkGender
            // 
            this.chkGender.AutoSize = true;
            this.chkGender.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkGender.IsSupportEditMode = false;
            this.chkGender.Location = new System.Drawing.Point(371, 17);
            this.chkGender.Name = "chkGender";
            this.chkGender.Size = new System.Drawing.Size(75, 21);
            this.chkGender.TabIndex = 242;
            this.chkGender.Text = "Gender";
            this.chkGender.UseVisualStyleBackColor = true;
            // 
            // chkSourceSize
            // 
            this.chkSourceSize.AutoSize = true;
            this.chkSourceSize.Checked = true;
            this.chkSourceSize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSourceSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkSourceSize.IsSupportEditMode = false;
            this.chkSourceSize.Location = new System.Drawing.Point(254, 17);
            this.chkSourceSize.Name = "chkSourceSize";
            this.chkSourceSize.Size = new System.Drawing.Size(99, 21);
            this.chkSourceSize.TabIndex = 241;
            this.chkSourceSize.Text = "SourceSize";
            this.chkSourceSize.UseVisualStyleBackColor = true;
            // 
            // lab1
            // 
            this.lab1.Location = new System.Drawing.Point(9, 15);
            this.lab1.Name = "lab1";
            this.lab1.Size = new System.Drawing.Size(123, 23);
            this.lab1.TabIndex = 8;
            this.lab1.Text = "Search Condition";
            // 
            // btnFindNow
            // 
            this.btnFindNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNow.Location = new System.Drawing.Point(947, 8);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(105, 30);
            this.btnFindNow.TabIndex = 6;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.btnFindNow_Click);
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Size = new System.Drawing.Size(1000, 691);
            this.shapeContainer1.TabIndex = 0;
            this.shapeContainer1.TabStop = false;
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.DataSource = this.listControlBindingSource1;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(0, 57);
            this.grid.Name = "grid";
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(1064, 342);
            this.grid.TabIndex = 11;
            this.grid.TabStop = false;
            // 
            // P25_PadPrintInUse
            // 
            this.ClientSize = new System.Drawing.Size(1064, 446);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Name = "P25_PadPrintInUse";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P25. PadPrint In Use";
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel4;
        private Win.UI.Button btnClose;
        private Win.UI.Panel panel3;
        private Win.UI.Label lab1;
        private Win.UI.Button btnFindNow;
        private Win.UI.CheckBox chkAgeGroup;
        private Win.UI.CheckBox chkSizePage;
        private Win.UI.CheckBox chkGender;
        private Win.UI.CheckBox chkSourceSize;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Win.UI.CheckBox chkCustOrderSize;
        private Win.UI.CheckBox chkPart;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Grid grid;
    }
}
