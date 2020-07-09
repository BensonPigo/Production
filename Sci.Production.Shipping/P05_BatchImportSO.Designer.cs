namespace Sci.Production.Shipping
{
    partial class P05_BatchImportSO
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.dateInvDate = new Sci.Win.UI.DateRange();
            this.comboShipper = new Sci.Win.UI.ComboBox();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.labBuyerDelivery = new Sci.Win.UI.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.grid = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.BtnQuery = new Sci.Win.UI.Button();
            this.BtnSave = new Sci.Win.UI.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BtnQuery);
            this.panel1.Controls.Add(this.dateInvDate);
            this.panel1.Controls.Add(this.comboShipper);
            this.panel1.Controls.Add(this.txtbrand);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.labBuyerDelivery);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1118, 64);
            this.panel1.TabIndex = 115;
            // 
            // dateInvDate
            // 
            // 
            // 
            // 
            this.dateInvDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateInvDate.DateBox1.Name = "";
            this.dateInvDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateInvDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateInvDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateInvDate.DateBox2.Name = "";
            this.dateInvDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateInvDate.DateBox2.TabIndex = 1;
            this.dateInvDate.Location = new System.Drawing.Point(498, 20);
            this.dateInvDate.Name = "dateInvDate";
            this.dateInvDate.Size = new System.Drawing.Size(280, 23);
            this.dateInvDate.TabIndex = 120;
            // 
            // comboShipper
            // 
            this.comboShipper.BackColor = System.Drawing.Color.White;
            this.comboShipper.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboShipper.FormattingEnabled = true;
            this.comboShipper.IsSupportUnselect = true;
            this.comboShipper.Location = new System.Drawing.Point(109, 19);
            this.comboShipper.Name = "comboShipper";
            this.comboShipper.OldText = "";
            this.comboShipper.Size = new System.Drawing.Size(96, 24);
            this.comboShipper.TabIndex = 119;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(324, 20);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(66, 23);
            this.txtbrand.TabIndex = 118;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(412, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 23);
            this.label2.TabIndex = 117;
            this.label2.Text = "Invoice Date";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(238, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 23);
            this.label1.TabIndex = 116;
            this.label1.Text = "Brand";
            // 
            // labBuyerDelivery
            // 
            this.labBuyerDelivery.Location = new System.Drawing.Point(23, 20);
            this.labBuyerDelivery.Name = "labBuyerDelivery";
            this.labBuyerDelivery.Size = new System.Drawing.Size(83, 23);
            this.labBuyerDelivery.TabIndex = 115;
            this.labBuyerDelivery.Text = "Shipper";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.grid);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 64);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1118, 398);
            this.panel2.TabIndex = 116;
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.Name = "grid";
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(1118, 398);
            this.grid.TabIndex = 1;
            this.grid.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.BtnSave);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 412);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1118, 50);
            this.panel3.TabIndex = 117;
            // 
            // BtnQuery
            // 
            this.BtnQuery.Location = new System.Drawing.Point(1026, 16);
            this.BtnQuery.Name = "BtnQuery";
            this.BtnQuery.Size = new System.Drawing.Size(80, 30);
            this.BtnQuery.TabIndex = 0;
            this.BtnQuery.Text = "Query";
            this.BtnQuery.UseVisualStyleBackColor = true;
            this.BtnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Location = new System.Drawing.Point(1026, 8);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(80, 30);
            this.BtnSave.TabIndex = 121;
            this.BtnSave.Text = "Save";
            this.BtnSave.UseVisualStyleBackColor = true;
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // P05_BatchImportSO
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1118, 462);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "P05_BatchImportSO";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P05. Batch Import S/O";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Win.UI.DateRange dateInvDate;
        private Win.UI.ComboBox comboShipper;
        private Class.Txtbrand txtbrand;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.Label labBuyerDelivery;
        private System.Windows.Forms.Panel panel2;
        private Win.UI.Grid grid;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Button BtnQuery;
        private System.Windows.Forms.Panel panel3;
        private Win.UI.Button BtnSave;
    }
}