namespace Sci.Production.PPIC
{
    partial class P19
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
            this.btnPMS = new Sci.Win.UI.Button();
            this.btnMWS = new Sci.Win.UI.Button();
            this.dateInputDate = new Sci.Win.UI.DateBox();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.btn_exp_AutoFabric = new Sci.Win.UI.Button();
            this.btn_imp_AutoFabric = new Sci.Win.UI.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(26, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(266, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Export Finishing Process data from PMS ";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(26, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(266, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Import Finishing Process data from MWS";
            // 
            // btnPMS
            // 
            this.btnPMS.Location = new System.Drawing.Point(431, 19);
            this.btnPMS.Name = "btnPMS";
            this.btnPMS.Size = new System.Drawing.Size(80, 30);
            this.btnPMS.TabIndex = 1;
            this.btnPMS.Text = "Update";
            this.btnPMS.UseVisualStyleBackColor = true;
            this.btnPMS.Click += new System.EventHandler(this.BtnPMS_Click);
            // 
            // btnMWS
            // 
            this.btnMWS.Location = new System.Drawing.Point(431, 67);
            this.btnMWS.Name = "btnMWS";
            this.btnMWS.Size = new System.Drawing.Size(80, 30);
            this.btnMWS.TabIndex = 2;
            this.btnMWS.Text = "Update";
            this.btnMWS.UseVisualStyleBackColor = true;
            this.btnMWS.Click += new System.EventHandler(this.BtnMWS_Click);
            // 
            // dateInputDate
            // 
            this.dateInputDate.IsSupportEditMode = false;
            this.dateInputDate.Location = new System.Drawing.Point(295, 26);
            this.dateInputDate.Name = "dateInputDate";
            this.dateInputDate.Size = new System.Drawing.Size(130, 23);
            this.dateInputDate.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(26, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(266, 23);
            this.label3.TabIndex = 4;
            this.label3.Text = "Export Auto W/H Fabric data from PMS";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(26, 162);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(266, 23);
            this.label4.TabIndex = 5;
            this.label4.Text = "Import Auto W/H Fabric data from MWS";
            // 
            // btn_exp_AutoFabric
            // 
            this.btn_exp_AutoFabric.Location = new System.Drawing.Point(431, 112);
            this.btn_exp_AutoFabric.Name = "btn_exp_AutoFabric";
            this.btn_exp_AutoFabric.Size = new System.Drawing.Size(80, 30);
            this.btn_exp_AutoFabric.TabIndex = 6;
            this.btn_exp_AutoFabric.Text = "Update";
            this.btn_exp_AutoFabric.UseVisualStyleBackColor = true;
            this.btn_exp_AutoFabric.Click += new System.EventHandler(this.Btn_exp_AutoFabric_Click);
            // 
            // btn_imp_AutoFabric
            // 
            this.btn_imp_AutoFabric.Location = new System.Drawing.Point(431, 158);
            this.btn_imp_AutoFabric.Name = "btn_imp_AutoFabric";
            this.btn_imp_AutoFabric.Size = new System.Drawing.Size(80, 30);
            this.btn_imp_AutoFabric.TabIndex = 7;
            this.btn_imp_AutoFabric.Text = "Update";
            this.btn_imp_AutoFabric.UseVisualStyleBackColor = true;
            this.btn_imp_AutoFabric.Click += new System.EventHandler(this.Btn_imp_AutoFabric_Click);
            // 
            // P19
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 216);
            this.Controls.Add(this.btn_imp_AutoFabric);
            this.Controls.Add(this.btn_exp_AutoFabric);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateInputDate);
            this.Controls.Add(this.btnMWS);
            this.Controls.Add(this.btnPMS);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "P19";
            this.OnLineHelpID = "Sci.Win.Tems.Base";
            this.Text = "P19. Update Finishing Process data (For SNP)";
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.btnPMS, 0);
            this.Controls.SetChildIndex(this.btnMWS, 0);
            this.Controls.SetChildIndex(this.dateInputDate, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.btn_exp_AutoFabric, 0);
            this.Controls.SetChildIndex(this.btn_imp_AutoFabric, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label label1;
        private Win.UI.Label label2;
        private Win.UI.Button btnPMS;
        private Win.UI.Button btnMWS;
        private Win.UI.DateBox dateInputDate;
        private Win.UI.Label label3;
        private Win.UI.Label label4;
        private Win.UI.Button btn_exp_AutoFabric;
        private Win.UI.Button btn_imp_AutoFabric;
    }
}