namespace Sci.Production.Cutting
{
    partial class P20_Import_Workorder
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridImport = new Sci.Win.UI.Grid();
            this.btnQuery = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.labelEstCutDate = new Sci.Win.UI.Label();
            this.dateEstCutDate = new Sci.Win.UI.DateBox();
            this.panelTOP = new Sci.Win.UI.Panel();
            this.panelBOTTOM = new Sci.Win.UI.Panel();
            this.panelMIDDLE = new Sci.Win.UI.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).BeginInit();
            this.panelTOP.SuspendLayout();
            this.panelBOTTOM.SuspendLayout();
            this.panelMIDDLE.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridImport
            // 
            this.gridImport.AllowUserToAddRows = false;
            this.gridImport.AllowUserToDeleteRows = false;
            this.gridImport.AllowUserToResizeRows = false;
            this.gridImport.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridImport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridImport.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridImport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridImport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridImport.Location = new System.Drawing.Point(0, 0);
            this.gridImport.Name = "gridImport";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridImport.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridImport.RowTemplate.Height = 24;
            this.gridImport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridImport.Size = new System.Drawing.Size(944, 439);
            this.gridImport.TabIndex = 3;
            this.gridImport.TabStop = false;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(852, 7);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 1;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.Query_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(766, 5);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(80, 30);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.Import_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(852, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.Close_Click);
            // 
            // labelEstCutDate
            // 
            this.labelEstCutDate.Lines = 0;
            this.labelEstCutDate.Location = new System.Drawing.Point(13, 12);
            this.labelEstCutDate.Name = "labelEstCutDate";
            this.labelEstCutDate.Size = new System.Drawing.Size(99, 23);
            this.labelEstCutDate.TabIndex = 5;
            this.labelEstCutDate.Text = "Est. Cut Date";
            // 
            // dateEstCutDate
            // 
            this.dateEstCutDate.Location = new System.Drawing.Point(115, 12);
            this.dateEstCutDate.Name = "dateEstCutDate";
            this.dateEstCutDate.Size = new System.Drawing.Size(130, 23);
            this.dateEstCutDate.TabIndex = 0;
            // 
            // panelTOP
            // 
            this.panelTOP.Controls.Add(this.labelEstCutDate);
            this.panelTOP.Controls.Add(this.dateEstCutDate);
            this.panelTOP.Controls.Add(this.btnQuery);
            this.panelTOP.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTOP.Location = new System.Drawing.Point(0, 0);
            this.panelTOP.Name = "panelTOP";
            this.panelTOP.Size = new System.Drawing.Size(944, 44);
            this.panelTOP.TabIndex = 0;
            // 
            // panelBOTTOM
            // 
            this.panelBOTTOM.Controls.Add(this.btnClose);
            this.panelBOTTOM.Controls.Add(this.btnImport);
            this.panelBOTTOM.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBOTTOM.Location = new System.Drawing.Point(0, 483);
            this.panelBOTTOM.Name = "panelBOTTOM";
            this.panelBOTTOM.Size = new System.Drawing.Size(944, 39);
            this.panelBOTTOM.TabIndex = 7;
            // 
            // panelMIDDLE
            // 
            this.panelMIDDLE.Controls.Add(this.gridImport);
            this.panelMIDDLE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMIDDLE.Location = new System.Drawing.Point(0, 44);
            this.panelMIDDLE.Name = "panelMIDDLE";
            this.panelMIDDLE.Size = new System.Drawing.Size(944, 439);
            this.panelMIDDLE.TabIndex = 8;
            // 
            // P20_Import_Workorder
            // 
            this.ClientSize = new System.Drawing.Size(944, 522);
            this.Controls.Add(this.panelMIDDLE);
            this.Controls.Add(this.panelBOTTOM);
            this.Controls.Add(this.panelTOP);
            this.DefaultControl = "dateBox1";
            this.Name = "P20_Import_Workorder";
            this.Text = "P20_Import";
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
            this.panelTOP.ResumeLayout(false);
            this.panelBOTTOM.ResumeLayout(false);
            this.panelMIDDLE.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid gridImport;
        private Win.UI.Button btnQuery;
        private Win.UI.Button btnImport;
        private new Win.UI.Button btnClose;
        private Win.UI.Label labelEstCutDate;
        private Win.UI.DateBox dateEstCutDate;
        private Win.UI.Panel panelTOP;
        private Win.UI.Panel panelBOTTOM;
        private Win.UI.Panel panelMIDDLE;
    }
}
