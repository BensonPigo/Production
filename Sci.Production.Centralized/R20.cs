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
    internal partial class R20 : Sci.Win.Tems.PrintForm
    {
        DataTable DT1;        
        string WhereStockStr = "", WhereSewingStr = "", WhereOnBoardStr = "", WherePulloutStr = "", WherePMS = "";
        string tsql_Production = "", tsql_Stock, tsql_Trade = "", tsql_Query = "";                
        string FOBorCPUifDetail = "";

        public R20(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            InitializeComponent();
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            this.radioCPU.Checked = true;            
            this.comboDropdownlist.SelectedIndex = 5;
        }

        protected override bool ValidateInput()
        {
            #region 判斷不可為空的條件
            if (this.dateQueryDateStart.Value.Empty() || this.dateQueryDateEnd.Value.Empty())
            {
                if (this.dateQueryDateStart.Value.Empty())
                {
                    MyUtility.Msg.ErrorBox("[Query Date Start] can't be  Empty");
                    this.dateQueryDateStart.Focus();
                }
                else
                {
                    MyUtility.Msg.ErrorBox("[Query Date End] can't be  Empty");
                    this.dateQueryDateEnd.Focus();
                }
                return false;
            }
            if (this.comboDropdownlist.SelectedValue.Empty())
            {
                MyUtility.Msg.ErrorBox("[Category] can't be Empty!!");
                return false;
            }
            #endregion

            #region 設定Where條件
            List<string> StockStrS = new List<string>();
            List<string> SewingStrS = new List<string>();
            List<string> OnBoardStrS = new List<string>();
            List<string> PulloutStrS = new List<string>();
            List<string> PMSStrS = new List<string>();

            string _QueryDateStart = ((DateTime)this.dateQueryDateStart.Value).ToShortDateString();
            string _QueryDateEnd = ((DateTime)this.dateQueryDateEnd.Value).ToShortDateString();
            string _Category = this.comboDropdownlist.SelectedValue.ToString();
            string _Factory = this.txtCentralizedFactory.Text;
            string _Brand = this.txtbrand.Text;
            string _Regioin = this.txtcountry.TextBox1.Text;

            if (!this.dateQueryDateStart.Value.Empty() || !this.dateQueryDateEnd.Value.Empty())
            {
                SewingStrS.Add(string.Format("so.OutputDate between '{0}' and  '{1}'", _QueryDateStart, _QueryDateEnd));
                OnBoardStrS.Add(string.Format("gi.ETD between '{0}' and '{1}'", _QueryDateStart, _QueryDateEnd));
                PulloutStrS.Add(string.Format("pd.PulloutDate between '{0}' and '{1}'", _QueryDateStart, _QueryDateEnd));
            }

            if (!this.comboDropdownlist.SelectedValue.Empty())
            {
                StockStrS.Add(string.Format("o.Category in ({0})", _Category));
                SewingStrS.Add(string.Format("o.Category in ({0})", _Category));
                OnBoardStrS.Add(string.Format("o.Category in ({0})", _Category));
                PulloutStrS.Add(string.Format("o.Category in ({0})", _Category));
            }

            if (!txtCentralizedFactory.Text.Empty())
            {
                StockStrS.Add(string.Format("o.FactoryID = '{0}'", _Factory));
                SewingStrS.Add(string.Format("o.FactoryID = '{0}'", _Factory));
                OnBoardStrS.Add(string.Format("o.FactoryID = '{0}'", _Factory));
                PulloutStrS.Add(string.Format("o.FactoryID = '{0}'", _Factory));
                PMSStrS.Add(string.Format("o.FactoryID = '{0}'", _Factory));
            }

            if (!txtbrand.Text.Empty())
            {
                StockStrS.Add(string.Format("o.BrandID = '{0}'", _Brand));
                SewingStrS.Add(string.Format("o.BrandID = '{0}'", _Brand));
                OnBoardStrS.Add(string.Format("o.BrandID = '{0}'", _Brand));
                PulloutStrS.Add(string.Format("o.BrandID = '{0}'", _Brand));
            }

            if (!txtcountry.TextBox1.Text.Empty())
            {
                StockStrS.Add(string.Format("o.FactoryID in (select ID from #tmpFactory Where CountryID = '{0}')", _Regioin));
                SewingStrS.Add(string.Format("o.FactoryID in (select ID from Factory Where CountryID = '{0}')", _Regioin));
                OnBoardStrS.Add(string.Format("o.FactoryID in (select ID from Factory Where CountryID = '{0}')", _Regioin));
                PulloutStrS.Add(string.Format("o.FactoryID in (select ID from Factory Where CountryID = '{0}')", _Regioin));
                PMSStrS.Add(string.Format("o.FactoryID in (select ID from Factory Where CountryID = '{0}')", _Regioin));
            }

            if (StockStrS.Count > 0)
            {
                WhereStockStr = " AND " + string.Join(" AND ", StockStrS);
            }
            else
            {
                WhereStockStr = "";
            }

            WhereSewingStr = " AND " + string.Join(" AND ", SewingStrS);
            WhereOnBoardStr = " AND " + string.Join(" AND ", OnBoardStrS);
            WherePulloutStr = " AND " + string.Join(" AND ", PulloutStrS);
            if (PMSStrS.Count > 0)
                WherePMS = " AND " + string.Join(" AND ", PMSStrS);
            else WherePMS = "";
            #endregion

            #region 判別是CPU or FOB、是否輸出明細
            /*cpu or fob*/
            if (radioCPU.Checked)
            {
                FOBorCPUifDetail = "CPU";
            }
            else
            {
                FOBorCPUifDetail = "FOB";
            }

            /*Export by SP#*/
            if (checkExportbySP.Checked)
            {
                FOBorCPUifDetail += "Detail";
            }
            else
            {
                FOBorCPUifDetail += "Total";
            }
            #endregion

            #region Stock SQL
            tsql_Production = string.Format(
@"
--Production
select o.FactoryID,o.ID,o.CurrencyID,Qty = pld.ShipQty,CpuRate,TotalCpu = round(o.CPU*pld.ShipQty*CpuRate,2),pld.OrderID,pld.Article,pld.SizeCode
from PackingList pl
inner join PackingList_Detail pld on pl.ID = pld.ID
inner join #tmpOrders o on pld.OrderID = o.ID
left join pullout p on pl.PulloutID = p.ID
outer apply(select CpuRate from dbo.GetCPURate(o.OrderTypeID,o.ProgramID,o.Category,o.BrandID,'O')) as CPURate
where pl.AddDate <= '{0}' and pld.ReceiveDate < '{0}' and (p.Status = 'New' or p.Status is null) and o.GMTComplete not in ('S','C')" + WhereStockStr +
@"
union all
select o.FactoryID,o.ID,o.CurrencyID,Qty = pld.ShipQty,CpuRate,TotalCpu = round(o.CPU*pld.ShipQty*CpuRate,2),pld.OrderID,pld.Article,pld.SizeCode
from PackingList pl
inner join PackingList_Detail pld on pl.ID = pld.ID
inner join #tmpOrders o on pld.OrderID = o.ID
left join pullout p on pl.PulloutID = p.ID
inner join #tmpGMTBooking tGI ON INVNo = tGI.ID
outer apply(select CpuRate from dbo.GetCPURate(o.OrderTypeID,o.ProgramID,o.Category,o.BrandID,'O')) as CPURate
where pl.AddDate < '{0}' and pld.ReceiveDate < '{0}' and p.Status <> 'New' and ( tGi.ETD > '{0}' or tGi.ETD is null ) " + WhereStockStr, _QueryDateEnd);

            tsql_Stock =
@"
select p.FactoryID,p.ID,p.CurrencyID,
       Qty = sum(p.Qty),CpuRate,
       TotalCpu = sum(TotalCpu),
       TotalFOB = Sum(round(p.Qty * Price.price,2)),
	   TotalUSD = sum(round(p.Qty * Price.price,2))/IIF(p.CurrencyID = 'CNY',6.1,1)
from #Production p
outer apply(select dbo.GetPoPriceByArticleSize(OrderID,Article,SizeCode) as price) as Price
group by p.FactoryID,p.ID,p.CurrencyID,CpuRate
";
            #endregion

            #region 主要Trade SQL 要額外排除Local訂單
            tsql_Trade =
@"
IF OBJECT_ID('tempdb.dbo.#Sewing') IS NOT NULL 
BEGIN
    DROP TABLE #Sewing
END
IF OBJECT_ID('tempdb.dbo.#OnBoard') IS NOT NULL 
BEGIN
    DROP TABLE #OnBoard
END
IF OBJECT_ID('tempdb.dbo.#Pullout') IS NOT NULL 
BEGIN
    DROP TABLE #Pullout
END
IF OBJECT_ID('tempdb.dbo.#Detail') IS NOT NULL 
BEGIN
    DROP TABLE #Detail
END

--Sewing
select so.FactoryID,o.ID,o.CurrencyID,
       Qty = sum(iif(o.StyleUnit = 'SETS',sodd.QAQty*SuitRate,sodd.QAQty)),
       CpuRate,
       TotalCpu = sum(round(sodd.QAQty*o.CPU*CpuRate*SuitRate,2)),
	   TotalFOB = sum(round(iif(o.StyleUnit = 'SETS',sodd.QAQty*SuitRate,sodd.QAQty)*price,2)),
       TotalUSD = sum(round(iif(o.StyleUnit = 'SETS',sodd.QAQty*SuitRate,sodd.QAQty)*price,2))/IIF(o.CurrencyID = 'CNY',6.1,1)
into #Sewing
from SewingOutput so
inner join SewingOutput_Detail_Detail sodd on sodd.ID = so.ID
inner join Orders o on o.ID = sodd.OrderId and o.LocalOrder=0 
left join CDCode cd on o.CdCodeID = cd.id
outer apply(select CpuRate from dbo.GetCPURate(o.OrderTypeID,o.ProgramID,o.Category,o.BrandID,'O')) as CPURate
outer apply(select dbo.GetSuitRate(cd.ComboPcs,sodd.ComboType) as SuitRate where cd.ID = o.CdCodeID) as GetSuitRate
outer apply(select dbo.GetPoPriceByArticleSize(sodd.OrderId,sodd.Article,sodd.SizeCode)as price) as price
where so.FactoryID <> 'TSR' " + WhereSewingStr +
@"
group by so.FactoryID,o.ID,o.CurrencyID,CpuRate

--OnBoard
IF Object_id('tempdb.dbo.#tmpFtyBooking1') IS NOT NULL 
  BEGIN 
      DROP TABLE #tmpftybooking1 
  END 

IF Object_id('tempdb.dbo.#tmpFtyBooking2') IS NOT NULL 
  BEGIN 
      DROP TABLE #tmpftybooking2 
  END 

IF Object_id('tempdb.dbo.#GarmentInvoice') IS NOT NULL 
  BEGIN 
      DROP TABLE #garmentinvoice 
  END 

--Create same as Trade Table: GarmentInvoice  
SELECT b.id, 
       b.etd 
INTO   #tmpftybooking1 
FROM   production.dbo.pullout_detail a, 
       production.dbo.gmtbooking b 
WHERE  a. invno = b.id 
ORDER  BY b.id 

SELECT a.packinglistid AS ID, 
       po1.pulloutdate AS ETD 
INTO   #tmpftybooking2 
FROM   (SELECT DISTINCT packinglistid 
        FROM   production.dbo.pullout_detail 
        WHERE  ( packinglisttype = 'F' 
                  OR packinglisttype = 'I' ) 
               AND packinglistid <> '' 
        EXCEPT 
        SELECT id AS PackingListID 
        FROM   #tmpftybooking1) a 
       LEFT JOIN production.dbo.packinglist p1 
              ON a. packinglistid = p1.id 
       LEFT JOIN production.dbo.pullout po1 
              ON p1.pulloutid = po1.id 

SELECT id, 
       etd 
INTO   #GarmentInvoice 
FROM   (SELECT DISTINCT id, 
                        etd 
        FROM   #tmpftybooking1 
        UNION ALL 
        SELECT id, 
               etd 
        FROM   #tmpftybooking2) a 

select o.FactoryID,o.ID,o.CurrencyID,
       Qty = sum(pdd.ShipQty),
       CpuRate,
       TotalCpu = sum(round(pdd.ShipQty*o.CPU*CpuRate,2)),
	   TotalFOB = sum(round(pdd.ShipQty*price.price,2)),
       TotalUSD = sum(round(pdd.ShipQty*price.price,2))/IIF(o.CurrencyID = 'CNY',6.1,1)
into #OnBoard 
from #GarmentInvoice gi
inner join Pullout_Detail pd on gi.ID=pd.INVNo
inner join Pullout p on pd.ID=p.ID
inner join Pullout_Detail_Detail pdd on pd.Ukey = pdd.Pullout_DetailUKey
inner join Orders o on pd.OrderID = o.ID and o.LocalOrder=0
outer apply(select CpuRate from dbo.GetCPURate(o.OrderTypeID,o.ProgramID,o.Category,o.BrandID,'O')) as CPURate
outer apply(select dbo.GetPoPriceByArticleSize(pdd.OrderId,pdd.Article,pdd.SizeCode)as price) as price
where o.FactoryID <> 'TSR' and p.Status<>'New' " + WhereOnBoardStr +
@"
group by o.FactoryID,o.ID,o.CurrencyID,CpuRate

--Pullout
select o.FactoryID,o.ID,o.CurrencyID,
       Qty = sum(pdd.ShipQty),
       CpuRate,
       TotalCpu = sum(round(pdd.ShipQty*o.CPU*CpuRate,2)),
	   TotalFOB = sum(round(pdd.ShipQty*price.price,2)),
       TotalUSD = sum(round(pdd.ShipQty*price.price,2))/IIF(o.CurrencyID = 'CNY',6.1,1)
into #Pullout
from Pullout p
inner join Pullout_Detail pd on p.ID=pd.ID
inner join Pullout_Detail_Detail pdd on pd.Ukey = pdd.Pullout_DetailUKey
inner join Orders o on pd.OrderID = o.ID and o.LocalOrder=0 
outer apply(select CpuRate from dbo.GetCPURate(o.OrderTypeID,o.ProgramID,o.Category,o.BrandID,'O')) as CPURate
outer apply(select dbo.GetPoPriceByArticleSize(pdd.OrderId,pdd.Article,pdd.SizeCode)as price) as price
where o.FactoryID <> 'TSR' and p.Status<>'New' " + WherePulloutStr +
@"
group by o.FactoryID,o.ID,o.CurrencyID,CpuRate";

            tsql_Trade +=
@"
;
with #temp as (
select (CASE WHEN ISNULL(s.FactoryID,'') <> '' THEN s.FactoryID 
			 WHEN ISNULL(ob.FactoryID,'') <> '' THEN ob.FactoryID 
			 WHEN ISNULL(p.FactoryID,'') <> '' THEN p.FactoryID 
			 WHEN ISNULL(st.FactoryID,'') <> '' THEN st.FactoryID 
			 ELSE '' END) as Factory,
	   (CASE WHEN ISNULL(s.CurrencyID, '') <> '' THEN s.CurrencyID 
			 WHEN ISNULL(ob.CurrencyID,'') <> '' THEN ob.CurrencyID 
			 WHEN ISNULL(p.CurrencyID,'') <> '' THEN p.CurrencyID 
			 WHEN ISNULL(st.CurrencyID,'') <> '' THEN st.CurrencyID 
			 ELSE '' END) as CurrencyID,
	   (CASE WHEN ISNULL(s.ID, '') <> '' THEN s.ID 
			 WHEN  ISNULL(ob.ID,'') <> '' THEN ob.ID 
			 WHEN  ISNULL(p.ID,'') <> '' THEN p.ID 
			 WHEN  ISNULL(st.ID,'') <> '' THEN st.ID 
			 ELSE '' END) as OrderID,
	   Sewing_Qty = isnull(s.Qty,0),Sewing_TotalCpu = isnull(s.TotalCpu,0),Sewing_TotalFOB = isnull(s.TotalFOB,0),Sewing_TotalUSD = isnull(s.TotalUSD,0),
	   Onboard_Qty = isnull(ob.Qty,0),Onboard_TotalCpu = isnull(ob.TotalCpu,0),Onboard_TotalFOB = isnull(ob.TotalFOB,0),Onboard_TotalUSD = isnull(ob.TotalUSD,0),
	   Pullout_Qty = isnull(p.Qty,0),Pullout_TotalCpu = isnull(p.TotalCpu,0),Pullout_TotalFOB = isnull(p.TotalFOB,0),Pullout_TotalUSD = isnull(p.TotalUSD,0),
	   Stock_Qty = isnull(st.Qty,0),Stock_TotalCpu = isnull(st.TotalCpu,0),Stock_TotalFOB = isnull(st.TotalFOB,0),Stock_TotalUSD = isnull(st.TotalUSD,0),
	   Start_Qty = isnull(st.Qty,0) + isnull(ob.Qty,0) - isnull(s.Qty,0),
	   Start_TotalCPU = isnull(st.TotalCpu,0) + isnull(ob.TotalCpu,0) - isnull(s.TotalCpu,0),
	   Start_TotalFOB = isnull(st.TotalFOB,0) + isnull(ob.TotalFOB,0) - isnull(s.TotalFOB,0),
	   Start_TotalUSD = isnull(st.TotalUSD,0) + isnull(ob.TotalUSD,0) - isnull(s.TotalUSD,0)
from #Sewing s  
full outer join #OnBoard ob on s.ID = ob.ID
full outer join #Pullout p on ob.ID = p.ID
full outer join #Stock st on s.ID = st.ID
)
select Factory,CurrencyID,OrderID,Sewing_Qty = sum(Sewing_Qty),Sewing_TotalCpu = sum(Sewing_TotalCpu),Sewing_TotalFOB = sum(Sewing_TotalFOB),Sewing_TotalUSD = sum(Sewing_TotalUSD),
	   Onboard_Qty = sum(Onboard_Qty),Onboard_TotalCpu = sum(Onboard_TotalCpu),Onboard_TotalFOB = sum(Onboard_TotalFOB),Onboard_TotalUSD = sum(Onboard_TotalUSD),
	   Pullout_Qty = sum(Pullout_Qty),Pullout_TotalCpu = sum(Pullout_TotalCpu),Pullout_TotalFOB = sum(Pullout_TotalFOB),Pullout_TotalUSD = sum(Pullout_TotalUSD),
	   Stock_Qty = sum(Stock_Qty),Stock_TotalCpu = sum(Stock_TotalCpu),Stock_TotalFOB = sum(Stock_TotalFOB),Stock_TotalUSD = sum(Stock_TotalUSD),Start_Qty = sum(Start_Qty),
	   Start_TotalCPU = sum(Start_TotalCPU),Start_TotalFOB = sum(Start_TotalFOB),Start_TotalUSD = sum(Start_TotalUSD)
into #Detail
from  #temp
group by Factory,CurrencyID,OrderID
";
            #endregion

            #region Query SQL
            tsql_Query =
@";
with #Query as (
select d.Factory,d.CurrencyID,d.OrderID,d.Sewing_Qty,d.Sewing_TotalCpu,d.Sewing_TotalFOB,d.Sewing_TotalUSD,d.Onboard_Qty,d.Onboard_TotalCpu,
       d.Onboard_TotalFOB,d.Onboard_TotalUSD,d.Stock_Qty,d.Stock_TotalCpu,d.Stock_TotalFOB,d.Stock_TotalUSD,d.Pullout_Qty,d.Pullout_TotalCpu,
       d.Pullout_TotalFOB,d.Pullout_TotalUSD,d.Start_Qty,d.Start_TotalCPU,d.Start_TotalFOB,d.Start_TotalUSD,o.BuyerDelivery,name,o.BrandID,
       o.Qty,o.StyleUnit,PoPrice = round(o.PoPrice,2),o.CPU,CpuRate,CPUAmount = o.Qty * o.CPU * CpuRate
from #Detail d
inner join Orders o on d.OrderID = o.ID and o.LocalOrder=0
outer apply(select name from DropDownList where ID = o.Category and Type = 'Category') as typename
outer apply(select CpuRate from dbo.GetCPURate(o.OrderTypeID,o.ProgramID,o.Category,o.BrandID,'O')) as CPURate
)";
            #endregion

            #region 依條件匯出所需資料SQL
            if (FOBorCPUifDetail == "CPUTotal")
            {
                tsql_Query +=
@"
select Factory,CPUAmount = sum(round(CPUAmount,2)),Start_Qty = sum(Start_Qty),Start_TotalCPU = sum(Start_TotalCPU),Sewing_Qty = sum(Sewing_Qty),
       Sewing_TotalCpu = sum(Sewing_TotalCpu),Onboard_Qty = sum(Onboard_Qty),Onboard_TotalCpu = sum(Onboard_TotalCpu),Stock_Qty = sum(Stock_Qty),
       Stock_TotalCpu = sum(Stock_TotalCpu),OutputOnboardQty = sum(Sewing_Qty - Onboard_Qty),OutputOnboardAmount = sum(Sewing_TotalCpu - Onboard_TotalCpu),
       Pullout_Qty = sum(Pullout_Qty),Pullout_TotalCpu = sum(Pullout_TotalCpu)
from #Query
group by Factory
";
            }
            else if (FOBorCPUifDetail == "FOBTotal")
            {
                tsql_Query +=
@"
select Factory,CurrencyID,Exchange = IIF(CurrencyID = 'CNY',6.1,1),CPUAmount = sum(round(CPUAmount,2)),Start_Qty = sum(Start_Qty),Start_TotalFOB = sum(Start_TotalFOB),
	   Start_TotalUSD = sum(Start_TotalUSD),Sewing_Qty = sum(Sewing_Qty),Sewing_TotalFOB = sum(Sewing_TotalFOB),Sewing_TotalUSD = sum(Sewing_TotalUSD),
       Onboard_Qty = sum(Onboard_Qty),Onboard_TotalFOB = sum(Onboard_TotalFOB),Onboard_TotalUSD = sum(Onboard_TotalUSD),Stock_Qty = sum(Stock_Qty),
	   Stock_TotalFOB = sum(Stock_TotalFOB),Stock_TotalUSD = sum(Stock_TotalUSD),OutputOnboardQty = sum(Sewing_Qty - Onboard_Qty),
       OutputOnboardAmount = sum(Sewing_TotalFOB - Onboard_TotalFOB),OutputOnboardAmountUSD = sum(Sewing_TotalUSD - Onboard_TotalUSD),
       Pullout_Qty = sum(Pullout_Qty),Pullout_TotalFOB = sum(Pullout_TotalFOB),Pullout_TotalUSD = sum(Pullout_TotalUSD)
from #Query
group by Factory,CurrencyID
";
            }
            else if (FOBorCPUifDetail == "CPUDetail")
            {
                tsql_Query +=
@"
select Factory,CurrencyID,OrderID,BuyerDelivery,OrderType = name,BrandID,OrderQty = Qty,StyleUnit,PoPrice,CPU,CpuRate,CPUAmount = round(CPUAmount,2),
       Start_Qty,Start_TotalCPU,Sewing_Qty,Sewing_TotalCpu,Onboard_Qty,Onboard_TotalCpu,Stock_Qty,Stock_TotalCpu,
       OutputOnboardQty = Sewing_Qty - Onboard_Qty,OutputOnboardAmount = Sewing_TotalCpu - Onboard_TotalCpu,Pullout_Qty,Pullout_TotalCpu
from #Query
";
            }
            else if (FOBorCPUifDetail == "FOBDetail")
            {
                tsql_Query +=
@"
select Factory,CurrencyID,OrderID,BuyerDelivery,OrderType = name,BrandID,OrderQty = Qty,StyleUnit,PoPrice,
	   Exchange = (case when CurrencyID = 'USD' then 1 when CurrencyID = 'CNY' then 6.1 else '' end ),
	   CPU,CpuRate,Amount = round(CPUAmount,2),
	   Start_Qty,Start_TotalFOB,Start_TotalUSD,Sewing_Qty,Sewing_TotalFOB,Sewing_TotalUSD,Onboard_Qty,Onboard_TotalFOB,Onboard_TotalUSD,
       Stock_Qty,Stock_TotalFOB,Stock_TotalUSD,OutputOnboardQty = Sewing_Qty - Onboard_Qty,OutputOnboardAmount = Sewing_TotalFOB - Onboard_TotalFOB,
       OutputOnboardAmountUSD = Sewing_TotalUSD - Onboard_TotalUSD,Pullout_Qty,Pullout_TotalFOB,Pullout_TotalUSD
from #Query
";
            }
            if (FOBorCPUifDetail == "CPUDetail" || FOBorCPUifDetail == "FOBDetail")
            {
                tsql_Query += @" order by Factory,OrderID";
            }
            else
            {
                tsql_Query += @" order by Factory";
            }
            #endregion

            return base.ValidateInput();
        }

        protected override Ict.DualResult OnAsyncDataLoad(Win.ReportEventArgs e)
        {            
            DT1 = null;
            DualResult result;
            DataTable dtGMTBooking;
            DataTable dtOrders;
            DataTable dtFactory;
            DataTable dtStock = new DataTable();
            DataTable dtAllStock = new DataTable();
            DataTable dtResult = new DataTable();

            #region --由Factory.PmsPath抓各個連線路徑
            this.SetLoadingText("Load connections... ");
            XDocument docx = XDocument.Load(Application.ExecutablePath + ".config");
            string[] strSevers = ConfigurationManager.AppSettings["ServerMatchFactory"].Split(new char[] { ';' });
            List<string> connectionString = new List<string>();// ←主要是要重組 List connectionString 
            foreach (string ss in strSevers)
            {
                //判斷工廠的欄位選項是否有值,有值代表只需要撈單獨一個System
                if (!MyUtility.Check.Empty(txtCentralizedFactory.Text))
                {
                    //只取:後的FactoryID 
                    string[] m = ss.Split(new char[] { ':' });
                    if (m.Count() > 1)
                    {
                        //判斷是否有同畫面上的工廠名稱
                        string[] mFactory = m[1].Split(new char[] { ',' });
                        //如果不同,就換下一個System,直到相同為止才跳出去
                        if (!mFactory.AsEnumerable().Any(f => f.EqualString(txtCentralizedFactory.Text.ToString())))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        continue;
                    }
                }
                var Connections = docx.Descendants("modules").Elements().Where(y => y.FirstAttribute.Value.Contains(ss.Split(new char[] { ':' })[0].ToString())).Descendants("connectionStrings").Elements().Where(x => x.FirstAttribute.Value.Contains("Production")).Select(z => z.LastAttribute.Value).ToList()[0].ToString();
                connectionString.Add(Connections);
            }
            if (null == connectionString || connectionString.Count == 0)
            {
                return new DualResult(false, "no connection loaded.");
            }
            #endregion

            #region 跨資料庫連線,將所需Table存入TempTable,供不同資料庫使用
            for (int i = 0; i < connectionString.Count; i++)
            {
                string conString = connectionString[i] + ";Connection Timeout=600";
                Logs.UI.LogInfo("");
                this.SetLoadingText(string.Format("Load data connection {0}/{1}", (i + 1), connectionString.Count));

                SqlConnection conn;                
                using (conn = new SqlConnection(conString))
                {
                    conn.Open();
                    result = DBProxy.Current.SelectByConn(conn,

@"--Create same as Trade Table: GarmentInvoice 
IF OBJECT_ID('tempdb.dbo.#tmpFtyBooking1') IS NOT NULL 
BEGIN
    DROP TABLE #tmpFtyBooking1
END
IF OBJECT_ID('tempdb.dbo.#tmpFtyBooking2') IS NOT NULL 
BEGIN
    DROP TABLE #tmpFtyBooking2
END
IF OBJECT_ID('tempdb.dbo.#GarmentInvoice') IS NOT NULL 
BEGIN
    DROP TABLE #GarmentInvoice
END

SELECT b.ID,b.ETD
INTO  #tmpFtyBooking1
FROM Production.dbo.Pullout_Detail  a, Production.dbo.GMTBooking b 
WHERE a. INVNo = b.id
ORDER BY b.id 

SELECT a.packinglistid                          AS ID,      
       po1.pulloutdate                          AS ETD      
into #tmpFtyBooking2
FROM   (SELECT DISTINCT packinglistid 
        FROM   production.dbo.pullout_detail 
        WHERE  ( packinglisttype = 'F' 
                  OR packinglisttype = 'I' ) 
               AND packinglistid <> '' 
        EXCEPT 
        SELECT id AS PackingListID 
        FROM   #tmpftybooking1) a 
       LEFT JOIN production.dbo.packinglist p1 
              ON a. packinglistid = p1.id 
       LEFT JOIN production.dbo.pullout po1 
              ON p1.pulloutid = po1.id 

select ID,ETD
into #GarmentInvoice 
from (
select distinct ID,  ETD 
from #tmpFtyBooking1
union all
select ID, ETD 
from #tmpFtyBooking2
) a 

select DISTINCT ID,ETD from #GarmentInvoice", out dtGMTBooking);
                    if (!result) { return result; }
                    result = DBProxy.Current.SelectByConn(conn, @" select FactoryID,ID,CurrencyID,CPU,OrderTypeID,ProgramID,Category,BrandID,GMTComplete from Orders where LocalOrder=0", out dtOrders);
                    if (!result) { return result; }
                    result = DBProxy.Current.SelectByConn(conn, @" select ID,CountryID from Factory  ", out dtFactory);
                    if (!result) { return result; }


                    DataTable dtProduction;
                    result = MyUtility.Tool.ProcessWithDatatable(dtFactory, "ID,CountryID", "select 1", out dtProduction, "#tmpFactory", conn);
                    if (!result) { return result; }
                    result = MyUtility.Tool.ProcessWithDatatable(dtOrders, "FactoryID,ID,CurrencyID,CPU,OrderTypeID,ProgramID,Category,BrandID,GMTComplete", "select 1", out dtProduction, "#tmpOrders", conn);
                    if (!result) { return result; }
                    result = MyUtility.Tool.ProcessWithDatatable(dtGMTBooking, "ID,ETD", tsql_Production, out dtProduction, "#tmpGMTBooking", conn);
                    if (!result) { return result; }
                    if (dtProduction == null || dtProduction.Rows.Count == 0 || dtProduction.Rows[0]["ID"] == DBNull.Value)
                    {
                        dtProduction.Rows.Add();
                    }
                    result = MyUtility.Tool.ProcessWithDatatable(dtProduction, "FactoryID,ID,CurrencyID,Qty,CpuRate,OrderID,Article,SizeCode,TotalCpu", tsql_Stock, out dtStock, "#Production",conn);
                    if (!result) { return result; }

                    if (dtStock == null || dtStock.Rows.Count == 0 || dtStock.Rows[0]["ID"] == DBNull.Value)
                    {
                        dtStock.Rows.Add();
                    }
                    if (dtStock == null || dtStock.Rows.Count == 0)
                    {
                        dtAllStock = dtStock;
                    }
                    else
                    {
                        dtAllStock.Merge(dtStock);
                    }
                    #region 合併各資料庫資料
                    result = MyUtility.Tool.ProcessWithDatatable(dtAllStock, "FactoryID,ID,CurrencyID,Qty,CpuRate,TotalCpu,TotalFOB,TotalUSD", tsql_Trade + tsql_Query, out dtResult, "#Stock", conn);
                    if (!result) { return result; }
                    if (DT1 == null || DT1.Rows.Count == 0)
                    {
                        DT1 = dtResult;
                    }
                    else
                    {
                        DT1.Merge(dtResult);
                    }
                    #endregion
                }
            }
            #endregion



            #region 計算Tatol金額
            if (DT1 != null && DT1.Rows.Count > 0)
            {
                DataRow totalrow = DT1.NewRow();
                int startIndex = 0;
                bool sumtotal = true;
                totalrow[0] = "Total : ";
                if (FOBorCPUifDetail == "CPUDetail" || FOBorCPUifDetail == "FOBDetail" || FOBorCPUifDetail == "FOBTotal")
                {
                    totalrow[1] = "USD";
                    if (FOBorCPUifDetail == "FOBTotal")
                    {
                        totalrow[2] = 1;
                    }
                }

                //for dt每個欄位
                decimal TTColumnAMT = 0;
                for (int colIdx = startIndex; colIdx < DT1.Columns.Count; colIdx++)
                {
                    TTColumnAMT = 0;
                    if (DT1.Columns[colIdx].ToString() == "PoPrice" || DT1.Columns[colIdx].ToString() == "CPU" || 
                        DT1.Columns[colIdx].ToString() == "CpuRate" || DT1.Columns[colIdx].ToString() == "Exchange") continue;
                    //for dt每一列
                    for (int rowIdx = 0; rowIdx < DT1.Rows.Count; rowIdx++)
                    {
                        if (DT1.Rows[rowIdx][colIdx].GetType().Name.ToString() == "Decimal" || DT1.Rows[rowIdx][colIdx].GetType().Name.ToString() == "Int32")
                        {
                            TTColumnAMT += MyUtility.Convert.GetDecimal(DT1.Rows[rowIdx][colIdx]);
                            sumtotal = true;
                        }
                        else 
                        {
                            sumtotal = false;
                            break;
                        }
                    }
                    if (!sumtotal) continue;
                    totalrow[colIdx] = TTColumnAMT;
                }
                DT1.Rows.Add(totalrow);
            }
            #endregion
            return Result.True;
        }

        protected override bool OnToExcel(Win.ReportDefinition report)
        {
            string CallsExcel = "";
            string Title2ForExcel = "";
            string Parameter = "";

            if (DT1 == null || DT1.Rows.Count == 0)
            {
                MyUtility.Msg.ErrorBox("Data not found!");
                return false;
            }

            if (FOBorCPUifDetail == "CPUTotal")
            {
                CallsExcel = "Centralized_R20_製成品進銷存明細表_CPUTotal.xltx";
                Title2ForExcel = "製成品進銷存明細表 -- CPU";
            }
            else if (FOBorCPUifDetail == "FOBTotal")
            {
                CallsExcel = "Centralized_R20_製成品進銷存明細表_FOBTotal.xltx";
                Title2ForExcel = "製成品進銷存明細表 -- FOB";
            }
            else if (FOBorCPUifDetail == "CPUDetail")
            {
                CallsExcel = "Centralized_R20_製成品進銷存明細表_CPUDetail.xltx";
                Title2ForExcel = "製成品進銷存明細表 -- CPU";
            }
            else if (FOBorCPUifDetail == "FOBDetail")
            {
                CallsExcel = "Centralized_R20_製成品進銷存明細表_FOBDetail.xltx";
                Title2ForExcel = "製成品進銷存明細表 -- FOB";
            }

            Sci.Utility.Excel.SaveXltReportCls xl = new Utility.Excel.SaveXltReportCls(CallsExcel);
            SaveXltReportCls.XltRptTable Data1 = new SaveXltReportCls.XltRptTable(DT1);
            xl.BoOpenFile = true;
            Data1.ShowHeader = false;

            xl.DicDatas.Add("##Title", "SINTEX INTERNATIONAL LTD.");
            xl.DicDatas.Add("##Title2", Title2ForExcel);

            Parameter = "查詢條件-日期區間 : " + ((DateTime)this.dateQueryDateStart.Value).ToShortDateString() + " ~ " + ((DateTime)this.dateQueryDateEnd.Value).ToShortDateString();
            Parameter += " , Categoty : " + this.comboDropdownlist.Text;
            if (!this.txtCentralizedFactory.Text.Empty()) Parameter += " , Factory : " + this.txtCentralizedFactory.Text;
            if (!this.txtbrand.Text.Empty()) Parameter += " , Brand : " + this.txtbrand.Text;
            if (!this.txtcountry.TextBox1.Text.Empty()) Parameter += " , Region : " + this.txtcountry.DisplayBox1.Text;

            xl.DicDatas.Add("##Parameter", Parameter);
            xl.DicDatas.Add("##Data", Data1);

            xl.Save(Sci.Production.Class.MicrosoftFile.GetName(Title2ForExcel), false);
            //因再次匯出資料時，會殘留上次的連線數，故先清空
            this.SetLoadingText("");
            return true;
        }
    }
}
