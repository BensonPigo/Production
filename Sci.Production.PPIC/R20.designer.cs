namespace Sci.Production.PPIC
{
    partial class R20
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
            this.lable1 = new Sci.Win.UI.Label();
            this.Style = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.editProgram = new Sci.Win.UI.EditBox();
            this.editArtworkType = new Sci.Win.UI.EditBox();
            this.txtMuiltSeason = new Sci.Win.UI.TextBox();
            this.txtStyle = new Sci.Win.UI.TextBox();
            this.UI_ckbExcludeLocalStyle = new Sci.Win.UI.CheckBox();
            this.UI_ckbExcludeDevOptionStyle = new Sci.Win.UI.CheckBox();
            this.txtbrand = new Sci.Production.Class.Txtbrand();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(371, 12);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(371, 48);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(371, 84);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(371, 170);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(371, 143);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(354, 1);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(4, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 23);
            this.label1.TabIndex = 112;
            this.label1.Text = "Season";
            // 
            // lable1
            // 
            this.lable1.Location = new System.Drawing.Point(4, 4);
            this.lable1.Name = "lable1";
            this.lable1.Size = new System.Drawing.Size(94, 23);
            this.lable1.TabIndex = 111;
            this.lable1.Text = "Brand";
            // 
            // Style
            // 
            this.Style.Location = new System.Drawing.Point(4, 52);
            this.Style.Name = "Style";
            this.Style.Size = new System.Drawing.Size(94, 23);
            this.Style.TabIndex = 115;
            this.Style.Text = "Style#";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(4, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 23);
            this.label2.TabIndex = 118;
            this.label2.Text = "Program";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.BackColor = System.Drawing.SystemColors.Control;
            this.label5.Location = new System.Drawing.Point(364, 117);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 23);
            this.label5.TabIndex = 124;
            this.label5.Text = "Paper Size A4";
            this.label5.TextStyle.BorderColor = System.Drawing.Color.Transparent;
            this.label5.TextStyle.Color = System.Drawing.Color.Blue;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(4, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 23);
            this.label3.TabIndex = 128;
            this.label3.Text = "Artwork Type";
            // 
            // editProgram
            // 
            this.editProgram.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editProgram.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editProgram.IsSupportEditMode = false;
            this.editProgram.Location = new System.Drawing.Point(98, 117);
            this.editProgram.Multiline = true;
            this.editProgram.Name = "editProgram";
            this.editProgram.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.editProgram.ReadOnly = true;
            this.editProgram.Size = new System.Drawing.Size(232, 43);
            this.editProgram.TabIndex = 131;
            this.editProgram.PopUp += new System.EventHandler<Sci.Win.UI.EditBoxPopUpEventArgs>(this.EditProgram_PopUp);
            // 
            // editArtworkType
            // 
            this.editArtworkType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editArtworkType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editArtworkType.IsSupportEditMode = false;
            this.editArtworkType.Location = new System.Drawing.Point(98, 76);
            this.editArtworkType.Multiline = true;
            this.editArtworkType.Name = "editArtworkType";
            this.editArtworkType.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.editArtworkType.ReadOnly = true;
            this.editArtworkType.Size = new System.Drawing.Size(232, 43);
            this.editArtworkType.TabIndex = 132;
            this.editArtworkType.PopUp += new System.EventHandler<Sci.Win.UI.EditBoxPopUpEventArgs>(this.EditArtworkType_PopUp);
            // 
            // txtMuiltSeason
            // 
            this.txtMuiltSeason.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtMuiltSeason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtMuiltSeason.IsSupportEditMode = false;
            this.txtMuiltSeason.Location = new System.Drawing.Point(99, 28);
            this.txtMuiltSeason.Name = "txtMuiltSeason";
            this.txtMuiltSeason.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditModeAndReadOnly;
            this.txtMuiltSeason.ReadOnly = true;
            this.txtMuiltSeason.Size = new System.Drawing.Size(167, 23);
            this.txtMuiltSeason.TabIndex = 136;
            this.txtMuiltSeason.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtMuiltSeason_PopUp);
            // 
            // txtStyle
            // 
            this.txtStyle.BackColor = System.Drawing.Color.White;
            this.txtStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtStyle.IsSupportEditMode = false;
            this.txtStyle.Location = new System.Drawing.Point(98, 51);
            this.txtStyle.Name = "txtStyle";
            this.txtStyle.Size = new System.Drawing.Size(167, 23);
            this.txtStyle.TabIndex = 137;
            this.txtStyle.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtStyle_PopUp);
            this.txtStyle.Validating += new System.ComponentModel.CancelEventHandler(this.TxtStyle_Validating);
            // 
            // UI_ckbExcludeLocalStyle
            // 
            this.UI_ckbExcludeLocalStyle.AutoSize = true;
            this.UI_ckbExcludeLocalStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.UI_ckbExcludeLocalStyle.Location = new System.Drawing.Point(188, 179);
            this.UI_ckbExcludeLocalStyle.Name = "UI_ckbExcludeLocalStyle";
            this.UI_ckbExcludeLocalStyle.Size = new System.Drawing.Size(142, 21);
            this.UI_ckbExcludeLocalStyle.TabIndex = 266;
            this.UI_ckbExcludeLocalStyle.Text = "Exclude local style";
            this.UI_ckbExcludeLocalStyle.UseVisualStyleBackColor = true;
            // 
            // UI_ckbExcludeDevOptionStyle
            // 
            this.UI_ckbExcludeDevOptionStyle.AutoSize = true;
            this.UI_ckbExcludeDevOptionStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.UI_ckbExcludeDevOptionStyle.Location = new System.Drawing.Point(3, 179);
            this.UI_ckbExcludeDevOptionStyle.Name = "UI_ckbExcludeDevOptionStyle";
            this.UI_ckbExcludeDevOptionStyle.Size = new System.Drawing.Size(179, 21);
            this.UI_ckbExcludeDevOptionStyle.TabIndex = 265;
            this.UI_ckbExcludeDevOptionStyle.Text = "Exclude dev.option style";
            this.UI_ckbExcludeDevOptionStyle.UseVisualStyleBackColor = true;
            // 
            // txtbrand
            // 
            this.txtbrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtbrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtbrand.Location = new System.Drawing.Point(99, 4);
            this.txtbrand.Name = "txtbrand";
            this.txtbrand.ReadOnly = true;
            this.txtbrand.Size = new System.Drawing.Size(229, 23);
            this.txtbrand.TabIndex = 269;
            // 
            // R20
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 235);
            this.Controls.Add(this.txtbrand);
            this.Controls.Add(this.UI_ckbExcludeLocalStyle);
            this.Controls.Add(this.UI_ckbExcludeDevOptionStyle);
            this.Controls.Add(this.txtStyle);
            this.Controls.Add(this.txtMuiltSeason);
            this.Controls.Add(this.editArtworkType);
            this.Controls.Add(this.editProgram);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Style);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lable1);
            this.Name = "R20";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "R20. Style Artwork List";
            this.Controls.SetChildIndex(this.lable1, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.Style, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.editProgram, 0);
            this.Controls.SetChildIndex(this.editArtworkType, 0);
            this.Controls.SetChildIndex(this.txtMuiltSeason, 0);
            this.Controls.SetChildIndex(this.txtStyle, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.UI_ckbExcludeDevOptionStyle, 0);
            this.Controls.SetChildIndex(this.UI_ckbExcludeLocalStyle, 0);
            this.Controls.SetChildIndex(this.txtbrand, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label lable1;
        private Win.UI.Label Style;
        private Win.UI.Label label2;
        //private Class.SCIControls.CustomizeControls.Brand.MultiPickIdBoxBrand txtBrand;
        private Win.UI.Label label5;
        private Win.UI.Label label3;
        //private Class.SCIControls.CustomizeControls.Country.MultiplePickIdCountry txtCOO;
        private Win.UI.EditBox editProgram;
        private Win.UI.EditBox editArtworkType;
        private Win.UI.TextBox txtMuiltSeason;
        private Win.UI.TextBox txtStyle;
        private Win.UI.CheckBox UI_ckbExcludeLocalStyle;
        private Win.UI.CheckBox UI_ckbExcludeDevOptionStyle;
        private Class.Txtbrand txtbrand;
    }
}