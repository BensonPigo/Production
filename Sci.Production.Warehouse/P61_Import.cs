using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ict;
using Ict.Win;
using Sci;
using Sci.Data;
using System.Linq;
using System.Data.SqlClient;

namespace Sci.Production.Warehouse
{
    public partial class P61_Import : Sci.Win.Subs.Base
    {
        Ict.Win.UI.DataGridViewCheckBoxColumn col_chk;
        DataRow P61;
        DataTable P61_Detail; 
        public P61_Import(DataRow master, DataTable detail)
        {
            InitializeComponent();
            P61 = master;
            P61_Detail = detail;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region CheckBox = true
            this.grid1.CellValueChanged += (s, e) =>
            {
                if (grid1.Columns[e.ColumnIndex].Name == col_chk.Name)
                {
                    DataRow dr = grid1.GetDataRow(e.RowIndex);
                    if (Convert.ToBoolean(dr["Selected"]) == true && Convert.ToDecimal(dr["Qty"].ToString()) == 0)
                    {
                        dr["Qty"] = dr["stockQty"];
                    } else if (Convert.ToBoolean(dr["Selected"]) == false)
                    {
                        dr["Qty"] = 0;
                    }
                    dr.EndEdit();
                }
            };
            #endregion 
            #region IssueQty != 0
            Ict.Win.DataGridViewGeneratorNumericColumnSettings setQty = new DataGridViewGeneratorNumericColumnSettings();
            setQty.CellValidating = (s, e) =>
            {
                DataRow dr = grid1.GetDataRow(e.RowIndex);
                dr["Qty"] = e.FormattedValue;
                dr["Selected"] = (Convert.ToDecimal(e.FormattedValue) != 0);
                dr.EndEdit();
            };
            #endregion 
            #region Set Grid
            this.grid1.IsEditingReadOnly = false;
            Helper.Controls.Grid.Generator(this.grid1)
                .CheckBox("Selected", header: "", width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0).Get(out col_chk)
                .Text("Refno", header: "Refno", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("ThreadColorID", header: "ThreadColor", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("unit", header: "Unit", iseditingreadonly: true)
                .Numeric("stockQty", header: "Stock Qty", iseditingreadonly: true)
                .Numeric("Qty", header: "Issue Qty", iseditingreadonly: false, minimum: -999999, settings: setQty)
                .EditText("desc", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true);
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
	from LocalInventory Linv
	left join LocalItem LItem on Linv.Refno = LItem.RefNo
    Where Linv.OrderID = @SP
) as s
where s.StockQty >= 0
order by s.Refno, s.ThreadColorID, s.StockQty, s.[Desc]
";

            List<SqlParameter> listPar = new List<SqlParameter>();
            listPar.Add(new SqlParameter("@SP", txtSP.Text.Trim()));
            #endregion 
            #region SQL Data Loading...
            Ict.DualResult result;
            DataTable dataTable;

            if (result = DBProxy.Current.Select(null, strSql, listPar, out dataTable))
            {
                if (dataTable == null || dataTable.Rows.Count == 0)
                {
                    MyUtility.Msg.InfoBox("Data not found!!");
                }
                listControlBindingSource1.DataSource = dataTable;
            }
            else
            {
                ShowErr(strSql, result);
            }
            #endregion 
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            grid1.ValidateControl();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dt)) return;
            DataRow[] dataRow = this.grid1.GetTable().Select("Selected = 1");
            foreach (DataRow dr in dataRow)
            {
                DataRow[] checkDR = P61_Detail.AsEnumerable().Where(row => row.RowState != DataRowState.Deleted && row["OrderID"].EqualString(dr["OrderID"])
                                                                        && row["Refno"].EqualString(dr["Refno"]) && row["ThreadColorID"].EqualString(dr["ThreadColorID"])
                                                                  ).ToArray();
                if (checkDR.Length > 0)
                {
                    checkDR[0]["Qty"] = dr["Qty"];
                }
                else
                {
                    dr["ID"] = P61["ID"];
                    dr.AcceptChanges();
                    dr.SetAdded();
                    P61_Detail.ImportRow(dr);
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
            if (checkBalance.Checked)
            {
                this.listControlBindingSource1.Filter = "StockQty > 0";
            }
            else
            {
                this.listControlBindingSource1.Filter = "";
            }
        }
    }
}
