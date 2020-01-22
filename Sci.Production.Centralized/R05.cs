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
            this.comboCentralizedZone1.SetDefalutIndex();
            this.comboCentralizedFactory1.SetDefalutIndex(Sci.Env.User.Factory, true);

            MyUtility.Tool.SetupCombox(this.cmbDate, 2, 1, "1,SCI Delivery Date,2,Buyer Delivery Date");
            this.cmbDate.SelectedValue = "1";
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
        private List<DataTable> Detaildt;
        private List<DataTable> Summarydt;

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            this.Year = MyUtility.Convert.GetInt(this.numYear.Value);
            this.Brand = this.txtbrand1.Text;
            this.M = this.comboCentralizedM1.Text;
            this.Zone = this.comboCentralizedZone1.Text;
            this.Factory = this.comboCentralizedFactory1.Text;
            this.Date = MyUtility.Convert.GetString(this.cmbDate.SelectedValue);
            this.Order = this.chkOrder.Checked;
            this.Forecast = this.chkForecast.Checked;
            this.FtyLocalOrder = this.chkFtyLocalOrder.Checked;
            this.ExcludeSampleFactory = this.chkExcludeSampleFactory.Checked;

            if (MyUtility.Check.Empty(this.Year) || MyUtility.Check.Empty(this.M))
            {
                MyUtility.Msg.WarningBox("Please input <Year> and <M> first!");
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
            this.Detaildt = new List<DataTable>();
            this.Summarydt = new List<DataTable>();
            #region where
            string where = string.Empty;
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
                where += $@" and o.MDivisionID = '{this.M}'";
            }

            if (!MyUtility.Check.Empty(this.Zone))
            {
                where += $@" and SCIFty.zone = '{this.Zone}'";
            }

            if (!MyUtility.Check.Empty(this.Factory))
            {
                where += $@" and o.FtyGroup = '{this.Factory}'";
            }

            if (this.ExcludeSampleFactory)
            {
                where += $@" and SCIFty.Type <> 'S'";
            }

            #endregion
            #region sqlcmd
            string sqlCmd = $@"
select
	o.MDivisionID,
	o.FtyGroup,
	o.BuyerDelivery,
	o.SciDelivery,
	[Date]=format(iif('{this.Date}'='1',dateadd(day,-7,o.SciDelivery),o.BuyerDelivery),'yyyyMM'),
	o.ID,
	Category =case when o.Category='B' then 'Bulk'
				when o.Category='S' then 'Sample'
				when o.Category='' then 'Forecast'
				end,
	Cancelled=iif(o.Junk=1,'Y',''),
	o.StyleID,
	o.SeasonID,
	o.CustPONO,
	o.BrandID,
	o.CPU,
	o.Qty,
	o.FOCQty,
	TotalCPU=isnull(o.CPU,0) * isnull(o.Qty,0),
	TotalSewingOutput=isnull(s.QAQty,0),
	BalanceQty=isnull(o.Qty,0)-isnull(s.QAQty,0),
	BalanceCPU=(isnull(o.Qty,0)-isnull(s.QAQty,0))* isnull(o.CPU,0),
	o.SewLine,
	o.Dest,
	o.OrderTypeID,
	o.ProgramID,
	o.CdCodeID,
	CDCode.ProductionFamilyID
from Orders o with(nolock)
inner join Factory f with(nolock) on f.ID = o.FactoryID and f.junk = 0
left join SCIFty with(nolock) on SCIFty.ID = o.FactoryID
left join CDCode with(nolock) on CDCode.ID = o.CdCodeID
outer apply(select QAQty = dbo.getMinCompleteSewQty(o.ID,null,null) )s
where 1=1
{where}
";
            string sqlCmd2 = $@"
