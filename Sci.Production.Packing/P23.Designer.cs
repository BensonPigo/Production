namespace Sci.Production.Packing
{
    partial class P23
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new Sci.Win.UI.Panel();
            this.comboReason = new Sci.Win.UI.ComboBox();
            this.btnUpdateAllReason = new Sci.Win.UI.Button();
            this.label3 = new Sci.Win.UI.Label();
            this.btnFind = new Sci.Win.UI.Button();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.txtPackID = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.txtPoNo = new Sci.Win.UI.TextBox();
            this.dateRangeSCIDelivery = new Sci.Win.UI.DateRange();
            this.label1 = new Sci.Win.UI.Label();
            this.btnSave = new Sci.Win.UI.Button();
            this.btnColse = new Sci.Win.UI.Button();
            this.btnPrint = new Sci.Win.UI.Button();
            this.grid = new Sci.Win.UI.Grid();
            this.listControlBindingSource = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.Border3DStyle.Bump;
            this.panel1.Controls.Add(this.comboReason);
            this.panel1.Controls.Add(this.btnUpdateAllReason);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.btnFind);
            this.panel1.Controls.Add(this.labelSCIDelivery);
            this.panel1.Controls.Add(this.txtPackID);
            this.panel1.Controls.Add(this.labelSPNo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtSPNo);
            this.panel1.Controls.Add(this.txtPoNo);
            this.panel1.Controls.Add(this.dateRangeSCIDelivery);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.RectStyle.BorderWidths.Bottom = 1F;
            this.panel1.RectStyle.BorderWidths.Left = 1F;
            this.panel1.RectStyle.BorderWidths.Right = 1F;
            this.panel1.RectStyle.BorderWidths.Top = 1F;
            this.panel1.Size = new System.Drawing.Size(725, 106);
            this.panel1.TabIndex = 1;
            // 
            // comboReason
            // 
            this.comboReason.BackColor = System.Drawing.Color.White;
            this.comboReason.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboReason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboReason.FormattingEnabled = true;
            this.comboReason.IsSupportUnselect = true;
            this.comboReason.Location = new System.Drawing.Point(101, 75);
            this.comboReason.Name = "comboReason";
            this.comboReason.OldText = "";
            this.comboReason.Size = new System.Drawing.Size(257, 24);
            this.comboReason.TabIndex = 14;
            // 
            // btnUpdateAllReason
            // 
            this.btnUpdateAllReason.Location = new System.Drawing.Point(364, 73);
            this.btnUpdateAllReason.Name = "btnUpdateAllReason";
            this.btnUpdateAllReason.Size = new System.Drawing.Size(156, 30);
            this.btnUpdateAllReason.TabIndex = 13;
            this.btnUpdateAllReason.Text = "Update All Reason";
            this.btnUpdateAllReason.UseVisualStyleBackColor = true;
            this.btnUpdateAllReason.Click += new System.EventHandler(this.BtnUpdateAllReason_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(16, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 23);
            this.label3.TabIndex = 11;
            this.label3.Text = "Reason";
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(622, 10);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 30);
            this.btnFind.TabIndex = 4;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(16, 46);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(82, 23);
            this.labelSCIDelivery.TabIndex = 10;
            this.labelSCIDelivery.Text = "SCI Delivery";
            // 
            // txtPackID
            // 
            this.txtPackID.BackColor = System.Drawing.Color.White;
            this.txtPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPackID.IsSupportEditMode = false;
            this.txtPackID.Location = new System.Drawing.Point(502, 14);
            this.txtPackID.Name = "txtPackID";
            this.txtPackID.Size = new System.Drawing.Size(114, 23);
            this.txtPackID.TabIndex = 2;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(16, 14);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(82, 23);
            this.labelSPNo.TabIndex = 7;
            this.labelSPNo.Text = "SP#";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(439, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 23);
            this.label2.TabIndex = 9;
            this.label2.Text = "Pack ID";
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.IsSupportEditMode = false;
            this.txtSPNo.Location = new System.Drawing.Point(101, 14);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(114, 23);
            this.txtSPNo.TabIndex = 0;
            // 
            // txtPoNo
            // 
            this.txtPoNo.BackColor = System.Drawing.Color.White;
            this.txtPoNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPoNo.IsSupportEditMode = false;
            this.txtPoNo.Location = new System.Drawing.Point(315, 14);
            this.txtPoNo.Name = "txtPoNo";
            this.txtPoNo.Size = new System.Drawing.Size(114, 23);
            this.txtPoNo.TabIndex = 1;
            // 
            // dateRangeSCIDelivery
            // 
            // 
            // 
            // 
            this.dateRangeSCIDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeSCIDelivery.DateBox1.Name = "";
            this.dateRangeSCIDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeSCIDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeSCIDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeSCIDelivery.DateBox2.Name = "";
            this.dateRangeSCIDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeSCIDelivery.DateBox2.TabIndex = 1;
            this.dateRangeSCIDelivery.IsSupportEditMode = false;
            this.dateRangeSCIDelivery.Location = new System.Drawing.Point(101, 46);
            this.dateRangeSCIDelivery.Name = "dateRangeSCIDelivery";
            this.dateRangeSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateRangeSCIDelivery.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(252, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "PO#";
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Location = new System.Drawing.Point(548, 124);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnColse
            // 
            this.btnColse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnColse.Location = new System.Drawing.Point(634, 124);
            this.btnColse.Name = "btnColse";
            this.btnColse.Size = new System.Drawing.Size(80, 30);
            this.btnColse.TabIndex = 6;
            this.btnColse.Text = "Close";
            this.btnColse.UseVisualStyleBackColor = true;
            this.btnColse.Click += new System.EventHandler(this.BtnColse_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrint.Location = new System.Drawing.Point(462, 124);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(80, 30);
            this.btnPrint.TabIndex = 7;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.DataSource = this.listControlBindingSource;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(7, 160);
            this.grid.Name = "grid";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grid.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(725, 465);
            this.grid.TabIndex = 8;
            this.grid.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.Grid_ColumnHeaderMouseClick);
            // 
            // P23
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 637);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnColse);
            this.Name = "P23";
            this.Text = "P23. Factory Select Carton For Return";
            this.Controls.SetChildIndex(this.btnColse, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.btnPrint, 0);
            this.Controls.SetChildIndex(this.grid, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Button btnFind;
        private Win.UI.Button btnSave;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Button btnColse;
        private Win.UI.TextBox txtPackID;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.TextBox txtPoNo;
        private Win.UI.DateRange dateRangeSCIDelivery;
        private Win.UI.Label label1;
        private Win.UI.Button btnPrint;
        private Win.UI.Grid grid;
        private Win.UI.ListControlBindingSource listControlBindingSource;
        private Win.UI.Button btnUpdateAllReason;
        private Win.UI.Label label3;
        private Win.UI.ComboBox comboReason;
    }
}