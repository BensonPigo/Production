namespace Sci.Production.PPIC
{
    partial class B12
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
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new Sci.Win.UI.SplitContainer();
            this.txtsupplier1 = new Sci.Production.Class.Txtsupplier();
            this.btnSearch = new Sci.Win.UI.Button();
            this.txtMold = new Sci.Win.UI.TextBox();
            this.lbMold = new Sci.Win.UI.Label();
            this.chkJunk = new Sci.Win.UI.CheckBox();
            this.txtBrand = new Sci.Win.UI.TextBox();
            this.txtCurrency = new Sci.Win.UI.TextBox();
            this.editRemark = new Sci.Win.UI.EditBox();
            this.btn_PurchaseHis = new Sci.Win.UI.Button();
            this.label7 = new Sci.Win.UI.Label();
            this.label6 = new Sci.Win.UI.Label();
            this.txtRefno = new Sci.Win.UI.TextBox();
            this.label2 = new Sci.Win.UI.Label();
            this.label3 = new Sci.Win.UI.Label();
            this.label1 = new Sci.Win.UI.Label();
            this.splitContainer2 = new Sci.Win.UI.SplitContainer();
            this.grid_Mold = new Sci.Win.UI.Grid();
            this.bs_Mold = new Sci.Win.UI.BindingSource(this.components);
            this.grid_Spec = new Sci.Win.UI.Grid();
            this.bs_Spec = new Sci.Win.UI.BindingSource(this.components);
            this.shapeContainer1 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            this.shapeContainer2 = new Microsoft.VisualBasic.PowerPacks.ShapeContainer();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.detail.SuspendLayout();
            this.detailcont.SuspendLayout();
            this.detailbtm.SuspendLayout();
            this.tabs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Mold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs_Mold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Spec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs_Spec)).BeginInit();
            this.SuspendLayout();
            // 
            // detail
            // 
            this.detail.Size = new System.Drawing.Size(897, 445);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.splitContainer1);
            this.detailcont.Size = new System.Drawing.Size(897, 407);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 407);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(897, 445);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(905, 474);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtsupplier1);
            this.splitContainer1.Panel1.Controls.Add(this.btnSearch);
            this.splitContainer1.Panel1.Controls.Add(this.txtMold);
            this.splitContainer1.Panel1.Controls.Add(this.lbMold);
            this.splitContainer1.Panel1.Controls.Add(this.chkJunk);
            this.splitContainer1.Panel1.Controls.Add(this.txtBrand);
            this.splitContainer1.Panel1.Controls.Add(this.txtCurrency);
            this.splitContainer1.Panel1.Controls.Add(this.editRemark);
            this.splitContainer1.Panel1.Controls.Add(this.btn_PurchaseHis);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.txtRefno);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1MinSize = 80;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(897, 407);
            this.splitContainer1.SplitterDistance = 176;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            // 
            // txtsupplier1
            // 
            this.txtsupplier1.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "suppID", true));
            this.txtsupplier1.DisplayBox1Binding = "";
            this.txtsupplier1.Location = new System.Drawing.Point(113, 57);
            this.txtsupplier1.Name = "txtsupplier1";
            this.txtsupplier1.Size = new System.Drawing.Size(147, 23);
            this.txtsupplier1.TabIndex = 69;
            this.txtsupplier1.TextBox1Binding = "";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(218, 143);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(80, 27);
            this.btnSearch.TabIndex = 68;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // txtMold
            // 
            this.txtMold.BackColor = System.Drawing.Color.White;
            this.txtMold.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtMold.IsSupportEditMode = false;
            this.txtMold.Location = new System.Drawing.Point(113, 145);
            this.txtMold.Name = "txtMold";
            this.txtMold.Size = new System.Drawing.Size(100, 23);
            this.txtMold.TabIndex = 67;
            // 
            // lbMold
            // 
            this.lbMold.Location = new System.Drawing.Point(11, 145);
            this.lbMold.Name = "lbMold";
            this.lbMold.Size = new System.Drawing.Size(100, 23);
            this.lbMold.TabIndex = 66;
            this.lbMold.Text = "Mold#";
            // 
            // chkJunk
            // 
            this.chkJunk.AutoSize = true;
            this.chkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.mtbs, "Junk", true));
            this.chkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.chkJunk.Location = new System.Drawing.Point(233, 8);
            this.chkJunk.Name = "chkJunk";
            this.chkJunk.Size = new System.Drawing.Size(57, 21);
            this.chkJunk.TabIndex = 65;
            this.chkJunk.Text = "Junk";
            this.chkJunk.UseVisualStyleBackColor = true;
            // 
            // txtBrand
            // 
            this.txtBrand.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtBrand.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BrandID", true));
            this.txtBrand.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtBrand.IsSupportEditMode = false;
            this.txtBrand.Location = new System.Drawing.Point(113, 32);
            this.txtBrand.Name = "txtBrand";
            this.txtBrand.ReadOnly = true;
            this.txtBrand.Size = new System.Drawing.Size(118, 23);
            this.txtBrand.TabIndex = 62;
            // 
            // txtCurrency
            // 
            this.txtCurrency.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtCurrency.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "CurrencyID", true));
            this.txtCurrency.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtCurrency.IsSupportEditMode = false;
            this.txtCurrency.Location = new System.Drawing.Point(335, 31);
            this.txtCurrency.Name = "txtCurrency";
            this.txtCurrency.ReadOnly = true;
            this.txtCurrency.Size = new System.Drawing.Size(118, 23);
            this.txtCurrency.TabIndex = 61;
            // 
            // editRemark
            // 
            this.editRemark.BackColor = System.Drawing.Color.White;
            this.editRemark.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Remark", true));
            this.editRemark.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editRemark.Location = new System.Drawing.Point(113, 82);
            this.editRemark.Multiline = true;
            this.editRemark.Name = "editRemark";
            this.editRemark.Size = new System.Drawing.Size(514, 58);
            this.editRemark.TabIndex = 60;
            // 
            // btn_PurchaseHis
            // 
            this.btn_PurchaseHis.Location = new System.Drawing.Point(702, 24);
            this.btn_PurchaseHis.Name = "btn_PurchaseHis";
            this.btn_PurchaseHis.Size = new System.Drawing.Size(141, 30);
            this.btn_PurchaseHis.TabIndex = 59;
            this.btn_PurchaseHis.Text = "Purchase history";
            this.btn_PurchaseHis.UseVisualStyleBackColor = true;
            this.btn_PurchaseHis.Click += new System.EventHandler(this.Btn_PurchaseHis_Click);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(10, 82);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 23);
            this.label7.TabIndex = 58;
            this.label7.Text = "Remark";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(233, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 23);
            this.label6.TabIndex = 57;
            this.label6.Text = "Currency";
            // 
            // txtRefno
            // 
            this.txtRefno.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.txtRefno.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Refno", true));
            this.txtRefno.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.txtRefno.IsSupportEditMode = false;
            this.txtRefno.Location = new System.Drawing.Point(113, 7);
            this.txtRefno.Name = "txtRefno";
            this.txtRefno.ReadOnly = true;
            this.txtRefno.Size = new System.Drawing.Size(118, 23);
            this.txtRefno.TabIndex = 56;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 54;
            this.label2.Text = "Brand";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 53;
            this.label3.Text = "Supplier";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 52;
            this.label1.Text = "Refno";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.grid_Mold);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.grid_Spec);
            this.splitContainer2.Size = new System.Drawing.Size(897, 227);
            this.splitContainer2.SplitterDistance = 453;
            this.splitContainer2.TabIndex = 0;
            this.splitContainer2.TabStop = false;
            // 
            // grid_Mold
            // 
            this.grid_Mold.AllowUserToAddRows = false;
            this.grid_Mold.AllowUserToDeleteRows = false;
            this.grid_Mold.AllowUserToResizeRows = false;
            this.grid_Mold.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid_Mold.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid_Mold.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_Mold.DataSource = this.bs_Mold;
            this.grid_Mold.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_Mold.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid_Mold.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid_Mold.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid_Mold.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid_Mold.Location = new System.Drawing.Point(0, 0);
            this.grid_Mold.Name = "grid_Mold";
            this.grid_Mold.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid_Mold.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid_Mold.RowTemplate.Height = 24;
            this.grid_Mold.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_Mold.ShowCellToolTips = false;
            this.grid_Mold.Size = new System.Drawing.Size(453, 227);
            this.grid_Mold.TabIndex = 0;
            this.grid_Mold.SelectionChanged += new System.EventHandler(this.GridMold_SelectionChanged);
            // 
            // grid_Spec
            // 
            this.grid_Spec.AllowUserToAddRows = false;
            this.grid_Spec.AllowUserToDeleteRows = false;
            this.grid_Spec.AllowUserToResizeRows = false;
            this.grid_Spec.BackgroundColor = System.Drawing.SystemColors.Control;
            this.grid_Spec.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.grid_Spec.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_Spec.DataSource = this.bs_Spec;
            this.grid_Spec.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid_Spec.EditingEnter = Ict.Win.UI.DataGridViewEditingEnter.NextCellOrNextRow;
            this.grid_Spec.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.grid_Spec.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.grid_Spec.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(161)))), ((int)(((byte)(162)))), ((int)(((byte)(163)))));
            this.grid_Spec.Location = new System.Drawing.Point(0, 0);
            this.grid_Spec.Name = "grid_Spec";
            this.grid_Spec.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(228)))), ((int)(((byte)(255)))));
            this.grid_Spec.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.grid_Spec.RowTemplate.Height = 24;
            this.grid_Spec.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_Spec.ShowCellToolTips = false;
            this.grid_Spec.Size = new System.Drawing.Size(440, 227);
            this.grid_Spec.SupportEditMode = Sci.Win.UI.AdvEditModesReadOnly.True;
            this.grid_Spec.TabIndex = 0;
            // 
            // shapeContainer1
            // 
            this.shapeContainer1.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer1.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer1.Name = "shapeContainer1";
            this.shapeContainer1.Size = new System.Drawing.Size(895, 242);
            this.shapeContainer1.TabIndex = 9;
            this.shapeContainer1.TabStop = false;
            // 
            // shapeContainer2
            // 
            this.shapeContainer2.Location = new System.Drawing.Point(0, 0);
            this.shapeContainer2.Margin = new System.Windows.Forms.Padding(0);
            this.shapeContainer2.Name = "shapeContainer2";
            this.shapeContainer2.Size = new System.Drawing.Size(895, 242);
            this.shapeContainer2.TabIndex = 9;
            this.shapeContainer2.TabStop = false;
            // 
            // B12
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 507);
            this.IsSupportClip = false;
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportEdit = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B12";
            this.OnLineHelpID = "Sci.Win.Tems.Input1";
            this.Text = "B12. Pad Print Data Maintain";
            this.WorkAlias = "PadPrint";
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.detail.ResumeLayout(false);
            this.detailcont.ResumeLayout(false);
            this.detailbtm.ResumeLayout(false);
            this.detailbtm.PerformLayout();
            this.tabs.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_Mold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs_Mold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid_Spec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs_Spec)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.SplitContainer splitContainer1;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer1;
        private Win.UI.BindingSource bs_Mold;
        private Microsoft.VisualBasic.PowerPacks.ShapeContainer shapeContainer2;
        private Win.UI.CheckBox chkJunk;
        private Win.UI.TextBox txtBrand;
        private Win.UI.TextBox txtCurrency;
        private Win.UI.EditBox editRemark;
        private Win.UI.Button btn_PurchaseHis;
        private Win.UI.Label label7;
        private Win.UI.Label label6;
        private Win.UI.TextBox txtRefno;
        private Win.UI.Label label2;
        private Win.UI.Label label3;
        private Win.UI.Label label1;
        private Win.UI.SplitContainer splitContainer2;
        private Win.UI.Grid grid_Mold;
        private Win.UI.Grid grid_Spec;
        private Win.UI.BindingSource bs_Spec;
        private Win.UI.Button btnSearch;
        private Win.UI.TextBox txtMold;
        private Win.UI.Label lbMold;
        private Class.Txtsupplier txtsupplier1;
    }
}
