namespace Sci.Production.Tools
{
    partial class NotificationSetting
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
            this.components = new System.ComponentModel.Container();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.txtID = new Sci.Win.UI.TextBox();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.txtName = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.ui_chkJunk = new Sci.Win.UI.CheckBox();
            this.cmbMenu = new Sci.Production.Class.ComboDropDownList(this.components);
            this.txtFormName = new Sci.Win.UI.TextBox();
            this.label6 = new Sci.Win.UI.Label();
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
            this.detail.Size = new System.Drawing.Size(634, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtFormName);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.cmbMenu);
            this.detailcont.Controls.Add(this.ui_chkJunk);
            this.detailcont.Controls.Add(this.txtName);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.editDescription);
            this.detailcont.Controls.Add(this.txtID);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Size = new System.Drawing.Size(634, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(634, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(634, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(642, 424);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(240, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(362, 7);
            this.editby.Size = new System.Drawing.Size(240, 23);
            // 
            // lblcreateby
            // 
            this.lblcreateby.Location = new System.Drawing.Point(5, 10);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(314, 10);
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.White;
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescription.Location = new System.Drawing.Point(149, 132);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.Size = new System.Drawing.Size(302, 65);
            this.editDescription.TabIndex = 4;
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtID.IsSupportEditMode = false;
            this.txtID.Location = new System.Drawing.Point(149, 71);
            this.txtID.Name = "txtID";
            this.txtID.ReadOnly = true;
            this.txtID.Size = new System.Drawing.Size(139, 23);
            this.txtID.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(71, 132);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 10;
            this.label4.Text = "Description";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(71, 71);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 9;
            this.label3.Text = "ID";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(71, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 8;
            this.label2.Text = "Menu";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.White;
            this.txtName.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Name", true));
            this.txtName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtName.Location = new System.Drawing.Point(149, 101);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(302, 23);
            this.txtName.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(71, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 14;
            this.label1.Text = "Name";
            // 
            // ui_chkJunk
            // 
            this.ui_chkJunk.AutoSize = true;
            this.ui_chkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.ui_chkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ui_chkJunk.Location = new System.Drawing.Point(394, 42);
            this.ui_chkJunk.Name = "ui_chkJunk";
            this.ui_chkJunk.Size = new System.Drawing.Size(57, 21);
            this.ui_chkJunk.TabIndex = 6;
            this.ui_chkJunk.Text = "Junk";
            this.ui_chkJunk.UseVisualStyleBackColor = true;
            // 
            // cmbMenu
            // 
            this.cmbMenu._Type = Sci.Production.Class.ComboDropDownList.ComboDropDownList_Type._None;
            this.cmbMenu.AddAllItem = false;
            this.cmbMenu.AddEmpty = false;
            this.cmbMenu.BackColor = System.Drawing.Color.White;
            this.cmbMenu.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "MenuName", true));
            this.cmbMenu.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbMenu.FormattingEnabled = true;
            this.cmbMenu.IsSupportUnselect = true;
            this.cmbMenu.Location = new System.Drawing.Point(149, 42);
            this.cmbMenu.Name = "cmbMenu";
            this.cmbMenu.OldText = "";
            this.cmbMenu.Size = new System.Drawing.Size(139, 24);
            this.cmbMenu.TabIndex = 27;
            this.cmbMenu.Type = "";
            // 
            // txtFormName
            // 
            this.txtFormName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtFormName.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.txtFormName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "FormName", true));
            this.txtFormName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtFormName.Location = new System.Drawing.Point(149, 203);
            this.txtFormName.Name = "txtFormName";
            this.txtFormName.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditMode;
            this.txtFormName.ReadOnly = true;
            this.txtFormName.Size = new System.Drawing.Size(302, 23);
            this.txtFormName.TabIndex = 28;
            this.txtFormName.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtFormName_PopUp);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(71, 203);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 23);
            this.label6.TabIndex = 29;
            this.label6.Text = "FormName";
            // 
            // NotificationSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 457);
            this.Name = "NotificationSetting";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "Notification Setting";
            this.WorkAlias = "NotificationList";
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
        private Win.UI.TextBox txtName;
        private Win.UI.Label label1;
        private Win.UI.EditBox editDescription;
        private Win.UI.TextBox txtID;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.CheckBox ui_chkJunk;
        private Class.ComboDropDownList cmbMenu;
        private Win.UI.TextBox txtFormName;
        private Win.UI.Label label6;
    }
}