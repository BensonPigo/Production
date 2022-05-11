namespace Sci.Production.Packing
{
    partial class P19_PackingErrorRecord
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
            this.labPoNo = new Sci.Win.UI.Label();
            this.txtPONo = new Sci.Win.UI.TextBox();
            this.labStyle = new Sci.Win.UI.Label();
            this.txtStyle = new Sci.Win.UI.TextBox();
            this.labBrand = new Sci.Win.UI.Label();
            this.txtBrand = new Sci.Win.UI.TextBox();
            this.dateBuyerDel = new Sci.Win.UI.DateBox();
            this.labErrQty = new Sci.Win.UI.Label();
            this.txtErrQty = new Sci.Win.UI.TextBox();
            this.labPAckQty = new Sci.Win.UI.Label();
            this.txtPackQty = new Sci.Win.UI.TextBox();
            this.labCTNNo = new Sci.Win.UI.Label();
            this.txtCTNNo = new Sci.Win.UI.TextBox();
            this.labPackID = new Sci.Win.UI.Label();
            this.txtPackID = new Sci.Win.UI.TextBox();
            this.labSeasno = new Sci.Win.UI.Label();
            this.txtSeason = new Sci.Win.UI.TextBox();
            this.labErrorType = new Sci.Win.UI.Label();
            this.txtErrorType = new Sci.Win.UI.TextBox();
            this.labDestination = new Sci.Win.UI.Label();
            this.txtDestination = new Sci.Win.UI.TextBox();
            this.labRemark = new Sci.Win.UI.Label();
            this.txtRemark = new Sci.Win.UI.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Size = new System.Drawing.Size(930, 40);
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(12, 115);
            this.gridcont.Size = new System.Drawing.Size(906, 332);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(840, 5);
            this.undo.Visible = false;
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(760, 5);
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSPNo.IsSupportEditMode = false;
            this.txtSPNo.Location = new System.Drawing.Point(346, 9);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.ReadOnly = true;
            this.txtSPNo.Size = new System.Drawing.Size(141, 23);
            this.txtSPNo.TabIndex = 133;
            // 
            // labSpNo
            // 
            this.labSpNo.Location = new System.Drawing.Point(265, 9);
            this.labSpNo.Name = "labSpNo";
            this.labSpNo.Size = new System.Drawing.Size(78, 23);
            this.labSpNo.TabIndex = 135;
            this.labSpNo.Text = "SP#";
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Location = new System.Drawing.Point(500, 87);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(110, 23);
            this.labelBuyerDelivery.TabIndex = 136;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            // 
            // labPoNo
            // 
            this.labPoNo.Location = new System.Drawing.Point(265, 35);
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
            this.txtPONo.Location = new System.Drawing.Point(346, 35);
            this.txtPONo.Name = "txtPONo";
            this.txtPONo.ReadOnly = true;
            this.txtPONo.Size = new System.Drawing.Size(141, 23);
            this.txtPONo.TabIndex = 139;
            // 
            // labStyle
            // 
            this.labStyle.Location = new System.Drawing.Point(265, 61);
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
            this.txtStyle.Location = new System.Drawing.Point(346, 61);
            this.txtStyle.Name = "txtStyle";
            this.txtStyle.ReadOnly = true;
            this.txtStyle.Size = new System.Drawing.Size(141, 23);
            this.txtStyle.TabIndex = 141;
            // 
            // labBrand
            // 
            this.labBrand.Location = new System.Drawing.Point(500, 9);
            this.labBrand.Name = "labBrand";
            this.labBrand.Size = new System.Drawing.Size(110, 23);
            this.labBrand.TabIndex = 144;
            this.labBrand.Text = "Brand";
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtBrand.IsSupportEditMode = false;
            this.txtBrand.Location = new System.Drawing.Point(613, 9);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.ReadOnly = true;
            this.txtBrand.Size = new System.Drawing.Size(90, 23);
            this.txtBrand.TabIndex = 143;
            // 
            // dateBuyerDel
            // 
            this.dateBuyerDel.IsSupportEditMode = false;
            this.dateBuyerDel.Location = new System.Drawing.Point(613, 87);
            this.dateBuyerDel.Name = "dateBuyerDel";
            this.dateBuyerDel.ReadOnly = true;
            this.dateBuyerDel.Size = new System.Drawing.Size(131, 23);
            this.dateBuyerDel.TabIndex = 145;
            // 
            // labErrQty
            // 
            this.labErrQty.Location = new System.Drawing.Point(26, 87);
            this.labErrQty.Name = "labErrQty";
            this.labErrQty.Size = new System.Drawing.Size(78, 23);
            this.labErrQty.TabIndex = 154;
            this.labErrQty.Text = "ErrQty";
            // 
            // txtErrQty
            // 
            this.txtErrQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtErrQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtErrQty.IsSupportEditMode = false;
            this.txtErrQty.Location = new System.Drawing.Point(107, 87);
            this.txtErrQty.Name = "txtErrQty";
            this.txtErrQty.ReadOnly = true;
            this.txtErrQty.Size = new System.Drawing.Size(141, 23);
            this.txtErrQty.TabIndex = 153;
            // 
            // labPAckQty
            // 
            this.labPAckQty.Location = new System.Drawing.Point(26, 61);
            this.labPAckQty.Name = "labPAckQty";
            this.labPAckQty.Size = new System.Drawing.Size(78, 23);
            this.labPAckQty.TabIndex = 152;
            this.labPAckQty.Text = "Pack Qty";
            // 
            // txtPackQty
            // 
            this.txtPackQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtPackQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtPackQty.IsSupportEditMode = false;
            this.txtPackQty.Location = new System.Drawing.Point(107, 61);
            this.txtPackQty.Name = "txtPackQty";
            this.txtPackQty.ReadOnly = true;
            this.txtPackQty.Size = new System.Drawing.Size(141, 23);
            this.txtPackQty.TabIndex = 151;
            // 
            // labCTNNo
            // 
            this.labCTNNo.Location = new System.Drawing.Point(26, 35);
            this.labCTNNo.Name = "labCTNNo";
            this.labCTNNo.Size = new System.Drawing.Size(78, 23);
            this.labCTNNo.TabIndex = 150;
            this.labCTNNo.Text = "CTN#";
            // 
            // txtCTNNo
            // 
            this.txtCTNNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtCTNNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtCTNNo.IsSupportEditMode = false;
            this.txtCTNNo.Location = new System.Drawing.Point(107, 35);
            this.txtCTNNo.Name = "txtCTNNo";
            this.txtCTNNo.ReadOnly = true;
            this.txtCTNNo.Size = new System.Drawing.Size(141, 23);
            this.txtCTNNo.TabIndex = 149;
            // 
            // labPackID
            // 
            this.labPackID.Location = new System.Drawing.Point(26, 9);
            this.labPackID.Name = "labPackID";
            this.labPackID.Size = new System.Drawing.Size(78, 23);
            this.labPackID.TabIndex = 148;
            this.labPackID.Text = "Pack ID";
            // 
            // txtPackID
            // 
            this.txtPackID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtPackID.IsSupportEditMode = false;
            this.txtPackID.Location = new System.Drawing.Point(107, 9);
            this.txtPackID.Name = "txtPackID";
            this.txtPackID.ReadOnly = true;
            this.txtPackID.Size = new System.Drawing.Size(141, 23);
            this.txtPackID.TabIndex = 147;
            // 
            // labSeasno
            // 
            this.labSeasno.Location = new System.Drawing.Point(265, 87);
            this.labSeasno.Name = "labSeasno";
            this.labSeasno.Size = new System.Drawing.Size(78, 23);
            this.labSeasno.TabIndex = 156;
            this.labSeasno.Text = "Season";
            // 
            // txtSeason
            // 
            this.txtSeason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSeason.IsSupportEditMode = false;
            this.txtSeason.Location = new System.Drawing.Point(346, 87);
            this.txtSeason.Name = "txtSeason";
            this.txtSeason.ReadOnly = true;
            this.txtSeason.Size = new System.Drawing.Size(141, 23);
            this.txtSeason.TabIndex = 155;
            // 
            // labErrorType
            // 
            this.labErrorType.Location = new System.Drawing.Point(500, 35);
            this.labErrorType.Name = "labErrorType";
            this.labErrorType.Size = new System.Drawing.Size(110, 23);
            this.labErrorType.TabIndex = 158;
            this.labErrorType.Text = "ErrorType";
            // 
            // txtErrorType
            // 
            this.txtErrorType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtErrorType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtErrorType.IsSupportEditMode = false;
            this.txtErrorType.Location = new System.Drawing.Point(613, 35);
            this.txtErrorType.Name = "txtErrorType";
            this.txtErrorType.ReadOnly = true;
            this.txtErrorType.Size = new System.Drawing.Size(90, 23);
            this.txtErrorType.TabIndex = 157;
            // 
            // labDestination
            // 
            this.labDestination.Location = new System.Drawing.Point(500, 61);
            this.labDestination.Name = "labDestination";
            this.labDestination.Size = new System.Drawing.Size(110, 23);
            this.labDestination.TabIndex = 160;
            this.labDestination.Text = "Destination";
            // 
            // txtDestination
            // 
            this.txtDestination.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtDestination.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtDestination.IsSupportEditMode = false;
            this.txtDestination.Location = new System.Drawing.Point(613, 61);
            this.txtDestination.Name = "txtDestination";
            this.txtDestination.ReadOnly = true;
            this.txtDestination.Size = new System.Drawing.Size(141, 23);
            this.txtDestination.TabIndex = 159;
            // 
            // labRemark
            // 
            this.labRemark.Location = new System.Drawing.Point(706, 9);
            this.labRemark.Name = "labRemark";
            this.labRemark.Size = new System.Drawing.Size(70, 23);
            this.labRemark.TabIndex = 162;
            this.labRemark.Text = "Remark";
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtRemark.IsSupportEditMode = false;
            this.txtRemark.Location = new System.Drawing.Point(779, 9);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.ReadOnly = true;
            this.txtRemark.Size = new System.Drawing.Size(141, 23);
            this.txtRemark.TabIndex = 161;
            // 
            // P19_PackingErrorRecord
            // 
            this.ClientSize = new System.Drawing.Size(930, 497);
            this.Controls.Add(this.labRemark);
            this.Controls.Add(this.txtRemark);
            this.Controls.Add(this.labDestination);
            this.Controls.Add(this.txtDestination);
            this.Controls.Add(this.labErrorType);
            this.Controls.Add(this.txtErrorType);
            this.Controls.Add(this.labSeasno);
            this.Controls.Add(this.txtSeason);
            this.Controls.Add(this.labErrQty);
            this.Controls.Add(this.txtErrQty);
            this.Controls.Add(this.labPAckQty);
            this.Controls.Add(this.txtPackQty);
            this.Controls.Add(this.labCTNNo);
            this.Controls.Add(this.txtCTNNo);
            this.Controls.Add(this.labPackID);
            this.Controls.Add(this.txtPackID);
            this.Controls.Add(this.dateBuyerDel);
            this.Controls.Add(this.labBrand);
            this.Controls.Add(this.txtBrand);
            this.Controls.Add(this.labStyle);
            this.Controls.Add(this.txtStyle);
            this.Controls.Add(this.labPoNo);
            this.Controls.Add(this.txtPONo);
            this.Controls.Add(this.labelBuyerDelivery);
            this.Controls.Add(this.labSpNo);
            this.Controls.Add(this.txtSPNo);
            this.EditMode = true;
            this.GridPopUp = false;
            this.KeyField1 = "PackID";
            this.KeyField2 = "CTN";
            this.Name = "P19_PackingErrorRecord";
            this.OnLineHelpID = "Sci.Win.Subs.Input4";
            this.Text = "Packing Error Record";
            this.WorkAlias = "PackingErrorRecord";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            this.Controls.SetChildIndex(this.txtSPNo, 0);
            this.Controls.SetChildIndex(this.labSpNo, 0);
            this.Controls.SetChildIndex(this.labelBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.txtPONo, 0);
            this.Controls.SetChildIndex(this.labPoNo, 0);
            this.Controls.SetChildIndex(this.txtStyle, 0);
            this.Controls.SetChildIndex(this.labStyle, 0);
            this.Controls.SetChildIndex(this.txtBrand, 0);
            this.Controls.SetChildIndex(this.labBrand, 0);
            this.Controls.SetChildIndex(this.dateBuyerDel, 0);
            this.Controls.SetChildIndex(this.txtPackID, 0);
            this.Controls.SetChildIndex(this.labPackID, 0);
            this.Controls.SetChildIndex(this.txtCTNNo, 0);
            this.Controls.SetChildIndex(this.labCTNNo, 0);
            this.Controls.SetChildIndex(this.txtPackQty, 0);
            this.Controls.SetChildIndex(this.labPAckQty, 0);
            this.Controls.SetChildIndex(this.txtErrQty, 0);
            this.Controls.SetChildIndex(this.labErrQty, 0);
            this.Controls.SetChildIndex(this.txtSeason, 0);
            this.Controls.SetChildIndex(this.labSeasno, 0);
            this.Controls.SetChildIndex(this.txtErrorType, 0);
            this.Controls.SetChildIndex(this.labErrorType, 0);
            this.Controls.SetChildIndex(this.txtDestination, 0);
            this.Controls.SetChildIndex(this.labDestination, 0);
            this.Controls.SetChildIndex(this.txtRemark, 0);
            this.Controls.SetChildIndex(this.labRemark, 0);
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
        private Win.UI.Label labPoNo;
        private Win.UI.TextBox txtPONo;
        private Win.UI.Label labStyle;
        private Win.UI.TextBox txtStyle;
        private Win.UI.Label labBrand;
        private Win.UI.TextBox txtBrand;
        private Win.UI.DateBox dateBuyerDel;
        private Win.UI.Label labErrQty;
        private Win.UI.TextBox txtErrQty;
        private Win.UI.Label labPAckQty;
        private Win.UI.TextBox txtPackQty;
        private Win.UI.Label labCTNNo;
        private Win.UI.TextBox txtCTNNo;
        private Win.UI.Label labPackID;
        private Win.UI.TextBox txtPackID;
        private Win.UI.Label labSeasno;
        private Win.UI.TextBox txtSeason;
        private Win.UI.Label labErrorType;
        private Win.UI.TextBox txtErrorType;
        private Win.UI.Label labDestination;
        private Win.UI.TextBox txtDestination;
        private Win.UI.Label labRemark;
        private Win.UI.TextBox txtRemark;
    }
}
