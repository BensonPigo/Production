namespace Sci.Production.Cutting
{
    partial class P20_Import_RFID
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelTOP = new Sci.Win.UI.Panel();
            this.txtfactory1 = new Sci.Production.Class.Txtfactory();
            this.lbFactory = new Sci.Win.UI.Label();
            this.dateRFID = new Sci.Win.UI.DateRange();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.btnQuery = new Sci.Win.UI.Button();
            this.panelBOTTOM = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.gridImport = new Sci.Win.UI.Grid();
            this.lbRFIDDate = new Sci.Win.UI.Label();
            this.lbSP = new Sci.Win.UI.Label();
            this.panelTOP.SuspendLayout();
            this.panelBOTTOM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).BeginInit();
            this.SuspendLayout();
            // 
            // panelTOP
            // 
            this.panelTOP.Controls.Add(this.lbSP);
            this.panelTOP.Controls.Add(this.lbRFIDDate);
            this.panelTOP.Controls.Add(this.txtfactory1);
            this.panelTOP.Controls.Add(this.lbFactory);
            this.panelTOP.Controls.Add(this.dateRFID);
            this.panelTOP.Controls.Add(this.txtSP);
            this.panelTOP.Controls.Add(this.btnQuery);
            this.panelTOP.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTOP.Location = new System.Drawing.Point(0, 0);
            this.panelTOP.Name = "panelTOP";
            this.panelTOP.Size = new System.Drawing.Size(883, 44);
            this.panelTOP.TabIndex = 1;
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.BoolFtyGroupList = true;
            this.txtfactory1.FilteMDivision = false;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.IsProduceFty = false;
            this.txtfactory1.IssupportJunk = false;
            this.txtfactory1.Location = new System.Drawing.Point(650, 11);
            this.txtfactory1.MDivision = null;
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.Size = new System.Drawing.Size(66, 23);
            this.txtfactory1.TabIndex = 21;
            // 
            // lbFactory
            // 
            this.lbFactory.Location = new System.Drawing.Point(589, 11);
            this.lbFactory.Name = "lbFactory";
            this.lbFactory.Size = new System.Drawing.Size(58, 23);
            this.lbFactory.TabIndex = 20;
            this.lbFactory.Text = "Factory:";
            // 
            // dateRFID
            // 
            // 
            // 
            // 
            this.dateRFID.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRFID.DateBox1.Name = "";
            this.dateRFID.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRFID.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRFID.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRFID.DateBox2.Name = "";
            this.dateRFID.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRFID.DateBox2.TabIndex = 1;
            this.dateRFID.IsRequired = false;
            this.dateRFID.Location = new System.Drawing.Point(115, 11);
            this.dateRFID.Name = "dateRFID";
            this.dateRFID.Size = new System.Drawing.Size(280, 23);
            this.dateRFID.TabIndex = 19;
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(441, 11);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(145, 23);
            this.txtSP.TabIndex = 17;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(793, 8);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 1;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // panelBOTTOM
            // 
            this.panelBOTTOM.Controls.Add(this.btnClose);
            this.panelBOTTOM.Controls.Add(this.btnImport);
            this.panelBOTTOM.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBOTTOM.Location = new System.Drawing.Point(0, 418);
            this.panelBOTTOM.Name = "panelBOTTOM";
            this.panelBOTTOM.Size = new System.Drawing.Size(883, 39);
            this.panelBOTTOM.TabIndex = 8;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(791, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(705, 5);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(80, 30);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
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
            this.gridImport.Location = new System.Drawing.Point(0, 44);
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
            this.gridImport.Size = new System.Drawing.Size(883, 374);
            this.gridImport.TabIndex = 9;
            this.gridImport.TabStop = false;
            // 
            // lbRFIDDate
            // 
            this.lbRFIDDate.Location = new System.Drawing.Point(14, 11);
            this.lbRFIDDate.Name = "lbRFIDDate";
            this.lbRFIDDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbRFIDDate.Size = new System.Drawing.Size(98, 23);
            this.lbRFIDDate.TabIndex = 130;
            this.lbRFIDDate.Text = "RFID Date";
            this.lbRFIDDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // lbSP
            // 
            this.lbSP.Location = new System.Drawing.Point(402, 11);
            this.lbSP.Name = "lbSP";
            this.lbSP.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbSP.Size = new System.Drawing.Size(36, 23);
            this.lbSP.TabIndex = 131;
            this.lbSP.Text = "SP#";
            this.lbSP.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // P20_Import_RFID
            // 
            this.ClientSize = new System.Drawing.Size(883, 457);
            this.Controls.Add(this.gridImport);
            this.Controls.Add(this.panelBOTTOM);
            this.Controls.Add(this.panelTOP);
            this.Name = "P20_Import_RFID";
            this.OnLineHelpID = "Sci.Win.Forms.Base";
            this.Text = "Import From RFID";
            this.panelTOP.ResumeLayout(false);
            this.panelTOP.PerformLayout();
            this.panelBOTTOM.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panelTOP;
        private Win.UI.TextBox txtSP;
        private Win.UI.Button btnQuery;
        private Win.UI.Panel panelBOTTOM;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnImport;
        private Win.UI.Grid gridImport;
        private Win.UI.DateRange dateRFID;
        private Class.Txtfactory txtfactory1;
        private Win.UI.Label lbFactory;
        private Win.UI.Label lbSP;
        private Win.UI.Label lbRFIDDate;
    }
}
