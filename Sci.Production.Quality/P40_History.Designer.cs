namespace Sci.Production.Quality
{
    partial class P40_History
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
            this.displayVersion = new Sci.Win.UI.DisplayBox();
            this.label8 = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).BeginInit();
            this.masterpanel.SuspendLayout();
            this.detailpanel.SuspendLayout();
            this.detail2.SuspendLayout();
            this.detailpanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.label8);
            this.masterpanel.Controls.Add(this.displayVersion);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayVersion, 0);
            this.masterpanel.Controls.SetChildIndex(this.label8, 0);
            // 
            // detail2
            // 
            this.detail2.Size = new System.Drawing.Size(948, 497);
            // 
            // detailgridcont2
            // 
            this.detailgridcont2.Size = new System.Drawing.Size(942, 451);
            // 
            // detailpanel2
            // 
            this.detailpanel2.Size = new System.Drawing.Size(942, 40);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(962, 485);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(970, 514);
            // 
            // displayVersion
            // 
            this.displayVersion.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayVersion.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Version", true));
            this.displayVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayVersion.Location = new System.Drawing.Point(845, 3);
            this.displayVersion.Name = "displayVersion";
            this.displayVersion.Size = new System.Drawing.Size(100, 23);
            this.displayVersion.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(767, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 23);
            this.label8.TabIndex = 10;
            this.label8.Text = "Version";
            // 
            // P40_History
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 547);
            this.DefaultOrder = "Version";
            this.GridAlias = "ADIDASComplain_Detail_History";
            this.IsSupportConfirm = false;
            this.IsSupportEdit = false;
            this.IsSupportUnconfirm = false;
            this.KeyField1 = "ID,Version";
            this.Name = "P40_History";
            this.OnLineHelpID = "Sci.Production.Quality.P40";
            this.Text = "P40. AdiComp Data History";
            this.WorkAlias = "ADIDASComplain_History";
            this.Controls.SetChildIndex(this.tabs, 0);
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).EndInit();
            this.masterpanel.ResumeLayout(false);
            this.masterpanel.PerformLayout();
            this.detailpanel.ResumeLayout(false);
            this.detail2.ResumeLayout(false);
            this.detailpanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label8;
        private Win.UI.DisplayBox displayVersion;
    }
}