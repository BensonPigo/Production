namespace Sci.Production.Cutting
{
    partial class B07
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
            this.numSetUpT = new Sci.Win.UI.NumericBox();
            this.label1 = new Sci.Win.UI.Label();
            this.numWindowTime = new Sci.Win.UI.NumericBox();
            this.numWindowLength = new Sci.Win.UI.NumericBox();
            this.label2 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.labelID = new Sci.Win.UI.Label();
            this.txtWeaveTypeID = new Sci.Win.UI.TextBox();
            this.displayBox1 = new Sci.Win.UI.DisplayBox();
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
            this.detail.Size = new System.Drawing.Size(828, 411);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.displayBox1);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.numWindowLength);
            this.detailcont.Controls.Add(this.numWindowTime);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Controls.Add(this.numSetUpT);
            this.detailcont.Controls.Add(this.txtWeaveTypeID);
            this.detailcont.Controls.Add(this.labelID);
            this.detailcont.Size = new System.Drawing.Size(828, 373);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 373);
            this.detailbtm.Size = new System.Drawing.Size(828, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(828, 411);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(836, 440);
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
            // numSetUpT
            // 
            this.numSetUpT.BackColor = System.Drawing.Color.White;
            this.numSetUpT.DecimalPlaces = 3;
            this.numSetUpT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numSetUpT.Location = new System.Drawing.Point(206, 59);
            this.numSetUpT.MaxLength = 99999;
            this.numSetUpT.Name = "numSetUpT";
            this.numSetUpT.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numSetUpT.Size = new System.Drawing.Size(100, 23);
            this.numSetUpT.TabIndex = 9;
            this.numSetUpT.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(21, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 23);
            this.label1.TabIndex = 10;
            this.label1.Text = "SetUpTime(secs/marker)";
            // 
            // numWindowTime
            // 
            this.numWindowTime.BackColor = System.Drawing.Color.White;
            this.numWindowTime.DecimalPlaces = 3;
            this.numWindowTime.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numWindowTime.Location = new System.Drawing.Point(206, 100);
            this.numWindowTime.MaxLength = 99999;
            this.numWindowTime.Name = "numWindowTime";
            this.numWindowTime.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numWindowTime.Size = new System.Drawing.Size(100, 23);
            this.numWindowTime.TabIndex = 11;
            this.numWindowTime.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // numWindowLength
            // 
            this.numWindowLength.BackColor = System.Drawing.Color.White;
            this.numWindowLength.DecimalPlaces = 3;
            this.numWindowLength.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numWindowLength.Location = new System.Drawing.Point(206, 139);
            this.numWindowLength.MaxLength = 99999;
            this.numWindowLength.Name = "numWindowLength";
            this.numWindowLength.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numWindowLength.Size = new System.Drawing.Size(100, 23);
            this.numWindowLength.TabIndex = 13;
            this.numWindowLength.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(21, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(182, 23);
            this.label2.TabIndex = 17;
            this.label2.Text = "WindowTime (secs)";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(21, 139);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(182, 23);
            this.label4.TabIndex = 19;
            this.label4.Text = "WindowLength(M)";
            // 
            // labelID
            // 
            this.labelID.Location = new System.Drawing.Point(21, 18);
            this.labelID.Name = "labelID";
            this.labelID.Size = new System.Drawing.Size(163, 23);
            this.labelID.TabIndex = 8;
            this.labelID.Text = "WeaveTypeID";
            // 
            // txtWeaveTypeID
            // 
            this.txtWeaveTypeID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtWeaveTypeID.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtWeaveTypeID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtWeaveTypeID.IsSupportEditMode = false;
            this.txtWeaveTypeID.Location = new System.Drawing.Point(187, 18);
            this.txtWeaveTypeID.Name = "txtWeaveTypeID";
            this.txtWeaveTypeID.ReadOnly = true;
            this.txtWeaveTypeID.Size = new System.Drawing.Size(199, 23);
            this.txtWeaveTypeID.TabIndex = 0;
            // 
            // displayBox1
            // 
            this.displayBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.displayBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBox1.Location = new System.Drawing.Point(21, 291);
            this.displayBox1.Multiline = true;
            this.displayBox1.Name = "displayBox1";
            this.displayBox1.Size = new System.Drawing.Size(616, 76);
            this.displayBox1.TabIndex = 24;
            // 
            // B07
            // 
            this.ClientSize = new System.Drawing.Size(836, 473);
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B07";
            this.Text = "B07 Cutting Time";
            this.WorkAlias = "WeaveType";
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
        private Win.UI.NumericBox numSetUpT;
        private Win.UI.NumericBox numWindowLength;
        private Win.UI.NumericBox numWindowTime;
        private Win.UI.Label label4;
        private Win.UI.Label label2;
        private Win.UI.TextBox txtWeaveTypeID;
        private Win.UI.Label labelID;
        private Win.UI.DisplayBox displayBox1;
    }
}
