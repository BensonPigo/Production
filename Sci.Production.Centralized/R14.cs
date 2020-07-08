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
using System.Xml.Linq;
using System.Configuration;

namespace Sci.Production.Centralized
{
    /// <summary>
    /// R14
    /// </summary>
    internal partial class R14 : Sci.Win.Tems.PrintForm
    {
        private DataTable dt_All;
        private DataTable dt_Tmp;
        private DataTable dt_detail;
        private DataTable dt_detail_All;
        private string tsql_Detail;
        private string tsql_LoadData;

        /// <summary>
        /// R14
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public R14(ToolStripMenuItem menuitem)
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
            if (!this.datePulloutDate1.Value.Empty() != !this.datePulloutDate2.Value.Empty())
            {
                if (this.datePulloutDate1.Value.Empty())
                {
                    MyUtility.Msg.ErrorBox("[Begin Pullout Date] can not be empty");
                    this.datePulloutDate1.Focus();
                }

                if (this.datePulloutDate2.Value.Empty())
                {
                    MyUtility.Msg.ErrorBox("[End Pullout Date] can not be empty");
                    this.datePulloutDate2.Focus();
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

            bool pullout_Date = false, approve_Date = false;
            string sqlWhere = string.Empty;
            List<string> sqlWheres = new List<string>();

            #region --查詢條件
            if (!this.datePulloutDate1.Value.Empty() && !this.datePulloutDate2.Value.Empty())
            {
                sqlWheres.Add(string.Format(
                    "Pullout_Detail.PulloutDate between '{0}' and '{1}' ",
                    ((DateTime)this.datePulloutDate1.Value).ToShortDateString(),
                    ((DateTime)this.datePulloutDate2.Value).ToShortDateString()));
                pullout_Date = true;
            }

            if (!this.dateApproveDate1.Value.Empty() && !this.dateApproveDate2.Value.Empty())
            {
                sqlWheres.Add(string.Format(
                    "ShippingAP.ApvDate between '{0}' and '{1}' ",
                    ((DateTime)this.dateApproveDate1.Value).ToShortDateString(),
                    ((DateTime)this.dateApproveDate2.Value).ToShortDateString()));
                approve_Date = true;
            }

            // [Pullout Date] and [Approve Date] 一定要有一個是必填
            if (!pullout_Date && !approve_Date)
            {
                MyUtility.Msg.ErrorBox("[Pullout Date] and [Approve Date] one of the inputs must be selected");
                this.datePulloutDate1.Focus();
                return false;
            }

            if (!this.txtBrandBrandID.Text.Empty())
            {
                sqlWheres.Add(string.Format("GMTBooking.BrandID = '{0}'", this.txtBrandBrandID.Text));
            }

            if (!this.txtCustCDCustCDID.Text.Empty())
            {
                sqlWheres.Add(string.Format("GMTBooking.CustCDID = '{0}'", this.txtCustCDCustCDID.Text));
            }

            if (!this.txtCountryDest.TextBox1.Text.Empty())
            {
                sqlWheres.Add(string.Format("GMTBooking.Dest = '{0}'", this.txtCountryDest.TextBox1.Text));
            }

            if (!this.comboShipmodeShipmodeID.Text.Empty())
            {
                sqlWheres.Add(string.Format("GMTBooking.ShipmodeID = '{0}'", this.comboShipmodeShipmodeID.Text));
            }

            if (!this.txtForwarder.Text.Empty())
            {
                sqlWheres.Add(string.Format("GMTBooking.Forwarder = '{0}'", this.txtForwarder.Text));
            }
            #endregion

            sqlWhere = string.Join(" and ", sqlWheres);

            // 判斷是不是要列印Detail報表明細資料
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

        // 先撈出報表Detail資料，再展成報表Summary
        #region -- summary Detail SQl

        /// <summary>
        /// 抓Transportation_list sql Function 資料 完成Sheep2
        /// </summary>
        /// <param name="sqlWhere">sqlWhere</param>
        public void Transportation_list(string sqlWhere)
        {
            this.tsql_LoadData = string.Format(
                @"
select GMTBooking.ID,
		dbo.Getrate('{0}',ShareExpense.CurrencyID,'USD',gmtbooking.InvDate) as Rate,
		ShareExpense.Amount * dbo.Getrate('{0}',ShareExpense.CurrencyID,'USD',gmtbooking.InvDate) as Amount,AccountID,
		(Pullout_Detail.ID) as PulloutID,Pullout_Detail.PulloutDate,Pullout_Detail.PackingListID,
		GMTBooking.BrandID,GMTBooking.CustCDID,GMTBooking.Dest,GMTBooking.ETD,GMTBooking.ShipModeID,
		GMTBooking.TotalGW,GMTBooking.TotalCBM,Pullout_Detail.OrderID,PackingList.CTNQty,PackingList.GW,PackingList.CBM,Pullout_Detail.ShipQty
		into #temp1
from Pullout_Detail 
inner join ShareExpense on Pullout_Detail.INVNo = ShareExpense.InvNo 
inner join ShippingAP on ShareExpense.ShippingAPID = ShippingAP.ID 
inner join GMTBooking on gmtbooking.ID = pullout_detail.INVNo
inner join PackingList on PackingList.ID = Pullout_Detail.PackingListID
Where ShippingAP.Type = 'EXPORT' and ShippingAP.subType <> 'SISTER FACTORY TRANSFER' and ", this.comboRateTypeID.Text) + sqlWhere +
@"
select * 
into #temp3 
from (
	select distinct OrderID,ID,
		   PulloutID,PulloutDate,BrandID,CustCDID,Dest,ETD,ShipModeID,TotalGW,TotalCBM,ShipQty,CTNQty,GW,CBM
	from #temp1 
) as tt
outer apply (select
	         isnull((select sum(Amount) from #temp1 where ID = tt.ID and OrderID = tt.OrderID and AccountID = '61022001'),0) as Fee01,
	         isnull((select sum(Amount) from #temp1 where ID = tt.ID and OrderID = tt.OrderID and AccountID = '61022002'),0) as Fee02,
	         isnull((select sum(Amount) from #temp1 where ID = tt.ID and OrderID = tt.OrderID and AccountID = '61022003'),0) as Fee03,
	         isnull((select sum(Amount) from #temp1 where ID = tt.ID and OrderID = tt.OrderID and AccountID = '61022004'),0) as Fee04,
	         isnull((select sum(Amount) from #temp1 where ID = tt.ID and OrderID = tt.OrderID and AccountID = '61022005'),0) as Fee05,
	         isnull((select sum(Amount) from #temp1 where ID = tt.ID and OrderID = tt.OrderID and ( AccountID LIKE'6105%' OR AccountID = '59121111')),0) as FeeAir,
	         isnull((select sum(Amount) from #temp1 where ID = tt.ID and OrderID = tt.OrderID and AccountID not in('61022001','61022002','61022003','61022004','61022005','59121111')AND AccountID NOT LIKE '6105%'),0) as Feeother
) as s
outer apply (select Feeother+Fee01+Fee02+Fee03+Fee04+Fee05+FeeAir as FeeAmo) s2
order by ID,OrderID

select query.ID,o.BrandID,DropDownList.Name as Category,query.OrderID,o.BuyerDelivery,o.Qty,o.CustCDID,o.Dest,query.ShipModeID,PL = '',
       query.PulloutID,query.PulloutDate,query.ShipQty,query.CTNQty,query.GW,query.CBM,gmForwarder.Forwarder,
	   BL = '',
	   isnull(round(query.Fee01*Ship.ShipAmount,2),0) as Fee01,
	   isnull(round(query.Fee02*Ship.ShipAmount,2),0) as Fee02,
	   isnull(round(query.Fee03*Ship.ShipAmount,2),0) as Fee03,
	   isnull(round(query.Fee04*Ship.ShipAmount,2),0) as Fee04,
	   isnull(round(query.Fee05*Ship.ShipAmount,2),0) as Fee05,
	   isnull(round(query.Feeother*Ship.ShipAmount,2),0) as Feeother,
	   isnull(round(query.FeeAir*Ship.ShipAmount,2),0) as FeeAir,
	   isnull(Working.WorkAmount,0) as feeAmount,FeeAmo,
       iif( isnull(f.KpiCode,'') = '',o.FactoryID,f.KpiCode) as KpiCode,
       o.FactoryID
into #test
from #temp3 AS query
left join Orders as o ON query.OrderID = o.ID 
left join #tmpFactory as f on f.ID = o.FactoryID
left join #tmpDropDownList as DropDownList on DropDownList.ID = o.Category and DropDownList.Type = 'Category' 
outer apply(select (GMTBooking.Forwarder + '-' + LocalSupp.Name) as Forwarder
			From GMTBooking inner join LocalSupp on GMTBooking.Forwarder = LocalSupp.ID
			where GMTBooking.ID = query.ID) as gmForwarder
outer apply(select iif(query.ShipModeID = 'A/P',iif(query.TotalGW = 0,0,(query.GW/query.TotalGW)),iif(query.TotalCBM = 0,0,(query.CBM/query.TotalCBM))) as ShipAmount ) as Ship
outer apply(select isnull(round(query.Fee01*Ship.ShipAmount,2),0)+ 
                   isnull(round(query.Fee02*Ship.ShipAmount,2),0)+
                   isnull(round(query.Fee03*Ship.ShipAmount,2),0)+
                   isnull(round(query.Fee04*Ship.ShipAmount,2),0)+
                   isnull(round(query.Fee05*Ship.ShipAmount,2),0)+
                   isnull(round(query.Feeother*Ship.ShipAmount,2),0)+
                   isnull(round(query.FeeAir*Ship.ShipAmount,2),0) as WorkAmount) as Working
where isnull(round(query.FeeAir*Ship.ShipAmount,2),0)<>0 and (iif( isnull(f.KpiCode,'') = '',o.FactoryID,f.KpiCode)) is not null
drop table #temp1
drop table #temp3

select distinct KpiCode from #test group by KpiCode order by KpiCode";
        }

        /// <summary>
        /// Transportation_Detail
        /// </summary>
        /// <param name="sqlWhere">sqlWhere</param>
        public void Transportation_Detail(string sqlWhere)
        {
            this.tsql_Detail = @"
select ID,FactoryID,BrandID,Category,OrderID,BuyerDelivery,Qty,CustCDID,Dest,ShipModeID,PL,PulloutID,PulloutDate,ShipQty,CTNQty,GW,CBM,Forwarder,BL,
       Fee01,Fee02,Fee03,Fee04,Fee05,Feeother,FeeAir,feeAmount,FeeAmo
from #test
order by ID,OrderID";
        }
        #endregion

        /// <inheritdoc/>
        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            this.dt_All = null;
            this.dt_Tmp = null;
            this.dt_detail = null;
            this.dt_detail_All = null;
            DualResult result;
            SortedList<string, string> kpiCode_All = new SortedList<string, string>();
            Dictionary<string, Dictionary<string, decimal>> brand_FtyAmt = new Dictionary<string, Dictionary<string, decimal>>();

            #region --由Factory.PmsPath抓各個連線路徑
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

                    // 連Trade DropDownList資料(為了Detail報表要顯示完整名稱的Category)
                    #region--連Trade DropDownList資料
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

                    // 用KpiCode重組Pivot的Tsql
                    #region --將KpiCode組到listSummary(Pivot SQL變數上)，並執行listSummary(pivot)
                    string listSummary =
@"select BrandID,{0}

from (
	select BrandID,KpiCode,feeAmount = SUM(feeAmount) from #test group by BrandID,KpiCode
) a pivot
	(sum(feeAmount) for KpiCode in ({1})
) b
order by BrandID
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

                        ftyAmt.Add("[" + kpiCode + "] = isnull([" + kpiCode + "],0)");
                        ftyList.Add("[" + kpiCode + "]");
                    }

                    listSummary = string.Format(
                        listSummary,
                        string.Join(",", ftyAmt),
                        string.Join(",", ftyList));

                    // 執行pivot
                    result = DBProxy.Current.SelectByConn(conn, listSummary, out this.dt_Tmp);
                    if (!result)
                    {
                        return result;
                    }

                    foreach (DataRow brandData in this.dt_Tmp.Rows)
                    {
                        string brand = brandData["BrandID"].ToString().Trim();
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

                    // 判斷是不是要列印Detail明細資料
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
                            this.dt_detail_All.Merge(this.dt_detail); // 如果dt_detail_All有資料時，將資料Merge上去
                        }
                    }
                }
            }
            #endregion

            this.dt_All = new DataTable();
            this.dt_All.ColumnsStringAdd("BrandID");

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
                row["BrandID"] = brandID;
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

                if (this.dt_All == null || this.dt_All.Rows.Count == 0)
                {
                    return new DualResult(false, "Data not found");
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

            #region --算最後一個欄位的百分比

            // 算TTLPER
            for (int i = 0; i < this.dt_All.Rows.Count; i++)
            {
                // TTColumnAMT為0時不做計算，直接帶0
                if (tTColumnAMT == 0)
                {
                    this.dt_All.Rows[i]["TTLPER"] = 0;
                }
                else
                {
                    decimal ttlamt = Convert.ToDecimal(this.dt_All.Rows[i]["TTLAMT"]);
                    this.dt_All.Rows[i]["TTLPER"] = decimal.Round((ttlamt / tTColumnAMT) * 100, 2);
                }
            }
            #endregion
            #endregion
            return Result.True;
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
            Sci.Utility.Excel.SaveXltReportCls xl = new Utility.Excel.SaveXltReportCls("Centralized_R14_Transportation_Cost_Air_Freight.xltx");
            xl.BoOpenFile = true;
            SaveXltReportCls.XltRptTable xdt_All = new SaveXltReportCls.XltRptTable(this.dt_All);
            xdt_All.ShowHeader = true;
            xdt_All.HeaderColor = Color.FromArgb(216, 228, 188);

            xl.DicDatas.Add("##R14UPRLLIST", xdt_All);

            if (this.checkExportDetail.Checked == true)
            {
                SaveXltReportCls.XltRptTable xdt_detail_All = new SaveXltReportCls.XltRptTable(this.dt_detail_All);
                xdt_detail_All.ShowHeader = false;
                xl.DicDatas.Add("##R14UNRLDETAIL", xdt_detail_All);
                xl.Save(Sci.Production.Class.MicrosoftFile.GetName("Centralized_R14_Transportation_Cost_Air_Freight"));
            }
            else
            {
                Microsoft.Office.Interop.Excel.Application excel = xl.ExcelApp;
                excel.Worksheets[2].Delete();
                xl.Save(Sci.Production.Class.MicrosoftFile.GetName("Centralized_R14_Transportation_Cost_Air_Freight"));
            }
            #endregion
            return true;
        }

        #region --控制項Validated
        private void DatePulloutDate1_Validated(object sender, EventArgs e)
        {
            if (!this.datePulloutDate1.Value.Empty())
            {
                this.datePulloutDate2.Value = Convert.ToDateTime(this.datePulloutDate1.Value).GetLastDayOfMonth();
            }
        }

        private void DateApproveDate1_Validated(object sender, EventArgs e)
        {
            if (!this.dateApproveDate1.Value.Empty())
            {
                this.dateApproveDate2.Value = Convert.ToDateTime(this.dateApproveDate1.Value).GetLastDayOfMonth();
            }
        }
        #endregion
    }
}
