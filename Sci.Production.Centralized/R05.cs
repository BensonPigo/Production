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
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Centralized
{
    public partial class R05 : Sci.Win.Tems.PrintForm
    {
        public R05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.numYear.Value = MyUtility.Convert.GetDecimal(DateTime.Now.Year);
            this.comboCentralizedM1.SetDefalutIndex(Sci.Env.User.Keyword);
            this.comboCentralizedFactory1.SetDefalutIndex(Sci.Env.User.Factory, true);

            MyUtility.Tool.SetupCombox(this.cmbDate, 2, 1, "1,SCI Delivery Date,2,Buyer Delivery Date");
            this.cmbDate.SelectedValue = "1";
            this.comboFtyZone.IsIncludeSampleRoom = false;
            this.comboFtyZone.setDataSourceAllFty();
        }

        private int Year;
        private string Brand;
        private string M;
        private string Zone;
        private string Factory;
        private string Date;
        private bool Order;
        private bool Forecast;
        private bool FtyLocalOrder;
        private bool ExcludeSampleFactory;
        private DataTable[] printData;
        private DataTable[] dtAllData;
        private List<DataTable> Summarydt;
        private List<string> listFtyZone;
        private DataTable dtAllDetail;

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.Year = MyUtility.Convert.GetInt(this.numYear.Value);
            this.Brand = this.txtbrand1.Text;
            this.M = this.comboCentralizedM1.Text;
            this.Zone = this.comboFtyZone.Text;
            this.Factory = this.comboCentralizedFactory1.Text;
            this.Date = MyUtility.Convert.GetString(this.cmbDate.SelectedValue);
            this.Order = this.chkOrder.Checked;
            this.Forecast = this.chkForecast.Checked;
            this.FtyLocalOrder = this.chkFtyLocalOrder.Checked;
            this.ExcludeSampleFactory = this.chkExcludeSampleFactory.Checked;

            if (MyUtility.Check.Empty(this.Year))
            {
                MyUtility.Msg.WarningBox("Please input <Year> first!");
                return false;
            }

            if (!this.Order && !this.Forecast && !this.FtyLocalOrder)
            {
                MyUtility.Msg.WarningBox("Order, Forecast, Fty Local Order must select one at least.");
                return false;
            }

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DBProxy.Current.DefaultTimeout = 1800;  // timeout時間改為30分鐘
            this.dtAllData = null;
            this.Summarydt = new List<DataTable>();
            this.listFtyZone = new List<string>();
            string smmmaryDateCol = this.radioMonthly.Checked ? "SUBSTRING(Date,1,4)+'/'+SUBSTRING(Date,5,6)" : "DateByHalfMonth";
            #region where
            string where = string.Empty;
            string whereFty = string.Empty;
            if (!MyUtility.Check.Empty(this.Year) && this.Date == "1")
            {
                where += $@" and cast(dateadd(day,-7,o.SciDelivery) as date) between '{this.Year}/01/01' and '{this.Year}/12/31'";
            }

            if (!MyUtility.Check.Empty(this.Year) && this.Date == "2")
            {
                where += $@" and cast(o.BuyerDelivery as date) between '{this.Year}/01/01' and '{this.Year}/12/31'";
            }

            if (!MyUtility.Check.Empty(this.Brand))
            {
                where += $@" and o.BrandID='{this.Brand}'";
            }

            if (!MyUtility.Check.Empty(this.M))
            {
                whereFty += $@" and o.MDivisionID = '{this.M}'";
            }

            if (!MyUtility.Check.Empty(this.Zone))
            {
                whereFty += $@" and SCIFty.FtyZone = '{this.Zone}'";
            }

            if (!MyUtility.Check.Empty(this.Factory))
            {
                whereFty += $@" and o.FtyGroup = '{this.Factory}'";
            }

            if (this.ExcludeSampleFactory)
            {
                whereFty += $@" and SCIFty.Type <> 'S'";
            }

            #endregion

            #region Source Type Where
            List<string> listWhereSource = new List<string>();
            if (this.Order)
            {
                listWhereSource.Add(" ( Category in ('B','S')  and (localorder = 0 or SubconInType=2)) ");
            }

            if (this.Forecast)
            {
                listWhereSource.Add(" ( IsForecast = 1 and (localorder = 0 or SubconInType=2)) ");
            }

            if (this.FtyLocalOrder)
            {
                listWhereSource.Add(" (LocalOrder = 1 and SubconInType <> 1) ");
            }

            string whereSource = listWhereSource.JoinToString("or");
            #endregion

            #region sqlcmd
            string sqlCmd = $@"

select  o.ID,o.FactoryID,[TransFtyZone] = ''
into    #tmpBaseOrderID
from Orders o with(nolock)
inner join Factory f with(nolock) on f.ID = o.FactoryID and f.junk = 0
left join SCIFty with(nolock) on SCIFty.ID = o.FactoryID
where   IsProduceFty = 1
        {where + whereFty}

--代工的訂單，以ProgramID抓出自己工廠的資料，後續要顯示在detail並扣除summary的資料
select  o.ID,[TransFtyZone] = f.FtyZone
into    #tmpBaseTransOrderID
from Orders o with(nolock)
left join Factory f with(nolock) on f.ID = o.ProgramID and f.junk = 0
where   o.LocalOrder = 1 and o.SubconInType = 2 {where} and
        o.ProgramID in (select distinct FactoryID from #tmpBaseOrderID)

select * into #tmpBaseStep1
from (
    select  ID,TransFtyZone from #tmpBaseOrderID where ID not in (select ID from #tmpBaseTransOrderID)
    union all
    select  ID,TransFtyZone from #tmpBaseTransOrderID
) a


select
    o.ID,
    [Date]= format(iif('{this.Date}' = '1', KeyDate.SCI, KeyDate.Buyer), 'yyyyMM'),
    [SCIKey] = format(KeyDate.SCI, 'yyyyMM'),
    [SCIKeyHalf] = iif(cast(format(KeyDate.SCI, 'dd') as int) between 1 and 15, format(KeyDate.SCI, 'yyyyMM01'), format(KeyDate.SCI, 'yyyyMM02')),
    [BuyerKey] = format(KeyDate.Buyer, 'yyyyMM'),
    [BuyerKeyHalf] = iif(cast(format(KeyDate.Buyer, 'dd') as int) between 1 and 15, format(KeyDate.Buyer, 'yyyyMM01'), format(KeyDate.Buyer, 'yyyyMM02')),
    [OutputDate] = FORMAT(s.OutputDate,'yyyyMM'),
    [OrderCPU] = o.Qty * gcRate.CpuRate * o.CPU,
    [OrderShortageCPU] = iif(o.GMTComplete = 'S' ,(o.Qty - GetPulloutData.Qty)  * gcRate.CpuRate * o.CPU ,0),
    [SewingOutput] = isnull(sum(isnull(sdd.QAQty,0) * isnull(ol.Rate, sl.Rate)),0) / 100,
    [SewingOutputCPU] = isnull(sum(isnull(sdd.QAQty,0) * isnull(ol.Rate, sl.Rate)),0) * gcRate.CpuRate * o.CPU / 100,
    o.Junk,
    o.Qty,
    o.Category,
    o.SubconInType,
    o.IsForecast,
    o.LocalOrder,
    o.FactoryID,
    o.FtyGroup,
    f.IsProduceFty,
    f.FtyZone,
    o.ProgramID,
    tbs.TransFtyZone,
    [IsCancelNeedProduction] = iif(o.Junk = 1 and o.NeedProduction = 1, 'Y' , 'N')
into #tmpBase
from #tmpBaseStep1 tbs
inner join Orders o with(nolock) on o.ID = tbs.ID
inner join Factory f with(nolock) on f.ID = o.FactoryID and f.junk = 0
left join SewingOutput_Detail_Detail sdd with (nolock) on o.ID = sdd.OrderId
left join SewingOutput s with (nolock) on sdd.ID = s.ID
left join Order_Location ol with (nolock) on ol.OrderId = sdd.OrderId and ol.Location = sdd.ComboType
left join Style_Location sl with (nolock) on sl.StyleUkey = o.StyleUkey and sl.Location = sdd.ComboType
outer apply (select [CpuRate] = case when o.IsForecast = 1 then (select CpuRate from GetCPURate(o.OrderTypeID, o.ProgramID, o.Category, o.BrandID, 'O'))
                                     when o.LocalOrder = 1 then 1
                                     else (select CpuRate from GetCPURate(o.OrderTypeID, o.ProgramID, o.Category, o.BrandID, 'O')) end
                     ) gcRate
outer apply (select Qty=sum(shipQty) from Pullout_Detail where orderid = o.id) GetPulloutData
outer apply (select [SCI] = dateadd(day,-7,o.SciDelivery),
                    [Buyer] = o.BuyerDelivery) KeyDate
group by o.ID,
KeyDate.SCI,
KeyDate.Buyer,
FORMAT(s.OutputDate,'yyyyMM'), 
o.CPU, 
o.Qty,
o.Junk,
o.NeedProduction,
o.Qty,
o.Category,
o.SubconInType,
o.IsForecast,
o.LocalOrder,
o.FactoryID,
o.FtyGroup,
f.IsProduceFty,
gcRate.CpuRate,
GetPulloutData.Qty,
o.GMTComplete,
f.FtyZone,
o.ProgramID,
tbs.TransFtyZone

select  ID,
        FactoryID,
        FtyGroup,
        Date,
        SCIKey,
        SCIKeyHalf,
        BuyerKey,
        BuyerKeyHalf,
        OutputDate,
        OrderCPU,
        OrderShortageCPU,
        SewingOutput,
        SewingOutputCPU,
        IsProduceFty,
        --summary頁面不算Junk訂單使用，Forecast沒排掉是因為Planning R10有含
        [isNormalOrderCanceled] = iif(  Junk = 1 and 
                                        --正常訂單
                                        (( Category in ('B','S')  and (localorder = 0 or SubconInType=2)) or
                                        --當地訂單
                                        (LocalOrder = 1 )),1,0),
        FtyZone,
        ProgramID,
        TransFtyZone,
        IsCancelNeedProduction,
        [DateByHalfMonth] = iif('{this.Date}' = '1', SCIKeyHalf, BuyerKeyHalf)
into #tmpBaseBySource
from #tmpBase
where   {whereSource} or TransFtyZone <> ''

select
    ID,
    Date,
    OrderCPU,
    OrderShortageCPU,
    [SewingOutput] = SUM(SewingOutput),
    [SewingOutputCPU] = SUM(SewingOutputCPU),
    FtyZone,
    TransFtyZone,
    IsCancelNeedProduction,
    SCIKey,
    SCIKeyHalf,
    BuyerKey,
    BuyerKeyHalf
into #tmpBaseByOrderID
from #tmpBaseBySource
group by ID,Date,OrderCPU,OrderShortageCPU,FtyZone,TransFtyZone,IsCancelNeedProduction,SCIKey,SCIKeyHalf,BuyerKey,BuyerKeyHalf

select  oq.ID, [LastBuyerDelivery] = Max(oq.BuyerDelivery), [PartialShipment] = iif(count(1) > 1, 'Y', '')
into #tmpOrder_QtyShip
from    Order_QtyShip oq with (nolock)
where exists (select 1 from #tmpBaseByOrderID tb where tb.ID = oq.ID)
group by    oq.ID

select  pd.OrderID, [PulloutQty] = sum(pd.shipQty)
into #tmpPullout_Detail
from Pullout_Detail pd with (nolock)
where exists (select 1 from #tmpBaseByOrderID tb where tb.ID = pd.OrderID)
group by pd.OrderID

select
	o.MDivisionID,
    tb.FtyZone,
	o.FactoryID,
	o.BuyerDelivery,
	o.SciDelivery,
    tb.SCIKey,
    tb.SCIKeyHalf,
    tb.BuyerKey,
    tb.BuyerKeyHalf,
	o.ID,
	Category =case when o.Category='B' then 'Bulk'
				when o.Category='S' then 'Sample'
				when o.Category='' then 'Forecast'
				end,
	Cancelled=iif(o.Junk=1,'Y',''),
    tb.IsCancelNeedProduction,
    toq.PartialShipment,
    toq.LastBuyerDelivery,
	o.StyleID,
	o.SeasonID,
	o.CustPONO,
	o.BrandID,
	o.CPU,
	o.Qty,
	o.FOCQty,
    tpd.PulloutQty,
    tb.OrderShortageCPU,
	[TotalCPU] = TotalCPU.val,
	tb.SewingOutput,
    tb.SewingOutputCPU,
	BalanceQty = isnull(o.Qty,0)-isnull(tb.SewingOutput,0),
	[BalanceCPU] = iif(BalanceCPU.val >= 0, BalanceCPU.val, null),
    BalanceCPUIrregular = iif(BalanceCPU.val >= 0, null, BalanceCPU.val),
	o.SewLine,
	o.Dest,
	o.OrderTypeID,
	o.ProgramID,
	o.CdCodeID,
	CDCode.ProductionFamilyID,
    o.FtyGroup,
    [PulloutComplete] = iif(o.PulloutComplete = 1, 'OK', ''),
    o.SewInLine,
    o.SewOffLine,
    tb.TransFtyZone
from #tmpBaseByOrderID tb with(nolock)
inner join Orders o with(nolock) on o.id = tb.ID
left join #tmpOrder_QtyShip toq on toq.ID = tb.ID
left join #tmpPullout_Detail tpd on tpd.OrderID = tb.ID
left join CDCode with(nolock) on CDCode.ID = o.CdCodeID
outer apply (select [val] = iif(tb.IsCancelNeedProduction = 'N' and o.Junk = 1, 0, isnull(tb.OrderCPU, 0))) TotalCPU
outer apply (select [val] =  TotalCPU.val - isnull(tb.SewingOutputCPU, 0) - isnull(tb.OrderShortageCPU, 0)) BalanceCPU

select  FtyGroup,
		[Date] = {smmmaryDateCol},
		ID,
		OutputDate,
		[OrderCPU] = iif(IsCancelNeedProduction = 'N' and isNormalOrderCanceled = 1,0 ,OrderCPU - OrderShortageCPU),
		[CanceledCPU] = iif(IsCancelNeedProduction = 'Y',OrderCPU, 0),
		OrderShortageCPU,
		SewingOutput,
		SewingOutputCPU,
		FtyZone,
		TransFtyZone 
from #tmpBaseBySource 

select  FtyGroup,OutputDate,[SewingOutputCPU] = sum(SewingOutputCPU) * -1,FtyZone
from    #tmpBase
where   Junk=1 and IsCancelNeedProduction = 'N' and OutputDate is not null 
group by FtyGroup,OutputDate,FtyZone

drop table #tmpBaseOrderID,#tmpBaseByOrderID,#tmpBaseTransOrderID,#tmpBaseStep1,#tmpBase,#tmpBaseBySource,#tmpOrder_QtyShip,#tmpPullout_Detail
";
            #endregion

            #region --由 appconfig 抓各個連線路徑
            this.SetLoadingText("Load connections... ");
            XDocument docx = XDocument.Load(Application.ExecutablePath + ".config");
            string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' }).Where(s => !s.Contains("testing_PMS")).ToArray();
            List<string> connectionString = new List<string>(); // ←主要是要重組 List connectionString
            foreach (string ss in strSevers)
            {
                if (!MyUtility.Check.Empty(ss))
                {
                    var connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.Contains(ss.Split(new char[] { ':' })[0].ToString())).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("Production")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();
                    connectionString.Add(connections);
                }
            }

            if (connectionString == null || connectionString.Count == 0)
            {
                return new DualResult(false, "no connection loaded.");
            }
            #endregion

            DualResult result = new DualResult(true);

            foreach (string conString in connectionString)
            {
                SqlConnection conn;
                using (conn = new SqlConnection(conString))
                {
                    conn.Open();
                    result = DBProxy.Current.SelectByConn(conn, sqlCmd, null, out this.printData);
                    if (!result)
                    {
                        DualResult failResult = new DualResult(false, "Query data fail\r\n" + result.ToString());
                        return failResult;
                    }

                    if (this.printData != null && this.printData[0].Rows.Count > 0)
                    {
                        if (this.dtAllData == null)
                        {
                            this.dtAllData = this.printData;
                        }
                        else
                        {
                            this.dtAllData[0].Merge(this.printData[0]);
                            this.dtAllData[1].Merge(this.printData[1]);
                            this.dtAllData[2].Merge(this.printData[2]);
                        }
                    }
                }
            }

            if (this.dtAllData == null || this.dtAllData[0].Rows.Count == 0)
            {
                return Result.F("Data not found!");
            }

            var allDetail = this.dtAllData[0].AsEnumerable().Where(w => !MyUtility.Check.Empty(w["FtyZone"]));
            this.dtAllDetail = allDetail.CopyToDataTable();
            this.listFtyZone = allDetail.Where(s => MyUtility.Check.Empty(s["TransFtyZone"])).Select(s => MyUtility.Convert.GetString(s["FtyZone"])).Distinct().ToList();

            foreach (string ftyZone in this.listFtyZone)
            {
                #region summary
                string sqlsum = $@"
select Date
    , ID
    , OrderCPU
    , CanceledCPU = iif(TransFtyZone = '{ftyZone}', 0,  CanceledCPU)
    , BalanceCPU = iif(TransFtyZone = '{ftyZone}', 0, OrderCPU-sum(SewingOutputCPU))
    , OrderShortageCPU = iif(TransFtyZone = '{ftyZone}', 0,  OrderShortageCPU)
    , TransFtyZone
into #tmp2_0
from #tmp
where (FtyZone = '{ftyZone}' or TransFtyZone = '{ftyZone}')
group by Date,ID,OrderCPU,CanceledCPU,OrderShortageCPU, TransFtyZone

select  Date
    , OrderCPU=sum(iif(TransFtyZone = '{ftyZone}', -OrderCPU, OrderCPU))
    , CanceledCPU = sum(CanceledCPU)
    , BalanceCPU = sum(iif(BalanceCPU >= 0, BalanceCPU, 0))
    , BalanceIrregularCPU = sum(iif(BalanceCPU >= 0, 0, BalanceCPU))
    , [OrderShortageCPU] = sum(OrderShortageCPU)
    , [SubconOutCPU] = sum(iif(TransFtyZone = '{ftyZone}', OrderCPU, 0))
into #tmp2
from #tmp2_0
group by Date

declare @col nvarchar(max)=(select stuff((
select concat(',[', OutputDate,']')
from(
	select distinct OutputDate
	from #tmp
	where OutputDate<>''
    and FtyZone = '{ftyZone}'
)x
order by OutputDate
for xml path('')
),1,1,''))

declare @col2 nvarchar(max)=(select stuff((
select concat(',[',OutputDate,']=sum([', OutputDate,'])')
from(
	select distinct OutputDate
	from #tmp
	where OutputDate<>''
    and FtyZone = '{ftyZone}'
)x
order by OutputDate
for xml path('')
),1,1,''))

declare @sql nvarchar(max)=N'
select t2.Date
    ,[Loading] = t2.OrderCPU
    ,[Canceled] = t2.CanceledCPU
    ,[Shortage] = t2.OrderShortageCPU
    ,[SubconOut] = t2.SubconOutCPU
    ,[Balance] = t2.BalanceCPU
    ,[BalanceIrregular] = t2.BalanceIrregularCPU,
	'+@col+N'
into #tmp3
from #tmp2 t2
inner join
(
	select*
	from(
		select
		Date,
		SewingOutputCPU,
		OutputDate
		from #tmp t
        where FtyZone = ''{ftyZone}''
	)x
	pivot(sum(SewingOutputCPU) for OutputDate in('+@col+N'))xx
)xxx on t2.Date=xxx.Date
order by t2.Date

select * from #tmp3
union all
select ''Total'' ,sum(Loading), sum(Canceled),sum(Shortage), sum(SubconOut), sum(Balance), sum(BalanceIrregular),'+@col2+' from #tmp3
'
exec (@sql)

if @sql is null
begin
	select Date='',Loading=null,Canceled = null,Shortage = null, SubconOut=null,Balance=null, BalanceIrregular=null,[ ]=''
end

drop table #tmp,#tmp2_0,#tmp2
";
                DataTable ftySummarydt;
                DualResult dual = MyUtility.Tool.ProcessWithDatatable(this.dtAllData[1], "FtyGroup,Date,ID,OrderCPU,SewingOutputCPU,OutputDate,OrderShortageCPU,FtyZone,CanceledCPU,TransFtyZone", sqlsum, out ftySummarydt);
                if (!dual)
                {
                    return dual;
                }

                sqlsum = $@"
declare @col nvarchar(max)=(select stuff((
select concat(',[', OutputDate,']')
from(
	select distinct OutputDate
	from #tmp
	where OutputDate<>''
    and FtyZone = '{ftyZone}'
)x
order by OutputDate
for xml path('')
),1,1,''))

declare @sql nvarchar(max)=N'
select  a.*,'+@col+N'
into #tmp3_junk
from(select Date=''Cancel order'',Loading=null,Shortage = null, Canceled = null,Balance=null)a
outer apply (
	select*
	from(
		select
			SewingOutputCPU,
			OutputDate
		from #tmp t
        where FtyZone = ''{ftyZone}''
	)x
	pivot(sum(SewingOutputCPU) for OutputDate in('+@col+N'))xx
)b

select*from #tmp3_junk
'
exec (@sql)
if @sql is null
begin
	select Date='Cancel order',Loading=null,Shortage = null, Canceled = null,Balance=null
end
drop table #tmp
";
                DataTable junkdt;
                dual = MyUtility.Tool.ProcessWithDatatable(this.dtAllData[2], string.Empty, sqlsum, out junkdt);
                if (!dual)
                {
                    return dual;
                }

                if (ftySummarydt != null)
                {
                    ftySummarydt.ImportRow(junkdt.Rows[0]);
                    this.Summarydt.Add(ftySummarydt);
                }

                #endregion
            }

            DBProxy.Current.DefaultTimeout = 300;  // timeout時間改回5分鐘
            return Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            if (this.dtAllData == null || this.dtAllData[0].Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.SetCount(this.dtAllData[0].Rows.Count);

            this.ShowWaitMessage("Starting EXCEL...");
            string excelFile = "Centralized_R05.xltx";
            Microsoft.Office.Interop.Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + excelFile); // 開excelapp
            //excelApp.Visible = true;

            Microsoft.Office.Interop.Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[1];
            if (this.Date == "1")
            {
                worksheet.Cells[3, 1] = "SCI delivery";
            }
            else
            {
                worksheet.Cells[3, 1] = "Buyer delivery";
            }
            // 複製分頁
            Excel.Worksheet worksheet1 = (Excel.Worksheet)excelApp.ActiveWorkbook.Worksheets[1];
            Excel.Worksheet newSummarySheet;

            this.dtAllDetail.Columns.Remove(this.dtAllDetail.Columns["FtyGroup"]);
            this.dtAllDetail.Columns.Remove(this.dtAllDetail.Columns["TransFtyZone"]);

            for (int j = 1; j < this.listFtyZone.Count; j++)
            {
                newSummarySheet = (Excel.Worksheet)excelApp.ActiveWorkbook.Worksheets[j];
                worksheet1.Copy(newSummarySheet);
            }

            #region detail data
            MyUtility.Excel.CopyToXls(this.dtAllDetail, string.Empty, xltfile: excelFile, headerRow: 1, excelApp: excelApp, wSheet: excelApp.Sheets[this.listFtyZone.Count + 1], showExcel: false, DisplayAlerts_ForSaveFile: true);
            worksheet = excelApp.ActiveWorkbook.Worksheets[this.listFtyZone.Count + 1]; // 取得工作表
            worksheet.Columns.AutoFit();

            worksheet.Columns[1].ColumnWidth = 5.5;
            worksheet.Columns[2].ColumnWidth = 5.5;
            worksheet.Columns[3].ColumnWidth = 11.13;
            worksheet.Columns[4].ColumnWidth = 11.88;
            worksheet.Columns[5].ColumnWidth = 11.88;
            worksheet.Columns[6].ColumnWidth = 7.88;
            worksheet.Columns[7].ColumnWidth = 10;
            worksheet.Columns[8].ColumnWidth = 7.88;
            worksheet.Columns[9].ColumnWidth = 10;
            worksheet.Columns[10].ColumnWidth = 17.75;
            worksheet.Columns[11].ColumnWidth = 12.75;
            worksheet.Columns[12].ColumnWidth = 14;
            worksheet.Columns[13].ColumnWidth = 14;
            worksheet.Columns[14].ColumnWidth = 14;
            worksheet.Columns[15].ColumnWidth = 15;
            worksheet.Columns[16].ColumnWidth = 25;
            worksheet.Columns[17].ColumnWidth = 11.13;
            worksheet.Columns[18].ColumnWidth = 25.25;
            worksheet.Columns[19].ColumnWidth = 20;
            worksheet.Columns[20].ColumnWidth = 7.63;
            worksheet.Columns[21].ColumnWidth = 13.38;
            worksheet.Columns[22].ColumnWidth = 11.88;
            worksheet.Columns[23].ColumnWidth = 15.25;
            worksheet.Columns[24].ColumnWidth = 15.25;
            worksheet.Columns[25].ColumnWidth = 15.25;
            worksheet.Columns[26].ColumnWidth = 25.75;
            worksheet.Columns[27].ColumnWidth = 16.38;
            worksheet.Columns[28].ColumnWidth = 17.5;
            worksheet.Columns[29].ColumnWidth = 17.5;
            worksheet.Columns[30].ColumnWidth = 17.5;
            worksheet.Columns[31].ColumnWidth = 18.13;
            worksheet.Columns[32].ColumnWidth = 8;
            worksheet.Columns[33].ColumnWidth = 22;
            worksheet.Columns[34].ColumnWidth = 16.38;
            worksheet.Columns[35].ColumnWidth = 6.5;
            worksheet.Columns[36].ColumnWidth = 19.88;
            worksheet.Columns[37].ColumnWidth = 16.38;
            worksheet.Columns[38].ColumnWidth = 16.38;
            worksheet.Columns[39].ColumnWidth = 16.38;
            #endregion

            for (int j = 1; j <= this.listFtyZone.Count; j++)
            {
                string ftyZone = MyUtility.Convert.GetString(this.listFtyZone[j - 1]);
                worksheet = excelApp.ActiveWorkbook.Worksheets[j];
                worksheet.Name = ftyZone;
                MyUtility.Excel.CopyToXls(this.Summarydt[j - 1], string.Empty, xltfile: excelFile, headerRow: 4, excelApp: excelApp, wSheet: excelApp.Sheets[j], showExcel: false, DisplayAlerts_ForSaveFile: true);
                worksheet.Cells[1, 1] = "Fty:" + ftyZone;
                int i = 1;
                foreach (DataColumn col in this.Summarydt[j - 1].Columns)
                {
                    if (i > 7)
                    {
                        worksheet.Cells[4, i] = col.ColumnName;
                    }

                    i++;
                }

                i--;
                worksheet.get_Range((Excel.Range)worksheet.Cells[3, 8], (Excel.Range)worksheet.Cells[3, i]).Merge(false);
                worksheet.get_Range((Excel.Range)worksheet.Cells[3, 1], (Excel.Range)worksheet.Cells[this.Summarydt[j - 1].Rows.Count + 4, i]).Borders.Weight = 3; // 設定全框線
                worksheet.Columns.AutoFit();
            }

            int sheetCnt = excelApp.ActiveWorkbook.Worksheets.Count;
            for (int i = 1; i < sheetCnt - 1; i++)
            {
                excelApp.ActiveWorkbook.Worksheets[i].Columns[1].ColumnWidth = 11.25;
            }

            excelApp.Visible = true;
            this.HideWaitMessage();
            return true;
        }
    }
}
