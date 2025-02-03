namespace Sci.Production.IE
{
    partial class P05_EditOperation
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
            this.gridEditOperation = new Sci.Win.UI.Grid();
            this.gridIconEditOperation = new Sci.Win.UI.GridIcon();
            this.panel1 = new Sci.Win.UI.Panel();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnCancel = new Sci.Win.UI.Button();
            this.gridEditOperationBs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.gridEditOperation)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEditOperationBs)).BeginInit();
            this.SuspendLayout();
            // 
            // gridEditOperation
            // 
            this.gridEditOperation.AllowUserToAddRows = false;
            this.gridEditOperation.AllowUserToDeleteRows = false;
            this.gridEditOperation.AllowUserToResizeRows = false;
            this.gridEditOperation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridEditOperation.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridEditOperation.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridEditOperation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridEditOperation.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridEditOperation.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridEditOperation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridEditOperation.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridEditOperation.Location = new System.Drawing.Point(3, 39);
            this.gridEditOperation.Name = "gridEditOperation";
            this.gridEditOperation.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridEditOperation.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridEditOperation.RowTemplate.Height = 24;
            this.gridEditOperation.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridEditOperation.ShowCellToolTips = false;
            this.gridEditOperation.Size = new System.Drawing.Size(976, 364);
            this.gridEditOperation.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridEditOperation.TabIndex = 1;
            // 
            // gridIconEditOperation
            // 
            this.gridIconEditOperation.Location = new System.Drawing.Point(4, 3);
            this.gridIconEditOperation.Name = "gridIconEditOperation";
            this.gridIconEditOperation.Size = new System.Drawing.Size(100, 32);
            this.gridIconEditOperation.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.gridIconEditOperation);
            this.panel1.Location = new System.Drawing.Point(934, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(39, 37);
            this.panel1.TabIndex = 3;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(813, 409);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(899, 409);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker_ProgressChanged);
            // 
            // P05_EditOperation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(985, 450);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.gridEditOperation);
            this.Name = "P05_EditOperation";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P05. Edit No. Operation";
            this.Controls.SetChildIndex(this.gridEditOperation, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridEditOperation)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEditOperationBs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid gridEditOperation;
        private Win.UI.GridIcon gridIconEditOperation;
        private Win.UI.Panel panel1;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnCancel;
        private Win.UI.ListControlBindingSource gridEditOperationBs;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
    }
}