namespace Sci.Production.Warehouse
{
    partial class P21
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblReceivingID = new Sci.Win.UI.Label();
            this.lblWK = new Sci.Win.UI.Label();
            this.lblArriveWHDate = new Sci.Win.UI.Label();
            this.lblSP = new Sci.Win.UI.Label();
            this.txtRecivingID = new Sci.Win.UI.TextBox();
            this.txtWK = new Sci.Win.UI.TextBox();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.dateBoxArriveWH = new Sci.Win.UI.DateBox();
            this.btnQuery = new Sci.Win.UI.Button();
            this.gridReceiving = new Sci.Win.UI.Grid();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnUpdate = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridReceiving)).BeginInit();
            this.SuspendLayout();
            // 
            // lblReceivingID
            // 
            this.lblReceivingID.Location = new System.Drawing.Point(9, 9);
            this.lblReceivingID.Name = "lblReceivingID";
            this.lblReceivingID.Size = new System.Drawing.Size(90, 23);
            this.lblReceivingID.TabIndex = 1;
            this.lblReceivingID.Text = "Receiving ID";
            // 
            // lblWK
            // 
            this.lblWK.Location = new System.Drawing.Point(9, 43);
            this.lblWK.Name = "lblWK";
            this.lblWK.Size = new System.Drawing.Size(90, 23);
            this.lblWK.TabIndex = 2;
            this.lblWK.Text = "WK#";
            // 
            // lblArriveWHDate
            // 
            this.lblArriveWHDate.Location = new System.Drawing.Point(251, 9);
            this.lblArriveWHDate.Name = "lblArriveWHDate";
            this.lblArriveWHDate.Size = new System.Drawing.Size(107, 23);
            this.lblArriveWHDate.TabIndex = 3;
            this.lblArriveWHDate.Text = "Arrive W/H Date";
            // 
            // lblSP
            // 
            this.lblSP.Location = new System.Drawing.Point(251, 43);
            this.lblSP.Name = "lblSP";
            this.lblSP.Size = new System.Drawing.Size(107, 23);
            this.lblSP.TabIndex = 4;
            this.lblSP.Text = "SP#";
            // 
            // txtRecivingID
            // 
            this.txtRecivingID.BackColor = System.Drawing.Color.White;
            this.txtRecivingID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRecivingID.Location = new System.Drawing.Point(102, 9);
            this.txtRecivingID.Name = "txtRecivingID";
            this.txtRecivingID.Size = new System.Drawing.Size(125, 23);
            this.txtRecivingID.TabIndex = 5;
            // 
            // txtWK
            // 
            this.txtWK.BackColor = System.Drawing.Color.White;
            this.txtWK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtWK.Location = new System.Drawing.Point(102, 43);
            this.txtWK.Name = "txtWK";
            this.txtWK.Size = new System.Drawing.Size(125, 23);
            this.txtWK.TabIndex = 6;
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(361, 43);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(125, 23);
            this.txtSP.TabIndex = 7;
            // 
            // dateBoxArriveWH
            // 
            this.dateBoxArriveWH.Location = new System.Drawing.Point(361, 9);
            this.dateBoxArriveWH.Name = "dateBoxArriveWH";
            this.dateBoxArriveWH.Size = new System.Drawing.Size(130, 23);
            this.dateBoxArriveWH.TabIndex = 8;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(916, 5);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 9;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // gridReceiving
            // 
            this.gridReceiving.AllowUserToAddRows = false;
            this.gridReceiving.AllowUserToDeleteRows = false;
            this.gridReceiving.AllowUserToResizeRows = false;
            this.gridReceiving.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridReceiving.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridReceiving.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridReceiving.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridReceiving.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridReceiving.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridReceiving.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridReceiving.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridReceiving.Location = new System.Drawing.Point(9, 76);
            this.gridReceiving.Name = "gridReceiving";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridReceiving.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridReceiving.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridReceiving.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridReceiving.RowTemplate.Height = 24;
            this.gridReceiving.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridReceiving.ShowCellToolTips = false;
            this.gridReceiving.Size = new System.Drawing.Size(987, 339);
            this.gridReceiving.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridReceiving.TabIndex = 10;
            this.gridReceiving.Sorted += new System.EventHandler(this.gridReceiving_Sorted);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(916, 423);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Location = new System.Drawing.Point(817, 423);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(80, 30);
            this.btnUpdate.TabIndex = 12;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // P21
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 462);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gridReceiving);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.dateBoxArriveWH);
            this.Controls.Add(this.txtSP);
            this.Controls.Add(this.txtWK);
            this.Controls.Add(this.txtRecivingID);
            this.Controls.Add(this.lblSP);
            this.Controls.Add(this.lblArriveWHDate);
            this.Controls.Add(this.lblWK);
            this.Controls.Add(this.lblReceivingID);
            this.Name = "P21";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P21. Batch update receiving Act.(kg) and Location";
            this.Controls.SetChildIndex(this.lblReceivingID, 0);
            this.Controls.SetChildIndex(this.lblWK, 0);
            this.Controls.SetChildIndex(this.lblArriveWHDate, 0);
            this.Controls.SetChildIndex(this.lblSP, 0);
            this.Controls.SetChildIndex(this.txtRecivingID, 0);
            this.Controls.SetChildIndex(this.txtWK, 0);
            this.Controls.SetChildIndex(this.txtSP, 0);
            this.Controls.SetChildIndex(this.dateBoxArriveWH, 0);
            this.Controls.SetChildIndex(this.btnQuery, 0);
            this.Controls.SetChildIndex(this.gridReceiving, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnUpdate, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridReceiving)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lblReceivingID;
        private Win.UI.Label lblWK;
        private Win.UI.Label lblArriveWHDate;
        private Win.UI.Label lblSP;
        private Win.UI.TextBox txtRecivingID;
        private Win.UI.TextBox txtWK;
        private Win.UI.TextBox txtSP;
        private Win.UI.DateBox dateBoxArriveWH;
        private Win.UI.Button btnQuery;
        private Win.UI.Grid gridReceiving;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnUpdate;
    }
}