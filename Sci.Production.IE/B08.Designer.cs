namespace Sci.Production.IE
{
    partial class B08
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
            this.labelEmployee = new Sci.Win.UI.Label();
            this.labelNickName = new Sci.Win.UI.Label();
            this.labelSkill = new Sci.Win.UI.Label();
            this.labelHiredOn = new Sci.Win.UI.Label();
            this.labelResigned = new Sci.Win.UI.Label();
            this.labelLine = new Sci.Win.UI.Label();
            this.txtEmployee = new Sci.Win.UI.TextBox();
            this.txtNickName = new Sci.Win.UI.TextBox();
            this.txtSkill = new Sci.Win.UI.TextBox();
            this.dateHiredOn = new Sci.Win.UI.DateBox();
            this.dateResigned = new Sci.Win.UI.DateBox();
            this.labelM = new Sci.Win.UI.Label();
            this.displayM = new Sci.Win.UI.DisplayBox();
            this.txtmfactory = new Sci.Production.Class.Txtfactory();
            this.txtsewingline = new Sci.Production.Class.Txtsewingline();
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
            this.detailcont.Controls.Add(this.txtmfactory);
            this.detailcont.Controls.Add(this.displayM);
            this.detailcont.Controls.Add(this.labelM);
            this.detailcont.Controls.Add(this.txtsewingline);
            this.detailcont.Controls.Add(this.dateResigned);
            this.detailcont.Controls.Add(this.dateHiredOn);
            this.detailcont.Controls.Add(this.txtSkill);
            this.detailcont.Controls.Add(this.txtNickName);
            this.detailcont.Controls.Add(this.txtEmployee);
            this.detailcont.Controls.Add(this.labelLine);
            this.detailcont.Controls.Add(this.labelResigned);
            this.detailcont.Controls.Add(this.labelHiredOn);
            this.detailcont.Controls.Add(this.labelSkill);
            this.detailcont.Controls.Add(this.labelNickName);
            this.detailcont.Controls.Add(this.labelEmployee);
            this.detailcont.Controls.Add(this.labelFactory);
            this.detailcont.Size = new System.Drawing.Size(832, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(832, 38);
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
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(53, 32);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(75, 23);
            this.labelFactory.TabIndex = 0;
            this.labelFactory.Text = "Factory";
            // 
            // labelEmployee
            // 
            this.labelEmployee.Lines = 0;
            this.labelEmployee.Location = new System.Drawing.Point(53, 80);
            this.labelEmployee.Name = "labelEmployee";
            this.labelEmployee.Size = new System.Drawing.Size(75, 23);
            this.labelEmployee.TabIndex = 1;
            this.labelEmployee.Text = "Employee#";
            // 
            // labelNickName
            // 
            this.labelNickName.Lines = 0;
            this.labelNickName.Location = new System.Drawing.Point(53, 131);
            this.labelNickName.Name = "labelNickName";
            this.labelNickName.Size = new System.Drawing.Size(75, 23);
            this.labelNickName.TabIndex = 2;
            this.labelNickName.Text = "Nick Name";
            // 
            // labelSkill
            // 
            this.labelSkill.Lines = 0;
            this.labelSkill.Location = new System.Drawing.Point(53, 182);
            this.labelSkill.Name = "labelSkill";
            this.labelSkill.Size = new System.Drawing.Size(75, 23);
            this.labelSkill.TabIndex = 3;
            this.labelSkill.Text = "Skill";
            // 
            // labelHiredOn
            // 
            this.labelHiredOn.Lines = 0;
            this.labelHiredOn.Location = new System.Drawing.Point(400, 32);
            this.labelHiredOn.Name = "labelHiredOn";
            this.labelHiredOn.Size = new System.Drawing.Size(75, 23);
            this.labelHiredOn.TabIndex = 4;
            this.labelHiredOn.Text = "Hired On";
            // 
            // labelResigned
            // 
            this.labelResigned.Lines = 0;
            this.labelResigned.Location = new System.Drawing.Point(400, 81);
            this.labelResigned.Name = "labelResigned";
            this.labelResigned.Size = new System.Drawing.Size(75, 23);
            this.labelResigned.TabIndex = 5;
            this.labelResigned.Text = "Resigned";
            // 
            // labelLine
            // 
            this.labelLine.Lines = 0;
            this.labelLine.Location = new System.Drawing.Point(400, 132);
            this.labelLine.Name = "labelLine";
            this.labelLine.Size = new System.Drawing.Size(75, 23);
            this.labelLine.TabIndex = 6;
            this.labelLine.Text = "Line";
            // 
            // txtEmployee
            // 
            this.txtEmployee.BackColor = System.Drawing.Color.White;
            this.txtEmployee.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtEmployee.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtEmployee.Location = new System.Drawing.Point(132, 80);
            this.txtEmployee.Name = "txtEmployee";
            this.txtEmployee.Size = new System.Drawing.Size(80, 23);
            this.txtEmployee.TabIndex = 0;
            // 
            // txtNickName
            // 
            this.txtNickName.BackColor = System.Drawing.Color.White;
            this.txtNickName.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtNickName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Name", true));
            this.txtNickName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtNickName.Location = new System.Drawing.Point(132, 131);
            this.txtNickName.Name = "txtNickName";
            this.txtNickName.Size = new System.Drawing.Size(200, 23);
            this.txtNickName.TabIndex = 1;
            // 
            // txtSkill
            // 
            this.txtSkill.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSkill.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Skill", true));
            this.txtSkill.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSkill.IsSupportEditMode = false;
            this.txtSkill.Location = new System.Drawing.Point(132, 182);
            this.txtSkill.Name = "txtSkill";
            this.txtSkill.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.txtSkill.ReadOnly = true;
            this.txtSkill.Size = new System.Drawing.Size(140, 23);
            this.txtSkill.TabIndex = 2;
            this.txtSkill.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtSkill_PopUp);
            // 
            // dateHiredOn
            // 
            this.dateHiredOn.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "OnBoardDate", true));
            this.dateHiredOn.Location = new System.Drawing.Point(479, 32);
            this.dateHiredOn.Name = "dateHiredOn";
            this.dateHiredOn.Size = new System.Drawing.Size(130, 23);
            this.dateHiredOn.TabIndex = 3;
            // 
            // dateResigned
            // 
            this.dateResigned.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ResignationDate", true));
            this.dateResigned.Location = new System.Drawing.Point(479, 81);
            this.dateResigned.Name = "dateResigned";
            this.dateResigned.Size = new System.Drawing.Size(130, 23);
            this.dateResigned.TabIndex = 4;
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(253, 32);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(16, 23);
            this.labelM.TabIndex = 14;
            this.labelM.Text = "M";
            // 
            // displayM
            // 
            this.displayM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayM.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MDivisionID", true));
            this.displayM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayM.Location = new System.Drawing.Point(273, 32);
            this.displayM.Name = "displayM";
            this.displayM.Size = new System.Drawing.Size(40, 23);
            this.displayM.TabIndex = 15;
            // 
            // txtmfactory
            // 
            this.txtmfactory.BackColor = System.Drawing.Color.White;
            this.txtmfactory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "FactoryID", true));
            this.txtmfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtmfactory.Location = new System.Drawing.Point(132, 32);
            this.txtmfactory.Name = "txtmfactory";
            this.txtmfactory.Size = new System.Drawing.Size(66, 23);
            this.txtmfactory.TabIndex = 16;
            this.txtmfactory.FilteMDivision = true;
            // 
            // txtsewingline
            // 
            this.txtsewingline.BackColor = System.Drawing.Color.White;
            this.txtsewingline.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SewingLineID", true));
            this.txtsewingline.FactoryobjectName = this.txtmfactory;
            this.txtsewingline.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtsewingline.Location = new System.Drawing.Point(479, 132);
            this.txtsewingline.Name = "txtsewingline";
            this.txtsewingline.Size = new System.Drawing.Size(60, 23);
            this.txtsewingline.TabIndex = 5;
            // 
            // B08
            // 
            this.ClientSize = new System.Drawing.Size(840, 457);
            this.DefaultControl = "txtEmployee";
            this.DefaultControlForEdit = "txtNickName";
            this.DefaultOrder = "FactoryID,ID";
            this.IsSupportCopy = false;
            this.Name = "B08";
            this.Text = "B08. Employee data maintain";
            this.UniqueExpress = "FactoryID,ID";
            this.WorkAlias = "Employee";
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

        private Win.UI.DateBox dateResigned;
        private Win.UI.DateBox dateHiredOn;
        private Win.UI.TextBox txtSkill;
        private Win.UI.TextBox txtNickName;
        private Win.UI.TextBox txtEmployee;
        private Win.UI.Label labelLine;
        private Win.UI.Label labelResigned;
        private Win.UI.Label labelHiredOn;
        private Win.UI.Label labelSkill;
        private Win.UI.Label labelNickName;
        private Win.UI.Label labelEmployee;
        private Win.UI.Label labelFactory;
        private Class.Txtsewingline txtsewingline;
        private Win.UI.DisplayBox displayM;
        private Win.UI.Label labelM;
        private Class.Txtfactory txtmfactory;
    }
}
