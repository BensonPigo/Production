namespace Sci.Production.Warehouse
{
    partial class P74_Import
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
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.BorrowItemBS = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel1 = new Sci.Win.UI.Panel();
            this.txtSeq = new Sci.Production.Class.txtSeq();
            this.displayColorID = new Sci.Win.UI.DisplayBox();
            this.labelColorID = new Sci.Win.UI.Label();
            this.displayRefno = new Sci.Win.UI.DisplayBox();
            this.labelRefno = new Sci.Win.UI.Label();
            this.displaySizeSpec = new Sci.Win.UI.DisplayBox();
            this.labelSizeSpec = new Sci.Win.UI.Label();
            this.txtBorrowFromSPNo = new Sci.Win.UI.TextBox();
            this.labelBorrowFromSPNo = new Sci.Win.UI.Label();
            this.labelDesc = new Sci.Win.UI.Label();
            this.editDesc = new Sci.Win.UI.EditBox();
            this.txtToSPNoSeq = new Sci.Win.UI.TextBox();
            this.labelToSPNoSeq = new Sci.Win.UI.Label();
            this.btnFindNow = new Sci.Win.UI.Button();
            this.panel2 = new Sci.Win.UI.Panel();
            this.grid1 = new Sci.Win.UI.Grid();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BorrowItemBS)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
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
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(816, 15);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 30);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 548);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1008, 53);
            this.groupBox2.TabIndex = 18;
            this.groupBox2.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtSeq);
            this.panel1.Controls.Add(this.displayColorID);
            this.panel1.Controls.Add(this.labelColorID);
            this.panel1.Controls.Add(this.displayRefno);
            this.panel1.Controls.Add(this.labelRefno);
            this.panel1.Controls.Add(this.displaySizeSpec);
            this.panel1.Controls.Add(this.labelSizeSpec);
            this.panel1.Controls.Add(this.txtBorrowFromSPNo);
            this.panel1.Controls.Add(this.labelBorrowFromSPNo);
            this.panel1.Controls.Add(this.labelDesc);
            this.panel1.Controls.Add(this.editDesc);
            this.panel1.Controls.Add(this.txtToSPNoSeq);
            this.panel1.Controls.Add(this.labelToSPNoSeq);
            this.panel1.Controls.Add(this.btnFindNow);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 155);
            this.panel1.TabIndex = 19;
            // 
            // txtSeq
            // 
            this.txtSeq.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSeq.Location = new System.Drawing.Point(236, 11);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.seq1 = "";
            this.txtSeq.seq2 = "";
            this.txtSeq.Size = new System.Drawing.Size(61, 23);
            this.txtSeq.TabIndex = 1;
            // 
            // displayColorID
            // 
            this.displayColorID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayColorID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayColorID.Location = new System.Drawing.Point(690, 43);
            this.displayColorID.Name = "displayColorID";
            this.displayColorID.Size = new System.Drawing.Size(113, 23);
            this.displayColorID.TabIndex = 25;
            // 
            // labelColorID
            // 
            this.labelColorID.Lines = 0;
            this.labelColorID.Location = new System.Drawing.Point(592, 43);
            this.labelColorID.Name = "labelColorID";
            this.labelColorID.Size = new System.Drawing.Size(95, 23);
            this.labelColorID.TabIndex = 24;
            this.labelColorID.Text = "Color ID";
            // 
            // displayRefno
            // 
            this.displayRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayRefno.Location = new System.Drawing.Point(414, 43);
            this.displayRefno.Name = "displayRefno";
            this.displayRefno.Size = new System.Drawing.Size(153, 23);
            this.displayRefno.TabIndex = 23;
            // 
            // labelRefno
            // 
            this.labelRefno.Lines = 0;
            this.labelRefno.Location = new System.Drawing.Point(316, 43);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.Size = new System.Drawing.Size(95, 23);
            this.labelRefno.TabIndex = 22;
            this.labelRefno.Text = "Refno";
            // 
            // displaySizeSpec
            // 
            this.displaySizeSpec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySizeSpec.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySizeSpec.Location = new System.Drawing.Point(107, 43);
            this.displaySizeSpec.Name = "displaySizeSpec";
            this.displaySizeSpec.Size = new System.Drawing.Size(189, 23);
            this.displaySizeSpec.TabIndex = 21;
            // 
            // labelSizeSpec
            // 
            this.labelSizeSpec.Lines = 0;
            this.labelSizeSpec.Location = new System.Drawing.Point(9, 43);
            this.labelSizeSpec.Name = "labelSizeSpec";
            this.labelSizeSpec.Size = new System.Drawing.Size(95, 23);
            this.labelSizeSpec.TabIndex = 20;
            this.labelSizeSpec.Text = "SizeSpec";
            // 
            // txtBorrowFromSPNo
            // 
            this.txtBorrowFromSPNo.BackColor = System.Drawing.Color.White;
            this.txtBorrowFromSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBorrowFromSPNo.Location = new System.Drawing.Point(445, 11);
            this.txtBorrowFromSPNo.MaxLength = 13;
            this.txtBorrowFromSPNo.Name = "txtBorrowFromSPNo";
            this.txtBorrowFromSPNo.Size = new System.Drawing.Size(122, 23);
            this.txtBorrowFromSPNo.TabIndex = 2;
            // 
            // labelBorrowFromSPNo
            // 
            this.labelBorrowFromSPNo.Lines = 0;
            this.labelBorrowFromSPNo.Location = new System.Drawing.Point(316, 11);
            this.labelBorrowFromSPNo.Name = "labelBorrowFromSPNo";
            this.labelBorrowFromSPNo.Size = new System.Drawing.Size(126, 23);
            this.labelBorrowFromSPNo.TabIndex = 19;
            this.labelBorrowFromSPNo.Text = "Borrow From  SP#";
            // 
            // labelDesc
            // 
            this.labelDesc.Lines = 0;
            this.labelDesc.Location = new System.Drawing.Point(9, 75);
            this.labelDesc.Name = "labelDesc";
            this.labelDesc.Size = new System.Drawing.Size(95, 23);
            this.labelDesc.TabIndex = 18;
            this.labelDesc.Text = "Desc.";
            // 
            // editDesc
            // 
            this.editDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editDesc.IsSupportEditMode = false;
            this.editDesc.Location = new System.Drawing.Point(107, 75);
            this.editDesc.Multiline = true;
            this.editDesc.Name = "editDesc";
            this.editDesc.ReadOnly = true;
            this.editDesc.Size = new System.Drawing.Size(525, 56);
            this.editDesc.TabIndex = 17;
            // 
            // txtToSPNoSeq
            // 
            this.txtToSPNoSeq.BackColor = System.Drawing.Color.White;
            this.txtToSPNoSeq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtToSPNoSeq.Location = new System.Drawing.Point(107, 11);
            this.txtToSPNoSeq.MaxLength = 13;
            this.txtToSPNoSeq.Name = "txtToSPNoSeq";
            this.txtToSPNoSeq.Size = new System.Drawing.Size(122, 23);
            this.txtToSPNoSeq.TabIndex = 0;
            // 
            // labelToSPNoSeq
            // 
            this.labelToSPNoSeq.Lines = 0;
            this.labelToSPNoSeq.Location = new System.Drawing.Point(9, 11);
            this.labelToSPNoSeq.Name = "labelToSPNoSeq";
            this.labelToSPNoSeq.Size = new System.Drawing.Size(95, 23);
            this.labelToSPNoSeq.TabIndex = 14;
            this.labelToSPNoSeq.Text = "To SP# Seq#";
            // 
            // btnFindNow
            // 
            this.btnFindNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFindNow.Location = new System.Drawing.Point(895, 11);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(101, 30);
            this.btnFindNow.TabIndex = 3;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.btnFindNow_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grid1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 155);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 393);
            this.panel2.TabIndex = 20;
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(0, 0);
            this.grid1.Name = "grid1";
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(1008, 393);
            this.grid1.TabIndex = 0;
            this.grid1.TabStop = false;
            // 
            // P74_Import
            // 
            this.ClientSize = new System.Drawing.Size(1008, 601);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox2);
            this.Name = "P74_Import";
            this.Text = "P74. Import Detail";
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.BorrowItemBS)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnCancel;
        private Win.UI.Button btnImport;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.ListControlBindingSource BorrowItemBS;
        private Win.UI.Panel panel1;
        private Win.UI.Button btnFindNow;
        private Win.UI.Panel panel2;
        private Win.UI.Grid grid1;
        private Win.UI.DisplayBox displayColorID;
        private Win.UI.Label labelColorID;
        private Win.UI.DisplayBox displayRefno;
        private Win.UI.Label labelRefno;
        private Win.UI.DisplayBox displaySizeSpec;
        private Win.UI.Label labelSizeSpec;
        private Win.UI.TextBox txtBorrowFromSPNo;
        private Win.UI.Label labelBorrowFromSPNo;
        private Win.UI.Label labelDesc;
        private Win.UI.EditBox editDesc;
        private Win.UI.TextBox txtToSPNoSeq;
        private Win.UI.Label labelToSPNoSeq;
        private Class.txtSeq txtSeq;
    }
}
