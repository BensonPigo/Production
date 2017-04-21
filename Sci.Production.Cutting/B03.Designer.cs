﻿namespace Sci.Production.Cutting
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
            this.labelID = new Sci.Win.UI.Label();
            this.labelCuttingLayer = new Sci.Win.UI.Label();
            this.displayID = new Sci.Win.UI.DisplayBox();
            this.displayCuttingLayer = new Sci.Win.UI.DisplayBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.displayName = new Sci.Win.UI.DisplayBox();
            this.labelName = new Sci.Win.UI.Label();
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
            this.detail.Size = new System.Drawing.Size(828, 380);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.displayName);
            this.detailcont.Controls.Add(this.labelName);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.displayCuttingLayer);
            this.detailcont.Controls.Add(this.displayID);
            this.detailcont.Controls.Add(this.labelCuttingLayer);
            this.detailcont.Controls.Add(this.labelID);
            this.detailcont.Size = new System.Drawing.Size(828, 342);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 342);
            this.detailbtm.Size = new System.Drawing.Size(828, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(828, 380);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(836, 409);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            // 
            // labelID
            // 
            this.labelID.Lines = 0;
            this.labelID.Location = new System.Drawing.Point(70, 57);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(89, 23);
            this.labelID.TabIndex = 0;
            this.labelID.Text = "ID";
            // 
            // labelCuttingLayer
            // 
            this.labelCuttingLayer.Lines = 0;
            this.labelCuttingLayer.Location = new System.Drawing.Point(70, 167);
            this.labelCuttingLayer.Name = "labelCuttingLayer";
            this.labelCuttingLayer.Size = new System.Drawing.Size(89, 23);
            this.labelCuttingLayer.TabIndex = 1;
            this.labelCuttingLayer.Text = "Cutting Layer";
            // 
            // displayID
            // 
            this.displayID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayID.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "id", true));
            this.displayID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayID.Location = new System.Drawing.Point(162, 57);
            this.displayID.Name = "displayID";
            this.displayID.Size = new System.Drawing.Size(90, 23);
            this.displayID.TabIndex = 2;
            // 
            // displayCuttingLayer
            // 
            this.displayCuttingLayer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCuttingLayer.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "cuttinglayer", true));
            this.displayCuttingLayer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCuttingLayer.Location = new System.Drawing.Point(162, 167);
            this.displayCuttingLayer.Name = "displayCuttingLayer";
            this.displayCuttingLayer.Size = new System.Drawing.Size(53, 23);
            this.displayCuttingLayer.TabIndex = 3;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(315, 59);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 4;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // displayName
            // 
            this.displayName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayName.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Name", true));
            this.displayName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayName.Location = new System.Drawing.Point(162, 112);
            this.displayName.Name = "displayName";
            this.displayName.Size = new System.Drawing.Size(375, 23);
            this.displayName.TabIndex = 9;
            // 
            // labelName
            // 
            this.labelName.Lines = 0;
            this.labelName.Location = new System.Drawing.Point(70, 112);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(89, 23);
            this.labelName.TabIndex = 8;
            this.labelName.Text = "Name";
            // 
            // B03
            // 
            this.ClientSize = new System.Drawing.Size(836, 442);
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

        private Win.UI.DisplayBox displayName;
        private Win.UI.Label labelName;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.DisplayBox displayCuttingLayer;
        private Win.UI.DisplayBox displayID;
        private Win.UI.Label labelCuttingLayer;
        private Win.UI.Label labelID;
    }
}
