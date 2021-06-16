namespace Sci.Production.Logistic
{
    partial class B02
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
            this.txtReason = new Sci.Win.UI.TextBox();
            this.txtID = new Sci.Win.UI.TextBox();
            this.labelDescription = new Sci.Win.UI.Label();
            this.labelCode = new Sci.Win.UI.Label();
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
            this.detail.Size = new System.Drawing.Size(692, 303);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.txtReason);
            this.detailcont.Controls.Add(this.txtID);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labelCode);
            this.detailcont.Size = new System.Drawing.Size(692, 265);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 265);
            this.detailbtm.Size = new System.Drawing.Size(692, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(792, 388);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(700, 332);
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(335, 33);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 7;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // txtReason
            // 
            this.txtReason.BackColor = System.Drawing.Color.White;
            this.txtReason.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtReason.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.txtReason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtReason.Location = new System.Drawing.Point(136, 81);
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(337, 23);
            this.txtReason.TabIndex = 6;
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.White;
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtID.Location = new System.Drawing.Point(136, 33);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(80, 23);
            this.txtID.TabIndex = 5;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(57, 81);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(75, 23);
            this.labelDescription.TabIndex = 9;
            this.labelDescription.Text = "Reason";
            // 
            // labelCode
            // 
            this.labelCode.Location = new System.Drawing.Point(57, 33);
            this.labelCode.Name = "labelCode";
            this.labelCode.Size = new System.Drawing.Size(75, 23);
            this.labelCode.TabIndex = 8;
            this.labelCode.Text = "ID";
            // 
            // B02
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(700, 365);
            this.DefaultFilter = "Type=\'ID\'";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B02";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B02. Modify IDD Reason";
            this.WorkAlias = "ClogReason";
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
        private Win.UI.TextBox txtReason;
        private Win.UI.TextBox txtID;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelCode;
    }
}