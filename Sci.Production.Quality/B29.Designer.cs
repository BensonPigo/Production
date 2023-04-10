namespace Sci.Production.Quality
{
    partial class B29
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
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.cbGroup = new Sci.Win.UI.ComboBox();
            this.cbWeaveType = new Sci.Win.UI.ComboBox();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.labelStockType = new Sci.Win.UI.Label();
            this.btnFindNow = new Sci.Win.UI.Button();
            this.gridImport = new Sci.Win.UI.Grid();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbGroup);
            this.groupBox1.Controls.Add(this.cbWeaveType);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtbrand);
            this.groupBox1.Controls.Add(this.labelStockType);
            this.groupBox1.Controls.Add(this.btnFindNow);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(800, 55);
            this.groupBox1.TabIndex = 18;
            this.groupBox1.TabStop = false;
            // 
            // cbGroup
            // 
            this.cbGroup.BackColor = System.Drawing.Color.White;
            this.cbGroup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbGroup.FormattingEnabled = true;
            this.cbGroup.IsSupportUnselect = true;
            this.cbGroup.Location = new System.Drawing.Point(495, 19);
            this.cbGroup.Name = "cbGroup";
            this.cbGroup.OldText = "";
            this.cbGroup.Size = new System.Drawing.Size(121, 24);
            this.cbGroup.TabIndex = 129;
            // 
            // cbWeaveType
            // 
            this.cbWeaveType.BackColor = System.Drawing.Color.White;
            this.cbWeaveType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbWeaveType.FormattingEnabled = true;
            this.cbWeaveType.IsSupportUnselect = true;
            this.cbWeaveType.Location = new System.Drawing.Point(269, 19);
            this.cbWeaveType.Name = "cbWeaveType";
            this.cbWeaveType.OldText = "";
            this.cbWeaveType.Size = new System.Drawing.Size(121, 24);
            this.cbWeaveType.TabIndex = 129;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(407, 19);
            this.label2.Name = "label2";
            this.label2.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.RectStyle.BorderWidth = 1F;
            this.label2.RectStyle.ExtBorderWidth = 1F;
            this.label2.Size = new System.Drawing.Size(85, 23);
            this.label2.TabIndex = 127;
            this.label2.Text = "Group";
            this.label2.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.TextStyle.Color = System.Drawing.Color.White;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(181, 19);
            this.label1.Name = "label1";
            this.label1.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.RectStyle.BorderWidth = 1F;
            this.label1.RectStyle.ExtBorderWidth = 1F;
            this.label1.Size = new System.Drawing.Size(85, 23);
            this.label1.TabIndex = 125;
            this.label1.Text = "Weave Type";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.TextStyle.Color = System.Drawing.Color.White;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(98, 19);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(66, 23);
            this.txtbrand.TabIndex = 124;
            // 
            // labelStockType
            // 
            this.labelStockType.Location = new System.Drawing.Point(9, 19);
            this.labelStockType.Name = "labelStockType";
            this.labelStockType.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelStockType.RectStyle.BorderWidth = 1F;
            this.labelStockType.RectStyle.ExtBorderWidth = 1F;
            this.labelStockType.Size = new System.Drawing.Size(85, 23);
            this.labelStockType.TabIndex = 123;
            this.labelStockType.Text = "Barand";
            this.labelStockType.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelStockType.TextStyle.Color = System.Drawing.Color.White;
            // 
            // btnFindNow
            // 
            this.btnFindNow.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFindNow.Location = new System.Drawing.Point(622, 14);
            this.btnFindNow.Name = "btnFindNow";
            this.btnFindNow.Size = new System.Drawing.Size(90, 30);
            this.btnFindNow.TabIndex = 5;
            this.btnFindNow.Text = "Find Now";
            this.btnFindNow.UseVisualStyleBackColor = true;
            this.btnFindNow.Click += new System.EventHandler(this.BtnFindNow_Click);
            // 
            // gridImport
            // 
            this.gridImport.AllowUserToAddRows = false;
            this.gridImport.AllowUserToDeleteRows = false;
            this.gridImport.AllowUserToResizeRows = false;
            this.gridImport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.gridImport.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridImport.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridImport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridImport.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridImport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridImport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridImport.Location = new System.Drawing.Point(0, 55);
            this.gridImport.Name = "gridImport";
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridImport.RowTemplate.Height = 24;
            this.gridImport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridImport.ShowCellToolTips = false;
            this.gridImport.Size = new System.Drawing.Size(800, 395);
            this.gridImport.TabIndex = 19;
            this.gridImport.TabStop = false;
            // 
            // B29
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.gridImport);
            this.Controls.Add(this.groupBox1);
            this.Name = "B29";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "B29";
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.gridImport, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.GroupBox groupBox1;
        private Win.UI.Label labelStockType;
        private Win.UI.Button btnFindNow;
        private Class.Txtbrand txtbrand;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Grid gridImport;
        private Win.UI.ComboBox cbWeaveType;
        private Win.UI.ComboBox cbGroup;
    }
}