namespace Sci.Production.PublicForm
{
    partial class IntendedDeliveryDate
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridIDD = new Sci.Win.UI.Grid();
            this.btnEdit = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridIDD)).BeginInit();
            this.SuspendLayout();
            // 
            // gridIDD
            // 
            this.gridIDD.AllowUserToAddRows = false;
            this.gridIDD.AllowUserToDeleteRows = false;
            this.gridIDD.AllowUserToResizeRows = false;
            this.gridIDD.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridIDD.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridIDD.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridIDD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridIDD.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridIDD.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridIDD.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridIDD.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridIDD.Location = new System.Drawing.Point(12, 12);
            this.gridIDD.Name = "gridIDD";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridIDD.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridIDD.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridIDD.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridIDD.RowTemplate.Height = 24;
            this.gridIDD.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridIDD.ShowCellToolTips = false;
            this.gridIDD.Size = new System.Drawing.Size(866, 409);
            this.gridIDD.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridIDD.TabIndex = 1;
            this.gridIDD.Sorted += new System.EventHandler(this.GridIDD_Sorted);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Location = new System.Drawing.Point(712, 427);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 30);
            this.btnEdit.TabIndex = 2;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.BtnEdit_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(798, 427);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // IntendedDeliveryDate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 471);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.gridIDD);
            this.Name = "IntendedDeliveryDate";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "Intended delivery by shipmode";
            this.Controls.SetChildIndex(this.gridIDD, 0);
            this.Controls.SetChildIndex(this.btnEdit, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridIDD)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid gridIDD;
        private Win.UI.Button btnEdit;
        private Win.UI.Button btnClose;
    }
}