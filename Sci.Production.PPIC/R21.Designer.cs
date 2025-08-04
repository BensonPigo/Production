namespace Sci.Production.PPIC
{
    partial class R21
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.dateRangeBuyerDelivery = new Sci.Win.UI.DateRange();
            this.dateTimeProcessFrom = new System.Windows.Forms.DateTimePicker();
            this.label5 = new Sci.Win.UI.Label();
            this.dateTimeProcessTo = new System.Windows.Forms.DateTimePicker();
            this.chkExcludeSisterTransferOut = new Sci.Win.UI.CheckBox();
            this.label6 = new Sci.Win.UI.Label();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.cbIncludeCencelOrder = new Sci.Win.UI.CheckBox();
            this.comboProcess = new Sci.Win.UI.ComboBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(573, 9);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(573, 45);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(573, 81);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(294, 131);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(426, 131);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(532, 131);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 23);
            this.label1.TabIndex = 97;
            this.label1.Text = "Buyer Delivery";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 23);
            this.label2.TabIndex = 98;
            this.label2.Text = "Process";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 23);
            this.label3.TabIndex = 99;
            this.label3.Text = "M";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 23);
            this.label4.TabIndex = 100;
            this.label4.Text = "Factory";
            // 
            // dateRangeBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateRangeBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRangeBuyerDelivery.DateBox1.Name = "";
            this.dateRangeBuyerDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRangeBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRangeBuyerDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRangeBuyerDelivery.DateBox2.Name = "";
            this.dateRangeBuyerDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRangeBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateRangeBuyerDelivery.Location = new System.Drawing.Point(111, 9);
            this.dateRangeBuyerDelivery.Name = "dateRangeBuyerDelivery";
            this.dateRangeBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateRangeBuyerDelivery.TabIndex = 101;
            // 
            // dateTimeProcessFrom
            // 
            this.dateTimeProcessFrom.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.dateTimeProcessFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimeProcessFrom.Location = new System.Drawing.Point(111, 74);
            this.dateTimeProcessFrom.Name = "dateTimeProcessFrom";
            this.dateTimeProcessFrom.Size = new System.Drawing.Size(200, 23);
            this.dateTimeProcessFrom.TabIndex = 103;
            this.dateTimeProcessFrom.Value = new System.DateTime(2022, 6, 1, 0, 0, 0, 0);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(314, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 23);
            this.label5.TabIndex = 104;
            this.label5.Text = "～";
            this.label5.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateTimeProcessTo
            // 
            this.dateTimeProcessTo.CustomFormat = "yyyy/MM/dd HH:mm:ss";
            this.dateTimeProcessTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimeProcessTo.Location = new System.Drawing.Point(338, 74);
            this.dateTimeProcessTo.Name = "dateTimeProcessTo";
            this.dateTimeProcessTo.Size = new System.Drawing.Size(200, 23);
            this.dateTimeProcessTo.TabIndex = 105;
            this.dateTimeProcessTo.Value = new System.DateTime(2022, 6, 1, 0, 0, 0, 0);
            // 
            // chkExcludeSisterTransferOut
            // 
            this.chkExcludeSisterTransferOut.AutoSize = true;
            this.chkExcludeSisterTransferOut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkExcludeSisterTransferOut.Location = new System.Drawing.Point(9, 182);
            this.chkExcludeSisterTransferOut.Name = "chkExcludeSisterTransferOut";
            this.chkExcludeSisterTransferOut.Size = new System.Drawing.Size(191, 21);
            this.chkExcludeSisterTransferOut.TabIndex = 108;
            this.chkExcludeSisterTransferOut.Text = "Exclude sister transfer out";
            this.chkExcludeSisterTransferOut.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(9, 227);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(326, 23);
            this.label6.TabIndex = 109;
            this.label6.Text = "This report include Bulk and Garment order only";
            this.label6.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.DefaultValue = true;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(111, 110);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.NeedInitialMdivision = false;
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 110;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = true;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsIE = false;
            this.txtfactory.IsMultiselect = false;
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(111, 147);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.NeedInitialFactory = false;
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 111;
            // 
            // cbIncludeCencelOrder
            // 
            this.cbIncludeCencelOrder.AutoSize = true;
            this.cbIncludeCencelOrder.Checked = true;
            this.cbIncludeCencelOrder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbIncludeCencelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbIncludeCencelOrder.Location = new System.Drawing.Point(9, 203);
            this.cbIncludeCencelOrder.Name = "cbIncludeCencelOrder";
            this.cbIncludeCencelOrder.Size = new System.Drawing.Size(160, 21);
            this.cbIncludeCencelOrder.TabIndex = 112;
            this.cbIncludeCencelOrder.Text = "Include Cancel Order";
            this.cbIncludeCencelOrder.UseVisualStyleBackColor = true;
            // 
            // comboProcess
            // 
            this.comboProcess.BackColor = System.Drawing.Color.White;
            this.comboProcess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboProcess.FormattingEnabled = true;
            this.comboProcess.IsSupportUnselect = true;
            this.comboProcess.Location = new System.Drawing.Point(111, 44);
            this.comboProcess.Name = "comboProcess";
            this.comboProcess.OldText = "";
            this.comboProcess.Size = new System.Drawing.Size(200, 24);
            this.comboProcess.TabIndex = 560;
            // 
            // R21
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(665, 278);
            this.Controls.Add(this.comboProcess);
            this.Controls.Add(this.cbIncludeCencelOrder);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chkExcludeSisterTransferOut);
            this.Controls.Add(this.dateTimeProcessTo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dateTimeProcessFrom);
            this.Controls.Add(this.dateRangeBuyerDelivery);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "R21";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R21. Carton Status Tracking List";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.dateRangeBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.dateTimeProcessFrom, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.dateTimeProcessTo, 0);
            this.Controls.SetChildIndex(this.chkExcludeSisterTransferOut, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.cbIncludeCencelOrder, 0);
            this.Controls.SetChildIndex(this.comboProcess, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.DateRange dateRangeBuyerDelivery;
        private System.Windows.Forms.DateTimePicker dateTimeProcessFrom;
        private Win.UI.Label label5;
        private System.Windows.Forms.DateTimePicker dateTimeProcessTo;
        private Win.UI.CheckBox chkExcludeSisterTransferOut;
        private Win.UI.Label label6;
        private Class.TxtMdivision txtMdivision;
        private Class.Txtfactory txtfactory;
        private Win.UI.CheckBox cbIncludeCencelOrder;
        private Win.UI.ComboBox comboProcess;
    }
}