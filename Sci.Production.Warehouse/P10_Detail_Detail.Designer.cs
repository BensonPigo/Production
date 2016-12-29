namespace Sci.Production.Warehouse
{
    partial class P10_Detail_Detail
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
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.num_variance = new Sci.Win.UI.NumericBox();
            this.label9 = new Sci.Win.UI.Label();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.label2 = new Sci.Win.UI.Label();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.dis_sizespec = new Sci.Win.UI.DisplayBox();
            this.label11 = new Sci.Win.UI.Label();
            this.dis_colorid = new Sci.Win.UI.DisplayBox();
            this.dis_desc = new Sci.Win.UI.DisplayBox();
            this.label4 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.dis_poid = new Sci.Win.UI.DisplayBox();
            this.label3 = new Sci.Win.UI.Label();
            this.dis_scirefno = new Sci.Win.UI.DisplayBox();
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
            this.button3.Location = new System.Drawing.Point(912, 15);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(90, 30);
            this.button3.TabIndex = 3;
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
            this.button2.TabIndex = 2;
            this.button2.Text = "Import";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.num_variance);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.displayBox1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 477);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1008, 53);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            // 
            // num_variance
            // 
            this.num_variance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.num_variance.DecimalPlaces = 2;
            this.num_variance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.num_variance.IsSupportEditMode = false;
            this.num_variance.Location = new System.Drawing.Point(311, 21);
            this.num_variance.Name = "num_variance";
            this.num_variance.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.num_variance.ReadOnly = true;
            this.num_variance.Size = new System.Drawing.Size(111, 23);
            this.num_variance.TabIndex = 115;
            this.num_variance.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(181, 21);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(127, 23);
            this.label9.TabIndex = 114;
            this.label9.Text = "Request Variance";
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(537, 22);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(100, 23);
            this.displayBox1.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(439, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Total Qty";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dis_sizespec);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.dis_colorid);
            this.groupBox1.Controls.Add(this.dis_desc);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.dis_poid);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dis_scirefno);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1008, 91);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // dis_sizespec
            // 
            this.dis_sizespec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dis_sizespec.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dis_sizespec.Location = new System.Drawing.Point(762, 19);
            this.dis_sizespec.Name = "dis_sizespec";
            this.dis_sizespec.Size = new System.Drawing.Size(124, 23);
            this.dis_sizespec.TabIndex = 120;
            // 
            // label11
            // 
            this.label11.Lines = 0;
            this.label11.Location = new System.Drawing.Point(684, 19);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 23);
            this.label11.TabIndex = 119;
            this.label11.Text = "SizeSpec";
            // 
            // dis_colorid
            // 
            this.dis_colorid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dis_colorid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dis_colorid.Location = new System.Drawing.Point(537, 19);
            this.dis_colorid.Name = "dis_colorid";
            this.dis_colorid.Size = new System.Drawing.Size(124, 23);
            this.dis_colorid.TabIndex = 116;
            // 
            // dis_desc
            // 
            this.dis_desc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dis_desc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dis_desc.Location = new System.Drawing.Point(84, 51);
            this.dis_desc.Name = "dis_desc";
            this.dis_desc.Size = new System.Drawing.Size(802, 23);
            this.dis_desc.TabIndex = 118;
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(459, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 115;
            this.label4.Text = "ColorID";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(6, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 111;
            this.label1.Text = "SP#";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(6, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 117;
            this.label5.Text = "Desc";
            // 
            // dis_poid
            // 
            this.dis_poid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dis_poid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dis_poid.Location = new System.Drawing.Point(84, 19);
            this.dis_poid.Name = "dis_poid";
            this.dis_poid.Size = new System.Drawing.Size(124, 23);
            this.dis_poid.TabIndex = 112;
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(233, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 113;
            this.label3.Text = "SciRefno";
            // 
            // dis_scirefno
            // 
            this.dis_scirefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dis_scirefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dis_scirefno.Location = new System.Drawing.Point(311, 19);
            this.dis_scirefno.Name = "dis_scirefno";
            this.dis_scirefno.Size = new System.Drawing.Size(124, 23);
            this.dis_scirefno.TabIndex = 114;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grid1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 91);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 386);
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
            this.grid1.Size = new System.Drawing.Size(1008, 386);
            this.grid1.TabIndex = 0;
            this.grid1.TabStop = false;
            // 
            // P10_Detail_Detail
            // 
            this.ClientSize = new System.Drawing.Size(1008, 530);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "P10_Detail_Detail";
            this.Text = "Roll#";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private Win.UI.GroupBox groupBox2;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Panel panel1;
        private Win.UI.Grid grid1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.DisplayBox displayBox1;
        private Win.UI.Label label2;
        private Win.UI.DisplayBox dis_sizespec;
        private Win.UI.Label label11;
        private Win.UI.DisplayBox dis_colorid;
        private Win.UI.DisplayBox dis_desc;
        private Win.UI.Label label4;
        private Win.UI.Label label1;
        private Win.UI.Label label5;
        private Win.UI.DisplayBox dis_poid;
        private Win.UI.Label label3;
        private Win.UI.DisplayBox dis_scirefno;
        private Win.UI.NumericBox num_variance;
        private Win.UI.Label label9;
    }
}
