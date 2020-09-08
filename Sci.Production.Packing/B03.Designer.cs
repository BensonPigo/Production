namespace Sci.Production.Packing
{
    partial class B03
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
            this.txtStickerComb = new Sci.Win.UI.TextBox();
            this.label4 = new Sci.Win.UI.Label();
            this.comboCategory = new Sci.Win.UI.ComboBox();
            this.label2 = new Sci.Win.UI.Label();
            this.txtCTNRefno = new Sci.Win.UI.TextBox();
            this.txtbrand1 = new Sci.Production.Class.Txtbrand();
            this.label3 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.pictureBox1 = new Sci.Win.UI.PictureBox();
            this.label10 = new Sci.Win.UI.Label();
            this.chkIsMixPack = new Sci.Win.UI.CheckBox();
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.chkIsMixPack);
            this.masterpanel.Controls.Add(this.pictureBox1);
            this.masterpanel.Controls.Add(this.label10);
            this.masterpanel.Controls.Add(this.txtStickerComb);
            this.masterpanel.Controls.Add(this.label4);
            this.masterpanel.Controls.Add(this.comboCategory);
            this.masterpanel.Controls.Add(this.label2);
            this.masterpanel.Controls.Add(this.txtCTNRefno);
            this.masterpanel.Controls.Add(this.txtbrand1);
            this.masterpanel.Controls.Add(this.label3);
            this.masterpanel.Controls.Add(this.label1);
            this.masterpanel.Size = new System.Drawing.Size(947, 164);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.label1, 0);
            this.masterpanel.Controls.SetChildIndex(this.label3, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtbrand1, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtCTNRefno, 0);
            this.masterpanel.Controls.SetChildIndex(this.label2, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboCategory, 0);
            this.masterpanel.Controls.SetChildIndex(this.label4, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtStickerComb, 0);
            this.masterpanel.Controls.SetChildIndex(this.label10, 0);
            this.masterpanel.Controls.SetChildIndex(this.pictureBox1, 0);
            this.masterpanel.Controls.SetChildIndex(this.chkIsMixPack, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 164);
            this.detailpanel.Size = new System.Drawing.Size(947, 302);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(830, 129);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(947, 302);
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
            this.detail.Size = new System.Drawing.Size(947, 504);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(947, 466);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 466);
            this.detailbtm.Size = new System.Drawing.Size(947, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(947, 504);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(955, 533);
            // 
            // txtStickerComb
            // 
            this.txtStickerComb.BackColor = System.Drawing.Color.White;
            this.txtStickerComb.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtStickerComb.Location = new System.Drawing.Point(204, 77);
            this.txtStickerComb.Name = "txtStickerComb";
            this.txtStickerComb.Size = new System.Drawing.Size(125, 23);
            this.txtStickerComb.TabIndex = 3;
            this.txtStickerComb.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtStickerComb_PopUp);
            this.txtStickerComb.Validating += new System.ComponentModel.CancelEventHandler(this.TxtStickerComb_Validating);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(20, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(181, 23);
            this.label4.TabIndex = 38;
            this.label4.Text = "Shipping Mark Combination";
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Category", true));
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(204, 44);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(125, 24);
            this.comboCategory.TabIndex = 2;
            this.comboCategory.SelectedIndexChanged += new System.EventHandler(this.ComboCategory_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(20, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(181, 23);
            this.label2.TabIndex = 36;
            this.label2.Text = "Category";
            // 
            // txtCTNRefno
            // 
            this.txtCTNRefno.BackColor = System.Drawing.Color.White;
            this.txtCTNRefno.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CTNRefno", true));
            this.txtCTNRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCTNRefno.Location = new System.Drawing.Point(204, 108);
            this.txtCTNRefno.Name = "txtCTNRefno";
            this.txtCTNRefno.Size = new System.Drawing.Size(125, 23);
            this.txtCTNRefno.TabIndex = 4;
            this.txtCTNRefno.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtCTNRefno_PopUp);
            this.txtCTNRefno.Validating += new System.ComponentModel.CancelEventHandler(this.TxtCTNRefno_Validating);
            // 
            // txtbrand1
            // 
            this.txtbrand1.BackColor = System.Drawing.Color.White;
            this.txtbrand1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BrandID", true));
            this.txtbrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand1.Location = new System.Drawing.Point(204, 15);
            this.txtbrand1.Name = "txtbrand1";
            this.txtbrand1.Size = new System.Drawing.Size(125, 23);
            this.txtbrand1.TabIndex = 1;
            this.txtbrand1.Validating += new System.ComponentModel.CancelEventHandler(this.Txtbrand1_Validating);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(20, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(181, 23);
            this.label3.TabIndex = 34;
            this.label3.Text = "CTN Refno";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(20, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(181, 23);
            this.label1.TabIndex = 33;
            this.label1.Text = "Brand";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = null;
            this.pictureBox1.Location = new System.Drawing.Point(455, 15);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(215, 124);
            this.pictureBox1.TabIndex = 40;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.WaitOnLoad = true;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(342, 15);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(110, 23);
            this.label10.TabIndex = 39;
            this.label10.Text = "Example ";
            // 
            // chkIsMixPack
            // 
            this.chkIsMixPack.AutoSize = true;
            this.chkIsMixPack.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.chkIsMixPack.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.chkIsMixPack.IsSupportEditMode = false;
            this.chkIsMixPack.Location = new System.Drawing.Point(345, 77);
            this.chkIsMixPack.Name = "chkIsMixPack";
            this.chkIsMixPack.ReadOnly = true;
            this.chkIsMixPack.Size = new System.Drawing.Size(107, 24);
            this.chkIsMixPack.TabIndex = 41;
            this.chkIsMixPack.Text = "Is Mix Pack";
            this.chkIsMixPack.UseVisualStyleBackColor = true;
            // 
            // B03
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 566);
            this.GridAlias = "ShippingMarkPicture_Detail";
            this.GridNew = 0;
            this.GridUniqueKey = "ShippingMarkPictureUkey,ShippingMarkTypeUkey";
            this.IsDeleteOnBrowse = false;
            this.IsGridIconVisible = false;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportJunk = true;
            this.IsSupportPrint = false;
            this.IsSupportUnJunk = true;
            this.KeyField1 = "Ukey";
            this.KeyField2 = "ShippingMarkPictureUkey";
            this.Name = "B03";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "B03. Shipping Mark Pic Setting (for GenSong)";
            this.WorkAlias = "ShippingMarkPicture";
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.TextBox txtStickerComb;
        private Win.UI.Label label4;
        private Win.UI.ComboBox comboCategory;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtCTNRefno;
        private Class.Txtbrand txtbrand1;
        private Win.UI.Label label3;
        private Win.UI.Label label1;
        private Win.UI.PictureBox pictureBox1;
        private Win.UI.Label label10;
        private Win.UI.CheckBox chkIsMixPack;
    }
}