namespace Sci.Production.Quality
{
    partial class B17
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
            this.txtID = new Sci.Win.UI.TextBox();
            this.txtSubProcessID = new Sci.Win.UI.TextBox();
            this.editDesc = new Sci.Win.UI.EditBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
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
            this.detail.Location = new System.Drawing.Point(4, 27);
            this.detail.Size = new System.Drawing.Size(897, 393);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.editDesc);
            this.detailcont.Controls.Add(this.txtSubProcessID);
            this.detailcont.Controls.Add(this.txtID);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Size = new System.Drawing.Size(897, 355);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 355);
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(4, 27);
            this.browse.Size = new System.Drawing.Size(857, 361);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(865, 392);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(31, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(31, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "SubProcess";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(31, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Description";
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.White;
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtID.Location = new System.Drawing.Point(127, 20);
            this.txtID.MaxLength = 20;
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(199, 24);
            this.txtID.TabIndex = 3;
            // 
            // txtSubProcessID
            // 
            this.txtSubProcessID.BackColor = System.Drawing.Color.White;
            this.txtSubProcessID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SubProcessID", true));
            this.txtSubProcessID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSubProcessID.Location = new System.Drawing.Point(127, 56);
            this.txtSubProcessID.Name = "txtSubProcessID";
            this.txtSubProcessID.Size = new System.Drawing.Size(199, 24);
            this.txtSubProcessID.TabIndex = 4;
            this.txtSubProcessID.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSubProcessID_PopUp);
            this.txtSubProcessID.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSubProcessID_Validating);
            // 
            // editDesc
            // 
            this.editDesc.BackColor = System.Drawing.Color.White;
            this.editDesc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDesc.Location = new System.Drawing.Point(127, 91);
            this.editDesc.Multiline = true;
            this.editDesc.Name = "editDesc";
            this.editDesc.Size = new System.Drawing.Size(378, 186);
            this.editDesc.TabIndex = 5;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(352, 20);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(59, 22);
            this.checkJunk.TabIndex = 6;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // B17
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(865, 425);
            this.Name = "B17";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B17. Sub-Process Process Location";
            this.WorkAlias = "SubProLocation";
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
        private Win.UI.EditBox editDesc;
        private Win.UI.TextBox txtSubProcessID;
        private Win.UI.TextBox txtID;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
    }
}