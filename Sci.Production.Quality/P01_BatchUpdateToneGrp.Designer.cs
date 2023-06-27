namespace Sci.Production.Quality
{
    partial class P01_BatchUpdateToneGrp
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
            this.txtSP = new Sci.Win.UI.DisplayBox();
            this.label2 = new Sci.Win.UI.Label();
            this.grids = new Sci.Win.UI.Grid();
            this.btnClose = new Sci.Win.UI.Button();
            this.label12 = new Sci.Win.UI.Label();
            this.cbRefColor = new Sci.Win.UI.ComboBox();
            this.btnFind = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grids)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSP.Location = new System.Drawing.Point(100, 9);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(121, 23);
            this.txtSP.TabIndex = 25;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 23);
            this.label2.TabIndex = 24;
            this.label2.Text = "SP#";
            // 
            // grids
            // 
            this.grids.AllowUserToAddRows = false;
            this.grids.AllowUserToDeleteRows = false;
            this.grids.AllowUserToResizeRows = false;
            this.grids.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grids.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grids.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grids.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grids.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grids.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grids.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grids.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grids.Location = new System.Drawing.Point(12, 38);
            this.grids.Name = "grids";
            this.grids.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grids.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grids.RowTemplate.Height = 24;
            this.grids.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grids.ShowCellToolTips = false;
            this.grids.Size = new System.Drawing.Size(776, 364);
            this.grids.TabIndex = 45;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(708, 408);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 46;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(224, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(89, 23);
            this.label12.TabIndex = 47;
            this.label12.Text = "Ref#/Color#";
            // 
            // cbRefColor
            // 
            this.cbRefColor.BackColor = System.Drawing.Color.White;
            this.cbRefColor.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.cbRefColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbRefColor.FormattingEnabled = true;
            this.cbRefColor.IsSupportUnselect = true;
            this.cbRefColor.Location = new System.Drawing.Point(316, 8);
            this.cbRefColor.Name = "cbRefColor";
            this.cbRefColor.OldText = "";
            this.cbRefColor.Size = new System.Drawing.Size(299, 24);
            this.cbRefColor.TabIndex = 48;
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.Location = new System.Drawing.Point(708, 3);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 30);
            this.btnFind.TabIndex = 51;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(622, 408);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 52;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // P01_BatchUpdateToneGrp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.cbRefColor);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.grids);
            this.Controls.Add(this.txtSP);
            this.Controls.Add(this.label2);
            this.EditMode = true;
            this.Name = "P01_BatchUpdateToneGrp";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P01_BatchUpdateToneGrp";
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtSP, 0);
            this.Controls.SetChildIndex(this.grids, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.label12, 0);
            this.Controls.SetChildIndex(this.cbRefColor, 0);
            this.Controls.SetChildIndex(this.btnFind, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            ((System.ComponentModel.ISupportInitialize)(this.grids)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DisplayBox txtSP;
        private Win.UI.Label label2;
        private Win.UI.Grid grids;
        private Win.UI.Button btnClose;
        private Win.UI.Label label12;
        private Win.UI.ComboBox cbRefColor;
        private Win.UI.Button btnFind;
        private Win.UI.Button btnSave;
    }
}