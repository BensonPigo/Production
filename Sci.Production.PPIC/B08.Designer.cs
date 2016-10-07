namespace Sci.Production.PPIC
{
    partial class B08
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
            this.editBox1 = new Sci.Win.UI.EditBox();
            this.textBox1 = new Sci.Win.UI.TextBox();
            this.lbID = new Sci.Win.UI.Label();
            this.txtID = new Sci.Win.UI.TextBox();
            this.lbFactory = new Sci.Win.UI.Label();
            this.txtFactory = new Sci.Win.UI.TextBox();
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
            this.masterpanel.Controls.Add(this.txtFactory);
            this.masterpanel.Controls.Add(this.lbFactory);
            this.masterpanel.Controls.Add(this.txtID);
            this.masterpanel.Controls.Add(this.lbID);
            this.masterpanel.Controls.Add(this.textBox1);
            this.masterpanel.Controls.Add(this.editBox1);
            this.masterpanel.Controls.Add(this.label4);
            this.masterpanel.Controls.Add(this.label3);
            this.masterpanel.Size = new System.Drawing.Size(676, 103);
            this.masterpanel.Controls.SetChildIndex(this.gridicon, 0);
            this.masterpanel.Controls.SetChildIndex(this.label3, 0);
            this.masterpanel.Controls.SetChildIndex(this.label4, 0);
            this.masterpanel.Controls.SetChildIndex(this.editBox1, 0);
            this.masterpanel.Controls.SetChildIndex(this.textBox1, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbID, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtID, 0);
            this.masterpanel.Controls.SetChildIndex(this.lbFactory, 0);
            this.masterpanel.Controls.SetChildIndex(this.txtFactory, 0);
            // 
            // detailpanel
            // 
            this.detailpanel.Location = new System.Drawing.Point(0, 103);
            this.detailpanel.Size = new System.Drawing.Size(676, 279);
            // 
            // gridicon
            // 
            this.gridicon.Location = new System.Drawing.Point(654, 68);
            // 
            // refresh
            // 
            this.refresh.Location = new System.Drawing.Point(586, 0);
            // 
            // detailgridcont
            // 
            this.detailgridcont.Size = new System.Drawing.Size(676, 279);
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
            this.detail.Size = new System.Drawing.Size(676, 420);
            // 
            // detailcont
            // 
            this.detailcont.Size = new System.Drawing.Size(676, 382);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 382);
            this.detailbtm.Size = new System.Drawing.Size(676, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(676, 420);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(684, 449);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(230, 23);
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(354, 7);
            this.editby.Size = new System.Drawing.Size(230, 23);
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(306, 13);
            // 
            // label3
            // 
            this.label3.Lines = 0;
            this.label3.Location = new System.Drawing.Point(11, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 1;
            this.label3.Text = "Name";
            // 
            // label4
            // 
            this.label4.Lines = 0;
            this.label4.Location = new System.Drawing.Point(11, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 2;
            this.label4.Text = "Description";
            // 
            // editBox1
            // 
            this.editBox1.BackColor = System.Drawing.Color.White;
            this.editBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBox1.Location = new System.Drawing.Point(90, 59);
            this.editBox1.Multiline = true;
            this.editBox1.Name = "editBox1";
            this.editBox1.Size = new System.Drawing.Size(500, 40);
            this.editBox1.TabIndex = 4;
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Name", true));
            this.textBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.textBox1.Location = new System.Drawing.Point(90, 32);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(500, 23);
            this.textBox1.TabIndex = 5;
            // 
            // lbID
            // 
            this.lbID.Lines = 0;
            this.lbID.Location = new System.Drawing.Point(11, 6);
            this.lbID.Name = "lbID";
            this.lbID.Size = new System.Drawing.Size(75, 23);
            this.lbID.TabIndex = 6;
            this.lbID.Text = "ID";
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.White;
            this.txtID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtID.Location = new System.Drawing.Point(90, 6);
            this.txtID.Name = "txtID";
            this.txtID.Size = new System.Drawing.Size(150, 23);
            this.txtID.TabIndex = 7;
            // 
            // lbFactory
            // 
            this.lbFactory.Lines = 0;
            this.lbFactory.Location = new System.Drawing.Point(362, 6);
            this.lbFactory.Name = "lbFactory";
            this.lbFactory.Size = new System.Drawing.Size(75, 23);
            this.lbFactory.TabIndex = 8;
            this.lbFactory.Text = "Factory";
            // 
            // txtFactory
            // 
            this.txtFactory.BackColor = System.Drawing.Color.White;
            this.txtFactory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "FactoryID", true));
            this.txtFactory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFactory.Location = new System.Drawing.Point(440, 6);
            this.txtFactory.Name = "txtFactory";
            this.txtFactory.Size = new System.Drawing.Size(150, 23);
            this.txtFactory.TabIndex = 9;
            this.txtFactory.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.txtFactory_PopUp);
            this.txtFactory.Validating += new System.ComponentModel.CancelEventHandler(this.txtFactory_Validating);
            // 
            // B08
            // 
            this.ClientSize = new System.Drawing.Size(684, 482);
            this.DefaultDetailOrder = "Day";
            this.DefaultOrder = "ID";
            this.GridAlias = "LearnCurve_Detail";
            this.GridNew = 0;
            this.IsGridIconVisible = false;
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.KeyField1 = "ID";
            this.Name = "B08";
            this.Text = "B08. Learning Curve";
            this.UniqueExpress = "ID";
            this.WorkAlias = "LearnCurve";
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

        private Win.UI.EditBox editBox1;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.TextBox textBox1;
        private Win.UI.Label lbFactory;
        private Win.UI.TextBox txtID;
        private Win.UI.Label lbID;
        private Win.UI.TextBox txtFactory;
    }
}
