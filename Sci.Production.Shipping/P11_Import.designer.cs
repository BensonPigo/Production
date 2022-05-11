namespace Sci.Production.Shipping
{
    partial class P11_Import
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
            this.groupBox2 = new Sci.Win.UI.GroupBox();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnImport = new Sci.Win.UI.Button();
            this.gridGMTbs = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.groupBox1 = new Sci.Win.UI.GroupBox();
            this.txtBrand = new Sci.Production.Class.Txtbrand();
            this.label2 = new Sci.Win.UI.Label();
            this.txtGBTo = new Sci.Win.UI.TextBox();
            this.label1 = new Sci.Win.UI.Label();
            this.txGBFrom = new Sci.Win.UI.TextBox();
            this.dateETD = new Sci.Win.UI.DateRange();
            this.labETD = new Sci.Win.UI.Label();
            this.btnQuery = new Sci.Win.UI.Button();
            this.gridGMTBooking = new Sci.Win.UI.Grid();
            this.label3 = new Sci.Win.UI.Label();
            this.txtShipper = new Sci.Win.UI.TextBox();
            this.label4 = new Sci.Win.UI.Label();
            this.numbTtlAmount = new Sci.Win.UI.NumericBox();
            this.numbTtlQty = new Sci.Win.UI.NumericBox();
            this.label10 = new Sci.Win.UI.Label();
            this.label11 = new Sci.Win.UI.Label();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridGMTbs)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridGMTBooking)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numbTtlAmount);
            this.groupBox2.Controls.Add(this.numbTtlQty);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnImport);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox2.Location = new System.Drawing.Point(0, 460);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1007, 70);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(898, 32);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnImport.Location = new System.Drawing.Point(802, 33);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(90, 30);
            this.btnImport.TabIndex = 0;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtShipper);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtBrand);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtGBTo);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txGBFrom);
            this.groupBox1.Controls.Add(this.dateETD);
            this.groupBox1.Controls.Add(this.labETD);
            this.groupBox1.Controls.Add(this.btnQuery);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1007, 150);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(129, 110);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(160, 23);
            this.txtBrand.TabIndex = 101;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(25, 110);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 23);
            this.label2.TabIndex = 15;
            this.label2.Text = "Brand";
            // 
            // txtGBTo
            // 
            this.txtGBTo.BackColor = System.Drawing.Color.White;
            this.txtGBTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtGBTo.Location = new System.Drawing.Point(316, 48);
            this.txtGBTo.MaxLength = 25;
            this.txtGBTo.Name = "txtGBTo";
            this.txtGBTo.Size = new System.Drawing.Size(160, 23);
            this.txtGBTo.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(292, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 23);
            this.label1.TabIndex = 11;
            this.label1.Text = "～";
            this.label1.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label1.TextStyle.Color = System.Drawing.Color.Black;
            this.label1.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label1.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txGBFrom
            // 
            this.txGBFrom.BackColor = System.Drawing.Color.White;
            this.txGBFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txGBFrom.Location = new System.Drawing.Point(129, 48);
            this.txGBFrom.MaxLength = 25;
            this.txGBFrom.Name = "txGBFrom";
            this.txGBFrom.Size = new System.Drawing.Size(160, 23);
            this.txGBFrom.TabIndex = 0;
            // 
            // dateETD
            // 
            // 
            // 
            // 
            this.dateETD.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateETD.DateBox1.Name = "";
            this.dateETD.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateETD.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateETD.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateETD.DateBox2.Name = "";
            this.dateETD.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateETD.DateBox2.TabIndex = 1;
            this.dateETD.IsRequired = false;
            this.dateETD.Location = new System.Drawing.Point(129, 17);
            this.dateETD.Name = "dateETD";
            this.dateETD.Size = new System.Drawing.Size(280, 23);
            this.dateETD.TabIndex = 3;
            // 
            // labETD
            // 
            this.labETD.BackColor = System.Drawing.Color.SkyBlue;
            this.labETD.Location = new System.Drawing.Point(25, 16);
            this.labETD.Name = "labETD";
            this.labETD.Size = new System.Drawing.Size(101, 23);
            this.labETD.TabIndex = 13;
            this.labETD.Text = "On Board Date";
            this.labETD.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnQuery.Location = new System.Drawing.Point(901, 22);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(90, 30);
            this.btnQuery.TabIndex = 8;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // gridGMTBooking
            // 
            this.gridGMTBooking.AllowUserToAddRows = false;
            this.gridGMTBooking.AllowUserToDeleteRows = false;
            this.gridGMTBooking.AllowUserToResizeRows = false;
            this.gridGMTBooking.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridGMTBooking.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridGMTBooking.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridGMTBooking.DataSource = this.gridGMTbs;
            this.gridGMTBooking.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridGMTBooking.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridGMTBooking.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridGMTBooking.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridGMTBooking.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridGMTBooking.Location = new System.Drawing.Point(0, 150);
            this.gridGMTBooking.Name = "gridGMTBooking";
            this.gridGMTBooking.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridGMTBooking.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridGMTBooking.RowTemplate.Height = 24;
            this.gridGMTBooking.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridGMTBooking.ShowCellToolTips = false;
            this.gridGMTBooking.Size = new System.Drawing.Size(1007, 310);
            this.gridGMTBooking.TabIndex = 0;
            this.gridGMTBooking.TabStop = false;
            this.gridGMTBooking.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.GridGMTBooking_ColumnHeaderMouseClick);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(25, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 23);
            this.label3.TabIndex = 102;
            this.label3.Text = "Shipper";
            // 
            // txtShipper
            // 
            this.txtShipper.BackColor = System.Drawing.Color.White;
            this.txtShipper.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtShipper.Location = new System.Drawing.Point(129, 78);
            this.txtShipper.MaxLength = 25;
            this.txtShipper.Name = "txtShipper";
            this.txtShipper.Size = new System.Drawing.Size(160, 23);
            this.txtShipper.TabIndex = 103;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(25, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 23);
            this.label4.TabIndex = 104;
            this.label4.Text = "GB#";
            // 
            // numbTtlAmount
            // 
            this.numbTtlAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numbTtlAmount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numbTtlAmount.IsSupportEditMode = false;
            this.numbTtlAmount.Location = new System.Drawing.Point(690, 36);
            this.numbTtlAmount.Name = "numbTtlAmount";
            this.numbTtlAmount.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numbTtlAmount.ReadOnly = true;
            this.numbTtlAmount.Size = new System.Drawing.Size(98, 23);
            this.numbTtlAmount.TabIndex = 10;
            this.numbTtlAmount.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numbTtlQty
            // 
            this.numbTtlQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numbTtlQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numbTtlQty.IsSupportEditMode = false;
            this.numbTtlQty.Location = new System.Drawing.Point(572, 37);
            this.numbTtlQty.Name = "numbTtlQty";
            this.numbTtlQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numbTtlQty.ReadOnly = true;
            this.numbTtlQty.Size = new System.Drawing.Size(97, 23);
            this.numbTtlQty.TabIndex = 9;
            this.numbTtlQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(687, 11);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(106, 23);
            this.label10.TabIndex = 8;
            this.label10.Text = "Total Amount";
            this.label10.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label10.TextStyle.Color = System.Drawing.Color.Black;
            this.label10.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label10.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(572, 12);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(97, 23);
            this.label11.TabIndex = 7;
            this.label11.Text = "Total Q\'ty";
            this.label11.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label11.TextStyle.Color = System.Drawing.Color.Black;
            this.label11.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label11.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // P11_Import
            // 
            this.ClientSize = new System.Drawing.Size(1007, 530);
            this.Controls.Add(this.gridGMTBooking);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.DefaultControl = "txtInvNo1";
            this.Name = "P11_Import";
            this.OnLineHelpID = "Sci.Win.Subs.Base";
            this.Text = "P11. Import Garment Booking";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridGMTbs)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridGMTBooking)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.GroupBox groupBox2;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnImport;
        private Win.UI.ListControlBindingSource gridGMTbs;
        private Win.UI.GroupBox groupBox1;
        private Win.UI.Button btnQuery;
        private Win.UI.Label labETD;
        private Win.UI.DateRange dateETD;
        private Win.UI.Grid gridGMTBooking;
        private Win.UI.TextBox txtGBTo;
        private Win.UI.Label label1;
        private Win.UI.TextBox txGBFrom;
        private Win.UI.Label label2;
        private Class.Txtbrand txtBrand;
        private Win.UI.Label label4;
        private Win.UI.TextBox txtShipper;
        private Win.UI.Label label3;
        private Win.UI.NumericBox numbTtlAmount;
        private Win.UI.NumericBox numbTtlQty;
        private Win.UI.Label label10;
        private Win.UI.Label label11;
    }
}
