namespace Sci.Production.Warehouse
{
    partial class R25
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
            this.components = new System.ComponentModel.Container();
            this.label2 = new Sci.Win.UI.Label();
            this.lbFactory = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.lbSPNo = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.comboMDivision1 = new Sci.Production.Class.comboMDivision(this.components);
            this.comboFactory1 = new Sci.Production.Class.comboFactory(this.components);
            this.dateETA = new Sci.Win.UI.DateRange();
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
            this.label2.Location = new System.Drawing.Point(9, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 23);
            this.label2.TabIndex = 144;
            this.label2.Text = "M";
            // 
            // lbFactory
            // 
            this.lbFactory.Location = new System.Drawing.Point(9, 100);
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
            this.label8.TabIndex = 141;
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
            this.txtSPNoEnd.TabIndex = 1;
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(106, 12);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(130, 23);
            this.txtSPNoStart.TabIndex = 0;
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
            this.label1.Location = new System.Drawing.Point(9, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 23);
            this.label1.TabIndex = 152;
            this.label1.Text = "ETA";
            // 
            // comboMDivision1
            // 
            this.comboMDivision1.BackColor = System.Drawing.Color.White;
            this.comboMDivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMDivision1.FormattingEnabled = true;
            this.comboMDivision1.IsSupportUnselect = true;
            this.comboMDivision1.Location = new System.Drawing.Point(106, 70);
            this.comboMDivision1.Name = "comboMDivision1";
            this.comboMDivision1.OldText = "";
            this.comboMDivision1.Size = new System.Drawing.Size(80, 24);
            this.comboMDivision1.TabIndex = 4;
            // 
            // comboFactory1
            // 
            this.comboFactory1.BackColor = System.Drawing.Color.White;
            this.comboFactory1.FilteMDivision = false;
            this.comboFactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory1.FormattingEnabled = true;
            this.comboFactory1.IssupportJunk = false;
            this.comboFactory1.IsSupportUnselect = true;
            this.comboFactory1.Location = new System.Drawing.Point(106, 100);
            this.comboFactory1.Name = "comboFactory1";
            this.comboFactory1.OldText = "";
            this.comboFactory1.Size = new System.Drawing.Size(80, 24);
            this.comboFactory1.TabIndex = 5;
            // 
            // dateETA
            // 
            // 
            // 
            // 
            this.dateETA.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateETA.DateBox1.Name = "";
            this.dateETA.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateETA.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateETA.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateETA.DateBox2.Name = "";
            this.dateETA.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateETA.DateBox2.TabIndex = 1;
            this.dateETA.IsRequired = false;
            this.dateETA.Location = new System.Drawing.Point(106, 41);
            this.dateETA.Name = "dateETA";
            this.dateETA.Size = new System.Drawing.Size(280, 23);
            this.dateETA.TabIndex = 2;
            // 
            // R25
            // 
            this.ClientSize = new System.Drawing.Size(509, 191);
            this.Controls.Add(this.dateETA);
            this.Controls.Add(this.comboFactory1);
            this.Controls.Add(this.comboMDivision1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbSPNo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbFactory);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtSPNoEnd);
            this.Controls.Add(this.txtSPNoStart);
            this.Name = "R25";
            this.Text = "R25. Import Schedule List(Fabric/Accessory)";
            this.Controls.SetChildIndex(this.txtSPNoStart, 0);
            this.Controls.SetChildIndex(this.txtSPNoEnd, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.lbFactory, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.lbSPNo, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.comboMDivision1, 0);
            this.Controls.SetChildIndex(this.comboFactory1, 0);
            this.Controls.SetChildIndex(this.dateETA, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Win.UI.Label label2;
        private Win.UI.Label lbFactory;
        private Win.UI.Label label8;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.Label lbSPNo;
        private Win.UI.Label label1;
        private Class.comboMDivision comboMDivision1;
        private Class.comboFactory comboFactory1;
        private Win.UI.DateRange dateETA;
    }
}
