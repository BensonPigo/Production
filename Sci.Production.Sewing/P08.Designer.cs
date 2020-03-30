namespace Sci.Production.Sewing
{
    partial class P08
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
            this.lbScanCartonsBarcode = new Sci.Win.UI.Label();
            this.lbDiscrepancy = new Sci.Win.UI.Label();
            this.txtScanCartonsBarcode = new Sci.Win.UI.TextBox();
            this.numericBoxDiscrepancy = new Sci.Win.UI.NumericBox();
            this.lbDiscrepancyMsg = new System.Windows.Forms.Label();
            this.lbSP = new Sci.Win.UI.Label();
            this.lbPO = new Sci.Win.UI.Label();
            this.lbStyle = new Sci.Win.UI.Label();
            this.lbSeason = new Sci.Win.UI.Label();
            this.lbBrand = new Sci.Win.UI.Label();
            this.lbBuyerDelivery = new Sci.Win.UI.Label();
            this.lbDestination = new Sci.Win.UI.Label();
            this.lbCartonQty = new Sci.Win.UI.Label();
            this.displaySP = new Sci.Win.UI.DisplayBox();
            this.displayPO = new Sci.Win.UI.DisplayBox();
            this.displayStyle = new Sci.Win.UI.DisplayBox();
            this.displaySeason = new Sci.Win.UI.DisplayBox();
            this.displayBrand = new Sci.Win.UI.DisplayBox();
            this.displayDestination = new Sci.Win.UI.DisplayBox();
            this.displayBuyerDelivery = new Sci.Win.UI.DisplayBox();
            this.displayCartonQty = new Sci.Win.UI.DisplayBox();
            this.btnSave = new Sci.Win.UI.Button();
            this.SuspendLayout();
            // 
            // lbScanCartonsBarcode
            // 
            this.lbScanCartonsBarcode.Location = new System.Drawing.Point(18, 19);
            this.lbScanCartonsBarcode.Name = "lbScanCartonsBarcode";
            this.lbScanCartonsBarcode.Size = new System.Drawing.Size(149, 23);
            this.lbScanCartonsBarcode.TabIndex = 1;
            this.lbScanCartonsBarcode.Text = "Scan Cartons Barcode";
            // 
            // lbDiscrepancy
            // 
            this.lbDiscrepancy.Location = new System.Drawing.Point(18, 52);
            this.lbDiscrepancy.Name = "lbDiscrepancy";
            this.lbDiscrepancy.Size = new System.Drawing.Size(149, 23);
            this.lbDiscrepancy.TabIndex = 2;
            this.lbDiscrepancy.Text = "Discrepancy";
            // 
            // txtScanCartonsBarcode
            // 
            this.txtScanCartonsBarcode.BackColor = System.Drawing.Color.White;
            this.txtScanCartonsBarcode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtScanCartonsBarcode.IsSupportEditMode = false;
            this.txtScanCartonsBarcode.Location = new System.Drawing.Point(170, 19);
            this.txtScanCartonsBarcode.Name = "txtScanCartonsBarcode";
            this.txtScanCartonsBarcode.Size = new System.Drawing.Size(134, 23);
            this.txtScanCartonsBarcode.TabIndex = 1;
            this.txtScanCartonsBarcode.Validating += new System.ComponentModel.CancelEventHandler(this.TxtScanCartonsBarcode_Validating);
            // 
            // numericBoxDiscrepancy
            // 
            this.numericBoxDiscrepancy.BackColor = System.Drawing.Color.White;
            this.numericBoxDiscrepancy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numericBoxDiscrepancy.IsSupportEditMode = false;
            this.numericBoxDiscrepancy.Location = new System.Drawing.Point(170, 52);
            this.numericBoxDiscrepancy.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBoxDiscrepancy.Name = "numericBoxDiscrepancy";
            this.numericBoxDiscrepancy.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numericBoxDiscrepancy.Size = new System.Drawing.Size(134, 23);
            this.numericBoxDiscrepancy.TabIndex = 2;
            this.numericBoxDiscrepancy.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // lbDiscrepancyMsg
            // 
            this.lbDiscrepancyMsg.AutoSize = true;
            this.lbDiscrepancyMsg.ForeColor = System.Drawing.Color.Red;
            this.lbDiscrepancyMsg.Location = new System.Drawing.Point(15, 87);
            this.lbDiscrepancyMsg.Name = "lbDiscrepancyMsg";
            this.lbDiscrepancyMsg.Size = new System.Drawing.Size(438, 17);
            this.lbDiscrepancyMsg.TabIndex = 3;
            this.lbDiscrepancyMsg.Text = "The Qty here is by pieces instead of complete set on below function.";
            // 
            // lbSP
            // 
            this.lbSP.Location = new System.Drawing.Point(18, 120);
            this.lbSP.Name = "lbSP";
            this.lbSP.Size = new System.Drawing.Size(75, 23);
            this.lbSP.TabIndex = 4;
            this.lbSP.Text = "SP#";
            // 
            // lbPO
            // 
            this.lbPO.Location = new System.Drawing.Point(18, 154);
            this.lbPO.Name = "lbPO";
            this.lbPO.Size = new System.Drawing.Size(75, 23);
            this.lbPO.TabIndex = 5;
            this.lbPO.Text = "PO#";
            // 
            // lbStyle
            // 
            this.lbStyle.Location = new System.Drawing.Point(18, 189);
            this.lbStyle.Name = "lbStyle";
            this.lbStyle.Size = new System.Drawing.Size(75, 23);
            this.lbStyle.TabIndex = 6;
            this.lbStyle.Text = "Style";
            // 
            // lbSeason
            // 
            this.lbSeason.Location = new System.Drawing.Point(18, 225);
            this.lbSeason.Name = "lbSeason";
            this.lbSeason.Size = new System.Drawing.Size(75, 23);
            this.lbSeason.TabIndex = 7;
            this.lbSeason.Text = "Season";
            // 
            // lbBrand
            // 
            this.lbBrand.Location = new System.Drawing.Point(262, 120);
            this.lbBrand.Name = "lbBrand";
            this.lbBrand.Size = new System.Drawing.Size(99, 23);
            this.lbBrand.TabIndex = 8;
            this.lbBrand.Text = "Brand";
            // 
            // lbBuyerDelivery
            // 
            this.lbBuyerDelivery.Location = new System.Drawing.Point(262, 189);
            this.lbBuyerDelivery.Name = "lbBuyerDelivery";
            this.lbBuyerDelivery.Size = new System.Drawing.Size(99, 23);
            this.lbBuyerDelivery.TabIndex = 9;
            this.lbBuyerDelivery.Text = "Buyer Delivery";
            // 
            // lbDestination
            // 
            this.lbDestination.Location = new System.Drawing.Point(262, 154);
            this.lbDestination.Name = "lbDestination";
            this.lbDestination.Size = new System.Drawing.Size(99, 23);
            this.lbDestination.TabIndex = 10;
            this.lbDestination.Text = "Destination";
            // 
            // lbCartonQty
            // 
            this.lbCartonQty.Location = new System.Drawing.Point(262, 225);
            this.lbCartonQty.Name = "lbCartonQty";
            this.lbCartonQty.Size = new System.Drawing.Size(99, 23);
            this.lbCartonQty.TabIndex = 11;
            this.lbCartonQty.Text = "Carton Qty";
            // 
            // displaySP
            // 
            this.displaySP.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySP.Location = new System.Drawing.Point(96, 120);
            this.displaySP.Name = "displaySP";
            this.displaySP.Size = new System.Drawing.Size(163, 23);
            this.displaySP.TabIndex = 12;
            // 
            // displayPO
            // 
            this.displayPO.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPO.Location = new System.Drawing.Point(96, 154);
            this.displayPO.Name = "displayPO";
            this.displayPO.Size = new System.Drawing.Size(163, 23);
            this.displayPO.TabIndex = 13;
            // 
            // displayStyle
            // 
            this.displayStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayStyle.Location = new System.Drawing.Point(96, 189);
            this.displayStyle.Name = "displayStyle";
            this.displayStyle.Size = new System.Drawing.Size(163, 23);
            this.displayStyle.TabIndex = 14;
            // 
            // displaySeason
            // 
            this.displaySeason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySeason.Location = new System.Drawing.Point(96, 225);
            this.displaySeason.Name = "displaySeason";
            this.displaySeason.Size = new System.Drawing.Size(163, 23);
            this.displaySeason.TabIndex = 15;
            // 
            // displayBrand
            // 
            this.displayBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBrand.Location = new System.Drawing.Point(364, 120);
            this.displayBrand.Name = "displayBrand";
            this.displayBrand.Size = new System.Drawing.Size(163, 23);
            this.displayBrand.TabIndex = 16;
            // 
            // displayDestination
            // 
            this.displayDestination.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayDestination.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayDestination.Location = new System.Drawing.Point(364, 154);
            this.displayDestination.Name = "displayDestination";
            this.displayDestination.Size = new System.Drawing.Size(163, 23);
            this.displayDestination.TabIndex = 17;
            // 
            // displayBuyerDelivery
            // 
            this.displayBuyerDelivery.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBuyerDelivery.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBuyerDelivery.Location = new System.Drawing.Point(364, 189);
            this.displayBuyerDelivery.Name = "displayBuyerDelivery";
            this.displayBuyerDelivery.Size = new System.Drawing.Size(163, 23);
            this.displayBuyerDelivery.TabIndex = 18;
            // 
            // displayCartonQty
            // 
            this.displayCartonQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCartonQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCartonQty.Location = new System.Drawing.Point(364, 225);
            this.displayCartonQty.Name = "displayCartonQty";
            this.displayCartonQty.Size = new System.Drawing.Size(163, 23);
            this.displayCartonQty.TabIndex = 19;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(459, 74);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // P08
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 257);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.displayCartonQty);
            this.Controls.Add(this.displayBuyerDelivery);
            this.Controls.Add(this.displayDestination);
            this.Controls.Add(this.displayBrand);
            this.Controls.Add(this.displaySeason);
            this.Controls.Add(this.displayStyle);
            this.Controls.Add(this.displayPO);
            this.Controls.Add(this.displaySP);
            this.Controls.Add(this.lbCartonQty);
            this.Controls.Add(this.lbDestination);
            this.Controls.Add(this.lbBuyerDelivery);
            this.Controls.Add(this.lbBrand);
            this.Controls.Add(this.lbSeason);
            this.Controls.Add(this.lbStyle);
            this.Controls.Add(this.lbPO);
            this.Controls.Add(this.lbSP);
            this.Controls.Add(this.lbDiscrepancyMsg);
            this.Controls.Add(this.numericBoxDiscrepancy);
            this.Controls.Add(this.txtScanCartonsBarcode);
            this.Controls.Add(this.lbDiscrepancy);
            this.Controls.Add(this.lbScanCartonsBarcode);
            this.Name = "P08";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P08.MD Room Scan";
            this.Controls.SetChildIndex(this.lbScanCartonsBarcode, 0);
            this.Controls.SetChildIndex(this.lbDiscrepancy, 0);
            this.Controls.SetChildIndex(this.txtScanCartonsBarcode, 0);
            this.Controls.SetChildIndex(this.numericBoxDiscrepancy, 0);
            this.Controls.SetChildIndex(this.lbDiscrepancyMsg, 0);
            this.Controls.SetChildIndex(this.lbSP, 0);
            this.Controls.SetChildIndex(this.lbPO, 0);
            this.Controls.SetChildIndex(this.lbStyle, 0);
            this.Controls.SetChildIndex(this.lbSeason, 0);
            this.Controls.SetChildIndex(this.lbBrand, 0);
            this.Controls.SetChildIndex(this.lbBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.lbDestination, 0);
            this.Controls.SetChildIndex(this.lbCartonQty, 0);
            this.Controls.SetChildIndex(this.displaySP, 0);
            this.Controls.SetChildIndex(this.displayPO, 0);
            this.Controls.SetChildIndex(this.displayStyle, 0);
            this.Controls.SetChildIndex(this.displaySeason, 0);
            this.Controls.SetChildIndex(this.displayBrand, 0);
            this.Controls.SetChildIndex(this.displayDestination, 0);
            this.Controls.SetChildIndex(this.displayBuyerDelivery, 0);
            this.Controls.SetChildIndex(this.displayCartonQty, 0);
            this.Controls.SetChildIndex(this.btnSave, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lbScanCartonsBarcode;
        private Win.UI.Label lbDiscrepancy;
        private Win.UI.TextBox txtScanCartonsBarcode;
        private Win.UI.NumericBox numericBoxDiscrepancy;
        private System.Windows.Forms.Label lbDiscrepancyMsg;
        private Win.UI.Label lbSP;
        private Win.UI.Label lbPO;
        private Win.UI.Label lbStyle;
        private Win.UI.Label lbSeason;
        private Win.UI.Label lbBrand;
        private Win.UI.Label lbBuyerDelivery;
        private Win.UI.Label lbDestination;
        private Win.UI.Label lbCartonQty;
        private Win.UI.DisplayBox displaySP;
        private Win.UI.DisplayBox displayPO;
        private Win.UI.DisplayBox displayStyle;
        private Win.UI.DisplayBox displaySeason;
        private Win.UI.DisplayBox displayBrand;
        private Win.UI.DisplayBox displayDestination;
        private Win.UI.DisplayBox displayBuyerDelivery;
        private Win.UI.DisplayBox displayCartonQty;
        private Win.UI.Button btnSave;
    }
}