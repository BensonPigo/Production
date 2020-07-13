namespace Sci.Production.Shipping
{
    partial class B53
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
            this.displayUnit = new Sci.Win.UI.DisplayBox();
            this.txtSubconSupplier = new Sci.Production.Class.TxtsubconNoConfirm();
            this.numWeight = new Sci.Win.UI.NumericBox();
            this.labelWeight = new Sci.Win.UI.Label();
            this.checkJunk = new Sci.Win.UI.CheckBox();
            this.displayCustomsUnit = new Sci.Win.UI.DisplayBox();
            this.txtGoodsDescription = new Sci.Win.UI.TextBox();
            this.displayMaterialType = new Sci.Win.UI.DisplayBox();
            this.editDescription = new Sci.Win.UI.EditBox();
            this.displayRefNo = new Sci.Win.UI.DisplayBox();
            this.labelSupplier = new Sci.Win.UI.Label();
            this.labelCustomsUnit = new Sci.Win.UI.Label();
            this.labelGoodsDescription = new Sci.Win.UI.Label();
            this.labelUnit = new Sci.Win.UI.Label();
            this.labelMaterialType = new Sci.Win.UI.Label();
            this.labelDescription = new Sci.Win.UI.Label();
            this.labelRefNo = new Sci.Win.UI.Label();
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
            this.detail.Size = new System.Drawing.Size(751, 312);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.displayUnit);
            this.detailcont.Controls.Add(this.txtSubconSupplier);
            this.detailcont.Controls.Add(this.numWeight);
            this.detailcont.Controls.Add(this.labelWeight);
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.displayCustomsUnit);
            this.detailcont.Controls.Add(this.txtGoodsDescription);
            this.detailcont.Controls.Add(this.displayMaterialType);
            this.detailcont.Controls.Add(this.editDescription);
            this.detailcont.Controls.Add(this.displayRefNo);
            this.detailcont.Controls.Add(this.labelSupplier);
            this.detailcont.Controls.Add(this.labelCustomsUnit);
            this.detailcont.Controls.Add(this.labelGoodsDescription);
            this.detailcont.Controls.Add(this.labelUnit);
            this.detailcont.Controls.Add(this.labelMaterialType);
            this.detailcont.Controls.Add(this.labelDescription);
            this.detailcont.Controls.Add(this.labelRefNo);
            this.detailcont.Size = new System.Drawing.Size(751, 274);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 274);
            this.detailbtm.Size = new System.Drawing.Size(751, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(751, 312);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(759, 341);
            // 
            // displayUnit
            // 
            this.displayUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayUnit.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "UnitID", true));
            this.displayUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayUnit.Location = new System.Drawing.Point(109, 154);
            this.displayUnit.Name = "displayUnit";
            this.displayUnit.Size = new System.Drawing.Size(80, 23);
            this.displayUnit.TabIndex = 60;
            // 
            // txtSubconSupplier
            // 
            this.txtSubconSupplier.DataBindings.Add(new System.Windows.Forms.Binding("TextBox1Binding", this.mtbs, "LocalSuppid", true));
            this.txtSubconSupplier.DisplayBox1Binding = "";
            this.txtSubconSupplier.IsIncludeJunk = false;
            this.txtSubconSupplier.Location = new System.Drawing.Point(474, 13);
            this.txtSubconSupplier.Name = "txtSubconSupplier";
            this.txtSubconSupplier.Size = new System.Drawing.Size(170, 23);
            this.txtSubconSupplier.TabIndex = 59;
            this.txtSubconSupplier.TextBox1Binding = "";
            // 
            // numWeight
            // 
            this.numWeight.BackColor = System.Drawing.Color.White;
            this.numWeight.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "PcsKg", true));
            this.numWeight.DecimalPlaces = 4;
            this.numWeight.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.numWeight.Location = new System.Drawing.Point(474, 186);
            this.numWeight.Name = "numWeight";
            this.numWeight.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numWeight.Size = new System.Drawing.Size(99, 23);
            this.numWeight.TabIndex = 40;
            this.numWeight.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // labelWeight
            // 
            this.labelWeight.Lines = 0;
            this.labelWeight.Location = new System.Drawing.Point(350, 186);
            this.labelWeight.Name = "labelWeight";
            this.labelWeight.Size = new System.Drawing.Size(120, 40);
            this.labelWeight.TabIndex = 56;
            this.labelWeight.Text = "Weight";
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(325, 13);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 53;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // displayCustomsUnit
            // 
            this.displayCustomsUnit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayCustomsUnit.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CustomsUnit", true));
            this.displayCustomsUnit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayCustomsUnit.Location = new System.Drawing.Point(474, 154);
            this.displayCustomsUnit.Name = "displayCustomsUnit";
            this.displayCustomsUnit.Size = new System.Drawing.Size(80, 23);
            this.displayCustomsUnit.TabIndex = 52;
            // 
            // txtGoodsDescription
            // 
            this.txtGoodsDescription.BackColor = System.Drawing.Color.White;
            this.txtGoodsDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtGoodsDescription.Location = new System.Drawing.Point(474, 122);
            this.txtGoodsDescription.Name = "txtGoodsDescription";
            this.txtGoodsDescription.Size = new System.Drawing.Size(263, 23);
            this.txtGoodsDescription.TabIndex = 36;
            this.txtGoodsDescription.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtGoodsDescription_PopUp);
            this.txtGoodsDescription.Validating += new System.ComponentModel.CancelEventHandler(this.TxtGoodsDescription_Validating);
            // 
            // displayMaterialType
            // 
            this.displayMaterialType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayMaterialType.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Category", true));
            this.displayMaterialType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayMaterialType.Location = new System.Drawing.Point(109, 122);
            this.displayMaterialType.Name = "displayMaterialType";
            this.displayMaterialType.Size = new System.Drawing.Size(190, 23);
            this.displayMaterialType.TabIndex = 50;
            // 
            // editDescription
            // 
            this.editDescription.BackColor = System.Drawing.Color.White;
            this.editDescription.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Description", true));
            this.editDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.editDescription.Location = new System.Drawing.Point(109, 43);
            this.editDescription.Multiline = true;
            this.editDescription.Name = "editDescription";
            this.editDescription.Size = new System.Drawing.Size(628, 70);
            this.editDescription.TabIndex = 49;
            // 
            // displayRefNo
            // 
            this.displayRefNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayRefNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "RefNo", true));
            this.displayRefNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayRefNo.Location = new System.Drawing.Point(109, 13);
            this.displayRefNo.Name = "displayRefNo";
            this.displayRefNo.Size = new System.Drawing.Size(190, 23);
            this.displayRefNo.TabIndex = 48;
            // 
            // labelSupplier
            // 
            this.labelSupplier.Lines = 0;
            this.labelSupplier.Location = new System.Drawing.Point(394, 13);
            this.labelSupplier.Name = "labelSupplier";
            this.labelSupplier.Size = new System.Drawing.Size(75, 23);
            this.labelSupplier.TabIndex = 47;
            this.labelSupplier.Text = "Supplier";
            // 
            // labelCustomsUnit
            // 
            this.labelCustomsUnit.Lines = 0;
            this.labelCustomsUnit.Location = new System.Drawing.Point(350, 154);
            this.labelCustomsUnit.Name = "labelCustomsUnit";
            this.labelCustomsUnit.Size = new System.Drawing.Size(120, 23);
            this.labelCustomsUnit.TabIndex = 46;
            this.labelCustomsUnit.Text = "Customs Unit";
            // 
            // labelGoodsDescription
            // 
            this.labelGoodsDescription.Lines = 0;
            this.labelGoodsDescription.Location = new System.Drawing.Point(350, 122);
            this.labelGoodsDescription.Name = "labelGoodsDescription";
            this.labelGoodsDescription.Size = new System.Drawing.Size(120, 23);
            this.labelGoodsDescription.TabIndex = 44;
            this.labelGoodsDescription.Text = "Good\'s Description";
            // 
            // labelUnit
            // 
            this.labelUnit.Lines = 0;
            this.labelUnit.Location = new System.Drawing.Point(15, 154);
            this.labelUnit.Name = "labelUnit";
            this.labelUnit.Size = new System.Drawing.Size(90, 23);
            this.labelUnit.TabIndex = 43;
            this.labelUnit.Text = "Unit";
            // 
            // labelMaterialType
            // 
            this.labelMaterialType.Lines = 0;
            this.labelMaterialType.Location = new System.Drawing.Point(15, 122);
            this.labelMaterialType.Name = "labelMaterialType";
            this.labelMaterialType.Size = new System.Drawing.Size(90, 23);
            this.labelMaterialType.TabIndex = 41;
            this.labelMaterialType.Text = "Material Type";
            // 
            // labelDescription
            // 
            this.labelDescription.Lines = 0;
            this.labelDescription.Location = new System.Drawing.Point(15, 45);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(90, 23);
            this.labelDescription.TabIndex = 39;
            this.labelDescription.Text = "Description";
            // 
            // labelRefNo
            // 
            this.labelRefNo.Lines = 0;
            this.labelRefNo.Location = new System.Drawing.Point(15, 13);
            this.labelRefNo.Name = "labelRefNo";
            this.labelRefNo.Size = new System.Drawing.Size(90, 23);
            this.labelRefNo.TabIndex = 35;
            this.labelRefNo.Text = "RefNo";
            // 
            // B53
            // 
            this.ClientSize = new System.Drawing.Size(759, 374);
            this.DefaultOrder = "RefNo";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportNew = false;
            this.IsSupportPrint = false;
            this.Name = "B53";
            this.Text = "B53. Material Basic data - Local Purchase Item";
            this.UniqueExpress = "RefNo";
            this.WorkAlias = "LocalItem";
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

        private Win.UI.DisplayBox displayUnit;
        private Class.TxtsubconNoConfirm txtSubconSupplier;
        private Win.UI.NumericBox numWeight;
        private Win.UI.Label labelWeight;
        private Win.UI.CheckBox checkJunk;
        private Win.UI.DisplayBox displayCustomsUnit;
        private Win.UI.TextBox txtGoodsDescription;
        private Win.UI.DisplayBox displayMaterialType;
        private Win.UI.EditBox editDescription;
        private Win.UI.DisplayBox displayRefNo;
        private Win.UI.Label labelSupplier;
        private Win.UI.Label labelCustomsUnit;
        private Win.UI.Label labelGoodsDescription;
        private Win.UI.Label labelUnit;
        private Win.UI.Label labelMaterialType;
        private Win.UI.Label labelDescription;
        private Win.UI.Label labelRefNo;
    }
}
