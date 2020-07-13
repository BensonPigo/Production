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
            this.labelStyleNo = new Sci.Win.UI.Label();
            this.labelbrand = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.txtstyleStart = new Sci.Production.Class.Txtstyle();
            this.txtstyleEnd = new Sci.Production.Class.Txtstyle();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.label4 = new Sci.Win.UI.Label();
            this.labelLocalMR = new Sci.Win.UI.Label();
            this.txtseason = new Sci.Production.Class.Txtseason();
            this.txtuserLocalMR = new Sci.Production.Class.Txtuser();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(433, 12);
            this.print.TabIndex = 6;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(433, 48);
            this.toexcel.TabIndex = 7;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(433, 84);
            this.close.TabIndex = 8;
            // 
            // labelStyleNo
            // 
            this.labelStyleNo.Location = new System.Drawing.Point(13, 13);
            this.labelStyleNo.Name = "labelStyleNo";
            this.labelStyleNo.Size = new System.Drawing.Size(64, 23);
            this.labelStyleNo.TabIndex = 94;
            this.labelStyleNo.Text = "Style No.";
            // 
            // labelbrand
            // 
            this.labelbrand.Location = new System.Drawing.Point(13, 48);
            this.labelbrand.Name = "labelbrand";
            this.labelbrand.Size = new System.Drawing.Size(64, 23);
            this.labelbrand.TabIndex = 95;
            this.labelbrand.Text = "Brand";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(13, 84);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(64, 23);
            this.labelSeason.TabIndex = 96;
            this.labelSeason.Text = "Season";
            // 
            // txtstyleStart
            // 
            this.txtstyleStart.BackColor = System.Drawing.Color.White;
            this.txtstyleStart.BrandObjectName = null;
            this.txtstyleStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyleStart.IsSupportEditMode = false;
            this.txtstyleStart.Location = new System.Drawing.Point(81, 13);
            this.txtstyleStart.Name = "txtstyleStart";
            this.txtstyleStart.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.NonReadOnly;
            this.txtstyleStart.Size = new System.Drawing.Size(130, 23);
            this.txtstyleStart.TabIndex = 1;
            this.txtstyleStart.TarBrand = null;
            this.txtstyleStart.TarSeason = null;
            // 
            // txtstyleEnd
            // 
            this.txtstyleEnd.BackColor = System.Drawing.Color.White;
            this.txtstyleEnd.BrandObjectName = null;
            this.txtstyleEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyleEnd.IsSupportEditMode = false;
            this.txtstyleEnd.Location = new System.Drawing.Point(235, 13);
            this.txtstyleEnd.Name = "txtstyleEnd";
            this.txtstyleEnd.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.NonReadOnly;
            this.txtstyleEnd.Size = new System.Drawing.Size(130, 23);
            this.txtstyleEnd.TabIndex = 2;
            this.txtstyleEnd.TarBrand = null;
            this.txtstyleEnd.TarSeason = null;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.IsSupportEditMode = false;
            this.txtbrand.Location = new System.Drawing.Point(81, 48);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.NonReadOnly;
            this.txtbrand.Size = new System.Drawing.Size(91, 23);
            this.txtbrand.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
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
            // labelLocalMR
            // 
            this.labelLocalMR.Location = new System.Drawing.Point(13, 119);
            this.labelLocalMR.Name = "labelLocalMR";
            this.labelLocalMR.Size = new System.Drawing.Size(64, 23);
            this.labelLocalMR.TabIndex = 102;
            this.labelLocalMR.Text = "Local MR";
            // 
            // txtseason
            // 
            this.txtseason.BackColor = System.Drawing.Color.White;
            this.txtseason.BrandObjectName = null;
            this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason.Location = new System.Drawing.Point(81, 84);
            this.txtseason.Name = "txtseason";
            this.txtseason.Size = new System.Drawing.Size(80, 23);
            this.txtseason.TabIndex = 4;
            // 
            // txtuserLocalMR
            // 
            this.txtuserLocalMR.DisplayBox1Binding = "";
            this.txtuserLocalMR.Location = new System.Drawing.Point(81, 119);
            this.txtuserLocalMR.Name = "txtuserLocalMR";
            this.txtuserLocalMR.Size = new System.Drawing.Size(300, 23);
            this.txtuserLocalMR.TabIndex = 5;
            this.txtuserLocalMR.TextBox1Binding = "";
            // 
            // P04_Print
            // 
            this.ClientSize = new System.Drawing.Size(533, 179);
            this.Controls.Add(this.txtuserLocalMR);
            this.Controls.Add(this.txtseason);
            this.Controls.Add(this.labelLocalMR);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.txtstyleEnd);
            this.Controls.Add(this.txtstyleStart);
            this.Controls.Add(this.labelSeason);
            this.Controls.Add(this.labelbrand);
            this.Controls.Add(this.labelStyleNo);
            this.DefaultControl = "txtstyleStart";
            this.DefaultControlForEdit = "txtstyleStart";
            this.IsSupportToPrint = false;
            this.Name = "P04_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "Print";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labelStyleNo, 0);
            this.Controls.SetChildIndex(this.labelbrand, 0);
            this.Controls.SetChildIndex(this.labelSeason, 0);
            this.Controls.SetChildIndex(this.txtstyleStart, 0);
            this.Controls.SetChildIndex(this.txtstyleEnd, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.labelLocalMR, 0);
            this.Controls.SetChildIndex(this.txtseason, 0);
            this.Controls.SetChildIndex(this.txtuserLocalMR, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelStyleNo;
        private Win.UI.Label labelbrand;
        private Win.UI.Label labelSeason;
        private Class.Txtstyle txtstyleStart;
        private Class.Txtstyle txtstyleEnd;
        private Class.Txtbrand txtbrand;
        private Win.UI.Label label4;
        private Win.UI.Label labelLocalMR;
        private Class.Txtseason txtseason;
        private Class.Txtuser txtuserLocalMR;
    }
}
