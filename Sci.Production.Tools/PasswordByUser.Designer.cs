namespace Sci.Production.Tools
{
    partial class PasswordByUser
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
            this.components = new System.ComponentModel.Container();
            this.radioGroup1 = new Sci.Win.UI.RadioGroup();
            this.textBox4 = new Sci.Win.UI.TextBox();
            this.txtuser3 = new Sci.Production.Class.txtuser();
            this.txtuser2 = new Sci.Production.Class.txtuser();
            this.txtuser1 = new Sci.Production.Class.txtuser();
            this.textBox8 = new Sci.Win.UI.TextBox();
            this.dateBox2 = new Sci.Win.UI.DateBox();
            this.dateBox1 = new Sci.Win.UI.DateBox();
            this.editBox2 = new Sci.Win.UI.EditBox();
            this.comboBox1 = new Sci.Win.UI.ComboBox();
            this.textBox7 = new Sci.Win.UI.TextBox();
            this.editBox1 = new Sci.Win.UI.EditBox();
            this.textBox3 = new Sci.Win.UI.TextBox();
            this.textBox2 = new Sci.Win.UI.TextBox();
            this.textBox1 = new Sci.Win.UI.TextBox();
            this.label15 = new Sci.Win.UI.Label();
            this.label14 = new Sci.Win.UI.Label();
            this.label13 = new Sci.Win.UI.Label();
            this.label12 = new Sci.Win.UI.Label();
            this.label11 = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.grid1 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.radioGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(993, 515);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.grid1);
            this.detailcont.Controls.Add(this.radioGroup1);
            this.detailcont.Size = new System.Drawing.Size(993, 477);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 477);
            this.detailbtm.Size = new System.Drawing.Size(993, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(993, 515);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(1001, 544);
            // 
            // radioGroup1
            // 
            this.radioGroup1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.radioGroup1.Controls.Add(this.textBox4);
            this.radioGroup1.Controls.Add(this.txtuser3);
            this.radioGroup1.Controls.Add(this.txtuser2);
            this.radioGroup1.Controls.Add(this.txtuser1);
            this.radioGroup1.Controls.Add(this.textBox8);
            this.radioGroup1.Controls.Add(this.dateBox2);
            this.radioGroup1.Controls.Add(this.dateBox1);
            this.radioGroup1.Controls.Add(this.editBox2);
            this.radioGroup1.Controls.Add(this.comboBox1);
            this.radioGroup1.Controls.Add(this.textBox7);
            this.radioGroup1.Controls.Add(this.editBox1);
            this.radioGroup1.Controls.Add(this.textBox3);
            this.radioGroup1.Controls.Add(this.textBox2);
            this.radioGroup1.Controls.Add(this.textBox1);
            this.radioGroup1.Controls.Add(this.label15);
            this.radioGroup1.Controls.Add(this.label14);
            this.radioGroup1.Controls.Add(this.label13);
            this.radioGroup1.Controls.Add(this.label12);
            this.radioGroup1.Controls.Add(this.label11);
            this.radioGroup1.Controls.Add(this.label10);
            this.radioGroup1.Controls.Add(this.label9);
            this.radioGroup1.Controls.Add(this.label8);
            this.radioGroup1.Controls.Add(this.label7);
            this.radioGroup1.Controls.Add(this.label6);
            this.radioGroup1.Controls.Add(this.label5);
            this.radioGroup1.Controls.Add(this.label4);
            this.radioGroup1.Controls.Add(this.label3);
            this.radioGroup1.Location = new System.Drawing.Point(10, 3);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Size = new System.Drawing.Size(463, 598);
            this.radioGroup1.TabIndex = 3;
            this.radioGroup1.TabStop = false;
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.textBox4.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Position", true));
            this.textBox4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.textBox4.IsSupportEditMode = false;
            this.textBox4.Location = new System.Drawing.Point(96, 275);
            this.textBox4.Name = "textBox4";
            this.textBox4.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(133, 23);
            this.textBox4.TabIndex = 10;
            // 
            // txtuser3
            // 
            this.txtuser3.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Deputy", true));
            this.txtuser3.DisplayBox1Binding = "";
            this.txtuser3.Location = new System.Drawing.Point(96, 207);
            this.txtuser3.Name = "txtuser3";
            this.txtuser3.Size = new System.Drawing.Size(302, 23);
            this.txtuser3.TabIndex = 8;
            this.txtuser3.TextBox1Binding = "";
            // 
            // txtuser2
            // 
            this.txtuser2.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Supervisor", true));
            this.txtuser2.DisplayBox1Binding = "";
            this.txtuser2.Location = new System.Drawing.Point(96, 175);
            this.txtuser2.Name = "txtuser2";
            this.txtuser2.Size = new System.Drawing.Size(302, 23);
            this.txtuser2.TabIndex = 7;
            this.txtuser2.TextBox1Binding = "";
            // 
            // txtuser1
            // 
            this.txtuser1.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Manager", true));
            this.txtuser1.DisplayBox1Binding = "";
            this.txtuser1.Location = new System.Drawing.Point(96, 142);
            this.txtuser1.Name = "txtuser1";
            this.txtuser1.Size = new System.Drawing.Size(302, 23);
            this.txtuser1.TabIndex = 6;
            this.txtuser1.TextBox1Binding = "";
            // 
            // textBox8
            // 
            this.textBox8.BackColor = System.Drawing.Color.White;
            this.textBox8.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.textBox8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox8.IsSupportEditMode = false;
            this.textBox8.Location = new System.Drawing.Point(96, 19);
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new System.Drawing.Size(121, 23);
            this.textBox8.TabIndex = 1;
            // 
            // dateBox2
            // 
            this.dateBox2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Resign", true));
            this.dateBox2.Location = new System.Drawing.Point(322, 309);
            this.dateBox2.Name = "dateBox2";
            this.dateBox2.Size = new System.Drawing.Size(130, 23);
            this.dateBox2.TabIndex = 13;
            // 
            // dateBox1
            // 
            this.dateBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "OnBoard", true));
            this.dateBox1.Location = new System.Drawing.Point(99, 309);
            this.dateBox1.Name = "dateBox1";
            this.dateBox1.Size = new System.Drawing.Size(130, 23);
            this.dateBox1.TabIndex = 12;
            // 
            // editBox2
            // 
            this.editBox2.BackColor = System.Drawing.Color.White;
            this.editBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox2.Location = new System.Drawing.Point(99, 343);
            this.editBox2.Multiline = true;
            this.editBox2.Name = "editBox2";
            this.editBox2.Size = new System.Drawing.Size(356, 125);
            this.editBox2.TabIndex = 14;
            // 
            // comboBox1
            // 
            this.comboBox1.BackColor = System.Drawing.Color.White;
            this.comboBox1.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "CodePage", true));
            this.comboBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.IsSupportUnselect = true;
            this.comboBox1.Location = new System.Drawing.Point(322, 275);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 24);
            this.comboBox1.TabIndex = 11;
            // 
            // textBox7
            // 
            this.textBox7.BackColor = System.Drawing.Color.White;
            this.textBox7.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.textBox7.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "EMail", true));
            this.textBox7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox7.Location = new System.Drawing.Point(96, 241);
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new System.Drawing.Size(362, 23);
            this.textBox7.TabIndex = 9;
            // 
            // editBox1
            // 
            this.editBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Factory", true));
            this.editBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editBox1.IsSupportEditMode = false;
            this.editBox1.Location = new System.Drawing.Point(96, 84);
            this.editBox1.Multiline = true;
            this.editBox1.Name = "editBox1";
            this.editBox1.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.editBox1.ReadOnly = true;
            this.editBox1.Size = new System.Drawing.Size(356, 50);
            this.editBox1.TabIndex = 5;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.White;
            this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ExtNo", true));
            this.textBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox3.Location = new System.Drawing.Point(322, 51);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(110, 23);
            this.textBox3.TabIndex = 4;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Password", true));
            this.textBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox2.Location = new System.Drawing.Point(96, 51);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(121, 23);
            this.textBox2.TabIndex = 3;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Name", true));
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox1.Location = new System.Drawing.Point(218, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(240, 23);
            this.textBox1.TabIndex = 2;
            // 
            // label15
            // 
            this.label15.Lines = 0;
            this.label15.Location = new System.Drawing.Point(12, 343);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(81, 23);
            this.label15.TabIndex = 13;
            this.label15.Text = "Remark";
            // 
            // label14
            // 
            this.label14.Lines = 0;
            this.label14.Location = new System.Drawing.Point(238, 309);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(81, 23);
            this.label14.TabIndex = 12;
            this.label14.Text = "Resign";
            // 
            // label13
            // 
            this.label13.Lines = 0;
            this.label13.Location = new System.Drawing.Point(238, 275);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(81, 23);
            this.label13.TabIndex = 11;
            this.label13.Text = "Language";
            // 
            // label12
            // 
            this.label12.Lines = 0;
            this.label12.Location = new System.Drawing.Point(238, 51);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(81, 23);
            this.label12.TabIndex = 10;
            this.label12.Text = "Ext#";
            // 
            // label11
            // 
            this.label11.Lines = 0;
            this.label11.Location = new System.Drawing.Point(12, 309);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(81, 23);
            this.label11.TabIndex = 9;
            this.label11.Text = "Date Hired";
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(12, 275);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(81, 23);
            this.label10.TabIndex = 8;
            this.label10.Text = "Position";
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(12, 241);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(81, 23);
            this.label9.TabIndex = 7;
            this.label9.Text = "E-Mail Addr.";
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(12, 207);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 23);
            this.label8.TabIndex = 6;
            this.label8.Text = "Deputy";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(12, 175);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(81, 23);
            this.label7.TabIndex = 5;
            this.label7.Text = "Supervisor";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(12, 142);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(81, 23);
            this.label6.TabIndex = 4;
            this.label6.Text = "Manager";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(12, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(81, 23);
            this.label5.TabIndex = 3;
            this.label5.Text = "Factory";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(12, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 23);
            this.label4.TabIndex = 2;
            this.label4.Text = "Password";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(12, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 23);
            this.label3.TabIndex = 1;
            this.label3.Text = "ID";
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(479, 8);
            this.grid1.Name = "grid1";
            this.grid1.RowHeadersVisible = false;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(514, 469);
            this.grid1.TabIndex = 15;
            this.grid1.TabStop = false;
            // 
            // PasswordByUser
            // 
            this.ClientSize = new System.Drawing.Size(1001, 577);
            this.DefaultControl = "textBox8";
            this.GridAlias = "Pass2";
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.Name = "PasswordByUser";
            this.Text = "Password by User";
            this.WorkAlias = "Pass1";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.radioGroup1.ResumeLayout(false);
            this.radioGroup1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.RadioGroup radioGroup1;
        private Win.UI.TextBox textBox8;
        private Win.UI.DateBox dateBox2;
        private Win.UI.DateBox dateBox1;
        private Win.UI.EditBox editBox2;
        private Win.UI.ComboBox comboBox1;
        private Win.UI.TextBox textBox7;
        private Win.UI.EditBox editBox1;
        private Win.UI.TextBox textBox3;
        private Win.UI.TextBox textBox2;
        private Win.UI.TextBox textBox1;
        private Win.UI.Label label15;
        private Win.UI.Label label14;
        private Win.UI.Label label13;
        private Win.UI.Label label12;
        private Win.UI.Label label11;
        private Win.UI.Label label10;
        private Win.UI.Label label9;
        private Win.UI.Label label8;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Grid grid1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Class.txtuser txtuser3;
        private Class.txtuser txtuser2;
        private Class.txtuser txtuser1;
        private Win.UI.TextBox textBox4;
    }
}
