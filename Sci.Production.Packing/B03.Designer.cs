namespace Sci.Production.Packing
{
    partial class B03
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.txtbrand1 = new Sci.Production.Class.txtbrand();
            this.txtcustcd1 = new Sci.Production.Class.txtcustcd();
            this.txtCTNRefno = new Sci.Win.UI.TextBox();
            this.numFromLeft = new Sci.Win.UI.NumericBox();
            this.numFromTop = new Sci.Win.UI.NumericBox();
            this.numPicLength = new Sci.Win.UI.NumericBox();
            this.numPicWidth = new Sci.Win.UI.NumericBox();
            this.cmbSide = new Sci.Win.UI.ComboBox();
            this.lblSeq = new Sci.Win.UI.Label();
            this.numSeq = new Sci.Win.UI.NumericBox();
            this.chkIs2Side = new Sci.Win.UI.CheckBox();
            this.chkIsRotate = new Sci.Win.UI.CheckBox();
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
            this.detail.Size = new System.Drawing.Size(716, 318);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.chkIsRotate);
            this.detailcont.Controls.Add(this.chkIs2Side);
            this.detailcont.Controls.Add(this.numSeq);
            this.detailcont.Controls.Add(this.lblSeq);
            this.detailcont.Controls.Add(this.cmbSide);
            this.detailcont.Controls.Add(this.numPicWidth);
            this.detailcont.Controls.Add(this.numPicLength);
            this.detailcont.Controls.Add(this.numFromTop);
            this.detailcont.Controls.Add(this.numFromLeft);
            this.detailcont.Controls.Add(this.txtCTNRefno);
            this.detailcont.Controls.Add(this.txtcustcd1);
            this.detailcont.Controls.Add(this.txtbrand1);
            this.detailcont.Controls.Add(this.label9);
            this.detailcont.Controls.Add(this.label8);
            this.detailcont.Controls.Add(this.label7);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Size = new System.Drawing.Size(716, 280);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 280);
            this.detailbtm.Size = new System.Drawing.Size(716, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(716, 318);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(724, 347);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(33, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 23);
            this.label1.TabIndex = 10;
            this.label1.Text = "Brand";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(33, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 23);
            this.label2.TabIndex = 11;
            this.label2.Text = "Cust CD";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(33, 115);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 23);
            this.label3.TabIndex = 12;
            this.label3.Text = "CTN Refno";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(33, 156);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 23);
            this.label4.TabIndex = 13;
            this.label4.Text = "Side";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(341, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(102, 23);
            this.label6.TabIndex = 15;
            this.label6.Text = "From Left";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(341, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 23);
            this.label7.TabIndex = 16;
            this.label7.Text = "From Top";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(341, 115);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(102, 23);
            this.label8.TabIndex = 17;
            this.label8.Text = "Pic Length";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(341, 156);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(102, 23);
            this.label9.TabIndex = 18;
            this.label9.Text = "Pic Width";
            // 
            // txtbrand1
            // 
            this.txtbrand1.BackColor = System.Drawing.Color.White;
            this.txtbrand1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BrandID", true));
            this.txtbrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand1.Location = new System.Drawing.Point(138, 33);
            this.txtbrand1.Name = "txtbrand1";
            this.txtbrand1.Size = new System.Drawing.Size(125, 23);
            this.txtbrand1.TabIndex = 0;
            // 
            // txtcustcd1
            // 
            this.txtcustcd1.BackColor = System.Drawing.Color.White;
            this.txtcustcd1.BrandObjectName = null;
            this.txtcustcd1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CustCD", true));
            this.txtcustcd1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtcustcd1.Location = new System.Drawing.Point(138, 74);
            this.txtcustcd1.Name = "txtcustcd1";
            this.txtcustcd1.Size = new System.Drawing.Size(125, 23);
            this.txtcustcd1.TabIndex = 1;
            // 
            // txtCTNRefno
            // 
            this.txtCTNRefno.BackColor = System.Drawing.Color.White;
            this.txtCTNRefno.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CTNRefno", true));
            this.txtCTNRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCTNRefno.Location = new System.Drawing.Point(138, 115);
            this.txtCTNRefno.Name = "txtCTNRefno";
            this.txtCTNRefno.Size = new System.Drawing.Size(125, 23);
            this.txtCTNRefno.TabIndex = 2;
            this.txtCTNRefno.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtCTNRefno_PopUp);
            this.txtCTNRefno.Validating += new System.ComponentModel.CancelEventHandler(this.TxtCTNRefno_Validating);
            // 
            // numFromLeft
            // 
            this.numFromLeft.BackColor = System.Drawing.Color.White;
            this.numFromLeft.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "FromLeft", true));
            this.numFromLeft.DecimalPlaces = 2;
            this.numFromLeft.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numFromLeft.Location = new System.Drawing.Point(446, 33);
            this.numFromLeft.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            131072});
            this.numFromLeft.Name = "numFromLeft";
            this.numFromLeft.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numFromLeft.Size = new System.Drawing.Size(100, 23);
            this.numFromLeft.TabIndex = 5;
            this.numFromLeft.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numFromTop
            // 
            this.numFromTop.BackColor = System.Drawing.Color.White;
            this.numFromTop.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "FromTop", true));
            this.numFromTop.DecimalPlaces = 2;
            this.numFromTop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numFromTop.Location = new System.Drawing.Point(446, 74);
            this.numFromTop.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            131072});
            this.numFromTop.Name = "numFromTop";
            this.numFromTop.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numFromTop.Size = new System.Drawing.Size(100, 23);
            this.numFromTop.TabIndex = 6;
            this.numFromTop.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numPicLength
            // 
            this.numPicLength.BackColor = System.Drawing.Color.White;
            this.numPicLength.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PicLength", true));
            this.numPicLength.DecimalPlaces = 2;
            this.numPicLength.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numPicLength.Location = new System.Drawing.Point(446, 115);
            this.numPicLength.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            131072});
            this.numPicLength.Name = "numPicLength";
            this.numPicLength.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPicLength.Size = new System.Drawing.Size(100, 23);
            this.numPicLength.TabIndex = 7;
            this.numPicLength.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numPicWidth
            // 
            this.numPicWidth.BackColor = System.Drawing.Color.White;
            this.numPicWidth.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PicWidth", true));
            this.numPicWidth.DecimalPlaces = 2;
            this.numPicWidth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numPicWidth.Location = new System.Drawing.Point(446, 156);
            this.numPicWidth.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            131072});
            this.numPicWidth.Name = "numPicWidth";
            this.numPicWidth.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPicWidth.Size = new System.Drawing.Size(100, 23);
            this.numPicWidth.TabIndex = 8;
            this.numPicWidth.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // cmbSide
            // 
            this.cmbSide.BackColor = System.Drawing.Color.White;
            this.cmbSide.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Side", true));
            this.cmbSide.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbSide.FormattingEnabled = true;
            this.cmbSide.IsSupportUnselect = true;
            this.cmbSide.Items.AddRange(new object[] {
            "Top",
            "Down",
            "Left",
            "Right",
            "Front",
            "Back"});
            this.cmbSide.Location = new System.Drawing.Point(138, 156);
            this.cmbSide.Name = "cmbSide";
            this.cmbSide.OldText = "";
            this.cmbSide.Size = new System.Drawing.Size(125, 24);
            this.cmbSide.TabIndex = 3;
            // 
            // lblSeq
            // 
            this.lblSeq.Location = new System.Drawing.Point(33, 199);
            this.lblSeq.Name = "lblSeq";
            this.lblSeq.Size = new System.Drawing.Size(102, 23);
            this.lblSeq.TabIndex = 14;
            this.lblSeq.Text = "Seq";
            // 
            // numSeq
            // 
            this.numSeq.BackColor = System.Drawing.Color.White;
            this.numSeq.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Seq", true));
            this.numSeq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numSeq.Location = new System.Drawing.Point(138, 199);
            this.numSeq.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.numSeq.Name = "numSeq";
            this.numSeq.NullValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSeq.Size = new System.Drawing.Size(125, 23);
            this.numSeq.TabIndex = 4;
            this.numSeq.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkIs2Side
            // 
            this.chkIs2Side.AutoSize = true;
            this.chkIs2Side.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Is2Side", true));
            this.chkIs2Side.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.chkIs2Side.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIs2Side.Location = new System.Drawing.Point(341, 198);
            this.chkIs2Side.Name = "chkIs2Side";
            this.chkIs2Side.Size = new System.Drawing.Size(90, 24);
            this.chkIs2Side.TabIndex = 9;
            this.chkIs2Side.Text = "Is 2 Side";
            this.chkIs2Side.UseVisualStyleBackColor = true;
            // 
            // chkIsRotate
            // 
            this.chkIsRotate.AutoSize = true;
            this.chkIsRotate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsRotate", true));
            this.chkIsRotate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.chkIsRotate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIsRotate.Location = new System.Drawing.Point(341, 228);
            this.chkIsRotate.Name = "chkIsRotate";
            this.chkIsRotate.Size = new System.Drawing.Size(94, 24);
            this.chkIsRotate.TabIndex = 10;
            this.chkIsRotate.Text = "Is Rotate";
            this.chkIsRotate.UseVisualStyleBackColor = true;
            // 
            // B03
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 380);
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.Name = "B03";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B03. Shipping Mark Pic Setting (for GenSong)";
            this.WorkAlias = "ShippingMarkPicture";
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

        private Win.UI.Label label1;
        private Win.UI.Label label9;
        private Win.UI.Label label8;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.NumericBox numPicWidth;
        private Win.UI.NumericBox numPicLength;
        private Win.UI.NumericBox numFromTop;
        private Win.UI.NumericBox numFromLeft;
        private Win.UI.TextBox txtCTNRefno;
        private Class.txtcustcd txtcustcd1;
        private Class.txtbrand txtbrand1;
        private Win.UI.ComboBox cmbSide;
        private Win.UI.NumericBox numSeq;
        private Win.UI.Label lblSeq;
        private Win.UI.CheckBox chkIs2Side;
        private Win.UI.CheckBox chkIsRotate;
    }
}