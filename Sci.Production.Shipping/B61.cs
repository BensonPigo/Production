using Ict;
using Ict.Win;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class B61 : Sci.Win.Tems.Input6
    {
        /// <inheritdoc/>
        public B61(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            DataGridViewGeneratorTextColumnSettings col_PurchaseUnit = new DataGridViewGeneratorTextColumnSettings();
            col_PurchaseUnit.EditingMouseDown += (s, e) =>
             {
                 if (this.CurrentDetailData == null || !this.EditMode)
                 {
                     return;
                 }

                 if (e.Button == MouseButtons.Right)
                 {
                     string sqlcmd = @"select ID,Description from Unit WITH (NOLOCK) where junk = 0 order by ID";
                     Win.Tools.SelectItem item = new Win.Tools.SelectItem(sqlcmd, "ID,Description", "20,35", this.CurrentDetailData["PurchaseUnit"].ToString(), "ID,Description");
                     DialogResult result = item.ShowDialog();
                     if (result == DialogResult.Cancel)
                     {
                         return;
                     }

                     this.CurrentDetailData["PurchaseUnit"] = item.GetSelecteds()[0]["ID"];
                 }
             };

            base.OnDetailGridSetup();
            #region 欄位設定

            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("PurchaseUnit", header: "Purchase Unit", width: Widths.AnsiChars(8), iseditingreadonly: true, settings: col_PurchaseUnit)
                .Numeric("Ratio", header: "1 CDC Unit : Purchase Unit", width: Widths.AnsiChars(24), decimal_places: 4, integer_places: 8, iseditingreadonly: false)
                ;
            #endregion
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            this.txtCustomsDesc.ReadOnly = false;
            this.checkJunk.ReadOnly = true;
            base.ClickNewAfter();
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            this.txtCustomsDesc.ReadOnly = true;
            if (this.EditMode && this.Perm.Junk)
            {
                this.checkJunk.ReadOnly = false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override void ClickEditAfter()
        {
            this.txtCustomsDesc.ReadOnly = true;
            if (this.EditMode && this.Perm.Junk)
            {
                this.checkJunk.ReadOnly = false;
            }

            base.ClickEditAfter();
        }

        /// <inheritdoc/>
        protected override void ClickUndo()
        {
            if (this.IsDetailInserting)
            {
                this.txtCustomsDesc.ReadOnly = false;
                this.checkJunk.ReadOnly = true;
            }
            else
            {
                this.txtCustomsDesc.ReadOnly = true;
                this.checkJunk.ReadOnly = true;
            }

            base.ClickUndo();
        }

        /// <inheritdoc/>
        protected override DualResult ClickSave()
        {
            this.txtCustomsDesc.ReadOnly = true;
            this.checkJunk.ReadOnly = true;
            return base.ClickSave();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (MyUtility.Check.Empty(this.txtCDCUnit.Text) || MyUtility.Check.Empty(this.txtCustomsDesc.Text))
            {
                MyUtility.Msg.WarningBox("<Customs Description > or < CDC Unit> cannot be empty.");
                return false;
            }

            foreach (DataRow dr in this.DetailDatas)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (MyUtility.Check.Empty(dr["PurchaseUnit"]))
                    {
                        dr.Delete();
                    }
                }
            }

            return base.ClickSaveBefore();
        }
    }
}
