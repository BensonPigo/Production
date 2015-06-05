namespace Sci.Production.Packing
{
    partial class P10
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
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.dateBox1 = new Sci.Win.UI.DateBox();
            this.button1 = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).BeginInit();
            this.masterpanel.SuspendLayout();
            this.detailpanel.SuspendLayout();
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
            this.masterpanel.Controls.Add(this.button1);
            this.masterpanel.Controls.Add(this.dateBox1);
            this.masterpanel.Controls.Add(this.displayBox1);
            this.masterpanel.Controls.Add(this.label4);
            this.masterpanel.Controls.Add(this.label3);
            this.masterpanel.Size = new System.Drawing.Size(892, 102);
            this.masterpanel.Controls.SetChildIndex(this.label3, 0);
            this.masterpanel.Controls.SetChildIndex(this.label4, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayBox1, 0);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateBox1, 0);
            this.masterpanel.Controls.SetChildIndex(this.button1, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 102);
            this.detailpanel.Size = new System.Drawing.Size(892, 247);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(666, 67);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(892, 247);
            // 
            // detailgridcont2
            // 
            this.detailgridcont2.Size = new System.Drawing.Size(886, 341);
            // 
            // detailpanel2
            // 
            this.detailpanel2.Size = new System.Drawing.Size(886, 40);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(779, 387);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(787, 416);
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(10, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 23);
            this.label3.TabIndex = 1;
            this.label3.Text = "No";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(10, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 23);
            this.label4.TabIndex = 2;
            this.label4.Text = "Transfer Date";
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(109, 9);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(130, 23);
            this.displayBox1.TabIndex = 3;
            // 
            // dateBox1
            // 
            this.dateBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "TransferDate", true));
            this.dateBox1.IsSupportEditMode = false;
            this.dateBox1.Location = new System.Drawing.Point(109, 36);
            this.dateBox1.Name = "dateBox1";
            this.dateBox1.ReadOnly = true;
            this.dateBox1.Size = new System.Drawing.Size(130, 23);
            this.dateBox1.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.button1.Location = new System.Drawing.Point(612, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(157, 50);
            this.button1.TabIndex = 5;
            this.button1.Text = "Select Trans PO or Import From Barcode";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // P10
            // 
            this.ClientSize = new System.Drawing.Size(787, 449);
            this.DefaultDetailOrder = "PackingListID";
            this.DefaultOrder = "ID";
            this.GridAlias = "TransferToClog_Detail";
            this.GridEdit = false;
            this.GridUniqueKey = "ID,PackingListID,CTNStartNo";
            this.IsSupportCopy = false;
            this.KeyField1 = "ID";
            this.Name = "P10";
            this.Text = "P10. Carton transfer to CLOG";
            this.UniqueExpress = "ID";
            this.WorkAlias = "TransferToClog";
            this.Controls.SetChildIndex(this.tabs, 0);
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.detailgrid2bs)).EndInit();
            this.masterpanel.ResumeLayout(false);
            this.masterpanel.PerformLayout();
            this.detailpanel.ResumeLayout(false);
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

        private Win.UI.Button button1;
        private Win.UI.DateBox dateBox1;
        private Win.UI.DisplayBox displayBox1;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
    }
}
