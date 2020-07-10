namespace Sci.Production.Quality
{
    partial class P26
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel6 = new System.Windows.Forms.Panel();
            this.txtReturnTo = new Sci.Production.Class.TxtDropDownList();
            this.label1 = new Sci.Win.UI.Label();
            this.txtPackID = new Sci.Win.UI.TextBox();
            this.dateReturnDate = new Sci.Win.UI.DateRange();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.btnFind = new Sci.Win.UI.Button();
            this.labelPONo = new Sci.Win.UI.Label();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.labTransDate = new Sci.Win.UI.Label();
            this.grid = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.txtReturnTo);
            this.panel6.Controls.Add(this.label1);
            this.panel6.Controls.Add(this.txtPackID);
            this.panel6.Controls.Add(this.dateReturnDate);
            this.panel6.Controls.Add(this.labelSPNo);
            this.panel6.Controls.Add(this.btnFind);
            this.panel6.Controls.Add(this.labelPONo);
            this.panel6.Controls.Add(this.txtSPNo);
            this.panel6.Controls.Add(this.labTransDate);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(747, 86);
            this.panel6.TabIndex = 41;
            // 
            // txtReturnTo
            // 
            this.txtReturnTo.BackColor = System.Drawing.Color.White;
            this.txtReturnTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtReturnTo.IsSupportEditMode = false;
            this.txtReturnTo.Location = new System.Drawing.Point(470, 8);
            this.txtReturnTo.Name = "txtReturnTo";
            this.txtReturnTo.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtReturnTo.Size = new System.Drawing.Size(153, 23);
            this.txtReturnTo.TabIndex = 1;
            this.txtReturnTo.Type = "Pms_CFAReturnReason";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(8, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 23);
            this.label1.TabIndex = 7;
            this.label1.Text = "Pack ID";
            // 
            // txtPackID
            // 
            this.txtPackID.BackColor = System.Drawing.Color.White;
            this.txtPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackID.IsSupportEditMode = false;
            this.txtPackID.Location = new System.Drawing.Point(103, 40);
            this.txtPackID.Name = "txtPackID";
            this.txtPackID.Size = new System.Drawing.Size(146, 23);
            this.txtPackID.TabIndex = 2;
            // 
            // dateReturnDate
            // 
            // 
            // 
            // 
            this.dateReturnDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateReturnDate.DateBox1.Name = "";
            this.dateReturnDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateReturnDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateReturnDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateReturnDate.DateBox2.Name = "";
            this.dateReturnDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateReturnDate.DateBox2.TabIndex = 1;
            this.dateReturnDate.IsRequired = false;
            this.dateReturnDate.IsSupportEditMode = false;
            this.dateReturnDate.Location = new System.Drawing.Point(103, 8);
            this.dateReturnDate.Name = "dateReturnDate";
            this.dateReturnDate.Size = new System.Drawing.Size(280, 23);
            this.dateReturnDate.TabIndex = 0;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(393, 40);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(74, 23);
            this.labelSPNo.TabIndex = 8;
            this.labelSPNo.Text = "SP#";
            // 
            // btnFind
            // 
            this.btnFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFind.Location = new System.Drawing.Point(654, 8);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 30);
            this.btnFind.TabIndex = 4;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // labelPONo
            // 
            this.labelPONo.Location = new System.Drawing.Point(393, 8);
            this.labelPONo.Name = "labelPONo";
            this.labelPONo.Size = new System.Drawing.Size(74, 23);
            this.labelPONo.TabIndex = 6;
            this.labelPONo.Text = "Return To";
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.IsSupportEditMode = false;
            this.txtSPNo.Location = new System.Drawing.Point(470, 40);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(153, 23);
            this.txtSPNo.TabIndex = 3;
            // 
            // labTransDate
            // 
            this.labTransDate.Location = new System.Drawing.Point(8, 8);
            this.labTransDate.Name = "labTransDate";
            this.labTransDate.Size = new System.Drawing.Size(92, 23);
            this.labTransDate.TabIndex = 5;
            this.labTransDate.Text = "Return Date";
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(0, 86);
            this.grid.Name = "grid";
            this.grid.RowHeadersVisible = false;
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(747, 349);
            this.grid.TabIndex = 43;
            this.grid.TabStop = false;
            // 
            // P26
            // 
            this.ClientSize = new System.Drawing.Size(747, 435);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.panel6);
            this.Name = "P26";
            this.Text = "P26. Query For CFA Return Record";
            this.Controls.SetChildIndex(this.panel6, 0);
            this.Controls.SetChildIndex(this.grid, 0);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel6;
        private Win.UI.DateRange dateReturnDate;
        private Win.UI.Label labelSPNo;
        private Win.UI.Button btnFind;
        private Win.UI.Label labelPONo;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.Label labTransDate;
        private Win.UI.Grid grid;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtPackID;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Class.TxtDropDownList txtReturnTo;
    }
}
