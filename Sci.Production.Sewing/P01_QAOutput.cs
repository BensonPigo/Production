using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;

namespace Sci.Production.Sewing
{
    public partial class P01_QAOutput : Sci.Win.Subs.Input8A
    {
        Ict.Win.DataGridViewGeneratorNumericColumnSettings qaqty = new Ict.Win.DataGridViewGeneratorNumericColumnSettings();
        public P01_QAOutput()
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            IsSupportDelete = false;
            IsSupportNew = false;
            IsSupportUpdate = false;
            append.Visible = false;
            revise.Visible = false;
            delete.Visible = false;
            prev.Visible = false;
            next.Visible = false;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            displayBox1.Value = MyUtility.Convert.GetString(CurrentDetailData["OrderID"]);
            displayBox2.Value = MyUtility.Convert.GetString(CurrentDetailData["ComboType"]);
            displayBox3.Value = MyUtility.Convert.GetString(CurrentDetailData["Article"]);
            displayBox4.Value = MyUtility.Convert.GetString(CurrentDetailData["Color"]);
            numericBox1.Value = MyUtility.Convert.GetInt(((DataTable)gridbs.DataSource).Compute("SUM(OrderQty)", ""));
            numericBox2.Value = MyUtility.Convert.GetInt(((DataTable)gridbs.DataSource).Compute("SUM(AccumQty)", ""));
            numericBox3.Value = MyUtility.Convert.GetInt(((DataTable)gridbs.DataSource).Compute("SUM(Variance)", ""));
            CalculateTotal();

            //528: SEWING_P01_QAOutput_QA Output，QA QTY無法輸入
            this.EditMode = true;
            this.grid.IsEditingReadOnly = !this.EditMode;
        }

        protected override bool OnGridSetup()
        {
            #region QA Q'ty的Validatng
            qaqty.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {
                    DataRow dr = grid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Convert.GetInt(e.FormattedValue) != MyUtility.Convert.GetInt(dr["QAQty"]))
                    {
                        if (MyUtility.Convert.GetInt(e.FormattedValue) > MyUtility.Convert.GetInt(dr["Variance"]))
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
                        CalculateTotal();
                    }
                }
            };
            #endregion
            Helper.Controls.Grid.Generator(grid)
                .Text("SizeCode", header: "Size", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("OrderQty", header: "Order Q'ty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("AccumQty", header: "Accum. Q'ty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("Variance", header: "Variance", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("QAQty", header: "QA Q'ty", width: Widths.AnsiChars(10), iseditingreadonly:false, settings: qaqty)
                .Numeric("BalQty", header: "Bal. Q'ty", width: Widths.AnsiChars(10), iseditingreadonly: true);

            for (int i = 0; i < grid.ColumnCount; i++)
            {
                grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            return true;
        }

        //計算Total QA Q'ty, Total Bal. Q'ty
        private void CalculateTotal()
        {
            numericBox4.Value = MyUtility.Convert.GetInt(((DataTable)gridbs.DataSource).Compute("SUM(QAQty)", ""));
            numericBox5.Value = MyUtility.Convert.GetInt(((DataTable)gridbs.DataSource).Compute("SUM(BalQty)", ""));
        }
    }
}
