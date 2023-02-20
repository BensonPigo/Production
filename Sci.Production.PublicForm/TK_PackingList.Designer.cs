namespace Sci.Production.PublicForm
{
    partial class TK_PackingList
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
            this.gridNotAssignCarton = new Sci.Win.UI.Grid();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.gridAssignedCarton = new Sci.Win.UI.Grid();
            this.label3 = new Sci.Win.UI.Label();
            this.gridCartonList = new Sci.Win.UI.Grid();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnEditSave = new Sci.Win.UI.Button();
            this.btnCancelAssingCarton = new Sci.Win.UI.Button();
            this.btnAssignCarton = new Sci.Win.UI.Button();
            this.bindingSourceNotAssignCarton = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.bindingSourceAssignedCarton = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.bindingSourceCartonList = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridNotAssignCarton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAssignedCarton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCartonList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceNotAssignCarton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceAssignedCarton)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCartonList)).BeginInit();
            this.SuspendLayout();
            // 
            // gridNotAssignCarton
            // 
            this.gridNotAssignCarton.AllowUserToAddRows = false;
            this.gridNotAssignCarton.AllowUserToDeleteRows = false;
            this.gridNotAssignCarton.AllowUserToResizeRows = false;
            this.gridNotAssignCarton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridNotAssignCarton.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridNotAssignCarton.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridNotAssignCarton.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridNotAssignCarton.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridNotAssignCarton.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridNotAssignCarton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridNotAssignCarton.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridNotAssignCarton.Location = new System.Drawing.Point(12, 39);
            this.gridNotAssignCarton.Name = "gridNotAssignCarton";
            this.gridNotAssignCarton.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridNotAssignCarton.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridNotAssignCarton.RowTemplate.Height = 24;
            this.gridNotAssignCarton.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridNotAssignCarton.ShowCellToolTips = false;
            this.gridNotAssignCarton.Size = new System.Drawing.Size(753, 255);
            this.gridNotAssignCarton.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridNotAssignCarton.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Not Assign Carton# List";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 317);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(153, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Assigned Carton# List";
            // 
            // gridAssignedCarton
            // 
            this.gridAssignedCarton.AllowUserToAddRows = false;
            this.gridAssignedCarton.AllowUserToDeleteRows = false;
            this.gridAssignedCarton.AllowUserToResizeRows = false;
            this.gridAssignedCarton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridAssignedCarton.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridAssignedCarton.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridAssignedCarton.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridAssignedCarton.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridAssignedCarton.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridAssignedCarton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridAssignedCarton.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridAssignedCarton.Location = new System.Drawing.Point(12, 343);
            this.gridAssignedCarton.Name = "gridAssignedCarton";
            this.gridAssignedCarton.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridAssignedCarton.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridAssignedCarton.RowTemplate.Height = 24;
            this.gridAssignedCarton.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridAssignedCarton.ShowCellToolTips = false;
            this.gridAssignedCarton.Size = new System.Drawing.Size(753, 336);
            this.gridAssignedCarton.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridAssignedCarton.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(771, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Carton List";
            // 
            // gridCartonList
            // 
            this.gridCartonList.AllowUserToAddRows = false;
            this.gridCartonList.AllowUserToDeleteRows = false;
            this.gridCartonList.AllowUserToResizeRows = false;
            this.gridCartonList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridCartonList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridCartonList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridCartonList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCartonList.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridCartonList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridCartonList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridCartonList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridCartonList.Location = new System.Drawing.Point(771, 39);
            this.gridCartonList.Name = "gridCartonList";
            this.gridCartonList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridCartonList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridCartonList.RowTemplate.Height = 24;
            this.gridCartonList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCartonList.ShowCellToolTips = false;
            this.gridCartonList.Size = new System.Drawing.Size(312, 640);
            this.gridCartonList.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridCartonList.TabIndex = 6;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(997, 687);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 7;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnEditSave
            // 
            this.btnEditSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditSave.Location = new System.Drawing.Point(911, 687);
            this.btnEditSave.Name = "btnEditSave";
            this.btnEditSave.Size = new System.Drawing.Size(80, 30);
            this.btnEditSave.TabIndex = 8;
            this.btnEditSave.Text = "Edit";
            this.btnEditSave.UseVisualStyleBackColor = true;
            this.btnEditSave.Click += new System.EventHandler(this.BtnEditSave_Click);
            // 
            // btnCancelAssingCarton
            // 
            this.btnCancelAssingCarton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelAssingCarton.BackgroundImage = global::Sci.Production.PublicForm.Properties.Resources.Up_arrow_icon;
            this.btnCancelAssingCarton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancelAssingCarton.Location = new System.Drawing.Point(392, 307);
            this.btnCancelAssingCarton.Name = "btnCancelAssingCarton";
            this.btnCancelAssingCarton.Size = new System.Drawing.Size(80, 30);
            this.btnCancelAssingCarton.TabIndex = 9;
            this.btnCancelAssingCarton.UseVisualStyleBackColor = true;
            this.btnCancelAssingCarton.Click += new System.EventHandler(this.BtnCancelAssingCarton_Click);
            // 
            // btnAssignCarton
            // 
            this.btnAssignCarton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAssignCarton.BackgroundImage = global::Sci.Production.PublicForm.Properties.Resources.Down_thick_arrow_icon;
            this.btnAssignCarton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAssignCarton.Location = new System.Drawing.Point(478, 307);
            this.btnAssignCarton.Name = "btnAssignCarton";
            this.btnAssignCarton.Size = new System.Drawing.Size(80, 30);
            this.btnAssignCarton.TabIndex = 10;
            this.btnAssignCarton.UseVisualStyleBackColor = true;
            this.btnAssignCarton.Click += new System.EventHandler(this.BtnAssignCarton_Click);
            // 
            // TK_PackingList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1089, 729);
            this.Controls.Add(this.btnAssignCarton);
            this.Controls.Add(this.btnCancelAssingCarton);
            this.Controls.Add(this.btnEditSave);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gridCartonList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.gridAssignedCarton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gridNotAssignCarton);
            this.Name = "TK_PackingList";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P06 Packing List";
            this.Controls.SetChildIndex(this.gridNotAssignCarton, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.gridAssignedCarton, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.gridCartonList, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnEditSave, 0);
            this.Controls.SetChildIndex(this.btnCancelAssingCarton, 0);
            this.Controls.SetChildIndex(this.btnAssignCarton, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridNotAssignCarton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAssignedCarton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCartonList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceNotAssignCarton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceAssignedCarton)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCartonList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid gridNotAssignCarton;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Grid gridAssignedCarton;
        private Win.UI.Label label3;
        private Win.UI.Grid gridCartonList;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnEditSave;
        private Win.UI.Button btnCancelAssingCarton;
        private Win.UI.Button btnAssignCarton;
        private Win.UI.ListControlBindingSource bindingSourceNotAssignCarton;
        private Win.UI.ListControlBindingSource bindingSourceAssignedCarton;
        private Win.UI.ListControlBindingSource bindingSourceCartonList;
    }
}