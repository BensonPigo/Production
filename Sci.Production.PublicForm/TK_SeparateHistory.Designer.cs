namespace Sci.Production.PublicForm
{
    partial class TK_SeparateHistory
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.comboNewID = new Sci.Win.UI.ComboBox();
            this.lblNumberNewTK = new Sci.Win.UI.Label();
            this.displayGroupNewID = new Sci.Win.UI.DisplayBox();
            this.dateFtyRequestSeparateDate = new Sci.Win.UI.DateBox();
            this.dateTPESeparateApprovedDate = new Sci.Win.UI.DateBox();
            this.dateWHSpearateConfirmDate = new Sci.Win.UI.DateBox();
            this.dateShippingSeparateConfirmDate = new Sci.Win.UI.DateBox();
            this.label9 = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
            this.gridTransferWKList = new Sci.Win.UI.Grid();
            this.gridPackingList = new Sci.Win.UI.Grid();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnSeparateConfirm = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridTransferWKList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridPackingList)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ori TK No.";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "New TK Group";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Number of New TK";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(290, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(140, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "Fty Request Separate";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(596, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(172, 23);
            this.label5.TabIndex = 5;
            this.label5.Text = "TPE Separate Approved";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(290, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(140, 23);
            this.label6.TabIndex = 6;
            this.label6.Text = "WH Separate Confirm";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(596, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(172, 23);
            this.label7.TabIndex = 7;
            this.label7.Text = "Shipping Separate Confirm";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(290, 63);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(140, 23);
            this.label8.TabIndex = 8;
            this.label8.Text = "New TK Filter";
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(137, 9);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(100, 23);
            this.displayID.TabIndex = 9;
            // 
            // comboNewID
            // 
            this.comboNewID.BackColor = System.Drawing.Color.White;
            this.comboNewID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboNewID.FormattingEnabled = true;
            this.comboNewID.IsSupportUnselect = true;
            this.comboNewID.Location = new System.Drawing.Point(433, 62);
            this.comboNewID.Name = "comboNewID";
            this.comboNewID.OldText = "";
            this.comboNewID.Size = new System.Drawing.Size(121, 24);
            this.comboNewID.TabIndex = 10;
            this.comboNewID.SelectedIndexChanged += new System.EventHandler(this.ComboNewID_SelectedIndexChanged);
            // 
            // lblNumberNewTK
            // 
            this.lblNumberNewTK.BackColor = System.Drawing.Color.Transparent;
            this.lblNumberNewTK.Location = new System.Drawing.Point(137, 63);
            this.lblNumberNewTK.Name = "lblNumberNewTK";
            this.lblNumberNewTK.Size = new System.Drawing.Size(125, 23);
            this.lblNumberNewTK.TabIndex = 11;
            this.lblNumberNewTK.Text = "0";
            this.lblNumberNewTK.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // displayGroupNewID
            // 
            this.displayGroupNewID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayGroupNewID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayGroupNewID.Location = new System.Drawing.Point(137, 36);
            this.displayGroupNewID.Name = "displayGroupNewID";
            this.displayGroupNewID.Size = new System.Drawing.Size(150, 23);
            this.displayGroupNewID.TabIndex = 12;
            // 
            // dateFtyRequestSeparateDate
            // 
            this.dateFtyRequestSeparateDate.IsSupportEditMode = false;
            this.dateFtyRequestSeparateDate.Location = new System.Drawing.Point(433, 9);
            this.dateFtyRequestSeparateDate.Name = "dateFtyRequestSeparateDate";
            this.dateFtyRequestSeparateDate.ReadOnly = true;
            this.dateFtyRequestSeparateDate.Size = new System.Drawing.Size(130, 23);
            this.dateFtyRequestSeparateDate.TabIndex = 13;
            // 
            // dateTPESeparateApprovedDate
            // 
            this.dateTPESeparateApprovedDate.IsSupportEditMode = false;
            this.dateTPESeparateApprovedDate.Location = new System.Drawing.Point(771, 9);
            this.dateTPESeparateApprovedDate.Name = "dateTPESeparateApprovedDate";
            this.dateTPESeparateApprovedDate.ReadOnly = true;
            this.dateTPESeparateApprovedDate.Size = new System.Drawing.Size(130, 23);
            this.dateTPESeparateApprovedDate.TabIndex = 14;
            // 
            // dateWHSpearateConfirmDate
            // 
            this.dateWHSpearateConfirmDate.IsSupportEditMode = false;
            this.dateWHSpearateConfirmDate.Location = new System.Drawing.Point(433, 36);
            this.dateWHSpearateConfirmDate.Name = "dateWHSpearateConfirmDate";
            this.dateWHSpearateConfirmDate.ReadOnly = true;
            this.dateWHSpearateConfirmDate.Size = new System.Drawing.Size(130, 23);
            this.dateWHSpearateConfirmDate.TabIndex = 14;
            // 
            // dateShippingSeparateConfirmDate
            // 
            this.dateShippingSeparateConfirmDate.IsSupportEditMode = false;
            this.dateShippingSeparateConfirmDate.Location = new System.Drawing.Point(771, 36);
            this.dateShippingSeparateConfirmDate.Name = "dateShippingSeparateConfirmDate";
            this.dateShippingSeparateConfirmDate.ReadOnly = true;
            this.dateShippingSeparateConfirmDate.Size = new System.Drawing.Size(130, 23);
            this.dateShippingSeparateConfirmDate.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(9, 97);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(125, 23);
            this.label9.TabIndex = 15;
            this.label9.Text = "Transfer WK List";
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Location = new System.Drawing.Point(9, 320);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(125, 23);
            this.label10.TabIndex = 16;
            this.label10.Text = "Packing List";
            this.label10.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // gridTransferWKList
            // 
            this.gridTransferWKList.AllowUserToAddRows = false;
            this.gridTransferWKList.AllowUserToDeleteRows = false;
            this.gridTransferWKList.AllowUserToResizeRows = false;
            this.gridTransferWKList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridTransferWKList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridTransferWKList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridTransferWKList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTransferWKList.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridTransferWKList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridTransferWKList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridTransferWKList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridTransferWKList.Location = new System.Drawing.Point(9, 123);
            this.gridTransferWKList.Name = "gridTransferWKList";
            this.gridTransferWKList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridTransferWKList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridTransferWKList.RowTemplate.Height = 24;
            this.gridTransferWKList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridTransferWKList.ShowCellToolTips = false;
            this.gridTransferWKList.Size = new System.Drawing.Size(1088, 181);
            this.gridTransferWKList.TabIndex = 17;
            this.gridTransferWKList.SelectionChanged += new System.EventHandler(this.GridTransferWKList_SelectionChanged);
            // 
            // gridPackingList
            // 
            this.gridPackingList.AllowUserToAddRows = false;
            this.gridPackingList.AllowUserToDeleteRows = false;
            this.gridPackingList.AllowUserToResizeRows = false;
            this.gridPackingList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridPackingList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridPackingList.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridPackingList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPackingList.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridPackingList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridPackingList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridPackingList.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridPackingList.Location = new System.Drawing.Point(9, 346);
            this.gridPackingList.Name = "gridPackingList";
            this.gridPackingList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridPackingList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridPackingList.RowTemplate.Height = 24;
            this.gridPackingList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPackingList.ShowCellToolTips = false;
            this.gridPackingList.Size = new System.Drawing.Size(1088, 209);
            this.gridPackingList.TabIndex = 18;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(1002, 561);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(92, 30);
            this.btnClose.TabIndex = 19;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnSeparateConfirm
            // 
            this.btnSeparateConfirm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSeparateConfirm.Location = new System.Drawing.Point(842, 561);
            this.btnSeparateConfirm.Name = "btnSeparateConfirm";
            this.btnSeparateConfirm.Size = new System.Drawing.Size(154, 30);
            this.btnSeparateConfirm.TabIndex = 21;
            this.btnSeparateConfirm.Text = "WH Separate Confirm";
            this.btnSeparateConfirm.UseVisualStyleBackColor = true;
            this.btnSeparateConfirm.Click += new System.EventHandler(this.BtnSeparateConfirm_Click);
            // 
            // TK_SeparateHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1106, 603);
            this.Controls.Add(this.btnSeparateConfirm);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gridPackingList);
            this.Controls.Add(this.gridTransferWKList);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.dateShippingSeparateConfirmDate);
            this.Controls.Add(this.dateWHSpearateConfirmDate);
            this.Controls.Add(this.dateTPESeparateApprovedDate);
            this.Controls.Add(this.dateFtyRequestSeparateDate);
            this.Controls.Add(this.displayGroupNewID);
            this.Controls.Add(this.lblNumberNewTK);
            this.Controls.Add(this.comboNewID);
            this.Controls.Add(this.displayID);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "TK_SeparateHistory";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "TK Separate History";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.displayID, 0);
            this.Controls.SetChildIndex(this.comboNewID, 0);
            this.Controls.SetChildIndex(this.lblNumberNewTK, 0);
            this.Controls.SetChildIndex(this.displayGroupNewID, 0);
            this.Controls.SetChildIndex(this.dateFtyRequestSeparateDate, 0);
            this.Controls.SetChildIndex(this.dateTPESeparateApprovedDate, 0);
            this.Controls.SetChildIndex(this.dateWHSpearateConfirmDate, 0);
            this.Controls.SetChildIndex(this.dateShippingSeparateConfirmDate, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.gridTransferWKList, 0);
            this.Controls.SetChildIndex(this.gridPackingList, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnSeparateConfirm, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridTransferWKList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridPackingList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Win.UI.Label label8;
        private Win.UI.DisplayBox displayID;
        private Win.UI.ComboBox comboNewID;
        private Win.UI.Label lblNumberNewTK;
        private Win.UI.DisplayBox displayGroupNewID;
        private Win.UI.DateBox dateFtyRequestSeparateDate;
        private Win.UI.DateBox dateTPESeparateApprovedDate;
        private Win.UI.DateBox dateWHSpearateConfirmDate;
        private Win.UI.DateBox dateShippingSeparateConfirmDate;
        private Win.UI.Label label9;
        private Win.UI.Label label10;
        private Win.UI.Grid gridTransferWKList;
        private Win.UI.Grid gridPackingList;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnSeparateConfirm;
    }
}