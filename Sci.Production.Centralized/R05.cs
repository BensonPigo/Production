using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Ict;
using Sci.Data;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text;

namespace Sci.Production.Centralized
{
    public partial class R05 : Win.Tems.PrintForm
    {
        public R05(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.numYear.Value = MyUtility.Convert.GetDecimal(DateTime.Now.Year);
            this.comboCentralizedM1.SetDefalutIndex(Env.User.Keyword);
            this.comboCentralizedFactory1.SetDefalutIndex(Env.User.Factory, true);

            MyUtility.Tool.SetupCombox(this.cmbDate, 2, 1, "1,SCI Delivery Date,2,Buyer Delivery Date");
            this.cmbDate.SelectedValue = "1";
            this.comboFtyZone.IsIncludeSampleRoom = false;
            this.comboFtyZone.SetDataSourceAllFty();
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
        private bool IncludeCancelOrder;

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
            this.IncludeCancelOrder = this.chkIncludeCancelOrder.Checked;

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
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            DBProxy.Current.DefaultTimeout = 1800;  // timeout時間改為30分鐘
            this.dtAllData = null;
            this.Summarydt = new List<DataTable>();
            this.listFtyZone = new List<string>();
            string smmmaryDateCol = this.radioMonthly.Checked ? "SUBSTRING(Date,1,4)+'/'+SUBSTRING(Date,5,6)" : "DateByHalfMonth";
            StringBuilder sqlcmdSP = new StringBuilder();

            sqlcmdSP.Append("exec dbo.GetProductionOutputSummary");

            #region where條件
            sqlcmdSP.Append(!MyUtility.Check.Empty(this.Year) ? $" '{this.Year}'," : "'',"); // Year
            sqlcmdSP.Append(!MyUtility.Check.Empty(this.Brand) ? $" '{this.Brand}'," : "'',"); // Brand
            sqlcmdSP.Append(!MyUtility.Check.Empty(this.M) ? $" '{this.M}'," : "'',"); // Mdivision
            sqlcmdSP.Append(!MyUtility.Check.Empty(this.Factory) ? $" '{this.Factory}'," : "'',"); // Factory
            sqlcmdSP.Append(!MyUtility.Check.Empty(this.Zone) ? $" '{this.Zone}'," : "'',"); // Zone
            sqlcmdSP.Append($" {this.Date},"); // DateType
            sqlcmdSP.Append(this.Order ? $" 1," : "0,"); // ChkOrder
            sqlcmdSP.Append(this.Forecast ? $" 1," : "0,"); // ChkForecast
            sqlcmdSP.Append(this.FtyLocalOrder ? $" 1," : "0,"); // ChkFtylocalOrder
            sqlcmdSP.Append(this.ExcludeSampleFactory ? $" 1," : "0,"); // ExcludeSampleFactory
            sqlcmdSP.Append(this.radioMonthly.Checked ? $" 1," : "0,"); // ChkMonthly
            sqlcmdSP.Append(this.chkIncludeCancelOrder.Checked ? $" 1," : "0,"); // @IncludeCancelOrder
            sqlcmdSP.Append($" 0,"); // IsFtySide 工廠端限制ForeCast單 僅顯示SCI delivery or buyer delivery 小於等於 當月份+4個月的月底+7天
            sqlcmdSP.Append($" 0,"); // @IsPowerBI
            sqlcmdSP.Append(this.chkCMPLockDate.Checked ? $" 1," : "0,"); // @IsByCMPLockDate
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
                    result = DBProxy.Current.SelectByConn(conn, sqlcmdSP.ToString().Substring(0, sqlcmdSP.Length - 1), null, out this.printData);
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
                return Ict.Result.F("Data not found!");
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
    , [SeqDate] = Cast(isnull(Replace(Date, '/', ''),'0') as int)
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
select t2.SeqDate,t2.Date
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
order by t2.SeqDate

select  Date
        ,Loading
        ,Canceled
        ,Shortage
        ,SubconOut
        ,Balance
        ,BalanceIrregular,
	    '+@col+N' 
from (
        select * from #tmp3 
        union all
        select 999999999,''Total'' ,sum(Loading), sum(Canceled),sum(Shortage), sum(SubconOut), sum(Balance), sum(BalanceIrregular),'+@col2+' from #tmp3
) a order by SeqDate
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
            return Ict.Result.True;
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
            Excel.Application excelApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + excelFile); // 開excelapp

            // excelApp.Visible = true;
            Excel.Worksheet worksheet = excelApp.ActiveWorkbook.Worksheets[1];
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
