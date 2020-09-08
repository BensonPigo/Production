namespace Sci.Production.Cutting
{
    partial class P14
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
            this.txtBundleNo = new Sci.Win.UI.TextBox();
            this.cmdComboType = new Sci.Win.UI.ComboBox();
            this.txtCardNo = new Sci.Win.UI.TextBox();
            this.disCutpart = new Sci.Win.UI.DisplayBox();
            this.disCutNo = new Sci.Win.UI.DisplayBox();
            this.disArticle = new Sci.Win.UI.DisplayBox();
            this.disColor = new Sci.Win.UI.DisplayBox();
            this.disSize = new Sci.Win.UI.DisplayBox();
            this.disBundleQty = new Sci.Win.UI.NumericBox();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.grid1 = new Sci.Win.UI.Grid();
            this.disSP = new Sci.Win.UI.EditBox();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(17, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Card No";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(17, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Bundle No";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(17, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Combo Type";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(17, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "SP#";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(17, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "Cutpart";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(266, 129);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 23);
            this.label6.TabIndex = 9;
            this.label6.Text = "Bundle Qty";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(266, 100);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(96, 23);
            this.label7.TabIndex = 8;
            this.label7.Text = "Size";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(266, 71);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 23);
            this.label8.TabIndex = 7;
            this.label8.Text = "Color";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(266, 41);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(96, 23);
            this.label9.TabIndex = 6;
            this.label9.Text = "Article";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(266, 12);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 23);
            this.label10.TabIndex = 5;
            this.label10.Text = "Cut No";
            // 
            // txtBundleNo
            // 
            this.txtBundleNo.BackColor = System.Drawing.Color.White;
            this.txtBundleNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBundleNo.IsSupportEditMode = false;
            this.txtBundleNo.Location = new System.Drawing.Point(116, 12);
            this.txtBundleNo.Name = "txtBundleNo";
            this.txtBundleNo.Size = new System.Drawing.Size(147, 23);
            this.txtBundleNo.TabIndex = 0;
            this.txtBundleNo.Validating += new System.ComponentModel.CancelEventHandler(this.TxtBundleNo_Validating);
            this.txtBundleNo.Validated += new System.EventHandler(this.TxtCardNoBundleNoComboType_Validated);
            // 
            // cmdComboType
            // 
            this.cmdComboType.BackColor = System.Drawing.Color.White;
            this.cmdComboType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmdComboType.FormattingEnabled = true;
            this.cmdComboType.IsSupportUnselect = true;
            this.cmdComboType.Location = new System.Drawing.Point(116, 41);
            this.cmdComboType.Name = "cmdComboType";
            this.cmdComboType.OldText = "";
            this.cmdComboType.Size = new System.Drawing.Size(147, 24);
            this.cmdComboType.TabIndex = 1;
            this.cmdComboType.SelectedIndexChanged += new System.EventHandler(this.CmdComboType_SelectedIndexChanged);
            // 
            // txtCardNo
            // 
            this.txtCardNo.BackColor = System.Drawing.Color.White;
            this.txtCardNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCardNo.Location = new System.Drawing.Point(116, 71);
            this.txtCardNo.Name = "txtCardNo";
            this.txtCardNo.Size = new System.Drawing.Size(147, 23);
            this.txtCardNo.TabIndex = 2;
            this.txtCardNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtCardNo_KeyPress);
            this.txtCardNo.Validated += new System.EventHandler(this.TxtCardNoBundleNoComboType_Validated);
            // 
            // disCutpart
            // 
            this.disCutpart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.disCutpart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.disCutpart.Location = new System.Drawing.Point(116, 129);
            this.disCutpart.Name = "disCutpart";
            this.disCutpart.Size = new System.Drawing.Size(147, 23);
            this.disCutpart.TabIndex = 22;
            // 
            // disCutNo
            // 
            this.disCutNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.disCutNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.disCutNo.Location = new System.Drawing.Point(365, 12);
            this.disCutNo.Name = "disCutNo";
            this.disCutNo.Size = new System.Drawing.Size(147, 23);
            this.disCutNo.TabIndex = 23;
            // 
            // disArticle
            // 
            this.disArticle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.disArticle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.disArticle.Location = new System.Drawing.Point(365, 41);
            this.disArticle.Name = "disArticle";
            this.disArticle.Size = new System.Drawing.Size(147, 23);
            this.disArticle.TabIndex = 24;
            // 
            // disColor
            // 
            this.disColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.disColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.disColor.Location = new System.Drawing.Point(365, 70);
            this.disColor.Name = "disColor";
            this.disColor.Size = new System.Drawing.Size(147, 23);
            this.disColor.TabIndex = 25;
            // 
            // disSize
            // 
            this.disSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.disSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.disSize.Location = new System.Drawing.Point(365, 100);
            this.disSize.Name = "disSize";
            this.disSize.Size = new System.Drawing.Size(147, 23);
            this.disSize.TabIndex = 26;
            // 
            // disBundleQty
            // 
            this.disBundleQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.disBundleQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.disBundleQty.IsSupportEditMode = false;
            this.disBundleQty.Location = new System.Drawing.Point(365, 129);
            this.disBundleQty.Name = "disBundleQty";
            this.disBundleQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.disBundleQty.ReadOnly = true;
            this.disBundleQty.Size = new System.Drawing.Size(147, 23);
            this.disBundleQty.TabIndex = 28;
            this.disBundleQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.disBundleQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
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
            this.grid1.ColumnHeadersVisible = false;
            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(17, 158);
            this.grid1.Name = "grid1";
            this.grid1.ReadOnly = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid1.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grid1.RowHeadersVisible = false;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(493, 79);
            this.grid1.TabIndex = 29;
            this.grid1.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.Grid1_RowsAdded);
            // 
            // disSP
            // 
            this.disSP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.disSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.disSP.IsSupportEditMode = false;
            this.disSP.Location = new System.Drawing.Point(116, 100);
            this.disSP.Multiline = true;
            this.disSP.Name = "disSP";
            this.disSP.ReadOnly = true;
            this.disSP.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.disSP.Size = new System.Drawing.Size(147, 23);
            this.disSP.TabIndex = 30;
            // 
            // P14
            // 
            this.ClientSize = new System.Drawing.Size(531, 249);
            this.Controls.Add(this.disSP);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.disBundleQty);
            this.Controls.Add(this.disSize);
            this.Controls.Add(this.disColor);
            this.Controls.Add(this.disArticle);
            this.Controls.Add(this.disCutNo);
            this.Controls.Add(this.disCutpart);
            this.Controls.Add(this.txtCardNo);
            this.Controls.Add(this.cmdComboType);
            this.Controls.Add(this.txtBundleNo);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "P14";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P14. RFID for Hanger System";
            this.FormLoaded += new System.EventHandler(this.P14_FormLoaded);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txtBundleNo, 0);
            this.Controls.SetChildIndex(this.cmdComboType, 0);
            this.Controls.SetChildIndex(this.txtCardNo, 0);
            this.Controls.SetChildIndex(this.disCutpart, 0);
            this.Controls.SetChildIndex(this.disCutNo, 0);
            this.Controls.SetChildIndex(this.disArticle, 0);
            this.Controls.SetChildIndex(this.disColor, 0);
            this.Controls.SetChildIndex(this.disSize, 0);
            this.Controls.SetChildIndex(this.disBundleQty, 0);
            this.Controls.SetChildIndex(this.grid1, 0);
            this.Controls.SetChildIndex(this.disSP, 0);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Win.UI.Label label8;
        private Win.UI.Label label9;
        private Win.UI.Label label10;
        private Win.UI.TextBox txtBundleNo;
        private Win.UI.ComboBox cmdComboType;
        private Win.UI.TextBox txtCardNo;
        private Win.UI.DisplayBox disCutpart;
        private Win.UI.DisplayBox disCutNo;
        private Win.UI.DisplayBox disArticle;
        private Win.UI.DisplayBox disColor;
        private Win.UI.DisplayBox disSize;
        private Win.UI.NumericBox disBundleQty;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Grid grid1;
        private Win.UI.EditBox disSP;
    }
}
