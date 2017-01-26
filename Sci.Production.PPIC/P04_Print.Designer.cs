namespace Sci.Production.PPIC
{
    partial class P04_Print
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
            this.txtstyle1 = new Sci.Production.Class.txtstyle();
            this.txtstyle2 = new Sci.Production.Class.txtstyle();
            this.txtbrand1 = new Sci.Production.Class.txtbrand();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.txtseason1 = new Sci.Production.Class.txtseason();
            this.txtuser1 = new Sci.Production.Class.txtuser();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(418, 12);
            this.print.TabIndex = 6;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(418, 48);
            this.toexcel.TabIndex = 7;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(418, 84);
            this.close.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 23);
            this.label1.TabIndex = 94;
            this.label1.Text = "Style No.";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(13, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 23);
            this.label2.TabIndex = 95;
            this.label2.Text = "Brand";
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(13, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 23);
            this.label3.TabIndex = 96;
            this.label3.Text = "Season";
            // 
            // txtstyle1
            // 
            this.txtstyle1.BackColor = System.Drawing.Color.White;
            this.txtstyle1.BrandObjectName = null;
            this.txtstyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle1.IsSupportEditMode = false;
            this.txtstyle1.Location = new System.Drawing.Point(81, 13);
            this.txtstyle1.Name = "txtstyle1";
            this.txtstyle1.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.NonReadOnly;
            this.txtstyle1.Size = new System.Drawing.Size(130, 23);
            this.txtstyle1.TabIndex = 1;
            // 
            // txtstyle2
            // 
            this.txtstyle2.BackColor = System.Drawing.Color.White;
            this.txtstyle2.BrandObjectName = null;
            this.txtstyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle2.IsSupportEditMode = false;
            this.txtstyle2.Location = new System.Drawing.Point(235, 13);
            this.txtstyle2.Name = "txtstyle2";
            this.txtstyle2.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.NonReadOnly;
            this.txtstyle2.Size = new System.Drawing.Size(130, 23);
            this.txtstyle2.TabIndex = 2;
            // 
            // txtbrand1
            // 
            this.txtbrand1.BackColor = System.Drawing.Color.White;
            this.txtbrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand1.IsSupportEditMode = false;
            this.txtbrand1.Location = new System.Drawing.Point(81, 48);
            this.txtbrand1.Name = "txtbrand1";
            this.txtbrand1.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.NonReadOnly;
            this.txtbrand1.Size = new System.Drawing.Size(91, 23);
            this.txtbrand1.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(214, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(18, 23);
            this.label4.TabIndex = 101;
            this.label4.Text = "～";
            this.label4.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            this.label4.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label4.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(13, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 23);
            this.label5.TabIndex = 102;
            this.label5.Text = "Local MR";
            // 
            // txtseason1
            // 
            this.txtseason1.BackColor = System.Drawing.Color.White;
            this.txtseason1.BrandObjectName = null;
            this.txtseason1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason1.Location = new System.Drawing.Point(81, 84);
            this.txtseason1.Name = "txtseason1";
            this.txtseason1.Size = new System.Drawing.Size(80, 23);
            this.txtseason1.TabIndex = 4;
            // 
            // txtuser1
            // 
            this.txtuser1.DisplayBox1Binding = "";
            this.txtuser1.Location = new System.Drawing.Point(81, 119);
            this.txtuser1.Name = "txtuser1";
            this.txtuser1.Size = new System.Drawing.Size(300, 23);
            this.txtuser1.TabIndex = 5;
            this.txtuser1.TextBox1Binding = "";
            // 
            // P04_Print
            // 
            this.ClientSize = new System.Drawing.Size(510, 179);
            this.Controls.Add(this.txtuser1);
            this.Controls.Add(this.txtseason1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtbrand1);
            this.Controls.Add(this.txtstyle2);
            this.Controls.Add(this.txtstyle1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.DefaultControl = "txtstyle1";
            this.DefaultControlForEdit = "txtstyle1";
            this.IsSupportToPrint = false;
            this.Name = "P04_Print";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.txtstyle1, 0);
            this.Controls.SetChildIndex(this.txtstyle2, 0);
            this.Controls.SetChildIndex(this.txtbrand1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.txtseason1, 0);
            this.Controls.SetChildIndex(this.txtuser1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Class.txtstyle txtstyle1;
        private Class.txtstyle txtstyle2;
        private Class.txtbrand txtbrand1;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Class.txtseason txtseason1;
        private Class.txtuser txtuser1;
    }
}
