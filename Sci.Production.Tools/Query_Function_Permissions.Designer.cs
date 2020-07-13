namespace Sci.Production.Tools
{
    partial class Query_Function_Permissions
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.txtUserID = new Sci.Production.Class.Txtmulituser();
            this.btnReset = new Sci.Win.UI.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkSend = new System.Windows.Forms.CheckBox();
            this.chkJunk = new System.Windows.Forms.CheckBox();
            this.chkReturn = new System.Windows.Forms.CheckBox();
            this.chkReceive = new System.Windows.Forms.CheckBox();
            this.chkUnClose = new System.Windows.Forms.CheckBox();
            this.chkClose = new System.Windows.Forms.CheckBox();
            this.chkUnCheck = new System.Windows.Forms.CheckBox();
            this.chkCheck = new System.Windows.Forms.CheckBox();
            this.chkRecall = new System.Windows.Forms.CheckBox();
            this.chkUnConfirmed = new System.Windows.Forms.CheckBox();
            this.chkConfirmed = new System.Windows.Forms.CheckBox();
            this.chkPrint = new System.Windows.Forms.CheckBox();
            this.chkDelete = new System.Windows.Forms.CheckBox();
            this.chkEdit = new System.Windows.Forms.CheckBox();
            this.chkNew = new System.Windows.Forms.CheckBox();
            this.labPermission = new Sci.Win.UI.Label();
            this.labPosition = new Sci.Win.UI.Label();
            this.txtPosition = new Sci.Win.UI.TextBox();
            this.txtFunction = new Sci.Win.UI.TextBox();
            this.labUserID = new Sci.Win.UI.Label();
            this.labModule = new Sci.Win.UI.Label();
            this.btnFind = new Sci.Win.UI.Button();
            this.txtModule = new Sci.Win.UI.TextBox();
            this.labFunction = new Sci.Win.UI.Label();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.grid = new Sci.Win.UI.Grid();
            this.panel1.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(820, 141);
            this.panel1.TabIndex = 55;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.txtUserID);
            this.panel6.Controls.Add(this.btnReset);
            this.panel6.Controls.Add(this.panel2);
            this.panel6.Controls.Add(this.labPermission);
            this.panel6.Controls.Add(this.labPosition);
            this.panel6.Controls.Add(this.txtPosition);
            this.panel6.Controls.Add(this.txtFunction);
            this.panel6.Controls.Add(this.labUserID);
            this.panel6.Controls.Add(this.labModule);
            this.panel6.Controls.Add(this.btnFind);
            this.panel6.Controls.Add(this.txtModule);
            this.panel6.Controls.Add(this.labFunction);
            this.panel6.Location = new System.Drawing.Point(0, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(815, 139);
            this.panel6.TabIndex = 45;
            // 
            // txtUserID
            // 
            this.txtUserID.DisplayBox1Binding = "";
            this.txtUserID.Location = new System.Drawing.Point(446, 5);
            this.txtUserID.Name = "txtUserID";
            this.txtUserID.Size = new System.Drawing.Size(275, 23);
            this.txtUserID.TabIndex = 1;
            this.txtUserID.TextBox1Binding = "";
            // 
            // btnReset
            // 
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnReset.Location = new System.Drawing.Point(731, 36);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(80, 30);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.chkSend);
            this.panel2.Controls.Add(this.chkJunk);
            this.panel2.Controls.Add(this.chkReturn);
            this.panel2.Controls.Add(this.chkReceive);
            this.panel2.Controls.Add(this.chkUnClose);
            this.panel2.Controls.Add(this.chkClose);
            this.panel2.Controls.Add(this.chkUnCheck);
            this.panel2.Controls.Add(this.chkCheck);
            this.panel2.Controls.Add(this.chkRecall);
            this.panel2.Controls.Add(this.chkUnConfirmed);
            this.panel2.Controls.Add(this.chkConfirmed);
            this.panel2.Controls.Add(this.chkPrint);
            this.panel2.Controls.Add(this.chkDelete);
            this.panel2.Controls.Add(this.chkEdit);
            this.panel2.Controls.Add(this.chkNew);
            this.panel2.Location = new System.Drawing.Point(103, 69);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(708, 66);
            this.panel2.TabIndex = 11;
            // 
            // chkSend
            // 
            this.chkSend.AutoSize = true;
            this.chkSend.ForeColor = System.Drawing.Color.Red;
            this.chkSend.Location = new System.Drawing.Point(513, 7);
            this.chkSend.Name = "chkSend";
            this.chkSend.Size = new System.Drawing.Size(60, 21);
            this.chkSend.TabIndex = 6;
            this.chkSend.Text = "Send";
            this.chkSend.UseVisualStyleBackColor = true;
            // 
            // chkJunk
            // 
            this.chkJunk.AutoSize = true;
            this.chkJunk.ForeColor = System.Drawing.Color.Red;
            this.chkJunk.Location = new System.Drawing.Point(592, 34);
            this.chkJunk.Name = "chkJunk";
            this.chkJunk.Size = new System.Drawing.Size(57, 21);
            this.chkJunk.TabIndex = 14;
            this.chkJunk.Text = "Junk";
            this.chkJunk.UseVisualStyleBackColor = true;
            // 
            // chkReturn
            // 
            this.chkReturn.AutoSize = true;
            this.chkReturn.ForeColor = System.Drawing.Color.Red;
            this.chkReturn.Location = new System.Drawing.Point(513, 34);
            this.chkReturn.Name = "chkReturn";
            this.chkReturn.Size = new System.Drawing.Size(70, 21);
            this.chkReturn.TabIndex = 13;
            this.chkReturn.Text = "Return";
            this.chkReturn.UseVisualStyleBackColor = true;
            // 
            // chkReceive
            // 
            this.chkReceive.AutoSize = true;
            this.chkReceive.ForeColor = System.Drawing.Color.Red;
            this.chkReceive.Location = new System.Drawing.Point(403, 34);
            this.chkReceive.Name = "chkReceive";
            this.chkReceive.Size = new System.Drawing.Size(78, 21);
            this.chkReceive.TabIndex = 12;
            this.chkReceive.Text = "Receive";
            this.chkReceive.UseVisualStyleBackColor = true;
            // 
            // chkUnClose
            // 
            this.chkUnClose.AutoSize = true;
            this.chkUnClose.ForeColor = System.Drawing.Color.Red;
            this.chkUnClose.Location = new System.Drawing.Point(309, 34);
            this.chkUnClose.Name = "chkUnClose";
            this.chkUnClose.Size = new System.Drawing.Size(80, 21);
            this.chkUnClose.TabIndex = 11;
            this.chkUnClose.Text = "UnClose";
            this.chkUnClose.UseVisualStyleBackColor = true;
            // 
            // chkClose
            // 
            this.chkClose.AutoSize = true;
            this.chkClose.ForeColor = System.Drawing.Color.Red;
            this.chkClose.Location = new System.Drawing.Point(244, 34);
            this.chkClose.Name = "chkClose";
            this.chkClose.Size = new System.Drawing.Size(62, 21);
            this.chkClose.TabIndex = 10;
            this.chkClose.Text = "Close";
            this.chkClose.UseVisualStyleBackColor = true;
            // 
            // chkUnCheck
            // 
            this.chkUnCheck.AutoSize = true;
            this.chkUnCheck.ForeColor = System.Drawing.Color.Red;
            this.chkUnCheck.Location = new System.Drawing.Point(156, 34);
            this.chkUnCheck.Name = "chkUnCheck";
            this.chkUnCheck.Size = new System.Drawing.Size(84, 21);
            this.chkUnCheck.TabIndex = 9;
            this.chkUnCheck.Text = "UnCheck";
            this.chkUnCheck.UseVisualStyleBackColor = true;
            // 
            // chkCheck
            // 
            this.chkCheck.AutoSize = true;
            this.chkCheck.ForeColor = System.Drawing.Color.Red;
            this.chkCheck.Location = new System.Drawing.Point(85, 34);
            this.chkCheck.Name = "chkCheck";
            this.chkCheck.Size = new System.Drawing.Size(66, 21);
            this.chkCheck.TabIndex = 8;
            this.chkCheck.Text = "Check";
            this.chkCheck.UseVisualStyleBackColor = true;
            // 
            // chkRecall
            // 
            this.chkRecall.AutoSize = true;
            this.chkRecall.ForeColor = System.Drawing.Color.Red;
            this.chkRecall.Location = new System.Drawing.Point(19, 34);
            this.chkRecall.Name = "chkRecall";
            this.chkRecall.Size = new System.Drawing.Size(66, 21);
            this.chkRecall.TabIndex = 7;
            this.chkRecall.Text = "Recall";
            this.chkRecall.UseVisualStyleBackColor = true;
            // 
            // chkUnConfirmed
            // 
            this.chkUnConfirmed.AutoSize = true;
            this.chkUnConfirmed.ForeColor = System.Drawing.Color.Red;
            this.chkUnConfirmed.Location = new System.Drawing.Point(403, 7);
            this.chkUnConfirmed.Name = "chkUnConfirmed";
            this.chkUnConfirmed.Size = new System.Drawing.Size(109, 21);
            this.chkUnConfirmed.TabIndex = 5;
            this.chkUnConfirmed.Text = "UnConfirmed";
            this.chkUnConfirmed.UseVisualStyleBackColor = true;
            // 
            // chkConfirmed
            // 
            this.chkConfirmed.AutoSize = true;
            this.chkConfirmed.ForeColor = System.Drawing.Color.Red;
            this.chkConfirmed.Location = new System.Drawing.Point(309, 7);
            this.chkConfirmed.Name = "chkConfirmed";
            this.chkConfirmed.Size = new System.Drawing.Size(91, 21);
            this.chkConfirmed.TabIndex = 4;
            this.chkConfirmed.Text = "Confirmed";
            this.chkConfirmed.UseVisualStyleBackColor = true;
            // 
            // chkPrint
            // 
            this.chkPrint.AutoSize = true;
            this.chkPrint.ForeColor = System.Drawing.Color.Red;
            this.chkPrint.Location = new System.Drawing.Point(244, 7);
            this.chkPrint.Name = "chkPrint";
            this.chkPrint.Size = new System.Drawing.Size(56, 21);
            this.chkPrint.TabIndex = 3;
            this.chkPrint.Text = "Print";
            this.chkPrint.UseVisualStyleBackColor = true;
            // 
            // chkDelete
            // 
            this.chkDelete.AutoSize = true;
            this.chkDelete.ForeColor = System.Drawing.Color.Red;
            this.chkDelete.Location = new System.Drawing.Point(156, 7);
            this.chkDelete.Name = "chkDelete";
            this.chkDelete.Size = new System.Drawing.Size(68, 21);
            this.chkDelete.TabIndex = 2;
            this.chkDelete.Text = "Delete";
            this.chkDelete.UseVisualStyleBackColor = true;
            // 
            // chkEdit
            // 
            this.chkEdit.AutoSize = true;
            this.chkEdit.ForeColor = System.Drawing.Color.Red;
            this.chkEdit.Location = new System.Drawing.Point(85, 7);
            this.chkEdit.Name = "chkEdit";
            this.chkEdit.Size = new System.Drawing.Size(51, 21);
            this.chkEdit.TabIndex = 1;
            this.chkEdit.Text = "Edit";
            this.chkEdit.UseVisualStyleBackColor = true;
            // 
            // chkNew
            // 
            this.chkNew.AutoSize = true;
            this.chkNew.ForeColor = System.Drawing.Color.Red;
            this.chkNew.Location = new System.Drawing.Point(19, 7);
            this.chkNew.Name = "chkNew";
            this.chkNew.Size = new System.Drawing.Size(54, 21);
            this.chkNew.TabIndex = 0;
            this.chkNew.Text = "New";
            this.chkNew.UseVisualStyleBackColor = true;
            // 
            // labPermission
            // 
            this.labPermission.Location = new System.Drawing.Point(8, 69);
            this.labPermission.Name = "labPermission";
            this.labPermission.Size = new System.Drawing.Size(92, 23);
            this.labPermission.TabIndex = 10;
            this.labPermission.Text = "Permission";
            // 
            // labPosition
            // 
            this.labPosition.Location = new System.Drawing.Point(351, 38);
            this.labPosition.Name = "labPosition";
            this.labPosition.Size = new System.Drawing.Size(92, 23);
            this.labPosition.TabIndex = 9;
            this.labPosition.Text = "Position";
            // 
            // txtPosition
            // 
            this.txtPosition.BackColor = System.Drawing.Color.White;
            this.txtPosition.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPosition.IsSupportEditMode = false;
            this.txtPosition.Location = new System.Drawing.Point(446, 38);
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.Size = new System.Drawing.Size(275, 23);
            this.txtPosition.TabIndex = 3;
            this.txtPosition.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtPosition_PopUp);
            this.txtPosition.Validating += new System.ComponentModel.CancelEventHandler(this.TxtPosition_Validating);
            // 
            // txtFunction
            // 
            this.txtFunction.BackColor = System.Drawing.Color.White;
            this.txtFunction.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFunction.IsSupportEditMode = false;
            this.txtFunction.Location = new System.Drawing.Point(103, 38);
            this.txtFunction.Name = "txtFunction";
            this.txtFunction.Size = new System.Drawing.Size(236, 23);
            this.txtFunction.TabIndex = 2;
            this.txtFunction.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtFunction_PopUp);
            this.txtFunction.Validating += new System.ComponentModel.CancelEventHandler(this.TxtFunction_Validating);
            // 
            // labUserID
            // 
            this.labUserID.Location = new System.Drawing.Point(351, 5);
            this.labUserID.Name = "labUserID";
            this.labUserID.Size = new System.Drawing.Size(92, 23);
            this.labUserID.TabIndex = 7;
            this.labUserID.Text = "User ID";
            // 
            // labModule
            // 
            this.labModule.Location = new System.Drawing.Point(8, 5);
            this.labModule.Name = "labModule";
            this.labModule.Size = new System.Drawing.Size(92, 23);
            this.labModule.TabIndex = 6;
            this.labModule.Text = "Module";
            // 
            // btnFind
            // 
            this.btnFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnFind.Location = new System.Drawing.Point(731, 3);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 30);
            this.btnFind.TabIndex = 4;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // txtModule
            // 
            this.txtModule.BackColor = System.Drawing.Color.White;
            this.txtModule.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtModule.IsSupportEditMode = false;
            this.txtModule.Location = new System.Drawing.Point(103, 5);
            this.txtModule.Name = "txtModule";
            this.txtModule.Size = new System.Drawing.Size(236, 23);
            this.txtModule.TabIndex = 0;
            this.txtModule.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtModule_PopUp);
            this.txtModule.Validating += new System.ComponentModel.CancelEventHandler(this.TxtModule_Validating);
            // 
            // labFunction
            // 
            this.labFunction.Location = new System.Drawing.Point(8, 38);
            this.labFunction.Name = "labFunction";
            this.labFunction.Size = new System.Drawing.Size(92, 23);
            this.labFunction.TabIndex = 8;
            this.labFunction.Text = "Function";
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.DataSource = this.listControlBindingSource1;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(0, 141);
            this.grid.Name = "grid";
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(820, 409);
            this.grid.TabIndex = 56;
            this.grid.TabStop = false;
            // 
            // Query_Function_Permissions
            // 
            this.ClientSize = new System.Drawing.Size(820, 550);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.panel1);
            this.Name = "Query_Function_Permissions";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "Query Function Permissions";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.grid, 0);
            this.panel1.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkNew;
        private System.Windows.Forms.Panel panel6;
        private Win.UI.Label labModule;
        private Win.UI.Button btnFind;
        private Win.UI.TextBox txtModule;
        private Win.UI.Label labFunction;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Label labUserID;
        private Win.UI.Label labPosition;
        private Win.UI.TextBox txtPosition;
        private Win.UI.TextBox txtFunction;
        private System.Windows.Forms.Panel panel2;
        private Win.UI.Label labPermission;
        private Win.UI.Button btnReset;
        private System.Windows.Forms.CheckBox chkSend;
        private System.Windows.Forms.CheckBox chkJunk;
        private System.Windows.Forms.CheckBox chkReturn;
        private System.Windows.Forms.CheckBox chkReceive;
        private System.Windows.Forms.CheckBox chkUnClose;
        private System.Windows.Forms.CheckBox chkClose;
        private System.Windows.Forms.CheckBox chkUnCheck;
        private System.Windows.Forms.CheckBox chkCheck;
        private System.Windows.Forms.CheckBox chkRecall;
        private System.Windows.Forms.CheckBox chkUnConfirmed;
        private System.Windows.Forms.CheckBox chkConfirmed;
        private System.Windows.Forms.CheckBox chkPrint;
        private System.Windows.Forms.CheckBox chkDelete;
        private System.Windows.Forms.CheckBox chkEdit;
        private Class.Txtmulituser txtUserID;
        private Win.UI.Grid grid;
    }
}
