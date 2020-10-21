namespace Sci.Production.Centralized
{
    partial class R07
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
            this.lbOutputDate = new Sci.Win.UI.Label();
            this.dateOutputDate = new Sci.Win.UI.DateRange();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.lbCDcode = new Sci.Win.UI.Label();
            this.lbShift = new Sci.Win.UI.Label();
            this.txtCDCode = new Sci.Win.UI.TextBox();
            this.comboShift = new Sci.Win.UI.ComboBox();
            this.comboM = new Sci.Production.Class.ComboCentralizedM(this.components);
            this.comboFactory = new Sci.Production.Class.ComboCentralizedFactory(this.components);
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(442, 12);
            this.print.TabIndex = 4;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(442, 48);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(442, 84);
            this.close.TabIndex = 6;
            // 
            // lbOutputDate
            // 
            this.lbOutputDate.Location = new System.Drawing.Point(13, 12);
            this.lbOutputDate.Name = "lbOutputDate";
            this.lbOutputDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.lbOutputDate.RectStyle.BorderWidth = 1F;
            this.lbOutputDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.lbOutputDate.RectStyle.ExtBorderWidth = 1F;
            this.lbOutputDate.Size = new System.Drawing.Size(98, 23);
            this.lbOutputDate.TabIndex = 96;
            this.lbOutputDate.Text = "OutputDate";
            this.lbOutputDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.lbOutputDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateOutputDate
            // 
            // 
            // 
            // 
            this.dateOutputDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateOutputDate.DateBox1.Name = "";
            this.dateOutputDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateOutputDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateOutputDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateOutputDate.DateBox2.Name = "";
            this.dateOutputDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateOutputDate.DateBox2.TabIndex = 1;
            this.dateOutputDate.IsRequired = false;
            this.dateOutputDate.Location = new System.Drawing.Point(115, 12);
            this.dateOutputDate.Name = "dateOutputDate";
            this.dateOutputDate.Size = new System.Drawing.Size(280, 23);
            this.dateOutputDate.TabIndex = 0;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 83);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 132;
            this.labelFactory.Text = "Factory";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 48);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 134;
            this.labelM.Text = "M";
            // 
            // lbCDcode
            // 
            this.lbCDcode.Location = new System.Drawing.Point(13, 121);
            this.lbCDcode.Name = "lbCDcode";
            this.lbCDcode.Size = new System.Drawing.Size(98, 23);
            this.lbCDcode.TabIndex = 136;
            this.lbCDcode.Text = "CD Code";
            // 
            // lbShift
            // 
            this.lbShift.Location = new System.Drawing.Point(13, 157);
            this.lbShift.Name = "lbShift";
            this.lbShift.Size = new System.Drawing.Size(98, 23);
            this.lbShift.TabIndex = 137;
            this.lbShift.Text = "Shift";
            // 
            // txtCDCode
            // 
            this.txtCDCode.BackColor = System.Drawing.Color.White;
            this.txtCDCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCDCode.Location = new System.Drawing.Point(115, 121);
            this.txtCDCode.Name = "txtCDCode";
            this.txtCDCode.Size = new System.Drawing.Size(100, 23);
            this.txtCDCode.TabIndex = 138;
            // 
            // comboShift
            // 
            this.comboShift.BackColor = System.Drawing.Color.White;
            this.comboShift.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboShift.FormattingEnabled = true;
            this.comboShift.IsSupportUnselect = true;
            this.comboShift.Location = new System.Drawing.Point(115, 157);
            this.comboShift.Name = "comboShift";
            this.comboShift.OldText = "";
            this.comboShift.Size = new System.Drawing.Size(66, 24);
            this.comboShift.TabIndex = 139;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(115, 47);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(80, 24);
            this.comboM.TabIndex = 140;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(115, 83);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(80, 24);
            this.comboFactory.TabIndex = 141;
            // 
            // R07
            // 
            this.ClientSize = new System.Drawing.Size(522, 242);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.comboShift);
            this.Controls.Add(this.txtCDCode);
            this.Controls.Add(this.lbShift);
            this.Controls.Add(this.lbCDcode);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.dateOutputDate);
            this.Controls.Add(this.lbOutputDate);
            this.DefaultControl = "dateOutputDate";
            this.DefaultControlForEdit = "dateOutputDate";
            this.IsSupportToPrint = false;
            this.Name = "R07";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R07. Adidas Efficiency Report";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.lbOutputDate, 0);
            this.Controls.SetChildIndex(this.dateOutputDate, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.lbCDcode, 0);
            this.Controls.SetChildIndex(this.lbShift, 0);
            this.Controls.SetChildIndex(this.txtCDCode, 0);
            this.Controls.SetChildIndex(this.comboShift, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lbOutputDate;
        private Win.UI.DateRange dateOutputDate;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelM;
        private Win.UI.Label lbCDcode;
        private Win.UI.Label lbShift;
        private Win.UI.TextBox txtCDCode;
        private Win.UI.ComboBox comboShift;
        private Class.ComboCentralizedM comboM;
        private Class.ComboCentralizedFactory comboFactory;
    }
}
