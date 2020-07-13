namespace Sci.Production.Quality
{
    partial class R09
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
            this.txtsupplier = new Sci.Production.Class.Txtsupplier();
            this.txtRefno = new Sci.Win.UI.TextBox();
            this.label13 = new Sci.Win.UI.Label();
            this.txtSPEnd = new Sci.Win.UI.TextBox();
            this.txtSPStart = new Sci.Win.UI.TextBox();
            this.dateInspDate = new Sci.Win.UI.DateRange();
            this.dateArriveWHDate = new Sci.Win.UI.DateRange();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.labelRefno = new Sci.Win.UI.Label();
            this.labelSP = new Sci.Win.UI.Label();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.labelArriveWHDate = new Sci.Win.UI.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtsupplier);
            this.panel1.Controls.Add(this.txtRefno);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.txtSPEnd);
            this.panel1.Controls.Add(this.txtSPStart);
            this.panel1.Controls.Add(this.dateInspDate);
            this.panel1.Controls.Add(this.dateArriveWHDate);
            this.panel1.Controls.Add(this.labelSupplier);
            this.panel1.Controls.Add(this.labelRefno);
            this.panel1.Controls.Add(this.labelSP);
            this.panel1.Controls.Add(this.labelSCIDelivery);
            this.panel1.Controls.Add(this.labelArriveWHDate);
            this.panel1.Location = new System.Drawing.Point(20, 9);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(472, 183);
            this.panel1.TabIndex = 94;
            // 
            // txtsupplier
            // 
            this.txtsupplier.DisplayBox1Binding = "";
            this.txtsupplier.Location = new System.Drawing.Point(174, 151);
            this.txtsupplier.Name = "txtsupplier";
            this.txtsupplier.Size = new System.Drawing.Size(147, 23);
            this.txtsupplier.TabIndex = 41;
            this.txtsupplier.TextBox1Binding = "";
            // 
            // txtRefno
            // 
            this.txtRefno.BackColor = System.Drawing.Color.White;
            this.txtRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefno.Location = new System.Drawing.Point(174, 113);
            this.txtRefno.Name = "txtRefno";
            this.txtRefno.Size = new System.Drawing.Size(125, 23);
            this.txtRefno.TabIndex = 38;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label13.Location = new System.Drawing.Point(302, 78);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(22, 23);
            this.label13.TabIndex = 35;
            this.label13.Text = "～";
            this.label13.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSPEnd
            // 
            this.txtSPEnd.BackColor = System.Drawing.Color.White;
            this.txtSPEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPEnd.Location = new System.Drawing.Point(323, 78);
            this.txtSPEnd.Name = "txtSPEnd";
            this.txtSPEnd.Size = new System.Drawing.Size(131, 23);
            this.txtSPEnd.TabIndex = 34;
            // 
            // txtSPStart
            // 
            this.txtSPStart.BackColor = System.Drawing.Color.White;
            this.txtSPStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPStart.Location = new System.Drawing.Point(174, 78);
            this.txtSPStart.Name = "txtSPStart";
            this.txtSPStart.Size = new System.Drawing.Size(125, 23);
            this.txtSPStart.TabIndex = 33;
            // 
            // dateInspDate
            // 
            // 
            // 
            // 
            this.dateInspDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateInspDate.DateBox1.Name = "";
            this.dateInspDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateInspDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateInspDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateInspDate.DateBox2.Name = "";
            this.dateInspDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateInspDate.DateBox2.TabIndex = 1;
            this.dateInspDate.IsRequired = false;
            this.dateInspDate.Location = new System.Drawing.Point(174, 3);
            this.dateInspDate.Name = "dateInspDate";
            this.dateInspDate.Size = new System.Drawing.Size(280, 23);
            this.dateInspDate.TabIndex = 29;
            // 
            // dateArriveWHDate
            // 
            // 
            // 
            // 
            this.dateArriveWHDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateArriveWHDate.DateBox1.Name = "";
            this.dateArriveWHDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateArriveWHDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateArriveWHDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateArriveWHDate.DateBox2.Name = "";
            this.dateArriveWHDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateArriveWHDate.DateBox2.TabIndex = 1;
            this.dateArriveWHDate.IsRequired = false;
            this.dateArriveWHDate.Location = new System.Drawing.Point(174, 40);
            this.dateArriveWHDate.Name = "dateArriveWHDate";
            this.dateArriveWHDate.Size = new System.Drawing.Size(280, 23);
            this.dateArriveWHDate.TabIndex = 28;
            // 
            // labelSupplier
            // 
            this.labelSupplier.Location = new System.Drawing.Point(9, 151);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(154, 23);
            this.labelSupplier.TabIndex = 21;
            this.labelSupplier.Text = "Supplier";
            // 
            // labelRefno
            // 
            this.labelRefno.Location = new System.Drawing.Point(9, 113);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.Size = new System.Drawing.Size(154, 23);
            this.labelRefno.TabIndex = 19;
            this.labelRefno.Text = "Refno";
            // 
            // labelSP
            // 
            this.labelSP.Location = new System.Drawing.Point(9, 78);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(154, 23);
            this.labelSP.TabIndex = 16;
            this.labelSP.Text = "SP#";
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(9, 3);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(154, 23);
            this.labelSCIDelivery.TabIndex = 13;
            this.labelSCIDelivery.Text = "Last Physical Insp Date";
            // 
            // labelArriveWHDate
            // 
            this.labelArriveWHDate.Location = new System.Drawing.Point(9, 40);
            this.labelArriveWHDate.Name = "labelArriveWHDate";
            this.labelArriveWHDate.Size = new System.Drawing.Size(154, 23);
            this.labelArriveWHDate.TabIndex = 12;
            this.labelArriveWHDate.Text = "Arrive W/H Date";
            // 
            // R09
            // 
            this.ClientSize = new System.Drawing.Size(627, 218);
            this.Controls.Add(this.panel1);
            this.Name = "R09";
            this.Text = "R09.Odor Inspection Report";
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
        private Win.UI.Label labelSupplier;
        private Win.UI.Label labelRefno;
        private Win.UI.Label labelSP;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Label labelArriveWHDate;
        private Class.Txtsupplier txtsupplier;
        private Win.UI.TextBox txtRefno;
        private Win.UI.Label label13;
        private Win.UI.TextBox txtSPEnd;
        private Win.UI.TextBox txtSPStart;
        private Win.UI.DateRange dateInspDate;
        private Win.UI.DateRange dateArriveWHDate;
    }
}
