namespace Sci.Production.Cutting
{
    partial class B02
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
            this.labelM = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.labelCellNo = new Sci.Win.UI.Label();
            this.displayM = new Sci.Win.UI.DisplayBox();
            this.txtCellNo = new Sci.Win.UI.TextBox();
            this.txtDescription = new Sci.Win.UI.TextBox();
            this.labelCuttingWidth = new Sci.Win.UI.Label();
            this.textCuttingWidth = new Sci.Win.UI.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.txtCuttingMachineID1 = new Sci.Production.Class.TxtCuttingMachineID();
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
            this.detail.Size = new System.Drawing.Size(826, 430);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtCuttingMachineID1);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.textCuttingWidth);
            this.detailcont.Controls.Add(this.txtDescription);
            this.detailcont.Controls.Add(this.txtCellNo);
            this.detailcont.Controls.Add(this.labelCellNo);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.displayM);
            this.detailcont.Controls.Add(this.labelCuttingWidth);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labelM);
            this.detailcont.Size = new System.Drawing.Size(826, 392);
            this.detailcont.TabIndex = 0;
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 392);
            this.detailbtm.Size = new System.Drawing.Size(826, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(826, 430);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(834, 459);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.TabIndex = 1;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // labelM
            // 
            this.labelM.Location = new System.Drawing.Point(70, 57);
            this.labelM.Name = "labelM";
            this.labelM.Size = new System.Drawing.Size(92, 23);
            this.labelM.TabIndex = 0;
            this.labelM.Text = "M";
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(70, 167);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(92, 23);
            this.labelDescription.TabIndex = 1;
            this.labelDescription.Text = "Description";
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(338, 59);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 3;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // labelCellNo
            // 
            this.labelCellNo.Location = new System.Drawing.Point(70, 112);
            this.labelCellNo.Name = "labelCellNo";
            this.labelCellNo.Size = new System.Drawing.Size(92, 23);
            this.labelCellNo.TabIndex = 8;
            this.labelCellNo.Text = "Cell No";
            // 
            // displayM
            // 
            this.displayM.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayM.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "mDivisionid", true));
            this.displayM.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayM.Location = new System.Drawing.Point(174, 57);
            this.displayM.Name = "displayM";
            this.displayM.Size = new System.Drawing.Size(64, 23);
            this.displayM.TabIndex = 0;
            // 
            // txtCellNo
            // 
            this.txtCellNo.BackColor = System.Drawing.Color.White;
            this.txtCellNo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "id", true));
            this.txtCellNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCellNo.Location = new System.Drawing.Point(174, 112);
            this.txtCellNo.Name = "txtCellNo";
            this.txtCellNo.Size = new System.Drawing.Size(43, 23);
            this.txtCellNo.TabIndex = 1;
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.White;
            this.txtDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "description", true));
            this.txtDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDescription.Location = new System.Drawing.Point(174, 167);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(172, 23);
            this.txtDescription.TabIndex = 2;
            // 
            // labelCuttingWidth
            // 
            this.labelCuttingWidth.Location = new System.Drawing.Point(70, 217);
            this.labelCuttingWidth.Name = "labelCuttingWidth";
            this.labelCuttingWidth.Size = new System.Drawing.Size(92, 23);
            this.labelCuttingWidth.TabIndex = 1;
            this.labelCuttingWidth.Text = "Cutting Width";
            // 
            // textCuttingWidth
            // 
            this.textCuttingWidth.BackColor = System.Drawing.Color.White;
            this.textCuttingWidth.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CuttingWidth", true));
            this.textCuttingWidth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textCuttingWidth.Location = new System.Drawing.Point(174, 217);
            this.textCuttingWidth.Name = "textCuttingWidth";
            this.textCuttingWidth.Size = new System.Drawing.Size(64, 23);
            this.textCuttingWidth.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(244, 220);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 17);
            this.label1.TabIndex = 9;
            this.label1.Text = "(cm)";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(70, 267);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(115, 23);
            this.label2.TabIndex = 10;
            this.label2.Text = "CuttingMachineID";
            // 
            // txtCuttingMachineID1
            // 
            this.txtCuttingMachineID1.BackColor = System.Drawing.Color.White;
            this.txtCuttingMachineID1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CuttingMachineID", true));
            this.txtCuttingMachineID1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtCuttingMachineID1.Location = new System.Drawing.Point(196, 267);
            this.txtCuttingMachineID1.Name = "txtCuttingMachineID1";
            this.txtCuttingMachineID1.Size = new System.Drawing.Size(150, 23);
            this.txtCuttingMachineID1.TabIndex = 11;
            // 
            // B02
            // 
            this.ClientSize = new System.Drawing.Size(834, 492);
            this.DefaultControl = "txtCellNo";
            this.DefaultControlForEdit = "txtDescription";
            this.DefaultOrder = "id";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B02";
            this.Text = "B02.Cutting Cell";
            this.WorkAlias = "Cutcell";
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

        private Win.UI.Label labelCellNo;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelM;
        private Win.UI.TextBox txtDescription;
        private Win.UI.TextBox txtCellNo;
        private Win.UI.DisplayBox displayM;
        private System.Windows.Forms.Label label1;
        private Win.UI.TextBox textCuttingWidth;
        private Win.UI.Label labelCuttingWidth;
        private Win.UI.Label label2;
        private Class.TxtCuttingMachineID txtCuttingMachineID1;
    }
}
