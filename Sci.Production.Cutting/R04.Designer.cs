namespace Sci.Production.Cutting
{
    partial class R04
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
            this.comboFactory = new Sci.Production.Class.ComboFactory(this.components);
            this.labelEstCutDate = new Sci.Win.UI.Label();
            this.radioByDetail = new Sci.Win.UI.RadioButton();
            this.radioByFactory = new Sci.Win.UI.RadioButton();
            this.radioByCutCell = new Sci.Win.UI.RadioButton();
            this.radioByM = new Sci.Win.UI.RadioButton();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.txtCutCellEnd = new Sci.Win.UI.TextBox();
            this.label9 = new Sci.Win.UI.Label();
            this.txtCutCellStart = new Sci.Win.UI.TextBox();
            this.dateEstCutDate = new Sci.Win.UI.DateRange();
            this.labelReportType = new Sci.Win.UI.Label();
            this.labelCutCell = new Sci.Win.UI.Label();
            this.labelFactory = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Enabled = false;
            this.print.Location = new System.Drawing.Point(460, 12);
            this.print.TabIndex = 1;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(460, 48);
            this.toexcel.TabIndex = 2;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(460, 84);
            this.close.TabIndex = 3;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.comboFactory);
            this.panel1.Controls.Add(this.labelEstCutDate);
            this.panel1.Controls.Add(this.radioByDetail);
            this.panel1.Controls.Add(this.radioByFactory);
            this.panel1.Controls.Add(this.radioByCutCell);
            this.panel1.Controls.Add(this.radioByM);
            this.panel1.Controls.Add(this.comboM);
            this.panel1.Controls.Add(this.txtCutCellEnd);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtCutCellStart);
            this.panel1.Controls.Add(this.dateEstCutDate);
            this.panel1.Controls.Add(this.labelReportType);
            this.panel1.Controls.Add(this.labelCutCell);
            this.panel1.Controls.Add(this.labelFactory);
            this.panel1.Controls.Add(this.labelM);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(418, 215);
            this.panel1.TabIndex = 0;
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.FilteMDivision = true;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IssupportJunk = false;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(331, 12);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(80, 24);
            this.comboFactory.TabIndex = 116;
            this.comboFactory.Visible = false;
            // 
            // labelEstCutDate
            // 
            this.labelEstCutDate.Location = new System.Drawing.Point(11, 43);
            this.labelEstCutDate.Name = "labelEstCutDate";
            this.labelEstCutDate.Size = new System.Drawing.Size(95, 23);
            this.labelEstCutDate.TabIndex = 115;
            this.labelEstCutDate.Text = "Est. Cut Date";
            // 
            // radioByDetail
            // 
            this.radioByDetail.AutoSize = true;
            this.radioByDetail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioByDetail.Location = new System.Drawing.Point(109, 184);
            this.radioByDetail.Name = "radioByDetail";
            this.radioByDetail.Size = new System.Drawing.Size(82, 21);
            this.radioByDetail.TabIndex = 6;
            this.radioByDetail.Text = "By Detail";
            this.radioByDetail.UseVisualStyleBackColor = true;
            this.radioByDetail.CheckedChanged += new System.EventHandler(this.radioByDetail_CheckedChanged);
            // 
            // radioByFactory
            // 
            this.radioByFactory.AutoSize = true;
            this.radioByFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioByFactory.Location = new System.Drawing.Point(109, 130);
            this.radioByFactory.Name = "radioByFactory";
            this.radioByFactory.Size = new System.Drawing.Size(93, 21);
            this.radioByFactory.TabIndex = 5;
            this.radioByFactory.Text = "By Factory";
            this.radioByFactory.UseVisualStyleBackColor = true;
            this.radioByFactory.CheckedChanged += new System.EventHandler(this.radioByFactory_CheckedChanged);
            // 
            // radioByCutCell
            // 
            this.radioByCutCell.AutoSize = true;
            this.radioByCutCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioByCutCell.Location = new System.Drawing.Point(109, 157);
            this.radioByCutCell.Name = "radioByCutCell";
            this.radioByCutCell.Size = new System.Drawing.Size(94, 21);
            this.radioByCutCell.TabIndex = 5;
            this.radioByCutCell.Text = "By Cut Cell";
            this.radioByCutCell.UseVisualStyleBackColor = true;
            this.radioByCutCell.CheckedChanged += new System.EventHandler(this.radioByCutCell_CheckedChanged);
            // 
            // radioByM
            // 
            this.radioByM.AutoSize = true;
            this.radioByM.Checked = true;
            this.radioByM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.radioByM.Location = new System.Drawing.Point(109, 103);
            this.radioByM.Name = "radioByM";
            this.radioByM.Size = new System.Drawing.Size(57, 21);
            this.radioByM.TabIndex = 4;
            this.radioByM.TabStop = true;
            this.radioByM.Text = "By M";
            this.radioByM.UseVisualStyleBackColor = true;
            this.radioByM.CheckedChanged += new System.EventHandler(this.radioByM_CheckedChanged);
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(109, 12);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(121, 24);
            this.comboM.TabIndex = 0;
            this.comboM.TextChanged += new System.EventHandler(this.comboM_TextChanged);
            // 
            // txtCutCellEnd
            // 
            this.txtCutCellEnd.BackColor = System.Drawing.Color.White;
            this.txtCutCellEnd.Enabled = false;
            this.txtCutCellEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutCellEnd.Location = new System.Drawing.Point(194, 73);
            this.txtCutCellEnd.MaxLength = 2;
            this.txtCutCellEnd.Name = "txtCutCellEnd";
            this.txtCutCellEnd.Size = new System.Drawing.Size(65, 23);
            this.txtCutCellEnd.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(171, 73);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 23);
            this.label9.TabIndex = 8;
            this.label9.Text = "～";
            this.label9.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.Color = System.Drawing.Color.Black;
            this.label9.TextStyle.ExtBorderColor = System.Drawing.Color.Black;
            this.label9.TextStyle.GradientColor = System.Drawing.Color.Black;
            // 
            // txtCutCellStart
            // 
            this.txtCutCellStart.BackColor = System.Drawing.Color.White;
            this.txtCutCellStart.Enabled = false;
            this.txtCutCellStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCutCellStart.Location = new System.Drawing.Point(109, 73);
            this.txtCutCellStart.MaxLength = 2;
            this.txtCutCellStart.Name = "txtCutCellStart";
            this.txtCutCellStart.Size = new System.Drawing.Size(65, 23);
            this.txtCutCellStart.TabIndex = 2;
            // 
            // dateEstCutDate
            // 
            this.dateEstCutDate.IsRequired = false;
            this.dateEstCutDate.Location = new System.Drawing.Point(109, 43);
            this.dateEstCutDate.Name = "dateEstCutDate";
            this.dateEstCutDate.Size = new System.Drawing.Size(280, 23);
            this.dateEstCutDate.TabIndex = 1;
            // 
            // labelReportType
            // 
            this.labelReportType.Location = new System.Drawing.Point(11, 103);
            this.labelReportType.Name = "labelReportType";
            this.labelReportType.Size = new System.Drawing.Size(95, 23);
            this.labelReportType.TabIndex = 99;
            this.labelReportType.Text = "Report Type";
            // 
            // labelCutCell
            // 
            this.labelCutCell.Location = new System.Drawing.Point(11, 73);
            this.labelCutCell.Name = "labelCutCell";
            this.labelCutCell.Size = new System.Drawing.Size(95, 23);
            this.labelCutCell.TabIndex = 98;
            this.labelCutCell.Text = "Cut Cell";
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(233, 13);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(95, 23);
            this.labelFactory.TabIndex = 96;
            this.labelFactory.Text = "Factory";
            this.labelFactory.Visible = false;
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(11, 13);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(95, 23);
            this.labelM.TabIndex = 96;
            this.labelM.Text = "M";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(448, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 22);
            this.label4.TabIndex = 96;
            this.label4.Text = "Paper Size A4";
            // 
            // R04
            // 
            this.ClientSize = new System.Drawing.Size(547, 258);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel1);
            this.DefaultControl = "comboM";
            this.DefaultControlForEdit = "comboM";
            this.Name = "R04";
            this.Text = "R04.Cutting BCS Report";
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Panel panel1;
        private Win.UI.RadioButton radioByDetail;
        private Win.UI.RadioButton radioByCutCell;
        private Win.UI.RadioButton radioByM;
        private Win.UI.ComboBox comboM;
        private Win.UI.TextBox txtCutCellEnd;
        private Win.UI.Label label9;
        private Win.UI.TextBox txtCutCellStart;
        private Win.UI.DateRange dateEstCutDate;
        private Win.UI.Label labelReportType;
        private Win.UI.Label labelCutCell;
        private Win.UI.Label labelM;
        private Win.UI.Label labelEstCutDate;
        private Win.UI.Label label4;
        private Win.UI.RadioButton radioByFactory;
        private Win.UI.Label labelFactory;
        private Class.ComboFactory comboFactory;
    }
}
