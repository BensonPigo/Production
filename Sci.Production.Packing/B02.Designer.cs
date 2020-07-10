namespace Sci.Production.Packing
{
    partial class B02
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
            this.label5 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label8 = new Sci.Win.UI.Label();
            this.label9 = new Sci.Win.UI.Label();
            this.txtbrand1 = new Sci.Production.Class.Txtbrand();
            this.txtCTNRefno = new Sci.Win.UI.TextBox();
            this.numFromRight = new Sci.Win.UI.NumericBox();
            this.numFromBottom = new Sci.Win.UI.NumericBox();
            this.numStampLength = new Sci.Win.UI.NumericBox();
            this.numStampWidth = new Sci.Win.UI.NumericBox();
            this.btnUpload = new Sci.Win.UI.Button();
            this.btnDownload = new Sci.Win.UI.Button();
            this.cmbSide = new Sci.Win.UI.ComboBox();
            this.displayFileName = new Sci.Win.UI.DisplayBox();
            this.label10 = new Sci.Win.UI.Label();
            this.pictureBox1 = new Sci.Win.UI.PictureBox();
            this.comboStickerSize = new Sci.Win.UI.ComboBox();
            this.label11 = new Sci.Win.UI.Label();
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
            this.detail.Size = new System.Drawing.Size(787, 375);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.label11);
            this.detailcont.Controls.Add(this.comboStickerSize);
            this.detailcont.Controls.Add(this.pictureBox1);
            this.detailcont.Controls.Add(this.label10);
            this.detailcont.Controls.Add(this.displayFileName);
            this.detailcont.Controls.Add(this.cmbSide);
            this.detailcont.Controls.Add(this.btnDownload);
            this.detailcont.Controls.Add(this.btnUpload);
            this.detailcont.Controls.Add(this.numStampWidth);
            this.detailcont.Controls.Add(this.numStampLength);
            this.detailcont.Controls.Add(this.numFromBottom);
            this.detailcont.Controls.Add(this.numFromRight);
            this.detailcont.Controls.Add(this.txtCTNRefno);
            this.detailcont.Controls.Add(this.txtbrand1);
            this.detailcont.Controls.Add(this.label9);
            this.detailcont.Controls.Add(this.label8);
            this.detailcont.Controls.Add(this.label7);
            this.detailcont.Controls.Add(this.label6);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Size = new System.Drawing.Size(787, 337);
            this.detailcont.TabStop = true;
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 337);
            this.detailbtm.Size = new System.Drawing.Size(787, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(787, 375);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(795, 404);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(33, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Brand";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(33, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "CTN Refno";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(33, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "Side";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(365, 243);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "File Name";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(365, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(131, 23);
            this.label6.TabIndex = 5;
            this.label6.Text = "From Right (mm)";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(365, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(131, 23);
            this.label7.TabIndex = 6;
            this.label7.Text = "From Bottom (mm)";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(365, 156);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(131, 23);
            this.label8.TabIndex = 7;
            this.label8.Text = "Stamp Length (mm)";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(365, 115);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(131, 23);
            this.label9.TabIndex = 8;
            this.label9.Text = "Stamp Width (mm)";
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
            // txtCTNRefno
            // 
            this.txtCTNRefno.BackColor = System.Drawing.Color.White;
            this.txtCTNRefno.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CTNRefno", true));
            this.txtCTNRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCTNRefno.Location = new System.Drawing.Point(138, 74);
            this.txtCTNRefno.Name = "txtCTNRefno";
            this.txtCTNRefno.Size = new System.Drawing.Size(100, 23);
            this.txtCTNRefno.TabIndex = 3;
            this.txtCTNRefno.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtCTNRefno_PopUp);
            this.txtCTNRefno.Validating += new System.ComponentModel.CancelEventHandler(this.TxtCTNRefno_Validating);
            // 
            // numFromRight
            // 
            this.numFromRight.BackColor = System.Drawing.Color.White;
            this.numFromRight.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "FromRight", true));
            this.numFromRight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numFromRight.Location = new System.Drawing.Point(499, 33);
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
            this.numFromRight.TabIndex = 5;
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
            this.numFromBottom.Location = new System.Drawing.Point(499, 74);
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
            this.numFromBottom.TabIndex = 6;
            this.numFromBottom.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numStampLength
            // 
            this.numStampLength.BackColor = System.Drawing.Color.White;
            this.numStampLength.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "StampLength", true));
            this.numStampLength.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numStampLength.Location = new System.Drawing.Point(499, 156);
            this.numStampLength.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            131072});
            this.numStampLength.Name = "numStampLength";
            this.numStampLength.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numStampLength.Size = new System.Drawing.Size(100, 23);
            this.numStampLength.TabIndex = 8;
            this.numStampLength.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numStampWidth
            // 
            this.numStampWidth.BackColor = System.Drawing.Color.White;
            this.numStampWidth.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "StampWidth", true));
            this.numStampWidth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numStampWidth.Location = new System.Drawing.Point(499, 115);
            this.numStampWidth.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            131072});
            this.numStampWidth.Name = "numStampWidth";
            this.numStampWidth.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numStampWidth.Size = new System.Drawing.Size(100, 23);
            this.numStampWidth.TabIndex = 7;
            this.numStampWidth.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // btnUpload
            // 
            this.btnUpload.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.btnUpload.Location = new System.Drawing.Point(593, 272);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(80, 30);
            this.btnUpload.TabIndex = 11;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.BtnUpload_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.btnDownload.Location = new System.Drawing.Point(695, 272);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(80, 30);
            this.btnDownload.TabIndex = 12;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.BtnDownload_Click);
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
            this.cmbSide.Location = new System.Drawing.Point(138, 115);
            this.cmbSide.Name = "cmbSide";
            this.cmbSide.OldText = "";
            this.cmbSide.Size = new System.Drawing.Size(121, 24);
            this.cmbSide.TabIndex = 4;
            // 
            // displayFileName
            // 
            this.displayFileName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFileName.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "FileName", true));
            this.displayFileName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFileName.Location = new System.Drawing.Point(470, 243);
            this.displayFileName.Name = "displayFileName";
            this.displayFileName.Size = new System.Drawing.Size(305, 23);
            this.displayFileName.TabIndex = 10;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(33, 157);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(102, 23);
            this.label10.TabIndex = 21;
            this.label10.Text = "Example ";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = null;
            this.pictureBox1.Location = new System.Drawing.Point(138, 157);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(215, 124);
            this.pictureBox1.TabIndex = 22;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.WaitOnLoad = true;
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
            this.comboStickerSize.Location = new System.Drawing.Point(499, 197);
            this.comboStickerSize.Name = "comboStickerSize";
            this.comboStickerSize.OldText = "";
            this.comboStickerSize.Size = new System.Drawing.Size(192, 24);
            this.comboStickerSize.TabIndex = 9;
            this.comboStickerSize.SelectedIndexChanged += new System.EventHandler(this.ComboStickerSize_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(365, 198);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(131, 23);
            this.label11.TabIndex = 24;
            this.label11.Text = "Sticker Size";
            // 
            // B02
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 437);
            this.EnableGridJunkColor = true;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportJunk = true;
            this.IsSupportPrint = false;
            this.IsSupportUnJunk = true;
            this.Name = "B02";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B02. Shipping Mark HTML Setting (for GenSong)";
            this.WorkAlias = "ShippingMarkStamp";
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
        private Win.UI.Label label9;
        private Win.UI.Label label8;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Button btnDownload;
        private Win.UI.Button btnUpload;
        private Win.UI.NumericBox numStampWidth;
        private Win.UI.NumericBox numStampLength;
        private Win.UI.NumericBox numFromBottom;
        private Win.UI.NumericBox numFromRight;
        private Win.UI.TextBox txtCTNRefno;
        private Class.Txtbrand txtbrand1;
        private Win.UI.ComboBox cmbSide;
        private Win.UI.DisplayBox displayFileName;
        private Win.UI.PictureBox pictureBox1;
        private Win.UI.Label label10;
        private Win.UI.Label label11;
        private Win.UI.ComboBox comboStickerSize;
    }
}