namespace Sci.Production.Thread
{
    partial class R21
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
            this.comboMDivision = new Sci.Production.Class.comboMDivision(this.components);
            this.labelM = new Sci.Win.UI.Label();
            this.label7 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.txtLocationEnd = new Sci.Win.UI.TextBox();
            this.txtLocationStart = new Sci.Win.UI.TextBox();
            this.txtThreadItem = new Sci.Win.UI.TextBox();
            this.txtType = new Sci.Win.UI.TextBox();
            this.txtShade = new Sci.Win.UI.TextBox();
            this.txtRefnoEnd = new Sci.Win.UI.TextBox();
            this.txtRefnoStart = new Sci.Win.UI.TextBox();
            this.labelLocation = new Sci.Win.UI.Label();
            this.labelThreadItem = new Sci.Win.UI.Label();
            this.labelType = new Sci.Win.UI.Label();
            this.labelShade = new Sci.Win.UI.Label();
            this.labelRefno = new Sci.Win.UI.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(463, 12);
            this.print.TabIndex = 1;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(463, 48);
            this.toexcel.TabIndex = 2;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(463, 84);
            this.close.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboMDivision);
            this.panel1.Controls.Add(this.labelM);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtLocationEnd);
            this.panel1.Controls.Add(this.txtLocationStart);
            this.panel1.Controls.Add(this.txtThreadItem);
            this.panel1.Controls.Add(this.txtType);
            this.panel1.Controls.Add(this.txtShade);
            this.panel1.Controls.Add(this.txtRefnoEnd);
            this.panel1.Controls.Add(this.txtRefnoStart);
            this.panel1.Controls.Add(this.labelLocation);
            this.panel1.Controls.Add(this.labelThreadItem);
            this.panel1.Controls.Add(this.labelType);
            this.panel1.Controls.Add(this.labelShade);
            this.panel1.Controls.Add(this.labelRefno);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(434, 190);
            this.panel1.TabIndex = 0;
            // 
            // comboMDivision
            // 
            this.comboMDivision.BackColor = System.Drawing.Color.White;
            this.comboMDivision.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboMDivision.FormattingEnabled = true;
            this.comboMDivision.IsSupportUnselect = true;
            this.comboMDivision.Location = new System.Drawing.Point(99, 153);
            this.comboMDivision.Name = "comboMDivision";
            this.comboMDivision.Size = new System.Drawing.Size(80, 24);
            this.comboMDivision.TabIndex = 7;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(16, 154);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(80, 23);
            this.labelM.TabIndex = 14;
            this.labelM.Text = "M";
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.Control;
            this.label7.Location = new System.Drawing.Point(220, 129);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 23);
            this.label7.TabIndex = 13;
            this.label7.Text = "~";
            this.label7.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.Control;
            this.label6.Location = new System.Drawing.Point(251, 8);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(12, 23);
            this.label6.TabIndex = 12;
            this.label6.Text = "~";
            this.label6.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // txtLocationEnd
            // 
            this.txtLocationEnd.BackColor = System.Drawing.Color.White;
            this.txtLocationEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocationEnd.Location = new System.Drawing.Point(243, 125);
            this.txtLocationEnd.Name = "txtLocationEnd";
            this.txtLocationEnd.Size = new System.Drawing.Size(113, 23);
            this.txtLocationEnd.TabIndex = 6;
            this.txtLocationEnd.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtLocationEnd_PopUp);
            this.txtLocationEnd.Validating += new System.ComponentModel.CancelEventHandler(this.TxtLocationEnd_Validating);
            // 
            // txtLocationStart
            // 
            this.txtLocationStart.BackColor = System.Drawing.Color.White;
            this.txtLocationStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocationStart.Location = new System.Drawing.Point(99, 125);
            this.txtLocationStart.Name = "txtLocationStart";
            this.txtLocationStart.Size = new System.Drawing.Size(113, 23);
            this.txtLocationStart.TabIndex = 5;
            this.txtLocationStart.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtLocationStart_PopUp);
            this.txtLocationStart.Validating += new System.ComponentModel.CancelEventHandler(this.TxtLocationStart_Validating);
            // 
            // txtThreadItem
            // 
            this.txtThreadItem.BackColor = System.Drawing.Color.White;
            this.txtThreadItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtThreadItem.Location = new System.Drawing.Point(99, 96);
            this.txtThreadItem.Name = "txtThreadItem";
            this.txtThreadItem.Size = new System.Drawing.Size(127, 23);
            this.txtThreadItem.TabIndex = 4;
            this.txtThreadItem.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtThreadItem_PopUp);
            this.txtThreadItem.Validating += new System.ComponentModel.CancelEventHandler(this.TxtThreadItem_Validating);
            // 
            // txtType
            // 
            this.txtType.BackColor = System.Drawing.Color.White;
            this.txtType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtType.Location = new System.Drawing.Point(99, 65);
            this.txtType.Name = "txtType";
            this.txtType.Size = new System.Drawing.Size(135, 23);
            this.txtType.TabIndex = 3;
            this.txtType.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtType_PopUp);
            this.txtType.Validating += new System.ComponentModel.CancelEventHandler(this.TxtType_Validating);
            // 
            // txtShade
            // 
            this.txtShade.BackColor = System.Drawing.Color.White;
            this.txtShade.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtShade.Location = new System.Drawing.Point(99, 36);
            this.txtShade.Name = "txtShade";
            this.txtShade.Size = new System.Drawing.Size(122, 23);
            this.txtShade.TabIndex = 2;
            this.txtShade.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtShade_PopUp);
            this.txtShade.Validating += new System.ComponentModel.CancelEventHandler(this.TxtShade_Validating);
            // 
            // txtRefnoEnd
            // 
            this.txtRefnoEnd.BackColor = System.Drawing.Color.White;
            this.txtRefnoEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefnoEnd.Location = new System.Drawing.Point(266, 7);
            this.txtRefnoEnd.Name = "txtRefnoEnd";
            this.txtRefnoEnd.Size = new System.Drawing.Size(148, 23);
            this.txtRefnoEnd.TabIndex = 1;
            this.txtRefnoEnd.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtRefnoEnd_PopUp);
            this.txtRefnoEnd.Validating += new System.ComponentModel.CancelEventHandler(this.TxtRefnoEnd_Validating);
            // 
            // txtRefnoStart
            // 
            this.txtRefnoStart.BackColor = System.Drawing.Color.White;
            this.txtRefnoStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtRefnoStart.Location = new System.Drawing.Point(99, 7);
            this.txtRefnoStart.Name = "txtRefnoStart";
            this.txtRefnoStart.Size = new System.Drawing.Size(148, 23);
            this.txtRefnoStart.TabIndex = 0;
            this.txtRefnoStart.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtRefnoStart_PopUp);
            this.txtRefnoStart.Validating += new System.ComponentModel.CancelEventHandler(this.TxtRefnoStart_Validating);
            // 
            // labelLocation
            // 
            this.labelLocation.Location = new System.Drawing.Point(16, 125);
            this.labelLocation.Name = "labelLocation";
            this.labelLocation.Size = new System.Drawing.Size(80, 23);
            this.labelLocation.TabIndex = 4;
            this.labelLocation.Text = "Location";
            // 
            // labelThreadItem
            // 
            this.labelThreadItem.Location = new System.Drawing.Point(16, 96);
            this.labelThreadItem.Name = "labelThreadItem";
            this.labelThreadItem.Size = new System.Drawing.Size(80, 23);
            this.labelThreadItem.TabIndex = 3;
            this.labelThreadItem.Text = "Thread Item";
            // 
            // labelType
            // 
            this.labelType.Location = new System.Drawing.Point(16, 65);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(80, 23);
            this.labelType.TabIndex = 2;
            this.labelType.Text = "Type";
            // 
            // labelShade
            // 
            this.labelShade.Location = new System.Drawing.Point(16, 36);
            this.labelShade.Name = "labelShade";
            this.labelShade.Size = new System.Drawing.Size(80, 23);
            this.labelShade.TabIndex = 1;
            this.labelShade.Text = "Shade";
            // 
            // labelRefno
            // 
            this.labelRefno.Location = new System.Drawing.Point(16, 8);
            this.labelRefno.Name = "labelRefno";
            this.labelRefno.Size = new System.Drawing.Size(80, 23);
            this.labelRefno.TabIndex = 0;
            this.labelRefno.Text = "Refno";
            // 
            // R21
            // 
            this.ClientSize = new System.Drawing.Size(555, 240);
            this.Controls.Add(this.panel1);
            this.Name = "R21";
            this.Text = "R21.Thread Stock List";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.TextBox txtLocationEnd;
        private Win.UI.TextBox txtLocationStart;
        private Win.UI.TextBox txtThreadItem;
        private Win.UI.TextBox txtType;
        private Win.UI.TextBox txtShade;
        private Win.UI.TextBox txtRefnoEnd;
        private Win.UI.TextBox txtRefnoStart;
        private Win.UI.Label labelLocation;
        private Win.UI.Label labelThreadItem;
        private Win.UI.Label labelType;
        private Win.UI.Label labelShade;
        private Win.UI.Label labelRefno;
        private Win.UI.Label labelM;
        private Class.comboMDivision comboMDivision;
    }
}
