namespace Sci.Production.PPIC
{
    partial class B06
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
            this.labelLine = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.labelCellNo = new Sci.Win.UI.Label();
            this.labelNoOfSewers = new Sci.Win.UI.Label();
            this.displayFactory = new Sci.Win.UI.DisplayBox();
            this.txtLine = new Sci.Win.UI.TextBox();
            this.txtDescription = new Sci.Win.UI.TextBox();
            this.numNoOfSewers = new Sci.Win.UI.NumericBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.lblGroup = new Sci.Win.UI.Label();
            this.txtGroup = new Sci.Win.UI.TextBox();
            this.txtCellNo = new Sci.Production.Class.TxtCell();
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
            this.detail.Size = new System.Drawing.Size(832, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtGroup);
            this.detailcont.Controls.Add(this.lblGroup);
            this.detailcont.Controls.Add(this.txtCellNo);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.numNoOfSewers);
            this.detailcont.Controls.Add(this.txtDescription);
            this.detailcont.Controls.Add(this.txtLine);
            this.detailcont.Controls.Add(this.displayFactory);
            this.detailcont.Controls.Add(this.labelNoOfSewers);
            this.detailcont.Controls.Add(this.labelCellNo);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labelLine);
            this.detailcont.Controls.Add(this.labelFactory);
            this.detailcont.Size = new System.Drawing.Size(832, 357);
            this.detailcont.TabIndex = 0;
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(832, 38);
            this.detailbtm.TabIndex = 1;
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(832, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(840, 424);
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
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(27, 23);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(80, 23);
            this.labelFactory.TabIndex = 0;
            this.labelFactory.Text = "Factory";
            // 
            // labelLine
            // 
            this.labelLine.Location = new System.Drawing.Point(27, 61);
            this.labelLine.Name = "labelLine";
            this.labelLine.Size = new System.Drawing.Size(80, 23);
            this.labelLine.TabIndex = 1;
            this.labelLine.Text = "Line#";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(27, 99);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(80, 23);
            this.labelDescription.TabIndex = 2;
            this.labelDescription.Text = "Description";
            // 
            // labelCellNo
            // 
            this.labelCellNo.Location = new System.Drawing.Point(27, 137);
            this.labelCellNo.Name = "labelCellNo";
            this.labelCellNo.Size = new System.Drawing.Size(80, 23);
            this.labelCellNo.TabIndex = 3;
            this.labelCellNo.Text = "Cell No.";
            // 
            // labelNoOfSewers
            // 
            this.labelNoOfSewers.Location = new System.Drawing.Point(27, 175);
            this.labelNoOfSewers.Name = "labelNoOfSewers";
            this.labelNoOfSewers.Size = new System.Drawing.Size(80, 23);
            this.labelNoOfSewers.TabIndex = 4;
            this.labelNoOfSewers.Text = "# of Sewers";
            // 
            // displayFactory
            // 
            this.displayFactory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFactory.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "FactoryID", true));
            this.displayFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFactory.Location = new System.Drawing.Point(112, 23);
            this.displayFactory.Name = "displayFactory";
            this.displayFactory.Size = new System.Drawing.Size(69, 23);
            this.displayFactory.TabIndex = 0;
            // 
            // txtLine
            // 
            this.txtLine.BackColor = System.Drawing.Color.White;
            this.txtLine.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLine.Location = new System.Drawing.Point(112, 61);
            this.txtLine.Name = "txtLine";
            this.txtLine.Size = new System.Drawing.Size(40, 23);
            this.txtLine.TabIndex = 1;
            this.txtLine.Validating += new System.ComponentModel.CancelEventHandler(this.TxtLine_Validating);
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.White;
            this.txtDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.txtDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDescription.Location = new System.Drawing.Point(112, 99);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(474, 23);
            this.txtDescription.TabIndex = 2;
            // 
            // numNoOfSewers
            // 
            this.numNoOfSewers.BackColor = System.Drawing.Color.White;
            this.numNoOfSewers.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Sewer", true));
            this.numNoOfSewers.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numNoOfSewers.Location = new System.Drawing.Point(112, 175);
            this.numNoOfSewers.Name = "numNoOfSewers";
            this.numNoOfSewers.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numNoOfSewers.Size = new System.Drawing.Size(40, 23);
            this.numNoOfSewers.TabIndex = 4;
            this.numNoOfSewers.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(313, 23);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 5;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // lblGroup
            // 
            this.lblGroup.Location = new System.Drawing.Point(27, 210);
            this.lblGroup.Name = "lblGroup";
            this.lblGroup.Size = new System.Drawing.Size(80, 23);
            this.lblGroup.TabIndex = 6;
            this.lblGroup.Text = "Group";
            // 
            // txtGroup
            // 
            this.txtGroup.BackColor = System.Drawing.Color.White;
            this.txtGroup.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "LineGroup", true));
            this.txtGroup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtGroup.Location = new System.Drawing.Point(112, 210);
            this.txtGroup.Name = "txtGroup";
            this.txtGroup.Size = new System.Drawing.Size(243, 23);
            this.txtGroup.TabIndex = 7;
            // 
            // txtCellNo
            // 
            this.txtCellNo.BackColor = System.Drawing.Color.White;
            this.txtCellNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SewingCell", true));
            this.txtCellNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCellNo.Location = new System.Drawing.Point(112, 137);
            this.txtCellNo.MDivisionID = "displayBox1";
            this.txtCellNo.Name = "txtCellNo";
            this.txtCellNo.Size = new System.Drawing.Size(30, 23);
            this.txtCellNo.TabIndex = 3;
            // 
            // B06
            // 
            this.ClientSize = new System.Drawing.Size(840, 457);
            this.DefaultControl = "displayFactory";
            this.DefaultControlForEdit = "displayFactory";
            this.DefaultOrder = "ID";
            this.IsSupportPrint = false;
            this.Name = "B06";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B06. Sewing Line";
            this.UniqueExpress = "FactoryID,ID";
            this.WorkAlias = "SewingLine";
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

        private Win.UI.NumericBox numNoOfSewers;
        private Win.UI.TextBox txtDescription;
        private Win.UI.TextBox txtLine;
        private Win.UI.DisplayBox displayFactory;
        private Win.UI.Label labelNoOfSewers;
        private Win.UI.Label labelCellNo;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelLine;
        private Win.UI.Label labelFactory;
        private Win.UI.CheckBox checkJunk;
        private Class.TxtCell txtCellNo;
        private Win.UI.TextBox txtGroup;
        private Win.UI.Label lblGroup;
    }
}
