namespace Sci.Production.Quality
{
    partial class B23
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
            this.displayWeaveTypeID = new Sci.Win.UI.DisplayBox();
            this.chkNonPhysical = new Sci.Win.UI.CheckBox();
            this.chkNonWeight = new Sci.Win.UI.CheckBox();
            this.chkNonShadebond = new Sci.Win.UI.CheckBox();
            this.chkNonContinuity = new Sci.Win.UI.CheckBox();
            this.chkNonOdor = new Sci.Win.UI.CheckBox();
            this.chkNonMoisture = new Sci.Win.UI.CheckBox();
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
            this.detail.Size = new System.Drawing.Size(792, 386);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.chkNonMoisture);
            this.detailcont.Controls.Add(this.chkNonOdor);
            this.detailcont.Controls.Add(this.chkNonContinuity);
            this.detailcont.Controls.Add(this.chkNonShadebond);
            this.detailcont.Controls.Add(this.chkNonWeight);
            this.detailcont.Controls.Add(this.chkNonPhysical);
            this.detailcont.Controls.Add(this.displayWeaveTypeID);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Size = new System.Drawing.Size(792, 348);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 348);
            this.detailbtm.Size = new System.Drawing.Size(792, 38);
            // 
            // browse
            // 
            this.browse.Location = new System.Drawing.Point(4, 27);
            this.browse.Size = new System.Drawing.Size(792, 386);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(800, 417);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(25, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Weave Type";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(25, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Fabric Inspection";
            // 
            // displayWeaveTypeID
            // 
            this.displayWeaveTypeID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayWeaveTypeID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "WeaveTypeID", true));
            this.displayWeaveTypeID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayWeaveTypeID.Location = new System.Drawing.Point(155, 11);
            this.displayWeaveTypeID.Name = "displayWeaveTypeID";
            this.displayWeaveTypeID.Size = new System.Drawing.Size(115, 24);
            this.displayWeaveTypeID.TabIndex = 2;
            // 
            // chkNonPhysical
            // 
            this.chkNonPhysical.AutoSize = true;
            this.chkNonPhysical.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "NonPhysical", true));
            this.chkNonPhysical.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkNonPhysical.Location = new System.Drawing.Point(155, 53);
            this.chkNonPhysical.Name = "chkNonPhysical";
            this.chkNonPhysical.Size = new System.Drawing.Size(110, 22);
            this.chkNonPhysical.TabIndex = 3;
            this.chkNonPhysical.Text = "NonPhysical";
            this.chkNonPhysical.UseVisualStyleBackColor = true;
            // 
            // chkNonWeight
            // 
            this.chkNonWeight.AutoSize = true;
            this.chkNonWeight.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "NonWeight", true));
            this.chkNonWeight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkNonWeight.Location = new System.Drawing.Point(280, 53);
            this.chkNonWeight.Name = "chkNonWeight";
            this.chkNonWeight.Size = new System.Drawing.Size(101, 22);
            this.chkNonWeight.TabIndex = 4;
            this.chkNonWeight.Text = "NonWeight";
            this.chkNonWeight.UseVisualStyleBackColor = true;
            // 
            // chkNonShadebond
            // 
            this.chkNonShadebond.AutoSize = true;
            this.chkNonShadebond.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "NonShadebond", true));
            this.chkNonShadebond.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkNonShadebond.Location = new System.Drawing.Point(387, 53);
            this.chkNonShadebond.Name = "chkNonShadebond";
            this.chkNonShadebond.Size = new System.Drawing.Size(130, 22);
            this.chkNonShadebond.TabIndex = 5;
            this.chkNonShadebond.Text = "NonShadebond";
            this.chkNonShadebond.UseVisualStyleBackColor = true;
            // 
            // chkNonContinuity
            // 
            this.chkNonContinuity.AutoSize = true;
            this.chkNonContinuity.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "NonContinuity", true));
            this.chkNonContinuity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkNonContinuity.Location = new System.Drawing.Point(155, 81);
            this.chkNonContinuity.Name = "chkNonContinuity";
            this.chkNonContinuity.Size = new System.Drawing.Size(120, 22);
            this.chkNonContinuity.TabIndex = 6;
            this.chkNonContinuity.Text = "NonContinuity";
            this.chkNonContinuity.UseVisualStyleBackColor = true;
            // 
            // chkNonOdor
            // 
            this.chkNonOdor.AutoSize = true;
            this.chkNonOdor.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "NonOdor", true));
            this.chkNonOdor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkNonOdor.Location = new System.Drawing.Point(280, 81);
            this.chkNonOdor.Name = "chkNonOdor";
            this.chkNonOdor.Size = new System.Drawing.Size(89, 22);
            this.chkNonOdor.TabIndex = 7;
            this.chkNonOdor.Text = "NonOdor";
            this.chkNonOdor.UseVisualStyleBackColor = true;
            // 
            // chkNonMoisture
            // 
            this.chkNonMoisture.AutoSize = true;
            this.chkNonMoisture.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "NonMoisture", true));
            this.chkNonMoisture.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkNonMoisture.Location = new System.Drawing.Point(387, 81);
            this.chkNonMoisture.Name = "chkNonMoisture";
            this.chkNonMoisture.Size = new System.Drawing.Size(113, 22);
            this.chkNonMoisture.TabIndex = 8;
            this.chkNonMoisture.Text = "NonMoisture";
            this.chkNonMoisture.UseVisualStyleBackColor = true;
            // 
            // B23
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.IsDeleteOnBrowse = false;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B23";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B23. QA Weave Type Setting";
            this.WorkAlias = "QAWeaveTypeSetting";
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

        private Win.UI.CheckBox chkNonMoisture;
        private Win.UI.CheckBox chkNonOdor;
        private Win.UI.CheckBox chkNonContinuity;
        private Win.UI.CheckBox chkNonShadebond;
        private Win.UI.CheckBox chkNonWeight;
        private Win.UI.CheckBox chkNonPhysical;
        private Win.UI.DisplayBox displayWeaveTypeID;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
    }
}