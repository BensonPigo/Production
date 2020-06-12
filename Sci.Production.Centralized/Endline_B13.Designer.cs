namespace Sci.Production.Centralized
{
    partial class Endline_B13
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
            this.txtID = new Sci.Win.UI.TextBox();
            this.txtDesc = new Sci.Win.UI.TextBox();
            this.chkJunk = new Sci.Win.UI.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.chkJunk);
            this.detailcont.Controls.Add(this.txtDesc);
            this.detailcont.Controls.Add(this.txtID);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(792, 388);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(800, 417);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(70, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "ID";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(70, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 23);
            this.label2.TabIndex = 9;
            this.label2.Text = "Description";
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtID.IsSupportEditMode = false;
            this.txtID.Location = new System.Drawing.Point(158, 41);
            this.txtID.Name = "txtID";
            this.txtID.ReadOnly = true;
            this.txtID.Size = new System.Drawing.Size(100, 23);
            this.txtID.TabIndex = 1;
            // 
            // txtDesc
            // 
            this.txtDesc.BackColor = System.Drawing.Color.White;
            this.txtDesc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.txtDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDesc.Location = new System.Drawing.Point(158, 92);
            this.txtDesc.MaxLength = 100;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(480, 23);
            this.txtDesc.TabIndex = 2;
            // 
            // chkJunk
            // 
            this.chkJunk.AutoSize = true;
            this.chkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.chkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkJunk.Location = new System.Drawing.Point(543, 43);
            this.chkJunk.Name = "chkJunk";
            this.chkJunk.Size = new System.Drawing.Size(57, 21);
            this.chkJunk.TabIndex = 3;
            this.chkJunk.Text = "Junk";
            this.chkJunk.UseVisualStyleBackColor = true;
            // 
            // Endline_B13
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.IsDeleteOnBrowse = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "Endline_B13";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "Endline_B13 Dispose Reason";
            this.WorkAlias = "DQSReason ";
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

        private Win.UI.CheckBox chkJunk;
        private Win.UI.TextBox txtDesc;
        private Win.UI.TextBox txtID;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
    }
}