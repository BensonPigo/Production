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
            this.buttonFindNow = new Sci.Win.UI.Button();
            this.dateRangeScanDate = new Sci.Win.UI.DateRange();
            this.textBoxSP = new Sci.Win.UI.TextBox();
            this.textBoxPackID = new Sci.Win.UI.TextBox();
            this.labelScanDate = new Sci.Win.UI.Label();
            this.labelSP = new Sci.Win.UI.Label();
            this.labelPackID = new Sci.Win.UI.Label();
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
            this.panel1.Controls.Add(this.buttonFindNow);
            this.panel1.Controls.Add(this.dateRangeScanDate);
            this.panel1.Controls.Add(this.textBoxSP);
            this.panel1.Controls.Add(this.textBoxPackID);
            this.panel1.Controls.Add(this.labelScanDate);
            this.panel1.Controls.Add(this.labelSP);
            this.panel1.Controls.Add(this.labelPackID);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(946, 64);
            this.panel1.TabIndex = 1;
            // 
            // buttonFindNow
            // 
            this.buttonFindNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFindNow.Location = new System.Drawing.Point(838, 19);
            this.buttonFindNow.Name = "buttonFindNow";
            this.buttonFindNow.Size = new System.Drawing.Size(99, 30);
            this.buttonFindNow.TabIndex = 3;
            this.buttonFindNow.Text = "Find Now";
            this.buttonFindNow.UseVisualStyleBackColor = true;
            this.buttonFindNow.Click += new System.EventHandler(this.ButtonFindNow_Click);
            // 
            // dateRangeScanDate
            // 
            // 
            // 
            // 
            this.dateRangeScanDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeScanDate.DateBox1.Name = "";
            this.dateRangeScanDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeScanDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeScanDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeScanDate.DateBox2.Name = "";
            this.dateRangeScanDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeScanDate.DateBox2.TabIndex = 1;
            this.dateRangeScanDate.IsRequired = false;
            this.dateRangeScanDate.Location = new System.Drawing.Point(99, 6);
            this.dateRangeScanDate.Name = "dateRangeScanDate";
            this.dateRangeScanDate.Size = new System.Drawing.Size(280, 23);
            this.dateRangeScanDate.TabIndex = 2;
            // 
            // textBoxSP
            // 
            this.textBoxSP.BackColor = System.Drawing.Color.White;
            this.textBoxSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBoxSP.Location = new System.Drawing.Point(322, 41);
            this.textBoxSP.Name = "textBoxSP";
            this.textBoxSP.Size = new System.Drawing.Size(121, 23);
            this.textBoxSP.TabIndex = 1;
            // 
            // textBoxPackID
            // 
            this.textBoxPackID.BackColor = System.Drawing.Color.White;
            this.textBoxPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBoxPackID.Location = new System.Drawing.Point(99, 41);
            this.textBoxPackID.Name = "textBoxPackID";
            this.textBoxPackID.Size = new System.Drawing.Size(121, 23);
            this.textBoxPackID.TabIndex = 1;
            // 
            // labelScanDate
            // 
            this.labelScanDate.Location = new System.Drawing.Point(0, 6);
            this.labelScanDate.Name = "labelScanDate";
            this.labelScanDate.Size = new System.Drawing.Size(96, 23);
            this.labelScanDate.TabIndex = 0;
            this.labelScanDate.Text = "Scan Date";
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(223, 41);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(96, 23);
            this.labelSP.TabIndex = 0;
            this.labelSP.Text = "SP#";
            // 
            // labelPackID
            // 
            this.labelPackID.Location = new System.Drawing.Point(0, 41);
            this.labelPackID.Name = "labelPackID";
            this.labelPackID.Size = new System.Drawing.Size(96, 23);
            this.labelPackID.TabIndex = 0;
            this.labelPackID.Text = "Pack ID";
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
            this.grid.Location = new System.Drawing.Point(3, 70);
            this.grid.Name = "grid";
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(946, 481);
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
        private Win.UI.DateRange dateRangeScanDate;
        private Win.UI.TextBox textBoxSP;
        private Win.UI.TextBox textBoxPackID;
        private Win.UI.Label labelScanDate;
        private Win.UI.Label labelSP;
        private Win.UI.Label labelPackID;
        private Win.UI.Grid grid;
        private Win.UI.Button buttonFindNow;
        private Win.UI.BindingSource bindingSource;
    }
}