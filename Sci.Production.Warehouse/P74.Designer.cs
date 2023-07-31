namespace Sci.Production.Warehouse
{
    partial class P74
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
            this.txtTransaction = new Sci.Win.UI.TextBox();
            this.lblTransactionID = new Sci.Win.UI.Label();
            this.lblPreparationRecord = new Sci.Win.UI.Label();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.lblFinishedandReceoved = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.txtFinished = new Sci.Win.UI.TextBox();
            this.cbPreparation = new Sci.Win.UI.ComboBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.txtRequset = new Sci.Win.UI.TextBox();
            this.label4 = new Sci.Win.UI.Label();
            this.txtStatus = new Sci.Win.UI.TextBox();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.txtType = new Sci.Win.UI.TextBox();
            this.txtApvDate = new Sci.Win.UI.TextBox();
            this.txtDepartment = new Sci.Win.UI.TextBox();
            this.dtIssueDate = new Sci.Win.UI.DateBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtFinishedBy = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.txtFinishedDate = new Sci.Win.UI.TextBox();
            this.label8 = new Sci.Win.UI.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtReceivedBy = new Sci.Win.UI.TextBox();
            this.label11 = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
            this.txtReceivedDate = new Sci.Win.UI.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtTransaction
            // 
            this.txtTransaction.BackColor = System.Drawing.Color.White;
            this.txtTransaction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTransaction.IsSupportEditMode = false;
            this.txtTransaction.Location = new System.Drawing.Point(142, 41);
            this.txtTransaction.Name = "txtTransaction";
            this.txtTransaction.Size = new System.Drawing.Size(278, 23);
            this.txtTransaction.TabIndex = 2;
            this.txtTransaction.Validating += new System.ComponentModel.CancelEventHandler(this.TxtTransaction_Validating);
            // 
            // lblTransactionID
            // 
            this.lblTransactionID.Location = new System.Drawing.Point(10, 41);
            this.lblTransactionID.Name = "lblTransactionID";
            this.lblTransactionID.Size = new System.Drawing.Size(129, 23);
            this.lblTransactionID.TabIndex = 70;
            this.lblTransactionID.Text = "Transaction ID";
            // 
            // lblPreparationRecord
            // 
            this.lblPreparationRecord.Location = new System.Drawing.Point(8, 12);
            this.lblPreparationRecord.Name = "lblPreparationRecord";
            this.lblPreparationRecord.Size = new System.Drawing.Size(130, 23);
            this.lblPreparationRecord.TabIndex = 69;
            this.lblPreparationRecord.Text = "Preparation Record";
            // 
            // lineShape1
            // 
            this.lineShape1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lineShape1.BorderWidth = 10;
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.SelectionColor = System.Drawing.SystemColors.InfoText;
            this.lineShape1.X1 = 15;
            this.lineShape1.X2 = 750;
            this.lineShape1.Y1 = 250;
            this.lineShape1.Y2 = 137;
            // 
            // lblFinishedandReceoved
            // 
            this.lblFinishedandReceoved.Location = new System.Drawing.Point(10, 70);
            this.lblFinishedandReceoved.Name = "lblFinishedandReceoved";
            this.lblFinishedandReceoved.Size = new System.Drawing.Size(129, 23);
            this.lblFinishedandReceoved.TabIndex = 70;
            this.lblFinishedandReceoved.Text = "Finished By";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 128);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 23);
            this.label1.TabIndex = 70;
            this.label1.Text = "Issue Date";
            // 
            // txtFinished
            // 
            this.txtFinished.BackColor = System.Drawing.Color.White;
            this.txtFinished.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFinished.IsSupportEditMode = false;
            this.txtFinished.Location = new System.Drawing.Point(142, 70);
            this.txtFinished.Name = "txtFinished";
            this.txtFinished.Size = new System.Drawing.Size(278, 23);
            this.txtFinished.TabIndex = 3;
            // 
            // cbPreparation
            // 
            this.cbPreparation.BackColor = System.Drawing.Color.White;
            this.cbPreparation.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.cbPreparation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbPreparation.FormattingEnabled = true;
            this.cbPreparation.IsSupportUnselect = true;
            this.cbPreparation.Location = new System.Drawing.Point(141, 12);
            this.cbPreparation.Name = "cbPreparation";
            this.cbPreparation.OldText = "";
            this.cbPreparation.Size = new System.Drawing.Size(278, 24);
            this.cbPreparation.TabIndex = 1;
            this.cbPreparation.SelectedIndexChanged += new System.EventHandler(this.CbPreparation_SelectedIndexChanged);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(440, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(93, 28);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label2.Location = new System.Drawing.Point(8, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(571, 2);
            this.label2.TabIndex = 74;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 23);
            this.label3.TabIndex = 70;
            this.label3.Text = "Requset#";
            // 
            // txtRequset
            // 
            this.txtRequset.BackColor = System.Drawing.Color.White;
            this.txtRequset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRequset.Location = new System.Drawing.Point(142, 102);
            this.txtRequset.Name = "txtRequset";
            this.txtRequset.Size = new System.Drawing.Size(109, 23);
            this.txtRequset.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(10, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 23);
            this.label4.TabIndex = 70;
            this.label4.Text = "Status";
            // 
            // txtStatus
            // 
            this.txtStatus.BackColor = System.Drawing.Color.White;
            this.txtStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtStatus.Location = new System.Drawing.Point(142, 154);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.Size = new System.Drawing.Size(109, 23);
            this.txtStatus.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(270, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(129, 23);
            this.label5.TabIndex = 70;
            this.label5.Text = "Apv. Date";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(270, 154);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(129, 23);
            this.label6.TabIndex = 70;
            this.label6.Text = "Department";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(270, 102);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(129, 23);
            this.label7.TabIndex = 70;
            this.label7.Text = "Type";
            // 
            // txtType
            // 
            this.txtType.BackColor = System.Drawing.Color.White;
            this.txtType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtType.Location = new System.Drawing.Point(402, 102);
            this.txtType.Name = "txtType";
            this.txtType.Size = new System.Drawing.Size(167, 23);
            this.txtType.TabIndex = 8;
            // 
            // txtApvDate
            // 
            this.txtApvDate.BackColor = System.Drawing.Color.White;
            this.txtApvDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtApvDate.Location = new System.Drawing.Point(402, 128);
            this.txtApvDate.Name = "txtApvDate";
            this.txtApvDate.Size = new System.Drawing.Size(167, 23);
            this.txtApvDate.TabIndex = 9;
            // 
            // txtDepartment
            // 
            this.txtDepartment.BackColor = System.Drawing.Color.White;
            this.txtDepartment.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDepartment.Location = new System.Drawing.Point(402, 154);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.Size = new System.Drawing.Size(167, 23);
            this.txtDepartment.TabIndex = 10;
            // 
            // dtIssueDate
            // 
            this.dtIssueDate.IsSupportCalendar = false;
            this.dtIssueDate.IsSupportEditMode = false;
            this.dtIssueDate.Location = new System.Drawing.Point(142, 128);
            this.dtIssueDate.Name = "dtIssueDate";
            this.dtIssueDate.ReadOnly = true;
            this.dtIssueDate.Size = new System.Drawing.Size(110, 23);
            this.dtIssueDate.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtFinishedBy);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtFinishedDate);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Location = new System.Drawing.Point(10, 183);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(277, 103);
            this.groupBox1.TabIndex = 78;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Warehouse Finished Preparation";
            // 
            // txtFinishedBy
            // 
            this.txtFinishedBy.BackColor = System.Drawing.Color.White;
            this.txtFinishedBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFinishedBy.Location = new System.Drawing.Point(105, 66);
            this.txtFinishedBy.Name = "txtFinishedBy";
            this.txtFinishedBy.Size = new System.Drawing.Size(166, 23);
            this.txtFinishedBy.TabIndex = 12;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(6, 66);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(96, 23);
            this.label9.TabIndex = 82;
            this.label9.Text = "Finished By";
            // 
            // txtFinishedDate
            // 
            this.txtFinishedDate.BackColor = System.Drawing.Color.White;
            this.txtFinishedDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFinishedDate.Location = new System.Drawing.Point(105, 32);
            this.txtFinishedDate.Name = "txtFinishedDate";
            this.txtFinishedDate.Size = new System.Drawing.Size(166, 23);
            this.txtFinishedDate.TabIndex = 11;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 32);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 23);
            this.label8.TabIndex = 80;
            this.label8.Text = "Finished Date";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtReceivedBy);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtReceivedDate);
            this.groupBox2.Location = new System.Drawing.Point(293, 183);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(282, 103);
            this.groupBox2.TabIndex = 79;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Factory Received";
            // 
            // txtReceivedBy
            // 
            this.txtReceivedBy.BackColor = System.Drawing.Color.White;
            this.txtReceivedBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtReceivedBy.Location = new System.Drawing.Point(109, 66);
            this.txtReceivedBy.Name = "txtReceivedBy";
            this.txtReceivedBy.Size = new System.Drawing.Size(167, 23);
            this.txtReceivedBy.TabIndex = 14;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(10, 32);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(96, 23);
            this.label11.TabIndex = 84;
            this.label11.Text = "Received Date";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(10, 66);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(96, 23);
            this.label10.TabIndex = 86;
            this.label10.Text = "Received By";
            // 
            // txtReceivedDate
            // 
            this.txtReceivedDate.BackColor = System.Drawing.Color.White;
            this.txtReceivedDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtReceivedDate.Location = new System.Drawing.Point(109, 32);
            this.txtReceivedDate.Name = "txtReceivedDate";
            this.txtReceivedDate.Size = new System.Drawing.Size(167, 23);
            this.txtReceivedDate.TabIndex = 13;
            // 
            // P74
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(587, 295);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dtIssueDate);
            this.Controls.Add(this.txtDepartment);
            this.Controls.Add(this.txtStatus);
            this.Controls.Add(this.txtApvDate);
            this.Controls.Add(this.txtType);
            this.Controls.Add(this.txtRequset);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.cbPreparation);
            this.Controls.Add(this.txtFinished);
            this.Controls.Add(this.txtTransaction);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblFinishedandReceoved);
            this.Controls.Add(this.lblTransactionID);
            this.Controls.Add(this.lblPreparationRecord);
            this.Name = "P74";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P74.  Issue Lacking & Replacement Preparation Record";
            this.Controls.SetChildIndex(this.lblPreparationRecord, 0);
            this.Controls.SetChildIndex(this.lblTransactionID, 0);
            this.Controls.SetChildIndex(this.lblFinishedandReceoved, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.txtTransaction, 0);
            this.Controls.SetChildIndex(this.txtFinished, 0);
            this.Controls.SetChildIndex(this.cbPreparation, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtRequset, 0);
            this.Controls.SetChildIndex(this.txtType, 0);
            this.Controls.SetChildIndex(this.txtApvDate, 0);
            this.Controls.SetChildIndex(this.txtStatus, 0);
            this.Controls.SetChildIndex(this.txtDepartment, 0);
            this.Controls.SetChildIndex(this.dtIssueDate, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            this.Controls.SetChildIndex(this.groupBox2, 0);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion
        
        private Win.UI.TextBox txtTransaction;
        private Win.UI.Label lblTransactionID;
        private Win.UI.Label lblPreparationRecord;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private Win.UI.Label lblFinishedandReceoved;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtFinished;
        private Win.UI.ComboBox cbPreparation;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label2;
        private Win.UI.Label label3;
        private Win.UI.TextBox txtRequset;
        private Win.UI.Label label4;
        private Win.UI.TextBox txtStatus;
        private Win.UI.Label label5;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Win.UI.TextBox txtType;
        private Win.UI.TextBox txtApvDate;
        private Win.UI.TextBox txtDepartment;
        private Win.UI.DateBox dtIssueDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private Win.UI.TextBox txtFinishedBy;
        private Win.UI.Label label9;
        private Win.UI.TextBox txtFinishedDate;
        private Win.UI.Label label8;
        private System.Windows.Forms.GroupBox groupBox2;
        private Win.UI.TextBox txtReceivedBy;
        private Win.UI.Label label11;
        private Win.UI.Label label10;
        private Win.UI.TextBox txtReceivedDate;
    }
}