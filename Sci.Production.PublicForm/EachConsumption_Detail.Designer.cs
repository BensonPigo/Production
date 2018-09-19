namespace Sci.Production.PublicForm
{
    partial class EachConsumption_Detail
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
            this.label1 = new Sci.Win.UI.Label();
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // detailgridcont
            // 
            this.detailgridcont.Location = new System.Drawing.Point(12, 174);
            this.detailgridcont.Size = new System.Drawing.Size(760, 136);
            // 
            // detailgridicon
            // 
            this.detailgridicon.Location = new System.Drawing.Point(413, 15);
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 316);
            this.btmcont.Size = new System.Drawing.Size(784, 40);
            this.btmcont.TabIndex = 1;
            // 
            // gridcont
            // 
            this.gridcont.Size = new System.Drawing.Size(760, 134);
            // 
            // append
            // 
            this.append.TabIndex = 1;
            this.append.Visible = false;
            // 
            // revise
            // 
            this.revise.TabIndex = 2;
            this.revise.Visible = false;
            // 
            // delete
            // 
            this.delete.TabIndex = 3;
            this.delete.Visible = false;
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(694, 5);
            this.undo.TabIndex = 5;
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(614, 5);
            this.save.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(12, 149);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 22);
            this.label1.TabIndex = 0;
            this.label1.Text = "Details of Article/Size";
            this.label1.TextStyle.Color = System.Drawing.Color.Red;
            // 
            // EachConsumption_Detail
            // 
            this.ClientSize = new System.Drawing.Size(784, 356);
            this.Controls.Add(this.label1);
            this.DefaultDetailOrder = "Article, SizeCode";
            this.DefaultOrder = "ColorID";
            this.DetailGridAlias = "Order_EachCons_Color_Article";
            this.DetailGridEdit = false;
            this.DetailGridPopUp = false;
            this.DetailKeyField = "Order_EachCons_ColorUkey";
            this.GridEdit = false;
            this.GridPopUp = false;
            this.KeyField1 = "Order_EachConsUkey";
            this.MasterKeyField = "Ukey";
            this.Name = "EachConsumption_Detail";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Cons. per Color - Detail";
            this.WorkAlias = "Order_EachCons_Color";
            this.Controls.SetChildIndex(this.detailgridicon, 0);
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            this.Controls.SetChildIndex(this.detailgridcont, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.detailgridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Win.UI.Label label1;
    }
}
