namespace Sci.Production.Warehouse
{
    partial class P06_ExportDetail
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
            this.gridExportMaterial = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnClose = new Sci.Win.UI.Button();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.displayFromSP = new Sci.Win.UI.DisplayBox();
            this.displayFromSeq = new Sci.Win.UI.DisplayBox();
            this.displayRefno = new Sci.Win.UI.DisplayBox();
            this.displayColorID = new Sci.Win.UI.DisplayBox();
            this.displayToSP = new Sci.Win.UI.DisplayBox();
            this.displayToSeq = new Sci.Win.UI.DisplayBox();
            this.displayExportQty = new Sci.Win.UI.DisplayBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridExportMaterial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridExportMaterial
            // 
            this.gridExportMaterial.AllowUserToAddRows = false;
            this.gridExportMaterial.AllowUserToDeleteRows = false;
            this.gridExportMaterial.AllowUserToResizeRows = false;
            this.gridExportMaterial.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridExportMaterial.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridExportMaterial.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridExportMaterial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridExportMaterial.DataSource = this.listControlBindingSource1;
            this.gridExportMaterial.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridExportMaterial.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridExportMaterial.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridExportMaterial.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridExportMaterial.Location = new System.Drawing.Point(12, 65);
            this.gridExportMaterial.Name = "gridExportMaterial";
            this.gridExportMaterial.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridExportMaterial.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridExportMaterial.RowTemplate.Height = 24;
            this.gridExportMaterial.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridExportMaterial.ShowCellToolTips = false;
            this.gridExportMaterial.Size = new System.Drawing.Size(866, 349);
            this.gridExportMaterial.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(798, 422);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "From SP#";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(260, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "From Seq";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(471, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "Refno";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(682, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 5;
            this.label4.Text = "ColorID";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(12, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 6;
            this.label5.Text = "To SP#";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(260, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 23);
            this.label6.TabIndex = 7;
            this.label6.Text = "To Seq";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(471, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 23);
            this.label7.TabIndex = 8;
            this.label7.Text = "Export Qty";
            // 
            // displayFromSP
            // 
            this.displayFromSP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFromSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFromSP.Location = new System.Drawing.Point(90, 9);
            this.displayFromSP.Name = "displayFromSP";
            this.displayFromSP.Size = new System.Drawing.Size(167, 23);
            this.displayFromSP.TabIndex = 9;
            // 
            // displayFromSeq
            // 
            this.displayFromSeq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFromSeq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFromSeq.Location = new System.Drawing.Point(338, 9);
            this.displayFromSeq.Name = "displayFromSeq";
            this.displayFromSeq.Size = new System.Drawing.Size(130, 23);
            this.displayFromSeq.TabIndex = 10;
            // 
            // displayRefno
            // 
            this.displayRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayRefno.Location = new System.Drawing.Point(549, 9);
            this.displayRefno.Name = "displayRefno";
            this.displayRefno.Size = new System.Drawing.Size(130, 23);
            this.displayRefno.TabIndex = 11;
            // 
            // displayColorID
            // 
            this.displayColorID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayColorID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayColorID.Location = new System.Drawing.Point(760, 9);
            this.displayColorID.Name = "displayColorID";
            this.displayColorID.Size = new System.Drawing.Size(118, 23);
            this.displayColorID.TabIndex = 12;
            // 
            // displayToSP
            // 
            this.displayToSP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayToSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayToSP.Location = new System.Drawing.Point(90, 36);
            this.displayToSP.Name = "displayToSP";
            this.displayToSP.Size = new System.Drawing.Size(167, 23);
            this.displayToSP.TabIndex = 13;
            // 
            // displayToSeq
            // 
            this.displayToSeq.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayToSeq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayToSeq.Location = new System.Drawing.Point(338, 36);
            this.displayToSeq.Name = "displayToSeq";
            this.displayToSeq.Size = new System.Drawing.Size(130, 23);
            this.displayToSeq.TabIndex = 14;
            // 
            // displayExportQty
            // 
            this.displayExportQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayExportQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayExportQty.Location = new System.Drawing.Point(549, 36);
            this.displayExportQty.Name = "displayExportQty";
            this.displayExportQty.Size = new System.Drawing.Size(130, 23);
            this.displayExportQty.TabIndex = 15;
            // 
            // P06_ExportDetail
            // 
            this.ClientSize = new System.Drawing.Size(890, 464);
            this.Controls.Add(this.displayExportQty);
            this.Controls.Add(this.displayToSeq);
            this.Controls.Add(this.displayToSP);
            this.Controls.Add(this.displayColorID);
            this.Controls.Add(this.displayRefno);
            this.Controls.Add(this.displayFromSeq);
            this.Controls.Add(this.displayFromSP);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gridExportMaterial);
            this.Name = "P06_ExportDetail";
            this.OnLineHelpID = "Sci.Win.Forms.Base";
            this.Text = "Export Material";
            ((System.ComponentModel.ISupportInitialize)(this.gridExportMaterial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Grid gridExportMaterial;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Button btnClose;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Win.UI.DisplayBox displayFromSP;
        private Win.UI.DisplayBox displayFromSeq;
        private Win.UI.DisplayBox displayRefno;
        private Win.UI.DisplayBox displayColorID;
        private Win.UI.DisplayBox displayToSP;
        private Win.UI.DisplayBox displayToSeq;
        private Win.UI.DisplayBox displayExportQty;
    }
}
