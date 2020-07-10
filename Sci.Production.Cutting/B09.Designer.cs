namespace Sci.Production.Cutting
{
    partial class B09
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
            this.txtID = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtDescription = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.chkJunk = new Sci.Win.UI.CheckBox();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.numLeadTime = new Sci.Win.UI.NumericBox();
            this.txtSubprocess = new Sci.Win.UI.TextBox();
            this.disArtworkType = new Sci.Win.UI.DisplayBox();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.lbM = new Sci.Win.UI.Label();
            this.lbFactory = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
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
            this.detail.Size = new System.Drawing.Size(918, 352);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtfactory);
            this.detailcont.Controls.Add(this.lbFactory);
            this.detailcont.Controls.Add(this.lbM);
            this.detailcont.Controls.Add(this.txtMdivision);
            this.detailcont.Controls.Add(this.disArtworkType);
            this.detailcont.Controls.Add(this.txtSubprocess);
            this.detailcont.Controls.Add(this.numLeadTime);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Size = new System.Drawing.Size(918, 314);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 314);
            this.detailbtm.Size = new System.Drawing.Size(918, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(918, 352);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(926, 381);
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
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.White;
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtID.Location = new System.Drawing.Point(93, 13);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(100, 22);
            this.txtID.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(15, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "ID";
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.White;
            this.txtDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.txtDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDescription.Location = new System.Drawing.Point(93, 42);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(708, 22);
            this.txtDescription.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(15, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 6;
            this.label2.Text = "Description";
            // 
            // chkJunk
            // 
            this.chkJunk.AutoSize = true;
            this.chkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.chkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkJunk.Location = new System.Drawing.Point(236, 15);
            this.chkJunk.Name = "chkJunk";
            this.chkJunk.Size = new System.Drawing.Size(57, 21);
            this.chkJunk.TabIndex = 8;
            this.chkJunk.Text = "Junk";
            this.chkJunk.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(24, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 23);
            this.label3.TabIndex = 0;
            this.label3.Text = "Lead Time";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(24, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 23);
            this.label4.TabIndex = 1;
            this.label4.Text = "Subprocess";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(24, 82);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 23);
            this.label5.TabIndex = 2;
            this.label5.Text = "Artwork Type";
            // 
            // numLeadTime
            // 
            this.numLeadTime.BackColor = System.Drawing.Color.White;
            this.numLeadTime.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "LeadTime", true));
            this.numLeadTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numLeadTime.Location = new System.Drawing.Point(125, 24);
            this.numLeadTime.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numLeadTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLeadTime.Name = "numLeadTime";
            this.numLeadTime.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numLeadTime.Size = new System.Drawing.Size(100, 23);
            this.numLeadTime.TabIndex = 3;
            this.numLeadTime.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtSubprocess
            // 
            this.txtSubprocess.BackColor = System.Drawing.Color.White;
            this.txtSubprocess.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Subprocess", true));
            this.txtSubprocess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSubprocess.Location = new System.Drawing.Point(125, 53);
            this.txtSubprocess.Name = "txtSubprocess";
            this.txtSubprocess.Size = new System.Drawing.Size(528, 23);
            this.txtSubprocess.TabIndex = 4;
            this.txtSubprocess.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSubprocess_PopUp);
            this.txtSubprocess.Validating += new System.ComponentModel.CancelEventHandler(this.TxtSubprocess_Validating);
            // 
            // disArtworkType
            // 
            this.disArtworkType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.disArtworkType.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ArtworkType", true));
            this.disArtworkType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.disArtworkType.Location = new System.Drawing.Point(125, 82);
            this.disArtworkType.Name = "disArtworkType";
            this.disArtworkType.Size = new System.Drawing.Size(528, 23);
            this.disArtworkType.TabIndex = 5;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "MDivisionID", true));
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(304, 24);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(92, 23);
            this.txtMdivision.TabIndex = 6;
            this.txtMdivision.TextChanged += new System.EventHandler(this.txtMdivision_TextChanged);
            // 
            // lbM
            // 
            this.lbM.Location = new System.Drawing.Point(257, 24);
            this.lbM.Name = "lbM";
            this.lbM.Size = new System.Drawing.Size(44, 23);
            this.lbM.TabIndex = 7;
            this.lbM.Text = "M";
            // 
            // lbFactory
            // 
            this.lbFactory.Location = new System.Drawing.Point(423, 24);
            this.lbFactory.Name = "lbFactory";
            this.lbFactory.Size = new System.Drawing.Size(98, 23);
            this.lbFactory.TabIndex = 8;
            this.lbFactory.Text = "Factory";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "FactoryID", true));
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(524, 24);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(129, 23);
            this.txtfactory.TabIndex = 9;
            // 
            // B09
            // 
            this.ClientSize = new System.Drawing.Size(926, 414);
            this.DefaultControl = "numLeadTime";
            this.DefaultControlForEdit = "numLeadTime";
            this.DefaultOrder = "ID";
            this.GridAlias = "SubprocessLeadTime_Detail";
            this.IsSupportClip = false;
            this.IsSupportPrint = false;
            this.Name = "B09";
            this.OnLineHelpID = "Sci.Win.Tems.Input6";
            this.Text = "B09. Subprocess Lead Time";
            this.WorkAlias = "SubprocessLeadTime";
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

        private Win.UI.CheckBox chkJunk;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtDescription;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtID;
        private Win.UI.TextBox txtSubprocess;
        private Win.UI.NumericBox numLeadTime;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.DisplayBox disArtworkType;
        private Win.UI.Label lbM;
        private Class.TxtMdivision txtMdivision;
        private Class.Txtfactory txtfactory;
        private Win.UI.Label lbFactory;
    }
}
