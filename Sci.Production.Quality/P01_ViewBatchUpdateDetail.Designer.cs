namespace Sci.Production.Quality
{
    partial class P01_ViewBatchUpdateDetail
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
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.tabControl1 = new Sci.Win.UI.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chkPasteShadebandTime = new Sci.Win.UI.CheckBox();
            this.comboBoxReceivingID = new Sci.Win.UI.ComboBox();
            this.label4 = new Sci.Win.UI.Label();
            this.comboBoxWKNo = new Sci.Win.UI.ComboBox();
            this.label3 = new Sci.Win.UI.Label();
            this.comboBoxSEQ = new Sci.Win.UI.ComboBox();
            this.label2 = new Sci.Win.UI.Label();
            this.displaySP = new Sci.Win.UI.DisplayBox();
            this.label1 = new Sci.Win.UI.Label();
            this.grid = new Sci.Win.UI.Grid();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.chkCheckByQC = new Sci.Win.UI.CheckBox();
            this.comboBoxReceivingID2 = new Sci.Win.UI.ComboBox();
            this.label5 = new Sci.Win.UI.Label();
            this.comboBoxWKNo2 = new Sci.Win.UI.ComboBox();
            this.label6 = new Sci.Win.UI.Label();
            this.comboBoxSEQ2 = new Sci.Win.UI.ComboBox();
            this.label7 = new Sci.Win.UI.Label();
            this.displaySP2 = new Sci.Win.UI.DisplayBox();
            this.label8 = new Sci.Win.UI.Label();
            this.grid2 = new Sci.Win.UI.Grid();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1340, 562);
            this.tabControl1.TabIndex = 46;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chkPasteShadebandTime);
            this.tabPage1.Controls.Add(this.comboBoxReceivingID);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.comboBoxWKNo);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.comboBoxSEQ);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.displaySP);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.grid);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1332, 500);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Shade Band";
            // 
            // chkPasteShadebandTime
            // 
            this.chkPasteShadebandTime.AutoSize = true;
            this.chkPasteShadebandTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkPasteShadebandTime.Location = new System.Drawing.Point(802, 6);
            this.chkPasteShadebandTime.Name = "chkPasteShadebandTime";
            this.chkPasteShadebandTime.Size = new System.Drawing.Size(175, 21);
            this.chkPasteShadebandTime.TabIndex = 55;
            this.chkPasteShadebandTime.Text = "Paste Shadeband Time";
            this.chkPasteShadebandTime.UseVisualStyleBackColor = true;
            this.chkPasteShadebandTime.CheckedChanged += new System.EventHandler(this.ChkPasteShadebandTime_CheckedChanged);
            // 
            // comboBoxReceivingID
            // 
            this.comboBoxReceivingID.BackColor = System.Drawing.Color.White;
            this.comboBoxReceivingID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxReceivingID.FormattingEnabled = true;
            this.comboBoxReceivingID.IsSupportUnselect = true;
            this.comboBoxReceivingID.Location = new System.Drawing.Point(639, 6);
            this.comboBoxReceivingID.Name = "comboBoxReceivingID";
            this.comboBoxReceivingID.OldText = "";
            this.comboBoxReceivingID.Size = new System.Drawing.Size(157, 24);
            this.comboBoxReceivingID.TabIndex = 54;
            this.comboBoxReceivingID.SelectedIndexChanged += new System.EventHandler(this.ComboBoxReceivingID_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(550, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 23);
            this.label4.TabIndex = 53;
            this.label4.Text = "ReceivingID";
            // 
            // comboBoxWKNo
            // 
            this.comboBoxWKNo.BackColor = System.Drawing.Color.White;
            this.comboBoxWKNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxWKNo.FormattingEnabled = true;
            this.comboBoxWKNo.IsSupportUnselect = true;
            this.comboBoxWKNo.Location = new System.Drawing.Point(390, 6);
            this.comboBoxWKNo.Name = "comboBoxWKNo";
            this.comboBoxWKNo.OldText = "";
            this.comboBoxWKNo.Size = new System.Drawing.Size(157, 24);
            this.comboBoxWKNo.TabIndex = 52;
            this.comboBoxWKNo.SelectedIndexChanged += new System.EventHandler(this.ComboBoxWKNo_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(337, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 23);
            this.label3.TabIndex = 51;
            this.label3.Text = "WKNo";
            // 
            // comboBoxSEQ
            // 
            this.comboBoxSEQ.BackColor = System.Drawing.Color.White;
            this.comboBoxSEQ.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxSEQ.FormattingEnabled = true;
            this.comboBoxSEQ.IsSupportUnselect = true;
            this.comboBoxSEQ.Location = new System.Drawing.Point(239, 6);
            this.comboBoxSEQ.Name = "comboBoxSEQ";
            this.comboBoxSEQ.OldText = "";
            this.comboBoxSEQ.Size = new System.Drawing.Size(95, 24);
            this.comboBoxSEQ.TabIndex = 50;
            this.comboBoxSEQ.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSEQ_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(195, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 23);
            this.label2.TabIndex = 49;
            this.label2.Text = "SEQ";
            // 
            // displaySP
            // 
            this.displaySP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySP.Location = new System.Drawing.Point(49, 6);
            this.displaySP.Name = "displaySP";
            this.displaySP.Size = new System.Drawing.Size(143, 23);
            this.displaySP.TabIndex = 48;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(5, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 23);
            this.label1.TabIndex = 47;
            this.label1.Text = "SP#";
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
            this.grid.DataSource = this.listControlBindingSource1;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(3, 35);
            this.grid.Name = "grid";
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(1326, 462);
            this.grid.TabIndex = 46;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chkCheckByQC);
            this.tabPage2.Controls.Add(this.comboBoxReceivingID2);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.comboBoxWKNo2);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.comboBoxSEQ2);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.displaySP2);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.grid2);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(1332, 533);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Continuity";
            // 
            // chkCheckByQC
            // 
            this.chkCheckByQC.AutoSize = true;
            this.chkCheckByQC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkCheckByQC.Location = new System.Drawing.Point(803, 6);
            this.chkCheckByQC.Name = "chkCheckByQC";
            this.chkCheckByQC.Size = new System.Drawing.Size(110, 21);
            this.chkCheckByQC.TabIndex = 64;
            this.chkCheckByQC.Text = "Check By QC";
            this.chkCheckByQC.UseVisualStyleBackColor = true;
            this.chkCheckByQC.CheckedChanged += new System.EventHandler(this.ChkCheckByQC_CheckedChanged);
            // 
            // comboBoxReceivingID2
            // 
            this.comboBoxReceivingID2.BackColor = System.Drawing.Color.White;
            this.comboBoxReceivingID2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxReceivingID2.FormattingEnabled = true;
            this.comboBoxReceivingID2.IsSupportUnselect = true;
            this.comboBoxReceivingID2.Location = new System.Drawing.Point(640, 6);
            this.comboBoxReceivingID2.Name = "comboBoxReceivingID2";
            this.comboBoxReceivingID2.OldText = "";
            this.comboBoxReceivingID2.Size = new System.Drawing.Size(157, 24);
            this.comboBoxReceivingID2.TabIndex = 63;
            this.comboBoxReceivingID2.SelectedIndexChanged += new System.EventHandler(this.ComboBoxReceivingID2_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(551, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 23);
            this.label5.TabIndex = 62;
            this.label5.Text = "ReceivingID";
            // 
            // comboBoxWKNo2
            // 
            this.comboBoxWKNo2.BackColor = System.Drawing.Color.White;
            this.comboBoxWKNo2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxWKNo2.FormattingEnabled = true;
            this.comboBoxWKNo2.IsSupportUnselect = true;
            this.comboBoxWKNo2.Location = new System.Drawing.Point(391, 6);
            this.comboBoxWKNo2.Name = "comboBoxWKNo2";
            this.comboBoxWKNo2.OldText = "";
            this.comboBoxWKNo2.Size = new System.Drawing.Size(157, 24);
            this.comboBoxWKNo2.TabIndex = 61;
            this.comboBoxWKNo2.SelectedIndexChanged += new System.EventHandler(this.ComboBoxWKNo2_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(338, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 23);
            this.label6.TabIndex = 60;
            this.label6.Text = "WKNo";
            // 
            // comboBoxSEQ2
            // 
            this.comboBoxSEQ2.BackColor = System.Drawing.Color.White;
            this.comboBoxSEQ2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxSEQ2.FormattingEnabled = true;
            this.comboBoxSEQ2.IsSupportUnselect = true;
            this.comboBoxSEQ2.Location = new System.Drawing.Point(240, 6);
            this.comboBoxSEQ2.Name = "comboBoxSEQ2";
            this.comboBoxSEQ2.OldText = "";
            this.comboBoxSEQ2.Size = new System.Drawing.Size(95, 24);
            this.comboBoxSEQ2.TabIndex = 59;
            this.comboBoxSEQ2.SelectedIndexChanged += new System.EventHandler(this.ComboBoxSEQ2_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(196, 6);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 23);
            this.label7.TabIndex = 58;
            this.label7.Text = "SEQ";
            // 
            // displaySP2
            // 
            this.displaySP2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySP2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySP2.Location = new System.Drawing.Point(50, 6);
            this.displaySP2.Name = "displaySP2";
            this.displaySP2.Size = new System.Drawing.Size(143, 23);
            this.displaySP2.TabIndex = 57;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(6, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 23);
            this.label8.TabIndex = 56;
            this.label8.Text = "SP#";
            // 
            // grid2
            // 
            this.grid2.AllowUserToAddRows = false;
            this.grid2.AllowUserToDeleteRows = false;
            this.grid2.AllowUserToResizeRows = false;
            this.grid2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid2.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid2.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid2.DataSource = this.listControlBindingSource2;
            this.grid2.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid2.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid2.Location = new System.Drawing.Point(3, 36);
            this.grid2.Name = "grid2";
            this.grid2.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid2.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid2.RowTemplate.Height = 24;
            this.grid2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid2.ShowCellToolTips = false;
            this.grid2.Size = new System.Drawing.Size(1326, 494);
            this.grid2.TabIndex = 46;
            // 
            // P01_ViewBatchUpdateDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1340, 562);
            this.Controls.Add(this.tabControl1);
            this.EditMode = true;
            this.Name = "P01_ViewBatchUpdateDetail";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P01_View Batch Update Detail  ";
            this.Controls.SetChildIndex(this.tabControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private Win.UI.Grid grid;
        private System.Windows.Forms.TabPage tabPage2;
        private Win.UI.Grid grid2;
        private Win.UI.Label label1;
        private Win.UI.Label label3;
        private Win.UI.ComboBox comboBoxSEQ;
        private Win.UI.Label label2;
        private Win.UI.DisplayBox displaySP;
        private Win.UI.ComboBox comboBoxReceivingID;
        private Win.UI.Label label4;
        private Win.UI.ComboBox comboBoxWKNo;
        private Win.UI.CheckBox chkPasteShadebandTime;
        private Win.UI.CheckBox chkCheckByQC;
        private Win.UI.ComboBox comboBoxReceivingID2;
        private Win.UI.Label label5;
        private Win.UI.ComboBox comboBoxWKNo2;
        private Win.UI.Label label6;
        private Win.UI.ComboBox comboBoxSEQ2;
        private Win.UI.Label label7;
        private Win.UI.DisplayBox displaySP2;
        private Win.UI.Label label8;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
    }
}