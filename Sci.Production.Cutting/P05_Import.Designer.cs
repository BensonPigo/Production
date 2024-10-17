namespace Sci.Production.Cutting
{
    partial class P05_Import
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
            this.gridImport = new Sci.Win.UI.Grid();
            this.gridBS = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnQuery = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.labelCutCell = new Sci.Win.UI.Label();
            this.labelEstCutDate = new Sci.Win.UI.Label();
            this.txtCutCell = new Sci.Production.Class.TxtCell();
            this.dateEstCutDate = new Sci.Win.UI.DateBox();
            this.panelTop = new Sci.Win.UI.Panel();
            this.txtMarkName = new Sci.Win.UI.TextBox();
            this.lbMarkerName = new Sci.Win.UI.Label();
            this.txtCuttingSP = new Sci.Win.UI.TextBox();
            this.lbCuttingSP = new Sci.Win.UI.Label();
            this.panelBottom = new Sci.Win.UI.Panel();
            this.panelMiddle = new Sci.Win.UI.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBS)).BeginInit();
            this.panelTop.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.panelMiddle.SuspendLayout();
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
            this.gridImport.DataSource = this.gridBS;
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
            this.gridImport.Size = new System.Drawing.Size(944, 410);
            this.gridImport.TabIndex = 8;
            this.gridImport.TabStop = false;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(827, 7);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 4;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(748, 7);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(80, 30);
            this.btnImport.TabIndex = 5;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(834, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // labelCutCell
            // 
            this.labelCutCell.Location = new System.Drawing.Point(17, 37);
            this.labelCutCell.Name = "labelCutCell";
            this.labelCutCell.Size = new System.Drawing.Size(99, 23);
            this.labelCutCell.TabIndex = 4;
            this.labelCutCell.Text = "Cut Cell";
            // 
            // labelEstCutDate
            // 
            this.labelEstCutDate.BackColor = System.Drawing.Color.SkyBlue;
            this.labelEstCutDate.Location = new System.Drawing.Point(17, 11);
            this.labelEstCutDate.Name = "labelEstCutDate";
            this.labelEstCutDate.Size = new System.Drawing.Size(99, 23);
            this.labelEstCutDate.TabIndex = 5;
            this.labelEstCutDate.Text = "Est. Cut Date";
            this.labelEstCutDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtCutCell
            // 
            this.txtCutCell.BackColor = System.Drawing.Color.White;
            this.txtCutCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutCell.Location = new System.Drawing.Point(119, 37);
            this.txtCutCell.MDivisionID = "";
            this.txtCutCell.Name = "txtCutCell";
            this.txtCutCell.Size = new System.Drawing.Size(108, 23);
            this.txtCutCell.TabIndex = 3;
            // 
            // dateEstCutDate
            // 
            this.dateEstCutDate.Location = new System.Drawing.Point(119, 11);
            this.dateEstCutDate.Name = "dateEstCutDate";
            this.dateEstCutDate.Size = new System.Drawing.Size(130, 23);
            this.dateEstCutDate.TabIndex = 0;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.txtMarkName);
            this.panelTop.Controls.Add(this.lbMarkerName);
            this.panelTop.Controls.Add(this.txtCuttingSP);
            this.panelTop.Controls.Add(this.lbCuttingSP);
            this.panelTop.Controls.Add(this.labelEstCutDate);
            this.panelTop.Controls.Add(this.dateEstCutDate);
            this.panelTop.Controls.Add(this.labelCutCell);
            this.panelTop.Controls.Add(this.btnQuery);
            this.panelTop.Controls.Add(this.txtCutCell);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(944, 69);
            this.panelTop.TabIndex = 7;
            // 
            // txtMarkName
            // 
            this.txtMarkName.BackColor = System.Drawing.Color.White;
            this.txtMarkName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMarkName.Location = new System.Drawing.Point(558, 11);
            this.txtMarkName.Name = "txtMarkName";
            this.txtMarkName.Size = new System.Drawing.Size(100, 23);
            this.txtMarkName.TabIndex = 2;
            // 
            // lbMarkerName
            // 
            this.lbMarkerName.BackColor = System.Drawing.Color.SkyBlue;
            this.lbMarkerName.Location = new System.Drawing.Point(466, 11);
            this.lbMarkerName.Name = "lbMarkerName";
            this.lbMarkerName.Size = new System.Drawing.Size(89, 23);
            this.lbMarkerName.TabIndex = 9;
            this.lbMarkerName.Text = "Marker Name";
            this.lbMarkerName.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtCuttingSP
            // 
            this.txtCuttingSP.BackColor = System.Drawing.Color.White;
            this.txtCuttingSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCuttingSP.Location = new System.Drawing.Point(354, 11);
            this.txtCuttingSP.Name = "txtCuttingSP";
            this.txtCuttingSP.Size = new System.Drawing.Size(100, 23);
            this.txtCuttingSP.TabIndex = 1;
            // 
            // lbCuttingSP
            // 
            this.lbCuttingSP.BackColor = System.Drawing.Color.SkyBlue;
            this.lbCuttingSP.Location = new System.Drawing.Point(262, 11);
            this.lbCuttingSP.Name = "lbCuttingSP";
            this.lbCuttingSP.Size = new System.Drawing.Size(89, 23);
            this.lbCuttingSP.TabIndex = 7;
            this.lbCuttingSP.Text = "Cutting SP#";
            this.lbCuttingSP.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // panelBottom
            // 
            this.panelBottom.Controls.Add(this.btnImport);
            this.panelBottom.Controls.Add(this.btnClose);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 479);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(944, 43);
            this.panelBottom.TabIndex = 7;
            // 
            // panelMiddle
            // 
            this.panelMiddle.Controls.Add(this.gridImport);
            this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMiddle.Location = new System.Drawing.Point(0, 69);
            this.panelMiddle.Name = "panelMiddle";
            this.panelMiddle.Size = new System.Drawing.Size(944, 410);
            this.panelMiddle.TabIndex = 8;
            // 
            // P05_Import
            // 
            this.ClientSize = new System.Drawing.Size(944, 522);
            this.Controls.Add(this.panelMiddle);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.DefaultControl = "dateEstCutDate";
            this.Name = "P05_Import";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P05_Import";
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBS)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelMiddle.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid gridImport;
        private Win.UI.Button btnQuery;
        private Win.UI.Button btnImport;
        private Win.UI.Button btnClose;
        private Win.UI.Label labelCutCell;
        private Win.UI.Label labelEstCutDate;
        private Class.TxtCell txtCutCell;
        private Win.UI.DateBox dateEstCutDate;
        private Win.UI.Panel panelTop;
        private Win.UI.Panel panelBottom;
        private Win.UI.Panel panelMiddle;
        private Win.UI.Label lbCuttingSP;
        private Win.UI.ListControlBindingSource gridBS;
        private Win.UI.TextBox txtCuttingSP;
        private Win.UI.TextBox txtMarkName;
        private Win.UI.Label lbMarkerName;
    }
}
