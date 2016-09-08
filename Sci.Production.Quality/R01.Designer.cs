namespace Sci.Production.Quality
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
            this.panel1 = new Sci.Win.UI.Panel();
            this.txtsupplier = new Sci.Production.Class.txtsupplier();
            this.comboOverallResSta = new Sci.Win.UI.ComboBox();
            this.comboCategory = new Sci.Win.UI.ComboBox();
            this.txtRefno = new Sci.Win.UI.TextBox();
            this.label13 = new Sci.Win.UI.Label();
            this.txtSPEnd = new Sci.Win.UI.TextBox();
            this.txtSPStart = new Sci.Win.UI.TextBox();
            this.DateEstCutting = new Sci.Win.UI.DateRange();
            this.DateSewInLine = new Sci.Win.UI.DateRange();
            this.dateRange4 = new Sci.Win.UI.DateRange();
            this.DateSCIDelivery = new Sci.Win.UI.DateRange();
            this.DateArriveWH = new Sci.Win.UI.DateRange();
            this.DateLastPhyIns = new Sci.Win.UI.DateRange();
            this.label12 = new Sci.Win.UI.Label();
            this.label11 = new Sci.Win.UI.Label();
            this.label10 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.txtBrand = new Sci.Production.Class.txtbrand();
            this.txtSeason = new Sci.Production.Class.txtseason();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtSeason);
            this.panel1.Controls.Add(this.txtBrand);
            this.panel1.Controls.Add(this.txtsupplier);
            this.panel1.Controls.Add(this.comboOverallResSta);
            this.panel1.Controls.Add(this.comboCategory);
            this.panel1.Controls.Add(this.txtRefno);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.txtSPEnd);
            this.panel1.Controls.Add(this.txtSPStart);
            this.panel1.Controls.Add(this.DateEstCutting);
            this.panel1.Controls.Add(this.DateSewInLine);
            this.panel1.Controls.Add(this.dateRange4);
            this.panel1.Controls.Add(this.DateSCIDelivery);
            this.panel1.Controls.Add(this.DateArriveWH);
            this.panel1.Controls.Add(this.DateLastPhyIns);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(18, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(491, 506);
            this.panel1.TabIndex = 94;
            // 
            // txtsupplier
            // 
            this.txtsupplier.DisplayBox1Binding = "";
            this.txtsupplier.Location = new System.Drawing.Point(192, 414);
            this.txtsupplier.Name = "txtsupplier";
            this.txtsupplier.Size = new System.Drawing.Size(147, 23);
            this.txtsupplier.TabIndex = 27;
            this.txtsupplier.TextBox1Binding = "";
            // 
            // comboOverallResSta
            // 
            this.comboOverallResSta.BackColor = System.Drawing.Color.White;
            this.comboOverallResSta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboOverallResSta.FormattingEnabled = true;
            this.comboOverallResSta.IsSupportUnselect = true;
            this.comboOverallResSta.Items.AddRange(new object[] {
            "All",
            "Pass",
            "Fail",
            "Empty Result",
            "N/A inspection & test"});
            this.comboOverallResSta.Location = new System.Drawing.Point(192, 458);
            this.comboOverallResSta.Name = "comboOverallResSta";
            this.comboOverallResSta.Size = new System.Drawing.Size(161, 24);
            this.comboOverallResSta.TabIndex = 26;
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(192, 373);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.Size = new System.Drawing.Size(125, 24);
            this.comboCategory.TabIndex = 24;
            // 
            // txtRefno
            // 
            this.txtRefno.BackColor = System.Drawing.Color.White;
            this.txtRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefno.Location = new System.Drawing.Point(192, 332);
            this.txtRefno.Name = "txtRefno";
            this.txtRefno.Size = new System.Drawing.Size(253, 23);
            this.txtRefno.TabIndex = 23;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.label13.Lines = 0;
            this.label13.Location = new System.Drawing.Point(320, 204);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(22, 23);
            this.label13.TabIndex = 20;
            this.label13.Text = "～";
            this.label13.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSPEnd
            // 
            this.txtSPEnd.BackColor = System.Drawing.Color.White;
            this.txtSPEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPEnd.Location = new System.Drawing.Point(341, 204);
            this.txtSPEnd.Name = "txtSPEnd";
            this.txtSPEnd.Size = new System.Drawing.Size(131, 23);
            this.txtSPEnd.TabIndex = 19;
            // 
            // txtSPStart
            // 
            this.txtSPStart.BackColor = System.Drawing.Color.White;
            this.txtSPStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPStart.Location = new System.Drawing.Point(192, 204);
            this.txtSPStart.Name = "txtSPStart";
            this.txtSPStart.Size = new System.Drawing.Size(125, 23);
            this.txtSPStart.TabIndex = 18;
            // 
            // DateEstCutting
            // 
            this.DateEstCutting.Location = new System.Drawing.Point(192, 165);
            this.DateEstCutting.Name = "DateEstCutting";
            this.DateEstCutting.Size = new System.Drawing.Size(280, 23);
            this.DateEstCutting.TabIndex = 17;
            // 
            // DateSewInLine
            // 
            this.DateSewInLine.Location = new System.Drawing.Point(192, 127);
            this.DateSewInLine.Name = "DateSewInLine";
            this.DateSewInLine.Size = new System.Drawing.Size(280, 23);
            this.DateSewInLine.TabIndex = 16;
            // 
            // dateRange4
            // 
            this.dateRange4.Location = new System.Drawing.Point(265, 127);
            this.dateRange4.Name = "dateRange4";
            this.dateRange4.Size = new System.Drawing.Size(8, 23);
            this.dateRange4.TabIndex = 15;
            // 
            // DateSCIDelivery
            // 
            this.DateSCIDelivery.Location = new System.Drawing.Point(192, 90);
            this.DateSCIDelivery.Name = "DateSCIDelivery";
            this.DateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.DateSCIDelivery.TabIndex = 14;
            // 
            // DateArriveWH
            // 
            this.DateArriveWH.Location = new System.Drawing.Point(192, 53);
            this.DateArriveWH.Name = "DateArriveWH";
            this.DateArriveWH.Size = new System.Drawing.Size(280, 23);
            this.DateArriveWH.TabIndex = 13;
            // 
            // DateLastPhyIns
            // 
            this.DateLastPhyIns.Location = new System.Drawing.Point(192, 18);
            this.DateLastPhyIns.Name = "DateLastPhyIns";
            this.DateLastPhyIns.Size = new System.Drawing.Size(280, 23);
            this.DateLastPhyIns.TabIndex = 12;
            // 
            // label12
            // 
            this.label12.Lines = 0;
            this.label12.Location = new System.Drawing.Point(26, 459);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(154, 23);
            this.label12.TabIndex = 11;
            this.label12.Text = "Overall Result Status:";
            // 
            // label11
            // 
            this.label11.Lines = 0;
            this.label11.Location = new System.Drawing.Point(26, 415);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(154, 23);
            this.label11.TabIndex = 10;
            this.label11.Text = "Supplier..................:";
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(26, 373);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(154, 23);
            this.label10.TabIndex = 9;
            this.label10.Text = "Category.................:";
            // 
            // label9
            // 
            this.label9.Lines = 0;
            this.label9.Location = new System.Drawing.Point(26, 332);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(154, 23);
            this.label9.TabIndex = 8;
            this.label9.Text = "Refno......................:";
            // 
            // label8
            // 
            this.label8.Lines = 0;
            this.label8.Location = new System.Drawing.Point(26, 291);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(154, 23);
            this.label8.TabIndex = 7;
            this.label8.Text = "Brand.......................:";
            // 
            // label7
            // 
            this.label7.Lines = 0;
            this.label7.Location = new System.Drawing.Point(26, 249);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(154, 23);
            this.label7.TabIndex = 6;
            this.label7.Text = "Season.....................:";
            // 
            // label6
            // 
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(26, 204);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(154, 23);
            this.label6.TabIndex = 5;
            this.label6.Text = "SP#...........................:";
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(26, 165);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(154, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "Est. Cutting Date.......:";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(26, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(154, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "Sewing in-line Date...:";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(26, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "SCI Delivery..............:";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(26, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Arrive W/H Date........:";
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(26, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "last Physical Insp Date:";
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(192, 291);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(108, 23);
            this.txtBrand.TabIndex = 28;
            // 
            // txtSeason
            // 
            this.txtSeason.BackColor = System.Drawing.Color.White;
            this.txtSeason.BrandObjectName = null;
            this.txtSeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSeason.Location = new System.Drawing.Point(192, 249);
            this.txtSeason.Name = "txtSeason";
            this.txtSeason.Size = new System.Drawing.Size(135, 23);
            this.txtSeason.TabIndex = 50;
            // 
            // R01
            // 
            this.ClientSize = new System.Drawing.Size(627, 578);
            this.Controls.Add(this.panel1);
            this.Name = "R01";
            this.Text = "R01.Fabric Inspection & Laboratory List Report";
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
        private Win.UI.Label label12;
        private Win.UI.Label label11;
        private Win.UI.Label label10;
        private Win.UI.Label label9;
        private Win.UI.Label label8;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtSPEnd;
        private Win.UI.TextBox txtSPStart;
        private Win.UI.DateRange DateEstCutting;
        private Win.UI.DateRange DateSewInLine;
        private Win.UI.DateRange dateRange4;
        private Win.UI.DateRange DateSCIDelivery;
        private Win.UI.DateRange DateArriveWH;
        private Win.UI.DateRange DateLastPhyIns;
        private Win.UI.Label label13;
        private Win.UI.ComboBox comboOverallResSta;
        private Win.UI.ComboBox comboCategory;
        private Win.UI.TextBox txtRefno;
        private Class.txtsupplier txtsupplier;
        private Class.txtbrand txtBrand;
        private Class.txtseason txtSeason;
    }
}
