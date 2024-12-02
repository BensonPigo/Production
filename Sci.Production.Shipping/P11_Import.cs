using Ict;
using Ict.Win;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class P11_Import : Sci.Win.Subs.Base
    {
        private string masterID;
        private DateTime addDate;
        private DataTable dt_detail;

        /// <inheritdoc/>
        public P11_Import(string masterID, DateTime addDate, DataTable detail)
        {
            this.InitializeComponent();
            this.masterID = masterID;
            this.addDate = addDate;
            this.dt_detail = detail;
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.txtBrand.MultiSelect = true;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();

            DataGridViewGeneratorCheckBoxColumnSettings col_sel = new DataGridViewGeneratorCheckBoxColumnSettings();
            col_sel.CellValidating += (s, e) =>
            {
                this.gridGMTBooking.EndEdit();
                this.CountSelectQty();
            };

            this.gridGMTBooking.DataSource = this.gridGMTbs;
            this.gridGMTBooking.IsEditingReadOnly = false;
            this.Helper.Controls.Grid.Generator(this.gridGMTBooking)
               .CheckBox("selected", header: string.Empty, width: Widths.AnsiChars(3), iseditable: true, trueValue: 1, falseValue: 0, settings: col_sel)
               .Text("InvNo", header: "GB#", width: Widths.AnsiChars(20), iseditingreadonly: true)
               .Text("BrandID", header: "Brand", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Date("ETD", header: "On Board Date", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("ETA", header: "ETA", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Numeric("TotalShipQty", header: "Q'ty (Pcs)", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(15), iseditingreadonly: true)
               ;
        }

        private void LoadData()
        {
            if (MyUtility.Check.Empty(this.dateETD.Value1) && MyUtility.Check.Empty(this.dateETD.Value2))
            {
                MyUtility.Msg.WarningBox("Please input <On Board Date>");
                return;
            }

            // [ISP20241007] 已報帳資料不更新
            string isNewData = this.addDate >= new DateTime(2024, 11, 05) ? "1" : "0";

            string where = string.Empty;

            #region Where

            if (!MyUtility.Check.Empty(this.dateETD.Value1))
            {
                where += $@" and GB.ETD >= '{((DateTime)this.dateETD.Value1).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.dateETD.Value2))
            {
                where += $@" and GB.ETD <= '{((DateTime)this.dateETD.Value2).ToString("yyyy/MM/dd")}'";
            }

            if (!MyUtility.Check.Empty(this.txGBFrom.Text))
            {
                where += $@" and GB.ID >= '{this.txGBFrom.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtGBTo.Text))
            {
                where += $@" and GB.ID <= '{this.txtGBTo.Text}'";
            }

            if (!MyUtility.Check.Empty(this.txtBrand.Text))
            {
                where += $@"and GB.BrandID in ({this.txtBrand.Text.Split(',').Select(s => $"'{s}'").JoinToString(",")})";
            }

            if (!MyUtility.Check.Empty(this.txtShipper.Text))
            {
                where += $@" and GB.Shipper  = '{this.txtShipper.Text}'";
            }

            #endregion

            string sqlcmd = $@"
select	o.ID,
        o.CustPONo,
        o.StyleID,
		[UnitPriceUSD] = ((isnull(o.CPU, 0) + isnull(SubProcessCPU.val, 0)) * isnull(CpuCost.val, 0)) + isnull(SubProcessAMT.val, 0) + isnull(LocalPurchase.val, 0)
into #tmpUnitPriceUSD
from Orders o with (nolock)
left join Factory f with (nolock) on f.ID = o.FactoryID
outer apply (select [val] = sum(Isnull(Price,0)) from GetSubProcessDetailByOrderID(o.ID,'CPU')) SubProcessCPU
outer apply (select [val] = sum(Isnull(Price,0)) from GetSubProcessDetailByOrderID(o.ID,'AMT')) SubProcessAMT
outer apply (
    select top 1 [val] = fd.CpuCost
    from FtyShipper_Detail fsd WITH (NOLOCK) , FSRCpuCost_Detail fd WITH (NOLOCK) 
    where fsd.BrandID = o.BrandID
    and fsd.FactoryID = o.FactoryID
    and o.OrigBuyerDelivery between fsd.BeginDate and fsd.EndDate
    and fsd.ShipperID = fd.ShipperID
    and o.OrigBuyerDelivery between fd.BeginDate and fd.EndDate
	and (fsd.SeasonID = o.SeasonID or fsd.SeasonID = '')
    and fd.OrderCompanyID = o.OrderCompanyID
	order by SeasonID desc
) CpuCost
outer apply (select [val] = iif(f.LocalCMT = 1 and {isNewData} = 1, dbo.GetLocalPurchaseStdCost(o.ID), 0)) LocalPurchase
where exists (select 1 
			  from PackingList p with (nolock)
			  inner join GMTBooking GB with (nolock) on GB.ID = p.INVNo
			  inner join PackingList_Detail pd with (nolock) on p.ID = pd.ID
			  where  1=1
			  {where}
			  and pd.OrderID = o.ID
			  )



SELECT  0 as [selected], 
        [ID] = '{this.masterID}',
        [InvNo] = GB.ID,
        GB.BrandID,
        GB.ETD,
        GB.ETA,
        GB.TotalShipQty,
		UnitPriceUSD.Amount
FROM GMTBooking GB with (nolock)
outer apply(
    select Amount = sum(Amount)
    from(
	    select Amount = t.UnitPriceUSD * sum(pd.ShipQty)
	    from PackingList p
	    inner join PackingList_Detail pd on pd.ID = p.ID
        inner join Orders o on o.id = pd.OrderID
	    inner join #tmpUnitPriceUSD t on t.ID = pd.OrderID and t.CustPONo = o.CustPONo and t.StyleID = o.StyleID 
	    where p.INVNo = gb.ID
        group by UnitPriceUSD
    )ttl
)UnitPriceUSD
WHERE GB.ETD IS NOT NULL
AND GB.[Status]='Confirmed'
AND NOT Exists (SELECT 1 FROM BIRInvoice_Detail bd with (nolock) where GB.ID = bd.InvNo)
{where}
and not exists(
    select 1 from #tmp t
    where GB.ID = t.InvNo
)
ORDER BY GB.ETD


drop table #tmpUnitPriceUSD
";

            DualResult result;

            if (result = MyUtility.Tool.ProcessWithDatatable(this.dt_detail, string.Empty, sqlcmd,  out DataTable dt))
            {
                if (dt != null && dt.Rows.Count <= 0)
                {
                    MyUtility.Msg.WarningBox("Data not found!");
                }

                this.gridGMTbs.DataSource = dt;
            }
            else
            {
                this.ShowErr(sqlcmd, result);
            }
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            this.gridGMTBooking.ValidateControl();
            DataTable dtGridBS = (DataTable)this.gridGMTbs.DataSource;
            if (MyUtility.Check.Empty(dtGridBS) || dtGridBS.Rows.Count == 0)
            {
                return;
            }

            DataRow[] drSelected = dtGridBS.Select("Selected = 1");
            if (drSelected.Length == 0)
            {
                MyUtility.Msg.WarningBox("Please select at least one.", "Warnning");
                return;
            }

            foreach (DataRow tmp in drSelected)
            {
                bool isExists = this.dt_detail.AsEnumerable()
                    .Where(row => row.RowState != DataRowState.Deleted &&
                    row["InvNo"].EqualString(tmp["InvNo"].ToString())).Any();

                if (!isExists)
                {
                    this.dt_detail.ImportRowAdded(tmp);
                }
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnQuery_Click(object sender, EventArgs e)
        {
            this.LoadData();
        }

        private void CountSelectQty()
        {
            if (this.gridGMTbs.DataSource == null)
            {
                return;
            }

            var upd_list = ((DataTable)this.gridGMTbs.DataSource).AsEnumerable().Where(x => x["selected"].EqualDecimal(1)).ToList();
            if (upd_list.Count == 0)
            {
                this.numbTtlQty.Value = 0;
                this.numbTtlAmount.Value = 0;
                return;
            }

            this.numbTtlQty.Value = upd_list.Sum(r => MyUtility.Convert.GetDecimal(r["TotalShipQty"]));
            this.numbTtlAmount.Value = upd_list.Sum(r => MyUtility.Convert.GetDecimal(r["Amount"]));
        }

        private void GridGMTBooking_ColumnHeaderMouseClick(object sender, System.Windows.Forms.DataGridViewCellMouseEventArgs e)
        {
            this.gridGMTBooking.ValidateControl();
            DataGridViewColumn column = this.gridGMTBooking.Columns["selected"];
            if (!MyUtility.Check.Empty(column) && !MyUtility.Check.Empty(this.gridGMTBooking.DataSource))
            {
                this.CountSelectQty();
            }
        }
    }
}
