namespace Sci.Production.Tools
{
    partial class AuthorityByPosition_Setting
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
            this.gridPositionAuthority = new Sci.Win.UI.Grid();
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.btnAllControl = new Sci.Win.UI.Button();
            this.btnReadonly = new Sci.Win.UI.Button();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.checkReturn = new Sci.Win.UI.CheckBox();
            this.checkReceive = new Sci.Win.UI.CheckBox();
            this.checkUnClose = new Sci.Win.UI.CheckBox();
            this.checkClose = new Sci.Win.UI.CheckBox();
            this.checkUncheck = new Sci.Win.UI.CheckBox();
            this.checkCheck = new Sci.Win.UI.CheckBox();
            this.checkRecall = new Sci.Win.UI.CheckBox();
            this.checkSend = new Sci.Win.UI.CheckBox();
            this.checkUnConfirm = new Sci.Win.UI.CheckBox();
            this.checkConfirm = new Sci.Win.UI.CheckBox();
            this.checkPrint = new Sci.Win.UI.CheckBox();
            this.checkDelete = new Sci.Win.UI.CheckBox();
            this.checkEdit = new Sci.Win.UI.CheckBox();
            this.checkNew = new Sci.Win.UI.CheckBox();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnUndo = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridPositionAuthority)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridPositionAuthority
            // 
            this.gridPositionAuthority.AllowUserToAddRows = false;
            this.gridPositionAuthority.AllowUserToDeleteRows = false;
            this.gridPositionAuthority.AllowUserToResizeRows = false;
            this.gridPositionAuthority.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridPositionAuthority.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridPositionAuthority.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridPositionAuthority.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridPositionAuthority.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridPositionAuthority.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridPositionAuthority.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridPositionAuthority.Location = new System.Drawing.Point(12, 12);
            this.gridPositionAuthority.Name = "gridPositionAuthority";
            this.gridPositionAuthority.RowHeadersVisible = false;
            this.gridPositionAuthority.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridPositionAuthority.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridPositionAuthority.RowTemplate.Height = 24;
            this.gridPositionAuthority.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridPositionAuthority.Size = new System.Drawing.Size(361, 279);
            this.gridPositionAuthority.TabIndex = 0;
            this.gridPositionAuthority.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnAllControl);
            this.groupBox1.Controls.Add(this.btnReadonly);
            this.groupBox1.Controls.Add(this.checkJunk);
            this.groupBox1.Controls.Add(this.checkReturn);
            this.groupBox1.Controls.Add(this.checkReceive);
            this.groupBox1.Controls.Add(this.checkUnClose);
            this.groupBox1.Controls.Add(this.checkClose);
            this.groupBox1.Controls.Add(this.checkUncheck);
            this.groupBox1.Controls.Add(this.checkCheck);
            this.groupBox1.Controls.Add(this.checkRecall);
            this.groupBox1.Controls.Add(this.checkSend);
            this.groupBox1.Controls.Add(this.checkUnConfirm);
            this.groupBox1.Controls.Add(this.checkConfirm);
            this.groupBox1.Controls.Add(this.checkPrint);
            this.groupBox1.Controls.Add(this.checkDelete);
            this.groupBox1.Controls.Add(this.checkEdit);
            this.groupBox1.Controls.Add(this.checkNew);
            this.groupBox1.Location = new System.Drawing.Point(379, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(201, 288);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // btnAllControl
            // 
            this.btnAllControl.Location = new System.Drawing.Point(103, 252);
            this.btnAllControl.Name = "btnAllControl";
            this.btnAllControl.Size = new System.Drawing.Size(93, 30);
            this.btnAllControl.TabIndex = 26;
            this.btnAllControl.Text = "All Control";
            this.btnAllControl.UseVisualStyleBackColor = true;
            this.btnAllControl.Click += new System.EventHandler(this.BtnAllControl_Click);
            // 
            // btnReadonly
            // 
            this.btnReadonly.Location = new System.Drawing.Point(5, 252);
            this.btnReadonly.Name = "btnReadonly";
            this.btnReadonly.Size = new System.Drawing.Size(93, 30);
            this.btnReadonly.TabIndex = 4;
            this.btnReadonly.Text = "Readonly";
            this.btnReadonly.UseVisualStyleBackColor = true;
            this.btnReadonly.Click += new System.EventHandler(this.BtnReadonly_Click);
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.FalseValue = "0";
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(109, 185);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 25;
            this.checkJunk.Text = "Junk";
            this.checkJunk.TrueValue = "1";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // checkReturn
            // 
            this.checkReturn.AutoSize = true;
            this.checkReturn.FalseValue = "0";
            this.checkReturn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkReturn.Location = new System.Drawing.Point(109, 158);
            this.checkReturn.Name = "checkReturn";
            this.checkReturn.Size = new System.Drawing.Size(70, 21);
            this.checkReturn.TabIndex = 24;
            this.checkReturn.Text = "Return";
            this.checkReturn.TrueValue = "1";
            this.checkReturn.UseVisualStyleBackColor = true;
            // 
            // checkReceive
            // 
            this.checkReceive.AutoSize = true;
            this.checkReceive.FalseValue = "0";
            this.checkReceive.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkReceive.Location = new System.Drawing.Point(109, 131);
            this.checkReceive.Name = "checkReceive";
            this.checkReceive.Size = new System.Drawing.Size(78, 21);
            this.checkReceive.TabIndex = 23;
            this.checkReceive.Text = "Receive";
            this.checkReceive.TrueValue = "1";
            this.checkReceive.UseVisualStyleBackColor = true;
            // 
            // checkUnClose
            // 
            this.checkUnClose.AutoSize = true;
            this.checkUnClose.FalseValue = "0";
            this.checkUnClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkUnClose.Location = new System.Drawing.Point(109, 103);
            this.checkUnClose.Name = "checkUnClose";
            this.checkUnClose.Size = new System.Drawing.Size(80, 21);
            this.checkUnClose.TabIndex = 22;
            this.checkUnClose.Text = "UnClose";
            this.checkUnClose.TrueValue = "1";
            this.checkUnClose.UseVisualStyleBackColor = true;
            // 
            // checkClose
            // 
            this.checkClose.AutoSize = true;
            this.checkClose.FalseValue = "0";
            this.checkClose.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkClose.Location = new System.Drawing.Point(109, 76);
            this.checkClose.Name = "checkClose";
            this.checkClose.Size = new System.Drawing.Size(62, 21);
            this.checkClose.TabIndex = 21;
            this.checkClose.Text = "Close";
            this.checkClose.TrueValue = "1";
            this.checkClose.UseVisualStyleBackColor = true;
            // 
            // checkUncheck
            // 
            this.checkUncheck.AutoSize = true;
            this.checkUncheck.FalseValue = "0";
            this.checkUncheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkUncheck.Location = new System.Drawing.Point(109, 49);
            this.checkUncheck.Name = "checkUncheck";
            this.checkUncheck.Size = new System.Drawing.Size(84, 21);
            this.checkUncheck.TabIndex = 20;
            this.checkUncheck.Text = "UnCheck";
            this.checkUncheck.TrueValue = "1";
            this.checkUncheck.UseVisualStyleBackColor = true;
            // 
            // checkCheck
            // 
            this.checkCheck.AutoSize = true;
            this.checkCheck.FalseValue = "0";
            this.checkCheck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkCheck.Location = new System.Drawing.Point(109, 22);
            this.checkCheck.Name = "checkCheck";
            this.checkCheck.Size = new System.Drawing.Size(66, 21);
            this.checkCheck.TabIndex = 19;
            this.checkCheck.Text = "Check";
            this.checkCheck.TrueValue = "1";
            this.checkCheck.UseVisualStyleBackColor = true;
            // 
            // checkRecall
            // 
            this.checkRecall.AutoSize = true;
            this.checkRecall.FalseValue = "0";
            this.checkRecall.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkRecall.Location = new System.Drawing.Point(11, 212);
            this.checkRecall.Name = "checkRecall";
            this.checkRecall.Size = new System.Drawing.Size(66, 21);
            this.checkRecall.TabIndex = 18;
            this.checkRecall.Text = "Recall";
            this.checkRecall.TrueValue = "1";
            this.checkRecall.UseVisualStyleBackColor = true;
            // 
            // checkSend
            // 
            this.checkSend.AutoSize = true;
            this.checkSend.FalseValue = "0";
            this.checkSend.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkSend.Location = new System.Drawing.Point(11, 185);
            this.checkSend.Name = "checkSend";
            this.checkSend.Size = new System.Drawing.Size(60, 21);
            this.checkSend.TabIndex = 17;
            this.checkSend.Text = "Send";
            this.checkSend.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkSend.TrueValue = "1";
            this.checkSend.UseVisualStyleBackColor = true;
            // 
            // checkUnConfirm
            // 
            this.checkUnConfirm.AutoSize = true;
            this.checkUnConfirm.FalseValue = "0";
            this.checkUnConfirm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkUnConfirm.Location = new System.Drawing.Point(11, 157);
            this.checkUnConfirm.Name = "checkUnConfirm";
            this.checkUnConfirm.Size = new System.Drawing.Size(93, 21);
            this.checkUnConfirm.TabIndex = 16;
            this.checkUnConfirm.Text = "UnConfirm";
            this.checkUnConfirm.TrueValue = "1";
            this.checkUnConfirm.UseVisualStyleBackColor = true;
            // 
            // checkConfirm
            // 
            this.checkConfirm.AutoSize = true;
            this.checkConfirm.FalseValue = "0";
            this.checkConfirm.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkConfirm.Location = new System.Drawing.Point(11, 130);
            this.checkConfirm.Name = "checkConfirm";
            this.checkConfirm.Size = new System.Drawing.Size(75, 21);
            this.checkConfirm.TabIndex = 15;
            this.checkConfirm.Text = "Confirm";
            this.checkConfirm.TrueValue = "1";
            this.checkConfirm.UseVisualStyleBackColor = true;
            // 
            // checkPrint
            // 
            this.checkPrint.AutoSize = true;
            this.checkPrint.FalseValue = "0";
            this.checkPrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkPrint.Location = new System.Drawing.Point(11, 103);
            this.checkPrint.Name = "checkPrint";
            this.checkPrint.Size = new System.Drawing.Size(56, 21);
            this.checkPrint.TabIndex = 11;
            this.checkPrint.Text = "Print";
            this.checkPrint.TrueValue = "1";
            this.checkPrint.UseVisualStyleBackColor = true;
            // 
            // checkDelete
            // 
            this.checkDelete.AutoSize = true;
            this.checkDelete.FalseValue = "0";
            this.checkDelete.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkDelete.Location = new System.Drawing.Point(11, 76);
            this.checkDelete.Name = "checkDelete";
            this.checkDelete.Size = new System.Drawing.Size(68, 21);
            this.checkDelete.TabIndex = 12;
            this.checkDelete.Text = "Delete";
            this.checkDelete.TrueValue = "1";
            this.checkDelete.UseVisualStyleBackColor = true;
            // 
            // checkEdit
            // 
            this.checkEdit.AutoSize = true;
            this.checkEdit.FalseValue = "0";
            this.checkEdit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkEdit.Location = new System.Drawing.Point(11, 49);
            this.checkEdit.Name = "checkEdit";
            this.checkEdit.Size = new System.Drawing.Size(51, 21);
            this.checkEdit.TabIndex = 10;
            this.checkEdit.Text = "Edit";
            this.checkEdit.TrueValue = "1";
            this.checkEdit.UseVisualStyleBackColor = true;
            // 
            // checkNew
            // 
            this.checkNew.AutoSize = true;
            this.checkNew.FalseValue = "0";
            this.checkNew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkNew.Location = new System.Drawing.Point(11, 22);
            this.checkNew.Name = "checkNew";
            this.checkNew.Size = new System.Drawing.Size(54, 21);
            this.checkNew.TabIndex = 9;
            this.checkNew.Text = "New";
            this.checkNew.TrueValue = "1";
            this.checkNew.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(586, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 2;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnUndo
            // 
            this.btnUndo.Location = new System.Drawing.Point(586, 46);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(80, 30);
            this.btnUndo.TabIndex = 3;
            this.btnUndo.Text = "Undo";
            this.btnUndo.UseVisualStyleBackColor = true;
            this.btnUndo.Click += new System.EventHandler(this.BtnUndo_Click);
            // 
            // AuthorityByPosition_Setting
            // 
            this.ClientSize = new System.Drawing.Size(678, 303);
            this.Controls.Add(this.btnUndo);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gridPositionAuthority);
            this.Name = "AuthorityByPosition_Setting";
            this.Text = "Position Authority";
            ((System.ComponentModel.ISupportInitialize)(this.gridPositionAuthority)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Grid gridPositionAuthority;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Button btnSave;
        private Win.UI.Button btnUndo;
        private Win.UI.CheckBox checkPrint;
        private Win.UI.CheckBox checkDelete;
        private Win.UI.CheckBox checkEdit;
        private Win.UI.CheckBox checkNew;
        private Win.UI.CheckBox checkUncheck;
        private Win.UI.CheckBox checkCheck;
        private Win.UI.CheckBox checkRecall;
        private Win.UI.CheckBox checkSend;
        private Win.UI.CheckBox checkUnConfirm;
        private Win.UI.CheckBox checkConfirm;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.CheckBox checkReturn;
        private Win.UI.CheckBox checkReceive;
        private Win.UI.CheckBox checkUnClose;
        private Win.UI.CheckBox checkClose;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Button btnAllControl;
        private Win.UI.Button btnReadonly;
    }
}
