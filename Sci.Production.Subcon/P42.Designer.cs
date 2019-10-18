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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnQuery = new Sci.Win.UI.Button();
            this.dateInline = new Sci.Win.UI.DateRange();
            this.label1 = new Sci.Win.UI.Label();
            this.txtSp2 = new Sci.Win.UI.TextBox();
            this.txtSp1 = new Sci.Win.UI.TextBox();
            this.labInlineDate = new Sci.Win.UI.Label();
            this.labSpno = new Sci.Win.UI.Label();
            this.grid1 = new Sci.Win.UI.Grid();
            this.txtPO = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.chkBulk = new Sci.Win.UI.CheckBox();
            this.chkSample = new Sci.Win.UI.CheckBox();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.label7 = new Sci.Win.UI.Label();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.label8 = new Sci.Win.UI.Label();
            this.cmbSummaryBy = new Sci.Win.UI.ComboBox();
            this.txtfactory1 = new Sci.Production.Class.txtfactory();
            this.txtMdivision1 = new Sci.Production.Class.txtMdivision();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.grid2 = new Sci.Win.UI.Grid();
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
            // labInlineDate
            // 
            this.labInlineDate.Location = new System.Drawing.Point(513, 38);
            this.labInlineDate.Name = "labInlineDate";
            this.labInlineDate.Size = new System.Drawing.Size(97, 23);
            this.labInlineDate.TabIndex = 22;
            this.labInlineDate.Text = "Inline Date";
            // 
            // labSpno
            // 
            this.labSpno.Location = new System.Drawing.Point(5, 9);
            this.labSpno.Name = "labSpno";
            this.labSpno.Size = new System.Drawing.Size(75, 23);
            this.labSpno.TabIndex = 21;
            this.labSpno.Text = "SP#";
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
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(5, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 44;
            this.label2.Text = "P.O.";
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
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(513, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 23);
            this.label6.TabIndex = 52;
            this.label6.Text = "SCI Delivery";
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
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(513, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 23);
            this.label7.TabIndex = 54;
            this.label7.Text = "Buyer Delivery";
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
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.grid2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid2.DataSource = this.listControlBindingSource2;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grid2.DefaultCellStyle = dataGridViewCellStyle3;
            this.grid2.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid2.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid2.Location = new System.Drawing.Point(945, 85);
            this.grid2.Name = "grid2";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid2.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
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
            // P42
            // 
            this.ClientSize = new System.Drawing.Size(1008, 562);
            this.Controls.Add(this.grid2);
            this.Controls.Add(this.cmbSummaryBy);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.dateBuyerDelivery);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dateSCIDelivery);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtfactory1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtMdivision1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chkSample);
            this.Controls.Add(this.chkBulk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPO);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.dateInline);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSp2);
            this.Controls.Add(this.txtSp1);
            this.Controls.Add(this.labInlineDate);
            this.Controls.Add(this.labSpno);
            this.Name = "P42";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P42. SubProcess Overview";
            this.Controls.SetChildIndex(this.labSpno, 0);
            this.Controls.SetChildIndex(this.labInlineDate, 0);
            this.Controls.SetChildIndex(this.txtSp1, 0);
            this.Controls.SetChildIndex(this.txtSp2, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.dateInline, 0);
            this.Controls.SetChildIndex(this.btnQuery, 0);
            this.Controls.SetChildIndex(this.grid1, 0);
            this.Controls.SetChildIndex(this.txtPO, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.chkBulk, 0);
            this.Controls.SetChildIndex(this.chkSample, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.txtMdivision1, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.txtfactory1, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.dateSCIDelivery, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.dateBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.cmbSummaryBy, 0);
            this.Controls.SetChildIndex(this.grid2, 0);
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
        private Win.UI.Label labInlineDate;
        private Win.UI.Label labSpno;
        private Win.UI.Grid grid1;
        private Win.UI.TextBox txtPO;
        private Win.UI.Label label2;
        private Win.UI.CheckBox chkBulk;
        private Win.UI.CheckBox chkSample;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Class.txtMdivision txtMdivision1;
        private Win.UI.Label label5;
        private Class.txtfactory txtfactory1;
        private Win.UI.Label label6;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.Label label7;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.Label label8;
        private Win.UI.ComboBox cmbSummaryBy;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private Win.UI.Grid grid2;
    }
}
