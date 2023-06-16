namespace Sci.Production.PPIC
{
    partial class R22
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txtseason = new Sci.Production.Class.Txtseason();
            this.txtstyle = new Sci.Production.Class.Txtstyle();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(316, 87);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(316, 12);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(316, 48);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(270, 84);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(296, 91);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(296, 89);
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(115, 12);
            this.txtbrand.MyDocumentdName = null;
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(98, 23);
            this.txtbrand.TabIndex = 105;
            // 
            // txtseason
            // 
            this.txtseason.BackColor = System.Drawing.Color.White;
            this.txtseason.BrandObjectName = null;
            this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason.Location = new System.Drawing.Point(115, 48);
            this.txtseason.Name = "txtseason";
            this.txtseason.Size = new System.Drawing.Size(98, 23);
            this.txtseason.TabIndex = 104;
            // 
            // txtstyle
            // 
            this.txtstyle.BackColor = System.Drawing.Color.White;
            this.txtstyle.BrandObjectName = null;
            this.txtstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle.Location = new System.Drawing.Point(115, 84);
            this.txtstyle.Name = "txtstyle";
            this.txtstyle.SeasonObjectName = null;
            this.txtstyle.Size = new System.Drawing.Size(186, 23);
            this.txtstyle.TabIndex = 103;
            this.txtstyle.TarBrand = null;
            this.txtstyle.TarSeason = null;
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(9, 12);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(102, 23);
            this.labelBrand.TabIndex = 108;
            this.labelBrand.Text = "Master Brand";
            // 
            // labelSeason
            // 
            this.labelSeason.Location = new System.Drawing.Point(9, 48);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(102, 23);
            this.labelSeason.TabIndex = 107;
            this.labelSeason.Text = "Master Season";
            // 
            // labelStyle
            // 
            this.labelStyle.Location = new System.Drawing.Point(9, 84);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(102, 23);
            this.labelStyle.TabIndex = 106;
            this.labelStyle.Text = "Master Style";
            // 
            // R22
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(408, 150);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.txtseason);
            this.Controls.Add(this.txtstyle);
            this.Controls.Add(this.labelBrand);
            this.Controls.Add(this.labelSeason);
            this.Controls.Add(this.labelStyle);
            this.Name = "R22";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R22. Similar Style List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.labelStyle, 0);
            this.Controls.SetChildIndex(this.labelSeason, 0);
            this.Controls.SetChildIndex(this.labelBrand, 0);
            this.Controls.SetChildIndex(this.txtstyle, 0);
            this.Controls.SetChildIndex(this.txtseason, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Class.Txtbrand txtbrand;
        private Class.Txtseason txtseason;
        private Class.Txtstyle txtstyle;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelStyle;
    }
}