namespace Sci.Production.Subcon
{
    partial class P11_SplitSP
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
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnSplit = new Sci.Win.UI.Button();
            this.btnFind = new Sci.Win.UI.Button();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.numttlSubconOutQty = new Sci.Win.UI.NumericBox();
            this.label4 = new Sci.Win.UI.Label();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.numSubconOutQty = new Sci.Win.UI.NumericBox();
            this.label2 = new Sci.Win.UI.Label();
            this.txtArticle = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtComboType = new Sci.Win.UI.TextBox();
            this.label3 = new Sci.Win.UI.Label();
            this.labstyle = new Sci.Win.UI.Label();
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridImport = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(811, 49);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnSplit
            // 
            this.btnSplit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSplit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSplit.Location = new System.Drawing.Point(715, 50);
            this.btnSplit.Name = "btnSplit";
            this.btnSplit.Size = new System.Drawing.Size(90, 30);
            this.btnSplit.TabIndex = 2;
            this.btnSplit.Text = "Split";
            this.btnSplit.UseVisualStyleBackColor = true;
            this.btnSplit.Click += new System.EventHandler(this.BtnSplit_Click);
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFind.Location = new System.Drawing.Point(817, 22);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(84, 30);
            this.btnFind.TabIndex = 4;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.IsSupportEditMode = false;
            this.txtSPNo.Location = new System.Drawing.Point(73, 26);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(122, 23);
            this.txtSPNo.TabIndex = 0;
            this.txtSPNo.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSPNo_Validating);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numttlSubconOutQty);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnSplit);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 425);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(913, 93);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // numttlSubconOutQty
            // 
            this.numttlSubconOutQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numttlSubconOutQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numttlSubconOutQty.IsSupportEditMode = false;
            this.numttlSubconOutQty.Location = new System.Drawing.Point(716, 15);
            this.numttlSubconOutQty.Name = "numttlSubconOutQty";
            this.numttlSubconOutQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numttlSubconOutQty.ReadOnly = true;
            this.numttlSubconOutQty.Size = new System.Drawing.Size(109, 23);
            this.numttlSubconOutQty.TabIndex = 1;
            this.numttlSubconOutQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(567, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(146, 23);
            this.label4.TabIndex = 0;
            this.label4.Text = "Total Subcon Out Qty";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numSubconOutQty);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtArticle);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtComboType);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnFind);
            this.groupBox1.Controls.Add(this.txtSPNo);
            this.groupBox1.Controls.Add(this.labstyle);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(913, 62);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // numSubconOutQty
            // 
            this.numSubconOutQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numSubconOutQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numSubconOutQty.IsSupportEditMode = false;
            this.numSubconOutQty.Location = new System.Drawing.Point(696, 26);
            this.numSubconOutQty.Name = "numSubconOutQty";
            this.numSubconOutQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSubconOutQty.ReadOnly = true;
            this.numSubconOutQty.Size = new System.Drawing.Size(109, 23);
            this.numSubconOutQty.TabIndex = 3;
            this.numSubconOutQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(581, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 23);
            this.label2.TabIndex = 8;
            this.label2.Text = "Subcon Out Qty";
            // 
            // txtArticle
            // 
            this.txtArticle.BackColor = System.Drawing.Color.White;
            this.txtArticle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtArticle.IsSupportEditMode = false;
            this.txtArticle.Location = new System.Drawing.Point(469, 26);
            this.txtArticle.Name = "txtArticle";
            this.txtArticle.Size = new System.Drawing.Size(93, 23);
            this.txtArticle.TabIndex = 2;
            this.txtArticle.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtArticle_PopUp);
            this.txtArticle.Validating += new System.ComponentModel.CancelEventHandler(this.TxtArticle_Validating);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(414, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 23);
            this.label1.TabIndex = 7;
            this.label1.Text = "Article";
            // 
            // txtComboType
            // 
            this.txtComboType.BackColor = System.Drawing.Color.White;
            this.txtComboType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtComboType.IsSupportEditMode = false;
            this.txtComboType.Location = new System.Drawing.Point(309, 26);
            this.txtComboType.Name = "txtComboType";
            this.txtComboType.Size = new System.Drawing.Size(93, 23);
            this.txtComboType.TabIndex = 1;
            this.txtComboType.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtComboType_PopUp);
            this.txtComboType.Validating += new System.ComponentModel.CancelEventHandler(this.TxtComboType_Validating);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(14, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "SP#";
            // 
            // labstyle
            // 
            this.labstyle.Location = new System.Drawing.Point(214, 25);
            this.labstyle.Name = "labstyle";
            this.labstyle.Size = new System.Drawing.Size(92, 23);
            this.labstyle.TabIndex = 6;
            this.labstyle.Text = "Combo Type";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridImport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 62);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(913, 363);
            this.panel1.TabIndex = 20;
            // 
            // gridImport
            // 
            this.gridImport.AllowUserToAddRows = false;
            this.gridImport.AllowUserToDeleteRows = false;
            this.gridImport.AllowUserToResizeRows = false;
            this.gridImport.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridImport.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridImport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridImport.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridImport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridImport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridImport.Location = new System.Drawing.Point(0, 0);
            this.gridImport.Name = "gridImport";
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridImport.RowTemplate.Height = 24;
            this.gridImport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridImport.ShowCellToolTips = false;
            this.gridImport.Size = new System.Drawing.Size(913, 363);
            this.gridImport.TabIndex = 0;
            this.gridImport.TabStop = false;
            // 
            // P11_SplitSP
            // 
            this.ClientSize = new System.Drawing.Size(913, 518);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.DefaultControl = "txtSPNo";
            this.Name = "P11_SplitSP";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Split SP#";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnCancel;
        private Win.UI.Button btnSplit;
        private Win.UI.Button btnFind;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Panel panel1;
        private Win.UI.Grid gridImport;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Label labstyle;
        private Win.UI.Label label3;
        private Win.UI.TextBox txtComboType;
        private Win.UI.Label label4;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtArticle;
        private Win.UI.Label label1;
        private Win.UI.NumericBox numSubconOutQty;
        private Win.UI.NumericBox numttlSubconOutQty;
    }
}
