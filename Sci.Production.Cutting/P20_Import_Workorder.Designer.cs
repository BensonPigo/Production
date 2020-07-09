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
            this.dateEstCutDate = new Sci.Win.UI.DateBox();
            this.panelTOP = new Sci.Win.UI.Panel();
            this.label2 = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtCutRef = new Sci.Win.UI.TextBox();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.panelBOTTOM = new Sci.Win.UI.Panel();
            this.panelMIDDLE = new Sci.Win.UI.Panel();
            this.lbEstCutDate = new Sci.Win.UI.Label();
            this.lbSP = new Sci.Win.UI.Label();
            this.lbCutRef = new Sci.Win.UI.Label();
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
            this.gridImport.ShowCellToolTips = false;
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
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
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
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
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
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
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
            this.panelTOP.Controls.Add(this.lbCutRef);
            this.panelTOP.Controls.Add(this.lbSP);
            this.panelTOP.Controls.Add(this.lbEstCutDate);
            this.panelTOP.Controls.Add(this.label2);
            this.panelTOP.Controls.Add(this.txtfactory);
            this.panelTOP.Controls.Add(this.txtCutRef);
            this.panelTOP.Controls.Add(this.txtSP);
            this.panelTOP.Controls.Add(this.dateEstCutDate);
            this.panelTOP.Controls.Add(this.btnQuery);
            this.panelTOP.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTOP.Location = new System.Drawing.Point(0, 0);
            this.panelTOP.Name = "panelTOP";
            this.panelTOP.Size = new System.Drawing.Size(944, 44);
            this.panelTOP.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(646, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 23);
            this.label2.TabIndex = 22;
            this.label2.Text = "Factory";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = true;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(712, 12);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 21;
            // 
            // txtCutRef
            // 
            this.txtCutRef.BackColor = System.Drawing.Color.White;
            this.txtCutRef.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutRef.Location = new System.Drawing.Point(530, 12);
            this.txtCutRef.Name = "txtCutRef";
            this.txtCutRef.Size = new System.Drawing.Size(101, 23);
            this.txtCutRef.TabIndex = 19;
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(304, 12);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(145, 23);
            this.txtSP.TabIndex = 17;
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
            // lbEstCutDate
            // 
            this.lbEstCutDate.Location = new System.Drawing.Point(14, 12);
            this.lbEstCutDate.Name = "lbEstCutDate";
            this.lbEstCutDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbEstCutDate.Size = new System.Drawing.Size(98, 23);
            this.lbEstCutDate.TabIndex = 129;
            this.lbEstCutDate.Text = "Est. Cut Date";
            this.lbEstCutDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // lbSP
            // 
            this.lbSP.Location = new System.Drawing.Point(260, 12);
            this.lbSP.Name = "lbSP";
            this.lbSP.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbSP.Size = new System.Drawing.Size(41, 23);
            this.lbSP.TabIndex = 130;
            this.lbSP.Text = "SP#";
            this.lbSP.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // lbCutRef
            // 
            this.lbCutRef.Location = new System.Drawing.Point(467, 12);
            this.lbCutRef.Name = "lbCutRef";
            this.lbCutRef.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbCutRef.Size = new System.Drawing.Size(60, 23);
            this.lbCutRef.TabIndex = 131;
            this.lbCutRef.Text = "CutRef#";
            this.lbCutRef.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // P20_Import_Workorder
            // 
            this.ClientSize = new System.Drawing.Size(944, 522);
            this.Controls.Add(this.panelMIDDLE);
            this.Controls.Add(this.panelBOTTOM);
            this.Controls.Add(this.panelTOP);
            this.DefaultControl = "dateEstCutDate";
            this.Name = "P20_Import_Workorder";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P20_Import";
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
            this.panelTOP.ResumeLayout(false);
            this.panelTOP.PerformLayout();
            this.panelBOTTOM.ResumeLayout(false);
            this.panelMIDDLE.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid gridImport;
        private Win.UI.Button btnQuery;
        private Win.UI.Button btnImport;
        private Win.UI.Button btnClose;
        private Win.UI.DateBox dateEstCutDate;
        private Win.UI.Panel panelTOP;
        private Win.UI.Panel panelBOTTOM;
        private Win.UI.Panel panelMIDDLE;
        private Win.UI.TextBox txtCutRef;
        private Win.UI.TextBox txtSP;
        private Class.Txtfactory txtfactory;
        private Win.UI.Label label2;
        private Win.UI.Label lbCutRef;
        private Win.UI.Label lbSP;
        private Win.UI.Label lbEstCutDate;
    }
}
