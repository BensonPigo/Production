namespace Sci.Production.Thread
{
    partial class P01_Detail
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
            this.panel2 = new Sci.Win.UI.Panel();
            this.button2 = new Sci.Win.UI.Button();
            this.button1 = new Sci.Win.UI.Button();
            this.label2 = new Sci.Win.UI.Label();
            this.grid1 = new Sci.Win.UI.Grid();
            this.panel1 = new Sci.Win.UI.Panel();
            this.displayBox4 = new Sci.Win.UI.DisplayBox();
            this.displayBox3 = new Sci.Win.UI.DisplayBox();
            this.label4 = new Sci.Win.UI.Label();
            this.displayBox2 = new Sci.Win.UI.DisplayBox();
            this.label3 = new Sci.Win.UI.Label();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.label1 = new Sci.Win.UI.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 437);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(946, 50);
            this.panel2.TabIndex = 11;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(856, 13);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 30);
            this.button2.TabIndex = 8;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(770, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "Edit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(448, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 23);
            this.label2.TabIndex = 15;
            this.label2.Text = "Thread Combination";
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
            this.grid1.Location = new System.Drawing.Point(0, 58);
            this.grid1.Name = "grid1";
            this.grid1.RowHeadersWidth = 21;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.RowHeadersVisible = true;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(946, 379);
            this.grid1.TabIndex = 0;
            this.grid1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.displayBox4);
            this.panel1.Controls.Add(this.displayBox3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.displayBox2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.displayBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(946, 58);
            this.panel1.TabIndex = 10;
            // 
            // displayBox4
            // 
            this.displayBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox4.Location = new System.Drawing.Point(584, 17);
            this.displayBox4.Name = "displayBox4";
            this.displayBox4.Size = new System.Drawing.Size(121, 23);
            this.displayBox4.TabIndex = 21;
            // 
            // displayBox3
            // 
            this.displayBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox3.Location = new System.Drawing.Point(352, 17);
            this.displayBox3.Name = "displayBox3";
            this.displayBox3.Size = new System.Drawing.Size(64, 23);
            this.displayBox3.TabIndex = 20;
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(286, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 23);
            this.label4.TabIndex = 19;
            this.label4.Text = "Season";
            // 
            // displayBox2
            // 
            this.displayBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox2.Location = new System.Drawing.Point(92, 17);
            this.displayBox2.Name = "displayBox2";
            this.displayBox2.Size = new System.Drawing.Size(165, 23);
            this.displayBox2.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(20, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 23);
            this.label3.TabIndex = 17;
            this.label3.Text = "Style No";
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(824, 17);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(100, 23);
            this.displayBox1.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(726, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 23);
            this.label1.TabIndex = 13;
            this.label1.Text = "Machine Type";
            // 
            // P01_Detail
            // 
            this.ClientSize = new System.Drawing.Size(946, 487);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.EditMode = false;
            this.Name = "P01_Detail";
            this.Text = "P01_Detail";
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel2;
        private Win.UI.Button button2;
        private Win.UI.Button button1;
        private Win.UI.Grid grid1;
        private Win.UI.Panel panel1;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.DisplayBox displayBox4;
        private Win.UI.DisplayBox displayBox3;
        private Win.UI.Label label4;
        private Win.UI.DisplayBox displayBox2;
        private Win.UI.Label label3;
        private Win.UI.DisplayBox displayBox1;
    }
}
