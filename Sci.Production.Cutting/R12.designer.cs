namespace Sci.Production.Cutting
{
    partial class R12
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
            this.components = new System.ComponentModel.Container();
            this.txtstyle1 = new Sci.Production.Class.Txtstyle();
            this.txtbrand1 = new Sci.Production.Class.Txtbrand();
            this.txtSeason = new Sci.Win.UI.TextBox();
            this.comboFactory1 = new Sci.Production.Class.ComboFactory(this.components);
            this.comboMDivision1 = new Sci.Production.Class.ComboMDivision(this.components);
            this.labelWKNoETA = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(446, 84);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(446, 12);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(446, 48);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomized.Location = new System.Drawing.Point(300, 84);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkUseCustomized.Location = new System.Drawing.Point(426, 120);
            // 
            // txtVersion
            // 
            this.txtVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVersion.Location = new System.Drawing.Point(326, 48);
            // 
            // txtstyle1
            // 
            this.txtstyle1.BackColor = System.Drawing.Color.White;
            this.txtstyle1.BrandObjectName = null;
            this.txtstyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle1.Location = new System.Drawing.Point(113, 36);
            this.txtstyle1.Name = "txtstyle1";
            this.txtstyle1.Size = new System.Drawing.Size(130, 23);
            this.txtstyle1.TabIndex = 5;
            this.txtstyle1.TarBrand = null;
            this.txtstyle1.TarSeason = null;
            // 
            // txtbrand1
            // 
            this.txtbrand1.BackColor = System.Drawing.Color.White;
            this.txtbrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand1.Location = new System.Drawing.Point(113, 64);
            this.txtbrand1.Name = "txtbrand1";
            this.txtbrand1.Size = new System.Drawing.Size(66, 23);
            this.txtbrand1.TabIndex = 6;
            // 
            // txtSeason
            // 
            this.txtSeason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtSeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtSeason.IsSupportEditMode = false;
            this.txtSeason.Location = new System.Drawing.Point(113, 9);
            this.txtSeason.MaxLength = 20;
            this.txtSeason.Name = "txtSeason";
            this.txtSeason.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtSeason.ReadOnly = true;
            this.txtSeason.Size = new System.Drawing.Size(228, 23);
            this.txtSeason.TabIndex = 138;
            this.txtSeason.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtSeason_PopUp);
            // 
            // comboFactory1
            // 
            this.comboFactory1.BackColor = System.Drawing.Color.White;
            this.comboFactory1.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboFactory1.FilteMDivision = false;
            this.comboFactory1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory1.FormattingEnabled = true;
            this.comboFactory1.IssupportJunk = false;
            this.comboFactory1.IsSupportUnselect = true;
            this.comboFactory1.Location = new System.Drawing.Point(113, 120);
            this.comboFactory1.Name = "comboFactory1";
            this.comboFactory1.OldText = "";
            this.comboFactory1.Size = new System.Drawing.Size(95, 24);
            this.comboFactory1.TabIndex = 139;
            // 
            // comboMDivision1
            // 
            this.comboMDivision1.BackColor = System.Drawing.Color.White;
            this.comboMDivision1.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboMDivision1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMDivision1.FormattingEnabled = true;
            this.comboMDivision1.IsSupportUnselect = true;
            this.comboMDivision1.Location = new System.Drawing.Point(113, 92);
            this.comboMDivision1.Name = "comboMDivision1";
            this.comboMDivision1.OldText = "";
            this.comboMDivision1.Size = new System.Drawing.Size(95, 24);
            this.comboMDivision1.TabIndex = 140;
            // 
            // labelWKNoETA
            // 
            this.labelWKNoETA.Location = new System.Drawing.Point(9, 9);
            this.labelWKNoETA.Name = "labelWKNoETA";
            this.labelWKNoETA.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelWKNoETA.RectStyle.BorderWidth = 1F;
            this.labelWKNoETA.RectStyle.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.labelWKNoETA.RectStyle.ExtBorderWidth = 1F;
            this.labelWKNoETA.Size = new System.Drawing.Size(101, 23);
            this.labelWKNoETA.TabIndex = 141;
            this.labelWKNoETA.Text = "Season";
            this.labelWKNoETA.TextStyle.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelWKNoETA.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(9, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 23);
            this.label6.TabIndex = 146;
            this.label6.Text = "Style";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 23);
            this.label4.TabIndex = 147;
            this.label4.Text = "Brand";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 92);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 23);
            this.label1.TabIndex = 148;
            this.label1.Text = "M";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 120);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 23);
            this.label2.TabIndex = 149;
            this.label2.Text = "Factory";
            // 
            // R12
            // 
            this.ClientSize = new System.Drawing.Size(538, 177);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.labelWKNoETA);
            this.Controls.Add(this.comboMDivision1);
            this.Controls.Add(this.comboFactory1);
            this.Controls.Add(this.txtSeason);
            this.Controls.Add(this.txtbrand1);
            this.Controls.Add(this.txtstyle1);
            this.Name = "R12";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R12. Marker List Detail";
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.txtstyle1, 0);
            this.Controls.SetChildIndex(this.txtbrand1, 0);
            this.Controls.SetChildIndex(this.txtSeason, 0);
            this.Controls.SetChildIndex(this.comboFactory1, 0);
            this.Controls.SetChildIndex(this.comboMDivision1, 0);
            this.Controls.SetChildIndex(this.labelWKNoETA, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Class.Txtstyle txtstyle1;
        private Class.Txtbrand txtbrand1;
        private Win.UI.TextBox txtSeason;
        private Class.ComboFactory comboFactory1;
        private Class.ComboMDivision comboMDivision1;
        private Win.UI.Label labelWKNoETA;
        private Win.UI.Label label6;
        private Win.UI.Label label4;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
    }
}
