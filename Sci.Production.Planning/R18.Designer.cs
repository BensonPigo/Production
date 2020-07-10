namespace Sci.Production.Planning
{
    partial class R18
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
            this.labelSewingDate = new Sci.Win.UI.Label();
            this.dateSewingDate = new Sci.Win.UI.DateRange();
            this.labelFactory = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.labelM = new Sci.Win.UI.Label();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.comboArtworkType = new Sci.Win.UI.ComboBox();
            this.labelArtworkType = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(430, 12);
            this.print.TabIndex = 4;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(430, 48);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(430, 84);
            this.close.TabIndex = 6;
            // 
            // labelSewingDate
            // 
            this.labelSewingDate.Lines = 0;
            this.labelSewingDate.Location = new System.Drawing.Point(13, 12);
            this.labelSewingDate.Name = "labelSewingDate";
            this.labelSewingDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSewingDate.RectStyle.BorderWidth = 1F;
            this.labelSewingDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSewingDate.RectStyle.ExtBorderWidth = 1F;
            this.labelSewingDate.Size = new System.Drawing.Size(98, 23);
            this.labelSewingDate.TabIndex = 96;
            this.labelSewingDate.Text = "Sewing Date";
            this.labelSewingDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSewingDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateSewingDate
            // 
            this.dateSewingDate.IsRequired = false;
            this.dateSewingDate.Location = new System.Drawing.Point(115, 12);
            this.dateSewingDate.Name = "dateSewingDate";
            this.dateSewingDate.Size = new System.Drawing.Size(280, 23);
            this.dateSewingDate.TabIndex = 0;
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(13, 78);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 132;
            this.labelFactory.Text = "Factory";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.Location = new System.Drawing.Point(115, 78);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 2;
            this.txtfactory.IssupportJunk = true;
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(13, 45);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 134;
            this.labelM.Text = "M";
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(115, 45);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 1;
            // 
            // comboArtworkType
            // 
            this.comboArtworkType.BackColor = System.Drawing.Color.White;
            this.comboArtworkType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboArtworkType.FormattingEnabled = true;
            this.comboArtworkType.IsSupportUnselect = true;
            this.comboArtworkType.Location = new System.Drawing.Point(115, 111);
            this.comboArtworkType.Name = "comboArtworkType";
            this.comboArtworkType.Size = new System.Drawing.Size(121, 24);
            this.comboArtworkType.TabIndex = 3;
            // 
            // labelArtworkType
            // 
            this.labelArtworkType.Lines = 0;
            this.labelArtworkType.Location = new System.Drawing.Point(14, 112);
            this.labelArtworkType.Name = "labelArtworkType";
            this.labelArtworkType.Size = new System.Drawing.Size(98, 23);
            this.labelArtworkType.TabIndex = 136;
            this.labelArtworkType.Text = "Artwork Type";
            // 
            // R18
            // 
            this.ClientSize = new System.Drawing.Size(522, 230);
            this.Controls.Add(this.labelArtworkType);
            this.Controls.Add(this.comboArtworkType);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.dateSewingDate);
            this.Controls.Add(this.labelSewingDate);
            this.DefaultControl = "dateSewingDate";
            this.DefaultControlForEdit = "dateSewingDate";
            this.IsSupportToPrint = false;
            this.Name = "R18";
            this.Text = "R18. Heat Transfer Machine Forecast";
            this.Controls.SetChildIndex(this.labelSewingDate, 0);
            this.Controls.SetChildIndex(this.dateSewingDate, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.comboArtworkType, 0);
            this.Controls.SetChildIndex(this.labelArtworkType, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelSewingDate;
        private Win.UI.DateRange dateSewingDate;
        private Win.UI.Label labelFactory;
        private Class.Txtfactory txtfactory;
        private Win.UI.Label labelM;
        private Class.TxtMdivision txtMdivision;
        private Win.UI.ComboBox comboArtworkType;
        private Win.UI.Label labelArtworkType;
    }
}
