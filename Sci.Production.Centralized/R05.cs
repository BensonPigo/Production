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
            this.comboCentralizedFactory1.SetDefalutIndex(Sci.Env.User.Factory);

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
        private DataTable printData;
        private DataTable dtAllData;
        private DataTable Summarydt;

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
	o.FactoryID,
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
	CDCode.ProductionFamilyID,
	ss.OutputDate,
	OutputCPU=isnull(s.QAQty,0)*isnull(o.CPU,0)
from Orders o with(nolock)
left join SCIFty with(nolock) on SCIFty.ID = o.FactoryID
left join CDCode with(nolock) on CDCode.ID = o.CdCodeID
outer apply(select QAQty = dbo.getMinCompleteSewQty(o.ID,null,null) )s
outer apply(
	select OutputDate=max(so.OutputDate)
	from SewingOutput_Detail sod with(nolock)
	inner join SewingOutput so with(nolock) on so.id = sod.id
	where sod.OrderId = o.ID
)ss
where 1=1
{where}
";
            #endregion

            string sqlOrder = sqlCmd + " And o.Junk = 0 and o.Qty > 0  And o.Category in ('B','S')  and (o.localorder = 0 or o.SubconInType=2)";
            string sqlForecast = sqlCmd + "And o.Qty > 0 AND o.IsForecast = 1 and (o.localorder = 0 or o.SubconInType=2)";
            string sqlFtyLocalOrder = sqlCmd + " AND o.LocalOrder = 1 ";
            List<string> sqlCmdlist = new List<string>();

            if (this.Order)
            {
                sqlCmdlist.Add(sqlOrder);
            }

            if (this.Forecast)
            {
                sqlCmdlist.Add(sqlForecast);
            }

            if (this.FtyLocalOrder)
            {
                sqlCmdlist.Add(sqlFtyLocalOrder);
            }

            string sql = string.Join(" union all ", sqlCmdlist);

            #region --由 appconfig 抓各個連線路徑
            this.SetLoadingText("Load connections... ");
            XDocument docx = XDocument.Load(Application.ExecutablePath + ".config");
            string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
            List<string> connectionString = new List<string>(); // ←主要是要重組 List connectionString
            foreach (string ss in strSevers)
            {
                var connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.Contains(ss.Split(new char[] { ':' })[0].ToString())).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("Production")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();
                connectionString.Add(connections);
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

                    if (this.printData != null && this.printData.Rows.Count > 0)
                    {
                        if (this.dtAllData == null)
                        {
                            this.dtAllData = this.printData;
                        }
                        else
                        {
                            this.dtAllData.Merge(this.printData);
                        }
                    }
                }
            }

            if (this.dtAllData == null || this.dtAllData.Rows.Count == 0)
            {
                return Result.F("Data not found!");
            }

            string sqlsum = $@"
select Date,TotalCPU=sum(TotalCPU),BalanceCPU=sum(BalanceCPU)
into #tmp2
from #tmp
group by Date

declare @col nvarchar(max)=(select stuff((
select concat(',[', OutputDate,']')
from(
	select distinct OutputDate= format(OutputDate,'yyyy/MM')
	from #tmp
	where OutputDate<>''
)x
order by OutputDate
for xml path('')
),1,1,''))

