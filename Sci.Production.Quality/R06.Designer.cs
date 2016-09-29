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
            this.DateArriveWH = new Sci.Win.UI.DateRange();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.txtsupplier1 = new Sci.Production.Class.txtsupplier();
            this.txtRefNo = new Sci.Win.UI.TextBox();
            this.txtbrand1 = new Sci.Production.Class.txtbrand();
            this.txtseason1 = new Sci.Production.Class.txtseason();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtseason1);
            this.panel1.Controls.Add(this.txtbrand1);
            this.panel1.Controls.Add(this.txtRefNo);
            this.panel1.Controls.Add(this.txtsupplier1);
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
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(16, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 23);
            this.label1.TabIndex = 18;
            this.label1.Text = "Supplier............:";
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
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(16, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 23);
            this.label4.TabIndex = 20;
            this.label4.Text = "Brand................:";
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
            // txtsupplier1
            // 
            this.txtsupplier1.DisplayBox1Binding = "";
            this.txtsupplier1.Location = new System.Drawing.Point(141, 60);
            this.txtsupplier1.Name = "txtsupplier1";
            this.txtsupplier1.Size = new System.Drawing.Size(147, 23);
            this.txtsupplier1.TabIndex = 22;
            this.txtsupplier1.TextBox1Binding = "";
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
            // txtbrand1
            // 
            this.txtbrand1.BackColor = System.Drawing.Color.White;
            this.txtbrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand1.Location = new System.Drawing.Point(141, 141);
            this.txtbrand1.Name = "txtbrand1";
            this.txtbrand1.Size = new System.Drawing.Size(94, 23);
            this.txtbrand1.TabIndex = 24;
            // 
            // txtseason1
            // 
            this.txtseason1.BackColor = System.Drawing.Color.White;
            this.txtseason1.BrandObjectName = null;
            this.txtseason1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason1.Location = new System.Drawing.Point(141, 186);
            this.txtseason1.Name = "txtseason1";
            this.txtseason1.Size = new System.Drawing.Size(104, 23);
            this.txtseason1.TabIndex = 25;
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
        private Class.txtseason txtseason1;
        private Class.txtbrand txtbrand1;
        private Win.UI.TextBox txtRefNo;
        private Class.txtsupplier txtsupplier1;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label1;
    }
}
