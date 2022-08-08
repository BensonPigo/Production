namespace Sci.Production.Sewing
{
    partial class B05
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
            this.btnBatchImport = new Sci.Win.UI.Button();
            this.btnHistory = new Sci.Win.UI.Button();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.displayFactory = new Sci.Win.UI.DisplayBox();
            this.txtstyle = new Sci.Production.Class.Txtstyle();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.txtseason = new Sci.Production.Class.Txtseason();
            this.numericEfficiency = new Sci.Win.UI.NumericBox();
            this.label6 = new Sci.Win.UI.Label();
            this.checkBoxJunk = new Sci.Win.UI.CheckBox();
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
            this.detail.Size = new System.Drawing.Size(921, 358);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.checkBoxJunk);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.numericEfficiency);
            this.detailcont.Controls.Add(this.txtseason);
            this.detailcont.Controls.Add(this.txtbrand);
            this.detailcont.Controls.Add(this.txtstyle);
            this.detailcont.Controls.Add(this.displayFactory);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.btnHistory);
            this.detailcont.Size = new System.Drawing.Size(921, 320);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 320);
            this.detailbtm.Size = new System.Drawing.Size(921, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(921, 358);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(929, 387);
            // 
            // btnBatchImport
            // 
            this.btnBatchImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBatchImport.Location = new System.Drawing.Point(802, 3);
            this.btnBatchImport.Name = "btnBatchImport";
            this.btnBatchImport.Size = new System.Drawing.Size(123, 30);
            this.btnBatchImport.TabIndex = 2;
            this.btnBatchImport.Text = "Batch Import";
            this.btnBatchImport.UseVisualStyleBackColor = true;
            this.btnBatchImport.Click += new System.EventHandler(this.BtnBatchImport_Click);
            // 
            // btnHistory
            // 
            this.btnHistory.Location = new System.Drawing.Point(265, 5);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(100, 30);
            this.btnHistory.TabIndex = 3;
            this.btnHistory.Text = "History";
            this.btnHistory.UseVisualStyleBackColor = true;
            this.btnHistory.Click += new System.EventHandler(this.BtnHistory_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(18, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "Factory";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(18, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 5;
            this.label2.Text = "Brand";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(18, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 6;
            this.label3.Text = "Style";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(18, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 7;
            this.label4.Text = "Season";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(18, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 8;
            this.label5.Text = "Set Eff(%)";
            // 
            // displayFactory
            // 
            this.displayFactory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFactory.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "FactoryID", true));
            this.displayFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFactory.Location = new System.Drawing.Point(96, 9);
            this.displayFactory.Name = "displayFactory";
            this.displayFactory.Size = new System.Drawing.Size(100, 23);
            this.displayFactory.TabIndex = 9;
            // 
            // txtstyle
            // 
            this.txtstyle.BackColor = System.Drawing.Color.White;
            this.txtstyle.BrandObjectName = this.txtbrand;
            this.txtstyle.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "StyleID", true));
            this.txtstyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtstyle.Location = new System.Drawing.Point(96, 83);
            this.txtstyle.Name = "txtstyle";
            this.txtstyle.Size = new System.Drawing.Size(130, 23);
            this.txtstyle.TabIndex = 10;
            this.txtstyle.TarBrand = this.txtbrand;
            this.txtstyle.TarSeason = this.txtseason;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.White;
            this.txtbrand.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BrandID", true));
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand.Location = new System.Drawing.Point(96, 45);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.Size = new System.Drawing.Size(66, 23);
            this.txtbrand.TabIndex = 11;
            // 
            // txtseason
            // 
            this.txtseason.BackColor = System.Drawing.Color.White;
            this.txtseason.BrandObjectName = this.txtbrand;
            this.txtseason.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "SeasonID", true));
            this.txtseason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtseason.Location = new System.Drawing.Point(96, 122);
            this.txtseason.Name = "txtseason";
            this.txtseason.Size = new System.Drawing.Size(80, 23);
            this.txtseason.TabIndex = 12;
            // 
            // numericEfficiency
            // 
            this.numericEfficiency.BackColor = System.Drawing.Color.White;
            this.numericEfficiency.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Efficiency", true));
            this.numericEfficiency.DecimalPlaces = 2;
            this.numericEfficiency.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericEfficiency.Location = new System.Drawing.Point(96, 157);
            this.numericEfficiency.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numericEfficiency.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericEfficiency.Name = "numericEfficiency";
            this.numericEfficiency.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericEfficiency.Size = new System.Drawing.Size(100, 23);
            this.numericEfficiency.TabIndex = 13;
            this.numericEfficiency.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(18, 193);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(418, 23);
            this.label6.TabIndex = 14;
            this.label6.Text = "* Only the [New] permission can use the [Batch Import] function";
            this.label6.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // checkBoxJunk
            // 
            this.checkBoxJunk.AutoSize = true;
            this.checkBoxJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkBoxJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBoxJunk.Location = new System.Drawing.Point(202, 11);
            this.checkBoxJunk.Name = "checkBoxJunk";
            this.checkBoxJunk.Size = new System.Drawing.Size(57, 21);
            this.checkBoxJunk.TabIndex = 15;
            this.checkBoxJunk.Text = "Junk";
            this.checkBoxJunk.UseVisualStyleBackColor = true;
            // 
            // B05
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 420);
            this.Controls.Add(this.btnBatchImport);
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B05";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B05. CMP Efficiency Setting By Factory";
            this.WorkAlias = "SewingOutputEfficiency";
            this.Controls.SetChildIndex(this.tabs, 0);
            this.Controls.SetChildIndex(this.btnBatchImport, 0);
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

        private Win.UI.Button btnBatchImport;
        private Class.Txtstyle txtstyle;
        private Win.UI.DisplayBox displayFactory;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.Button btnHistory;
        private Class.Txtseason txtseason;
        private Class.Txtbrand txtbrand;
        private Win.UI.NumericBox numericEfficiency;
        private Win.UI.CheckBox checkBoxJunk;
        private Win.UI.Label label6;
    }
}