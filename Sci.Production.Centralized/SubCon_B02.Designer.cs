namespace Sci.Production.Centralized
{
    partial class SubCon_B02
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
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.comboResponsible = new Sci.Production.Class.ComboDropDownList(this.components);
            this.txtReason = new Sci.Win.UI.TextBox();
            this.chk_junk = new Sci.Win.UI.CheckBox();
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
            this.detail.Size = new System.Drawing.Size(682, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.chk_junk);
            this.detailcont.Controls.Add(this.txtReason);
            this.detailcont.Controls.Add(this.comboResponsible);
            this.detailcont.Controls.Add(this.displayID);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Size = new System.Drawing.Size(682, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(682, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(637, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(690, 424);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(33, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(33, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Responsible";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(33, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Reason";
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(121, 22);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(100, 23);
            this.displayID.TabIndex = 3;
            // 
            // comboResponsible
            // 
            this.comboResponsible.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.comboResponsible.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.mtbs, "Responsible", true));
            this.comboResponsible.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.comboResponsible.FormattingEnabled = true;
            this.comboResponsible.IsSupportUnselect = true;
            this.comboResponsible.Location = new System.Drawing.Point(121, 61);
            this.comboResponsible.Name = "comboResponsible";
            this.comboResponsible.OldText = "";
            this.comboResponsible.ReadOnly = true;
            this.comboResponsible.Size = new System.Drawing.Size(121, 24);
            this.comboResponsible.TabIndex = 4;
            this.comboResponsible.Type = "Pms_PoIr_Responsible";
            // 
            // txtReason
            // 
            this.txtReason.BackColor = System.Drawing.Color.White;
            this.txtReason.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Reason", true));
            this.txtReason.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtReason.Location = new System.Drawing.Point(121, 102);
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(452, 23);
            this.txtReason.TabIndex = 5;
            // 
            // chk_junk
            // 
            this.chk_junk.AutoSize = true;
            this.chk_junk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.chk_junk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chk_junk.Location = new System.Drawing.Point(244, 22);
            this.chk_junk.Name = "chk_junk";
            this.chk_junk.Size = new System.Drawing.Size(57, 21);
            this.chk_junk.TabIndex = 6;
            this.chk_junk.Text = "Junk";
            this.chk_junk.UseVisualStyleBackColor = true;
            // 
            // SubCon_B02
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 457);
            this.ConnectionName = "ProductionTPE";
            this.DefaultFilter = "Type = \'IP\'";
            this.IsDeleteOnBrowse = false;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "SubCon_B02";
            this.Text = "SubCon_B02. Irregular Price Reason";
            this.WorkAlias = "SubconReason";
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

        private Win.UI.Label label1;
        private Win.UI.CheckBox chk_junk;
        private Win.UI.TextBox txtReason;
        private Class.ComboDropDownList comboResponsible;
        private Win.UI.DisplayBox displayID;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
    }
}