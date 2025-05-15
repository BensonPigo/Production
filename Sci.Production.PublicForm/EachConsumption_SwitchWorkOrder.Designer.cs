namespace Sci.Production.PublicForm
{
    partial class EachConsumption_SwitchWorkOrder
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
            this.radioBySP = new Sci.Win.UI.RadioButton();
            this.radioCombination = new Sci.Win.UI.RadioButton();
            this.btnOK = new Sci.Win.UI.Button();
            this.btnCancel = new Sci.Win.UI.Button();
            this.grid1 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.radioUseCutRefToRequestFabric = new Sci.Win.UI.RadioGroup();
            this.radioButtonNo = new Sci.Win.UI.RadioButton();
            this.radioButtonYes = new Sci.Win.UI.RadioButton();
            this.label3 = new Sci.Win.UI.Label();
            this.radioGroup1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.radioUseCutRefToRequestFabric.SuspendLayout();
            this.SuspendLayout();
            // 
            // radioGroup1
            // 
            this.radioGroup1.Controls.Add(this.radioBySP);
            this.radioGroup1.Controls.Add(this.radioCombination);
            this.radioGroup1.Location = new System.Drawing.Point(11, 8);
            this.radioGroup1.Name = "radioGroup1";
            this.radioGroup1.Size = new System.Drawing.Size(183, 85);
            this.radioGroup1.TabIndex = 0;
            this.radioGroup1.TabStop = false;
            // 
            // radioBySP
            // 
            this.radioBySP.AutoSize = true;
            this.radioBySP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioBySP.Location = new System.Drawing.Point(19, 48);
            this.radioBySP.Name = "radioBySP";
            this.radioBySP.Size = new System.Drawing.Size(72, 21);
            this.radioBySP.TabIndex = 1;
            this.radioBySP.TabStop = true;
            this.radioBySP.Text = "By SP#";
            this.radioBySP.UseVisualStyleBackColor = true;
            // 
            // radioCombination
            // 
            this.radioCombination.AutoSize = true;
            this.radioCombination.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioCombination.Location = new System.Drawing.Point(19, 21);
            this.radioCombination.Name = "radioCombination";
            this.radioCombination.Size = new System.Drawing.Size(104, 21);
            this.radioCombination.TabIndex = 0;
            this.radioCombination.TabStop = true;
            this.radioCombination.Text = "Combination";
            this.radioCombination.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(669, 15);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 30);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(669, 51);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
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
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(12, 99);
            this.grid1.Name = "grid1";
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(737, 185);
            this.grid1.TabIndex = 3;
            // 
            // radioUseCutRefToRequestFabric
            // 
            this.radioUseCutRefToRequestFabric.Controls.Add(this.label3);
            this.radioUseCutRefToRequestFabric.Controls.Add(this.radioButtonNo);
            this.radioUseCutRefToRequestFabric.Controls.Add(this.radioButtonYes);
            this.radioUseCutRefToRequestFabric.Location = new System.Drawing.Point(200, 8);
            this.radioUseCutRefToRequestFabric.Name = "radioUseCutRefToRequestFabric";
            this.radioUseCutRefToRequestFabric.Size = new System.Drawing.Size(278, 85);
            this.radioUseCutRefToRequestFabric.TabIndex = 4;
            this.radioUseCutRefToRequestFabric.TabStop = false;
            // 
            // radioButtonNo
            // 
            this.radioButtonNo.AutoSize = true;
            this.radioButtonNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioButtonNo.Location = new System.Drawing.Point(19, 55);
            this.radioButtonNo.Name = "radioButtonNo";
            this.radioButtonNo.Size = new System.Drawing.Size(44, 21);
            this.radioButtonNo.TabIndex = 1;
            this.radioButtonNo.TabStop = true;
            this.radioButtonNo.Text = "No";
            this.radioButtonNo.UseVisualStyleBackColor = true;
            this.radioButtonNo.Value = "2";
            // 
            // radioButtonYes
            // 
            this.radioButtonYes.AutoSize = true;
            this.radioButtonYes.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioButtonYes.Location = new System.Drawing.Point(19, 32);
            this.radioButtonYes.Name = "radioButtonYes";
            this.radioButtonYes.Size = new System.Drawing.Size(50, 21);
            this.radioButtonYes.TabIndex = 0;
            this.radioButtonYes.TabStop = true;
            this.radioButtonYes.Text = "Yes";
            this.radioButtonYes.UseVisualStyleBackColor = true;
            this.radioButtonYes.Value = "1";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(238, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Use Cutting Cutref# to Request Fabric";
            this.label3.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // EachConsumption_SwitchWorkOrder
            // 
            this.ClientSize = new System.Drawing.Size(761, 296);
            this.Controls.Add(this.radioUseCutRefToRequestFabric);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.radioGroup1);
            this.Name = "EachConsumption_SwitchWorkOrder";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Switch To WorkOrder";
            this.radioGroup1.ResumeLayout(false);
            this.radioGroup1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.radioUseCutRefToRequestFabric.ResumeLayout(false);
            this.radioUseCutRefToRequestFabric.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.RadioGroup radioGroup1;
        private Win.UI.RadioButton radioBySP;
        private Win.UI.RadioButton radioCombination;
        private Win.UI.Button btnOK;
        private Win.UI.Button btnCancel;
        private Win.UI.Grid grid1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.RadioGroup radioUseCutRefToRequestFabric;
        private Win.UI.RadioButton radioButtonNo;
        private Win.UI.RadioButton radioButtonYes;
        private Win.UI.Label label3;
    }
}
