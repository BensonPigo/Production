namespace Sci.Production.Centralized
{
    partial class Shipping_P06_History
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
            this.gridShippingHistory = new Sci.Win.UI.Grid();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.txtCentralizedmulitM = new Sci.Production.Class.TxtCentralizedmulitM();
            this.txtShippingReason = new Sci.Production.Class.TxtShippingReason();
            this.txtUnlocker = new Sci.Win.UI.TextBox();
            this.displayUnlockerName = new Sci.Win.UI.DisplayBox();
            this.txtPulloutID = new Sci.Win.UI.TextBox();
            this.dateRangeUnlock = new Sci.Win.UI.DateRange();
            this.btnQuery = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridShippingHistory)).BeginInit();
            this.SuspendLayout();
            // 
            // gridShippingHistory
            // 
            this.gridShippingHistory.AllowUserToAddRows = false;
            this.gridShippingHistory.AllowUserToDeleteRows = false;
            this.gridShippingHistory.AllowUserToResizeRows = false;
            this.gridShippingHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridShippingHistory.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridShippingHistory.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridShippingHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridShippingHistory.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridShippingHistory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridShippingHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridShippingHistory.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridShippingHistory.Location = new System.Drawing.Point(12, 71);
            this.gridShippingHistory.Name = "gridShippingHistory";
            this.gridShippingHistory.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridShippingHistory.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridShippingHistory.RowTemplate.Height = 24;
            this.gridShippingHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridShippingHistory.ShowCellToolTips = false;
            this.gridShippingHistory.Size = new System.Drawing.Size(865, 361);
            this.gridShippingHistory.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "M";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Unlocker";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(200, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "Pullout ID";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(414, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 23);
            this.label4.TabIndex = 5;
            this.label4.Text = "Reason";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(414, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 23);
            this.label5.TabIndex = 6;
            this.label5.Text = "Unlock Date";
            // 
            // txtCentralizedmulitM
            // 
            this.txtCentralizedmulitM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtCentralizedmulitM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtCentralizedmulitM.Location = new System.Drawing.Point(90, 9);
            this.txtCentralizedmulitM.Name = "txtCentralizedmulitM";
            this.txtCentralizedmulitM.ReadOnly = true;
            this.txtCentralizedmulitM.Size = new System.Drawing.Size(104, 23);
            this.txtCentralizedmulitM.TabIndex = 7;
            // 
            // txtShippingReason
            // 
            this.txtShippingReason.DisplayBox1Binding = "";
            this.txtShippingReason.LinkDB = "Production";
            this.txtShippingReason.Location = new System.Drawing.Point(502, 7);
            this.txtShippingReason.Name = "txtShippingReason";
            this.txtShippingReason.Size = new System.Drawing.Size(280, 27);
            this.txtShippingReason.TabIndex = 8;
            this.txtShippingReason.TextBox1Binding = "";
            this.txtShippingReason.Type = null;
            // 
            // txtUnlocker
            // 
            this.txtUnlocker.BackColor = System.Drawing.Color.White;
            this.txtUnlocker.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtUnlocker.Location = new System.Drawing.Point(90, 39);
            this.txtUnlocker.Name = "txtUnlocker";
            this.txtUnlocker.Size = new System.Drawing.Size(104, 23);
            this.txtUnlocker.TabIndex = 9;
            this.txtUnlocker.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtUnlocker_PopUp);
            this.txtUnlocker.Validating += new System.ComponentModel.CancelEventHandler(this.TxtUnlocker_Validating);
            // 
            // displayUnlockerName
            // 
            this.displayUnlockerName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayUnlockerName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayUnlockerName.Location = new System.Drawing.Point(200, 39);
            this.displayUnlockerName.Name = "displayUnlockerName";
            this.displayUnlockerName.Size = new System.Drawing.Size(211, 23);
            this.displayUnlockerName.TabIndex = 10;
            // 
            // txtPulloutID
            // 
            this.txtPulloutID.BackColor = System.Drawing.Color.White;
            this.txtPulloutID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPulloutID.Location = new System.Drawing.Point(278, 9);
            this.txtPulloutID.Name = "txtPulloutID";
            this.txtPulloutID.Size = new System.Drawing.Size(133, 23);
            this.txtPulloutID.TabIndex = 11;
            // 
            // dateRangeUnlock
            // 
            // 
            // 
            // 
            this.dateRangeUnlock.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeUnlock.DateBox1.Name = "";
            this.dateRangeUnlock.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeUnlock.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeUnlock.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeUnlock.DateBox2.Name = "";
            this.dateRangeUnlock.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeUnlock.DateBox2.TabIndex = 1;
            this.dateRangeUnlock.IsRequired = false;
            this.dateRangeUnlock.Location = new System.Drawing.Point(502, 39);
            this.dateRangeUnlock.Name = "dateRangeUnlock";
            this.dateRangeUnlock.Size = new System.Drawing.Size(280, 23);
            this.dateRangeUnlock.TabIndex = 12;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(799, 5);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 13;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // Shipping_P06_History
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(889, 444);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.dateRangeUnlock);
            this.Controls.Add(this.txtPulloutID);
            this.Controls.Add(this.displayUnlockerName);
            this.Controls.Add(this.txtUnlocker);
            this.Controls.Add(this.txtShippingReason);
            this.Controls.Add(this.txtCentralizedmulitM);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gridShippingHistory);
            this.Name = "Shipping_P06_History";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "Shipping_P06_History";
            this.Controls.SetChildIndex(this.gridShippingHistory, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.txtCentralizedmulitM, 0);
            this.Controls.SetChildIndex(this.txtShippingReason, 0);
            this.Controls.SetChildIndex(this.txtUnlocker, 0);
            this.Controls.SetChildIndex(this.displayUnlockerName, 0);
            this.Controls.SetChildIndex(this.txtPulloutID, 0);
            this.Controls.SetChildIndex(this.dateRangeUnlock, 0);
            this.Controls.SetChildIndex(this.btnQuery, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridShippingHistory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Grid gridShippingHistory;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Class.TxtCentralizedmulitM txtCentralizedmulitM;
        private Class.TxtShippingReason txtShippingReason;
        private Win.UI.TextBox txtUnlocker;
        private Win.UI.DisplayBox displayUnlockerName;
        private Win.UI.TextBox txtPulloutID;
        private Win.UI.DateRange dateRangeUnlock;
        private Win.UI.Button btnQuery;
    }
}