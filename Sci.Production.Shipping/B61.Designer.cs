namespace Sci.Production.Shipping
{
	partial class B61
	{
	        /// <summary>
	        /// 設計工具所需的變數。
	        /// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清除任何使用中的資源。
		/// </summary>
	        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 設計工具產生的程式碼

		/// <summary>
		/// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
		/// 這個方法的內容。
		/// </summary>
		private void InitializeComponent()
		{
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.txtCustomsDesc = new Sci.Win.UI.TextBox();
            this.labCustomsDesc = new Sci.Win.UI.Label();
            this.txtCDCUnit = new Sci.Win.UI.TextBox();
            this.labCDCUnit = new Sci.Win.UI.Label();
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
            this.masterpanel.Controls.Add(this.txtCDCUnit);
            this.masterpanel.Controls.Add(this.labCDCUnit);
            this.masterpanel.Controls.Add(this.checkJunk);
            this.masterpanel.Controls.Add(this.txtCustomsDesc);
            this.masterpanel.Controls.Add(this.labCustomsDesc);
            this.masterpanel.Size = new System.Drawing.Size(724, 106);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labCustomsDesc, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtCustomsDesc, 0);
            this.masterpanel.Controls.SetChildIndex(this.checkJunk, 0);
            this.masterpanel.Controls.SetChildIndex(this.labCDCUnit, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtCDCUnit, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 106);
            this.detailpanel.Size = new System.Drawing.Size(724, 243);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(614, 68);
            this.gridicon.TabIndex = 3;
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(634, 1);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(724, 243);
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
            this.detail.Size = new System.Drawing.Size(724, 387);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(724, 349);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(724, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(724, 387);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(732, 416);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(252, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(378, 7);
            this.editby.Size = new System.Drawing.Size(247, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(330, 13);
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkJunk.IsSupportEditMode = false;
            this.checkJunk.Location = new System.Drawing.Point(387, 21);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.ReadOnly = true;
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 2;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // txtCustomsDesc
            // 
            this.txtCustomsDesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtCustomsDesc.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtCustomsDesc.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtCustomsDesc.IsSupportEditMode = false;
            this.txtCustomsDesc.Location = new System.Drawing.Point(227, 21);
            this.txtCustomsDesc.Name = "txtCustomsDesc";
            this.txtCustomsDesc.ReadOnly = true;
            this.txtCustomsDesc.Size = new System.Drawing.Size(151, 23);
            this.txtCustomsDesc.TabIndex = 0;
            // 
            // labCustomsDesc
            // 
            this.labCustomsDesc.Location = new System.Drawing.Point(80, 21);
            this.labCustomsDesc.Name = "labCustomsDesc";
            this.labCustomsDesc.Size = new System.Drawing.Size(144, 23);
            this.labCustomsDesc.TabIndex = 4;
            this.labCustomsDesc.Text = "Customs Description";
            this.labCustomsDesc.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtCDCUnit
            // 
            this.txtCDCUnit.BackColor = System.Drawing.Color.White;
            this.txtCDCUnit.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CDCUnit", true));
            this.txtCDCUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCDCUnit.Location = new System.Drawing.Point(227, 57);
            this.txtCDCUnit.Name = "txtCDCUnit";
            this.txtCDCUnit.Size = new System.Drawing.Size(114, 23);
            this.txtCDCUnit.TabIndex = 1;
            // 
            // labCDCUnit
            // 
            this.labCDCUnit.Location = new System.Drawing.Point(80, 57);
            this.labCDCUnit.Name = "labCDCUnit";
            this.labCDCUnit.Size = new System.Drawing.Size(144, 23);
            this.labCDCUnit.TabIndex = 5;
            this.labCDCUnit.Text = "CDC Unit";
            this.labCDCUnit.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // B61
            // 
            this.ClientSize = new System.Drawing.Size(732, 449);
            this.DefaultControl = "txtCustomsDesc";
            this.DefaultControlForEdit = "txtCDCUnit";
            this.GridAlias = "KHCustomsDescription_Detail";
            this.IsDeleteOnBrowse = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.KeyField1 = "ID";
            this.Name = "B61";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "B61. KH Customs Description";
            this.UniqueExpress = "ID";
            this.WorkAlias = "KHCustomsDescription";
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

        private Win.UI.TextBox txtCDCUnit;
        private Win.UI.Label labCDCUnit;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.TextBox txtCustomsDesc;
        private Win.UI.Label labCustomsDesc;
    }
}
