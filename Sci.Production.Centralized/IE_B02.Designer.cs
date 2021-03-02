namespace Sci.Production.Centralized
{
    partial class IE_B02
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
            this.labelDate = new Sci.Win.UI.Label();
            this.labelType = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelTarget = new Sci.Win.UI.Label();
            this.dateDate = new Sci.Win.UI.DateBox();
            this.comboType = new Sci.Win.UI.ComboBox();
            this.numTarget = new Sci.Win.UI.NumericBox();
            this.displayM = new Sci.Win.UI.DisplayBox();
            this.chkJunk = new Sci.Win.UI.CheckBox();
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
            this.detailcont.Controls.Add(this.chkJunk);
            this.detailcont.Controls.Add(this.displayM);
            this.detailcont.Controls.Add(this.numTarget);
            this.detailcont.Controls.Add(this.comboType);
            this.detailcont.Controls.Add(this.dateDate);
            this.detailcont.Controls.Add(this.labelTarget);
            this.detailcont.Controls.Add(this.labelM);
            this.detailcont.Controls.Add(this.labelType);
            this.detailcont.Controls.Add(this.labelDate);
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
            this.editby.TabIndex = 1;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // labelDate
            // 
            this.labelDate.Location = new System.Drawing.Point(70, 37);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(85, 23);
            this.labelDate.TabIndex = 0;
            this.labelDate.Text = "Date";
            // 
            // labelType
            // 
            this.labelType.Location = new System.Drawing.Point(70, 84);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(85, 23);
            this.labelType.TabIndex = 1;
            this.labelType.Text = "Type";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(352, 37);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(18, 23);
            this.labelM.TabIndex = 2;
            this.labelM.Text = "M";
            // 
            // labelTarget
            // 
            this.labelTarget.Location = new System.Drawing.Point(70, 129);
            this.labelTarget.Name = "labelTarget";
            this.labelTarget.Size = new System.Drawing.Size(85, 23);
            this.labelTarget.TabIndex = 3;
            this.labelTarget.Text = "Target";
            // 
            // dateDate
            // 
            this.dateDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "EffectiveDate", true));
            this.dateDate.Location = new System.Drawing.Point(161, 37);
            this.dateDate.Name = "dateDate";
            this.dateDate.Size = new System.Drawing.Size(130, 23);
            this.dateDate.TabIndex = 0;
            // 
            // comboType
            // 
            this.comboType.BackColor = System.Drawing.Color.White;
            this.comboType.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Type", true));
            this.comboType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboType.FormattingEnabled = true;
            this.comboType.IsSupportUnselect = true;
            this.comboType.Location = new System.Drawing.Point(161, 84);
            this.comboType.Name = "comboType";
            this.comboType.OldText = "";
            this.comboType.Size = new System.Drawing.Size(188, 24);
            this.comboType.TabIndex = 1;
            this.comboType.SelectedIndexChanged += new System.EventHandler(this.ComboType_SelectedIndexChanged);
            // 
            // numTarget
            // 
            this.numTarget.BackColor = System.Drawing.Color.White;
            this.numTarget.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Target", true));
            this.numTarget.DecimalPlaces = 2;
            this.numTarget.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numTarget.Location = new System.Drawing.Point(161, 129);
            this.numTarget.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            131072});
            this.numTarget.MaxLength = 7;
            this.numTarget.Name = "numTarget";
            this.numTarget.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numTarget.Size = new System.Drawing.Size(66, 23);
            this.numTarget.TabIndex = 2;
            this.numTarget.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // displayM
            // 
            this.displayM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayM.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MDivisionID", true));
            this.displayM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayM.Location = new System.Drawing.Point(374, 37);
            this.displayM.Name = "displayM";
            this.displayM.Size = new System.Drawing.Size(40, 23);
            this.displayM.TabIndex = 3;
            // 
            // chkJunk
            // 
            this.chkJunk.AutoSize = true;
            this.chkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.chkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkJunk.Location = new System.Drawing.Point(450, 37);
            this.chkJunk.Name = "chkJunk";
            this.chkJunk.Size = new System.Drawing.Size(56, 21);
            this.chkJunk.TabIndex = 4;
            this.chkJunk.Text = "Juck";
            this.chkJunk.UseVisualStyleBackColor = true;
            // 
            // B02
            // 
            this.ClientSize = new System.Drawing.Size(836, 457);
            this.ConnectionName = "ProductionTPE";
            this.DefaultControl = "dateDate";
            this.DefaultControlForEdit = "numTarget";
            this.DefaultOrder = "EffectiveDate";
            this.IsSupportPrint = false;
            this.Name = "B02";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B02. Lean Measurement Target Setting";
            this.UniqueExpress = "EffectiveDate,MDivisionID,Type";
            this.WorkAlias = "ChgOverTarget";
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

        private Win.UI.Label labelTarget;
        private Win.UI.Label labelM;
        private Win.UI.Label labelType;
        private Win.UI.Label labelDate;
        private Win.UI.DateBox dateDate;
        private Win.UI.ComboBox comboType;
        private Win.UI.NumericBox numTarget;
        private Win.UI.DisplayBox displayM;
        private Win.UI.CheckBox chkJunk;
    }
}
