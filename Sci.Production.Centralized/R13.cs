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
using System.Xml.Linq;
using System.Configuration;
namespace Sci.Production.Centralized
{
    internal partial class R13 : Sci.Win.Tems.PrintForm
    {
        #region --宣告物件
        DataTable dt_All;
        DataTable dt_Tmp;
        DataTable dt_detail, dt_detail_All;
        DataTable dt_detail_detail, dt_detail_detail_All;
        String tsql_Detail;
        String tsql_Detail_Detail;
        String tsql_LoadData;
        #endregion
        public R13(ToolStripMenuItem menuitem)
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
            if (!this.datePulloutDate1.Value.Empty() != !this.datePulloutDate2.Value.Empty())
            {
                if (datePulloutDate1.Value.Empty())
                {
                    MyUtility.Msg.ErrorBox("[Begin Pullout Date] can not be empty");
                    datePulloutDate1.Focus();
                }
                if (datePulloutDate2.Value.Empty())
                {
                    MyUtility.Msg.ErrorBox("[End Pullout Date] can not be empty");
                    datePulloutDate2.Focus();
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
            if (!this.dateOnboardDate1.Value.Empty() != !this.dateOnboardDate2.Value.Empty())
            {
                if (dateOnboardDate1.Value.Empty())
                {
                    MyUtility.Msg.ErrorBox("[Begin On Board Date] can not  be empty");
                    dateOnboardDate1.Focus();
                }

                if (dateOnboardDate2.Value.Empty())
                {
                    MyUtility.Msg.ErrorBox("[End On Board Date] can not  be empty");
                    dateOnboardDate2.Focus();
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
            bool Pullout_Date = false,Approve_Date = false,On_Board_Date = false;
            string sqlWhere = "";
            List<string> sqlWheres = new List<string>();
            #region --查詢條件
            if (!this.datePulloutDate1.Value.Empty() && !this.datePulloutDate2.Value.Empty())
            {
                sqlWheres.Add(string.Format("Pullout_Detail.PulloutDate between '{0}' and '{1}' "
                   ,((DateTime)this.datePulloutDate1.Value).ToShortDateString()
                   ,((DateTime)this.datePulloutDate2.Value).ToShortDateString()));

                Pullout_Date = true;
            }
            if (!this.dateApproveDate1.Value.Empty() && !this.dateApproveDate2.Value.Empty())
            {
                sqlWheres.Add(string.Format("ShippingAP.ApvDate between '{0}' and '{1}' "
                   ,((DateTime)this.dateApproveDate1.Value).ToShortDateString()
                   ,((DateTime)this.dateApproveDate2.Value).ToShortDateString()));

                Approve_Date = true;
            }

            if (!this.dateOnboardDate1.Value.Empty() && !this.dateOnboardDate2.Value.Empty())
            {
                sqlWheres.Add(string.Format("GMTBooking.ETD between '{0}' and '{1}' "
                   ,((DateTime)this.dateOnboardDate1.Value).ToShortDateString()
                   ,((DateTime)this.dateOnboardDate2.Value).ToShortDateString()));

                On_Board_Date = true;
            }

            //[Pullout Date] and [Approve Date] and[On Board  Date]一定要有一個是必填
            if (!Pullout_Date && !Approve_Date && !On_Board_Date)
            {
                MyUtility.Msg.ErrorBox("[Pullout Date] and [Approve Date] and[On Board  Date]  one of the inputs must be selected");
                datePulloutDate1.Focus();
                return false;
            }

            if (!this.txtBrandBrandID.Text.Empty())
            {
                sqlWheres.Add(string.Format("GMTBooking.BrandID = '{0}'",this.txtBrandBrandID.Text));
            }

            if (!this.txtCustCDCustCDID.Text.Empty())
            {
                sqlWheres.Add(string.Format("GMTBooking.CustCDID = '{0}'",this.txtCustCDCustCDID.Text));
            }

            if (!this.txtCountryDest.TextBox1.Text.Empty())
            {
                sqlWheres.Add(string.Format("GMTBooking.Dest = '{0}'",this.txtCountryDest.TextBox1.Text));
            }

            if (!this.comboShipmodeShipmodeID.Text.Empty())
            {
                sqlWheres.Add(string.Format("GMTBooking.ShipmodeID = '{0}'",this.comboShipmodeShipmodeID.Text));
            }

            if (!this.txtForwarder.Text.Empty())
            {
                sqlWheres.Add(string.Format("GMTBooking.Forwarder ='{0}'",this.txtForwarder.Text));
            }

            sqlWhere = string.Join(" and ",sqlWheres);

            //連線的SQL語法
            #endregion
            //判斷是不是要列印Detail報表明細資料
            if (this.checkExportDetail.Checked == true)
            {
                Transportation_list(sqlWhere);
                Transportation_Detail(sqlWhere);
                Transportation_Detail_Detail(sqlWhere);
            }
            else
            {
                Transportation_list(sqlWhere);
            }
            return base.ValidateInput();
        }
        //先撈出報表Detail資料，再展成報表Summary
        #region -- summary Detail SQl
        public void Transportation_list(string sqlWhere)
        {
            tsql_LoadData = string.Format(@"
select GMTBooking.ID,
	   dbo.Getrate('{0}',ShareExpense.CurrencyID,'USD',gmtbooking.InvDate) as Rate,
	   ShareExpense.Amount * dbo.Getrate('{0}',ShareExpense.CurrencyID,'USD',gmtbooking.InvDate) as Amount,AccountID,
	   (Pullout_Detail.ID) as PulloutID,Pullout_Detail.PulloutDate,Pullout_Detail.PackingListID,
	   GMTBooking.BrandID,GMTBooking.CustCDID,GMTBooking.Dest,GMTBooking.ETD,GMTBooking.ShipModeID,
	   GMTBooking.TotalGW,GMTBooking.TotalCBM,Pullout_Detail.OrderID,PackingList.CTNQty,PackingList.GW,PackingList.CBM,Pullout_Detail.ShipQty
into #temp1
from Pullout_Detail 
inner join ShareExpense ON Pullout_Detail.INVNo = ShareExpense.InvNo 
inner join ShippingAP ON ShareExpense.ShippingAPID = ShippingAP.ID 
inner join GMTBooking ON gmtbooking.ID = pullout_detail.INVNo
inner join PackingList on PackingList.ID = Pullout_Detail.PackingListID
where ShippingAP.Type = 'EXPORT' and ShippingAP.subType <> 'SISTER FACTORY TRANSFER' and ", this.comboRateTypeID.Text) + sqlWhere+
 @"
select * 
into #temp3 
from (
	select distinct OrderID,ID,
		   PulloutID,PulloutDate,BrandID,CustCDID,Dest,ETD,ShipModeID,TotalGW,TotalCBM,ShipQty,CTNQty,GW,CBM
	from #temp1 
) as tt
outer apply (select
	         isnull((select sum(Amount) from #temp1 where ID = tt.ID and OrderID=tt.OrderID and AccountID = '61022001'),0) as Fee01,
	         isnull((select sum(Amount) from #temp1 where ID = tt.ID and OrderID=tt.OrderID and AccountID = '61022002'),0) as Fee02,
	         isnull((select sum(Amount) from #temp1 where ID = tt.ID and OrderID=tt.OrderID and AccountID = '61022003'),0) as Fee03,
	         isnull((select sum(Amount) from #temp1 where ID = tt.ID and OrderID=tt.OrderID and AccountID = '61022004'),0) as Fee04,
	         isnull((select sum(Amount) from #temp1 where ID = tt.ID and OrderID=tt.OrderID and AccountID = '61022005'),0) as Fee05,
	         isnull((select sum(Amount) from #temp1 where ID = tt.ID and OrderID=tt.OrderID and ( AccountID LIKE'6105%' or AccountID = '59121111')),0) as FeeAir,
	         isnull((select sum(Amount) from #temp1 where ID = tt.ID and OrderID=tt.OrderID and AccountID not in('61022001','61022002','61022003','61022004','61022005','59121111')AND AccountID NOT LIKE '6105%'),0) as Feeother
) as s
outer apply (select Feeother+Fee01+Fee02+Fee03+Fee04+Fee05+FeeAir as FeeAmo) s2
order by ID,OrderID


select query.ID,o.BrandID,DropDownList.Name as Category,query.OrderID,o.BuyerDelivery,o.Qty,o.CustCDID,o.Dest,query.ShipModeID,PL='',
       query.PulloutID,query.PulloutDate,query.ShipQty,query.CTNQty,query.GW,query.CBM,gmForwarder.Forwarder,
	   BL = '',
	   isnull(round(query.Fee01*Ship.ShipAmount,2),0) as Fee01,
	   isnull(round(query.Fee02*Ship.ShipAmount,2),0) as Fee02,
	   isnull(round(query.Fee03*Ship.ShipAmount,2),0) as Fee03,
	   isnull(round(query.Fee04*Ship.ShipAmount,2),0) as Fee04,
	   isnull(round(query.Fee05*Ship.ShipAmount,2),0) as Fee05,
	   isnull(round(query.Feeother*Ship.ShipAmount,2),0) as Feeother,
	   isnull(round(query.FeeAir*Ship.ShipAmount,2),0) as FeeAir,
	   isnull(Working.WorkAmount,0) as feeAmount,isnull(round(FeeAmo,2),0) as FeeAmo,
       iif( isnull(f.KpiCode,'') ='',isnull(o.FactoryID,'None'),f.KpiCode) as KpiCode,o.FactoryID
into #data
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
where (iif( isnull(f.KpiCode,'') = '',o.FactoryID,f.KpiCode)) is not null
drop table #temp1
drop table #temp3

select distinct KpiCode from #data where FeeAir = 0 order by KpiCode
 ";

        }

        //抓Transportation_list sql Function 資料 完成Sheep2
        public void Transportation_Detail(string sqlWhere)
        {
            tsql_Detail = @"
select ID,FactoryID,BrandID,Category,OrderID,BuyerDelivery,Qty,CustCDID,Dest,ShipModeID,PL,PulloutID,PulloutDate,ShipQty,CTNQty,GW,CBM,Forwarder,BL,
Fee01,Fee02,Fee03,Fee04,Fee05,Feeother,FeeAir,feeAmount,FeeAmo
from #data where FeeAir = 0
order by ID,OrderID";
         }

        //抓Transportation_list sql Function 資料 完成Sheep3
        public void Transportation_Detail_Detail(string sqlWhere)
         {
             tsql_Detail_Detail = @"
select ID,FactoryID,BrandID,Category,OrderID,BuyerDelivery,Qty,CustCDID,Dest,ShipModeID,PL,PulloutID,PulloutDate,ShipQty,CTNQty,GW,CBM,Forwarder,BL,
Fee01,Fee02,Fee03,Fee04,Fee05,Feeother,FeeAir,feeAmount,FeeAmo
from #data where FeeAir <> 0
order by ID,OrderID";
         }

        #endregion

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {
            dt_All = null;
            dt_Tmp = null;
            dt_detail = null;
            dt_detail_All = null;
            dt_detail_detail = null;
            dt_detail_detail_All = null;
            DualResult result;
            SortedList<string,string> kpiCode_All = new SortedList<string,string>();

            Dictionary<string,Dictionary<string,decimal>> brand_FtyAmt = new Dictionary<string,Dictionary<string,decimal>>();

            #region --由Factory.PmsPath抓各個連線路徑
            this.SetLoadingText("Load connections... ");
            XDocument docx = XDocument.Load(Application.ExecutablePath + ".config");
            string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
            List<string> connectionString = new List<string>();
            foreach (string ss in strSevers)
            {
                var Connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.Contains(ss.Split(new char[] { ':' })[0].ToString())).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("Production")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();
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
                   ,(i + 1),connectionString.Count));

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
@"select BrandID,{0}

from (
	   select BrandID,KpiCode,feeAmount=SUM(feeAmount) from #data  where FeeAir = 0 group by BrandID,KpiCode
     ) 
      a pivot
	(
       sum(feeAmount) for KpiCode in ({1})
    ) b
order by BrandID
";
                    List<String> ftyAmt = new List<string>();
                    List<String> ftyList = new List<string>();
                   
                    foreach (DataRow kpi in kpiCodes.Rows)
                    {
                        string kpiCode = kpi["KpiCode"].ToString().Trim();
                        if (!kpiCode_All.ContainsKey(kpiCode)) { kpiCode_All.Add(kpiCode,kpiCode); }
                        ftyAmt.Add("[" + kpiCode + "]=isnull([" + kpiCode + "],0)");
                        ftyList.Add("[" + kpiCode + "]");
                    }

                    listSummary = string.Format(listSummary
                       ,string.Join(",",ftyAmt)
                       ,string.Join(",",ftyList));

                    // 執行pivot
                     result = DBProxy.Current.SelectByConn(conn,listSummary,out dt_Tmp);
                    if (!result) { return result; }
                    #endregion
                    if (null == dt_Tmp || dt_Tmp.Rows.Count == 0)
                    {
                        return new DualResult(false,"Data not found.");
                    }
                    
                    foreach (DataRow brandData in dt_Tmp.Rows)
                    {
                        string brand = brandData["BrandID"].ToString().Trim();
                        Dictionary<string,decimal> brand_Fty;
                        if (brand_FtyAmt.ContainsKey(brand)) { brand_Fty = brand_FtyAmt[brand]; }
                        else { brand_Fty = new Dictionary<string,decimal>(); brand_FtyAmt.Add(brand,brand_Fty); }

                        foreach (DataRow kpi in kpiCodes.Rows)
                        {
                            string kpiCode = kpi["KpiCode"].ToString().Trim();
                            decimal Amt = brand_Fty.ContainsKey(kpiCode)
                                ? brand_Fty[kpiCode]
                                : 0;
                            Amt = Amt + MyUtility.Convert.GetDecimal(brandData[kpiCode]);

                            if (brand_Fty.ContainsKey(kpiCode)) { brand_Fty.Set(kpiCode,Amt); }
                            else { brand_Fty.Add(kpiCode,Amt); }
                        }

                    }
                    //判斷是不是要列印Detail明細資料
                    if (this.checkExportDetail.Checked == true)
                    {
                        DualResult result2 = DBProxy.Current.SelectByConn(conn,tsql_Detail,out dt_detail);
                        if (!result2) { return result2; }
                        if (null == dt_detail_All || 0 == dt_detail_All.Rows.Count)
                        {
                            dt_detail_All = dt_detail;
                        }
                        else
                        {
                            dt_detail_All.Merge(dt_detail);//如果dt_detail_All有資料時，將資料Merge上去
                        }
                        DualResult result3 = DBProxy.Current.SelectByConn(conn,tsql_Detail_Detail,out dt_detail_detail);
                        if (!result3) { return result3; }
                        
                        if (null == dt_detail_detail_All || 0 == dt_detail_detail_All.Rows.Count)
                        {
                            dt_detail_detail_All = dt_detail_detail;
                        }
                        else
                        {
                            dt_detail_detail_All.Merge(dt_detail_detail);//如果dt_detail_All有資料時，將資料Merge上去
                        }
                    }
                }
            }
            #endregion

            dt_All = new DataTable();
            dt_All.ColumnsStringAdd("BrandID");

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
                Dictionary<string,decimal> ftyAmt = brand_FtyAmt[brandID];
                DataRow row = dt_All.NewRow();
                row["BrandID"] = brandID;
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

                return new DualResult(false,"Data not found");
            }

            #region --計算總Total 和百分比及最後一列Total
            dt_All.Columns.Add("TTLAMT",typeof(Decimal));//typeof(Decimal)是設定欄位格式
            dt_All.Columns.Add("TTLPER",typeof(Decimal));
            int startIndex = 1;
            //for dt每一列
            #region --計算總Total
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

            #region --算最後一個欄位的百分比
            for (int i = 0; i < dt_All.Rows.Count; i++)
            {
                decimal ttlamt = Convert.ToDecimal(dt_All.Rows[i]["TTLAMT"]);
                //TTColumnAMT為0時不做計算，直接帶0
                dt_All.Rows[i]["TTLPER"] =TTColumnAMT==0?0: decimal.Round((ttlamt / TTColumnAMT) * 100,2);
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
                if ((dt_detail_detail_All == null || dt_detail_detail_All.Rows.Count == 0) && (dt_detail_All == null || dt_detail_All.Rows.Count == 0))
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

            Sci.Utility.Excel.SaveXltReportCls xl = new Utility.Excel.SaveXltReportCls("Centralized_R13_Transportation_Cost_Garment_Export_fee.xltx");
            xl.boOpenFile = true;
            SaveXltReportCls.xltRptTable xdt_All = new SaveXltReportCls.xltRptTable(dt_All);
            xdt_All.ShowHeader = true;
            xdt_All.HeaderColor = Color.FromArgb(216, 228, 188);

            xl.dicDatas.Add("##R13UPRLLIST", xdt_All);
            if (this.checkExportDetail.Checked == true)
            {
                SaveXltReportCls.xltRptTable xdt_detail_All = new SaveXltReportCls.xltRptTable(dt_detail_All);
                xdt_detail_All.ShowHeader = false;
                SaveXltReportCls.xltRptTable xdt_detail_detail_All = new SaveXltReportCls.xltRptTable(dt_detail_detail_All);
                xdt_detail_detail_All.ShowHeader = false;
                xl.dicDatas.Add("##R13UNRLDETAIL", xdt_detail_All);
                xl.dicDatas.Add("##R13UNRLDETAILDETAIL", xdt_detail_detail_All);                
                xl.Save();
            }
            else
            {
                Microsoft.Office.Interop.Excel.Application excel = xl.ExcelApp;
                xl.Save();
                excel.Worksheets[3].Delete();
                excel.Worksheets[2].Delete();
            }
            #endregion
            return true;
        }

        #region --控制項Validated
        private void datePulloutDate1_Validated(object sender,EventArgs e)
        {
            if (!datePulloutDate1.Value.Empty())
            {
                datePulloutDate2.Value = Convert.ToDateTime(this.datePulloutDate1.Value).GetLastDayOfMonth();
            }
        }

        private void dateApproveDate1_Validated(object sender,EventArgs e)
        {
            if (!dateApproveDate1.Value.Empty())
            {
                dateApproveDate2.Value = Convert.ToDateTime(this.dateApproveDate1.Value).GetLastDayOfMonth();
            }
        }

        private void dateOnboardDate1_Validated(object sender,EventArgs e)
        {
            if (!dateOnboardDate1.Value.Empty())
            {
                dateOnboardDate2.Value = Convert.ToDateTime(this.dateOnboardDate1.Value).GetLastDayOfMonth();
            }
        }
        #endregion
    }
}
  