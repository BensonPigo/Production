namespace Sci.Production.Centralized
{
    partial class Shipping_B04
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
            this.txtAccountNo = new Sci.Win.UI.TextBox();
            this.comboSharebase = new Sci.Win.UI.ComboBox();
            this.labelSharebase = new Sci.Win.UI.Label();
            this.labelShippingMode = new Sci.Win.UI.Label();
            this.labelExpenseReason = new Sci.Win.UI.Label();
            this.labelAccountNo = new Sci.Win.UI.Label();
            this.chkJunk = new Sci.Win.UI.CheckBox();
            this.comboExpenseReason = new Sci.Production.Class.ComboDropDownList(this.components);
            this.txtShipModeID = new Sci.Win.UI.TextBox();
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
            this.detail.Size = new System.Drawing.Size(792, 388);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtShipModeID);
            this.detailcont.Controls.Add(this.comboExpenseReason);
            this.detailcont.Controls.Add(this.chkJunk);
            this.detailcont.Controls.Add(this.txtAccountNo);
            this.detailcont.Controls.Add(this.comboSharebase);
            this.detailcont.Controls.Add(this.labelSharebase);
            this.detailcont.Controls.Add(this.labelShippingMode);
            this.detailcont.Controls.Add(this.labelExpenseReason);
            this.detailcont.Controls.Add(this.labelAccountNo);
            this.detailcont.Size = new System.Drawing.Size(792, 350);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 350);
            this.detailbtm.Size = new System.Drawing.Size(792, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(792, 388);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(800, 417);
            // 
            // txtAccountNo
            // 
            this.txtAccountNo.BackColor = System.Drawing.Color.White;
            this.txtAccountNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "AccountID", true));
            this.txtAccountNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtAccountNo.Location = new System.Drawing.Point(142, 26);
            this.txtAccountNo.Name = "txtAccountNo";
            this.txtAccountNo.Size = new System.Drawing.Size(97, 23);
            this.txtAccountNo.TabIndex = 1;
            // 
            // comboSharebase
            // 
            this.comboSharebase.BackColor = System.Drawing.Color.White;
            this.comboSharebase.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "ShareBase", true));
            this.comboSharebase.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboSharebase.FormattingEnabled = true;
            this.comboSharebase.IsSupportUnselect = true;
            this.comboSharebase.Location = new System.Drawing.Point(142, 101);
            this.comboSharebase.Name = "comboSharebase";
            this.comboSharebase.OldText = "";
            this.comboSharebase.Size = new System.Drawing.Size(97, 24);
            this.comboSharebase.TabIndex = 3;
            // 
            // labelSharebase
            // 
            this.labelSharebase.Location = new System.Drawing.Point(24, 101);
            this.labelSharebase.Name = "labelSharebase";
            this.labelSharebase.Size = new System.Drawing.Size(115, 23);
            this.labelSharebase.TabIndex = 15;
            this.labelSharebase.Text = "Share base";
            // 
            // labelShippingMode
            // 
            this.labelShippingMode.Location = new System.Drawing.Point(24, 138);
            this.labelShippingMode.Name = "labelShippingMode";
            this.labelShippingMode.Size = new System.Drawing.Size(115, 23);
            this.labelShippingMode.TabIndex = 14;
            this.labelShippingMode.Text = "Shipping Mode";
            // 
            // labelExpenseReason
            // 
            this.labelExpenseReason.Location = new System.Drawing.Point(24, 62);
            this.labelExpenseReason.Name = "labelExpenseReason";
            this.labelExpenseReason.Size = new System.Drawing.Size(115, 23);
            this.labelExpenseReason.TabIndex = 13;
            this.labelExpenseReason.Text = "Expense Reason";
            // 
            // labelAccountNo
            // 
            this.labelAccountNo.Location = new System.Drawing.Point(24, 26);
            this.labelAccountNo.Name = "labelAccountNo";
            this.labelAccountNo.Size = new System.Drawing.Size(115, 23);
            this.labelAccountNo.TabIndex = 11;
            this.labelAccountNo.Text = "Account No.";
            // 
            // chkJunk
            // 
            this.chkJunk.AutoSize = true;
            this.chkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.chkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkJunk.Location = new System.Drawing.Point(245, 28);
            this.chkJunk.Name = "chkJunk";
            this.chkJunk.Size = new System.Drawing.Size(57, 21);
            this.chkJunk.TabIndex = 21;
            this.chkJunk.Text = "Junk";
            this.chkJunk.UseVisualStyleBackColor = true;
            // 
            // comboExpenseReason
            // 
            this.comboExpenseReason.AddAllItem = false;
            this.comboExpenseReason.BackColor = System.Drawing.Color.White;
            this.comboExpenseReason.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ExpenseReason", true));
            this.comboExpenseReason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboExpenseReason.FormattingEnabled = true;
            this.comboExpenseReason.IsSupportUnselect = true;
            this.comboExpenseReason.Location = new System.Drawing.Point(142, 62);
            this.comboExpenseReason.Name = "comboExpenseReason";
            this.comboExpenseReason.OldText = "";
            this.comboExpenseReason.Size = new System.Drawing.Size(188, 24);
            this.comboExpenseReason.TabIndex = 2;
            this.comboExpenseReason.Type = null;
            // 
            // txtShipModeID
            // 
            this.txtShipModeID.BackColor = System.Drawing.Color.White;
            this.txtShipModeID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ShipModeID", true));
            this.txtShipModeID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtShipModeID.Location = new System.Drawing.Point(142, 138);
            this.txtShipModeID.Name = "txtShipModeID";
            this.txtShipModeID.PopUpMode = Sci.Win.UI.TextBoxPopUpMode.EditMode;
            this.txtShipModeID.Size = new System.Drawing.Size(432, 23);
            this.txtShipModeID.TabIndex = 22;
            this.txtShipModeID.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtShipModeID_PopUp);
            // 
            // Shipping_B04
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.ConnectionName = "ProductionTPE";
            this.IsDeleteOnBrowse = false;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "Shipping_B04";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "Shipping_B04. Share base of Share Expense";
            this.WorkAlias = "ShareRule";
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

        private Win.UI.TextBox txtAccountNo;
        private Win.UI.ComboBox comboSharebase;
        private Win.UI.Label labelSharebase;
        private Win.UI.Label labelShippingMode;
        private Win.UI.Label labelExpenseReason;
        private Win.UI.Label labelAccountNo;
        private Win.UI.CheckBox chkJunk;
        private Class.ComboDropDownList comboExpenseReason;
        private Win.UI.TextBox txtShipModeID;
    }
}