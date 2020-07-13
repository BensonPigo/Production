using Ict;
using Sci.Data;
using Sci.Utility.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Configuration;
using System.Xml.Linq;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// R12
    /// </summary>
    public partial class R12 : Win.Tems.PrintForm
    {
        private DataTable dt_All;
        private DataTable dt_Tmp;
        private DataTable dt_detail;
        private DataTable dt_detail_All;

        private string tsql_Detail;
        private string tsql_LoadData;

        /// <summary>
        /// R12
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R12(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.comboRateTypeID.SelectedIndex = 0;
            DataTable tEMP = (DataTable)this.comboShipmodeShipmodeID.DataSource;
            tEMP.Rows.InsertAt(tEMP.NewRow(), 0);
            this.comboShipmodeShipmodeID.DataSource = tEMP;
            this.comboShipmodeShipmodeID.SelectedIndex = 0;
        }

        /// <inheritdoc/>
        protected override bool ValidateInput()
        {
            #region --檢查查詢條件
            if (!this.dateShipDate1.Value.Empty() != !this.dateShipDate2.Value.Empty())
            {
                if (this.dateShipDate1.Value.Empty())
                {
                    MyUtility.Msg.ErrorBox("[Begin Ship Date] can not be empty");
                    this.dateShipDate1.Focus();
                }

                if (this.dateShipDate2.Value.Empty())
                {
                    MyUtility.Msg.ErrorBox("[End Ship Date] can not be empty");
                    this.dateShipDate2.Focus();
                }

                return false;
            }

            if (!this.dateApproveDate1.Value.Empty() != !this.dateApproveDate2.Value.Empty())
            {
                if (this.dateApproveDate1.Value.Empty())
                {
                    MyUtility.Msg.ErrorBox("[Begin Approve Date] can not be empty");
                    this.dateApproveDate1.Focus();
                }

                if (this.dateApproveDate2.Value.Empty())
                {
                    MyUtility.Msg.ErrorBox("[End Approve Date] can not be empty");
                    this.dateApproveDate2.Focus();
                }

                return false;
            }

            if (this.comboRateTypeID.Text.Empty())
            {
                MyUtility.Msg.ErrorBox("Exchange Rate Type can not be empty !!");
                this.dateApproveDate2.Focus();
                return false;
            }
            #endregion

            bool ship_Date = false, approve_Date = false;
            string sqlWhere = string.Empty;
            List<string> sqlWheres = new List<string>();

            #region --查詢條件
            if (!this.dateShipDate1.Value.Empty() && !this.dateShipDate2.Value.Empty())
            {
                sqlWheres.Add(string.Format(
                    "fe.PortArrival between '{0}' and '{1}' ",
                    ((DateTime)this.dateShipDate1.Value).ToShortDateString(),
                    ((DateTime)this.dateShipDate2.Value).ToShortDateString()));

                ship_Date = true;
            }

            if (!this.dateApproveDate1.Value.Empty() && !this.dateApproveDate2.Value.Empty())
            {
                sqlWheres.Add(string.Format(
                    "ShippingAP.ApvDate between '{0}' and '{1}' ",
                    ((DateTime)this.dateApproveDate1.Value).ToShortDateString(),
                    ((DateTime)this.dateApproveDate2.Value).ToShortDateString()));

                approve_Date = true;
            }

            // [Ship Date] and [Approve Date] 一定要有一個是必填
            if (!ship_Date && !approve_Date)
            {
                MyUtility.Msg.ErrorBox("[Ship Date] and [Approve Date] one of the inputs must be selected");
                this.dateShipDate1.Focus();
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
                this.Transportation_list(sqlWhere);
                this.Transportation_Detail(sqlWhere);
            }
            else
            {
                this.Transportation_list(sqlWhere);
            }

            return base.ValidateInput();
        }

        #region -- summary Detail SQl

        /// <summary>
        /// Transportation_list
        /// </summary>
        /// <param name="sqlWhere">sqlWhere</param>
        public void Transportation_list(string sqlWhere)
        {
            this.tsql_LoadData = string.Format(
                @"
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

        /// <summary>
        /// Transportation_Detail
        /// </summary>
        /// <param name="sqlWhere">sqlWhere</param>
        public void Transportation_Detail(string sqlWhere)
        {
            this.tsql_Detail = @" 
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

        /// <inheritdoc/>
        protected override DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            this.dt_All = null;
            this.dt_Tmp = null;
            this.dt_detail = null;
            this.dt_detail_All = null;
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
                var connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.Contains(ss.Split(new char[] { ':' })[0].ToString())).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("Production")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();
                connectionString.Add(connections);
            }

            if (connectionString == null || connectionString.Count == 0)
            {
                return new DualResult(false, "no connection loaded.");
            }

            #endregion

            #region --依各個連線抓該連線的DB資料,並且將抓取到的資料合併到 dt_Tmp(DataTable)
            for (int i = 0; i < connectionString.Count; i++)
            {
                string conString = connectionString[i];

                // 跳提示視窗顯示跑到第幾筆連線
                this.SetLoadingText(
                    string.Format(
                        "Load data from connection {0}/{1} ",
                        i + 1,
                        connectionString.Count));
                SqlConnection conn;
                using (conn = new SqlConnection(conString))
                {
                    conn.Open();
                    DataTable factorys;
                    DataTable dropDownLists;
                    DataTable kpiCodes = new DataTable();

                    #region--先從Trade撈 DropDownList 、Factory資料
                    #region--連Trade DropDownList資料(為了Detail報表要顯示完整名稱的Category)
                    result = DBProxy.Current.SelectByConn(conn, "select id,Name,type from DropDownList   ", out dropDownLists);
                    if (!result)
                    {
                        return result;
                    }

                    // 執行DropDownLists要寫入暫存#tmpDropDownList提供tsql_LoadData(要將從Trade撈到的Factory及DropDownLists加入到tsql_LoadData，設定的變數中)使用
                    result = MyUtility.Tool.ProcessWithDatatable(dropDownLists, "id,Name,type", "Select 1", out kpiCodes, "#tmpDropDownList", conn);
                    if (!result)
                    {
                        return result;
                    }
                    #endregion

                    #region --連Trade Factory資料(取得KpiCode)
                    result = DBProxy.Current.SelectByConn(conn, "select id,KpiCode from Factory  ", out factorys);
                    if (!result)
                    {
                        return result;
                    }

                    // 執行tsql_LoadData抓Query資料，將從Trade撈到的Factory及DropDownLists加入到tsql_LoadData中設定的變數中，最後並list出有哪些KpiCode
                    result = MyUtility.Tool.ProcessWithDatatable(factorys, "ID,KpiCode", this.tsql_LoadData, out kpiCodes, "#tmpFactory", conn);
                    if (!result)
                    {
                        return result;
                    }
                    #endregion
                    #endregion

                    // 沒有資料就繼續跑下一個連線，直到所有連線都跑完
                    if (kpiCodes.Rows.Count == 0)
                    {
                        continue;
                    }

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
                    List<string> ftyAmt = new List<string>();
                    List<string> ftyList = new List<string>();
                    foreach (DataRow kpi in kpiCodes.Rows)
                    {
                        string kpiCode = kpi["KpiCode"].ToString().Trim();
                        if (!kpiCode_All.ContainsKey(kpiCode))
                        {
                            kpiCode_All.Add(kpiCode, kpiCode);
                        }

                        ftyAmt.Add("[" + kpiCode + "]=isnull([" + kpiCode + "],0)");
                        ftyList.Add("[" + kpiCode + "]");
                    }

                    listSummary = string.Format(
                        listSummary,
                        string.Join(",", ftyAmt),
                        string.Join(",", ftyList));
                    result = DBProxy.Current.SelectByConn(conn, listSummary, out this.dt_Tmp);
                    if (!result)
                    {
                        return result;
                    }

                    if (this.dt_Tmp == null || this.dt_Tmp.Rows.Count == 0)
                    {
                        return new DualResult(false, "Data not found.");
                    }

                    foreach (DataRow brandData in this.dt_Tmp.Rows)
                    {
                        string brand = brandData["Brand"].ToString().Trim();
                        Dictionary<string, decimal> brand_Fty;
                        if (brand_FtyAmt.ContainsKey(brand))
                        {
                            brand_Fty = brand_FtyAmt[brand];
                        }
                        else
                        {
                            brand_Fty = new Dictionary<string, decimal>();
                            brand_FtyAmt.Add(brand, brand_Fty);
                        }

                        foreach (DataRow kpi in kpiCodes.Rows)
                        {
                            string kpiCode = kpi["KpiCode"].ToString().Trim();
                            decimal amt = brand_Fty.ContainsKey(kpiCode)
                                ? brand_Fty[kpiCode]
                                : 0;
                            amt = amt + MyUtility.Convert.GetDecimal(brandData[kpiCode]);

                            if (brand_Fty.ContainsKey(kpiCode))
                            {
                                brand_Fty.Set(kpiCode, amt);
                            }
                            else
                            {
                                brand_Fty.Add(kpiCode, amt);
                            }
                        }
                    }
                    #endregion

                    #region --判斷是不是要列印Detail明細資料
                    if (this.checkExportDetail.Checked == true)
                    {
                        DualResult result2 = DBProxy.Current.SelectByConn(conn, this.tsql_Detail, out this.dt_detail);
                        if (!result2)
                        {
                            return result2;
                        }

                        if (this.dt_detail_All == null || this.dt_detail_All.Rows.Count == 0)
                        {
                            this.dt_detail_All = this.dt_detail;
                        }
                        else
                        {
                            this.dt_detail_All.Merge(this.dt_detail);
                        }
                    }
                    #endregion
                }
            }
            #endregion

            this.dt_All = new DataTable();
            this.dt_All.ColumnsStringAdd("Brand");

            // 1.產生第一列表頭資料
            // 2.表頭資料依照FactorySort排序
            #region --撈出Factory的KpiCode及FactorySort，依照FactorySort做排序
            for (int i = 0; i < connectionString.Count; i++)
            {
                string conString = connectionString[i];

                // 跳提示視窗顯示跑到第幾筆連線
                this.SetLoadingText(
                    string.Format(
                        "Load data from connection {0}/{1} ",
                        i + 1,
                        connectionString.Count));
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
                    if (!result)
                    {
                        return result;
                    }

                    // 如果kpicode資料中有None，將dt_factory新增None的資料
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
                            // 如果row["KpiCode"]等於跨表撈出kpicode時將表頭資料傳到dt_All
                            if (row["KpiCode"].ToString() == kpicode && !this.dt_All.Columns.Contains(kpicode))
                            {
                                this.dt_All.ColumnsDecimalAdd(row["KpiCode"].ToString());
                            }
                        }
                    }
                }
            }
            #endregion

            // 1.產生表身資料-Dt_All
            // 2.表身資料依照 KpiCode 及BrandID 將金額填入DataTable
            #region--依照工廠及品牌將金額填入dt_All(最後要ToExcel的DataTable)
            foreach (var brandID in brand_FtyAmt.Keys)
            {
                Dictionary<string, decimal> ftyAmt = brand_FtyAmt[brandID];
                DataRow row = this.dt_All.NewRow();
                row["Brand"] = brandID;
                foreach (string kpiCode in ftyAmt.Keys)
                {
                    row[kpiCode] = ftyAmt[kpiCode];
                }

                this.dt_All.Rows.Add(row);
            }

            // 如果kpicode資料中有None，將None移到第二個位置
            foreach (string kpicode in kpiCode_All.Values)
            {
                if (kpicode == "None")
                {
                    this.dt_All.Columns[this.dt_All.Columns.Count - 1].SetOrdinal(1);
                }
            }
            #endregion

            if (this.dt_All == null || this.dt_All.Rows.Count == 0)
            {
                return new DualResult(false, "Data not found");
            }

            #region --計算總Total 和百分比及最後一列Total
            this.dt_All.Columns.Add("TTLAMT", typeof(decimal)); // typeof(Decimal)是設定欄位格式
            this.dt_All.Columns.Add("TTLPER", typeof(decimal));
            int startIndex = 1;
            #region --計算總Total

            // for dt每一列
            for (int rowIdx = 0; rowIdx < this.dt_All.Rows.Count; rowIdx++)
            {
                decimal tTAMT = 0;

                // for dt每個欄位
                for (int colIdx = startIndex; colIdx < this.dt_All.Columns.Count - 2; colIdx++)
                {
                    tTAMT += Convert.ToDecimal(this.dt_All.Rows[rowIdx][colIdx]);
                }

                this.dt_All.Rows[rowIdx]["TTLAMT"] = tTAMT;
            }
            #endregion

            #region --最後一列Total
            DataRow totalrow = this.dt_All.NewRow();
            totalrow[0] = "Total";

            // for dt每個欄位
            decimal tTColumnAMT = 0;
            for (int colIdx = startIndex; colIdx < this.dt_All.Columns.Count - 1; colIdx++)
            {
                tTColumnAMT = 0;

                // for dt每一列
                for (int rowIdx = 0; rowIdx < this.dt_All.Rows.Count; rowIdx++)
                {
                    tTColumnAMT += Convert.ToDecimal(this.dt_All.Rows[rowIdx][colIdx]);
                }

                totalrow[colIdx] = tTColumnAMT;
            }

            this.dt_All.Rows.Add(totalrow);
            #endregion

            // 算TTLPER
            for (int i = 0; i < this.dt_All.Rows.Count; i++)
            {
                decimal ttlamt = Convert.ToDecimal(this.dt_All.Rows[i]["TTLAMT"]);
                this.dt_All.Rows[i]["TTLPER"] = tTColumnAMT == 0 ? 0 : decimal.Round((ttlamt / tTColumnAMT) * 100, 2);
            }
            #endregion
            return Ict.Result.True;
        }

        /// <inheritdoc/>
        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            #region --判斷DataTable有沒有資料
            if (this.checkExportDetail.Checked == true)
            {
                if (this.dt_All == null || this.dt_All.Rows.Count == 0)
                {
                    MyUtility.Msg.ErrorBox("Data not found");
                    return false;
                }

                if (this.dt_detail_All == null || this.dt_detail_All.Rows.Count == 0)
                {
                    MyUtility.Msg.ErrorBox("Data not found");
                    return false;
                }
            }
            else
            {
                if (this.dt_All == null || this.dt_All.Rows.Count == 0)
                {
                    MyUtility.Msg.ErrorBox("Data not found");
                    return false;
                }
            }
            #endregion

            #region --匯出Excel
            SaveXltReportCls xl = new SaveXltReportCls("Centralized_R12_Transportation_Cost_Sister_Factory_Transfer.xltx");
            xl.BoOpenFile = true;
            SaveXltReportCls.XltRptTable xdt_All = new SaveXltReportCls.XltRptTable(this.dt_All);
            xdt_All.ShowHeader = true;
            xdt_All.HeaderColor = Color.FromArgb(216, 228, 188);
            xl.DicDatas.Add("##R21UPRLLIST", xdt_All);

            if (this.checkExportDetail.Checked == true)
            {
                SaveXltReportCls.XltRptTable xdt_detail_All = new SaveXltReportCls.XltRptTable(this.dt_detail_All);
                xdt_detail_All.ShowHeader = false;
                xl.DicDatas.Add("##R21UNRLDETAIL", xdt_detail_All);
                xl.Save(Class.MicrosoftFile.GetName("Centralized_R12_Transportation_Cost_Sister_Factory_Transfer"));
            }
            else
            {
                Microsoft.Office.Interop.Excel.Application excel = xl.ExcelApp;
                excel.Worksheets[2].Delete();
                xl.Save(Class.MicrosoftFile.GetName("Centralized_R12_Transportation_Cost_Sister_Factory_Transfer"));
            }

            #endregion
            return true;
        }

        #region -- Validated
        private void DateShipDate1_Validated(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.dateShipDate1.Value))
            {
                this.dateShipDate2.Value = Convert.ToDateTime(this.dateShipDate1.Value).GetLastDayOfMonth();
            }
        }

        private void DateApproveDate1_Validated(object sender, EventArgs e)
        {
            if (!MyUtility.Check.Empty(this.dateApproveDate1.Value))
            {
                this.dateApproveDate2.Value = Convert.ToDateTime(this.dateApproveDate1.Value).GetLastDayOfMonth();
            }
        }

        #endregion
    }
}
