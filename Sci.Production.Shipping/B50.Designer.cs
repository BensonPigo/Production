namespace Sci.Production.Shipping
{
    partial class B50
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
            this.labelDescriptionofGoods = new Sci.Win.UI.Label();
            this.labelHSCode = new Sci.Win.UI.Label();
            this.labelCategory = new Sci.Win.UI.Label();
            this.labelNo = new Sci.Win.UI.Label();
            this.txtDescriptionofGoods = new Sci.Win.UI.TextBox();
            this.txtHSCode = new Sci.Win.UI.TextBox();
            this.comboCategory = new Sci.Win.UI.ComboBox();
            this.displayNo = new Sci.Win.UI.DisplayBox();
            this.checkJunk = new Sci.Win.UI.CheckBox();
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
            this.detail.Size = new System.Drawing.Size(830, 299);
            // 
            // detailcont
            // 
            this.detailcont.Controls.Add(this.checkJunk);
            this.detailcont.Controls.Add(this.displayNo);
            this.detailcont.Controls.Add(this.comboCategory);
            this.detailcont.Controls.Add(this.txtHSCode);
            this.detailcont.Controls.Add(this.txtDescriptionofGoods);
            this.detailcont.Controls.Add(this.labelNo);
            this.detailcont.Controls.Add(this.labelCategory);
            this.detailcont.Controls.Add(this.labelHSCode);
            this.detailcont.Controls.Add(this.labelDescriptionofGoods);
            this.detailcont.Size = new System.Drawing.Size(830, 261);
            // 
            // detailbtm
            // 
            this.detailbtm.Location = new System.Drawing.Point(0, 261);
            this.detailbtm.Size = new System.Drawing.Size(830, 38);
            // 
            // browse
            // 
            this.browse.Size = new System.Drawing.Size(830, 299);
            // 
            // tabs
            // 
            this.tabs.Size = new System.Drawing.Size(838, 328);
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
            // labelDescriptionofGoods
            // 
            this.labelDescriptionofGoods.Lines = 0;
            this.labelDescriptionofGoods.Location = new System.Drawing.Point(16, 21);
            this.labelDescriptionofGoods.Name = "labelDescriptionofGoods";
            this.labelDescriptionofGoods.Size = new System.Drawing.Size(132, 23);
            this.labelDescriptionofGoods.TabIndex = 0;
            this.labelDescriptionofGoods.Text = "Description of Goods";
            // 
            // labelHSCode
            // 
            this.labelHSCode.Lines = 0;
            this.labelHSCode.Location = new System.Drawing.Point(16, 57);
            this.labelHSCode.Name = "labelHSCode";
            this.labelHSCode.Size = new System.Drawing.Size(132, 23);
            this.labelHSCode.TabIndex = 1;
            this.labelHSCode.Text = "HS Code";
            // 
            // labelCategory
            // 
            this.labelCategory.Lines = 0;
            this.labelCategory.Location = new System.Drawing.Point(16, 94);
            this.labelCategory.Name = "labelCategory";
            this.labelCategory.Size = new System.Drawing.Size(132, 23);
            this.labelCategory.TabIndex = 2;
            this.labelCategory.Text = "Category";
            // 
            // labelNo
            // 
            this.labelNo.Lines = 0;
            this.labelNo.Location = new System.Drawing.Point(16, 130);
            this.labelNo.Name = "labelNo";
            this.labelNo.Size = new System.Drawing.Size(132, 23);
            this.labelNo.TabIndex = 3;
            this.labelNo.Text = "No.";
            // 
            // txtDescriptionofGoods
            // 
            this.txtDescriptionofGoods.BackColor = System.Drawing.Color.White;
            this.txtDescriptionofGoods.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "GoodsDescription", true));
            this.txtDescriptionofGoods.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDescriptionofGoods.Location = new System.Drawing.Point(152, 21);
            this.txtDescriptionofGoods.Name = "txtDescriptionofGoods";
            this.txtDescriptionofGoods.Size = new System.Drawing.Size(400, 23);
            this.txtDescriptionofGoods.TabIndex = 4;
            // 
            // txtHSCode
            // 
            this.txtHSCode.BackColor = System.Drawing.Color.White;
            this.txtHSCode.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "HSCode", true));
            this.txtHSCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtHSCode.Location = new System.Drawing.Point(152, 57);
            this.txtHSCode.Name = "txtHSCode";
            this.txtHSCode.Size = new System.Drawing.Size(80, 23);
            this.txtHSCode.TabIndex = 5;
            // 
            // comboCategory
            // 
            this.comboCategory.BackColor = System.Drawing.Color.White;
            this.comboCategory.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "Category", true));
            this.comboCategory.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.comboCategory.FormattingEnabled = true;
            this.comboCategory.IsSupportUnselect = true;
            this.comboCategory.Location = new System.Drawing.Point(152, 94);
            this.comboCategory.Name = "comboCategory";
            this.comboCategory.Size = new System.Drawing.Size(121, 24);
            this.comboCategory.TabIndex = 6;
            // 
            // displayNo
            // 
            this.displayNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayNo.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "NLCode", true));
            this.displayNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayNo.Location = new System.Drawing.Point(152, 130);
            this.displayNo.Name = "displayNo";
            this.displayNo.Size = new System.Drawing.Size(60, 23);
            this.displayNo.TabIndex = 7;
            // 
            // checkJunk
            // 
            this.checkJunk.AutoSize = true;
            this.checkJunk.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Junk", true));
            this.checkJunk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.checkJunk.Location = new System.Drawing.Point(591, 21);
            this.checkJunk.Name = "checkJunk";
            this.checkJunk.Size = new System.Drawing.Size(57, 21);
            this.checkJunk.TabIndex = 8;
            this.checkJunk.Text = "Junk";
            this.checkJunk.UseVisualStyleBackColor = true;
            // 
            // B50
            // 
            this.ClientSize = new System.Drawing.Size(838, 361);
            this.DefaultOrder = "NLCode";
            this.IsSupportCopy = false;
            this.IsSupportDelete = false;
            this.IsSupportPrint = false;
            this.Name = "B50";
            this.Text = "B50. CDC Goods\'s Description Basic Data";
            this.UniqueExpress = "ID";
            this.WorkAlias = "KHGoodsHSCode";
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

        private Win.UI.CheckBox checkJunk;
        private Win.UI.DisplayBox displayNo;
        private Win.UI.ComboBox comboCategory;
        private Win.UI.TextBox txtHSCode;
        private Win.UI.TextBox txtDescriptionofGoods;
        private Win.UI.Label labelNo;
        private Win.UI.Label labelCategory;
        private Win.UI.Label labelHSCode;
        private Win.UI.Label labelDescriptionofGoods;
    }
}
