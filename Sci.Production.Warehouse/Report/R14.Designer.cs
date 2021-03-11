namespace Sci.Production.Warehouse
{
    partial class R14
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
            this.dateWKNoETA = new Sci.Win.UI.DateRange();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelFabricType = new Sci.Win.UI.Label();
            this.labelWKNoETA = new Sci.Win.UI.Label();
            this.txtdropdownlistOrderType = new Sci.Production.Class.Txtdropdownlist();
            this.labelOrderType = new Sci.Win.UI.Label();
            this.comboFabricType = new System.Windows.Forms.ComboBox();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(439, 12);
            this.print.TabIndex = 4;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(439, 48);
            this.toexcel.TabIndex = 5;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(439, 84);
            this.close.TabIndex = 6;
            // 
            // dateWKNoETA
            // 
            this.dateWKNoETA.IsRequired = false;
            this.dateWKNoETA.Location = new System.Drawing.Point(115, 12);
            this.dateWKNoETA.Name = "dateWKNoETA";
            this.dateWKNoETA.Size = new System.Drawing.Size(280, 23);
            this.dateWKNoETA.TabIndex = 0;
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(13, 84);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 103;
            this.labelFactory.Text = "Factory";
            // 
            // labelFabricType
            // 
            this.labelFabricType.Lines = 0;
            this.labelFabricType.Location = new System.Drawing.Point(13, 48);
            this.labelFabricType.Name = "labelFabricType";
            this.labelFabricType.Size = new System.Drawing.Size(98, 23);
            this.labelFabricType.TabIndex = 118;
            this.labelFabricType.Text = "Material Type";
            // 
            // labelWKNoETA
            // 
            this.labelWKNoETA.Lines = 0;
            this.labelWKNoETA.Location = new System.Drawing.Point(14, 12);
            this.labelWKNoETA.Name = "labelWKNoETA";
            this.labelWKNoETA.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelWKNoETA.RectStyle.BorderWidth = 1F;
            this.labelWKNoETA.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.labelWKNoETA.RectStyle.ExtBorderWidth = 1F;
            this.labelWKNoETA.Size = new System.Drawing.Size(98, 23);
            this.labelWKNoETA.TabIndex = 122;
            this.labelWKNoETA.Text = "WK# ETA";
            this.labelWKNoETA.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelWKNoETA.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtdropdownlistOrderType
            // 
            this.txtdropdownlistOrderType.BackColor = System.Drawing.Color.White;
            this.txtdropdownlistOrderType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtdropdownlistOrderType.FormattingEnabled = true;
            this.txtdropdownlistOrderType.IsSupportUnselect = true;
            this.txtdropdownlistOrderType.Location = new System.Drawing.Point(115, 123);
            this.txtdropdownlistOrderType.Name = "txtdropdownlistOrderType";
            this.txtdropdownlistOrderType.Size = new System.Drawing.Size(261, 24);
            this.txtdropdownlistOrderType.TabIndex = 3;
            this.txtdropdownlistOrderType.Type = "Category";
            // 
            // labelOrderType
            // 
            this.labelOrderType.Lines = 0;
            this.labelOrderType.Location = new System.Drawing.Point(14, 123);
            this.labelOrderType.Name = "labelOrderType";
            this.labelOrderType.Size = new System.Drawing.Size(98, 23);
            this.labelOrderType.TabIndex = 125;
            this.labelOrderType.Text = "Order Type";
            // 
            // comboFabricType
            // 
            this.comboFabricType.FormattingEnabled = true;
            this.comboFabricType.Location = new System.Drawing.Point(115, 47);
            this.comboFabricType.Name = "comboFabricType";
            this.comboFabricType.Size = new System.Drawing.Size(121, 24);
            this.comboFabricType.TabIndex = 1;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.Location = new System.Drawing.Point(115, 84);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 126;
            this.txtfactory.IssupportJunk = true;
            // 
            // R14
            // 
            this.ClientSize = new System.Drawing.Size(531, 182);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.comboFabricType);
            this.Controls.Add(this.txtdropdownlistOrderType);
            this.Controls.Add(this.labelOrderType);
            this.Controls.Add(this.labelWKNoETA);
            this.Controls.Add(this.labelFabricType);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.dateWKNoETA);
            this.IsSupportToPrint = false;
            this.Name = "R14";
            this.Text = "R14. Material Outstanding / Lacking Report";
            this.Controls.SetChildIndex(this.dateWKNoETA, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelFabricType, 0);
            this.Controls.SetChildIndex(this.labelWKNoETA, 0);
            this.Controls.SetChildIndex(this.labelOrderType, 0);
            this.Controls.SetChildIndex(this.txtdropdownlistOrderType, 0);
            this.Controls.SetChildIndex(this.comboFabricType, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateRange dateWKNoETA;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelFabricType;
        private Win.UI.Label labelWKNoETA;
        private Class.Txtdropdownlist txtdropdownlistOrderType;
        private Win.UI.Label labelOrderType;
        private System.Windows.Forms.ComboBox comboFabricType;
        private Class.Txtfactory txtfactory;
    }
}
