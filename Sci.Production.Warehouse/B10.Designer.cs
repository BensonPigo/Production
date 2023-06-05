namespace Sci.Production.Warehouse
{
    partial class B10
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
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.labelID = new Sci.Win.UI.Label();
            this.txtCode = new Sci.Win.UI.TextBox();
            this.txtDesc = new Sci.Win.UI.TextBox();
            this.lblDesc = new Sci.Win.UI.Label();
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
            this.detail.Size = new System.Drawing.Size(890, 411);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtDesc);
            this.detailcont.Controls.Add(this.lblDesc);
            this.detailcont.Controls.Add(this.txtCode);
            this.detailcont.Controls.Add(this.labelID);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Size = new System.Drawing.Size(890, 373);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 373);
            this.detailbtm.Size = new System.Drawing.Size(890, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(890, 411);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(898, 440);
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(243, 16);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 2;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(14, 14);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(53, 23);
            this.labelID.TabIndex = 5;
            this.labelID.Text = "Code";
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "id", true));
            this.txtCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCode.Location = new System.Drawing.Point(70, 14);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(167, 23);
            this.txtCode.TabIndex = 8;
            // 
            // txtDesc
            // 
            this.txtDesc.BackColor = System.Drawing.Color.White;
            this.txtDesc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.txtDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDesc.Location = new System.Drawing.Point(70, 51);
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(384, 23);
            this.txtDesc.TabIndex = 10;
            // 
            // lblDesc
            // 
            this.lblDesc.Location = new System.Drawing.Point(14, 51);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(53, 23);
            this.lblDesc.TabIndex = 9;
            this.lblDesc.Text = "Desc";
            // 
            // B10
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 473);
            this.EnableGridJunkColor = true;
            this.ExpressQuery = true;
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.Name = "B10";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B10. Issue to place";
            this.WorkAlias = "WHIssueToPlace";
            this.FormLoaded += new System.EventHandler(this.B10_FormLoaded);
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

        private Win.UI.CheckBox checkJunk;
        private Win.UI.Label labelID;
        private Win.UI.TextBox txtCode;
        private Win.UI.TextBox txtDesc;
        private Win.UI.Label lblDesc;
    }
}