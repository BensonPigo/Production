namespace Sci.Production.Sewing
{
    partial class B05_BatchImport
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            this.displayFactory = new Sci.Win.UI.DisplayBox();
            this.txtBrand = new Sci.Production.Class.Txtbrand();
            this.txtStyle = new Sci.Production.Class.Txtstyle();
            this.txtSeason = new Sci.Production.Class.Txtseason();
            this.gridSewingOutputEfficiency = new Sci.Win.UI.Grid();
            this.btnImport = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridSewingOutputEfficiency)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Factory";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(163, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Brand";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(310, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Style";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(521, 8);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "Season";
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(748, 3);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 4;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // displayFactory
            // 
            this.displayFactory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFactory.Location = new System.Drawing.Point(87, 9);
            this.displayFactory.Name = "displayFactory";
            this.displayFactory.Size = new System.Drawing.Size(73, 23);
            this.displayFactory.TabIndex = 5;
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(241, 9);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(66, 23);
            this.txtBrand.TabIndex = 6;
            // 
            // txtStyle
            // 
            this.txtStyle.BackColor = System.Drawing.Color.White;
            this.txtStyle.BrandObjectName = this.txtBrand;
            this.txtStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtStyle.Location = new System.Drawing.Point(388, 8);
            this.txtStyle.Name = "txtStyle";
            this.txtStyle.Size = new System.Drawing.Size(130, 23);
            this.txtStyle.TabIndex = 7;
            this.txtStyle.TarBrand = null;
            this.txtStyle.TarSeason = null;
            // 
            // txtSeason
            // 
            this.txtSeason.BackColor = System.Drawing.Color.White;
            this.txtSeason.BrandObjectName = this.txtBrand;
            this.txtSeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeason.Location = new System.Drawing.Point(599, 9);
            this.txtSeason.Name = "txtSeason";
            this.txtSeason.Size = new System.Drawing.Size(80, 23);
            this.txtSeason.TabIndex = 8;
            // 
            // gridSewingOutputEfficiency
            // 
            this.gridSewingOutputEfficiency.AllowUserToAddRows = false;
            this.gridSewingOutputEfficiency.AllowUserToDeleteRows = false;
            this.gridSewingOutputEfficiency.AllowUserToResizeRows = false;
            this.gridSewingOutputEfficiency.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridSewingOutputEfficiency.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSewingOutputEfficiency.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSewingOutputEfficiency.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSewingOutputEfficiency.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSewingOutputEfficiency.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSewingOutputEfficiency.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSewingOutputEfficiency.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSewingOutputEfficiency.Location = new System.Drawing.Point(9, 37);
            this.gridSewingOutputEfficiency.Name = "gridSewingOutputEfficiency";
            this.gridSewingOutputEfficiency.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSewingOutputEfficiency.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSewingOutputEfficiency.RowTemplate.Height = 24;
            this.gridSewingOutputEfficiency.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSewingOutputEfficiency.ShowCellToolTips = false;
            this.gridSewingOutputEfficiency.Size = new System.Drawing.Size(819, 367);
            this.gridSewingOutputEfficiency.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridSewingOutputEfficiency.TabIndex = 9;
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(652, 410);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(80, 30);
            this.btnImport.TabIndex = 10;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(748, 410);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // B05_BatchImport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(840, 450);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.gridSewingOutputEfficiency);
            this.Controls.Add(this.txtSeason);
            this.Controls.Add(this.txtStyle);
            this.Controls.Add(this.txtBrand);
            this.Controls.Add(this.displayFactory);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "B05_BatchImport";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "B05. BatchImport";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.btnQuery, 0);
            this.Controls.SetChildIndex(this.displayFactory, 0);
            this.Controls.SetChildIndex(this.txtBrand, 0);
            this.Controls.SetChildIndex(this.txtStyle, 0);
            this.Controls.SetChildIndex(this.txtSeason, 0);
            this.Controls.SetChildIndex(this.gridSewingOutputEfficiency, 0);
            this.Controls.SetChildIndex(this.btnImport, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridSewingOutputEfficiency)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Button btnQuery;
        private Win.UI.DisplayBox displayFactory;
        private Class.Txtbrand txtBrand;
        private Class.Txtstyle txtStyle;
        private Class.Txtseason txtSeason;
        private Win.UI.Grid gridSewingOutputEfficiency;
        private Win.UI.Button btnImport;
        private Win.UI.Button btnClose;
    }
}