namespace Sci.Production.Warehouse
{
    partial class P52_Import
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonFindNow = new Sci.Win.UI.Button();
            this.textBoxSPNumEnd = new Sci.Win.UI.TextBox();
            this.textBoxSPNumStart = new Sci.Win.UI.TextBox();
            this.textBoxCountOfRandom = new Sci.Win.UI.TextBox();
            this.textBoxLocation = new Sci.Win.UI.TextBox();
            this.textBoxUnitPriceEnd = new Sci.Win.UI.TextBox();
            this.textBoxUnitPriceStart = new Sci.Win.UI.TextBox();
            this.comboBoxCategory = new Sci.Win.UI.ComboBox();
            this.labelSPNum = new Sci.Win.UI.Label();
            this.labelCountOfRandom = new Sci.Win.UI.Label();
            this.labelLocation = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.labelUnitPrice = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.grid = new Sci.Win.UI.Grid();
            this.listControlBindingSource = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.buttonImport = new Sci.Win.UI.Button();
            this.buttonCancel = new Sci.Win.UI.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.buttonFindNow);
            this.panel1.Controls.Add(this.textBoxSPNumEnd);
            this.panel1.Controls.Add(this.textBoxSPNumStart);
            this.panel1.Controls.Add(this.textBoxCountOfRandom);
            this.panel1.Controls.Add(this.textBoxLocation);
            this.panel1.Controls.Add(this.textBoxUnitPriceEnd);
            this.panel1.Controls.Add(this.textBoxUnitPriceStart);
            this.panel1.Controls.Add(this.comboBoxCategory);
            this.panel1.Controls.Add(this.labelSPNum);
            this.panel1.Controls.Add(this.labelCountOfRandom);
            this.panel1.Controls.Add(this.labelLocation);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.labelUnitPrice);
            this.panel1.Controls.Add(this.labelCategory);
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(933, 77);
            this.panel1.TabIndex = 0;
            // 
            // buttonFindNow
            // 
            this.buttonFindNow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFindNow.Location = new System.Drawing.Point(850, 41);
            this.buttonFindNow.Name = "buttonFindNow";
            this.buttonFindNow.Size = new System.Drawing.Size(80, 30);
            this.buttonFindNow.TabIndex = 7;
            this.buttonFindNow.Text = "Find Now";
            this.buttonFindNow.UseVisualStyleBackColor = true;
            this.buttonFindNow.Click += new System.EventHandler(this.ButtonFindNow_Click);
            // 
            // textBoxSPNumEnd
            // 
            this.textBoxSPNumEnd.BackColor = System.Drawing.Color.White;
            this.textBoxSPNumEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBoxSPNumEnd.Location = new System.Drawing.Point(473, 41);
            this.textBoxSPNumEnd.Name = "textBoxSPNumEnd";
            this.textBoxSPNumEnd.Size = new System.Drawing.Size(100, 23);
            this.textBoxSPNumEnd.TabIndex = 4;
            // 
            // textBoxSPNumStart
            // 
            this.textBoxSPNumStart.BackColor = System.Drawing.Color.White;
            this.textBoxSPNumStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBoxSPNumStart.Location = new System.Drawing.Point(345, 41);
            this.textBoxSPNumStart.Name = "textBoxSPNumStart";
            this.textBoxSPNumStart.Size = new System.Drawing.Size(100, 23);
            this.textBoxSPNumStart.TabIndex = 3;
            // 
            // textBoxCountOfRandom
            // 
            this.textBoxCountOfRandom.BackColor = System.Drawing.Color.White;
            this.textBoxCountOfRandom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBoxCountOfRandom.Location = new System.Drawing.Point(705, 41);
            this.textBoxCountOfRandom.Name = "textBoxCountOfRandom";
            this.textBoxCountOfRandom.Size = new System.Drawing.Size(100, 23);
            this.textBoxCountOfRandom.TabIndex = 6;
            this.textBoxCountOfRandom.Text = "5";
            // 
            // textBoxLocation
            // 
            this.textBoxLocation.BackColor = System.Drawing.Color.White;
            this.textBoxLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBoxLocation.Location = new System.Drawing.Point(705, 8);
            this.textBoxLocation.Name = "textBoxLocation";
            this.textBoxLocation.Size = new System.Drawing.Size(100, 23);
            this.textBoxLocation.TabIndex = 5;
            this.textBoxLocation.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TextBoxLocation_PopUp);
            // 
            // textBoxUnitPriceEnd
            // 
            this.textBoxUnitPriceEnd.BackColor = System.Drawing.Color.White;
            this.textBoxUnitPriceEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBoxUnitPriceEnd.Location = new System.Drawing.Point(473, 8);
            this.textBoxUnitPriceEnd.Name = "textBoxUnitPriceEnd";
            this.textBoxUnitPriceEnd.Size = new System.Drawing.Size(100, 23);
            this.textBoxUnitPriceEnd.TabIndex = 2;
            // 
            // textBoxUnitPriceStart
            // 
            this.textBoxUnitPriceStart.BackColor = System.Drawing.Color.White;
            this.textBoxUnitPriceStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBoxUnitPriceStart.Location = new System.Drawing.Point(345, 8);
            this.textBoxUnitPriceStart.Name = "textBoxUnitPriceStart";
            this.textBoxUnitPriceStart.Size = new System.Drawing.Size(100, 23);
            this.textBoxUnitPriceStart.TabIndex = 1;
            // 
            // comboBoxCategory
            // 
            this.comboBoxCategory.BackColor = System.Drawing.Color.White;
            this.comboBoxCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxCategory.FormattingEnabled = true;
            this.comboBoxCategory.IsSupportUnselect = true;
            this.comboBoxCategory.Location = new System.Drawing.Point(86, 7);
            this.comboBoxCategory.Name = "comboBoxCategory";
            this.comboBoxCategory.Size = new System.Drawing.Size(121, 24);
            this.comboBoxCategory.TabIndex = 0;
            // 
            // labelSPNum
            // 
            this.labelSPNum.BackColor = System.Drawing.Color.SkyBlue;
            this.labelSPNum.Location = new System.Drawing.Point(231, 41);
            this.labelSPNum.Name = "labelSPNum";
            this.labelSPNum.Size = new System.Drawing.Size(111, 23);
            this.labelSPNum.TabIndex = 0;
            this.labelSPNum.Text = "SP#";
            this.labelSPNum.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelCountOfRandom
            // 
            this.labelCountOfRandom.Location = new System.Drawing.Point(585, 41);
            this.labelCountOfRandom.Name = "labelCountOfRandom";
            this.labelCountOfRandom.Size = new System.Drawing.Size(117, 23);
            this.labelCountOfRandom.TabIndex = 0;
            this.labelCountOfRandom.Text = "Count of Random";
            // 
            // labelLocation
            // 
            this.labelLocation.Location = new System.Drawing.Point(585, 8);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(117, 23);
            this.labelLocation.TabIndex = 0;
            this.labelLocation.Text = "Location";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(448, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 23);
            this.label4.TabIndex = 0;
            this.label4.Text = "~";
            this.label4.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.label4.TextStyle.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(448, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "~";
            this.label1.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.TextStyle.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelUnitPrice
            // 
            this.labelUnitPrice.Location = new System.Drawing.Point(231, 8);
            this.labelUnitPrice.Name = "labelUnitPrice";
            this.labelUnitPrice.Size = new System.Drawing.Size(111, 23);
            this.labelUnitPrice.TabIndex = 0;
            this.labelUnitPrice.Text = "Unit Price (US$)";
            // 
            // labelCategory
            // 
            this.labelCategory.BackColor = System.Drawing.Color.SkyBlue;
            this.labelCategory.Location = new System.Drawing.Point(7, 7);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(75, 23);
            this.labelCategory.TabIndex = 0;
            this.labelCategory.Text = "Category";
            this.labelCategory.TextStyle.Color = System.Drawing.Color.Black;
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
            this.grid.DataSource = this.listControlBindingSource;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(2, 85);
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
            this.grid.Size = new System.Drawing.Size(930, 465);
            this.grid.TabIndex = 1;
            // 
            // buttonImport
            // 
            this.buttonImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonImport.Location = new System.Drawing.Point(758, 556);
            this.buttonImport.Name = "buttonImport";
            this.buttonImport.Size = new System.Drawing.Size(80, 30);
            this.buttonImport.TabIndex = 2;
            this.buttonImport.Text = "Import";
            this.buttonImport.UseVisualStyleBackColor = true;
            this.buttonImport.Click += new System.EventHandler(this.ButtonImport_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.Location = new System.Drawing.Point(844, 556);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(80, 30);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // P52_Import
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 592);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonImport);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.panel1);
            this.EditMode = true;
            this.Name = "P52_Import";
            this.Text = "P52_Import";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.grid, 0);
            this.Controls.SetChildIndex(this.buttonImport, 0);
            this.Controls.SetChildIndex(this.buttonCancel, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Win.UI.Button buttonFindNow;
        private Win.UI.TextBox textBoxSPNumEnd;
        private Win.UI.TextBox textBoxSPNumStart;
        private Win.UI.TextBox textBoxCountOfRandom;
        private Win.UI.TextBox textBoxUnitPriceEnd;
        private Win.UI.TextBox textBoxUnitPriceStart;
        private Win.UI.ComboBox comboBoxCategory;
        private Win.UI.Label labelSPNum;
        private Win.UI.Label labelCountOfRandom;
        private Win.UI.Label labelLocation;
        private Win.UI.Label label4;
        private Win.UI.Label label1;
        private Win.UI.Label labelUnitPrice;
        private Win.UI.Label labelCategory;
        private Win.UI.Grid grid;
        private Win.UI.Button buttonImport;
        private Win.UI.Button buttonCancel;
        private Win.UI.TextBox textBoxLocation;
        private Win.UI.ListControlBindingSource listControlBindingSource;
    }
}