using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Win.Tools;
using System.Data;
using System.Linq;
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
            DataGridViewGeneratorTextColumnSettings WeaveTypeID = new DataGridViewGeneratorTextColumnSettings();
            WeaveTypeID.EditingMouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (!this.EditMode)
                    {
                        return;
                    }

                    if (e.RowIndex == -1)
                    {
                        return;
                    }

                    if (this.CurrentDetailData == null)
                    {
                        return;
                    }

                    DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                    DataTable dt;
                    DBProxy.Current.Select(null, $"Select id from WeaveType WITH (NOLOCK) where junk=0", out dt);
                    SelectItem sele = new SelectItem(dt, "ID", "10@300,300", dr["WeaveTypeID"].ToString());
                    DialogResult result = sele.ShowDialog();
                    if (result == DialogResult.Cancel)
                    {
                        return;
                    }

                    dr["WeaveTypeID"] = sele.GetSelectedString();
                    dr.EndEdit();
                }
            };
            WeaveTypeID.CellValidating += (s, e) =>
            {
                if (!this.EditMode)
                {
                    return;
                }

                if (e.RowIndex == -1)
                {
                    return;
                }

                if (this.CurrentDetailData == null)
                {
                    return;
                }

                if (MyUtility.Check.Empty(e.FormattedValue))
                {
                    return;
                }

                DataRow dr = this.detailgrid.GetDataRow(e.RowIndex);
                if (!MyUtility.Check.Seek($"Select id from WeaveType WITH (NOLOCK) where junk=0 and id = '{e.FormattedValue}'"))
                {
                    MyUtility.Msg.WarningBox("WeaveType not exists!");
                    dr["WeaveTypeID"] = string.Empty;
                }
                else
                {
                    dr["WeaveTypeID"] = e.FormattedValue;
                }

                dr.EndEdit();
            };

            this.Helper.Controls.Grid.Generator(this.detailgrid)
           .Text("WeaveTypeID", header: "WeaveType", width: Widths.AnsiChars(20), settings: WeaveTypeID)
           .Numeric("LayerLowerBound", header: "LayerLowerBound", width: Widths.AnsiChars(8))
           .Numeric("LayerUpperBound", header: "LayerUpperBound", width: Widths.AnsiChars(8))
           .Numeric("ActualSpeed", header: "ActualSpeed\n(M/Min)", width: Widths.AnsiChars(8), integer_places: 2, decimal_places: 3, maximum: 99.999M, minimum: 0)
           .Text("remark", header: "remark", width: Widths.AnsiChars(15))
           ;

            for (int i = 0; i < this.detailgrid.ColumnCount; i++)
            {
                this.detailgrid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        protected override bool ClickEdit()
        {
            this.txtID.ReadOnly = true;
            return base.ClickEdit();
        }

        protected override bool ClickSaveBefore()
        {
            #region WeaveTypeID,LayerLowerBound,LayerUpperBound,ActualSpeed必輸入,不能是0 .. LayerLowerBound不能大於LayerUpperBound
            foreach (DataRow dr in this.DetailDatas.Where(r => r.RowState != DataRowState.Deleted))
            {
                if (MyUtility.Check.Empty(dr["WeaveTypeID"]) || MyUtility.Check.Empty(dr["LayerLowerBound"]) ||
                    MyUtility.Check.Empty(dr["LayerUpperBound"]) || MyUtility.Check.Empty(dr["ActualSpeed"]))
                {
                    MyUtility.Msg.WarningBox("WeaveType, LayerLowerBound, LayerUpperBound, ActualSpeed can not empty!");
                    return false;
                }

                if (MyUtility.Convert.GetDecimal(dr["LayerLowerBound"]) > MyUtility.Convert.GetDecimal(dr["LayerUpperBound"]))
                {
                    MyUtility.Msg.WarningBox("LayerLowerBound cannot be greater than LayerUpperBound.");
                    return false;
                }
            }
            #endregion

            #region 同WeaveTypeID有多筆時, LayerUpperBound(數字加1)與下一筆LayerLowerBound要連續
            string oldWeaveTypeID = string.Empty;
            int oldLayerUpperBound = -1;
            foreach (DataRow dr in this.DetailDatas.Where(r => r.RowState != DataRowState.Deleted)
                .OrderBy(o => o["LayerUpperBound"]).OrderBy(o => o["LayerLowerBound"]).OrderBy(o => o["WeaveTypeID"]))
            {
                if (oldWeaveTypeID == MyUtility.Convert.GetString(dr["WeaveTypeID"]) &&
                    oldLayerUpperBound + 1 != MyUtility.Convert.GetInt(dr["LayerLowerBound"]))
                {
                    MyUtility.Msg.WarningBox("Layers of the same WeaveType must be consecutive.");
                    return false;
                }

                oldLayerUpperBound = MyUtility.Convert.GetInt(dr["LayerUpperBound"]);
                oldWeaveTypeID = MyUtility.Convert.GetString(dr["WeaveTypeID"]);
            }
            #endregion

            return base.ClickSaveBefore();
        }
    }
}
