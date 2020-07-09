namespace Sci.Production.Warehouse
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
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.dateSuppDelivery = new Sci.Win.UI.DateRange();
            this.labelSuppDelivery = new Sci.Win.UI.Label();
            this.txtSPNoEnd = new Sci.Win.UI.TextBox();
            this.txtSPNoStart = new Sci.Win.UI.TextBox();
            this.label10 = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.txtRefnoEnd = new Sci.Win.UI.TextBox();
            this.txtRefnoStart = new Sci.Win.UI.TextBox();
            this.label12 = new Sci.Win.UI.Label();
            this.datelabelFinalETA = new Sci.Win.UI.DateRange();
            this.labelFinalETA = new Sci.Win.UI.Label();
            this.dateETA = new Sci.Win.UI.DateRange();
            this.labelETA = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelFabricType = new Sci.Win.UI.Label();
            this.txtMdivision = new Sci.Production.Class.TxtMdivision();
            this.labelM = new Sci.Win.UI.Label();
            this.txtseason = new Sci.Production.Class.Txtseason();
            this.comboFabricType = new System.Windows.Forms.ComboBox();
            this.labelCountry = new Sci.Win.UI.Label();
            this.txtcountry = new Sci.Production.Class.Txtcountry();
            this.labelRefno = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.txtstyle = new Sci.Production.Class.Txtstyle();
            this.txtsupplier = new Sci.Production.Class.Txtsupplier();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.comboOrderBy = new Sci.Win.UI.ComboBox();
            this.labelOrderBy = new Sci.Win.UI.Label();
            this.checkIncludeCompleteItem = new Sci.Win.UI.CheckBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.txtfactory = new Sci.Production.Class.Txtfactory();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(431, 12);
            this.print.TabIndex = 16;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(431, 48);
            this.toexcel.TabIndex = 17;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(431, 84);
            this.close.TabIndex = 18;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Lines = 0;
            this.labelSCIDelivery.Location = new System.Drawing.Point(13, 12);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSCIDelivery.RectStyle.BorderWidth = 1F;
            this.labelSCIDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSCIDelivery.RectStyle.ExtBorderWidth = 1F;
            this.labelSCIDelivery.Size = new System.Drawing.Size(98, 23);
            this.labelSCIDelivery.TabIndex = 96;
            this.labelSCIDelivery.Text = "SCI Delivery";
            this.labelSCIDelivery.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSCIDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateSCIDelivery
            // 
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(115, 12);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 0;
            // 
            // dateSuppDelivery
            // 
            this.dateSuppDelivery.IsRequired = false;
            this.dateSuppDelivery.Location = new System.Drawing.Point(115, 48);
            this.dateSuppDelivery.Name = "dateSuppDelivery";
            this.dateSuppDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSuppDelivery.TabIndex = 1;
            // 
            // labelSuppDelivery
            // 
            this.labelSuppDelivery.Lines = 0;
            this.labelSuppDelivery.Location = new System.Drawing.Point(13, 48);
            this.labelSuppDelivery.Name = "labelSuppDelivery";
            this.labelSuppDelivery.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSuppDelivery.RectStyle.BorderWidth = 1F;
            this.labelSuppDelivery.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSuppDelivery.RectStyle.ExtBorderWidth = 1F;
            this.labelSuppDelivery.Size = new System.Drawing.Size(98, 23);
            this.labelSuppDelivery.TabIndex = 105;
            this.labelSuppDelivery.Text = "Supp Delivery";
            this.labelSuppDelivery.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSuppDelivery.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtSPNoEnd
            // 
            this.txtSPNoEnd.BackColor = System.Drawing.Color.White;
            this.txtSPNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoEnd.Location = new System.Drawing.Point(267, 155);
            this.txtSPNoEnd.Name = "txtSPNoEnd";
            this.txtSPNoEnd.Size = new System.Drawing.Size(128, 23);
            this.txtSPNoEnd.TabIndex = 5;
            // 
            // txtSPNoStart
            // 
            this.txtSPNoStart.BackColor = System.Drawing.Color.White;
            this.txtSPNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNoStart.Location = new System.Drawing.Point(114, 155);
            this.txtSPNoStart.Name = "txtSPNoStart";
            this.txtSPNoStart.Size = new System.Drawing.Size(128, 23);
            this.txtSPNoStart.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.Lines = 0;
            this.label10.Location = new System.Drawing.Point(245, 155);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(18, 23);
            this.label10.TabIndex = 115;
            this.label10.Text = "～";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Lines = 0;
            this.labelSPNo.Location = new System.Drawing.Point(13, 155);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSPNo.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelSPNo.Size = new System.Drawing.Size(98, 23);
            this.labelSPNo.TabIndex = 114;
            this.labelSPNo.Text = "SP#";
            this.labelSPNo.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSPNo.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtRefnoEnd
            // 
            this.txtRefnoEnd.BackColor = System.Drawing.Color.White;
            this.txtRefnoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefnoEnd.Location = new System.Drawing.Point(303, 191);
            this.txtRefnoEnd.Name = "txtRefnoEnd";
            this.txtRefnoEnd.Size = new System.Drawing.Size(164, 23);
            this.txtRefnoEnd.TabIndex = 7;
            // 
            // txtRefnoStart
            // 
            this.txtRefnoStart.BackColor = System.Drawing.Color.White;
            this.txtRefnoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefnoStart.Location = new System.Drawing.Point(114, 191);
            this.txtRefnoStart.Name = "txtRefnoStart";
            this.txtRefnoStart.Size = new System.Drawing.Size(164, 23);
            this.txtRefnoStart.TabIndex = 6;
            // 
            // label12
            // 
            this.label12.Lines = 0;
            this.label12.Location = new System.Drawing.Point(281, 191);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(18, 23);
            this.label12.TabIndex = 123;
            this.label12.Text = "～";
            // 
            // datelabelFinalETA
            // 
            this.datelabelFinalETA.IsRequired = false;
            this.datelabelFinalETA.Location = new System.Drawing.Point(115, 120);
            this.datelabelFinalETA.Name = "datelabelFinalETA";
            this.datelabelFinalETA.Size = new System.Drawing.Size(280, 23);
            this.datelabelFinalETA.TabIndex = 3;
            // 
            // labelFinalETA
            // 
            this.labelFinalETA.Lines = 0;
            this.labelFinalETA.Location = new System.Drawing.Point(13, 120);
            this.labelFinalETA.Name = "labelFinalETA";
            this.labelFinalETA.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelFinalETA.RectStyle.BorderWidth = 1F;
            this.labelFinalETA.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelFinalETA.RectStyle.ExtBorderWidth = 1F;
            this.labelFinalETA.Size = new System.Drawing.Size(98, 23);
            this.labelFinalETA.TabIndex = 128;
            this.labelFinalETA.Text = "Final ETA";
            this.labelFinalETA.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelFinalETA.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateETA
            // 
            this.dateETA.IsRequired = false;
            this.dateETA.Location = new System.Drawing.Point(115, 84);
            this.dateETA.Name = "dateETA";
            this.dateETA.Size = new System.Drawing.Size(280, 23);
            this.dateETA.TabIndex = 2;
            // 
            // labelETA
            // 
            this.labelETA.Lines = 0;
            this.labelETA.Location = new System.Drawing.Point(13, 84);
            this.labelETA.Name = "labelETA";
            this.labelETA.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelETA.RectStyle.BorderWidth = 1F;
            this.labelETA.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelETA.RectStyle.ExtBorderWidth = 1F;
            this.labelETA.Size = new System.Drawing.Size(98, 23);
            this.labelETA.TabIndex = 127;
            this.labelETA.Text = "ETA";
            this.labelETA.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelETA.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelSeason
            // 
            this.labelSeason.Lines = 0;
            this.labelSeason.Location = new System.Drawing.Point(13, 265);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(98, 23);
            this.labelSeason.TabIndex = 95;
            this.labelSeason.Text = "Season";
            // 
            // labelFabricType
            // 
            this.labelFabricType.Lines = 0;
            this.labelFabricType.Location = new System.Drawing.Point(13, 433);
            this.labelFabricType.Name = "labelFabricType";
            this.labelFabricType.Size = new System.Drawing.Size(98, 23);
            this.labelFabricType.TabIndex = 98;
            this.labelFabricType.Text = "Fabric Type";
            // 
            // txtMdivision
            // 
            this.txtMdivision.BackColor = System.Drawing.Color.White;
            this.txtMdivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMdivision.Location = new System.Drawing.Point(115, 373);
            this.txtMdivision.Name = "txtMdivision";
            this.txtMdivision.Size = new System.Drawing.Size(66, 23);
            this.txtMdivision.TabIndex = 12;
            // 
            // labelM
            // 
            this.labelM.Lines = 0;
            this.labelM.Location = new System.Drawing.Point(13, 372);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(98, 23);
            this.labelM.TabIndex = 103;
            this.labelM.Text = "M";
            // 
            // txtseason
            // 
            this.txtseason.BackColor = System.Drawing.Color.White;
            this.txtseason.BrandObjectName = null;
            this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason.Location = new System.Drawing.Point(115, 265);
            this.txtseason.Name = "txtseason";
            this.txtseason.Size = new System.Drawing.Size(80, 23);
            this.txtseason.TabIndex = 9;
            // 
            // comboFabricType
            // 
            this.comboFabricType.FormattingEnabled = true;
            this.comboFabricType.Location = new System.Drawing.Point(114, 433);
            this.comboFabricType.Name = "comboFabricType";
            this.comboFabricType.Size = new System.Drawing.Size(121, 24);
            this.comboFabricType.TabIndex = 13;
            // 
            // labelCountry
            // 
            this.labelCountry.Lines = 0;
            this.labelCountry.Location = new System.Drawing.Point(13, 301);
            this.labelCountry.Name = "labelCountry";
            this.labelCountry.Size = new System.Drawing.Size(98, 23);
            this.labelCountry.TabIndex = 113;
            this.labelCountry.Text = "Country";
            // 
            // txtcountry
            // 
            this.txtcountry.DisplayBox1Binding = "";
            this.txtcountry.Location = new System.Drawing.Point(115, 301);
            this.txtcountry.Name = "txtcountry";
            this.txtcountry.Size = new System.Drawing.Size(232, 30);
            this.txtcountry.TabIndex = 10;
            this.txtcountry.TextBox1Binding = "";
            // 
            // labelRefno
            // 
            this.labelRefno.Lines = 0;
            this.labelRefno.Location = new System.Drawing.Point(13, 191);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.RectStyle.BorderColor = System.Drawing.Color.Black;
            this.labelRefno.RectStyle.Color = System.Drawing.Color.SkyBlue;
            this.labelRefno.Size = new System.Drawing.Size(98, 23);
            this.labelRefno.TabIndex = 130;
            this.labelRefno.Text = "Refno";
            this.labelRefno.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelRefno.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // labelStyle
            // 
            this.labelStyle.Lines = 0;
            this.labelStyle.Location = new System.Drawing.Point(13, 227);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(98, 23);
            this.labelStyle.TabIndex = 132;
            this.labelStyle.Text = "Style";
            // 
            // txtstyle
            // 
            this.txtstyle.BackColor = System.Drawing.Color.White;
            this.txtstyle.BrandObjectName = null;
            this.txtstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle.Location = new System.Drawing.Point(114, 227);
            this.txtstyle.Name = "txtstyle";
            this.txtstyle.Size = new System.Drawing.Size(130, 23);
            this.txtstyle.TabIndex = 8;
            // 
            // txtsupplier
            // 
            this.txtsupplier.DisplayBox1Binding = "";
            this.txtsupplier.Location = new System.Drawing.Point(114, 337);
            this.txtsupplier.Name = "txtsupplier";
            this.txtsupplier.Size = new System.Drawing.Size(147, 23);
            this.txtsupplier.TabIndex = 11;
            this.txtsupplier.TextBox1Binding = "";
            // 
            // labelSupplier
            // 
            this.labelSupplier.Lines = 0;
            this.labelSupplier.Location = new System.Drawing.Point(13, 337);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(98, 23);
            this.labelSupplier.TabIndex = 135;
            this.labelSupplier.Text = "Supplier";
            // 
            // comboOrderBy
            // 
            this.comboOrderBy.BackColor = System.Drawing.Color.White;
            this.comboOrderBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboOrderBy.FormattingEnabled = true;
            this.comboOrderBy.IsSupportUnselect = true;
            this.comboOrderBy.Items.AddRange(new object[] {
            "Issue Date",
            "Supplier"});
            this.comboOrderBy.Location = new System.Drawing.Point(114, 463);
            this.comboOrderBy.Name = "comboOrderBy";
            this.comboOrderBy.Size = new System.Drawing.Size(121, 24);
            this.comboOrderBy.TabIndex = 14;
            // 
            // labelOrderBy
            // 
            this.labelOrderBy.Lines = 0;
            this.labelOrderBy.Location = new System.Drawing.Point(13, 464);
            this.labelOrderBy.Name = "labelOrderBy";
            this.labelOrderBy.Size = new System.Drawing.Size(98, 23);
            this.labelOrderBy.TabIndex = 137;
            this.labelOrderBy.Text = "Order By";
            // 
            // checkIncludeCompleteItem
            // 
            this.checkIncludeCompleteItem.AutoSize = true;
            this.checkIncludeCompleteItem.Checked = true;
            this.checkIncludeCompleteItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkIncludeCompleteItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIncludeCompleteItem.Location = new System.Drawing.Point(267, 466);
            this.checkIncludeCompleteItem.Name = "checkIncludeCompleteItem";
            this.checkIncludeCompleteItem.Size = new System.Drawing.Size(163, 21);
            this.checkIncludeCompleteItem.TabIndex = 15;
            this.checkIncludeCompleteItem.Text = "Include complete item";
            this.checkIncludeCompleteItem.UseVisualStyleBackColor = true;
            // 
            // labelFactory
            // 
            this.labelFactory.Lines = 0;
            this.labelFactory.Location = new System.Drawing.Point(13, 402);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(98, 23);
            this.labelFactory.TabIndex = 138;
            this.labelFactory.Text = "Factory";
            // 
            // txtfactory
            // 
            this.txtfactory.BackColor = System.Drawing.Color.White;
            this.txtfactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtfactory.Location = new System.Drawing.Point(114, 402);
            this.txtfactory.Name = "txtfactory";
            this.txtfactory.Size = new System.Drawing.Size(66, 23);
            this.txtfactory.TabIndex = 139;
            this.txtfactory.IssupportJunk = true;
            // 
            // R01
            // 
            this.ClientSize = new System.Drawing.Size(523, 521);
            this.Controls.Add(this.txtfactory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.checkIncludeCompleteItem);
            this.Controls.Add(this.comboOrderBy);
            this.Controls.Add(this.labelOrderBy);
            this.Controls.Add(this.labelSupplier);
            this.Controls.Add(this.txtsupplier);
            this.Controls.Add(this.txtstyle);
            this.Controls.Add(this.labelStyle);
            this.Controls.Add(this.labelRefno);
            this.Controls.Add(this.txtcountry);
            this.Controls.Add(this.datelabelFinalETA);
            this.Controls.Add(this.labelFinalETA);
            this.Controls.Add(this.dateETA);
            this.Controls.Add(this.labelETA);
            this.Controls.Add(this.comboFabricType);
            this.Controls.Add(this.txtRefnoEnd);
            this.Controls.Add(this.txtRefnoStart);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtseason);
            this.Controls.Add(this.txtSPNoEnd);
            this.Controls.Add(this.txtSPNoStart);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.labelSPNo);
            this.Controls.Add(this.labelCountry);
            this.Controls.Add(this.dateSuppDelivery);
            this.Controls.Add(this.labelSuppDelivery);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.txtMdivision);
            this.Controls.Add(this.dateSCIDelivery);
            this.Controls.Add(this.labelFabricType);
            this.Controls.Add(this.labelSCIDelivery);
            this.Controls.Add(this.labelSeason);
            this.IsSupportToPrint = false;
            this.Name = "R01";
            this.Text = "R01. 3rd Country Material Status";
            this.Controls.SetChildIndex(this.labelSeason, 0);
            this.Controls.SetChildIndex(this.labelSCIDelivery, 0);
            this.Controls.SetChildIndex(this.labelFabricType, 0);
            this.Controls.SetChildIndex(this.dateSCIDelivery, 0);
            this.Controls.SetChildIndex(this.txtMdivision, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelSuppDelivery, 0);
            this.Controls.SetChildIndex(this.dateSuppDelivery, 0);
            this.Controls.SetChildIndex(this.labelCountry, 0);
            this.Controls.SetChildIndex(this.labelSPNo, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.txtSPNoStart, 0);
            this.Controls.SetChildIndex(this.txtSPNoEnd, 0);
            this.Controls.SetChildIndex(this.txtseason, 0);
            this.Controls.SetChildIndex(this.label12, 0);
            this.Controls.SetChildIndex(this.txtRefnoStart, 0);
            this.Controls.SetChildIndex(this.txtRefnoEnd, 0);
            this.Controls.SetChildIndex(this.comboFabricType, 0);
            this.Controls.SetChildIndex(this.labelETA, 0);
            this.Controls.SetChildIndex(this.dateETA, 0);
            this.Controls.SetChildIndex(this.labelFinalETA, 0);
            this.Controls.SetChildIndex(this.datelabelFinalETA, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.txtcountry, 0);
            this.Controls.SetChildIndex(this.labelRefno, 0);
            this.Controls.SetChildIndex(this.labelStyle, 0);
            this.Controls.SetChildIndex(this.txtstyle, 0);
            this.Controls.SetChildIndex(this.txtsupplier, 0);
            this.Controls.SetChildIndex(this.labelSupplier, 0);
            this.Controls.SetChildIndex(this.labelOrderBy, 0);
            this.Controls.SetChildIndex(this.comboOrderBy, 0);
            this.Controls.SetChildIndex(this.checkIncludeCompleteItem, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.txtfactory, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelSCIDelivery;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.DateRange dateSuppDelivery;
        private Win.UI.Label labelSuppDelivery;
        private Win.UI.TextBox txtSPNoEnd;
        private Win.UI.TextBox txtSPNoStart;
        private Win.UI.Label label10;
        private Win.UI.Label labelSPNo;
        private Win.UI.TextBox txtRefnoEnd;
        private Win.UI.TextBox txtRefnoStart;
        private Win.UI.Label label12;
        private Win.UI.DateRange datelabelFinalETA;
        private Win.UI.Label labelFinalETA;
        private Win.UI.DateRange dateETA;
        private Win.UI.Label labelETA;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelFabricType;
        private Class.TxtMdivision txtMdivision;
        private Win.UI.Label labelM;
        private Class.Txtseason txtseason;
        private System.Windows.Forms.ComboBox comboFabricType;
        private Win.UI.Label labelCountry;
        private Class.Txtcountry txtcountry;
        private Win.UI.Label labelRefno;
        private Win.UI.Label labelStyle;
        private Class.Txtstyle txtstyle;
        private Class.Txtsupplier txtsupplier;
        private Win.UI.Label labelSupplier;
        private Win.UI.ComboBox comboOrderBy;
        private Win.UI.Label labelOrderBy;
        private Win.UI.CheckBox checkIncludeCompleteItem;
        private Win.UI.Label labelFactory;
        private Class.Txtfactory txtfactory;
    }
}
