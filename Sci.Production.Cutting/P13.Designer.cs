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
            this.checkBoxExtendAllParts = new Sci.Win.UI.CheckBox();
            this.buttonQuery = new Sci.Win.UI.Button();
            this.textBoxPOID = new Sci.Win.UI.TextBox();
            this.buttonClose = new Sci.Win.UI.Button();
            this.buttonToExcel = new Sci.Win.UI.Button();
            this.buttonPrint = new Sci.Win.UI.Button();
            this.grid = new Sci.Win.UI.Grid();
            this.bindingSource1 = new Sci.Win.UI.BindingSource(this.components);
            this.labelPatternPanel = new Sci.Win.UI.Label();
            this.textBoxPatterPanel = new Sci.Win.UI.TextBox();
            this.dateRangeCreateDate = new Sci.Win.UI.DateRange();
            this.labelCutNo = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.txtCutNo = new Sci.Win.UI.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBoxExtendAllParts
            // 
            this.checkBoxExtendAllParts.AutoSize = true;
            this.checkBoxExtendAllParts.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBoxExtendAllParts.Location = new System.Drawing.Point(851, 572);
            this.checkBoxExtendAllParts.Name = "checkBoxExtendAllParts";
            this.checkBoxExtendAllParts.Size = new System.Drawing.Size(126, 21);
            this.checkBoxExtendAllParts.TabIndex = 4;
            this.checkBoxExtendAllParts.Text = "Extend All Parts";
            this.checkBoxExtendAllParts.UseVisualStyleBackColor = true;
            // 
            // buttonQuery
            // 
            this.buttonQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonQuery.Location = new System.Drawing.Point(1155, 9);
            this.buttonQuery.Name = "buttonQuery";
            this.buttonQuery.Size = new System.Drawing.Size(80, 30);
            this.buttonQuery.TabIndex = 3;
            this.buttonQuery.Text = "Query";
            this.buttonQuery.UseVisualStyleBackColor = true;
            this.buttonQuery.Click += new System.EventHandler(this.ButtonQuery_Click);
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
            this.buttonClose.TabIndex = 7;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // buttonToExcel
            // 
            this.buttonToExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonToExcel.Location = new System.Drawing.Point(1069, 566);
            this.buttonToExcel.Name = "buttonToExcel";
            this.buttonToExcel.Size = new System.Drawing.Size(80, 30);
            this.buttonToExcel.TabIndex = 6;
            this.buttonToExcel.Text = "ToExcel";
            this.buttonToExcel.UseVisualStyleBackColor = true;
            this.buttonToExcel.Click += new System.EventHandler(this.ButtonToExcel_Click);
            // 
            // buttonPrint
            // 
            this.buttonPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPrint.Location = new System.Drawing.Point(983, 566);
            this.buttonPrint.Name = "buttonPrint";
            this.buttonPrint.Size = new System.Drawing.Size(80, 30);
            this.buttonPrint.TabIndex = 5;
            this.buttonPrint.Text = "Print";
            this.buttonPrint.UseVisualStyleBackColor = true;
            this.buttonPrint.Click += new System.EventHandler(this.ButtonPrint_Click);
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
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(1226, 515);
            this.grid.TabIndex = 8;
            // 
            // labelPatternPanel
            // 
            this.labelPatternPanel.Location = new System.Drawing.Point(721, 12);
            this.labelPatternPanel.Name = "labelPatternPanel";
            this.labelPatternPanel.Size = new System.Drawing.Size(82, 23);
            this.labelPatternPanel.TabIndex = 1;
            this.labelPatternPanel.Text = "PatterPanel";
            // 
            // textBoxPatterPanel
            // 
            this.textBoxPatterPanel.BackColor = System.Drawing.Color.White;
            this.textBoxPatterPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBoxPatterPanel.Location = new System.Drawing.Point(807, 12);
            this.textBoxPatterPanel.Name = "textBoxPatterPanel";
            this.textBoxPatterPanel.Size = new System.Drawing.Size(73, 23);
            this.textBoxPatterPanel.TabIndex = 3;
            // 
            // dateRangeCreateDate
            // 
            // 
            // 
            // 
            this.dateRangeCreateDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeCreateDate.DateBox1.Name = "";
            this.dateRangeCreateDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeCreateDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeCreateDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeCreateDate.DateBox2.Name = "";
            this.dateRangeCreateDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeCreateDate.DateBox2.TabIndex = 1;
            this.dateRangeCreateDate.IsRequired = false;
            this.dateRangeCreateDate.Location = new System.Drawing.Point(438, 12);
            this.dateRangeCreateDate.Name = "dateRangeCreateDate";
            this.dateRangeCreateDate.Size = new System.Drawing.Size(280, 23);
            this.dateRangeCreateDate.TabIndex = 2;
            // 
            // labelCutNo
            // 
            this.labelCutNo.Location = new System.Drawing.Point(187, 12);
            this.labelCutNo.Name = "labelCutNo";
            this.labelCutNo.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelCutNo.RectStyle.BorderWidth = 1F;
            this.labelCutNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelCutNo.RectStyle.ExtBorderWidth = 1F;
            this.labelCutNo.Size = new System.Drawing.Size(62, 23);
            this.labelCutNo.TabIndex = 97;
            this.labelCutNo.Text = "CutNo";
            this.labelCutNo.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelCutNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.RectStyle.BorderWidth = 1F;
            this.label1.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label1.RectStyle.ExtBorderWidth = 1F;
            this.label1.Size = new System.Drawing.Size(46, 23);
            this.label1.TabIndex = 98;
            this.label1.Text = "POID";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(355, 12);
            this.label2.Name = "label2";
            this.label2.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.RectStyle.BorderWidth = 1F;
            this.label2.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label2.RectStyle.ExtBorderWidth = 1F;
            this.label2.Size = new System.Drawing.Size(80, 23);
            this.label2.TabIndex = 100;
            this.label2.Text = "Create Date";
            this.label2.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtCutNo
            // 
            this.txtCutNo.BackColor = System.Drawing.Color.White;
            this.txtCutNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutNo.Location = new System.Drawing.Point(252, 12);
            this.txtCutNo.Name = "txtCutNo";
            this.txtCutNo.Size = new System.Drawing.Size(100, 23);
            this.txtCutNo.TabIndex = 1;
            this.txtCutNo.Validating += new System.ComponentModel.CancelEventHandler(this.TxtCutNo_Validating);
            // 
            // P13
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1244, 599);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtCutNo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelCutNo);
            this.Controls.Add(this.dateRangeCreateDate);
            this.Controls.Add(this.textBoxPatterPanel);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.buttonPrint);
            this.Controls.Add(this.buttonToExcel);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.textBoxPOID);
            this.Controls.Add(this.buttonQuery);
            this.Controls.Add(this.checkBoxExtendAllParts);
            this.Controls.Add(this.labelPatternPanel);
            this.EditMode = true;
            this.Name = "P13";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P13. Batch Print Bundle Check List";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.P13_FormClosed);
            this.Controls.SetChildIndex(this.labelPatternPanel, 0);
            this.Controls.SetChildIndex(this.checkBoxExtendAllParts, 0);
            this.Controls.SetChildIndex(this.buttonQuery, 0);
            this.Controls.SetChildIndex(this.textBoxPOID, 0);
            this.Controls.SetChildIndex(this.buttonClose, 0);
            this.Controls.SetChildIndex(this.buttonToExcel, 0);
            this.Controls.SetChildIndex(this.buttonPrint, 0);
            this.Controls.SetChildIndex(this.grid, 0);
            this.Controls.SetChildIndex(this.textBoxPatterPanel, 0);
            this.Controls.SetChildIndex(this.dateRangeCreateDate, 0);
            this.Controls.SetChildIndex(this.labelCutNo, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtCutNo, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.CheckBox checkBoxExtendAllParts;
        private Win.UI.Button buttonQuery;
        private Win.UI.TextBox textBoxPOID;
        private Win.UI.Button buttonClose;
        private Win.UI.Button buttonToExcel;
        private Win.UI.Button buttonPrint;
        private Win.UI.Grid grid;
        private Win.UI.BindingSource bindingSource1;
        private Win.UI.Label labelPatternPanel;
        private Win.UI.TextBox textBoxPatterPanel;
        private Win.UI.DateRange dateRangeCreateDate;
        private Win.UI.Label labelCutNo;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtCutNo;
    }
}