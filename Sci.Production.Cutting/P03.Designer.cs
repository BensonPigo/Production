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
            this.btnUpdate = new Sci.Win.UI.Button();
            this.txtCutRefNo = new Sci.Win.UI.TextBox();
            this.txtSEQ = new Sci.Win.UI.TextBox();
            this.btnQuery = new Sci.Win.UI.Button();
            this.labelNewEstCutDate = new Sci.Win.UI.Label();
            this.dateNewEstCutDate = new Sci.Win.UI.DateBox();
            this.dateSewingInline = new Sci.Win.UI.DateBox();
            this.dateEstCutDate = new Sci.Win.UI.DateBox();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.txtCuttingSPNo = new Sci.Win.UI.TextBox();
            this.labelCutRefNo = new Sci.Win.UI.Label();
            this.labelReason = new Sci.Win.UI.Label();
            this.labelSEQ = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelSewingInline = new Sci.Win.UI.Label();
            this.labelEstCutDate = new Sci.Win.UI.Label();
            this.labelCuttingSPNo = new Sci.Win.UI.Label();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.txtcutReason = new Sci.Production.Class.txtcutReason();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.gridbs = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(900, 16);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(92, 30);
            this.btnUpdate.TabIndex = 8;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // txtCutRefNo
            // 
            this.txtCutRefNo.BackColor = System.Drawing.Color.White;
            this.txtCutRefNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutRefNo.Location = new System.Drawing.Point(488, 51);
            this.txtCutRefNo.Name = "txtCutRefNo";
            this.txtCutRefNo.Size = new System.Drawing.Size(78, 23);
            this.txtCutRefNo.TabIndex = 5;
            // 
            // txtSEQ
            // 
            this.txtSEQ.BackColor = System.Drawing.Color.White;
            this.txtSEQ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSEQ.Location = new System.Drawing.Point(302, 51);
            this.txtSEQ.Mask = "000-00";
            this.txtSEQ.Name = "txtSEQ";
            this.txtSEQ.Size = new System.Drawing.Size(67, 23);
            this.txtSEQ.TabIndex = 4;
            this.txtSEQ.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(572, 48);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 6;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // labelNewEstCutDate
            // 
            this.labelNewEstCutDate.Lines = 0;
            this.labelNewEstCutDate.Location = new System.Drawing.Point(669, 19);
            this.labelNewEstCutDate.Name = "labelNewEstCutDate";
            this.labelNewEstCutDate.Size = new System.Drawing.Size(89, 23);
            this.labelNewEstCutDate.TabIndex = 32;
            this.labelNewEstCutDate.Text = "Est. Cut Date";
            // 
            // dateNewEstCutDate
            // 
            this.dateNewEstCutDate.Location = new System.Drawing.Point(761, 19);
            this.dateNewEstCutDate.Name = "dateNewEstCutDate";
            this.dateNewEstCutDate.Size = new System.Drawing.Size(130, 23);
            this.dateNewEstCutDate.TabIndex = 7;
            this.dateNewEstCutDate.Validating += new System.ComponentModel.CancelEventHandler(this.dateNewEstCutDate_Validating);
            // 
            // dateSewingInline
            // 
            this.dateSewingInline.Location = new System.Drawing.Point(511, 19);
            this.dateSewingInline.Name = "dateSewingInline";
            this.dateSewingInline.Size = new System.Drawing.Size(130, 23);
            this.dateSewingInline.TabIndex = 2;
            // 
            // dateEstCutDate
            // 
            this.dateEstCutDate.Location = new System.Drawing.Point(104, 51);
            this.dateEstCutDate.Name = "dateEstCutDate";
            this.dateEstCutDate.Size = new System.Drawing.Size(130, 23);
            this.dateEstCutDate.TabIndex = 3;
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(302, 19);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(108, 23);
            this.txtSPNo.TabIndex = 1;
            // 
            // txtCuttingSPNo
            // 
            this.txtCuttingSPNo.BackColor = System.Drawing.Color.White;
            this.txtCuttingSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCuttingSPNo.Location = new System.Drawing.Point(104, 19);
            this.txtCuttingSPNo.Name = "txtCuttingSPNo";
            this.txtCuttingSPNo.Size = new System.Drawing.Size(108, 23);
            this.txtCuttingSPNo.TabIndex = 0;
            // 
            // labelCutRefNo
            // 
            this.labelCutRefNo.Lines = 0;
            this.labelCutRefNo.Location = new System.Drawing.Point(420, 51);
            this.labelCutRefNo.Name = "labelCutRefNo";
            this.labelCutRefNo.Size = new System.Drawing.Size(64, 23);
            this.labelCutRefNo.TabIndex = 26;
            this.labelCutRefNo.Text = "Cut Ref#";
            // 
            // labelReason
            // 
            this.labelReason.Lines = 0;
            this.labelReason.Location = new System.Drawing.Point(669, 51);
            this.labelReason.Name = "labelReason";
            this.labelReason.Size = new System.Drawing.Size(89, 23);
            this.labelReason.TabIndex = 25;
            this.labelReason.Text = "Reason";
            // 
            // labelSEQ
            // 
            this.labelSEQ.Lines = 0;
            this.labelSEQ.Location = new System.Drawing.Point(243, 51);
            this.labelSEQ.Name = "labelSEQ";
            this.labelSEQ.Size = new System.Drawing.Size(55, 23);
            this.labelSEQ.TabIndex = 24;
            this.labelSEQ.Text = "SEQ";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Lines = 0;
            this.labelSPNo.Location = new System.Drawing.Point(244, 19);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(55, 23);
            this.labelSPNo.TabIndex = 23;
            this.labelSPNo.Text = "SP#";
            // 
            // labelSewingInline
            // 
            this.labelSewingInline.Lines = 0;
            this.labelSewingInline.Location = new System.Drawing.Point(420, 19);
            this.labelSewingInline.Name = "labelSewingInline";
            this.labelSewingInline.Size = new System.Drawing.Size(88, 23);
            this.labelSewingInline.TabIndex = 22;
            this.labelSewingInline.Text = "Sewing inline";
            // 
            // labelEstCutDate
            // 
            this.labelEstCutDate.Lines = 0;
            this.labelEstCutDate.Location = new System.Drawing.Point(12, 51);
            this.labelEstCutDate.Name = "labelEstCutDate";
            this.labelEstCutDate.Size = new System.Drawing.Size(89, 23);
            this.labelEstCutDate.TabIndex = 21;
            this.labelEstCutDate.Text = "Est. Cut Date";
            // 
            // labelCuttingSPNo
            // 
            this.labelCuttingSPNo.Lines = 0;
            this.labelCuttingSPNo.Location = new System.Drawing.Point(12, 19);
            this.labelCuttingSPNo.Name = "labelCuttingSPNo";
            this.labelCuttingSPNo.Size = new System.Drawing.Size(89, 23);
            this.labelCuttingSPNo.TabIndex = 20;
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
            this.gridDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDetail.Location = new System.Drawing.Point(8, 88);
            this.gridDetail.Name = "gridDetail";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridDetail.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.Size = new System.Drawing.Size(992, 311);
            this.gridDetail.TabIndex = 9;
            this.gridDetail.TabStop = false;
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
            this.groupBox2.Controls.Add(this.txtcutReason);
            this.groupBox2.Location = new System.Drawing.Point(662, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(337, 74);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            // 
            // txtcutReason
            // 
            this.txtcutReason.DisplayBox1Binding = "";
            this.txtcutReason.Location = new System.Drawing.Point(98, 43);
            this.txtcutReason.Name = "txtcutReason";
            this.txtcutReason.Size = new System.Drawing.Size(234, 27);
            this.txtcutReason.TabIndex = 0;
            this.txtcutReason.TextBox1Binding = "";
            this.txtcutReason.Type = "RC";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(810, 405);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(92, 30);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(908, 405);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(92, 30);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // P03
            // 
            this.ClientSize = new System.Drawing.Size(1006, 439);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.txtCutRefNo);
            this.Controls.Add(this.txtSEQ);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.labelNewEstCutDate);
            this.Controls.Add(this.dateNewEstCutDate);
            this.Controls.Add(this.dateSewingInline);
            this.Controls.Add(this.dateEstCutDate);
            this.Controls.Add(this.txtSPNo);
            this.Controls.Add(this.txtCuttingSPNo);
            this.Controls.Add(this.labelCutRefNo);
            this.Controls.Add(this.labelReason);
            this.Controls.Add(this.labelSEQ);
            this.Controls.Add(this.labelSPNo);
            this.Controls.Add(this.labelSewingInline);
            this.Controls.Add(this.labelEstCutDate);
            this.Controls.Add(this.labelCuttingSPNo);
            this.Controls.Add(this.gridDetail);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.DefaultControl = "txtCuttingSPNo";
            this.DefaultControlForEdit = "txtCuttingSPNo";
            this.EditMode = true;
            this.Name = "P03";
            this.Text = "P03.Change Est. Cut Date after finished Cutting Daily Plan";
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.gridDetail, 0);
            this.Controls.SetChildIndex(this.labelCuttingSPNo, 0);
            this.Controls.SetChildIndex(this.labelEstCutDate, 0);
            this.Controls.SetChildIndex(this.labelSewingInline, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.labelSEQ, 0);
            this.Controls.SetChildIndex(this.labelReason, 0);
            this.Controls.SetChildIndex(this.labelCutRefNo, 0);
            this.Controls.SetChildIndex(this.txtCuttingSPNo, 0);
            this.Controls.SetChildIndex(this.txtSPNo, 0);
            this.Controls.SetChildIndex(this.dateEstCutDate, 0);
            this.Controls.SetChildIndex(this.dateSewingInline, 0);
            this.Controls.SetChildIndex(this.dateNewEstCutDate, 0);
            this.Controls.SetChildIndex(this.labelNewEstCutDate, 0);
            this.Controls.SetChildIndex(this.btnQuery, 0);
            this.Controls.SetChildIndex(this.txtSEQ, 0);
            this.Controls.SetChildIndex(this.txtCutRefNo, 0);
            this.Controls.SetChildIndex(this.btnUpdate, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Button btnUpdate;
        private Win.UI.TextBox txtCutRefNo;
        private Win.UI.TextBox txtSEQ;
        private Win.UI.Button btnQuery;
        private Win.UI.Label labelNewEstCutDate;
        private Win.UI.DateBox dateNewEstCutDate;
        private Win.UI.DateBox dateSewingInline;
        private Win.UI.DateBox dateEstCutDate;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.TextBox txtCuttingSPNo;
        private Win.UI.Label labelCutRefNo;
        private Win.UI.Label labelReason;
        private Win.UI.Label labelSEQ;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelSewingInline;
        private Win.UI.Label labelEstCutDate;
        private Win.UI.Label labelCuttingSPNo;
        private Win.UI.Grid gridDetail;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnClose;
        private Class.txtcutReason txtcutReason;
        private Win.UI.ListControlBindingSource gridbs;
    }
}
