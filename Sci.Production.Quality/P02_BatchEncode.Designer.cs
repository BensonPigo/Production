namespace Sci.Production.Quality
{
    partial class P02_BatchEncode
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
            this.grid = new Sci.Win.UI.Grid();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.dateInspectDt = new Sci.Win.UI.DateBox();
            this.comboResult = new Sci.Win.UI.ComboBox();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.btnEncode = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.txtInspector = new Sci.Production.Class.Txtuser();
            this.numInspectRate = new Sci.Win.UI.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.radioInspected = new Sci.Win.UI.RadioButton();
            this.radioAQL = new Sci.Win.UI.RadioButton();
            this.radioMaterialType = new Sci.Win.UI.RadioButton();
            this.radioPanelInspected = new Sci.Win.UI.RadioPanel();
            this.comboDropDownList1 = new Sci.Production.Class.ComboDropDownList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInspectRate)).BeginInit();
            this.radioPanelInspected.SuspendLayout();
            this.SuspendLayout();
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
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(12, 12);
            this.grid.Name = "grid";
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(622, 247);
            this.grid.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Location = new System.Drawing.Point(12, 271);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Inspected %";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.Location = new System.Drawing.Point(12, 381);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Inspector";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.Location = new System.Drawing.Point(12, 416);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "Result";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.Location = new System.Drawing.Point(12, 352);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 23);
            this.label4.TabIndex = 5;
            this.label4.Text = "Inspect Date";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.Location = new System.Drawing.Point(223, 416);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 23);
            this.label5.TabIndex = 6;
            this.label5.Text = "Remark";
            // 
            // dateInspectDt
            // 
            this.dateInspectDt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dateInspectDt.Location = new System.Drawing.Point(100, 352);
            this.dateInspectDt.Name = "dateInspectDt";
            this.dateInspectDt.Size = new System.Drawing.Size(130, 23);
            this.dateInspectDt.TabIndex = 9;
            // 
            // comboResult
            // 
            this.comboResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboResult.BackColor = System.Drawing.Color.White;
            this.comboResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboResult.FormattingEnabled = true;
            this.comboResult.IsSupportUnselect = true;
            this.comboResult.Location = new System.Drawing.Point(99, 415);
            this.comboResult.Name = "comboResult";
            this.comboResult.OldText = "";
            this.comboResult.Size = new System.Drawing.Size(121, 24);
            this.comboResult.TabIndex = 10;
            // 
            // txtRemark
            // 
            this.txtRemark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(311, 416);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(282, 23);
            this.txtRemark.TabIndex = 11;
            // 
            // btnEncode
            // 
            this.btnEncode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnEncode.Location = new System.Drawing.Point(382, 445);
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.Size = new System.Drawing.Size(80, 30);
            this.btnEncode.TabIndex = 13;
            this.btnEncode.Text = "Encode";
            this.btnEncode.UseVisualStyleBackColor = true;
            this.btnEncode.Click += new System.EventHandler(this.BtnEncode_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(554, 445);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 14;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(468, 445);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 15;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // txtInspector
            // 
            this.txtInspector.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtInspector.DisplayBox1Binding = "";
            this.txtInspector.Location = new System.Drawing.Point(100, 381);
            this.txtInspector.Name = "txtInspector";
            this.txtInspector.Size = new System.Drawing.Size(306, 23);
            this.txtInspector.TabIndex = 12;
            this.txtInspector.TextBox1Binding = "";
            // 
            // numInspectRate
            // 
            this.numInspectRate.BackColor = System.Drawing.Color.White;
            this.numInspectRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numInspectRate.Location = new System.Drawing.Point(91, 0);
            this.numInspectRate.Name = "numInspectRate";
            this.numInspectRate.Size = new System.Drawing.Size(93, 23);
            this.numInspectRate.TabIndex = 16;
            this.numInspectRate.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(186, 2);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 17);
            this.label6.TabIndex = 17;
            this.label6.Text = "%";
            // 
            // radioInspected
            // 
            this.radioInspected.AutoSize = true;
            this.radioInspected.Checked = true;
            this.radioInspected.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioInspected.Location = new System.Drawing.Point(3, 0);
            this.radioInspected.Name = "radioInspected";
            this.radioInspected.Size = new System.Drawing.Size(87, 21);
            this.radioInspected.TabIndex = 18;
            this.radioInspected.TabStop = true;
            this.radioInspected.Text = "Inspected";
            this.radioInspected.UseVisualStyleBackColor = true;
            this.radioInspected.Value = "1";
            // 
            // radioAQL
            // 
            this.radioAQL.AutoSize = true;
            this.radioAQL.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioAQL.Location = new System.Drawing.Point(3, 27);
            this.radioAQL.Name = "radioAQL";
            this.radioAQL.Size = new System.Drawing.Size(54, 21);
            this.radioAQL.TabIndex = 19;
            this.radioAQL.Text = "AQL";
            this.radioAQL.UseVisualStyleBackColor = true;
            this.radioAQL.Value = "2";
            // 
            // radioMaterialType
            // 
            this.radioMaterialType.AutoSize = true;
            this.radioMaterialType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioMaterialType.Location = new System.Drawing.Point(3, 54);
            this.radioMaterialType.Name = "radioMaterialType";
            this.radioMaterialType.Size = new System.Drawing.Size(216, 21);
            this.radioMaterialType.TabIndex = 20;
            this.radioMaterialType.Text = "Base on Material Type Setting";
            this.radioMaterialType.UseVisualStyleBackColor = true;
            this.radioMaterialType.Value = "3";
            // 
            // radioPanelInspected
            // 
            this.radioPanelInspected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.radioPanelInspected.Controls.Add(this.comboDropDownList1);
            this.radioPanelInspected.Controls.Add(this.radioInspected);
            this.radioPanelInspected.Controls.Add(this.label6);
            this.radioPanelInspected.Controls.Add(this.numInspectRate);
            this.radioPanelInspected.Controls.Add(this.radioAQL);
            this.radioPanelInspected.Controls.Add(this.radioMaterialType);
            this.radioPanelInspected.Location = new System.Drawing.Point(100, 271);
            this.radioPanelInspected.Name = "radioPanelInspected";
            this.radioPanelInspected.Size = new System.Drawing.Size(232, 75);
            this.radioPanelInspected.TabIndex = 22;
            this.radioPanelInspected.Value = "1";
            // 
            // comboDropDownList1
            // 
            this.comboDropDownList1.AddAllItem = false;
            this.comboDropDownList1.BackColor = System.Drawing.Color.White;
            this.comboDropDownList1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDropDownList1.FormattingEnabled = true;
            this.comboDropDownList1.IsSupportUnselect = true;
            this.comboDropDownList1.Location = new System.Drawing.Point(63, 26);
            this.comboDropDownList1.Name = "comboDropDownList1";
            this.comboDropDownList1.OldText = "";
            this.comboDropDownList1.Size = new System.Drawing.Size(121, 24);
            this.comboDropDownList1.TabIndex = 23;
            this.comboDropDownList1.Type = "PMS_QA_AQL";
            // 
            // P02_BatchEncode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 482);
            this.Controls.Add(this.radioPanelInspected);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnEncode);
            this.Controls.Add(this.txtInspector);
            this.Controls.Add(this.txtRemark);
            this.Controls.Add(this.comboResult);
            this.Controls.Add(this.dateInspectDt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.grid);
            this.Name = "P02_BatchEncode";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "Batch Encode";
            this.Controls.SetChildIndex(this.grid, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.dateInspectDt, 0);
            this.Controls.SetChildIndex(this.comboResult, 0);
            this.Controls.SetChildIndex(this.txtRemark, 0);
            this.Controls.SetChildIndex(this.txtInspector, 0);
            this.Controls.SetChildIndex(this.btnEncode, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.radioPanelInspected, 0);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInspectRate)).EndInit();
            this.radioPanelInspected.ResumeLayout(false);
            this.radioPanelInspected.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Grid grid;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.DateBox dateInspectDt;
        private Win.UI.ComboBox comboResult;
        private Win.UI.TextBox txtRemark;
        private Class.Txtuser txtInspector;
        private Win.UI.Button btnEncode;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnSave;
        private Win.UI.NumericUpDown numInspectRate;
        private System.Windows.Forms.Label label6;
        private Win.UI.RadioButton radioInspected;
        private Win.UI.RadioButton radioAQL;
        private Win.UI.RadioButton radioMaterialType;
        private Win.UI.RadioPanel radioPanelInspected;
        private Class.ComboDropDownList comboDropDownList1;
    }
}