namespace Sci.Production.IE
{
    partial class B16
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
            this.txtTypeID = new Sci.Win.UI.DisplayBox();
            this.lblTypeID = new Sci.Win.UI.Label();
            this.txtID = new Sci.Win.UI.DisplayBox();
            this.lblID = new Sci.Win.UI.Label();
            this.txtName = new Sci.Win.UI.DisplayBox();
            this.lblName = new Sci.Win.UI.Label();
            this.displayBox3 = new Sci.Win.UI.DisplayBox();
            this.lblType = new Sci.Win.UI.Label();
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
            this.detailcont.Controls.Add(this.displayBox3);
            this.detailcont.Controls.Add(this.lblType);
            this.detailcont.Controls.Add(this.txtName);
            this.detailcont.Controls.Add(this.lblName);
            this.detailcont.Controls.Add(this.txtID);
            this.detailcont.Controls.Add(this.lblID);
            this.detailcont.Controls.Add(this.txtTypeID);
            this.detailcont.Controls.Add(this.lblTypeID);
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
            // txtTypeID
            // 
            this.txtTypeID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtTypeID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Type", true));
            this.txtTypeID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtTypeID.Location = new System.Drawing.Point(105, 26);
            this.txtTypeID.Name = "txtTypeID";
            this.txtTypeID.Size = new System.Drawing.Size(138, 23);
            this.txtTypeID.TabIndex = 6;
            // 
            // lblTypeID
            // 
            this.lblTypeID.Location = new System.Drawing.Point(26, 26);
            this.lblTypeID.Name = "lblTypeID";
            this.lblTypeID.Size = new System.Drawing.Size(75, 23);
            this.lblTypeID.TabIndex = 5;
            this.lblTypeID.Text = "TypeID";
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtID.Location = new System.Drawing.Point(105, 78);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(274, 23);
            this.txtID.TabIndex = 8;
            // 
            // lblID
            // 
            this.lblID.Location = new System.Drawing.Point(26, 78);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(75, 23);
            this.lblID.TabIndex = 7;
            this.lblID.Text = "ID";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtName.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Name", true));
            this.txtName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtName.Location = new System.Drawing.Point(105, 123);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(416, 23);
            this.txtName.TabIndex = 10;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(26, 123);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(75, 23);
            this.lblName.TabIndex = 9;
            this.lblName.Text = "Name";
            // 
            // displayBox3
            // 
            this.displayBox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox3.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ReasonName", true));
            this.displayBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox3.Location = new System.Drawing.Point(383, 26);
            this.displayBox3.Name = "displayBox3";
            this.displayBox3.Size = new System.Drawing.Size(138, 23);
            this.displayBox3.TabIndex = 12;
            // 
            // lblType
            // 
            this.lblType.Location = new System.Drawing.Point(304, 26);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(75, 23);
            this.lblType.TabIndex = 11;
            this.lblType.Text = "Type";
            // 
            // B16
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportLocate = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B16";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B16. IE Select Code";
            this.WorkAlias = "IESelectCode";
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

        private Win.UI.DisplayBox displayBox3;
        private Win.UI.Label lblType;
        private Win.UI.DisplayBox txtName;
        private Win.UI.Label lblName;
        private Win.UI.DisplayBox txtID;
        private Win.UI.Label lblID;
        private Win.UI.DisplayBox txtTypeID;
        private Win.UI.Label lblTypeID;
    }
}