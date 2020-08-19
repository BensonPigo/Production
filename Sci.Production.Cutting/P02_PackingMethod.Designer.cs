namespace Sci.Production.Cutting
{
    partial class P02_PackingMethod
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
            this.labelOrderList = new Sci.Win.UI.Label();
            this.labelPackingMethod = new Sci.Win.UI.Label();
            this.labelQtyCarton = new Sci.Win.UI.Label();
            this.numQtyCarton = new Sci.Win.UI.NumericBox();
            this.btnBreakdown = new Sci.Win.UI.Button();
            this.editpacking = new Sci.Win.UI.EditBox();
            this.displayPackingMethod = new Sci.Win.UI.DisplayBox();
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).BeginInit();
            this.btmcont.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridbs
            // 
            this.gridbs.PositionChanged += new System.EventHandler(this.Gridbs_PositionChanged);
            // 
            // btmcont
            // 
            this.btmcont.Location = new System.Drawing.Point(0, 415);
            this.btmcont.Size = new System.Drawing.Size(632, 40);
            // 
            // gridcont
            // 
            this.gridcont.Location = new System.Drawing.Point(12, 51);
            this.gridcont.Size = new System.Drawing.Size(172, 353);
            // 
            // undo
            // 
            this.undo.Location = new System.Drawing.Point(542, 5);
            // 
            // save
            // 
            this.save.Location = new System.Drawing.Point(462, 5);
            // 
            // labelOrderList
            // 
            this.labelOrderList.Lines = 0;
            this.labelOrderList.Location = new System.Drawing.Point(12, 22);
            this.labelOrderList.Name = "labelOrderList";
            this.labelOrderList.Size = new System.Drawing.Size(108, 23);
            this.labelOrderList.TabIndex = 98;
            this.labelOrderList.Text = "Order List";
            // 
            // labelPackingMethod
            // 
            this.labelPackingMethod.Lines = 0;
            this.labelPackingMethod.Location = new System.Drawing.Point(190, 22);
            this.labelPackingMethod.Name = "labelPackingMethod";
            this.labelPackingMethod.Size = new System.Drawing.Size(108, 23);
            this.labelPackingMethod.TabIndex = 100;
            this.labelPackingMethod.Text = "Packing Method";
            // 
            // labelQtyCarton
            // 
            this.labelQtyCarton.Lines = 0;
            this.labelQtyCarton.Location = new System.Drawing.Point(368, 22);
            this.labelQtyCarton.Name = "labelQtyCarton";
            this.labelQtyCarton.Size = new System.Drawing.Size(82, 23);
            this.labelQtyCarton.TabIndex = 101;
            this.labelQtyCarton.Text = "Qty/Carton";
            // 
            // numQtyCarton
            // 
            this.numQtyCarton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.numQtyCarton.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "CtnQty", true));
            this.numQtyCarton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.numQtyCarton.Location = new System.Drawing.Point(368, 49);
            this.numQtyCarton.Name = "numQtyCarton";
            this.numQtyCarton.NullValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numQtyCarton.ReadOnly = true;
            this.numQtyCarton.Size = new System.Drawing.Size(100, 23);
            this.numQtyCarton.TabIndex = 102;
            this.numQtyCarton.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            // 
            // btnBreakdown
            // 
            this.btnBreakdown.Location = new System.Drawing.Point(474, 45);
            this.btnBreakdown.Name = "btnBreakdown";
            this.btnBreakdown.Size = new System.Drawing.Size(96, 30);
            this.btnBreakdown.TabIndex = 103;
            this.btnBreakdown.Text = "Breakdown";
            this.btnBreakdown.UseVisualStyleBackColor = true;
            this.btnBreakdown.Click += new System.EventHandler(this.BtnBreakdown_Click);
            // 
            // editpacking
            // 
            this.editpacking.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.editpacking.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.mtbs, "packing", true));
            this.editpacking.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.editpacking.Location = new System.Drawing.Point(190, 78);
            this.editpacking.Multiline = true;
            this.editpacking.Name = "editpacking";
            this.editpacking.ReadOnly = true;
            this.editpacking.Size = new System.Drawing.Size(418, 331);
            this.editpacking.TabIndex = 104;
            // 
            // displayPackingMethod
            // 
            this.displayPackingMethod.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(227)))), ((int)(((byte)(255)))));
            this.displayPackingMethod.DataBindings.Add(new System.Windows.Forms.Binding("Value", this.mtbs, "Packingmethod", true));
            this.displayPackingMethod.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.displayPackingMethod.Location = new System.Drawing.Point(190, 49);
            this.displayPackingMethod.Name = "displayPackingMethod";
            this.displayPackingMethod.Size = new System.Drawing.Size(172, 23);
            this.displayPackingMethod.TabIndex = 105;
            // 
            // P02_PackingMethod
            // 
            this.ClientSize = new System.Drawing.Size(632, 455);
            this.Controls.Add(this.displayPackingMethod);
            this.Controls.Add(this.editpacking);
            this.Controls.Add(this.btnBreakdown);
            this.Controls.Add(this.numQtyCarton);
            this.Controls.Add(this.labelQtyCarton);
            this.Controls.Add(this.labelPackingMethod);
            this.Controls.Add(this.labelOrderList);
            this.GridEdit = false;
            this.GridPopUp = false;
            this.KeyField1 = "cuttingsp";
            this.Name = "P02_PackingMethod";
            this.Text = "Packing Method";
            this.WorkAlias = "orders";
            this.Controls.SetChildIndex(this.btmcont, 0);
            this.Controls.SetChildIndex(this.gridcont, 0);
            this.Controls.SetChildIndex(this.labelOrderList, 0);
            this.Controls.SetChildIndex(this.labelPackingMethod, 0);
            this.Controls.SetChildIndex(this.labelQtyCarton, 0);
            this.Controls.SetChildIndex(this.numQtyCarton, 0);
            this.Controls.SetChildIndex(this.btnBreakdown, 0);
            this.Controls.SetChildIndex(this.editpacking, 0);
            this.Controls.SetChildIndex(this.displayPackingMethod, 0);
            ((System.ComponentModel.ISupportInitialize)(this.gridbs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mtbs)).EndInit();
            this.btmcont.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Win.UI.Label labelOrderList;
        private Win.UI.Label labelPackingMethod;
        private Win.UI.Label labelQtyCarton;
        private Win.UI.NumericBox numQtyCarton;
        private Win.UI.Button btnBreakdown;
        private Win.UI.EditBox editpacking;
        private Win.UI.DisplayBox displayPackingMethod;
    }
}
