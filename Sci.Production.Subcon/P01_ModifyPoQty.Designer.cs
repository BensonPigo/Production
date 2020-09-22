namespace Sci.Production.Subcon
{
    partial class P01_ModifyPoQty
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
            this.components = new System.ComponentModel.Container();
            this.numPOQty = new Sci.Win.UI.NumericBox();
            this.displayExceedQty = new Sci.Win.UI.DisplayBox();
            this.displayCurrentOrderQty = new Sci.Win.UI.DisplayBox();
            this.displayFarmIn = new Sci.Win.UI.DisplayBox();
            this.displayAPQty = new Sci.Win.UI.DisplayBox();
            this.displayFarmOut = new Sci.Win.UI.DisplayBox();
            this.displayCutpartName = new Sci.Win.UI.DisplayBox();
            this.displayArtwork = new Sci.Win.UI.DisplayBox();
            this.displayCutpartID = new Sci.Win.UI.DisplayBox();
            this.displaySPNo = new Sci.Win.UI.DisplayBox();
            this.labelExceedQty = new Sci.Win.UI.Label();
            this.labelCurrentOrderQty = new Sci.Win.UI.Label();
            this.labelPOQty = new Sci.Win.UI.Label();
            this.labelFarmIn = new Sci.Win.UI.Label();
            this.labelAPQty = new Sci.Win.UI.Label();
            this.labelFarmOut = new Sci.Win.UI.Label();
            this.labelCutpartName = new Sci.Win.UI.Label();
            this.labelArtwork = new Sci.Win.UI.Label();
            this.labelCutpartID = new Sci.Win.UI.Label();
            this.labelSPNo = new Sci.Win.UI.Label();
            this.bindingSource1 = new Sci.Win.UI.BindingSource(this.components);
            this.panel1 = new Sci.Win.UI.Panel();
            this.btnCancel = new Sci.Win.UI.Button();
            this.btnSave = new Sci.Win.UI.Button();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // numPOQty
            // 
            this.numPOQty.BackColor = System.Drawing.Color.White;
            this.numPOQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numPOQty.Location = new System.Drawing.Point(87, 190);
            this.numPOQty.Name = "numPOQty";
            this.numPOQty.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPOQty.Size = new System.Drawing.Size(149, 23);
            this.numPOQty.TabIndex = 8;
            this.numPOQty.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numPOQty.Validating += new System.ComponentModel.CancelEventHandler(this.NumPOQty_Validating);
            this.numPOQty.Validated += new System.EventHandler(this.NumPOQty_Validated);
            // 
            // displayExceedQty
            // 
            this.displayExceedQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayExceedQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayExceedQty.Location = new System.Drawing.Point(377, 190);
            this.displayExceedQty.Name = "displayExceedQty";
            this.displayExceedQty.Size = new System.Drawing.Size(149, 23);
            this.displayExceedQty.TabIndex = 9;
            // 
            // displayCurrentOrderQty
            // 
            this.displayCurrentOrderQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCurrentOrderQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCurrentOrderQty.Location = new System.Drawing.Point(377, 158);
            this.displayCurrentOrderQty.Name = "displayCurrentOrderQty";
            this.displayCurrentOrderQty.Size = new System.Drawing.Size(149, 23);
            this.displayCurrentOrderQty.TabIndex = 7;
            // 
            // displayFarmIn
            // 
            this.displayFarmIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFarmIn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFarmIn.Location = new System.Drawing.Point(87, 126);
            this.displayFarmIn.Name = "displayFarmIn";
            this.displayFarmIn.Size = new System.Drawing.Size(149, 23);
            this.displayFarmIn.TabIndex = 5;
            // 
            // displayAPQty
            // 
            this.displayAPQty.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayAPQty.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayAPQty.Location = new System.Drawing.Point(87, 158);
            this.displayAPQty.Name = "displayAPQty";
            this.displayAPQty.Size = new System.Drawing.Size(149, 23);
            this.displayAPQty.TabIndex = 6;
            // 
            // displayFarmOut
            // 
            this.displayFarmOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayFarmOut.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayFarmOut.Location = new System.Drawing.Point(87, 93);
            this.displayFarmOut.Name = "displayFarmOut";
            this.displayFarmOut.Size = new System.Drawing.Size(149, 23);
            this.displayFarmOut.TabIndex = 4;
            // 
            // displayCutpartName
            // 
            this.displayCutpartName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCutpartName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCutpartName.Location = new System.Drawing.Point(377, 41);
            this.displayCutpartName.Name = "displayCutpartName";
            this.displayCutpartName.Size = new System.Drawing.Size(149, 23);
            this.displayCutpartName.TabIndex = 3;
            // 
            // displayArtwork
            // 
            this.displayArtwork.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayArtwork.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayArtwork.Location = new System.Drawing.Point(377, 9);
            this.displayArtwork.Name = "displayArtwork";
            this.displayArtwork.Size = new System.Drawing.Size(149, 23);
            this.displayArtwork.TabIndex = 1;
            // 
            // displayCutpartID
            // 
            this.displayCutpartID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCutpartID.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCutpartID.Location = new System.Drawing.Point(87, 41);
            this.displayCutpartID.Name = "displayCutpartID";
            this.displayCutpartID.Size = new System.Drawing.Size(149, 23);
            this.displayCutpartID.TabIndex = 2;
            // 
            // displaySPNo
            // 
            this.displaySPNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displaySPNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displaySPNo.Location = new System.Drawing.Point(87, 9);
            this.displaySPNo.Name = "displaySPNo";
            this.displaySPNo.Size = new System.Drawing.Size(149, 23);
            this.displaySPNo.TabIndex = 0;
            // 
            // labelExceedQty
            // 
            this.labelExceedQty.Lines = 0;
            this.labelExceedQty.Location = new System.Drawing.Point(260, 190);
            this.labelExceedQty.Name = "labelExceedQty";
            this.labelExceedQty.Size = new System.Drawing.Size(114, 23);
            this.labelExceedQty.TabIndex = 124;
            this.labelExceedQty.Text = "Exceed Qty";
            // 
            // labelCurrentOrderQty
            // 
            this.labelCurrentOrderQty.Lines = 0;
            this.labelCurrentOrderQty.Location = new System.Drawing.Point(260, 158);
            this.labelCurrentOrderQty.Name = "labelCurrentOrderQty";
            this.labelCurrentOrderQty.Size = new System.Drawing.Size(114, 23);
            this.labelCurrentOrderQty.TabIndex = 123;
            this.labelCurrentOrderQty.Text = "Current Order Qty";
            // 
            // labelPOQty
            // 
            this.labelPOQty.Lines = 0;
            this.labelPOQty.Location = new System.Drawing.Point(9, 190);
            this.labelPOQty.Name = "labelPOQty";
            this.labelPOQty.Size = new System.Drawing.Size(75, 23);
            this.labelPOQty.TabIndex = 122;
            this.labelPOQty.Text = "PO Qty";
            // 
            // labelFarmIn
            // 
            this.labelFarmIn.Lines = 0;
            this.labelFarmIn.Location = new System.Drawing.Point(9, 126);
            this.labelFarmIn.Name = "labelFarmIn";
            this.labelFarmIn.Size = new System.Drawing.Size(75, 23);
            this.labelFarmIn.TabIndex = 121;
            this.labelFarmIn.Text = "Farm In";
            // 
            // labelAPQty
            // 
            this.labelAPQty.Lines = 0;
            this.labelAPQty.Location = new System.Drawing.Point(9, 158);
            this.labelAPQty.Name = "labelAPQty";
            this.labelAPQty.Size = new System.Drawing.Size(75, 23);
            this.labelAPQty.TabIndex = 120;
            this.labelAPQty.Text = "AP Qty";
            // 
            // labelFarmOut
            // 
            this.labelFarmOut.Lines = 0;
            this.labelFarmOut.Location = new System.Drawing.Point(9, 93);
            this.labelFarmOut.Name = "labelFarmOut";
            this.labelFarmOut.Size = new System.Drawing.Size(75, 23);
            this.labelFarmOut.TabIndex = 119;
            this.labelFarmOut.Text = "Farm Out";
            // 
            // labelCutpartName
            // 
            this.labelCutpartName.Lines = 0;
            this.labelCutpartName.Location = new System.Drawing.Point(260, 41);
            this.labelCutpartName.Name = "labelCutpartName";
            this.labelCutpartName.Size = new System.Drawing.Size(114, 23);
            this.labelCutpartName.TabIndex = 118;
            this.labelCutpartName.Text = "Cutpart Name";
            // 
            // labelArtwork
            // 
            this.labelArtwork.Lines = 0;
            this.labelArtwork.Location = new System.Drawing.Point(260, 9);
            this.labelArtwork.Name = "labelArtwork";
            this.labelArtwork.Size = new System.Drawing.Size(114, 23);
            this.labelArtwork.TabIndex = 117;
            this.labelArtwork.Text = "Artwork";
            // 
            // labelCutpartID
            // 
            this.labelCutpartID.Lines = 0;
            this.labelCutpartID.Location = new System.Drawing.Point(9, 41);
            this.labelCutpartID.Name = "labelCutpartID";
            this.labelCutpartID.Size = new System.Drawing.Size(75, 23);
            this.labelCutpartID.TabIndex = 116;
            this.labelCutpartID.Text = "Cutpart ID";
            // 
            // labelSPNo
            // 
            this.labelSPNo.Lines = 0;
            this.labelSPNo.Location = new System.Drawing.Point(9, 9);
            this.labelSPNo.Name = "labelSPNo";
            this.labelSPNo.Size = new System.Drawing.Size(75, 23);
            this.labelSPNo.TabIndex = 115;
            this.labelSPNo.Text = "SP#";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 235);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(539, 44);
            this.panel1.TabIndex = 11;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnCancel.Location = new System.Drawing.Point(447, 6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnSave.Location = new System.Drawing.Point(360, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 30);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // P01_ModifyPoQty
            // 
            this.ClientSize = new System.Drawing.Size(539, 279);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.numPOQty);
            this.Controls.Add(this.displayExceedQty);
            this.Controls.Add(this.displayCurrentOrderQty);
            this.Controls.Add(this.displayFarmIn);
            this.Controls.Add(this.displayAPQty);
            this.Controls.Add(this.displayFarmOut);
            this.Controls.Add(this.displayCutpartName);
            this.Controls.Add(this.displayArtwork);
            this.Controls.Add(this.displayCutpartID);
            this.Controls.Add(this.displaySPNo);
            this.Controls.Add(this.labelExceedQty);
            this.Controls.Add(this.labelCurrentOrderQty);
            this.Controls.Add(this.labelPOQty);
            this.Controls.Add(this.labelFarmIn);
            this.Controls.Add(this.labelAPQty);
            this.Controls.Add(this.labelFarmOut);
            this.Controls.Add(this.labelCutpartName);
            this.Controls.Add(this.labelArtwork);
            this.Controls.Add(this.labelCutpartID);
            this.Controls.Add(this.labelSPNo);
            this.DefaultControl = "numPOQty";
            this.Name = "P01_ModifyPoQty";
            this.Text = "Modify PO Qty";
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.NumericBox numPOQty;
        private Win.UI.DisplayBox displayExceedQty;
        private Win.UI.DisplayBox displayCurrentOrderQty;
        private Win.UI.DisplayBox displayFarmIn;
        private Win.UI.DisplayBox displayAPQty;
        private Win.UI.DisplayBox displayFarmOut;
        private Win.UI.DisplayBox displayCutpartName;
        private Win.UI.DisplayBox displayArtwork;
        private Win.UI.DisplayBox displayCutpartID;
        private Win.UI.DisplayBox displaySPNo;
        private Win.UI.Label labelExceedQty;
        private Win.UI.Label labelCurrentOrderQty;
        private Win.UI.Label labelPOQty;
        private Win.UI.Label labelFarmIn;
        private Win.UI.Label labelAPQty;
        private Win.UI.Label labelFarmOut;
        private Win.UI.Label labelCutpartName;
        private Win.UI.Label labelArtwork;
        private Win.UI.Label labelCutpartID;
        private Win.UI.Label labelSPNo;
        private Win.UI.BindingSource bindingSource1;
        private Win.UI.Panel panel1;
        private Win.UI.Button btnCancel;
        private Win.UI.Button btnSave;
    }
}
