namespace Sci.Production.Warehouse
{
    partial class R26
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelConfirmedDate = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.dateConfirmedDate = new Sci.Win.UI.DateRange();
            this.label1 = new Sci.Win.UI.Label();
            this.txtuser1 = new Sci.Production.Class.Txtuser();
            this.txtMdivision1 = new Sci.Production.Class.TxtMdivision();
            this.txtfactory1 = new Sci.Production.Class.Txtfactory();
            this.txtbrand1 = new Sci.Production.Class.Txtbrand();
            this.label2 = new Sci.Win.UI.Label();
            this.txtWK = new Sci.Win.UI.TextBox();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSP2 = new Sci.Win.UI.TextBox();
            this.txtSP1 = new Sci.Win.UI.TextBox();
            this.label5 = new Sci.Win.UI.Label();
            this.chkWHP21only = new Sci.Win.UI.CheckBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(492, 84);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(492, 12);
            this.toexcel.TabIndex = 3;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(492, 48);
            this.close.TabIndex = 4;
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(360, 70);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(386, 106);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(386, 133);
            // 
            // labelConfirmedDate
            // 
            this.labelConfirmedDate.Location = new System.Drawing.Point(9, 12);
            this.labelConfirmedDate.Name = "labelConfirmedDate";
            this.labelConfirmedDate.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelConfirmedDate.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelConfirmedDate.Size = new System.Drawing.Size(110, 23);
            this.labelConfirmedDate.TabIndex = 5;
            this.labelConfirmedDate.Text = "Confirmed Date";
            this.labelConfirmedDate.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelConfirmedDate.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(9, 99);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(110, 23);
            this.labelFactory.TabIndex = 7;
            this.labelFactory.Text = "Factory";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(9, 70);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(110, 23);
            this.labelM.TabIndex = 6;
            this.labelM.Text = "M";
            // 
            // dateConfirmedDate
            // 
            // 
            // 
            // 
            this.dateConfirmedDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateConfirmedDate.DateBox1.Name = "";
            this.dateConfirmedDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateConfirmedDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateConfirmedDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateConfirmedDate.DateBox2.Name = "";
            this.dateConfirmedDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateConfirmedDate.DateBox2.TabIndex = 1;
            this.dateConfirmedDate.Location = new System.Drawing.Point(122, 12);
            this.dateConfirmedDate.Name = "dateConfirmedDate";
            this.dateConfirmedDate.Size = new System.Drawing.Size(280, 23);
            this.dateConfirmedDate.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 23);
            this.label1.TabIndex = 97;
            this.label1.Text = "Confirmed User";
            // 
            // txtuser1
            // 
            this.txtuser1.DisplayBox1Binding = "";
            this.txtuser1.Location = new System.Drawing.Point(122, 41);
            this.txtuser1.Name = "txtuser1";
            this.txtuser1.Size = new System.Drawing.Size(300, 23);
            this.txtuser1.TabIndex = 1;
            this.txtuser1.TextBox1Binding = "";
            // 
            // txtMdivision1
            // 
            this.txtMdivision1.BackColor = System.Drawing.Color.White;
            this.txtMdivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision1.Location = new System.Drawing.Point(122, 70);
            this.txtMdivision1.Name = "txtMdivision1";
            this.txtMdivision1.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision1.TabIndex = 2;
            // 
            // txtfactory1
            // 
            this.txtfactory1.BackColor = System.Drawing.Color.White;
            this.txtfactory1.BoolFtyGroupList = true;
            this.txtfactory1.FilteMDivision = false;
            this.txtfactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory1.IsProduceFty = false;
            this.txtfactory1.IssupportJunk = true;
            this.txtfactory1.Location = new System.Drawing.Point(122, 99);
            this.txtfactory1.MDivision = null;
            this.txtfactory1.Name = "txtfactory1";
            this.txtfactory1.Size = new System.Drawing.Size(66, 23);
            this.txtfactory1.TabIndex = 3;
            // 
            // txtbrand1
            // 
            this.txtbrand1.BackColor = System.Drawing.Color.White;
            this.txtbrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand1.Location = new System.Drawing.Point(122, 128);
            this.txtbrand1.Name = "txtbrand1";
            this.txtbrand1.Size = new System.Drawing.Size(66, 23);
            this.txtbrand1.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(110, 23);
            this.label2.TabIndex = 102;
            this.label2.Text = "Brand";
            // 
            // txtWK
            // 
            this.txtWK.BackColor = System.Drawing.Color.White;
            this.txtWK.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtWK.Location = new System.Drawing.Point(122, 157);
            this.txtWK.Name = "txtWK";
            this.txtWK.Size = new System.Drawing.Size(100, 23);
            this.txtWK.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(9, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 23);
            this.label3.TabIndex = 104;
            this.label3.Text = "WK#";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(248, 189);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(22, 17);
            this.label4.TabIndex = 139;
            this.label4.Text = "～";
            // 
            // txtSP2
            // 
            this.txtSP2.BackColor = System.Drawing.Color.White;
            this.txtSP2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP2.Location = new System.Drawing.Point(276, 186);
            this.txtSP2.Name = "txtSP2";
            this.txtSP2.Size = new System.Drawing.Size(120, 23);
            this.txtSP2.TabIndex = 7;
            // 
            // txtSP1
            // 
            this.txtSP1.BackColor = System.Drawing.Color.White;
            this.txtSP1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP1.Location = new System.Drawing.Point(122, 186);
            this.txtSP1.Name = "txtSP1";
            this.txtSP1.Size = new System.Drawing.Size(120, 23);
            this.txtSP1.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 186);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(110, 23);
            this.label5.TabIndex = 140;
            this.label5.Text = "SP#";
            // 
            // chkWHP21only
            // 
            this.chkWHP21only.AutoSize = true;
            this.chkWHP21only.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkWHP21only.Location = new System.Drawing.Point(427, 188);
            this.chkWHP21only.Name = "chkWHP21only";
            this.chkWHP21only.Size = new System.Drawing.Size(145, 21);
            this.chkWHP21only.TabIndex = 141;
            this.chkWHP21only.Text = "W/H P21 data only";
            this.chkWHP21only.UseVisualStyleBackColor = true;
            // 
            // R26
            // 
            this.ClientSize = new System.Drawing.Size(584, 250);
            this.Controls.Add(this.chkWHP21only);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSP2);
            this.Controls.Add(this.txtSP1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtWK);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtbrand1);
            this.Controls.Add(this.txtfactory1);
            this.Controls.Add(this.txtMdivision1);
            this.Controls.Add(this.txtuser1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelConfirmedDate);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.dateConfirmedDate);
            this.Name = "R26";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R26. Material Location Update Report";
            this.Controls.SetChildIndex(this.dateConfirmedDate, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.labelConfirmedDate, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtuser1, 0);
            this.Controls.SetChildIndex(this.txtMdivision1, 0);
            this.Controls.SetChildIndex(this.txtfactory1, 0);
            this.Controls.SetChildIndex(this.txtbrand1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtWK, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.txtSP1, 0);
            this.Controls.SetChildIndex(this.txtSP2, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.chkWHP21only, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelConfirmedDate;
        private Win.UI.Label labelFactory;
        private Win.UI.Label labelM;
        private Win.UI.DateRange dateConfirmedDate;
        private Win.UI.Label label1;
        private Class.Txtuser txtuser1;
        private Class.TxtMdivision txtMdivision1;
        private Class.Txtfactory txtfactory1;
        private Class.Txtbrand txtbrand1;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtWK;
        private Win.UI.Label label3;
        private System.Windows.Forms.Label label4;
        private Win.UI.TextBox txtSP2;
        private Win.UI.TextBox txtSP1;
        private Win.UI.Label label5;
        private Win.UI.CheckBox chkWHP21only;
    }
}
