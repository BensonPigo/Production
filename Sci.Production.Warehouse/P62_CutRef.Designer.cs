namespace Sci.Production.Warehouse
{
    partial class P62_CuttingRef
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
            this.bindingSource1 = new Sci.Win.UI.BindingSource(this.components);
            this.panel2 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.gridCutplanID = new Sci.Win.UI.Grid();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCutplanID)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 386);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(838, 48);
            this.panel2.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(742, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click_1);
            // 
            // gridCutplanID
            // 
            this.gridCutplanID.AllowUserToAddRows = false;
            this.gridCutplanID.AllowUserToDeleteRows = false;
            this.gridCutplanID.AllowUserToResizeRows = false;
            this.gridCutplanID.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridCutplanID.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridCutplanID.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCutplanID.DataSource = this.bindingSource1;
            this.gridCutplanID.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCutplanID.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridCutplanID.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridCutplanID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridCutplanID.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridCutplanID.Location = new System.Drawing.Point(0, 0);
            this.gridCutplanID.Name = "gridCutplanID";
            this.gridCutplanID.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridCutplanID.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridCutplanID.RowTemplate.Height = 24;
            this.gridCutplanID.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCutplanID.ShowCellToolTips = false;
            this.gridCutplanID.Size = new System.Drawing.Size(838, 386);
            this.gridCutplanID.TabIndex = 5;
            this.gridCutplanID.TabStop = false;
            // 
            // P62_CuttingRef
            // 
            this.ClientSize = new System.Drawing.Size(838, 434);
            this.Controls.Add(this.gridCutplanID);
            this.Controls.Add(this.panel2);
            this.Name = "P62_CuttingRef";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P62. Cutplan ID";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCutplanID)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Win.UI.BindingSource bindingSource1;
        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private Win.UI.Grid gridCutplanID;
    }
}
