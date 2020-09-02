namespace Sci.Production.Warehouse
{
    partial class P03_InspectionList
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.labelLaboratory = new Sci.Win.UI.Label();
            this.labelInspection = new Sci.Win.UI.Label();
            this.gridFir_Laboratory = new Sci.Win.UI.Grid();
            this.gridFirAir = new Sci.Win.UI.Grid();
            this.panel2 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.bsAIR_FIR = new Sci.Win.UI.BindingSource(this.components);
            this.bsFIR_Laboratory = new Sci.Win.UI.BindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFir_Laboratory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFirAir)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsAIR_FIR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFIR_Laboratory)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelLaboratory);
            this.panel1.Controls.Add(this.labelInspection);
            this.panel1.Controls.Add(this.gridFir_Laboratory);
            this.panel1.Controls.Add(this.gridFirAir);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 561);
            this.panel1.TabIndex = 0;
            // 
            // labelLaboratory
            // 
            this.labelLaboratory.Lines = 0;
            this.labelLaboratory.Location = new System.Drawing.Point(9, 308);
            this.labelLaboratory.Name = "labelLaboratory";
            this.labelLaboratory.Size = new System.Drawing.Size(75, 23);
            this.labelLaboratory.TabIndex = 3;
            this.labelLaboratory.Text = "Laboratory";
            // 
            // labelInspection
            // 
            this.labelInspection.Lines = 0;
            this.labelInspection.Location = new System.Drawing.Point(9, 14);
            this.labelInspection.Name = "labelInspection";
            this.labelInspection.Size = new System.Drawing.Size(75, 23);
            this.labelInspection.TabIndex = 2;
            this.labelInspection.Text = "Inspection";
            // 
            // gridFir_Laboratory
            // 
            this.gridFir_Laboratory.AllowUserToAddRows = false;
            this.gridFir_Laboratory.AllowUserToDeleteRows = false;
            this.gridFir_Laboratory.AllowUserToResizeRows = false;
            this.gridFir_Laboratory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridFir_Laboratory.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridFir_Laboratory.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridFir_Laboratory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFir_Laboratory.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridFir_Laboratory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridFir_Laboratory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridFir_Laboratory.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridFir_Laboratory.Location = new System.Drawing.Point(3, 334);
            this.gridFir_Laboratory.Name = "gridFir_Laboratory";
            this.gridFir_Laboratory.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridFir_Laboratory.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridFir_Laboratory.RowTemplate.Height = 24;
            this.gridFir_Laboratory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFir_Laboratory.Size = new System.Drawing.Size(778, 173);
            this.gridFir_Laboratory.TabIndex = 1;
            this.gridFir_Laboratory.TabStop = false;
            // 
            // gridFirAir
            // 
            this.gridFirAir.AllowUserToAddRows = false;
            this.gridFirAir.AllowUserToDeleteRows = false;
            this.gridFirAir.AllowUserToResizeRows = false;
            this.gridFirAir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridFirAir.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridFirAir.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridFirAir.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFirAir.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridFirAir.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridFirAir.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridFirAir.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridFirAir.Location = new System.Drawing.Point(0, 40);
            this.gridFirAir.Name = "gridFirAir";
            this.gridFirAir.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridFirAir.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridFirAir.RowTemplate.Height = 24;
            this.gridFirAir.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFirAir.Size = new System.Drawing.Size(781, 260);
            this.gridFirAir.TabIndex = 0;
            this.gridFirAir.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 513);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(784, 48);
            this.panel2.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(692, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // P03_InspectionList
            // 
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "P03_InspectionList";
            this.Text = "Inspection List";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridFir_Laboratory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridFirAir)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bsAIR_FIR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsFIR_Laboratory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Grid gridFirAir;
        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private Win.UI.BindingSource bsAIR_FIR;
        private Win.UI.Label labelLaboratory;
        private Win.UI.Label labelInspection;
        private Win.UI.Grid gridFir_Laboratory;
        private Win.UI.BindingSource bsFIR_Laboratory;
    }
}
