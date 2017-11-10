namespace Sci.Production.Planning
{
    partial class B03_Copy
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
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel1 = new Sci.Win.UI.Panel();
            this.gridCopyTo = new Sci.Win.UI.Grid();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnCommit = new Sci.Win.UI.Button();
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCopyTo)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.gridCopyTo);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 59);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 449);
            this.panel1.TabIndex = 23;
            // 
            // gridCopyTo
            // 
            this.gridCopyTo.AllowUserToAddRows = false;
            this.gridCopyTo.AllowUserToDeleteRows = false;
            this.gridCopyTo.AllowUserToResizeRows = false;
            this.gridCopyTo.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridCopyTo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.gridCopyTo.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridCopyTo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridCopyTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCopyTo.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridCopyTo.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridCopyTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridCopyTo.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridCopyTo.Location = new System.Drawing.Point(0, 0);
            this.gridCopyTo.Name = "gridCopyTo";
            this.gridCopyTo.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridCopyTo.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridCopyTo.RowTemplate.Height = 24;
            this.gridCopyTo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridCopyTo.Size = new System.Drawing.Size(1008, 449);
            this.gridCopyTo.TabIndex = 0;
            this.gridCopyTo.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1008, 59);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(912, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnCommit
            // 
            this.btnCommit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCommit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCommit.Location = new System.Drawing.Point(816, 15);
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Size = new System.Drawing.Size(90, 30);
            this.btnCommit.TabIndex = 0;
            this.btnCommit.Text = "Commit";
            this.btnCommit.UseVisualStyleBackColor = true;
            this.btnCommit.Click += new System.EventHandler(this.BtnCommit_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnClose);
            this.groupBox2.Controls.Add(this.btnCommit);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 508);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1008, 53);
            this.groupBox2.TabIndex = 22;
            this.groupBox2.TabStop = false;
            // 
            // B03_Copy
            // 
            this.ClientSize = new System.Drawing.Size(1008, 561);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Name = "B03_Copy";
            this.Text = "Copy To";
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCopyTo)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel1;
        private Win.UI.Grid gridCopyTo;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnCommit;
        private Win.UI.GroupBox groupBox2;
    }
}
