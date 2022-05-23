namespace Sci.Production.Shipping
{
    partial class P08_ShareExpense_ImportGarment
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
            this.btnQuery = new Sci.Win.UI.Button();
            this.datePulloutDate = new Sci.Win.UI.DateRange();
            this.labelPulloutDate = new Sci.Win.UI.Label();
            this.txtShipmode = new Sci.Production.Class.Txtshipmode();
            this.labelShipMode = new Sci.Win.UI.Label();
            this.txtCountryDestination = new Sci.Production.Class.Txtcountry();
            this.labelDestination = new Sci.Win.UI.Label();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.labelBrand = new Sci.Win.UI.Label();
            this.txtTruck = new Sci.Win.UI.TextBox();
            this.labelTruckNo = new Sci.Win.UI.Label();
            this.txtSubconForwarder = new Sci.Production.Class.TxtsubconNoConfirm();
            this.labelForwarder = new Sci.Win.UI.Label();
            this.dateFCRDate = new Sci.Win.UI.DateRange();
            this.labelFCRDate = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.btnClose = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.panel5 = new Sci.Win.UI.Panel();
            this.gridImport = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.dateOnBoard = new Sci.Win.UI.DateRange();
            this.labOnBoardDate = new Sci.Win.UI.Label();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 605);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(778, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 605);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dateOnBoard);
            this.panel3.Controls.Add(this.labOnBoardDate);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.datePulloutDate);
            this.panel3.Controls.Add(this.labelPulloutDate);
            this.panel3.Controls.Add(this.txtShipmode);
            this.panel3.Controls.Add(this.labelShipMode);
            this.panel3.Controls.Add(this.txtCountryDestination);
            this.panel3.Controls.Add(this.labelDestination);
            this.panel3.Controls.Add(this.txtbrand);
            this.panel3.Controls.Add(this.labelBrand);
            this.panel3.Controls.Add(this.txtTruck);
            this.panel3.Controls.Add(this.labelTruckNo);
            this.panel3.Controls.Add(this.txtSubconForwarder);
            this.panel3.Controls.Add(this.labelForwarder);
            this.panel3.Controls.Add(this.dateFCRDate);
            this.panel3.Controls.Add(this.labelFCRDate);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(773, 126);
            this.panel3.TabIndex = 2;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(675, 85);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 16;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // datePulloutDate
            // 
            // 
            // 
            // 
            this.datePulloutDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.datePulloutDate.DateBox1.Name = "";
            this.datePulloutDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.datePulloutDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.datePulloutDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.datePulloutDate.DateBox2.Name = "";
            this.datePulloutDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.datePulloutDate.DateBox2.TabIndex = 1;
            this.datePulloutDate.IsRequired = false;
            this.datePulloutDate.Location = new System.Drawing.Point(475, 7);
            this.datePulloutDate.Name = "datePulloutDate";
            this.datePulloutDate.Size = new System.Drawing.Size(280, 23);
            this.datePulloutDate.TabIndex = 13;
            // 
            // labelPulloutDate
            // 
            this.labelPulloutDate.Location = new System.Drawing.Point(392, 7);
            this.labelPulloutDate.Name = "labelPulloutDate";
            this.labelPulloutDate.Size = new System.Drawing.Size(79, 23);
            this.labelPulloutDate.TabIndex = 12;
            this.labelPulloutDate.Text = "Pullout Date";
            // 
            // txtShipmode
            // 
            this.txtShipmode.BackColor = System.Drawing.Color.White;
            this.txtShipmode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtShipmode.FormattingEnabled = true;
            this.txtShipmode.IsSupportUnselect = true;
            this.txtShipmode.Location = new System.Drawing.Point(429, 65);
            this.txtShipmode.Name = "txtShipmode";
            this.txtShipmode.OldText = "";
            this.txtShipmode.Size = new System.Drawing.Size(80, 24);
            this.txtShipmode.TabIndex = 11;
            this.txtShipmode.UseFunction = null;
            // 
            // labelShipMode
            // 
            this.labelShipMode.Location = new System.Drawing.Point(350, 66);
            this.labelShipMode.Name = "labelShipMode";
            this.labelShipMode.Size = new System.Drawing.Size(75, 23);
            this.labelShipMode.TabIndex = 10;
            this.labelShipMode.Text = "Ship Mode";
            // 
            // txtCountryDestination
            // 
            this.txtCountryDestination.DisplayBox1Binding = "";
            this.txtCountryDestination.Location = new System.Drawing.Point(106, 66);
            this.txtCountryDestination.Name = "txtCountryDestination";
            this.txtCountryDestination.Size = new System.Drawing.Size(232, 22);
            this.txtCountryDestination.TabIndex = 9;
            this.txtCountryDestination.TextBox1Binding = "";
            // 
            // labelDestination
            // 
            this.labelDestination.Location = new System.Drawing.Point(3, 65);
            this.labelDestination.Name = "labelDestination";
            this.labelDestination.Size = new System.Drawing.Size(101, 23);
            this.labelDestination.TabIndex = 8;
            this.labelDestination.Text = "Destination";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(572, 64);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(84, 23);
            this.txtbrand.TabIndex = 7;
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(525, 65);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(44, 23);
            this.labelBrand.TabIndex = 6;
            this.labelBrand.Text = "Brand";
            // 
            // txtTruck
            // 
            this.txtTruck.BackColor = System.Drawing.Color.White;
            this.txtTruck.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTruck.Location = new System.Drawing.Point(353, 95);
            this.txtTruck.Name = "txtTruck";
            this.txtTruck.Size = new System.Drawing.Size(159, 23);
            this.txtTruck.TabIndex = 5;
            // 
            // labelTruckNo
            // 
            this.labelTruckNo.Location = new System.Drawing.Point(298, 95);
            this.labelTruckNo.Name = "labelTruckNo";
            this.labelTruckNo.Size = new System.Drawing.Size(51, 23);
            this.labelTruckNo.TabIndex = 4;
            this.labelTruckNo.Text = "Truck#";
            // 
            // txtSubconForwarder
            // 
            this.txtSubconForwarder.DisplayBox1Binding = "";
            this.txtSubconForwarder.IsFreightForwarder = false;
            this.txtSubconForwarder.IsIncludeJunk = false;
            this.txtSubconForwarder.IsMisc = false;
            this.txtSubconForwarder.IsShipping = false;
            this.txtSubconForwarder.IsSubcon = false;
            this.txtSubconForwarder.Location = new System.Drawing.Point(106, 95);
            this.txtSubconForwarder.Name = "txtSubconForwarder";
            this.txtSubconForwarder.Size = new System.Drawing.Size(170, 23);
            this.txtSubconForwarder.TabIndex = 3;
            this.txtSubconForwarder.TextBox1Binding = "";
            // 
            // labelForwarder
            // 
            this.labelForwarder.Location = new System.Drawing.Point(3, 94);
            this.labelForwarder.Name = "labelForwarder";
            this.labelForwarder.Size = new System.Drawing.Size(101, 23);
            this.labelForwarder.TabIndex = 2;
            this.labelForwarder.Text = "Forwarder";
            // 
            // dateFCRDate
            // 
            // 
            // 
            // 
            this.dateFCRDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateFCRDate.DateBox1.Name = "";
            this.dateFCRDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateFCRDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateFCRDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateFCRDate.DateBox2.Name = "";
            this.dateFCRDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateFCRDate.DateBox2.TabIndex = 1;
            this.dateFCRDate.IsRequired = false;
            this.dateFCRDate.Location = new System.Drawing.Point(106, 7);
            this.dateFCRDate.Name = "dateFCRDate";
            this.dateFCRDate.Size = new System.Drawing.Size(280, 23);
            this.dateFCRDate.TabIndex = 1;
            // 
            // labelFCRDate
            // 
            this.labelFCRDate.Location = new System.Drawing.Point(4, 7);
            this.labelFCRDate.Name = "labelFCRDate";
            this.labelFCRDate.Size = new System.Drawing.Size(100, 23);
            this.labelFCRDate.TabIndex = 0;
            this.labelFCRDate.Text = "FCR Date";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btnClose);
            this.panel4.Controls.Add(this.btnImport);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 566);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(773, 39);
            this.panel4.TabIndex = 3;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(684, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(589, 4);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(80, 30);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridImport);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 126);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(773, 440);
            this.panel5.TabIndex = 4;
            // 
            // gridImport
            // 
            this.gridImport.AllowUserToAddRows = false;
            this.gridImport.AllowUserToDeleteRows = false;
            this.gridImport.AllowUserToResizeRows = false;
            this.gridImport.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridImport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridImport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.gridImport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridImport.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridImport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridImport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridImport.Location = new System.Drawing.Point(0, 0);
            this.gridImport.Name = "gridImport";
            this.gridImport.RowHeadersVisible = false;
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridImport.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridImport.RowTemplate.Height = 24;
            this.gridImport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridImport.ShowCellToolTips = false;
            this.gridImport.Size = new System.Drawing.Size(773, 440);
            this.gridImport.TabIndex = 0;
            this.gridImport.TabStop = false;
            // 
            // dateOnBoard
            // 
            // 
            // 
            // 
            this.dateOnBoard.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateOnBoard.DateBox1.Name = "";
            this.dateOnBoard.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateOnBoard.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateOnBoard.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateOnBoard.DateBox2.Name = "";
            this.dateOnBoard.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateOnBoard.DateBox2.TabIndex = 1;
            this.dateOnBoard.IsRequired = false;
            this.dateOnBoard.Location = new System.Drawing.Point(106, 36);
            this.dateOnBoard.Name = "dateOnBoard";
            this.dateOnBoard.Size = new System.Drawing.Size(280, 23);
            this.dateOnBoard.TabIndex = 18;
            // 
            // labOnBoardDate
            // 
            this.labOnBoardDate.Location = new System.Drawing.Point(4, 36);
            this.labOnBoardDate.Name = "labOnBoardDate";
            this.labOnBoardDate.Size = new System.Drawing.Size(100, 23);
            this.labOnBoardDate.TabIndex = 17;
            this.labOnBoardDate.Text = "On Board Date";
            // 
            // P08_ShareExpense_ImportGarment
            // 
            this.AcceptButton = this.btnImport;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(783, 605);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "P08_ShareExpense_ImportGarment";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Import - Garment";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridImport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Win.UI.Button btnQuery;
        private Win.UI.DateRange datePulloutDate;
        private Win.UI.Label labelPulloutDate;
        private Class.Txtshipmode txtShipmode;
        private Win.UI.Label labelShipMode;
        private Class.Txtcountry txtCountryDestination;
        private Win.UI.Label labelDestination;
        private Class.Txtbrand txtbrand;
        private Win.UI.Label labelBrand;
        private Win.UI.TextBox txtTruck;
        private Win.UI.Label labelTruckNo;
        private Class.TxtsubconNoConfirm txtSubconForwarder;
        private Win.UI.Label labelForwarder;
        private Win.UI.DateRange dateFCRDate;
        private Win.UI.Label labelFCRDate;
        private Win.UI.Panel panel4;
        private Win.UI.Button btnClose;
        private Win.UI.Button btnImport;
        private Win.UI.Panel panel5;
        private Win.UI.Grid gridImport;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.DateRange dateOnBoard;
        private Win.UI.Label labOnBoardDate;
    }
}
