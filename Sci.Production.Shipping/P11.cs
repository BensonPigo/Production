using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict.Win;
using Ict;
using Sci.Data;
using System.Data.SqlClient;
using System.Linq;
using Sci.Win.Tools;
using Sci.Production.CallPmsAPI;

namespace Sci.Production.Shipping
{
    /// <summary>
    /// P11
    /// </summary>
    public partial class P11 : Win.Tems.Input6
    {
        /// <summary>
        /// P11
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            this.Helper.Controls.Grid.Generator(this.detailgrid)
            .Text("ID", header: "Garment Booking", width: Widths.AnsiChars(30))
            ;
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();

            this.disExVoucherID.Text = this.CurrentMaintain["ExVoucherID"].ToString();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Garment Booking cannot be empty !");
                return false;
            }

            List<string> ls = new List<string>();
            foreach (DataRow dr in this.DetailDatas)
            {
                if (!MyUtility.Check.Seek($@"select 1 from GMTBooking where id = '{dr["id"]}'"))
                {
                    ls.Add(MyUtility.Convert.GetString(dr["id"]));
                }
            }

            if (ls.Count > 0)
            {
                MyUtility.Msg.WarningBox($@"Garment Booking does not exist, please check again!
{string.Join(", ", ls)}");
                return false;
            }

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["InvDate"] = DateTime.Now;
        }

        /// <inheritdoc/>
        protected override DualResult OnSaveDetail(IList<DataRow> details, ITableSchema detailtableschema)
        {
            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                return new DualResult(false, $"ID({this.CurrentMaintain["ID"].ToString()}) is empty,Please inform MIS to handle this issue");
            }

            string updatesql = $@"update GMTBooking set BIRID = null  where BIRID = '{this.CurrentMaintain["ID"]}'  ";
            DualResult result = DBProxy.Current.Execute(null, updatesql);
            if (!result)
            {
                return result;
            }

            IList<string> updateCmds = new List<string>();

            foreach (DataRow dr in details)
            {
                if (dr.RowState == DataRowState.Added || dr.RowState == DataRowState.Modified)
                {
                    updateCmds.Add($@"update GMTBooking set BIRID = '{this.CurrentMaintain["ID"]}' where ID = '{dr["id"]}';");
                }

                if (dr.RowState == DataRowState.Deleted)
                {
                    updateCmds.Add($@"update GMTBooking set BIRID = null where ID = '{dr["id", DataRowVersion.Original]}';");
                }
            }

            if (updateCmds.Count != 0)
            {
                result = DBProxy.Current.Executes(null, updateCmds);
                if (!result)
                {
                    DualResult failResult = new DualResult(false, result.ToString());
                    return failResult;
                }
            }

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override DualResult ClickDelete()
        {
            string updatesql = $@"update GMTBooking set BIRID = null  where BIRID = '{this.CurrentMaintain["ID"]}'";
            DualResult result = DBProxy.Current.Execute(null, updatesql);
            if (!result)
            {
                return result;
            }

            return base.ClickDelete();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            string sqlupdate = $@"
update BIRInvoice set Status='Approved', Approve='{Env.User.UserID}', ApproveDate=getdate(), EditName='{Env.User.UserID}', EditDate=getdate()
where id = '{this.CurrentMaintain["ID"]}'
";
            DualResult result = DBProxy.Current.Execute(null, sqlupdate);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            base.ClickConfirm();
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
            string sqlchk = $@"select 1 from BIRInvoice  where ExVoucherID !='' and id = '{this.CurrentMaintain["ID"]}'";
            if (MyUtility.Check.Seek(sqlchk))
            {
                MyUtility.Msg.WarningBox("Cannot unconfirm because already created voucher no");
                return;
            }

            string sqlupdate = $@"
update BIRInvoice set Status='New', Approve='{Env.User.UserID}', ApproveDate=getdate(), EditName='{Env.User.UserID}', EditDate=getdate()
where id = '{this.CurrentMaintain["ID"]}'
";
            DualResult result = DBProxy.Current.Execute(null, sqlupdate);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            base.ClickUnconfirm();
        }

