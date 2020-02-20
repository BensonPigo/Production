namespace Sci.Production.Cutting
{
    partial class B10
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(B10));
            this.grid1 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.pictureBoxup = new Sci.Win.UI.PictureBox();
            this.pictureBoxdown = new Sci.Win.UI.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxdown)).BeginInit();
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
            this.grid1.IsSupportCancelSorting = true;
            this.grid1.Location = new System.Drawing.Point(12, 36);
            this.grid1.MultiSelect = false;
            this.grid1.Name = "grid1";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid1.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(566, 614);
            this.grid1.TabIndex = 1;
            // 
            // pictureBoxup
            // 
            this.pictureBoxup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxup.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxup.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxup.Image")));
            this.pictureBoxup.InitialImage = global::Sci.Production.Cutting.Properties.Resources.trffc15;
            this.pictureBoxup.Location = new System.Drawing.Point(595, 139);
            this.pictureBoxup.Name = "pictureBoxup";
            this.pictureBoxup.Size = new System.Drawing.Size(25, 31);
            this.pictureBoxup.TabIndex = 36;
            this.pictureBoxup.TabStop = false;
            this.pictureBoxup.WaitOnLoad = true;
            this.pictureBoxup.Click += new System.EventHandler(this.PictureBoxup_Click);
            // 
            // pictureBoxdown
            // 
            this.pictureBoxdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxdown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxdown.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxdown.Image")));
            this.pictureBoxdown.InitialImage = global::Sci.Production.Cutting.Properties.Resources.trffc152;
            this.pictureBoxdown.Location = new System.Drawing.Point(595, 241);
            this.pictureBoxdown.Name = "pictureBoxdown";
            this.pictureBoxdown.Size = new System.Drawing.Size(25, 31);
            this.pictureBoxdown.TabIndex = 37;
            this.pictureBoxdown.TabStop = false;
            this.pictureBoxdown.WaitOnLoad = true;
            this.pictureBoxdown.Click += new System.EventHandler(this.PictureBoxdown_Click);
            // 
            // B10
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 662);
            this.Controls.Add(this.pictureBoxdown);
            this.Controls.Add(this.pictureBoxup);
            this.Controls.Add(this.grid1);
            this.Name = "B10";
            this.OnLineHelpID = "Sci.Win.Tems.Input7";
            this.Text = "B10";
            this.WorkAlias = "SubProcess";
            this.Controls.SetChildIndex(this.grid1, 0);
            this.Controls.SetChildIndex(this.pictureBoxup, 0);
            this.Controls.SetChildIndex(this.pictureBoxdown, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxdown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Grid grid1;
        private Win.UI.PictureBox pictureBoxup;
        private Win.UI.PictureBox pictureBoxdown;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}