namespace Sci.Production.Packing
{
    partial class P01
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
            this.labelSP = new Sci.Win.UI.Label();
            this.labelPONo = new Sci.Win.UI.Label();
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelStyle = new Sci.Win.UI.Label();
            this.labelSeason = new Sci.Win.UI.Label();
            this.labelDestination = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.labelPackingMethod = new Sci.Win.UI.Label();
            this.labelQtyCarton = new Sci.Win.UI.Label();
            this.labelLocalMR = new Sci.Win.UI.Label();
            this.labelSMR = new Sci.Win.UI.Label();
            this.labelOrderHandle = new Sci.Win.UI.Label();
            this.labelPOcombo = new Sci.Win.UI.Label();
            this.displaySP = new Sci.Win.UI.DisplayBox();
            this.displayPONo = new Sci.Win.UI.DisplayBox();
            this.displayBrand = new Sci.Win.UI.DisplayBox();
            this.displayStyle = new Sci.Win.UI.DisplayBox();
            this.displaySeason = new Sci.Win.UI.DisplayBox();
            this.txtcountryDestination = new Sci.Production.Class.Txtcountry();
            this.displayDescription = new Sci.Win.UI.DisplayBox();
            this.numQtyCarton = new Sci.Win.UI.NumericBox();
            this.txttpeuserSMR = new Sci.Production.Class.Txttpeuser();
            this.txtuserLocalMR = new Sci.Production.Class.Txtuser();
            this.txttpeuserOrderHandle = new Sci.Production.Class.Txttpeuser();
            this.displayPOcombo = new Sci.Win.UI.DisplayBox();
            this.btnbdown = new Sci.Win.UI.Button();
            this.checkLocalOrder = new Sci.Win.UI.CheckBox();
            this.checkCancelledOrder = new Sci.Win.UI.CheckBox();
            this.checkPullForwardOrder = new Sci.Win.UI.CheckBox();
            this.labelCategory = new Sci.Win.UI.Label();
            this.labelProject = new Sci.Win.UI.Label();
            this.labelOrderQty = new Sci.Win.UI.Label();
            this.labelSewingInLine = new Sci.Win.UI.Label();
            this.labelSewingOffline = new Sci.Win.UI.Label();
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.labelCloseGarment = new Sci.Win.UI.Label();
            this.txtdropdownlistCategory = new Sci.Production.Class.Txtdropdownlist();
            this.displayProject = new Sci.Win.UI.DisplayBox();
            this.numOrderQty = new Sci.Win.UI.NumericBox();
            this.displayOrderQty = new Sci.Win.UI.DisplayBox();
            this.dateSewingInLine = new Sci.Win.UI.DateBox();
            this.dateSewingOffline = new Sci.Win.UI.DateBox();
            this.dateBuyerDelivery = new Sci.Win.UI.DateBox();
            this.dateCloseGarment = new Sci.Win.UI.DateBox();
            this.btnQuantityBreakdown = new Sci.Win.UI.Button();
            this.btnPackingMethod = new Sci.Win.UI.Button();
            this.btnCartonStatus = new Sci.Win.UI.Button();
            this.btnMaterialImport = new Sci.Win.UI.Button();
            this.btnCartonBooking = new Sci.Win.UI.Button();
            this.btnOverrunGarmentRecord = new Sci.Win.UI.Button();
            this.txtdropdownlistPackingMethod = new Sci.Production.Class.Txtdropdownlist();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(824, 484);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtdropdownlistPackingMethod);
            this.detailcont.Controls.Add(this.btnOverrunGarmentRecord);
            this.detailcont.Controls.Add(this.btnCartonBooking);
            this.detailcont.Controls.Add(this.btnMaterialImport);
            this.detailcont.Controls.Add(this.btnCartonStatus);
            this.detailcont.Controls.Add(this.btnPackingMethod);
            this.detailcont.Controls.Add(this.btnQuantityBreakdown);
            this.detailcont.Controls.Add(this.dateCloseGarment);
            this.detailcont.Controls.Add(this.dateBuyerDelivery);
            this.detailcont.Controls.Add(this.dateSewingOffline);
            this.detailcont.Controls.Add(this.dateSewingInLine);
            this.detailcont.Controls.Add(this.displayOrderQty);
            this.detailcont.Controls.Add(this.numOrderQty);
            this.detailcont.Controls.Add(this.displayProject);
            this.detailcont.Controls.Add(this.txtdropdownlistCategory);
            this.detailcont.Controls.Add(this.labelCloseGarment);
            this.detailcont.Controls.Add(this.labelBuyerDelivery);
            this.detailcont.Controls.Add(this.labelSewingOffline);
            this.detailcont.Controls.Add(this.labelSewingInLine);
            this.detailcont.Controls.Add(this.labelOrderQty);
            this.detailcont.Controls.Add(this.labelProject);
            this.detailcont.Controls.Add(this.labelCategory);
            this.detailcont.Controls.Add(this.checkPullForwardOrder);
            this.detailcont.Controls.Add(this.checkCancelledOrder);
            this.detailcont.Controls.Add(this.checkLocalOrder);
            this.detailcont.Controls.Add(this.btnbdown);
            this.detailcont.Controls.Add(this.displayPOcombo);
            this.detailcont.Controls.Add(this.txttpeuserOrderHandle);
            this.detailcont.Controls.Add(this.txtuserLocalMR);
            this.detailcont.Controls.Add(this.txttpeuserSMR);
            this.detailcont.Controls.Add(this.numQtyCarton);
            this.detailcont.Controls.Add(this.displayDescription);
            this.detailcont.Controls.Add(this.txtcountryDestination);
            this.detailcont.Controls.Add(this.displaySeason);
            this.detailcont.Controls.Add(this.displayStyle);
            this.detailcont.Controls.Add(this.displayBrand);
            this.detailcont.Controls.Add(this.displayPONo);
            this.detailcont.Controls.Add(this.displaySP);
            this.detailcont.Controls.Add(this.labelPOcombo);
            this.detailcont.Controls.Add(this.labelOrderHandle);
            this.detailcont.Controls.Add(this.labelSMR);
            this.detailcont.Controls.Add(this.labelLocalMR);
            this.detailcont.Controls.Add(this.labelQtyCarton);
            this.detailcont.Controls.Add(this.labelPackingMethod);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labelDestination);
            this.detailcont.Controls.Add(this.labelSeason);
            this.detailcont.Controls.Add(this.labelStyle);
            this.detailcont.Controls.Add(this.labelBrand);
            this.detailcont.Controls.Add(this.labelPONo);
            this.detailcont.Controls.Add(this.labelSP);
            this.detailcont.Size = new System.Drawing.Size(824, 446);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 446);
            this.detailbtm.Size = new System.Drawing.Size(824, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(824, 484);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(832, 513);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // labelSP
            // 
            this.labelSP.Lines = 0;
            this.labelSP.Location = new System.Drawing.Point(9, 13);
            this.labelSP.Name = "labelSP";
            this.labelSP.Size = new System.Drawing.Size(75, 23);
            this.labelSP.TabIndex = 0;
            this.labelSP.Text = "SP#";
            // 
            // labelPONo
            // 
            this.labelPONo.Lines = 0;
            this.labelPONo.Location = new System.Drawing.Point(9, 40);
            this.labelPONo.Name = "labelPONo";
            this.labelPONo.Size = new System.Drawing.Size(75, 23);
            this.labelPONo.TabIndex = 1;
            this.labelPONo.Text = "PO No.";
            // 
            // labelBrand
            // 
            this.labelBrand.Lines = 0;
            this.labelBrand.Location = new System.Drawing.Point(9, 67);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(75, 23);
            this.labelBrand.TabIndex = 2;
            this.labelBrand.Text = "Brand";
            // 
            // labelStyle
            // 
            this.labelStyle.Lines = 0;
            this.labelStyle.Location = new System.Drawing.Point(9, 94);
            this.labelStyle.Name = "labelStyle";
            this.labelStyle.Size = new System.Drawing.Size(75, 23);
            this.labelStyle.TabIndex = 3;
            this.labelStyle.Text = "Style#";
            // 
            // labelSeason
            // 
            this.labelSeason.Lines = 0;
            this.labelSeason.Location = new System.Drawing.Point(9, 121);
            this.labelSeason.Name = "labelSeason";
            this.labelSeason.Size = new System.Drawing.Size(75, 23);
            this.labelSeason.TabIndex = 4;
            this.labelSeason.Text = "Season";
            // 
            // labelDestination
            // 
            this.labelDestination.Lines = 0;
            this.labelDestination.Location = new System.Drawing.Point(9, 148);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(75, 23);
            this.labelDestination.TabIndex = 5;
            this.labelDestination.Text = "Destination";
            // 
            // labelDescription
            // 
            this.labelDescription.Lines = 0;
            this.labelDescription.Location = new System.Drawing.Point(9, 175);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(75, 23);
            this.labelDescription.TabIndex = 6;
            this.labelDescription.Text = "Description";
            // 
            // labelPackingMethod
            // 
            this.labelPackingMethod.Lines = 0;
            this.labelPackingMethod.Location = new System.Drawing.Point(9, 229);
            this.labelPackingMethod.Name = "labelPackingMethod";
            this.labelPackingMethod.Size = new System.Drawing.Size(104, 23);
            this.labelPackingMethod.TabIndex = 7;
            this.labelPackingMethod.Text = "Packing method";
            // 
            // labelQtyCarton
            // 
            this.labelQtyCarton.Lines = 0;
            this.labelQtyCarton.Location = new System.Drawing.Point(9, 202);
            this.labelQtyCarton.Name = "labelQtyCarton";
            this.labelQtyCarton.Size = new System.Drawing.Size(75, 23);
            this.labelQtyCarton.TabIndex = 8;
            this.labelQtyCarton.Text = "Qty/carton";
            // 
            // labelLocalMR
            // 
            this.labelLocalMR.Lines = 0;
            this.labelLocalMR.Location = new System.Drawing.Point(9, 256);
            this.labelLocalMR.Name = "labelLocalMR";
            this.labelLocalMR.Size = new System.Drawing.Size(104, 23);
            this.labelLocalMR.TabIndex = 9;
            this.labelLocalMR.Text = "Local MR";
            // 
            // labelSMR
            // 
            this.labelSMR.Lines = 0;
            this.labelSMR.Location = new System.Drawing.Point(9, 283);
            this.labelSMR.Name = "labelSMR";
            this.labelSMR.Size = new System.Drawing.Size(104, 23);
            this.labelSMR.TabIndex = 10;
            this.labelSMR.Text = "SMR";
            // 
            // labelOrderHandle
            // 
            this.labelOrderHandle.Lines = 0;
            this.labelOrderHandle.Location = new System.Drawing.Point(9, 310);
            this.labelOrderHandle.Name = "labelOrderHandle";
            this.labelOrderHandle.Size = new System.Drawing.Size(104, 23);
            this.labelOrderHandle.TabIndex = 11;
            this.labelOrderHandle.Text = "Order Handle";
            // 
            // labelPOcombo
            // 
            this.labelPOcombo.Lines = 0;
            this.labelPOcombo.Location = new System.Drawing.Point(9, 337);
            this.labelPOcombo.Name = "labelPOcombo";
            this.labelPOcombo.Size = new System.Drawing.Size(104, 23);
            this.labelPOcombo.TabIndex = 12;
            this.labelPOcombo.Text = "PO combo";
            // 
            // displaySP
            // 
            this.displaySP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySP.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displaySP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySP.Location = new System.Drawing.Point(88, 13);
            this.displaySP.Name = "displaySP";
            this.displaySP.Size = new System.Drawing.Size(120, 23);
            this.displaySP.TabIndex = 13;
            // 
            // displayPONo
            // 
            this.displayPONo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPONo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CustPONo", true));
            this.displayPONo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPONo.Location = new System.Drawing.Point(88, 40);
            this.displayPONo.Name = "displayPONo";
            this.displayPONo.Size = new System.Drawing.Size(200, 23);
            this.displayPONo.TabIndex = 14;
            // 
            // displayBrand
            // 
            this.displayBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBrand.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "BrandID", true));
            this.displayBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBrand.Location = new System.Drawing.Point(88, 67);
            this.displayBrand.Name = "displayBrand";
            this.displayBrand.Size = new System.Drawing.Size(75, 23);
            this.displayBrand.TabIndex = 15;
            // 
            // displayStyle
            // 
            this.displayStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyle.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "StyleID", true));
            this.displayStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyle.Location = new System.Drawing.Point(88, 94);
            this.displayStyle.Name = "displayStyle";
            this.displayStyle.Size = new System.Drawing.Size(140, 23);
            this.displayStyle.TabIndex = 16;
            // 
            // displaySeason
            // 
            this.displaySeason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySeason.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "SeasonID", true));
            this.displaySeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySeason.Location = new System.Drawing.Point(88, 121);
            this.displaySeason.Name = "displaySeason";
            this.displaySeason.Size = new System.Drawing.Size(90, 23);
            this.displaySeason.TabIndex = 17;
            // 
            // txtcountryDestination
            // 
            this.txtcountryDestination.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "Dest", true));
            this.txtcountryDestination.DisplayBox1Binding = "";
            this.txtcountryDestination.Location = new System.Drawing.Point(88, 148);
            this.txtcountryDestination.Name = "txtcountryDestination";
            this.txtcountryDestination.Size = new System.Drawing.Size(232, 22);
            this.txtcountryDestination.TabIndex = 18;
            this.txtcountryDestination.TextBox1Binding = "";
            // 
            // displayDescription
            // 
            this.displayDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDescription.Location = new System.Drawing.Point(88, 175);
            this.displayDescription.Name = "displayDescription";
            this.displayDescription.Size = new System.Drawing.Size(376, 23);
            this.displayDescription.TabIndex = 19;
            // 
            // numQtyCarton
            // 
            this.numQtyCarton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numQtyCarton.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CTNQty", true));
            this.numQtyCarton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numQtyCarton.IsSupportEditMode = false;
            this.numQtyCarton.Location = new System.Drawing.Point(88, 202);
            this.numQtyCarton.Name = "numQtyCarton";
            this.numQtyCarton.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numQtyCarton.ReadOnly = true;
            this.numQtyCarton.Size = new System.Drawing.Size(50, 23);
            this.numQtyCarton.TabIndex = 20;
            this.numQtyCarton.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txttpeuserSMR
            // 
            this.txttpeuserSMR.DataBindings.Add(new System.Windows.Forms.Binding("DisplayBox1Binding", this.mtbs, "SMR", true));
            this.txttpeuserSMR.DisplayBox1Binding = "";
            this.txttpeuserSMR.DisplayBox2Binding = "";
            this.txttpeuserSMR.Location = new System.Drawing.Point(117, 283);
            this.txttpeuserSMR.Name = "txttpeuserSMR";
            this.txttpeuserSMR.Size = new System.Drawing.Size(302, 23);
            this.txttpeuserSMR.TabIndex = 22;
            // 
            // txtuserLocalMR
            // 
            this.txtuserLocalMR.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "LocalMR", true));
            this.txtuserLocalMR.DisplayBox1Binding = "";
            this.txtuserLocalMR.Location = new System.Drawing.Point(117, 256);
            this.txtuserLocalMR.Name = "txtuserLocalMR";
            this.txtuserLocalMR.Size = new System.Drawing.Size(302, 23);
            this.txtuserLocalMR.TabIndex = 23;
            this.txtuserLocalMR.TextBox1Binding = "";
            // 
            // txttpeuserOrderHandle
            // 
            this.txttpeuserOrderHandle.DataBindings.Add(new System.Windows.Forms.Binding("DisplayBox1Binding", this.mtbs, "MRHandle", true));
            this.txttpeuserOrderHandle.DisplayBox1Binding = "";
            this.txttpeuserOrderHandle.DisplayBox2Binding = "";
            this.txttpeuserOrderHandle.Location = new System.Drawing.Point(117, 310);
            this.txttpeuserOrderHandle.Name = "txttpeuserOrderHandle";
            this.txttpeuserOrderHandle.Size = new System.Drawing.Size(302, 23);
            this.txttpeuserOrderHandle.TabIndex = 24;
            // 
            // displayPOcombo
            // 
            this.displayPOcombo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPOcombo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPOcombo.Location = new System.Drawing.Point(117, 337);
            this.displayPOcombo.Name = "displayPOcombo";
            this.displayPOcombo.Size = new System.Drawing.Size(659, 23);
            this.displayPOcombo.TabIndex = 25;
            // 
            // btnbdown
            // 
            this.btnbdown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnbdown.Location = new System.Drawing.Point(143, 201);
            this.btnbdown.Name = "btnbdown";
            this.btnbdown.Size = new System.Drawing.Size(64, 25);
            this.btnbdown.TabIndex = 26;
            this.btnbdown.Text = "b\'down";
            this.btnbdown.UseVisualStyleBackColor = true;
            this.btnbdown.Click += new System.EventHandler(this.Btnbdown_Click);
            // 
            // checkLocalOrder
            // 
            this.checkLocalOrder.AutoSize = true;
            this.checkLocalOrder.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "LocalOrder", true));
            this.checkLocalOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkLocalOrder.IsSupportEditMode = false;
            this.checkLocalOrder.Location = new System.Drawing.Point(362, 13);
            this.checkLocalOrder.Name = "checkLocalOrder";
            this.checkLocalOrder.ReadOnly = true;
            this.checkLocalOrder.Size = new System.Drawing.Size(99, 21);
            this.checkLocalOrder.TabIndex = 27;
            this.checkLocalOrder.Text = "Local order";
            this.checkLocalOrder.UseVisualStyleBackColor = true;
            // 
            // checkCancelledOrder
            // 
            this.checkCancelledOrder.AutoSize = true;
            this.checkCancelledOrder.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkCancelledOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkCancelledOrder.IsSupportEditMode = false;
            this.checkCancelledOrder.Location = new System.Drawing.Point(362, 40);
            this.checkCancelledOrder.Name = "checkCancelledOrder";
            this.checkCancelledOrder.ReadOnly = true;
            this.checkCancelledOrder.Size = new System.Drawing.Size(127, 21);
            this.checkCancelledOrder.TabIndex = 28;
            this.checkCancelledOrder.Text = "Cancelled order";
            this.checkCancelledOrder.UseVisualStyleBackColor = true;
            // 
            // checkPullForwardOrder
            // 
            this.checkPullForwardOrder.AutoSize = true;
            this.checkPullForwardOrder.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PFOrder", true));
            this.checkPullForwardOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkPullForwardOrder.IsSupportEditMode = false;
            this.checkPullForwardOrder.Location = new System.Drawing.Point(362, 67);
            this.checkPullForwardOrder.Name = "checkPullForwardOrder";
            this.checkPullForwardOrder.ReadOnly = true;
            this.checkPullForwardOrder.Size = new System.Drawing.Size(139, 21);
            this.checkPullForwardOrder.TabIndex = 29;
            this.checkPullForwardOrder.Text = "Pull forward order";
            this.checkPullForwardOrder.UseVisualStyleBackColor = true;
            // 
            // labelCategory
            // 
            this.labelCategory.Lines = 0;
            this.labelCategory.Location = new System.Drawing.Point(548, 13);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(98, 23);
            this.labelCategory.TabIndex = 30;
            this.labelCategory.Text = "Category";
            // 
            // labelProject
            // 
            this.labelProject.Lines = 0;
            this.labelProject.Location = new System.Drawing.Point(548, 40);
            this.labelProject.Name = "labelProject";
            this.labelProject.Size = new System.Drawing.Size(98, 23);
            this.labelProject.TabIndex = 31;
            this.labelProject.Text = "Project";
            // 
            // labelOrderQty
            // 
            this.labelOrderQty.Lines = 0;
            this.labelOrderQty.Location = new System.Drawing.Point(548, 67);
            this.labelOrderQty.Name = "labelOrderQty";
            this.labelOrderQty.Size = new System.Drawing.Size(98, 23);
            this.labelOrderQty.TabIndex = 32;
            this.labelOrderQty.Text = "Order qty";
            // 
            // labelSewingInLine
            // 
            this.labelSewingInLine.Lines = 0;
            this.labelSewingInLine.Location = new System.Drawing.Point(548, 94);
            this.labelSewingInLine.Name = "labelSewingInLine";
            this.labelSewingInLine.Size = new System.Drawing.Size(98, 23);
            this.labelSewingInLine.TabIndex = 33;
            this.labelSewingInLine.Text = "Sewing inline";
            // 
            // labelSewingOffline
            // 
            this.labelSewingOffline.Lines = 0;
            this.labelSewingOffline.Location = new System.Drawing.Point(548, 121);
            this.labelSewingOffline.Name = "labelSewingOffline";
            this.labelSewingOffline.Size = new System.Drawing.Size(98, 23);
            this.labelSewingOffline.TabIndex = 34;
            this.labelSewingOffline.Text = "Sewing offline";
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Lines = 0;
            this.labelBuyerDelivery.Location = new System.Drawing.Point(548, 148);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(98, 23);
            this.labelBuyerDelivery.TabIndex = 35;
            this.labelBuyerDelivery.Text = "Buyer delivery";
            // 
            // labelCloseGarment
            // 
            this.labelCloseGarment.Lines = 0;
            this.labelCloseGarment.Location = new System.Drawing.Point(548, 175);
            this.labelCloseGarment.Name = "labelCloseGarment";
            this.labelCloseGarment.Size = new System.Drawing.Size(98, 23);
            this.labelCloseGarment.TabIndex = 36;
            this.labelCloseGarment.Text = "Close Garment";
            // 
            // txtdropdownlistCategory
            // 
            this.txtdropdownlistCategory.BackColor = System.Drawing.Color.White;
            this.txtdropdownlistCategory.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Category", true));
            this.txtdropdownlistCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtdropdownlistCategory.FormattingEnabled = true;
            this.txtdropdownlistCategory.IsSupportUnselect = true;
            this.txtdropdownlistCategory.Location = new System.Drawing.Point(649, 13);
            this.txtdropdownlistCategory.Name = "txtdropdownlistCategory";
            this.txtdropdownlistCategory.Size = new System.Drawing.Size(121, 24);
            this.txtdropdownlistCategory.TabIndex = 37;
            this.txtdropdownlistCategory.Type = "Category";
            // 
            // displayProject
            // 
            this.displayProject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayProject.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ProjectID", true));
            this.displayProject.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayProject.Location = new System.Drawing.Point(649, 40);
            this.displayProject.Name = "displayProject";
            this.displayProject.Size = new System.Drawing.Size(60, 23);
            this.displayProject.TabIndex = 38;
            // 
            // numOrderQty
            // 
            this.numOrderQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numOrderQty.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Qty", true));
            this.numOrderQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numOrderQty.IsSupportEditMode = false;
            this.numOrderQty.Location = new System.Drawing.Point(649, 67);
            this.numOrderQty.Name = "numOrderQty";
            this.numOrderQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numOrderQty.ReadOnly = true;
            this.numOrderQty.Size = new System.Drawing.Size(70, 23);
            this.numOrderQty.TabIndex = 39;
            this.numOrderQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // displayOrderQty
            // 
            this.displayOrderQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayOrderQty.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "StyleUnit", true));
            this.displayOrderQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayOrderQty.Location = new System.Drawing.Point(721, 67);
            this.displayOrderQty.Name = "displayOrderQty";
            this.displayOrderQty.Size = new System.Drawing.Size(55, 23);
            this.displayOrderQty.TabIndex = 40;
            // 
            // dateSewingInLine
            // 
            this.dateSewingInLine.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "SewInLine", true));
            this.dateSewingInLine.IsSupportEditMode = false;
            this.dateSewingInLine.Location = new System.Drawing.Point(649, 94);
            this.dateSewingInLine.Name = "dateSewingInLine";
            this.dateSewingInLine.ReadOnly = true;
            this.dateSewingInLine.Size = new System.Drawing.Size(100, 23);
            this.dateSewingInLine.TabIndex = 41;
            // 
            // dateSewingOffline
            // 
            this.dateSewingOffline.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "SewOffLine", true));
            this.dateSewingOffline.IsSupportEditMode = false;
            this.dateSewingOffline.Location = new System.Drawing.Point(649, 121);
            this.dateSewingOffline.Name = "dateSewingOffline";
            this.dateSewingOffline.ReadOnly = true;
            this.dateSewingOffline.Size = new System.Drawing.Size(100, 23);
            this.dateSewingOffline.TabIndex = 42;
            // 
            // dateBuyerDelivery
            // 
            this.dateBuyerDelivery.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "BuyerDelivery", true));
            this.dateBuyerDelivery.IsSupportEditMode = false;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(649, 148);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.ReadOnly = true;
            this.dateBuyerDelivery.Size = new System.Drawing.Size(100, 23);
            this.dateBuyerDelivery.TabIndex = 43;
            // 
            // dateCloseGarment
            // 
            this.dateCloseGarment.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "GMTClose", true));
            this.dateCloseGarment.IsSupportEditMode = false;
            this.dateCloseGarment.Location = new System.Drawing.Point(649, 175);
            this.dateCloseGarment.Name = "dateCloseGarment";
            this.dateCloseGarment.ReadOnly = true;
            this.dateCloseGarment.Size = new System.Drawing.Size(100, 23);
            this.dateCloseGarment.TabIndex = 44;
            // 
            // btnQuantityBreakdown
            // 
            this.btnQuantityBreakdown.Location = new System.Drawing.Point(20, 366);
            this.btnQuantityBreakdown.Name = "btnQuantityBreakdown";
            this.btnQuantityBreakdown.Size = new System.Drawing.Size(194, 30);
            this.btnQuantityBreakdown.TabIndex = 45;
            this.btnQuantityBreakdown.Text = "Quantity breakdown";
            this.btnQuantityBreakdown.UseVisualStyleBackColor = true;
            this.btnQuantityBreakdown.Click += new System.EventHandler(this.BtnQuantityBreakdown_Click);
            // 
            // btnPackingMethod
            // 
            this.btnPackingMethod.Location = new System.Drawing.Point(20, 403);
            this.btnPackingMethod.Name = "btnPackingMethod";
            this.btnPackingMethod.Size = new System.Drawing.Size(194, 30);
            this.btnPackingMethod.TabIndex = 46;
            this.btnPackingMethod.Text = "Packing method";
            this.btnPackingMethod.UseVisualStyleBackColor = true;
            this.btnPackingMethod.Click += new System.EventHandler(this.BtnPackingMethod_Click);
            // 
            // btnCartonStatus
            // 
            this.btnCartonStatus.Location = new System.Drawing.Point(221, 367);
            this.btnCartonStatus.Name = "btnCartonStatus";
            this.btnCartonStatus.Size = new System.Drawing.Size(194, 30);
            this.btnCartonStatus.TabIndex = 47;
            this.btnCartonStatus.Text = "Carton Status";
            this.btnCartonStatus.UseVisualStyleBackColor = true;
            this.btnCartonStatus.Click += new System.EventHandler(this.BtnCartonStatus_Click);
            // 
            // btnMaterialImport
            // 
            this.btnMaterialImport.Location = new System.Drawing.Point(221, 403);
            this.btnMaterialImport.Name = "btnMaterialImport";
            this.btnMaterialImport.Size = new System.Drawing.Size(194, 30);
            this.btnMaterialImport.TabIndex = 48;
            this.btnMaterialImport.Text = "Material import";
            this.btnMaterialImport.UseVisualStyleBackColor = true;
            this.btnMaterialImport.Click += new System.EventHandler(this.BtnMaterialImport_Click);
            // 
            // btnCartonBooking
            // 
            this.btnCartonBooking.Location = new System.Drawing.Point(422, 367);
            this.btnCartonBooking.Name = "btnCartonBooking";
            this.btnCartonBooking.Size = new System.Drawing.Size(194, 30);
            this.btnCartonBooking.TabIndex = 49;
            this.btnCartonBooking.Text = "Carton Booking";
            this.btnCartonBooking.UseVisualStyleBackColor = true;
            this.btnCartonBooking.Click += new System.EventHandler(this.BtnCartonBooking_Click);
            // 
            // btnOverrunGarmentRecord
            // 
            this.btnOverrunGarmentRecord.Location = new System.Drawing.Point(422, 403);
            this.btnOverrunGarmentRecord.Name = "btnOverrunGarmentRecord";
            this.btnOverrunGarmentRecord.Size = new System.Drawing.Size(194, 30);
            this.btnOverrunGarmentRecord.TabIndex = 50;
            this.btnOverrunGarmentRecord.Text = "Overrun garment record";
            this.btnOverrunGarmentRecord.UseVisualStyleBackColor = true;
            this.btnOverrunGarmentRecord.Click += new System.EventHandler(this.BtnOverrunGarmentRecord_Click);
            // 
            // txtdropdownlistPackingMethod
            // 
            this.txtdropdownlistPackingMethod.BackColor = System.Drawing.Color.White;
            this.txtdropdownlistPackingMethod.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "CtnType", true));
            this.txtdropdownlistPackingMethod.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtdropdownlistPackingMethod.FormattingEnabled = true;
            this.txtdropdownlistPackingMethod.IsSupportUnselect = true;
            this.txtdropdownlistPackingMethod.Location = new System.Drawing.Point(117, 229);
            this.txtdropdownlistPackingMethod.Name = "txtdropdownlistPackingMethod";
            this.txtdropdownlistPackingMethod.Size = new System.Drawing.Size(298, 24);
            this.txtdropdownlistPackingMethod.TabIndex = 51;
            this.txtdropdownlistPackingMethod.Type = "PackingMethod";
            // 
            // P01
            // 
            this.ClientSize = new System.Drawing.Size(832, 546);
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "P01";
            this.Text = "P01. Packing Master List";
            this.UniqueExpress = "ID";
            this.WorkAlias = "Orders";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailcont.PerformLayout();
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.CheckBox checkPullForwardOrder;
        private Win.UI.CheckBox checkCancelledOrder;
        private Win.UI.CheckBox checkLocalOrder;
        private Win.UI.Button btnbdown;
        private Win.UI.DisplayBox displayPOcombo;
        private Class.Txttpeuser txttpeuserOrderHandle;
        private Class.Txtuser txtuserLocalMR;
        private Class.Txttpeuser txttpeuserSMR;
        private Win.UI.NumericBox numQtyCarton;
        private Win.UI.DisplayBox displayDescription;
        private Class.Txtcountry txtcountryDestination;
        private Win.UI.DisplayBox displaySeason;
        private Win.UI.DisplayBox displayStyle;
        private Win.UI.DisplayBox displayBrand;
        private Win.UI.DisplayBox displayPONo;
        private Win.UI.DisplayBox displaySP;
        private Win.UI.Label labelPOcombo;
        private Win.UI.Label labelOrderHandle;
        private Win.UI.Label labelSMR;
        private Win.UI.Label labelLocalMR;
        private Win.UI.Label labelQtyCarton;
        private Win.UI.Label labelPackingMethod;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelDestination;
        private Win.UI.Label labelSeason;
        private Win.UI.Label labelStyle;
        private Win.UI.Label labelBrand;
        private Win.UI.Label labelPONo;
        private Win.UI.Label labelSP;
        private Win.UI.Label labelCloseGarment;
        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.Label labelSewingOffline;
        private Win.UI.Label labelSewingInLine;
        private Win.UI.Label labelOrderQty;
        private Win.UI.Label labelProject;
        private Win.UI.Label labelCategory;
        private Win.UI.DisplayBox displayProject;
        private Class.Txtdropdownlist txtdropdownlistCategory;
        private Win.UI.DateBox dateCloseGarment;
        private Win.UI.DateBox dateBuyerDelivery;
        private Win.UI.DateBox dateSewingOffline;
        private Win.UI.DateBox dateSewingInLine;
        private Win.UI.DisplayBox displayOrderQty;
        private Win.UI.NumericBox numOrderQty;
        private Win.UI.Button btnPackingMethod;
        private Win.UI.Button btnQuantityBreakdown;
        private Win.UI.Button btnOverrunGarmentRecord;
        private Win.UI.Button btnCartonBooking;
        private Win.UI.Button btnMaterialImport;
        private Win.UI.Button btnCartonStatus;
        private Class.Txtdropdownlist txtdropdownlistPackingMethod;
    }
}
