namespace MailQMS_Fabric_Inspection_record
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            this.gridBS = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.btnUpdate = new Sci.Win.UI.Button();
            this.statusStrip = new Sci.Win.UI.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.labelProgress = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnTestWebAPI = new Sci.Win.UI.Button();
            this.btnTestMail = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBS)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnUpdate.Location = new System.Drawing.Point(57, 12);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(106, 30);
            this.btnUpdate.TabIndex = 4;
            this.btnUpdate.Text = "EXECUTE";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.labelProgress});
            this.statusStrip.Location = new System.Drawing.Point(0, 129);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(225, 22);
            this.statusStrip.TabIndex = 8;
            this.statusStrip.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(59, 17);
            this.StatusLabel.Text = "Progress:";
            // 
            // labelProgress
            // 
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(65, 17);
            this.labelProgress.Text = "                   ";
            // 
            // btnTestWebAPI
            // 
            this.btnTestWebAPI.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnTestWebAPI.Location = new System.Drawing.Point(57, 47);
            this.btnTestWebAPI.Name = "btnTestWebAPI";
            this.btnTestWebAPI.Size = new System.Drawing.Size(106, 30);
            this.btnTestWebAPI.TabIndex = 9;
            this.btnTestWebAPI.Text = "Test WebApi";
            this.btnTestWebAPI.UseVisualStyleBackColor = true;
            this.btnTestWebAPI.Click += new System.EventHandler(this.btnTestWebAPI_Click);
            // 
            // btnTestMail
            // 
            this.btnTestMail.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnTestMail.Location = new System.Drawing.Point(57, 83);
            this.btnTestMail.Name = "btnTestMail";
            this.btnTestMail.Size = new System.Drawing.Size(106, 30);
            this.btnTestMail.TabIndex = 10;
            this.btnTestMail.Text = "Test Mail";
            this.btnTestMail.UseVisualStyleBackColor = true;
            this.btnTestMail.Click += new System.EventHandler(this.btnTestMail_Click);
            // 
            // Main
            // 
            this.AcceptButton = this.btnUpdate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(225, 151);
            this.Controls.Add(this.btnTestMail);
            this.Controls.Add(this.btnTestWebAPI);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.btnUpdate);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsSupportClip = false;
            this.IsSupportLocate = false;
            this.IsSupportPrint = false;
            this.IsToolbarVisible = false;
            this.Name = "Main";
            this.Text = "Data Transfer";
            this.WorkAlias = "System";
            this.Controls.SetChildIndex(this.btnUpdate, 0);
            this.Controls.SetChildIndex(this.statusStrip, 0);
            this.Controls.SetChildIndex(this.btnTestWebAPI, 0);
            this.Controls.SetChildIndex(this.btnTestMail, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBS)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sci.Win.UI.ListControlBindingSource gridBS;
        private Sci.Win.UI.Button btnUpdate;
        private Sci.Win.UI.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel labelProgress;
        private Sci.Win.UI.Button btnTestWebAPI;
        private Sci.Win.UI.Button btnTestMail;
    }
}