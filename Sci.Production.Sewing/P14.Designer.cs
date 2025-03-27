namespace Sci.Production.Sewing
{
    partial class P14
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
            this.chkNyDCP = new System.Windows.Forms.CheckBox();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.txtfactory1 = new Sci.Production.Class.Txtfactory();
            this.txtMdivision1 = new Sci.Production.Class.TxtMdivision();
            this.lbSP = new Sci.Win.UI.Label();
            this.lbPackID = new Sci.Win.UI.Label();
            this.lbScanDate = new Sci.Win.UI.Label();
            this.buttonFindNow = new Sci.Win.UI.Button();
            this.dateScanDate = new Sci.Win.UI.DateRange();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.txtPackID = new Sci.Win.UI.TextBox();
            this.grid = new Sci.Win.UI.Grid();
            this.bindingSource = new Sci.Win.UI.BindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.chkNyDCP);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtfactory1);
            this.panel1.Controls.Add(this.txtMdivision1);
            this.panel1.Controls.Add(this.lbSP);
            this.panel1.Controls.Add(this.lbPackID);
            this.panel1.Controls.Add(this.lbScanDate);
            this.panel1.Controls.Add(this.buttonFindNow);
            this.panel1.Controls.Add(this.dateScanDate);
            this.panel1.Controls.Add(this.txtSP);
            this.panel1.Controls.Add(this.txtPackID);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(946, 101);
            this.panel1.TabIndex = 1;
            // 
            // chkNyDCP
            // 
            this.chkNyDCP.AutoSize = true;
            this.chkNyDCP.Location = new System.Drawing.Point(10, 72);
            this.chkNyDCP.Name = "chkNyDCP";
            this.chkNyDCP.Size = new System.Drawing.Size(187, 21);
            this.chkNyDCP.TabIndex = 11;
            this.chkNyDCP.Text = "Not Yet Done Scan & Pack";
            this.chkNyDCP.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label3.Location = new System.Drawing.Point(470, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 23);
            this.label3.TabIndex = 10;
            this.label3.Text = "Factory";
            this.label3.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.LightSkyBlue;
            this.label2.Location = new System.Drawing.Point(470, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "M";
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.BoolFtyGroupList = true;
            this.txtfactory1.FilteMDivision = false;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.IsIE = false;
            this.txtfactory1.IsMultiselect = false;
            this.txtfactory1.IsProduceFty = false;
            this.txtfactory1.IssupportJunk = false;
            this.txtfactory1.Location = new System.Drawing.Point(550, 41);
            this.txtfactory1.MDivision = null;
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.NeedInitialFactory = false;
            this.txtfactory1.Size = new System.Drawing.Size(121, 23);
            this.txtfactory1.TabIndex = 4;
            // 
            // txtMdivision1
            // 
            this.txtMdivision1.BackColor = System.Drawing.Color.White;
            this.txtMdivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision1.Location = new System.Drawing.Point(550, 6);
            this.txtMdivision1.Name = "txtMdivision1";
            this.txtMdivision1.NeedInitialMdivision = false;
            this.txtMdivision1.Size = new System.Drawing.Size(121, 23);
            this.txtMdivision1.TabIndex = 1;
            // 
            // lbSP
            // 
            this.lbSP.BackColor = System.Drawing.Color.LightSkyBlue;
            this.lbSP.Location = new System.Drawing.Point(244, 41);
            this.lbSP.Name = "lbSP";
            this.lbSP.Size = new System.Drawing.Size(75, 23);
            this.lbSP.TabIndex = 9;
            this.lbSP.Text = "SP#";
            this.lbSP.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // lbPackID
            // 
            this.lbPackID.BackColor = System.Drawing.Color.LightSkyBlue;
            this.lbPackID.Location = new System.Drawing.Point(3, 41);
            this.lbPackID.Name = "lbPackID";
            this.lbPackID.Size = new System.Drawing.Size(93, 23);
            this.lbPackID.TabIndex = 8;
            this.lbPackID.Text = "Pack ID";
            this.lbPackID.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // lbScanDate
            // 
            this.lbScanDate.BackColor = System.Drawing.Color.LightSkyBlue;
            this.lbScanDate.Location = new System.Drawing.Point(3, 6);
            this.lbScanDate.Name = "lbScanDate";
            this.lbScanDate.Size = new System.Drawing.Size(93, 23);
            this.lbScanDate.TabIndex = 6;
            this.lbScanDate.Text = "Scan Date";
            this.lbScanDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // buttonFindNow
            // 
            this.buttonFindNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFindNow.Location = new System.Drawing.Point(838, 56);
            this.buttonFindNow.Name = "buttonFindNow";
            this.buttonFindNow.Size = new System.Drawing.Size(99, 30);
            this.buttonFindNow.TabIndex = 5;
            this.buttonFindNow.Text = "Find Now";
            this.buttonFindNow.UseVisualStyleBackColor = true;
            this.buttonFindNow.Click += new System.EventHandler(this.ButtonFindNow_Click);
            // 
            // dateScanDate
            // 
            // 
            // 
            // 
            this.dateScanDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateScanDate.DateBox1.Name = "";
            this.dateScanDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateScanDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateScanDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateScanDate.DateBox2.Name = "";
            this.dateScanDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateScanDate.DateBox2.TabIndex = 1;
            this.dateScanDate.IsRequired = false;
            this.dateScanDate.Location = new System.Drawing.Point(99, 6);
            this.dateScanDate.Name = "dateScanDate";
            this.dateScanDate.Size = new System.Drawing.Size(280, 23);
            this.dateScanDate.TabIndex = 0;
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(322, 41);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(121, 23);
            this.txtSP.TabIndex = 3;
            // 
            // txtPackID
            // 
            this.txtPackID.BackColor = System.Drawing.Color.White;
            this.txtPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackID.Location = new System.Drawing.Point(99, 41);
            this.txtPackID.Name = "txtPackID";
            this.txtPackID.Size = new System.Drawing.Size(121, 23);
            this.txtPackID.TabIndex = 2;
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
            this.grid.DataSource = this.bindingSource;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(3, 110);
            this.grid.Name = "grid";
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(946, 441);
            this.grid.TabIndex = 2;
            // 
            // P14
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(952, 552);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.panel1);
            this.EditMode = true;
            this.Name = "P14";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P14. Query For Hauled Carton Record";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.grid, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.DateRange dateScanDate;
        private Win.UI.TextBox txtSP;
        private Win.UI.TextBox txtPackID;
        private Win.UI.Grid grid;
        private Win.UI.Button buttonFindNow;
        private Win.UI.BindingSource bindingSource;
        private Win.UI.Label lbSP;
        private Win.UI.Label lbPackID;
        private Win.UI.Label lbScanDate;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Class.Txtfactory txtfactory1;
        private Class.TxtMdivision txtMdivision1;
        private System.Windows.Forms.CheckBox chkNyDCP;
    }
}