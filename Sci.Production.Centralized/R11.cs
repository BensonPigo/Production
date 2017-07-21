using Ict;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using Sci.Utility.Excel;
using System.Xml.Linq;
using System.Configuration;

namespace Sci.Production.Centralized
{
    public partial class R11 : Sci.Win.Tems.PrintForm
    {
        String factoryid, countryid;
        String tsql_Detail;
        String tsql_LoadData;
        String tsql_GetConnectionString;
        DataTable dt_All;
        DataTable dt_Tmp;
        DataTable dt_detail, dt_detail_All;

        public R11(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.comboExportRateBy.SelectedIndex = 0;
        }

        protected override bool ValidateInput()
        {
            factoryid = txtFactoryFactoryID.Text;
            countryid = txtCountryRegion.TextBox1.Text;

            #region --檢查查詢條件
            //dateCloseDate1有值，dateCloseDate2就一定要有值
            if (!this.dateCloseDate1.Value.Empty() != !this.dateCloseDate2.Value.Empty())
            {
                if (dateCloseDate1.Value.Empty())
                {
                    MyUtility.Msg.ErrorBox("[Begin Close Date] can not be empty");
                    dateCloseDate1.Focus();
                }
                if (dateCloseDate2.Value.Empty())
                {
                    MyUtility.Msg.ErrorBox("[End Close Date] can not be empty");
                    dateCloseDate2.Focus();
                }
                return false;
            }
            //dateApproveDate1有值，dateApproveDate2就一定要有值
            if (!this.dateApproveDate1.Value.Empty() != !this.dateApproveDate2.Value.Empty())
            {
                if (dateApproveDate1.Value.Empty())
                {
                    MyUtility.Msg.ErrorBox("[Begin Approve Date] can not be empty");
                    dateApproveDate1.Focus();
                }

                if (dateApproveDate2.Value.Empty())
                {
                    MyUtility.Msg.ErrorBox("[End Approve Date] can not be empty");
                    dateApproveDate2.Focus();
                }
                return false;
            }
            //dateETA1有值，dateETA2就一定要有值
            if (!this.dateETA1.Value.Empty() != !this.dateETA2.Value.Empty())
            {
                if (dateETA1.Value.Empty())
                {
                    MyUtility.Msg.ErrorBox("[Begin ETA] can not be empty");
                    dateETA1.Focus();
                }
                if (dateETA2.Value.Empty())
                {
                    MyUtility.Msg.ErrorBox("[End ETA] can not be empty");
                    dateETA2.Focus();
                }
                return false;
            }
            if (this.comboExportRateBy.Text.Empty())
            {
                MyUtility.Msg.ErrorBox("Exchange Rate Type can not be empty !!");
                comboExportRateBy.Focus();
                return false;
            }
            #endregion

            bool Close_Date = false, Approve_Date = false;
            string sqlWhere = "";
            string sqlWherew = "";
            string sqlFactory = "";
            List<string> sqlWheres = new List<string>();
            List<string> sqlWherews = new List<string>();
            List<string> sqlFactorys = new List<string>();

            #region  --查詢條件
            if (!this.dateCloseDate1.Value.Empty() && !this.dateCloseDate2.Value.Empty())
            {
                sqlWheres.Add(string.Format("e.CloseDate between '{0}' and '{1}' "
                   , ((DateTime)this.dateCloseDate1.Value).ToShortDateString()
                   , ((DateTime)this.dateCloseDate2.Value).ToShortDateString()));
                Close_Date = true;
            }

            if (!this.dateApproveDate1.Value.Empty() && !this.dateApproveDate2.Value.Empty())
            {
                sqlWheres.Add(string.Format("ShippingAP.ApvDate between '{0}' and '{1}' "
                   , ((DateTime)this.dateApproveDate1.Value).ToShortDateString()
                   , ((DateTime)this.dateApproveDate2.Value).ToShortDateString()));
                Approve_Date = true;
            }
            //[Close Date] and [Approve Date] 一定要有一個為必填
            if (!Close_Date && !Approve_Date)
            {
                MyUtility.Msg.ErrorBox("[Close Date] and [Approve Date] one of the inputs must be selected");
                dateCloseDate1.Focus();
                return false;
            }
            if (!this.dateETA1.Value.Empty() && !this.dateETA2.Value.Empty())
            {
                sqlWheres.Add(string.Format("e.ETA between '{0}' and '{1}' "
                   , ((DateTime)this.dateETA1.Value).ToShortDateString()
                   , ((DateTime)this.dateETA2.Value).ToShortDateString()));
            }
            if (!this.txtBrandBranadID.Text.Empty())
            {
                sqlWherews.Add(string.Format("and o.BrandID ='{0}'", this.txtBrandBranadID.Text));
            }
            if (!this.txtFactoryFactoryID.Text.Empty())
            {
                sqlWheres.Add(string.Format("e.FactoryID ='{0}' ", this.txtFactoryFactoryID.Text));
                sqlWherews.Add(string.Format("and o.FactoryID ='{0}'", this.txtFactoryFactoryID.Text));
                sqlFactorys.Add(string.Format(" Factory.ID ='{0}'", this.txtFactoryFactoryID.Text));
            }
            if (!this.txtCountryRegion.TextBox1.Text.Empty())
            {
                sqlWheres.Add(string.Format("e.FactoryID in (select ID from Factory where CountryID ='{0}') ", this.txtCountryRegion.TextBox1.Text));
                sqlWherews.Add(string.Format("and f.ID  in (select ID from Factory where CountryID ='{0}')", this.txtCountryRegion.TextBox1.Text));
                sqlFactorys.Add(string.Format(" Factory.CountryID ='{0}'", this.txtCountryRegion.TextBox1.Text));
            }
            #endregion

            sqlWhere = string.Join(" and ", sqlWheres);
            sqlWherew = string.Join(" ", sqlWherews);
            sqlFactorys.Add(" IsSCI = 1 and PmsPath <> '' ");
            sqlFactory = string.Join(" and ", sqlFactorys);
            //連線的SQL語法
            tsql_GetConnectionString = @"select distinct PmsPath  from Factory where  " + sqlFactory;

            if (this.checkExportDetail.Checked == true)
            {
                Transportation_list(sqlWhere, sqlWherew);
                Transportation_Detail();
            }
            else
            {
                Transportation_list(sqlWhere, sqlWherew);
            }
            return base.ValidateInput();
        }

