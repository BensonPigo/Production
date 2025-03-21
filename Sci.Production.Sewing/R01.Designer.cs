namespace Sci.Production.Sewing
{
    partial class R01
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
            this.components = new System.ComponentModel.Container();
            this.labelDate = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelTeam = new Sci.Win.UI.Label();
            this.dateDate = new Sci.Win.UI.DateBox();
            this.label5 = new Sci.Win.UI.Label();
            this.comboSewingTeam1 = new Sci.Production.Class.ComboSewingTeam(this.components);
            this.comboFactory = new Sci.Production.Class.ComboFactory(this.components);
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(238, 12);
            this.print.TabIndex = 5;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(238, 48);
            this.toexcel.TabIndex = 6;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(238, 84);
            this.close.TabIndex = 7;
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(188, 37);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(188, 37);
            // 
            // labelDate
            // 
            this.labelDate.Location = new System.Drawing.Point(13, 13);
            this.labelDate.Name = "labelDate";
            this.labelDate.Size = new System.Drawing.Size(70, 23);
            this.labelDate.TabIndex = 94;
            this.labelDate.Text = "Date";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 48);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(70, 23);
            this.labelFactory.TabIndex = 95;
            this.labelFactory.Text = "Factory";
            // 
            // labelTeam
            // 
            this.labelTeam.Location = new System.Drawing.Point(13, 84);
            this.labelTeam.Name = "labelTeam";
            this.labelTeam.Size = new System.Drawing.Size(70, 23);
            this.labelTeam.TabIndex = 96;
            this.labelTeam.Text = "Team";
            // 
            // dateDate
            // 
            this.dateDate.Location = new System.Drawing.Point(88, 13);
            this.dateDate.Name = "dateDate";
            this.dateDate.Size = new System.Drawing.Size(103, 23);
            this.dateDate.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(13, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(226, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "P.S. This report include subcon-out.";
            this.label5.TextStyle.BorderColor = System.Drawing.Color.Red;
            this.label5.TextStyle.Color = System.Drawing.Color.Red;
            this.label5.TextStyle.ExtBorderColor = System.Drawing.Color.Red;
            this.label5.TextStyle.GradientColor = System.Drawing.Color.Red;
            // 
            // comboSewingTeam1
            // 
            this.comboSewingTeam1.BackColor = System.Drawing.Color.White;
            this.comboSewingTeam1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSewingTeam1.FormattingEnabled = true;
            this.comboSewingTeam1.IssupportEmptyitem = true;
            this.comboSewingTeam1.IssupportJunk = true;
            this.comboSewingTeam1.IsSupportUnselect = true;
            this.comboSewingTeam1.Location = new System.Drawing.Point(88, 84);
            this.comboSewingTeam1.Name = "comboSewingTeam1";
            this.comboSewingTeam1.OldText = "";
            this.comboSewingTeam1.Size = new System.Drawing.Size(88, 24);
            this.comboSewingTeam1.TabIndex = 2;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.FilteMDivision = true;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IssupportJunk = false;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(88, 47);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(88, 24);
            this.comboFactory.TabIndex = 564;
            // 
            // R01
            // 
            this.ClientSize = new System.Drawing.Size(330, 177);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.comboSewingTeam1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dateDate);
            this.Controls.Add(this.labelTeam);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelDate);
            this.DefaultControl = "dateDate";
            this.DefaultControlForEdit = "dateDate";
            this.IsSupportToPrint = false;
            this.Name = "R01";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R01. Daily CMP Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.labelDate, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelTeam, 0);
            this.Controls.SetChildIndex(this.dateDate, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.comboSewingTeam1, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelDate;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelTeam;
        private Win.UI.DateBox dateDate;
        private Win.UI.Label label5;
        private Class.ComboSewingTeam comboSewingTeam1;
        private Class.ComboFactory comboFactory;
    }
}
