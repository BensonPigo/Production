namespace Sci.Production.Cutting
{
    partial class P04_Import
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.txtCell1 = new Sci.Production.Class.txtCell();
            this.dateBox1 = new Sci.Win.UI.DateBox();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
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
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(8, 52);
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
            this.grid1.Size = new System.Drawing.Size(927, 424);
            this.grid1.TabIndex = 3;
            this.grid1.TabStop = false;
            // 
            // Query
            // 
            this.Query.Location = new System.Drawing.Point(852, 12);
            this.Query.Name = "Query";
            this.Query.Size = new System.Drawing.Size(80, 30);
            this.Query.TabIndex = 2;
            this.Query.Text = "Query";
            this.Query.UseVisualStyleBackColor = true;
            this.Query.Click += new System.EventHandler(this.Query_Click);
            // 
            // Import
            // 
            this.Import.Location = new System.Drawing.Point(769, 482);
            this.Import.Name = "Import";
            this.Import.Size = new System.Drawing.Size(80, 30);
            this.Import.TabIndex = 2;
            this.Import.Text = "Import";
            this.Import.UseVisualStyleBackColor = true;
            this.Import.Click += new System.EventHandler(this.Import_Click);
            // 
            // Close
            // 
            this.Close.Location = new System.Drawing.Point(855, 482);
            this.Close.Name = "Close";
            this.Close.Size = new System.Drawing.Size(80, 30);
            this.Close.TabIndex = 3;
            this.Close.Text = "Close";
            this.Close.UseVisualStyleBackColor = true;
            this.Close.Click += new System.EventHandler(this.Close_Click);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(257, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "Cut Cell";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(9, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = "Est. Cut Date";
            // 
            // txtCell1
            // 
            this.txtCell1.BackColor = System.Drawing.Color.White;
            this.txtCell1.FactoryId = "";
            this.txtCell1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCell1.Location = new System.Drawing.Point(335, 12);
            this.txtCell1.Name = "txtCell1";
            this.txtCell1.Size = new System.Drawing.Size(30, 23);
            this.txtCell1.TabIndex = 1;
            // 
            // dateBox1
            // 
            this.dateBox1.Location = new System.Drawing.Point(111, 12);
            this.dateBox1.Name = "dateBox1";
            this.dateBox1.Size = new System.Drawing.Size(130, 23);
            this.dateBox1.TabIndex = 0;
            // 
            // P04_Import
            // 
            this.ClientSize = new System.Drawing.Size(944, 522);
            this.Controls.Add(this.dateBox1);
            this.Controls.Add(this.txtCell1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Close);
            this.Controls.Add(this.Import);
            this.Controls.Add(this.Query);
            this.Controls.Add(this.grid1);
            this.Name = "P04_Import";
            this.Text = "P04_Import";
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Grid grid1;
        private Win.UI.Button Query;
        private Win.UI.Button Import;
        private Win.UI.Button Close;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Class.txtCell txtCell1;
        private Win.UI.DateBox dateBox1;
    }
}