select
	o.FtyGroup,
	[Date]=format(iif('{this.Date}'='1',dateadd(day,-7,o.SciDelivery),o.BuyerDelivery),'yyyyMM'),
	o.ID,
	TotalCPU=isnull(o.CPU,0) * isnull(o.Qty,0),
	s.OutputDate,
	OutputCPU=isnull(s.QAQty,0)*isnull(o.CPU,0)
from Orders o with(nolock)
inner join Factory f with(nolock) on f.ID = o.FactoryID and f.junk = 0
left join SCIFty with(nolock) on SCIFty.ID = o.FactoryID
left join CDCode with(nolock) on CDCode.ID = o.CdCodeID
outer apply(
	select OutputDate=format(x.OutputDate,'yyyyMM'),QAQty=sum(x.QAQty)
	from dbo.[getMinCompleteSewQtyByDate] (o.id,null,null,1)x
	group by format(x.OutputDate,'yyyyMM')
)s
where 1=1
and f.IsProduceFty = 1
{where}
";
            string sqlCmd3 = $@"
select j.FtyGroup,j.OutputDate,OutputCPU=sum(j.OutputCPU)
from(
	select
		o.FtyGroup,
		s.OutputDate,
		OutputCPU=isnull(s.QAQty,0)*isnull(o.CPU,0)*(-1)
	from Orders o with(nolock)
    inner join Factory f with(nolock) on f.ID = o.FactoryID and f.junk = 0
	left join SCIFty with(nolock) on SCIFty.ID = o.FactoryID
	left join CDCode with(nolock) on CDCode.ID = o.CdCodeID
	outer apply(
		select OutputDate=format(x.OutputDate,'yyyyMM'),QAQty=sum(x.QAQty)
		from dbo.[getMinCompleteSewQtyByDate] (o.id,null,null,1)x
		group by format(x.OutputDate,'yyyyMM')
	)s
	where o.Junk=1
	and OutputDate is not null
	{where}
)j
group by j.FtyGroup,j.OutputDate
";
            #endregion

            string sqlOrder = sqlCmd + " And o.Junk = 0 and o.Qty > 0  And o.Category in ('B','S')  and (o.localorder = 0 or o.SubconInType=2)";
            string sqlForecast = sqlCmd + "And o.Qty > 0 AND o.IsForecast = 1 and (o.localorder = 0 or o.SubconInType=2)";
            string sqlFtyLocalOrder = sqlCmd + " AND o.LocalOrder = 1 ";
            string sqlOrder2 = sqlCmd2 + " And o.Junk = 0 and o.Qty > 0  And o.Category in ('B','S')  and (o.localorder = 0 or o.SubconInType=2)";
            string sqlForecast2 = sqlCmd2 + "And o.Qty > 0 AND o.IsForecast = 1 and (o.localorder = 0 or o.SubconInType=2)";
            string sqlFtyLocalOrder2 = sqlCmd2 + " AND o.LocalOrder = 1 ";
            List<string> sqlCmdlist = new List<string>();
            List<string> sqlCmdlist2 = new List<string>();

            if (this.Order)
            {
                sqlCmdlist.Add(sqlOrder);
                sqlCmdlist2.Add(sqlOrder2);
            }

            if (this.Forecast)
            {
                sqlCmdlist.Add(sqlForecast);
                sqlCmdlist2.Add(sqlForecast2);
            }

            if (this.FtyLocalOrder)
            {
                sqlCmdlist.Add(sqlFtyLocalOrder);
                sqlCmdlist2.Add(sqlFtyLocalOrder2);
            }

            string sql = string.Join(" union all ", sqlCmdlist) + string.Join(" union all ", sqlCmdlist2) + sqlCmd3;

            #region --由 appconfig 抓各個連線路徑
            this.SetLoadingText("Load connections... ");
            XDocument docx = XDocument.Load(Application.ExecutablePath + ".config");
            string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
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
                    result = DBProxy.Current.SelectByConn(conn, sql, null, out this.printData);
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

            List<string> ftys = this.dtAllData[0].AsEnumerable().Where(w => !MyUtility.Check.Empty(w["FtyGroup"])).Select(s => MyUtility.Convert.GetString(s["FtyGroup"])).Distinct().ToList();

            foreach (string fty in ftys)
            {
                DataTable detail = this.dtAllData[0].Select($"FtyGroup = '{fty}'").CopyToDataTable();
                this.Detaildt.Add(detail);

                #region summary
                string sqlsum = $@"
select Date,ID,TotalCPU,BalanceCPU=TotalCPU-sum(OutputCPU)
into #tmp2_0
from #tmp
where Ftygroup = '{fty}'
group by Date,ID,TotalCPU

select Date,TotalCPU=sum(TotalCPU),BalanceCPU=sum(BalanceCPU)
into #tmp2
from #tmp2_0
group by Date

declare @col nvarchar(max)=(select stuff((
select concat(',[', OutputDate,']')
from(
	select distinct OutputDate
	from #tmp
	where OutputDate<>''
    and Ftygroup = '{fty}'
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
    and Ftygroup = '{fty}'
)x
order by OutputDate
for xml path('')
),1,1,''))

declare @sql nvarchar(max)=N'
select Date=SUBSTRING(t2.Date,1,4)+''/''+SUBSTRING(t2.Date,5,6),
	Loading=t2.TotalCPU,Balance=t2.BalanceCPU,
	'+@col+N'
into #tmp3
from #tmp2 t2
inner join
(
	select*
	from(
		select
		Date=SUBSTRING(t.Date,1,4)+''/''+SUBSTRING(t.Date,5,6),
		OutputCPU,
		OutputDate
		from #tmp t
        where Ftygroup = ''{fty}''
	)x
	pivot(sum(OutputCPU) for OutputDate in('+@col+N'))xx
)xxx on SUBSTRING(t2.Date,1,4)+''/''+SUBSTRING(t2.Date,5,6)=xxx.Date
order by t2.Date

select*from #tmp3
union all
select ''Total'' ,null, sum(Balance),'+@col2+' from #tmp3
'
exec (@sql)

if @sql is null
begin
	select Date='',Loading=null,Balance=null,[ ]=''
end

drop table #tmp,#tmp2,#tmp2_0
";
                DataTable ftySummarydt;
                DualResult dual = MyUtility.Tool.ProcessWithDatatable(this.dtAllData[1], "FtyGroup,Date,ID,TotalCPU,OutputCPU,OutputDate", sqlsum, out ftySummarydt);
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
    and Ftygroup = '{fty}'
)x
order by OutputDate
for xml path('')
),1,1,''))

