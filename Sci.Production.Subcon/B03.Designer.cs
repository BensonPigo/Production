namespace Sci.Production.Subcon
{
    partial class B03
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
            this.chk_junk = new Sci.Win.UI.CheckBox();
            this.txtReason = new Sci.Win.UI.TextBox();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.label3 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(792, 388);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.chk_junk);
            this.detailcont.Controls.Add(this.txtReason);
            this.detailcont.Controls.Add(this.displayID);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Size = new System.Drawing.Size(792, 350);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 350);
            this.detailbtm.Size = new System.Drawing.Size(792, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(792, 388);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(800, 417);
            // 
            // chk_junk
            // 
            this.chk_junk.AutoSize = true;
            this.chk_junk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.chk_junk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chk_junk.Location = new System.Drawing.Point(247, 30);
            this.chk_junk.Name = "chk_junk";
            this.chk_junk.Size = new System.Drawing.Size(57, 21);
            this.chk_junk.TabIndex = 2;
            this.chk_junk.Text = "Junk";
            this.chk_junk.UseVisualStyleBackColor = true;
            // 
            // txtReason
            // 
            this.txtReason.BackColor = System.Drawing.Color.White;
            this.txtReason.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Reason", true));
            this.txtReason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtReason.Location = new System.Drawing.Point(124, 70);
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(452, 23);
            this.txtReason.TabIndex = 3;
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(124, 30);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(100, 23);
            this.displayID.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(36, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 23);
            this.label3.TabIndex = 9;
            this.label3.Text = "Reason";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(36, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 23);
            this.label1.TabIndex = 7;
            this.label1.Text = "ID";
            // 
            // B03
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.DefaultFilter = "Type=\'WR\'";
            this.IsDeleteOnBrowse = false;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B03";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B03. Reason for 2nd PO without requisition";
            this.WorkAlias = "SubconReason";
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

        private Win.UI.CheckBox chk_junk;
        private Win.UI.TextBox txtReason;
        private Win.UI.DisplayBox displayID;
        private Win.UI.Label label3;
        private Win.UI.Label label1;
    }
}