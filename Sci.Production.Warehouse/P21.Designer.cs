namespace Sci.Production.Warehouse
{
    partial class P21
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.lblReceivingID = new Sci.Win.UI.Label();
            this.lblWK = new Sci.Win.UI.Label();
            this.lblArriveWHDate = new Sci.Win.UI.Label();
            this.lblSP = new Sci.Win.UI.Label();
            this.txtRecivingID = new Sci.Win.UI.TextBox();
            this.txtWK = new Sci.Win.UI.TextBox();
            this.txtSP = new Sci.Win.UI.TextBox();
            this.btnQuery = new Sci.Win.UI.Button();
            this.gridReceiving = new Sci.Win.UI.Grid();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnUpdate = new Sci.Win.UI.Button();
            this.txtSeq = new Sci.Production.Class.TxtSeq();
            this.labelRef = new Sci.Win.UI.Label();
            this.txtRef = new Sci.Win.UI.TextBox();
            this.txtRoll = new Sci.Win.UI.TextBox();
            this.label6 = new Sci.Win.UI.Label();
            this.txtColor = new Sci.Win.UI.TextBox();
            this.label5 = new Sci.Win.UI.Label();
            this.txtDyelot = new Sci.Win.UI.TextBox();
            this.labelDyelot = new Sci.Win.UI.Label();
            this.dateBoxArriveWH = new Sci.Win.UI.DateRange();
            this.labSelectCnt = new Sci.Win.UI.Label();
            this.numSelectCnt = new Sci.Win.UI.NumericBox();
            this.labLocation = new Sci.Win.UI.Label();
            this.txtMtlLocation = new Sci.Production.Class.TxtMtlLocation(this.components);
            this.btnUpdateTime = new Sci.Win.UI.Button();
            this.dateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridReceiving)).BeginInit();
            this.SuspendLayout();
            // 
            // lblReceivingID
            // 
            this.lblReceivingID.Location = new System.Drawing.Point(9, 9);
            this.lblReceivingID.Name = "lblReceivingID";
            this.lblReceivingID.Size = new System.Drawing.Size(90, 23);
            this.lblReceivingID.TabIndex = 12;
            this.lblReceivingID.Text = "Receiving ID";
            // 
            // lblWK
            // 
            this.lblWK.Location = new System.Drawing.Point(9, 43);
            this.lblWK.Name = "lblWK";
            this.lblWK.Size = new System.Drawing.Size(90, 23);
            this.lblWK.TabIndex = 16;
            this.lblWK.Text = "WK#";
            // 
            // lblArriveWHDate
            // 
            this.lblArriveWHDate.Location = new System.Drawing.Point(220, 9);
            this.lblArriveWHDate.Name = "lblArriveWHDate";
            this.lblArriveWHDate.Size = new System.Drawing.Size(107, 23);
            this.lblArriveWHDate.TabIndex = 13;
            this.lblArriveWHDate.Text = "Arrive W/H Date";
            // 
            // lblSP
            // 
            this.lblSP.Location = new System.Drawing.Point(220, 43);
            this.lblSP.Name = "lblSP";
            this.lblSP.Size = new System.Drawing.Size(107, 23);
            this.lblSP.TabIndex = 17;
            this.lblSP.Text = "SP#";
            // 
            // txtRecivingID
            // 
            this.txtRecivingID.BackColor = System.Drawing.Color.White;
            this.txtRecivingID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRecivingID.Location = new System.Drawing.Point(102, 9);
            this.txtRecivingID.Name = "txtRecivingID";
            this.txtRecivingID.Size = new System.Drawing.Size(114, 23);
            this.txtRecivingID.TabIndex = 0;
            // 
            // txtWK
            // 
            this.txtWK.BackColor = System.Drawing.Color.White;
            this.txtWK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtWK.Location = new System.Drawing.Point(102, 43);
            this.txtWK.Name = "txtWK";
            this.txtWK.Size = new System.Drawing.Size(114, 23);
            this.txtWK.TabIndex = 4;
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(330, 43);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(112, 23);
            this.txtSP.TabIndex = 5;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(877, 7);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(119, 30);
            this.btnQuery.TabIndex = 11;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // gridReceiving
            // 
            this.gridReceiving.AllowUserToAddRows = false;
            this.gridReceiving.AllowUserToDeleteRows = false;
            this.gridReceiving.AllowUserToResizeRows = false;
            this.gridReceiving.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridReceiving.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridReceiving.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridReceiving.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridReceiving.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridReceiving.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridReceiving.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridReceiving.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridReceiving.Location = new System.Drawing.Point(9, 102);
            this.gridReceiving.Name = "gridReceiving";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridReceiving.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.gridReceiving.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridReceiving.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridReceiving.RowTemplate.Height = 24;
            this.gridReceiving.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridReceiving.ShowCellToolTips = false;
            this.gridReceiving.Size = new System.Drawing.Size(988, 313);
            this.gridReceiving.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridReceiving.TabIndex = 26;
            this.gridReceiving.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridReceiving_ColumnHeaderMouseClick);
            this.gridReceiving.Sorted += new System.EventHandler(this.GridReceiving_Sorted);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(917, 423);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 25;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Location = new System.Drawing.Point(818, 423);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(80, 30);
            this.btnUpdate.TabIndex = 24;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // txtSeq
            // 
            this.txtSeq.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.txtSeq.Location = new System.Drawing.Point(445, 43);
            this.txtSeq.Name = "txtSeq";
            this.txtSeq.Seq1 = "";
            this.txtSeq.Seq2 = "";
            this.txtSeq.Size = new System.Drawing.Size(61, 23);
            this.txtSeq.TabIndex = 6;
            // 
            // labelRef
            // 
            this.labelRef.Location = new System.Drawing.Point(513, 43);
            this.labelRef.Name = "labelRef";
            this.labelRef.Size = new System.Drawing.Size(40, 23);
            this.labelRef.TabIndex = 18;
            this.labelRef.Text = "Ref#";
            // 
            // txtRef
            // 
            this.txtRef.BackColor = System.Drawing.Color.White;
            this.txtRef.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRef.IsSupportEditMode = false;
            this.txtRef.Location = new System.Drawing.Point(556, 43);
            this.txtRef.Name = "txtRef";
            this.txtRef.Size = new System.Drawing.Size(87, 23);
            this.txtRef.TabIndex = 7;
            // 
            // txtRoll
            // 
            this.txtRoll.BackColor = System.Drawing.Color.White;
            this.txtRoll.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRoll.IsSupportEditMode = false;
            this.txtRoll.Location = new System.Drawing.Point(815, 43);
            this.txtRoll.Name = "txtRoll";
            this.txtRoll.Size = new System.Drawing.Size(60, 23);
            this.txtRoll.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(773, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 23);
            this.label6.TabIndex = 20;
            this.label6.Text = "Roll";
            // 
            // txtColor
            // 
            this.txtColor.BackColor = System.Drawing.Color.White;
            this.txtColor.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtColor.IsSupportEditMode = false;
            this.txtColor.Location = new System.Drawing.Point(700, 43);
            this.txtColor.Name = "txtColor";
            this.txtColor.Size = new System.Drawing.Size(70, 23);
            this.txtColor.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(646, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 23);
            this.label5.TabIndex = 19;
            this.label5.Text = "Color";
            // 
            // txtDyelot
            // 
            this.txtDyelot.BackColor = System.Drawing.Color.White;
            this.txtDyelot.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDyelot.IsSupportEditMode = false;
            this.txtDyelot.Location = new System.Drawing.Point(931, 43);
            this.txtDyelot.Name = "txtDyelot";
            this.txtDyelot.Size = new System.Drawing.Size(65, 23);
            this.txtDyelot.TabIndex = 10;
            // 
            // labelDyelot
            // 
            this.labelDyelot.Location = new System.Drawing.Point(877, 43);
            this.labelDyelot.Name = "labelDyelot";
            this.labelDyelot.Size = new System.Drawing.Size(51, 23);
            this.labelDyelot.TabIndex = 21;
            this.labelDyelot.Text = "Dyelot";
            // 
            // dateBoxArriveWH
            // 
            // 
            // 
            // 
            this.dateBoxArriveWH.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBoxArriveWH.DateBox1.Name = "";
            this.dateBoxArriveWH.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBoxArriveWH.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBoxArriveWH.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBoxArriveWH.DateBox2.Name = "";
            this.dateBoxArriveWH.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBoxArriveWH.DateBox2.TabIndex = 1;
            this.dateBoxArriveWH.IsRequired = false;
            this.dateBoxArriveWH.Location = new System.Drawing.Point(330, 7);
            this.dateBoxArriveWH.Name = "dateBoxArriveWH";
            this.dateBoxArriveWH.Size = new System.Drawing.Size(280, 23);
            this.dateBoxArriveWH.TabIndex = 1;
            // 
            // labSelectCnt
            // 
            this.labSelectCnt.Location = new System.Drawing.Point(9, 73);
            this.labSelectCnt.Name = "labSelectCnt";
            this.labSelectCnt.Size = new System.Drawing.Size(139, 23);
            this.labSelectCnt.TabIndex = 22;
            this.labSelectCnt.Text = "Total Items Selected:";
            // 
            // numSelectCnt
            // 
            this.numSelectCnt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numSelectCnt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numSelectCnt.IsSupportEditMode = false;
            this.numSelectCnt.Location = new System.Drawing.Point(151, 73);
            this.numSelectCnt.Name = "numSelectCnt";
            this.numSelectCnt.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSelectCnt.ReadOnly = true;
            this.numSelectCnt.Size = new System.Drawing.Size(65, 23);
            this.numSelectCnt.TabIndex = 23;
            this.numSelectCnt.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labLocation
            // 
            this.labLocation.Location = new System.Drawing.Point(613, 7);
            this.labLocation.Name = "labLocation";
            this.labLocation.Size = new System.Drawing.Size(65, 23);
            this.labLocation.TabIndex = 15;
            this.labLocation.Text = "Location";
            // 
            // txtMtlLocation
            // 
            this.txtMtlLocation.BackColor = System.Drawing.Color.White;
            this.txtMtlLocation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMtlLocation.Location = new System.Drawing.Point(681, 7);
            this.txtMtlLocation.Name = "txtMtlLocation";
            this.txtMtlLocation.Size = new System.Drawing.Size(95, 23);
            this.txtMtlLocation.StockTypeFilte = "";
            this.txtMtlLocation.TabIndex = 3;
            // 
            // btnUpdateTime
            // 
            this.btnUpdateTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUpdateTime.Location = new System.Drawing.Point(220, 423);
            this.btnUpdateTime.Name = "btnUpdateTime";
            this.btnUpdateTime.Size = new System.Drawing.Size(222, 30);
            this.btnUpdateTime.TabIndex = 42;
            this.btnUpdateTime.Text = "Update Cut Shadeband Time";
            this.btnUpdateTime.UseVisualStyleBackColor = true;
            this.btnUpdateTime.Click += new System.EventHandler(this.BtnUpdateTime_Click);
            // 
            // dateTimePicker
            // 
            this.dateTimePicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker.Location = new System.Drawing.Point(9, 425);
            this.dateTimePicker.Name = "dateTimePicker";
            this.dateTimePicker.Size = new System.Drawing.Size(200, 23);
            this.dateTimePicker.TabIndex = 43;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(643, 431);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 17);
            this.label1.TabIndex = 44;
            this.label1.Text = "Display Fabric data only.";
            // 
            // P21
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 462);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePicker);
            this.Controls.Add(this.btnUpdateTime);
            this.Controls.Add(this.txtMtlLocation);
            this.Controls.Add(this.labLocation);
            this.Controls.Add(this.numSelectCnt);
            this.Controls.Add(this.labSelectCnt);
            this.Controls.Add(this.dateBoxArriveWH);
            this.Controls.Add(this.txtRoll);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtColor);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtDyelot);
            this.Controls.Add(this.labelDyelot);
            this.Controls.Add(this.labelRef);
            this.Controls.Add(this.txtRef);
            this.Controls.Add(this.txtSeq);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.gridReceiving);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.txtSP);
            this.Controls.Add(this.txtWK);
            this.Controls.Add(this.txtRecivingID);
            this.Controls.Add(this.lblSP);
            this.Controls.Add(this.lblArriveWHDate);
            this.Controls.Add(this.lblWK);
            this.Controls.Add(this.lblReceivingID);
            this.Name = "P21";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P21. Batch update receiving Act.(kg) and Location";
            this.Controls.SetChildIndex(this.lblReceivingID, 0);
            this.Controls.SetChildIndex(this.lblWK, 0);
            this.Controls.SetChildIndex(this.lblArriveWHDate, 0);
            this.Controls.SetChildIndex(this.lblSP, 0);
            this.Controls.SetChildIndex(this.txtRecivingID, 0);
            this.Controls.SetChildIndex(this.txtWK, 0);
            this.Controls.SetChildIndex(this.txtSP, 0);
            this.Controls.SetChildIndex(this.btnQuery, 0);
            this.Controls.SetChildIndex(this.gridReceiving, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.btnUpdate, 0);
            this.Controls.SetChildIndex(this.txtSeq, 0);
            this.Controls.SetChildIndex(this.txtRef, 0);
            this.Controls.SetChildIndex(this.labelRef, 0);
            this.Controls.SetChildIndex(this.labelDyelot, 0);
            this.Controls.SetChildIndex(this.txtDyelot, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.txtColor, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txtRoll, 0);
            this.Controls.SetChildIndex(this.dateBoxArriveWH, 0);
            this.Controls.SetChildIndex(this.labSelectCnt, 0);
            this.Controls.SetChildIndex(this.numSelectCnt, 0);
            this.Controls.SetChildIndex(this.labLocation, 0);
            this.Controls.SetChildIndex(this.txtMtlLocation, 0);
            this.Controls.SetChildIndex(this.btnUpdateTime, 0);
            this.Controls.SetChildIndex(this.dateTimePicker, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridReceiving)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lblReceivingID;
        private Win.UI.Label lblWK;
        private Win.UI.Label lblArriveWHDate;
        private Win.UI.Label lblSP;
        private Win.UI.TextBox txtRecivingID;
        private Win.UI.TextBox txtWK;
        private Win.UI.TextBox txtSP;
        private Win.UI.Button btnQuery;
        private Win.UI.Grid gridReceiving;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnUpdate;
        private Class.TxtSeq txtSeq;
        private Win.UI.Label labelRef;
        private Win.UI.TextBox txtRef;
        private Win.UI.TextBox txtRoll;
        private Win.UI.Label label6;
        private Win.UI.TextBox txtColor;
        private Win.UI.Label label5;
        private Win.UI.TextBox txtDyelot;
        private Win.UI.Label labelDyelot;
        private Win.UI.DateRange dateBoxArriveWH;
        private Win.UI.Label labSelectCnt;
        private Win.UI.NumericBox numSelectCnt;
        private Win.UI.Label labLocation;
        private Class.TxtMtlLocation txtMtlLocation;
        private Win.UI.Button btnUpdateTime;
        private System.Windows.Forms.DateTimePicker dateTimePicker;
        private System.Windows.Forms.Label label1;
    }
}