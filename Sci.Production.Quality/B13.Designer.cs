namespace Sci.Production.Quality
{
    partial class B13
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
            this.labelBrand = new Sci.Win.UI.Label();
            this.labelCustCD = new Sci.Win.UI.Label();
            this.labelCountry = new Sci.Win.UI.Label();
            this.labelCity = new Sci.Win.UI.Label();
            this.displayBrand = new Sci.Win.UI.DisplayBox();
            this.displayCustCD = new Sci.Win.UI.DisplayBox();
            this.displayCity = new Sci.Win.UI.DisplayBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.checkNeed3rdInspect = new Sci.Win.UI.CheckBox();
            this.lbEditName = new Sci.Win.UI.Label();
            this.lbEditDate = new Sci.Win.UI.Label();
            this.displayEditName = new Sci.Win.UI.DisplayBox();
            this.txtCountry = new Sci.Production.Class.Txtcountry();
            this.displayBoxQAEditDate = new Sci.Win.UI.DisplayBox();
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
            this.detail.Size = new System.Drawing.Size(713, 258);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.displayBoxQAEditDate);
            this.detailcont.Controls.Add(this.displayEditName);
            this.detailcont.Controls.Add(this.lbEditDate);
            this.detailcont.Controls.Add(this.lbEditName);
            this.detailcont.Controls.Add(this.checkNeed3rdInspect);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.displayCity);
            this.detailcont.Controls.Add(this.displayCustCD);
            this.detailcont.Controls.Add(this.displayBrand);
            this.detailcont.Controls.Add(this.labelCity);
            this.detailcont.Controls.Add(this.labelCountry);
            this.detailcont.Controls.Add(this.labelCustCD);
            this.detailcont.Controls.Add(this.labelBrand);
            this.detailcont.Controls.Add(this.txtCountry);
            this.detailcont.Size = new System.Drawing.Size(713, 220);
            this.detailcont.TabIndex = 0;
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 220);
            this.detailbtm.Size = new System.Drawing.Size(713, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(713, 258);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(721, 287);
            // 
            // createby
            // 
            this.createby.Size = new System.Drawing.Size(350, 23);
            this.createby.Visible = false;
            // 
            // editby
            // 
            this.editby.Location = new System.Drawing.Point(471, 7);
            this.editby.Size = new System.Drawing.Size(350, 23);
            this.editby.TabIndex = 1;
            this.editby.Visible = false;
            // 
            // lblcreateby
            // 
            this.lblcreateby.Visible = false;
            // 
            // lbleditby
            // 
            this.lbleditby.Location = new System.Drawing.Point(423, 13);
            this.lbleditby.Visible = false;
            // 
            // labelBrand
            // 
            this.labelBrand.Location = new System.Drawing.Point(27, 15);
            this.labelBrand.Name = "labelBrand";
            this.labelBrand.Size = new System.Drawing.Size(148, 23);
            this.labelBrand.TabIndex = 0;
            this.labelBrand.Text = "Brand";
            // 
            // labelCustCD
            // 
            this.labelCustCD.Location = new System.Drawing.Point(27, 45);
            this.labelCustCD.Name = "labelCustCD";
            this.labelCustCD.Size = new System.Drawing.Size(148, 23);
            this.labelCustCD.TabIndex = 1;
            this.labelCustCD.Text = "CustCD";
            // 
            // labelCountry
            // 
            this.labelCountry.Location = new System.Drawing.Point(27, 75);
            this.labelCountry.Name = "labelCountry";
            this.labelCountry.Size = new System.Drawing.Size(148, 23);
            this.labelCountry.TabIndex = 2;
            this.labelCountry.Text = "Country";
            // 
            // labelCity
            // 
            this.labelCity.Location = new System.Drawing.Point(27, 105);
            this.labelCity.Name = "labelCity";
            this.labelCity.Size = new System.Drawing.Size(148, 23);
            this.labelCity.TabIndex = 3;
            this.labelCity.Text = "City";
            // 
            // displayBrand
            // 
            this.displayBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBrand.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "BrandID", true));
            this.displayBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBrand.Location = new System.Drawing.Point(186, 15);
            this.displayBrand.Name = "displayBrand";
            this.displayBrand.Size = new System.Drawing.Size(85, 23);
            this.displayBrand.TabIndex = 0;
            // 
            // displayCustCD
            // 
            this.displayCustCD.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCustCD.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ID", true));
            this.displayCustCD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCustCD.Location = new System.Drawing.Point(186, 45);
            this.displayCustCD.Name = "displayCustCD";
            this.displayCustCD.Size = new System.Drawing.Size(114, 23);
            this.displayCustCD.TabIndex = 1;
            // 
            // displayCity
            // 
            this.displayCity.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCity.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "City", true));
            this.displayCity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCity.Location = new System.Drawing.Point(186, 105);
            this.displayCity.Name = "displayCity";
            this.displayCity.Size = new System.Drawing.Size(144, 23);
            this.displayCity.TabIndex = 3;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.checkJunk.IsSupportEditMode = false;
            this.checkJunk.Location = new System.Drawing.Point(471, 15);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.ReadOnly = true;
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 15;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // checkNeed3rdInspect
            // 
            this.checkNeed3rdInspect.AutoSize = true;
            this.checkNeed3rdInspect.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Need3rdInspect", true));
            this.checkNeed3rdInspect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkNeed3rdInspect.Location = new System.Drawing.Point(471, 42);
            this.checkNeed3rdInspect.Name = "checkNeed3rdInspect";
            this.checkNeed3rdInspect.Size = new System.Drawing.Size(190, 21);
            this.checkNeed3rdInspect.TabIndex = 16;
            this.checkNeed3rdInspect.Text = "Need 3rd party inspection";
            this.checkNeed3rdInspect.UseVisualStyleBackColor = true;
            // 
            // lbEditName
            // 
            this.lbEditName.Location = new System.Drawing.Point(27, 135);
            this.lbEditName.Name = "lbEditName";
            this.lbEditName.Size = new System.Drawing.Size(148, 23);
            this.lbEditName.TabIndex = 17;
            this.lbEditName.Text = "EditName";
            // 
            // lbEditDate
            // 
            this.lbEditDate.Location = new System.Drawing.Point(27, 165);
            this.lbEditDate.Name = "lbEditDate";
            this.lbEditDate.Size = new System.Drawing.Size(148, 23);
            this.lbEditDate.TabIndex = 18;
            this.lbEditDate.Text = "EditDate";
            // 
            // displayEditName
            // 
            this.displayEditName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayEditName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayEditName.Location = new System.Drawing.Point(186, 135);
            this.displayEditName.Name = "displayEditName";
            this.displayEditName.Size = new System.Drawing.Size(114, 23);
            this.displayEditName.TabIndex = 19;
            // 
            // txtCountry
            // 
            this.txtCountry.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "CountryID", true));
            this.txtCountry.DisplayBox1Binding = "";
            this.txtCountry.Location = new System.Drawing.Point(186, 75);
            this.txtCountry.Name = "txtCountry";
            this.txtCountry.Size = new System.Drawing.Size(232, 22);
            this.txtCountry.TabIndex = 2;
            this.txtCountry.TextBox1Binding = "";
            // 
            // displayBoxQAEditDate
            // 
            this.displayBoxQAEditDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayBoxQAEditDate.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "QAEditDate", true));
            this.displayBoxQAEditDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayBoxQAEditDate.FormatString = "yyyy/MM/dd HH:mm:ss";
            this.displayBoxQAEditDate.Location = new System.Drawing.Point(186, 164);
            this.displayBoxQAEditDate.Name = "displayBoxQAEditDate";
            this.displayBoxQAEditDate.Size = new System.Drawing.Size(144, 23);
            this.displayBoxQAEditDate.TabIndex = 21;
            // 
            // B13
            // 
            this.ClientSize = new System.Drawing.Size(721, 320);
            this.DefaultControlForEdit = "txtDestination";
            this.DefaultOrder = "BrandID,ID";
            this.EnableGridJunkColor = true;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B13";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B13. 3rd party inspection";
            this.UniqueExpress = "BrandID,ID";
            this.WorkAlias = "CustCD";
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
        private Win.UI.DisplayBox displayCity;
        private Class.Txtcountry txtCountry;
        private Win.UI.DisplayBox displayCustCD;
        private Win.UI.DisplayBox displayBrand;
        private Win.UI.Label labelCity;
        private Win.UI.Label labelCountry;
        private Win.UI.Label labelCustCD;
        private Win.UI.Label labelBrand;
        private Win.UI.CheckBox checkNeed3rdInspect;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.DisplayBox displayEditName;
        private Win.UI.Label lbEditDate;
        private Win.UI.Label lbEditName;
        private Win.UI.DisplayBox displayBoxQAEditDate;
    }
}
