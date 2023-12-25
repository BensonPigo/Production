namespace Sci.Production.Quality
{
    partial class P01_BatchUpdateShadeBand
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
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnClose = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.comboBoxScale = new Sci.Win.UI.ComboBox();
            this.comboBoxResult = new Sci.Win.UI.ComboBox();
            this.label4 = new Sci.Win.UI.Label();
            this.btnInspected = new Sci.Win.UI.Button();
            this.txtScanQRCode = new Sci.Win.UI.TextBox();
            this.txtToneGrp = new Sci.Win.UI.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
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
            this.grid.DataSource = this.listControlBindingSource1;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(12, 38);
            this.grid.Name = "grid";
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(1316, 375);
            this.grid.TabIndex = 45;
            this.grid.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.Grid_ColumnHeaderMouseClick);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(1248, 419);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(1162, 419);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 23);
            this.label1.TabIndex = 24;
            this.label1.Text = "Scan QRCode";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.Location = new System.Drawing.Point(12, 419);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 23);
            this.label2.TabIndex = 24;
            this.label2.Text = "Tone/Grp";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.Location = new System.Drawing.Point(175, 419);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 23);
            this.label3.TabIndex = 24;
            this.label3.Text = "Scale";
            // 
            // comboBoxScale
            // 
            this.comboBoxScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxScale.BackColor = System.Drawing.Color.White;
            this.comboBoxScale.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxScale.FormattingEnabled = true;
            this.comboBoxScale.IsSupportUnselect = true;
            this.comboBoxScale.Location = new System.Drawing.Point(245, 419);
            this.comboBoxScale.Name = "comboBoxScale";
            this.comboBoxScale.OldText = "";
            this.comboBoxScale.Size = new System.Drawing.Size(76, 24);
            this.comboBoxScale.TabIndex = 2;
            // 
            // comboBoxResult
            // 
            this.comboBoxResult.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxResult.BackColor = System.Drawing.Color.White;
            this.comboBoxResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxResult.FormattingEnabled = true;
            this.comboBoxResult.IsSupportUnselect = true;
            this.comboBoxResult.Location = new System.Drawing.Point(394, 419);
            this.comboBoxResult.Name = "comboBoxResult";
            this.comboBoxResult.OldText = "";
            this.comboBoxResult.Size = new System.Drawing.Size(86, 24);
            this.comboBoxResult.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.Location = new System.Drawing.Point(324, 419);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 23);
            this.label4.TabIndex = 55;
            this.label4.Text = "Result";
            // 
            // btnInspected
            // 
            this.btnInspected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnInspected.Location = new System.Drawing.Point(486, 419);
            this.btnInspected.Name = "btnInspected";
            this.btnInspected.Size = new System.Drawing.Size(80, 30);
            this.btnInspected.TabIndex = 4;
            this.btnInspected.Text = "Inspected";
            this.btnInspected.UseVisualStyleBackColor = true;
            this.btnInspected.Click += new System.EventHandler(this.BtnInspected_Click);
            // 
            // txtScanQRCode
            // 
            this.txtScanQRCode.BackColor = System.Drawing.Color.White;
            this.txtScanQRCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtScanQRCode.Location = new System.Drawing.Point(109, 12);
            this.txtScanQRCode.Name = "txtScanQRCode";
            this.txtScanQRCode.Size = new System.Drawing.Size(161, 23);
            this.txtScanQRCode.TabIndex = 0;
            this.txtScanQRCode.Validating += new System.ComponentModel.CancelEventHandler(this.TxtScanQRCode_Validating);
            // 
            // txtToneGrp
            // 
            this.txtToneGrp.BackColor = System.Drawing.Color.White;
            this.txtToneGrp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtToneGrp.Location = new System.Drawing.Point(82, 419);
            this.txtToneGrp.Name = "txtToneGrp";
            this.txtToneGrp.Size = new System.Drawing.Size(90, 23);
            this.txtToneGrp.TabIndex = 1;
            this.txtToneGrp.Validating += new System.ComponentModel.CancelEventHandler(this.TxtScanQRCode_Validating);
            // 
            // P01_BatchUpdateShadeBand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1340, 461);
            this.Controls.Add(this.txtToneGrp);
            this.Controls.Add(this.txtScanQRCode);
            this.Controls.Add(this.btnInspected);
            this.Controls.Add(this.comboBoxResult);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxScale);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.label1);
            this.EditMode = true;
            this.Name = "P01_BatchUpdateShadeBand";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P01_Batch Update ShadeBand";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.grid, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.comboBoxScale, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.comboBoxResult, 0);
            this.Controls.SetChildIndex(this.btnInspected, 0);
            this.Controls.SetChildIndex(this.txtScanQRCode, 0);
            this.Controls.SetChildIndex(this.txtToneGrp, 0);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.Grid grid;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnSave;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.ComboBox comboBoxScale;
        private Win.UI.ComboBox comboBoxResult;
        private Win.UI.Label label4;
        private Win.UI.Button btnInspected;
        private Win.UI.TextBox txtScanQRCode;
        private Win.UI.TextBox txtToneGrp;
    }
}