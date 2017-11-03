namespace Sci.Production.Basic
{
    partial class B08_ProductionFabricType
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
            this.labelProdType = new Sci.Win.UI.Label();
            this.labelFabricType = new Sci.Win.UI.Label();
            this.labelTop = new Sci.Win.UI.Label();
            this.labelBottom = new Sci.Win.UI.Label();
            this.labelInner = new Sci.Win.UI.Label();
            this.labelOuter = new Sci.Win.UI.Label();
            this.txtProdTypeTop = new Sci.Win.UI.TextBox();
            this.txtFabricTypeTop = new Sci.Win.UI.TextBox();
            this.txtProdTypeBottom = new Sci.Win.UI.TextBox();
            this.txtFabricTypeBottom = new Sci.Win.UI.TextBox();
            this.txtProdTypeInner = new Sci.Win.UI.TextBox();
            this.txtFabricTypeInner = new Sci.Win.UI.TextBox();
            this.txtProdTypeOuter = new Sci.Win.UI.TextBox();
            this.txtFabricTypeOuter = new Sci.Win.UI.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 216);
            this.btmcont.Size = new System.Drawing.Size(418, 44);
            // 
            // edit
            // 
            this.edit.Size = new System.Drawing.Size(80, 34);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(248, 5);
            this.save.Size = new System.Drawing.Size(80, 34);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(328, 5);
            this.undo.Size = new System.Drawing.Size(80, 34);
            // 
            // labelProdType
            // 
            this.labelProdType.Lines = 0;
            this.labelProdType.Location = new System.Drawing.Point(129, 21);
            this.labelProdType.Name = "labelProdType";
            this.labelProdType.Size = new System.Drawing.Size(80, 23);
            this.labelProdType.TabIndex = 94;
            this.labelProdType.Text = "Prod. Type";
            // 
            // labelFabricType
            // 
            this.labelFabricType.Lines = 0;
            this.labelFabricType.Location = new System.Drawing.Point(283, 21);
            this.labelFabricType.Name = "labelFabricType";
            this.labelFabricType.Size = new System.Drawing.Size(80, 23);
            this.labelFabricType.TabIndex = 95;
            this.labelFabricType.Text = "Fabric Type";
            // 
            // labelTop
            // 
            this.labelTop.Lines = 0;
            this.labelTop.Location = new System.Drawing.Point(25, 58);
            this.labelTop.Name = "labelTop";
            this.labelTop.Size = new System.Drawing.Size(60, 23);
            this.labelTop.TabIndex = 96;
            this.labelTop.Text = "Top";
            // 
            // labelBottom
            // 
            this.labelBottom.Lines = 0;
            this.labelBottom.Location = new System.Drawing.Point(25, 96);
            this.labelBottom.Name = "labelBottom";
            this.labelBottom.Size = new System.Drawing.Size(60, 23);
            this.labelBottom.TabIndex = 97;
            this.labelBottom.Text = "Bottom";
            // 
            // labelInner
            // 
            this.labelInner.Lines = 0;
            this.labelInner.Location = new System.Drawing.Point(25, 134);
            this.labelInner.Name = "labelInner";
            this.labelInner.Size = new System.Drawing.Size(60, 23);
            this.labelInner.TabIndex = 98;
            this.labelInner.Text = "Inner";
            // 
            // labelOuter
            // 
            this.labelOuter.Lines = 0;
            this.labelOuter.Location = new System.Drawing.Point(25, 172);
            this.labelOuter.Name = "labelOuter";
            this.labelOuter.Size = new System.Drawing.Size(60, 23);
            this.labelOuter.TabIndex = 99;
            this.labelOuter.Text = "Outer";
            // 
            // txtProdTypeTop
            // 
            this.txtProdTypeTop.BackColor = System.Drawing.Color.White;
            this.txtProdTypeTop.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "TopProductionType", true));
            this.txtProdTypeTop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtProdTypeTop.Location = new System.Drawing.Point(102, 58);
            this.txtProdTypeTop.Name = "txtProdTypeTop";
            this.txtProdTypeTop.Size = new System.Drawing.Size(140, 23);
            this.txtProdTypeTop.TabIndex = 100;
            this.txtProdTypeTop.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtProdTypeTopButtomInnerOuter_PopUp);
            this.txtProdTypeTop.Validating += new System.ComponentModel.CancelEventHandler(this.TxtProdTypeTopButtomInnerOuter_Validating);
            // 
            // txtFabricTypeTop
            // 
            this.txtFabricTypeTop.BackColor = System.Drawing.Color.White;
            this.txtFabricTypeTop.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "TopFabricType", true));
            this.txtFabricTypeTop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFabricTypeTop.Location = new System.Drawing.Point(262, 58);
            this.txtFabricTypeTop.Name = "txtFabricTypeTop";
            this.txtFabricTypeTop.Size = new System.Drawing.Size(140, 23);
            this.txtFabricTypeTop.TabIndex = 101;
            this.txtFabricTypeTop.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtFabricTypeTopButtomInnerOuter_PopUp);
            this.txtFabricTypeTop.Validating += new System.ComponentModel.CancelEventHandler(this.TxtFabricTypeTopButtomInnerOuter_Validating);
            // 
            // txtProdTypeBottom
            // 
            this.txtProdTypeBottom.BackColor = System.Drawing.Color.White;
            this.txtProdTypeBottom.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BottomProductionType", true));
            this.txtProdTypeBottom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtProdTypeBottom.Location = new System.Drawing.Point(102, 96);
            this.txtProdTypeBottom.Name = "txtProdTypeBottom";
            this.txtProdTypeBottom.Size = new System.Drawing.Size(140, 23);
            this.txtProdTypeBottom.TabIndex = 102;
            this.txtProdTypeBottom.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtProdTypeTopButtomInnerOuter_PopUp);
            this.txtProdTypeBottom.Validating += new System.ComponentModel.CancelEventHandler(this.TxtProdTypeTopButtomInnerOuter_Validating);
            // 
            // txtFabricTypeBottom
            // 
            this.txtFabricTypeBottom.BackColor = System.Drawing.Color.White;
            this.txtFabricTypeBottom.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "BottomFabricType", true));
            this.txtFabricTypeBottom.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFabricTypeBottom.Location = new System.Drawing.Point(262, 96);
            this.txtFabricTypeBottom.Name = "txtFabricTypeBottom";
            this.txtFabricTypeBottom.Size = new System.Drawing.Size(140, 23);
            this.txtFabricTypeBottom.TabIndex = 103;
            this.txtFabricTypeBottom.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtFabricTypeTopButtomInnerOuter_PopUp);
            this.txtFabricTypeBottom.Validating += new System.ComponentModel.CancelEventHandler(this.TxtFabricTypeTopButtomInnerOuter_Validating);
            // 
            // txtProdTypeInner
            // 
            this.txtProdTypeInner.BackColor = System.Drawing.Color.White;
            this.txtProdTypeInner.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "InnerProductionType", true));
            this.txtProdTypeInner.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtProdTypeInner.Location = new System.Drawing.Point(102, 134);
            this.txtProdTypeInner.Name = "txtProdTypeInner";
            this.txtProdTypeInner.Size = new System.Drawing.Size(140, 23);
            this.txtProdTypeInner.TabIndex = 104;
            this.txtProdTypeInner.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtProdTypeTopButtomInnerOuter_PopUp);
            this.txtProdTypeInner.Validating += new System.ComponentModel.CancelEventHandler(this.TxtProdTypeTopButtomInnerOuter_Validating);
            // 
            // txtFabricTypeInner
            // 
            this.txtFabricTypeInner.BackColor = System.Drawing.Color.White;
            this.txtFabricTypeInner.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "InnerFabricType", true));
            this.txtFabricTypeInner.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFabricTypeInner.Location = new System.Drawing.Point(262, 133);
            this.txtFabricTypeInner.Name = "txtFabricTypeInner";
            this.txtFabricTypeInner.Size = new System.Drawing.Size(140, 23);
            this.txtFabricTypeInner.TabIndex = 105;
            this.txtFabricTypeInner.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtFabricTypeTopButtomInnerOuter_PopUp);
            this.txtFabricTypeInner.Validating += new System.ComponentModel.CancelEventHandler(this.TxtFabricTypeTopButtomInnerOuter_Validating);
            // 
            // txtProdTypeOuter
            // 
            this.txtProdTypeOuter.BackColor = System.Drawing.Color.White;
            this.txtProdTypeOuter.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "OuterProductionType", true));
            this.txtProdTypeOuter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtProdTypeOuter.Location = new System.Drawing.Point(102, 172);
            this.txtProdTypeOuter.Name = "txtProdTypeOuter";
            this.txtProdTypeOuter.Size = new System.Drawing.Size(140, 23);
            this.txtProdTypeOuter.TabIndex = 106;
            this.txtProdTypeOuter.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtProdTypeTopButtomInnerOuter_PopUp);
            this.txtProdTypeOuter.Validating += new System.ComponentModel.CancelEventHandler(this.TxtProdTypeTopButtomInnerOuter_Validating);
            // 
            // txtFabricTypeOuter
            // 
            this.txtFabricTypeOuter.BackColor = System.Drawing.Color.White;
            this.txtFabricTypeOuter.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "OuterFabricType", true));
            this.txtFabricTypeOuter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtFabricTypeOuter.Location = new System.Drawing.Point(262, 171);
            this.txtFabricTypeOuter.Name = "txtFabricTypeOuter";
            this.txtFabricTypeOuter.Size = new System.Drawing.Size(140, 23);
            this.txtFabricTypeOuter.TabIndex = 107;
            this.txtFabricTypeOuter.PopUp += new System.EventHandler<Sci.Win.UI.TextBoxPopUpEventArgs>(this.TxtFabricTypeTopButtomInnerOuter_PopUp);
            this.txtFabricTypeOuter.Validating += new System.ComponentModel.CancelEventHandler(this.TxtFabricTypeTopButtomInnerOuter_Validating);
            // 
            // B08_ProductionFabricType
            // 
            this.ClientSize = new System.Drawing.Size(418, 260);
            this.Controls.Add(this.txtFabricTypeOuter);
            this.Controls.Add(this.txtProdTypeOuter);
            this.Controls.Add(this.txtFabricTypeInner);
            this.Controls.Add(this.txtProdTypeInner);
            this.Controls.Add(this.txtFabricTypeBottom);
            this.Controls.Add(this.txtProdTypeBottom);
            this.Controls.Add(this.txtFabricTypeTop);
            this.Controls.Add(this.txtProdTypeTop);
            this.Controls.Add(this.labelOuter);
            this.Controls.Add(this.labelInner);
            this.Controls.Add(this.labelBottom);
            this.Controls.Add(this.labelTop);
            this.Controls.Add(this.labelFabricType);
            this.Controls.Add(this.labelProdType);
            this.Name = "B08_ProductionFabricType";
            this.Text = "Prod./Fabric Type";
            this.WorkAlias = "CDCode_Content";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.labelProdType, 0);
            this.Controls.SetChildIndex(this.labelFabricType, 0);
            this.Controls.SetChildIndex(this.labelTop, 0);
            this.Controls.SetChildIndex(this.labelBottom, 0);
            this.Controls.SetChildIndex(this.labelInner, 0);
            this.Controls.SetChildIndex(this.labelOuter, 0);
            this.Controls.SetChildIndex(this.txtProdTypeTop, 0);
            this.Controls.SetChildIndex(this.txtFabricTypeTop, 0);
            this.Controls.SetChildIndex(this.txtProdTypeBottom, 0);
            this.Controls.SetChildIndex(this.txtFabricTypeBottom, 0);
            this.Controls.SetChildIndex(this.txtProdTypeInner, 0);
            this.Controls.SetChildIndex(this.txtFabricTypeInner, 0);
            this.Controls.SetChildIndex(this.txtProdTypeOuter, 0);
            this.Controls.SetChildIndex(this.txtFabricTypeOuter, 0);
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelProdType;
        private Win.UI.Label labelFabricType;
        private Win.UI.Label labelTop;
        private Win.UI.Label labelBottom;
        private Win.UI.Label labelInner;
        private Win.UI.Label labelOuter;
        private Win.UI.TextBox txtProdTypeTop;
        private Win.UI.TextBox txtFabricTypeTop;
        private Win.UI.TextBox txtProdTypeBottom;
        private Win.UI.TextBox txtFabricTypeBottom;
        private Win.UI.TextBox txtProdTypeInner;
        private Win.UI.TextBox txtFabricTypeInner;
        private Win.UI.TextBox txtProdTypeOuter;
        private Win.UI.TextBox txtFabricTypeOuter;
    }
}
