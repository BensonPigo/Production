namespace Sci.Production.Quality
{
    partial class P01_PhysicalInspection_Defect
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridFabricInspection = new Sci.Win.UI.Grid();
            this.labelRoll = new Sci.Win.UI.Label();
            this.labelDyelot = new Sci.Win.UI.Label();
            this.labelSP = new Sci.Win.UI.Label();
            this.labelSEQ1 = new Sci.Win.UI.Label();
            this.labelActYdsInspected = new Sci.Win.UI.Label();
            this.displayRoll = new Sci.Win.UI.DisplayBox();
            this.displayDyelot = new Sci.Win.UI.DisplayBox();
            this.displaySP = new Sci.Win.UI.DisplayBox();
            this.displaySEQ1 = new Sci.Win.UI.DisplayBox();
            this.labelSEQ2 = new Sci.Win.UI.Label();
            this.displaySEQ2 = new Sci.Win.UI.DisplayBox();
            this.displayActYdsInspected = new Sci.Win.UI.DisplayBox();
            this.btnDefectPic = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridFabricInspection)).BeginInit();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 433);
            this.btmcont.Size = new System.Drawing.Size(715, 40);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(625, 5);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(545, 5);
            // 
            // left
            // 
            this.left.Visible = false;
            // 
            // right
            // 
            this.right.Visible = false;
            // 
            // gridFabricInspection
            // 
            this.gridFabricInspection.AllowUserToAddRows = false;
            this.gridFabricInspection.AllowUserToDeleteRows = false;
            this.gridFabricInspection.AllowUserToResizeRows = false;
            this.gridFabricInspection.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridFabricInspection.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridFabricInspection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridFabricInspection.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridFabricInspection.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridFabricInspection.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridFabricInspection.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridFabricInspection.Location = new System.Drawing.Point(10, 96);
            this.gridFabricInspection.Name = "gridFabricInspection";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridFabricInspection.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridFabricInspection.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridFabricInspection.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridFabricInspection.RowTemplate.Height = 24;
            this.gridFabricInspection.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridFabricInspection.ShowCellToolTips = false;
            this.gridFabricInspection.Size = new System.Drawing.Size(695, 323);
            this.gridFabricInspection.TabIndex = 95;
            this.gridFabricInspection.TabStop = false;
            // 
            // labelRoll
            // 
            this.labelRoll.Location = new System.Drawing.Point(9, 19);
            this.labelRoll.Name = "labelRoll";
            this.labelRoll.Size = new System.Drawing.Size(75, 23);
            this.labelRoll.TabIndex = 96;
            this.labelRoll.Text = "Roll#";
            // 
            // labelDyelot
            // 
            this.labelDyelot.Location = new System.Drawing.Point(9, 55);
            this.labelDyelot.Name = "labelDyelot";
            this.labelDyelot.Size = new System.Drawing.Size(75, 23);
            this.labelDyelot.TabIndex = 97;
            this.labelDyelot.Text = "Dyelot";
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(205, 19);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(75, 23);
            this.labelSP.TabIndex = 98;
            this.labelSP.Text = "SP#";
            // 
            // labelSEQ1
            // 
            this.labelSEQ1.Location = new System.Drawing.Point(205, 55);
            this.labelSEQ1.Name = "labelSEQ1";
            this.labelSEQ1.Size = new System.Drawing.Size(49, 23);
            this.labelSEQ1.TabIndex = 99;
            this.labelSEQ1.Text = "SEQ1";
            // 
            // labelActYdsInspected
            // 
            this.labelActYdsInspected.Location = new System.Drawing.Point(431, 19);
            this.labelActYdsInspected.Name = "labelActYdsInspected";
            this.labelActYdsInspected.Size = new System.Drawing.Size(124, 23);
            this.labelActYdsInspected.TabIndex = 100;
            this.labelActYdsInspected.Text = "Act. Yds Inspected";
            // 
            // displayRoll
            // 
            this.displayRoll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayRoll.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "roll", true));
            this.displayRoll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayRoll.Location = new System.Drawing.Point(87, 19);
            this.displayRoll.Name = "displayRoll";
            this.displayRoll.Size = new System.Drawing.Size(100, 23);
            this.displayRoll.TabIndex = 101;
            // 
            // displayDyelot
            // 
            this.displayDyelot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDyelot.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "dyelot", true));
            this.displayDyelot.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDyelot.Location = new System.Drawing.Point(87, 55);
            this.displayDyelot.Name = "displayDyelot";
            this.displayDyelot.Size = new System.Drawing.Size(100, 23);
            this.displayDyelot.TabIndex = 102;
            // 
            // displaySP
            // 
            this.displaySP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySP.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "poid", true));
            this.displaySP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySP.Location = new System.Drawing.Point(283, 19);
            this.displaySP.Name = "displaySP";
            this.displaySP.Size = new System.Drawing.Size(136, 23);
            this.displaySP.TabIndex = 103;
            // 
            // displaySEQ1
            // 
            this.displaySEQ1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySEQ1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "seq1", true));
            this.displaySEQ1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySEQ1.Location = new System.Drawing.Point(257, 55);
            this.displaySEQ1.Name = "displaySEQ1";
            this.displaySEQ1.Size = new System.Drawing.Size(56, 23);
            this.displaySEQ1.TabIndex = 104;
            // 
            // labelSEQ2
            // 
            this.labelSEQ2.Location = new System.Drawing.Point(319, 55);
            this.labelSEQ2.Name = "labelSEQ2";
            this.labelSEQ2.Size = new System.Drawing.Size(52, 23);
            this.labelSEQ2.TabIndex = 105;
            this.labelSEQ2.Text = "SEQ2";
            // 
            // displaySEQ2
            // 
            this.displaySEQ2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySEQ2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "seq2", true));
            this.displaySEQ2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySEQ2.Location = new System.Drawing.Point(374, 55);
            this.displaySEQ2.Name = "displaySEQ2";
            this.displaySEQ2.Size = new System.Drawing.Size(45, 23);
            this.displaySEQ2.TabIndex = 106;
            // 
            // displayActYdsInspected
            // 
            this.displayActYdsInspected.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayActYdsInspected.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "actualyds", true));
            this.displayActYdsInspected.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayActYdsInspected.Location = new System.Drawing.Point(558, 19);
            this.displayActYdsInspected.Name = "displayActYdsInspected";
            this.displayActYdsInspected.Size = new System.Drawing.Size(97, 23);
            this.displayActYdsInspected.TabIndex = 107;
            // 
            // btnDefectPic
            // 
            this.btnDefectPic.Location = new System.Drawing.Point(563, 51);
            this.btnDefectPic.Name = "btnDefectPic";
            this.btnDefectPic.Size = new System.Drawing.Size(140, 30);
            this.btnDefectPic.TabIndex = 129;
            this.btnDefectPic.Text = "Defect Picture";
            this.btnDefectPic.UseVisualStyleBackColor = true;
            this.btnDefectPic.Click += new System.EventHandler(this.BtnDefectPic_Click);
            // 
            // P01_PhysicalInspection_Defect
            // 
            this.ClientSize = new System.Drawing.Size(715, 473);
            this.Controls.Add(this.btnDefectPic);
            this.Controls.Add(this.displayActYdsInspected);
            this.Controls.Add(this.displaySEQ2);
            this.Controls.Add(this.labelSEQ2);
            this.Controls.Add(this.displaySEQ1);
            this.Controls.Add(this.displaySP);
            this.Controls.Add(this.displayDyelot);
            this.Controls.Add(this.displayRoll);
            this.Controls.Add(this.labelActYdsInspected);
            this.Controls.Add(this.labelSEQ1);
            this.Controls.Add(this.labelSP);
            this.Controls.Add(this.labelDyelot);
            this.Controls.Add(this.labelRoll);
            this.Controls.Add(this.gridFabricInspection);
            this.Name = "P01_PhysicalInspection_Defect";
            this.OnLineHelpID = "Sci.Win.Subs.Input6A";
            this.Text = "Fabric Inspection - Point Record";
            this.WorkAlias = "Fir_Physical";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridFabricInspection, 0);
            this.Controls.SetChildIndex(this.labelRoll, 0);
            this.Controls.SetChildIndex(this.labelDyelot, 0);
            this.Controls.SetChildIndex(this.labelSP, 0);
            this.Controls.SetChildIndex(this.labelSEQ1, 0);
            this.Controls.SetChildIndex(this.labelActYdsInspected, 0);
            this.Controls.SetChildIndex(this.displayRoll, 0);
            this.Controls.SetChildIndex(this.displayDyelot, 0);
            this.Controls.SetChildIndex(this.displaySP, 0);
            this.Controls.SetChildIndex(this.displaySEQ1, 0);
            this.Controls.SetChildIndex(this.labelSEQ2, 0);
            this.Controls.SetChildIndex(this.displaySEQ2, 0);
            this.Controls.SetChildIndex(this.displayActYdsInspected, 0);
            this.Controls.SetChildIndex(this.btnDefectPic, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridFabricInspection)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Grid gridFabricInspection;
        private Win.UI.Label labelRoll;
        private Win.UI.Label labelDyelot;
        private Win.UI.Label labelSP;
        private Win.UI.Label labelSEQ1;
        private Win.UI.Label labelActYdsInspected;
        private Win.UI.DisplayBox displayRoll;
        private Win.UI.DisplayBox displayDyelot;
        private Win.UI.DisplayBox displaySP;
        private Win.UI.DisplayBox displaySEQ1;
        private Win.UI.Label labelSEQ2;
        private Win.UI.DisplayBox displaySEQ2;
        private Win.UI.DisplayBox displayActYdsInspected;
        private Win.UI.Button btnDefectPic;
    }
}
