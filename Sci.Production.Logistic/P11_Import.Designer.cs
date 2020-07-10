namespace Sci.Production.Logistic
{
    partial class P11_Import
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gridImport = new Sci.Win.UI.Grid();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.checkCancelOrder = new Sci.Win.UI.CheckBox();
            this.btnFind = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.btnCancel = new Sci.Win.UI.Button();
            this.dateRangeBuyer = new Sci.Win.UI.DateRange();
            this.textSP_From = new Sci.Win.UI.TextBox();
            this.label5 = new Sci.Win.UI.Label();
            this.textSP_To = new Sci.Win.UI.TextBox();
            this.textPackID = new Sci.Win.UI.TextBox();
            this.textCTN = new Sci.Win.UI.TextBox();
            this.txtcloglocation = new Sci.Production.Class.Txtcloglocation();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).BeginInit();
            this.SuspendLayout();
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
            this.gridImport.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridImport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridImport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridImport.Location = new System.Drawing.Point(12, 79);
            this.gridImport.Name = "gridImport";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridImport.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridImport.RowTemplate.Height = 24;
            this.gridImport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridImport.ShowCellToolTips = false;
            this.gridImport.Size = new System.Drawing.Size(892, 294);
            this.gridImport.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridImport.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Buyer Dlv.";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "SP#";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(373, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 3;
            this.label3.Text = "Pack ID";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(373, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "Location";
            // 
            // checkCancelOrder
            // 
            this.checkCancelOrder.AutoSize = true;
            this.checkCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkCancelOrder.Location = new System.Drawing.Point(611, 48);
            this.checkCancelOrder.Name = "checkCancelOrder";
            this.checkCancelOrder.Size = new System.Drawing.Size(130, 21);
            this.checkCancelOrder.TabIndex = 5;
            this.checkCancelOrder.Text = "Cancelled Order";
            this.checkCancelOrder.UseVisualStyleBackColor = true;
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(672, 5);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 30);
            this.btnFind.TabIndex = 6;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(738, 379);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(80, 30);
            this.btnImport.TabIndex = 7;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(824, 379);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // dateRangeBuyer
            // 
            // 
            // 
            // 
            this.dateRangeBuyer.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeBuyer.DateBox1.Name = "";
            this.dateRangeBuyer.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeBuyer.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeBuyer.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeBuyer.DateBox2.Name = "";
            this.dateRangeBuyer.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeBuyer.DateBox2.TabIndex = 1;
            this.dateRangeBuyer.Location = new System.Drawing.Point(90, 9);
            this.dateRangeBuyer.Name = "dateRangeBuyer";
            this.dateRangeBuyer.Size = new System.Drawing.Size(280, 23);
            this.dateRangeBuyer.TabIndex = 9;
            // 
            // textSP_From
            // 
            this.textSP_From.BackColor = System.Drawing.Color.White;
            this.textSP_From.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textSP_From.Location = new System.Drawing.Point(90, 46);
            this.textSP_From.Name = "textSP_From";
            this.textSP_From.Size = new System.Drawing.Size(126, 23);
            this.textSP_From.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(215, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(26, 23);
            this.label5.TabIndex = 11;
            this.label5.Text = " ～";
            this.label5.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // textSP_To
            // 
            this.textSP_To.BackColor = System.Drawing.Color.White;
            this.textSP_To.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textSP_To.Location = new System.Drawing.Point(242, 46);
            this.textSP_To.Name = "textSP_To";
            this.textSP_To.Size = new System.Drawing.Size(126, 23);
            this.textSP_To.TabIndex = 12;
            // 
            // textPackID
            // 
            this.textPackID.BackColor = System.Drawing.Color.White;
            this.textPackID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textPackID.Location = new System.Drawing.Point(451, 9);
            this.textPackID.Name = "textPackID";
            this.textPackID.Size = new System.Drawing.Size(157, 23);
            this.textPackID.TabIndex = 13;
            // 
            // textCTN
            // 
            this.textCTN.BackColor = System.Drawing.Color.White;
            this.textCTN.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textCTN.Location = new System.Drawing.Point(608, 9);
            this.textCTN.Name = "textCTN";
            this.textCTN.Size = new System.Drawing.Size(37, 23);
            this.textCTN.TabIndex = 14;
            // 
            // txtcloglocation
            // 
            this.txtcloglocation.BackColor = System.Drawing.Color.White;
            this.txtcloglocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcloglocation.IsSupportSytsemContextMenu = false;
            this.txtcloglocation.Location = new System.Drawing.Point(451, 46);
            this.txtcloglocation.MDivisionObjectName = null;
            this.txtcloglocation.Name = "txtcloglocation";
            this.txtcloglocation.Size = new System.Drawing.Size(157, 23);
            this.txtcloglocation.TabIndex = 15;
            // 
            // P11_Import
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 419);
            this.Controls.Add(this.txtcloglocation);
            this.Controls.Add(this.textCTN);
            this.Controls.Add(this.textPackID);
            this.Controls.Add(this.textSP_To);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textSP_From);
            this.Controls.Add(this.dateRangeBuyer);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.checkCancelOrder);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gridImport);
            this.Name = "P11_Import";
            this.Text = "P11_Import";
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Grid gridImport;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.CheckBox checkCancelOrder;
        private Win.UI.Button btnFind;
        private Win.UI.Button btnImport;
        private Win.UI.Button btnCancel;
        private Win.UI.DateRange dateRangeBuyer;
        private Win.UI.TextBox textSP_From;
        private Win.UI.Label label5;
        private Win.UI.TextBox textSP_To;
        private Win.UI.TextBox textPackID;
        private Win.UI.TextBox textCTN;
        private Class.Txtcloglocation txtcloglocation;
    }
}