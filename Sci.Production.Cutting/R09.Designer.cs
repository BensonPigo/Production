namespace Sci.Production.Cutting
{
    partial class R09
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
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.labFactory = new Sci.Win.UI.Label();
            this.labM = new Sci.Win.UI.Label();
            this.dateEstCutDate = new Sci.Win.UI.DateRange();
            this.labEstCutDate = new Sci.Win.UI.Label();
            this.txtSpreadingNo2 = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.txtSpreadingNo1 = new Sci.Win.UI.TextBox();
            this.labSpreadingNo = new Sci.Win.UI.Label();
            this.txtCutCell2 = new Sci.Win.UI.TextBox();
            this.label4 = new Sci.Win.UI.Label();
            this.txtCutCell1 = new Sci.Win.UI.TextBox();
            this.labCutCell = new Sci.Win.UI.Label();
            this.labCuttingSP = new Sci.Win.UI.Label();
            this.txtCuttingSP = new Sci.Win.UI.TextBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(413, 132);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(413, 12);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(413, 48);
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(115, 42);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(71, 23);
            this.txtfactory.TabIndex = 98;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(115, 11);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(71, 23);
            this.txtMdivision.TabIndex = 97;
            // 
            // labFactory
            // 
            this.labFactory.Location = new System.Drawing.Point(22, 42);
            this.labFactory.Name = "labFactory";
            this.labFactory.Size = new System.Drawing.Size(90, 23);
            this.labFactory.TabIndex = 100;
            this.labFactory.Text = "Factory";
            // 
            // labM
            // 
            this.labM.Location = new System.Drawing.Point(22, 12);
            this.labM.Name = "labM";
            this.labM.Size = new System.Drawing.Size(90, 23);
            this.labM.TabIndex = 99;
            this.labM.Text = "M";
            // 
            // dateEstCutDate
            // 
            // 
            // 
            // 
            this.dateEstCutDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateEstCutDate.DateBox1.Name = "";
            this.dateEstCutDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateEstCutDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateEstCutDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateEstCutDate.DateBox2.Name = "";
            this.dateEstCutDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateEstCutDate.DateBox2.TabIndex = 1;
            this.dateEstCutDate.Location = new System.Drawing.Point(115, 102);
            this.dateEstCutDate.Name = "dateEstCutDate";
            this.dateEstCutDate.Size = new System.Drawing.Size(280, 23);
            this.dateEstCutDate.TabIndex = 100;
            // 
            // labEstCutDate
            // 
            this.labEstCutDate.Location = new System.Drawing.Point(22, 102);
            this.labEstCutDate.Name = "labEstCutDate";
            this.labEstCutDate.Size = new System.Drawing.Size(90, 23);
            this.labEstCutDate.TabIndex = 102;
            this.labEstCutDate.Text = "Est.Cut Date";
            // 
            // txtSpreadingNo2
            // 
            this.txtSpreadingNo2.BackColor = System.Drawing.Color.White;
            this.txtSpreadingNo2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSpreadingNo2.Location = new System.Drawing.Point(200, 132);
            this.txtSpreadingNo2.MaxLength = 2;
            this.txtSpreadingNo2.Name = "txtSpreadingNo2";
            this.txtSpreadingNo2.Size = new System.Drawing.Size(65, 23);
            this.txtSpreadingNo2.TabIndex = 102;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(177, 132);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 23);
            this.label9.TabIndex = 105;
            this.label9.Text = "～";
            this.label9.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            this.label9.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtSpreadingNo1
            // 
            this.txtSpreadingNo1.BackColor = System.Drawing.Color.White;
            this.txtSpreadingNo1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSpreadingNo1.Location = new System.Drawing.Point(115, 132);
            this.txtSpreadingNo1.MaxLength = 2;
            this.txtSpreadingNo1.Name = "txtSpreadingNo1";
            this.txtSpreadingNo1.Size = new System.Drawing.Size(65, 23);
            this.txtSpreadingNo1.TabIndex = 101;
            // 
            // labSpreadingNo
            // 
            this.labSpreadingNo.Location = new System.Drawing.Point(22, 132);
            this.labSpreadingNo.Name = "labSpreadingNo";
            this.labSpreadingNo.Size = new System.Drawing.Size(90, 23);
            this.labSpreadingNo.TabIndex = 106;
            this.labSpreadingNo.Text = "Spreading No";
            // 
            // txtCutCell2
            // 
            this.txtCutCell2.BackColor = System.Drawing.Color.White;
            this.txtCutCell2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutCell2.Location = new System.Drawing.Point(200, 162);
            this.txtCutCell2.MaxLength = 2;
            this.txtCutCell2.Name = "txtCutCell2";
            this.txtCutCell2.Size = new System.Drawing.Size(65, 23);
            this.txtCutCell2.TabIndex = 104;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(177, 162);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 23);
            this.label4.TabIndex = 109;
            this.label4.Text = "～";
            this.label4.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            this.label4.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label4.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtCutCell1
            // 
            this.txtCutCell1.BackColor = System.Drawing.Color.White;
            this.txtCutCell1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutCell1.Location = new System.Drawing.Point(115, 162);
            this.txtCutCell1.MaxLength = 2;
            this.txtCutCell1.Name = "txtCutCell1";
            this.txtCutCell1.Size = new System.Drawing.Size(65, 23);
            this.txtCutCell1.TabIndex = 103;
            // 
            // labCutCell
            // 
            this.labCutCell.Location = new System.Drawing.Point(22, 162);
            this.labCutCell.Name = "labCutCell";
            this.labCutCell.Size = new System.Drawing.Size(90, 23);
            this.labCutCell.TabIndex = 110;
            this.labCutCell.Text = "Cut Cell";
            // 
            // labCuttingSP
            // 
            this.labCuttingSP.Location = new System.Drawing.Point(22, 72);
            this.labCuttingSP.Name = "labCuttingSP";
            this.labCuttingSP.Size = new System.Drawing.Size(90, 23);
            this.labCuttingSP.TabIndex = 111;
            this.labCuttingSP.Text = "Cutting SP#";
            // 
            // txtCuttingSP
            // 
            this.txtCuttingSP.BackColor = System.Drawing.Color.White;
            this.txtCuttingSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCuttingSP.Location = new System.Drawing.Point(115, 72);
            this.txtCuttingSP.MaxLength = 20;
            this.txtCuttingSP.Name = "txtCuttingSP";
            this.txtCuttingSP.Size = new System.Drawing.Size(150, 23);
            this.txtCuttingSP.TabIndex = 99;
            // 
            // R09
            // 
            this.ClientSize = new System.Drawing.Size(505, 230);
            this.Controls.Add(this.txtCuttingSP);
            this.Controls.Add(this.labCuttingSP);
            this.Controls.Add(this.txtCutCell2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtCutCell1);
            this.Controls.Add(this.labCutCell);
            this.Controls.Add(this.txtSpreadingNo2);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtSpreadingNo1);
            this.Controls.Add(this.labSpreadingNo);
            this.Controls.Add(this.dateEstCutDate);
            this.Controls.Add(this.labEstCutDate);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.labFactory);
            this.Controls.Add(this.labM);
            this.Name = "R09";
            this.Text = "R09. Revised Marker Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labM, 0);
            this.Controls.SetChildIndex(this.labFactory, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.labEstCutDate, 0);
            this.Controls.SetChildIndex(this.dateEstCutDate, 0);
            this.Controls.SetChildIndex(this.labSpreadingNo, 0);
            this.Controls.SetChildIndex(this.txtSpreadingNo1, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.txtSpreadingNo2, 0);
            this.Controls.SetChildIndex(this.labCutCell, 0);
            this.Controls.SetChildIndex(this.txtCutCell1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.txtCutCell2, 0);
            this.Controls.SetChildIndex(this.labCuttingSP, 0);
            this.Controls.SetChildIndex(this.txtCuttingSP, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Class.Txtfactory txtfactory;
        private Class.TxtMdivision txtMdivision;
        private Win.UI.Label labFactory;
        private Win.UI.Label labM;
        private Win.UI.DateRange dateEstCutDate;
        private Win.UI.Label labEstCutDate;
        private Win.UI.TextBox txtSpreadingNo2;
        private Win.UI.Label label9;
        private Win.UI.TextBox txtSpreadingNo1;
        private Win.UI.Label labSpreadingNo;
        private Win.UI.TextBox txtCutCell2;
        private Win.UI.Label label4;
        private Win.UI.TextBox txtCutCell1;
        private Win.UI.Label labCutCell;
        private Win.UI.Label labCuttingSP;
        private Win.UI.TextBox txtCuttingSP;
    }
}
