namespace Sci.Production.Packing
{
    partial class B05
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
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.checkIsSSCC = new Sci.Win.UI.CheckBox();
            this.txtID = new Sci.Win.UI.TextBox();
            this.comboCategory = new Sci.Win.UI.ComboBox();
            this.label4 = new Sci.Win.UI.Label();
            this.chkIsTemplate = new Sci.Win.UI.CheckBox();
            this.btnTemplateUpload = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(792, 388);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.btnTemplateUpload);
            this.detailcont.Controls.Add(this.chkIsTemplate);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.comboCategory);
            this.detailcont.Controls.Add(this.txtID);
            this.detailcont.Controls.Add(this.checkIsSSCC);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.txtbrand);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Size = new System.Drawing.Size(792, 350);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 350);
            this.detailbtm.Size = new System.Drawing.Size(792, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(792, 388);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(800, 417);
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BrandID", true));
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(171, 25);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(140, 23);
            this.txtbrand.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(35, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(133, 23);
            this.label1.TabIndex = 12;
            this.label1.Text = "Brand";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(35, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 23);
            this.label2.TabIndex = 13;
            this.label2.Text = "Category";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(35, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(133, 23);
            this.label3.TabIndex = 14;
            this.label3.Text = "Shipping Mark Type";
            // 
            // checkIsSSCC
            // 
            this.checkIsSSCC.AutoSize = true;
            this.checkIsSSCC.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsSSCC", true));
            this.checkIsSSCC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkIsSSCC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkIsSSCC.IsSupportEditMode = false;
            this.checkIsSSCC.Location = new System.Drawing.Point(319, 114);
            this.checkIsSSCC.Name = "checkIsSSCC";
            this.checkIsSSCC.ReadOnly = true;
            this.checkIsSSCC.Size = new System.Drawing.Size(89, 24);
            this.checkIsSSCC.TabIndex = 4;
            this.checkIsSSCC.Text = "Is SSCC";
            this.checkIsSSCC.UseVisualStyleBackColor = true;
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.White;
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtID.Location = new System.Drawing.Point(171, 115);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(140, 23);
            this.txtID.TabIndex = 3;
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Category", true));
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(171, 68);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.OldText = "";
            this.comboCategory.Size = new System.Drawing.Size(140, 24);
            this.comboCategory.TabIndex = 2;
            this.comboCategory.SelectedIndexChanged += new System.EventHandler(this.ComboCategory_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(35, 157);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(133, 23);
            this.label4.TabIndex = 15;
            this.label4.Text = "Shipping Mark From";
            // 
            // chkIsTemplate
            // 
            this.chkIsTemplate.AutoSize = true;
            this.chkIsTemplate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "FromTemplate", true));
            this.chkIsTemplate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.chkIsTemplate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.chkIsTemplate.IsSupportEditMode = false;
            this.chkIsTemplate.Location = new System.Drawing.Point(171, 157);
            this.chkIsTemplate.Name = "chkIsTemplate";
            this.chkIsTemplate.ReadOnly = true;
            this.chkIsTemplate.Size = new System.Drawing.Size(94, 24);
            this.chkIsTemplate.TabIndex = 16;
            this.chkIsTemplate.Text = "Template";
            this.chkIsTemplate.UseVisualStyleBackColor = true;
            // 
            // btnTemplateUpload
            // 
            this.btnTemplateUpload.Location = new System.Drawing.Point(171, 187);
            this.btnTemplateUpload.Name = "btnTemplateUpload";
            this.btnTemplateUpload.Size = new System.Drawing.Size(140, 30);
            this.btnTemplateUpload.TabIndex = 17;
            this.btnTemplateUpload.Text = "Template Upload";
            this.btnTemplateUpload.UseVisualStyleBackColor = true;
            this.btnTemplateUpload.Click += new System.EventHandler(this.BtnTemplateUpload_Click);
            // 
            // B05
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.EnableGridJunkColor = true;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportJunk = true;
            this.IsSupportPrint = false;
            this.IsSupportUnJunk = true;
            this.Name = "B05";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B05. Shipping Mark Type";
            this.WorkAlias = "ShippingMarkType";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Class.Txtbrand txtbrand;
        private Win.UI.Label label1;
        private Win.UI.CheckBox checkIsSSCC;
        private Win.UI.ComboBox comboCategory;
        private Win.UI.TextBox txtID;
        private Win.UI.CheckBox chkIsTemplate;
        private Win.UI.Label label4;
        private Win.UI.Button btnTemplateUpload;
    }
}