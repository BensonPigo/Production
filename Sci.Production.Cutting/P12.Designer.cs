namespace Sci.Production.Cutting
{
    partial class P12
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.dateBox1 = new Sci.Win.UI.DateBox();
            this.txtCell = new Sci.Win.UI.TextBox();
            this.txtSize = new Sci.Win.UI.TextBox();
            this.txtPOID = new Sci.Win.UI.TextBox();
            this.label10 = new Sci.Win.UI.Label();
            this.txtBundleEnd = new Sci.Win.UI.TextBox();
            this.txtBundleStart = new Sci.Win.UI.TextBox();
            this.label8 = new Sci.Win.UI.Label();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            this.checkExtendAllParts = new Sci.Win.UI.CheckBox();
            this.txtCutRefEnd = new Sci.Win.UI.TextBox();
            this.txtCutRefStart = new Sci.Win.UI.TextBox();
            this.labelSize = new Sci.Win.UI.Label();
            this.labelCell = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.labelBundleNo = new Sci.Win.UI.Label();
            this.labelPOID = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelCutRef = new Sci.Win.UI.Label();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.labelSortBy = new Sci.Win.UI.Label();
            this.comboSortBy = new Sci.Win.UI.ComboBox();
            this.btnBundleCard = new Sci.Win.UI.Button();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new Sci.Win.UI.Panel();
            this.grid1 = new Sci.Win.UI.Grid();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dateBox1);
            this.panel1.Controls.Add(this.txtCell);
            this.panel1.Controls.Add(this.txtSize);
            this.panel1.Controls.Add(this.txtPOID);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.txtBundleEnd);
            this.panel1.Controls.Add(this.txtBundleStart);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtSPNoEnd);
            this.panel1.Controls.Add(this.txtSPNoStart);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.checkExtendAllParts);
            this.panel1.Controls.Add(this.txtCutRefEnd);
            this.panel1.Controls.Add(this.txtCutRefStart);
            this.panel1.Controls.Add(this.labelSize);
            this.panel1.Controls.Add(this.labelCell);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.labelBundleNo);
            this.panel1.Controls.Add(this.labelPOID);
            this.panel1.Controls.Add(this.labelSPNo);
            this.panel1.Controls.Add(this.labelCutRef);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1187, 86);
            this.panel1.TabIndex = 0;
            // 
            // dateBox1
            // 
            this.dateBox1.Location = new System.Drawing.Point(687, 16);
            this.dateBox1.Name = "dateBox1";
            this.dateBox1.Size = new System.Drawing.Size(130, 23);
            this.dateBox1.TabIndex = 3;
            // 
            // txtCell
            // 
            this.txtCell.BackColor = System.Drawing.Color.White;
            this.txtCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCell.Location = new System.Drawing.Point(706, 49);
            this.txtCell.Name = "txtCell";
            this.txtCell.Size = new System.Drawing.Size(100, 23);
            this.txtCell.TabIndex = 8;
            // 
            // txtSize
            // 
            this.txtSize.BackColor = System.Drawing.Color.White;
            this.txtSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSize.Location = new System.Drawing.Point(883, 48);
            this.txtSize.Name = "txtSize";
            this.txtSize.Size = new System.Drawing.Size(100, 23);
            this.txtSize.TabIndex = 9;
            // 
            // txtPOID
            // 
            this.txtPOID.BackColor = System.Drawing.Color.White;
            this.txtPOID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPOID.Location = new System.Drawing.Point(389, 15);
            this.txtPOID.Name = "txtPOID";
            this.txtPOID.Size = new System.Drawing.Size(100, 23);
            this.txtPOID.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(509, 50);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(22, 22);
            this.label10.TabIndex = 18;
            this.label10.Text = "~";
            // 
            // txtBundleEnd
            // 
            this.txtBundleEnd.BackColor = System.Drawing.Color.White;
            this.txtBundleEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBundleEnd.Location = new System.Drawing.Point(534, 49);
            this.txtBundleEnd.Name = "txtBundleEnd";
            this.txtBundleEnd.Size = new System.Drawing.Size(99, 23);
            this.txtBundleEnd.TabIndex = 7;
            // 
            // txtBundleStart
            // 
            this.txtBundleStart.BackColor = System.Drawing.Color.White;
            this.txtBundleStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBundleStart.Location = new System.Drawing.Point(408, 50);
            this.txtBundleStart.Name = "txtBundleStart";
            this.txtBundleStart.Size = new System.Drawing.Size(99, 23);
            this.txtBundleStart.TabIndex = 6;
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(189, 50);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(22, 22);
            this.label8.TabIndex = 15;
            this.label8.Text = "~";
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(214, 49);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(99, 23);
            this.txtSPNoEnd.TabIndex = 5;
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(88, 50);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(99, 23);
            this.txtSPNoStart.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(189, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(22, 22);
            this.label9.TabIndex = 12;
            this.label9.Text = "~";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(1041, 9);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(94, 35);
            this.btnQuery.TabIndex = 11;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkExtendAllParts
            // 
            this.checkExtendAllParts.AutoSize = true;
            this.checkExtendAllParts.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkExtendAllParts.Location = new System.Drawing.Point(1009, 54);
            this.checkExtendAllParts.Name = "checkExtendAllParts";
            this.checkExtendAllParts.Size = new System.Drawing.Size(126, 21);
            this.checkExtendAllParts.TabIndex = 10;
            this.checkExtendAllParts.Text = "Extend All Parts";
            this.checkExtendAllParts.UseVisualStyleBackColor = true;
            // 
            // txtCutRefEnd
            // 
            this.txtCutRefEnd.BackColor = System.Drawing.Color.White;
            this.txtCutRefEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutRefEnd.Location = new System.Drawing.Point(214, 15);
            this.txtCutRefEnd.Name = "txtCutRefEnd";
            this.txtCutRefEnd.Size = new System.Drawing.Size(99, 23);
            this.txtCutRefEnd.TabIndex = 1;
            // 
            // txtCutRefStart
            // 
            this.txtCutRefStart.BackColor = System.Drawing.Color.White;
            this.txtCutRefStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutRefStart.Location = new System.Drawing.Point(88, 16);
            this.txtCutRefStart.Name = "txtCutRefStart";
            this.txtCutRefStart.Size = new System.Drawing.Size(99, 23);
            this.txtCutRefStart.TabIndex = 0;
            // 
            // labelSize
            // 
            this.labelSize.Lines = 0;
            this.labelSize.Location = new System.Drawing.Point(844, 48);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(36, 23);
            this.labelSize.TabIndex = 6;
            this.labelSize.Text = "Size";
            // 
            // labelCell
            // 
            this.labelCell.Lines = 0;
            this.labelCell.Location = new System.Drawing.Point(671, 48);
            this.labelCell.Name = "labelCell";
            this.labelCell.Size = new System.Drawing.Size(32, 23);
            this.labelCell.TabIndex = 5;
            this.labelCell.Text = "Cell";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(592, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "Est. Cut Date";
            // 
            // labelBundleNo
            // 
            this.labelBundleNo.Lines = 0;
            this.labelBundleNo.Location = new System.Drawing.Point(339, 49);
            this.labelBundleNo.Name = "labelBundleNo";
            this.labelBundleNo.Size = new System.Drawing.Size(66, 23);
            this.labelBundleNo.TabIndex = 3;
            this.labelBundleNo.Text = "Bundle#";
            // 
            // labelPOID
            // 
            this.labelPOID.Lines = 0;
            this.labelPOID.Location = new System.Drawing.Point(339, 15);
            this.labelPOID.Name = "labelPOID";
            this.labelPOID.Size = new System.Drawing.Size(47, 23);
            this.labelPOID.TabIndex = 2;
            this.labelPOID.Text = "POID";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Lines = 0;
            this.labelSPNo.Location = new System.Drawing.Point(10, 51);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(75, 21);
            this.labelSPNo.TabIndex = 1;
            this.labelSPNo.Text = "SP#";
            // 
            // labelCutRef
            // 
            this.labelCutRef.Lines = 0;
            this.labelCutRef.Location = new System.Drawing.Point(10, 16);
            this.labelCutRef.Name = "labelCutRef";
            this.labelCutRef.Size = new System.Drawing.Size(75, 22);
            this.labelCutRef.TabIndex = 0;
            this.labelCutRef.Text = "Cut Ref";
            // 
            // labelSortBy
            // 
            this.labelSortBy.Lines = 0;
            this.labelSortBy.Location = new System.Drawing.Point(10, 11);
            this.labelSortBy.Name = "labelSortBy";
            this.labelSortBy.Size = new System.Drawing.Size(50, 22);
            this.labelSortBy.TabIndex = 0;
            this.labelSortBy.Text = "Sort by";
            // 
            // comboSortBy
            // 
            this.comboSortBy.BackColor = System.Drawing.Color.White;
            this.comboSortBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSortBy.FormattingEnabled = true;
            this.comboSortBy.IsSupportUnselect = true;
            this.comboSortBy.Items.AddRange(new object[] {
            "Bundle#",
            "SP#"});
            this.comboSortBy.Location = new System.Drawing.Point(63, 11);
            this.comboSortBy.Name = "comboSortBy";
            this.comboSortBy.Size = new System.Drawing.Size(144, 24);
            this.comboSortBy.TabIndex = 0;
            // 
            // btnBundleCard
            // 
            this.btnBundleCard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBundleCard.Location = new System.Drawing.Point(742, 11);
            this.btnBundleCard.Name = "btnBundleCard";
            this.btnBundleCard.Size = new System.Drawing.Size(123, 33);
            this.btnBundleCard.TabIndex = 1;
            this.btnBundleCard.Text = "Bundle Card";
            this.btnBundleCard.UseVisualStyleBackColor = true;
            this.btnBundleCard.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnToExcel
            // 
            this.btnToExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToExcel.Location = new System.Drawing.Point(881, 11);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(136, 33);
            this.btnToExcel.TabIndex = 2;
            this.btnToExcel.Text = "ToExcel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(1034, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(136, 33);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.button4_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.labelSortBy);
            this.panel3.Controls.Add(this.comboSortBy);
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Controls.Add(this.btnBundleCard);
            this.panel3.Controls.Add(this.btnToExcel);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 368);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1187, 52);
            this.panel3.TabIndex = 1;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.grid1);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 86);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1187, 282);
            this.panel5.TabIndex = 10;
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(0, 0);
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
            this.grid1.Size = new System.Drawing.Size(1187, 282);
            this.grid1.TabIndex = 0;
            this.grid1.TabStop = false;
            // 
            // P12
            // 
            this.ClientSize = new System.Drawing.Size(1187, 420);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "textBox1";
            this.Name = "P12";
            this.Text = "P12. Batch Print Bundle Card";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel5, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.TextBox txtCutRefEnd;
        private Win.UI.TextBox txtCutRefStart;
        private Win.UI.Label labelSize;
        private Win.UI.Label labelCell;
        private Win.UI.Label label5;
        private Win.UI.Label labelBundleNo;
        private Win.UI.Label labelPOID;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelCutRef;
        private Win.UI.Label label9;
        private Win.UI.Button btnQuery;
        private Win.UI.CheckBox checkExtendAllParts;
        private Win.UI.Label label10;
        private Win.UI.TextBox txtBundleEnd;
        private Win.UI.TextBox txtBundleStart;
        private Win.UI.Label label8;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.TextBox txtCell;
        private Win.UI.TextBox txtSize;
        private Win.UI.TextBox txtPOID;
        private Win.UI.Label labelSortBy;
        private Win.UI.ComboBox comboSortBy;
        private Win.UI.Button btnBundleCard;
        private Win.UI.Button btnToExcel;
        private Win.UI.Button btnClose;
        private Win.UI.DateBox dateBox1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private System.Windows.Forms.Panel panel3;
        private Win.UI.Panel panel5;
        private Win.UI.Grid grid1;
    }
}
