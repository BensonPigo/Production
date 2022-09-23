namespace Sci.Production.Shipping
{
    partial class ShippingMemo
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
            this.gridShippingMemo = new Sci.Win.UI.Grid();
            this.btnUndo = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnAppend = new Sci.Win.UI.Button();
            this.btnDelete = new Sci.Win.UI.Button();
            this.editDesc = new Sci.Win.UI.EditBox();
            this.label1 = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridShippingMemo)).BeginInit();
            this.SuspendLayout();
            // 
            // gridShippingMemo
            // 
            this.gridShippingMemo.AllowUserToAddRows = false;
            this.gridShippingMemo.AllowUserToDeleteRows = false;
            this.gridShippingMemo.AllowUserToResizeRows = false;
            this.gridShippingMemo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridShippingMemo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridShippingMemo.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridShippingMemo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridShippingMemo.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridShippingMemo.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridShippingMemo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridShippingMemo.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridShippingMemo.Location = new System.Drawing.Point(12, 12);
            this.gridShippingMemo.Name = "gridShippingMemo";
            this.gridShippingMemo.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridShippingMemo.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridShippingMemo.RowTemplate.Height = 24;
            this.gridShippingMemo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridShippingMemo.ShowCellToolTips = false;
            this.gridShippingMemo.Size = new System.Drawing.Size(750, 281);
            this.gridShippingMemo.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridShippingMemo.TabIndex = 0;
            this.gridShippingMemo.SelectionChanged += new System.EventHandler(this.GridShippingMemo_SelectionChanged);
            // 
            // btnUndo
            // 
            this.btnUndo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUndo.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnUndo.Location = new System.Drawing.Point(682, 429);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(80, 30);
            this.btnUndo.TabIndex = 1;
            this.btnUndo.Text = "Undo";
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.BtnUndo_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(596, 429);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnAppend
            // 
            this.btnAppend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAppend.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnAppend.Location = new System.Drawing.Point(12, 429);
            this.btnAppend.Name = "btnAppend";
            this.btnAppend.Size = new System.Drawing.Size(80, 30);
            this.btnAppend.TabIndex = 3;
            this.btnAppend.Text = "Append";
            this.btnAppend.UseVisualStyleBackColor = true;
            this.btnAppend.Click += new System.EventHandler(this.BtnAppend_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDelete.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnDelete.Location = new System.Drawing.Point(98, 429);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 30);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // editDesc
            // 
            this.editDesc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editDesc.BackColor = System.Drawing.Color.White;
            this.editDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDesc.Location = new System.Drawing.Point(12, 322);
            this.editDesc.Multiline = true;
            this.editDesc.Name = "editDesc";
            this.editDesc.Size = new System.Drawing.Size(750, 101);
            this.editDesc.TabIndex = 5;
            this.editDesc.Validating += new System.ComponentModel.CancelEventHandler(this.EditDesc_Validating);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.Location = new System.Drawing.Point(12, 296);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Description";
            // 
            // ShippingMemo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 471);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.editDesc);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnAppend);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnUndo);
            this.Controls.Add(this.gridShippingMemo);
            this.Name = "ShippingMemo";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "ShippingMemo";
            ((System.ComponentModel.ISupportInitialize)(this.gridShippingMemo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Grid gridShippingMemo;
        private Win.UI.Button btnUndo;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnAppend;
        private Win.UI.Button btnDelete;
        private Win.UI.EditBox editDesc;
        private Win.UI.Label label1;
    }
}