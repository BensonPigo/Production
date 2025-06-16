namespace Sci.Production.Warehouse
{
    partial class P79
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
            this.btnQuery = new Sci.Win.UI.Button();
            this.txtBrandID = new Sci.Production.Class.Txtbrand();
            this.txtFactoryID = new Sci.Production.Class.Txtfactory();
            this.txtMdivisionID = new Sci.Production.Class.TxtMdivision();
            this.label6 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.txtEndPOID = new Sci.Win.UI.TextBox();
            this.txtBeginPOID = new Sci.Win.UI.TextBox();
            this.dateRange1 = new Sci.Win.UI.DateRange();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.grid1 = new Sci.Win.UI.Grid();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.txtBrandID);
            this.panel1.Controls.Add(this.txtFactoryID);
            this.panel1.Controls.Add(this.txtMdivisionID);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtEndPOID);
            this.panel1.Controls.Add(this.txtBeginPOID);
            this.panel1.Controls.Add(this.dateRange1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1138, 80);
            this.panel1.TabIndex = 1;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(1046, 5);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 12;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // txtBrandID
            // 
            this.txtBrandID.BackColor = System.Drawing.Color.White;
            this.txtBrandID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrandID.Location = new System.Drawing.Point(732, 9);
            this.txtBrandID.MyDocumentdName = null;
            this.txtBrandID.Name = "txtBrandID";
            this.txtBrandID.Size = new System.Drawing.Size(97, 23);
            this.txtBrandID.TabIndex = 11;
            // 
            // txtFactoryID
            // 
            this.txtFactoryID.BackColor = System.Drawing.Color.White;
            this.txtFactoryID.BoolFtyGroupList = true;
            this.txtFactoryID.FilteMDivision = false;
            this.txtFactoryID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactoryID.IsIE = false;
            this.txtFactoryID.IsMultiselect = false;
            this.txtFactoryID.IsProduceFty = false;
            this.txtFactoryID.IssupportJunk = false;
            this.txtFactoryID.Location = new System.Drawing.Point(525, 44);
            this.txtFactoryID.MDivision = null;
            this.txtFactoryID.Name = "txtFactoryID";
            this.txtFactoryID.NeedInitialFactory = false;
            this.txtFactoryID.Size = new System.Drawing.Size(79, 23);
            this.txtFactoryID.TabIndex = 9;
            // 
            // txtMdivisionID
            // 
            this.txtMdivisionID.BackColor = System.Drawing.Color.White;
            this.txtMdivisionID.DefaultValue = false;
            this.txtMdivisionID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivisionID.Location = new System.Drawing.Point(525, 9);
            this.txtMdivisionID.Name = "txtMdivisionID";
            this.txtMdivisionID.NeedInitialMdivision = false;
            this.txtMdivisionID.Size = new System.Drawing.Size(79, 23);
            this.txtMdivisionID.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(622, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(106, 23);
            this.label6.TabIndex = 10;
            this.label6.Text = "Brand";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(416, 44);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 23);
            this.label5.TabIndex = 8;
            this.label5.Text = "Factory";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(416, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 23);
            this.label4.TabIndex = 6;
            this.label4.Text = "M";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(247, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "～";
            this.label3.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtEndPOID
            // 
            this.txtEndPOID.BackColor = System.Drawing.Color.White;
            this.txtEndPOID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtEndPOID.Location = new System.Drawing.Point(271, 44);
            this.txtEndPOID.Name = "txtEndPOID";
            this.txtEndPOID.Size = new System.Drawing.Size(126, 23);
            this.txtEndPOID.TabIndex = 5;
            // 
            // txtBeginPOID
            // 
            this.txtBeginPOID.BackColor = System.Drawing.Color.White;
            this.txtBeginPOID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBeginPOID.Location = new System.Drawing.Point(118, 44);
            this.txtBeginPOID.Name = "txtBeginPOID";
            this.txtBeginPOID.Size = new System.Drawing.Size(126, 23);
            this.txtBeginPOID.TabIndex = 3;
            // 
            // dateRange1
            // 
            // 
            // 
            // 
            this.dateRange1.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRange1.DateBox1.Name = "";
            this.dateRange1.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRange1.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRange1.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRange1.DateBox2.Name = "";
            this.dateRange1.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRange1.DateBox2.TabIndex = 1;
            this.dateRange1.Location = new System.Drawing.Point(118, 9);
            this.dateRange1.Name = "dateRange1";
            this.dateRange1.Size = new System.Drawing.Size(280, 23);
            this.dateRange1.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.SkyBlue;
            this.label2.Location = new System.Drawing.Point(9, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "SP#";
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.SkyBlue;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Date";
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(2, 82);
            this.grid1.Name = "grid1";
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(1133, 467);
            this.grid1.TabIndex = 2;
            // 
            // P79
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1138, 552);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.panel1);
            this.EditMode = true;
            this.Name = "P79";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P79. Bulk Fabric Transaction List";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.grid1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.Button btnQuery;
        private Class.Txtbrand txtBrandID;
        private Class.Txtfactory txtFactoryID;
        private Class.TxtMdivision txtMdivisionID;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.TextBox txtEndPOID;
        private Win.UI.TextBox txtBeginPOID;
        private Win.UI.DateRange dateRange1;
        private Win.UI.Grid grid1;
    }
}