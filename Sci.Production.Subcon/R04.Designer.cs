namespace Sci.Production.Subcon
{
    partial class R04
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
            this.txtSupplier = new Sci.Production.Class.TxtsubconNoConfirm();
            this.labelM = new Sci.Win.UI.Label();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.lblIssueDate = new Sci.Win.UI.Label();
            this.dateIssueDate = new Sci.Win.UI.DateRange();
            this.label1 = new Sci.Win.UI.Label();
            this.txtstyle = new Sci.Production.Class.Txtstyle();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.txtSP_End = new Sci.Win.UI.TextBox();
            this.txtSP_Start = new Sci.Win.UI.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chShow = new Sci.Win.UI.CheckBox();
            this.radioPanelReportType = new Sci.Win.UI.RadioPanel();
            this.rbPaidnotContractQty = new Sci.Win.UI.RadioButton();
            this.rbPandQry_SewingQty = new Sci.Win.UI.RadioButton();
            this.radioPanelReportType.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(401, 91);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(401, 10);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(401, 46);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(355, 222);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(381, 222);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(381, 220);
            // 
            // txtSupplier
            // 
            this.txtSupplier.DisplayBox1Binding = "";
            this.txtSupplier.IsFreightForwarder = false;
            this.txtSupplier.IsIncludeJunk = false;
            this.txtSupplier.IsMisc = false;
            this.txtSupplier.IsShipping = false;
            this.txtSupplier.IsSubcon = false;
            this.txtSupplier.Location = new System.Drawing.Point(110, 38);
            this.txtSupplier.Name = "txtSupplier";
            this.txtSupplier.Size = new System.Drawing.Size(174, 23);
            this.txtSupplier.TabIndex = 137;
            this.txtSupplier.TextBox1Binding = "";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(10, 123);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 136;
            this.labelM.Text = "M";
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(111, 124);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 133;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(111, 152);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(121, 24);
            this.comboFactory.TabIndex = 134;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(10, 153);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 135;
            this.labelFactory.Text = "Factory";
            // 
            // lblIssueDate
            // 
            this.lblIssueDate.Location = new System.Drawing.Point(9, 9);
            this.lblIssueDate.Name = "lblIssueDate";
            this.lblIssueDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.lblIssueDate.RectStyle.BorderWidth = 1F;
            this.lblIssueDate.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.lblIssueDate.RectStyle.ExtBorderWidth = 1F;
            this.lblIssueDate.Size = new System.Drawing.Size(98, 23);
            this.lblIssueDate.TabIndex = 132;
            this.lblIssueDate.Text = "Issue Date";
            this.lblIssueDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.lblIssueDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateIssueDate
            // 
            // 
            // 
            // 
            this.dateIssueDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateIssueDate.DateBox1.Name = "";
            this.dateIssueDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateIssueDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateIssueDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateIssueDate.DateBox2.Name = "";
            this.dateIssueDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateIssueDate.DateBox2.TabIndex = 1;
            this.dateIssueDate.IsRequired = false;
            this.dateIssueDate.Location = new System.Drawing.Point(110, 9);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(280, 23);
            this.dateIssueDate.TabIndex = 131;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 38);
            this.label1.Name = "label1";
            this.label1.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.RectStyle.BorderWidth = 1F;
            this.label1.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label1.RectStyle.ExtBorderWidth = 1F;
            this.label1.Size = new System.Drawing.Size(98, 23);
            this.label1.TabIndex = 138;
            this.label1.Text = "Supplier";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtstyle
            // 
            this.txtstyle.BackColor = System.Drawing.Color.White;
            this.txtstyle.BrandObjectName = null;
            this.txtstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle.Location = new System.Drawing.Point(111, 66);
            this.txtstyle.Name = "txtstyle";
            this.txtstyle.SeasonObjectName = null;
            this.txtstyle.Size = new System.Drawing.Size(130, 23);
            this.txtstyle.TabIndex = 139;
            this.txtstyle.TarBrand = null;
            this.txtstyle.TarSeason = null;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 66);
            this.label2.Name = "label2";
            this.label2.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.RectStyle.BorderWidth = 1F;
            this.label2.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label2.RectStyle.ExtBorderWidth = 1F;
            this.label2.Size = new System.Drawing.Size(98, 23);
            this.label2.TabIndex = 140;
            this.label2.Text = "Style";
            this.label2.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 95);
            this.label3.Name = "label3";
            this.label3.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label3.RectStyle.BorderWidth = 1F;
            this.label3.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.label3.RectStyle.ExtBorderWidth = 1F;
            this.label3.Size = new System.Drawing.Size(98, 23);
            this.label3.TabIndex = 141;
            this.label3.Text = "SP#";
            this.label3.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label3.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSP_End
            // 
            this.txtSP_End.BackColor = System.Drawing.Color.White;
            this.txtSP_End.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP_End.Location = new System.Drawing.Point(264, 94);
            this.txtSP_End.Name = "txtSP_End";
            this.txtSP_End.Size = new System.Drawing.Size(128, 23);
            this.txtSP_End.TabIndex = 144;
            // 
            // txtSP_Start
            // 
            this.txtSP_Start.BackColor = System.Drawing.Color.White;
            this.txtSP_Start.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP_Start.Location = new System.Drawing.Point(111, 94);
            this.txtSP_Start.Name = "txtSP_Start";
            this.txtSP_Start.Size = new System.Drawing.Size(128, 23);
            this.txtSP_Start.TabIndex = 143;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(241, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 17);
            this.label4.TabIndex = 146;
            this.label4.Text = "～";
            // 
            // chShow
            // 
            this.chShow.AutoSize = true;
            this.chShow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chShow.Location = new System.Drawing.Point(5, 188);
            this.chShow.Name = "chShow";
            this.chShow.Size = new System.Drawing.Size(61, 21);
            this.chShow.TabIndex = 147;
            this.chShow.Text = "Show";
            this.chShow.UseVisualStyleBackColor = true;
            this.chShow.CheckedChanged += new System.EventHandler(this.ChShow_CheckedChanged);
            // 
            // radioPanelReportType
            // 
            this.radioPanelReportType.Controls.Add(this.rbPaidnotContractQty);
            this.radioPanelReportType.Controls.Add(this.rbPandQry_SewingQty);
            this.radioPanelReportType.Enabled = false;
            this.radioPanelReportType.Location = new System.Drawing.Point(66, 180);
            this.radioPanelReportType.Name = "radioPanelReportType";
            this.radioPanelReportType.Size = new System.Drawing.Size(256, 91);
            this.radioPanelReportType.TabIndex = 148;
            this.radioPanelReportType.Value = "detail";
            // 
            // rbPaidnotContractQty
            // 
            this.rbPaidnotContractQty.AutoSize = true;
            this.rbPaidnotContractQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rbPaidnotContractQty.Location = new System.Drawing.Point(6, 34);
            this.rbPaidnotContractQty.Name = "rbPaidnotContractQty";
            this.rbPaidnotContractQty.Size = new System.Drawing.Size(246, 38);
            this.rbPaidnotContractQty.TabIndex = 10;
            this.rbPaidnotContractQty.Text = "Paid Qty not equal to Contract Qty \r\n (Exclude P11 Staus = \'Closed\')";
            this.rbPaidnotContractQty.UseVisualStyleBackColor = true;
            this.rbPaidnotContractQty.Value = "summary";
            // 
            // rbPandQry_SewingQty
            // 
            this.rbPandQry_SewingQty.AutoSize = true;
            this.rbPandQry_SewingQty.Checked = true;
            this.rbPandQry_SewingQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.rbPandQry_SewingQty.Location = new System.Drawing.Point(6, 7);
            this.rbPandQry_SewingQty.Name = "rbPandQry_SewingQty";
            this.rbPandQry_SewingQty.Size = new System.Drawing.Size(167, 21);
            this.rbPandQry_SewingQty.TabIndex = 9;
            this.rbPandQry_SewingQty.TabStop = true;
            this.rbPandQry_SewingQty.Text = "Paid Qty > Sewing Qty";
            this.rbPandQry_SewingQty.UseVisualStyleBackColor = true;
            this.rbPandQry_SewingQty.Value = "detail";
            // 
            // R04
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(493, 298);
            this.Controls.Add(this.radioPanelReportType);
            this.Controls.Add(this.chShow);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSP_End);
            this.Controls.Add(this.txtSP_Start);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtstyle);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSupplier);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.lblIssueDate);
            this.Controls.Add(this.dateIssueDate);
            this.Name = "R04";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R04. Garment Subcon Out Payment Tracking Report";
            this.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.Controls.SetChildIndex(this.lblIssueDate, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.txtSupplier, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtstyle, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtSP_Start, 0);
            this.Controls.SetChildIndex(this.txtSP_End, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.chShow, 0);
            this.Controls.SetChildIndex(this.radioPanelReportType, 0);
            this.radioPanelReportType.ResumeLayout(false);
            this.radioPanelReportType.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Class.TxtsubconNoConfirm txtSupplier;
        private Win.UI.Label labelM;
        private Class.TxtMdivision txtMdivision;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.Label labelFactory;
        private Win.UI.Label lblIssueDate;
        private Win.UI.DateRange dateIssueDate;
        private Win.UI.Label label1;
        private Class.Txtstyle txtstyle;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.TextBox txtSP_End;
        private Win.UI.TextBox txtSP_Start;
        private System.Windows.Forms.Label label4;
        private Win.UI.CheckBox chShow;
        private Win.UI.RadioPanel radioPanelReportType;
        private Win.UI.RadioButton rbPaidnotContractQty;
        private Win.UI.RadioButton rbPandQry_SewingQty;
    }
}