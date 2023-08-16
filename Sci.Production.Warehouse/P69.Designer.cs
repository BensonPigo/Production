namespace Sci.Production.Warehouse
{
    partial class P69
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new Sci.Win.UI.Panel();
            this.btnCreateSeq = new Sci.Win.UI.Button();
            this.btnQuery = new Sci.Win.UI.Button();
            this.btnNewSearch = new Sci.Win.UI.Button();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.panel3 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.panel2 = new Sci.Win.UI.Panel();
            this.grid = new Sci.Win.UI.Grid();
            this.detail_contextMenuStrip = new Sci.Win.UI.ContextMenuStrip();
            this.Edit_Seq = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.detail_contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCreateSeq);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.btnNewSearch);
            this.panel1.Controls.Add(this.txtSP);
            this.panel1.Controls.Add(this.labelSPNo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(864, 43);
            this.panel1.TabIndex = 3;
            // 
            // btnCreateSeq
            // 
            this.btnCreateSeq.Location = new System.Drawing.Point(494, 6);
            this.btnCreateSeq.Name = "btnCreateSeq";
            this.btnCreateSeq.Size = new System.Drawing.Size(111, 30);
            this.btnCreateSeq.TabIndex = 22;
            this.btnCreateSeq.Text = "Create Seq";
            this.btnCreateSeq.UseVisualStyleBackColor = true;
            this.btnCreateSeq.Click += new System.EventHandler(this.BtnCreateSeq_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(377, 7);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(111, 30);
            this.btnQuery.TabIndex = 21;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // btnNewSearch
            // 
            this.btnNewSearch.Location = new System.Drawing.Point(260, 7);
            this.btnNewSearch.Name = "btnNewSearch";
            this.btnNewSearch.Size = new System.Drawing.Size(111, 30);
            this.btnNewSearch.TabIndex = 20;
            this.btnNewSearch.Text = "New Search";
            this.btnNewSearch.UseVisualStyleBackColor = true;
            this.btnNewSearch.Click += new System.EventHandler(this.BtnNewSearch_Click);
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.IsSupportEditMode = false;
            this.txtSP.Location = new System.Drawing.Point(121, 9);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(133, 23);
            this.txtSP.TabIndex = 19;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(9, 9);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(109, 23);
            this.labelSPNo.TabIndex = 18;
            this.labelSPNo.Text = "SP#";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 392);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(864, 60);
            this.panel3.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(778, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grid);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 43);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(864, 349);
            this.panel2.TabIndex = 5;
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.ContextMenuStrip = this.detail_contextMenuStrip;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(3, 3);
            this.grid.Name = "grid";
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(855, 343);
            this.grid.TabIndex = 1;
            this.grid.TabStop = false;
            // 
            // detail_contextMenuStrip
            // 
            this.detail_contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Edit_Seq});
            this.detail_contextMenuStrip.Name = "art_contextMenuStrip";
            this.detail_contextMenuStrip.Size = new System.Drawing.Size(122, 26);
            // 
            // Edit_Seq
            // 
            this.Edit_Seq.Name = "Edit_Seq";
            this.Edit_Seq.Size = new System.Drawing.Size(121, 22);
            this.Edit_Seq.Text = "Edit Seq";
            this.Edit_Seq.Click += new System.EventHandler(this.Edit_Seq_Click);
            // 
            // P69
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 452);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "P69";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P69. Material Status (Local Order)";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.detail_contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Label labelSPNo;
        private Win.UI.Panel panel3;
        private Win.UI.Button btnClose;
        private Win.UI.Panel panel2;
        private Win.UI.Grid grid;
        private Win.UI.TextBox txtSP;
        private Win.UI.Button btnCreateSeq;
        private Win.UI.Button btnQuery;
        private Win.UI.Button btnNewSearch;
        private Win.UI.ContextMenuStrip detail_contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem Edit_Seq;
    }
}