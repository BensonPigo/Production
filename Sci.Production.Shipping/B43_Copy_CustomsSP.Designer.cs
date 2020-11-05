namespace Sci.Production.Shipping
{
    partial class B43_Copy_CustomsSP
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnCopyCustomsSP = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.gridSubconFrom = new Sci.Win.UI.Grid();
            this.gridSubconIn = new Sci.Win.UI.Grid();
            this.panel3 = new Sci.Win.UI.Panel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btnFind = new Sci.Win.UI.Button();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.labBrand = new Sci.Win.UI.Label();
            this.txtStyle = new Sci.Production.Class.Txtstyle();
            this.labStyle = new Sci.Win.UI.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtSubconFromContract = new Sci.Win.UI.TextBox();
            this.txtSubconFromFty = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.labSubconFromFty = new Sci.Win.UI.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtSubconInContractNo = new Sci.Win.UI.TextBox();
            this.txtSubconInFty = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.SubConFromDataBinding = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.SubConToDataBinding = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridSubconFrom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSubconIn)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SubConFromDataBinding)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SubConToDataBinding)).BeginInit();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnCopyCustomsSP);
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 492);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(991, 47);
            this.panel4.TabIndex = 5;
            // 
            // btnCopyCustomsSP
            // 
            this.btnCopyCustomsSP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyCustomsSP.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnCopyCustomsSP.Location = new System.Drawing.Point(761, 9);
            this.btnCopyCustomsSP.Name = "btnCopyCustomsSP";
            this.btnCopyCustomsSP.Size = new System.Drawing.Size(132, 30);
            this.btnCopyCustomsSP.TabIndex = 95;
            this.btnCopyCustomsSP.Text = "Copy Customs SP";
            this.btnCopyCustomsSP.UseVisualStyleBackColor = true;
            this.btnCopyCustomsSP.Click += new System.EventHandler(this.BtnCopyCustomsSP_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(899, 9);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 101);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.gridSubconFrom);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.gridSubconIn);
            this.splitContainer1.Size = new System.Drawing.Size(991, 391);
            this.splitContainer1.SplitterDistance = 485;
            this.splitContainer1.TabIndex = 8;
            // 
            // gridSubconFrom
            // 
            this.gridSubconFrom.AllowUserToAddRows = false;
            this.gridSubconFrom.AllowUserToDeleteRows = false;
            this.gridSubconFrom.AllowUserToResizeRows = false;
            this.gridSubconFrom.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSubconFrom.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSubconFrom.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSubconFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSubconFrom.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSubconFrom.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSubconFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSubconFrom.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSubconFrom.Location = new System.Drawing.Point(0, 0);
            this.gridSubconFrom.Name = "gridSubconFrom";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridSubconFrom.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridSubconFrom.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSubconFrom.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSubconFrom.RowTemplate.Height = 24;
            this.gridSubconFrom.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSubconFrom.ShowCellToolTips = false;
            this.gridSubconFrom.Size = new System.Drawing.Size(485, 391);
            this.gridSubconFrom.TabIndex = 23;
            this.gridSubconFrom.TabStop = false;
            // 
            // gridSubconIn
            // 
            this.gridSubconIn.AllowUserToAddRows = false;
            this.gridSubconIn.AllowUserToDeleteRows = false;
            this.gridSubconIn.AllowUserToResizeRows = false;
            this.gridSubconIn.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridSubconIn.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridSubconIn.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridSubconIn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridSubconIn.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridSubconIn.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridSubconIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridSubconIn.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridSubconIn.Location = new System.Drawing.Point(0, 0);
            this.gridSubconIn.Name = "gridSubconIn";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridSubconIn.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.gridSubconIn.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridSubconIn.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridSubconIn.RowTemplate.Height = 24;
            this.gridSubconIn.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridSubconIn.ShowCellToolTips = false;
            this.gridSubconIn.Size = new System.Drawing.Size(502, 391);
            this.gridSubconIn.TabIndex = 21;
            this.gridSubconIn.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.splitContainer2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(991, 101);
            this.panel3.TabIndex = 7;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.btnFind);
            this.splitContainer2.Panel1.Controls.Add(this.txtbrand);
            this.splitContainer2.Panel1.Controls.Add(this.labBrand);
            this.splitContainer2.Panel1.Controls.Add(this.txtStyle);
            this.splitContainer2.Panel1.Controls.Add(this.labStyle);
            this.splitContainer2.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer2.Size = new System.Drawing.Size(991, 101);
            this.splitContainer2.SplitterDistance = 485;
            this.splitContainer2.TabIndex = 0;
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnFind.Location = new System.Drawing.Point(401, 61);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(75, 30);
            this.btnFind.TabIndex = 98;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtbrand.IsSupportEditMode = false;
            this.txtbrand.Location = new System.Drawing.Point(307, 65);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtbrand.ReadOnly = true;
            this.txtbrand.Size = new System.Drawing.Size(76, 23);
            this.txtbrand.TabIndex = 97;
            // 
            // labBrand
            // 
            this.labBrand.Location = new System.Drawing.Point(239, 65);
            this.labBrand.Name = "labBrand";
            this.labBrand.Size = new System.Drawing.Size(65, 23);
            this.labBrand.TabIndex = 96;
            this.labBrand.Text = "Brand";
            // 
            // txtStyle
            // 
            this.txtStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtStyle.BrandObjectName = null;
            this.txtStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtStyle.IsSupportEditMode = false;
            this.txtStyle.Location = new System.Drawing.Point(86, 65);
            this.txtStyle.Name = "txtStyle";
            this.txtStyle.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtStyle.ReadOnly = true;
            this.txtStyle.Size = new System.Drawing.Size(150, 23);
            this.txtStyle.TabIndex = 95;
            this.txtStyle.TarBrand = null;
            this.txtStyle.TarSeason = null;
            // 
            // labStyle
            // 
            this.labStyle.Location = new System.Drawing.Point(9, 65);
            this.labStyle.Name = "labStyle";
            this.labStyle.Size = new System.Drawing.Size(74, 23);
            this.labStyle.TabIndex = 94;
            this.labStyle.Text = "Style";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSubconFromContract);
            this.groupBox1.Controls.Add(this.txtSubconFromFty);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.labSubconFromFty);
            this.groupBox1.Location = new System.Drawing.Point(5, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(475, 56);
            this.groupBox1.TabIndex = 93;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Subcon From";
            // 
            // txtSubconFromContract
            // 
            this.txtSubconFromContract.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSubconFromContract.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSubconFromContract.IsSupportEditMode = false;
            this.txtSubconFromContract.Location = new System.Drawing.Point(273, 21);
            this.txtSubconFromContract.Name = "txtSubconFromContract";
            this.txtSubconFromContract.ReadOnly = true;
            this.txtSubconFromContract.Size = new System.Drawing.Size(196, 23);
            this.txtSubconFromContract.TabIndex = 13;
            // 
            // txtSubconFromFty
            // 
            this.txtSubconFromFty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSubconFromFty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSubconFromFty.IsSupportEditMode = false;
            this.txtSubconFromFty.Location = new System.Drawing.Point(81, 21);
            this.txtSubconFromFty.Name = "txtSubconFromFty";
            this.txtSubconFromFty.ReadOnly = true;
            this.txtSubconFromFty.Size = new System.Drawing.Size(92, 23);
            this.txtSubconFromFty.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(182, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 23);
            this.label2.TabIndex = 11;
            this.label2.Text = "Contract No.";
            // 
            // labSubconFromFty
            // 
            this.labSubconFromFty.Location = new System.Drawing.Point(4, 21);
            this.labSubconFromFty.Name = "labSubconFromFty";
            this.labSubconFromFty.Size = new System.Drawing.Size(74, 23);
            this.labSubconFromFty.TabIndex = 10;
            this.labSubconFromFty.Text = "Factory";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtSubconInContractNo);
            this.groupBox2.Controls.Add(this.txtSubconInFty);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(6, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(485, 56);
            this.groupBox2.TabIndex = 94;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Subcon In";
            // 
            // txtSubconInContractNo
            // 
            this.txtSubconInContractNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSubconInContractNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSubconInContractNo.IsSupportEditMode = false;
            this.txtSubconInContractNo.Location = new System.Drawing.Point(273, 21);
            this.txtSubconInContractNo.Name = "txtSubconInContractNo";
            this.txtSubconInContractNo.ReadOnly = true;
            this.txtSubconInContractNo.Size = new System.Drawing.Size(196, 23);
            this.txtSubconInContractNo.TabIndex = 13;
            // 
            // txtSubconInFty
            // 
            this.txtSubconInFty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSubconInFty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSubconInFty.IsSupportEditMode = false;
            this.txtSubconInFty.Location = new System.Drawing.Point(81, 21);
            this.txtSubconInFty.Name = "txtSubconInFty";
            this.txtSubconInFty.ReadOnly = true;
            this.txtSubconInFty.Size = new System.Drawing.Size(92, 23);
            this.txtSubconInFty.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(182, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 23);
            this.label1.TabIndex = 11;
            this.label1.Text = "Contract No.";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(4, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 23);
            this.label3.TabIndex = 10;
            this.label3.Text = "Factory";
            // 
            // B43_Copy_CustomsSP
            // 
            this.ClientSize = new System.Drawing.Size(991, 539);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Name = "B43_Copy_CustomsSP";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Copy Customs SP for subcon order";
            this.panel4.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridSubconFrom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridSubconIn)).EndInit();
            this.panel3.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SubConFromDataBinding)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SubConToDataBinding)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel4;
        private Win.UI.Button btnClose;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private Win.UI.Grid gridSubconFrom;
        private Win.UI.Grid gridSubconIn;
        private Win.UI.Button btnCopyCustomsSP;
        private Win.UI.Panel panel3;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private Win.UI.Label labStyle;
        private System.Windows.Forms.GroupBox groupBox1;
        private Win.UI.TextBox txtSubconFromContract;
        private Win.UI.TextBox txtSubconFromFty;
        private Win.UI.Label label2;
        private Win.UI.Label labSubconFromFty;
        private Class.Txtstyle txtStyle;
        private Win.UI.Button btnFind;
        private Class.Txtbrand txtbrand;
        private Win.UI.Label labBrand;
        private System.Windows.Forms.GroupBox groupBox2;
        private Win.UI.TextBox txtSubconInContractNo;
        private Win.UI.TextBox txtSubconInFty;
        private Win.UI.Label label1;
        private Win.UI.Label label3;
        private Win.UI.ListControlBindingSource SubConFromDataBinding;
        private Win.UI.ListControlBindingSource SubConToDataBinding;
    }
}
