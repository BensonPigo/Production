namespace Sci.Production.Warehouse
{
    partial class P07_Sticker
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnPrint = new Sci.Win.UI.Button();
            this.gridSticker = new Sci.Win.UI.Grid();
            ((System.ComponentModel.ISupportInitialize)(this.gridSticker)).BeginInit();
            this.SuspendLayout();
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(469, 316);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(80, 30);
            this.btnPrint.TabIndex = 1;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // gridSticker
            // 
            this.gridSticker.AllowUserToAddRows = false;
            this.gridSticker.AllowUserToDeleteRows = false;
            this.gridSticker.AllowUserToResizeRows = false;
            this.gridSticker.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSticker.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSticker.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSticker.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSticker.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSticker.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSticker.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSticker.Location = new System.Drawing.Point(12, 12);
            this.gridSticker.Name = "gridSticker";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridSticker.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridSticker.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSticker.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSticker.RowTemplate.Height = 24;
            this.gridSticker.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSticker.Size = new System.Drawing.Size(537, 298);
            this.gridSticker.TabIndex = 2;
            this.gridSticker.TabStop = false;
            // 
            // P07_Sticker
            // 
            this.ClientSize = new System.Drawing.Size(561, 352);
            this.Controls.Add(this.gridSticker);
            this.Controls.Add(this.btnPrint);
            this.Name = "P07_Sticker";
            this.Text = "() ";
            this.Controls.SetChildIndex(this.btnPrint, 0);
            this.Controls.SetChildIndex(this.gridSticker, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridSticker)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnPrint;
        private Win.UI.Grid gridSticker;
    }
}
