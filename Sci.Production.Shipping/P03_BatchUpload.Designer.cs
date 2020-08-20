namespace Sci.Production.Shipping
{
    partial class P03_BatchUpload
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(P03_BatchUpload));
            this.panel3 = new Sci.Win.UI.Panel();
            this.txtWKNo2 = new Sci.Win.UI.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtWKNo1 = new Sci.Win.UI.TextBox();
            this.dateETA = new Sci.Win.UI.DateRange();
            this.labETA = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            this.labWkNo = new Sci.Win.UI.Label();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel6 = new Sci.Win.UI.Panel();
            this.pictureBox2 = new Sci.Win.UI.PictureBox();
            this.labDoxRcvDate = new Sci.Win.UI.Label();
            this.datedocRcvDate = new Sci.Win.UI.DateBox();
            this.pictureBox1 = new Sci.Win.UI.PictureBox();
            this.labArrivePortDate = new Sci.Win.UI.Label();
            this.dateArrivePortDate = new Sci.Win.UI.DateBox();
            this.button1 = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.gridUpload = new Sci.Win.UI.Grid();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridUpload)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtWKNo2);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.txtWKNo1);
            this.panel3.Controls.Add(this.dateETA);
            this.panel3.Controls.Add(this.labETA);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.labWkNo);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1004, 45);
            this.panel3.TabIndex = 3;
            // 
            // txtWKNo2
            // 
            this.txtWKNo2.BackColor = System.Drawing.Color.White;
            this.txtWKNo2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtWKNo2.Location = new System.Drawing.Point(228, 12);
            this.txtWKNo2.Name = "txtWKNo2";
            this.txtWKNo2.Size = new System.Drawing.Size(120, 23);
            this.txtWKNo2.TabIndex = 48;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(206, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(16, 17);
            this.label1.TabIndex = 47;
            this.label1.Text = "~";
            // 
            // txtWKNo1
            // 
            this.txtWKNo1.BackColor = System.Drawing.Color.White;
            this.txtWKNo1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtWKNo1.Location = new System.Drawing.Point(81, 13);
            this.txtWKNo1.Name = "txtWKNo1";
            this.txtWKNo1.Size = new System.Drawing.Size(120, 23);
            this.txtWKNo1.TabIndex = 46;
            // 
            // dateETA
            // 
            // 
            // 
            // 
            this.dateETA.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateETA.DateBox1.Name = "";
            this.dateETA.DateBox1.Size = new System.Drawing.Size(108, 23);
            this.dateETA.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateETA.DateBox2.Location = new System.Drawing.Point(130, 0);
            this.dateETA.DateBox2.Name = "";
            this.dateETA.DateBox2.Size = new System.Drawing.Size(108, 23);
            this.dateETA.DateBox2.TabIndex = 1;
            this.dateETA.IsRequired = false;
            this.dateETA.Location = new System.Drawing.Point(558, 12);
            this.dateETA.Name = "dateETA";
            this.dateETA.Size = new System.Drawing.Size(238, 23);
            this.dateETA.TabIndex = 4;
            // 
            // labETA
            // 
            this.labETA.Location = new System.Drawing.Point(503, 12);
            this.labETA.Name = "labETA";
            this.labETA.Size = new System.Drawing.Size(52, 23);
            this.labETA.TabIndex = 3;
            this.labETA.Text = "ETA";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(870, 8);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 2;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // labWkNo
            // 
            this.labWkNo.Location = new System.Drawing.Point(9, 12);
            this.labWkNo.Name = "labWkNo";
            this.labWkNo.Size = new System.Drawing.Size(69, 23);
            this.labWkNo.TabIndex = 0;
            this.labWkNo.Text = "WK No.";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.pictureBox2);
            this.panel6.Controls.Add(this.labDoxRcvDate);
            this.panel6.Controls.Add(this.datedocRcvDate);
            this.panel6.Controls.Add(this.pictureBox1);
            this.panel6.Controls.Add(this.labArrivePortDate);
            this.panel6.Controls.Add(this.dateArrivePortDate);
            this.panel6.Controls.Add(this.button1);
            this.panel6.Controls.Add(this.btnSave);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 478);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1004, 95);
            this.panel6.TabIndex = 7;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(962, 10);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(27, 32);
            this.pictureBox2.TabIndex = 11;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.WaitOnLoad = true;
            this.pictureBox2.Click += new System.EventHandler(this.PictureBox2_Click);
            // 
            // labDoxRcvDate
            // 
            this.labDoxRcvDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labDoxRcvDate.Location = new System.Drawing.Point(786, 15);
            this.labDoxRcvDate.Name = "labDoxRcvDate";
            this.labDoxRcvDate.Size = new System.Drawing.Size(74, 23);
            this.labDoxRcvDate.TabIndex = 10;
            this.labDoxRcvDate.Text = "Dox Rcv Date";
            // 
            // datedocRcvDate
            // 
            this.datedocRcvDate.Location = new System.Drawing.Point(863, 15);
            this.datedocRcvDate.Name = "datedocRcvDate";
            this.datedocRcvDate.Size = new System.Drawing.Size(95, 23);
            this.datedocRcvDate.TabIndex = 9;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(748, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(27, 32);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.WaitOnLoad = true;
            this.pictureBox1.Click += new System.EventHandler(this.PictureBox1_Click);
            // 
            // labArrivePortDate
            // 
            this.labArrivePortDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labArrivePortDate.Location = new System.Drawing.Point(562, 15);
            this.labArrivePortDate.Name = "labArrivePortDate";
            this.labArrivePortDate.Size = new System.Drawing.Size(86, 23);
            this.labArrivePortDate.TabIndex = 7;
            this.labArrivePortDate.Text = "Arrive Port Date";
            // 
            // dateArrivePortDate
            // 
            this.dateArrivePortDate.Location = new System.Drawing.Point(650, 15);
            this.dateArrivePortDate.Name = "dateArrivePortDate";
            this.dateArrivePortDate.Size = new System.Drawing.Size(95, 23);
            this.dateArrivePortDate.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(879, 53);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(787, 53);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // gridUpload
            // 
            this.gridUpload.AllowUserToAddRows = false;
            this.gridUpload.AllowUserToDeleteRows = false;
            this.gridUpload.AllowUserToResizeRows = false;
            this.gridUpload.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridUpload.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridUpload.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridUpload.DataSource = this.listControlBindingSource1;
            this.gridUpload.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridUpload.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridUpload.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridUpload.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridUpload.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridUpload.Location = new System.Drawing.Point(0, 45);
            this.gridUpload.Name = "gridUpload";
            this.gridUpload.RowHeadersVisible = false;
            this.gridUpload.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridUpload.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridUpload.RowTemplate.Height = 24;
            this.gridUpload.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridUpload.ShowCellToolTips = false;
            this.gridUpload.Size = new System.Drawing.Size(1004, 433);
            this.gridUpload.TabIndex = 8;
            this.gridUpload.TabStop = false;
            // 
            // P03_BatchUpload
            // 
            this.ClientSize = new System.Drawing.Size(1004, 573);
            this.Controls.Add(this.gridUpload);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel3);
            this.Name = "P03_BatchUpload";
            this.Text = "Batch Upload - Shipping";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridUpload)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel3;
        private Win.UI.Button btnQuery;
        private Win.UI.Label labWkNo;
        private Win.UI.DateRange dateETA;
        private Win.UI.Label labETA;
        private Win.UI.TextBox txtWKNo2;
        private System.Windows.Forms.Label label1;
        private Win.UI.TextBox txtWKNo1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel6;
        private Win.UI.PictureBox pictureBox2;
        private Win.UI.Label labDoxRcvDate;
        private Win.UI.DateBox datedocRcvDate;
        private Win.UI.PictureBox pictureBox1;
        private Win.UI.Label labArrivePortDate;
        private Win.UI.DateBox dateArrivePortDate;
        private Win.UI.Button button1;
        private Win.UI.Button btnSave;
        private Win.UI.Grid gridUpload;
    }
}
