namespace PMSUploadDataToAPS.Daily
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
            this.MDivision = new Sci.Win.UI.Label();
            this.MDivisionID = new Sci.Win.UI.TextBox();
            this.panelFTP = new Sci.Win.UI.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBS)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.panelFTP.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnUpdate
            // 
            this.btnUpdate.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnUpdate.Location = new System.Drawing.Point(203, 12);
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
            this.statusStrip.Location = new System.Drawing.Point(0, 60);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(314, 22);
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
            // MDivision
            // 
            this.MDivision.Lines = 0;
            this.MDivision.Location = new System.Drawing.Point(10, 10);
            this.MDivision.Name = "MDivision";
            this.MDivision.Size = new System.Drawing.Size(66, 21);
            this.MDivision.TabIndex = 0;
            this.MDivision.Text = "MDivision";
            // 
            // MDivisionID
            // 
            this.MDivisionID.BackColor = System.Drawing.Color.White;
            this.MDivisionID.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.MDivisionID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.MDivisionID.Location = new System.Drawing.Point(79, 10);
            this.MDivisionID.Name = "MDivisionID";
            this.MDivisionID.Size = new System.Drawing.Size(88, 21);
            this.MDivisionID.TabIndex = 1;
            // 
            // panelFTP
            // 
            this.panelFTP.Controls.Add(this.MDivisionID);
            this.panelFTP.Controls.Add(this.MDivision);
            this.panelFTP.DrawBorder = true;
            this.panelFTP.Location = new System.Drawing.Point(12, 12);
            this.panelFTP.Name = "panelFTP";
            this.panelFTP.Size = new System.Drawing.Size(185, 40);
            this.panelFTP.TabIndex = 1;
            // 
            // Main
            // 
            this.AcceptButton = this.btnUpdate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(314, 82);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.panelFTP);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsSupportClip = false;
            this.IsSupportLocate = false;
            this.IsSupportPrint = false;
            this.IsToolbarVisible = false;
            this.Name = "Main";
            this.Text = "Data Transfer";
            this.WorkAlias = "System";
            this.Controls.SetChildIndex(this.panelFTP, 0);
            this.Controls.SetChildIndex(this.btnUpdate, 0);
            this.Controls.SetChildIndex(this.statusStrip, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridBS)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.panelFTP.ResumeLayout(false);
            this.panelFTP.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Sci.Win.UI.ListControlBindingSource gridBS;
        private Sci.Win.UI.Button btnUpdate;
        private Sci.Win.UI.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel labelProgress;
        private Sci.Win.UI.Label MDivision;
        private Sci.Win.UI.TextBox MDivisionID;
        private Sci.Win.UI.Panel panelFTP;
    }
}