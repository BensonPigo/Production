namespace Sci.Production.Cutting
{
    partial class P13
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
            this.labelPOID = new Sci.Win.UI.Label();
            this.checkBoxExtendAllParts = new Sci.Win.UI.CheckBox();
            this.buttonQuery = new Sci.Win.UI.Button();
            this.textBoxPOID = new Sci.Win.UI.TextBox();
            this.buttonClose = new Sci.Win.UI.Button();
            this.buttonToExcel = new Sci.Win.UI.Button();
            this.buttonPrint = new Sci.Win.UI.Button();
            this.grid = new Sci.Win.UI.Grid();
            this.bindingSource1 = new Sci.Win.UI.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelPOID
            // 
            this.labelPOID.Location = new System.Drawing.Point(9, 12);
            this.labelPOID.Name = "labelPOID";
            this.labelPOID.Size = new System.Drawing.Size(45, 23);
            this.labelPOID.TabIndex = 1;
            this.labelPOID.Text = "POID";
            // 
            // checkBoxExtendAllParts
            // 
            this.checkBoxExtendAllParts.AutoSize = true;
            this.checkBoxExtendAllParts.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBoxExtendAllParts.Location = new System.Drawing.Point(201, 15);
            this.checkBoxExtendAllParts.Name = "checkBoxExtendAllParts";
            this.checkBoxExtendAllParts.Size = new System.Drawing.Size(126, 21);
            this.checkBoxExtendAllParts.TabIndex = 1;
            this.checkBoxExtendAllParts.Text = "Extend All Parts";
            this.checkBoxExtendAllParts.UseVisualStyleBackColor = true;
            // 
            // buttonQuery
            // 
            this.buttonQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonQuery.Location = new System.Drawing.Point(1155, 9);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Size = new System.Drawing.Size(80, 30);
            this.buttonQuery.TabIndex = 2;
            this.buttonQuery.Text = "Query";
            this.buttonQuery.UseVisualStyleBackColor = true;
            this.buttonQuery.Click += new System.EventHandler(this.buttonQuery_Click);
            // 
            // textBoxPOID
            // 
            this.textBoxPOID.BackColor = System.Drawing.Color.White;
            this.textBoxPOID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBoxPOID.Location = new System.Drawing.Point(58, 12);
            this.textBoxPOID.Name = "textBoxPOID";
            this.textBoxPOID.Size = new System.Drawing.Size(126, 23);
            this.textBoxPOID.TabIndex = 0;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(1155, 566);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(80, 30);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonToExcel
            // 
            this.buttonToExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonToExcel.Location = new System.Drawing.Point(1069, 566);
            this.buttonToExcel.Name = "buttonToExcel";
            this.buttonToExcel.Size = new System.Drawing.Size(80, 30);
            this.buttonToExcel.TabIndex = 4;
            this.buttonToExcel.Text = "ToExcel";
            this.buttonToExcel.UseVisualStyleBackColor = true;
            this.buttonToExcel.Click += new System.EventHandler(this.buttonToExcel_Click);
            // 
            // buttonPrint
            // 
            this.buttonPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPrint.Location = new System.Drawing.Point(983, 566);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(80, 30);
            this.buttonPrint.TabIndex = 3;
            this.buttonPrint.Text = "Print";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.buttonPrint_Click);
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
            this.grid.DataSource = this.bindingSource1;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(9, 45);
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
            this.grid.Size = new System.Drawing.Size(1226, 515);
            this.grid.TabIndex = 6;
            // 
            // P13
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1244, 599);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.buttonPrint);
            this.Controls.Add(this.buttonToExcel);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.textBoxPOID);
            this.Controls.Add(this.buttonQuery);
            this.Controls.Add(this.checkBoxExtendAllParts);
            this.Controls.Add(this.labelPOID);
            this.EditMode = true;
            this.Name = "P13";
            this.Text = "P13. Batch Print Bundle Check List";
            this.Controls.SetChildIndex(this.labelPOID, 0);
            this.Controls.SetChildIndex(this.checkBoxExtendAllParts, 0);
            this.Controls.SetChildIndex(this.buttonQuery, 0);
            this.Controls.SetChildIndex(this.textBoxPOID, 0);
            this.Controls.SetChildIndex(this.buttonClose, 0);
            this.Controls.SetChildIndex(this.buttonToExcel, 0);
            this.Controls.SetChildIndex(this.buttonPrint, 0);
            this.Controls.SetChildIndex(this.grid, 0);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelPOID;
        private Win.UI.CheckBox checkBoxExtendAllParts;
        private Win.UI.Button buttonQuery;
        private Win.UI.TextBox textBoxPOID;
        private Win.UI.Button buttonClose;
        private Win.UI.Button buttonToExcel;
        private Win.UI.Button buttonPrint;
        private Win.UI.Grid grid;
        private Win.UI.BindingSource bindingSource1;
    }
}