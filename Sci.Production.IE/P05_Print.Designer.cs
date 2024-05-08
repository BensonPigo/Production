namespace Sci.Production.IE
{
    partial class P05_Print
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.comboDisplayBy = new Sci.Production.Class.ComboDropDownList(this.components);
            this.comboContentBy = new Sci.Production.Class.ComboDropDownList(this.components);
            this.comboLanguageBy = new Sci.Production.Class.ComboDropDownList(this.components);
            this.cbDirection = new Sci.Production.Class.ComboDropDownList(this.components);
            this.label4 = new Sci.Win.UI.Label();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(329, 87);
            this.print.Visible = false;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(317, 8);
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(317, 84);
            // 
            // buttonCustomized
            // 
            this.buttonCustomized.Location = new System.Drawing.Point(283, 87);
            // 
            // checkUseCustomized
            // 
            this.checkUseCustomized.Location = new System.Drawing.Point(309, 87);
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(309, 91);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 23);
            this.label1.TabIndex = 97;
            this.label1.Text = "Display By";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 23);
            this.label2.TabIndex = 98;
            this.label2.Text = "Content By";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 23);
            this.label3.TabIndex = 99;
            this.label3.Text = "Language By";
            // 
            // comboDisplayBy
            // 
            this.comboDisplayBy.AddAllItem = false;
            this.comboDisplayBy.BackColor = System.Drawing.Color.White;
            this.comboDisplayBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboDisplayBy.FormattingEnabled = true;
            this.comboDisplayBy.IsSupportUnselect = true;
            this.comboDisplayBy.Location = new System.Drawing.Point(106, 8);
            this.comboDisplayBy.Name = "comboDisplayBy";
            this.comboDisplayBy.OldText = "";
            this.comboDisplayBy.Size = new System.Drawing.Size(163, 24);
            this.comboDisplayBy.TabIndex = 100;
            this.comboDisplayBy.Type = "Pms_LMDisplay";
            // 
            // comboContentBy
            // 
            this.comboContentBy.AddAllItem = false;
            this.comboContentBy.BackColor = System.Drawing.Color.White;
            this.comboContentBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboContentBy.FormattingEnabled = true;
            this.comboContentBy.IsSupportUnselect = true;
            this.comboContentBy.Location = new System.Drawing.Point(106, 47);
            this.comboContentBy.Name = "comboContentBy";
            this.comboContentBy.OldText = "";
            this.comboContentBy.Size = new System.Drawing.Size(163, 24);
            this.comboContentBy.TabIndex = 101;
            this.comboContentBy.Type = "Pms_LMContent";
            // 
            // comboLanguageBy
            // 
            this.comboLanguageBy.AddAllItem = false;
            this.comboLanguageBy.BackColor = System.Drawing.Color.White;
            this.comboLanguageBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboLanguageBy.FormattingEnabled = true;
            this.comboLanguageBy.IsSupportUnselect = true;
            this.comboLanguageBy.Location = new System.Drawing.Point(106, 86);
            this.comboLanguageBy.Name = "comboLanguageBy";
            this.comboLanguageBy.OldText = "";
            this.comboLanguageBy.Size = new System.Drawing.Size(163, 24);
            this.comboLanguageBy.TabIndex = 102;
            this.comboLanguageBy.Type = "Pms_Language";
            // 
            // cbDirection
            // 
            this.cbDirection.AddAllItem = false;
            this.cbDirection.BackColor = System.Drawing.Color.White;
            this.cbDirection.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbDirection.FormattingEnabled = true;
            this.cbDirection.IsSupportUnselect = true;
            this.cbDirection.Location = new System.Drawing.Point(106, 122);
            this.cbDirection.Name = "cbDirection";
            this.cbDirection.OldText = "";
            this.cbDirection.Size = new System.Drawing.Size(163, 24);
            this.cbDirection.TabIndex = 104;
            this.cbDirection.Type = "Pms_Language";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 23);
            this.label4.TabIndex = 103;
            this.label4.Text = "Direction";
            // 
            // P05_Print
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 178);
            this.Controls.Add(this.cbDirection);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboLanguageBy);
            this.Controls.Add(this.comboContentBy);
            this.Controls.Add(this.comboDisplayBy);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "P05_Print";
            this.OnLineHelpID = "Sci.Win.Tems.PrintForm";
            this.Text = "P05_Print";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.buttonCustomized, 0);
            this.Controls.SetChildIndex(this.checkUseCustomized, 0);
            this.Controls.SetChildIndex(this.txtVersion, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.comboDisplayBy, 0);
            this.Controls.SetChildIndex(this.comboContentBy, 0);
            this.Controls.SetChildIndex(this.comboLanguageBy, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.cbDirection, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Class.ComboDropDownList comboDisplayBy;
        private Class.ComboDropDownList comboContentBy;
        private Class.ComboDropDownList comboLanguageBy;
        private Class.ComboDropDownList cbDirection;
        private Win.UI.Label label4;
    }
}