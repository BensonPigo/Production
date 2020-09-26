namespace Sci.Production.Subcon
{
    partial class B01_History
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
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.displayRefno = new Sci.Win.UI.DisplayBox();
            this.displayDescription = new Sci.Win.UI.DisplayBox();
            this.gridHistory = new Sci.Win.UI.Grid();
            this.labelRefno = new Sci.Win.UI.Label();
            this.btnClose = new Sci.Win.UI.Button();
            this.labelDescription = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // displayRefno
            // 
            this.displayRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayRefno.Location = new System.Drawing.Point(87, 9);
            this.displayRefno.Name = "displayRefno";
            this.displayRefno.Size = new System.Drawing.Size(197, 23);
            this.displayRefno.TabIndex = 0;
            // 
            // displayDescription
            // 
            this.displayDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDescription.Location = new System.Drawing.Point(405, 9);
            this.displayDescription.Name = "displayDescription";
            this.displayDescription.Size = new System.Drawing.Size(486, 23);
            this.displayDescription.TabIndex = 1;
            // 
            // gridHistory
            // 
            this.gridHistory.AllowUserToAddRows = false;
            this.gridHistory.AllowUserToDeleteRows = false;
            this.gridHistory.AllowUserToResizeRows = false;
            this.gridHistory.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridHistory.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridHistory.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridHistory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridHistory.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridHistory.Location = new System.Drawing.Point(12, 41);
            this.gridHistory.Name = "gridHistory";
            this.gridHistory.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridHistory.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridHistory.RowTemplate.Height = 24;
            this.gridHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridHistory.Size = new System.Drawing.Size(938, 470);
            this.gridHistory.TabIndex = 2;
            this.gridHistory.TabStop = false;
            // 
            // labelRefno
            // 
            this.labelRefno.Lines = 0;
            this.labelRefno.Location = new System.Drawing.Point(9, 9);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.Size = new System.Drawing.Size(75, 23);
            this.labelRefno.TabIndex = 3;
            this.labelRefno.Text = "Refno";
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(870, 519);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // labelDescription
            // 
            this.labelDescription.Lines = 0;
            this.labelDescription.Location = new System.Drawing.Point(327, 9);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(75, 23);
            this.labelDescription.TabIndex = 5;
            this.labelDescription.Text = "Description";
            // 
            // B01_History
            // 
            this.ClientSize = new System.Drawing.Size(962, 561);
            this.Controls.Add(this.labelDescription);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.labelRefno);
            this.Controls.Add(this.gridHistory);
            this.Controls.Add(this.displayDescription);
            this.Controls.Add(this.displayRefno);
            this.Name = "B01_History";
            this.Text = "Payment History";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridHistory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource bindingSource1;
        private Win.UI.DisplayBox displayRefno;
        private Win.UI.DisplayBox displayDescription;
        private Win.UI.Grid gridHistory;
        private Win.UI.Label labelRefno;
        private Win.UI.Button btnClose;
        private Win.UI.Label labelDescription;
    }
}
