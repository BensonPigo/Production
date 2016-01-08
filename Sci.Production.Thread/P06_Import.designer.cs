namespace Sci.Production.Thread
{
    partial class P06_Import
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
            this.button3 = new Sci.Win.UI.Button();
            this.panel2 = new Sci.Win.UI.Panel();
            this.button2 = new Sci.Win.UI.Button();
            this.button1 = new Sci.Win.UI.Button();
            this.grid1 = new Sci.Win.UI.Grid();
            this.panel1 = new Sci.Win.UI.Panel();
            this.txtlocalitem2 = new Sci.Production.Class.txtlocalitem();
            this.label6 = new Sci.Win.UI.Label();
            this.txtthreadcolor2 = new Sci.Production.Class.txtthreadcolor();
            this.label8 = new Sci.Win.UI.Label();
            this.txtthreadcolor1 = new Sci.Production.Class.txtthreadcolor();
            this.label7 = new Sci.Win.UI.Label();
            this.txtthreadlocation2 = new Sci.Production.Class.txtthreadlocation();
            this.txtthreadlocation1 = new Sci.Production.Class.txtthreadlocation();
            this.txtlocalitem1 = new Sci.Production.Class.txtlocalitem();
            this.label1 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(750, 13);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(80, 30);
            this.button3.TabIndex = 7;
            this.button3.Text = "Query";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 376);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(840, 44);
            this.panel2.TabIndex = 11;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(750, 7);
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
            this.button1.Location = new System.Drawing.Point(664, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "Import";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
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
            this.grid1.Location = new System.Drawing.Point(0, 81);
            this.grid1.Name = "grid1";
            this.grid1.RowHeadersVisible = false;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(840, 295);
            this.grid1.TabIndex = 0;
            this.grid1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtlocalitem2);
            this.panel1.Controls.Add(this.txtthreadcolor2);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtthreadcolor1);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtthreadlocation2);
            this.panel1.Controls.Add(this.txtthreadlocation1);
            this.panel1.Controls.Add(this.txtlocalitem1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(840, 81);
            this.panel1.TabIndex = 10;
            // 
            // txtlocalitem2
            // 
            this.txtlocalitem2.BackColor = System.Drawing.Color.White;
            this.txtlocalitem2.CategoryObjectName = this.label6;
            this.txtlocalitem2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtlocalitem2.LocalSuppObjectName = null;
            this.txtlocalitem2.Location = new System.Drawing.Point(265, 17);
            this.txtlocalitem2.Name = "txtlocalitem2";
            this.txtlocalitem2.Size = new System.Drawing.Size(150, 23);
            this.txtlocalitem2.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(70, 84);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 23);
            this.label6.TabIndex = 23;
            this.label6.Text = "Thread";
            this.label6.Visible = false;
            // 
            // txtthreadcolor2
            // 
            this.txtthreadcolor2.BackColor = System.Drawing.Color.White;
            this.txtthreadcolor2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtthreadcolor2.IsSupportSytsemContextMenu = false;
            this.txtthreadcolor2.Location = new System.Drawing.Point(200, 49);
            this.txtthreadcolor2.Name = "txtthreadcolor2";
            this.txtthreadcolor2.Size = new System.Drawing.Size(90, 23);
            this.txtthreadcolor2.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(183, 55);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 15);
            this.label8.TabIndex = 28;
            this.label8.Text = "~";
            this.label8.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtthreadcolor1
            // 
            this.txtthreadcolor1.BackColor = System.Drawing.Color.White;
            this.txtthreadcolor1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtthreadcolor1.IsSupportSytsemContextMenu = false;
            this.txtthreadcolor1.Location = new System.Drawing.Point(90, 49);
            this.txtthreadcolor1.Name = "txtthreadcolor1";
            this.txtthreadcolor1.Size = new System.Drawing.Size(90, 23);
            this.txtthreadcolor1.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(13, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 23);
            this.label7.TabIndex = 26;
            this.label7.Text = "Color";
            // 
            // txtthreadlocation2
            // 
            this.txtthreadlocation2.BackColor = System.Drawing.Color.White;
            this.txtthreadlocation2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtthreadlocation2.IsSupportSytsemContextMenu = false;
            this.txtthreadlocation2.Location = new System.Drawing.Point(629, 17);
            this.txtthreadlocation2.Name = "txtthreadlocation2";
            this.txtthreadlocation2.Size = new System.Drawing.Size(90, 23);
            this.txtthreadlocation2.TabIndex = 3;
            // 
            // txtthreadlocation1
            // 
            this.txtthreadlocation1.BackColor = System.Drawing.Color.White;
            this.txtthreadlocation1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtthreadlocation1.IsSupportSytsemContextMenu = false;
            this.txtthreadlocation1.Location = new System.Drawing.Point(519, 17);
            this.txtthreadlocation1.Name = "txtthreadlocation1";
            this.txtthreadlocation1.Size = new System.Drawing.Size(90, 23);
            this.txtthreadlocation1.TabIndex = 2;
            // 
            // txtlocalitem1
            // 
            this.txtlocalitem1.BackColor = System.Drawing.Color.White;
            this.txtlocalitem1.CategoryObjectName = this.label6;
            this.txtlocalitem1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtlocalitem1.LocalSuppObjectName = null;
            this.txtlocalitem1.Location = new System.Drawing.Point(90, 17);
            this.txtlocalitem1.Name = "txtlocalitem1";
            this.txtlocalitem1.Size = new System.Drawing.Size(150, 23);
            this.txtlocalitem1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(612, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 15);
            this.label1.TabIndex = 15;
            this.label1.Text = "~";
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label3
            // 
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(436, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 14;
            this.label3.Text = "Location";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(248, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(14, 15);
            this.label4.TabIndex = 13;
            this.label4.Text = "~";
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label2
            // 
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(12, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = "Refno";
            // 
            // P06_Import
            // 
            this.ClientSize = new System.Drawing.Size(840, 420);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label6);
            this.Name = "P06_Import";
            this.Text = "Import from Part";
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button button3;
        private Win.UI.Panel panel2;
        private Win.UI.Button button2;
        private Win.UI.Button button1;
        private Win.UI.Grid grid1;
        private Win.UI.Panel panel1;
        private Win.UI.Label label2;
        private Win.UI.Label label4;
        private Win.UI.Label label1;
        private Win.UI.Label label3;
        private Production.Class.txtlocalitem txtlocalitem1;
        private Win.UI.Label label6;
        private Class.txtthreadlocation txtthreadlocation2;
        private Class.txtthreadlocation txtthreadlocation1;
        private Win.UI.Label label7;
        private Class.txtthreadcolor txtthreadcolor2;
        private Win.UI.Label label8;
        private Class.txtthreadcolor txtthreadcolor1;
        private Class.txtlocalitem txtlocalitem2;
    }
}
