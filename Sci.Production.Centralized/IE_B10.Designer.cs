namespace Sci.Production.Centralized
{
    partial class IE_B10
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
            this.btnPicture2Delete = new Sci.Win.UI.Button();
            this.btnPicture2Attach = new Sci.Win.UI.Button();
            this.picture2 = new Sci.Win.UI.PictureBox();
            this.btnPicture1Delete = new Sci.Win.UI.Button();
            this.btnPicture1Attach = new Sci.Win.UI.Button();
            this.picture1 = new Sci.Win.UI.PictureBox();
            this.labelPicture2 = new Sci.Win.UI.Label();
            this.labelPicture1 = new Sci.Win.UI.Label();
            this.labelID = new Sci.Win.UI.Label();
            this.txtID = new Sci.Win.UI.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture1)).BeginInit();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(923, 610);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtID);
            this.detailcont.Controls.Add(this.labelID);
            this.detailcont.Controls.Add(this.btnPicture2Delete);
            this.detailcont.Controls.Add(this.btnPicture2Attach);
            this.detailcont.Controls.Add(this.picture2);
            this.detailcont.Controls.Add(this.btnPicture1Delete);
            this.detailcont.Controls.Add(this.btnPicture1Attach);
            this.detailcont.Controls.Add(this.picture1);
            this.detailcont.Controls.Add(this.labelPicture2);
            this.detailcont.Controls.Add(this.labelPicture1);
            this.detailcont.Size = new System.Drawing.Size(923, 572);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 572);
            this.detailbtm.Size = new System.Drawing.Size(923, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(923, 610);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(931, 639);
            // 
            // btnPicture2Delete
            // 
            this.btnPicture2Delete.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnPicture2Delete.Location = new System.Drawing.Point(598, 76);
            this.btnPicture2Delete.Name = "btnPicture2Delete";
            this.btnPicture2Delete.Size = new System.Drawing.Size(63, 30);
            this.btnPicture2Delete.TabIndex = 23;
            this.btnPicture2Delete.Text = "Delete";
            this.btnPicture2Delete.UseVisualStyleBackColor = true;
            this.btnPicture2Delete.Click += new System.EventHandler(this.BtnPicture2Delete_Click);
            // 
            // btnPicture2Attach
            // 
            this.btnPicture2Attach.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnPicture2Attach.Location = new System.Drawing.Point(532, 76);
            this.btnPicture2Attach.Name = "btnPicture2Attach";
            this.btnPicture2Attach.Size = new System.Drawing.Size(63, 30);
            this.btnPicture2Attach.TabIndex = 22;
            this.btnPicture2Attach.Text = "Attach";
            this.btnPicture2Attach.UseVisualStyleBackColor = true;
            this.btnPicture2Attach.Click += new System.EventHandler(this.BtnPicture2Attach_Click);
            // 
            // picture2
            // 
            this.picture2.Image = null;
            this.picture2.Location = new System.Drawing.Point(468, 113);
            this.picture2.Name = "picture2";
            this.picture2.Size = new System.Drawing.Size(430, 434);
            this.picture2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picture2.TabIndex = 21;
            this.picture2.TabStop = false;
            this.picture2.WaitOnLoad = true;
            // 
            // btnPicture1Delete
            // 
            this.btnPicture1Delete.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnPicture1Delete.Location = new System.Drawing.Point(149, 76);
            this.btnPicture1Delete.Name = "btnPicture1Delete";
            this.btnPicture1Delete.Size = new System.Drawing.Size(63, 30);
            this.btnPicture1Delete.TabIndex = 20;
            this.btnPicture1Delete.Text = "Delete";
            this.btnPicture1Delete.UseVisualStyleBackColor = true;
            this.btnPicture1Delete.Click += new System.EventHandler(this.BtnPicture1Delete_Click);
            // 
            // btnPicture1Attach
            // 
            this.btnPicture1Attach.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnPicture1Attach.Location = new System.Drawing.Point(83, 76);
            this.btnPicture1Attach.Name = "btnPicture1Attach";
            this.btnPicture1Attach.Size = new System.Drawing.Size(63, 30);
            this.btnPicture1Attach.TabIndex = 19;
            this.btnPicture1Attach.Text = "Attach";
            this.btnPicture1Attach.UseVisualStyleBackColor = true;
            this.btnPicture1Attach.Click += new System.EventHandler(this.BtnPicture1Attach_Click);
            // 
            // picture1
            // 
            this.picture1.Image = null;
            this.picture1.Location = new System.Drawing.Point(19, 113);
            this.picture1.Name = "picture1";
            this.picture1.Size = new System.Drawing.Size(430, 434);
            this.picture1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picture1.TabIndex = 18;
            this.picture1.TabStop = false;
            this.picture1.WaitOnLoad = true;
            // 
            // labelPicture2
            // 
            this.labelPicture2.Location = new System.Drawing.Point(468, 79);
            this.labelPicture2.Name = "labelPicture2";
            this.labelPicture2.Size = new System.Drawing.Size(61, 23);
            this.labelPicture2.TabIndex = 17;
            this.labelPicture2.Text = "Picture 2";
            // 
            // labelPicture1
            // 
            this.labelPicture1.Location = new System.Drawing.Point(19, 79);
            this.labelPicture1.Name = "labelPicture1";
            this.labelPicture1.Size = new System.Drawing.Size(61, 23);
            this.labelPicture1.TabIndex = 16;
            this.labelPicture1.Text = "Picture 1";
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(19, 23);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(142, 23);
            this.labelID.TabIndex = 50;
            this.labelID.Text = "ID";
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtID.Location = new System.Drawing.Point(164, 23);
            this.txtID.Name = "txtID";
            this.txtID.ReadOnly = true;
            this.txtID.Size = new System.Drawing.Size(365, 23);
            this.txtID.TabIndex = 51;
            // 
            // IE_B10
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 672);
            this.ConnectionName = "ProductionTPE";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportPrint = false;
            this.Name = "IE_B10";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "IE_B10. Sewing Machine Attachment";
            this.WorkAlias = "SewingMachineAttachment";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picture2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picture1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.Button btnPicture2Delete;
        private Win.UI.Button btnPicture2Attach;
        private Win.UI.PictureBox picture2;
        private Win.UI.Button btnPicture1Delete;
        private Win.UI.Button btnPicture1Attach;
        private Win.UI.PictureBox picture1;
        private Win.UI.Label labelPicture2;
        private Win.UI.Label labelPicture1;
        private Win.UI.Label labelID;
        private Win.UI.TextBox txtID;
    }
}