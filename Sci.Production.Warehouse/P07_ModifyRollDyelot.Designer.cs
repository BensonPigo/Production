namespace Sci.Production.Warehouse
{
    partial class P07_ModifyRollDyelot
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
            this.panel2 = new Sci.Win.UI.Panel();
            this.btnCommit = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.bindingSource1 = new Sci.Win.UI.BindingSource(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridModifyRoll = new Sci.Win.UI.Grid();
            this.gridDyelot = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridModifyRoll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDyelot)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnCommit);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 663);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 48);
            this.panel2.TabIndex = 0;
            // 
            // btnCommit
            // 
            this.btnCommit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCommit.Location = new System.Drawing.Point(834, 9);
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Size = new System.Drawing.Size(80, 30);
            this.btnCommit.TabIndex = 1;
            this.btnCommit.Text = "Commit";
            this.btnCommit.UseVisualStyleBackColor = true;
            this.btnCommit.Click += new System.EventHandler(this.BtnCommit_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(916, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridModifyRoll);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridDyelot);
            this.splitContainer1.Size = new System.Drawing.Size(1008, 663);
            this.splitContainer1.SplitterDistance = 433;
            this.splitContainer1.TabIndex = 1;
            // 
            // gridModifyRoll
            // 
            this.gridModifyRoll.AllowUserToAddRows = false;
            this.gridModifyRoll.AllowUserToDeleteRows = false;
            this.gridModifyRoll.AllowUserToResizeRows = false;
            this.gridModifyRoll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridModifyRoll.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridModifyRoll.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridModifyRoll.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridModifyRoll.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridModifyRoll.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridModifyRoll.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridModifyRoll.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridModifyRoll.Location = new System.Drawing.Point(0, 0);
            this.gridModifyRoll.Name = "gridModifyRoll";
            this.gridModifyRoll.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridModifyRoll.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridModifyRoll.RowTemplate.Height = 24;
            this.gridModifyRoll.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridModifyRoll.ShowCellToolTips = false;
            this.gridModifyRoll.Size = new System.Drawing.Size(1008, 430);
            this.gridModifyRoll.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridModifyRoll.TabIndex = 0;
            this.gridModifyRoll.TabStop = false;
            this.gridModifyRoll.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.Grid1_RowEnter);
            // 
            // gridDyelot
            // 
            this.gridDyelot.AllowUserToAddRows = false;
            this.gridDyelot.AllowUserToDeleteRows = false;
            this.gridDyelot.AllowUserToResizeRows = false;
            this.gridDyelot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridDyelot.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDyelot.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDyelot.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDyelot.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDyelot.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDyelot.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDyelot.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDyelot.Location = new System.Drawing.Point(3, 3);
            this.gridDyelot.Name = "gridDyelot";
            this.gridDyelot.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDyelot.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDyelot.RowTemplate.Height = 24;
            this.gridDyelot.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDyelot.ShowCellToolTips = false;
            this.gridDyelot.Size = new System.Drawing.Size(1002, 217);
            this.gridDyelot.TabIndex = 0;
            this.gridDyelot.TabStop = false;
            // 
            // P07_ModifyRollDyelot
            // 
            this.ClientSize = new System.Drawing.Size(1008, 711);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel2);
            this.Name = "P07_ModifyRollDyelot";
            this.Text = "P07. Modify Roll & Dyelot";
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridModifyRoll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDyelot)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private Win.UI.BindingSource bindingSource1;
        private Win.UI.Button btnCommit;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Grid gridModifyRoll;
        private Win.UI.Grid gridDyelot;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}