declare @sql nvarchar(max)=N'
select  a.*,'+@col+N'
into #tmp3_junk
from(select Date=''Cancel order'',Loading=null,Balance=null)a
outer apply (
	select*
	from(
		select
			OutputCPU,
			OutputDate
		from #tmp t
        where Ftygroup = ''{fty}''
	)x
	pivot(sum(OutputCPU) for OutputDate in('+@col+N'))xx
)b

select*from #tmp3_junk
'
exec (@sql)
if @sql is null
begin
	select Date='Cancel order',Loading=null,Balance=null
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
            for (int j = 1; j < this.Detaildt.Count; j++)
            {
                Excel.Worksheet worksheet1 = (Excel.Worksheet)excelApp.ActiveWorkbook.Worksheets[1];
                Excel.Worksheet worksheet3 = (Excel.Worksheet)excelApp.ActiveWorkbook.Worksheets[j * 2 + 1];
                worksheet1.Copy(worksheet3);

                Excel.Worksheet worksheet2 = (Excel.Worksheet)excelApp.ActiveWorkbook.Worksheets[2];
                Excel.Worksheet worksheet4 = (Excel.Worksheet)excelApp.ActiveWorkbook.Worksheets[j * 2 + 2];
                worksheet2.Copy(worksheet4);
            }

            for (int j = 1; j <= this.Detaildt.Count; j++)
            {
                worksheet = excelApp.ActiveWorkbook.Worksheets[j * 2 - 1];
                worksheet.Name = MyUtility.Convert.GetString(this.Detaildt[j - 1].Rows[0]["FtyGroup"]) + "-Summary";
                MyUtility.Excel.CopyToXls(this.Summarydt[j - 1], string.Empty, xltfile: excelFile, headerRow: 4, excelApp: excelApp, wSheet: excelApp.Sheets[j * 2 - 1], showExcel: false, DisplayAlerts_ForSaveFile: true);
                worksheet.Cells[1, 1] = "Fty:" + MyUtility.Convert.GetString(this.Detaildt[j - 1].Rows[0]["FtyGroup"]);

                int i = 1;
                foreach (DataColumn col in this.Summarydt[j - 1].Columns)
                {
                    if (i > 3)
                    {
                        worksheet.Cells[4, i] = col.ColumnName;
                    }

                    i++;
                }

                i--;
                worksheet.get_Range((Excel.Range)worksheet.Cells[3, 4], (Excel.Range)worksheet.Cells[3, i]).Merge(false);
                worksheet.get_Range((Excel.Range)worksheet.Cells[3, 1], (Excel.Range)worksheet.Cells[this.Summarydt[j - 1].Rows.Count + 4, i]).Borders.Weight = 3; // 設定全框線

                MyUtility.Excel.CopyToXls(this.Detaildt[j - 1], string.Empty, xltfile: excelFile, headerRow: 1, excelApp: excelApp, wSheet: excelApp.Sheets[j * 2], showExcel: false, DisplayAlerts_ForSaveFile: true);
                worksheet = excelApp.ActiveWorkbook.Worksheets[j * 2 - 1]; // 取得工作表
                worksheet.Columns.AutoFit();
                worksheet.Columns[1].ColumnWidth = 12;

                worksheet = excelApp.ActiveWorkbook.Worksheets[j * 2]; // 取得工作表
                worksheet.Name = MyUtility.Convert.GetString(this.Detaildt[j - 1].Rows[0]["FtyGroup"]) + "-Balance Detail";
                worksheet.Columns[1].ColumnWidth = 5.5;
                worksheet.Columns[2].ColumnWidth = 11.13;
                worksheet.Columns[3].ColumnWidth = 11.88;
                worksheet.Columns[4].ColumnWidth = 11.88;
                worksheet.Columns[5].ColumnWidth = 7.88;
                worksheet.Columns[6].ColumnWidth = 17.75;
                worksheet.Columns[7].ColumnWidth = 12.75;
                worksheet.Columns[8].ColumnWidth = 14;
                worksheet.Columns[9].ColumnWidth = 25;
                worksheet.Columns[10].ColumnWidth = 11.13;
                worksheet.Columns[11].ColumnWidth = 25.25;
                worksheet.Columns[12].ColumnWidth = 20;
                worksheet.Columns[13].ColumnWidth = 7.63;
                worksheet.Columns[14].ColumnWidth = 13.38;
                worksheet.Columns[15].ColumnWidth = 11.88;
                worksheet.Columns[16].ColumnWidth = 15.25;
                worksheet.Columns[17].ColumnWidth = 25.75;
                worksheet.Columns[18].ColumnWidth = 16.38;
                worksheet.Columns[19].ColumnWidth = 17.5;
                worksheet.Columns[20].ColumnWidth = 18.13;
                worksheet.Columns[21].ColumnWidth = 8;
                worksheet.Columns[22].ColumnWidth = 22;
                worksheet.Columns[23].ColumnWidth = 16.38;
                worksheet.Columns[24].ColumnWidth = 6.5;
                worksheet.Columns[25].ColumnWidth = 19.88;
            }

            excelApp.Visible = true;
            this.HideWaitMessage();
            return true;
        }
    }
}
