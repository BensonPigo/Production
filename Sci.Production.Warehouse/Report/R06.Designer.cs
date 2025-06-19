namespace Sci.Production.Warehouse
{
    partial class R06
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
            this.dateIssueDate = new Sci.Win.UI.DateRange();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelFabricType = new Sci.Win.UI.Label();
            this.labelShift = new Sci.Win.UI.Label();
            this.comboFabricType = new System.Windows.Forms.ComboBox();
            this.txtdropdownlistShift = new Sci.Production.Class.Txtdropdownlist();
            this.dateApproveDate = new Sci.Win.UI.DateRange();
            this.labelApproveDate = new Sci.Win.UI.Label();
            this.labelIssueDate = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.label1 = new Sci.Win.UI.Label();
            this.dateRequest = new Sci.Win.UI.DateRange();
            this.txtSP2 = new Sci.Win.UI.TextBox();
            this.label6 = new Sci.Win.UI.Label();
            this.txtSP1 = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(439, 12);
            this.print.TabIndex = 7;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(439, 48);
            this.toexcel.TabIndex = 8;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(439, 84);
            this.close.TabIndex = 9;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(393, 121);
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
            this.dateIssueDate.Location = new System.Drawing.Point(115, 48);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(280, 23);
            this.dateIssueDate.TabIndex = 1;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(14, 195);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 103;
            this.labelFactory.Text = "Factory";
            // 
            // labelFabricType
            // 
            this.labelFabricType.Location = new System.Drawing.Point(14, 267);
            this.labelFabricType.Name = "labelFabricType";
            this.labelFabricType.Size = new System.Drawing.Size(98, 23);
            this.labelFabricType.TabIndex = 118;
            this.labelFabricType.Text = "Material Type";
            // 
            // labelShift
            // 
            this.labelShift.Location = new System.Drawing.Point(14, 231);
            this.labelShift.Name = "labelShift";
            this.labelShift.Size = new System.Drawing.Size(98, 23);
            this.labelShift.TabIndex = 125;
            this.labelShift.Text = "Shift";
            // 
            // comboFabricType
            // 
            this.comboFabricType.FormattingEnabled = true;
            this.comboFabricType.Location = new System.Drawing.Point(115, 266);
            this.comboFabricType.Name = "comboFabricType";
            this.comboFabricType.Size = new System.Drawing.Size(121, 24);
            this.comboFabricType.TabIndex = 6;
            // 
            // txtdropdownlistShift
            // 
            this.txtdropdownlistShift.BackColor = System.Drawing.Color.White;
            this.txtdropdownlistShift.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtdropdownlistShift.FormattingEnabled = true;
            this.txtdropdownlistShift.IsSupportUnselect = true;
            this.txtdropdownlistShift.Location = new System.Drawing.Point(115, 230);
            this.txtdropdownlistShift.Name = "txtdropdownlistShift";
            this.txtdropdownlistShift.OldText = "";
            this.txtdropdownlistShift.Size = new System.Drawing.Size(121, 24);
            this.txtdropdownlistShift.TabIndex = 5;
            this.txtdropdownlistShift.Type = "Shift";
            // 
            // dateApproveDate
            // 
            // 
            // 
            // 
            this.dateApproveDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateApproveDate.DateBox1.Name = "";
            this.dateApproveDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateApproveDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateApproveDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateApproveDate.DateBox2.Name = "";
            this.dateApproveDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateApproveDate.DateBox2.TabIndex = 1;
            this.dateApproveDate.IsRequired = false;
            this.dateApproveDate.Location = new System.Drawing.Point(115, 84);
            this.dateApproveDate.Name = "dateApproveDate";
            this.dateApproveDate.Size = new System.Drawing.Size(280, 23);
            this.dateApproveDate.TabIndex = 2;
            // 
            // labelApproveDate
            // 
            this.labelApproveDate.BackColor = System.Drawing.Color.SkyBlue;
            this.labelApproveDate.Location = new System.Drawing.Point(14, 84);
            this.labelApproveDate.Name = "labelApproveDate";
            this.labelApproveDate.Size = new System.Drawing.Size(97, 23);
            this.labelApproveDate.TabIndex = 129;
            this.labelApproveDate.Text = "Approve Date";
            this.labelApproveDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelIssueDate
            // 
            this.labelIssueDate.Location = new System.Drawing.Point(14, 48);
            this.labelIssueDate.Name = "labelIssueDate";
            this.labelIssueDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelIssueDate.Size = new System.Drawing.Size(98, 23);
            this.labelIssueDate.TabIndex = 128;
            this.labelIssueDate.Text = "Issue Date";
            this.labelIssueDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(14, 159);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 131;
            this.labelM.Text = "M";
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.DefaultValue = false;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(115, 159);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.NeedInitialMdivision = false;
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 3;
            this.txtMdivision.Validated += new System.EventHandler(this.TxtMdivision_Validated);
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsIE = false;
            this.txtfactory.IsMultiselect = false;
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(115, 195);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.NeedInitialFactory = false;
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(14, 9);
            this.label1.Name = "label1";
            this.label1.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label1.Size = new System.Drawing.Size(98, 23);
            this.label1.TabIndex = 134;
            this.label1.Text = "Request Date";
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateRequest
            // 
            // 
            // 
            // 
            this.dateRequest.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateRequest.DateBox1.Name = "";
            this.dateRequest.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateRequest.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateRequest.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateRequest.DateBox2.Name = "";
            this.dateRequest.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateRequest.DateBox2.TabIndex = 1;
            this.dateRequest.IsRequired = false;
            this.dateRequest.Location = new System.Drawing.Point(115, 9);
            this.dateRequest.Name = "dateRequest";
            this.dateRequest.Size = new System.Drawing.Size(280, 23);
            this.dateRequest.TabIndex = 0;
            // 
            // txtSP2
            // 
            this.txtSP2.BackColor = System.Drawing.Color.White;
            this.txtSP2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP2.Location = new System.Drawing.Point(244, 121);
            this.txtSP2.Name = "txtSP2";
            this.txtSP2.Size = new System.Drawing.Size(100, 23);
            this.txtSP2.TabIndex = 136;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(218, 121);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 23);
            this.label6.TabIndex = 138;
            this.label6.Text = "～";
            // 
            // txtSP1
            // 
            this.txtSP1.BackColor = System.Drawing.Color.White;
            this.txtSP1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP1.Location = new System.Drawing.Point(115, 121);
            this.txtSP1.Name = "txtSP1";
            this.txtSP1.Size = new System.Drawing.Size(100, 23);
            this.txtSP1.TabIndex = 135;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(14, 121);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 23);
            this.label2.TabIndex = 137;
            this.label2.Text = "SP#";
            // 
            // R06
            // 
            this.ClientSize = new System.Drawing.Size(531, 344);
            this.Controls.Add(this.txtSP2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtSP1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateRequest);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.labelApproveDate);
            this.Controls.Add(this.labelIssueDate);
            this.Controls.Add(this.dateApproveDate);
            this.Controls.Add(this.txtdropdownlistShift);
            this.Controls.Add(this.comboFabricType);
            this.Controls.Add(this.labelShift);
            this.Controls.Add(this.labelFabricType);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.dateIssueDate);
            this.IsSupportToPrint = false;
            this.Name = "R06";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R06. Fabric/Accessory Lacking & Replacement Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.dateIssueDate, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelFabricType, 0);
            this.Controls.SetChildIndex(this.labelShift, 0);
            this.Controls.SetChildIndex(this.comboFabricType, 0);
            this.Controls.SetChildIndex(this.txtdropdownlistShift, 0);
            this.Controls.SetChildIndex(this.dateApproveDate, 0);
            this.Controls.SetChildIndex(this.labelIssueDate, 0);
            this.Controls.SetChildIndex(this.labelApproveDate, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.dateRequest, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtSP1, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.txtSP2, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateRange dateIssueDate;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelFabricType;
        private Win.UI.Label labelShift;
        private System.Windows.Forms.ComboBox comboFabricType;
        private Class.Txtdropdownlist txtdropdownlistShift;
        private Win.UI.DateRange dateApproveDate;
        private Win.UI.Label labelApproveDate;
        private Win.UI.Label labelIssueDate;
        private Win.UI.Label labelM;
        private Class.TxtMdivision txtMdivision;
        private Class.Txtfactory txtfactory;
        private Win.UI.Label label1;
        private Win.UI.DateRange dateRequest;
        private Win.UI.TextBox txtSP2;
        private Win.UI.Label label6;
        private Win.UI.TextBox txtSP1;
        private Win.UI.Label label2;
    }
}
