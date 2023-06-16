namespace Sci.Production.IE
{
    partial class B10_Import
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
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.panel4 = new Sci.Win.UI.Panel();
            this.button5 = new Sci.Win.UI.Button();
            this.btnWriteIn = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.panel7 = new Sci.Win.UI.Panel();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.panel6 = new Sci.Win.UI.Panel();
            this.panel10 = new Sci.Win.UI.Panel();
            this.panel12 = new Sci.Win.UI.Panel();
            this.gridAttachFile = new Sci.Win.UI.Grid();
            this.panel11 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Panel();
            this.btnCheckImport = new Sci.Win.UI.Button();
            this.btnRemoveExcel = new Sci.Win.UI.Button();
            this.btnAddExcel = new Sci.Win.UI.Button();
            this.panel8 = new Sci.Win.UI.Panel();
            this.labelAttachFile = new Sci.Win.UI.Label();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridAttachFile)).BeginInit();
            this.btnClose.SuspendLayout();
            this.panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(10, 450);
            this.panel1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(790, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(10, 450);
            this.panel2.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(10, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(780, 10);
            this.panel3.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.button5);
            this.panel4.Controls.Add(this.btnWriteIn);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(10, 408);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(780, 42);
            this.panel4.TabIndex = 4;
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button5.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button5.Location = new System.Drawing.Point(695, 6);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(80, 30);
            this.button5.TabIndex = 1;
            this.button5.Text = "Close";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.Button5_Click);
            // 
            // btnWriteIn
            // 
            this.btnWriteIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnWriteIn.Location = new System.Drawing.Point(605, 6);
            this.btnWriteIn.Name = "btnWriteIn";
            this.btnWriteIn.Size = new System.Drawing.Size(80, 30);
            this.btnWriteIn.TabIndex = 0;
            this.btnWriteIn.Text = "Write In";
            this.btnWriteIn.UseVisualStyleBackColor = true;
            this.btnWriteIn.Click += new System.EventHandler(this.BtnWriteIn_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(10, 10);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(780, 398);
            this.panel5.TabIndex = 5;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.gridDetail);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 144);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(780, 254);
            this.panel7.TabIndex = 1;
            // 
            // gridDetail
            // 
            this.gridDetail.AllowUserToAddRows = false;
            this.gridDetail.AllowUserToDeleteRows = false;
            this.gridDetail.AllowUserToResizeRows = false;
            this.gridDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDetail.Location = new System.Drawing.Point(0, 0);
            this.gridDetail.Name = "gridDetail";
            this.gridDetail.RowHeadersVisible = false;
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.ShowCellToolTips = false;
            this.gridDetail.Size = new System.Drawing.Size(780, 254);
            this.gridDetail.TabIndex = 0;
            this.gridDetail.TabStop = false;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.panel10);
            this.panel6.Controls.Add(this.btnClose);
            this.panel6.Controls.Add(this.panel8);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(780, 144);
            this.panel6.TabIndex = 0;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.panel12);
            this.panel10.Controls.Add(this.panel11);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel10.Location = new System.Drawing.Point(68, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(540, 144);
            this.panel10.TabIndex = 2;
            // 
            // panel12
            // 
            this.panel12.Controls.Add(this.gridAttachFile);
            this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel12.Location = new System.Drawing.Point(0, 0);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(540, 134);
            this.panel12.TabIndex = 1;
            // 
            // gridAttachFile
            // 
            this.gridAttachFile.AllowUserToAddRows = false;
            this.gridAttachFile.AllowUserToDeleteRows = false;
            this.gridAttachFile.AllowUserToResizeRows = false;
            this.gridAttachFile.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridAttachFile.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridAttachFile.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridAttachFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridAttachFile.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridAttachFile.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridAttachFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridAttachFile.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridAttachFile.Location = new System.Drawing.Point(0, 0);
            this.gridAttachFile.Name = "gridAttachFile";
            this.gridAttachFile.RowHeadersVisible = false;
            this.gridAttachFile.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridAttachFile.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridAttachFile.RowTemplate.Height = 24;
            this.gridAttachFile.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridAttachFile.ShowCellToolTips = false;
            this.gridAttachFile.Size = new System.Drawing.Size(540, 134);
            this.gridAttachFile.TabIndex = 0;
            this.gridAttachFile.TabStop = false;
            // 
            // panel11
            // 
            this.panel11.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel11.Location = new System.Drawing.Point(0, 134);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(540, 10);
            this.panel11.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Controls.Add(this.btnCheckImport);
            this.btnClose.Controls.Add(this.btnRemoveExcel);
            this.btnClose.Controls.Add(this.btnAddExcel);
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.Location = new System.Drawing.Point(608, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(172, 144);
            this.btnClose.TabIndex = 1;
            // 
            // btnCheckImport
            // 
            this.btnCheckImport.Location = new System.Drawing.Point(23, 106);
            this.btnCheckImport.Name = "btnCheckImport";
            this.btnCheckImport.Size = new System.Drawing.Size(146, 30);
            this.btnCheckImport.TabIndex = 2;
            this.btnCheckImport.Text = "Check && Import";
            this.btnCheckImport.UseVisualStyleBackColor = true;
            this.btnCheckImport.Click += new System.EventHandler(this.BtnCheckImport_Click);
            // 
            // btnRemoveExcel
            // 
            this.btnRemoveExcel.Location = new System.Drawing.Point(23, 44);
            this.btnRemoveExcel.Name = "btnRemoveExcel";
            this.btnRemoveExcel.Size = new System.Drawing.Size(146, 30);
            this.btnRemoveExcel.TabIndex = 1;
            this.btnRemoveExcel.Text = "Remove Excel";
            this.btnRemoveExcel.UseVisualStyleBackColor = true;
            this.btnRemoveExcel.Click += new System.EventHandler(this.BtnRemoveExcel_Click);
            // 
            // btnAddExcel
            // 
            this.btnAddExcel.Location = new System.Drawing.Point(23, 7);
            this.btnAddExcel.Name = "btnAddExcel";
            this.btnAddExcel.Size = new System.Drawing.Size(146, 30);
            this.btnAddExcel.TabIndex = 0;
            this.btnAddExcel.Text = "Add Excel";
            this.btnAddExcel.UseVisualStyleBackColor = true;
            this.btnAddExcel.Click += new System.EventHandler(this.BtnAddExcel_Click);
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.labelAttachFile);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(68, 144);
            this.panel8.TabIndex = 0;
            // 
            // labelAttachFile
            // 
            this.labelAttachFile.Location = new System.Drawing.Point(0, 0);
            this.labelAttachFile.Name = "labelAttachFile";
            this.labelAttachFile.Size = new System.Drawing.Size(64, 23);
            this.labelAttachFile.TabIndex = 0;
            this.labelAttachFile.Text = "Attach file";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // B10_Import
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "B10_Import";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "B10_Import";
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridAttachFile)).EndInit();
            this.btnClose.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Win.UI.Button button5;
        private Win.UI.Button btnWriteIn;
        private Win.UI.Panel panel5;
        private Win.UI.Panel panel7;
        private Win.UI.Grid gridDetail;
        private Win.UI.Panel panel6;
        private Win.UI.Panel panel10;
        private Win.UI.Panel panel12;
        private Win.UI.Grid gridAttachFile;
        private Win.UI.Panel panel11;
        private Win.UI.Panel btnClose;
        private Win.UI.Button btnCheckImport;
        private Win.UI.Button btnRemoveExcel;
        private Win.UI.Button btnAddExcel;
        private Win.UI.Panel panel8;
        private Win.UI.Label labelAttachFile;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}