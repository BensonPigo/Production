namespace Sci.Production.Sewing
{
    partial class P07
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lbReceiveDate = new Sci.Win.UI.Label();
            this.lbPackID = new Sci.Win.UI.Label();
            this.lbSP = new Sci.Win.UI.Label();
            this.dateReceive = new Sci.Win.UI.DateRange();
            this.txtPackID = new Sci.Win.UI.TextBox();
            this.txtsp = new Sci.Win.UI.TextBox();
            this.btnQuery = new Sci.Win.UI.Button();
            this.grid1 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbReceiveDate
            // 
            this.lbReceiveDate.Location = new System.Drawing.Point(9, 9);
            this.lbReceiveDate.Name = "lbReceiveDate";
            this.lbReceiveDate.Size = new System.Drawing.Size(93, 23);
            this.lbReceiveDate.TabIndex = 1;
            this.lbReceiveDate.Text = "Receive Date";
            this.lbReceiveDate.BackColor = System.Drawing.Color.LightSkyBlue;
            this.lbReceiveDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // lbPackID
            // 
            this.lbPackID.Location = new System.Drawing.Point(9, 46);
            this.lbPackID.Name = "lbPackID";
            this.lbPackID.Size = new System.Drawing.Size(93, 23);
            this.lbPackID.TabIndex = 2;
            this.lbPackID.Text = "Pack ID";
            this.lbPackID.BackColor = System.Drawing.Color.LightSkyBlue;
            this.lbPackID.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // lbSP
            // 
            this.lbSP.Location = new System.Drawing.Point(390, 46);
            this.lbSP.Name = "lbSP";
            this.lbSP.Size = new System.Drawing.Size(75, 23);
            this.lbSP.TabIndex = 3;
            this.lbSP.Text = "SP#";
            this.lbSP.BackColor = System.Drawing.Color.LightSkyBlue;
            this.lbSP.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateReceive
            // 
            // 
            // 
            // 
            this.dateReceive.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateReceive.DateBox1.Name = "";
            this.dateReceive.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateReceive.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateReceive.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateReceive.DateBox2.Name = "";
            this.dateReceive.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateReceive.DateBox2.TabIndex = 1;
            this.dateReceive.IsSupportEditMode = false;
            this.dateReceive.Location = new System.Drawing.Point(105, 9);
            this.dateReceive.Name = "dateReceive";
            this.dateReceive.Size = new System.Drawing.Size(280, 23);
            this.dateReceive.TabIndex = 4;
            // 
            // txtPackID
            // 
            this.txtPackID.BackColor = System.Drawing.Color.White;
            this.txtPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackID.IsSupportEditMode = false;
            this.txtPackID.Location = new System.Drawing.Point(105, 46);
            this.txtPackID.Name = "txtPackID";
            this.txtPackID.Size = new System.Drawing.Size(163, 23);
            this.txtPackID.TabIndex = 5;
            // 
            // txtsp
            // 
            this.txtsp.BackColor = System.Drawing.Color.White;
            this.txtsp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtsp.IsSupportEditMode = false;
            this.txtsp.Location = new System.Drawing.Point(468, 46);
            this.txtsp.Name = "txtsp";
            this.txtsp.Size = new System.Drawing.Size(118, 23);
            this.txtsp.TabIndex = 6;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(598, 9);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 7;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
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
            this.grid1.Location = new System.Drawing.Point(12, 75);
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
            this.grid1.Size = new System.Drawing.Size(672, 296);
            this.grid1.TabIndex = 8;
            // 
            // P07
            // 
            this.ClientSize = new System.Drawing.Size(696, 383);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.txtsp);
            this.Controls.Add(this.txtPackID);
            this.Controls.Add(this.dateReceive);
            this.Controls.Add(this.lbSP);
            this.Controls.Add(this.lbPackID);
            this.Controls.Add(this.lbReceiveDate);
            this.Name = "P07";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P07. Query For Dry Room Receive Carton Record";
            this.Controls.SetChildIndex(this.lbReceiveDate, 0);
            this.Controls.SetChildIndex(this.lbPackID, 0);
            this.Controls.SetChildIndex(this.lbSP, 0);
            this.Controls.SetChildIndex(this.dateReceive, 0);
            this.Controls.SetChildIndex(this.txtPackID, 0);
            this.Controls.SetChildIndex(this.txtsp, 0);
            this.Controls.SetChildIndex(this.btnQuery, 0);
            this.Controls.SetChildIndex(this.grid1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lbReceiveDate;
        private Win.UI.Label lbPackID;
        private Win.UI.Label lbSP;
        private Win.UI.DateRange dateReceive;
        private Win.UI.TextBox txtPackID;
        private Win.UI.TextBox txtsp;
        private Win.UI.Button btnQuery;
        private Win.UI.Grid grid1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}
