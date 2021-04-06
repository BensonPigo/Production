
namespace Sci.Production.Packing
{
    partial class P18_PackingError
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
            this.lbPackingError = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.cbmPackingError = new Sci.Win.UI.ComboBox();
            this.numErrorQty = new Sci.Win.UI.NumericBox();
            this.txtAuditQC = new Sci.Win.UI.TextBox();
            this.btnOK = new Sci.Win.UI.Button();
            this.SuspendLayout();
            // 
            // lbPackingError
            // 
            this.lbPackingError.Location = new System.Drawing.Point(9, 8);
            this.lbPackingError.Name = "lbPackingError";
            this.lbPackingError.Size = new System.Drawing.Size(118, 23);
            this.lbPackingError.TabIndex = 101;
            this.lbPackingError.Text = "Packing Error";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(118, 23);
            this.label1.TabIndex = 102;
            this.label1.Text = "Error Qty";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 23);
            this.label2.TabIndex = 103;
            this.label2.Text = "Packing Audit QC";
            // 
            // cbmPackingError
            // 
            this.cbmPackingError.BackColor = System.Drawing.Color.White;
            this.cbmPackingError.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cbmPackingError.FormattingEnabled = true;
            this.cbmPackingError.IsSupportUnselect = true;
            this.cbmPackingError.Location = new System.Drawing.Point(130, 8);
            this.cbmPackingError.Name = "cbmPackingError";
            this.cbmPackingError.OldText = "";
            this.cbmPackingError.Size = new System.Drawing.Size(167, 24);
            this.cbmPackingError.TabIndex = 104;
            // 
            // numErrorQty
            // 
            this.numErrorQty.BackColor = System.Drawing.Color.White;
            this.numErrorQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numErrorQty.Location = new System.Drawing.Point(130, 45);
            this.numErrorQty.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numErrorQty.Name = "numErrorQty";
            this.numErrorQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numErrorQty.Size = new System.Drawing.Size(167, 23);
            this.numErrorQty.TabIndex = 105;
            this.numErrorQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // txtAuditQC
            // 
            this.txtAuditQC.BackColor = System.Drawing.Color.White;
            this.txtAuditQC.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtAuditQC.Location = new System.Drawing.Point(130, 80);
            this.txtAuditQC.Name = "txtAuditQC";
            this.txtAuditQC.Size = new System.Drawing.Size(167, 23);
            this.txtAuditQC.TabIndex = 106;
            this.txtAuditQC.Validating += new System.ComponentModel.CancelEventHandler(this.TxtAuditQC_Validating);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(217, 109);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 30);
            this.btnOK.TabIndex = 107;
            this.btnOK.Text = "Ok";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // P18_PackingError
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 156);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtAuditQC);
            this.Controls.Add(this.numErrorQty);
            this.Controls.Add(this.cbmPackingError);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbPackingError);
            this.Name = "P18_PackingError";
            this.OnLineHelpID = "Sci.Win.Tems.QueryForm";
            this.Text = "P18_PackingError";
            this.Controls.SetChildIndex(this.lbPackingError, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.cbmPackingError, 0);
            this.Controls.SetChildIndex(this.numErrorQty, 0);
            this.Controls.SetChildIndex(this.txtAuditQC, 0);
            this.Controls.SetChildIndex(this.btnOK, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lbPackingError;
        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.ComboBox cbmPackingError;
        private Win.UI.NumericBox numErrorQty;
        private Win.UI.TextBox txtAuditQC;
        private Win.UI.Button btnOK;
    }
}