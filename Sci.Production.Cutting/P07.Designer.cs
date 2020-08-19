namespace Sci.Production.Cutting
{
    partial class P07
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnQuery = new Sci.Win.UI.Button();
            this.labelEstCutDate = new Sci.Win.UI.Label();
            this.txtCutplanID = new Sci.Win.UI.TextBox();
            this.label3 = new Sci.Win.UI.Label();
            this.txtfactoryByM = new Sci.Production.Class.Txtfactory();
            this.label1 = new Sci.Win.UI.Label();
            this.labelSewingInline = new Sci.Win.UI.Label();
            this.dateSewingInline = new Sci.Win.UI.DateBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.txtSEQ = new Sci.Win.UI.TextBox();
            this.txtCutRefNo = new Sci.Win.UI.TextBox();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.labelCutRefNo = new Sci.Win.UI.Label();
            this.labelSEQ = new Sci.Win.UI.Label();
            this.dateEstCutDate = new Sci.Win.UI.DateBox();
            this.txtCuttingSPNo = new Sci.Win.UI.TextBox();
            this.labelCuttingSPNo = new Sci.Win.UI.Label();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.btnClose = new Sci.Win.UI.Button();
            this.gridbs = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            this.SuspendLayout();
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(886, 9);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 37;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // labelEstCutDate
            // 
            this.labelEstCutDate.Location = new System.Drawing.Point(212, 38);
            this.labelEstCutDate.Name = "labelEstCutDate";
            this.labelEstCutDate.Size = new System.Drawing.Size(89, 23);
            this.labelEstCutDate.TabIndex = 39;
            this.labelEstCutDate.Text = "Est. Cut Date";
            // 
            // txtCutplanID
            // 
            this.txtCutplanID.BackColor = System.Drawing.Color.White;
            this.txtCutplanID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutplanID.IsSupportEditMode = false;
            this.txtCutplanID.Location = new System.Drawing.Point(101, 38);
            this.txtCutplanID.Name = "txtCutplanID";
            this.txtCutplanID.Size = new System.Drawing.Size(108, 23);
            this.txtCutplanID.TabIndex = 47;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 23);
            this.label3.TabIndex = 46;
            this.label3.Text = "CutplanID";
            // 
            // txtfactoryByM
            // 
            this.txtfactoryByM.BackColor = System.Drawing.Color.White;
            this.txtfactoryByM.FilteMDivision = true;
            this.txtfactoryByM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactoryByM.IsSupportEditMode = false;
            this.txtfactoryByM.IssupportJunk = false;
            this.txtfactoryByM.Location = new System.Drawing.Point(673, 9);
            this.txtfactoryByM.Name = "txtfactoryByM";
            this.txtfactoryByM.Size = new System.Drawing.Size(66, 23);
            this.txtfactoryByM.TabIndex = 45;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(606, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 23);
            this.label1.TabIndex = 44;
            this.label1.Text = "Factory";
            // 
            // labelSewingInline
            // 
            this.labelSewingInline.Location = new System.Drawing.Point(415, 38);
            this.labelSewingInline.Name = "labelSewingInline";
            this.labelSewingInline.Size = new System.Drawing.Size(88, 23);
            this.labelSewingInline.TabIndex = 40;
            this.labelSewingInline.Text = "Sewing inline";
            // 
            // dateSewingInline
            // 
            this.dateSewingInline.IsSupportEditMode = false;
            this.dateSewingInline.Location = new System.Drawing.Point(506, 38);
            this.dateSewingInline.Name = "dateSewingInline";
            this.dateSewingInline.Size = new System.Drawing.Size(97, 23);
            this.dateSewingInline.TabIndex = 33;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(212, 9);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(89, 23);
            this.labelSPNo.TabIndex = 41;
            this.labelSPNo.Text = "SP#";
            // 
            // txtSEQ
            // 
            this.txtSEQ.BackColor = System.Drawing.Color.White;
            this.txtSEQ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSEQ.IsSupportEditMode = false;
            this.txtSEQ.Location = new System.Drawing.Point(506, 9);
            this.txtSEQ.Mask = "000-00";
            this.txtSEQ.Name = "txtSEQ";
            this.txtSEQ.Size = new System.Drawing.Size(67, 23);
            this.txtSEQ.TabIndex = 35;
            this.txtSEQ.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            // 
            // txtCutRefNo
            // 
            this.txtCutRefNo.BackColor = System.Drawing.Color.White;
            this.txtCutRefNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutRefNo.IsSupportEditMode = false;
            this.txtCutRefNo.Location = new System.Drawing.Point(673, 35);
            this.txtCutRefNo.Name = "txtCutRefNo";
            this.txtCutRefNo.Size = new System.Drawing.Size(78, 23);
            this.txtCutRefNo.TabIndex = 36;
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.IsSupportEditMode = false;
            this.txtSPNo.Location = new System.Drawing.Point(304, 9);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(108, 23);
            this.txtSPNo.TabIndex = 32;
            // 
            // labelCutRefNo
            // 
            this.labelCutRefNo.Location = new System.Drawing.Point(606, 35);
            this.labelCutRefNo.Name = "labelCutRefNo";
            this.labelCutRefNo.Size = new System.Drawing.Size(64, 23);
            this.labelCutRefNo.TabIndex = 43;
            this.labelCutRefNo.Text = "Cut Ref#";
            // 
            // labelSEQ
            // 
            this.labelSEQ.Location = new System.Drawing.Point(415, 9);
            this.labelSEQ.Name = "labelSEQ";
            this.labelSEQ.Size = new System.Drawing.Size(88, 23);
            this.labelSEQ.TabIndex = 42;
            this.labelSEQ.Text = "SEQ";
            // 
            // dateEstCutDate
            // 
            this.dateEstCutDate.IsSupportEditMode = false;
            this.dateEstCutDate.Location = new System.Drawing.Point(304, 38);
            this.dateEstCutDate.Name = "dateEstCutDate";
            this.dateEstCutDate.Size = new System.Drawing.Size(100, 23);
            this.dateEstCutDate.TabIndex = 34;
            // 
            // txtCuttingSPNo
            // 
            this.txtCuttingSPNo.BackColor = System.Drawing.Color.White;
            this.txtCuttingSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCuttingSPNo.IsSupportEditMode = false;
            this.txtCuttingSPNo.Location = new System.Drawing.Point(101, 9);
            this.txtCuttingSPNo.Name = "txtCuttingSPNo";
            this.txtCuttingSPNo.Size = new System.Drawing.Size(108, 23);
            this.txtCuttingSPNo.TabIndex = 31;
            // 
            // labelCuttingSPNo
            // 
            this.labelCuttingSPNo.Location = new System.Drawing.Point(9, 9);
            this.labelCuttingSPNo.Name = "labelCuttingSPNo";
            this.labelCuttingSPNo.Size = new System.Drawing.Size(89, 23);
            this.labelCuttingSPNo.TabIndex = 38;
            this.labelCuttingSPNo.Text = "Cutting SP#";
            // 
            // gridDetail
            // 
            this.gridDetail.AllowUserToAddRows = false;
            this.gridDetail.AllowUserToDeleteRows = false;
            this.gridDetail.AllowUserToResizeRows = false;
            this.gridDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetail.DataSource = this.gridbs;
            this.gridDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDetail.Location = new System.Drawing.Point(9, 76);
            this.gridDetail.Name = "gridDetail";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridDetail.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.ShowCellToolTips = false;
            this.gridDetail.Size = new System.Drawing.Size(957, 346);
            this.gridDetail.TabIndex = 48;
            this.gridDetail.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(874, 428);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(92, 30);
            this.btnClose.TabIndex = 49;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // P07
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 470);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gridDetail);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.labelEstCutDate);
            this.Controls.Add(this.txtCutplanID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtfactoryByM);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelSewingInline);
            this.Controls.Add(this.dateSewingInline);
            this.Controls.Add(this.labelSPNo);
            this.Controls.Add(this.txtSEQ);
            this.Controls.Add(this.txtCutRefNo);
            this.Controls.Add(this.txtSPNo);
            this.Controls.Add(this.labelCutRefNo);
            this.Controls.Add(this.labelSEQ);
            this.Controls.Add(this.dateEstCutDate);
            this.Controls.Add(this.txtCuttingSPNo);
            this.Controls.Add(this.labelCuttingSPNo);
            this.Name = "P07";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P07. Query for Change Est. Cut Date Record";
            this.Controls.SetChildIndex(this.labelCuttingSPNo, 0);
            this.Controls.SetChildIndex(this.txtCuttingSPNo, 0);
            this.Controls.SetChildIndex(this.dateEstCutDate, 0);
            this.Controls.SetChildIndex(this.labelSEQ, 0);
            this.Controls.SetChildIndex(this.labelCutRefNo, 0);
            this.Controls.SetChildIndex(this.txtSPNo, 0);
            this.Controls.SetChildIndex(this.txtCutRefNo, 0);
            this.Controls.SetChildIndex(this.txtSEQ, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.dateSewingInline, 0);
            this.Controls.SetChildIndex(this.labelSewingInline, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtfactoryByM, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtCutplanID, 0);
            this.Controls.SetChildIndex(this.labelEstCutDate, 0);
            this.Controls.SetChildIndex(this.btnQuery, 0);
            this.Controls.SetChildIndex(this.gridDetail, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Button btnQuery;
        private Win.UI.Label labelEstCutDate;
        private Win.UI.TextBox txtCutplanID;
        private Win.UI.Label label3;
        private Class.Txtfactory txtfactoryByM;
        private Win.UI.Label label1;
        private Win.UI.Label labelSewingInline;
        private Win.UI.DateBox dateSewingInline;
        private Win.UI.Label labelSPNo;
        private Win.UI.TextBox txtSEQ;
        private Win.UI.TextBox txtCutRefNo;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.Label labelCutRefNo;
        private Win.UI.Label labelSEQ;
        private Win.UI.DateBox dateEstCutDate;
        private Win.UI.TextBox txtCuttingSPNo;
        private Win.UI.Label labelCuttingSPNo;
        private Win.UI.Grid gridDetail;
        private Win.UI.Button btnClose;
        private Win.UI.ListControlBindingSource gridbs;
    }
}