        /// <inheritdoc/>
        protected override bool ClickDeleteBefore()
        {
            string sqlchk = $@"select 1 from BIRInvoice  where ExVoucherID is not null and id = '{this.CurrentMaintain["ID"]}' and status = 'Approved' ";
            if (MyUtility.Check.Seek(sqlchk))
            {
                MyUtility.Msg.WarningBox("Already approved, cannot delete!");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        private void Btnimport_Click(object sender, EventArgs e)
        {
            if (!this.EditMode)
            {
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["InvSerial"]))
            {
                MyUtility.Msg.WarningBox("Invoice Serial cannot be empty!");
                return;
            }

            if (MyUtility.Check.Empty(this.CurrentMaintain["BrandID"]))
            {
                MyUtility.Msg.WarningBox("Brand cannot be empty!");
                return;
            }

            string sqlchk = $@"select 1 from BIRInvoice b where b.InvSerial = '{this.CurrentMaintain["InvSerial"]}' and b.BrandID = '{this.CurrentMaintain["BrandID"]}'";
            if (MyUtility.Check.Seek(sqlchk))
            {
                MyUtility.Msg.WarningBox("Already has this reocrd!");
                return;
            }

            string sqlcmd = $@"
select *
from GMTBooking with(nolock)
where isnull(BIRID,0) = 0
and BrandID = '{this.CurrentMaintain["BrandID"]}'
and InvSerial like '{this.CurrentMaintain["InvSerial"]}%'
        ";
            DataTable dt;
            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            if (dt.Rows.Count == 0)
            {
                this.ShowErr("Import error!");
                return;
            }

            foreach (DataRow dr in dt.Rows)
            {
                dr.AcceptChanges();
                dr.SetAdded();
                ((DataTable)this.detailgridbs.DataSource).ImportRow(dr);
            }
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            P11_Print p11_Print = new P11_Print(this.CurrentMaintain, this.DetailDatas);
            p11_Print.ShowDialog();
            return base.ClickPrint();
        }

        private void BtnBatchApprove(object sender, EventArgs e)
        {
            P11_BatchApprove callNextForm = new P11_BatchApprove(this.Reload);
            callNextForm.ShowDialog(this);
        }

        public void Reload()
        {
            this.ReloadDatas();
            this.RenewData();
        }

        private void DateInvDate_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.CurrentMaintain["InvDate"] = this.dateInvDate.Value;
            this.RefreshExchangeRate();
        }

        private void RefreshExchangeRate()
        {
            List<SqlParameter> listPar = new List<SqlParameter>() { new SqlParameter("@InvDate", this.CurrentMaintain["InvDate"]) };

            string sqlGetExchangeRate = @"
SELECT top 1 Rate 
FROM FinanceEN..Rate 
WHERE   RateTypeID='KP'           and
        OriginalCurrency='USD'    and
        ExchangeCurrency='PHP'    and
        @InvDate between BeginDate and EndDate
";
            DataRow drResult;

            if (MyUtility.Check.Seek(sqlGetExchangeRate, listPar, out drResult))
            {
                this.CurrentMaintain["ExchangeRate"] = drResult["Rate"];
            }
            else
            {
                this.CurrentMaintain["ExchangeRate"] = 0;
            }
        }

