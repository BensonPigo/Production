﻿namespace Sci.Production.Subcon
{
    partial class B40_RFIDReaderSetting
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Size = new System.Drawing.Size(935, 40);
            // 
            // gridcont
            // 
            this.gridcont.Size = new System.Drawing.Size(911, 435);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(845, 5);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(765, 5);
            // 
            // B40_RFIDReaderSetting
            // 
            this.ClientSize = new System.Drawing.Size(935, 497);
            this.GridPopUp = false;
            this.KeyField1 = "RFIDReaderID";
            this.Name = "B40_RFIDReaderSetting";
            this.OnLineHelpID = "Sci.Win.Subs.Input4";
            this.Text = "B40. RFID Reader Set Panel, Cut Cell";
            this.WorkAlias = "RFIDReader_Panel";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
