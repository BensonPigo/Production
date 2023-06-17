namespace Sci.Production.Quality
{
    partial class P01_PrintFormatReport8
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
            this.grid1 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnPrint = new Sci.Win.UI.Button();
            this.comboToneGrp = new Sci.Win.UI.ComboBox();
            this.label6 = new Sci.Win.UI.Label();
            this.comboBoxDyelot = new Sci.Win.UI.ComboBox();
            this.labDyelot = new Sci.Win.UI.Label();
            this.btnFind = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(12, 38);
            this.grid1.Name = "grid1";
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(497, 352);
            this.grid1.TabIndex = 1;
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Location = new System.Drawing.Point(429, 396);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(80, 30);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // comboToneGrp
            // 
            this.comboToneGrp.BackColor = System.Drawing.Color.White;
            this.comboToneGrp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboToneGrp.FormattingEnabled = true;
            this.comboToneGrp.IsSupportUnselect = true;
            this.comboToneGrp.Location = new System.Drawing.Point(81, 8);
            this.comboToneGrp.Name = "comboToneGrp";
            this.comboToneGrp.OldText = "";
            this.comboToneGrp.Size = new System.Drawing.Size(121, 24);
            this.comboToneGrp.TabIndex = 238;
            this.comboToneGrp.SelectedValueChanged += new System.EventHandler(this.ComboToneGrp_SelectedValueChanged);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label6.Location = new System.Drawing.Point(12, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 23);
            this.label6.TabIndex = 237;
            this.label6.Text = "Tone/Grp";
            // 
            // comboBoxDyelot
            // 
            this.comboBoxDyelot.BackColor = System.Drawing.Color.White;
            this.comboBoxDyelot.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxDyelot.FormattingEnabled = true;
            this.comboBoxDyelot.IsSupportUnselect = true;
            this.comboBoxDyelot.Location = new System.Drawing.Point(283, 7);
            this.comboBoxDyelot.Name = "comboBoxDyelot";
            this.comboBoxDyelot.OldText = "";
            this.comboBoxDyelot.Size = new System.Drawing.Size(121, 24);
            this.comboBoxDyelot.TabIndex = 240;
            // 
            // labDyelot
            // 
            this.labDyelot.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labDyelot.Location = new System.Drawing.Point(214, 8);
            this.labDyelot.Name = "labDyelot";
            this.labDyelot.Size = new System.Drawing.Size(66, 23);
            this.labDyelot.TabIndex = 239;
            this.labDyelot.Text = "Dyelot";
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.Location = new System.Drawing.Point(429, 4);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 30);
            this.btnFind.TabIndex = 241;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // P01_PrintFormatReport8
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 435);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.comboBoxDyelot);
            this.Controls.Add(this.labDyelot);
            this.Controls.Add(this.comboToneGrp);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.grid1);
            this.Name = "P01_PrintFormatReport8";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "Print Format Report (8 Slot)";
            this.Controls.SetChildIndex(this.grid1, 0);
            this.Controls.SetChildIndex(this.btnPrint, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.comboToneGrp, 0);
            this.Controls.SetChildIndex(this.labDyelot, 0);
            this.Controls.SetChildIndex(this.comboBoxDyelot, 0);
            this.Controls.SetChildIndex(this.btnFind, 0);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid grid1;
        private Win.UI.Button btnPrint;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.ComboBox comboToneGrp;
        private Win.UI.Label label6;
        private Win.UI.ComboBox comboBoxDyelot;
        private Win.UI.Label labDyelot;
        private Win.UI.Button btnFind;
    }
}