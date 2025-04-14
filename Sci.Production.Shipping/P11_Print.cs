using Ict;
using Newtonsoft.Json;
using Sci.Andy.ExtensionMethods;
using Sci.Data;
using Sci.Production.CallPmsAPI;
using Sci.Production.Class.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Shipping
{
    /// <inheritdoc/>
    public partial class P11_Print : Win.Tems.PrintForm
    {
        private IList<DataRow> DetailDatas;
        private DataRow CurrentMaintain;

        /// <summary>
        /// P11_Print
        /// </summary>
        /// <param name="currentMaintain">Main DataRow</param>
        /// <param name="detailDatas">detailDatas</param>
        public P11_Print(DataRow currentMaintain, IList<DataRow> detailDatas)
        {
            this.InitializeComponent();
            this.DetailDatas = detailDatas;
            this.CurrentMaintain = currentMaintain;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.dateInvoice.ReadOnly = true;
            this.txtShipper.ReadOnly = true;
            this.txtGBNo.ReadOnly = true;
            this.txtbrand.ReadOnly = true;
        }

        /// <inheritdoc/>
        protected override bool ToExcel()
        {
            if (this.radioBIRSalesInv.Checked)
            {
                return this.BIRSalesInvoice();
            }
            else
            {
                return this.BIRSalesReport();
            }
        }

        private bool BIRSalesReport()
        {
            #region get data
            string sqlGetData;
            string sqlInvDateWhere = string.Empty;
            string sqlShipperWhere = string.Empty;
            List<SqlParameter> listPar = new List<SqlParameter>();

            if ((!this.dateInvoice.HasValue1 && !this.dateInvoice.HasValue2) &&
                 MyUtility.Check.Empty(this.txtGBNo.Text))
            {
                if (this.chkOutStanding.Checked)
                {
                    MyUtility.Msg.WarningBox("On Board Date or GB# cannot be empty.");
                }
                else
                {
                    MyUtility.Msg.WarningBox("Invoice Date or GB# cannot be empty.");
                }

                return false;
            }

            // [ISP20241007] 已報帳資料不更新
            string isNewData = this.CurrentMaintain["AddDate"].ToDateTime() >= new DateTime(2024, 11, 05) ? "1" : "0";

            if (this.dateInvoice.HasValue1)
            {
                if (this.chkOutStanding.Checked)
                {
                    sqlInvDateWhere += " and gb.ETD >= @InvDateFrom";
                    sqlInvDateWhere += " and isnull(gb.CMTInvoiceNo,'') = ''";
                }
                else
                {
                    sqlInvDateWhere += " and bi.InvDate >= @InvDateFrom";
                }

                listPar.Add(new SqlParameter("@InvDateFrom", this.dateInvoice.DateBox1.Value));
            }

            if (this.dateInvoice.HasValue2)
            {
                if (this.chkOutStanding.Checked)
                {
                    sqlInvDateWhere += " and gb.ETD <= @InvDateTo";
                }
                else
                {
                    sqlInvDateWhere += " and bi.InvDate <= @InvDateTo";
                }

                listPar.Add(new SqlParameter("@InvDateTo", this.dateInvoice.DateBox2.Value));
            }

            if (!MyUtility.Check.Empty(this.txtbrand.Text))
            {
                sqlInvDateWhere += " and gb.BrandID <= @BrandID";
                listPar.Add(new SqlParameter("@BrandID", this.txtbrand.Text));
            }

            if (!MyUtility.Check.Empty(this.txtGBNo.Text))
            {
                sqlInvDateWhere += " and gb.ID = @InvNo";
                listPar.Add(new SqlParameter("@InvNo", this.txtGBNo.Text));
            }

            if (this.txtShipper.Text.Length > 0)
            {
                sqlShipperWhere += $" and gb.Shipper = '{this.txtShipper.Text}'";
            }

            #region Get A2B
            string sqlGetA2BGMT;
            if (this.chkOutStanding.Checked)
            {
                sqlGetA2BGMT = $@"
select	distinct ID = '',
		[GMTBooking] = gb.ID,
		[InvDate] = cast(null as date),
        gbd.PLFromRgCode,
        [PackID] = '',
        [GRS_WEIGHT] = cast(0 as numeric(11,3)),
        [Qty] = cast(0 as numeric(25,4)),
        [OrderID] = '',
        CustPONo = '',
        StyleID = '',
		Description = '',	
        [ShipQty] = cast(0 as numeric(25,4)),
        [Dest] = '',
        INVNo = '',
        UnitPriceUSD = cast(0 as numeric(25,4))
from GMTBooking gb
left join GMTBooking_Detail gbd with (nolock) on gbd.ID = gb.ID
where	1=1
        {sqlInvDateWhere}
		{sqlShipperWhere}
";
            }
            else
            {
                sqlGetA2BGMT = $@"
select distinct	bi.ID,
		[GMTBooking] = gb.ID,
		[InvDate] = bi.InvDate,
        gbd.PLFromRgCode,
        [PackID] = '',
        [GRS_WEIGHT] = cast(0 as numeric(11,3)),
        [Qty] = cast(0 as numeric(25,4)),
        [OrderID] = '',
        CustPONo = '',
        StyleID = '',
		Description = '',	
        [ShipQty] = cast(0 as numeric(25,4)),
        [Dest] = '',
        INVNo = '',
        UnitPriceUSD = cast(0 as numeric(25,4))
from BIRInvoice bi with (nolock)
inner join BIRInvoice_Detail bd with (nolock) on bi.ID = bd.ID
inner join GMTBooking gb on bi.ID = gb.CMTInvoiceNo
inner join GMTBooking_Detail gbd with (nolock) on gbd.ID = gb.ID
where	1=1
        {sqlInvDateWhere}
		{sqlShipperWhere}
";
            }

            DualResult result = DBProxy.Current.Select(null, sqlGetA2BGMT, listPar, out DataTable dtA2BGMT);

            if (!result)
            {
                this.ShowErr(result);
                return false;
            }

            DataTable dtA2BResult = new DataTable();
            dtA2BResult = dtA2BGMT.Clone();

            if (dtA2BGMT.Rows.Count > 0)
            {
                string sqlGetPackingA2B = $@"
alter table #tmp alter column [GMTBooking] varchar(25)

select  t.InvDate,
		t.ID,		
		pld.OrderID,
		o.CustPONo,
		t.GMTBooking,
		o.StyleID,
		s.Description,		
		[ShipQty] = pld.ShipQty,
        [PackID] = pl.ID,
		[GRS_WEIGHT] = pl.GW,
		[Qty] = pl.ShipQty,
        pl.INVNo,
		[UnitPriceUSD] = ((isnull(o.CPU, 0) + isnull(SubProcessCPU.val, 0)) * isnull(CpuCost.val, 0)) + isnull(SubProcessAMT.val, 0) + isnull(LocalPurchase.val, 0)
from #tmp t
inner join PackingList pl with (nolock) on pl.INVNo = t.GMTBooking
inner join PackingList_Detail pld with (nolock) on pl.ID = pld.ID
inner join Orders o with (nolock) on pld.OrderID = o.ID
inner join Style s with (nolock) on s.Ukey = o.StyleUkey
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
            
";
                foreach (var groupA2BGMT in dtA2BGMT.AsEnumerable().GroupBy(s => s["PLFromRgCode"].ToString()))
                {
                    if (MyUtility.Check.Empty(groupA2BGMT.Key))
                    {
                        continue;
                    }

                    PackingA2BWebAPI_Model.DataBySql dataBySql = new PackingA2BWebAPI_Model.DataBySql()
                    {
                        SqlString = sqlGetPackingA2B,
                        TmpTable = JsonConvert.SerializeObject(dtA2BGMT.Select($"PLFromRgCode = '{groupA2BGMT.Key}'").CopyToDataTable()),
                    };

                    DataTable dtA2BPAcking;
                    result = PackingA2BWebAPI.GetDataBySql(groupA2BGMT.Key, dataBySql, out dtA2BPAcking);
                    if (!result)
                    {
                        this.ShowErr(result);
                        return false;
                    }

                    dtA2BPAcking.MergeTo(ref dtA2BResult);
                }
            }

            #endregion

            sqlGetData = $@"
select	bi.InvDate,
		bi.ID,
		pd.OrderID,
		o.CustPONo,
		[GMTBooking] = gb.ID,
		o.StyleID,
		s.Description,		
		[ShipQty] = sum(pd.ShipQty),
		[PackID] = pl.ID,
		[GRS_WEIGHT] = pl.GW,
        gb.Dest
into #tmpBIRInvoice
from BIRInvoice bi with (nolock)
inner join BIRInvoice_Detail bd with (nolock) on bd.id = bi.id
inner join GMTBooking gb on gb.id = bd.invno
inner join PackingList pl with (nolock) on pl.INVNo = gb.ID
inner join PackingList_Detail pd with (nolock) on pd.ID = pl.ID
inner join Orders o with (nolock) on o.ID = pd.OrderID
inner join Style s with (nolock) on s.Ukey = o.StyleUkey
where	1=1
        {sqlInvDateWhere}
		{sqlShipperWhere}
group by bi.InvDate,
		bi.ID,
		pd.OrderID,
		o.CustPONo,
		gb.ID,
        gb.Dest,
		o.StyleID,
		s.Description,		
		pl.ID,
		pl.GW

select	o.ID,
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
			  inner join PackingList_Detail pd with (nolock) on p.ID = pd.ID
			  where p.INVNo in (select GMTBooking from #tmpBIRInvoice ) 
              and pd.OrderID = o.ID
			  )

select
[InvDate] = tbi.InvDate,
[ID] = tbi.ID,
[PARTICULAR_DESCRIPTION] = 'SINTEX INTERNATIONAL LTD',
OrderID,
CustPONo,
GMTBooking,
StyleID,
Description,		
[Qty] = sum(tbi.ShipQty),
[Dest] = tbi.Dest,
[GW] = tbi.GRS_WEIGHT,
tup.UnitPriceUSD,
[AmountUSD] = sum(tbi.ShipQty) * tup.UnitPriceUSD,
bi.ExchangeRate,
[UnitPricePHP] = tup.UnitPriceUSD * bi.ExchangeRate,
[AmountPHP] = Round(sum(tbi.ShipQty) * tup.UnitPriceUSD * bi.ExchangeRate, 0)
from #tmpBIRInvoice tbi
left join BIRInvoice bi on tbi.ID = bi.ID
left join #tmpUnitPriceUSD tup on tbi.OrderID = tup.ID
group by	 tup.UnitPriceUSD,Description,StyleID,GMTBooking,CustPONo,OrderID,tbi.ID,tbi.InvDate,bi.ExchangeRate,tbi.Dest,tbi.GRS_WEIGHT

union all
select
        [InvDate] = t.InvDate,
        [ID] = t.ID,
        [PARTICULAR_DESCRIPTION] = 'SINTEX INTERNATIONAL LTD',
        t.OrderID,
        t.CustPONo,
        t.GMTBooking,
        t.StyleID,
        t.Description,		
        [Qty] = sum(t.ShipQty),
        [Dest] = g.Dest,
        [GW] = t.GRS_WEIGHT,
        t.UnitPriceUSD,
        [AmountUSD] = sum(t.ShipQty) * t.UnitPriceUSD,
        bi.ExchangeRate,
        [UnitPricePHP] = t.UnitPriceUSD * bi.ExchangeRate,
        [AmountPHP] = Round(sum(t.ShipQty) * t.UnitPriceUSD * bi.ExchangeRate, 0)
from #tmp t with (nolock)
left join BIRInvoice bi on t.ID = bi.ID
Left join GMTBooking g on g.ID = t.INVNo
group by t.UnitPriceUSD,t.Description,t.StyleID,t.GMTBooking,t.CustPONo,t.OrderID,t.ID,t.InvDate,bi.ExchangeRate,g.Dest,t.GRS_WEIGHT

drop table #tmpBIRInvoice
";

            DataTable dtResult;
            this.ShowWaitMessage("Excel Processing...");
            result = MyUtility.Tool.ProcessWithDatatable(dtA2BResult, null, sqlGetData, out dtResult, paramters: listPar);
            if (!result)
            {
                this.ShowErr(result);
                this.HideWaitMessage();
                return false;
            }

            this.SetCount(dtResult.Rows.Count);

            if (dtResult.Rows.Count == 0)
            {
                MyUtility.Msg.InfoBox("No data found");
                this.HideWaitMessage();
                return false;
            }

            string strXltName = Env.Cfg.XltPathDir + "\\Shipping_P11_BIRSalesReport.xltx";
            Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                this.HideWaitMessage();
                return false;
            }

            MyUtility.Excel.CopyToXls(dtResult, string.Empty, "Shipping_P11_BIRSalesReport.xltx", 1, false, null, excel, wSheet: excel.Sheets[1]);

            Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            this.HideWaitMessage();

            #region Save & Show Excel
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_P11_BIRSalesReport");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
            #endregion

        }

        private bool BIRSalesInvoice()
        {
            #region
            List<string> ids = new List<string>();
            foreach (DataRow dr in this.DetailDatas)
            {
                ids.Add("'" + dr["INVno"] + "'");
            }

            List<string> listPLFromRgCode = PackingA2BWebAPI.GetPLFromRgCodeByMutiInvNo(ids);

            // [ISP20241007] 已報帳資料不更新
            string isNewData = this.CurrentMaintain["AddDate"].ToDateTime() >= new DateTime(2024, 11, 05) ? "1" : "0";

            DataTable dt;
            string sqlcmd = $@"
select 
	A=o.CustPONo,
	B=o.StyleID,
	C=s.Description,
	E=sum(pd.ShipQty),
	F='PCS',
	G=o.CurrencyID,
	H=ROUND(std.FtyCMP,2),
	J=sum(pd.ShipQty)*ROUND(isnull(std.FtyCMP,0),2)
from orders o with(nolock)
inner join PackingList_Detail pd with(nolock) on pd.OrderID = o.id
inner join PackingList p with(nolock) on p.id = pd.id
left join Style s with(nolock) on s.Ukey = o.StyleUkey
outer apply(select SubProcessCPU= sum(Isnull(Price,0)) from GetSubProcessDetailByOrderID(o.id,'CPU'))a
outer apply(select subProcessAMT= sum(Isnull(Price,0)) from GetSubProcessDetailByOrderID(o.id,'AMT'))b
outer apply(
	select fd.CpuCost
	from FtyShipper_Detail fsd WITH (NOLOCK) , FSRCpuCost_Detail fd WITH (NOLOCK) 
	where fsd.BrandID = o.BrandID
	and fsd.FactoryID = o.FactoryID
	and o.OrigBuyerDelivery between fsd.BeginDate and fsd.EndDate
	and fsd.ShipperID = fd.ShipperID
	and o.OrigBuyerDelivery between fd.BeginDate and fd.EndDate
	and o.OrigBuyerDelivery is not null
    and fsd.seasonID = o.seasonID
    and fd.OrderCompanyID = o.OrderCompanyID
)f1
outer apply(
	select fd.CpuCost
	from FtyShipper_Detail fsd WITH (NOLOCK) , FSRCpuCost_Detail fd WITH (NOLOCK) 
	where fsd.BrandID = o.BrandID
	and fsd.FactoryID = o.FactoryID
	and o.OrigBuyerDelivery between fsd.BeginDate and fsd.EndDate
	and fsd.ShipperID = fd.ShipperID
	and o.OrigBuyerDelivery between fd.BeginDate and fd.EndDate
	and o.OrigBuyerDelivery is not null
    and fsd.seasonID = ''
    and fd.OrderCompanyID = o.OrderCompanyID
)f
outer apply(
	select iif({isNewData} = 1,dbo.GetLocalPurchaseStdCost(o.id),0) price
)s3
outer apply(
	select FtyCMP = Round((isnull(round(o.CPU,3,1),0) + isnull(round(a.SubProcessCPU,3,1),0)) * 
	isnull(round(isnull(f1.CpuCost,f.CpuCost),3,1),0) + isnull(round(b.subProcessAMT,3,1),0) + isnull(round(s3.price,3,1),0), 3)
)std
where p.INVNo in({string.Join(",", ids)})
group by o.CustPONo,o.StyleID,s.Description,o.PoPrice,o.id,o.CPU,o.CurrencyID,std.FtyCMP
";
            this.ShowWaitMessage("Excel Processing...");

            DualResult result = DBProxy.Current.Select(null, sqlcmd, out dt);
            if (!result)
            {
                this.ShowErr(result);
                this.HideWaitMessage();
                return false;
            }

            foreach (string plFromRgCode in listPLFromRgCode)
            {
                DataTable dtA2B;
                result = PackingA2BWebAPI.GetDataBySql(plFromRgCode, sqlcmd, out dtA2B);

                if (!result)
                {
                    this.ShowErr(result);
                    return false;
                }

                dtA2B.MergeTo(ref dt);
            }
            #endregion

            string strXltName = Env.Cfg.XltPathDir + "\\Shipping_P11.xltx";
            Excel.Application excel = MyUtility.Excel.ConnectExcel(strXltName);
            if (excel == null)
            {
                this.HideWaitMessage();
                return false;
            }

            Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[1];

            #region 產生頁首頁尾資料

            // 頁首
            string top1idsql = $@"
select top 1 id
from GMTBooking
where id in({string.Join(",", ids)})
order by FCRDate desc
";
            string top1id = MyUtility.GetValue.Lookup(top1idsql);

            string bIRShipToSql = $@"
select top 1 b.BIRShipTo
from GMTBooking a
inner join CustCD b on b.id = a.CustCDID and b.BrandID = a.BrandID
where a.id = '{top1id}' 
";
            string top1BIRShipTo = MyUtility.GetValue.Lookup(bIRShipToSql);
            if (!MyUtility.Check.Empty(top1BIRShipTo))
            {
                string[] bIRShipToarry = top1BIRShipTo.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                for (int i = 0, j = 0; i < bIRShipToarry.Length; i++)
                {
                    if (!MyUtility.Check.Empty(bIRShipToarry[i]))
                    {
                        worksheet.Cells[12 + j, 2] = bIRShipToarry[i];
                        j++;
                    }
                }
            }

            DataRow drGMT;
            string top1GMT = $@"select * from GMTBooking where id = '{top1id}'";
            if (MyUtility.Check.Seek(top1GMT, out drGMT))
            {
                worksheet.Cells[9, 10] = drGMT["FCRDate"];
                worksheet.Cells[11, 10] = drGMT["Vessel"];
                worksheet.Cells[16, 10] = MyUtility.GetValue.Lookup($@"select NameEN from Country where id = '{drGMT["Dest"]}'");
            }

            // 頁尾
            decimal sumJ = MyUtility.Convert.GetDecimal(dt.Compute("sum(J)", null));
            decimal sumM = sumJ;
            decimal sumE = MyUtility.Convert.GetDecimal(dt.Compute("sum(E)", null));

            // worksheet.Cells[48, 3] = MyUtility.Convert.USDMoney(sumI).Replace("AND CENTS", Environment.NewLine + "AND CENTS");
            worksheet.Cells[57, 3] = MyUtility.Convert.USDMoney(sumJ);

            string sqlSumGW = $@"
select sumGW = isnull(Sum(p.GW), 0), sumNW = isnull(sum(p.NW), 0), sumCBM = isnull(sum(p.CBM), 0)
from PackingList p with(nolock)
where p.INVNo in ({string.Join(",", ids)})
";
            decimal sumGW = 0;
            decimal sumNW = 0;
            decimal sumCBM = 0;

            foreach (string plFromRgCode in listPLFromRgCode)
            {
                DataRow drResult;
                result = PackingA2BWebAPI.SeekBySql(plFromRgCode, sqlSumGW, out drResult);
                if (!result)
                {
                    this.ShowErr(result);
                    return false;
                }

                sumGW += MyUtility.Convert.GetDecimal(drResult["sumGW"]);
                sumNW += MyUtility.Convert.GetDecimal(drResult["sumNW"]);
                sumCBM += MyUtility.Convert.GetDecimal(drResult["sumCBM"]);
            }

            DataRow drsum;
            if (MyUtility.Check.Seek(sqlSumGW, out drsum))
            {
                sumGW += MyUtility.Convert.GetDecimal(drsum["sumGW"]);
                sumNW += MyUtility.Convert.GetDecimal(drsum["sumNW"]);
                sumCBM += MyUtility.Convert.GetDecimal(drsum["sumCBM"]);
            }

            worksheet.Cells[66, 3] = sumGW;
            worksheet.Cells[67, 3] = sumNW;
            worksheet.Cells[68, 3] = sumCBM;

            worksheet.Cells[63, 10] = sumJ;
            worksheet.Cells[65, 10] = 0;
            worksheet.Cells[66, 10] = sumM;
            worksheet.Cells[60, 2] = sumE;
            #endregion

            #region 內容

            // 如果內容超過41筆插入新的頁面
            int insertSheetCount = dt.Rows.Count / 41;
            for (int i = 1; i <= insertSheetCount; i++)
            {
                worksheet.Copy(Type.Missing, worksheet);
            }

            int contentCount = 41;
            int ttlSheetCount = excel.ActiveWorkbook.Worksheets.Count;
            for (int i = 1; i <= ttlSheetCount; i++)
            {
                var xlNewSheet = (Excel.Worksheet)excel.ActiveWorkbook.Worksheets[i];
                xlNewSheet.Name = "BIR Invoice-" + i.ToString();
                int intRowsStart = 25;
                int dataEnd = i * contentCount;
                int dataStart = dataEnd - contentCount;
                object[,] objArray = new object[1, 10];

                for (int j = dataStart; j < dataEnd; j++)
                {
                    if (j >= dt.Rows.Count)
                    {
                        break;
                    }

                    DataRow dr = dt.Rows[j];
                    int rownum = intRowsStart++;
                    objArray[0, 0] = dr["A"];
                    objArray[0, 1] = dr["B"];
                    objArray[0, 2] = dr["C"];
                    objArray[0, 3] = string.Empty;
                    objArray[0, 4] = dr["E"];
                    objArray[0, 5] = dr["F"];
                    objArray[0, 6] = dr["G"];
                    objArray[0, 7] = dr["H"];
                    objArray[0, 8] = string.Empty;
                    objArray[0, 9] = dr["J"];
                    xlNewSheet.Range[string.Format("A{0}:J{0}", rownum)].Value2 = objArray;
                }

                Marshal.ReleaseComObject(xlNewSheet);
            }
            #endregion

            #region Save & Show Excel
            worksheet = excel.ActiveWorkbook.Worksheets[1];
            worksheet.Activate();
            string strExcelName = Class.MicrosoftFile.GetName("Shipping_P11");
            excel.ActiveWorkbook.SaveAs(strExcelName);
            excel.Quit();
            Marshal.ReleaseComObject(excel);
            Marshal.ReleaseComObject(worksheet);

            strExcelName.OpenFile();
            #endregion
            this.HideWaitMessage();
            return true;
        }

        private void RadioBIRSalesReport_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioBIRSalesInv.Checked)
            {
                this.dateInvoice.ReadOnly = true;
                this.txtShipper.ReadOnly = true;
                this.txtGBNo.ReadOnly = true;
                this.txtbrand.ReadOnly = true;
            }
            else
            {
                this.dateInvoice.ReadOnly = false;
                this.txtShipper.ReadOnly = false;
                this.txtGBNo.ReadOnly = false;
                this.txtbrand.ReadOnly = false;
            }
        }

        private void ChkOutStanding_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkOutStanding.Checked)
            {
                this.lblInvDate.Text = "On Board Date";
            }
            else
            {
                this.lblInvDate.Text = "Invoice Date";
            }
        }
    }
}
