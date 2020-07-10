namespace Sci.Production.Centralized
{
    partial class R10
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.dateOutputDateStart = new Sci.Win.UI.DateBox();
            this.label5 = new Sci.Win.UI.Label();
            this.dateOutputDateEnd = new Sci.Win.UI.DateBox();
            this.comboExchangeRate = new Sci.Win.UI.ComboBox();
            this.checkIncludeLocalOrder = new Sci.Win.UI.CheckBox();
            this.txtCentralizedFactory1 = new Sci.Production.Class.TxtCentralizedFactory();
            this.txtcountry1 = new Sci.Production.Class.Txtcountry();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(457, 37);
            this.print.TabIndex = 6;
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(457, 73);
            this.toexcel.TabIndex = 6;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(457, 109);
            this.close.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(41, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 23);
            this.label1.TabIndex = 94;
            this.label1.Text = "Output Date";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(41, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 23);
            this.label2.TabIndex = 95;
            this.label2.Text = "Factory";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(41, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 23);
            this.label3.TabIndex = 96;
            this.label3.Text = "Country";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(41, 154);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(106, 23);
            this.label4.TabIndex = 97;
            this.label4.Text = "Exchange Rate";
            // 
            // dateOutputDateStart
            // 
            this.dateOutputDateStart.Location = new System.Drawing.Point(150, 48);
            this.dateOutputDateStart.Name = "dateOutputDateStart";
            this.dateOutputDateStart.Size = new System.Drawing.Size(130, 23);
            this.dateOutputDateStart.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(283, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 23);
            this.label5.TabIndex = 99;
            this.label5.Text = "~";
            // 
            // dateOutputDateEnd
            // 
            this.dateOutputDateEnd.Location = new System.Drawing.Point(303, 48);
            this.dateOutputDateEnd.Name = "dateOutputDateEnd";
            this.dateOutputDateEnd.Size = new System.Drawing.Size(130, 23);
            this.dateOutputDateEnd.TabIndex = 1;
            // 
            // comboExchangeRate
            // 
            this.comboExchangeRate.BackColor = System.Drawing.Color.White;
            this.comboExchangeRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboExchangeRate.FormattingEnabled = true;
            this.comboExchangeRate.IsSupportUnselect = true;
            this.comboExchangeRate.Items.AddRange(new object[] {
            "FX",
            "KP"});
            this.comboExchangeRate.Location = new System.Drawing.Point(150, 153);
            this.comboExchangeRate.Name = "comboExchangeRate";
            this.comboExchangeRate.OldText = "";
            this.comboExchangeRate.Size = new System.Drawing.Size(121, 24);
            this.comboExchangeRate.TabIndex = 4;
            // 
            // checkIncludeLocalOrder
            // 
            this.checkIncludeLocalOrder.AutoSize = true;
            this.checkIncludeLocalOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIncludeLocalOrder.Location = new System.Drawing.Point(41, 195);
            this.checkIncludeLocalOrder.Name = "checkIncludeLocalOrder";
            this.checkIncludeLocalOrder.Size = new System.Drawing.Size(151, 21);
            this.checkIncludeLocalOrder.TabIndex = 5;
            this.checkIncludeLocalOrder.Text = "Include Local Order";
            this.checkIncludeLocalOrder.UseVisualStyleBackColor = true;
            // 
            // txtCentralizedFactory1
            // 
            this.txtCentralizedFactory1.BackColor = System.Drawing.Color.White;
            this.txtCentralizedFactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCentralizedFactory1.Location = new System.Drawing.Point(150, 84);
            this.txtCentralizedFactory1.Name = "txtCentralizedFactory1";
            this.txtCentralizedFactory1.Size = new System.Drawing.Size(108, 23);
            this.txtCentralizedFactory1.TabIndex = 2;
            // 
            // txtcountry1
            // 
            this.txtcountry1.DisplayBox1Binding = "";
            this.txtcountry1.Location = new System.Drawing.Point(150, 117);
            this.txtcountry1.Name = "txtcountry1";
            this.txtcountry1.Size = new System.Drawing.Size(232, 22);
            this.txtcountry1.TabIndex = 3;
            this.txtcountry1.TextBox1Binding = "";
            // 
            // R10
            // 
            this.ClientSize = new System.Drawing.Size(549, 280);
            this.Controls.Add(this.txtcountry1);
            this.Controls.Add(this.txtCentralizedFactory1);
            this.Controls.Add(this.checkIncludeLocalOrder);
            this.Controls.Add(this.comboExchangeRate);
            this.Controls.Add(this.dateOutputDateEnd);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dateOutputDateStart);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "R10";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R10.Output Summary with FOB";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.dateOutputDateStart, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.dateOutputDateEnd, 0);
            this.Controls.SetChildIndex(this.comboExchangeRate, 0);
            this.Controls.SetChildIndex(this.checkIncludeLocalOrder, 0);
            this.Controls.SetChildIndex(this.txtCentralizedFactory1, 0);
            this.Controls.SetChildIndex(this.txtcountry1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.DateBox dateOutputDateStart;
        private Win.UI.Label label5;
        private Win.UI.DateBox dateOutputDateEnd;
        //private Class.TxtFactory txtFactoryID;
        //private Class.TxtCountry txtCountryID;
        private Win.UI.ComboBox comboExchangeRate;
        private Win.UI.CheckBox checkIncludeLocalOrder;
        private Production.Class.TxtCentralizedFactory txtCentralizedFactory1;
        private Production.Class.Txtcountry txtcountry1;
    }
}
