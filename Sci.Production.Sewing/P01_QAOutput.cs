using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using System.Linq;
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

        protected override void OnEditModeChanged()
        {
            base.OnEditModeChanged();
        }

        protected override bool OnSaveBefore()
        {
            grid.ValidateControl();
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
        
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            
            prev.Visible = false;
            next.Visible = false;
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            string strFilter = string.Format("OrderID = '{0}' and ComboType = '{1}' and Article = '{2}'", CurrentDetailData["OrderID"]
                                                                                                        , CurrentDetailData["ComboType"]
                                                                                                        , CurrentDetailData["Article"]);
            gridbs.Filter = strFilter;
            displaySPNo.Value = MyUtility.Convert.GetString(CurrentDetailData["OrderID"]);
            displaySPNo2.Value = MyUtility.Convert.GetString(CurrentDetailData["ComboType"]);
            displayArticle.Value = MyUtility.Convert.GetString(CurrentDetailData["Article"]);
            displayColor.Value = MyUtility.Convert.GetString(CurrentDetailData["Color"]);
            numTotalOrderQty.Value = MyUtility.Convert.GetInt(((DataTable)gridbs.DataSource).Compute("SUM(OrderQty)", strFilter));
            numTotalAccumQty.Value = MyUtility.Convert.GetInt(((DataTable)gridbs.DataSource).Compute("SUM(AccumQty)", strFilter));
            numTotalVariance.Value = MyUtility.Convert.GetInt(((DataTable)gridbs.DataSource).Compute("SUM(Variance)", strFilter));
            CalculateTotal();
            //this.grid.AutoResizeColumns();
        }

        protected override bool OnGridSetup()
        {
            #region QA Q'ty的Validatng
            qaqty.CellValidating += (s, e) =>
            {
                if (this.EditMode)
                {                    
                    DataRow dr = grid.GetDataRow<DataRow>(e.RowIndex);
                    if (MyUtility.Convert.GetInt(e.FormattedValue) == MyUtility.Convert.GetInt(dr["QAQty"])) return;                   
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
            string strFilter = string.Format("OrderID = '{0}' and ComboType = '{1}' and Article = '{2}'", CurrentDetailData["OrderID"]
                                                                                                        , CurrentDetailData["ComboType"]
                                                                                                        , CurrentDetailData["Article"]);
            numTotalQAQty.Value = MyUtility.Convert.GetInt(((DataTable)gridbs.DataSource).Compute("SUM(QAQty)", strFilter));
            numTotalBalQty.Value = MyUtility.Convert.GetInt(((DataTable)gridbs.DataSource).Compute("SUM(BalQty)", strFilter));
        }
    }
}
