using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sci.Production.Subcon
{
    /// <inheritdoc/>
    public partial class P11_BatchImport : Win.Subs.Base
    {
        private DataTable dt_detail;
        private DataTable dtDetail;
        private Dictionary<string, string> selectedLocation = new Dictionary<string, string>();

        /// <inheritdoc/>
        public P11_BatchImport(DataTable detail)
        {
            this.InitializeComponent();
            this.dt_detail = detail;
            this.EditMode = true;
        }

        // Button Query
        private void BtnQuery_Click(object sender, EventArgs e) 
        {
            StringBuilder strSQLCmd = new StringBuilder();
            string sp1 = this.txtSPNo1.Text.TrimEnd();
            string sp2 = this.txtSPNo2.Text.TrimEnd();
            string po1 = this.txtPO1.Text.TrimEnd();
            string po2 = this.txtPO2.Text.TrimEnd();
            string style1 = this.txtStyle1.Text.TrimEnd();
            string style2 = this.txtStyle2.Text.TrimEnd();
            DateTime? buyerDel1 = this.dateBuyerDelivery.Value1;
            DateTime? buyerDel2 = this.dateBuyerDelivery.Value2;

            // SP#不可為空
            if (MyUtility.Check.Empty(sp1) && MyUtility.Check.Empty(sp2) &&
                MyUtility.Check.Empty(po1) && MyUtility.Check.Empty(po2) &&
                MyUtility.Check.Empty(style1) && MyUtility.Check.Empty(style2) &&
                MyUtility.Check.Empty(buyerDel1) && MyUtility.Check.Empty(buyerDel2))
            {
                MyUtility.Msg.WarningBox("SP#, Style#, PO#, Buyer Delivery cannot all be empty.");
                return;
            }

            strSQLCmd.Append(
$@"
select 
[Selected] = 0
,[OrderId] = o.ID
,[StyleID] = o.StyleID
,[ComboType] = iif(ol.Location is null, sl.Location,ol.Location)
,[Article] = oq.Article
,[OrderQty] = sum(oq.Qty)
,[OutputQty] = 0
,[UnitPrice] = 0
,[LocalCurrencyID] = ''
,[LocalUnitPrice] = 0
,[Vat] = 0
,[UPIncludeVAT] = 0
,[KpiRate] = 0

,[SewingCPU] = 0
,[CuttingCPU] = 0
,[InspectionCPU] = 0
,[OtherCPU] = 0
,[OtherAmt] = 0
,[EMBAmt] = 0
,[PrintingAmt] = 0
,[OtherPrice] = 0
,[EMBPrice] = 0
,[PrintingPrice] = 0
from Orders o
inner join Factory f on f.ID = o.FactoryID
inner join Order_Qty oq on o.ID = oq.ID
left join Order_Location ol on ol.OrderId = o.ID
left join Style_Location sl on sl.StyleUkey = o.StyleUkey
where 1=1
and f.IsSubcon = 1
and (o.Junk = 0 or (o.Junk = 1 and o.NeedProduction = 1))
and o.Category in ('B','S')
");
            if (!MyUtility.Check.Empty(sp1))
            {
                strSQLCmd.Append($@" and o.ID >= '{sp1}'");
            }

            if (!MyUtility.Check.Empty(sp2))
            {
                strSQLCmd.Append($@" and o.ID <= '{sp2}'");
            }

            if (!MyUtility.Check.Empty(po1))
            {
                strSQLCmd.Append($@" and o.CustPONo >= '{po1}'");
            }

            if (!MyUtility.Check.Empty(po2))
            {
                strSQLCmd.Append($@" and o.CustPONo <= '{po2}'");
            }

            if (!MyUtility.Check.Empty(style1))
            {
                strSQLCmd.Append($@" and o.StyleID >= '{style1}'");
            }

            if (!MyUtility.Check.Empty(style2))
            {
                strSQLCmd.Append($@" and o.StyleID <= '{style2}'");
            }

            strSQLCmd.Append(Environment.NewLine + @"group by o.ID,o.StyleID,iif(ol.Location is null, sl.Location,ol.Location),oq.Article");

            this.ShowWaitMessage("Data Loading....");
            DualResult result;
            if (!(result = DBProxy.Current.Select(null, strSQLCmd.ToString(), out this.dtDetail)))
            {
                this.ShowErr(strSQLCmd.ToString(), result);
            }
            else
            {
                if (this.dtDetail.Rows.Count == 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!!");
                }

                this.listControlBindingSource1.DataSource = this.dtDetail;
            }

            this.HideWaitMessage();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.txtSPNo1.Focus();

            #region
            DataGridViewGeneratorNumericColumnSettings localUnitPrice = new DataGridViewGeneratorNumericColumnSettings();
            localUnitPrice.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                dr["LocalUnitPrice"] = e.FormattedValue;
                dr["UPIncludeVAT"] = MyUtility.Convert.GetDecimal(dr["LocalUnitPrice"]) + MyUtility.Convert.GetDecimal(dr["Vat"]);
                dr.EndEdit();
            };

            DataGridViewGeneratorNumericColumnSettings vat = new DataGridViewGeneratorNumericColumnSettings();
            vat.CellValidating += (s, e) =>
            {
                DataRow dr = this.gridImport.GetDataRow(e.RowIndex);
                dr["Vat"] = e.FormattedValue;
                dr["UPIncludeVAT"] = MyUtility.Convert.GetDecimal(dr["LocalUnitPrice"]) + MyUtility.Convert.GetDecimal(dr["Vat"]);
                dr.EndEdit();
            };
            #endregion

            this.gridImport.IsEditingReadOnly = false; // 必設定, 否則CheckBox會顯示圖示
            this.gridImport.DataSource = this.listControlBindingSource1;
            this.Helper.Controls.Grid.Generator(this.gridImport)
                .CheckBox("Selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0)
                .Text("OrderId", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(6), iseditingreadonly: true)
                .Text("ComboType", header: "Combo Type", width: Widths.AnsiChars(9), iseditingreadonly: true)
                .Text("Article", header: "Article", width: Widths.AnsiChars(8), iseditingreadonly: true)
                .Numeric("OrderQty", header: "Order Qty", width: Widths.AnsiChars(10), iseditingreadonly: true)
                .Numeric("OutputQty", header: "Subcon Out Qty", width: Widths.AnsiChars(10), iseditingreadonly: false)
                .Numeric("UnitPrice", header: "Price(Unit)", width: Widths.AnsiChars(10), integer_places: 12, decimal_places: 4)
                .Text("LocalCurrencyID", header: "Currency", width: Widths.AnsiChars(3))
                .Numeric("LocalUnitPrice", header: "U/P Exclude VAT(Local currency)", width: Widths.AnsiChars(12), decimal_places: 4, settings: localUnitPrice)
                .Numeric("Vat", header: "VAT (Local currency)", width: Widths.AnsiChars(10), decimal_places: 2, settings: vat)
                .Numeric("UPIncludeVAT", header: "U/P Include VAT(Local currency)", width: Widths.AnsiChars(10), decimal_places: 4, iseditingreadonly: true)
                .Numeric("KpiRate", header: "Kpi Rate", width: Widths.AnsiChars(3), maximum: 9, decimal_places: 2)
            ;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
            this.gridImport.ValidateControl();
            DataTable dtGridBS1 = (DataTable)this.listControlBindingSource1.DataSource;
            if (MyUtility.Check.Empty(dtGridBS1) || dtGridBS1.Rows.Count == 0)
            {
                return;
            }

            DataRow[] dr2 = dtGridBS1.Select("Selected = 1");

            if (dr2.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select rows first!", "Warnning");
                return;
            }

            foreach (DataRow tmp in dr2)
            {
                if (tmp.RowState != DataRowState.Deleted)
                {
                    DataRow[] findrow = this.dt_detail.AsEnumerable().Where(
                    row => row.RowState != DataRowState.Deleted &&
                    row["OrderID"].EqualString(tmp["OrderID"].ToString()) &&
                    row["ComboType"].EqualString(tmp["ComboType"].ToString()) &&
                    row["Article"].EqualString(tmp["Article"].ToString())
                    ).ToArray();

                    if (findrow.Length > 0)
                    {
                        findrow[0]["OrderQty"] = tmp["OrderQty"];
                        findrow[0]["OutputQty"] = tmp["OutputQty"];
                        findrow[0]["UnitPrice"] = tmp["UnitPrice"];
                        findrow[0]["LocalCurrencyID"] = tmp["LocalCurrencyID"];
                        findrow[0]["LocalUnitPrice"] = tmp["LocalUnitPrice"];
                        findrow[0]["Vat"] = tmp["Vat"];
                        findrow[0]["UPIncludeVAT"] = tmp["UPIncludeVAT"];
                        findrow[0]["KpiRate"] = tmp["KpiRate"];
                    }
                    else
                    {
                        tmp.AcceptChanges();
                        tmp.SetAdded();
                        this.dt_detail.ImportRow(tmp);
                    }
                }
            }

            this.Close();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            this.listControlBindingSource1.EndEdit();
            DataTable dt = (DataTable)this.listControlBindingSource1.DataSource;
            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            DataRow[] drfound = dt.Select("selected = 1");

            foreach (var item in drfound)
            {
                switch (this.comboOrderBy.SelectedItem)
                {
                    case "Price(Unit)":
                        item["UnitPrice"] = this.txtBatchUpdate.Text;
                        break;
                    case "Currency":
                        item["LocalCurrencyID"] = this.txtBatchUpdate.Text;
                        break;
                    case "U/P Exclude VAT(Local currency)":
                        item["LocalUnitPrice"] = this.txtBatchUpdate.Text;
                        break;
                    case "VAT(Local currency)":
                        item["Vat"] = this.txtBatchUpdate.Text;
                        break;
                    case "Kpi Rate":
                        item["KpiRate"] = this.txtBatchUpdate.Text;
                        break;
                }
            }
        }
    }
}
