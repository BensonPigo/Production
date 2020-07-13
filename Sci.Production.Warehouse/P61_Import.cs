using System;
using System.Collections.Generic;
using System.Data;
using Ict;
using Ict.Win;
using Sci.Data;
using System.Linq;
using System.Data.SqlClient;

namespace Sci.Production.Warehouse
{
    public partial class P61_Import : Win.Subs.Base
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        DataRow P61;
        DataTable P61_Detail;

        public P61_Import(DataRow master, DataTable detail)
        {
            this.InitializeComponent();
            this.P61 = master;
            this.P61_Detail = detail;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region CheckBox = true
            this.gridImport.CellValueChanged += (s, e) =>
            {
                if (this.gridImport.Columns[e.ColumnIndex].Name == this.col_chk.Name)
                {
                    DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                    if (Convert.ToBoolean(dr["Selected"]) == true && Convert.ToDecimal(dr["Qty"].ToString()) == 0)
                    {
                        dr["Qty"] = dr["stockQty"];
                    }
                    else if (Convert.ToBoolean(dr["Selected"]) == false)
                    {
                        dr["Qty"] = 0;
                    }

                    dr.EndEdit();
                }
            };
            #endregion
            #region IssueQty != 0
            DataGridViewGeneratorNumericColumnSettings setQty = new DataGridViewGeneratorNumericColumnSettings();
            setQty.IsSupportNegative = true;
            setQty.CellValidating = (s, e) =>
            {
                DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                dr["Qty"] = e.FormattedValue;
                dr["Selected"] = Convert.ToDecimal(e.FormattedValue) != 0;
                dr.EndEdit();
            };
            #endregion
            #region Set Grid
            this.gridImport.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out this.col_chk)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("ThreadColorID", header: "ThreadColor", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("unit", header: "Unit", iseditingreadonly: true)
                .Numeric("stockQty", header: "Stock Qty", iseditingreadonly: true)
                .Numeric("Qty", header: "Issue Qty", iseditingreadonly: false, settings: setQty)
                .EditText("desc", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .EditText("Location", header: "Bulk Location", width: Widths.AnsiChars(10), iseditingreadonly: true);
            #endregion
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            #region SqlCommand & SqlParameter
            string strSql = @"
select	selected = 0
		, Qty = 0
		, *
        , ID = ''
from (
	select	Linv.OrderID
            , Refno = Ltrim(Rtrim(Linv.Refno))
			, ThreadColorID = Ltrim(Rtrim(Linv.ThreadColorID))
			, unit = Linv.UnitId
			, StockQty = Linv.InQty - Linv.OutQty + Linv.AdjustQty
			, [Desc] = LItem.Description
            , Location = Linv.ALocation
	from LocalInventory Linv
	left join LocalItem LItem on Linv.Refno = LItem.RefNo
    inner join dbo.Orders on Linv.OrderID = orders.id
    inner join dbo.Factory on orders.FactoryID = factory.ID
    Where Linv.OrderID = @SP
    and factory.MDivisionID = @M
) as s
where s.StockQty >= 0
order by s.Refno, s.ThreadColorID, s.StockQty, s.[Desc]
";

            List<SqlParameter> listPar = new List<SqlParameter>();
            listPar.Add(new SqlParameter("@SP", this.txtSPNo.Text.Trim()));
            listPar.Add(new SqlParameter("@M", Env.User.Keyword));
            #endregion
            #region SQL Data Loading...
            DualResult result;
            DataTable dataTable;

            if (result = DBProxy.Current.Select(null, strSql, listPar, out dataTable))
            {
                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("Data not found!!");
                }

                this.listControlBindingSource1.DataSource = dataTable;
            }
            else
            {
                this.ShowErr(strSql, result);
            }
            #endregion
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            this.gridImport.ValidateControl();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dt))
            {
                return;
            }

            DataRow[] dataRow = this.gridImport.GetTable().Select("Selected = 1");
            foreach (DataRow dr in dataRow)
            {
                DataRow[] checkDR = this.P61_Detail.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted && row["OrderID"].EqualString(dr["OrderID"])
                                                                        && row["Refno"].EqualString(dr["Refno"]) && row["ThreadColorID"].EqualString(dr["ThreadColorID"]))
                                                                  .ToArray();
                if (checkDR.Length > 0)
                {
                    checkDR[0]["Qty"] = dr["Qty"];
                }
                else
                {
                    dr["ID"] = this.P61["ID"];
                    dr.AcceptChanges();
                    dr.SetAdded();
                    this.P61_Detail.ImportRow(dr);
                }
            }

            this.Close();
        }

        private void btnCanel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBalance_Click(object sender, EventArgs e)
        {
            if (this.checkBalance.Checked)
            {
                this.listControlBindingSource1.Filter = "StockQty > 0";
            }
            else
            {
                this.listControlBindingSource1.Filter = string.Empty;
            }
        }
    }
}
