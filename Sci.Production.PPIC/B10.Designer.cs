namespace Sci.Production.PPIC
{
    partial class B10
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new Sci.Win.UI.Panel();
            this.txtFactory = new Sci.Production.Class.Txtfactory();
            this.label5 = new Sci.Win.UI.Label();
            this.btnUploadFromMercury = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnQuery = new Sci.Win.UI.Button();
            this.dateSCIDev = new Sci.Win.UI.DateRange();
            this.dateBuyerDev = new Sci.Win.UI.DateRange();
            this.txtBrand = new Sci.Production.Class.Txtbrand();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.chkIncludeHistory = new Sci.Win.UI.CheckBox();
            this.chkIncludeCancel = new Sci.Win.UI.CheckBox();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.panel4 = new Sci.Win.UI.Panel();
            this.panel5 = new Sci.Win.UI.Panel();
            this.grid = new Sci.Win.UI.Grid();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtFactory);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.btnUploadFromMercury);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.dateSCIDev);
            this.panel1.Controls.Add(this.dateBuyerDev);
            this.panel1.Controls.Add(this.txtBrand);
            this.panel1.Controls.Add(this.txtSP);
            this.panel1.Controls.Add(this.chkIncludeHistory);
            this.panel1.Controls.Add(this.chkIncludeCancel);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 107);
            this.panel1.TabIndex = 1;
            // 
            // txtFactory
            // 
            this.txtFactory.BackColor = System.Drawing.Color.White;
            this.txtFactory.BoolFtyGroupList = false;
            this.txtFactory.FilteMDivision = false;
            this.txtFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactory.IsProduceFty = false;
            this.txtFactory.IssupportJunk = false;
            this.txtFactory.Location = new System.Drawing.Point(88, 73);
            this.txtFactory.MDivision = null;
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Size = new System.Drawing.Size(66, 23);
            this.txtFactory.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(10, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 10;
            this.label5.Text = "Factory";
            // 
            // btnUploadFromMercury
            // 
            this.btnUploadFromMercury.Location = new System.Drawing.Point(765, 37);
            this.btnUploadFromMercury.Name = "btnUploadFromMercury";
            this.btnUploadFromMercury.Size = new System.Drawing.Size(175, 30);
            this.btnUploadFromMercury.TabIndex = 9;
            this.btnUploadFromMercury.Text = "Upload From Mercury";
            this.btnUploadFromMercury.UseVisualStyleBackColor = true;
            this.btnUploadFromMercury.Click += new System.EventHandler(this.BtnUploadFromMercury_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(860, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(765, 5);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 7;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // dateSCIDev
            // 
            // 
            // 
            // 
            this.dateSCIDev.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSCIDev.DateBox1.Name = "";
            this.dateSCIDev.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDev.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSCIDev.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSCIDev.DateBox2.Name = "";
            this.dateSCIDev.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDev.DateBox2.TabIndex = 1;
            this.dateSCIDev.Location = new System.Drawing.Point(292, 41);
            this.dateSCIDev.Name = "dateSCIDev";
            this.dateSCIDev.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDev.TabIndex = 4;
            // 
            // dateBuyerDev
            // 
            // 
            // 
            // 
            this.dateBuyerDev.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerDev.DateBox1.Name = "";
            this.dateBuyerDev.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDev.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerDev.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBuyerDev.DateBox2.Name = "";
            this.dateBuyerDev.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDev.DateBox2.TabIndex = 1;
            this.dateBuyerDev.Location = new System.Drawing.Point(291, 9);
            this.dateBuyerDev.Name = "dateBuyerDev";
            this.dateBuyerDev.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDev.TabIndex = 3;
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(88, 40);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(66, 23);
            this.txtBrand.TabIndex = 2;
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(88, 9);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(100, 23);
            this.txtSP.TabIndex = 1;
            // 
            // chkIncludeHistory
            // 
            this.chkIncludeHistory.AutoSize = true;
            this.chkIncludeHistory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeHistory.Location = new System.Drawing.Point(578, 40);
            this.chkIncludeHistory.Name = "chkIncludeHistory";
            this.chkIncludeHistory.Size = new System.Drawing.Size(161, 21);
            this.chkIncludeHistory.TabIndex = 6;
            this.chkIncludeHistory.Text = "Include History Order";
            this.chkIncludeHistory.UseVisualStyleBackColor = true;
            // 
            // chkIncludeCancel
            // 
            this.chkIncludeCancel.AutoSize = true;
            this.chkIncludeCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeCancel.Location = new System.Drawing.Point(577, 11);
            this.chkIncludeCancel.Name = "chkIncludeCancel";
            this.chkIncludeCancel.Size = new System.Drawing.Size(160, 21);
            this.chkIncludeCancel.TabIndex = 5;
            this.chkIncludeCancel.Text = "Include Cancel Order";
            this.chkIncludeCancel.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(192, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(97, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "SCI Delivery";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(191, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Buyer Delivery";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Brand";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "SP#";
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 107);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 489);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(10, 583);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(998, 13);
            this.panel3.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(998, 107);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(10, 476);
            this.panel4.TabIndex = 3;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.grid);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 107);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(988, 476);
            this.panel5.TabIndex = 4;
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.Name = "grid";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(988, 476);
            this.grid.TabIndex = 0;
            // 
            // B10
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 596);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "B10";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B10. Upload Finishing Process Data (For SNP)";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel4, 0);
            this.Controls.SetChildIndex(this.panel5, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel5;
        private Win.UI.DateRange dateSCIDev;
        private Win.UI.DateRange dateBuyerDev;
        private Class.Txtbrand txtBrand;
        private Win.UI.TextBox txtSP;
        private Win.UI.CheckBox chkIncludeHistory;
        private Win.UI.CheckBox chkIncludeCancel;
        private Win.UI.Label label4;
        private Win.UI.Button btnUploadFromMercury;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnQuery;
        private Win.UI.Grid grid;
        private Class.Txtfactory txtFactory;
        private Win.UI.Label label5;
    }
}