namespace Sci.Production.Cutting
{
    partial class P20_Import_Workorder
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.grid1 = new Sci.Win.UI.Grid();
            this.Query = new Sci.Win.UI.Button();
            this.Import = new Sci.Win.UI.Button();
            this.Close = new Sci.Win.UI.Button();
            this.label2 = new Sci.Win.UI.Label();
            this.dateBox1 = new Sci.Win.UI.DateBox();
            this.panelTOP = new Sci.Win.UI.Panel();
            this.panelBOTTOM = new Sci.Win.UI.Panel();
            this.panelMIDDLE = new Sci.Win.UI.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.panelTOP.SuspendLayout();
            this.panelBOTTOM.SuspendLayout();
            this.panelMIDDLE.SuspendLayout();
            this.SuspendLayout();
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(0, 0);
            this.grid1.Name = "grid1";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid1.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(944, 439);
            this.grid1.TabIndex = 3;
            this.grid1.TabStop = false;
            // 
            // Query
            // 
            this.Query.Location = new System.Drawing.Point(852, 7);
            this.Query.Name = "Query";
            this.Query.Size = new System.Drawing.Size(80, 30);
            this.Query.TabIndex = 1;
            this.Query.Text = "Query";
            this.Query.UseVisualStyleBackColor = true;
            this.Query.Click += new System.EventHandler(this.Query_Click);
            // 
            // Import
            // 
            this.Import.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Import.Location = new System.Drawing.Point(766, 5);
            this.Import.Name = "Import";
            this.Import.Size = new System.Drawing.Size(80, 30);
            this.Import.TabIndex = 0;
            this.Import.Text = "Import";
            this.Import.UseVisualStyleBackColor = true;
            this.Import.Click += new System.EventHandler(this.Import_Click);
            // 
            // Close
            // 
            this.Close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Close.Location = new System.Drawing.Point(852, 5);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(80, 30);
            this.Close.TabIndex = 1;
            this.Close.Text = "Close";
            this.Close.UseVisualStyleBackColor = true;
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(13, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = "Est. Cut Date";
            // 
            // dateBox1
            // 
            this.dateBox1.Location = new System.Drawing.Point(115, 12);
            this.dateBox1.Name = "dateBox1";
            this.dateBox1.Size = new System.Drawing.Size(130, 23);
            this.dateBox1.TabIndex = 0;
            // 
            // panelTOP
            // 
            this.panelTOP.Controls.Add(this.label2);
            this.panelTOP.Controls.Add(this.dateBox1);
            this.panelTOP.Controls.Add(this.Query);
            this.panelTOP.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTOP.Location = new System.Drawing.Point(0, 0);
            this.panelTOP.Name = "panelTOP";
            this.panelTOP.Size = new System.Drawing.Size(944, 44);
            this.panelTOP.TabIndex = 0;
            // 
            // panelBOTTOM
            // 
            this.panelBOTTOM.Controls.Add(this.Close);
            this.panelBOTTOM.Controls.Add(this.Import);
            this.panelBOTTOM.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBOTTOM.Location = new System.Drawing.Point(0, 483);
            this.panelBOTTOM.Name = "panelBOTTOM";
            this.panelBOTTOM.Size = new System.Drawing.Size(944, 39);
            this.panelBOTTOM.TabIndex = 7;
            // 
            // panelMIDDLE
            // 
            this.panelMIDDLE.Controls.Add(this.grid1);
            this.panelMIDDLE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMIDDLE.Location = new System.Drawing.Point(0, 44);
            this.panelMIDDLE.Name = "panelMIDDLE";
            this.panelMIDDLE.Size = new System.Drawing.Size(944, 439);
            this.panelMIDDLE.TabIndex = 8;
            // 
            // P20_Import_Workorder
            // 
            this.ClientSize = new System.Drawing.Size(944, 522);
            this.Controls.Add(this.panelMIDDLE);
            this.Controls.Add(this.panelBOTTOM);
            this.Controls.Add(this.panelTOP);
            this.DefaultControl = "dateBox1";
            this.Name = "P20_Import_Workorder";
            this.Text = "P20_Import";
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.panelTOP.ResumeLayout(false);
            this.panelBOTTOM.ResumeLayout(false);
            this.panelMIDDLE.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid grid1;
        private Win.UI.Button Query;
        private Win.UI.Button Import;
        private Win.UI.Button Close;
        private Win.UI.Label label2;
        private Win.UI.DateBox dateBox1;
        private Win.UI.Panel panelTOP;
        private Win.UI.Panel panelBOTTOM;
        private Win.UI.Panel panelMIDDLE;
    }
}
