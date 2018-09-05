namespace Sci.Production.Thread
{
    partial class B07
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
            this.editBoxDescription = new Sci.Win.UI.EditBox();
            this.checkBoxJunk = new Sci.Win.UI.CheckBox();
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
            this.detailcont.Controls.Add(this.checkBoxJunk);
            this.detailcont.Controls.Add(this.editBoxDescription);
            this.detailcont.Controls.Add(this.txtID);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(684, 257);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(692, 286);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(23, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Thread Color Group";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(23, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Description";
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.White;
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtID.Location = new System.Drawing.Point(154, 13);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(238, 23);
            this.txtID.TabIndex = 2;
            // 
            // editBoxDescription
            // 
            this.editBoxDescription.BackColor = System.Drawing.Color.White;
            this.editBoxDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editBoxDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBoxDescription.Location = new System.Drawing.Point(154, 48);
            this.editBoxDescription.Multiline = true;
            this.editBoxDescription.Name = "editBoxDescription";
            this.editBoxDescription.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditMode;
            this.editBoxDescription.Size = new System.Drawing.Size(342, 150);
            this.editBoxDescription.TabIndex = 3;
            // 
            // checkBoxJunk
            // 
            this.checkBoxJunk.AutoSize = true;
            this.checkBoxJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkBoxJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBoxJunk.Location = new System.Drawing.Point(420, 13);
            this.checkBoxJunk.Name = "checkBoxJunk";
            this.checkBoxJunk.Size = new System.Drawing.Size(57, 21);
            this.checkBoxJunk.TabIndex = 4;
            this.checkBoxJunk.Text = "Junk";
            this.checkBoxJunk.UseVisualStyleBackColor = true;
            // 
            // B07
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 319);
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportMove = false;
            this.IsSupportPrint = false;
            this.Name = "B07";
            this.Text = "B07. Thread Color Group";
            this.WorkAlias = "ThreadColorGroup";
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

        private Win.UI.CheckBox checkBoxJunk;
        private Win.UI.EditBox editBoxDescription;
        private Win.UI.TextBox txtID;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
    }
}