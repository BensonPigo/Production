namespace Sci.Production.Centralized
{
    partial class IE_B13
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
            this.labDescChinese = new Sci.Win.UI.Label();
            this.labelID = new Sci.Win.UI.Label();
            this.labDescVietnam = new Sci.Win.UI.Label();
            this.labDescCambodia = new Sci.Win.UI.Label();
            this.txtID = new Sci.Win.UI.TextBox();
            this.editDescChinese = new Sci.Win.UI.EditBox();
            this.editDescVietnam = new Sci.Win.UI.EditBox();
            this.editDescCambodia = new Sci.Win.UI.EditBox();
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
            this.detail.Size = new System.Drawing.Size(762, 410);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.editDescCambodia);
            this.detailcont.Controls.Add(this.editDescVietnam);
            this.detailcont.Controls.Add(this.editDescChinese);
            this.detailcont.Controls.Add(this.txtID);
            this.detailcont.Controls.Add(this.labDescCambodia);
            this.detailcont.Controls.Add(this.labDescVietnam);
            this.detailcont.Controls.Add(this.labDescChinese);
            this.detailcont.Controls.Add(this.labelID);
            this.detailcont.Size = new System.Drawing.Size(762, 372);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 372);
            this.detailbtm.Size = new System.Drawing.Size(762, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(738, 311);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(770, 439);
            // 
            // labDescChinese
            // 
            this.labDescChinese.Location = new System.Drawing.Point(22, 76);
            this.labDescChinese.Name = "labDescChinese";
            this.labDescChinese.Size = new System.Drawing.Size(123, 23);
            this.labDescChinese.TabIndex = 5;
            this.labDescChinese.Text = "Desc Chinese";
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(22, 34);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(123, 23);
            this.labelID.TabIndex = 3;
            this.labelID.Text = "Operation";
            // 
            // labDescVietnam
            // 
            this.labDescVietnam.Location = new System.Drawing.Point(22, 168);
            this.labDescVietnam.Name = "labDescVietnam";
            this.labDescVietnam.Size = new System.Drawing.Size(123, 23);
            this.labDescVietnam.TabIndex = 7;
            this.labDescVietnam.Text = "Desc Vietnam";
            // 
            // labDescCambodia
            // 
            this.labDescCambodia.Location = new System.Drawing.Point(22, 264);
            this.labDescCambodia.Name = "labDescCambodia";
            this.labDescCambodia.Size = new System.Drawing.Size(123, 23);
            this.labDescCambodia.TabIndex = 9;
            this.labDescCambodia.Text = "Desc Cambodia";
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.White;
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtID.Location = new System.Drawing.Point(148, 34);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(521, 23);
            this.txtID.TabIndex = 0;
            // 
            // editDescChinese
            // 
            this.editDescChinese.BackColor = System.Drawing.Color.White;
            this.editDescChinese.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DescCHS", true));
            this.editDescChinese.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescChinese.Location = new System.Drawing.Point(148, 76);
            this.editDescChinese.Multiline = true;
            this.editDescChinese.Name = "editDescChinese";
            this.editDescChinese.Size = new System.Drawing.Size(521, 69);
            this.editDescChinese.TabIndex = 1;
            // 
            // editDescVietnam
            // 
            this.editDescVietnam.BackColor = System.Drawing.Color.White;
            this.editDescVietnam.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DescVI", true));
            this.editDescVietnam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescVietnam.Location = new System.Drawing.Point(148, 168);
            this.editDescVietnam.Multiline = true;
            this.editDescVietnam.Name = "editDescVietnam";
            this.editDescVietnam.Size = new System.Drawing.Size(521, 69);
            this.editDescVietnam.TabIndex = 2;
            // 
            // editDescCambodia
            // 
            this.editDescCambodia.BackColor = System.Drawing.Color.White;
            this.editDescCambodia.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "DescKH", true));
            this.editDescCambodia.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescCambodia.Location = new System.Drawing.Point(148, 264);
            this.editDescCambodia.Multiline = true;
            this.editDescCambodia.Name = "editDescCambodia";
            this.editDescCambodia.Size = new System.Drawing.Size(521, 69);
            this.editDescCambodia.TabIndex = 3;
            // 
            // IE_B13
            // 
            this.ClientSize = new System.Drawing.Size(770, 472);
            this.ConnectionName = "ProductionTPE";
            this.DefaultOrder = "ID";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.Name = "IE_B13";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "IE_B13 Operation Translation List";
            this.UniqueExpress = "ID";
            this.WorkAlias = "Operationdesc";
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
        private Win.UI.Label labDescCambodia;
        private Win.UI.Label labDescVietnam;
        private Win.UI.Label labDescChinese;
        private Win.UI.Label labelID;
        private Win.UI.TextBox txtID;
        private Win.UI.EditBox editDescChinese;
        private Win.UI.EditBox editDescCambodia;
        private Win.UI.EditBox editDescVietnam;
    }
}
