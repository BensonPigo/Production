namespace Sci.Production.Basic
{
    partial class B13
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
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.displayArtwork = new Sci.Win.UI.DisplayBox();
            this.displayProduction = new Sci.Win.UI.DisplayBox();
            this.comboMaterialType = new Sci.Win.UI.ComboBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.checkExtend = new Sci.Win.UI.CheckBox();
            this.checkZipper = new Sci.Win.UI.CheckBox();
            this.checkIsICRItem = new Sci.Win.UI.CheckBox();
            this.label6 = new Sci.Win.UI.Label();
            this.txtDropdownlistIssueType = new Sci.Production.Class.txtdropdownlist();
            this.checkIsTrimCardOther = new Sci.Win.UI.CheckBox();
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
            this.detail.Size = new System.Drawing.Size(828, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.checkIsTrimCardOther);
            this.detailcont.Controls.Add(this.txtDropdownlistIssueType);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.checkIsICRItem);
            this.detailcont.Controls.Add(this.checkZipper);
            this.detailcont.Controls.Add(this.checkExtend);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.comboMaterialType);
            this.detailcont.Controls.Add(this.displayProduction);
            this.detailcont.Controls.Add(this.displayArtwork);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Size = new System.Drawing.Size(828, 357);
            this.detailcont.TabIndex = 0;
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(828, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(828, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(836, 424);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(473, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.TabIndex = 1;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(425, 13);
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(70, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "Material Type";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(70, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 23);
            this.label4.TabIndex = 1;
            this.label4.Text = "Artwork";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(70, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 23);
            this.label5.TabIndex = 2;
            this.label5.Text = "Production";
            // 
            // displayArtwork
            // 
            this.displayArtwork.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayArtwork.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayArtwork.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayArtwork.Location = new System.Drawing.Point(169, 70);
            this.displayArtwork.Name = "displayArtwork";
            this.displayArtwork.Size = new System.Drawing.Size(170, 23);
            this.displayArtwork.TabIndex = 1;
            // 
            // displayProduction
            // 
            this.displayProduction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayProduction.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ProductionType", true));
            this.displayProduction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayProduction.Location = new System.Drawing.Point(169, 110);
            this.displayProduction.Name = "displayProduction";
            this.displayProduction.Size = new System.Drawing.Size(100, 23);
            this.displayProduction.TabIndex = 2;
            // 
            // comboMaterialType
            // 
            this.comboMaterialType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.comboMaterialType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Type", true));
            this.comboMaterialType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.comboMaterialType.FormattingEnabled = true;
            this.comboMaterialType.IsSupportUnselect = true;
            this.comboMaterialType.Location = new System.Drawing.Point(169, 30);
            this.comboMaterialType.Name = "comboMaterialType";
            this.comboMaterialType.ReadOnly = true;
            this.comboMaterialType.Size = new System.Drawing.Size(100, 24);
            this.comboMaterialType.TabIndex = 0;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(400, 30);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 4;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // checkExtend
            // 
            this.checkExtend.AutoSize = true;
            this.checkExtend.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsExtensionUnit", true));
            this.checkExtend.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkExtend.Location = new System.Drawing.Point(400, 70);
            this.checkExtend.Name = "checkExtend";
            this.checkExtend.Size = new System.Drawing.Size(70, 21);
            this.checkExtend.TabIndex = 5;
            this.checkExtend.Text = "Extend";
            this.checkExtend.UseVisualStyleBackColor = true;
            // 
            // checkZipper
            // 
            this.checkZipper.AutoSize = true;
            this.checkZipper.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CheckZipper", true));
            this.checkZipper.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkZipper.Location = new System.Drawing.Point(400, 110);
            this.checkZipper.Name = "checkZipper";
            this.checkZipper.Size = new System.Drawing.Size(111, 21);
            this.checkZipper.TabIndex = 6;
            this.checkZipper.Text = "Check Zipper";
            this.checkZipper.UseVisualStyleBackColor = true;
            // 
            // checkIsICRItem
            // 
            this.checkIsICRItem.AutoSize = true;
            this.checkIsICRItem.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IrregularCost", true));
            this.checkIsICRItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIsICRItem.Location = new System.Drawing.Point(400, 150);
            this.checkIsICRItem.Name = "checkIsICRItem";
            this.checkIsICRItem.Size = new System.Drawing.Size(93, 21);
            this.checkIsICRItem.TabIndex = 7;
            this.checkIsICRItem.Text = "is ICR Item";
            this.checkIsICRItem.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(70, 150);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 23);
            this.label6.TabIndex = 11;
            this.label6.Text = "Issue Type";
            // 
            // txtDropdownlistIssueType
            // 
            this.txtDropdownlistIssueType.BackColor = System.Drawing.Color.White;
            this.txtDropdownlistIssueType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "IssueType", true));
            this.txtDropdownlistIssueType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDropdownlistIssueType.FormattingEnabled = true;
            this.txtDropdownlistIssueType.IsSupportUnselect = true;
            this.txtDropdownlistIssueType.Location = new System.Drawing.Point(169, 150);
            this.txtDropdownlistIssueType.Name = "txtDropdownlistIssueType";
            this.txtDropdownlistIssueType.Size = new System.Drawing.Size(121, 24);
            this.txtDropdownlistIssueType.TabIndex = 3;
            this.txtDropdownlistIssueType.Type = "IssueType";
            // 
            // checkIsTrimCardOther
            // 
            this.checkIsTrimCardOther.AutoSize = true;
            this.checkIsTrimCardOther.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsTrimCardOther", true));
            this.checkIsTrimCardOther.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkIsTrimCardOther.Location = new System.Drawing.Point(400, 191);
            this.checkIsTrimCardOther.Name = "checkIsTrimCardOther";
            this.checkIsTrimCardOther.ReadOnly = true;
            this.checkIsTrimCardOther.Size = new System.Drawing.Size(143, 21);
            this.checkIsTrimCardOther.TabIndex = 8;
            this.checkIsTrimCardOther.Text = "Is Trim Card Other";
            this.checkIsTrimCardOther.UseVisualStyleBackColor = true;
            // 
            // B13
            // 
            this.ClientSize = new System.Drawing.Size(836, 457);
            this.DefaultControlForEdit = "txtdropdownlist2";
            this.DefaultOrder = "ID";
            this.EnableGridJunkColor = true;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B13";
            this.Text = "B13. Material Type";
            this.UniqueExpress = "ID";
            this.WorkAlias = "MtlType";
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

        private Win.UI.ComboBox comboMaterialType;
        private Win.UI.DisplayBox displayProduction;
        private Win.UI.DisplayBox displayArtwork;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.CheckBox checkIsICRItem;
        private Win.UI.CheckBox checkZipper;
        private Win.UI.CheckBox checkExtend;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.Label label6;
        private Class.txtdropdownlist txtDropdownlistIssueType;
        private Win.UI.CheckBox checkIsTrimCardOther;
    }
}
