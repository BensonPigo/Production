namespace Sci.Production.Quality
{
    partial class B04
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
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.txtLevel = new Sci.Win.UI.TextBox();
            this.labelLevel = new Sci.Win.UI.Label();
            this.labelRateRange = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.labelMaterialType = new Sci.Win.UI.Label();
            this.txtRateRangeStart = new Sci.Win.UI.TextBox();
            this.txtRateRangeEnd = new Sci.Win.UI.TextBox();
            this.comboMaterialType = new Sci.Win.UI.ComboBox();
            this.labConfirm = new System.Windows.Forms.Label();
            this.label4 = new Sci.Win.UI.Label();
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
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.labConfirm);
            this.detailcont.Controls.Add(this.comboMaterialType);
            this.detailcont.Controls.Add(this.txtRateRangeEnd);
            this.detailcont.Controls.Add(this.txtRateRangeStart);
            this.detailcont.Controls.Add(this.labelMaterialType);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.labelRateRange);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.txtLevel);
            this.detailcont.Controls.Add(this.labelLevel);
            this.detailcont.Size = new System.Drawing.Size(828, 357);
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
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(272, 47);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 23;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // txtLevel
            // 
            this.txtLevel.BackColor = System.Drawing.Color.White;
            this.txtLevel.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtLevel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLevel.Location = new System.Drawing.Point(151, 45);
            this.txtLevel.Name = "txtLevel";
            this.txtLevel.Size = new System.Drawing.Size(100, 23);
            this.txtLevel.TabIndex = 4;
            // 
            // labelLevel
            // 
            this.labelLevel.Lines = 0;
            this.labelLevel.Location = new System.Drawing.Point(52, 45);
            this.labelLevel.Name = "labelLevel";
            this.labelLevel.Size = new System.Drawing.Size(96, 23);
            this.labelLevel.TabIndex = 21;
            this.labelLevel.Text = "Level ";
            // 
            // labelRateRange
            // 
            this.labelRateRange.Lines = 0;
            this.labelRateRange.Location = new System.Drawing.Point(52, 78);
            this.labelRateRange.Name = "labelRateRange";
            this.labelRateRange.Size = new System.Drawing.Size(96, 23);
            this.labelRateRange.TabIndex = 24;
            this.labelRateRange.Text = "Rate Range";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(254, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 23);
            this.label3.TabIndex = 25;
            this.label3.Text = "%";
            // 
            // labelMaterialType
            // 
            this.labelMaterialType.Lines = 0;
            this.labelMaterialType.Location = new System.Drawing.Point(52, 112);
            this.labelMaterialType.Name = "labelMaterialType";
            this.labelMaterialType.Size = new System.Drawing.Size(96, 23);
            this.labelMaterialType.TabIndex = 27;
            this.labelMaterialType.Text = "Material Type";
            // 
            // txtRateRangeStart
            // 
            this.txtRateRangeStart.BackColor = System.Drawing.Color.White;
            this.txtRateRangeStart.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Range1", true));
            this.txtRateRangeStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRateRangeStart.Location = new System.Drawing.Point(151, 78);
            this.txtRateRangeStart.Name = "txtRateRangeStart";
            this.txtRateRangeStart.Size = new System.Drawing.Size(100, 23);
            this.txtRateRangeStart.TabIndex = 5;
            // 
            // txtRateRangeEnd
            // 
            this.txtRateRangeEnd.BackColor = System.Drawing.Color.White;
            this.txtRateRangeEnd.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Range2", true));
            this.txtRateRangeEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRateRangeEnd.Location = new System.Drawing.Point(303, 78);
            this.txtRateRangeEnd.Name = "txtRateRangeEnd";
            this.txtRateRangeEnd.Size = new System.Drawing.Size(121, 23);
            this.txtRateRangeEnd.TabIndex = 6;
            // 
            // comboMaterialType
            // 
            this.comboMaterialType.BackColor = System.Drawing.Color.White;
            this.comboMaterialType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "type", true));
            this.comboMaterialType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMaterialType.FormattingEnabled = true;
            this.comboMaterialType.IsSupportUnselect = true;
            this.comboMaterialType.Location = new System.Drawing.Point(151, 112);
            this.comboMaterialType.Name = "comboMaterialType";
            this.comboMaterialType.Size = new System.Drawing.Size(178, 24);
            this.comboMaterialType.TabIndex = 7;
            // 
            // labConfirm
            // 
            this.labConfirm.AutoSize = true;
            this.labConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labConfirm.ForeColor = System.Drawing.Color.DimGray;
            this.labConfirm.Location = new System.Drawing.Point(280, 84);
            this.labConfirm.Name = "labConfirm";
            this.labConfirm.Size = new System.Drawing.Size(21, 24);
            this.labConfirm.TabIndex = 29;
            this.labConfirm.Text = "~";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(427, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 23);
            this.label4.TabIndex = 30;
            this.label4.Text = "%";
            // 
            // B04
            // 
            this.ClientSize = new System.Drawing.Size(836, 457);
            this.DefaultControl = "txtLevel";
            this.DefaultControlForEdit = "txtRateRangeStart";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportLocate = false;
            this.IsSupportPrint = false;
            this.Name = "B04";
            this.Text = "B04 .The Level of Supplier";
            this.WorkAlias = "SuppLevel";
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
        private Win.UI.TextBox txtRateRangeEnd;
        private Win.UI.TextBox txtRateRangeStart;
        private Win.UI.Label labelMaterialType;
        private Win.UI.Label label3;
        private Win.UI.Label labelRateRange;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.TextBox txtLevel;
        private Win.UI.Label labelLevel;
        private Win.UI.Label label4;
        private System.Windows.Forms.Label labConfirm;
    }
}