        private void GetGridPOListData()
        {
            if (!this.DetailDatas.Any(s => !MyUtility.Check.Empty(s["InvNo"])))
            {
                this.detailgridbs.DataSource = null;
                return;
            }

            string whereInvNo = this.DetailDatas.Where(s => !MyUtility.Check.Empty(s["InvNo"])).Select(s => $"'{s["InvNo"].ToString()}'").JoinToString(",");
            string sqlGetGridPOListData = $@"
Declare @ExchangeRate decimal(18, 8) = {this.CurrentMaintain["ExchangeRate"]}

select	o.ID,
		[UnitPriceUSD] = ((isnull(o.CPU, 0) + isnull(SubProcessCPU.val, 0)) * isnull(CpuCost.val, 0)) + isnull(SubProcessAMT.val, 0) + isnull(LocalPurchase.val, 0)
		into #tmpUnitPriceUSD
from Orders o with (nolock)
left join Factory f with (nolock) on f.ID = o.FactoryID
outer apply (select [val] = sum(Isnull(Price,0)) from GetSubProcessDetailByOrderID(o.ID,'CPU')) SubProcessCPU
outer apply (select [val] = sum(Isnull(Price,0)) from GetSubProcessDetailByOrderID(o.ID,'AMT')) SubProcessAMT
outer apply (select top 1 [val] = fd.CpuCost
             from FtyShipper_Detail fsd WITH (NOLOCK) , FSRCpuCost_Detail fd WITH (NOLOCK) 
             where fsd.BrandID = o.BrandID
             and fsd.FactoryID = o.FactoryID
             and o.OrigBuyerDelivery between fsd.BeginDate and fsd.EndDate
             and fsd.ShipperID = fd.ShipperID
             and o.OrigBuyerDelivery between fd.BeginDate and fd.EndDate
			 and (fsd.SeasonID = o.SeasonID or fsd.SeasonID = '')
			 order by SeasonID desc) CpuCost
outer apply (select [val] = iif(f.LocalCMT = 1, dbo.GetLocalPurchaseStdCost(o.ID), 0)) LocalPurchase
where exists (select 1 
			  from PackingList p with (nolock)
			  inner join PackingList_Detail pd with (nolock) on p.ID = pd.ID
			  where p.INVNo in ({whereInvNo}) and pd.OrderID = o.ID
			  )

select	[No] = 0,
		[OrderID] = o.ID,
        o.CustPONo,		
		p.INVNo,
		o.StyleID,
		s.Description,
		tup.UnitPriceUSD,
		[ShipQty] = sum(pd.ShipQty),
		[AmountUSD] = sum(pd.ShipQty) * tup.UnitPriceUSD,
		[AmountPHP] = Round(sum(pd.ShipQty) * tup.UnitPriceUSD * @ExchangeRate, 0)
from PackingList p with (nolock)
inner join PackingList_Detail pd with (nolock) on p.ID = pd.ID
inner join Orders o with (nolock) on pd.OrderID = o.ID
inner join Style s with (nolock) on s.Ukey = o.StyleUkey
left join #tmpUnitPriceUSD tup on tup.ID = o.ID
where p.INVNo in ({whereInvNo})
group by    o.CustPONo,
		    o.ID,
		    o.StyleID,
		    s.Description,
		    tup.UnitPriceUSD

drop table #tmpUnitPriceUSD
";

            DataTable dtPOList;

            DualResult result = DBProxy.Current.Select(null, sqlGetGridPOListData, out dtPOList);

            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            List<string> listPLFromRgCode = PackingA2BWebAPI.GetPLFromRgCodeByMutiInvNo(this.DetailDatas.Select(s => s["InvNo"].ToString()).ToList());

            foreach (string plFromRgCode in listPLFromRgCode)
            {
                DataTable dtA2BResult;
                result = PackingA2BWebAPI.GetDataBySql(plFromRgCode, sqlGetGridPOListData, out dtA2BResult);

                if (!result)
                {
                    this.ShowErr(result);
                    return;
                }

                dtPOList.MergeBySyncColType(dtA2BResult);
            }

            // 因為A2B會使用webapi抓，所以No要在資料合併完再排序
            if (dtPOList.Rows.Count > 0)
            {
                dtPOList = dtPOList.AsEnumerable().OrderBy(s => s["CustPONo"]).ThenBy(s => s["OrderID"]).ThenBy(s => s["StyleID"]).CopyToDataTable();
                int rowNum = 1;
                foreach (DataRow dr in dtPOList.Rows)
                {
                    dr["No"] = rowNum;
                    rowNum++;
                }
            }

            this.detailgridbs.DataSource = dtPOList;
            this.numDetailTotalQty.Value = dtPOList.AsEnumerable().Sum(s => MyUtility.Convert.GetInt(s["ShipQty"]));

            if (dtPOList.Rows.Count > 0)
            {
                List<CurrencyAMT> listCurrencyAMT = new List<CurrencyAMT>();
                listCurrencyAMT.Add(new CurrencyAMT { Currency = "PHP", Amount = 0 });
                listCurrencyAMT.Add(new CurrencyAMT { Currency = "USD", Amount = 0 });
                var currencyResult = dtPOList.AsEnumerable()
                    .GroupBy(s => string.Empty)
                    .Select(s => new
                    {
                        AmtUSD = s.Sum(groupItem => MyUtility.Convert.GetDecimal(groupItem["AmountUSD"])),
                        AmtKHR = s.Sum(groupItem => MyUtility.Convert.GetDecimal(groupItem["AmountPHP"])),
                    }).First();

                foreach (CurrencyAMT itemCurrencyAMT in listCurrencyAMT)
                {
                    switch (itemCurrencyAMT.Currency)
                    {
                        case "PHP":
                            itemCurrencyAMT.Amount = currencyResult.AmtKHR;
                            break;
                        case "USD":
                            itemCurrencyAMT.Amount = currencyResult.AmtUSD;
                            break;
                        default:
                            break;
                    }
                }

                this.gridCurrency.DataSource = listCurrencyAMT;
            }
        }

        private class CurrencyAMT
        {
            public string Currency { get; set; }

            public decimal Amount { get; set; }
        }

        private void BtnExchangeRate_Click(object sender, EventArgs e)
        {
            string sqlGetExchangeRate = @"
SELECT  [Begin Date] = r.BeginDate,
        [End Date] = r.EndDate,
        [Exchange Rate] = r.Rate 
FROM FinanceEN..Rate r 
WHERE RateTypeID='KP'
AND OriginalCurrency='USD'
AND ExchangeCurrency='PHP' 
order by r.BeginDate
";
            SelectItem selectItem = new SelectItem(sqlGetExchangeRate, null, null);

            DialogResult dialogResult = selectItem.ShowDialog();

            if (!this.EditMode)
            {
                return;
            }

            this.CurrentMaintain["ExchangeRate"] = selectItem.GetSelecteds()[0]["Exchange Rate"];
        }
    }
}
