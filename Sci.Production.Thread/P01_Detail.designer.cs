namespace Sci.Production.Thread
{
    partial class P01_Detail
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
            this.panel2 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnEdit = new Sci.Win.UI.Button();
            this.label2 = new Sci.Win.UI.Label();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.panel1 = new Sci.Win.UI.Panel();
            this.displayThreadCombination = new Sci.Win.UI.DisplayBox();
            this.displaySeason = new Sci.Win.UI.DisplayBox();
            this.labelSeason = new Sci.Win.UI.Label();
            this.displayStyleNo = new Sci.Win.UI.DisplayBox();
            this.labelStyleNo = new Sci.Win.UI.Label();
            this.displayMachineType = new Sci.Win.UI.DisplayBox();
            this.labelMachineType = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.displayBoxEdit = new Sci.Win.UI.DisplayBox();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.displayBoxEdit);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Controls.Add(this.btnEdit);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 437);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(946, 50);
            this.panel2.TabIndex = 11;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(856, 13);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Location = new System.Drawing.Point(770, 13);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(80, 30);
            this.btnEdit.TabIndex = 0;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(448, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 23);
            this.label2.TabIndex = 15;
            this.label2.Text = "Thread Combination";
            // 
            // gridDetail
            // 
            this.gridDetail.AllowUserToAddRows = false;
            this.gridDetail.AllowUserToDeleteRows = false;
            this.gridDetail.AllowUserToResizeRows = false;
            this.gridDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDetail.Location = new System.Drawing.Point(0, 58);
            this.gridDetail.Name = "gridDetail";
            this.gridDetail.RowHeadersWidth = 21;
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.Size = new System.Drawing.Size(946, 379);
            this.gridDetail.TabIndex = 0;
            this.gridDetail.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.displayThreadCombination);
            this.panel1.Controls.Add(this.displaySeason);
            this.panel1.Controls.Add(this.labelSeason);
            this.panel1.Controls.Add(this.displayStyleNo);
            this.panel1.Controls.Add(this.labelStyleNo);
            this.panel1.Controls.Add(this.displayMachineType);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.labelMachineType);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(946, 58);
            this.panel1.TabIndex = 10;
            // 
            // displayThreadCombination
            // 
            this.displayThreadCombination.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayThreadCombination.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayThreadCombination.Location = new System.Drawing.Point(584, 17);
            this.displayThreadCombination.Name = "displayThreadCombination";
            this.displayThreadCombination.Size = new System.Drawing.Size(121, 23);
            this.displayThreadCombination.TabIndex = 21;
            // 
            // displaySeason
            // 
            this.displaySeason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySeason.Location = new System.Drawing.Point(352, 17);
            this.displaySeason.Name = "displaySeason";
            this.displaySeason.Size = new System.Drawing.Size(64, 23);
            this.displaySeason.TabIndex = 20;
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(286, 17);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(63, 23);
            this.labelSeason.TabIndex = 19;
            this.labelSeason.Text = "Season";
            // 
            // displayStyleNo
            // 
            this.displayStyleNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyleNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyleNo.Location = new System.Drawing.Point(92, 17);
            this.displayStyleNo.Name = "displayStyleNo";
            this.displayStyleNo.Size = new System.Drawing.Size(165, 23);
            this.displayStyleNo.TabIndex = 18;
            // 
            // labelStyleNo
            // 
            this.labelStyleNo.Location = new System.Drawing.Point(20, 17);
            this.labelStyleNo.Name = "labelStyleNo";
            this.labelStyleNo.Size = new System.Drawing.Size(69, 23);
            this.labelStyleNo.TabIndex = 17;
            this.labelStyleNo.Text = "Style No";
            // 
            // displayMachineType
            // 
            this.displayMachineType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayMachineType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayMachineType.Location = new System.Drawing.Point(824, 17);
            this.displayMachineType.Name = "displayMachineType";
            this.displayMachineType.Size = new System.Drawing.Size(100, 23);
            this.displayMachineType.TabIndex = 16;
            // 
            // labelMachineType
            // 
            this.labelMachineType.Location = new System.Drawing.Point(726, 17);
            this.labelMachineType.Name = "labelMachineType";
            this.labelMachineType.Size = new System.Drawing.Size(95, 23);
            this.labelMachineType.TabIndex = 13;
            this.labelMachineType.Text = "Machine Type";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(20, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 23);
            this.label1.TabIndex = 18;
            this.label1.Text = "Edit By:";
            // 
            // displayBoxEdit
            // 
            this.displayBoxEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxEdit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxEdit.Location = new System.Drawing.Point(92, 13);
            this.displayBoxEdit.Name = "displayBoxEdit";
            this.displayBoxEdit.Size = new System.Drawing.Size(309, 23);
            this.displayBoxEdit.TabIndex = 19;
            // 
            // P01_Detail
            // 
            this.ClientSize = new System.Drawing.Size(946, 487);
            this.Controls.Add(this.gridDetail);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.EditMode = false;
            this.Name = "P01_Detail";
            this.Text = "P01_Detail";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel2;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnEdit;
        private Win.UI.Grid gridDetail;
        private Win.UI.Panel panel1;
        private Win.UI.Label labelMachineType;
        private Win.UI.Label label2;
        private Win.UI.DisplayBox displayThreadCombination;
        private Win.UI.DisplayBox displaySeason;
        private Win.UI.Label labelSeason;
        private Win.UI.DisplayBox displayStyleNo;
        private Win.UI.Label labelStyleNo;
        private Win.UI.DisplayBox displayMachineType;
        private Win.UI.Label label1;
        private Win.UI.DisplayBox displayBoxEdit;
    }
}
