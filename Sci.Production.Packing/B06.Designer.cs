namespace Sci.Production.Packing
{
    partial class B06
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
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.comboCategory = new Sci.Win.UI.ComboBox();
            this.txtID = new Sci.Win.UI.TextBox();
            this.checkIsDefault = new Sci.Win.UI.CheckBox();
            this.chkIsMixPack = new Sci.Win.UI.CheckBox();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
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
            this.masterpanel.Controls.Add(this.chkIsMixPack);
            this.masterpanel.Controls.Add(this.checkIsDefault);
            this.masterpanel.Controls.Add(this.txtID);
            this.masterpanel.Controls.Add(this.comboCategory);
            this.masterpanel.Controls.Add(this.txtbrand);
            this.masterpanel.Controls.Add(this.label3);
            this.masterpanel.Controls.Add(this.label2);
            this.masterpanel.Controls.Add(this.label1);
            this.masterpanel.Size = new System.Drawing.Size(792, 106);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.label1, 0);
            this.masterpanel.Controls.SetChildIndex(this.label2, 0);
            this.masterpanel.Controls.SetChildIndex(this.label3, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtbrand, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboCategory, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtID, 0);
            this.masterpanel.Controls.SetChildIndex(this.checkIsDefault, 0);
            this.masterpanel.Controls.SetChildIndex(this.chkIsMixPack, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 106);
            this.detailpanel.Size = new System.Drawing.Size(792, 284);
            // 
            // gridicon
            // 
            this.gridicon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.gridicon.Location = new System.Drawing.Point(380, 71);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(792, 284);
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
            this.detail.Size = new System.Drawing.Size(792, 428);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(792, 390);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 390);
            this.detailbtm.Size = new System.Drawing.Size(792, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(792, 428);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(800, 457);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(19, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(181, 23);
            this.label3.TabIndex = 17;
            this.label3.Text = "Shipping Mark Combination";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(19, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(181, 23);
            this.label2.TabIndex = 16;
            this.label2.Text = "Category";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(19, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(181, 23);
            this.label1.TabIndex = 15;
            this.label1.Text = "Brand";
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Category", true));
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(203, 42);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(140, 24);
            this.comboCategory.TabIndex = 2;
            this.comboCategory.SelectedIndexChanged += new System.EventHandler(this.ComboCategory_SelectedIndexChanged);
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.White;
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtID.Location = new System.Drawing.Point(203, 70);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(140, 23);
            this.txtID.TabIndex = 3;
            // 
            // checkIsDefault
            // 
            this.checkIsDefault.AutoSize = true;
            this.checkIsDefault.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsDefault", true));
            this.checkIsDefault.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkIsDefault.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkIsDefault.IsSupportEditMode = false;
            this.checkIsDefault.Location = new System.Drawing.Point(352, 42);
            this.checkIsDefault.Name = "checkIsDefault";
            this.checkIsDefault.ReadOnly = true;
            this.checkIsDefault.Size = new System.Drawing.Size(97, 24);
            this.checkIsDefault.TabIndex = 5;
            this.checkIsDefault.Text = "Is Default";
            this.checkIsDefault.UseVisualStyleBackColor = true;
            // 
            // chkIsMixPack
            // 
            this.chkIsMixPack.AutoSize = true;
            this.chkIsMixPack.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsMixPack", true));
            this.chkIsMixPack.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.chkIsMixPack.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.chkIsMixPack.IsSupportEditMode = false;
            this.chkIsMixPack.Location = new System.Drawing.Point(352, 14);
            this.chkIsMixPack.Name = "chkIsMixPack";
            this.chkIsMixPack.ReadOnly = true;
            this.chkIsMixPack.Size = new System.Drawing.Size(107, 24);
            this.chkIsMixPack.TabIndex = 4;
            this.chkIsMixPack.Text = "Is Mix Pack";
            this.chkIsMixPack.UseVisualStyleBackColor = true;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BrandID", true));
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(203, 14);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(140, 23);
            this.txtbrand.TabIndex = 1;
            this.txtbrand.Validating += new System.ComponentModel.CancelEventHandler(this.Txtbrand_Validating);
            // 
            // B06
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 490);
            this.EnableGridJunkColor = true;
            this.GridAlias = "ShippingMarkCombination_Detail";
            this.GridNew = 0;
            this.GridUniqueKey = "ShippingMarkCombinationUkey,ShippingMarkTypeUkey";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportJunk = true;
            this.IsSupportPrint = false;
            this.IsSupportUnJunk = true;
            this.KeyField1 = "Ukey";
            this.KeyField2 = "ShippingMarkCombinationUkey";
            this.Name = "B06";
            this.OnLineHelpID = "Sci.Win.Tems.Input2";
            this.Text = "B06. Shipping Mark Combination";
            this.WorkAlias = "ShippingMarkCombination";
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

        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Class.Txtbrand txtbrand;
        private Win.UI.ComboBox comboCategory;
        private Win.UI.TextBox txtID;
        private Win.UI.CheckBox chkIsMixPack;
        private Win.UI.CheckBox checkIsDefault;
    }
}