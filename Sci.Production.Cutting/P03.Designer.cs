namespace Sci.Production.Cutting
{
    partial class P03
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.button_update = new Sci.Win.UI.Button();
            this.textBox_cutref = new Sci.Win.UI.TextBox();
            this.textBox_seq = new Sci.Win.UI.TextBox();
            this.button_Query = new Sci.Win.UI.Button();
            this.label6 = new Sci.Win.UI.Label();
            this.dateBox_newestcutdate = new Sci.Win.UI.DateBox();
            this.dateBox_sewinginline = new Sci.Win.UI.DateBox();
            this.dateBox_estcutdate = new Sci.Win.UI.DateBox();
            this.textBox_sp = new Sci.Win.UI.TextBox();
            this.textBox_Cutsp = new Sci.Win.UI.TextBox();
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.detailgrid = new Sci.Win.UI.Grid();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.txtcutReason1 = new Sci.Production.Class.txtcutReason();
            this.button_save = new Sci.Win.UI.Button();
            this.button_close = new Sci.Win.UI.Button();
            this.gridbs = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            this.SuspendLayout();
            // 
            // button_update
            // 
            this.button_update.Location = new System.Drawing.Point(900, 16);
            this.button_update.Name = "button_update";
            this.button_update.Size = new System.Drawing.Size(92, 30);
            this.button_update.TabIndex = 8;
            this.button_update.Text = "Update";
            this.button_update.UseVisualStyleBackColor = true;
            this.button_update.Click += new System.EventHandler(this.button_update_Click);
            // 
            // textBox_cutref
            // 
            this.textBox_cutref.BackColor = System.Drawing.Color.White;
            this.textBox_cutref.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox_cutref.Location = new System.Drawing.Point(488, 51);
            this.textBox_cutref.Name = "textBox_cutref";
            this.textBox_cutref.Size = new System.Drawing.Size(78, 23);
            this.textBox_cutref.TabIndex = 5;
            // 
            // textBox_seq
            // 
            this.textBox_seq.BackColor = System.Drawing.Color.White;
            this.textBox_seq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox_seq.Location = new System.Drawing.Point(302, 51);
            this.textBox_seq.Mask = "000-00";
            this.textBox_seq.Name = "textBox_seq";
            this.textBox_seq.Size = new System.Drawing.Size(67, 23);
            this.textBox_seq.TabIndex = 3;
            this.textBox_seq.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            // 
            // button_Query
            // 
            this.button_Query.Location = new System.Drawing.Point(572, 48);
            this.button_Query.Name = "button_Query";
            this.button_Query.Size = new System.Drawing.Size(80, 30);
            this.button_Query.TabIndex = 6;
            this.button_Query.Text = "Query";
            this.button_Query.UseVisualStyleBackColor = true;
            this.button_Query.Click += new System.EventHandler(this.button_Query_Click);
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(669, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 23);
            this.label6.TabIndex = 32;
            this.label6.Text = "Est. Cut Date";
            // 
            // dateBox_newestcutdate
            // 
            this.dateBox_newestcutdate.Location = new System.Drawing.Point(761, 19);
            this.dateBox_newestcutdate.Name = "dateBox_newestcutdate";
            this.dateBox_newestcutdate.Size = new System.Drawing.Size(130, 23);
            this.dateBox_newestcutdate.TabIndex = 7;
            this.dateBox_newestcutdate.Validating += new System.ComponentModel.CancelEventHandler(this.dateBox_newestcutdate_Validating);
            // 
            // dateBox_sewinginline
            // 
            this.dateBox_sewinginline.Location = new System.Drawing.Point(511, 19);
            this.dateBox_sewinginline.Name = "dateBox_sewinginline";
            this.dateBox_sewinginline.Size = new System.Drawing.Size(130, 23);
            this.dateBox_sewinginline.TabIndex = 4;
            // 
            // dateBox_estcutdate
            // 
            this.dateBox_estcutdate.Location = new System.Drawing.Point(104, 51);
            this.dateBox_estcutdate.Name = "dateBox_estcutdate";
            this.dateBox_estcutdate.Size = new System.Drawing.Size(130, 23);
            this.dateBox_estcutdate.TabIndex = 1;
            // 
            // textBox_sp
            // 
            this.textBox_sp.BackColor = System.Drawing.Color.White;
            this.textBox_sp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox_sp.Location = new System.Drawing.Point(302, 19);
            this.textBox_sp.Name = "textBox_sp";
            this.textBox_sp.Size = new System.Drawing.Size(108, 23);
            this.textBox_sp.TabIndex = 2;
            // 
            // textBox_Cutsp
            // 
            this.textBox_Cutsp.BackColor = System.Drawing.Color.White;
            this.textBox_Cutsp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox_Cutsp.Location = new System.Drawing.Point(104, 19);
            this.textBox_Cutsp.Name = "textBox_Cutsp";
            this.textBox_Cutsp.Size = new System.Drawing.Size(108, 23);
            this.textBox_Cutsp.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(420, 51);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 23);
            this.label7.TabIndex = 26;
            this.label7.Text = "Cut Ref#";
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(669, 51);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 23);
            this.label8.TabIndex = 25;
            this.label8.Text = "Reason";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(243, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 23);
            this.label5.TabIndex = 24;
            this.label5.Text = "SEQ";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(244, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 23);
            this.label4.TabIndex = 23;
            this.label4.Text = "SP#";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(420, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 23);
            this.label3.TabIndex = 22;
            this.label3.Text = "Sewing inline";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(12, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 23);
            this.label2.TabIndex = 21;
            this.label2.Text = "Est. Cut Date";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 23);
            this.label1.TabIndex = 20;
            this.label1.Text = "Cutting SP#";
            // 
            // detailgrid
            // 
            this.detailgrid.AllowUserToAddRows = false;
            this.detailgrid.AllowUserToDeleteRows = false;
            this.detailgrid.AllowUserToResizeRows = false;
            this.detailgrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.detailgrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.detailgrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.detailgrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.detailgrid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.detailgrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.detailgrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.detailgrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.detailgrid.Location = new System.Drawing.Point(8, 88);
            this.detailgrid.Name = "detailgrid";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.detailgrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.detailgrid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.detailgrid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.detailgrid.RowTemplate.Height = 24;
            this.detailgrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.detailgrid.Size = new System.Drawing.Size(992, 311);
            this.detailgrid.TabIndex = 9;
            this.detailgrid.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(655, 74);
            this.groupBox1.TabIndex = 37;
            this.groupBox1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtcutReason1);
            this.groupBox2.Location = new System.Drawing.Point(662, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(337, 74);
            this.groupBox2.TabIndex = 38;
            this.groupBox2.TabStop = false;
            // 
            // txtcutReason1
            // 
            this.txtcutReason1.DisplayBox1Binding = "";
            this.txtcutReason1.Location = new System.Drawing.Point(98, 43);
            this.txtcutReason1.Name = "txtcutReason1";
            this.txtcutReason1.Size = new System.Drawing.Size(234, 27);
            this.txtcutReason1.TabIndex = 0;
            this.txtcutReason1.TextBox1Binding = "";
            this.txtcutReason1.Type = "RC";
            // 
            // button_save
            // 
            this.button_save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_save.Location = new System.Drawing.Point(810, 405);
            this.button_save.Name = "button_save";
            this.button_save.Size = new System.Drawing.Size(92, 30);
            this.button_save.TabIndex = 39;
            this.button_save.Text = "Save";
            this.button_save.UseVisualStyleBackColor = true;
            this.button_save.Click += new System.EventHandler(this.button_save_Click);
            // 
            // button_close
            // 
            this.button_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_close.Location = new System.Drawing.Point(908, 405);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(92, 30);
            this.button_close.TabIndex = 40;
            this.button_close.Text = "Close";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.button_close_Click);
            // 
            // P03
            // 
            this.ClientSize = new System.Drawing.Size(1006, 439);
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.button_save);
            this.Controls.Add(this.button_update);
            this.Controls.Add(this.textBox_cutref);
            this.Controls.Add(this.textBox_seq);
            this.Controls.Add(this.button_Query);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dateBox_newestcutdate);
            this.Controls.Add(this.dateBox_sewinginline);
            this.Controls.Add(this.dateBox_estcutdate);
            this.Controls.Add(this.textBox_sp);
            this.Controls.Add(this.textBox_Cutsp);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.detailgrid);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.EditMode = true;
            this.Name = "P03";
            this.Text = "P03.Change Est. Cut Date after finished Cutting Daily Plan";
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.detailgrid, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.textBox_Cutsp, 0);
            this.Controls.SetChildIndex(this.textBox_sp, 0);
            this.Controls.SetChildIndex(this.dateBox_estcutdate, 0);
            this.Controls.SetChildIndex(this.dateBox_sewinginline, 0);
            this.Controls.SetChildIndex(this.dateBox_newestcutdate, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.button_Query, 0);
            this.Controls.SetChildIndex(this.textBox_seq, 0);
            this.Controls.SetChildIndex(this.textBox_cutref, 0);
            this.Controls.SetChildIndex(this.button_update, 0);
            this.Controls.SetChildIndex(this.button_save, 0);
            this.Controls.SetChildIndex(this.button_close, 0);
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Button button_update;
        private Win.UI.TextBox textBox_cutref;
        private Win.UI.TextBox textBox_seq;
        private Win.UI.Button button_Query;
        private Win.UI.Label label6;
        private Win.UI.DateBox dateBox_newestcutdate;
        private Win.UI.DateBox dateBox_sewinginline;
        private Win.UI.DateBox dateBox_estcutdate;
        private Win.UI.TextBox textBox_sp;
        private Win.UI.TextBox textBox_Cutsp;
        private Win.UI.Label label7;
        private Win.UI.Label label8;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.Grid detailgrid;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.Button button_save;
        private Win.UI.Button button_close;
        private Class.txtcutReason txtcutReason1;
        private Win.UI.ListControlBindingSource gridbs;
    }
}
