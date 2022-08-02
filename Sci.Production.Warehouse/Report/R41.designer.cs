namespace Sci.Production.Warehouse
{
    partial class R41
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
            this.labRequest = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.txtRequest_From = new Sci.Win.UI.TextBox();
            this.txtRequest_To = new Sci.Win.UI.TextBox();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.txtFactory = new Sci.Production.Class.Txtfactory();
            this.dateIssue = new Sci.Win.UI.DateRange();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(415, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(415, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(415, 84);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(283, 95);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(291, 71);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(207, 99);
            // 
            // labRequest
            // 
            this.labRequest.Location = new System.Drawing.Point(9, 42);
            this.labRequest.Name = "labRequest";
            this.labRequest.Size = new System.Drawing.Size(98, 23);
            this.labRequest.TabIndex = 97;
            this.labRequest.Text = "Request#";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 23);
            this.label2.TabIndex = 98;
            this.label2.Text = "M";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 23);
            this.label3.TabIndex = 99;
            this.label3.Text = "Factory";
            // 
            // txtRequest_From
            // 
            this.txtRequest_From.BackColor = System.Drawing.Color.White;
            this.txtRequest_From.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRequest_From.Location = new System.Drawing.Point(110, 42);
            this.txtRequest_From.Name = "txtRequest_From";
            this.txtRequest_From.Size = new System.Drawing.Size(114, 23);
            this.txtRequest_From.TabIndex = 102;
            // 
            // txtRequest_To
            // 
            this.txtRequest_To.BackColor = System.Drawing.Color.White;
            this.txtRequest_To.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRequest_To.Location = new System.Drawing.Point(251, 42);
            this.txtRequest_To.Name = "txtRequest_To";
            this.txtRequest_To.Size = new System.Drawing.Size(114, 23);
            this.txtRequest_To.TabIndex = 104;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(110, 68);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 105;
            // 
            // txtFactory
            // 
            this.txtFactory.BackColor = System.Drawing.Color.White;
            this.txtFactory.BoolFtyGroupList = true;
            this.txtFactory.FilteMDivision = false;
            this.txtFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactory.IsProduceFty = false;
            this.txtFactory.IssupportJunk = false;
            this.txtFactory.Location = new System.Drawing.Point(110, 98);
            this.txtFactory.MDivision = null;
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Size = new System.Drawing.Size(66, 23);
            this.txtFactory.TabIndex = 106;
            // 
            // dateIssue
            // 
            // 
            // 
            // 
            this.dateIssue.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateIssue.DateBox1.Name = "";
            this.dateIssue.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateIssue.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateIssue.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateIssue.DateBox2.Name = "";
            this.dateIssue.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateIssue.DateBox2.TabIndex = 1;
            this.dateIssue.IsRequired = false;
            this.dateIssue.Location = new System.Drawing.Point(111, 14);
            this.dateIssue.Name = "dateIssue";
            this.dateIssue.Size = new System.Drawing.Size(280, 23);
            this.dateIssue.TabIndex = 107;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(9, 14);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSCIDelivery.RectStyle.BorderWidth = 1F;
            this.labelSCIDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSCIDelivery.RectStyle.ExtBorderWidth = 1F;
            this.labelSCIDelivery.Size = new System.Drawing.Size(98, 23);
            this.labelSCIDelivery.TabIndex = 108;
            this.labelSCIDelivery.Text = "Issue Date";
            this.labelSCIDelivery.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSCIDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(227, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 23);
            this.label1.TabIndex = 109;
            this.label1.Text = "～";
            // 
            // R41
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 163);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateIssue);
            this.Controls.Add(this.labelSCIDelivery);
            this.Controls.Add(this.txtFactory);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.txtRequest_To);
            this.Controls.Add(this.txtRequest_From);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labRequest);
            this.Name = "R41";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R41. Issue Fabric for Cutting Tape List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.labRequest, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtRequest_From, 0);
            this.Controls.SetChildIndex(this.txtRequest_To, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.txtFactory, 0);
            this.Controls.SetChildIndex(this.labelSCIDelivery, 0);
            this.Controls.SetChildIndex(this.dateIssue, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labRequest;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.TextBox txtRequest_From;
        private Win.UI.TextBox txtRequest_To;
        private Class.TxtMdivision txtMdivision;
        private Class.Txtfactory txtFactory;
        private Win.UI.DateRange dateIssue;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Label label1;
    }

}