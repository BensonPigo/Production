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
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(439, 12);
            this.print.TabIndex = 6;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(439, 48);
            this.toexcel.TabIndex = 7;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(439, 84);
            this.close.TabIndex = 8;
            // 
            // dateIssueDate
            // 
            this.dateIssueDate.IsRequired = false;
            this.dateIssueDate.Location = new System.Drawing.Point(116, 12);
            this.dateIssueDate.Name = "dateIssueDate";
            this.dateIssueDate.Size = new System.Drawing.Size(280, 23);
            this.dateIssueDate.TabIndex = 0;
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(15, 120);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 103;
            this.labelFactory.Text = "Factory";
            // 
            // labelFabricType
            // 
            this.labelFabricType.Lines = 0;
            this.labelFabricType.Location = new System.Drawing.Point(15, 192);
            this.labelFabricType.Name = "labelFabricType";
            this.labelFabricType.Size = new System.Drawing.Size(98, 23);
            this.labelFabricType.TabIndex = 118;
            this.labelFabricType.Text = "Fabric Type";
            // 
            // labelShift
            // 
            this.labelShift.Lines = 0;
            this.labelShift.Location = new System.Drawing.Point(15, 156);
            this.labelShift.Name = "labelShift";
            this.labelShift.Size = new System.Drawing.Size(98, 23);
            this.labelShift.TabIndex = 125;
            this.labelShift.Text = "Shift";
            // 
            // comboFabricType
            // 
            this.comboFabricType.FormattingEnabled = true;
            this.comboFabricType.Location = new System.Drawing.Point(116, 191);
            this.comboFabricType.Name = "comboFabricType";
            this.comboFabricType.Size = new System.Drawing.Size(121, 24);
            this.comboFabricType.TabIndex = 5;
            // 
            // txtdropdownlistShift
            // 
            this.txtdropdownlistShift.BackColor = System.Drawing.Color.White;
            this.txtdropdownlistShift.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtdropdownlistShift.FormattingEnabled = true;
            this.txtdropdownlistShift.IsSupportUnselect = true;
            this.txtdropdownlistShift.Location = new System.Drawing.Point(116, 155);
            this.txtdropdownlistShift.Name = "txtdropdownlistShift";
            this.txtdropdownlistShift.Size = new System.Drawing.Size(121, 24);
            this.txtdropdownlistShift.TabIndex = 4;
            this.txtdropdownlistShift.Type = "Shift";
            // 
            // dateApproveDate
            // 
            this.dateApproveDate.IsRequired = false;
            this.dateApproveDate.Location = new System.Drawing.Point(116, 48);
            this.dateApproveDate.Name = "dateApproveDate";
            this.dateApproveDate.Size = new System.Drawing.Size(280, 23);
            this.dateApproveDate.TabIndex = 1;
            // 
            // labelApproveDate
            // 
            this.labelApproveDate.BackColor = System.Drawing.Color.SkyBlue;
            this.labelApproveDate.Lines = 0;
            this.labelApproveDate.Location = new System.Drawing.Point(15, 48);
            this.labelApproveDate.Name = "labelApproveDate";
            this.labelApproveDate.Size = new System.Drawing.Size(97, 23);
            this.labelApproveDate.TabIndex = 129;
            this.labelApproveDate.Text = "Approve Date";
            this.labelApproveDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelIssueDate
            // 
            this.labelIssueDate.Lines = 0;
            this.labelIssueDate.Location = new System.Drawing.Point(15, 12);
            this.labelIssueDate.Name = "labelIssueDate";
            this.labelIssueDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelIssueDate.Size = new System.Drawing.Size(98, 23);
            this.labelIssueDate.TabIndex = 128;
            this.labelIssueDate.Text = "Issue Date";
            this.labelIssueDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(15, 84);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 131;
            this.labelM.Text = "M";
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(116, 84);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 2;
            this.txtMdivision.Validated += new System.EventHandler(this.txtMdivision_Validated);
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.Location = new System.Drawing.Point(116, 120);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 132;
            this.txtfactory.IssupportJunk = true;
            // 
            // R06
            // 
            this.ClientSize = new System.Drawing.Size(531, 266);
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
            this.Text = @"R06. Fabric/Accessory Lacking & Replacement Report";
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
    }
}
