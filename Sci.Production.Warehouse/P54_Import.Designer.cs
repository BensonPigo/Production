namespace Sci.Production.Warehouse
{
    partial class P54_Import
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
            this.grid1 = new Sci.Win.UI.Grid();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.displayTotal = new Sci.Win.UI.DisplayBox();
            this.labelTotal = new Sci.Win.UI.Label();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.txtRefno = new Sci.Win.UI.TextBox();
            this.labFabricType = new Sci.Win.UI.Label();
            this.txtSeq1 = new Sci.Production.Class.TxtSeq();
            this.btnFindNow = new Sci.Win.UI.Button();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.label1 = new Sci.Win.UI.Label();
            this.comboBoxStockType = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.grid1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(838, 539);
            this.panel1.TabIndex = 23;
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(0, 58);
            this.grid1.Name = "grid1";
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(838, 428);
            this.grid1.TabIndex = 5;
            this.grid1.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.displayTotal);
            this.groupBox2.Controls.Add(this.labelTotal);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 486);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(838, 53);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            // 
            // displayTotal
            // 
            this.displayTotal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayTotal.Location = new System.Drawing.Point(537, 22);
            this.displayTotal.Name = "displayTotal";
            this.displayTotal.Size = new System.Drawing.Size(100, 23);
            this.displayTotal.TabIndex = 4;
            // 
            // labelTotal
            // 
            this.labelTotal.Location = new System.Drawing.Point(429, 22);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(105, 23);
            this.labelTotal.TabIndex = 3;
            this.labelTotal.Text = "Total Stock Qty";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(742, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(646, 16);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 30);
            this.btnImport.TabIndex = 6;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxStockType);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtRefno);
            this.groupBox1.Controls.Add(this.labFabricType);
            this.groupBox1.Controls.Add(this.txtSeq1);
            this.groupBox1.Controls.Add(this.btnFindNow);
            this.groupBox1.Controls.Add(this.txtSP);
            this.groupBox1.Controls.Add(this.labelSPNo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(838, 58);
            this.groupBox1.TabIndex = 22;
            this.groupBox1.TabStop = false;
            // 
            // txtRefno
            // 
            this.txtRefno.BackColor = System.Drawing.Color.White;
            this.txtRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefno.Location = new System.Drawing.Point(563, 19);
            this.txtRefno.Name = "txtRefno";
            this.txtRefno.Size = new System.Drawing.Size(122, 23);
            this.txtRefno.TabIndex = 3;
            // 
            // labFabricType
            // 
            this.labFabricType.Location = new System.Drawing.Point(511, 19);
            this.labFabricType.Name = "labFabricType";
            this.labFabricType.Size = new System.Drawing.Size(49, 23);
            this.labFabricType.TabIndex = 5;
            this.labFabricType.Text = "Refno";
            // 
            // txtSeq1
            // 
            this.txtSeq1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSeq1.Location = new System.Drawing.Point(235, 19);
            this.txtSeq1.Name = "txtSeq1";
            this.txtSeq1.Seq1 = "";
            this.txtSeq1.Seq2 = "";
            this.txtSeq1.Size = new System.Drawing.Size(72, 23);
            this.txtSeq1.TabIndex = 1;
            // 
            // btnFindNow
            // 
            this.btnFindNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFindNow.Location = new System.Drawing.Point(699, 15);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(101, 30);
            this.btnFindNow.TabIndex = 4;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.BtnFindNow_Click);
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(107, 19);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(122, 23);
            this.txtSP.TabIndex = 0;
            // 
            // labelSPNo
            // 
            this.labelSPNo.BackColor = System.Drawing.Color.SkyBlue;
            this.labelSPNo.Location = new System.Drawing.Point(9, 19);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(95, 23);
            this.labelSPNo.TabIndex = 4;
            this.labelSPNo.Text = "SP#";
            this.labelSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(310, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 23);
            this.label1.TabIndex = 7;
            this.label1.Text = "Stock Type";
            // 
            // comboBoxStockType
            // 
            this.comboBoxStockType.FormattingEnabled = true;
            this.comboBoxStockType.Items.AddRange(new object[] {
            "",
            "Bulk",
            "Inventory"});
            this.comboBoxStockType.Location = new System.Drawing.Point(387, 18);
            this.comboBoxStockType.Name = "comboBoxStockType";
            this.comboBoxStockType.Size = new System.Drawing.Size(121, 24);
            this.comboBoxStockType.TabIndex = 2;
            // 
            // P54_Import
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 539);
            this.Controls.Add(this.panel1);
            this.Name = "P54_Import";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P54 Import Detail";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Win.UI.Panel panel1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.TextBox txtRefno;
        private Win.UI.Label labFabricType;
        private Class.TxtSeq txtSeq1;
        private Win.UI.Button btnFindNow;
        private Win.UI.TextBox txtSP;
        private Win.UI.Label labelSPNo;
        private Win.UI.GroupBox groupBox2;
        private Win.UI.DisplayBox displayTotal;
        private Win.UI.Label labelTotal;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnImport;
        private Win.UI.Grid grid1;
        private System.Windows.Forms.ComboBox comboBoxStockType;
        private Win.UI.Label label1;
    }
}