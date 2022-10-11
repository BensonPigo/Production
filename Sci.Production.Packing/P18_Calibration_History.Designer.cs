namespace Sci.Production.Packing
{
    partial class P18_Calibration_History
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
            this.btnOK = new Sci.Win.UI.Button();
            this.gridInfo = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnOK);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 349);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(634, 50);
            this.panel4.TabIndex = 7;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(492, 11);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(119, 30);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // gridInfo
            // 
            this.gridInfo.AllowUserToAddRows = false;
            this.gridInfo.AllowUserToDeleteRows = false;
            this.gridInfo.AllowUserToResizeRows = false;
            this.gridInfo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridInfo.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridInfo.DataSource = this.listControlBindingSource1;
            this.gridInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridInfo.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridInfo.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridInfo.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridInfo.Location = new System.Drawing.Point(0, 0);
            this.gridInfo.Name = "gridInfo";
            this.gridInfo.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridInfo.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridInfo.RowTemplate.Height = 24;
            this.gridInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridInfo.ShowCellToolTips = false;
            this.gridInfo.Size = new System.Drawing.Size(634, 349);
            this.gridInfo.TabIndex = 5;
            this.gridInfo.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridInfo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(634, 349);
            this.panel1.TabIndex = 8;
            // 
            // P18_Calibration_History
            // 
            this.ClientSize = new System.Drawing.Size(634, 399);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel4);
            this.Name = "P18_Calibration_History";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Warning";
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel4;
        private Win.UI.Button btnOK;
        private Win.UI.Grid gridInfo;
        private System.Windows.Forms.Panel panel1;
    }
}