        public void Transportation_list(string sqlWhere, string sqlWherew)
        {
            tsql_LoadData = string.Format(@"
select  ShareExpense.WKNo
        , Blno = e.Blno
        , ExportCountry = e.ExportCountry
        , ImportCountry = e.ImportCountry 
        , CloseDate = e.CloseDate 
        , Etd = e.Etd
        , Eta = e.Eta 
        , ShipModeID = e.ShipModeID 
        , ID = e.ID
        , Amount = ShareExpense.Amount * dbo.Getrate ('{0}', ShareExpense.CurrencyID, 'USD', ShippingAP.ApvDate)
        , AccountID
        , ApvDate = ShippingAP.ApvDate 
into #temp
from ShareExpense 
inner join ShippingAP on ShareExpense.ShippingAPID = ShippingAP.ID 
left join Export as e on e.ID = ShareExpense.WKNo 
where   e.Junk <> 1 
        and ShippingAP.Type='IMPORT' 
        and " + sqlWhere +
@"
select  Wkno
        , Fee01 = isnull((select sum(amount) from #Temp where Wkno = Data.WKNo and AccountID = '61012001'),0)
        , Fee02 = isnull((select sum(amount) from #Temp where Wkno = Data.WKNo and AccountID = '61012002'),0)
        , Fee03 = isnull((select sum(amount) from #Temp where Wkno = Data.WKNo and AccountID = '61012003'),0) 
        , Fee04 = isnull((select sum(amount) from #Temp where Wkno = Data.WKNo and AccountID = '61012004'),0)
        , Fee05 = isnull((select sum(amount) from #Temp where Wkno = Data.WKNo and AccountID = '61012005'),0)
        , other = isnull((select sum(amount) from #Temp where Wkno = Data.WKNo and AccountID not in('61012001','61012002','61012003','61012004','61012005')),0)
        , Blno
        , ExportCountry
        , ImportCountry
        , CloseDate
        , Etd
        , Eta
        , ShipModeID
        , ID
        , ApvDate
into #temp1 
from #temp as Data
group by WKNo, CloseDate, Blno, ExportCountry, ImportCountry, CloseDate, Etd, Eta, ShipModeID, ID, ApvDate
 
--因為Function GetWkAmount 執行時間過長，將 GetWkAmount另外撈

select  a.id
        , WKAmount = round(sum(amount),4)  
into #temp2
from (
       select   a.id
                , Amount = Price * (Qty+Foc) * dbo.GetFinanceRate('{0}', a.CloseDate, (Select CurrencyID from Supp where ID = b.SuppID),'USD') / iif(b.UnitID = 'P',100,iif(b.UnitID = 'PX',1000,1)) 
       from (
            select  distinct id
                    , closedate 
            from #temp1
       ) a
        ,EXPORT_dETAIL b
       where a.id = b.id
) a
group by a.id

select  distinct t.Blno
        , t.WKNo
        , t.ExportCountry
        , t.ImportCountry
        , t.ShipModeID
        , t.CloseDate
        , t.Etd
        , t.Eta
        , t.Fee01
        , t.Fee02
        , t.Fee03
        , t.Fee04
        , t.Fee05
        , t.other
        , t2.WKAmount
        , ApvDate 
into #temp3
from #temp1 as t
inner join #temp2 as t2 on t.wkno = t2.id
;
with #Result as (
    select  distinct query.Blno
            , query.WKNo
            , ed.PoID
            , o.BrandID
            , query.ExportCountry
            , query.ImportCountry
            , KpiCode = iif (isnull (f.KpiCode, '') = ' ', isnull (o.FactoryID, 'None'), f.KpiCode)
            , o.FactoryID
            , query.ShipModeID
            , expor.WkPOAmount
            , CloseDate = FORMAT (query.CloseDate, 'yyyy-MM-dd')
            , Etd = FORMAT (query.Etd, 'yyyy-MM-dd')
            , Eta = FORMAT (query.Eta, 'yyyy-MM-dd') 
            , Fee01 = round((query.Fee01*(iif(query.WKAmount = 0,0,expor.WkPOAmount/query.WKAmount))),4)
            , Fee02 = round((query.Fee02*(iif(query.WKAmount = 0,0,expor.WkPOAmount/query.WKAmount))),4)
            , Fee03 = round((query.Fee03*(iif(query.WKAmount = 0,0,expor.WkPOAmount/query.WKAmount))),4)
            , Fee04 = round((query.Fee04*(iif(query.WKAmount = 0,0,expor.WkPOAmount/query.WKAmount))),4) 
            , Fee05 = round((query.Fee05*(iif(query.WKAmount = 0,0,expor.WkPOAmount/query.WKAmount))),4) 
            , other = round((query.other*(iif(query.WKAmount = 0,0,expor.WkPOAmount/query.WKAmount))),4) 
    from #temp3 as query
    outer apply (
        select  distinct poid 
        from dbo.Export_detail as ed 
        where ed.ID = query.WKNo 
    ) as ed
    left join Orders as o on ed.POID = o.ID
    left join #tmpFactory as f on f.ID = o.FactoryID
    outer apply (
	    select  WkPOAmount = dBO.GetExportOrderAmount (query.WKNo, ed.PoID, O.BuyerDelivery, '{0}') 
    ) as expor
    where   1 = 1
            ", this.comboExportRateBy.Text) + sqlWherew
    + @"
)
select  *
        , feeAmount = Fee01 + Fee02 + Fee03 + Fee04 + Fee05 + other 
into #data
from #Result 

drop table #temp
drop table #temp1
drop table #temp2
drop table #temp3

--KpiCode要依照FactorySort排序
select distinct KpiCode 
from #data 
order by KpiCode 
 "
      ;
        }

        //Transportation_list Tsql 抓出報表Detail資料，完成Sheep2
        public void Transportation_Detail()
        {
            tsql_Detail = @"
select  Blno
        , WKNo
        , PoID
        , BrandID
        , ExportCountry
        , ImportCountry
        , KpiCode
        , FactoryID
        , ShipModeID
        , WkPOAmount
        , CloseDate
        , Etd
        , Eta
        , Fee01
        , Fee02
        , Fee03
        , Fee04
        , Fee05
        , other
        , feeAmount 
from #data";
        }

        private void dateCloseDate1_Validated(object sender, EventArgs e)
        {
            if (!this.dateCloseDate1.Value.Empty())
            {
                dateCloseDate2.Value = Convert.ToDateTime(this.dateCloseDate1.Value).GetLastDayOfMonth();
            }
        }

        private void dateApproveDate1_Validated(object sender, EventArgs e)
        {
            if (!this.dateApproveDate1.Value.Empty())
            {
                dateApproveDate2.Value = Convert.ToDateTime(this.dateApproveDate1.Value).GetLastDayOfMonth();
            }
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            dt_All = null;
            dt_Tmp = null;
            dt_detail = null;
            dt_detail_All = null;
            DualResult result;
            DataTable kpiCodes = new DataTable();
            SortedList<string, string> kpiCode_All = new SortedList<string, string>();

            Dictionary<string, Dictionary<string, decimal>> brand_FtyAmt = new Dictionary<string, Dictionary<string, decimal>>();

            #region --由 appconfig 抓各個連線路徑
            this.SetLoadingText("Load connections... ");
            XDocument docx = XDocument.Load(Application.ExecutablePath + ".config");
            string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
            List<string> connectionString = new List<string>();
            foreach (string ss in strSevers)
            {
                #region 選擇單一工廠時，只需保留該工廠的連線
                if (!txtFactoryFactoryID.Text.ToString().Empty())
                {
                    /*
                     * 將 strSevers 切割成 0 : 連線 1 : 連線中所有的工廠
                     */ 
                    string[] m = ss.Split(new char[] { ':' });
                    if (m.Count() > 1)
                    {
                        /*
                         * 判斷該連線中，是否有與畫面中相同的工廠名稱
                         */ 
                        string[] mFactory = m[1].Split(new char[] { ',' });
                        if (!mFactory.AsEnumerable().Any(f => f.EqualString(txtFactoryFactoryID.Text.ToString())))
                            continue;
                    }
                    else
                    {
                        continue;
                    }
                }
                #endregion 
                var Connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.Contains(ss.Split(new char[]{':'})[0].ToString())).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("Production")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();
                connectionString.Add(Connections);
            }
            if (null == connectionString || connectionString.Count == 0)
            {
                return new DualResult(false, "no connection loaded.");
            }
            #endregion

            #region --依各個連線抓該連線的DB資料,並且將抓取到的資料合併到 dt_Tmp(DataTable)
            for (int i = 0; i < connectionString.Count; i++)
            {
                string conString = connectionString[i];
                Logs.UI.LogInfo("");
                //跳提示視窗顯示跑到第幾筆連線
                this.SetLoadingText(
                    string.Format("Load data from connection {0}/{1} "
                   , (i + 1), connectionString.Count));
                SqlConnection conn;
                using (conn = new SqlConnection(conString))
                {
                    conn.Open();
                    DataTable Factorys;

                    #region --連 Factory 資料
                    result = DBProxy.Current.SelectByConn(conn, "select id,KpiCode from Factory  ", out Factorys);
                    if (!result) { return result; }
                    //執行tsql_LoadData抓Query資料，將從Trade撈到的Factory,加入到tsql_LoadData中設定的變數中，最後並list出有哪些KpiCode

                    result = MyUtility.Tool.ProcessWithDatatable(Factorys, "ID,KpiCode", tsql_LoadData, out kpiCodes, "#tmpFactory", conn);

                    // 用KpiCode重組Pivot的Tsql
                    if (!result) { return result; }
                    #endregion

                    //沒有資料就繼續跑下一個連線，直到所有連線都跑完
                    if (0 == kpiCodes.Rows.Count) { continue; }
                    #region --將KpiCode組到listSummary(Pivot SQL變數上)，並執行listSummary(pivot)
                    string listSummary =
@"select BrandID,{0}

from (
     	select BrandID,KpiCode,feeAmount=SUM(round(feeAmount,4)) from #data group by BrandID,KpiCode
     ) 
a pivot
	 (
        sum(feeAmount) for KpiCode in ({1})
     ) b
order by BrandID";
                    List<String> ftyAmt = new List<string>();
                    List<String> ftyList = new List<string>();

                    foreach (DataRow kpi in kpiCodes.Rows)
                    {
                        string kpiCode = kpi["KpiCode"].ToString().Trim();
                        if (!kpiCode_All.ContainsKey(kpiCode)) { kpiCode_All.Add(kpiCode, kpiCode); }
                        ftyAmt.Add("[" + kpiCode + "]=isnull([" + kpiCode + "],0)");
                        ftyList.Add("[" + kpiCode + "]");
                    }
                    Logs.UI.LogInfo("listSummary");
                    listSummary = string.Format(listSummary
                       , string.Join(",", ftyAmt)
                       , string.Join(",", ftyList));

                    // 執行pivot
                    result = DBProxy.Current.SelectByConn(conn, listSummary, out dt_Tmp);
                    if (!result) { return result; }
                    if (null == dt_Tmp || dt_Tmp.Rows.Count == 0)
                    {
                        return new DualResult(false, "Data not found.");
                    }
                    foreach (DataRow brandData in dt_Tmp.Rows)
                    {
                        string brand = brandData["BrandID"].ToString().Trim();
                        Dictionary<string, decimal> brand_Fty;
                        if (brand_FtyAmt.ContainsKey(brand)) { brand_Fty = brand_FtyAmt[brand]; }
                        else { brand_Fty = new Dictionary<string, decimal>(); brand_FtyAmt.Add(brand, brand_Fty); }
                        foreach (DataRow kpi in kpiCodes.Rows)
                        {
                            string kpiCode = kpi["KpiCode"].ToString().Trim();
                            decimal Amt = brand_Fty.ContainsKey(kpiCode)
                                ? brand_Fty[kpiCode]
                                : 0;
                            Amt = Amt + MyUtility.Convert.GetDecimal(brandData[kpiCode]);

                            if (brand_Fty.ContainsKey(kpiCode)) { brand_Fty.Set(kpiCode, Amt); }
                            else { brand_Fty.Add(kpiCode, Amt); }
                        }

                    }
                    #endregion

                    #region --判斷是不是要列印Detail明細資料
                    if (this.checkExportDetail.Checked == true)
                    {
                        DualResult result2 = DBProxy.Current.SelectByConn(conn, tsql_Detail, out dt_detail);
                        if (!result2) { return result2; }
                        if (null == dt_detail_All || 0 == dt_detail_All.Rows.Count)
                        {
                            dt_detail_All = dt_detail;
                        }
                        else
                        {
                            dt_detail_All.Merge(dt_detail);
                        }
                    }
                    #endregion
                }
            }
            #endregion

            dt_All = new DataTable();
            dt_All.ColumnsStringAdd("BrandID");
            //1.產生第一列表頭資料
            //2.表頭資料依照FactorySort排序
            #region --依照FactorySort把表頭資料做排序
            //撈出Factory的KpiCode及FactorySort，依照FactorySort做排序
            for (int i = 0; i < connectionString.Count; i++)
            {
                string conString = connectionString[i];

                //跳提示視窗顯示跑到第幾筆連線
                this.SetLoadingText(
                    string.Format("Load data from connection {0}/{1} "
                   , (i + 1), connectionString.Count));
                SqlConnection conn;
                using (conn = new SqlConnection(conString))
                {
                    string tsql_Factory = @"
select  distinct  KpiCode.KpiCode
        , max(FactorySort) as FactorySort 
from  Factory
outer apply(
    select  KpiCode = iif(KpiCode='',Factory.ID,KpiCode)
) as KpiCode
group by KpiCode.KpiCode 
order by FactorySort
";
                    DataTable dt_factory = new DataTable();
                    result = DBProxy.Current.SelectByConn(conn, tsql_Factory, out dt_factory);
                    if (!result) { return result; }

                    //如果kpicode資料中有None，將dt_factory新增None的資料
                    foreach (string kpicode in kpiCode_All.Values)
                    {
                        if (kpicode == "None")
                        {
                            dt_factory.Rows.Add("None", "100");
                        }
                    }
                    foreach (DataRow row in dt_factory.Rows)
                    {
                        foreach (string kpicode in kpiCode_All.Values)
                        {
                            //如果row["KpiCode"]等於跨表撈出kpicode時，傳表頭資料到dt_All
                            if (row["KpiCode"].ToString() == kpicode && !dt_All.Columns.Contains(kpicode))
                            {
                                dt_All.ColumnsDecimalAdd(row["KpiCode"].ToString());
                            }
                        }
                    }
                    //如果kpicode資料中有None，將None移到第二個位置
                    foreach (string kpicode in kpiCode_All.Values)
                    {
                        if (kpicode == "None")
                        {
                            dt_All.Columns[dt_All.Columns.Count - 1].SetOrdinal(1);
                        }
                    }
                }
            }
            #endregion

            //1.產生表身資料-Dt_All
            //2.表身資料依照 KpiCode 及BrandID 將金額填入DataTable
            #region--依照工廠及品牌將金額填入dt_All(To Excel的DataTable)
            foreach (var brandID in brand_FtyAmt.Keys)
            {
                Dictionary<string, decimal> ftyAmt = brand_FtyAmt[brandID];
                DataRow row = dt_All.NewRow();
                row["BrandID"] = brandID;
                foreach (string kpiCode in ftyAmt.Keys)
                {
                    row[kpiCode] = ftyAmt[kpiCode];
                }
                dt_All.Rows.Add(row);
            }
            #endregion

            if (null == dt_All || dt_All.Rows.Count == 0)
            {
                return new DualResult(false, "Data not found");
            }

            #region --計算總Total 和百分比及最後一列Total
            dt_All.Columns.Add("TTLAMT", typeof(Decimal));//typeof(Decimal)是設定欄位格式
            dt_All.Columns.Add("TTLPER", typeof(Decimal));//typeof(Decimal)是設定欄位格式
            int startIndex = 1;

            #region --計算總Total
            //for dt每一列
            for (int rowIdx = 0; rowIdx < dt_All.Rows.Count; rowIdx++)
            {
                decimal TTAMT = 0;
                //for dt每個欄位
                for (int colIdx = startIndex; colIdx < dt_All.Columns.Count - 2; colIdx++)
                {
                    TTAMT += Convert.ToDecimal(dt_All.Rows[rowIdx][colIdx]);
                }
                dt_All.Rows[rowIdx]["TTLAMT"] = TTAMT;
            }
            #endregion

            #region --最後一列Total
            //最後一列Total
            DataRow totalrow = dt_All.NewRow();
            totalrow[0] = "Total";

            //for dt每個欄位
            decimal TTColumnAMT = 0;
            for (int colIdx = startIndex; colIdx < dt_All.Columns.Count - 1; colIdx++)
            {
                TTColumnAMT = 0;
                //for dt每一列
                for (int rowIdx = 0; rowIdx < dt_All.Rows.Count; rowIdx++)
                {
                    TTColumnAMT += Convert.ToDecimal(dt_All.Rows[rowIdx][colIdx]);
                }
                totalrow[colIdx] = TTColumnAMT;
            }
            dt_All.Rows.Add(totalrow);
            #endregion

            #region --算最後一個欄位的百分比
            //算TTLPER
            for (int i = 0; i < dt_All.Rows.Count; i++)
            {
                decimal ttlamt = Convert.ToDecimal(dt_All.Rows[i]["TTLAMT"]);
                if (TTColumnAMT == 0) { TTColumnAMT = 1; }
                dt_All.Rows[i]["TTLPER"] = decimal.Round((ttlamt / TTColumnAMT) * 100, 2);
            }
            #endregion
            #endregion
            return Result.True;
        }
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            #region --判斷DataTable有沒有資料
            if (this.checkExportDetail.Checked == true)
            {
                if (dt_All == null || dt_All.Rows.Count == 0)
                {
                    MyUtility.Msg.ErrorBox("Data not found");
                    return false;
                }
                if (dt_detail_All == null || dt_detail_All.Rows.Count == 0)
                {
                    MyUtility.Msg.ErrorBox("Data not found");
                    return false;
                }
            }
            else
            {
                if (dt_All == null || dt_All.Rows.Count == 0)
                {
                    MyUtility.Msg.ErrorBox("Data not found");
                    return false;
                }
            }
            #endregion

            #region --匯出Excel
            Sci.Utility.Excel.SaveXltReportCls xl = new Utility.Excel.SaveXltReportCls("Centralized_R11_Transportation_Cost_Material_Import_clearance.xltx");
            xl.boOpenFile = true;
            SaveXltReportCls.xltRptTable xdt_All = new SaveXltReportCls.xltRptTable(dt_All);
            xdt_All.ShowHeader = true;
            xdt_All.HeaderColor = Color.FromArgb(216, 228, 188);
            xl.dicDatas.Add("##R20UPRLLIST", xdt_All);

            if (this.checkExportDetail.Checked == true)
            {
                SaveXltReportCls.xltRptTable xdt_detail_All = new SaveXltReportCls.xltRptTable(dt_detail_All);
                xdt_detail_All.ShowHeader = false;
                xl.dicDatas.Add("##R20UNRLDETAIL", xdt_detail_All);
                xl.Save();
            }
            else
            {
                Microsoft.Office.Interop.Excel.Application excel = xl.ExcelApp;
                xl.Save();
                excel.Worksheets[2].Delete();
            }
            #endregion

            return true;
        }
    }
}
