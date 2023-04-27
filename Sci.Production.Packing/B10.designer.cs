namespace Sci.Production.Packing
{
    partial class B10
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
            this.txtDescription = new Sci.Win.UI.TextBox();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.labelDescription = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.txtLocalDescription = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
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
            this.detail.Size = new System.Drawing.Size(831, 304);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtLocalDescription);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.txtDescription);
            this.detailcont.Controls.Add(this.displayBox1);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Size = new System.Drawing.Size(831, 266);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 266);
            this.detailbtm.Size = new System.Drawing.Size(831, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(831, 304);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(839, 333);
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
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.txtDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtDescription.IsSupportEditMode = false;
            this.txtDescription.Location = new System.Drawing.Point(149, 95);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(409, 23);
            this.txtDescription.TabIndex = 12;
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(149, 46);
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(50, 23);
            this.displayBox1.TabIndex = 11;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkJunk.IsSupportEditMode = false;
            this.checkJunk.Location = new System.Drawing.Point(400, 46);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.ReadOnly = true;
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 10;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // labelDescription
            // 
            this.labelDescription.Location = new System.Drawing.Point(70, 95);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(75, 23);
            this.labelDescription.TabIndex = 9;
            this.labelDescription.Text = "Description";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(70, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "ID";
            // 
            // txtLocalDescription
            // 
            this.txtLocalDescription.BackColor = System.Drawing.Color.White;
            this.txtLocalDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "LocalDescription", true));
            this.txtLocalDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocalDescription.Location = new System.Drawing.Point(186, 134);
            this.txtLocalDescription.Name = "txtLocalDescription";
            this.txtLocalDescription.Size = new System.Drawing.Size(372, 23);
            this.txtLocalDescription.TabIndex = 14;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(70, 134);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 23);
            this.label2.TabIndex = 13;
            this.label2.Text = "Local Description";
            // 
            // B10
            // 
            this.ClientSize = new System.Drawing.Size(839, 366);
            this.DefaultFilter = "Type = \'MD\'";
            this.DefaultOrder = "ID";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B10";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B10. Metal Detection Type Reason";
            this.UniqueExpress = "Type,ID";
            this.WorkAlias = "PackingReason";
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

        private Win.UI.TextBox txtDescription;
        private Win.UI.DisplayBox displayBox1;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.Label labelDescription;
        private Win.UI.Label label1;
        private Win.UI.TextBox txtLocalDescription;
        private Win.UI.Label label2;
    }
}
