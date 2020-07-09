namespace Sci.Production.Warehouse
{
    partial class R38
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
            this.label2 = new Sci.Win.UI.Label();
            this.lbFactory = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.comboStockType = new Sci.Win.UI.ComboBox();
            this.labelStockType = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.lbSPNo = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.comboStatus = new Sci.Win.UI.ComboBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(419, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(419, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(419, 84);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 23);
            this.label2.TabIndex = 7;
            this.label2.Text = "M";
            // 
            // lbFactory
            // 
            this.lbFactory.Location = new System.Drawing.Point(9, 72);
            this.lbFactory.Name = "lbFactory";
            this.lbFactory.Size = new System.Drawing.Size(90, 23);
            this.lbFactory.TabIndex = 142;
            this.lbFactory.Text = "Factory";
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.Control;
            this.label8.Location = new System.Drawing.Point(243, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 34);
            this.label8.TabIndex = 0;
            this.label8.Text = "~";
            this.label8.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(262, 12);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(130, 23);
            this.txtSPNoEnd.TabIndex = 2;
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(106, 12);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(130, 23);
            this.txtSPNoStart.TabIndex = 1;
            // 
            // comboStockType
            // 
            this.comboStockType.BackColor = System.Drawing.Color.White;
            this.comboStockType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStockType.FormattingEnabled = true;
            this.comboStockType.IsSupportUnselect = true;
            this.comboStockType.Location = new System.Drawing.Point(106, 101);
            this.comboStockType.Name = "comboStockType";
            this.comboStockType.OldText = "";
            this.comboStockType.Size = new System.Drawing.Size(121, 24);
            this.comboStockType.TabIndex = 5;
            // 
            // labelStockType
            // 
            this.labelStockType.Location = new System.Drawing.Point(9, 102);
            this.labelStockType.Name = "labelStockType";
            this.labelStockType.Size = new System.Drawing.Size(90, 23);
            this.labelStockType.TabIndex = 148;
            this.labelStockType.Text = "Stock Type";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(106, 72);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 4;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(106, 42);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 3;
            // 
            // lbSPNo
            // 
            this.lbSPNo.Location = new System.Drawing.Point(9, 12);
            this.lbSPNo.Name = "lbSPNo";
            this.lbSPNo.Size = new System.Drawing.Size(90, 23);
            this.lbSPNo.TabIndex = 151;
            this.lbSPNo.Text = "SP#";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 134);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 23);
            this.label1.TabIndex = 152;
            this.label1.Text = "Status";
            // 
            // comboStatus
            // 
            this.comboStatus.BackColor = System.Drawing.Color.White;
            this.comboStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStatus.FormattingEnabled = true;
            this.comboStatus.IsSupportUnselect = true;
            this.comboStatus.Location = new System.Drawing.Point(106, 134);
            this.comboStatus.Name = "comboStatus";
            this.comboStatus.OldText = "";
            this.comboStatus.Size = new System.Drawing.Size(121, 24);
            this.comboStatus.TabIndex = 6;
            // 
            // R38
            // 
            this.ClientSize = new System.Drawing.Size(509, 219);
            this.Controls.Add(this.comboStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbSPNo);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.comboStockType);
            this.Controls.Add(this.labelStockType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbFactory);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtSPNoEnd);
            this.Controls.Add(this.txtSPNoStart);
            this.Name = "R38";
            this.Text = "R38. Material Lock/Unlock Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.txtSPNoStart, 0);
            this.Controls.SetChildIndex(this.txtSPNoEnd, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.lbFactory, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.labelStockType, 0);
            this.Controls.SetChildIndex(this.comboStockType, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.lbSPNo, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.comboStatus, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.Label label2;
        private Win.UI.Label lbFactory;
        private Win.UI.Label label8;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.ComboBox comboStockType;
        private Win.UI.Label labelStockType;
        private Class.Txtfactory txtfactory;
        private Class.TxtMdivision txtMdivision;
        private Win.UI.Label lbSPNo;
        private Win.UI.Label label1;
        private Win.UI.ComboBox comboStatus;
    }
}
