namespace Sci.Production.Quality
{
    partial class P20
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
            this.txtSP = new Sci.Win.UI.TextBox();
            this.lbDate = new Sci.Win.UI.Label();
            this.CDate = new Sci.Win.UI.DateBox();
            this.lbSP = new Sci.Win.UI.Label();
            this.lbStyle = new Sci.Win.UI.Label();
            this.lbLine = new Sci.Win.UI.Label();
            this.txtLine = new Sci.Win.UI.TextBox();
            this.DisplayCell = new Sci.Win.UI.DisplayBox();
            this.lbCell = new Sci.Win.UI.Label();
            this.txtRemark = new Sci.Win.UI.TextBox();
            this.lbRemark = new Sci.Win.UI.Label();
            this.lbShift = new Sci.Win.UI.Label();
            this.lbInspected = new Sci.Win.UI.Label();
            this.lbRejected = new Sci.Win.UI.Label();
            this.lbDefectQty = new Sci.Win.UI.Label();
            this.lbDestination = new Sci.Win.UI.Label();
            this.NumInspected = new Sci.Win.UI.NumericBox();
            this.NumRejected = new Sci.Win.UI.NumericBox();
            this.NumDefect = new Sci.Win.UI.NumericBox();
            this.lbTeam = new Sci.Win.UI.Label();
            this.lbCPU = new Sci.Win.UI.Label();
            this.NumCPU = new Sci.Win.UI.NumericBox();
            this.lbRFT = new Sci.Win.UI.Label();
            this.NumRFT = new Sci.Win.UI.NumericBox();
            this.lbFactory = new Sci.Win.UI.Label();
            this.DisplayFactory = new Sci.Win.UI.DisplayBox();
            this.btnEncode = new Sci.Win.UI.Button();
            this.DisplayStyle = new Sci.Win.UI.DisplayBox();
            this.DisplayDest = new Sci.Win.UI.DisplayBox();
            this.comboShift = new Sci.Trade.Class.ComboDropDownList();
            this.comboTeam = new Sci.Trade.Class.ComboDropDownList();
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).BeginInit();
            this.masterpanel.SuspendLayout();
            this.detailpanel.SuspendLayout();
            this.detail2.SuspendLayout();
            this.detailpanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.comboTeam);
            this.masterpanel.Controls.Add(this.comboShift);
            this.masterpanel.Controls.Add(this.DisplayDest);
            this.masterpanel.Controls.Add(this.btnEncode);
            this.masterpanel.Controls.Add(this.DisplayFactory);
            this.masterpanel.Controls.Add(this.lbFactory);
            this.masterpanel.Controls.Add(this.NumRFT);
            this.masterpanel.Controls.Add(this.lbRFT);
            this.masterpanel.Controls.Add(this.NumCPU);
            this.masterpanel.Controls.Add(this.lbCPU);
            this.masterpanel.Controls.Add(this.lbTeam);
            this.masterpanel.Controls.Add(this.NumDefect);
            this.masterpanel.Controls.Add(this.NumRejected);
            this.masterpanel.Controls.Add(this.NumInspected);
            this.masterpanel.Controls.Add(this.lbDestination);
            this.masterpanel.Controls.Add(this.lbDefectQty);
            this.masterpanel.Controls.Add(this.lbRejected);
            this.masterpanel.Controls.Add(this.lbInspected);
            this.masterpanel.Controls.Add(this.lbShift);
            this.masterpanel.Controls.Add(this.txtRemark);
            this.masterpanel.Controls.Add(this.lbRemark);
            this.masterpanel.Controls.Add(this.DisplayCell);
            this.masterpanel.Controls.Add(this.lbCell);
            this.masterpanel.Controls.Add(this.txtLine);
            this.masterpanel.Controls.Add(this.lbLine);
            this.masterpanel.Controls.Add(this.DisplayStyle);
            this.masterpanel.Controls.Add(this.lbStyle);
            this.masterpanel.Controls.Add(this.lbSP);
            this.masterpanel.Controls.Add(this.txtSP);
            this.masterpanel.Controls.Add(this.lbDate);
            this.masterpanel.Controls.Add(this.CDate);
            this.masterpanel.Size = new System.Drawing.Size(876, 170);
            this.masterpanel.Controls.SetChildIndex(this.CDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbSP, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbStyle, 0);
            this.masterpanel.Controls.SetChildIndex(this.DisplayStyle, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbLine, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtLine, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbCell, 0);
            this.masterpanel.Controls.SetChildIndex(this.DisplayCell, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtRemark, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbShift, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbInspected, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbRejected, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbDefectQty, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbDestination, 0);
            this.masterpanel.Controls.SetChildIndex(this.NumInspected, 0);
            this.masterpanel.Controls.SetChildIndex(this.NumRejected, 0);
            this.masterpanel.Controls.SetChildIndex(this.NumDefect, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbTeam, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbCPU, 0);
            this.masterpanel.Controls.SetChildIndex(this.NumCPU, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbRFT, 0);
            this.masterpanel.Controls.SetChildIndex(this.NumRFT, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbFactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.DisplayFactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnEncode, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.DisplayDest, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboShift, 0);
            this.masterpanel.Controls.SetChildIndex(this.comboTeam, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 170);
            this.detailpanel.Size = new System.Drawing.Size(876, 192);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(769, 135);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(876, 192);
            // 
            // detail2
            // 
            this.detail2.Size = new System.Drawing.Size(892, 387);
            // 
            // detailgridcont2
            // 
            this.detailgridcont2.Size = new System.Drawing.Size(886, 341);
            // 
            // detailpanel2
            // 
            this.detailpanel2.Size = new System.Drawing.Size(886, 40);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(876, 400);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(876, 362);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 362);
            this.detailbtm.Size = new System.Drawing.Size(876, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(876, 400);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(884, 429);
            // 
            // txtSP
            // 
            this.txtSP.BackColor = System.Drawing.Color.White;
            this.txtSP.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "OrderID", true));
            this.txtSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSP.Location = new System.Drawing.Point(66, 33);
            this.txtSP.Name = "txtSP";
            this.txtSP.Size = new System.Drawing.Size(118, 23);
            this.txtSP.TabIndex = 101;
            this.txtSP.Validating += new System.ComponentModel.CancelEventHandler(this.txtSP_Validating);
            // 
            // lbDate
            // 
            this.lbDate.Lines = 0;
            this.lbDate.Location = new System.Drawing.Point(14, 6);
            this.lbDate.Name = "lbDate";
            this.lbDate.Size = new System.Drawing.Size(50, 23);
            this.lbDate.TabIndex = 100;
            this.lbDate.Text = "Date:";
            // 
            // CDate
            // 
            this.CDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CDate", true));
            this.CDate.Location = new System.Drawing.Point(66, 6);
            this.CDate.Name = "CDate";
            this.CDate.Size = new System.Drawing.Size(120, 23);
            this.CDate.TabIndex = 109;
            // 
            // lbSP
            // 
            this.lbSP.Lines = 0;
            this.lbSP.Location = new System.Drawing.Point(14, 33);
            this.lbSP.Name = "lbSP";
            this.lbSP.Size = new System.Drawing.Size(50, 23);
            this.lbSP.TabIndex = 110;
            this.lbSP.Text = "SP#:";
            // 
            // lbStyle
            // 
            this.lbStyle.Lines = 0;
            this.lbStyle.Location = new System.Drawing.Point(14, 60);
            this.lbStyle.Name = "lbStyle";
            this.lbStyle.Size = new System.Drawing.Size(50, 23);
            this.lbStyle.TabIndex = 111;
            this.lbStyle.Text = "Style:";
            // 
            // lbLine
            // 
            this.lbLine.Lines = 0;
            this.lbLine.Location = new System.Drawing.Point(14, 87);
            this.lbLine.Name = "lbLine";
            this.lbLine.Size = new System.Drawing.Size(50, 23);
            this.lbLine.TabIndex = 113;
            this.lbLine.Text = "Line#:";
            // 
            // txtLine
            // 
            this.txtLine.BackColor = System.Drawing.Color.White;
            this.txtLine.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SewinglineID", true));
            this.txtLine.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLine.Location = new System.Drawing.Point(66, 87);
            this.txtLine.Name = "txtLine";
            this.txtLine.Size = new System.Drawing.Size(118, 23);
            this.txtLine.TabIndex = 114;
            this.txtLine.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtLine_PopUp);
            this.txtLine.Validating += new System.ComponentModel.CancelEventHandler(this.txtLine_Validating);
            // 
            // DisplayCell
            // 
            this.DisplayCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.DisplayCell.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.DisplayCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.DisplayCell.Location = new System.Drawing.Point(66, 116);
            this.DisplayCell.Name = "DisplayCell";
            this.DisplayCell.Size = new System.Drawing.Size(118, 21);
            this.DisplayCell.TabIndex = 116;
            // 
            // lbCell
            // 
            this.lbCell.Lines = 0;
            this.lbCell.Location = new System.Drawing.Point(14, 114);
            this.lbCell.Name = "lbCell";
            this.lbCell.Size = new System.Drawing.Size(50, 23);
            this.lbCell.TabIndex = 115;
            this.lbCell.Text = "Cell#:";
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.txtRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRemark.Location = new System.Drawing.Point(77, 140);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(521, 23);
            this.txtRemark.TabIndex = 118;
            // 
            // lbRemark
            // 
            this.lbRemark.Lines = 0;
            this.lbRemark.Location = new System.Drawing.Point(14, 140);
            this.lbRemark.Name = "lbRemark";
            this.lbRemark.Size = new System.Drawing.Size(60, 23);
            this.lbRemark.TabIndex = 117;
            this.lbRemark.Text = "Remark:";
            // 
            // lbShift
            // 
            this.lbShift.Lines = 0;
            this.lbShift.Location = new System.Drawing.Point(243, 6);
            this.lbShift.Name = "lbShift";
            this.lbShift.Size = new System.Drawing.Size(50, 23);
            this.lbShift.TabIndex = 119;
            this.lbShift.Text = "Shift:";
            // 
            // lbInspected
            // 
            this.lbInspected.Lines = 0;
            this.lbInspected.Location = new System.Drawing.Point(243, 33);
            this.lbInspected.Name = "lbInspected";
            this.lbInspected.Size = new System.Drawing.Size(100, 23);
            this.lbInspected.TabIndex = 121;
            this.lbInspected.Text = "Qty Inspected:";
            // 
            // lbRejected
            // 
            this.lbRejected.Lines = 0;
            this.lbRejected.Location = new System.Drawing.Point(243, 60);
            this.lbRejected.Name = "lbRejected";
            this.lbRejected.Size = new System.Drawing.Size(100, 23);
            this.lbRejected.TabIndex = 122;
            this.lbRejected.Text = "Qty Rejected:";
            // 
            // lbDefectQty
            // 
            this.lbDefectQty.Lines = 0;
            this.lbDefectQty.Location = new System.Drawing.Point(243, 87);
            this.lbDefectQty.Name = "lbDefectQty";
            this.lbDefectQty.Size = new System.Drawing.Size(100, 23);
            this.lbDefectQty.TabIndex = 123;
            this.lbDefectQty.Text = "Ttl Defect Qty:";
            // 
            // lbDestination
            // 
            this.lbDestination.Lines = 0;
            this.lbDestination.Location = new System.Drawing.Point(243, 114);
            this.lbDestination.Name = "lbDestination";
            this.lbDestination.Size = new System.Drawing.Size(100, 23);
            this.lbDestination.TabIndex = 124;
            this.lbDestination.Text = "Destination:";
            // 
            // NumInspected
            // 
            this.NumInspected.BackColor = System.Drawing.Color.White;
            this.NumInspected.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "InspectQty", true));
            this.NumInspected.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.NumInspected.Location = new System.Drawing.Point(346, 33);
            this.NumInspected.Name = "NumInspected";
            this.NumInspected.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.NumInspected.Size = new System.Drawing.Size(100, 23);
            this.NumInspected.TabIndex = 125;
            this.NumInspected.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // NumRejected
            // 
            this.NumRejected.BackColor = System.Drawing.Color.White;
            this.NumRejected.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "RejectQty", true));
            this.NumRejected.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.NumRejected.Location = new System.Drawing.Point(346, 60);
            this.NumRejected.Name = "NumRejected";
            this.NumRejected.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.NumRejected.Size = new System.Drawing.Size(100, 23);
            this.NumRejected.TabIndex = 126;
            this.NumRejected.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // NumDefect
            // 
            this.NumDefect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.NumDefect.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "DefectQty", true));
            this.NumDefect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.NumDefect.IsSupportEditMode = false;
            this.NumDefect.Location = new System.Drawing.Point(346, 87);
            this.NumDefect.Name = "NumDefect";
            this.NumDefect.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.NumDefect.ReadOnly = true;
            this.NumDefect.Size = new System.Drawing.Size(100, 23);
            this.NumDefect.TabIndex = 127;
            this.NumDefect.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // lbTeam
            // 
            this.lbTeam.Lines = 0;
            this.lbTeam.Location = new System.Drawing.Point(513, 7);
            this.lbTeam.Name = "lbTeam";
            this.lbTeam.Size = new System.Drawing.Size(60, 23);
            this.lbTeam.TabIndex = 129;
            this.lbTeam.Text = "Team:";
            // 
            // lbCPU
            // 
            this.lbCPU.Lines = 0;
            this.lbCPU.Location = new System.Drawing.Point(513, 33);
            this.lbCPU.Name = "lbCPU";
            this.lbCPU.Size = new System.Drawing.Size(60, 23);
            this.lbCPU.TabIndex = 131;
            this.lbCPU.Text = "CPU:";
            // 
            // NumCPU
            // 
            this.NumCPU.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.NumCPU.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.NumCPU.IsSupportEditMode = false;
            this.NumCPU.Location = new System.Drawing.Point(574, 33);
            this.NumCPU.Name = "NumCPU";
            this.NumCPU.NullValue = null;
            this.NumCPU.ReadOnly = true;
            this.NumCPU.Size = new System.Drawing.Size(100, 23);
            this.NumCPU.TabIndex = 132;
            // 
            // lbRFT
            // 
            this.lbRFT.Lines = 0;
            this.lbRFT.Location = new System.Drawing.Point(513, 60);
            this.lbRFT.Name = "lbRFT";
            this.lbRFT.Size = new System.Drawing.Size(60, 23);
            this.lbRFT.TabIndex = 133;
            this.lbRFT.Text = "RFT(%):";
            // 
            // NumRFT
            // 
            this.NumRFT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.NumRFT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.NumRFT.IsSupportEditMode = false;
            this.NumRFT.Location = new System.Drawing.Point(574, 60);
            this.NumRFT.Name = "NumRFT";
            this.NumRFT.NullValue = null;
            this.NumRFT.ReadOnly = true;
            this.NumRFT.Size = new System.Drawing.Size(100, 23);
            this.NumRFT.TabIndex = 134;
            // 
            // lbFactory
            // 
            this.lbFactory.Lines = 0;
            this.lbFactory.Location = new System.Drawing.Point(513, 87);
            this.lbFactory.Name = "lbFactory";
            this.lbFactory.Size = new System.Drawing.Size(60, 23);
            this.lbFactory.TabIndex = 135;
            this.lbFactory.Text = "Factory:";
            // 
            // DisplayFactory
            // 
            this.DisplayFactory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.DisplayFactory.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "FactoryID", true));
            this.DisplayFactory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.DisplayFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.DisplayFactory.Location = new System.Drawing.Point(574, 87);
            this.DisplayFactory.Name = "DisplayFactory";
            this.DisplayFactory.Size = new System.Drawing.Size(98, 21);
            this.DisplayFactory.TabIndex = 136;
            // 
            // btnEncode
            // 
            this.btnEncode.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEncode.Location = new System.Drawing.Point(680, 7);
            this.btnEncode.Name = "btnEncode";
            this.btnEncode.Size = new System.Drawing.Size(100, 25);
            this.btnEncode.TabIndex = 137;
            this.btnEncode.Text = "Encode";
            this.btnEncode.UseVisualStyleBackColor = true;
            this.btnEncode.Click += new System.EventHandler(this.btnEncode_Click);
            // 
            // DisplayStyle
            // 
            this.DisplayStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.DisplayStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.DisplayStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.DisplayStyle.Location = new System.Drawing.Point(66, 62);
            this.DisplayStyle.Name = "DisplayStyle";
            this.DisplayStyle.Size = new System.Drawing.Size(118, 21);
            this.DisplayStyle.TabIndex = 112;
            // 
            // DisplayDest
            // 
            this.DisplayDest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.DisplayDest.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.DisplayDest.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.DisplayDest.Location = new System.Drawing.Point(346, 116);
            this.DisplayDest.Name = "DisplayDest";
            this.DisplayDest.Size = new System.Drawing.Size(252, 21);
            this.DisplayDest.TabIndex = 138;
            // 
            // comboShift
            // 
            this.comboShift._RaiseErrMsg = null;
            this.comboShift._Type = Sci.Trade.Class.ComboDropDownList.ComboDropDownList_Type._None;
            this.comboShift.AddEmpty = false;
            this.comboShift.BackColor = System.Drawing.Color.White;
            this.comboShift.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue2", this.mtbs, "Shift", true));
            this.comboShift.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboShift.FormattingEnabled = true;
            this.comboShift.IsSupportUnselect = true;
            this.comboShift.Location = new System.Drawing.Point(296, 5);
            this.comboShift.Name = "comboShift";
            this.comboShift.Size = new System.Drawing.Size(150, 24);
            this.comboShift.TabIndex = 270;
            this.comboShift.Type = "Responsible";
            // 
            // comboTeam
            // 
            this.comboTeam._RaiseErrMsg = null;
            this.comboTeam._Type = Sci.Trade.Class.ComboDropDownList.ComboDropDownList_Type._None;
            this.comboTeam.AddEmpty = false;
            this.comboTeam.BackColor = System.Drawing.Color.White;
            this.comboTeam.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue2", this.mtbs, "Team", true));
            this.comboTeam.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboTeam.FormattingEnabled = true;
            this.comboTeam.IsSupportUnselect = true;
            this.comboTeam.Location = new System.Drawing.Point(574, 7);
            this.comboTeam.Name = "comboTeam";
            this.comboTeam.Size = new System.Drawing.Size(100, 24);
            this.comboTeam.TabIndex = 271;
            this.comboTeam.Type = "Responsible";
            // 
            // P20
            // 
            this.ClientSize = new System.Drawing.Size(884, 462);
            this.DefaultDetailOrder = "GarmentDefectTypeid,GarmentDefectCodeID";
            this.DefaultOrder = "ID";
            this.GridAlias = "Rft_Detail";
            this.KeyField1 = "ID";
            this.KeyField2 = "ID";
            this.Name = "P20";
            this.Text = "P20.Right First Time";
            this.UniqueExpress = "ID";
            this.WorkAlias = "Rft";
            this.Controls.SetChildIndex(this.tabs, 0);
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).EndInit();
            this.masterpanel.ResumeLayout(false);
            this.masterpanel.PerformLayout();
            this.detailpanel.ResumeLayout(false);
            this.detail2.ResumeLayout(false);
            this.detailpanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.TextBox txtSP;
        private Win.UI.Label lbDate;
        private Win.UI.DateBox CDate;
        private Win.UI.Label lbSP;
        private Win.UI.Label lbStyle;
        private Win.UI.Label lbLine;
        private Win.UI.TextBox txtLine;
        private Win.UI.TextBox txtRemark;
        private Win.UI.Label lbRemark;
        private Win.UI.DisplayBox DisplayCell;
        private Win.UI.Label lbCell;
        private Win.UI.Label lbShift;
        private Win.UI.Label lbInspected;
        private Win.UI.Label lbDestination;
        private Win.UI.Label lbDefectQty;
        private Win.UI.Label lbRejected;
        private Win.UI.NumericBox NumDefect;
        private Win.UI.NumericBox NumRejected;
        private Win.UI.NumericBox NumInspected;
        private Win.UI.Label lbTeam;
        private Win.UI.DisplayBox DisplayFactory;
        private Win.UI.Label lbFactory;
        private Win.UI.NumericBox NumRFT;
        private Win.UI.Label lbRFT;
        private Win.UI.NumericBox NumCPU;
        private Win.UI.Label lbCPU;
        private Win.UI.Button btnEncode;
        private Win.UI.DisplayBox DisplayDest;
        private Win.UI.DisplayBox DisplayStyle;
        private Trade.Class.ComboDropDownList comboShift;
        private Trade.Class.ComboDropDownList comboTeam;
    }
}
