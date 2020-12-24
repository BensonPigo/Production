namespace Sci.Production.Shipping
{
    partial class P62_Import
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
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.txtSP2 = new Sci.Win.UI.TextBox();
            this.label3 = new Sci.Win.UI.Label();
            this.txtSP1 = new Sci.Win.UI.TextBox();
            this.labSP = new Sci.Win.UI.Label();
            this.txtInvNo2 = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtInvNo1 = new Sci.Win.UI.TextBox();
            this.labInvno = new Sci.Win.UI.Label();
            this.dateETD = new Sci.Win.UI.DateRange();
            this.labETD = new Sci.Win.UI.Label();
            this.txtPo2 = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.txtPo1 = new Sci.Win.UI.TextBox();
            this.labPo = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.grid1 = new Sci.Win.UI.Grid();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 457);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(973, 53);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(877, 15);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(781, 16);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 30);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSP2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtSP1);
            this.groupBox1.Controls.Add(this.labSP);
            this.groupBox1.Controls.Add(this.txtInvNo2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtInvNo1);
            this.groupBox1.Controls.Add(this.labInvno);
            this.groupBox1.Controls.Add(this.dateETD);
            this.groupBox1.Controls.Add(this.labETD);
            this.groupBox1.Controls.Add(this.txtPo2);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtPo1);
            this.groupBox1.Controls.Add(this.labPo);
            this.groupBox1.Controls.Add(this.btnQuery);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(973, 100);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            // 
            // txtSP2
            // 
            this.txtSP2.BackColor = System.Drawing.Color.White;
            this.txtSP2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP2.Location = new System.Drawing.Point(296, 51);
            this.txtSP2.MaxLength = 13;
            this.txtSP2.Name = "txtSP2";
            this.txtSP2.Size = new System.Drawing.Size(160, 23);
            this.txtSP2.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(272, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 23);
            this.label3.TabIndex = 12;
            this.label3.Text = "～";
            this.label3.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label3.TextStyle.Color = System.Drawing.Color.Black;
            this.label3.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label3.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtSP1
            // 
            this.txtSP1.BackColor = System.Drawing.Color.White;
            this.txtSP1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP1.Location = new System.Drawing.Point(109, 51);
            this.txtSP1.MaxLength = 13;
            this.txtSP1.Name = "txtSP1";
            this.txtSP1.Size = new System.Drawing.Size(160, 23);
            this.txtSP1.TabIndex = 4;
            // 
            // labSP
            // 
            this.labSP.Location = new System.Drawing.Point(25, 51);
            this.labSP.Name = "labSP";
            this.labSP.Size = new System.Drawing.Size(80, 23);
            this.labSP.TabIndex = 10;
            this.labSP.Text = "SP#";
            // 
            // txtInvNo2
            // 
            this.txtInvNo2.BackColor = System.Drawing.Color.White;
            this.txtInvNo2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInvNo2.Location = new System.Drawing.Point(296, 22);
            this.txtInvNo2.MaxLength = 25;
            this.txtInvNo2.Name = "txtInvNo2";
            this.txtInvNo2.Size = new System.Drawing.Size(160, 23);
            this.txtInvNo2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(272, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 23);
            this.label1.TabIndex = 11;
            this.label1.Text = "～";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            this.label1.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label1.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtInvNo1
            // 
            this.txtInvNo1.BackColor = System.Drawing.Color.White;
            this.txtInvNo1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInvNo1.Location = new System.Drawing.Point(109, 22);
            this.txtInvNo1.MaxLength = 25;
            this.txtInvNo1.Name = "txtInvNo1";
            this.txtInvNo1.Size = new System.Drawing.Size(160, 23);
            this.txtInvNo1.TabIndex = 0;
            // 
            // labInvno
            // 
            this.labInvno.Location = new System.Drawing.Point(25, 22);
            this.labInvno.Name = "labInvno";
            this.labInvno.Size = new System.Drawing.Size(80, 23);
            this.labInvno.TabIndex = 9;
            this.labInvno.Text = "Invoice No.";
            // 
            // dateETD
            // 
            // 
            // 
            // 
            this.dateETD.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateETD.DateBox1.Name = "";
            this.dateETD.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateETD.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateETD.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateETD.DateBox2.Name = "";
            this.dateETD.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateETD.DateBox2.TabIndex = 1;
            this.dateETD.IsRequired = false;
            this.dateETD.Location = new System.Drawing.Point(531, 22);
            this.dateETD.Name = "dateETD";
            this.dateETD.Size = new System.Drawing.Size(280, 23);
            this.dateETD.TabIndex = 3;
            // 
            // labETD
            // 
            this.labETD.Location = new System.Drawing.Point(477, 22);
            this.labETD.Name = "labETD";
            this.labETD.Size = new System.Drawing.Size(51, 23);
            this.labETD.TabIndex = 13;
            this.labETD.Text = "ETD";
            // 
            // txtPo2
            // 
            this.txtPo2.BackColor = System.Drawing.Color.White;
            this.txtPo2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPo2.Location = new System.Drawing.Point(683, 51);
            this.txtPo2.MaxLength = 13;
            this.txtPo2.Name = "txtPo2";
            this.txtPo2.Size = new System.Drawing.Size(130, 23);
            this.txtPo2.TabIndex = 7;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(661, 51);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 23);
            this.label9.TabIndex = 0;
            this.label9.Text = "～";
            this.label9.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            this.label9.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtPo1
            // 
            this.txtPo1.BackColor = System.Drawing.Color.White;
            this.txtPo1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPo1.Location = new System.Drawing.Point(532, 51);
            this.txtPo1.MaxLength = 13;
            this.txtPo1.Name = "txtPo1";
            this.txtPo1.Size = new System.Drawing.Size(130, 23);
            this.txtPo1.TabIndex = 6;
            // 
            // labPo
            // 
            this.labPo.Location = new System.Drawing.Point(477, 51);
            this.labPo.Name = "labPo";
            this.labPo.Size = new System.Drawing.Size(51, 23);
            this.labPo.TabIndex = 14;
            this.labPo.Text = "PO";
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnQuery.Location = new System.Drawing.Point(856, 22);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(90, 30);
            this.btnQuery.TabIndex = 8;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(0, 100);
            this.grid1.Name = "grid1";
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(973, 357);
            this.grid1.TabIndex = 0;
            this.grid1.TabStop = false;
            // 
            // P62_Import
            // 
            this.ClientSize = new System.Drawing.Size(973, 510);
            this.Controls.Add(this.grid1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.DefaultControl = "txtInvNo1";
            this.Name = "P62_Import";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P62. Batch Import";
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.GroupBox groupBox2;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnImport;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Button btnQuery;
        private Win.UI.Label labPo;
        private Win.UI.TextBox txtPo1;
        private Win.UI.Label labETD;
        private Win.UI.TextBox txtPo2;
        private Win.UI.Label label9;
        private Win.UI.DateRange dateETD;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private Win.UI.Grid grid1;
        private Win.UI.TextBox txtSP2;
        private Win.UI.Label label3;
        private Win.UI.TextBox txtSP1;
        private Win.UI.Label labSP;
        private Win.UI.TextBox txtInvNo2;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtInvNo1;
        private Win.UI.Label labInvno;
    }
}
