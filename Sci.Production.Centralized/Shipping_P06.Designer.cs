namespace Sci.Production.Centralized
{
    partial class Shipping_P06
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
            this.txtPulloutID = new Sci.Win.UI.TextBox();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.btnUnlock = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnHistory = new Sci.Win.UI.Button();
            this.txtShippingReason = new Sci.Production.Class.TxtShippingReason();
            this.comboRegion = new Sci.Win.UI.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Region";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "PullOut ID";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Reason";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "Remark";
            // 
            // txtPulloutID
            // 
            this.txtPulloutID.BackColor = System.Drawing.Color.White;
            this.txtPulloutID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPulloutID.Location = new System.Drawing.Point(87, 37);
            this.txtPulloutID.MaxLength = 13;
            this.txtPulloutID.Name = "txtPulloutID";
            this.txtPulloutID.Size = new System.Drawing.Size(137, 23);
            this.txtPulloutID.TabIndex = 6;
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(87, 93);
            this.txtRemark.MaxLength = 300;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(295, 23);
            this.txtRemark.TabIndex = 7;
            // 
            // btnUnlock
            // 
            this.btnUnlock.Location = new System.Drawing.Point(131, 135);
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new System.Drawing.Size(80, 30);
            this.btnUnlock.TabIndex = 8;
            this.btnUnlock.Text = "Unlock";
            this.btnUnlock.UseVisualStyleBackColor = true;
            this.btnUnlock.Click += new System.EventHandler(this.BtnUnlock_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(248, 135);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnHistory
            // 
            this.btnHistory.Location = new System.Drawing.Point(248, 5);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(80, 30);
            this.btnHistory.TabIndex = 10;
            this.btnHistory.Text = "History";
            this.btnHistory.UseVisualStyleBackColor = true;
            this.btnHistory.Click += new System.EventHandler(this.BtnHistory_Click);
            // 
            // txtShippingReason
            // 
            this.txtShippingReason.DisplayBox1Binding = "";
            this.txtShippingReason.LinkDB = "Production";
            this.txtShippingReason.Location = new System.Drawing.Point(87, 63);
            this.txtShippingReason.Name = "txtShippingReason";
            this.txtShippingReason.Size = new System.Drawing.Size(296, 27);
            this.txtShippingReason.TabIndex = 11;
            this.txtShippingReason.TextBox1Binding = "";
            this.txtShippingReason.Type = null;
            // 
            // comboRegion
            // 
            this.comboRegion.BackColor = System.Drawing.Color.White;
            this.comboRegion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboRegion.FormattingEnabled = true;
            this.comboRegion.IsSupportUnselect = true;
            this.comboRegion.Location = new System.Drawing.Point(87, 8);
            this.comboRegion.Name = "comboRegion";
            this.comboRegion.OldText = "";
            this.comboRegion.Size = new System.Drawing.Size(121, 24);
            this.comboRegion.TabIndex = 12;
            // 
            // Shipping_P06
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 177);
            this.Controls.Add(this.comboRegion);
            this.Controls.Add(this.txtShippingReason);
            this.Controls.Add(this.btnHistory);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnUnlock);
            this.Controls.Add(this.txtRemark);
            this.Controls.Add(this.txtPulloutID);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Shipping_P06";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "Shipping_P06. Pullout Unlock";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.txtPulloutID, 0);
            this.Controls.SetChildIndex(this.txtRemark, 0);
            this.Controls.SetChildIndex(this.btnUnlock, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnHistory, 0);
            this.Controls.SetChildIndex(this.txtShippingReason, 0);
            this.Controls.SetChildIndex(this.comboRegion, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.TextBox txtPulloutID;
        private Win.UI.TextBox txtRemark;
        private Win.UI.Button btnUnlock;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnHistory;
        private Class.TxtShippingReason txtShippingReason;
        private Win.UI.ComboBox comboRegion;
    }
}