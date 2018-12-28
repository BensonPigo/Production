namespace Sci.Production.Warehouse
{
    partial class P10_PrintBarcode
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
            this.lbl_issueID = new Sci.Win.UI.Label();
            this.txtIssueIdFrom = new Sci.Win.UI.TextBox();
            this.txtIssueIdTo = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(316, 12);
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(161, 70);
            this.toexcel.Visible = false;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(316, 48);
            // 
            // lbl_issueID
            // 
            this.lbl_issueID.Location = new System.Drawing.Point(9, 19);
            this.lbl_issueID.Name = "lbl_issueID";
            this.lbl_issueID.Size = new System.Drawing.Size(64, 23);
            this.lbl_issueID.TabIndex = 94;
            this.lbl_issueID.Text = "Issue ID";
            // 
            // txtIssueIdFrom
            // 
            this.txtIssueIdFrom.BackColor = System.Drawing.Color.White;
            this.txtIssueIdFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtIssueIdFrom.Location = new System.Drawing.Point(76, 19);
            this.txtIssueIdFrom.Name = "txtIssueIdFrom";
            this.txtIssueIdFrom.Size = new System.Drawing.Size(100, 23);
            this.txtIssueIdFrom.TabIndex = 95;
            // 
            // txtIssueIdTo
            // 
            this.txtIssueIdTo.BackColor = System.Drawing.Color.White;
            this.txtIssueIdTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtIssueIdTo.Location = new System.Drawing.Point(199, 19);
            this.txtIssueIdTo.Name = "txtIssueIdTo";
            this.txtIssueIdTo.Size = new System.Drawing.Size(100, 23);
            this.txtIssueIdTo.TabIndex = 96;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(179, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 23);
            this.label1.TabIndex = 97;
            this.label1.Text = "~";
            this.label1.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // P10_PrintBarcode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 130);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtIssueIdTo);
            this.Controls.Add(this.txtIssueIdFrom);
            this.Controls.Add(this.lbl_issueID);
            this.Name = "P10_PrintBarcode";
            this.Text = "P10_PrintBarcode";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.lbl_issueID, 0);
            this.Controls.SetChildIndex(this.txtIssueIdFrom, 0);
            this.Controls.SetChildIndex(this.txtIssueIdTo, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lbl_issueID;
        private Win.UI.TextBox txtIssueIdFrom;
        private Win.UI.TextBox txtIssueIdTo;
        private Win.UI.Label label1;
    }
}