namespace Sci.Production.Subcon
{
    partial class P01_Import
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
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.labelNoQuoteHintColor = new Sci.Win.UI.Label();
            this.labelNoQuoteHint = new Sci.Win.UI.Label();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.dateApproveDate = new Sci.Win.UI.DateRange();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.labelApproveDate = new Sci.Win.UI.Label();
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridBatchImport = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.lblIrregularQtyReason = new Sci.Win.UI.Label();
            this.txtIrregularQtyReason = new Sci.Production.Class.TxtSubconReason();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(855, 15);
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
            this.btnImport.Location = new System.Drawing.Point(759, 16);
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
            this.btnFindNow.Location = new System.Drawing.Point(838, 12);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(101, 30);
            this.btnFindNow.TabIndex = 6;
            this.btnFindNow.TabStop = false;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.BtnFindNow_Click);
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(121, 15);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(122, 23);
            this.txtSPNoStart.TabIndex = 0;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(23, 15);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSPNo.Size = new System.Drawing.Size(95, 23);
            this.labelSPNo.TabIndex = 0;
            this.labelSPNo.Text = "SP#";
            this.labelSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelNoQuoteHintColor);
            this.groupBox2.Controls.Add(this.labelNoQuoteHint);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 477);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(951, 53);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // labelNoQuoteHintColor
            // 
            this.labelNoQuoteHintColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(108)))), ((int)(((byte)(126)))));
            this.labelNoQuoteHintColor.Location = new System.Drawing.Point(23, 19);
            this.labelNoQuoteHintColor.Name = "labelNoQuoteHintColor";
            this.labelNoQuoteHintColor.Size = new System.Drawing.Size(19, 20);
            this.labelNoQuoteHintColor.TabIndex = 4;
            // 
            // labelNoQuoteHint
            // 
            this.labelNoQuoteHint.BackColor = System.Drawing.Color.Transparent;
            this.labelNoQuoteHint.Location = new System.Drawing.Point(45, 18);
            this.labelNoQuoteHint.Name = "labelNoQuoteHint";
            this.labelNoQuoteHint.Size = new System.Drawing.Size(395, 23);
            this.labelNoQuoteHint.TabIndex = 3;
            this.labelNoQuoteHint.Text = "No quote record, please check with Planning Team";
            this.labelNoQuoteHint.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblIrregularQtyReason);
            this.groupBox1.Controls.Add(this.txtIrregularQtyReason);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.dateSCIDelivery);
            this.groupBox1.Controls.Add(this.dateApproveDate);
            this.groupBox1.Controls.Add(this.labelSCIDelivery);
            this.groupBox1.Controls.Add(this.txtSPNoEnd);
            this.groupBox1.Controls.Add(this.labelApproveDate);
            this.groupBox1.Controls.Add(this.btnFindNow);
            this.groupBox1.Controls.Add(this.txtSPNoStart);
            this.groupBox1.Controls.Add(this.labelSPNo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(951, 87);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(247, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(22, 17);
            this.label8.TabIndex = 14;
            this.label8.Text = "～";
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
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(556, 12);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 2;
            // 
            // dateApproveDate
            // 
            // 
            // 
            // 
            this.dateApproveDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateApproveDate.DateBox1.Name = "";
            this.dateApproveDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateApproveDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateApproveDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateApproveDate.DateBox2.Name = "";
            this.dateApproveDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateApproveDate.DateBox2.TabIndex = 1;
            this.dateApproveDate.IsRequired = false;
            this.dateApproveDate.Location = new System.Drawing.Point(121, 48);
            this.dateApproveDate.Name = "dateApproveDate";
            this.dateApproveDate.Size = new System.Drawing.Size(280, 23);
            this.dateApproveDate.TabIndex = 3;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(417, 15);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSCIDelivery.Size = new System.Drawing.Size(136, 23);
            this.labelSCIDelivery.TabIndex = 7;
            this.labelSCIDelivery.Text = "SCI  Delivery";
            this.labelSCIDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(274, 15);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(122, 23);
            this.txtSPNoEnd.TabIndex = 1;
            // 
            // labelApproveDate
            // 
            this.labelApproveDate.Location = new System.Drawing.Point(23, 48);
            this.labelApproveDate.Name = "labelApproveDate";
            this.labelApproveDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelApproveDate.Size = new System.Drawing.Size(95, 23);
            this.labelApproveDate.TabIndex = 5;
            this.labelApproveDate.Text = "Req. Apv Date";
            this.labelApproveDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridBatchImport);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 87);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(951, 390);
            this.panel1.TabIndex = 20;
            // 
            // gridBatchImport
            // 
            this.gridBatchImport.AllowUserToAddRows = false;
            this.gridBatchImport.AllowUserToDeleteRows = false;
            this.gridBatchImport.AllowUserToResizeRows = false;
            this.gridBatchImport.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridBatchImport.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridBatchImport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridBatchImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridBatchImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridBatchImport.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridBatchImport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridBatchImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridBatchImport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridBatchImport.Location = new System.Drawing.Point(0, 0);
            this.gridBatchImport.Name = "gridBatchImport";
            this.gridBatchImport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridBatchImport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridBatchImport.RowTemplate.Height = 24;
            this.gridBatchImport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridBatchImport.ShowCellToolTips = false;
            this.gridBatchImport.Size = new System.Drawing.Size(951, 390);
            this.gridBatchImport.TabIndex = 0;
            this.gridBatchImport.TabStop = false;
            // 
            // lblIrregularQtyReason
            // 
            this.lblIrregularQtyReason.Location = new System.Drawing.Point(417, 48);
            this.lblIrregularQtyReason.Name = "lblIrregularQtyReason";
            this.lblIrregularQtyReason.Size = new System.Drawing.Size(136, 23);
            this.lblIrregularQtyReason.TabIndex = 20;
            this.lblIrregularQtyReason.Text = "Irregular Qty Reason";
            // 
            // txtIrregularQtyReason
            // 
            this.txtIrregularQtyReason.DisplayBox1Binding = "";
            this.txtIrregularQtyReason.Location = new System.Drawing.Point(556, 46);
            this.txtIrregularQtyReason.MutiSelect = true;
            this.txtIrregularQtyReason.Name = "txtIrregularQtyReason";
            this.txtIrregularQtyReason.Size = new System.Drawing.Size(393, 27);
            this.txtIrregularQtyReason.TabIndex = 19;
            this.txtIrregularQtyReason.TextBox1Binding = "";
            this.txtIrregularQtyReason.Type = "SQ";
            // 
            // P01_Import
            // 
            this.ClientSize = new System.Drawing.Size(951, 530);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.DefaultControl = "dateApproveDate";
            this.Name = "P01_Import";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Batch Import";
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridBatchImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Button btnCancel;
        private Win.UI.Button btnImport;
        private Win.UI.Button btnFindNow;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.Label labelSPNo;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.Label labelApproveDate;
        private Win.UI.Panel panel1;
        private Win.UI.Grid gridBatchImport;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.DateRange dateApproveDate;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private System.Windows.Forms.Label label8;
        private Win.UI.Label labelNoQuoteHint;
        private Win.UI.Label labelNoQuoteHintColor;
        private Win.UI.Label lblIrregularQtyReason;
        private Class.TxtSubconReason txtIrregularQtyReason;
    }
}
