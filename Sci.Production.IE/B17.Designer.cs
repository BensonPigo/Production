namespace Sci.Production.IE
{
    partial class B17
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
            this.labelFactory = new Sci.Win.UI.Label();
            this.lblPosition = new Sci.Win.UI.Label();
            this.lblDept = new Sci.Win.UI.Label();
            this.txtDept = new Sci.Win.UI.TextBox();
            this.txtPosition = new Sci.Win.UI.TextBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.chP03 = new Sci.Win.UI.CheckBox();
            this.chP06 = new Sci.Win.UI.CheckBox();
            this.txtfactory1 = new Sci.Production.Class.Txtfactory();
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
            this.detail.Size = new System.Drawing.Size(894, 388);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.chP06);
            this.detailcont.Controls.Add(this.chP03);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.txtfactory1);
            this.detailcont.Controls.Add(this.labelFactory);
            this.detailcont.Controls.Add(this.lblPosition);
            this.detailcont.Controls.Add(this.lblDept);
            this.detailcont.Controls.Add(this.txtDept);
            this.detailcont.Controls.Add(this.txtPosition);
            this.detailcont.Size = new System.Drawing.Size(894, 350);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 350);
            this.detailbtm.Size = new System.Drawing.Size(894, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(894, 388);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(902, 417);
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(26, 33);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(75, 23);
            this.labelFactory.TabIndex = 4;
            this.labelFactory.Text = "Factory";
            // 
            // lblPosition
            // 
            this.lblPosition.Location = new System.Drawing.Point(26, 117);
            this.lblPosition.Name = "lblPosition";
            this.lblPosition.Size = new System.Drawing.Size(75, 23);
            this.lblPosition.TabIndex = 21;
            this.lblPosition.Text = "Position";
            // 
            // lblDept
            // 
            this.lblDept.Location = new System.Drawing.Point(26, 74);
            this.lblDept.Name = "lblDept";
            this.lblDept.Size = new System.Drawing.Size(75, 23);
            this.lblDept.TabIndex = 19;
            this.lblDept.Text = "Dept";
            // 
            // txtDept
            // 
            this.txtDept.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtDept.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtDept.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Dept", true));
            this.txtDept.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtDept.Location = new System.Drawing.Point(104, 74);
            this.txtDept.Name = "txtDept";
            this.txtDept.ReadOnly = true;
            this.txtDept.Size = new System.Drawing.Size(200, 23);
            this.txtDept.TabIndex = 18;
            this.txtDept.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtDept_PopUp);
            // 
            // txtPosition
            // 
            this.txtPosition.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtPosition.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtPosition.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Position", true));
            this.txtPosition.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtPosition.Location = new System.Drawing.Point(105, 117);
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.ReadOnly = true;
            this.txtPosition.Size = new System.Drawing.Size(200, 23);
            this.txtPosition.TabIndex = 20;
            this.txtPosition.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtPosition_PopUp);
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(352, 33);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 23;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // chP03
            // 
            this.chP03.AutoSize = true;
            this.chP03.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "P03", true));
            this.chP03.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chP03.Location = new System.Drawing.Point(352, 74);
            this.chP03.Name = "chP03";
            this.chP03.Size = new System.Drawing.Size(68, 21);
            this.chP03.TabIndex = 24;
            this.chP03.Text = "IE P03";
            this.chP03.UseVisualStyleBackColor = true;
            // 
            // chP06
            // 
            this.chP06.AutoSize = true;
            this.chP06.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "P06", true));
            this.chP06.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chP06.Location = new System.Drawing.Point(352, 117);
            this.chP06.Name = "chP06";
            this.chP06.Size = new System.Drawing.Size(68, 21);
            this.chP06.TabIndex = 25;
            this.chP06.Text = "IE P06";
            this.chP06.UseVisualStyleBackColor = true;
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.BoolFtyGroupList = true;
            this.txtfactory1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "FactoryID", true));
            this.txtfactory1.FilteMDivision = false;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.IsIE = true;
            this.txtfactory1.IsMultiselect = false;
            this.txtfactory1.IsProduceFty = false;
            this.txtfactory1.IssupportJunk = false;
            this.txtfactory1.Location = new System.Drawing.Point(105, 33);
            this.txtfactory1.MDivision = null;
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.Size = new System.Drawing.Size(66, 23);
            this.txtfactory1.TabIndex = 22;
            // 
            // B17
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(902, 450);
            this.DefaultOrder = "FactoryID,Dept,Position";
            this.EnableGridJunkColor = true;
            this.ExpressQuery = true;
            this.Name = "B17";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B17. Employee Allocation Setting";
            this.WorkAlias = "EmployeeAllocationSetting";
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
        private Win.UI.Label labelFactory;
        private Win.UI.Label lblPosition;
        private Win.UI.Label lblDept;
        private Win.UI.TextBox txtDept;
        private Win.UI.TextBox txtPosition;
        private Class.Txtfactory txtfactory1;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.CheckBox chP06;
        private Win.UI.CheckBox chP03;
    }
}