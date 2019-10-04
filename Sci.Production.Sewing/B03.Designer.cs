namespace Sci.Production.Sewing
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
            this.lbDate = new Sci.Win.UI.Label();
            this.txtFactoryID = new Sci.Win.UI.TextBox();
            this.lbFactoryID = new Sci.Win.UI.Label();
            this.dateProductionDate = new Sci.Win.UI.DateBox();
            this.btnCopyDate = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).BeginInit();
            this.masterpanel.SuspendLayout();
            this.detailpanel.SuspendLayout();
            this.detail2.SuspendLayout();
            this.detailpanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // masterpanel
            // 
            this.masterpanel.Controls.Add(this.btnCopyDate);
            this.masterpanel.Controls.Add(this.lbFactoryID);
            this.masterpanel.Controls.Add(this.txtFactoryID);
            this.masterpanel.Controls.Add(this.lbDate);
            this.masterpanel.Controls.Add(this.dateProductionDate);
            this.masterpanel.Size = new System.Drawing.Size(792, 100);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateProductionDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbDate, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtFactoryID, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbFactoryID, 0);
            this.masterpanel.Controls.SetChildIndex(this.btnCopyDate, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Size = new System.Drawing.Size(792, 249);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(792, 249);
            // 
            // detail2
            // 
            this.detail2.Size = new System.Drawing.Size(892, 387);
            // 
            // detailgridcont2
            // 
            this.detailgridcont2.Size = new System.Drawing.Size(886, 341);
            // 
            // detailpanel2
            // 
            this.detailpanel2.Size = new System.Drawing.Size(886, 40);
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(792, 387);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(792, 349);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(792, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(792, 387);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(800, 416);
            // 
            // lbDate
            // 
            this.lbDate.Location = new System.Drawing.Point(5, 10);
            this.lbDate.Name = "lbDate";
            this.lbDate.Size = new System.Drawing.Size(55, 23);
            this.lbDate.TabIndex = 3;
            this.lbDate.Text = "Date";
            // 
            // txtFactoryID
            // 
            this.txtFactoryID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtFactoryID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "FactoryID", true));
            this.txtFactoryID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtFactoryID.IsSupportEditMode = false;
            this.txtFactoryID.Location = new System.Drawing.Point(286, 10);
            this.txtFactoryID.Name = "txtFactoryID";
            this.txtFactoryID.ReadOnly = true;
            this.txtFactoryID.Size = new System.Drawing.Size(111, 23);
            this.txtFactoryID.TabIndex = 1;
            // 
            // lbFactoryID
            // 
            this.lbFactoryID.Location = new System.Drawing.Point(228, 10);
            this.lbFactoryID.Name = "lbFactoryID";
            this.lbFactoryID.Size = new System.Drawing.Size(55, 23);
            this.lbFactoryID.TabIndex = 5;
            this.lbFactoryID.Text = "Factory";
            // 
            // dateProductionDate
            // 
            this.dateProductionDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ProductionDate", true));
            this.dateProductionDate.Location = new System.Drawing.Point(63, 10);
            this.dateProductionDate.Name = "dateProductionDate";
            this.dateProductionDate.Size = new System.Drawing.Size(130, 23);
            this.dateProductionDate.TabIndex = 0;
            // 
            // btnCopyDate
            // 
            this.btnCopyDate.Location = new System.Drawing.Point(422, 10);
            this.btnCopyDate.Name = "btnCopyDate";
            this.btnCopyDate.Size = new System.Drawing.Size(173, 30);
            this.btnCopyDate.TabIndex = 6;
            this.btnCopyDate.Text = "Copy from previous date";
            this.btnCopyDate.UseVisualStyleBackColor = true;
            this.btnCopyDate.Click += new System.EventHandler(this.BtnCopyDate_Click);
            // 
            // B03
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 449);
            this.Grid2UniqueKey = "LineLocationID,SewingLineID,Team";
            this.GridAlias = "ProductionLineAllocation_Detail";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportLocate = false;
            this.IsSupportPrint = false;
            this.KeyField1 = "FactoryID,ProductionDate";
            this.KeyField2 = "FactoryID,ProductionDate";
            this.Name = "B03";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B03";
            this.WorkAlias = "ProductionLineAllocation";
            this.Controls.SetChildIndex(this.tabs, 0);
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).EndInit();
            this.masterpanel.ResumeLayout(false);
            this.masterpanel.PerformLayout();
            this.detailpanel.ResumeLayout(false);
            this.detail2.ResumeLayout(false);
            this.detailpanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label lbDate;
        private Win.UI.Label lbFactoryID;
        private Win.UI.TextBox txtFactoryID;
        private Win.UI.DateBox dateProductionDate;
        private Win.UI.Button btnCopyDate;
    }
}