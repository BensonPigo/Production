namespace Sci.Production.Warehouse
{
    partial class P67_Import
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
            this.label1 = new Sci.Win.UI.Label();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.labSeq = new Sci.Win.UI.Label();
            this.txtSeq = new Sci.Win.UI.TextBox();
            this.txtLocation = new Sci.Win.UI.TextBox();
            this.label3 = new Sci.Win.UI.Label();
            this.btnFind = new Sci.Win.UI.Button();
            this.gridImport = new Sci.Win.UI.Grid();
            this.btnImport = new Sci.Win.UI.Button();
            this.btnCancel = new Sci.Win.UI.Button();
            this.txtColor = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.txtRoll = new Sci.Win.UI.TextBox();
            this.label4 = new Sci.Win.UI.Label();
            this.txtDyelot = new Sci.Win.UI.TextBox();
            this.label5 = new Sci.Win.UI.Label();
            this.chkBalance = new Sci.Win.UI.CheckBox();
            this.label6 = new Sci.Win.UI.Label();
            this.txtUpadteLocation = new Sci.Win.UI.TextBox();
            this.btnUpdateAllLocation = new Sci.Win.UI.Button();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "SP#";
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(90, 12);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(110, 23);
            this.txtSP.TabIndex = 2;
            // 
            // labSeq
            // 
            this.labSeq.Location = new System.Drawing.Point(203, 12);
            this.labSeq.Name = "labSeq";
            this.labSeq.Size = new System.Drawing.Size(75, 23);
            this.labSeq.TabIndex = 3;
            this.labSeq.Text = "Seq";
            // 
            // txtSeq
            // 
            this.txtSeq.BackColor = System.Drawing.Color.White;
            this.txtSeq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeq.Location = new System.Drawing.Point(281, 12);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.Size = new System.Drawing.Size(110, 23);
            this.txtSeq.TabIndex = 4;
            // 
            // txtLocation
            // 
            this.txtLocation.BackColor = System.Drawing.Color.White;
            this.txtLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocation.Location = new System.Drawing.Point(90, 41);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(110, 23);
            this.txtLocation.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Location";
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.Location = new System.Drawing.Point(901, 12);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(95, 30);
            this.btnFind.TabIndex = 7;
            this.btnFind.Text = "Query";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // gridImport
            // 
            this.gridImport.AllowUserToAddRows = false;
            this.gridImport.AllowUserToDeleteRows = false;
            this.gridImport.AllowUserToResizeRows = false;
            this.gridImport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridImport.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridImport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.gridImport.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridImport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridImport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridImport.Location = new System.Drawing.Point(9, 71);
            this.gridImport.Name = "gridImport";
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridImport.RowTemplate.Height = 25;
            this.gridImport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridImport.ShowCellToolTips = false;
            this.gridImport.Size = new System.Drawing.Size(987, 512);
            this.gridImport.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridImport.TabIndex = 8;
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(830, 589);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(80, 30);
            this.btnImport.TabIndex = 9;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(916, 589);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // txtColor
            // 
            this.txtColor.BackColor = System.Drawing.Color.White;
            this.txtColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtColor.Location = new System.Drawing.Point(444, 12);
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(85, 23);
            this.txtColor.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(394, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 23);
            this.label2.TabIndex = 11;
            this.label2.Text = "Color";
            // 
            // txtRoll
            // 
            this.txtRoll.BackColor = System.Drawing.Color.White;
            this.txtRoll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRoll.Location = new System.Drawing.Point(582, 12);
            this.txtRoll.Name = "txtRoll";
            this.txtRoll.Size = new System.Drawing.Size(85, 23);
            this.txtRoll.TabIndex = 14;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(532, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 23);
            this.label4.TabIndex = 13;
            this.label4.Text = "Roll";
            // 
            // txtDyelot
            // 
            this.txtDyelot.BackColor = System.Drawing.Color.White;
            this.txtDyelot.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDyelot.Location = new System.Drawing.Point(720, 12);
            this.txtDyelot.Name = "txtDyelot";
            this.txtDyelot.Size = new System.Drawing.Size(57, 23);
            this.txtDyelot.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(670, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 23);
            this.label5.TabIndex = 15;
            this.label5.Text = "Dyelot";
            // 
            // chkBalance
            // 
            this.chkBalance.AutoSize = true;
            this.chkBalance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkBalance.Location = new System.Drawing.Point(12, 595);
            this.chkBalance.Name = "chkBalance";
            this.chkBalance.Size = new System.Drawing.Size(128, 21);
            this.chkBalance.TabIndex = 17;
            this.chkBalance.Text = "Balance Qty > 0";
            this.chkBalance.UseVisualStyleBackColor = true;
            this.chkBalance.CheckedChanged += new System.EventHandler(this.ChkBalance_CheckedChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(143, 593);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 23);
            this.label6.TabIndex = 18;
            this.label6.Text = "Location";
            // 
            // txtUpadteLocation
            // 
            this.txtUpadteLocation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtUpadteLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtUpadteLocation.IsSupportEditMode = false;
            this.txtUpadteLocation.Location = new System.Drawing.Point(221, 593);
            this.txtUpadteLocation.Name = "txtUpadteLocation";
            this.txtUpadteLocation.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtUpadteLocation.ReadOnly = true;
            this.txtUpadteLocation.Size = new System.Drawing.Size(358, 23);
            this.txtUpadteLocation.TabIndex = 19;
            this.txtUpadteLocation.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtUpadteLocation_PopUp);
            this.txtUpadteLocation.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TxtUpadteLocation_MouseDoubleClick);
            // 
            // btnUpdateAllLocation
            // 
            this.btnUpdateAllLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateAllLocation.Location = new System.Drawing.Point(605, 589);
            this.btnUpdateAllLocation.Name = "btnUpdateAllLocation";
            this.btnUpdateAllLocation.Size = new System.Drawing.Size(172, 30);
            this.btnUpdateAllLocation.TabIndex = 20;
            this.btnUpdateAllLocation.Text = "Update All Location";
            this.btnUpdateAllLocation.UseVisualStyleBackColor = true;
            this.btnUpdateAllLocation.Click += new System.EventHandler(this.BtnUpdateAllLocation_Click);
            // 
            // P67_Import
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 627);
            this.Controls.Add(this.btnUpdateAllLocation);
            this.Controls.Add(this.txtUpadteLocation);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chkBalance);
            this.Controls.Add(this.txtDyelot);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtRoll);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtColor);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.gridImport);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSeq);
            this.Controls.Add(this.labSeq);
            this.Controls.Add(this.txtSP);
            this.Controls.Add(this.label1);
            this.Name = "P67_Import";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P65 Import";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtSP, 0);
            this.Controls.SetChildIndex(this.labSeq, 0);
            this.Controls.SetChildIndex(this.txtSeq, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtLocation, 0);
            this.Controls.SetChildIndex(this.btnFind, 0);
            this.Controls.SetChildIndex(this.gridImport, 0);
            this.Controls.SetChildIndex(this.btnImport, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtColor, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.txtRoll, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.txtDyelot, 0);
            this.Controls.SetChildIndex(this.chkBalance, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txtUpadteLocation, 0);
            this.Controls.SetChildIndex(this.btnUpdateAllLocation, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.TextBox txtSP;
        private Win.UI.Label labSeq;
        private Win.UI.TextBox txtSeq;
        private Win.UI.TextBox txtLocation;
        private Win.UI.Label label3;
        private Win.UI.Button btnFind;
        private Win.UI.Grid gridImport;
        private Win.UI.Button btnImport;
        private Win.UI.Button btnCancel;
        private Win.UI.TextBox txtColor;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtRoll;
        private Win.UI.Label label4;
        private Win.UI.TextBox txtDyelot;
        private Win.UI.Label label5;
        private Win.UI.CheckBox chkBalance;
        private Win.UI.Label label6;
        private Win.UI.TextBox txtUpadteLocation;
        private Win.UI.Button btnUpdateAllLocation;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}