namespace Sci.Production.Cutting
{
    partial class R10
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
            this.txtCutCell2 = new Sci.Win.UI.TextBox();
            this.label4 = new Sci.Win.UI.Label();
            this.txtCutCell1 = new Sci.Win.UI.TextBox();
            this.labCutCell = new Sci.Win.UI.Label();
            this.labCuttingSP = new Sci.Win.UI.Label();
            this.txtSP1 = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txtSP2 = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(446, 84);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(446, 12);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(446, 48);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomized.Location = new System.Drawing.Point(401, 120);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkUseCustomized.Location = new System.Drawing.Point(426, 156);
            // 
            // txtVersion
            // 
            this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVersion.Location = new System.Drawing.Point(426, 183);
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.BoolFtyGroupList = true;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IsProduceFty = false;
            this.txtfactory.IssupportJunk = false;
            this.txtfactory.Location = new System.Drawing.Point(106, 70);
            this.txtfactory.MDivision = null;
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(71, 23);
            this.txtfactory.TabIndex = 2;
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(106, 41);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(71, 23);
            this.txtMdivision.TabIndex = 1;
            // 
            // labFactory
            // 
            this.labFactory.Location = new System.Drawing.Point(13, 70);
            this.labFactory.Name = "labFactory";
            this.labFactory.Size = new System.Drawing.Size(90, 23);
            this.labFactory.TabIndex = 100;
            this.labFactory.Text = "Factory";
            // 
            // labM
            // 
            this.labM.Location = new System.Drawing.Point(13, 41);
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
            this.dateEstCutDate.Location = new System.Drawing.Point(106, 12);
            this.dateEstCutDate.Name = "dateEstCutDate";
            this.dateEstCutDate.Size = new System.Drawing.Size(280, 23);
            this.dateEstCutDate.TabIndex = 0;
            // 
            // txtCutCell2
            // 
            this.txtCutCell2.BackColor = System.Drawing.Color.White;
            this.txtCutCell2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutCell2.Location = new System.Drawing.Point(197, 128);
            this.txtCutCell2.MaxLength = 2;
            this.txtCutCell2.Name = "txtCutCell2";
            this.txtCutCell2.Size = new System.Drawing.Size(65, 23);
            this.txtCutCell2.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(174, 128);
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
            this.txtCutCell1.Location = new System.Drawing.Point(106, 128);
            this.txtCutCell1.MaxLength = 2;
            this.txtCutCell1.Name = "txtCutCell1";
            this.txtCutCell1.Size = new System.Drawing.Size(65, 23);
            this.txtCutCell1.TabIndex = 5;
            // 
            // labCutCell
            // 
            this.labCutCell.Location = new System.Drawing.Point(13, 128);
            this.labCutCell.Name = "labCutCell";
            this.labCutCell.Size = new System.Drawing.Size(90, 23);
            this.labCutCell.TabIndex = 110;
            this.labCutCell.Text = "Cut Cell";
            // 
            // labCuttingSP
            // 
            this.labCuttingSP.Location = new System.Drawing.Point(13, 99);
            this.labCuttingSP.Name = "labCuttingSP";
            this.labCuttingSP.Size = new System.Drawing.Size(90, 23);
            this.labCuttingSP.TabIndex = 111;
            this.labCuttingSP.Text = "Cutting SP#";
            // 
            // txtSP1
            // 
            this.txtSP1.BackColor = System.Drawing.Color.White;
            this.txtSP1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP1.Location = new System.Drawing.Point(106, 99);
            this.txtSP1.MaxLength = 20;
            this.txtSP1.Name = "txtSP1";
            this.txtSP1.Size = new System.Drawing.Size(150, 23);
            this.txtSP1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(259, 99);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 23);
            this.label1.TabIndex = 112;
            this.label1.Text = "～";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            this.label1.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label1.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtSP2
            // 
            this.txtSP2.BackColor = System.Drawing.Color.White;
            this.txtSP2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP2.Location = new System.Drawing.Point(282, 99);
            this.txtSP2.MaxLength = 20;
            this.txtSP2.Name = "txtSP2";
            this.txtSP2.Size = new System.Drawing.Size(150, 23);
            this.txtSP2.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 12);
            this.label2.Name = "label2";
            this.label2.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.label2.Size = new System.Drawing.Size(90, 23);
            this.label2.TabIndex = 126;
            this.label2.Text = "Est.Cut Date";
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // R10
            // 
            this.ClientSize = new System.Drawing.Size(538, 236);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSP2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSP1);
            this.Controls.Add(this.labCuttingSP);
            this.Controls.Add(this.txtCutCell2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtCutCell1);
            this.Controls.Add(this.labCutCell);
            this.Controls.Add(this.dateEstCutDate);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.labFactory);
            this.Controls.Add(this.labM);
            this.Name = "R10";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R10. Bulk Marker Release Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labM, 0);
            this.Controls.SetChildIndex(this.labFactory, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.dateEstCutDate, 0);
            this.Controls.SetChildIndex(this.labCutCell, 0);
            this.Controls.SetChildIndex(this.txtCutCell1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.txtCutCell2, 0);
            this.Controls.SetChildIndex(this.labCuttingSP, 0);
            this.Controls.SetChildIndex(this.txtSP1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtSP2, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Class.Txtfactory txtfactory;
        private Class.TxtMdivision txtMdivision;
        private Win.UI.Label labFactory;
        private Win.UI.Label labM;
        private Win.UI.DateRange dateEstCutDate;
        private Win.UI.TextBox txtCutCell2;
        private Win.UI.Label label4;
        private Win.UI.TextBox txtCutCell1;
        private Win.UI.Label labCutCell;
        private Win.UI.Label labCuttingSP;
        private Win.UI.TextBox txtSP1;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtSP2;
        private Win.UI.Label label2;
    }
}
