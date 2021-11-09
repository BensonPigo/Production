using System.Data;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using System.Linq;

namespace Sci.Production.Sewing
{
    /// <summary>
    /// P01_QAOutput
    /// </summary>
    public partial class P01_QAOutput : Win.Subs.Input8A
    {
        private DataGridViewGeneratorNumericColumnSettings qaqty = new DataGridViewGeneratorNumericColumnSettings();
        P01 p01;

        /// <summary>
        /// P01_QAOutput
        /// </summary>
        /// <param name="p01">p01</param>
        public P01_QAOutput(P01 p01)
        {
            this.InitializeComponent();
            this.p01 = p01;
        }

        /// <inheritdoc/>
        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
        }

        /// <inheritdoc/>
        protected override bool OnSaveBefore()
        {
            this.grid.ValidateControl();

            // 經由[月解鎖]的資料在點選[QA Output]後, 改為可以修正[QA Qty], 但點選存檔時, [QA Qty]的總數需要與原本的相同
            // 如不相同則不可存檔顯示以下訊息並return,
            // [Ttl.QA Qty] Can’t be different from the original.
            if (this.p01.IsUnlockFromMonthLock)
            {
                int subOutputQty = this.CurrentSubDetailDatas.AsEnumerable().Sum(s => MyUtility.Convert.GetInt(s["QAQty"]));
                if (subOutputQty != MyUtility.Convert.GetInt(this.CurrentDetailData["QAQty"]))
                {
                    MyUtility.Msg.WarningBox("[Ttl. QA Qty] Can’t be different from the original.");
                    return false;
                }
            }

            foreach (DataRow row in this.CurrentSubDetailDatas.Rows)
            {
                if (row.RowState != DataRowState.Deleted)
                {
                    if (row["ID"].ToString().Empty())
                    {
                        row["ID"] = this.CurrentDetailData["ID"];
                    }
                }
            }

            return base.OnSaveBefore();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.prev.Visible = false;
            this.next.Visible = false;
        }

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();
            string strFilter = string.Format(
                "OrderID = '{0}' and ComboType = '{1}' and Article = '{2}'",
                this.CurrentDetailData["OrderID"],
                this.CurrentDetailData["ComboType"],
                this.CurrentDetailData["Article"]);

            this.gridbs.Filter = strFilter;
            this.displaySPNo.Value = MyUtility.Convert.GetString(this.CurrentDetailData["OrderID"]);
            this.displaySPNo2.Value = MyUtility.Convert.GetString(this.CurrentDetailData["ComboType"]);
            this.displayArticle.Value = MyUtility.Convert.GetString(this.CurrentDetailData["Article"]);
            this.displayColor.Value = MyUtility.Convert.GetString(this.CurrentDetailData["Color"]);
            this.numTotalOrderQty.Value = MyUtility.Convert.GetInt(((DataTable)this.gridbs.DataSource).Compute("SUM(OrderQty)", strFilter));
            this.numTotalAccumQty.Value = MyUtility.Convert.GetInt(((DataTable)this.gridbs.DataSource).Compute("SUM(AccumQty)", strFilter));
            this.numTotalVariance.Value = MyUtility.Convert.GetInt(((DataTable)this.gridbs.DataSource).Compute("SUM(Variance)", strFilter));
            this.CalculateTotal();

            // this.grid.AutoResizeColumns();
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            #region QA Q'ty的Validatng
            this.qaqty.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Convert.GetInt(e.FormattedValue) == MyUtility.Convert.GetInt(dr["QAQty"]))
                    {
                        return;
                    }

                    if (MyUtility.Convert.GetDecimal(e.FormattedValue) > MyUtility.Convert.GetDecimal(dr["OrderQtyUpperlimit"]) - MyUtility.Convert.GetDecimal(dr["AccumQty"]))
                    {
                        MyUtility.Msg.WarningBox("< QA Q'ty > can't exceed < Variance >");
                        dr["QAQty"] = 0;
                        e.Cancel = true;
                    }
                    else
                    {
                        if (MyUtility.Convert.GetInt(dr["QAQty"]) == 0 && (dr.RowState != DataRowState.Added && dr.RowState == DataRowState.Unchanged))
                        {
                            dr.SetAdded();
                        }

                        dr["QAQty"] = e.FormattedValue;
                    }

                    dr["BalQty"] = MyUtility.Convert.GetInt(dr["Variance"]) - MyUtility.Convert.GetInt(dr["QAQty"]);
                    dr.EndEdit();
                    this.CalculateTotal();
                }
            };
            #endregion
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("OrderQty", header: "Order Q'ty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("AccumQty", header: "Accum. Q'ty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Variance", header: "Variance", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("QAQty", header: "QA Q'ty", width: Widths.AnsiChars(10), iseditingreadonly: false, settings: this.qaqty)
                .Numeric("BalQty", header: "Bal. Q'ty", width: Widths.AnsiChars(10), iseditingreadonly: true);

            for (int i = 0; i < this.grid.ColumnCount; i++)
            {
                this.grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            return true;
        }

        // 計算Total QA Q'ty, Total Bal. Q'ty
        private void CalculateTotal()
        {
            string strFilter = string.Format(
                "OrderID = '{0}' and ComboType = '{1}' and Article = '{2}'",
                this.CurrentDetailData["OrderID"],
                this.CurrentDetailData["ComboType"],
                this.CurrentDetailData["Article"]);

            this.numTotalQAQty.Value = MyUtility.Convert.GetInt(((DataTable)this.gridbs.DataSource).Compute("SUM(QAQty)", strFilter));
            this.numTotalBalQty.Value = MyUtility.Convert.GetInt(((DataTable)this.gridbs.DataSource).Compute("SUM(BalQty)", strFilter));
        }
    }
}
