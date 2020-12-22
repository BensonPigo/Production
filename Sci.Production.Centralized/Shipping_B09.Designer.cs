namespace Sci.Production.Centralized
{
    partial class Shipping_B09
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lbConsignee = new Sci.Win.UI.Label();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.txtLocalSupp1 = new Sci.Production.Class.TxtLocalSuppPMSDB();
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
            this.masterpanel.Controls.Add(this.txtLocalSupp1);
            this.masterpanel.Controls.Add(this.checkJunk);
            this.masterpanel.Controls.Add(this.lbConsignee);
            this.masterpanel.Size = new System.Drawing.Size(892, 61);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbConsignee, 0);
            this.masterpanel.Controls.SetChildIndex(this.checkJunk, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtLocalSupp1, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 61);
            this.detailpanel.Size = new System.Drawing.Size(892, 288);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(643, 26);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(821, 1);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(892, 288);
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
            this.browse.Size = new System.Drawing.Size(909, 265);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(917, 294);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(470, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(422, 13);
            // 
            // lbConsignee
            // 
            this.lbConsignee.Location = new System.Drawing.Point(7, 9);
            this.lbConsignee.Name = "lbConsignee";
            this.lbConsignee.Size = new System.Drawing.Size(70, 23);
            this.lbConsignee.TabIndex = 11;
            this.lbConsignee.Text = "Consignee";
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(391, 9);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 21;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // txtLocalSupp1
            // 
            this.txtLocalSupp1.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "ID", true));
            this.txtLocalSupp1.DisplayBox1Binding = "";
            this.txtLocalSupp1.IsFactory = true;
            this.txtLocalSupp1.ConnectionName = "ProductionTPE";
            this.txtLocalSupp1.Location = new System.Drawing.Point(80, 9);
            this.txtLocalSupp1.Name = "txtLocalSupp1";
            this.txtLocalSupp1.Size = new System.Drawing.Size(258, 25);
            this.txtLocalSupp1.TabIndex = 22;
            this.txtLocalSupp1.TextBox1Binding = "";
            // 
            // Shipping_B09
            // 
            this.ClientSize = new System.Drawing.Size(917, 327);
            this.ConnectionName = "ProductionTPE";
            this.EditMode = true;
            this.GridAlias = "Consignee_Detail";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.KeyField1 = "ID";
            this.Name = "Shipping_B09";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "Shipping_B09. Consignee Information";
            this.WorkAlias = "Consignee";
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
        private Win.UI.Label lbConsignee;
        private Win.UI.CheckBox checkJunk;
        private Class.TxtLocalSuppPMSDB txtLocalSupp1;
    }
}
