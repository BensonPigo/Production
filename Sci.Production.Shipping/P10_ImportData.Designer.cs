namespace Sci.Production.Shipping
{
    partial class P10_ImportData
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
            this.labCancelOrder = new System.Windows.Forms.Label();
            this.comboContainerType = new Sci.Win.UI.ComboBox();
            this.label1 = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            this.txtSPNo = new Sci.Win.UI.TextBox();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.txtSubconForwarder = new Sci.Production.Class.TxtsubconNoConfirm();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.labelBrand = new Sci.Win.UI.Label();
            this.txtshipmode = new Sci.Production.Class.Txtshipmode();
            this.labelShipMode = new Sci.Win.UI.Label();
            this.dateCutoffDate = new Sci.Win.UI.DateBox();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.txtGBNoEnd = new Sci.Win.UI.TextBox();
            this.label4 = new Sci.Win.UI.Label();
            this.txtGBNoStart = new Sci.Win.UI.TextBox();
            this.labelForwarder = new Sci.Win.UI.Label();
            this.labelCutoffDate = new Sci.Win.UI.Label();
            this.labelGB = new Sci.Win.UI.Label();
            this.panel4 = new Sci.Win.UI.Panel();
            this.panel5 = new Sci.Win.UI.Panel();
            this.panel7 = new Sci.Win.UI.Panel();
            this.grid2 = new Sci.Win.UI.Grid();
            this.listControlBindingSource2 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel6 = new Sci.Win.UI.Panel();
            this.panel9 = new Sci.Win.UI.Panel();
            this.grid1 = new Sci.Win.UI.Grid();
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel8 = new Sci.Win.UI.Panel();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnImportData = new Sci.Win.UI.Button();
            this.dateIDD = new Sci.Win.UI.DateBox();
            this.label3 = new Sci.Win.UI.Label();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(5, 612);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(784, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(5, 612);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dateIDD);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.labCancelOrder);
            this.panel3.Controls.Add(this.comboContainerType);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.btnQuery);
            this.panel3.Controls.Add(this.txtSPNo);
            this.panel3.Controls.Add(this.labelSPNo);
            this.panel3.Controls.Add(this.txtSubconForwarder);
            this.panel3.Controls.Add(this.txtbrand);
            this.panel3.Controls.Add(this.labelBrand);
            this.panel3.Controls.Add(this.txtshipmode);
            this.panel3.Controls.Add(this.labelShipMode);
            this.panel3.Controls.Add(this.dateCutoffDate);
            this.panel3.Controls.Add(this.dateBuyerDelivery);
            this.panel3.Controls.Add(this.labelBuyerDelivery);
            this.panel3.Controls.Add(this.txtGBNoEnd);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.txtGBNoStart);
            this.panel3.Controls.Add(this.labelForwarder);
            this.panel3.Controls.Add(this.labelCutoffDate);
            this.panel3.Controls.Add(this.labelGB);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(5, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(779, 124);
            this.panel3.TabIndex = 2;
            // 
            // labCancelOrder
            // 
            this.labCancelOrder.AutoSize = true;
            this.labCancelOrder.ForeColor = System.Drawing.Color.Red;
            this.labCancelOrder.Location = new System.Drawing.Point(278, 99);
            this.labCancelOrder.Name = "labCancelOrder";
            this.labCancelOrder.Size = new System.Drawing.Size(286, 17);
            this.labCancelOrder.TabIndex = 18;
            this.labCancelOrder.Text = "Cancel order cannot import in the Ship Plan.";
            // 
            // comboContainerType
            // 
            this.comboContainerType.BackColor = System.Drawing.Color.White;
            this.comboContainerType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboContainerType.FormattingEnabled = true;
            this.comboContainerType.IsSupportUnselect = true;
            this.comboContainerType.Location = new System.Drawing.Point(563, 66);
            this.comboContainerType.Name = "comboContainerType";
            this.comboContainerType.OldText = "";
            this.comboContainerType.Size = new System.Drawing.Size(121, 24);
            this.comboContainerType.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(468, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 23);
            this.label1.TabIndex = 17;
            this.label1.Text = "Loading Type";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(688, 64);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 16;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // txtSPNo
            // 
            this.txtSPNo.BackColor = System.Drawing.Color.White;
            this.txtSPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPNo.Location = new System.Drawing.Point(326, 67);
            this.txtSPNo.Name = "txtSPNo";
            this.txtSPNo.Size = new System.Drawing.Size(120, 23);
            this.txtSPNo.TabIndex = 15;
            // 
            // labelSPNo
            // 
            this.labelSPNo.Location = new System.Drawing.Point(281, 67);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(41, 23);
            this.labelSPNo.TabIndex = 14;
            this.labelSPNo.Text = "SP#";
            // 
            // txtSubconForwarder
            // 
            this.txtSubconForwarder.DisplayBox1Binding = "";
            this.txtSubconForwarder.IsIncludeJunk = false;
            this.txtSubconForwarder.IsMisc = false;
            this.txtSubconForwarder.IsShipping = false;
            this.txtSubconForwarder.IsSubcon = false;
            this.txtSubconForwarder.Location = new System.Drawing.Point(83, 67);
            this.txtSubconForwarder.Name = "txtSubconForwarder";
            this.txtSubconForwarder.Size = new System.Drawing.Size(170, 23);
            this.txtSubconForwarder.TabIndex = 13;
            this.txtSubconForwarder.TextBox1Binding = "";
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(563, 37);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(66, 23);
            this.txtbrand.TabIndex = 12;
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(511, 37);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(48, 23);
            this.labelBrand.TabIndex = 11;
            this.labelBrand.Text = "Brand";
            // 
            // txtshipmode
            // 
            this.txtshipmode.BackColor = System.Drawing.Color.White;
            this.txtshipmode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtshipmode.FormattingEnabled = true;
            this.txtshipmode.IsSupportUnselect = true;
            this.txtshipmode.Location = new System.Drawing.Point(359, 35);
            this.txtshipmode.Name = "txtshipmode";
            this.txtshipmode.OldText = "";
            this.txtshipmode.Size = new System.Drawing.Size(85, 24);
            this.txtshipmode.TabIndex = 10;
            this.txtshipmode.UseFunction = "ORDER";
            // 
            // labelShipMode
            // 
            this.labelShipMode.Location = new System.Drawing.Point(281, 36);
            this.labelShipMode.Name = "labelShipMode";
            this.labelShipMode.Size = new System.Drawing.Size(75, 23);
            this.labelShipMode.TabIndex = 9;
            this.labelShipMode.Text = "Ship Mode";
            // 
            // dateCutoffDate
            // 
            this.dateCutoffDate.Location = new System.Drawing.Point(88, 37);
            this.dateCutoffDate.Name = "dateCutoffDate";
            this.dateCutoffDate.Size = new System.Drawing.Size(130, 23);
            this.dateCutoffDate.TabIndex = 8;
            // 
            // dateBuyerDelivery
            // 
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerDelivery.DateBox1.Name = "";
            this.dateBuyerDelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerDelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBuyerDelivery.DateBox2.Name = "";
            this.dateBuyerDelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerDelivery.DateBox2.TabIndex = 1;
            this.dateBuyerDelivery.IsRequired = false;
            this.dateBuyerDelivery.Location = new System.Drawing.Point(481, 7);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 7;
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Location = new System.Drawing.Point(383, 6);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(95, 23);
            this.labelBuyerDelivery.TabIndex = 6;
            this.labelBuyerDelivery.Text = "Buyer Delivery";
            // 
            // txtGBNoEnd
            // 
            this.txtGBNoEnd.BackColor = System.Drawing.Color.White;
            this.txtGBNoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtGBNoEnd.Location = new System.Drawing.Point(202, 7);
            this.txtGBNoEnd.Name = "txtGBNoEnd";
            this.txtGBNoEnd.Size = new System.Drawing.Size(120, 23);
            this.txtGBNoEnd.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(178, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(20, 23);
            this.label4.TabIndex = 4;
            this.label4.Text = "～";
            this.label4.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label4.TextStyle.Color = System.Drawing.Color.Black;
            this.label4.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label4.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtGBNoStart
            // 
            this.txtGBNoStart.BackColor = System.Drawing.Color.White;
            this.txtGBNoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtGBNoStart.Location = new System.Drawing.Point(60, 7);
            this.txtGBNoStart.Name = "txtGBNoStart";
            this.txtGBNoStart.Size = new System.Drawing.Size(120, 23);
            this.txtGBNoStart.TabIndex = 3;
            // 
            // labelForwarder
            // 
            this.labelForwarder.Location = new System.Drawing.Point(4, 67);
            this.labelForwarder.Name = "labelForwarder";
            this.labelForwarder.Size = new System.Drawing.Size(75, 23);
            this.labelForwarder.TabIndex = 2;
            this.labelForwarder.Text = "Forwarder";
            // 
            // labelCutoffDate
            // 
            this.labelCutoffDate.Location = new System.Drawing.Point(4, 37);
            this.labelCutoffDate.Name = "labelCutoffDate";
            this.labelCutoffDate.Size = new System.Drawing.Size(80, 23);
            this.labelCutoffDate.TabIndex = 1;
            this.labelCutoffDate.Text = "Cut-off Date";
            // 
            // labelGB
            // 
            this.labelGB.Location = new System.Drawing.Point(4, 7);
            this.labelGB.Name = "labelGB";
            this.labelGB.Size = new System.Drawing.Size(52, 23);
            this.labelGB.TabIndex = 0;
            this.labelGB.Text = "GB#";
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(5, 607);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(779, 5);
            this.panel4.TabIndex = 3;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.panel7);
            this.panel5.Controls.Add(this.panel6);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(5, 124);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(779, 483);
            this.panel5.TabIndex = 4;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.grid2);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(0, 307);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(779, 176);
            this.panel7.TabIndex = 1;
            // 
            // grid2
            // 
            this.grid2.AllowUserToAddRows = false;
            this.grid2.AllowUserToDeleteRows = false;
            this.grid2.AllowUserToResizeRows = false;
            this.grid2.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid2.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid2.DataSource = this.listControlBindingSource2;
            this.grid2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid2.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid2.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid2.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid2.Location = new System.Drawing.Point(0, 0);
            this.grid2.Name = "grid2";
            this.grid2.RowHeadersVisible = false;
            this.grid2.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid2.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid2.RowTemplate.Height = 24;
            this.grid2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid2.ShowCellToolTips = false;
            this.grid2.Size = new System.Drawing.Size(779, 176);
            this.grid2.TabIndex = 0;
            this.grid2.TabStop = false;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.panel9);
            this.panel6.Controls.Add(this.panel8);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(779, 307);
            this.panel6.TabIndex = 0;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.grid1);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(779, 267);
            this.panel9.TabIndex = 1;
            // 
            // grid1
            // 
            this.grid1.AllowUserToAddRows = false;
            this.grid1.AllowUserToDeleteRows = false;
            this.grid1.AllowUserToResizeRows = false;
            this.grid1.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid1.DataSource = this.listControlBindingSource1;
            this.grid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid1.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid1.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid1.Location = new System.Drawing.Point(0, 0);
            this.grid1.Name = "grid1";
            this.grid1.RowHeadersVisible = false;
            this.grid1.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid1.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid1.RowTemplate.Height = 24;
            this.grid1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid1.ShowCellToolTips = false;
            this.grid1.Size = new System.Drawing.Size(779, 267);
            this.grid1.TabIndex = 0;
            this.grid1.TabStop = false;
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.btnCancel);
            this.panel8.Controls.Add(this.btnImportData);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel8.Location = new System.Drawing.Point(0, 267);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(779, 40);
            this.panel8.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(688, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // btnImportData
            // 
            this.btnImportData.Location = new System.Drawing.Point(563, 4);
            this.btnImportData.Name = "btnImportData";
            this.btnImportData.Size = new System.Drawing.Size(116, 30);
            this.btnImportData.TabIndex = 0;
            this.btnImportData.Text = "Import Data";
            this.btnImportData.UseVisualStyleBackColor = true;
            this.btnImportData.Click += new System.EventHandler(this.BtnImportData_Click);
            // 
            // dateIDD
            // 
            this.dateIDD.Location = new System.Drawing.Point(123, 94);
            this.dateIDD.Name = "dateIDD";
            this.dateIDD.Size = new System.Drawing.Size(130, 23);
            this.dateIDD.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(4, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 23);
            this.label3.TabIndex = 19;
            this.label3.Text = "Intended Delivery";
            // 
            // P10_ImportData
            // 
            this.AcceptButton = this.btnImportData;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(789, 612);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "P10_ImportData";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "Import Data";
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource2)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel8.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Panel panel2;
        private Win.UI.Panel panel3;
        private Class.Txtshipmode txtshipmode;
        private Win.UI.Label labelShipMode;
        private Win.UI.DateBox dateCutoffDate;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.TextBox txtGBNoEnd;
        private Win.UI.Label label4;
        private Win.UI.TextBox txtGBNoStart;
        private Win.UI.Label labelForwarder;
        private Win.UI.Label labelCutoffDate;
        private Win.UI.Label labelGB;
        private Win.UI.Panel panel4;
        private Win.UI.Panel panel5;
        private Win.UI.Panel panel7;
        private Win.UI.Panel panel6;
        private Win.UI.Button btnQuery;
        private Win.UI.TextBox txtSPNo;
        private Win.UI.Label labelSPNo;
        private Class.TxtsubconNoConfirm txtSubconForwarder;
        private Class.Txtbrand txtbrand;
        private Win.UI.Label labelBrand;
        private Win.UI.Grid grid2;
        private Win.UI.ListControlBindingSource listControlBindingSource2;
        private Win.UI.Panel panel9;
        private Win.UI.Grid grid1;
        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel8;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnImportData;
        private Win.UI.Label label1;
        private Win.UI.ComboBox comboContainerType;
        private System.Windows.Forms.Label labCancelOrder;
        private Win.UI.DateBox dateIDD;
        private Win.UI.Label label3;
    }
}
