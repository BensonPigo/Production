namespace Sci.Production.Quality
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.txtseason = new Sci.Production.Class.txtseason();
            this.txtbrand = new Sci.Production.Class.txtbrand();
            this.txtRefNo = new Sci.Win.UI.TextBox();
            this.txtsupplier = new Sci.Production.Class.txtsupplier();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.DateArriveWH = new Sci.Win.UI.DateRange();
            this.label2 = new Sci.Win.UI.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtseason);
            this.panel1.Controls.Add(this.txtbrand);
            this.panel1.Controls.Add(this.txtRefNo);
            this.panel1.Controls.Add(this.txtsupplier);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.DateArriveWH);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(29, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(477, 242);
            this.panel1.TabIndex = 94;
            // 
            // txtseason
            // 
            this.txtseason.BackColor = System.Drawing.Color.White;
            this.txtseason.BrandObjectName = null;
            this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason.Location = new System.Drawing.Point(141, 186);
            this.txtseason.Name = "txtseason";
            this.txtseason.Size = new System.Drawing.Size(104, 23);
            this.txtseason.TabIndex = 25;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(141, 141);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(94, 23);
            this.txtbrand.TabIndex = 24;
            // 
            // txtRefNo
            // 
            this.txtRefNo.BackColor = System.Drawing.Color.White;
            this.txtRefNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefNo.Location = new System.Drawing.Point(141, 100);
            this.txtRefNo.Name = "txtRefNo";
            this.txtRefNo.Size = new System.Drawing.Size(257, 23);
            this.txtRefNo.TabIndex = 23;
            // 
            // txtsupplier
            // 
            this.txtsupplier.DisplayBox1Binding = "";
            this.txtsupplier.Location = new System.Drawing.Point(141, 60);
            this.txtsupplier.Name = "txtsupplier";
            this.txtsupplier.Size = new System.Drawing.Size(147, 23);
            this.txtsupplier.TabIndex = 22;
            this.txtsupplier.TextBox1Binding = "";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(16, 186);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 23);
            this.label5.TabIndex = 21;
            this.label5.Text = "Season.............:";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(16, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 23);
            this.label4.TabIndex = 20;
            this.label4.Text = "Brand................:";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(16, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 23);
            this.label3.TabIndex = 19;
            this.label3.Text = "Ref#..................:";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(16, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 23);
            this.label1.TabIndex = 18;
            this.label1.Text = "Supplier............:";
            // 
            // DateArriveWH
            // 
            this.DateArriveWH.Location = new System.Drawing.Point(141, 23);
            this.DateArriveWH.Name = "DateArriveWH";
            this.DateArriveWH.Size = new System.Drawing.Size(280, 23);
            this.DateArriveWH.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(16, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 23);
            this.label2.TabIndex = 16;
            this.label2.Text = "Arrive W/H Date:";
            // 
            // R06
            // 
            this.ClientSize = new System.Drawing.Size(627, 314);
            this.Controls.Add(this.panel1);
            this.Name = "R06";
            this.Text = "R06. Supplier Score - Fabric";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.DateRange DateArriveWH;
        private Win.UI.Label label2;
        private Class.txtseason txtseason;
        private Class.txtbrand txtbrand;
        private Win.UI.TextBox txtRefNo;
        private Class.txtsupplier txtsupplier;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label1;
    }
}
