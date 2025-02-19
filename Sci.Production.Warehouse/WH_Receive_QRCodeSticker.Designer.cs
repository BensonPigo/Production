namespace Sci.Production.Warehouse
{
    partial class WH_Receive_QRCodeSticker
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
            this.grid1 = new Sci.Win.UI.Grid();
            this.listControlBindingSource = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnPrint = new Sci.Win.UI.Button();
            this.label1 = new Sci.Win.UI.Label();
            this.comboFilterQRCode = new Sci.Win.UI.ComboBox();
            this.radioPanel1 = new Sci.Win.UI.RadioPanel();
            this.radioEncodeSeq = new Sci.Win.UI.RadioButton();
            this.radiobySP = new Sci.Win.UI.RadioButton();
            this.labSortBy = new Sci.Win.UI.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource)).BeginInit();
            this.radioPanel1.SuspendLayout();
            this.SuspendLayout();
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
            this.grid1.DataSource = this.listControlBindingSource;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(1, 36);
            this.grid1.Name = "grid1";
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(851, 333);
            this.grid1.TabIndex = 1;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Location = new System.Drawing.Point(762, 376);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(80, 30);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 3;
            this.label1.Text = "QR Code";
            // 
            // comboFilterQRCode
            // 
            this.comboFilterQRCode.BackColor = System.Drawing.Color.White;
            this.comboFilterQRCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFilterQRCode.FormattingEnabled = true;
            this.comboFilterQRCode.IsSupportUnselect = true;
            this.comboFilterQRCode.Location = new System.Drawing.Point(87, 8);
            this.comboFilterQRCode.Name = "comboFilterQRCode";
            this.comboFilterQRCode.OldText = "";
            this.comboFilterQRCode.Size = new System.Drawing.Size(140, 24);
            this.comboFilterQRCode.TabIndex = 4;
            this.comboFilterQRCode.SelectedIndexChanged += new System.EventHandler(this.ComboFilterQRCode_SelectedIndexChanged);
            // 
            // radioPanel1
            // 
            this.radioPanel1.Controls.Add(this.radioEncodeSeq);
            this.radioPanel1.Controls.Add(this.radiobySP);
            this.radioPanel1.IsSupportEditMode = false;
            this.radioPanel1.Location = new System.Drawing.Point(343, 4);
            this.radioPanel1.Name = "radioPanel1";
            this.radioPanel1.Size = new System.Drawing.Size(339, 31);
            this.radioPanel1.TabIndex = 66;
            this.radioPanel1.Value = "2";
            this.radioPanel1.ValueChanged += new System.EventHandler(this.RadioPanel1_ValueChanged);
            // 
            // radioEncodeSeq
            // 
            this.radioEncodeSeq.AutoSize = true;
            this.radioEncodeSeq.Checked = true;
            this.radioEncodeSeq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioEncodeSeq.Location = new System.Drawing.Point(182, 5);
            this.radioEncodeSeq.Name = "radioEncodeSeq";
            this.radioEncodeSeq.Size = new System.Drawing.Size(103, 21);
            this.radioEncodeSeq.TabIndex = 1;
            this.radioEncodeSeq.TabStop = true;
            this.radioEncodeSeq.Text = "Encode Seq";
            this.radioEncodeSeq.UseVisualStyleBackColor = true;
            this.radioEncodeSeq.Value = "2";
            // 
            // radiobySP
            // 
            this.radiobySP.AutoSize = true;
            this.radiobySP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radiobySP.Location = new System.Drawing.Point(9, 5);
            this.radiobySP.Name = "radiobySP";
            this.radiobySP.Size = new System.Drawing.Size(165, 21);
            this.radiobySP.TabIndex = 0;
            this.radiobySP.Text = "SP#, Seq, Roll, Dyelot";
            this.radiobySP.UseVisualStyleBackColor = true;
            this.radiobySP.Value = "1";
            // 
            // labSortBy
            // 
            this.labSortBy.Location = new System.Drawing.Point(230, 8);
            this.labSortBy.Name = "labSortBy";
            this.labSortBy.Size = new System.Drawing.Size(110, 23);
            this.labSortBy.TabIndex = 65;
            this.labSortBy.Text = "Sort by";
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Size = new System.Drawing.Size(1007, 234);
            this.shapeContainer1.TabIndex = 54;
            this.shapeContainer1.TabStop = false;
            // 
            // P07_QRCodeSticker
            // 
            this.ClientSize = new System.Drawing.Size(854, 410);
            this.Controls.Add(this.radioPanel1);
            this.Controls.Add(this.labSortBy);
            this.Controls.Add(this.comboFilterQRCode);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.grid1);
            this.Name = "WH_Receive_QRCodeSticker";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Print Fabric Sticker";
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource)).EndInit();
            this.radioPanel1.ResumeLayout(false);
            this.radioPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid grid1;
        private Win.UI.Button btnPrint;
        private Win.UI.ListControlBindingSource listControlBindingSource;
        private Win.UI.Label label1;
        private Win.UI.ComboBox comboFilterQRCode;
        private Win.UI.RadioPanel radioPanel1;
        private Win.UI.RadioButton radioEncodeSeq;
        private Win.UI.RadioButton radiobySP;
        private Win.UI.Label labSortBy;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
    }
}
