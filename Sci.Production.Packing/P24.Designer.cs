namespace Sci.Production.Packing
{
    partial class P24
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
            this.txtPackingListID = new Sci.Win.UI.TextBox();
            this.label4 = new Sci.Win.UI.Label();
            this.btnDownload = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
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
            this.masterpanel.Controls.Add(this.btnImport);
            this.masterpanel.Controls.Add(this.btnDownload);
            this.masterpanel.Controls.Add(this.label4);
            this.masterpanel.Controls.Add(this.txtPackingListID);
            this.masterpanel.Size = new System.Drawing.Size(1085, 92);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtPackingListID, 0);
            this.masterpanel.Controls.SetChildIndex(this.label4, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnDownload, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnImport, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 92);
            this.detailpanel.Size = new System.Drawing.Size(1085, 257);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(16, 54);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(1085, 257);
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
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(1085, 387);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(1085, 349);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(1085, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(1085, 387);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1093, 416);
            // 
            // txtPackingListID
            // 
            this.txtPackingListID.BackColor = System.Drawing.Color.White;
            this.txtPackingListID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "PackingListID", true));
            this.txtPackingListID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackingListID.Location = new System.Drawing.Point(120, 17);
            this.txtPackingListID.Name = "txtPackingListID";
            this.txtPackingListID.Size = new System.Drawing.Size(121, 23);
            this.txtPackingListID.TabIndex = 1;
            this.txtPackingListID.Validating += new System.ComponentModel.CancelEventHandler(this.TxtPackingListID_Validating);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(16, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 23);
            this.label4.TabIndex = 24;
            this.label4.Text = "Packing No.";
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(860, 13);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(217, 30);
            this.btnDownload.TabIndex = 4;
            this.btnDownload.Text = "Download ShippingMark pic";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.BtnDownload_Click);
            // 
            // btnImport
            // 
            this.btnImport.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnImport.Location = new System.Drawing.Point(860, 49);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(217, 30);
            this.btnImport.TabIndex = 5;
            this.btnImport.Text = "Import ShippingMark pic";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // P24
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1093, 449);
            this.GridAlias = "ShippingMarkPic_Detail";
            this.GridNew = 0;
            this.GridUniqueKey = "ShippingMarkPicUkey,SCICtnNo,ShippingMarkTypeUkey";
            this.IsGridIcon2Visible = false;
            this.IsGridIconVisible = false;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.KeyField1 = "Ukey";
            this.KeyField2 = "ShippingMarkPicUkey";
            this.Name = "P24";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "P24. Shipping Mark Pic (for GenSong)";
            this.WorkAlias = "ShippingMarkPic";
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
        private Win.UI.TextBox txtPackingListID;
        private Win.UI.Button btnImport;
        private Win.UI.Button btnDownload;
        private Win.UI.Label label4;
    }
}