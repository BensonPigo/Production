namespace Sci.Production.Shipping
{
    partial class P10_UpdatePulloutDate
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.panel4 = new Sci.Win.UI.Panel();
            this.pictureBox1 = new Sci.Win.UI.PictureBox();
            this.labelPulloutDate = new Sci.Win.UI.Label();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.datePulloutDate = new Sci.Win.UI.DateBox();
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridUpdatePulloutDate = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridUpdatePulloutDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 507);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(843, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 507);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(838, 5);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.pictureBox1);
            this.panel4.Controls.Add(this.labelPulloutDate);
            this.panel4.Controls.Add(this.btnCancel);
            this.panel4.Controls.Add(this.btnSave);
            this.panel4.Controls.Add(this.datePulloutDate);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 464);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(838, 43);
            this.panel4.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = global::Sci.Production.Shipping.Properties.Resources.trffc15;
            this.pictureBox1.Location = new System.Drawing.Point(436, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(27, 32);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.PictureBox1_Click);
            // 
            // labelPulloutDate
            // 
            this.labelPulloutDate.Lines = 0;
            this.labelPulloutDate.Location = new System.Drawing.Point(213, 11);
            this.labelPulloutDate.Name = "labelPulloutDate";
            this.labelPulloutDate.Size = new System.Drawing.Size(82, 23);
            this.labelPulloutDate.TabIndex = 4;
            this.labelPulloutDate.Text = "Pullout Date";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(755, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(665, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(84, 30);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // datePulloutDate
            // 
            this.datePulloutDate.Location = new System.Drawing.Point(300, 11);
            this.datePulloutDate.Name = "datePulloutDate";
            this.datePulloutDate.Size = new System.Drawing.Size(130, 23);
            this.datePulloutDate.TabIndex = 0;
            this.datePulloutDate.Validating += new System.ComponentModel.CancelEventHandler(this.DatePulloutDate_Validating);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridUpdatePulloutDate);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 5);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(838, 459);
            this.panel5.TabIndex = 4;
            // 
            // gridUpdatePulloutDate
            // 
            this.gridUpdatePulloutDate.AllowUserToAddRows = false;
            this.gridUpdatePulloutDate.AllowUserToDeleteRows = false;
            this.gridUpdatePulloutDate.AllowUserToResizeRows = false;
            this.gridUpdatePulloutDate.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridUpdatePulloutDate.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridUpdatePulloutDate.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridUpdatePulloutDate.DataSource = this.listControlBindingSource1;
            this.gridUpdatePulloutDate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridUpdatePulloutDate.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridUpdatePulloutDate.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridUpdatePulloutDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridUpdatePulloutDate.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridUpdatePulloutDate.Location = new System.Drawing.Point(0, 0);
            this.gridUpdatePulloutDate.Name = "gridUpdatePulloutDate";
            this.gridUpdatePulloutDate.RowHeadersVisible = false;
            this.gridUpdatePulloutDate.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridUpdatePulloutDate.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridUpdatePulloutDate.RowTemplate.Height = 24;
            this.gridUpdatePulloutDate.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridUpdatePulloutDate.Size = new System.Drawing.Size(838, 459);
            this.gridUpdatePulloutDate.TabIndex = 0;
            this.gridUpdatePulloutDate.TabStop = false;
            // 
            // P10_UpdatePulloutDate
            // 
            this.AcceptButton = this.btnSave;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(848, 507);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Name = "P10_UpdatePulloutDate";
            this.Text = "Update Pullout Date";
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridUpdatePulloutDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Panel panel4;
        private Win.UI.DateBox datePulloutDate;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridUpdatePulloutDate;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnSave;
        private Win.UI.Label labelPulloutDate;
        private Win.UI.PictureBox pictureBox1;
    }
}
