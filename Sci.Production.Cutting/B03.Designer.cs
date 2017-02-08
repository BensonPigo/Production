namespace Sci.Production.Cutting
{
    partial class B03
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
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.displayBoxID = new Sci.Win.UI.DisplayBox();
            this.displayBoxCuttingLayer = new Sci.Win.UI.DisplayBox();
            this.checkBoxJunk = new Sci.Win.UI.CheckBox();
            this.displayBoxName = new Sci.Win.UI.DisplayBox();
            this.label3 = new Sci.Win.UI.Label();
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
            this.detail.Size = new System.Drawing.Size(676, 380);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.displayBoxName);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.checkBoxJunk);
            this.detailcont.Controls.Add(this.displayBoxCuttingLayer);
            this.detailcont.Controls.Add(this.displayBoxID);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Size = new System.Drawing.Size(676, 342);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 342);
            this.detailbtm.Size = new System.Drawing.Size(676, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(676, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(684, 409);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(70, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(70, 167);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Cutting Layer";
            // 
            // displayBoxID
            // 
            this.displayBoxID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayBoxID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxID.Location = new System.Drawing.Point(162, 57);
            this.displayBoxID.Name = "displayBoxID";
            this.displayBoxID.Size = new System.Drawing.Size(90, 23);
            this.displayBoxID.TabIndex = 2;
            // 
            // displayBoxCuttingLayer
            // 
            this.displayBoxCuttingLayer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxCuttingLayer.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "cuttinglayer", true));
            this.displayBoxCuttingLayer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxCuttingLayer.Location = new System.Drawing.Point(162, 167);
            this.displayBoxCuttingLayer.Name = "displayBoxCuttingLayer";
            this.displayBoxCuttingLayer.Size = new System.Drawing.Size(53, 23);
            this.displayBoxCuttingLayer.TabIndex = 3;
            // 
            // checkBoxJunk
            // 
            this.checkBoxJunk.AutoSize = true;
            this.checkBoxJunk.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "junk", true));
            this.checkBoxJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkBoxJunk.Location = new System.Drawing.Point(315, 59);
            this.checkBoxJunk.Name = "checkBoxJunk";
            this.checkBoxJunk.Size = new System.Drawing.Size(57, 21);
            this.checkBoxJunk.TabIndex = 4;
            this.checkBoxJunk.Text = "Junk";
            this.checkBoxJunk.UseVisualStyleBackColor = true;
            // 
            // displayBoxName
            // 
            this.displayBoxName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxName.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Name", true));
            this.displayBoxName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxName.Location = new System.Drawing.Point(162, 112);
            this.displayBoxName.Name = "displayBoxName";
            this.displayBoxName.Size = new System.Drawing.Size(375, 23);
            this.displayBoxName.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(70, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 23);
            this.label3.TabIndex = 8;
            this.label3.Text = "Name";
            // 
            // B03
            // 
            this.ClientSize = new System.Drawing.Size(684, 442);
            this.DefaultOrder = "id";
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B03";
            this.Text = "B03.Construction";
            this.WorkAlias = "Construction";
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

        private Win.UI.DisplayBox displayBoxName;
        private Win.UI.Label label3;
        private Win.UI.CheckBox checkBoxJunk;
        private Win.UI.DisplayBox displayBoxCuttingLayer;
        private Win.UI.DisplayBox displayBoxID;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
    }
}
