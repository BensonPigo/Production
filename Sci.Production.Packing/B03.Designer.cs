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
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.txtCTNRefno = new Sci.Win.UI.TextBox();
            this.numFromRight = new Sci.Win.UI.NumericBox();
            this.numFromBottom = new Sci.Win.UI.NumericBox();
            this.cmbSide = new Sci.Win.UI.ComboBox();
            this.lblSeq = new Sci.Win.UI.Label();
            this.numSeq = new Sci.Win.UI.NumericBox();
            this.chkIs2Side = new Sci.Win.UI.CheckBox();
            this.chkIsHorizontal = new Sci.Win.UI.CheckBox();
            this.pictureBox1 = new Sci.Win.UI.PictureBox();
            this.label10 = new Sci.Win.UI.Label();
            this.checkIsSSCC = new Sci.Win.UI.CheckBox();
            this.label11 = new Sci.Win.UI.Label();
            this.comboStickerSize = new Sci.Win.UI.ComboBox();
            this.txtbrand1 = new Sci.Production.Class.txtbrand();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(770, 374);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.label11);
            this.detailcont.Controls.Add(this.comboStickerSize);
            this.detailcont.Controls.Add(this.checkIsSSCC);
            this.detailcont.Controls.Add(this.pictureBox1);
            this.detailcont.Controls.Add(this.label10);
            this.detailcont.Controls.Add(this.chkIsHorizontal);
            this.detailcont.Controls.Add(this.chkIs2Side);
            this.detailcont.Controls.Add(this.numSeq);
            this.detailcont.Controls.Add(this.lblSeq);
            this.detailcont.Controls.Add(this.cmbSide);
            this.detailcont.Controls.Add(this.numFromBottom);
            this.detailcont.Controls.Add(this.numFromRight);
            this.detailcont.Controls.Add(this.txtCTNRefno);
            this.detailcont.Controls.Add(this.txtbrand1);
            this.detailcont.Controls.Add(this.label7);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Size = new System.Drawing.Size(770, 336);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 336);
            this.detailbtm.Size = new System.Drawing.Size(770, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(770, 374);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(778, 403);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(33, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 23);
            this.label1.TabIndex = 10;
            this.label1.Text = "Brand";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(33, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 23);
            this.label3.TabIndex = 12;
            this.label3.Text = "CTN Refno";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(33, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 23);
            this.label4.TabIndex = 13;
            this.label4.Text = "Side";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(381, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(123, 23);
            this.label6.TabIndex = 15;
            this.label6.Text = "From Right (mm)";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(381, 95);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(123, 23);
            this.label7.TabIndex = 16;
            this.label7.Text = "From Bottom (mm)";
            // 
            // txtCTNRefno
            // 
            this.txtCTNRefno.BackColor = System.Drawing.Color.White;
            this.txtCTNRefno.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CTNRefno", true));
            this.txtCTNRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCTNRefno.Location = new System.Drawing.Point(138, 73);
            this.txtCTNRefno.Name = "txtCTNRefno";
            this.txtCTNRefno.Size = new System.Drawing.Size(125, 23);
            this.txtCTNRefno.TabIndex = 3;
            this.txtCTNRefno.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtCTNRefno_PopUp);
            this.txtCTNRefno.Validating += new System.ComponentModel.CancelEventHandler(this.TxtCTNRefno_Validating);
            // 
            // numFromRight
            // 
            this.numFromRight.BackColor = System.Drawing.Color.White;
            this.numFromRight.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "FromRight", true));
            this.numFromRight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numFromRight.Location = new System.Drawing.Point(507, 63);
            this.numFromRight.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            131072});
            this.numFromRight.Name = "numFromRight";
            this.numFromRight.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numFromRight.Size = new System.Drawing.Size(100, 23);
            this.numFromRight.TabIndex = 6;
            this.numFromRight.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numFromBottom
            // 
            this.numFromBottom.BackColor = System.Drawing.Color.White;
            this.numFromBottom.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "FromBottom", true));
            this.numFromBottom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numFromBottom.Location = new System.Drawing.Point(507, 95);
            this.numFromBottom.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            131072});
            this.numFromBottom.Name = "numFromBottom";
            this.numFromBottom.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numFromBottom.Size = new System.Drawing.Size(100, 23);
            this.numFromBottom.TabIndex = 7;
            this.numFromBottom.Value = new decimal(new int[] {
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
            "A",
            "B",
            "C",
            "D"});
            this.cmbSide.Location = new System.Drawing.Point(138, 114);
            this.cmbSide.Name = "cmbSide";
            this.cmbSide.OldText = "";
            this.cmbSide.Size = new System.Drawing.Size(125, 24);
            this.cmbSide.TabIndex = 4;
            // 
            // lblSeq
            // 
            this.lblSeq.Location = new System.Drawing.Point(381, 33);
            this.lblSeq.Name = "lblSeq";
            this.lblSeq.Size = new System.Drawing.Size(123, 23);
            this.lblSeq.TabIndex = 14;
            this.lblSeq.Text = "Seq";
            // 
            // numSeq
            // 
            this.numSeq.BackColor = System.Drawing.Color.White;
            this.numSeq.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Seq", true));
            this.numSeq.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numSeq.Location = new System.Drawing.Point(507, 33);
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
            this.numSeq.Size = new System.Drawing.Size(100, 23);
            this.numSeq.TabIndex = 5;
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
            this.chkIs2Side.Location = new System.Drawing.Point(381, 165);
            this.chkIs2Side.Name = "chkIs2Side";
            this.chkIs2Side.Size = new System.Drawing.Size(90, 24);
            this.chkIs2Side.TabIndex = 11;
            this.chkIs2Side.Text = "Is 2 Side";
            this.chkIs2Side.UseVisualStyleBackColor = true;
            // 
            // chkIsHorizontal
            // 
            this.chkIsHorizontal.AutoSize = true;
            this.chkIsHorizontal.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsHorizontal", true));
            this.chkIsHorizontal.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.chkIsHorizontal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIsHorizontal.Location = new System.Drawing.Point(381, 195);
            this.chkIsHorizontal.Name = "chkIsHorizontal";
            this.chkIsHorizontal.Size = new System.Drawing.Size(117, 24);
            this.chkIsHorizontal.TabIndex = 12;
            this.chkIsHorizontal.Text = "Is Horizontal";
            this.chkIsHorizontal.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = null;
            this.pictureBox1.Location = new System.Drawing.Point(138, 155);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(215, 124);
            this.pictureBox1.TabIndex = 24;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.WaitOnLoad = true;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(33, 155);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 23);
            this.label10.TabIndex = 23;
            this.label10.Text = "Example ";
            // 
            // checkIsSSCC
            // 
            this.checkIsSSCC.AutoSize = true;
            this.checkIsSSCC.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "IsSSCC", true));
            this.checkIsSSCC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.checkIsSSCC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkIsSSCC.Location = new System.Drawing.Point(381, 225);
            this.checkIsSSCC.Name = "checkIsSSCC";
            this.checkIsSSCC.Size = new System.Drawing.Size(89, 24);
            this.checkIsSSCC.TabIndex = 13;
            this.checkIsSSCC.Text = "Is SSCC";
            this.checkIsSSCC.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(381, 129);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(121, 23);
            this.label11.TabIndex = 26;
            this.label11.Text = "Sticker Size";
            // 
            // comboStickerSize
            // 
            this.comboStickerSize.BackColor = System.Drawing.Color.White;
            this.comboStickerSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboStickerSize.FormattingEnabled = true;
            this.comboStickerSize.IsSupportUnselect = true;
            this.comboStickerSize.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D"});
            this.comboStickerSize.Location = new System.Drawing.Point(507, 129);
            this.comboStickerSize.Name = "comboStickerSize";
            this.comboStickerSize.OldText = "";
            this.comboStickerSize.Size = new System.Drawing.Size(192, 24);
            this.comboStickerSize.TabIndex = 10;
            this.comboStickerSize.SelectedValueChanged += new System.EventHandler(this.ComboStickerSize_SelectedValueChanged);
            // 
            // txtbrand1
            // 
            this.txtbrand1.BackColor = System.Drawing.Color.White;
            this.txtbrand1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BrandID", true));
            this.txtbrand1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtbrand1.Location = new System.Drawing.Point(138, 33);
            this.txtbrand1.Name = "txtbrand1";
            this.txtbrand1.Size = new System.Drawing.Size(125, 23);
            this.txtbrand1.TabIndex = 1;
            // 
            // B03
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 436);
            this.EnableGridJunkColor = true;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportJunk = true;
            this.IsSupportPrint = false;
            this.IsSupportUnJunk = true;
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.NumericBox numFromBottom;
        private Win.UI.NumericBox numFromRight;
        private Win.UI.TextBox txtCTNRefno;
        private Class.txtbrand txtbrand1;
        private Win.UI.ComboBox cmbSide;
        private Win.UI.NumericBox numSeq;
        private Win.UI.Label lblSeq;
        private Win.UI.CheckBox chkIs2Side;
        private Win.UI.CheckBox chkIsHorizontal;
        private Win.UI.PictureBox pictureBox1;
        private Win.UI.Label label10;
        private Win.UI.CheckBox checkIsSSCC;
        private Win.UI.Label label11;
        private Win.UI.ComboBox comboStickerSize;
    }
}