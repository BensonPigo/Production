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
            this.txtfactoryByM = new Sci.Production.Class.txtfactory();
            this.label1 = new Sci.Win.UI.Label();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.txtCell1 = new Sci.Production.Class.txtCell();
            this.txtcutReason = new Sci.Production.Class.txtcutReason();
            this.label2 = new Sci.Win.UI.Label();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.gridbs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.label3 = new Sci.Win.UI.Label();
            this.txtCutplanID = new Sci.Win.UI.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            this.groupBox1.SuspendLayout();
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
            this.txtCutRefNo.Location = new System.Drawing.Point(467, 45);
            this.txtCutRefNo.Name = "txtCutRefNo";
            this.txtCutRefNo.Size = new System.Drawing.Size(78, 23);
            this.txtCutRefNo.TabIndex = 5;
            // 
            // txtSEQ
            // 
            this.txtSEQ.BackColor = System.Drawing.Color.White;
            this.txtSEQ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSEQ.Location = new System.Drawing.Point(436, 13);
            this.txtSEQ.Mask = "000-00";
            this.txtSEQ.Name = "txtSEQ";
            this.txtSEQ.Size = new System.Drawing.Size(67, 23);
            this.txtSEQ.TabIndex = 4;
            this.txtSEQ.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(564, 41);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 6;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // labelNewEstCutDate
            // 
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
            this.dateSewingInline.Location = new System.Drawing.Point(300, 45);
            this.dateSewingInline.Name = "dateSewingInline";
            this.dateSewingInline.Size = new System.Drawing.Size(97, 23);
            this.dateSewingInline.TabIndex = 2;
            // 
            // dateEstCutDate
            // 
            this.dateEstCutDate.Location = new System.Drawing.Point(104, 51);
            this.dateEstCutDate.Name = "dateEstCutDate";
            this.dateEstCutDate.Size = new System.Drawing.Size(100, 23);
            this.dateEstCutDate.TabIndex = 3;
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(267, 13);
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
            this.labelCutRefNo.Location = new System.Drawing.Point(400, 45);
            this.labelCutRefNo.Name = "labelCutRefNo";
            this.labelCutRefNo.Size = new System.Drawing.Size(64, 23);
            this.labelCutRefNo.TabIndex = 26;
            this.labelCutRefNo.Text = "Cut Ref#";
            // 
            // labelReason
            // 
            this.labelReason.Location = new System.Drawing.Point(7, 83);
            this.labelReason.Name = "labelReason";
            this.labelReason.Size = new System.Drawing.Size(89, 23);
            this.labelReason.TabIndex = 25;
            this.labelReason.Text = "Reason";
            // 
            // labelSEQ
            // 
            this.labelSEQ.Location = new System.Drawing.Point(378, 13);
            this.labelSEQ.Name = "labelSEQ";
            this.labelSEQ.Size = new System.Drawing.Size(55, 23);
            this.labelSEQ.TabIndex = 24;
            this.labelSEQ.Text = "SEQ";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(209, 13);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(55, 23);
            this.labelSPNo.TabIndex = 23;
            this.labelSPNo.Text = "SP#";
            // 
            // labelSewingInline
            // 
            this.labelSewingInline.Location = new System.Drawing.Point(209, 45);
            this.labelSewingInline.Name = "labelSewingInline";
            this.labelSewingInline.Size = new System.Drawing.Size(88, 23);
            this.labelSewingInline.TabIndex = 22;
            this.labelSewingInline.Text = "Sewing inline";
            // 
            // labelEstCutDate
            // 
            this.labelEstCutDate.Location = new System.Drawing.Point(12, 51);
            this.labelEstCutDate.Name = "labelEstCutDate";
            this.labelEstCutDate.Size = new System.Drawing.Size(89, 23);
            this.labelEstCutDate.TabIndex = 21;
            this.labelEstCutDate.Text = "Est. Cut Date";
            // 
            // labelCuttingSPNo
            // 
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
            this.gridDetail.Location = new System.Drawing.Point(8, 121);
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
            this.gridDetail.ShowCellToolTips = false;
            this.gridDetail.Size = new System.Drawing.Size(994, 278);
            this.gridDetail.TabIndex = 9;
            this.gridDetail.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtCutplanID);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtfactoryByM);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.labelSewingInline);
            this.groupBox1.Controls.Add(this.dateSewingInline);
            this.groupBox1.Controls.Add(this.labelSPNo);
            this.groupBox1.Controls.Add(this.txtSEQ);
            this.groupBox1.Controls.Add(this.txtCutRefNo);
            this.groupBox1.Controls.Add(this.btnQuery);
            this.groupBox1.Controls.Add(this.txtSPNo);
            this.groupBox1.Controls.Add(this.labelCutRefNo);
            this.groupBox1.Controls.Add(this.labelSEQ);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(655, 109);
            this.groupBox1.TabIndex = 37;
            this.groupBox1.TabStop = false;
            // 
            // txtfactoryByM
            // 
            this.txtfactoryByM.BackColor = System.Drawing.Color.White;
            this.txtfactoryByM.FilteMDivision = true;
            this.txtfactoryByM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactoryByM.IssupportJunk = false;
            this.txtfactoryByM.Location = new System.Drawing.Point(564, 13);
            this.txtfactoryByM.Name = "txtfactoryByM";
            this.txtfactoryByM.Size = new System.Drawing.Size(66, 23);
            this.txtfactoryByM.TabIndex = 28;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(506, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 23);
            this.label1.TabIndex = 27;
            this.label1.Text = "Factory";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtCell1);
            this.groupBox2.Controls.Add(this.txtcutReason);
            this.groupBox2.Controls.Add(this.labelReason);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(662, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(337, 109);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            // 
            // txtCell1
            // 
            this.txtCell1.BackColor = System.Drawing.Color.White;
            this.txtCell1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCell1.Location = new System.Drawing.Point(99, 48);
            this.txtCell1.MDivisionID = "";
            this.txtCell1.Name = "txtCell1";
            this.txtCell1.Size = new System.Drawing.Size(30, 23);
            this.txtCell1.TabIndex = 39;
            // 
            // txtcutReason
            // 
            this.txtcutReason.DisplayBox1Binding = "";
            this.txtcutReason.Location = new System.Drawing.Point(99, 79);
            this.txtcutReason.Name = "txtcutReason";
            this.txtcutReason.Size = new System.Drawing.Size(234, 27);
            this.txtcutReason.TabIndex = 0;
            this.txtcutReason.TextBox1Binding = "";
            this.txtcutReason.Type = "RC";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(7, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 23);
            this.label2.TabIndex = 38;
            this.label2.Text = "Cell";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(812, 405);
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
            this.btnClose.Location = new System.Drawing.Point(910, 405);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(92, 30);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 79);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 23);
            this.label3.TabIndex = 29;
            this.label3.Text = "CutplanID";
            // 
            // txtCutplanID
            // 
            this.txtCutplanID.BackColor = System.Drawing.Color.White;
            this.txtCutplanID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutplanID.Location = new System.Drawing.Point(98, 79);
            this.txtCutplanID.Name = "txtCutplanID";
            this.txtCutplanID.Size = new System.Drawing.Size(108, 23);
            this.txtCutplanID.TabIndex = 30;
            // 
            // P03
            // 
            this.ClientSize = new System.Drawing.Size(1008, 439);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.labelNewEstCutDate);
            this.Controls.Add(this.dateNewEstCutDate);
            this.Controls.Add(this.dateEstCutDate);
            this.Controls.Add(this.txtCuttingSPNo);
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
            this.Controls.SetChildIndex(this.txtCuttingSPNo, 0);
            this.Controls.SetChildIndex(this.dateEstCutDate, 0);
            this.Controls.SetChildIndex(this.dateNewEstCutDate, 0);
            this.Controls.SetChildIndex(this.labelNewEstCutDate, 0);
            this.Controls.SetChildIndex(this.btnUpdate, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private Class.txtfactory txtfactoryByM;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Class.txtCell txtCell1;
        private Win.UI.TextBox txtCutplanID;
        private Win.UI.Label label3;
    }
}
