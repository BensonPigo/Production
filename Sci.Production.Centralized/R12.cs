using Ict;
using Sci.Data;
using Sci.Utility.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Configuration;
using System.Xml.Linq;

namespace Sci.Production.Centralized
{
    public partial class R12 : Sci.Win.Tems.PrintForm
    {
        DataTable dt_All;
        DataTable dt_Tmp;
        DataTable dt_detail, dt_detail_All;

        String tsql_Detail;
        String tsql_LoadData;

        public R12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.comboRateTypeID.SelectedIndex = 0;
            DataTable TEMP = (DataTable)this.comboShipmodeShipmodeID.DataSource;
            TEMP.Rows.InsertAt(TEMP.NewRow(), 0);
            this.comboShipmodeShipmodeID.DataSource = TEMP;
            comboShipmodeShipmodeID.SelectedIndex = 0;
        }

        protected override bool ValidateInput()
        {
            #region --檢查查詢條件
            if (!this.dateShipDate1.Value.Empty() != !this.dateShipDate2.Value.Empty())
            {
                if (dateShipDate1.Value.Empty())
                {
                    MyUtility.Msg.ErrorBox("[Begin Ship Date] can not be empty");
                    dateShipDate1.Focus();
                }
                if (dateShipDate2.Value.Empty())
                {
                    MyUtility.Msg.ErrorBox("[End Ship Date] can not be empty");
                    dateShipDate2.Focus();
                }
                return false;
            }
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
            if (this.comboRateTypeID.Text.Empty())
            {
                MyUtility.Msg.ErrorBox("Exchange Rate Type can not be empty !!");
                dateApproveDate2.Focus();
                return false;

            }
            #endregion

            bool Ship_Date = false, Approve_Date = false;
            string sqlWhere = "";
            List<string> sqlWheres = new List<string>();

            #region --查詢條件
            if (!this.dateShipDate1.Value.Empty() && !this.dateShipDate2.Value.Empty())
            {
                sqlWheres.Add(string.Format("fe.PortArrival between '{0}' and '{1}' "
                   , ((DateTime)this.dateShipDate1.Value).ToShortDateString()
                   , ((DateTime)this.dateShipDate2.Value).ToShortDateString()));

                Ship_Date = true;
            }
            if (!this.dateApproveDate1.Value.Empty() && !this.dateApproveDate2.Value.Empty())
            {
                sqlWheres.Add(string.Format("ShippingAP.ApvDate between '{0}' and '{1}' "
                   , ((DateTime)this.dateApproveDate1.Value).ToShortDateString()
                   , ((DateTime)this.dateApproveDate2.Value).ToShortDateString()));

                Approve_Date = true;
            }
            //[Ship Date] and [Approve Date] 一定要有一個是必填
            if (!Ship_Date && !Approve_Date)
            {
                MyUtility.Msg.ErrorBox("[Ship Date] and [Approve Date] one of the inputs must be selected");
                dateShipDate1.Focus();
                return false;
            }
            if (!this.txtCountryImportCountry.TextBox1.Text.Empty())
            {
                sqlWheres.Add(string.Format("fe.ImportCountry = '{0}'", this.txtCountryImportCountry.TextBox1.Text));
            }
            if (!this.comboShipmodeShipmodeID.Text.Empty())
            {
                sqlWheres.Add(string.Format("fe.ShipmodeID = '{0}'", this.comboShipmodeShipmodeID.Text));
            }
            if (!this.txtForwarder.Text.Empty())
            {
                sqlWheres.Add(string.Format("fe.Forwarder = '{0}'", this.txtForwarder.Text));
            }

            #endregion
            sqlWhere = string.Join(" and ", sqlWheres);

            if (this.checkExportDetail.Checked == true)
            {
                Transportation_list(sqlWhere);
                Transportation_Detail(sqlWhere);
            }
            else
            {
                Transportation_list(sqlWhere);
            }
            return base.ValidateInput();
        }

