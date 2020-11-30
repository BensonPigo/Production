namespace Sci.Production.Centralized
{
    partial class Shipping_B07
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
            this.chkIsAirPort = new Sci.Win.UI.CheckBox();
            this.chkIsSeaPort = new Sci.Win.UI.CheckBox();
            this.chkJunk = new Sci.Win.UI.CheckBox();
            this.editBoxRemark = new Sci.Win.UI.EditBox();
            this.txtContinent = new Sci.Win.UI.TextBox();
            this.displayContinent = new Sci.Win.UI.DisplayBox();
            this.label5 = new Sci.Win.UI.Label();
            this.label4 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label2 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.txtPulloutPortID = new Sci.Production.Class.TxtPulloutPort();
            this.txtcountry = new Sci.Production.Class.Txtcountry();
            this.txtBrand = new Sci.Production.Class.Txtbrand();
            this.btnImport = new Sci.Win.UI.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
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
            this.detail.Size = new System.Drawing.Size(974, 388);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.chkIsAirPort);
            this.detailcont.Controls.Add(this.chkIsSeaPort);
            this.detailcont.Controls.Add(this.chkJunk);
            this.detailcont.Controls.Add(this.txtPulloutPortID);
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
            this.detailcont.Size = new System.Drawing.Size(974, 350);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 350);
            this.detailbtm.Size = new System.Drawing.Size(974, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(974, 388);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(982, 417);
            // 
            // chkIsAirPort
            // 
            this.chkIsAirPort.AutoSize = true;
            this.chkIsAirPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.chkIsAirPort.IsSupportEditMode = false;
            this.chkIsAirPort.Location = new System.Drawing.Point(630, 89);
            this.chkIsAirPort.Name = "chkIsAirPort";
            this.chkIsAirPort.ReadOnly = true;
            this.chkIsAirPort.Size = new System.Drawing.Size(88, 21);
            this.chkIsAirPort.TabIndex = 29;
            this.chkIsAirPort.Text = "Is Air Port";
            this.chkIsAirPort.UseVisualStyleBackColor = true;
            // 
            // chkIsSeaPort
            // 
            this.chkIsSeaPort.AutoSize = true;
            this.chkIsSeaPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.chkIsSeaPort.IsSupportEditMode = false;
            this.chkIsSeaPort.Location = new System.Drawing.Point(630, 62);
            this.chkIsSeaPort.Name = "chkIsSeaPort";
            this.chkIsSeaPort.ReadOnly = true;
            this.chkIsSeaPort.Size = new System.Drawing.Size(96, 21);
            this.chkIsSeaPort.TabIndex = 28;
            this.chkIsSeaPort.Text = "Is Sea Port";
            this.chkIsSeaPort.UseVisualStyleBackColor = true;
            // 
            // chkJunk
            // 
            this.chkJunk.AutoSize = true;
            this.chkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.chkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkJunk.Location = new System.Drawing.Point(630, 35);
            this.chkJunk.Name = "chkJunk";
            this.chkJunk.Size = new System.Drawing.Size(57, 21);
            this.chkJunk.TabIndex = 27;
            this.chkJunk.Text = "Junk";
            this.chkJunk.UseVisualStyleBackColor = true;
            // 
            // editBoxRemark
            // 
            this.editBoxRemark.BackColor = System.Drawing.Color.White;
            this.editBoxRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editBoxRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editBoxRemark.Location = new System.Drawing.Point(145, 223);
            this.editBoxRemark.Multiline = true;
            this.editBoxRemark.Name = "editBoxRemark";
            this.editBoxRemark.Size = new System.Drawing.Size(361, 93);
            this.editBoxRemark.TabIndex = 25;
            // 
            // txtContinent
            // 
            this.txtContinent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtContinent.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "ContinentID", true));
            this.txtContinent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtContinent.IsSupportEditMode = false;
            this.txtContinent.Location = new System.Drawing.Point(145, 174);
            this.txtContinent.Name = "txtContinent";
            this.txtContinent.ReadOnly = true;
            this.txtContinent.Size = new System.Drawing.Size(31, 23);
            this.txtContinent.TabIndex = 24;
            // 
            // displayContinent
            // 
            this.displayContinent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayContinent.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "ContinentName", true));
            this.displayContinent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayContinent.Location = new System.Drawing.Point(176, 174);
            this.displayContinent.Name = "displayContinent";
            this.displayContinent.Size = new System.Drawing.Size(292, 23);
            this.displayContinent.TabIndex = 23;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(67, 223);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 23);
            this.label5.TabIndex = 20;
            this.label5.Text = "Remark ";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(67, 174);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 23);
            this.label4.TabIndex = 19;
            this.label4.Text = "Continent";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(67, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 23);
            this.label3.TabIndex = 18;
            this.label3.Text = "Country";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(67, 80);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 23);
            this.label2.TabIndex = 17;
            this.label2.Text = "Port";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(67, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 23);
            this.label1.TabIndex = 16;
            this.label1.Text = "Brand";
            // 
            // txtPulloutPortID
            // 
            this.txtPulloutPortID.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "PulloutPortID", true));
            this.txtPulloutPortID.Location = new System.Drawing.Point(145, 80);
            this.txtPulloutPortID.Name = "txtPulloutPortID";
            this.txtPulloutPortID.Size = new System.Drawing.Size(462, 22);
            this.txtPulloutPortID.TabIndex = 26;
            this.txtPulloutPortID.TextBox1Binding = "";
            this.txtPulloutPortID.Leave += new System.EventHandler(this.TxtPulloutPortID_Leave);
            // 
            // txtcountry
            // 
            this.txtcountry.DisplayBox1Binding = "";
            this.txtcountry.Location = new System.Drawing.Point(145, 125);
            this.txtcountry.Name = "txtcountry";
            this.txtcountry.Size = new System.Drawing.Size(232, 22);
            this.txtcountry.TabIndex = 22;
            this.txtcountry.TextBox1Binding = "";
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.White;
            this.txtBrand.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BrandID", true));
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtBrand.Location = new System.Drawing.Point(145, 35);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.Size = new System.Drawing.Size(66, 23);
            this.txtBrand.TabIndex = 21;
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(824, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(146, 30);
            this.btnImport.TabIndex = 2;
            this.btnImport.Text = "Import from Excel";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog1";
            // 
            // Shipping_B07
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 450);
            this.ConnectionName = "ProductionTPE";
            this.Controls.Add(this.btnImport);
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportPrint = false;
            this.Name = "Shipping_B07";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "Shipping_B07";
            this.WorkAlias = "PortByBrandShipmode";
            this.Controls.SetChildIndex(this.tabs, 0);
            this.Controls.SetChildIndex(this.btnImport, 0);
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

        private Win.UI.CheckBox chkIsAirPort;
        private Win.UI.CheckBox chkIsSeaPort;
        private Win.UI.CheckBox chkJunk;
        private Class.TxtPulloutPort txtPulloutPortID;
        private Win.UI.EditBox editBoxRemark;
        private Win.UI.TextBox txtContinent;
        private Win.UI.DisplayBox displayContinent;
        private Class.Txtcountry txtcountry;
        private Class.Txtbrand txtBrand;
        private Win.UI.Label label5;
        private Win.UI.Label label4;
        private Win.UI.Label label3;
        private Win.UI.Label label2;
        private Win.UI.Label label1;
        private Win.UI.Button btnImport;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}