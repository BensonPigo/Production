namespace Sci.Production.Warehouse
{
    partial class P07_UpdateWeight
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
            this.gridUpdateAct = new Sci.Win.UI.Grid();
            this.panel2 = new Sci.Win.UI.Panel();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridUpdateAct)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.gridUpdateAct);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 507);
            this.panel1.TabIndex = 0;
            // 
            // gridUpdateAct
            // 
            this.gridUpdateAct.AllowUserToAddRows = false;
            this.gridUpdateAct.AllowUserToDeleteRows = false;
            this.gridUpdateAct.AllowUserToResizeRows = false;
            this.gridUpdateAct.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridUpdateAct.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridUpdateAct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridUpdateAct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridUpdateAct.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridUpdateAct.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridUpdateAct.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridUpdateAct.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridUpdateAct.Location = new System.Drawing.Point(0, 0);
            this.gridUpdateAct.Name = "gridUpdateAct";
            this.gridUpdateAct.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridUpdateAct.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridUpdateAct.RowTemplate.Height = 24;
            this.gridUpdateAct.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridUpdateAct.ShowCellToolTips = false;
            this.gridUpdateAct.Size = new System.Drawing.Size(1008, 507);
            this.gridUpdateAct.TabIndex = 0;
            this.gridUpdateAct.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 513);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 48);
            this.panel2.TabIndex = 0;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(834, 11);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(916, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // P07_UpdateWeight
            // 
            this.ClientSize = new System.Drawing.Size(1008, 561);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "P07_UpdateWeight";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P07. Update Weight";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridUpdateAct)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Grid gridUpdateAct;
        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnSave;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}