        #region -- summary Detail SQl
        public void Transportation_list(string sqlWhere)
        {
            tsql_LoadData = string.Format(@"
select  WkNo = s.Invno
        , Amount = s.Amount * dbo.Getrate('{0}', s.CurrencyID, 'USD', ShippingAP.ApvDate)
        , AccountID
        , ShippingAP.ApvDate
into #temp
from ShareExpense as s 
left join ShippingAP on ShippingAP.id = s.ShippingAPID  
left join FtyExport as fe on s.InvNo = fe.ID 
where   ShippingAP.Type = 'EXPORT' 
        And ShippingAP.subType = 'SISTER FACTORY TRANSFER'
        and " + sqlWhere +
@"
select  Wkno
        , Fee01 = isnull((select sum(round(Amount,4)) from #Temp where Wkno = Data.WKNo and AccountID = '61022001'), 0)
        , Fee02 = isnull((select sum(round(Amount,4)) from #Temp where Wkno = Data.WKNo and AccountID = '61022002'), 0)
        , Fee03 = isnull((select sum(round(Amount,4)) from #Temp where Wkno = Data.WKNo and AccountID = '61022003'), 0)
        , Fee04 = isnull((select sum(round(Amount,4)) from #Temp where Wkno = Data.WKNo and AccountID = '61022004'), 0)
        , Fee05 = isnull((select sum(round(Amount,4)) from #Temp where Wkno = Data.WKNo and AccountID = '61012005'), 0)
        , other = isnull((select sum(round(Amount,4)) from #Temp where Wkno = Data.WKNo and AccountID not in('61022001', '61022002', '61022003', '61022004', '61012005')), 0)
into #temp2
from #temp as Data 
where amount <> 0
group by Wkno

select  query.WkNo
        , fed.poid
        , sp.Qty
        , Fee01 = sum(Round(iif(sp.cnts<>0,(query.Fee01/sp.cnts),(query.Fee01)), 2))
        , Fee02 = sum(Round(iif(sp.cnts<>0,(query.Fee02/sp.cnts),(query.Fee02)), 2))
        , Fee03 = sum(Round(iif(sp.cnts<>0,(query.Fee03/sp.cnts),(query.Fee03)), 2))
        , Fee04 = sum(Round(iif(sp.cnts<>0,(query.Fee04/sp.cnts),(query.Fee04)), 2))
        , Fee05 = sum(Round(iif(sp.cnts<>0,(query.Fee05/sp.cnts),(query.Fee05)), 2))
        , other = sum(Round(iif(sp.cnts<>0,(query.other/sp.cnts),(query.other)), 2))
        , feeAmount = sum(Round(iif(sp.cnts<>0,(working.workingAmount/sp.cnts),(working.workingAmount)), 2))
into #queryAmount
from #temp2 as query
left join FtyExport_Detail as fed on fed.ID = query.WkNo 
outer apply (
    select  cnts = count(poid)
            , Qty = sum(Qty)
    from FtyExport_Detail 
    where ID = query.WkNo
) as sp
outer apply(
    select  workingAmount = Fee01 + Fee02 + Fee03 + Fee04 + Fee05 + other
) as working
group by query.wkno, fed.poid, sp.Qty

select  query.WkNo
        , FactoryID = isnull(o.FactoryID,'None')
        , Brand = iif (isnull (o.BrandID, '') = '', '', BrandID)
        , Category = DropDownList.Name
        , o.ID
        , BuyerDelivery = FORMAT(o.BuyerDelivery,'yyyy-MM-dd')
        , qy = (o.Qty)
        , o.CustCDID
        , fe.ImportPort
        , fe.ShipModeID
        , [pl#] = ''
        , [pulloutID] = ' '
        , PortArrival = FORMAT (fe.PortArrival, 'yyyy-MM-dd')
        , query.Qty
        , ctn = ''
        , fe.WeightKg
        , fe.Cbm
        , Forwarder = fe.Forwarder + '-' + suppData.Name
        , fe.Blno
        , Fee01
        , Fee02
        , Fee03
        , Fee04
        , Fee05
        , other
        , feeAmount
        , KpiCode = iif(isnull(f.KpiCode,'')='','None',f.KpiCode) 
into #data
from #queryAmount as query
left join FtyExport as fe on fe.id = query.WkNo
left join (
    select  id
            , name 
    from LocalSupp --以前舊版supp, localsupp是放在同一個Table
    
    union
    select  id
            , name = supp.AbbEN 
    from supp
) as suppData on fe.Forwarder = suppData.ID
left join Orders as o on o.ID = query.POID
left join #tmpFactory as f on f.ID = o.FactoryID
left join #tmpDropDownList as DropDownList on  DropDownList.ID = o.Category 
                                               and DropDownList.Type = 'Category' 
order by query.WkNo

drop table #temp
drop table #temp2
drop table #queryAmount

select distinct KpiCode 
from #data 
order by KpiCode
 ", this.comboRateTypeID.Text);
        }

        public void Transportation_Detail(string sqlWhere)
        {
            tsql_Detail = @" 
select  WkNo
        , FactoryID
        , Brand
        , Category
        , ID
        , BuyerDelivery
        , qy
        , CustCDID
        , ImportPort
        , ShipModeID
        , [pl#] = ''
        , [pulloutID] = ' '
        , PortArrival
        , Qty
        , ctn=''
        , WeightKg
        , Cbm
        , Forwarder
        , Blno
        , Fee01
        , Fee02
        , Fee03
        , Fee04
        , Fee05
        , other
        , feeAmount
from #data";
        }
        #endregion

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            dt_All = null;
            dt_Tmp = null;
            dt_detail = null;
            dt_detail_All = null;
            DualResult result;
            SortedList<string, string> kpiCode_All = new SortedList<string, string>();
            Dictionary<string, Dictionary<string, decimal>> brand_FtyAmt = new Dictionary<string, Dictionary<string, decimal>>();

            #region --抓各個連線路徑
            this.SetLoadingText("Load connections... ");
            XDocument docx = XDocument.Load(Application.ExecutablePath + ".config");
            string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
            List<string> connectionString = new List<string>();
            foreach (string ss in strSevers)
            {
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

                //跳提示視窗顯示跑到第幾筆連線
                this.SetLoadingText(
                    string.Format("Load data from connection {0}/{1} "
                   , (i + 1), connectionString.Count));
                SqlConnection conn;
                using (conn = new SqlConnection(conString))
                {
                    conn.Open();
                    DataTable Factorys;
                    DataTable DropDownLists;
                    DataTable kpiCodes = new DataTable();

                    #region--先從Trade撈 DropDownList 、Factory資料
                    #region--連Trade DropDownList資料(為了Detail報表要顯示完整名稱的Category)
                    result = DBProxy.Current.SelectByConn(conn, "select id,Name,type from DropDownList   ", out DropDownLists);
                    if (!result) { return result; }

                    //執行DropDownLists要寫入暫存#tmpDropDownList提供tsql_LoadData(要將從Trade撈到的Factory及DropDownLists加入到tsql_LoadData，設定的變數中)使用 
                    result = MyUtility.Tool.ProcessWithDatatable(DropDownLists, "id,Name,type", "Select 1", out kpiCodes, "#tmpDropDownList", conn);
                    if (!result) { return result; }
                    #endregion

                    #region --連Trade Factory資料(取得KpiCode)
                    result = DBProxy.Current.SelectByConn(conn, "select id,KpiCode from Factory  ", out Factorys);
                    if (!result) { return result; }

                    //執行tsql_LoadData抓Query資料，將從Trade撈到的Factory及DropDownLists加入到tsql_LoadData中設定的變數中，最後並list出有哪些KpiCode
                    result = MyUtility.Tool.ProcessWithDatatable(Factorys, "ID,KpiCode", tsql_LoadData, out kpiCodes, "#tmpFactory", conn);
                    if (!result) { return result; }
                    #endregion
                    #endregion

                    //沒有資料就繼續跑下一個連線，直到所有連線都跑完
                    if (0 == kpiCodes.Rows.Count) { continue; }

                    #region --將KpiCode組到listSummary(Pivot SQL變數上)，並執行listSummary(pivot)
                    // 用KpiCode重組Pivot的Tsql
                    string listSummary =
@"select Brand,{0}

from (
	select  Brand
            , KpiCode
            , feeAmount = SUM(feeAmount) 
    from #data 
    group by Brand,KpiCode
) a pivot
	(sum(feeAmount) for KpiCode in ({1})
) b
order by Brand
";
                    List<String> ftyAmt = new List<string>();
                    List<String> ftyList = new List<string>();
                    foreach (DataRow kpi in kpiCodes.Rows)
                    {
                        string kpiCode = kpi["KpiCode"].ToString().Trim();
                        if (!kpiCode_All.ContainsKey(kpiCode)) { kpiCode_All.Add(kpiCode, kpiCode); }
                        ftyAmt.Add("[" + kpiCode + "]=isnull([" + kpiCode + "],0)");
                        ftyList.Add("[" + kpiCode + "]");
                    }
                    listSummary = string.Format(listSummary
                       , string.Join(",", ftyAmt)
                       , string.Join(",", ftyList));
                    result = DBProxy.Current.SelectByConn(conn, listSummary, out dt_Tmp);
                    if (!result) { return result; }
                    if (null == dt_Tmp || dt_Tmp.Rows.Count == 0)
                    {
                        return new DualResult(false, "Data not found.");
                    }
                    foreach (DataRow brandData in dt_Tmp.Rows)
                    {
                        string brand = brandData["Brand"].ToString().Trim();
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
            dt_All.ColumnsStringAdd("Brand");
            //1.產生第一列表頭資料
            //2.表頭資料依照FactorySort排序
            #region --撈出Factory的KpiCode及FactorySort，依照FactorySort做排序
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
    select  iif(KpiCode='',Factory.ID,KpiCode) as KpiCode
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
                            //如果row["KpiCode"]等於跨表撈出kpicode時將表頭資料傳到dt_All
                            if (row["KpiCode"].ToString() == kpicode && !dt_All.Columns.Contains(kpicode))
                            {
                                dt_All.ColumnsDecimalAdd(row["KpiCode"].ToString());
                            }
                        }
                    }
                }
            }
            #endregion

            //1.產生表身資料-Dt_All
            //2.表身資料依照 KpiCode 及BrandID 將金額填入DataTable
            #region--依照工廠及品牌將金額填入dt_All(最後要ToExcel的DataTable)
            foreach (var brandID in brand_FtyAmt.Keys)
            {
                Dictionary<string, decimal> ftyAmt = brand_FtyAmt[brandID];
                DataRow row = dt_All.NewRow();
                row["Brand"] = brandID;
                foreach (string kpiCode in ftyAmt.Keys)
                {
                    row[kpiCode] = ftyAmt[kpiCode];
                }
                dt_All.Rows.Add(row);
            }

            //如果kpicode資料中有None，將None移到第二個位置
            foreach (string kpicode in kpiCode_All.Values)
            {
                if (kpicode == "None")
                {
                    dt_All.Columns[dt_All.Columns.Count - 1].SetOrdinal(1);
                }
            }
            #endregion

            if (null == dt_All || dt_All.Rows.Count == 0)
            {

                return new DualResult(false, "Data not found");
            }

            #region --計算總Total 和百分比及最後一列Total
            dt_All.Columns.Add("TTLAMT", typeof(Decimal));//typeof(Decimal)是設定欄位格式
            dt_All.Columns.Add("TTLPER", typeof(Decimal));
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

            //算TTLPER
            for (int i = 0; i < dt_All.Rows.Count; i++)
            {
                decimal ttlamt = Convert.ToDecimal(dt_All.Rows[i]["TTLAMT"]);
                dt_All.Rows[i]["TTLPER"] = TTColumnAMT == 0 ? 0 : decimal.Round((ttlamt / TTColumnAMT) * 100, 2);
            }
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
            Sci.Utility.Excel.SaveXltReportCls xl = new Utility.Excel.SaveXltReportCls("Centralized_R12_Transportation_Cost_Sister_Factory_Transfer.xltx");
            xl.boOpenFile = true;
            SaveXltReportCls.xltRptTable xdt_All = new SaveXltReportCls.xltRptTable(dt_All);
            xdt_All.ShowHeader = true;
            xdt_All.HeaderColor = Color.FromArgb(216, 228, 188);
            xl.dicDatas.Add("##R21UPRLLIST", xdt_All);

            if (this.checkExportDetail.Checked == true)
            {
                SaveXltReportCls.xltRptTable xdt_detail_All = new SaveXltReportCls.xltRptTable(dt_detail_All);
                xdt_detail_All.ShowHeader = false;
                xl.dicDatas.Add("##R21UNRLDETAIL", xdt_detail_All);
                xl.Save(new Sci.Production.Class.GetExcelName().GetName("Centralized_R12_Transportation_Cost_Sister_Factory_Transfer"));
            }
            else
            {
                Microsoft.Office.Interop.Excel.Application excel = xl.ExcelApp;                
                excel.Worksheets[2].Delete();
                xl.Save(new Sci.Production.Class.GetExcelName().GetName("Centralized_R12_Transportation_Cost_Sister_Factory_Transfer"));
            }

            #endregion
            return true;
        }

        #region -- Validated
        private void dateShipDate1_Validated(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.dateShipDate1.Value))
            {
                dateShipDate2.Value = Convert.ToDateTime(this.dateShipDate1.Value).GetLastDayOfMonth();
            }
        }

        private void dateApproveDate1_Validated(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.dateApproveDate1.Value))
            {
                dateApproveDate2.Value = Convert.ToDateTime(this.dateApproveDate1.Value).GetLastDayOfMonth();
            }
        }

        #endregion
    }
}