declare @col2 nvarchar(max)=(select stuff((
select concat(',[',OutputDate,']=sum([', OutputDate,'])')
from(
	select distinct OutputDate= format(OutputDate,'yyyy/MM')
	from #tmp
	where OutputDate<>''
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
		OutputDate=format(OutputDate,''yyyy/MM'')
		from #tmp t
	)x
	pivot(sum(OutputCPU) for OutputDate in('+@col+N'))xx
)xxx on SUBSTRING(t2.Date,1,4)+''/''+SUBSTRING(t2.Date,5,6)=xxx.Date
order by t2.Date

select*from #tmp3
union all
select ''Total'' ,null, sum(Balance),'+@col2+'  from #tmp3
'
exec (@sql)
";
            DualResult dual = MyUtility.Tool.ProcessWithDatatable(this.dtAllData, "Date,TotalCPU,BalanceCPU,OutputCPU,OutputDate", sqlsum, out this.Summarydt);
            if (!dual)
            {
                return dual;
            }

            this.dtAllData.Columns.Remove("OutputDate");
            this.dtAllData.Columns.Remove("OutputCPU");
            DBProxy.Current.DefaultTimeout = 300;  // timeout時間改回5分鐘
            return Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            // 顯示筆數於PrintForm上Count欄位
            if (this.dtAllData == null || this.dtAllData.Rows.Count <= 0)
            {
                MyUtility.Msg.WarningBox("Data not found!");
                return false;
            }

            this.SetCount(this.dtAllData.Rows.Count);

            this.ShowWaitMessage("Starting EXCEL...");
            string excelFile = "Centralized_R05.xltx";
            Microsoft.Office.Interop.Excel.Application objApp = MyUtility.Excel.ConnectExcel(Sci.Env.Cfg.XltPathDir + excelFile); // 開excelapp
            MyUtility.Excel.CopyToXls(this.Summarydt, string.Empty, xltfile: excelFile, headerRow: 4, excelApp: objApp, wSheet: objApp.Sheets[1], showExcel: false, DisplayAlerts_ForSaveFile: true);

            Microsoft.Office.Interop.Excel.Worksheet objSheets = objApp.ActiveWorkbook.Worksheets[1]; // 取得工作表
            if (this.Date == "1")
            {
                objSheets.Cells[3, 1] = "SCI delivery";
            }
            else
            {
                objSheets.Cells[3, 1] = "Buyer delivery";
            }

            objSheets.Cells[1, 1] = "Fty:" + this.Factory;

            int i = 1;
            foreach (DataColumn col in this.Summarydt.Columns)
            {
                if (i > 3)
                {
                    objSheets.Cells[4, i] = col.ColumnName;
                }

                i++;
            }

            i--;
            objSheets.get_Range((Excel.Range)objSheets.Cells[3, 4], (Excel.Range)objSheets.Cells[3, i]).Merge(false);
            objSheets.get_Range((Excel.Range)objSheets.Cells[3, 1], (Excel.Range)objSheets.Cells[this.Summarydt.Rows.Count+4, i]).Borders.Weight = 3; // 設定全框線

            MyUtility.Excel.CopyToXls(this.dtAllData, string.Empty, xltfile: excelFile, headerRow: 1, excelApp: objApp, wSheet: objApp.Sheets[2], showExcel: false, DisplayAlerts_ForSaveFile: true);
            objSheets = objApp.ActiveWorkbook.Worksheets[1]; // 取得工作表
            objSheets.Columns[1].ColumnWidth = 10.38;

            objSheets = objApp.ActiveWorkbook.Worksheets[2]; // 取得工作表
            objSheets.Columns[1].ColumnWidth = 5.5;
            objSheets.Columns[2].ColumnWidth = 11.13;
            objSheets.Columns[3].ColumnWidth = 11.88;
            objSheets.Columns[4].ColumnWidth = 11.88;
            objSheets.Columns[5].ColumnWidth = 7.88;
            objSheets.Columns[6].ColumnWidth = 17.75;
            objSheets.Columns[7].ColumnWidth = 12.75;
            objSheets.Columns[8].ColumnWidth = 14;
            objSheets.Columns[9].ColumnWidth = 25;
            objSheets.Columns[10].ColumnWidth = 11.13;
            objSheets.Columns[11].ColumnWidth = 25.25;
            objSheets.Columns[12].ColumnWidth = 20;
            objSheets.Columns[13].ColumnWidth = 7.63;
            objSheets.Columns[14].ColumnWidth = 13.38;
            objSheets.Columns[15].ColumnWidth = 11.88;
            objSheets.Columns[16].ColumnWidth = 15.25;
            objSheets.Columns[17].ColumnWidth = 25.75;
            objSheets.Columns[18].ColumnWidth = 16.38;
            objSheets.Columns[19].ColumnWidth = 17.5;
            objSheets.Columns[20].ColumnWidth = 18.13;
            objSheets.Columns[21].ColumnWidth = 8;
            objSheets.Columns[22].ColumnWidth = 22;
            objSheets.Columns[23].ColumnWidth = 16.38;
            objSheets.Columns[24].ColumnWidth = 6.5;
            objSheets.Columns[25].ColumnWidth = 19.88;

            objApp.Visible = true;
            this.HideWaitMessage();
            return true;
        }
    }
}
