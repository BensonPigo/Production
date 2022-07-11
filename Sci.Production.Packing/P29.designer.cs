namespace Sci.Production.Packing
{
    partial class P29
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new Sci.Win.UI.Panel();
            this.lbSP = new Sci.Win.UI.Label();
            this.lbPackID = new Sci.Win.UI.Label();
            this.labAuditDate = new Sci.Win.UI.Label();
            this.buttonFindNow = new Sci.Win.UI.Button();
            this.dateAuditDate = new Sci.Win.UI.DateRange();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.txtPackID = new Sci.Win.UI.TextBox();
            this.labelSP = new Sci.Win.UI.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridMain = new Sci.Win.UI.Grid();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.label1 = new Sci.Win.UI.Label();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelSP);
            this.panel1.Controls.Add(this.lbSP);
            this.panel1.Controls.Add(this.lbPackID);
            this.panel1.Controls.Add(this.labAuditDate);
            this.panel1.Controls.Add(this.buttonFindNow);
            this.panel1.Controls.Add(this.dateAuditDate);
            this.panel1.Controls.Add(this.txtSP);
            this.panel1.Controls.Add(this.txtPackID);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(979, 64);
            this.panel1.TabIndex = 1;
            // 
            // lbSP
            // 
            this.lbSP.BackColor = System.Drawing.Color.LightSkyBlue;
            this.lbSP.Location = new System.Drawing.Point(640, 6);
            this.lbSP.Name = "lbSP";
            this.lbSP.Size = new System.Drawing.Size(75, 23);
            this.lbSP.TabIndex = 6;
            this.lbSP.Text = "SP#";
            this.lbSP.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // lbPackID
            // 
            this.lbPackID.BackColor = System.Drawing.Color.LightSkyBlue;
            this.lbPackID.Location = new System.Drawing.Point(399, 6);
            this.lbPackID.Name = "lbPackID";
            this.lbPackID.Size = new System.Drawing.Size(93, 23);
            this.lbPackID.TabIndex = 5;
            this.lbPackID.Text = "Pack ID";
            this.lbPackID.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labAuditDate
            // 
            this.labAuditDate.BackColor = System.Drawing.Color.LightSkyBlue;
            this.labAuditDate.Location = new System.Drawing.Point(6, 6);
            this.labAuditDate.Name = "labAuditDate";
            this.labAuditDate.Size = new System.Drawing.Size(93, 23);
            this.labAuditDate.TabIndex = 4;
            this.labAuditDate.Text = "Audit Date";
            this.labAuditDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // buttonFindNow
            // 
            this.buttonFindNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFindNow.Location = new System.Drawing.Point(871, 19);
            this.buttonFindNow.Name = "buttonFindNow";
            this.buttonFindNow.Size = new System.Drawing.Size(99, 30);
            this.buttonFindNow.TabIndex = 3;
            this.buttonFindNow.Text = "Find Now";
            this.buttonFindNow.UseVisualStyleBackColor = true;
            this.buttonFindNow.Click += new System.EventHandler(this.ButtonFindNow_Click);
            // 
            // dateAuditDate
            // 
            // 
            // 
            // 
            this.dateAuditDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateAuditDate.DateBox1.Name = "";
            this.dateAuditDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateAuditDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateAuditDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateAuditDate.DateBox2.Name = "";
            this.dateAuditDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateAuditDate.DateBox2.TabIndex = 1;
            this.dateAuditDate.IsRequired = false;
            this.dateAuditDate.Location = new System.Drawing.Point(102, 6);
            this.dateAuditDate.Name = "dateAuditDate";
            this.dateAuditDate.Size = new System.Drawing.Size(280, 23);
            this.dateAuditDate.TabIndex = 0;
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(718, 6);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(121, 23);
            this.txtSP.TabIndex = 2;
            // 
            // txtPackID
            // 
            this.txtPackID.BackColor = System.Drawing.Color.White;
            this.txtPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackID.Location = new System.Drawing.Point(495, 6);
            this.txtPackID.Name = "txtPackID";
            this.txtPackID.Size = new System.Drawing.Size(121, 23);
            this.txtPackID.TabIndex = 1;
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(6, 35);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(149, 23);
            this.labelSP.TabIndex = 7;
            this.labelSP.Text = "Packing Audit Record";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 64);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridMain);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(979, 443);
            this.splitContainer1.SplitterDistance = 260;
            this.splitContainer1.TabIndex = 6;
            // 
            // gridMain
            // 
            this.gridMain.AllowUserToAddRows = false;
            this.gridMain.AllowUserToDeleteRows = false;
            this.gridMain.AllowUserToResizeRows = false;
            this.gridMain.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridMain.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridMain.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridMain.DataSource = this.listControlBindingSource1;
            this.gridMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridMain.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridMain.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridMain.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridMain.Location = new System.Drawing.Point(0, 0);
            this.gridMain.Name = "gridMain";
            this.gridMain.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridMain.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridMain.RowTemplate.Height = 24;
            this.gridMain.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridMain.ShowCellToolTips = false;
            this.gridMain.Size = new System.Drawing.Size(979, 260);
            this.gridMain.TabIndex = 0;
            this.gridMain.TabStop = false;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.gridDetail);
            this.splitContainer2.Size = new System.Drawing.Size(979, 179);
            this.splitContainer2.SplitterDistance = 32;
            this.splitContainer2.TabIndex = 6;
            // 
            // gridDetail
            // 
            this.gridDetail.AllowUserToAddRows = false;
            this.gridDetail.AllowUserToDeleteRows = false;
            this.gridDetail.AllowUserToResizeRows = false;
            this.gridDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetail.DataSource = this.listControlBindingSource2;
            this.gridDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDetail.Location = new System.Drawing.Point(0, 0);
            this.gridDetail.Name = "gridDetail";
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.ShowCellToolTips = false;
            this.gridDetail.Size = new System.Drawing.Size(979, 143);
            this.gridDetail.TabIndex = 0;
            this.gridDetail.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(149, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "Type Of Error";
            // 
            // P29
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(979, 507);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.EditMode = true;
            this.Name = "P29";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P29. Query For Packing Audit Record";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.splitContainer1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridMain)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.DateRange dateAuditDate;
        private Win.UI.TextBox txtSP;
        private Win.UI.TextBox txtPackID;
        private Win.UI.Button buttonFindNow;
        private Win.UI.Label lbSP;
        private Win.UI.Label lbPackID;
        private Win.UI.Label labAuditDate;
        private Win.UI.Label labelSP;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Grid gridMain;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Win.UI.Label label1;
        private Win.UI.Grid gridDetail;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
    }
}