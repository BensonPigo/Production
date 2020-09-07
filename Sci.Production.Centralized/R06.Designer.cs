namespace Sci.Production.Centralized
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
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
            this.comboM = new Sci.Production.Class.ComboCentralizedM(this.components);
            this.labelM = new Sci.Win.UI.Label();
            this.dateOutPutDate = new Sci.Win.UI.DateRange();
            this.comboFactory = new Sci.Production.Class.ComboCentralizedFactory(this.components);
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelOoutputDate = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(422, 129);
            this.print.TabIndex = 4;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(422, 16);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(422, 52);
            this.close.TabIndex = 6;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(261, 82);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(270, 118);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(270, 52);
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(117, 51);
            this.comboM.Name = "comboM";
            this.comboM.OldText = "";
            this.comboM.Size = new System.Drawing.Size(80, 24);
            this.comboM.TabIndex = 1;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(25, 51);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(88, 23);
            this.labelM.TabIndex = 8;
            this.labelM.Text = "M";
            // 
            // dateOutPutDate
            // 
            // 
            // 
            // 
            this.dateOutPutDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateOutPutDate.DateBox1.Name = "";
            this.dateOutPutDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateOutPutDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateOutPutDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateOutPutDate.DateBox2.Name = "";
            this.dateOutPutDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateOutPutDate.DateBox2.TabIndex = 1;
            this.dateOutPutDate.IsRequired = false;
            this.dateOutPutDate.Location = new System.Drawing.Point(117, 16);
            this.dateOutPutDate.Name = "dateOutPutDate";
            this.dateOutPutDate.Size = new System.Drawing.Size(280, 23);
            this.dateOutPutDate.TabIndex = 0;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(117, 85);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.OldText = "";
            this.comboFactory.Size = new System.Drawing.Size(80, 24);
            this.comboFactory.TabIndex = 2;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(117, 119);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(80, 23);
            this.txtbrand.TabIndex = 3;
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(25, 119);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(88, 23);
            this.labelBrand.TabIndex = 10;
            this.labelBrand.Text = "Brand";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(25, 85);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(88, 23);
            this.labelFactory.TabIndex = 9;
            this.labelFactory.Text = "Factory";
            // 
            // labelOoutputDate
            // 
            this.labelOoutputDate.Location = new System.Drawing.Point(25, 16);
            this.labelOoutputDate.Name = "labelOoutputDate";
            this.labelOoutputDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelOoutputDate.RectStyle.BorderWidth = 1F;
            this.labelOoutputDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelOoutputDate.RectStyle.ExtBorderWidth = 1F;
            this.labelOoutputDate.Size = new System.Drawing.Size(88, 23);
            this.labelOoutputDate.TabIndex = 104;
            this.labelOoutputDate.Text = "Output Date";
            this.labelOoutputDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelOoutputDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // R06
            // 
            this.ClientSize = new System.Drawing.Size(514, 184);
            this.Controls.Add(this.labelOoutputDate);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.dateOutPutDate);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.labelM);
            this.Name = "R06";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R06. Difference TMS&Cost between Subcon In & Out order";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.dateOutPutDate, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.labelOoutputDate, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Class.ComboCentralizedM comboM;
        private Win.UI.Label labelM;
        private Win.UI.DateRange dateOutPutDate;
        private Class.ComboCentralizedFactory comboFactory;
        private Class.Txtbrand txtbrand;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelOoutputDate;
    }
}
