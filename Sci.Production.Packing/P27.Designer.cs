namespace Sci.Production.Packing
{
    partial class P27
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
            this.label4 = new Sci.Win.UI.Label();
            this.txtPackingListID = new Sci.Win.UI.TextBox();
            this.btnGenerate = new Sci.Win.UI.Button();
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
            this.masterpanel.Controls.Add(this.label4);
            this.masterpanel.Controls.Add(this.txtPackingListID);
            this.masterpanel.Size = new System.Drawing.Size(892, 73);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtPackingListID, 0);
            this.masterpanel.Controls.SetChildIndex(this.label4, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 73);
            this.detailpanel.Size = new System.Drawing.Size(892, 276);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(5, 38);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(892, 276);
            // 
            // detail2
            // 
            this.detail2.Size = new System.Drawing.Size(892, 387);
            // 
            // detailgridcont2
            // 
            this.detailgridcont2.Size = new System.Drawing.Size(886, 341);
            // 
            // detailpanel2
            // 
            this.detailpanel2.Size = new System.Drawing.Size(886, 40);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1006, 416);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1014, 445);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(16, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 23);
            this.label4.TabIndex = 26;
            this.label4.Text = "Packing No.";
            // 
            // txtPackingListID
            // 
            this.txtPackingListID.BackColor = System.Drawing.Color.White;
            this.txtPackingListID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "PackingListID", true));
            this.txtPackingListID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackingListID.Location = new System.Drawing.Point(120, 17);
            this.txtPackingListID.Name = "txtPackingListID";
            this.txtPackingListID.Size = new System.Drawing.Size(121, 23);
            this.txtPackingListID.TabIndex = 25;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(839, 3);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(171, 30);
            this.btnGenerate.TabIndex = 4;
            this.btnGenerate.Text = "Generate Stamp File";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
            // 
            // P27
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 478);
            this.Controls.Add(this.btnGenerate);
            this.GridAlias = "ShippingMarkStamp_Detail";
            this.GridUniqueKey = "PackingListID,SCICtnNo,ShippingMarkTypeUkey";
            this.IsGridIconVisible = false;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.KeyField1 = "PackingListID";
            this.KeyField2 = "PackingListID";
            this.Name = "P27";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P27. Shipping Mark Stamp (for GenSong)";
            this.WorkAlias = "ShippingMarkStamp";
            this.Controls.SetChildIndex(this.tabs, 0);
            this.Controls.SetChildIndex(this.btnGenerate, 0);
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

        private Win.UI.Label label4;
        private Win.UI.TextBox txtPackingListID;
        private Win.UI.Button btnGenerate;
    }
}