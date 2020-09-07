namespace Sci.Production.Warehouse
{
    partial class P15_Unfinish
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

        #region 元件設計工具產生的程式碼

        /// <summary> 
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gridUnfinish = new Sci.Win.UI.Grid();
            this.panel2 = new Sci.Win.UI.Panel();
            this.btnHide = new Sci.Win.UI.Button();
            this.btnRefresh = new Sci.Win.UI.Button();
            this.bindingSource1 = new Sci.Win.UI.BindingSource(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridUnfinish)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridUnfinish
            // 
            this.gridUnfinish.AllowUserToAddRows = false;
            this.gridUnfinish.AllowUserToDeleteRows = false;
            this.gridUnfinish.AllowUserToResizeRows = false;
            this.gridUnfinish.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridUnfinish.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridUnfinish.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridUnfinish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridUnfinish.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridUnfinish.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridUnfinish.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridUnfinish.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridUnfinish.Location = new System.Drawing.Point(0, 0);
            this.gridUnfinish.Name = "gridUnfinish";
            this.gridUnfinish.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridUnfinish.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridUnfinish.RowTemplate.Height = 24;
            this.gridUnfinish.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridUnfinish.Size = new System.Drawing.Size(522, 347);
            this.gridUnfinish.TabIndex = 2;
            this.gridUnfinish.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnHide);
            this.panel2.Controls.Add(this.btnRefresh);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 347);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(522, 48);
            this.panel2.TabIndex = 3;
            // 
            // btnHide
            // 
            this.btnHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHide.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnHide.Location = new System.Drawing.Point(432, 6);
            this.btnHide.Name = "btnHide";
            this.btnHide.Size = new System.Drawing.Size(80, 30);
            this.btnHide.TabIndex = 1;
            this.btnHide.Text = "Hide";
            this.btnHide.UseVisualStyleBackColor = true;
            this.btnHide.Visible = false;
            this.btnHide.Click += new System.EventHandler(this.BtnHide_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnRefresh.Location = new System.Drawing.Point(346, 6);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(80, 30);
            this.btnRefresh.TabIndex = 0;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.BtnRefresh_Click);
            // 
            // P15_Unfinish
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 395);
            this.Controls.Add(this.gridUnfinish);
            this.Controls.Add(this.panel2);
            this.Name = "P15_Unfinish";
            ((System.ComponentModel.ISupportInitialize)(this.gridUnfinish)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid gridUnfinish;
        private Win.UI.BindingSource bindingSource1;
        private Win.UI.Panel panel2;
        private Win.UI.Button btnHide;
        private Win.UI.Button btnRefresh;
        private System.Windows.Forms.Timer timer1;
    }
}
