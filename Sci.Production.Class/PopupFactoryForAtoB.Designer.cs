namespace Sci.Production.Class
{
    partial class PopupFactoryForAtoB
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
            this.gridFactory = new Sci.Win.UI.Grid();
            this.btnSelect = new Sci.Win.UI.Button();
            this.btnCancel = new Sci.Win.UI.Button();
            this.label2 = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridFactory)).BeginInit();
            this.SuspendLayout();
            // 
            // gridFactory
            // 
            this.gridFactory.AllowUserToAddRows = false;
            this.gridFactory.AllowUserToDeleteRows = false;
            this.gridFactory.AllowUserToResizeRows = false;
            this.gridFactory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridFactory.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridFactory.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridFactory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFactory.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridFactory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridFactory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridFactory.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridFactory.Location = new System.Drawing.Point(9, 38);
            this.gridFactory.Name = "gridFactory";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridFactory.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridFactory.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridFactory.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridFactory.RowTemplate.Height = 24;
            this.gridFactory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFactory.ShowCellToolTips = false;
            this.gridFactory.Size = new System.Drawing.Size(224, 390);
            this.gridFactory.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridFactory.TabIndex = 3;
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.Location = new System.Drawing.Point(239, 5);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(80, 30);
            this.btnSelect.TabIndex = 4;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.BtnSelect_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(239, 41);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(9, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 6;
            this.label2.Text = "System:";
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(63, 5);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 7;
            this.comboFactory.SelectedIndexChanged += new System.EventHandler(this.ComboFactory_SelectedIndexChanged);
            // 
            // PopupFactoryForAtoB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 440);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.gridFactory);
            this.Name = "PopupFactoryForAtoB";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "PopupFactoryForAtoB";
            this.Controls.SetChildIndex(this.gridFactory, 0);
            this.Controls.SetChildIndex(this.btnSelect, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridFactory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Win.UI.Grid gridFactory;
        private Win.UI.Button btnSelect;
        private Win.UI.Button btnCancel;
        private Win.UI.Label label2;
        private Win.UI.ComboBox comboFactory;
    }
}