namespace Sci.Production.Warehouse
{
    partial class P01_BatchCloseRowMaterial
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
            this.txtSP1 = new Sci.Win.UI.TextBox();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.btnToEexcel = new Sci.Win.UI.Button();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.txtstyle1 = new Sci.Production.Class.txtstyle();
            this.txtmfactory1 = new Sci.Production.Class.txtmfactory();
            this.label8 = new Sci.Win.UI.Label();
            this.txtbrand1 = new Sci.Production.Class.txtbrand();
            this.label6 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.PullOutdateRange = new Sci.Win.UI.DateRange();
            this.BuyerDeliverydateRange = new Sci.Win.UI.DateRange();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.cbbCategory = new Sci.Win.UI.ComboBox();
            this.txtSP2 = new Sci.Win.UI.TextBox();
            this.panel1 = new Sci.Win.UI.Panel();
            this.grid1 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button3.Location = new System.Drawing.Point(912, 17);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(90, 30);
            this.button3.TabIndex = 5;
            this.button3.Text = "Cancel";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button2.Location = new System.Drawing.Point(746, 17);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(160, 30);
            this.button2.TabIndex = 4;
            this.button2.Text = "Batch Close R/MTL";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button1.Location = new System.Drawing.Point(895, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 30);
            this.button1.TabIndex = 8;
            this.button1.Text = "Find Now";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtSP1
            // 
            this.txtSP1.BackColor = System.Drawing.Color.White;
            this.txtSP1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP1.Location = new System.Drawing.Point(124, 90);
            this.txtSP1.Name = "txtSP1";
            this.txtSP1.Size = new System.Drawing.Size(122, 23);
            this.txtSP1.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnToEexcel);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 477);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1008, 53);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            // 
            // btnToEexcel
            // 
            this.btnToEexcel.Location = new System.Drawing.Point(650, 17);
            this.btnToEexcel.Name = "btnToEexcel";
            this.btnToEexcel.Size = new System.Drawing.Size(90, 30);
            this.btnToEexcel.TabIndex = 3;
            this.btnToEexcel.Text = "To Excel";
            this.btnToEexcel.UseVisualStyleBackColor = true;
            this.btnToEexcel.Click += new System.EventHandler(this.btnToEexcel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtstyle1);
            this.groupBox1.Controls.Add(this.txtmfactory1);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtbrand1);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.PullOutdateRange);
            this.groupBox1.Controls.Add(this.BuyerDeliverydateRange);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cbbCategory);
            this.groupBox1.Controls.Add(this.txtSP2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.txtSP1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1008, 135);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // txtstyle1
            // 
            this.txtstyle1.BackColor = System.Drawing.Color.White;
            this.txtstyle1.BrandObjectName = null;
            this.txtstyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle1.Location = new System.Drawing.Point(535, 54);
            this.txtstyle1.Name = "txtstyle1";
            this.txtstyle1.Size = new System.Drawing.Size(130, 23);
            this.txtstyle1.TabIndex = 5;
            // 
            // txtmfactory1
            // 
            this.txtmfactory1.BackColor = System.Drawing.Color.White;
            this.txtmfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtmfactory1.Location = new System.Drawing.Point(799, 18);
            this.txtmfactory1.Name = "txtmfactory1";
            this.txtmfactory1.Size = new System.Drawing.Size(66, 23);
            this.txtmfactory1.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(684, 19);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(112, 23);
            this.label8.TabIndex = 106;
            this.label8.Text = "Factory";
            // 
            // txtbrand1
            // 
            this.txtbrand1.BackColor = System.Drawing.Color.White;
            this.txtbrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand1.Location = new System.Drawing.Point(535, 90);
            this.txtbrand1.Name = "txtbrand1";
            this.txtbrand1.Size = new System.Drawing.Size(66, 23);
            this.txtbrand1.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(420, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(112, 23);
            this.label6.TabIndex = 104;
            this.label6.Text = "Brand";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(420, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 23);
            this.label1.TabIndex = 103;
            this.label1.Text = "Style";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(9, 19);
            this.label4.Name = "label4";
            this.label4.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label4.Size = new System.Drawing.Size(112, 23);
            this.label4.TabIndex = 102;
            this.label4.Text = "PullOut Date";
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // PullOutdateRange
            // 
            this.PullOutdateRange.Location = new System.Drawing.Point(124, 19);
            this.PullOutdateRange.Name = "PullOutdateRange";
            this.PullOutdateRange.Size = new System.Drawing.Size(280, 23);
            this.PullOutdateRange.TabIndex = 0;
            // 
            // BuyerDeliverydateRange
            // 
            this.BuyerDeliverydateRange.Location = new System.Drawing.Point(124, 54);
            this.BuyerDeliverydateRange.Name = "BuyerDeliverydateRange";
            this.BuyerDeliverydateRange.Size = new System.Drawing.Size(280, 23);
            this.BuyerDeliverydateRange.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(9, 54);
            this.label3.Name = "label3";
            this.label3.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label3.Size = new System.Drawing.Size(112, 23);
            this.label3.TabIndex = 99;
            this.label3.Text = "Buyer Delivery";
            this.label3.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(9, 90);
            this.label2.Name = "label2";
            this.label2.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label2.Size = new System.Drawing.Size(112, 23);
            this.label2.TabIndex = 97;
            this.label2.Text = "SP#";
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(420, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(112, 23);
            this.label7.TabIndex = 12;
            this.label7.Text = "Category";
            // 
            // cbbCategory
            // 
            this.cbbCategory.BackColor = System.Drawing.Color.White;
            this.cbbCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbbCategory.FormattingEnabled = true;
            this.cbbCategory.IsSupportUnselect = true;
            this.cbbCategory.Items.AddRange(new object[] {
            "ALL",
            "Bulk",
            "Sample",
            "Material"});
            this.cbbCategory.Location = new System.Drawing.Point(535, 18);
            this.cbbCategory.Name = "cbbCategory";
            this.cbbCategory.Size = new System.Drawing.Size(121, 24);
            this.cbbCategory.TabIndex = 4;
            // 
            // txtSP2
            // 
            this.txtSP2.BackColor = System.Drawing.Color.White;
            this.txtSP2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP2.Location = new System.Drawing.Point(252, 90);
            this.txtSP2.Name = "txtSP2";
            this.txtSP2.Size = new System.Drawing.Size(116, 23);
            this.txtSP2.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grid1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 135);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 342);
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
            this.grid1.Size = new System.Drawing.Size(1008, 342);
            this.grid1.TabIndex = 0;
            this.grid1.TabStop = false;
            // 
            // P01_BatchCloseRowMaterial
            // 
            this.ClientSize = new System.Drawing.Size(1008, 530);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "P01_BatchCloseRowMaterial";
            this.Text = "P01. Batch Close Row Material";
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button button3;
        private Win.UI.Button button2;
        private Win.UI.Button button1;
        private Win.UI.TextBox txtSP1;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Panel panel1;
        private Win.UI.Grid grid1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.TextBox txtSP2;
        private Win.UI.Label label7;
        private Win.UI.ComboBox cbbCategory;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Class.txtstyle txtstyle1;
        private Class.txtmfactory txtmfactory1;
        private Win.UI.Label label8;
        private Class.txtbrand txtbrand1;
        private Win.UI.Label label6;
        private Win.UI.Label label1;
        private Win.UI.Label label4;
        private Win.UI.DateRange PullOutdateRange;
        private Win.UI.DateRange BuyerDeliverydateRange;
        private Win.UI.Button btnToEexcel;
    }
}
