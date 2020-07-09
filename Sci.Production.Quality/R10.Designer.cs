namespace Sci.Production.Quality
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.labTypeofPrint = new Sci.Win.UI.Label();
            this.labStyle = new Sci.Win.UI.Label();
            this.labBrand = new Sci.Win.UI.Label();
            this.labSeason = new Sci.Win.UI.Label();
            this.labT1SubconName = new Sci.Win.UI.Label();
            this.labFabricRefNo = new Sci.Win.UI.Label();
            this.labT2SubconName = new Sci.Win.UI.Label();
            this.comboBoxTypeofPrint = new Sci.Win.UI.ComboBox();
            this.txtCombineStyle = new Sci.Win.UI.TextBox();
            this.txtseason = new Sci.Production.Class.Txtseason();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txtLocalSupp = new Sci.Production.Class.TxtLocalSuppNoConfirm();
            this.txtLocalTPESupp = new Sci.Production.Class.TxtLocalTPESuppNoConfirm();
            this.txtFabRefno = new Sci.Win.UI.TextBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(535, 148);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(535, 10);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(535, 46);
            // 
            // labTypeofPrint
            // 
            this.labTypeofPrint.Location = new System.Drawing.Point(21, 16);
            this.labTypeofPrint.Name = "labTypeofPrint";
            this.labTypeofPrint.Size = new System.Drawing.Size(119, 23);
            this.labTypeofPrint.TabIndex = 94;
            this.labTypeofPrint.Text = "Type of Testing";
            // 
            // labStyle
            // 
            this.labStyle.Location = new System.Drawing.Point(21, 46);
            this.labStyle.Name = "labStyle";
            this.labStyle.Size = new System.Drawing.Size(119, 23);
            this.labStyle.TabIndex = 95;
            this.labStyle.Text = "Style";
            // 
            // labBrand
            // 
            this.labBrand.Location = new System.Drawing.Point(21, 106);
            this.labBrand.Name = "labBrand";
            this.labBrand.Size = new System.Drawing.Size(119, 23);
            this.labBrand.TabIndex = 97;
            this.labBrand.Text = "Brand";
            // 
            // labSeason
            // 
            this.labSeason.Location = new System.Drawing.Point(21, 76);
            this.labSeason.Name = "labSeason";
            this.labSeason.Size = new System.Drawing.Size(119, 23);
            this.labSeason.TabIndex = 96;
            this.labSeason.Text = "Season";
            // 
            // labT1SubconName
            // 
            this.labT1SubconName.Location = new System.Drawing.Point(21, 166);
            this.labT1SubconName.Name = "labT1SubconName";
            this.labT1SubconName.Size = new System.Drawing.Size(119, 23);
            this.labT1SubconName.TabIndex = 99;
            this.labT1SubconName.Text = "T1/Subcon Name";
            // 
            // labFabricRefNo
            // 
            this.labFabricRefNo.Location = new System.Drawing.Point(21, 136);
            this.labFabricRefNo.Name = "labFabricRefNo";
            this.labFabricRefNo.Size = new System.Drawing.Size(119, 23);
            this.labFabricRefNo.TabIndex = 98;
            this.labFabricRefNo.Text = "Fabric Ref No.";
            // 
            // labT2SubconName
            // 
            this.labT2SubconName.Location = new System.Drawing.Point(21, 196);
            this.labT2SubconName.Name = "labT2SubconName";
            this.labT2SubconName.Size = new System.Drawing.Size(119, 23);
            this.labT2SubconName.TabIndex = 101;
            this.labT2SubconName.Text = "T2/Subcon Name";
            // 
            // comboBoxTypeofPrint
            // 
            this.comboBoxTypeofPrint.BackColor = System.Drawing.Color.White;
            this.comboBoxTypeofPrint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboBoxTypeofPrint.FormattingEnabled = true;
            this.comboBoxTypeofPrint.IsSupportUnselect = true;
            this.comboBoxTypeofPrint.Location = new System.Drawing.Point(143, 15);
            this.comboBoxTypeofPrint.Name = "comboBoxTypeofPrint";
            this.comboBoxTypeofPrint.OldText = "";
            this.comboBoxTypeofPrint.Size = new System.Drawing.Size(121, 24);
            this.comboBoxTypeofPrint.TabIndex = 102;
            this.comboBoxTypeofPrint.SelectedValueChanged += new System.EventHandler(this.comboBoxTypeofPrint_SelectedValueChanged);
            // 
            // txtCombineStyle
            // 
            this.txtCombineStyle.BackColor = System.Drawing.Color.White;
            this.txtCombineStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCombineStyle.Location = new System.Drawing.Point(143, 46);
            this.txtCombineStyle.MaxLength = 120;
            this.txtCombineStyle.Name = "txtCombineStyle";
            this.txtCombineStyle.Size = new System.Drawing.Size(252, 23);
            this.txtCombineStyle.TabIndex = 103;
            this.txtCombineStyle.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtCombineStyle_PopUp);
            this.txtCombineStyle.Validating += new System.ComponentModel.CancelEventHandler(this.txtCombineStyle_Validating);
            // 
            // txtseason
            // 
            this.txtseason.BackColor = System.Drawing.Color.White;
            this.txtseason.BrandObjectName = null;
            this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason.Location = new System.Drawing.Point(143, 75);
            this.txtseason.Name = "txtseason";
            this.txtseason.Size = new System.Drawing.Size(121, 23);
            this.txtseason.TabIndex = 104;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(143, 106);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(121, 23);
            this.txtbrand.TabIndex = 105;
            // 
            // txtLocalSupp
            // 
            this.txtLocalSupp.DisplayBox1Binding = "";
            this.txtLocalSupp.Location = new System.Drawing.Point(143, 166);
            this.txtLocalSupp.Name = "txtLocalSupp";
            this.txtLocalSupp.Size = new System.Drawing.Size(252, 23);
            this.txtLocalSupp.TabIndex = 106;
            this.txtLocalSupp.TextBox1Binding = "";
            // 
            // txtLocalTPESupp
            // 
            this.txtLocalTPESupp.DisplayBox1Binding = "";
            this.txtLocalTPESupp.IsIncludeJunk = false;
            this.txtLocalTPESupp.Location = new System.Drawing.Point(143, 196);
            this.txtLocalTPESupp.Name = "txtLocalTPESupp";
            this.txtLocalTPESupp.Size = new System.Drawing.Size(252, 23);
            this.txtLocalTPESupp.TabIndex = 107;
            this.txtLocalTPESupp.TextBox1Binding = "";
            // 
            // txtFabRefno
            // 
            this.txtFabRefno.BackColor = System.Drawing.Color.White;
            this.txtFabRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFabRefno.Location = new System.Drawing.Point(143, 136);
            this.txtFabRefno.Name = "txtFabRefno";
            this.txtFabRefno.Size = new System.Drawing.Size(252, 23);
            this.txtFabRefno.TabIndex = 108;
            // 
            // R10
            // 
            this.ClientSize = new System.Drawing.Size(627, 253);
            this.Controls.Add(this.txtFabRefno);
            this.Controls.Add(this.txtLocalTPESupp);
            this.Controls.Add(this.txtLocalSupp);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.txtseason);
            this.Controls.Add(this.txtCombineStyle);
            this.Controls.Add(this.comboBoxTypeofPrint);
            this.Controls.Add(this.labT2SubconName);
            this.Controls.Add(this.labT1SubconName);
            this.Controls.Add(this.labFabricRefNo);
            this.Controls.Add(this.labBrand);
            this.Controls.Add(this.labSeason);
            this.Controls.Add(this.labStyle);
            this.Controls.Add(this.labTypeofPrint);
            this.Name = "R10";
            this.Text = "R10. Mockup Testing Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.labTypeofPrint, 0);
            this.Controls.SetChildIndex(this.labStyle, 0);
            this.Controls.SetChildIndex(this.labSeason, 0);
            this.Controls.SetChildIndex(this.labBrand, 0);
            this.Controls.SetChildIndex(this.labFabricRefNo, 0);
            this.Controls.SetChildIndex(this.labT1SubconName, 0);
            this.Controls.SetChildIndex(this.labT2SubconName, 0);
            this.Controls.SetChildIndex(this.comboBoxTypeofPrint, 0);
            this.Controls.SetChildIndex(this.txtCombineStyle, 0);
            this.Controls.SetChildIndex(this.txtseason, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.Controls.SetChildIndex(this.txtLocalSupp, 0);
            this.Controls.SetChildIndex(this.txtLocalTPESupp, 0);
            this.Controls.SetChildIndex(this.txtFabRefno, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labTypeofPrint;
        private Win.UI.Label labStyle;
        private Win.UI.Label labBrand;
        private Win.UI.Label labSeason;
        private Win.UI.Label labT1SubconName;
        private Win.UI.Label labFabricRefNo;
        private Win.UI.Label labT2SubconName;
        private Win.UI.ComboBox comboBoxTypeofPrint;
        private Win.UI.TextBox txtCombineStyle;
        private Class.Txtseason txtseason;
        private Class.Txtbrand txtbrand;
        private Class.TxtLocalSuppNoConfirm txtLocalSupp;
        private Class.TxtLocalTPESuppNoConfirm txtLocalTPESupp;
        private Win.UI.TextBox txtFabRefno;
    }
}
