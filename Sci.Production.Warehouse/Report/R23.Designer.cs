namespace Sci.Production.Warehouse
{
    partial class R23
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
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.labelLocation = new Sci.Win.UI.Label();
            this.checkBalanceQty = new Sci.Win.UI.CheckBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.txtLocationEnd = new Sci.Production.Class.TxtMtlLocation(this.components);
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.txtMtlLocationStart = new Sci.Production.Class.TxtMtlLocation(this.components);
            this.label2 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(405, 12);
            this.print.TabIndex = 7;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(405, 48);
            this.toexcel.TabIndex = 8;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(405, 84);
            this.close.TabIndex = 9;
            // 
            // dateSCIDelivery
            // 
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSCIDelivery.DateBox1.Name = "";
            this.dateSCIDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSCIDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSCIDelivery.DateBox2.Name = "";
            this.dateSCIDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDelivery.DateBox2.TabIndex = 1;
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(100, 44);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 2;
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(100, 12);
            this.txtSPNo.MaxLength = 13;
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(118, 23);
            this.txtSPNo.TabIndex = 0;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(8, 12);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSPNo.Size = new System.Drawing.Size(89, 23);
            this.labelSPNo.TabIndex = 96;
            this.labelSPNo.Text = "SP#";
            this.labelSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.BackColor = System.Drawing.Color.PaleGreen;
            this.labelSCIDelivery.Location = new System.Drawing.Point(8, 44);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSCIDelivery.Size = new System.Drawing.Size(89, 23);
            this.labelSCIDelivery.TabIndex = 99;
            this.labelSCIDelivery.Text = "SCI Delivery";
            this.labelSCIDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelLocation
            // 
            this.labelLocation.BackColor = System.Drawing.Color.PaleGreen;
            this.labelLocation.Location = new System.Drawing.Point(8, 76);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.RectStyle.Color = System.Drawing.Color.LightSkyBlue;
            this.labelLocation.Size = new System.Drawing.Size(89, 23);
            this.labelLocation.TabIndex = 100;
            this.labelLocation.Text = "Location";
            this.labelLocation.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // checkBalanceQty
            // 
            this.checkBalanceQty.AutoSize = true;
            this.checkBalanceQty.Checked = true;
            this.checkBalanceQty.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBalanceQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBalanceQty.Location = new System.Drawing.Point(203, 111);
            this.checkBalanceQty.Name = "checkBalanceQty";
            this.checkBalanceQty.Size = new System.Drawing.Size(128, 21);
            this.checkBalanceQty.TabIndex = 6;
            this.checkBalanceQty.Text = "Balance Qty > 0";
            this.checkBalanceQty.UseVisualStyleBackColor = true;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(8, 109);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(89, 23);
            this.labelFactory.TabIndex = 106;
            this.labelFactory.Text = "Factory";
            // 
            // txtLocationEnd
            // 
            this.txtLocationEnd.BackColor = System.Drawing.Color.White;
            this.txtLocationEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocationEnd.Location = new System.Drawing.Point(228, 76);
            this.txtLocationEnd.Name = "txtLocationEnd";
            this.txtLocationEnd.Size = new System.Drawing.Size(100, 23);
            this.txtLocationEnd.StockTypeFilte = "";
            this.txtLocationEnd.TabIndex = 4;
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.FilteMDivision = false;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.IssupportJunk = true;
            this.txtfactory.Location = new System.Drawing.Point(100, 109);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 5;
            // 
            // txtMtlLocationStart
            // 
            this.txtMtlLocationStart.BackColor = System.Drawing.Color.White;
            this.txtMtlLocationStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMtlLocationStart.Location = new System.Drawing.Point(100, 76);
            this.txtMtlLocationStart.Name = "txtMtlLocationStart";
            this.txtMtlLocationStart.Size = new System.Drawing.Size(100, 23);
            this.txtMtlLocationStart.StockTypeFilte = "";
            this.txtMtlLocationStart.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(203, 76);
            this.label2.Name = "label2";
            this.label2.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.label2.RectStyle.Color = System.Drawing.Color.Transparent;
            this.label2.RectStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label2.Size = new System.Drawing.Size(22, 23);
            this.label2.TabIndex = 109;
            this.label2.Text = "～";
            this.label2.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label2.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // R23
            // 
            this.ClientSize = new System.Drawing.Size(524, 172);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtMtlLocationStart);
            this.Controls.Add(this.txtLocationEnd);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.checkBalanceQty);
            this.Controls.Add(this.labelLocation);
            this.Controls.Add(this.labelSCIDelivery);
            this.Controls.Add(this.labelSPNo);
            this.Controls.Add(this.txtSPNo);
            this.Controls.Add(this.dateSCIDelivery);
            this.Name = "R23";
            this.Text = "R23. Material Location Query";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.dateSCIDelivery, 0);
            this.Controls.SetChildIndex(this.txtSPNo, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.labelSCIDelivery, 0);
            this.Controls.SetChildIndex(this.labelLocation, 0);
            this.Controls.SetChildIndex(this.checkBalanceQty, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.Controls.SetChildIndex(this.txtLocationEnd, 0);
            this.Controls.SetChildIndex(this.txtMtlLocationStart, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.Label labelSPNo;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Label labelLocation;
        private Win.UI.CheckBox checkBalanceQty;
        private Win.UI.Label labelFactory;
        private Class.Txtfactory txtfactory;
        private Class.TxtMtlLocation txtLocationEnd;
        private Class.TxtMtlLocation txtMtlLocationStart;
        private Win.UI.Label label2;
    }
}
