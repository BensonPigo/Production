namespace Sci.Production.Basic
{
    partial class B04_BankDetail
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
            this.displayCode = new Sci.Win.UI.DisplayBox();
            this.labelCode = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.chkPaybyCheck = new Sci.Win.UI.CheckBox();
            this.txtAbb = new Sci.Win.UI.TextBox();
            this.txtCode = new Sci.Win.UI.TextBox();
            this.labelStatus = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).BeginInit();
            this.masterpanel.SuspendLayout();
            this.detailpanel.SuspendLayout();
            this.detail2.SuspendLayout();
            this.detailpanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.labelStatus);
            this.masterpanel.Controls.Add(this.txtCode);
            this.masterpanel.Controls.Add(this.txtAbb);
            this.masterpanel.Controls.Add(this.chkPaybyCheck);
            this.masterpanel.Controls.Add(this.label3);
            this.masterpanel.Controls.Add(this.label2);
            this.masterpanel.Size = new System.Drawing.Size(876, 102);
            this.masterpanel.Controls.SetChildIndex(this.label2, 0);
            this.masterpanel.Controls.SetChildIndex(this.label3, 0);
            this.masterpanel.Controls.SetChildIndex(this.chkPaybyCheck, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtAbb, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtCode, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelStatus, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 102);
            this.detailpanel.Size = new System.Drawing.Size(876, 316);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(699, 67);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(876, 316);
            // 
            // detail2
            // 
            this.detail2.Size = new System.Drawing.Size(892, 387);
            // 
            // detailgridcont2
            // 
            this.detailgridcont2.Size = new System.Drawing.Size(886, 341);
            // 
            // detailpanel2
            // 
            this.detailpanel2.Size = new System.Drawing.Size(886, 40);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(876, 456);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.displayCode);
            this.detailcont.Controls.Add(this.labelCode);
            this.detailcont.Size = new System.Drawing.Size(876, 418);
            this.detailcont.Controls.SetChildIndex(this.labelCode, 0);
            this.detailcont.Controls.SetChildIndex(this.displayCode, 0);
            this.detailcont.Controls.SetChildIndex(this.masterpanel, 0);
            this.detailcont.Controls.SetChildIndex(this.detailpanel, 0);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 418);
            this.detailbtm.Size = new System.Drawing.Size(876, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(876, 456);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(884, 485);
            // 
            // displayCode
            // 
            this.displayCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCode.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCode.Location = new System.Drawing.Point(93, 32);
            this.displayCode.Name = "displayCode";
            this.displayCode.Size = new System.Drawing.Size(140, 23);
            this.displayCode.TabIndex = 65;
            // 
            // labelCode
            // 
            this.labelCode.Location = new System.Drawing.Point(47, 32);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(42, 23);
            this.labelCode.TabIndex = 64;
            this.labelCode.Text = "Code";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(24, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 23);
            this.label2.TabIndex = 32;
            this.label2.Text = "Code";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(205, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 34;
            this.label3.Text = "Abbreviation";
            // 
            // chkPaybyCheck
            // 
            this.chkPaybyCheck.AutoSize = true;
            this.chkPaybyCheck.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ByCheck", true));
            this.chkPaybyCheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkPaybyCheck.Location = new System.Drawing.Point(520, 22);
            this.chkPaybyCheck.Name = "chkPaybyCheck";
            this.chkPaybyCheck.Size = new System.Drawing.Size(113, 21);
            this.chkPaybyCheck.TabIndex = 3;
            this.chkPaybyCheck.Text = "Pay by Check";
            this.chkPaybyCheck.UseVisualStyleBackColor = true;
            this.chkPaybyCheck.CheckedChanged += new System.EventHandler(this.ChkPaybyCheck_CheckedChanged);
            // 
            // txtAbb
            // 
            this.txtAbb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtAbb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtAbb.IsSupportEditMode = false;
            this.txtAbb.Location = new System.Drawing.Point(308, 22);
            this.txtAbb.Name = "txtAbb";
            this.txtAbb.ReadOnly = true;
            this.txtAbb.Size = new System.Drawing.Size(196, 23);
            this.txtAbb.TabIndex = 2;
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtCode.IsSupportEditMode = false;
            this.txtCode.Location = new System.Drawing.Point(70, 22);
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(118, 23);
            this.txtCode.TabIndex = 1;
            // 
            // labelStatus
            // 
            this.labelStatus.BackColor = System.Drawing.Color.Transparent;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F);
            this.labelStatus.Location = new System.Drawing.Point(675, 20);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(148, 25);
            this.labelStatus.TabIndex = 57;
            this.labelStatus.Text = "status";
            this.labelStatus.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelStatus.TextStyle.BorderColor = System.Drawing.Color.Red;
            this.labelStatus.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // B04_BankDetail
            // 
            this.ApvChkValue = "New";
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 518);
            this.ExpressQuery = true;
            this.Grid2New = 0;
            this.GridAlias = "LocalSupp_Bank_Detail";
            this.IsSupportConfirm = true;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.KeyField1 = "ID,Pkey";
            this.Name = "B04_BankDetail";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "B04_BankData";
            this.WorkAlias = "LocalSupp_Bank";
            this.Controls.SetChildIndex(this.tabs, 0);
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).EndInit();
            this.masterpanel.ResumeLayout(false);
            this.masterpanel.PerformLayout();
            this.detailpanel.ResumeLayout(false);
            this.detail2.ResumeLayout(false);
            this.detailpanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.DisplayBox displayCode;
        private Win.UI.Label labelCode;
        private Win.UI.CheckBox chkPaybyCheck;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtAbb;
        private Win.UI.TextBox txtCode;
        private Win.UI.Label labelStatus;
    }
}