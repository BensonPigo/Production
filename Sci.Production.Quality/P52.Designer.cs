namespace Sci.Production.Quality
{
    partial class P52
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
            this.txtSP = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.dateETA = new Sci.Win.UI.DateRange();
            this.btnQuery = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.grid_Material = new Sci.Win.UI.Grid();
            this.bs_Material = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.grid_Document = new Sci.Win.UI.Grid();
            this.bs_Document = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.txtSeq1 = new Sci.Win.UI.TextBox();
            this.label3 = new Sci.Win.UI.Label();
            this.txtseq2 = new Sci.Win.UI.TextBox();
            this.label7 = new Sci.Win.UI.Label();
            this.txtSeason = new Sci.Production.Class.Txtseason();
            this.txtBrand = new Sci.Production.Class.Txtbrand();
            this.label14 = new Sci.Win.UI.Label();
            this.txtSupplier = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.dateATA = new Sci.Win.UI.DateRange();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.txtRefno = new Sci.Win.UI.TextBox();
            this.label12 = new Sci.Win.UI.Label();
            this.txtColor = new Sci.Win.UI.TextBox();
            this.label8 = new Sci.Win.UI.Label();
            this.comboMaterialType = new System.Windows.Forms.ComboBox();
            this.grid_Report = new Sci.Win.UI.Grid();
            this.bs_Report = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Material)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs_Material)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Document)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs_Document)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Report)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs_Report)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(113, 12);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(130, 23);
            this.txtSP.TabIndex = 0;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(14, 12);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(96, 23);
            this.labelSPNo.TabIndex = 12;
            this.labelSPNo.Text = "SP#";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(14, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 23);
            this.label2.TabIndex = 20;
            this.label2.Text = "ETA";
            // 
            // dateETA
            // 
            // 
            // 
            // 
            this.dateETA.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateETA.DateBox1.Name = "";
            this.dateETA.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateETA.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateETA.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateETA.DateBox2.Name = "";
            this.dateETA.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateETA.DateBox2.TabIndex = 1;
            this.dateETA.Location = new System.Drawing.Point(113, 68);
            this.dateETA.Name = "dateETA";
            this.dateETA.Size = new System.Drawing.Size(280, 23);
            this.dateETA.TabIndex = 8;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(819, 12);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(95, 30);
            this.btnQuery.TabIndex = 11;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(767, 565);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 23;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(853, 565);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 24;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // grid_Material
            // 
            this.grid_Material.AllowUserToAddRows = false;
            this.grid_Material.AllowUserToDeleteRows = false;
            this.grid_Material.AllowUserToResizeRows = false;
            this.grid_Material.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid_Material.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid_Material.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_Material.DataSource = this.bs_Material;
            this.grid_Material.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_Material.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid_Material.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid_Material.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid_Material.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid_Material.Location = new System.Drawing.Point(0, 0);
            this.grid_Material.Name = "grid_Material";
            this.grid_Material.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid_Material.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid_Material.RowTemplate.Height = 24;
            this.grid_Material.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_Material.ShowCellToolTips = false;
            this.grid_Material.Size = new System.Drawing.Size(669, 292);
            this.grid_Material.TabIndex = 117;
            // 
            // grid_Document
            // 
            this.grid_Document.AllowUserToAddRows = false;
            this.grid_Document.AllowUserToDeleteRows = false;
            this.grid_Document.AllowUserToResizeRows = false;
            this.grid_Document.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid_Document.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid_Document.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_Document.DataSource = this.bs_Document;
            this.grid_Document.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_Document.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid_Document.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid_Document.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid_Document.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid_Document.Location = new System.Drawing.Point(0, 0);
            this.grid_Document.Name = "grid_Document";
            this.grid_Document.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid_Document.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid_Document.RowTemplate.Height = 24;
            this.grid_Document.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_Document.ShowCellToolTips = false;
            this.grid_Document.Size = new System.Drawing.Size(253, 292);
            this.grid_Document.TabIndex = 171;
            // 
            // bs_Document
            // 
            this.bs_Document.PositionChanged += new System.EventHandler(this.bs_Document_PositionChanged);
            // 
            // txtSeq1
            // 
            this.txtSeq1.BackColor = System.Drawing.Color.White;
            this.txtSeq1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeq1.Location = new System.Drawing.Point(316, 12);
            this.txtSeq1.MaxLength = 3;
            this.txtSeq1.Name = "txtSeq1";
            this.txtSeq1.Size = new System.Drawing.Size(71, 23);
            this.txtSeq1.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(252, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 23);
            this.label3.TabIndex = 13;
            this.label3.Text = "SEQ";
            // 
            // txtseq2
            // 
            this.txtseq2.BackColor = System.Drawing.Color.White;
            this.txtseq2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseq2.Location = new System.Drawing.Point(390, 12);
            this.txtseq2.MaxLength = 2;
            this.txtseq2.Name = "txtseq2";
            this.txtseq2.Size = new System.Drawing.Size(52, 23);
            this.txtseq2.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(451, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 23);
            this.label7.TabIndex = 14;
            this.label7.Text = "Season";
            // 
            // txtSeason
            // 
            this.txtSeason.BackColor = System.Drawing.Color.White;
            this.txtSeason.BrandObjectName = null;
            this.txtSeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeason.Location = new System.Drawing.Point(522, 12);
            this.txtSeason.Name = "txtSeason";
            this.txtSeason.Size = new System.Drawing.Size(90, 23);
            this.txtSeason.TabIndex = 3;
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(693, 12);
            this.txtBrand.MyDocumentdName = null;
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(95, 23);
            this.txtBrand.TabIndex = 4;
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(615, 12);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(75, 23);
            this.label14.TabIndex = 15;
            this.label14.Text = "Brand";
            // 
            // txtSupplier
            // 
            this.txtSupplier.BackColor = System.Drawing.Color.White;
            this.txtSupplier.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSupplier.Location = new System.Drawing.Point(113, 40);
            this.txtSupplier.Name = "txtSupplier";
            this.txtSupplier.Size = new System.Drawing.Size(130, 23);
            this.txtSupplier.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 23);
            this.label1.TabIndex = 16;
            this.label1.Text = "Supplier";
            // 
            // dateATA
            // 
            // 
            // 
            // 
            this.dateATA.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateATA.DateBox1.Name = "";
            this.dateATA.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateATA.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateATA.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateATA.DateBox2.Name = "";
            this.dateATA.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateATA.DateBox2.TabIndex = 1;
            this.dateATA.Location = new System.Drawing.Point(522, 69);
            this.dateATA.Name = "dateATA";
            this.dateATA.Size = new System.Drawing.Size(280, 23);
            this.dateATA.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(451, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 23);
            this.label4.TabIndex = 21;
            this.label4.Text = "ATA";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(14, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 23);
            this.label5.TabIndex = 22;
            this.label5.Text = "Material Filter";
            // 
            // txtRefno
            // 
            this.txtRefno.BackColor = System.Drawing.Color.White;
            this.txtRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefno.Location = new System.Drawing.Point(316, 40);
            this.txtRefno.Name = "txtRefno";
            this.txtRefno.Size = new System.Drawing.Size(200, 23);
            this.txtRefno.TabIndex = 6;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(252, 40);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(59, 23);
            this.label12.TabIndex = 17;
            this.label12.Text = "Refno";
            // 
            // txtColor
            // 
            this.txtColor.BackColor = System.Drawing.Color.White;
            this.txtColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtColor.Location = new System.Drawing.Point(615, 40);
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(132, 23);
            this.txtColor.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(522, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 23);
            this.label8.TabIndex = 19;
            this.label8.Text = "Color";
            // 
            // comboMaterialType
            // 
            this.comboMaterialType.FormattingEnabled = true;
            this.comboMaterialType.Location = new System.Drawing.Point(113, 96);
            this.comboMaterialType.Name = "comboMaterialType";
            this.comboMaterialType.Size = new System.Drawing.Size(121, 24);
            this.comboMaterialType.TabIndex = 10;
            // 
            // grid_Report
            // 
            this.grid_Report.AllowUserToAddRows = false;
            this.grid_Report.AllowUserToDeleteRows = false;
            this.grid_Report.AllowUserToResizeRows = false;
            this.grid_Report.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid_Report.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid_Report.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid_Report.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_Report.DataSource = this.bs_Report;
            this.grid_Report.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid_Report.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid_Report.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid_Report.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid_Report.Location = new System.Drawing.Point(14, 424);
            this.grid_Report.Name = "grid_Report";
            this.grid_Report.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid_Report.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid_Report.RowTemplate.Height = 24;
            this.grid_Report.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_Report.ShowCellToolTips = false;
            this.grid_Report.Size = new System.Drawing.Size(926, 135);
            this.grid_Report.TabIndex = 1179;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(14, 126);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.grid_Material);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grid_Document);
            this.splitContainer1.Size = new System.Drawing.Size(926, 292);
            this.splitContainer1.SplitterDistance = 669;
            this.splitContainer1.TabIndex = 1180;
            // 
            // P52
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 603);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.grid_Report);
            this.Controls.Add(this.comboMaterialType);
            this.Controls.Add(this.txtColor);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtRefno);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dateATA);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSupplier);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBrand);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtSeason);
            this.Controls.Add(this.txtseq2);
            this.Controls.Add(this.txtSeq1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.dateETA);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSP);
            this.Controls.Add(this.labelSPNo);
            this.DefaultControl = "txtSP";
            this.DefaultControlForEdit = "txtSP";
            this.Name = "P52";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P52. Material Document Record";
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.txtSP, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.dateETA, 0);
            this.Controls.SetChildIndex(this.btnQuery, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtSeq1, 0);
            this.Controls.SetChildIndex(this.txtseq2, 0);
            this.Controls.SetChildIndex(this.txtSeason, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.label14, 0);
            this.Controls.SetChildIndex(this.txtBrand, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtSupplier, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.dateATA, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label12, 0);
            this.Controls.SetChildIndex(this.txtRefno, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.txtColor, 0);
            this.Controls.SetChildIndex(this.comboMaterialType, 0);
            this.Controls.SetChildIndex(this.grid_Report, 0);
            this.Controls.SetChildIndex(this.splitContainer1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.grid_Material)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs_Material)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Document)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs_Document)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Report)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs_Report)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.TextBox txtSP;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label label2;
        private Win.UI.DateRange dateETA;
        private Win.UI.Button btnQuery;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnClose;
        private Win.UI.Grid grid_Material;
        private Win.UI.ListControlBindingSource bs_Material;
        private Win.UI.Grid grid_Document;
        private Win.UI.TextBox txtSeq1;
        private Win.UI.Label label3;
        private Win.UI.TextBox txtseq2;
        private Win.UI.Label label7;
        private Class.Txtseason txtSeason;
        private Class.Txtbrand txtBrand;
        private Win.UI.Label label14;
        private Win.UI.TextBox txtSupplier;
        private Win.UI.Label label1;
        private Win.UI.DateRange dateATA;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.TextBox txtRefno;
        private Win.UI.Label label12;
        private Win.UI.TextBox txtColor;
        private Win.UI.Label label8;
        private System.Windows.Forms.ComboBox comboMaterialType;
        private Win.UI.Grid grid_Report;
        private Win.UI.ListControlBindingSource bs_Document;
        private Win.UI.ListControlBindingSource bs_Report;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}