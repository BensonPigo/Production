namespace Sci.Production.Shipping
{
    partial class B07
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label5 = new Sci.Win.UI.Label();
            this.displayContinent = new Sci.Win.UI.DisplayBox();
            this.txtContinent = new Sci.Win.UI.TextBox();
            this.editBoxRemark = new Sci.Win.UI.EditBox();
            this.txtPort1 = new Sci.Production.Class.TxtPulloutPort();
            this.txtcountry = new Sci.Production.Class.Txtcountry();
            this.txtBrand = new Sci.Production.Class.Txtbrand();
            this.chkJunk = new Sci.Win.UI.CheckBox();
            this.chkIsSeaPort = new Sci.Win.UI.CheckBox();
            this.chkIsAirPort = new Sci.Win.UI.CheckBox();
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
            this.detail.Size = new System.Drawing.Size(882, 388);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.chkIsAirPort);
            this.detailcont.Controls.Add(this.chkIsSeaPort);
            this.detailcont.Controls.Add(this.chkJunk);
            this.detailcont.Controls.Add(this.txtPort1);
            this.detailcont.Controls.Add(this.editBoxRemark);
            this.detailcont.Controls.Add(this.txtContinent);
            this.detailcont.Controls.Add(this.displayContinent);
            this.detailcont.Controls.Add(this.txtcountry);
            this.detailcont.Controls.Add(this.txtBrand);
            this.detailcont.Controls.Add(this.label5);
            this.detailcont.Controls.Add(this.label4);
            this.detailcont.Controls.Add(this.label3);
            this.detailcont.Controls.Add(this.label2);
            this.detailcont.Controls.Add(this.label1);
            this.detailcont.Size = new System.Drawing.Size(882, 350);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 350);
            this.detailbtm.Size = new System.Drawing.Size(882, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(882, 388);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(890, 417);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(55, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Brand";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(55, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 1;
            this.label2.Text = "Port";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(55, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Country";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(55, 159);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 3;
            this.label4.Text = "Continent";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(55, 208);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 4;
            this.label5.Text = "Remark ";
            // 
            // displayContinent
            // 
            this.displayContinent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayContinent.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ContinentName", true));
            this.displayContinent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayContinent.Location = new System.Drawing.Point(164, 159);
            this.displayContinent.Name = "displayContinent";
            this.displayContinent.Size = new System.Drawing.Size(292, 23);
            this.displayContinent.TabIndex = 9;
            // 
            // txtContinent
            // 
            this.txtContinent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtContinent.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ContinentID", true));
            this.txtContinent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtContinent.IsSupportEditMode = false;
            this.txtContinent.Location = new System.Drawing.Point(133, 159);
            this.txtContinent.Name = "txtContinent";
            this.txtContinent.ReadOnly = true;
            this.txtContinent.Size = new System.Drawing.Size(31, 23);
            this.txtContinent.TabIndex = 10;
            // 
            // editBoxRemark
            // 
            this.editBoxRemark.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editBoxRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editBoxRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editBoxRemark.IsSupportEditMode = false;
            this.editBoxRemark.Location = new System.Drawing.Point(133, 208);
            this.editBoxRemark.Multiline = true;
            this.editBoxRemark.Name = "editBoxRemark";
            this.editBoxRemark.ReadOnly = true;
            this.editBoxRemark.Size = new System.Drawing.Size(361, 93);
            this.editBoxRemark.TabIndex = 11;
            // 
            // txtPort1
            // 
            this.txtPort1.BrandID = null;
            this.txtPort1.ConnectionName = null;
            this.txtPort1.CountryID = null;
            this.txtPort1.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "PulloutPortID", true));
            this.txtPort1.DisplayBox1Binding = "";
            this.txtPort1.Location = new System.Drawing.Point(133, 65);
            this.txtPort1.Name = "txtPort1";
            this.txtPort1.ShipModeID = null;
            this.txtPort1.Size = new System.Drawing.Size(462, 22);
            this.txtPort1.TabIndex = 12;
            this.txtPort1.TextBox1Binding = "";
            // 
            // txtcountry
            // 
            this.txtcountry.DisplayBox1Binding = "";
            this.txtcountry.Location = new System.Drawing.Point(133, 110);
            this.txtcountry.Name = "txtcountry";
            this.txtcountry.Size = new System.Drawing.Size(232, 22);
            this.txtcountry.TabIndex = 7;
            this.txtcountry.TextBox1Binding = "";
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BrandID", true));
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(133, 20);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(66, 23);
            this.txtBrand.TabIndex = 5;
            // 
            // chkJunk
            // 
            this.chkJunk.AutoSize = true;
            this.chkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.chkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkJunk.Location = new System.Drawing.Point(618, 20);
            this.chkJunk.Name = "chkJunk";
            this.chkJunk.Size = new System.Drawing.Size(57, 21);
            this.chkJunk.TabIndex = 13;
            this.chkJunk.Text = "Junk";
            this.chkJunk.UseVisualStyleBackColor = true;
            // 
            // chkIsSeaPort
            // 
            this.chkIsSeaPort.AutoSize = true;
            this.chkIsSeaPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIsSeaPort.Location = new System.Drawing.Point(618, 47);
            this.chkIsSeaPort.Name = "chkIsSeaPort";
            this.chkIsSeaPort.Size = new System.Drawing.Size(96, 21);
            this.chkIsSeaPort.TabIndex = 14;
            this.chkIsSeaPort.Text = "Is Sea Port";
            this.chkIsSeaPort.UseVisualStyleBackColor = true;
            // 
            // chkIsAirPort
            // 
            this.chkIsAirPort.AutoSize = true;
            this.chkIsAirPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkIsAirPort.Location = new System.Drawing.Point(618, 74);
            this.chkIsAirPort.Name = "chkIsAirPort";
            this.chkIsAirPort.Size = new System.Drawing.Size(88, 21);
            this.chkIsAirPort.TabIndex = 15;
            this.chkIsAirPort.Text = "Is Air Port";
            this.chkIsAirPort.UseVisualStyleBackColor = true;
            // 
            // B07
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 450);
            this.IsDeleteOnBrowse = false;
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B07";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B07. Port of Discharge";
            this.WorkAlias = "PortByBrandShipmode";
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

        private Class.Txtbrand txtBrand;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Class.Txtcountry txtcountry;
        private Win.UI.EditBox editBoxRemark;
        private Win.UI.TextBox txtContinent;
        private Win.UI.DisplayBox displayContinent;
        private Class.TxtPulloutPort txtPort1;
        private Win.UI.CheckBox chkJunk;
        private Win.UI.CheckBox chkIsAirPort;
        private Win.UI.CheckBox chkIsSeaPort;
    }
}