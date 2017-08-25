namespace Sci.Production.PPIC
{
    partial class R06
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
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.labelM = new Sci.Win.UI.Label();
            this.labelOrderType = new Sci.Win.UI.Label();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.comboM = new Sci.Win.UI.ComboBox();
            this.comboOrderType = new Sci.Win.UI.ComboBox();
            this.checkExcludedReplacementItem = new Sci.Win.UI.CheckBox();
            this.checkPOMaterialCompletion = new Sci.Win.UI.CheckBox();
            this.labelFactory = new Sci.Win.UI.Label();
            this.comboFactory = new Sci.Win.UI.ComboBox();
            this.lbsp = new Sci.Win.UI.Label();
            this.labelSign = new Sci.Win.UI.Label();
            this.textSPEnd = new Sci.Win.UI.TextBox();
            this.textSPStart = new Sci.Win.UI.TextBox();
            this.SuspendLayout();
            // 
            // print
            // 
            this.print.Location = new System.Drawing.Point(424, 12);
            this.print.TabIndex = 6;
            // 
            // toexcel
            // 
            this.toexcel.Location = new System.Drawing.Point(424, 48);
            this.toexcel.TabIndex = 7;
            // 
            // close
            // 
            this.close.Location = new System.Drawing.Point(424, 84);
            this.close.TabIndex = 8;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Location = new System.Drawing.Point(13, 41);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(84, 23);
            this.labelSCIDelivery.TabIndex = 94;
            this.labelSCIDelivery.Text = "SCI Delivery";
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(13, 71);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(84, 23);
            this.labelM.TabIndex = 95;
            this.labelM.Text = "M";
            // 
            // labelOrderType
            // 
            this.labelOrderType.Location = new System.Drawing.Point(13, 130);
            this.labelOrderType.Name = "labelOrderType";
            this.labelOrderType.Size = new System.Drawing.Size(84, 23);
            this.labelOrderType.TabIndex = 96;
            this.labelOrderType.Text = "Order Type";
            // 
            // dateSCIDelivery
            // 
            this.dateSCIDelivery.IsRequired = false;
            this.dateSCIDelivery.Location = new System.Drawing.Point(102, 41);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 2;
            // 
            // comboM
            // 
            this.comboM.BackColor = System.Drawing.Color.White;
            this.comboM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboM.FormattingEnabled = true;
            this.comboM.IsSupportUnselect = true;
            this.comboM.Location = new System.Drawing.Point(102, 70);
            this.comboM.Name = "comboM";
            this.comboM.Size = new System.Drawing.Size(80, 24);
            this.comboM.TabIndex = 3;
            // 
            // comboOrderType
            // 
            this.comboOrderType.BackColor = System.Drawing.Color.White;
            this.comboOrderType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboOrderType.FormattingEnabled = true;
            this.comboOrderType.IsSupportUnselect = true;
            this.comboOrderType.Location = new System.Drawing.Point(102, 130);
            this.comboOrderType.Name = "comboOrderType";
            this.comboOrderType.Size = new System.Drawing.Size(110, 24);
            this.comboOrderType.TabIndex = 5;
            // 
            // checkExcludedReplacementItem
            // 
            this.checkExcludedReplacementItem.AutoSize = true;
            this.checkExcludedReplacementItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkExcludedReplacementItem.Location = new System.Drawing.Point(13, 160);
            this.checkExcludedReplacementItem.Name = "checkExcludedReplacementItem";
            this.checkExcludedReplacementItem.Size = new System.Drawing.Size(201, 21);
            this.checkExcludedReplacementItem.TabIndex = 6;
            this.checkExcludedReplacementItem.Text = "Excluded Replacement Item";
            this.checkExcludedReplacementItem.UseVisualStyleBackColor = true;
            // 
            // checkPOMaterialCompletion
            // 
            this.checkPOMaterialCompletion.AutoSize = true;
            this.checkPOMaterialCompletion.Checked = true;
            this.checkPOMaterialCompletion.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkPOMaterialCompletion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkPOMaterialCompletion.Location = new System.Drawing.Point(13, 188);
            this.checkPOMaterialCompletion.Name = "checkPOMaterialCompletion";
            this.checkPOMaterialCompletion.Size = new System.Drawing.Size(183, 21);
            this.checkPOMaterialCompletion.TabIndex = 7;
            this.checkPOMaterialCompletion.Text = "PO# Material Completion";
            this.checkPOMaterialCompletion.UseVisualStyleBackColor = true;
            // 
            // labelFactory
            // 
            this.labelFactory.Location = new System.Drawing.Point(13, 100);
            this.labelFactory.Name = "labelFactory";
            this.labelFactory.Size = new System.Drawing.Size(84, 23);
            this.labelFactory.TabIndex = 111;
            this.labelFactory.Text = "Factory";
            // 
            // comboFactory
            // 
            this.comboFactory.BackColor = System.Drawing.Color.White;
            this.comboFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboFactory.FormattingEnabled = true;
            this.comboFactory.IsSupportUnselect = true;
            this.comboFactory.Location = new System.Drawing.Point(102, 100);
            this.comboFactory.Name = "comboFactory";
            this.comboFactory.Size = new System.Drawing.Size(80, 24);
            this.comboFactory.TabIndex = 4;
            // 
            // lbsp
            // 
            this.lbsp.Location = new System.Drawing.Point(13, 12);
            this.lbsp.Name = "lbsp";
            this.lbsp.Size = new System.Drawing.Size(84, 23);
            this.lbsp.TabIndex = 112;
            this.lbsp.Text = "SP#";
            // 
            // labelSign
            // 
            this.labelSign.BackColor = System.Drawing.Color.Transparent;
            this.labelSign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.labelSign.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSign.Location = new System.Drawing.Point(222, 15);
            this.labelSign.Name = "labelSign";
            this.labelSign.Size = new System.Drawing.Size(27, 23);
            this.labelSign.TabIndex = 115;
            this.labelSign.Text = "~";
            this.labelSign.TextStyle.Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelSign.TextStyle.BorderColor = System.Drawing.Color.Black;
            this.labelSign.TextStyle.Color = System.Drawing.Color.Black;
            // 
            // textSPEnd
            // 
            this.textSPEnd.BackColor = System.Drawing.Color.White;
            this.textSPEnd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textSPEnd.Location = new System.Drawing.Point(252, 12);
            this.textSPEnd.Name = "textSPEnd";
            this.textSPEnd.Size = new System.Drawing.Size(117, 23);
            this.textSPEnd.TabIndex = 1;
            // 
            // textSPStart
            // 
            this.textSPStart.BackColor = System.Drawing.Color.White;
            this.textSPStart.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textSPStart.Location = new System.Drawing.Point(102, 12);
            this.textSPStart.Name = "textSPStart";
            this.textSPStart.Size = new System.Drawing.Size(117, 23);
            this.textSPStart.TabIndex = 0;
            // 
            // R06
            // 
            this.ClientSize = new System.Drawing.Size(516, 242);
            this.Controls.Add(this.labelSign);
            this.Controls.Add(this.textSPEnd);
            this.Controls.Add(this.textSPStart);
            this.Controls.Add(this.lbsp);
            this.Controls.Add(this.comboFactory);
            this.Controls.Add(this.labelFactory);
            this.Controls.Add(this.checkPOMaterialCompletion);
            this.Controls.Add(this.checkExcludedReplacementItem);
            this.Controls.Add(this.comboOrderType);
            this.Controls.Add(this.comboM);
            this.Controls.Add(this.dateSCIDelivery);
            this.Controls.Add(this.labelOrderType);
            this.Controls.Add(this.labelM);
            this.Controls.Add(this.labelSCIDelivery);
            this.DefaultControl = "dateSCIDelivery";
            this.DefaultControlForEdit = "dateSCIDelivery";
            this.IsSupportToPrint = false;
            this.Name = "R06";
            this.Text = "R06. Monthly Material Completion";
            this.Controls.SetChildIndex(this.labelSCIDelivery, 0);
            this.Controls.SetChildIndex(this.labelM, 0);
            this.Controls.SetChildIndex(this.labelOrderType, 0);
            this.Controls.SetChildIndex(this.dateSCIDelivery, 0);
            this.Controls.SetChildIndex(this.comboM, 0);
            this.Controls.SetChildIndex(this.comboOrderType, 0);
            this.Controls.SetChildIndex(this.checkExcludedReplacementItem, 0);
            this.Controls.SetChildIndex(this.checkPOMaterialCompletion, 0);
            this.Controls.SetChildIndex(this.labelFactory, 0);
            this.Controls.SetChildIndex(this.comboFactory, 0);
            this.Controls.SetChildIndex(this.print, 0);
            this.Controls.SetChildIndex(this.toexcel, 0);
            this.Controls.SetChildIndex(this.close, 0);
            this.Controls.SetChildIndex(this.lbsp, 0);
            this.Controls.SetChildIndex(this.textSPStart, 0);
            this.Controls.SetChildIndex(this.textSPEnd, 0);
            this.Controls.SetChildIndex(this.labelSign, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Label labelM;
        private Win.UI.Label labelOrderType;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.ComboBox comboM;
        private Win.UI.ComboBox comboOrderType;
        private Win.UI.CheckBox checkExcludedReplacementItem;
        private Win.UI.CheckBox checkPOMaterialCompletion;
        private Win.UI.Label labelFactory;
        private Win.UI.ComboBox comboFactory;
        private Win.UI.Label lbsp;
        private Win.UI.Label labelSign;
        private Win.UI.TextBox textSPEnd;
        private Win.UI.TextBox textSPStart;
    }
}
