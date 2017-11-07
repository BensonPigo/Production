namespace Sci.Production.PPIC
{
    partial class P01_ProductionKit
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
            this.labelFactory = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.labelMR = new Sci.Win.UI.Label();
            this.comboMR = new Sci.Win.UI.ComboBox();
            this.labelSMR = new Sci.Win.UI.Label();
            this.comboSMR = new Sci.Win.UI.ComboBox();
            this.checkMRnotSendYet = new Sci.Win.UI.CheckBox();
            this.checkFactorynotReceived = new Sci.Win.UI.CheckBox();
            this.btnViewDetail = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Controls.Add(this.btnViewDetail);
            this.btmcont.Location = new System.Drawing.Point(0, 453);
            this.btmcont.Size = new System.Drawing.Size(908, 44);
            this.btmcont.TabIndex = 5;
            this.btmcont.Controls.SetChildIndex(this.delete, 0);
            this.btmcont.Controls.SetChildIndex(this.revise, 0);
            this.btmcont.Controls.SetChildIndex(this.undo, 0);
            this.btmcont.Controls.SetChildIndex(this.save, 0);
            this.btmcont.Controls.SetChildIndex(this.append, 0);
            this.btmcont.Controls.SetChildIndex(this.btnViewDetail, 0);
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(12, 43);
            this.gridcont.Size = new System.Drawing.Size(884, 404);
            // 
            // append
            // 
            this.append.Location = new System.Drawing.Point(170, 5);
            this.append.Size = new System.Drawing.Size(80, 34);
            this.append.TabIndex = 2;
            // 
            // revise
            // 
            this.revise.Size = new System.Drawing.Size(80, 34);
            this.revise.TabIndex = 1;
            // 
            // delete
            // 
            this.delete.Location = new System.Drawing.Point(10, 5);
            this.delete.Size = new System.Drawing.Size(80, 34);
            this.delete.TabIndex = 0;
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(818, 5);
            this.undo.Size = new System.Drawing.Size(80, 34);
            this.undo.TabIndex = 5;
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(738, 5);
            this.save.Size = new System.Drawing.Size(80, 34);
            this.save.TabIndex = 4;
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(15, 11);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(51, 23);
            this.labelFactory.TabIndex = 98;
            this.labelFactory.Text = "Factory";
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(70, 11);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(57, 24);
            this.comboFactory.TabIndex = 0;
            this.comboFactory.SelectedIndexChanged += new System.EventHandler(this.ComboFactory_SelectedIndexChanged);
            // 
            // labelMR
            // 
            this.labelMR.Lines = 0;
            this.labelMR.Location = new System.Drawing.Point(195, 11);
            this.labelMR.Name = "labelMR";
            this.labelMR.Size = new System.Drawing.Size(29, 23);
            this.labelMR.TabIndex = 100;
            this.labelMR.Text = "MR";
            // 
            // comboMR
            // 
            this.comboMR.BackColor = System.Drawing.Color.White;
            this.comboMR.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboMR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMR.FormattingEnabled = true;
            this.comboMR.IsSupportUnselect = true;
            this.comboMR.Location = new System.Drawing.Point(228, 11);
            this.comboMR.Name = "comboMR";
            this.comboMR.Size = new System.Drawing.Size(80, 24);
            this.comboMR.TabIndex = 1;
            this.comboMR.SelectedIndexChanged += new System.EventHandler(this.ComboMR_SelectedIndexChanged);
            // 
            // labelSMR
            // 
            this.labelSMR.Lines = 0;
            this.labelSMR.Location = new System.Drawing.Point(380, 11);
            this.labelSMR.Name = "labelSMR";
            this.labelSMR.Size = new System.Drawing.Size(37, 23);
            this.labelSMR.TabIndex = 102;
            this.labelSMR.Text = "SMR";
            // 
            // comboSMR
            // 
            this.comboSMR.BackColor = System.Drawing.Color.White;
            this.comboSMR.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboSMR.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSMR.FormattingEnabled = true;
            this.comboSMR.IsSupportUnselect = true;
            this.comboSMR.Location = new System.Drawing.Point(421, 11);
            this.comboSMR.Name = "comboSMR";
            this.comboSMR.Size = new System.Drawing.Size(80, 24);
            this.comboSMR.TabIndex = 2;
            this.comboSMR.SelectedIndexChanged += new System.EventHandler(this.ComboSMR_SelectedIndexChanged);
            // 
            // checkMRnotSendYet
            // 
            this.checkMRnotSendYet.AutoSize = true;
            this.checkMRnotSendYet.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkMRnotSendYet.IsSupportEditMode = false;
            this.checkMRnotSendYet.Location = new System.Drawing.Point(550, 11);
            this.checkMRnotSendYet.Name = "checkMRnotSendYet";
            this.checkMRnotSendYet.Size = new System.Drawing.Size(130, 21);
            this.checkMRnotSendYet.TabIndex = 3;
            this.checkMRnotSendYet.Text = "MR not send yet";
            this.checkMRnotSendYet.UseVisualStyleBackColor = true;
            this.checkMRnotSendYet.CheckedChanged += new System.EventHandler(this.CheckMRnotSendYet_CheckedChanged);
            // 
            // checkFactorynotReceived
            // 
            this.checkFactorynotReceived.AutoSize = true;
            this.checkFactorynotReceived.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkFactorynotReceived.IsSupportEditMode = false;
            this.checkFactorynotReceived.Location = new System.Drawing.Point(702, 11);
            this.checkFactorynotReceived.Name = "checkFactorynotReceived";
            this.checkFactorynotReceived.Size = new System.Drawing.Size(156, 21);
            this.checkFactorynotReceived.TabIndex = 4;
            this.checkFactorynotReceived.Text = "Factory not received";
            this.checkFactorynotReceived.UseVisualStyleBackColor = true;
            this.checkFactorynotReceived.CheckedChanged += new System.EventHandler(this.CheckFactorynotReceived_CheckedChanged);
            // 
            // btnViewDetail
            // 
            this.btnViewDetail.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnViewDetail.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnViewDetail.Location = new System.Drawing.Point(250, 5);
            this.btnViewDetail.Name = "btnViewDetail";
            this.btnViewDetail.Size = new System.Drawing.Size(105, 34);
            this.btnViewDetail.TabIndex = 3;
            this.btnViewDetail.Text = "View Detail";
            this.btnViewDetail.UseVisualStyleBackColor = true;
            this.btnViewDetail.Click += new System.EventHandler(this.BtnViewDetail_Click);
            // 
            // P01_ProductionKit
            // 
            this.ClientSize = new System.Drawing.Size(908, 497);
            this.Controls.Add(this.checkFactorynotReceived);
            this.Controls.Add(this.checkMRnotSendYet);
            this.Controls.Add(this.comboSMR);
            this.Controls.Add(this.labelSMR);
            this.Controls.Add(this.comboMR);
            this.Controls.Add(this.labelMR);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.labelFactory);
            this.DefaultControl = "comboFactory";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.KeyField1 = "StyleUkey";
            this.Name = "P01_ProductionKit";
            this.Text = "Production Kit";
            this.WorkAlias = "Style_ProductionKits";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.labelMR, 0);
            this.Controls.SetChildIndex(this.comboMR, 0);
            this.Controls.SetChildIndex(this.labelSMR, 0);
            this.Controls.SetChildIndex(this.comboSMR, 0);
            this.Controls.SetChildIndex(this.checkMRnotSendYet, 0);
            this.Controls.SetChildIndex(this.checkFactorynotReceived, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Button btnViewDetail;
        private Win.UI.Label labelFactory;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.Label labelMR;
        private Win.UI.ComboBox comboMR;
        private Win.UI.Label labelSMR;
        private Win.UI.ComboBox comboSMR;
        private Win.UI.CheckBox checkMRnotSendYet;
        private Win.UI.CheckBox checkFactorynotReceived;
    }
}
