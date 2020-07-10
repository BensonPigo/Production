namespace Sci.Production.Packing
{
    partial class P04_BatchImport
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
            this.components = new System.ComponentModel.Container();
            this.panel1 = new Sci.Win.UI.Panel();
            this.panel2 = new Sci.Win.UI.Panel();
            this.panel3 = new Sci.Win.UI.Panel();
            this.btnFind = new Sci.Win.UI.Button();
            this.txtLocateSPNo = new Sci.Win.UI.TextBox();
            this.btnQuery = new Sci.Win.UI.Button();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.txtcountryDestination = new Sci.Production.Class.Txtcountry();
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.labelDestination = new Sci.Win.UI.Label();
            this.txtdropdownlistBuyMonth = new Sci.Production.Class.Txtdropdownlist();
            this.txtseason = new Sci.Production.Class.Txtseason();
            this.displayShipMode = new Sci.Win.UI.DisplayBox();
            this.labelBuyMonth = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelShipMode = new Sci.Win.UI.Label();
            this.txtOrderType = new Sci.Win.UI.TextBox();
            this.txtcustcd = new Sci.Production.Class.Txtcustcd();
            this.displayBrand = new Sci.Win.UI.DisplayBox();
            this.labelLocateSPNo = new Sci.Win.UI.Label();
            this.labelOrderType = new Sci.Win.UI.Label();
            this.labelCustCD = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.lineShape1 = new Microsoft.VisualBasic.PowerPacks.LineShape();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridDetail = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 555);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(901, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 555);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnFind);
            this.panel3.Controls.Add(this.txtLocateSPNo);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.dateBuyerDelivery);
            this.panel3.Controls.Add(this.txtcountryDestination);
            this.panel3.Controls.Add(this.labelBuyerDelivery);
            this.panel3.Controls.Add(this.labelDestination);
            this.panel3.Controls.Add(this.txtdropdownlistBuyMonth);
            this.panel3.Controls.Add(this.txtseason);
            this.panel3.Controls.Add(this.displayShipMode);
            this.panel3.Controls.Add(this.labelBuyMonth);
            this.panel3.Controls.Add(this.labelSeason);
            this.panel3.Controls.Add(this.labelShipMode);
            this.panel3.Controls.Add(this.txtOrderType);
            this.panel3.Controls.Add(this.txtcustcd);
            this.panel3.Controls.Add(this.displayBrand);
            this.panel3.Controls.Add(this.labelLocateSPNo);
            this.panel3.Controls.Add(this.labelOrderType);
            this.panel3.Controls.Add(this.labelCustCD);
            this.panel3.Controls.Add(this.labelBrand);
            this.panel3.Controls.Add(this.shapeContainer1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(896, 132);
            this.panel3.TabIndex = 2;
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(226, 97);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(80, 30);
            this.btnFind.TabIndex = 19;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // txtLocateSPNo
            // 
            this.txtLocateSPNo.BackColor = System.Drawing.Color.White;
            this.txtLocateSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocateSPNo.Location = new System.Drawing.Point(101, 101);
            this.txtLocateSPNo.Name = "txtLocateSPNo";
            this.txtLocateSPNo.Size = new System.Drawing.Size(120, 23);
            this.txtLocateSPNo.TabIndex = 18;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(809, 9);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 17;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // dateBuyerDelivery
            // 
            this.dateBuyerDelivery.IsRequired = false;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(528, 38);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 16;
            // 
            // txtcountryDestination
            // 
            this.txtcountryDestination.DisplayBox1Binding = "";
            this.txtcountryDestination.Location = new System.Drawing.Point(528, 11);
            this.txtcountryDestination.Name = "txtcountryDestination";
            this.txtcountryDestination.Size = new System.Drawing.Size(232, 22);
            this.txtcountryDestination.TabIndex = 15;
            this.txtcountryDestination.TextBox1Binding = "";
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Lines = 0;
            this.labelBuyerDelivery.Location = new System.Drawing.Point(429, 38);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(95, 23);
            this.labelBuyerDelivery.TabIndex = 14;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            // 
            // labelDestination
            // 
            this.labelDestination.Lines = 0;
            this.labelDestination.Location = new System.Drawing.Point(429, 11);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(95, 23);
            this.labelDestination.TabIndex = 13;
            this.labelDestination.Text = "Destination";
            // 
            // txtdropdownlistBuyMonth
            // 
            this.txtdropdownlistBuyMonth.BackColor = System.Drawing.Color.White;
            this.txtdropdownlistBuyMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtdropdownlistBuyMonth.FormattingEnabled = true;
            this.txtdropdownlistBuyMonth.IsSupportUnselect = true;
            this.txtdropdownlistBuyMonth.Location = new System.Drawing.Point(301, 64);
            this.txtdropdownlistBuyMonth.Name = "txtdropdownlistBuyMonth";
            this.txtdropdownlistBuyMonth.Size = new System.Drawing.Size(121, 24);
            this.txtdropdownlistBuyMonth.TabIndex = 12;
            this.txtdropdownlistBuyMonth.Type = "BuyMonth";
            // 
            // txtseason
            // 
            this.txtseason.BackColor = System.Drawing.Color.White;
            this.txtseason.BrandObjectName = null;
            this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason.Location = new System.Drawing.Point(301, 37);
            this.txtseason.Name = "txtseason";
            this.txtseason.Size = new System.Drawing.Size(80, 23);
            this.txtseason.TabIndex = 11;
            // 
            // displayShipMode
            // 
            this.displayShipMode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayShipMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayShipMode.Location = new System.Drawing.Point(301, 11);
            this.displayShipMode.Name = "displayShipMode";
            this.displayShipMode.Size = new System.Drawing.Size(100, 23);
            this.displayShipMode.TabIndex = 10;
            // 
            // labelBuyMonth
            // 
            this.labelBuyMonth.Lines = 0;
            this.labelBuyMonth.Location = new System.Drawing.Point(222, 64);
            this.labelBuyMonth.Name = "labelBuyMonth";
            this.labelBuyMonth.Size = new System.Drawing.Size(75, 23);
            this.labelBuyMonth.TabIndex = 9;
            this.labelBuyMonth.Text = "Buy Month";
            // 
            // labelSeason
            // 
            this.labelSeason.Lines = 0;
            this.labelSeason.Location = new System.Drawing.Point(222, 37);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(75, 23);
            this.labelSeason.TabIndex = 8;
            this.labelSeason.Text = "Season";
            // 
            // labelShipMode
            // 
            this.labelShipMode.Lines = 0;
            this.labelShipMode.Location = new System.Drawing.Point(222, 10);
            this.labelShipMode.Name = "labelShipMode";
            this.labelShipMode.Size = new System.Drawing.Size(75, 23);
            this.labelShipMode.TabIndex = 7;
            this.labelShipMode.Text = "Ship Mode";
            // 
            // txtOrderType
            // 
            this.txtOrderType.BackColor = System.Drawing.Color.White;
            this.txtOrderType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtOrderType.Location = new System.Drawing.Point(83, 65);
            this.txtOrderType.Name = "txtOrderType";
            this.txtOrderType.Size = new System.Drawing.Size(125, 23);
            this.txtOrderType.TabIndex = 6;
            this.txtOrderType.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtOrderType_PopUp);
            // 
            // txtcustcd
            // 
            this.txtcustcd.BackColor = System.Drawing.Color.White;
            this.txtcustcd.BrandObjectName = null;
            this.txtcustcd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcustcd.Location = new System.Drawing.Point(83, 38);
            this.txtcustcd.Name = "txtcustcd";
            this.txtcustcd.Size = new System.Drawing.Size(125, 23);
            this.txtcustcd.TabIndex = 5;
            // 
            // displayBrand
            // 
            this.displayBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBrand.Location = new System.Drawing.Point(83, 11);
            this.displayBrand.Name = "displayBrand";
            this.displayBrand.Size = new System.Drawing.Size(66, 23);
            this.displayBrand.TabIndex = 4;
            // 
            // labelLocateSPNo
            // 
            this.labelLocateSPNo.Lines = 0;
            this.labelLocateSPNo.Location = new System.Drawing.Point(4, 101);
            this.labelLocateSPNo.Name = "labelLocateSPNo";
            this.labelLocateSPNo.Size = new System.Drawing.Size(93, 23);
            this.labelLocateSPNo.TabIndex = 3;
            this.labelLocateSPNo.Text = "Locate SP No.";
            // 
            // labelOrderType
            // 
            this.labelOrderType.Lines = 0;
            this.labelOrderType.Location = new System.Drawing.Point(4, 65);
            this.labelOrderType.Name = "labelOrderType";
            this.labelOrderType.Size = new System.Drawing.Size(75, 23);
            this.labelOrderType.TabIndex = 2;
            this.labelOrderType.Text = "Order Type";
            // 
            // labelCustCD
            // 
            this.labelCustCD.Lines = 0;
            this.labelCustCD.Location = new System.Drawing.Point(4, 38);
            this.labelCustCD.Name = "labelCustCD";
            this.labelCustCD.Size = new System.Drawing.Size(75, 23);
            this.labelCustCD.TabIndex = 1;
            this.labelCustCD.Text = "CustCD";
            // 
            // labelBrand
            // 
            this.labelBrand.Lines = 0;
            this.labelBrand.Location = new System.Drawing.Point(4, 11);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(75, 23);
            this.labelBrand.TabIndex = 0;
            this.labelBrand.Text = "Brand";
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Shapes.AddRange(new Microsoft.VisualBasic.PowerPacks.Shape[] {
            this.lineShape1});
            this.shapeContainer1.Size = new System.Drawing.Size(896, 132);
            this.shapeContainer1.TabIndex = 20;
            this.shapeContainer1.TabStop = false;
            // 
            // lineShape1
            // 
            this.lineShape1.Name = "lineShape1";
            this.lineShape1.X1 = 0;
            this.lineShape1.X2 = 854;
            this.lineShape1.Y1 = 93;
            this.lineShape1.Y2 = 93;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Controls.Add(this.btnImport);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 512);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(896, 43);
            this.panel4.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(801, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(710, 6);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(80, 30);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridDetail);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 132);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(896, 380);
            this.panel5.TabIndex = 4;
            // 
            // gridDetail
            // 
            this.gridDetail.AllowUserToAddRows = false;
            this.gridDetail.AllowUserToDeleteRows = false;
            this.gridDetail.AllowUserToResizeRows = false;
            this.gridDetail.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridDetail.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridDetail.DataSource = this.listControlBindingSource1;
            this.gridDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetail.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridDetail.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridDetail.Location = new System.Drawing.Point(0, 0);
            this.gridDetail.Name = "gridDetail";
            this.gridDetail.RowHeadersVisible = false;
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridDetail.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridDetail.RowTemplate.Height = 24;
            this.gridDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDetail.Size = new System.Drawing.Size(896, 380);
            this.gridDetail.TabIndex = 0;
            this.gridDetail.TabStop = false;
            // 
            // P04_BatchImport
            // 
            this.AcceptButton = this.btnImport;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(906, 555);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Name = "P04_BatchImport";
            this.Text = "Batch Import";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.DateRange dateBuyerDelivery;
        private Class.Txtcountry txtcountryDestination;
        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.Label labelDestination;
        private Class.Txtdropdownlist txtdropdownlistBuyMonth;
        private Class.Txtseason txtseason;
        private Win.UI.DisplayBox displayShipMode;
        private Win.UI.Label labelBuyMonth;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelShipMode;
        private Win.UI.TextBox txtOrderType;
        private Class.Txtcustcd txtcustcd;
        private Win.UI.DisplayBox displayBrand;
        private Win.UI.Label labelLocateSPNo;
        private Win.UI.Label labelOrderType;
        private Win.UI.Label labelCustCD;
        private Win.UI.Label labelBrand;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel5;
        private Win.UI.TextBox txtLocateSPNo;
        private Win.UI.Button btnQuery;
        private Win.UI.Button btnFind;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Microsoft.VisualBasic.PowerPacks.LineShape lineShape1;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnImport;
        private Win.UI.Grid gridDetail;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
    }
}
