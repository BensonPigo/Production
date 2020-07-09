using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Linq;
using System.Data.SqlClient;
using System.Configuration;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// R10
    /// </summary>
    internal partial class R10 : Win.Tems.PrintForm
    {
        private string tsql_Summary;
        private string tsql_detail;
        private DataTable dtSummary;
        private DataTable dtDetail;

        /// <summary>
        /// R10
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R10(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region --設定元件預設值
            this.comboExchangeRate.SelectedIndex = 1;
            #endregion
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            string sqlWhere = string.Empty;
            string sqlFactory = string.Empty;    // 要抓工廠的資料
            string sqlProduction = string.Empty;
            List<string> sqlWheres = new List<string>();
            List<string> sqlFactorys = new List<string>();
            List<string> sqlProductions = new List<string>();

            #region 判斷不可為空的條件
            if (this.dateOutputDateStart.Value.Empty())
            {
                MyUtility.Msg.ErrorBox("[Begin Output Date] can not be Empty!!");
                this.dateOutputDateStart.Focus();
                return false;
            }

            if (this.dateOutputDateEnd.Value.Empty())
            {
                MyUtility.Msg.ErrorBox("[End Output Date] can not be Empty!!");
                this.dateOutputDateEnd.Focus();
                return false;
            }

            if (this.comboExchangeRate.Text.Empty())
            {
                MyUtility.Msg.ErrorBox("[Exchange Rate] can not be Empty!!");
                return false;
            }
            #endregion

            #region 設定Where 條件
            sqlWheres.Add("pod.Category NOT IN ( 'G','A') ");
            if (!this.dateOutputDateStart.Value.Empty() && !this.dateOutputDateEnd.Value.Empty())
            {
                sqlWheres.Add(string.Format(
                    "so.OutputDate between '{0}' and '{1}'",
                    ((DateTime)this.dateOutputDateStart.Value).ToShortDateString(),
                    ((DateTime)this.dateOutputDateEnd.Value).ToShortDateString()));
            }

            if (!this.txtCentralizedFactory1.Text.Empty())
            {
                sqlWheres.Add(string.Format(
                    "so.FactoryID ='{0}'",
                    this.txtCentralizedFactory1.Text));
                sqlFactorys.Add(string.Format(
                    "Factory.ID ='{0}'",
                    this.txtCentralizedFactory1.Text));
            }

            if (!this.txtcountry1.TextBox1.Text.Empty())
            {
                sqlWheres.Add(string.Format(
                    "so.FactoryID in(select ID from Factory where CountryID='{0}')",
                    this.txtcountry1.TextBox1.Text));
            }

            if (this.checkIncludeLocalOrder.Checked == false)
            {
                sqlWheres.Add("LocalOrder = 0");
            }

            sqlWhere = string.Join(" and ", sqlWheres);
            sqlProduction = string.Join(string.Empty, sqlProductions);
            if (sqlFactorys.Count() > 0)
            {
                sqlFactory = " and " + string.Join(" and ", sqlFactorys);
            }
            else
            {
                sqlFactory = string.Empty;
            }
            #endregion

            #region Detail SQL
            this.tsql_detail = string.Format(
@"
select   so.FactoryID
	    ,so.OutputDate
	    ,f.CountryID
	    ,pod.BrandID
	    ,sodd.OrderId
	    ,GetCategory.Category
	    ,pod.CurrencyID
	    ,CValue.Article
	    ,CValue.Size
	    ,CValue.EType
	    ,CValue.OutputQty
	    ,CPU = round(iif(CValue.EType is null,CValue.CPU,CValue.CPU * GetSuitRate.SuitRate),3)
	    ,GetCpuRate.CpuRate
	    ,GetSuitRate.SuitRate
	    ,GetCurrencyRate.CurrencyRate
	    ,GetTotalConfirmPrice.PoPrice
INTO #tmp
from SewingOutput so
inner join Factory f on f.ID = so.FactoryID
inner join SewingOutput_Detail sod on sod.ID = so.ID
inner join SewingOutput_Detail_Detail sodd on sod.UKey = sodd.SewingOutput_DetailUKey
inner join Orders pod on pod.ID = sodd.OrderId 
inner join CDCode cd on cd.ID = pod.CDCodeID
left join MockupOrder pmo on pmo.ID = sod.OrderId
outer apply(select Name as Category from DropDownList ddl where Type = 'Category' and pod.Category = ddl.ID) as GetCategory
outer apply(select Article = iif(so.ID = sodd.ID,sodd.Article,''),
				   Size = iif(so.ID = sodd.ID,sodd.SizeCode,''),
				   EType = iif(so.ID = sodd.ID,sodd.ComboType,sod.ComboType),
				   OutputQty = iif(so.ID = sodd.ID,sodd.QAQty,sod.QAQty),
				   CPU = iif(so.ID = sodd.ID,pod.Cpu,pmo.Cpu)) as CValue
outer apply(select iif(so.ID = sodd.ID,(select * from dbo.GetCPURate(pod.OrderTypeID,pod.ProgramID,pod.Category,pod.BrandID,'O'))
									  ,(select * from dbo.GetCPURate('',pmo.ProgramID,'Mockup',pmo.BrandID,'M'))) as CpuRate) as GetCpuRate
outer apply(
	select [SuitRate]=iif(so.ID = sodd.ID
						,(SELECT Rate*1.0 /100 FROM Order_Location where OrderID = sodd.OrderID and Location = sodd.ComboType)
						,1) 
) as GetSuitRate

outer apply(select iif(so.ID = sodd.ID,iif(pod.CurrencyID <> 'USD',(select  dbo.GetFinanceRate('{0}',so.OutputDate,pod.CurrencyID,'USD')),1),0)as CurrencyRate) as GetCurrencyRate
outer apply(select iif(so.ID = sodd.ID,(select dbo.GetPoPriceByArticleSize(sodd.Orderid,sodd.Article,sodd.SizeCode)),0) as PoPrice) as GetTotalConfirmPrice
where  ", this.comboExchangeRate.Text) + sqlWhere +
@"
order by FactoryID,sodd.OrderId

----Detail
SELECT 
         FactoryID
        ,OutputDate 
        ,CountryID
        ,BrandID
        ,OrderId
        ,Category
        ,CurrencyID
        ,Article
        ,Size
        ,EType
        ,[OutputQty]=SUM(OutputQty)
        ,CPU
        ,CpuRate
        ,SuitRate
        ,CurrencyRate
        ,TotalCpu = round(SUM(OutputQty) * CPU * CpuRate * SuitRate ,2)
        ,TotalFOB = round(PoPrice*SUM(OutputQty) * SuitRate ,2)
FROM #tmp
GROUP BY FactoryID, OutputDate ,CountryID,BrandID,OrderId,Category,CurrencyID,Article,Size,EType,CPU,CpuRate,SuitRate,CurrencyRate,PoPrice


DROP TABLE #tmp
";
            #endregion

            #region Summary SQL
            this.tsql_Summary =
@"
select FactoryID,BrandID,
	   QAQty = sum(OutputQty),
	   TotalCPUUSD = sum(TotalCpu),
	   TotalConfirmPriceUSD = sum(TotalFOB)
from #Detail
GROUP BY FactoryID,BrandID
order by FactoryID
";
            #endregion

            return base.ValidateInput();
        }

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            // 把DataTable的值清空
            this.dtSummary = null;
            this.dtDetail = null;

            #region --由Factory.PmsPath抓各個連線路徑
            this.SetLoadingText("Load connections... ");
            XDocument docx = XDocument.Load(Application.ExecutablePath + ".config");
            string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
            List<string> connectionString = new List<string>(); // ←主要是要重組 List connectionString
            foreach (string ss in strSevers)
            {
                // 判斷工廠的欄位選項是否有值,有值代表只需要撈單獨一個System
                if (!MyUtility.Check.Empty(this.txtCentralizedFactory1.Text))
                {
                    // 只取:後的FactoryID
                    string[] m = ss.Split(new char[] { ':' });
                    if (m.Count() > 1)
                    {
                        // 判斷是否有同畫面上的工廠名稱
                        string[] mFactory = m[1].Split(new char[] { ',' });

                        // 如果不同,就換下一個System,直到相同為止才跳出去
                        if (!mFactory.AsEnumerable().Any(f => f.EqualString(this.txtCentralizedFactory1.Text.ToString())))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }

                var connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.Contains(ss.Split(new char[] { ':' })[0].ToString())).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("Production")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();
                connectionString.Add(connections);
            }

            if (connectionString == null || connectionString.Count == 0)
            {
                return new DualResult(false, "no connection loaded.");
            }
#endregion

            // 依各個連線抓該DB的資料, 並且合併到 dt_All
            DataTable tmpSummary = new DataTable();
            for (int i = 0; i < connectionString.Count; i++)
            {
                string conString = connectionString[i];
                this.SetLoadingText(
                    string.Format(
                        "Load data from connection {0}/{1} ",
                        i + 1,
                        connectionString.Count));

                // 跨資料庫連線，將所需資料存到TempTable，再給不同資料庫使用
                SqlConnection pmsConn;
                using (pmsConn = new SqlConnection(conString))
                {
                    pmsConn.Open();
                    DataTable tmpDetails;
                    DualResult result3;

                    string detailList = "FactoryID,OutputDate,CountryID,BrandID,OrderId,Category,CurrencyID,Article,Size,EType,OutputQty,CPU,CpuRate,SuitRate,CurrencyRate,TotalCpu,TotalFOB";
                    SqlConnection tradeConn;
                    result3 = DBProxy.Current.OpenConnection(string.Empty, out tradeConn);
                    if (!result3)
                    {
                        return result3;
                    }

                    result3 = DBProxy.Current.SelectByConn(pmsConn, this.tsql_detail, out tmpDetails);
                    if (!result3)
                    {
                        return result3;
                    }

                    result3 = MyUtility.Tool.ProcessWithDatatable(tmpDetails, detailList, this.tsql_Summary, out tmpSummary, "#Detail", tradeConn);
                    if (!result3)
                    {
                        return result3;
                    }

                    if (this.dtSummary == null || this.dtSummary.Rows.Count == 0)
                    {
                        this.dtSummary = tmpSummary;
                    }
                    else
                    {
                        this.dtSummary.Merge(tmpSummary);
                    }

                    if (this.dtDetail == null || this.dtDetail.Rows.Count == 0)
                    {
                        this.dtDetail = tmpDetails;
                    }
                    else
                    {
                        this.dtDetail.Merge(tmpDetails);
                    }
                }
            }

            #region 加總Total
            int startIndex = 2;

            // 最後一列Total
            DataRow totalrow = this.dtSummary.NewRow();
            totalrow[0] = "Total";

            // for dt每個欄位
            decimal tTColumnAMT = 0;
            for (int colIdx = startIndex; colIdx < this.dtSummary.Columns.Count; colIdx++)
            {
                tTColumnAMT = 0;

                // for dt每一列
                for (int rowIdx = 0; rowIdx < this.dtSummary.Rows.Count; rowIdx++)
                {
                    tTColumnAMT += Convert.ToDecimal(this.dtSummary.Rows[rowIdx][colIdx]);
                }

                totalrow[colIdx] = tTColumnAMT;
            }

            this.dtSummary.Rows.Add(totalrow);
            #endregion

            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            if (this.dtSummary == null || this.dtSummary.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("data not found");
                return false;
            }

            if (this.dtDetail == null || this.dtDetail.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("data not found");
                return false;
            }
            #region Save & Show Excel
            Excel.Application objApp = MyUtility.Excel.ConnectExcel(Env.Cfg.XltPathDir + "\\Centralized_R10_OutputSummaryWithFOBReport(Summary_Detail).xltx");
            MyUtility.Excel.CopyToXls(this.dtSummary, null, "Centralized_R10_OutputSummaryWithFOBReport(Summary_Detail).xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp, wSheet: objApp.Sheets[1]);
            MyUtility.Excel.CopyToXls(this.dtDetail, null, "Centralized_R10_OutputSummaryWithFOBReport(Summary_Detail).xltx", 1, showExcel: false, showSaveMsg: false, excelApp: objApp, wSheet: objApp.Sheets[2]);
            for (int i = 1; i <= 2; i++)
            {
                objApp.Sheets[i].Columns.AutoFit();
            }

            Excel.Workbook workbook = objApp.Workbooks[1];
            string strFileName = Class.MicrosoftFile.GetName("Centralized_R10_OutputSummaryWithFOBReport(Summary_Detail).xltx");
            workbook.SaveAs(strFileName);
            workbook.Close();
            objApp.Quit();
            Marshal.ReleaseComObject(objApp);
            strFileName.OpenFile();
            #endregion
            return true;
        }
    }
}
