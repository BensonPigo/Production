namespace Sci.Production.Logistic
{
    partial class P16_BrokenNeedlesRecord
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
            if (disposing && (components != null))
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
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.labSpNo = new Sci.Win.UI.Label();
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.labSciDelivery = new Sci.Win.UI.Label();
            this.labPoNo = new Sci.Win.UI.Label();
            this.txtPONo = new Sci.Win.UI.TextBox();
            this.labStyle = new Sci.Win.UI.Label();
            this.txtStyle = new Sci.Win.UI.TextBox();
            this.labBrand = new Sci.Win.UI.Label();
            this.txtBrand = new Sci.Win.UI.TextBox();
            this.dateBuyerDel = new Sci.Win.UI.DateBox();
            this.dateSCIDel = new Sci.Win.UI.DateBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Size = new System.Drawing.Size(518, 40);
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(12, 115);
            this.gridcont.Size = new System.Drawing.Size(494, 332);
            // 
            // append
            // 
            this.append.EditMode = Sci.Win.UI.AdvEditModes.None;
            // 
            // revise
            // 
            this.revise.EditMode = Sci.Win.UI.AdvEditModes.None;
            // 
            // delete
            // 
            this.delete.EditMode = Sci.Win.UI.AdvEditModes.None;
            // 
            // undo
            // 
            this.undo.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.undo.Location = new System.Drawing.Point(428, 5);
            this.undo.Visible = false;
            // 
            // save
            // 
            this.save.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.save.Location = new System.Drawing.Point(348, 5);
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSPNo.IsSupportEditMode = false;
            this.txtSPNo.Location = new System.Drawing.Point(93, 8);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.ReadOnly = true;
            this.txtSPNo.Size = new System.Drawing.Size(141, 23);
            this.txtSPNo.TabIndex = 133;
            // 
            // labSpNo
            // 
            this.labSpNo.Location = new System.Drawing.Point(12, 8);
            this.labSpNo.Name = "labSpNo";
            this.labSpNo.Size = new System.Drawing.Size(78, 23);
            this.labSpNo.TabIndex = 135;
            this.labSpNo.Text = "SP#";
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Location = new System.Drawing.Point(252, 8);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(110, 23);
            this.labelBuyerDelivery.TabIndex = 136;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            // 
            // labSciDelivery
            // 
            this.labSciDelivery.Location = new System.Drawing.Point(252, 34);
            this.labSciDelivery.Name = "labSciDelivery";
            this.labSciDelivery.Size = new System.Drawing.Size(110, 23);
            this.labSciDelivery.TabIndex = 138;
            this.labSciDelivery.Text = "SCI Delivery";
            // 
            // labPoNo
            // 
            this.labPoNo.Location = new System.Drawing.Point(12, 34);
            this.labPoNo.Name = "labPoNo";
            this.labPoNo.Size = new System.Drawing.Size(78, 23);
            this.labPoNo.TabIndex = 140;
            this.labPoNo.Text = "PO#";
            // 
            // txtPONo
            // 
            this.txtPONo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtPONo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtPONo.IsSupportEditMode = false;
            this.txtPONo.Location = new System.Drawing.Point(93, 34);
            this.txtPONo.Name = "txtPONo";
            this.txtPONo.ReadOnly = true;
            this.txtPONo.Size = new System.Drawing.Size(141, 23);
            this.txtPONo.TabIndex = 139;
            // 
            // labStyle
            // 
            this.labStyle.Location = new System.Drawing.Point(12, 60);
            this.labStyle.Name = "labStyle";
            this.labStyle.Size = new System.Drawing.Size(78, 23);
            this.labStyle.TabIndex = 142;
            this.labStyle.Text = "Style";
            // 
            // txtStyle
            // 
            this.txtStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtStyle.IsSupportEditMode = false;
            this.txtStyle.Location = new System.Drawing.Point(93, 60);
            this.txtStyle.Name = "txtStyle";
            this.txtStyle.ReadOnly = true;
            this.txtStyle.Size = new System.Drawing.Size(141, 23);
            this.txtStyle.TabIndex = 141;
            // 
            // labBrand
            // 
            this.labBrand.Location = new System.Drawing.Point(12, 86);
            this.labBrand.Name = "labBrand";
            this.labBrand.Size = new System.Drawing.Size(78, 23);
            this.labBrand.TabIndex = 144;
            this.labBrand.Text = "Brand";
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtBrand.IsSupportEditMode = false;
            this.txtBrand.Location = new System.Drawing.Point(93, 86);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.ReadOnly = true;
            this.txtBrand.Size = new System.Drawing.Size(141, 23);
            this.txtBrand.TabIndex = 143;
            // 
            // dateBuyerDel
            // 
            this.dateBuyerDel.IsSupportEditMode = false;
            this.dateBuyerDel.Location = new System.Drawing.Point(365, 8);
            this.dateBuyerDel.Name = "dateBuyerDel";
            this.dateBuyerDel.ReadOnly = true;
            this.dateBuyerDel.Size = new System.Drawing.Size(131, 23);
            this.dateBuyerDel.TabIndex = 145;
            // 
            // dateSCIDel
            // 
            this.dateSCIDel.IsSupportEditMode = false;
            this.dateSCIDel.Location = new System.Drawing.Point(365, 37);
            this.dateSCIDel.Name = "dateSCIDel";
            this.dateSCIDel.ReadOnly = true;
            this.dateSCIDel.Size = new System.Drawing.Size(131, 23);
            this.dateSCIDel.TabIndex = 146;
            // 
            // P16_BrokenNeedlesRecord
            // 
            this.ClientSize = new System.Drawing.Size(518, 497);
            this.Controls.Add(this.dateSCIDel);
            this.Controls.Add(this.dateBuyerDel);
            this.Controls.Add(this.labBrand);
            this.Controls.Add(this.txtBrand);
            this.Controls.Add(this.labStyle);
            this.Controls.Add(this.txtStyle);
            this.Controls.Add(this.labPoNo);
            this.Controls.Add(this.txtPONo);
            this.Controls.Add(this.labSciDelivery);
            this.Controls.Add(this.labelBuyerDelivery);
            this.Controls.Add(this.labSpNo);
            this.Controls.Add(this.txtSPNo);
            this.EditMode = true;
            this.GridPopUp = false;
            this.KeyField1 = "SP";
            this.Name = "P16_BrokenNeedlesRecord";
            this.OnLineHelpID = "Sci.Win.Subs.Input4";
            this.Text = "Broken Needles Record";
            this.WorkAlias = "BrokenNeedlesRecord";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            this.Controls.SetChildIndex(this.txtSPNo, 0);
            this.Controls.SetChildIndex(this.labSpNo, 0);
            this.Controls.SetChildIndex(this.labelBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.labSciDelivery, 0);
            this.Controls.SetChildIndex(this.txtPONo, 0);
            this.Controls.SetChildIndex(this.labPoNo, 0);
            this.Controls.SetChildIndex(this.txtStyle, 0);
            this.Controls.SetChildIndex(this.labStyle, 0);
            this.Controls.SetChildIndex(this.txtBrand, 0);
            this.Controls.SetChildIndex(this.labBrand, 0);
            this.Controls.SetChildIndex(this.dateBuyerDel, 0);
            this.Controls.SetChildIndex(this.dateSCIDel, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.TextBox txtSPNo;
        private Win.UI.Label labSpNo;
        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.Label labSciDelivery;
        private Win.UI.Label labPoNo;
        private Win.UI.TextBox txtPONo;
        private Win.UI.Label labStyle;
        private Win.UI.TextBox txtStyle;
        private Win.UI.Label labBrand;
        private Win.UI.TextBox txtBrand;
        private Win.UI.DateBox dateBuyerDel;
        private Win.UI.DateBox dateSCIDel;
    }
}
