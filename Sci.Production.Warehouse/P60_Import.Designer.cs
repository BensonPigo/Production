namespace Sci.Production.Warehouse
{
    partial class P60_Import
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
            this.components = new System.ComponentModel.Container();
            this.button3 = new Sci.Win.UI.Button();
            this.button2 = new Sci.Win.UI.Button();
            this.button1 = new Sci.Win.UI.Button();
            this.textBox1 = new Sci.Win.UI.TextBox();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.label4 = new Sci.Win.UI.Label();
            this.txtartworktype_fty1 = new Sci.Production.Class.txtartworktype_fty();
            this.label1 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.dateRange1 = new Sci.Win.UI.DateRange();
            this.label2 = new Sci.Win.UI.Label();
            this.txtSpno2 = new Sci.Win.UI.TextBox();
            this.txtSpno1 = new Sci.Win.UI.TextBox();
            this.panel1 = new Sci.Win.UI.Panel();
            this.grid1 = new Sci.Win.UI.Grid();
            this.detailBS = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailBS)).BeginInit();
            this.SuspendLayout();
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button3.Location = new System.Drawing.Point(912, 15);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(90, 30);
            this.button3.TabIndex = 1;
            this.button3.Text = "Cancel";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button2.Location = new System.Drawing.Point(816, 16);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(90, 30);
            this.button2.TabIndex = 0;
            this.button2.Text = "Import";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button1.Location = new System.Drawing.Point(886, 50);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 30);
            this.button1.TabIndex = 5;
            this.button1.Text = "Find Now";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox1.Location = new System.Drawing.Point(107, 19);
            this.textBox1.MaxLength = 13;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(141, 23);
            this.textBox1.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 608);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1008, 53);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtartworktype_fty1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dateRange1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtSpno2);
            this.groupBox1.Controls.Add(this.txtSpno1);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1008, 93);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(383, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 23);
            this.label4.TabIndex = 105;
            this.label4.Text = "Category";
            // 
            // txtartworktype_fty1
            // 
            this.txtartworktype_fty1.BackColor = System.Drawing.Color.White;
            this.txtartworktype_fty1.cClassify = "";
            this.txtartworktype_fty1.cSubprocess = "Y";
            this.txtartworktype_fty1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtartworktype_fty1.Location = new System.Drawing.Point(490, 54);
            this.txtartworktype_fty1.Name = "txtartworktype_fty1";
            this.txtartworktype_fty1.Size = new System.Drawing.Size(140, 23);
            this.txtartworktype_fty1.TabIndex = 104;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(9, 19);
            this.label1.Name = "label1";
            this.label1.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label1.Size = new System.Drawing.Size(95, 23);
            this.label1.TabIndex = 103;
            this.label1.Text = "Local PO#";
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.PaleGreen;
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(383, 19);
            this.label3.Name = "label3";
            this.label3.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label3.Size = new System.Drawing.Size(104, 23);
            this.label3.TabIndex = 102;
            this.label3.Text = "PO Issue Date";
            this.label3.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateRange1
            // 
            this.dateRange1.Location = new System.Drawing.Point(490, 19);
            this.dateRange1.Name = "dateRange1";
            this.dateRange1.Size = new System.Drawing.Size(280, 23);
            this.dateRange1.TabIndex = 101;
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(9, 54);
            this.label2.Name = "label2";
            this.label2.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label2.Size = new System.Drawing.Size(95, 23);
            this.label2.TabIndex = 100;
            this.label2.Text = "SP#";
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSpno2
            // 
            this.txtSpno2.BackColor = System.Drawing.Color.White;
            this.txtSpno2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSpno2.Location = new System.Drawing.Point(235, 54);
            this.txtSpno2.Name = "txtSpno2";
            this.txtSpno2.Size = new System.Drawing.Size(116, 23);
            this.txtSpno2.TabIndex = 99;
            // 
            // txtSpno1
            // 
            this.txtSpno1.BackColor = System.Drawing.Color.White;
            this.txtSpno1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSpno1.Location = new System.Drawing.Point(107, 54);
            this.txtSpno1.Name = "txtSpno1";
            this.txtSpno1.Size = new System.Drawing.Size(122, 23);
            this.txtSpno1.TabIndex = 98;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grid1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 93);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 515);
            this.panel1.TabIndex = 20;
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(0, 0);
            this.grid1.Name = "grid1";
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(1008, 515);
            this.grid1.TabIndex = 0;
            this.grid1.TabStop = false;
            // 
            // P60_Import
            // 
            this.ClientSize = new System.Drawing.Size(1008, 661);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "P60_Import";
            this.Text = "P60. Import Detail";
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailBS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button button3;
        private Win.UI.Button button2;
        private Win.UI.Button button1;
        private Win.UI.TextBox textBox1;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Panel panel1;
        private Win.UI.Grid grid1;
        private Win.UI.ListControlBindingSource detailBS;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtSpno2;
        private Win.UI.TextBox txtSpno1;
        private Win.UI.Label label1;
        private Win.UI.Label label3;
        private Win.UI.DateRange dateRange1;
        private Win.UI.Label label4;
        private Class.txtartworktype_fty txtartworktype_fty1;
    }
}
