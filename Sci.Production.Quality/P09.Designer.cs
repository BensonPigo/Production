namespace Sci.Production.Quality
{
    partial class P09
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.displayBrand = new Sci.Win.UI.DisplayBox();
            this.displaySeason = new Sci.Win.UI.DisplayBox();
            this.displayStyle = new Sci.Win.UI.DisplayBox();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.labelSP = new Sci.Win.UI.Label();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.labelRemark = new Sci.Win.UI.Label();
            this.btnReImport = new Sci.Win.UI.Button();
            this.txtID = new Sci.Win.UI.TextBox();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
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
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.txtID);
            this.masterpanel.Controls.Add(this.btnReImport);
            this.masterpanel.Controls.Add(this.editRemark);
            this.masterpanel.Controls.Add(this.labelRemark);
            this.masterpanel.Controls.Add(this.displayBrand);
            this.masterpanel.Controls.Add(this.displaySeason);
            this.masterpanel.Controls.Add(this.displayStyle);
            this.masterpanel.Controls.Add(this.labelBrand);
            this.masterpanel.Controls.Add(this.labelSeason);
            this.masterpanel.Controls.Add(this.labelStyle);
            this.masterpanel.Controls.Add(this.labelSP);
            this.masterpanel.Size = new System.Drawing.Size(892, 105);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelStyle, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelSeason, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelBrand, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayStyle, 0);
            this.masterpanel.Controls.SetChildIndex(this.displaySeason, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayBrand, 0);
            this.masterpanel.Controls.SetChildIndex(this.labelRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.editRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnReImport, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtID, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 105);
            this.detailpanel.Size = new System.Drawing.Size(892, 244);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(5, 70);
            this.gridicon.TabIndex = 6;
            this.gridicon.Visible = false;
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(892, 244);
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
            this.detail.Size = new System.Drawing.Size(892, 387);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(892, 349);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 349);
            this.detailbtm.Size = new System.Drawing.Size(892, 38);
            // 
            // displayBrand
            // 
            this.displayBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBrand.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBrand.Location = new System.Drawing.Point(323, 41);
            this.displayBrand.Name = "displayBrand";
            this.displayBrand.Size = new System.Drawing.Size(158, 21);
            this.displayBrand.TabIndex = 5;
            // 
            // displaySeason
            // 
            this.displaySeason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySeason.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displaySeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySeason.Location = new System.Drawing.Point(323, 11);
            this.displaySeason.Name = "displaySeason";
            this.displaySeason.Size = new System.Drawing.Size(158, 21);
            this.displaySeason.TabIndex = 3;
            // 
            // displayStyle
            // 
            this.displayStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.displayStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyle.Location = new System.Drawing.Point(70, 43);
            this.displayStyle.Name = "displayStyle";
            this.displayStyle.Size = new System.Drawing.Size(158, 21);
            this.displayStyle.TabIndex = 4;
            // 
            // labelBrand
            // 
            this.labelBrand.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelBrand.Location = new System.Drawing.Point(266, 41);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(54, 23);
            this.labelBrand.TabIndex = 22;
            this.labelBrand.Text = "Brand";
            // 
            // labelSeason
            // 
            this.labelSeason.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelSeason.Location = new System.Drawing.Point(266, 11);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(54, 23);
            this.labelSeason.TabIndex = 21;
            this.labelSeason.Text = "Season";
            // 
            // labelStyle
            // 
            this.labelStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelStyle.Location = new System.Drawing.Point(13, 44);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(54, 23);
            this.labelStyle.TabIndex = 20;
            this.labelStyle.Text = "Style";
            // 
            // labelSP
            // 
            this.labelSP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelSP.Location = new System.Drawing.Point(13, 15);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(54, 23);
            this.labelSP.TabIndex = 19;
            this.labelSP.Text = "SP#";
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "remark", true));
            this.editRemark.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(601, 7);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(272, 55);
            this.editRemark.TabIndex = 1;
            // 
            // labelRemark
            // 
            this.labelRemark.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelRemark.Location = new System.Drawing.Point(499, 9);
            this.labelRemark.Name = "labelRemark";
            this.labelRemark.Size = new System.Drawing.Size(95, 23);
            this.labelRemark.TabIndex = 28;
            this.labelRemark.Text = "Remark";
            // 
            // btnReImport
            // 
            this.btnReImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnReImport.Location = new System.Drawing.Point(669, 68);
            this.btnReImport.Name = "btnReImport";
            this.btnReImport.Size = new System.Drawing.Size(204, 31);
            this.btnReImport.TabIndex = 2;
            this.btnReImport.Text = "Re-Import Table";
            this.btnReImport.UseVisualStyleBackColor = true;
            this.btnReImport.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "id", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtID.IsSupportEditMode = false;
            this.txtID.Location = new System.Drawing.Point(70, 15);
            this.txtID.Name = "txtID";
            this.txtID.ReadOnly = true;
            this.txtID.Size = new System.Drawing.Size(158, 23);
            this.txtID.TabIndex = 0;
            this.txtID.Validating += new System.ComponentModel.CancelEventHandler(this.txtID_Validating);
            // 
            // P09
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(900, 449);
            this.DefaultControl = "txtID";
            this.DefaultControlForEdit = "editRemark";
            this.DefaultDetailOrder = "seq1,seq2";
            this.DefaultOrder = "id";
            this.GridAlias = "FabricInspDoc_Detail";
            this.GridNew = 0;
            this.GridUniqueKey = "id,seq1,seq2";
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "id";
            this.Name = "P09";
            this.Text = "P09.Fabric Inspection Document Record";
            this.UnApvChkValue = "Confirmed";
            this.WorkAlias = "FabricInspDoc";
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
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DisplayBox displayBrand;
        private Win.UI.DisplayBox displaySeason;
        private Win.UI.DisplayBox displayStyle;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelStyle;
        private Win.UI.Label labelSP;
        private Win.UI.EditBox editRemark;
        private Win.UI.Label labelRemark;
        private Win.UI.Button btnReImport;
        private Win.UI.TextBox txtID;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}
