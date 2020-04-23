namespace Sci.Production.Subcon
{
    partial class P42
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnQuery = new Sci.Win.UI.Button();
            this.dateInline = new Sci.Win.UI.DateRange();
            this.label1 = new Sci.Win.UI.Label();
            this.txtSp2 = new Sci.Win.UI.TextBox();
            this.txtSp1 = new Sci.Win.UI.TextBox();
            this.grid1 = new Sci.Win.UI.Grid();
            this.txtPO = new Sci.Win.UI.TextBox();
            this.chkBulk = new Sci.Win.UI.CheckBox();
            this.chkSample = new Sci.Win.UI.CheckBox();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.label8 = new Sci.Win.UI.Label();
            this.cmbSummaryBy = new Sci.Win.UI.ComboBox();
            this.txtfactory1 = new Sci.Production.Class.txtfactory();
            this.txtMdivision1 = new Sci.Production.Class.txtMdivision();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.grid2 = new Sci.Win.UI.Grid();
            this.label9 = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label11 = new Sci.Win.UI.Label();
            this.label12 = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(916, 9);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 11;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // dateInline
            // 
            // 
            // 
            // 
            this.dateInline.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateInline.DateBox1.Name = "";
            this.dateInline.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateInline.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateInline.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateInline.DateBox2.Name = "";
            this.dateInline.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateInline.DateBox2.TabIndex = 1;
            this.dateInline.IsSupportEditMode = false;
            this.dateInline.Location = new System.Drawing.Point(613, 38);
            this.dateInline.Name = "dateInline";
            this.dateInline.Size = new System.Drawing.Size(280, 23);
            this.dateInline.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(215, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 23);
            this.label1.TabIndex = 26;
            this.label1.Text = "~";
            this.label1.TextStyle.Alignment = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // txtSp2
            // 
            this.txtSp2.BackColor = System.Drawing.Color.White;
            this.txtSp2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSp2.IsSupportEditMode = false;
            this.txtSp2.Location = new System.Drawing.Point(236, 9);
            this.txtSp2.Name = "txtSp2";
            this.txtSp2.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtSp2.Size = new System.Drawing.Size(127, 23);
            this.txtSp2.TabIndex = 1;
            // 
            // txtSp1
            // 
            this.txtSp1.BackColor = System.Drawing.Color.White;
            this.txtSp1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSp1.IsSupportEditMode = false;
            this.txtSp1.Location = new System.Drawing.Point(83, 9);
            this.txtSp1.Name = "txtSp1";
            this.txtSp1.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtSp1.Size = new System.Drawing.Size(127, 23);
            this.txtSp1.TabIndex = 0;
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(12, 121);
            this.grid1.Name = "grid1";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid1.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(984, 429);
            this.grid1.TabIndex = 42;
            // 
            // txtPO
            // 
            this.txtPO.BackColor = System.Drawing.Color.White;
            this.txtPO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPO.IsSupportEditMode = false;
            this.txtPO.Location = new System.Drawing.Point(83, 38);
            this.txtPO.Name = "txtPO";
            this.txtPO.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtPO.Size = new System.Drawing.Size(127, 23);
            this.txtPO.TabIndex = 2;
            // 
            // chkBulk
            // 
            this.chkBulk.AutoSize = true;
            this.chkBulk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkBulk.IsSupportEditMode = false;
            this.chkBulk.Location = new System.Drawing.Point(83, 68);
            this.chkBulk.Name = "chkBulk";
            this.chkBulk.Size = new System.Drawing.Size(54, 21);
            this.chkBulk.TabIndex = 3;
            this.chkBulk.Text = "Bulk";
            this.chkBulk.UseVisualStyleBackColor = true;
            // 
            // chkSample
            // 
            this.chkSample.AutoSize = true;
            this.chkSample.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkSample.IsSupportEditMode = false;
            this.chkSample.Location = new System.Drawing.Point(143, 68);
            this.chkSample.Name = "chkSample";
            this.chkSample.Size = new System.Drawing.Size(74, 21);
            this.chkSample.TabIndex = 4;
            this.chkSample.Text = "Sample";
            this.chkSample.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(5, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 47;
            this.label3.Text = "Category";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(366, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 48;
            this.label4.Text = "M";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(366, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 50;
            this.label5.Text = "Factory";
            // 
            // dateSCIDelivery
            // 
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSCIDelivery.DateBox1.Name = "";
            this.dateSCIDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSCIDelivery.DateBox2.Name = "";
            this.dateSCIDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDelivery.DateBox2.TabIndex = 1;
            this.dateSCIDelivery.IsSupportEditMode = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(613, 9);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 7;
            // 
            // dateBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerDelivery.DateBox1.Name = "";
            this.dateBuyerDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBuyerDelivery.DateBox2.Name = "";
            this.dateBuyerDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateBuyerDelivery.IsSupportEditMode = false;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(613, 67);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(513, 95);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 23);
            this.label8.TabIndex = 56;
            this.label8.Text = "Summary By";
            // 
            // cmbSummaryBy
            // 
            this.cmbSummaryBy.BackColor = System.Drawing.Color.White;
            this.cmbSummaryBy.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.cmbSummaryBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbSummaryBy.FormattingEnabled = true;
            this.cmbSummaryBy.IsSupportUnselect = true;
            this.cmbSummaryBy.Location = new System.Drawing.Point(613, 95);
            this.cmbSummaryBy.Name = "cmbSummaryBy";
            this.cmbSummaryBy.OldText = "";
            this.cmbSummaryBy.Size = new System.Drawing.Size(130, 24);
            this.cmbSummaryBy.TabIndex = 10;
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.boolFtyGroupList = true;
            this.txtfactory1.FilteMDivision = false;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.IsProduceFty = false;
            this.txtfactory1.IsSupportEditMode = false;
            this.txtfactory1.IssupportJunk = false;
            this.txtfactory1.Location = new System.Drawing.Point(444, 38);
            this.txtfactory1.MDivision = null;
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtfactory1.Size = new System.Drawing.Size(66, 23);
            this.txtfactory1.TabIndex = 6;
            // 
            // txtMdivision1
            // 
            this.txtMdivision1.BackColor = System.Drawing.Color.White;
            this.txtMdivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision1.IsSupportEditMode = false;
            this.txtMdivision1.Location = new System.Drawing.Point(444, 9);
            this.txtMdivision1.Name = "txtMdivision1";
            this.txtMdivision1.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtMdivision1.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision1.TabIndex = 5;
            // 
            // grid2
            // 
            this.grid2.AllowUserToAddRows = false;
            this.grid2.AllowUserToDeleteRows = false;
            this.grid2.AllowUserToResizeRows = false;
            this.grid2.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid2.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid2.DataSource = this.listControlBindingSource2;
            this.grid2.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid2.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid2.Location = new System.Drawing.Point(945, 85);
            this.grid2.Name = "grid2";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid2.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grid2.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid2.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid2.RowTemplate.Height = 24;
            this.grid2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid2.ShowCellToolTips = false;
            this.grid2.Size = new System.Drawing.Size(51, 17);
            this.grid2.TabIndex = 57;
            this.grid2.Visible = false;
            this.grid2.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grid2_CellFormatting);
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label9.Location = new System.Drawing.Point(5, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 23);
            this.label9.TabIndex = 101;
            this.label9.Text = "SP#";
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label10.Location = new System.Drawing.Point(5, 38);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(75, 23);
            this.label10.TabIndex = 102;
            this.label10.Text = "P.O.";
            this.label10.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label2.Location = new System.Drawing.Point(513, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 23);
            this.label2.TabIndex = 103;
            this.label2.Text = "SCI Delivery";
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label11.Location = new System.Drawing.Point(513, 38);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(97, 23);
            this.label11.TabIndex = 104;
            this.label11.Text = "Inline Date";
            this.label11.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label12.Location = new System.Drawing.Point(513, 67);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(97, 23);
            this.label12.TabIndex = 105;
            this.label12.Text = "Buyer Delivery";
            this.label12.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // P42
            // 
            this.ClientSize = new System.Drawing.Size(1008, 562);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.grid2);
            this.Controls.Add(this.cmbSummaryBy);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.dateBuyerDelivery);
            this.Controls.Add(this.dateSCIDelivery);
            this.Controls.Add(this.txtfactory1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtMdivision1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkSample);
            this.Controls.Add(this.chkBulk);
            this.Controls.Add(this.txtPO);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.dateInline);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSp2);
            this.Controls.Add(this.txtSp1);
            this.Name = "P42";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P42. SubProcess Overview";
            this.Controls.SetChildIndex(this.txtSp1, 0);
            this.Controls.SetChildIndex(this.txtSp2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.dateInline, 0);
            this.Controls.SetChildIndex(this.btnQuery, 0);
            this.Controls.SetChildIndex(this.grid1, 0);
            this.Controls.SetChildIndex(this.txtPO, 0);
            this.Controls.SetChildIndex(this.chkBulk, 0);
            this.Controls.SetChildIndex(this.chkSample, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.txtMdivision1, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.txtfactory1, 0);
            this.Controls.SetChildIndex(this.dateSCIDelivery, 0);
            this.Controls.SetChildIndex(this.dateBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.cmbSummaryBy, 0);
            this.Controls.SetChildIndex(this.grid2, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label11, 0);
            this.Controls.SetChildIndex(this.label12, 0);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Button btnQuery;
        private Win.UI.DateRange dateInline;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtSp2;
        private Win.UI.TextBox txtSp1;
        private Win.UI.Grid grid1;
        private Win.UI.TextBox txtPO;
        private Win.UI.CheckBox chkBulk;
        private Win.UI.CheckBox chkSample;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Class.txtMdivision txtMdivision1;
        private Win.UI.Label label5;
        private Class.txtfactory txtfactory1;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.Label label8;
        private Win.UI.ComboBox cmbSummaryBy;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private Win.UI.Grid grid2;
        private Win.UI.Label label9;
        private Win.UI.Label label10;
        private Win.UI.Label label2;
        private Win.UI.Label label11;
        private Win.UI.Label label12;
    }
}
