namespace Sci.Production.Subcon
{
    partial class P23_ImportBarcode
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboTo = new Sci.Win.UI.ComboBox();
            this.label2 = new Sci.Win.UI.Label();
            this.btnImportfromscanner = new Sci.Win.UI.Button();
            this.comboSubprocess = new Sci.Win.UI.ComboBox();
            this.labelSubprocess = new Sci.Win.UI.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtNumsofBundle = new Sci.Win.UI.TextBox();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnCreate = new Sci.Win.UI.Button();
            this.label1 = new Sci.Win.UI.Label();
            this.grid1 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboTo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnImportfromscanner);
            this.panel1.Controls.Add(this.comboSubprocess);
            this.panel1.Controls.Add(this.labelSubprocess);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(983, 40);
            this.panel1.TabIndex = 0;
            // 
            // comboTo
            // 
            this.comboTo.BackColor = System.Drawing.Color.White;
            this.comboTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboTo.FormattingEnabled = true;
            this.comboTo.IsSupportUnselect = true;
            this.comboTo.Location = new System.Drawing.Point(306, 9);
            this.comboTo.Name = "comboTo";
            this.comboTo.Size = new System.Drawing.Size(179, 24);
            this.comboTo.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(277, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "To";
            // 
            // btnImportfromscanner
            // 
            this.btnImportfromscanner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportfromscanner.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImportfromscanner.Location = new System.Drawing.Point(784, 5);
            this.btnImportfromscanner.Name = "btnImportfromscanner";
            this.btnImportfromscanner.Size = new System.Drawing.Size(187, 30);
            this.btnImportfromscanner.TabIndex = 6;
            this.btnImportfromscanner.Text = "Import from scanner";
            this.btnImportfromscanner.UseVisualStyleBackColor = true;
            this.btnImportfromscanner.Click += new System.EventHandler(this.BtnImportfromscanner_Click);
            // 
            // comboSubprocess
            // 
            this.comboSubprocess.BackColor = System.Drawing.Color.White;
            this.comboSubprocess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSubprocess.FormattingEnabled = true;
            this.comboSubprocess.IsSupportUnselect = true;
            this.comboSubprocess.Location = new System.Drawing.Point(98, 8);
            this.comboSubprocess.Name = "comboSubprocess";
            this.comboSubprocess.Size = new System.Drawing.Size(121, 24);
            this.comboSubprocess.TabIndex = 4;
            // 
            // labelSubprocess
            // 
            this.labelSubprocess.Location = new System.Drawing.Point(9, 9);
            this.labelSubprocess.Name = "labelSubprocess";
            this.labelSubprocess.Size = new System.Drawing.Size(86, 23);
            this.labelSubprocess.TabIndex = 1;
            this.labelSubprocess.Text = "Sub Process";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtNumsofBundle);
            this.panel2.Controls.Add(this.btnClose);
            this.panel2.Controls.Add(this.btnCreate);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 478);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(983, 42);
            this.panel2.TabIndex = 1;
            // 
            // txtNumsofBundle
            // 
            this.txtNumsofBundle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtNumsofBundle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtNumsofBundle.IsSupportEditMode = false;
            this.txtNumsofBundle.Location = new System.Drawing.Point(129, 9);
            this.txtNumsofBundle.Name = "txtNumsofBundle";
            this.txtNumsofBundle.ReadOnly = true;
            this.txtNumsofBundle.Size = new System.Drawing.Size(123, 23);
            this.txtNumsofBundle.TabIndex = 10;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnClose.Location = new System.Drawing.Point(904, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(67, 30);
            this.btnClose.TabIndex = 9;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreate.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCreate.Location = new System.Drawing.Point(824, 6);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(74, 30);
            this.btnCreate.TabIndex = 8;
            this.btnCreate.Text = "Create";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Nums of Byndle :";
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(0, 40);
            this.grid1.Name = "grid1";
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.Size = new System.Drawing.Size(983, 438);
            this.grid1.TabIndex = 2;
            this.grid1.TabStop = false;
            // 
            // P23_ImportBarcode
            // 
            this.ClientSize = new System.Drawing.Size(983, 520);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "P23_ImportBarcode";
            this.Text = "ImportBarcode (Farm Out)";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Win.UI.Grid grid1;
        private Win.UI.Label labelSubprocess;
        private Win.UI.Label label1;
        private Win.UI.ComboBox comboSubprocess;
        private Win.UI.Button btnImportfromscanner;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnCreate;
        private Win.UI.TextBox txtNumsofBundle;
        private Win.UI.ComboBox comboTo;
        private Win.UI.Label label2;
        private Win.UI.ListControlBindingSource listControlBindingSource1;

    }
}
