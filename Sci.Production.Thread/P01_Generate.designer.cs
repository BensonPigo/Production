namespace Sci.Production.Thread
{
    partial class P01_Generate
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
            this.textBox1 = new Sci.Win.UI.TextBox();
            this.button3 = new Sci.Win.UI.Button();
            this.panel2 = new Sci.Win.UI.Panel();
            this.button4 = new Sci.Win.UI.Button();
            this.label2 = new Sci.Win.UI.Label();
            this.txtthreadcomb1 = new Sci.Production.Class.txtthreadcomb();
            this.button2 = new Sci.Win.UI.Button();
            this.btn_Generate = new Sci.Win.UI.Button();
            this.grid1 = new Sci.Win.UI.Grid();
            this.panel1 = new Sci.Win.UI.Panel();
            this.checkBox1 = new Sci.Win.UI.CheckBox();
            this.label1 = new Sci.Win.UI.Label();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox1.Location = new System.Drawing.Point(165, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(150, 23);
            this.textBox1.TabIndex = 18;
            this.textBox1.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.textBox1_PopUp);
            this.textBox1.Validating += new System.ComponentModel.CancelEventHandler(this.textBox1_Validating);
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(732, 13);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(80, 30);
            this.button3.TabIndex = 2;
            this.button3.Text = "Filter";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txtthreadcomb1);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.btn_Generate);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 430);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(946, 77);
            this.panel2.TabIndex = 11;
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button4.Location = new System.Drawing.Point(856, 6);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(80, 30);
            this.button4.TabIndex = 16;
            this.button4.Text = "Batch update";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(624, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 23);
            this.label2.TabIndex = 15;
            this.label2.Text = "Thread Combination";
            // 
            // txtthreadcomb1
            // 
            this.txtthreadcomb1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtthreadcomb1.BackColor = System.Drawing.Color.White;
            this.txtthreadcomb1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtthreadcomb1.IsSupportSytsemContextMenu = false;
            this.txtthreadcomb1.Location = new System.Drawing.Point(760, 10);
            this.txtthreadcomb1.Name = "txtthreadcomb1";
            this.txtthreadcomb1.Size = new System.Drawing.Size(90, 23);
            this.txtthreadcomb1.TabIndex = 9;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(856, 40);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 30);
            this.button2.TabIndex = 8;
            this.button2.Text = "Close";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btn_Generate
            // 
            this.btn_Generate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_Generate.Location = new System.Drawing.Point(770, 40);
            this.btn_Generate.Name = "btn_Generate";
            this.btn_Generate.Size = new System.Drawing.Size(80, 30);
            this.btn_Generate.TabIndex = 0;
            this.btn_Generate.Text = "Generate";
            this.btn_Generate.UseVisualStyleBackColor = true;
            this.btn_Generate.Click += new System.EventHandler(this.btn_Generate_Click);
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
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(946, 372);
            this.grid1.TabIndex = 0;
            this.grid1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(946, 58);
            this.panel1.TabIndex = 10;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBox1.Location = new System.Drawing.Point(356, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(264, 21);
            this.checkBox1.TabIndex = 14;
            this.checkBox1.Text = "Only show not yet assign combination";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(57, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 23);
            this.label1.TabIndex = 13;
            this.label1.Text = "Machine Type";
            // 
            // P01_Generate
            // 
            this.ClientSize = new System.Drawing.Size(946, 507);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "P01_Generate";
            this.Text = "Generate  Thread Combination";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button button3;
        private Win.UI.Panel panel2;
        private Win.UI.Button button2;
        private Win.UI.Button btn_Generate;
        private Win.UI.Grid grid1;
        private Win.UI.Panel panel1;
        private Win.UI.CheckBox checkBox1;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Class.txtthreadcomb txtthreadcomb1;
        private Win.UI.Button button4;
        private Win.UI.TextBox textBox1;
    }
}
