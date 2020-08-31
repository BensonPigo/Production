namespace Sci.Production.Logistic
{
    partial class P01_BatchUpdateIDD
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(P01_BatchUpdateIDD));
            this.gridIDD = new Sci.Win.UI.Grid();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.txtFactory = new Sci.Production.Class.Txtfactory();
            this.txtBrand = new Sci.Production.Class.Txtbrand();
            this.txtStyle = new Sci.Production.Class.Txtstyle();
            this.txtDest = new Sci.Production.Class.Txtcountry();
            this.txtSPFrom = new Sci.Win.UI.TextBox();
            this.txtSPTo = new Sci.Win.UI.TextBox();
            this.label8 = new Sci.Win.UI.Label();
            this.dateSewingOffline = new Sci.Win.UI.DateRange();
            this.dateBuyerdelivery = new Sci.Win.UI.DateRange();
            this.btnQuery = new Sci.Win.UI.Button();
            this.btnUpdate = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.chkExcludeExistsGB = new Sci.Win.UI.CheckBox();
            this.label9 = new Sci.Win.UI.Label();
            this.dateIDD = new Sci.Win.UI.DateBox();
            this.btnUpdateGridIDD = new Sci.Win.UI.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridIDD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdateGridIDD)).BeginInit();
            this.SuspendLayout();
            // 
            // gridIDD
            // 
            this.gridIDD.AllowUserToAddRows = false;
            this.gridIDD.AllowUserToDeleteRows = false;
            this.gridIDD.AllowUserToResizeRows = false;
            this.gridIDD.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridIDD.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridIDD.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridIDD.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridIDD.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridIDD.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridIDD.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridIDD.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridIDD.Location = new System.Drawing.Point(12, 133);
            this.gridIDD.Name = "gridIDD";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.gridIDD.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.gridIDD.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridIDD.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridIDD.RowTemplate.Height = 24;
            this.gridIDD.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridIDD.ShowCellToolTips = false;
            this.gridIDD.Size = new System.Drawing.Size(874, 372);
            this.gridIDD.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.gridIDD.TabIndex = 1;
            this.gridIDD.Sorted += new System.EventHandler(this.GridIDD_Sorted);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "Factory";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 3;
            this.label2.Text = "Brand";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "Style";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 5;
            this.label4.Text = "Destination";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.SkyBlue;
            this.label5.Location = new System.Drawing.Point(347, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 23);
            this.label5.TabIndex = 6;
            this.label5.Text = "SP#";
            this.label5.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.SkyBlue;
            this.label6.Location = new System.Drawing.Point(347, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 23);
            this.label6.TabIndex = 7;
            this.label6.Text = "Sewing Offline";
            this.label6.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.SkyBlue;
            this.label7.Location = new System.Drawing.Point(347, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 23);
            this.label7.TabIndex = 8;
            this.label7.Text = "Buyer Delivery";
            this.label7.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtFactory
            // 
            this.txtFactory.BackColor = System.Drawing.Color.White;
            this.txtFactory.BoolFtyGroupList = true;
            this.txtFactory.FilteMDivision = true;
            this.txtFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactory.IsProduceFty = false;
            this.txtFactory.IssupportJunk = false;
            this.txtFactory.Location = new System.Drawing.Point(90, 9);
            this.txtFactory.MDivision = null;
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Size = new System.Drawing.Size(66, 23);
            this.txtFactory.TabIndex = 9;
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(90, 38);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(66, 23);
            this.txtBrand.TabIndex = 10;
            // 
            // txtStyle
            // 
            this.txtStyle.BackColor = System.Drawing.Color.White;
            this.txtStyle.BrandObjectName = null;
            this.txtStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtStyle.Location = new System.Drawing.Point(90, 67);
            this.txtStyle.Name = "txtStyle";
            this.txtStyle.Size = new System.Drawing.Size(130, 23);
            this.txtStyle.TabIndex = 11;
            this.txtStyle.TarBrand = null;
            this.txtStyle.TarSeason = null;
            // 
            // txtDest
            // 
            this.txtDest.DisplayBox1Binding = "";
            this.txtDest.Location = new System.Drawing.Point(90, 97);
            this.txtDest.Name = "txtDest";
            this.txtDest.Size = new System.Drawing.Size(232, 22);
            this.txtDest.TabIndex = 12;
            this.txtDest.TextBox1Binding = "";
            // 
            // txtSPFrom
            // 
            this.txtSPFrom.BackColor = System.Drawing.Color.White;
            this.txtSPFrom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPFrom.Location = new System.Drawing.Point(449, 9);
            this.txtSPFrom.Name = "txtSPFrom";
            this.txtSPFrom.Size = new System.Drawing.Size(116, 23);
            this.txtSPFrom.TabIndex = 13;
            // 
            // txtSPTo
            // 
            this.txtSPTo.BackColor = System.Drawing.Color.White;
            this.txtSPTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSPTo.Location = new System.Drawing.Point(592, 9);
            this.txtSPTo.Name = "txtSPTo";
            this.txtSPTo.Size = new System.Drawing.Size(116, 23);
            this.txtSPTo.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(569, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(20, 23);
            this.label8.TabIndex = 15;
            this.label8.Text = "～";
            this.label8.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // dateSewingOffline
            // 
            // 
            // 
            // 
            this.dateSewingOffline.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateSewingOffline.DateBox1.Name = "";
            this.dateSewingOffline.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateSewingOffline.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateSewingOffline.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateSewingOffline.DateBox2.Name = "";
            this.dateSewingOffline.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateSewingOffline.DateBox2.TabIndex = 1;
            this.dateSewingOffline.Location = new System.Drawing.Point(449, 38);
            this.dateSewingOffline.Name = "dateSewingOffline";
            this.dateSewingOffline.Size = new System.Drawing.Size(280, 23);
            this.dateSewingOffline.TabIndex = 16;
            // 
            // dateBuyerdelivery
            // 
            // 
            // 
            // 
            this.dateBuyerdelivery.DateBox1.Location = new System.Drawing.Point(0, 0);
            this.dateBuyerdelivery.DateBox1.Name = "";
            this.dateBuyerdelivery.DateBox1.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerdelivery.DateBox1.TabIndex = 0;
            // 
            // 
            // 
            this.dateBuyerdelivery.DateBox2.Location = new System.Drawing.Point(151, 0);
            this.dateBuyerdelivery.DateBox2.Name = "";
            this.dateBuyerdelivery.DateBox2.Size = new System.Drawing.Size(129, 23);
            this.dateBuyerdelivery.DateBox2.TabIndex = 1;
            this.dateBuyerdelivery.Location = new System.Drawing.Point(449, 67);
            this.dateBuyerdelivery.Name = "dateBuyerdelivery";
            this.dateBuyerdelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerdelivery.TabIndex = 17;
            // 
            // btnQuery
            // 
            this.btnQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuery.Location = new System.Drawing.Point(806, 5);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 18;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Location = new System.Drawing.Point(806, 41);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(80, 30);
            this.btnUpdate.TabIndex = 19;
            this.btnUpdate.Text = "Update";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(806, 77);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 20;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // chkExcludeExistsGB
            // 
            this.chkExcludeExistsGB.AutoSize = true;
            this.chkExcludeExistsGB.Checked = true;
            this.chkExcludeExistsGB.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExcludeExistsGB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkExcludeExistsGB.Location = new System.Drawing.Point(347, 98);
            this.chkExcludeExistsGB.Name = "chkExcludeExistsGB";
            this.chkExcludeExistsGB.Size = new System.Drawing.Size(369, 21);
            this.chkExcludeExistsGB.TabIndex = 21;
            this.chkExcludeExistsGB.Text = "Exclude shipment already created in Garment Booking";
            this.chkExcludeExistsGB.UseVisualStyleBackColor = true;
            this.chkExcludeExistsGB.CheckedChanged += new System.EventHandler(this.ChkExcludeExistsGB_CheckedChanged);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.Location = new System.Drawing.Point(12, 510);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(116, 23);
            this.label9.TabIndex = 22;
            this.label9.Text = "Intended Delivery";
            // 
            // dateIDD
            // 
            this.dateIDD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dateIDD.Location = new System.Drawing.Point(131, 510);
            this.dateIDD.Name = "dateIDD";
            this.dateIDD.Size = new System.Drawing.Size(130, 23);
            this.dateIDD.TabIndex = 23;
            // 
            // btnUpdateGridIDD
            // 
            this.btnUpdateGridIDD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnUpdateGridIDD.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUpdateGridIDD.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdateGridIDD.Image")));
            this.btnUpdateGridIDD.Location = new System.Drawing.Point(267, 510);
            this.btnUpdateGridIDD.Name = "btnUpdateGridIDD";
            this.btnUpdateGridIDD.Size = new System.Drawing.Size(39, 23);
            this.btnUpdateGridIDD.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnUpdateGridIDD.TabIndex = 24;
            this.btnUpdateGridIDD.TabStop = false;
            this.btnUpdateGridIDD.WaitOnLoad = true;
            this.btnUpdateGridIDD.Click += new System.EventHandler(this.BtnUpdateGridIDD_Click);
            // 
            // P01_BatchUpdateIDD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 539);
            this.Controls.Add(this.btnUpdateGridIDD);
            this.Controls.Add(this.dateIDD);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.chkExcludeExistsGB);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnUpdate);
            this.Controls.Add(this.btnQuery);
            this.Controls.Add(this.dateBuyerdelivery);
            this.Controls.Add(this.dateSewingOffline);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtSPTo);
            this.Controls.Add(this.txtSPFrom);
            this.Controls.Add(this.txtDest);
            this.Controls.Add(this.txtStyle);
            this.Controls.Add(this.txtBrand);
            this.Controls.Add(this.txtFactory);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.gridIDD);
            this.Name = "P01_BatchUpdateIDD";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "Batch Update Intended delivery date";
            this.Controls.SetChildIndex(this.gridIDD, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.label7, 0);
            this.Controls.SetChildIndex(this.txtFactory, 0);
            this.Controls.SetChildIndex(this.txtBrand, 0);
            this.Controls.SetChildIndex(this.txtStyle, 0);
            this.Controls.SetChildIndex(this.txtDest, 0);
            this.Controls.SetChildIndex(this.txtSPFrom, 0);
            this.Controls.SetChildIndex(this.txtSPTo, 0);
            this.Controls.SetChildIndex(this.label8, 0);
            this.Controls.SetChildIndex(this.dateSewingOffline, 0);
            this.Controls.SetChildIndex(this.dateBuyerdelivery, 0);
            this.Controls.SetChildIndex(this.btnQuery, 0);
            this.Controls.SetChildIndex(this.btnUpdate, 0);
            this.Controls.SetChildIndex(this.btnClose, 0);
            this.Controls.SetChildIndex(this.chkExcludeExistsGB, 0);
            this.Controls.SetChildIndex(this.label9, 0);
            this.Controls.SetChildIndex(this.dateIDD, 0);
            this.Controls.SetChildIndex(this.btnUpdateGridIDD, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridIDD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnUpdateGridIDD)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Grid gridIDD;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Label label5;
        private Win.UI.Label label6;
        private Win.UI.Label label7;
        private Class.Txtfactory txtFactory;
        private Class.Txtbrand txtBrand;
        private Class.Txtstyle txtStyle;
        private Class.Txtcountry txtDest;
        private Win.UI.TextBox txtSPFrom;
        private Win.UI.TextBox txtSPTo;
        private Win.UI.Label label8;
        private Win.UI.DateRange dateSewingOffline;
        private Win.UI.DateRange dateBuyerdelivery;
        private Win.UI.Button btnQuery;
        private Win.UI.Button btnUpdate;
        private Win.UI.Button btnClose;
        private Win.UI.CheckBox chkExcludeExistsGB;
        private Win.UI.Label label9;
        private Win.UI.DateBox dateIDD;
        private Win.UI.PictureBox btnUpdateGridIDD;
    }
}