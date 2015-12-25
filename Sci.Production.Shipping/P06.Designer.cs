namespace Sci.Production.Shipping
{
    partial class P06
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
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.displayBox2 = new Sci.Win.UI.DisplayBox();
            this.label3 = new Sci.Win.UI.Label();
            this.dateBox1 = new Sci.Win.UI.DateBox();
            this.dateBox2 = new Sci.Win.UI.DateBox();
            this.button1 = new Sci.Win.UI.Button();
            this.label6 = new Sci.Win.UI.Label();
            this.button2 = new Sci.Win.UI.Button();
            this.button3 = new Sci.Win.UI.Button();
            this.label5 = new Sci.Win.UI.Label();
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
            this.masterpanel.Controls.Add(this.button2);
            this.masterpanel.Controls.Add(this.label6);
            this.masterpanel.Controls.Add(this.button1);
            this.masterpanel.Controls.Add(this.label5);
            this.masterpanel.Controls.Add(this.label3);
            this.masterpanel.Controls.Add(this.displayBox2);
            this.masterpanel.Controls.Add(this.displayBox1);
            this.masterpanel.Controls.Add(this.label2);
            this.masterpanel.Controls.Add(this.label1);
            this.masterpanel.Controls.Add(this.dateBox2);
            this.masterpanel.Controls.Add(this.dateBox1);
            this.masterpanel.Size = new System.Drawing.Size(892, 66);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateBox1, 0);
            this.masterpanel.Controls.SetChildIndex(this.dateBox2, 0);
            this.masterpanel.Controls.SetChildIndex(this.label1, 0);
            this.masterpanel.Controls.SetChildIndex(this.label2, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayBox1, 0);
            this.masterpanel.Controls.SetChildIndex(this.displayBox2, 0);
            this.masterpanel.Controls.SetChildIndex(this.label3, 0);
            this.masterpanel.Controls.SetChildIndex(this.label5, 0);
            this.masterpanel.Controls.SetChildIndex(this.button1, 0);
            this.masterpanel.Controls.SetChildIndex(this.label6, 0);
            this.masterpanel.Controls.SetChildIndex(this.button2, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 66);
            this.detailpanel.Size = new System.Drawing.Size(892, 283);
            // 
            // gridicon
            // 
            this.gridicon.Enabled = false;
            this.gridicon.Location = new System.Drawing.Point(791, 31);
            this.gridicon.Visible = false;
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(892, 283);
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
            // detailbtm
            // 
            this.detailbtm.Controls.Add(this.button3);
            this.detailbtm.Controls.SetChildIndex(this.lbleditby, 0);
            this.detailbtm.Controls.SetChildIndex(this.lblcreateby, 0);
            this.detailbtm.Controls.SetChildIndex(this.editby, 0);
            this.detailbtm.Controls.SetChildIndex(this.createby, 0);
            this.detailbtm.Controls.SetChildIndex(this.refresh, 0);
            this.detailbtm.Controls.SetChildIndex(this.button3, 0);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(903, 484);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(911, 513);
            // 
            // label1
            // 
            this.label1.Lines = 0;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "No.";
            // 
            // label2
            // 
            this.label2.Lines = 0;
            this.label2.Location = new System.Drawing.Point(5, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "M";
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(37, 5);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(130, 23);
            this.displayBox1.TabIndex = 3;
            // 
            // displayBox2
            // 
            this.displayBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "MDivisionID", true));
            this.displayBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox2.Location = new System.Drawing.Point(37, 32);
            this.displayBox2.Name = "displayBox2";
            this.displayBox2.Size = new System.Drawing.Size(68, 23);
            this.displayBox2.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(196, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 23);
            this.label3.TabIndex = 5;
            this.label3.Text = "Pull-out date";
            // 
            // dateBox1
            // 
            this.dateBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PulloutDate", true));
            this.dateBox1.IsSupportEditMode = false;
            this.dateBox1.Location = new System.Drawing.Point(283, 5);
            this.dateBox1.Name = "dateBox1";
            this.dateBox1.ReadOnly = true;
            this.dateBox1.Size = new System.Drawing.Size(100, 23);
            this.dateBox1.TabIndex = 7;
            // 
            // dateBox2
            // 
            this.dateBox2.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "SendToTPE", true));
            this.dateBox2.IsSupportEditMode = false;
            this.dateBox2.Location = new System.Drawing.Point(506, 5);
            this.dateBox2.Name = "dateBox2";
            this.dateBox2.ReadOnly = true;
            this.dateBox2.Size = new System.Drawing.Size(100, 23);
            this.dateBox2.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.EditMode = Sci.Win.UI.AdvEditModes.EnableOnEdit;
            this.button1.Location = new System.Drawing.Point(630, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(181, 50);
            this.button1.TabIndex = 11;
            this.button1.Text = "Revise from ship plan and FOC/LO packing list";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold);
            this.label6.Lines = 0;
            this.label6.Location = new System.Drawing.Point(827, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 28);
            this.label6.TabIndex = 12;
            this.label6.Text = "Lock";
            this.label6.TextStyle.BorderColor = System.Drawing.Color.Red;
            this.label6.TextStyle.Color = System.Drawing.Color.Red;
            this.label6.TextStyle.ExtBorderColor = System.Drawing.Color.Red;
            this.label6.TextStyle.GradientColor = System.Drawing.Color.Red;
            // 
            // button2
            // 
            this.button2.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.button2.Location = new System.Drawing.Point(818, 32);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 30);
            this.button2.TabIndex = 13;
            this.button2.Text = "History";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.EditMode = Sci.Win.UI.AdvEditModes.DisableOnEdit;
            this.button3.Location = new System.Drawing.Point(681, 4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(130, 30);
            this.button3.TabIndex = 3;
            this.button3.Text = "Revised History";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label5
            // 
            this.label5.Lines = 0;
            this.label5.Location = new System.Drawing.Point(422, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 23);
            this.label5.TabIndex = 9;
            this.label5.Text = "Send to SCI";
            // 
            // P06
            // 
            this.ApvChkValue = "New";
            this.ClientSize = new System.Drawing.Size(911, 546);
            this.DefaultDetailOrder = "OrderID";
            this.DefaultOrder = "ID";
            this.GridAlias = "Pullout_Detail";
            this.GridNew = 0;
            this.IsSupportConfirm = true;
            this.IsSupportCopy = false;
            this.IsSupportUnconfirm = true;
            this.KeyField1 = "ID";
            this.Name = "P06";
            this.SubDetailKeyField1 = "UKey";
            this.SubDetailKeyField2 = "Pullout_DetailUKey";
            this.SubGridAlias = "Pullout_Detail_Detail";
            this.SubKeyField1 = "UKey";
            this.Text = "P06. Pullout Report";
            this.UnApvChkValue = "Confirmed";
            this.UniqueExpress = "ID";
            this.WorkAlias = "Pullout";
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

        private Win.UI.Button button2;
        private Win.UI.Label label6;
        private Win.UI.Button button1;
        private Win.UI.DateBox dateBox2;
        private Win.UI.Label label5;
        private Win.UI.DateBox dateBox1;
        private Win.UI.Label label3;
        private Win.UI.DisplayBox displayBox2;
        private Win.UI.DisplayBox displayBox1;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.Button button3;
    }
}
