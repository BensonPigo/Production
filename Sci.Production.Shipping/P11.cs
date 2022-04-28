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
            this.gridPOListbs.DataSource = new DataTable();
        }

        /// <inheritdoc/>
        protected override DualResult OnDetailSelectCommandPrepare(PrepareDetailSelectCommandEventArgs e)
        {
            string masterID = (e.Master == null) ? string.Empty : e.Master["ID"].ToString();
            this.DetailSelectCommand = $@"
select  bd.*,
        GB.ETD,
        GB.ETA,
        GB.TotalShipQty
from BIRInvoice_Detail bd
left join GMTBooking GB with (nolock) on bd.InvNo = GB.ID
where bd.id = '{masterID}'
";
            return base.OnDetailSelectCommandPrepare(e);
        }

        /// <inheritdoc/>
        protected override void OnDetailGridSetup()
        {
            base.OnDetailGridSetup();

            this.Helper.Controls.Grid.Generator(this.gridCurrency)
                .Text("Currency", header: "Currency", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Numeric("Amount", header: "Amount", width: Widths.AnsiChars(15), iseditingreadonly: true);

            // 此Detail 被隱藏起來
            this.Helper.Controls.Grid.Generator(this.detailgrid)
                .Text("InvNo", header: "GB#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("ETD", header: "On Board Date", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Date("ETA", header: "ETA", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Numeric("TotalShipQty", header: "Q'ty (Pcs)", width: Widths.AnsiChars(15), iseditingreadonly: true)
                ;

            this.Helper.Controls.Grid.Generator(this.gridPOList)
                .Numeric("No", header: "No.", width: Widths.AnsiChars(5), iseditingreadonly: true)
                .Text("OrderID", header: "SP#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("CustPONo", header: "PO#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .Text("InvNo", header: "GB#", width: Widths.AnsiChars(20), iseditingreadonly: true)
                .Text("StyleID", header: "Style#", width: Widths.AnsiChars(15), iseditingreadonly: true)
                .EditText("Description", header: "Description", width: Widths.AnsiChars(30), iseditingreadonly: true)
                .Numeric("ShipQty", header: "Q'ty", width: Widths.AnsiChars(12), iseditingreadonly: true)
                .Numeric("UnitPriceUSD", header: "Unit Price (USD)", width: Widths.AnsiChars(10), integer_places: 9, decimal_places: 3, iseditingreadonly: true)
                .Numeric("AmountUSD", header: "Amount(USD)", width: Widths.AnsiChars(15), integer_places: 12, decimal_places: 3, iseditingreadonly: true)
                .Numeric("UnitPricePHP", header: "Unit Price (PHP)", width: Widths.AnsiChars(10), integer_places: 9, decimal_places: 3, iseditingreadonly: true)
                .Numeric("AmountPHP", header: "Amount(PHP)", width: Widths.AnsiChars(15), integer_places: 12, decimal_places: 3, iseditingreadonly: true)
            ;
        }

        /// <inheritdoc/>
        protected override void OnDetailEntered()
        {
            base.OnDetailEntered();
            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                this.lblStatus.Text = "FIN Manager Approve";
            }
            else
            {
                this.lblStatus.Text = this.CurrentMaintain["Status"].ToString();
            }

            this.GetGridPOListData();
        }

        /// <inheritdoc/>
        protected override bool ClickEditBefore()
        {
            if (this.CurrentMaintain["Status"].ToString() == "Confirmed")
            {
                MyUtility.Msg.InfoBox("This record already confirmed, can not edit");
                return false;
            }

            return base.ClickEditBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickSaveBefore()
        {
            if (this.DetailDatas.Count == 0)
            {
                MyUtility.Msg.WarningBox("Detail no data");
                return false;
            }

            string whereInvNo = this.DetailDatas.Select(s => $"'{s["InvNo"]}'").JoinToString(",");

            string sqlCheckInvoice = $@"
select  bd.ID as [CMT Invoice No.],
        bd.InvNo as [GB#],
        gb.ETD as [On Board Date],
        gb.ETA as [ETA]
from BIRInvoice_Detail bd with (nolock)
left join GMTBooking gb with (nolock) on gb.ID = bd.InvNo
where   bd.ID <> '{this.CurrentMaintain["ID"]}' and
        bd.InvNo in ({whereInvNo})
        
";
            DataTable dtCheckInvoice;
            DualResult result = DBProxy.Current.Select(null, sqlCheckInvoice, out dtCheckInvoice);
            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            if (dtCheckInvoice.Rows.Count > 0)
            {
                MyUtility.Msg.ShowMsgGrid_LockScreen(dtCheckInvoice, "The following GB# already exist in CMT Invoice#.", "P11. Save");
                return false;
            }

            #region 產生ID 序號by 年(YY)重置

            if (MyUtility.Check.Empty(this.CurrentMaintain["ID"]))
            {
                List<SqlParameter> listPar = new List<SqlParameter>() { new SqlParameter("@CMTInvDate", this.CurrentMaintain["InvDate"]) };

                string idHeader = MyUtility.GetValue.Lookup("select RgCode + Format(@CMTInvDate, 'yyMM') + '-' from dbo.system", listPar);
                string sqlGetID = @"
Declare @IDkeyWord varchar(5)
select @IDkeyWord = RgCode + Format(@CMTInvDate, 'yy') from dbo.system

select ID from BIRInvoice where ID like @IDkeyWord + '%'
";
                result = DBProxy.Current.Select(null, sqlGetID, listPar, out DataTable dtBIRInvoiceID);

                if (!result)
                {
                    this.ShowErr(result);
                    return false;
                }

                string seq = "00001";

                if (dtBIRInvoiceID.Rows.Count > 0)
                {
                    seq = (dtBIRInvoiceID.AsEnumerable()
                            .Select(s => MyUtility.Convert.GetInt(s["ID"].ToString().Split('-')[1]))
                            .Max() + 1
                          ).ToString().PadLeft(5, '0');
                }

                this.CurrentMaintain["ID"] = idHeader + seq;
            }

            #endregion

            return base.ClickSaveBefore();
        }

        /// <inheritdoc/>
        protected override void ClickNewAfter()
        {
            base.ClickNewAfter();
            this.CurrentMaintain["Status"] = "New";
            this.CurrentMaintain["InvDate"] = DateTime.Now;
            this.numDetailTotalQty.Value = 0;
            this.gridCurrency.DataSource = null;
            this.RefreshExchangeRate();
        }

        /// <inheritdoc/>
        protected override void ClickConfirm()
        {
            base.ClickConfirm();
            string sqlupdate = $@"
update BIRInvoice set Status='Confirmed', Approve='{Env.User.UserID}', ApproveDate=getdate(), EditName='{Env.User.UserID}', EditDate=getdate()
where id = '{this.CurrentMaintain["ID"]}'
";
            DualResult result = DBProxy.Current.Execute(null, sqlupdate);
            if (!result)
            {
                this.ShowErr(result);
                return;
            }

            MyUtility.Msg.InfoBox("Finance manager approve success!!");
        }

        /// <inheritdoc/>
        protected override void ClickUnconfirm()
        {
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
        protected override bool ClickDeleteBefore()
        {
            if (MyUtility.Convert.GetString(this.CurrentMaintain["Status"]) != "New")
            {
                MyUtility.Msg.WarningBox("This record was approved, can't delete.");
                return false;
            }

            return base.ClickDeleteBefore();
        }

        /// <inheritdoc/>
        protected override bool ClickPrint()
        {
            P11_Print p11_Print = new P11_Print(this.CurrentMaintain, this.DetailDatas);
            p11_Print.ShowDialog();
            return base.ClickPrint();
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
                this.gridPOListbs.DataSource = null;
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
		P.INVNo,
		o.StyleID,
		s.Description,
		tup.UnitPriceUSD,
		[ShipQty] = sum(pd.ShipQty),
		[AmountUSD] = sum(pd.ShipQty) * tup.UnitPriceUSD,
        [UnitPricePHP] = tup.UnitPriceUSD * @ExchangeRate,
		[AmountPHP] = Round(sum(pd.ShipQty) * tup.UnitPriceUSD * @ExchangeRate, 0),
		bd.ID,bd.InvNo
from PackingList p with (nolock)
left join BIRInvoice_Detail bd with (nolock) on p.INVNo = bd.InvNo
inner join PackingList_Detail pd with (nolock) on p.ID = pd.ID
inner join GMTBooking g with (nolock) on g.ID = P.invno
inner join Orders o with (nolock) on pd.OrderID = o.ID
inner join Style s with (nolock) on s.Ukey = o.StyleUkey
left join #tmpUnitPriceUSD tup on tup.ID = o.ID
where p.INVNo in ({whereInvNo})
group by      o.CustPONo,
		    o.ID,
		    o.StyleID,
            P.INVNo,
		    s.Description,
		    tup.UnitPriceUSD,
			bd.id,
			bd.invno

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

            this.gridPOListbs.DataSource = dtPOList;
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
                        AmtPHP = s.Sum(groupItem => MyUtility.Convert.GetDecimal(groupItem["AmountPHP"])),
                    }).First();

                foreach (CurrencyAMT itemCurrencyAMT in listCurrencyAMT)
                {
                    switch (itemCurrencyAMT.Currency)
                    {
                        case "PHP":
                            itemCurrencyAMT.Amount = currencyResult.AmtPHP;
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

            if (!this.EditMode || dialogResult == DialogResult.Cancel)
            {
                return;
            }

            this.CurrentMaintain["ExchangeRate"] = selectItem.GetSelecteds()[0]["Exchange Rate"];
            this.GetGridPOListData();
        }

        private void BtnImportGMTBooking_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = new P11_Import(this.CurrentMaintain["ID"].ToString(), (DataTable)this.detailgridbs.DataSource).ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                this.GetGridPOListData();
            }
        }

        private void NumExchangeRate_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MyUtility.Convert.GetDecimal(this.CurrentMaintain["ExchangeRate"]) == this.numExchangeRate.Value)
            {
                return;
            }

            this.CurrentMaintain["ExchangeRate"] = this.numExchangeRate.Value;

            this.GetGridPOListData();
        }
    }
}
