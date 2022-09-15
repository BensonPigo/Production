namespace Sci.Production.Shipping
{
    partial class B01
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
            this.labelBrand = new Sci.Win.UI.Label();
            this.editAddress = new Sci.Win.UI.EditBox();
            this.labelAddress = new Sci.Win.UI.Label();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.editBrandID = new Sci.Win.UI.EditBox();
            this.txtWhseCode = new Sci.Win.UI.TextBox();
            this.labPortWHCode = new Sci.Win.UI.Label();
            this.txtWhseName = new Sci.Win.UI.TextBox();
            this.labName = new Sci.Win.UI.Label();
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
            this.masterpanel.Controls.Add(this.txtWhseName);
            this.masterpanel.Controls.Add(this.labName);
            this.masterpanel.Controls.Add(this.txtWhseCode);
            this.masterpanel.Controls.Add(this.labPortWHCode);
            this.masterpanel.Controls.Add(this.editBrandID);
            this.masterpanel.Controls.Add(this.checkJunk);
            this.masterpanel.Controls.Add(this.editAddress);
            this.masterpanel.Controls.Add(this.labelAddress);
            this.masterpanel.Controls.Add(this.labelBrand);
            this.masterpanel.Size = new System.Drawing.Size(892, 168);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelBrand, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelAddress, 0);
            this.masterpanel.Controls.SetChildIndex(this.editAddress, 0);
            this.masterpanel.Controls.SetChildIndex(this.checkJunk, 0);
            this.masterpanel.Controls.SetChildIndex(this.editBrandID, 0);
            this.masterpanel.Controls.SetChildIndex(this.labPortWHCode, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtWhseCode, 0);
            this.masterpanel.Controls.SetChildIndex(this.labName, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtWhseName, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 168);
            this.detailpanel.Size = new System.Drawing.Size(892, 181);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(709, 130);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(821, 1);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(892, 181);
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
            this.browse.Size = new System.Drawing.Size(824, 429);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(832, 458);
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
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(377, 19);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(90, 23);
            this.labelBrand.TabIndex = 11;
            this.labelBrand.Text = "Brand";
            // 
            // editAddress
            // 
            this.editAddress.BackColor = System.Drawing.Color.White;
            this.editAddress.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Address", true));
            this.editAddress.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editAddress.Location = new System.Drawing.Point(108, 84);
            this.editAddress.Multiline = true;
            this.editAddress.Name = "editAddress";
            this.editAddress.Size = new System.Drawing.Size(301, 73);
            this.editAddress.TabIndex = 6;
            // 
            // labelAddress
            // 
            this.labelAddress.Location = new System.Drawing.Point(7, 84);
            this.labelAddress.Name = "labelAddress";
            this.labelAddress.Size = new System.Drawing.Size(98, 23);
            this.labelAddress.TabIndex = 20;
            this.labelAddress.Text = "Address";
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(298, 19);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 21;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // editBrandID
            // 
            this.editBrandID.BackColor = System.Drawing.Color.White;
            this.editBrandID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BrandID", true));
            this.editBrandID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBrandID.Location = new System.Drawing.Point(470, 19);
            this.editBrandID.Multiline = true;
            this.editBrandID.Name = "editBrandID";
            this.editBrandID.Size = new System.Drawing.Size(339, 54);
            this.editBrandID.TabIndex = 63;
            this.editBrandID.TabStop = false;
            this.editBrandID.PopUp += new System.EventHandler<Sci.Win.UI.EditBoxPopUpEventArgs>(this.editPOCombo_PopUp);
            this.editBrandID.Validating += new System.ComponentModel.CancelEventHandler(this.editBrandID_Validating);
            // 
            // txtWhseCode
            // 
            this.txtWhseCode.BackColor = System.Drawing.Color.White;
            this.txtWhseCode.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtWhseCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "WhseCode", true));
            this.txtWhseCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtWhseCode.Location = new System.Drawing.Point(108, 19);
            this.txtWhseCode.Name = "txtWhseCode";
            this.txtWhseCode.Size = new System.Drawing.Size(173, 23);
            this.txtWhseCode.TabIndex = 64;
            // 
            // labPortWHCode
            // 
            this.labPortWHCode.Location = new System.Drawing.Point(7, 19);
            this.labPortWHCode.Name = "labPortWHCode";
            this.labPortWHCode.Size = new System.Drawing.Size(98, 23);
            this.labPortWHCode.TabIndex = 65;
            this.labPortWHCode.Text = "Port/WH Code";
            // 
            // txtWhseName
            // 
            this.txtWhseName.BackColor = System.Drawing.Color.White;
            this.txtWhseName.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtWhseName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "WhseName", true));
            this.txtWhseName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtWhseName.Location = new System.Drawing.Point(108, 50);
            this.txtWhseName.Name = "txtWhseName";
            this.txtWhseName.Size = new System.Drawing.Size(173, 23);
            this.txtWhseName.TabIndex = 66;
            // 
            // labName
            // 
            this.labName.Location = new System.Drawing.Point(7, 50);
            this.labName.Name = "labName";
            this.labName.Size = new System.Drawing.Size(98, 23);
            this.labName.TabIndex = 67;
            this.labName.Text = "Name";
            // 
            // B01
            // 
            this.ClientSize = new System.Drawing.Size(832, 491);
            this.DefaultControl = "txtWhseCode";
            this.DefaultControlForEdit = "txtWhseCode";
            this.EditMode = true;
            this.GridAlias = "ForwarderWarehouse_Detail";
            this.GridUniqueKey = "ShipModeID";
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.KeyField1 = "ID";
            this.Name = "B01";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "B01. Forwarder Warehouse";
            this.UniqueExpress = "ID";
            this.WorkAlias = "ForwarderWarehouse";
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
        private Win.UI.Label labelBrand;
        private Win.UI.EditBox editAddress;
        private Win.UI.Label labelAddress;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.TextBox txtWhseName;
        private Win.UI.Label labName;
        private Win.UI.TextBox txtWhseCode;
        private Win.UI.Label labPortWHCode;
        private Win.UI.EditBox editBrandID;
    }
}
