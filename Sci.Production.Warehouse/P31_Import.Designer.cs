namespace Sci.Production.Warehouse
{
    partial class P31_Import
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
            this.btnImport = new Sci.Win.UI.Button();
            this.btnFindNow = new Sci.Win.UI.Button();
            this.txtToSP = new Sci.Win.UI.TextBox();
            this.labelToSP = new Sci.Win.UI.Label();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.displayTotalQty = new Sci.Win.UI.DisplayBox();
            this.labelTotalQty = new Sci.Win.UI.Label();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.txtSeq = new Sci.Production.Class.TxtSeq();
            this.displayColorID = new Sci.Win.UI.DisplayBox();
            this.labelColorID = new Sci.Win.UI.Label();
            this.displayRefno = new Sci.Win.UI.DisplayBox();
            this.labelRefno = new Sci.Win.UI.Label();
            this.displaySizeSpec = new Sci.Win.UI.DisplayBox();
            this.labelSizeSpec = new Sci.Win.UI.Label();
            this.txtBorrowFromSP = new Sci.Win.UI.TextBox();
            this.labelBorrowFromSP = new Sci.Win.UI.Label();
            this.labelDesc = new Sci.Win.UI.Label();
            this.editDesc = new Sci.Win.UI.EditBox();
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridImport = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.chkNoLock = new Sci.Win.UI.CheckBox();
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
            this.btnCancel.Location = new System.Drawing.Point(912, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(816, 16);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 30);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // btnFindNow
            // 
            this.btnFindNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFindNow.Location = new System.Drawing.Point(871, 19);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(101, 30);
            this.btnFindNow.TabIndex = 3;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.BtnFindNow_Click);
            // 
            // txtToSP
            // 
            this.txtToSP.BackColor = System.Drawing.Color.White;
            this.txtToSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtToSP.Location = new System.Drawing.Point(107, 19);
            this.txtToSP.MaxLength = 13;
            this.txtToSP.Name = "txtToSP";
            this.txtToSP.Size = new System.Drawing.Size(122, 23);
            this.txtToSP.TabIndex = 0;
            this.txtToSP.Validating += new System.ComponentModel.CancelEventHandler(this.TxtToSP_Validating);
            // 
            // labelToSP
            // 
            this.labelToSP.Location = new System.Drawing.Point(9, 19);
            this.labelToSP.Name = "labelToSP";
            this.labelToSP.Size = new System.Drawing.Size(95, 23);
            this.labelToSP.TabIndex = 0;
            this.labelToSP.Text = "To SP# Seq#";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkNoLock);
            this.groupBox2.Controls.Add(this.displayTotalQty);
            this.groupBox2.Controls.Add(this.labelTotalQty);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 477);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1008, 53);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            // 
            // displayTotalQty
            // 
            this.displayTotalQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayTotalQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayTotalQty.Location = new System.Drawing.Point(537, 22);
            this.displayTotalQty.Name = "displayTotalQty";
            this.displayTotalQty.Size = new System.Drawing.Size(100, 23);
            this.displayTotalQty.TabIndex = 4;
            // 
            // labelTotalQty
            // 
            this.labelTotalQty.Location = new System.Drawing.Point(439, 22);
            this.labelTotalQty.Name = "labelTotalQty";
            this.labelTotalQty.Size = new System.Drawing.Size(95, 23);
            this.labelTotalQty.TabIndex = 3;
            this.labelTotalQty.Text = "Total Qty";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSeq);
            this.groupBox1.Controls.Add(this.displayColorID);
            this.groupBox1.Controls.Add(this.labelColorID);
            this.groupBox1.Controls.Add(this.displayRefno);
            this.groupBox1.Controls.Add(this.labelRefno);
            this.groupBox1.Controls.Add(this.displaySizeSpec);
            this.groupBox1.Controls.Add(this.labelSizeSpec);
            this.groupBox1.Controls.Add(this.txtBorrowFromSP);
            this.groupBox1.Controls.Add(this.labelBorrowFromSP);
            this.groupBox1.Controls.Add(this.labelDesc);
            this.groupBox1.Controls.Add(this.editDesc);
            this.groupBox1.Controls.Add(this.btnFindNow);
            this.groupBox1.Controls.Add(this.txtToSP);
            this.groupBox1.Controls.Add(this.labelToSP);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1008, 155);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            // 
            // txtSeq
            // 
            this.txtSeq.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSeq.Location = new System.Drawing.Point(236, 19);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.Seq1 = "";
            this.txtSeq.Seq2 = "";
            this.txtSeq.Size = new System.Drawing.Size(61, 23);
            this.txtSeq.TabIndex = 1;
            // 
            // displayColorID
            // 
            this.displayColorID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayColorID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayColorID.Location = new System.Drawing.Point(690, 51);
            this.displayColorID.Name = "displayColorID";
            this.displayColorID.Size = new System.Drawing.Size(113, 23);
            this.displayColorID.TabIndex = 12;
            // 
            // labelColorID
            // 
            this.labelColorID.Location = new System.Drawing.Point(592, 51);
            this.labelColorID.Name = "labelColorID";
            this.labelColorID.Size = new System.Drawing.Size(95, 23);
            this.labelColorID.TabIndex = 11;
            this.labelColorID.Text = "Color ID";
            // 
            // displayRefno
            // 
            this.displayRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayRefno.Location = new System.Drawing.Point(414, 51);
            this.displayRefno.Name = "displayRefno";
            this.displayRefno.Size = new System.Drawing.Size(153, 23);
            this.displayRefno.TabIndex = 10;
            // 
            // labelRefno
            // 
            this.labelRefno.Location = new System.Drawing.Point(316, 51);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.Size = new System.Drawing.Size(95, 23);
            this.labelRefno.TabIndex = 9;
            this.labelRefno.Text = "Refno";
            // 
            // displaySizeSpec
            // 
            this.displaySizeSpec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySizeSpec.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySizeSpec.Location = new System.Drawing.Point(107, 51);
            this.displaySizeSpec.Name = "displaySizeSpec";
            this.displaySizeSpec.Size = new System.Drawing.Size(189, 23);
            this.displaySizeSpec.TabIndex = 8;
            // 
            // labelSizeSpec
            // 
            this.labelSizeSpec.Location = new System.Drawing.Point(9, 51);
            this.labelSizeSpec.Name = "labelSizeSpec";
            this.labelSizeSpec.Size = new System.Drawing.Size(95, 23);
            this.labelSizeSpec.TabIndex = 7;
            this.labelSizeSpec.Text = "SizeSpec";
            // 
            // txtBorrowFromSP
            // 
            this.txtBorrowFromSP.BackColor = System.Drawing.Color.White;
            this.txtBorrowFromSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBorrowFromSP.Location = new System.Drawing.Point(445, 19);
            this.txtBorrowFromSP.MaxLength = 13;
            this.txtBorrowFromSP.Name = "txtBorrowFromSP";
            this.txtBorrowFromSP.Size = new System.Drawing.Size(122, 23);
            this.txtBorrowFromSP.TabIndex = 2;
            // 
            // labelBorrowFromSP
            // 
            this.labelBorrowFromSP.Location = new System.Drawing.Point(316, 19);
            this.labelBorrowFromSP.Name = "labelBorrowFromSP";
            this.labelBorrowFromSP.Size = new System.Drawing.Size(126, 23);
            this.labelBorrowFromSP.TabIndex = 6;
            this.labelBorrowFromSP.Text = "Borrow From  SP#";
            // 
            // labelDesc
            // 
            this.labelDesc.Location = new System.Drawing.Point(9, 83);
            this.labelDesc.Name = "labelDesc";
            this.labelDesc.Size = new System.Drawing.Size(95, 23);
            this.labelDesc.TabIndex = 4;
            this.labelDesc.Text = "Desc.";
            // 
            // editDesc
            // 
            this.editDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editDesc.IsSupportEditMode = false;
            this.editDesc.Location = new System.Drawing.Point(107, 83);
            this.editDesc.Multiline = true;
            this.editDesc.Name = "editDesc";
            this.editDesc.ReadOnly = true;
            this.editDesc.Size = new System.Drawing.Size(525, 56);
            this.editDesc.TabIndex = 13;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridImport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 155);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 322);
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
            this.gridImport.Size = new System.Drawing.Size(1008, 322);
            this.gridImport.TabIndex = 0;
            this.gridImport.TabStop = false;
            this.gridImport.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridImport_ColumnHeaderMouseClick);
            this.gridImport.Validated += new System.EventHandler(this.GridImport_Validated);
            // 
            // chkNoLock
            // 
            this.chkNoLock.AutoSize = true;
            this.chkNoLock.Checked = true;
            this.chkNoLock.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkNoLock.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkNoLock.Location = new System.Drawing.Point(9, 22);
            this.chkNoLock.Name = "chkNoLock";
            this.chkNoLock.Size = new System.Drawing.Size(192, 21);
            this.chkNoLock.TabIndex = 15;
            this.chkNoLock.Text = "only show no lock material";
            this.chkNoLock.UseVisualStyleBackColor = true;
            this.chkNoLock.CheckedChanged += new System.EventHandler(this.ChkNoLock_CheckedChanged);
            // 
            // P31_Import
            // 
            this.ClientSize = new System.Drawing.Size(1008, 530);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "P31_Import";
            this.Text = "P31. Import Detail";
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
        private Win.UI.Button btnImport;
        private Win.UI.Button btnFindNow;
        private Win.UI.TextBox txtToSP;
        private Win.UI.Label labelToSP;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Panel panel1;
        private Win.UI.Grid gridImport;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.DisplayBox displayTotalQty;
        private Win.UI.Label labelTotalQty;
        private Win.UI.DisplayBox displayColorID;
        private Win.UI.Label labelColorID;
        private Win.UI.DisplayBox displayRefno;
        private Win.UI.Label labelRefno;
        private Win.UI.DisplayBox displaySizeSpec;
        private Win.UI.Label labelSizeSpec;
        private Win.UI.TextBox txtBorrowFromSP;
        private Win.UI.Label labelBorrowFromSP;
        private Win.UI.Label labelDesc;
        private Win.UI.EditBox editDesc;
        private Class.TxtSeq txtSeq;
        private Win.UI.CheckBox chkNoLock;
    }
}
