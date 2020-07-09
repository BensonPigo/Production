namespace Sci.Production.Warehouse
{
    partial class R37
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.dateIssueDelivery = new Sci.Win.UI.DateRange();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.label10 = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.labelOrderBy = new Sci.Win.UI.Label();
            this.comboStockType = new System.Windows.Forms.ComboBox();
            this.labelFabricType = new Sci.Win.UI.Label();
            this.comboReason = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(414, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(414, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(414, 84);
            // 
            // dateIssueDelivery
            // 
            // 
            // 
            // 
            this.dateIssueDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateIssueDelivery.DateBox1.Name = "";
            this.dateIssueDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateIssueDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateIssueDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateIssueDelivery.DateBox2.Name = "";
            this.dateIssueDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateIssueDelivery.DateBox2.TabIndex = 1;
            this.dateIssueDelivery.IsRequired = false;
            this.dateIssueDelivery.Location = new System.Drawing.Point(120, 12);
            this.dateIssueDelivery.Name = "dateIssueDelivery";
            this.dateIssueDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateIssueDelivery.TabIndex = 97;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(18, 12);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSCIDelivery.RectStyle.BorderWidth = 1F;
            this.labelSCIDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSCIDelivery.RectStyle.ExtBorderWidth = 1F;
            this.labelSCIDelivery.Size = new System.Drawing.Size(98, 23);
            this.labelSCIDelivery.TabIndex = 98;
            this.labelSCIDelivery.Text = "Issue Date";
            this.labelSCIDelivery.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSCIDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(272, 48);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(128, 23);
            this.txtSPNoEnd.TabIndex = 117;
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(119, 48);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(128, 23);
            this.txtSPNoStart.TabIndex = 116;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(250, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 23);
            this.label10.TabIndex = 119;
            this.label10.Text = "～";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(18, 48);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSPNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSPNo.Size = new System.Drawing.Size(98, 23);
            this.labelSPNo.TabIndex = 118;
            this.labelSPNo.Text = "SP#";
            this.labelSPNo.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(119, 108);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 143;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(18, 108);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 142;
            this.labelFactory.Text = "Factory";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(18, 78);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 141;
            this.labelM.Text = "M";
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(120, 79);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 140;
            // 
            // labelOrderBy
            // 
            this.labelOrderBy.Location = new System.Drawing.Point(18, 168);
            this.labelOrderBy.Name = "labelOrderBy";
            this.labelOrderBy.Size = new System.Drawing.Size(98, 23);
            this.labelOrderBy.TabIndex = 147;
            this.labelOrderBy.Text = "Reason Code";
            // 
            // comboStockType
            // 
            this.comboStockType.ForeColor = System.Drawing.Color.Red;
            this.comboStockType.FormattingEnabled = true;
            this.comboStockType.Location = new System.Drawing.Point(119, 137);
            this.comboStockType.Name = "comboStockType";
            this.comboStockType.Size = new System.Drawing.Size(121, 24);
            this.comboStockType.TabIndex = 144;
            // 
            // labelFabricType
            // 
            this.labelFabricType.Location = new System.Drawing.Point(18, 137);
            this.labelFabricType.Name = "labelFabricType";
            this.labelFabricType.Size = new System.Drawing.Size(98, 23);
            this.labelFabricType.TabIndex = 146;
            this.labelFabricType.Text = "Stock Type";
            // 
            // comboReason
            // 
            this.comboReason.ForeColor = System.Drawing.Color.Red;
            this.comboReason.FormattingEnabled = true;
            this.comboReason.Location = new System.Drawing.Point(119, 167);
            this.comboReason.Name = "comboReason";
            this.comboReason.Size = new System.Drawing.Size(281, 24);
            this.comboReason.TabIndex = 148;
            // 
            // R37
            // 
            this.ClientSize = new System.Drawing.Size(506, 227);
            this.Controls.Add(this.comboReason);
            this.Controls.Add(this.labelOrderBy);
            this.Controls.Add(this.comboStockType);
            this.Controls.Add(this.labelFabricType);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.txtSPNoEnd);
            this.Controls.Add(this.txtSPNoStart);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.labelSPNo);
            this.Controls.Add(this.dateIssueDelivery);
            this.Controls.Add(this.labelSCIDelivery);
            this.Name = "R37";
            this.Text = "R37. Return Receiving Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelSCIDelivery, 0);
            this.Controls.SetChildIndex(this.dateIssueDelivery, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.txtSPNoStart, 0);
            this.Controls.SetChildIndex(this.txtSPNoEnd, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.labelFabricType, 0);
            this.Controls.SetChildIndex(this.comboStockType, 0);
            this.Controls.SetChildIndex(this.labelOrderBy, 0);
            this.Controls.SetChildIndex(this.comboReason, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateRange dateIssueDelivery;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.Label label10;
        private Win.UI.Label labelSPNo;
        private Class.Txtfactory txtfactory;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelM;
        private Class.TxtMdivision txtMdivision;
        private Win.UI.Label labelOrderBy;
        private System.Windows.Forms.ComboBox comboStockType;
        private Win.UI.Label labelFabricType;
        private System.Windows.Forms.ComboBox comboReason;
    }
}
