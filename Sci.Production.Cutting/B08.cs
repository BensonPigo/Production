using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Class;
using Sci.Win.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Windows.Forms;

namespace Sci.Production.Cutting
{
    /// <summary>
    /// B08
    /// </summary>
    public partial class B08 : Sci.Win.Tems.Input6
    {
        /// <summary>
        /// B08
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public B08(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();
            DataGridViewGeneratorTextColumnSettings mtlTypeID = new DataGridViewGeneratorTextColumnSettings();
            mtlTypeID.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (!this.EditMode) return;
                    if (e.RowIndex == -1) return;
                    if (this.CurrentDetailData == null) return;

                    DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                    DataTable dt;
                    DBProxy.Current.Select(null, $"Select id from MtlType WITH (NOLOCK) where junk=0", out dt);
                    SelectItem sele = new SelectItem(dt, "ID", "10@300,300", dr["MtlTypeID"].ToString());
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel) return; 
                    dr["MtlTypeID"] = sele.GetSelectedString();
                    dr.EndEdit();
                }
            };

            mtlTypeID.CellValidating += (s, e) =>
            {
                if (!this.EditMode) return;
                if (e.RowIndex == -1) return;
                if (this.CurrentDetailData == null) return;
                if (MyUtility.Check.Empty(e.FormattedValue)) return;
                DataRow dr = detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Seek($"Select id from MtlType WITH (NOLOCK) where junk=0 and id = '{e.FormattedValue}'"))
                {
                    MyUtility.Msg.WarningBox("MtlType not exists!");
                    dr["MtlTypeID"] = string.Empty;
                }
                else
                {
                    dr["MtlTypeID"] = e.FormattedValue;
                }
                dr.EndEdit();
            };
            Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("MtlTypeID", header: "MtlType", width: Widths.AnsiChars(20), settings: mtlTypeID)
            .Numeric("LayerLowerBound", header: "LayerLowerBound", width: Widths.AnsiChars(8))
            .Numeric("LayerUpperBound", header: "LayerUpperBound", width: Widths.AnsiChars(8))
            .Numeric("ActualSpeed", header: "ActualSpeed\n(1000m/min)", width: Widths.AnsiChars(8), decimal_places: 3)
            .Text("remark", header: "remark", width: Widths.AnsiChars(15))
            ;
        }

        protected override bool ClickEdit()
        {
            this.txtID.ReadOnly = true;
            return base.ClickEdit();
        }

        protected override bool ClickSaveBefore()
        {
            #region MtlTypeID,LayerLowerBound,LayerUpperBound,ActualSpeed必輸入,不能是0 .. LayerLowerBound不能大於LayerUpperBound
            foreach (DataRow dr in DetailDatas.Where(r => r.RowState != DataRowState.Deleted))
            {
                if (MyUtility.Check.Empty(dr["MtlTypeID"]) || MyUtility.Check.Empty(dr["LayerLowerBound"]) ||
                    MyUtility.Check.Empty(dr["LayerUpperBound"]) || MyUtility.Check.Empty(dr["ActualSpeed"]))
                {
                    MyUtility.Msg.WarningBox("MtlType, LayerLowerBound, LayerUpperBound, ActualSpeed can not empty!");
                    return false;
                }

                if (MyUtility.Convert.GetDecimal(dr["LayerLowerBound"]) > MyUtility.Convert.GetDecimal(dr["LayerUpperBound"]))
                {
                    MyUtility.Msg.WarningBox("LayerLowerBound cannot be greater than LayerUpperBound.");
                    return false;
                }
            }
            #endregion

            #region 同mtlTypeID有多筆時, LayerUpperBound(數字加1)與下一筆LayerLowerBound要連續 
            string oldMtlTypeID = string.Empty;
            int oldLayerUpperBound = -1;
            foreach (DataRow dr in DetailDatas.Where(r => r.RowState != DataRowState.Deleted)
                .OrderBy(o => o["LayerUpperBound"]).OrderBy(o => o["LayerLowerBound"]).OrderBy(o => o["MtlTypeID"]))
            {
                if (oldMtlTypeID == MyUtility.Convert.GetString(dr["MtlTypeID"]) &&
                    oldLayerUpperBound + 1 != MyUtility.Convert.GetInt(dr["LayerLowerBound"]))
                {
                    MyUtility.Msg.WarningBox("Layers of the same MtlType must be consecutive.");
                    return false;
                }

                oldLayerUpperBound = MyUtility.Convert.GetInt(dr["LayerUpperBound"]);
                oldMtlTypeID = MyUtility.Convert.GetString(dr["MtlTypeID"]);
            }
            #endregion

            return base.ClickSaveBefore();
        }
    }
}
