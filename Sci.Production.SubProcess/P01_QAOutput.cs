using System.Data;
using System.Windows.Forms;
using Ict.Win;
using Ict;

namespace Sci.Production.SubProcess
{
    /// <summary>
    /// P01_QAOutput
    /// </summary>
    public partial class P01_QAOutput : Win.Subs.Input8A
    {
        /// <summary>
        /// P01_QAOutput
        /// </summary>
        public P01_QAOutput()
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            this.prev.Visible = false;
            this.next.Visible = false;
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
        protected override void OnAttached()
        {
            base.OnAttached();
            string strFilter = string.Format("OrderID = '{0}' and Article = '{1}' ", this.CurrentDetailData["OrderID"], this.CurrentDetailData["Article"]);

            this.gridbs.Filter = strFilter;
            this.displaySPNo.Value = MyUtility.Convert.GetString(this.CurrentDetailData["OrderID"]);
            this.displayArticle.Value = MyUtility.Convert.GetString(this.CurrentDetailData["Article"]);
            this.displayColor.Value = MyUtility.Convert.GetString(this.CurrentDetailData["Color"]);
            this.numTotalOrderQty.Value = MyUtility.Convert.GetInt(((DataTable)this.gridbs.DataSource).Compute("SUM(OrderQty)", strFilter));
            this.numTotalAccumQty.Value = MyUtility.Convert.GetInt(((DataTable)this.gridbs.DataSource).Compute("SUM(AccumQty)", strFilter));
            this.numTotalVariance.Value = MyUtility.Convert.GetInt(((DataTable)this.gridbs.DataSource).Compute("SUM(Variance)", strFilter));
            this.CalculateTotal();
        }

        /// <inheritdoc/>
        protected override bool OnGridSetup()
        {
            // QA Q'ty的Validatng
            DataGridViewGeneratorNumericColumnSettings qaqty = new DataGridViewGeneratorNumericColumnSettings();
            qaqty.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = this.grid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Convert.GetInt(e.FormattedValue) == MyUtility.Convert.GetInt(dr["QAQty"]))
                    {
                        return;
                    }

                    if (MyUtility.Convert.GetInt(e.FormattedValue) > MyUtility.Convert.GetInt(dr["Variance"]))
                    {
                        MyUtility.Msg.WarningBox("QA QTY can not exceed Variance !!");
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

            this.Helper.Controls.Grid.Generator(this.grid)
            .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
            .Numeric("OrderQty", header: "Order Q'ty", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("AccumQty", header: "Accum. Q'ty", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("Variance", header: "Variance", width: Widths.AnsiChars(10), iseditingreadonly: true)
            .Numeric("QAQty", header: "QA Q'ty", width: Widths.AnsiChars(10), iseditingreadonly: false, settings: qaqty)
            .Numeric("BalQty", header: "Bal. Q'ty", width: Widths.AnsiChars(10), iseditingreadonly: true);

            #region 關閉排序功能
            for (int i = 0; i < this.grid.ColumnCount; i++)
            {
                this.grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            #endregion

            return true;
        }

        /// <inheritdoc/>
        protected override bool OnUndo()
        {
            DialogResult redult;
            bool flag = true;
            foreach (DataRow item in ((DataTable)this.gridbs.DataSource).Rows)
            {
                if (item.RowState == DataRowState.Modified)
                {
                    flag = false;
                }

                if (item.RowState == DataRowState.Added)
                {
                    flag = false;
                }
            }

            if (!flag)
            {
                redult = MyUtility.Msg.QuestionBox("Data has been modified. Comfirm that you want to cancel the change?");
                if (redult == DialogResult.No)
                {
                    return false;
                }
            }

            return base.OnUndo();
        }

        // 計算Total QA Q'ty, Total Bal. Q'ty
        private void CalculateTotal()
        {
            string strFilter = string.Format("OrderID = '{0}' and Article = '{1}'", this.CurrentDetailData["OrderID"], this.CurrentDetailData["Article"]);
            this.numTotalQAQty.Value = MyUtility.Convert.GetInt(((DataTable)this.gridbs.DataSource).Compute("SUM(QAQty)", strFilter));
            this.numTotalBalQty.Value = MyUtility.Convert.GetInt(((DataTable)this.gridbs.DataSource).Compute("SUM(BalQty)", strFilter));
        }
    }
}
