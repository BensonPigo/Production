namespace Sci.Production.Warehouse
{
    partial class P41
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
            this.listControlBindingSource1 = new Sci.Win.UI.ListControlBindingSource(this.components);
            this.panel1 = new Sci.Win.UI.Panel();
            this.btnFind = new Sci.Win.UI.Button();
            this.btnQuery = new Sci.Win.UI.Button();
            this.txtLocateForSP = new Sci.Win.UI.TextBox();
            this.labelLocateForSP = new Sci.Win.UI.Label();
            this.checkEmptyMtlETA = new Sci.Win.UI.CheckBox();
            this.checkEachCons = new Sci.Win.UI.CheckBox();
            this.dateBuyerDelivery = new Sci.Win.UI.DateRange();
            this.labelBuyerDelivery = new Sci.Win.UI.Label();
            this.dateSewingInline = new Sci.Win.UI.DateRange();
            this.labelSewingInline = new Sci.Win.UI.Label();
            this.dateSCIDelivery = new Sci.Win.UI.DateRange();
            this.labelSCIDelivery = new Sci.Win.UI.Label();
            this.panel3 = new Sci.Win.UI.Panel();
            this.btnToExcel = new Sci.Win.UI.Button();
            this.btnClose = new Sci.Win.UI.Button();
            this.panel2 = new Sci.Win.UI.Panel();
            this.gridEmbAppliqueQuery = new Sci.Win.UI.Grid();
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEmbAppliqueQuery)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnFind);
            this.panel1.Controls.Add(this.btnQuery);
            this.panel1.Controls.Add(this.txtLocateForSP);
            this.panel1.Controls.Add(this.labelLocateForSP);
            this.panel1.Controls.Add(this.checkEmptyMtlETA);
            this.panel1.Controls.Add(this.checkEachCons);
            this.panel1.Controls.Add(this.dateBuyerDelivery);
            this.panel1.Controls.Add(this.labelBuyerDelivery);
            this.panel1.Controls.Add(this.dateSewingInline);
            this.panel1.Controls.Add(this.labelSewingInline);
            this.panel1.Controls.Add(this.dateSCIDelivery);
            this.panel1.Controls.Add(this.labelSCIDelivery);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 106);
            this.panel1.TabIndex = 0;
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(272, 70);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(98, 30);
            this.btnFind.TabIndex = 7;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.BtnFind_Click);
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(867, 5);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(80, 30);
            this.btnQuery.TabIndex = 5;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.BtnQuery_Click);
            // 
            // txtLocateForSP
            // 
            this.txtLocateForSP.BackColor = System.Drawing.Color.White;
            this.txtLocateForSP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtLocateForSP.Location = new System.Drawing.Point(121, 74);
            this.txtLocateForSP.Name = "txtLocateForSP";
            this.txtLocateForSP.Size = new System.Drawing.Size(145, 23);
            this.txtLocateForSP.TabIndex = 6;
            // 
            // labelLocateForSP
            // 
            this.labelLocateForSP.Lines = 0;
            this.labelLocateForSP.Location = new System.Drawing.Point(9, 74);
            this.labelLocateForSP.Name = "labelLocateForSP";
            this.labelLocateForSP.Size = new System.Drawing.Size(109, 23);
            this.labelLocateForSP.TabIndex = 16;
            this.labelLocateForSP.Text = "Locate for SP#";
            // 
            // checkEmptyMtlETA
            // 
            this.checkEmptyMtlETA.AutoSize = true;
            this.checkEmptyMtlETA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkEmptyMtlETA.Location = new System.Drawing.Point(643, 40);
            this.checkEmptyMtlETA.Name = "checkEmptyMtlETA";
            this.checkEmptyMtlETA.Size = new System.Drawing.Size(157, 21);
            this.checkEmptyMtlETA.TabIndex = 4;
            this.checkEmptyMtlETA.Text = "Filter empty Mtl. ETA";
            this.checkEmptyMtlETA.UseVisualStyleBackColor = true;
            this.checkEmptyMtlETA.CheckedChanged += new System.EventHandler(this.CheckEmptyMtlETA_CheckedChanged);
            // 
            // checkEachCons
            // 
            this.checkEachCons.AutoSize = true;
            this.checkEachCons.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkEachCons.Location = new System.Drawing.Point(423, 38);
            this.checkEachCons.Name = "checkEachCons";
            this.checkEachCons.Size = new System.Drawing.Size(176, 21);
            this.checkEachCons.TabIndex = 3;
            this.checkEachCons.Text = "Filter empty Each Cons.";
            this.checkEachCons.UseVisualStyleBackColor = true;
            this.checkEachCons.CheckedChanged += new System.EventHandler(this.CheckEachCons_CheckedChanged);
            // 
            // dateBuyerDelivery
            // 
            this.dateBuyerDelivery.Location = new System.Drawing.Point(122, 40);
            this.dateBuyerDelivery.Name = "dateBuyerDelivery";
            this.dateBuyerDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateBuyerDelivery.TabIndex = 1;
            // 
            // labelBuyerDelivery
            // 
            this.labelBuyerDelivery.Lines = 0;
            this.labelBuyerDelivery.Location = new System.Drawing.Point(9, 40);
            this.labelBuyerDelivery.Name = "labelBuyerDelivery";
            this.labelBuyerDelivery.Size = new System.Drawing.Size(109, 23);
            this.labelBuyerDelivery.TabIndex = 13;
            this.labelBuyerDelivery.Text = "Buyer  Delivery";
            // 
            // dateSewingInline
            // 
            this.dateSewingInline.Location = new System.Drawing.Point(540, 9);
            this.dateSewingInline.Name = "dateSewingInline";
            this.dateSewingInline.Size = new System.Drawing.Size(280, 23);
            this.dateSewingInline.TabIndex = 2;
            // 
            // labelSewingInline
            // 
            this.labelSewingInline.Lines = 0;
            this.labelSewingInline.Location = new System.Drawing.Point(423, 9);
            this.labelSewingInline.Name = "labelSewingInline";
            this.labelSewingInline.Size = new System.Drawing.Size(114, 23);
            this.labelSewingInline.TabIndex = 11;
            this.labelSewingInline.Text = "1st Sewing Inline";
            // 
            // dateSCIDelivery
            // 
            this.dateSCIDelivery.Location = new System.Drawing.Point(122, 9);
            this.dateSCIDelivery.Name = "dateSCIDelivery";
            this.dateSCIDelivery.Size = new System.Drawing.Size(280, 23);
            this.dateSCIDelivery.TabIndex = 0;
            // 
            // labelSCIDelivery
            // 
            this.labelSCIDelivery.Lines = 0;
            this.labelSCIDelivery.Location = new System.Drawing.Point(9, 9);
            this.labelSCIDelivery.Name = "labelSCIDelivery";
            this.labelSCIDelivery.Size = new System.Drawing.Size(109, 23);
            this.labelSCIDelivery.TabIndex = 9;
            this.labelSCIDelivery.Text = "SCI  Delivery";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnToExcel);
            this.panel3.Controls.Add(this.btnClose);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 601);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1008, 60);
            this.panel3.TabIndex = 3;
            // 
            // btnToExcel
            // 
            this.btnToExcel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnToExcel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.btnToExcel.Location = new System.Drawing.Point(826, 15);
            this.btnToExcel.Name = "btnToExcel";
            this.btnToExcel.Size = new System.Drawing.Size(90, 30);
            this.btnToExcel.TabIndex = 4;
            this.btnToExcel.Text = "To Excel";
            this.btnToExcel.UseVisualStyleBackColor = true;
            this.btnToExcel.Click += new System.EventHandler(this.BtnToExcel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(922, 15);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(80, 30);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridEmbAppliqueQuery);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 106);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1008, 495);
            this.panel2.TabIndex = 4;
            // 
            // gridEmbAppliqueQuery
            // 
            this.gridEmbAppliqueQuery.AllowUserToAddRows = false;
            this.gridEmbAppliqueQuery.AllowUserToDeleteRows = false;
            this.gridEmbAppliqueQuery.AllowUserToResizeRows = false;
            this.gridEmbAppliqueQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridEmbAppliqueQuery.BackgroundColor = System.Drawing.SystemColors.Control;
            this.gridEmbAppliqueQuery.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.gridEmbAppliqueQuery.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridEmbAppliqueQuery.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.gridEmbAppliqueQuery.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.gridEmbAppliqueQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.gridEmbAppliqueQuery.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.gridEmbAppliqueQuery.Location = new System.Drawing.Point(3, 4);
            this.gridEmbAppliqueQuery.Name = "gridEmbAppliqueQuery";
            this.gridEmbAppliqueQuery.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.gridEmbAppliqueQuery.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.gridEmbAppliqueQuery.RowTemplate.Height = 24;
            this.gridEmbAppliqueQuery.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridEmbAppliqueQuery.Size = new System.Drawing.Size(1002, 486);
            this.gridEmbAppliqueQuery.TabIndex = 1;
            this.gridEmbAppliqueQuery.TabStop = false;
            // 
            // P41
            // 
            this.ClientSize = new System.Drawing.Size(1008, 661);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Name = "P41";
            this.Text = "P41. Emb. Applique Query";
            this.Controls.SetChildIndex(this.panel1, 0);
            this.Controls.SetChildIndex(this.panel3, 0);
            this.Controls.SetChildIndex(this.panel2, 0);
            ((System.ComponentModel.ISupportInitialize)(this.listControlBindingSource1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEmbAppliqueQuery)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.ListControlBindingSource listControlBindingSource1;
        private Win.UI.Panel panel1;
        private Win.UI.Button btnFind;
        private Win.UI.Button btnQuery;
        private Win.UI.TextBox txtLocateForSP;
        private Win.UI.Label labelLocateForSP;
        private Win.UI.CheckBox checkEmptyMtlETA;
        private Win.UI.CheckBox checkEachCons;
        private Win.UI.DateRange dateBuyerDelivery;
        private Win.UI.Label labelBuyerDelivery;
        private Win.UI.DateRange dateSewingInline;
        private Win.UI.Label labelSewingInline;
        private Win.UI.DateRange dateSCIDelivery;
        private Win.UI.Label labelSCIDelivery;
        private Win.UI.Panel panel3;
        private Win.UI.Button btnClose;
        private Win.UI.Panel panel2;
        private Win.UI.Grid gridEmbAppliqueQuery;
        private Win.UI.Button btnToExcel;
    }
}
