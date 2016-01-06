namespace Sci.Production.Warehouse
{
    partial class P11_Detail
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.dis_seq = new Sci.Win.UI.DisplayBox();
            this.dis_sizeunit = new Sci.Win.UI.DisplayBox();
            this.dis_colorid = new Sci.Win.UI.DisplayBox();
            this.label3 = new Sci.Win.UI.Label();
            this.dis_special = new Sci.Win.UI.DisplayBox();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.eb_desc = new Sci.Win.UI.EditBox();
            this.eb_orderlist = new Sci.Win.UI.EditBox();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.dis_usedqty = new Sci.Win.UI.DisplayBox();
            this.dis_sizespec = new Sci.Win.UI.DisplayBox();
            this.label11 = new Sci.Win.UI.Label();
            this.grid1 = new Sci.Win.UI.Grid();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(560, 223);
            this.gridcont.Size = new System.Drawing.Size(436, 284);
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 517);
            this.btmcont.Size = new System.Drawing.Size(1008, 40);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(838, 5);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(918, 5);
            // 
            // next
            // 
            this.next.Location = new System.Drawing.Point(783, 5);
            // 
            // prev
            // 
            this.prev.Location = new System.Drawing.Point(728, 5);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(9, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 99;
            this.label1.Text = "Seq#";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(252, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 100;
            this.label2.Text = "Unit";
            // 
            // dis_seq
            // 
            this.dis_seq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dis_seq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dis_seq.Location = new System.Drawing.Point(87, 22);
            this.dis_seq.Name = "dis_seq";
            this.dis_seq.Size = new System.Drawing.Size(124, 23);
            this.dis_seq.TabIndex = 101;
            // 
            // dis_sizeunit
            // 
            this.dis_sizeunit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dis_sizeunit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dis_sizeunit.Location = new System.Drawing.Point(330, 22);
            this.dis_sizeunit.Name = "dis_sizeunit";
            this.dis_sizeunit.Size = new System.Drawing.Size(124, 23);
            this.dis_sizeunit.TabIndex = 102;
            // 
            // dis_colorid
            // 
            this.dis_colorid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dis_colorid.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dis_colorid.Location = new System.Drawing.Point(87, 92);
            this.dis_colorid.Name = "dis_colorid";
            this.dis_colorid.Size = new System.Drawing.Size(124, 23);
            this.dis_colorid.TabIndex = 104;
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(9, 92);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 103;
            this.label3.Text = "Color";
            // 
            // dis_special
            // 
            this.dis_special.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dis_special.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dis_special.Location = new System.Drawing.Point(87, 127);
            this.dis_special.Name = "dis_special";
            this.dis_special.Size = new System.Drawing.Size(124, 23);
            this.dis_special.TabIndex = 106;
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(9, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 105;
            this.label4.Text = "Special";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(9, 220);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 107;
            this.label5.Text = "Desc";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.eb_desc);
            this.groupBox1.Controls.Add(this.eb_orderlist);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.dis_usedqty);
            this.groupBox1.Controls.Add(this.dis_sizespec);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dis_special);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.dis_seq);
            this.groupBox1.Controls.Add(this.dis_sizeunit);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dis_colorid);
            this.groupBox1.Location = new System.Drawing.Point(12, 223);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(542, 284);
            this.groupBox1.TabIndex = 110;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Isse Item";
            // 
            // eb_desc
            // 
            this.eb_desc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.eb_desc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.eb_desc.IsSupportEditMode = false;
            this.eb_desc.Location = new System.Drawing.Point(87, 220);
            this.eb_desc.Multiline = true;
            this.eb_desc.Name = "eb_desc";
            this.eb_desc.ReadOnly = true;
            this.eb_desc.Size = new System.Drawing.Size(449, 50);
            this.eb_desc.TabIndex = 116;
            // 
            // eb_orderlist
            // 
            this.eb_orderlist.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.eb_orderlist.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.eb_orderlist.IsSupportEditMode = false;
            this.eb_orderlist.Location = new System.Drawing.Point(87, 162);
            this.eb_orderlist.Multiline = true;
            this.eb_orderlist.Name = "eb_orderlist";
            this.eb_orderlist.ReadOnly = true;
            this.eb_orderlist.Size = new System.Drawing.Size(449, 50);
            this.eb_orderlist.TabIndex = 115;
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(9, 57);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 23);
            this.label6.TabIndex = 111;
            this.label6.Text = "@Qty";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(252, 57);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 23);
            this.label7.TabIndex = 112;
            this.label7.Text = "Size";
            // 
            // dis_usedqty
            // 
            this.dis_usedqty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dis_usedqty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dis_usedqty.Location = new System.Drawing.Point(87, 57);
            this.dis_usedqty.Name = "dis_usedqty";
            this.dis_usedqty.Size = new System.Drawing.Size(124, 23);
            this.dis_usedqty.TabIndex = 113;
            // 
            // dis_sizespec
            // 
            this.dis_sizespec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.dis_sizespec.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.dis_sizespec.Location = new System.Drawing.Point(330, 57);
            this.dis_sizespec.Name = "dis_sizespec";
            this.dis_sizespec.Size = new System.Drawing.Size(124, 23);
            this.dis_sizespec.TabIndex = 114;
            // 
            // label11
            // 
            this.label11.Lines = 0;
            this.label11.Location = new System.Drawing.Point(9, 162);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 23);
            this.label11.TabIndex = 109;
            this.label11.Text = "Order List";
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
            this.grid1.Location = new System.Drawing.Point(10, 12);
            this.grid1.Name = "grid1";
            this.grid1.RowHeadersVisible = false;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(986, 205);
            this.grid1.TabIndex = 111;
            this.grid1.TabStop = false;
            // 
            // P11_Detail
            // 
            this.ClientSize = new System.Drawing.Size(1008, 557);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.groupBox1);
            this.Name = "P11_Detail";
            this.Text = "P11. Output Detail";
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.grid1, 0);
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.DisplayBox dis_seq;
        private Win.UI.DisplayBox dis_sizeunit;
        private Win.UI.DisplayBox dis_colorid;
        private Win.UI.Label label3;
        private Win.UI.DisplayBox dis_special;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Label label11;
        private Win.UI.EditBox eb_desc;
        private Win.UI.EditBox eb_orderlist;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Win.UI.DisplayBox dis_usedqty;
        private Win.UI.DisplayBox dis_sizespec;
        private Win.UI.Grid grid1;
    }
}
