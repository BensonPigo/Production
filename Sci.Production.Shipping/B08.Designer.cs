namespace Sci.Production.Shipping
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
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
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
            this.txtPort = new Sci.Win.UI.TextBox();
            this.txtInternationalCode = new Sci.Win.UI.TextBox();
            this.txtName = new Sci.Win.UI.TextBox();
            this.labName = new Sci.Win.UI.Label();
            this.chkAirPort = new Sci.Win.UI.CheckBox();
            this.chkSeaPort = new Sci.Win.UI.CheckBox();
            this.chkJunk = new Sci.Win.UI.CheckBox();
            this.labInternationalcode = new Sci.Win.UI.Label();
            this.editBoxRemark = new Sci.Win.UI.EditBox();
            this.label5 = new Sci.Win.UI.Label();
            this.txtcountry = new Sci.Production.Class.Txtcountry();
            this.labCountry = new Sci.Win.UI.Label();
            this.labPort = new Sci.Win.UI.Label();
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
            this.detail.Size = new System.Drawing.Size(684, 395);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.txtPort);
            this.detailcont.Controls.Add(this.txtInternationalCode);
            this.detailcont.Controls.Add(this.txtName);
            this.detailcont.Controls.Add(this.labName);
            this.detailcont.Controls.Add(this.chkAirPort);
            this.detailcont.Controls.Add(this.chkSeaPort);
            this.detailcont.Controls.Add(this.chkJunk);
            this.detailcont.Controls.Add(this.labInternationalcode);
            this.detailcont.Controls.Add(this.editBoxRemark);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.txtcountry);
            this.detailcont.Controls.Add(this.labCountry);
            this.detailcont.Controls.Add(this.labPort);
            this.detailcont.Size = new System.Drawing.Size(684, 357);
            // 
            // detailbtm
            // 
            this.detailbtm.Size = new System.Drawing.Size(684, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(684, 395);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(692, 424);
            // 
            // txtPort
            // 
            this.txtPort.BackColor = System.Drawing.Color.White;
            this.txtPort.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ID", true));
            this.txtPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtPort.Location = new System.Drawing.Point(148, 26);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(190, 23);
            this.txtPort.TabIndex = 53;
            // 
            // txtInternationalCode
            // 
            this.txtInternationalCode.BackColor = System.Drawing.Color.White;
            this.txtInternationalCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "InternationalCode", true));
            this.txtInternationalCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtInternationalCode.Location = new System.Drawing.Point(195, 107);
            this.txtInternationalCode.Name = "txtInternationalCode";
            this.txtInternationalCode.Size = new System.Drawing.Size(181, 23);
            this.txtInternationalCode.TabIndex = 52;
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.White;
            this.txtName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Name", true));
            this.txtName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtName.Location = new System.Drawing.Point(148, 53);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(190, 23);
            this.txtName.TabIndex = 50;
            // 
            // labName
            // 
            this.labName.Location = new System.Drawing.Point(70, 53);
            this.labName.Name = "labName";
            this.labName.Size = new System.Drawing.Size(75, 23);
            this.labName.TabIndex = 51;
            this.labName.Text = "Name";
            // 
            // chkAirPort
            // 
            this.chkAirPort.AutoSize = true;
            this.chkAirPort.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "AirPort", true));
            this.chkAirPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkAirPort.Location = new System.Drawing.Point(391, 55);
            this.chkAirPort.Name = "chkAirPort";
            this.chkAirPort.Size = new System.Drawing.Size(74, 21);
            this.chkAirPort.TabIndex = 49;
            this.chkAirPort.Text = "Air Port";
            this.chkAirPort.UseVisualStyleBackColor = true;
            // 
            // chkSeaPort
            // 
            this.chkSeaPort.AutoSize = true;
            this.chkSeaPort.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "SeaPort", true));
            this.chkSeaPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkSeaPort.Location = new System.Drawing.Point(391, 28);
            this.chkSeaPort.Name = "chkSeaPort";
            this.chkSeaPort.Size = new System.Drawing.Size(82, 21);
            this.chkSeaPort.TabIndex = 48;
            this.chkSeaPort.Text = "Sea Port";
            this.chkSeaPort.UseVisualStyleBackColor = true;
            // 
            // chkJunk
            // 
            this.chkJunk.AutoSize = true;
            this.chkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.chkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkJunk.Location = new System.Drawing.Point(479, 28);
            this.chkJunk.Name = "chkJunk";
            this.chkJunk.Size = new System.Drawing.Size(57, 21);
            this.chkJunk.TabIndex = 47;
            this.chkJunk.Text = "Junk";
            this.chkJunk.UseVisualStyleBackColor = true;
            // 
            // labInternationalcode
            // 
            this.labInternationalcode.Location = new System.Drawing.Point(70, 107);
            this.labInternationalcode.Name = "labInternationalcode";
            this.labInternationalcode.Size = new System.Drawing.Size(120, 23);
            this.labInternationalcode.TabIndex = 46;
            this.labInternationalcode.Text = "International Code";
            // 
            // editBoxRemark
            // 
            this.editBoxRemark.BackColor = System.Drawing.Color.White;
            this.editBoxRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editBoxRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBoxRemark.Location = new System.Drawing.Point(148, 134);
            this.editBoxRemark.Multiline = true;
            this.editBoxRemark.Name = "editBoxRemark";
            this.editBoxRemark.Size = new System.Drawing.Size(388, 182);
            this.editBoxRemark.TabIndex = 45;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(70, 134);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 44;
            this.label5.Text = "Remark ";
            // 
            // txtcountry
            // 
            this.txtcountry.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "CountryID", true));
            this.txtcountry.DisplayBox1Binding = "";
            this.txtcountry.Location = new System.Drawing.Point(148, 80);
            this.txtcountry.Name = "txtcountry";
            this.txtcountry.Size = new System.Drawing.Size(240, 22);
            this.txtcountry.TabIndex = 43;
            this.txtcountry.TextBox1Binding = "";
            // 
            // labCountry
            // 
            this.labCountry.Location = new System.Drawing.Point(70, 80);
            this.labCountry.Name = "labCountry";
            this.labCountry.Size = new System.Drawing.Size(75, 23);
            this.labCountry.TabIndex = 42;
            this.labCountry.Text = "Country";
            // 
            // labPort
            // 
            this.labPort.Location = new System.Drawing.Point(70, 26);
            this.labPort.Name = "labPort";
            this.labPort.Size = new System.Drawing.Size(75, 23);
            this.labPort.TabIndex = 41;
            this.labPort.Text = "Port";
            // 
            // B08
            // 
            this.ClientSize = new System.Drawing.Size(692, 457);
            this.IsDeleteOnBrowse = false;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.Name = "B08";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B08. Port";
            this.WorkAlias = "PulloutPort";
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

        private Win.UI.TextBox txtPort;
        private Win.UI.TextBox txtInternationalCode;
        private Win.UI.TextBox txtName;
        private Win.UI.Label labName;
        private Win.UI.CheckBox chkAirPort;
        private Win.UI.CheckBox chkSeaPort;
        private Win.UI.CheckBox chkJunk;
        private Win.UI.Label labInternationalcode;
        private Win.UI.EditBox editBoxRemark;
        private Win.UI.Label label5;
        private Class.Txtcountry txtcountry;
        private Win.UI.Label labCountry;
        private Win.UI.Label labPort;
    }
}
