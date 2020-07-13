namespace Sci.Production.Planning
{
    partial class P06
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
            this.panel3 = new Sci.Win.UI.Panel();
            this.gridIcon1 = new Sci.Win.UI.GridIcon();
            this.comboColumnType = new Sci.Win.UI.ComboBox();
            this.checkTargetDate = new Sci.Win.UI.CheckBox();
            this.dateSCIDlvDate = new Sci.Win.UI.DateRange();
            this.labSCIDlv = new Sci.Win.UI.Label();
            this.labColumnType = new Sci.Win.UI.Label();
            this.txtstyle = new Sci.Production.Class.Txtstyle();
            this.labStyle = new Sci.Win.UI.Label();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.labBrandID = new Sci.Win.UI.Label();
            this.labSPNo = new Sci.Win.UI.Label();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.btnQuery = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            this.grid = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.chkIncludeCancelOrder = new Sci.Win.UI.CheckBox();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.chkIncludeCancelOrder);
            this.panel3.Controls.Add(this.gridIcon1);
            this.panel3.Controls.Add(this.comboColumnType);
            this.panel3.Controls.Add(this.checkTargetDate);
            this.panel3.Controls.Add(this.dateSCIDlvDate);
            this.panel3.Controls.Add(this.labSCIDlv);
            this.panel3.Controls.Add(this.labColumnType);
            this.panel3.Controls.Add(this.txtstyle);
            this.panel3.Controls.Add(this.labStyle);
            this.panel3.Controls.Add(this.txtbrand);
            this.panel3.Controls.Add(this.labBrandID);
            this.panel3.Controls.Add(this.labSPNo);
            this.panel3.Controls.Add(this.txtSPNo);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(857, 121);
            this.panel3.TabIndex = 20;
            // 
            // gridIcon1
            // 
            this.gridIcon1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridIcon1.Location = new System.Drawing.Point(719, 83);
            this.gridIcon1.Name = "gridIcon1";
            this.gridIcon1.Size = new System.Drawing.Size(100, 32);
            this.gridIcon1.TabIndex = 55;
            this.gridIcon1.Text = "gridIcon1";
            this.gridIcon1.AppendClick += new System.EventHandler(this.gridIcon1_AppendClick);
            this.gridIcon1.RemoveClick += new System.EventHandler(this.gridIcon1_RemoveClick);
            // 
            // comboColumnType
            // 
            this.comboColumnType.BackColor = System.Drawing.Color.White;
            this.comboColumnType.EditMode = Sci.Win.UI.AdvEditModes.None;
            this.comboColumnType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboColumnType.FormattingEnabled = true;
            this.comboColumnType.IsSupportUnselect = true;
            this.comboColumnType.Location = new System.Drawing.Point(118, 44);
            this.comboColumnType.Name = "comboColumnType";
            this.comboColumnType.OldText = "";
            this.comboColumnType.Size = new System.Drawing.Size(157, 24);
            this.comboColumnType.TabIndex = 3;
            // 
            // checkTargetDate
            // 
            this.checkTargetDate.AutoSize = true;
            this.checkTargetDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkTargetDate.IsSupportEditMode = false;
            this.checkTargetDate.Location = new System.Drawing.Point(19, 80);
            this.checkTargetDate.Name = "checkTargetDate";
            this.checkTargetDate.Size = new System.Drawing.Size(206, 21);
            this.checkTargetDate.TabIndex = 6;
            this.checkTargetDate.Text = "Query have target date data";
            this.checkTargetDate.UseVisualStyleBackColor = true;
            // 
            // dateSCIDlvDate
            // 
            // 
            // 
            // 
            this.dateSCIDlvDate.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSCIDlvDate.DateBox1.Name = "";
            this.dateSCIDlvDate.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDlvDate.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSCIDlvDate.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSCIDlvDate.DateBox2.Name = "";
            this.dateSCIDlvDate.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSCIDlvDate.DateBox2.TabIndex = 1;
            this.dateSCIDlvDate.IsRequired = false;
            this.dateSCIDlvDate.IsSupportEditMode = false;
            this.dateSCIDlvDate.Location = new System.Drawing.Point(395, 43);
            this.dateSCIDlvDate.Name = "dateSCIDlvDate";
            this.dateSCIDlvDate.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDlvDate.TabIndex = 5;
            // 
            // labSCIDlv
            // 
            this.labSCIDlv.Location = new System.Drawing.Point(295, 43);
            this.labSCIDlv.Name = "labSCIDlv";
            this.labSCIDlv.Size = new System.Drawing.Size(97, 23);
            this.labSCIDlv.TabIndex = 54;
            this.labSCIDlv.Text = "SCI Dlv.";
            // 
            // labColumnType
            // 
            this.labColumnType.Location = new System.Drawing.Point(18, 44);
            this.labColumnType.Name = "labColumnType";
            this.labColumnType.Size = new System.Drawing.Size(97, 23);
            this.labColumnType.TabIndex = 52;
            this.labColumnType.Text = "Column Type";
            // 
            // txtstyle
            // 
            this.txtstyle.BackColor = System.Drawing.Color.White;
            this.txtstyle.BrandObjectName = null;
            this.txtstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle.IsSupportEditMode = false;
            this.txtstyle.Location = new System.Drawing.Point(544, 12);
            this.txtstyle.Name = "txtstyle";
            this.txtstyle.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtstyle.Size = new System.Drawing.Size(130, 23);
            this.txtstyle.TabIndex = 2;
            this.txtstyle.TarBrand = null;
            this.txtstyle.TarSeason = null;
            // 
            // labStyle
            // 
            this.labStyle.Location = new System.Drawing.Point(476, 12);
            this.labStyle.Name = "labStyle";
            this.labStyle.Size = new System.Drawing.Size(65, 23);
            this.labStyle.TabIndex = 50;
            this.labStyle.Text = "Style";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.IsSupportEditMode = false;
            this.txtbrand.Location = new System.Drawing.Point(363, 12);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.Any;
            this.txtbrand.Size = new System.Drawing.Size(110, 23);
            this.txtbrand.TabIndex = 1;
            // 
            // labBrandID
            // 
            this.labBrandID.Location = new System.Drawing.Point(295, 12);
            this.labBrandID.Name = "labBrandID";
            this.labBrandID.Size = new System.Drawing.Size(65, 23);
            this.labBrandID.TabIndex = 48;
            this.labBrandID.Text = "Brand";
            // 
            // labSPNo
            // 
            this.labSPNo.Location = new System.Drawing.Point(18, 12);
            this.labSPNo.Name = "labSPNo";
            this.labSPNo.Size = new System.Drawing.Size(97, 23);
            this.labSPNo.TabIndex = 46;
            this.labSPNo.Text = "SP#";
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.IsSupportEditMode = false;
            this.txtSPNo.Location = new System.Drawing.Point(118, 12);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(157, 23);
            this.txtSPNo.TabIndex = 0;
            // 
            // btnQuery
            // 
            this.btnQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnQuery.Location = new System.Drawing.Point(719, 10);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(97, 30);
            this.btnQuery.TabIndex = 6;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // btnSave
            // 
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSave.Location = new System.Drawing.Point(719, 44);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(97, 30);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToResizeRows = false;
            this.grid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid.Location = new System.Drawing.Point(0, 121);
            this.grid.Name = "grid";
            this.grid.RowHeadersVisible = false;
            this.grid.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid.RowTemplate.Height = 24;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.ShowCellToolTips = false;
            this.grid.Size = new System.Drawing.Size(857, 335);
            this.grid.TabIndex = 21;
            this.grid.TabStop = false;
            // 
            // chkIncludeCancelOrder
            // 
            this.chkIncludeCancelOrder.AutoSize = true;
            this.chkIncludeCancelOrder.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIncludeCancelOrder.Location = new System.Drawing.Point(235, 80);
            this.chkIncludeCancelOrder.Name = "chkIncludeCancelOrder";
            this.chkIncludeCancelOrder.Size = new System.Drawing.Size(157, 21);
            this.chkIncludeCancelOrder.TabIndex = 128;
            this.chkIncludeCancelOrder.Text = "Include Cancel order";
            this.chkIncludeCancelOrder.UseVisualStyleBackColor = true;
            // 
            // P06
            // 
            this.ClientSize = new System.Drawing.Size(857, 456);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.panel3);
            this.Name = "P06";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P06. Critical Activity Target Adjust";
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.grid, 0);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel3;
        private Win.UI.Button btnQuery;
        private Win.UI.Button btnSave;
        private Win.UI.Label labSCIDlv;
        private Win.UI.Label labColumnType;
        private Class.Txtstyle txtstyle;
        private Win.UI.Label labStyle;
        private Class.Txtbrand txtbrand;
        private Win.UI.Label labBrandID;
        private Win.UI.Label labSPNo;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.Grid grid;
        private Win.UI.DateRange dateSCIDlvDate;
        private Win.UI.CheckBox checkTargetDate;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.ComboBox comboColumnType;
        private Win.UI.GridIcon gridIcon1;
        private Win.UI.CheckBox chkIncludeCancelOrder;
    }
}
