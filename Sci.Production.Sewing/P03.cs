using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sci.Data;
using Ict;
using System.Linq;
using System.Transactions;

namespace Sci.Production.Sewing
{
    public partial class P03 : Sci.Win.Tems.QueryForm
    {
        public P03(ToolStripMenuItem menuitem)
            :base(menuitem)
        {
            InitializeComponent();
            #region Set Default Data
            this.dateRangeBuyerDelivery.Value1 = DateTime.Today;
            this.dateRangeBuyerDelivery.Value2 = DateTime.Today.AddMonths(1);
            #endregion 
            this.grid.IsEditingReadOnly = false;
        }

        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region Grid Setting
            this.Helper.Controls.Grid.Generator(this.grid)
                .CheckBox("sel", header: "", trueValue: 1, falseValue: 0, iseditable: true)
                .Text("ID", header: "SP", iseditingreadonly: true)
                .Text("StyleLocation", header: "*", iseditingreadonly: true)
                .Text("OrderIDFrom", header: "From SP", iseditingreadonly: true)
                .Text("Article", header: "Color Way", iseditingreadonly: true)
                .Text("SizeCode", header: "Size", iseditingreadonly: true)
                .Numeric("AllocateQty", header: "Allocate Qty", iseditingreadonly: true)
                .Numeric("ToSPQty", header: "To SP" + Environment.NewLine + "Qty", iseditingreadonly: true)
                .Numeric("ToSPAllocatedQty", header: "To SP" + Environment.NewLine + "Allocated Qty", iseditingreadonly: true)
                .Numeric("ToSPBalance", header: "To SP" + Environment.NewLine + "Balance", iseditingreadonly: true)
                .Date("ToSPBuyerDeliver", header: "To SP" + Environment.NewLine + "Buyer Delivery", iseditingreadonly: true)
                .Numeric("FromSPSewingOutputQty", header: "From SP " + Environment.NewLine + "Sewing output Qty", iseditingreadonly: true)
                .Numeric("FromSPPackingQty", header: "From SP" + Environment.NewLine + "Packing Qty", iseditingreadonly: true)
                .Numeric("FromSPAvailableQty", header: "From SP" + Environment.NewLine + "Available Qty", iseditingreadonly: true);

            for (int i = 0; i < this.grid.Columns.Count; i++)
            {
                this.grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                this.grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            #endregion 
            this.findNow();
        }

        private void buttonNewSearch_Click(object sender, EventArgs e)
        {
            this.textBoxToSpNum.Text = "";
            this.textBoxFromSpNum.Text = "";
            this.dateRangeBuyerDelivery.Value1 = null;
            this.dateRangeBuyerDelivery.Value2 = null;
        }

        private void buttonFindNow_Click(object sender, EventArgs e)
        {
            this.findNow();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            DataTable dtSelectData = (DataTable)((BindingSource)(this.grid.DataSource)).DataSource;
            #region Select Data
             if (dtSelectData.AsEnumerable().Any(row => row["Sel"].EqualDecimal(1)))
            {
                dtSelectData = dtSelectData.AsEnumerable().Where(row => row["Sel"].EqualDecimal(1)).CopyToDataTable();
            }
            else
            {
                MyUtility.Msg.InfoBox("Please select data first.");
                return;
            }
            #endregion
            
            //每一筆單獨計算
            foreach (DataRow dr in dtSelectData.Rows)
            {
                DualResult boolResult = setSewingOutput(dr["ID"], dr["OrderIDFrom"], dr["StyleLocation"], dr["Article"], dr["SizeCode"], dr["ToSPBalance"]);

                if (!boolResult)
                {
                    MyUtility.Msg.WarningBox(string.Format("ID {0}, FromSP {1},  ComboType {2}, Article {3}, SizeCode {4}", dr["ID"], dr["OrderIDFrom"], dr["StyleLocation"], dr["Article"], dr["SizeCode"])
                                             + Environment.NewLine
                                             + boolResult.Description);
                }
            }

            MyUtility.Msg.InfoBox("Complete");
            this.findNow();
        }

        /// <summary>
        /// Get SewingOutput Data
        /// </summary>
        /// <param name="ToOrderID">ID</param>
        /// <param name="FromOrderID">From Poid</param>
        /// <param name="ComboType">StyleLocation</param>
        /// <param name="Article">Color Way</param>
        /// <param name="SizeCode">Size</param>
        /// <param name="NeedQty">ToSPQty</param>
        private DualResult setSewingOutput(object ToOrderID, object FromOrderID, object ComboType, object Article, object SizeCode, object NeedQty)
        {
            DualResult boolResult;

            List<SqlParameter> listSqlPara = new List<SqlParameter>();
            listSqlPara.Add(new SqlParameter("@ToOrderID", ToOrderID));
            listSqlPara.Add(new SqlParameter("@FromOrderID", FromOrderID));
            listSqlPara.Add(new SqlParameter("@ComboType", ComboType));
            listSqlPara.Add(new SqlParameter("@Article", Article));
            listSqlPara.Add(new SqlParameter("@SizeCode", SizeCode));
            listSqlPara.Add(new SqlParameter("@NeedQty", NeedQty));

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                #region Sewing P03 拆單
                string strSqlCmd = @"
select	OutputData.*
		, SodStatus = SodStatus.value
		, SodNewQaQty = SodNewQaQty.value
		, SoddStatus = SoddStatus.value
		, SoddNewQaQty = SoddNewQaQty.value
		, TakeQty = TakeQty.value
        , InlineQty = InlineQty.value
        , DefectQty = DefectQty.value
into #tmp
from (
	select	soID = so.ID
			, soSewingLineID = so.SewingLineID
            , sodInlineQty = sod.InlineQty
            , sodDefectQty = sod.DefectQty
			, sodArticle = sod.Article
			, sodColor = sod.Color
			, FromOrderID = sodd.OrderId
			, ToOrderID = @ToOrderID
			, Article = sodd.Article
			, ComboType = sodd.ComboType
			, SizeCode = sodd.SizeCode
			, sodQAQty = sod.QAQty
			, soddQaQty = sodd.QaQty
			, soddQaQtyRunningTotal = sum (sodd.QaQty) over (order by OutPutDate)
			, sodUkey = sod.UKey
	from SewingOutput so
	inner join SewingOutput_Detail sod on so.id = sod.id
	inner join SewingOutput_Detail_Detail sodd on sod.UKey = sodd.SewingOutput_DetailUKey
	where	sodd.OrderId = @FromOrderID
			and sodd.ComboType = @ComboType
			and sodd.Article = @Article
			and sodd.SizeCode = @SizeCode
) OutputData
-- SoddStatus -- 
-- 判斷 SewingOutput_Detail_Detail => D = 刪除
--                                    U = 修改
--                                    N = 不會使用到
outer apply (
	select	value = case 	
						when (OutputData.soddQaQtyRunningTotal <= @NeedQty) then 'D'
						when (OutputData.soddQaQtyRunningTotal > @NeedQty and OutputData.soddQaQtyRunningTotal - OutputData.soddQaQty < @NeedQty) then 'U'
						else 'N'
					end
) SoddStatus
-- SoddNewQaQty --
-- 計算 SewingOutput_Detail_Detail 拆單後剩餘數量 --
outer apply (
	select value = case SoddStatus.value
						when 'U' then soddQaQtyRunningTotal - @NeedQty
						when 'D' then 0
				   end
) SoddNewQaQty
-- SodStatus -- 
-- 判斷 SewingOutput_Detail => D = 刪除
--                             U = 修改
--                             N = 不會使用到
outer apply (
	select	value = case 	
						when (SoddStatus.value = 'D' and OutputData.sodQAQty = OutputData.soddQAQty) then 'D'
						when (SoddStatus.value = 'D' and OutputData.sodQAQty != OutputData.soddQAQty) then 'U'
						when (SoddStatus.value = 'U') then 'U'
						else 'N'
					end
) SodStatus
-- SoddNewQaQty --
-- 計算 SewingOutput_Detail 拆單後剩餘數量 --
outer apply (
	select value = case SodStatus.value
						when 'U' then OutputData.sodQAQty - (OutputData.soddQaQty - SoddNewQaQty.value)
				   end
) SodNewQaQty
-- TakeQty --
-- 計算這一張單拿多少 Qty --
outer apply (
	select value = case SodStatus.value
						when 'U' then OutputData.soddQaQty - SoddNewQaQty.value
						else OutputData.soddQaQty
				   end
) TakeQty
-- InlineQty --
-- 若母單全拆，InlineQty Qty 拆到子單 --
outer apply (
    select value = case SodStatus.value
                        when 'D' then OutputData.sodInlineQty
                        else 0
                   end
) InlineQty
-- DefectQty --
-- 若母單全拆，Defect Qty 拆到子單 --
outer apply (
    select value = case SodStatus.value
                        when 'D' then OutputData.sodDefectQty
                        else 0
                   end
) DefectQty
where SoddStatus.value != 'N'

-- 母單 Delete & Update SewingOutput_Detail --
Delete sod
from SewingOutput_Detail sod
inner join #tmp on sod.UKey = #tmp.sodUkey
where	#tmp.SodStatus = 'D'

update sod
	set	sod.QAQty = #tmp.SodNewQaQty
from SewingOutput_Detail sod
inner join #tmp on sod.UKey = #tmp.sodUkey
where	#tmp.SodStatus = 'U'

-- 母單 Delete & Update SewingOutput_Detail_Detail --
Delete sodd
from SewingOutput_Detail_Detail sodd
inner join #tmp on sodd.SewingOutput_DetailUKey = #tmp.sodUkey
				   and sodd.ID = #tmp.soID
				   and sodd.OrderId = #tmp.FromOrderID
				   and sodd.ComboType = #tmp.ComboType
				   and sodd.Article = #tmp.Article
				   and sodd.SizeCode = #tmp.SizeCode
where	#tmp.SoddStatus = 'D'

update sodd
	set	sodd.QAQty = #tmp.SoddNewQaQty
from SewingOutput_Detail_Detail sodd
inner join #tmp on sodd.SewingOutput_DetailUKey = #tmp.sodUkey
				   and sodd.ID = #tmp.soID
				   and sodd.OrderId = #tmp.FromOrderID
				   and sodd.ComboType = #tmp.ComboType
				   and sodd.Article = #tmp.Article
				   and sodd.SizeCode = #tmp.SizeCode
where	#tmp.SoddStatus = 'U'

-- 子單 Merge SewingOutput_Detail --
Merge SewingOutput_Detail as t
using (
	select	#tmp.*
			, TMS = o.CPU 
					* o.CPUFactor 
					* (sl.Rate / 100) 
					* (select StdTMS 
					   from System WITH (NOLOCK))
			, HourlyStandardOutput = ss.StandardOutput
	from #tmp 
	left join Orders o on #tmp.ToOrderID = o.ID
	left join Style_Location sl on o.StyleUkey = sl.StyleUkey
								   and #tmp.ComboType = sl.Location
	left join SewingSchedule ss on #tmp.ToOrderID = ss.OrderID
								   and #tmp.ComboType = ss.ComboType
								   and #tmp.soSewingLineID = ss.SewingLineID
) as s on t.ID = s.soID
		  and t.OrderID = s.ToOrderID
		  and t.ComboType = s.ComboType
		  and t.Article = s.sodArticle
		  and t.Color = s.sodColor
when matched then 
	update set	t.QaQty = t.QaQty + s.TakeQty
				, t.InlineQty = t.InlineQty + s.InlineQty
                , t.DefectQty = t.DefectQty + s.DefectQty
				, t.TMS = s.TMS
				, t.HourlyStandardOutput = s.HourlyStandardOutput
when not matched by target then
	insert (
		ID			    , OrderID				, ComboType			, Article		, Color
		, TMS		    , HourlyStandardOutput	, WorkHour			, QaQty			, DefectQty
		, InlineQty     , AutoCreate
	) values (
		s.soID		    , s.ToOrderID			, s.ComboType		, s.sodArticle	, s.sodColor
		, TMS		    , s.HourlyStandardOutput, 0					, s.TakeQty		, s.DefectQty
		, s.InlineQty   , 1
	);

-- 子單 Merge SewingOutput_Detail_Detail --
Merge SewingOutput_Detail_Detail as t
using (
	select	#tmp.*
			, sod.Ukey
	from #tmp
	inner join SewingOutput_Detail sod on sod.ID = #tmp.soID
										  and sod.OrderID = #tmp.ToOrderID
										  and sod.ComboType = #tmp.ComboType
										  and sod.Article = #tmp.sodArticle
										  and sod.Color = #tmp.sodColor
) as s on t.SewingOutput_DetailUkey = s.Ukey
		  and t.ID = s.soID
		  and t.OrderID = s.ToOrderID
		  and t.ComboType = s.ComboType
		  and t.Article = s.Article
		  and t.SizeCode = s.SizeCode
when matched then
	update set	t.QaQty = t.QaQty + s.TakeQty
when not matched by target then 
	insert (
		ID				, SewingOutput_DetailUkey	, OrderID		, ComboType		, Article
		, SizeCode		, QaQty
	) values (
		s.soID			, s.Ukey					, s.ToOrderID	, s.ComboType	, s.Article
		, s.SizeCode	, s.TakeQty
	);

-- Update WorkHour --
-- 1. 累計超過設定值
---- a. 推算上一筆也超過設定值 WorkHour = 0
---- b. 推算上一筆沒有超過 WorkHour = 設下的 WorkHour
-- 2. 累計沒超過 WorkHour = newWorkHour
update sod
	set sod.WorkHour = case 
							when setWorkHour.rowNum = setWorkHour.rowCounts then
								iif (setWorkHour.AccuWorkHour - setWorkHour.newWorkHour > setWorkHour.soWorkHour, 0
																												, setWorkHour.soWorkHour - (setWorkHour.AccuWorkHour - setWorkHour.newWorkHour))
							else
								iif (setWorkHour.AccuWorkHour > setWorkHour.soWorkHour, iif (setWorkHour.AccuWorkHour - setWorkHour.newWorkHour >  setWorkHour.soWorkHour, 0
										 																																 , setWorkHour.soWorkHour - (setWorkHour.AccuWorkHour - setWorkHour.newWorkHour))
																					  , setWorkHour.newWorkHour)
					   end
from SewingOutput_Detail sod
inner join (
	select	sod.Ukey
			, soWorkHour = so.WorkHour
			, newWorkHour = ComputeWorkHour.value
			, AccuWorkHour = sum(ComputeWorkHour.value) over (partition by so.ID order by sod.ukey)
            , rowNum = row_number() over (partition by so.ID order by sod.ukey)
			, rowCounts = count(1) over (partition by so.ID)
	from #tmp
	inner join SewingOutput so on so.ID = #tmp.soID
	inner join SewingOutput_Detail sod on so.ID = sod.ID
	outer apply (
		select value = (select	isnull(sum(QaQty * TMS), 0)
						from SewingOutput_Detail
						where ID = sod.ID)
	) TotalQaQty
	outer apply (
		select value = Round(1.0 * sod.QaQty * sod.TMS / TotalQaQty.value * so.WorkHour, 3)
	) ComputeWorkHour
) setWorkHour on sod.UKey = setWorkHour.UKey

--select * from #tmp
drop table #tmp";
                #endregion 
                boolResult = DBProxy.Current.Execute(null, strSqlCmd, listSqlPara);
                if (!boolResult)
                {
                    transactionscope.Dispose();
                    return boolResult;
                }

                #region Check 母單 PackingList = Confirmed，必須保證拆單後，母單剩餘數量大於 Packing 數量
                strSqlCmd = @"
-- Check 母單 PackingList = Confirmed，必須保證拆單後，母單剩餘數量大於 Packing 數量 --
-- checkValue => Packing > Sewing = 0 數量不足
--				 Packing <= Sewing = 1 數量足夠
declare @PackingTtlQty int = 0;
declare @SewingTtlQty int = 0;

select	@PackingTtlQty = isnull (sum (pld.ShipQty), 0)
from PackingList pl
inner join PackingList_Detail pld on pl.ID = pld.ID							  
where	pl.OrderID = @FromOrderID
		and pl.Status = 'Confirmed'		

select	@SewingTtlQty = isnull (sum (sodd.QAQty), 0)
from PackingList pl
inner join PackingList_Detail pld on pl.ID = pld.ID
inner join SewingOutput_Detail sod on pl.OrderID = sod.OrderId
inner join SewingOutput_Detail_Detail sodd on sod.UKey = sodd.SewingOutput_DetailUKey
											  and pld.Article = sodd.Article
											  and pld.SizeCode = sodd.SizeCode											  
where	pl.OrderID = @FromOrderID
		and pl.Status = 'Confirmed'

select checkValue = iif (@SewingTtlQty >= @PackingTtlQty, 1
													    , 0)";
                #endregion
                if (MyUtility.GetValue.Lookup(strSqlCmd, listSqlPara).Equals("0"))
                {
                    transactionscope.Dispose();
                    MyUtility.Msg.WarningBox(string.Format("From SP {0} Sewing Output Qty can't less then PackingList Qty.", FromOrderID));
                    return new DualResult(true);
                }

                #region Check 拆單後的數量，總數不得超過子單 Order_Qty
                strSqlCmd = @"
-- Check 拆單後的數量，總數不得超過子單 Order_Qty --
select	*
from Order_Qty oq
cross apply (
	select value = MIN(value)
	from (
		select value = sum(sodd.QaQty)
		from SewingOutput_Detail_Detail sodd 
		where oq.Article = sodd.Article
			  and oq.SizeCode = sodd.SizeCode
			  and oq.ID = sodd.OrderId		  
		group by ComboType
	)x
) SewingQty
where	oq.id = @ToOrderID
		and oq.Qty < SewingQty.value";
                #endregion                
                if (MyUtility.Check.Seek(strSqlCmd, listSqlPara))
                {
                    transactionscope.Dispose();
                    MyUtility.Msg.WarningBox(string.Format("SP {0} Sewing Output Qty can't more then Order_Qty.", ToOrderID));
                    return new DualResult(true);
                }

                transactionscope.Complete();
                transactionscope.Dispose();
            }
            return boolResult;
        }

        private void findNow()
        {
            #region SQL Parameter
            List<SqlParameter> listSqlParameter = new List<SqlParameter>();
            listSqlParameter.Add(new SqlParameter("@ToSP", this.textBoxToSpNum.Text));
            listSqlParameter.Add(new SqlParameter("@FromSP", this.textBoxFromSpNum.Text));
            listSqlParameter.Add(new SqlParameter("@StartDate", (this.dateRangeBuyerDelivery.Value1.Empty()) ? "" : ((DateTime)this.dateRangeBuyerDelivery.Value1).ToString("yyyy/MM/dd")));
            listSqlParameter.Add(new SqlParameter("@EndDate", (this.dateRangeBuyerDelivery.Value2.Empty()) ? "" : ((DateTime)this.dateRangeBuyerDelivery.Value2).ToString("yyyy/MM/dd")));
            listSqlParameter.Add(new SqlParameter("@Factory", Sci.Env.User.Factory));
            #endregion 
            #region SQL Filte
            #region BuyerDelivery Filte
            string strBuyerDeliveryFilte = "";
            if (!this.dateRangeBuyerDelivery.Value1.Empty() && !this.dateRangeBuyerDelivery.Value2.Empty())
            {
                strBuyerDeliveryFilte = "and ToSPOrders.BuyerDelivery between @StartDate and @EndDate";
            }
            else if (!this.dateRangeBuyerDelivery.Value1.Empty() && this.dateRangeBuyerDelivery.Value2.Empty())
            {
                strBuyerDeliveryFilte = "and @StartDate <= ToSPOrders.BuyerDelivery";
            }
            else if (this.dateRangeBuyerDelivery.Value1.Empty() && !this.dateRangeBuyerDelivery.Value2.Empty())
            {
                strBuyerDeliveryFilte = "and ToSPOrders.BuyerDelivery <= @EndDate";
            }
            #endregion 
            Dictionary<string, string> dicSqlFilte = new Dictionary<string, string>();
            dicSqlFilte.Add("ToSP", (this.textBoxToSpNum.Text.Empty()) ? "" : "and OQG.ID = @ToSP");
            dicSqlFilte.Add("FromSP", (this.textBoxFromSpNum.Text.Empty()) ? "" : "and OQG.OrderIDFrom = @FromSP");
            dicSqlFilte.Add("BuyerDelivery", strBuyerDeliveryFilte);
            #endregion 
            #region SQL Command
            string strSqlCmd = string.Format(@"
select	sel = 1
		, ID = OQG.ID
		, StyleLocation = SL.Location
		, OrderIDFrom = OQG.OrderIDFrom
		, Article = OQG.Article
		, SizeCode = OQG.SizeCode
		, AllocateQty = case 
							when ToSPBalance.value < FromSPAvailableQty.value then ToSPBalance.value
							else FromSPAvailableQty.value
						end
		, ToSPQty = ToSPQty.value
		, ToSPAllocatedQty = ToSPAllocatedQty.value
		, ToSPBalance = ToSPBalance.value
		, ToSPBuyerDeliver = ToSPOrders.BuyerDelivery
		, FromSPSewingOutputQty = FromSPSewingOutputQty.value
		, FromSPPackingQty = FromSPPackingQty.value
		, FromSPAvailableQty = FromSPAvailableQty.value
from Order_Qty_Garment OQG
inner join Orders ToSPOrders on OQG.ID = ToSPOrders.ID
inner join Style_Location SL on ToSPOrders.StyleUkey = SL.StyleUkey
outer apply (
	select value = isnull (OQG.Qty, 0)
) ToSPQty
outer apply (
	select value = isnull (sum (SODD.QAQty), 0)
	from SewingOutput_Detail_Detail SODD
	where	SODD.OrderId = OQG.ID
			and SODD.ComboType = SL.Location
			and SODD.Article = OQG.Article
			and SODD.SizeCode = OQG.SizeCode
) ToSPAllocatedQty
outer apply (
	select value = ToSPQty.value - ToSPAllocatedQty.value
) ToSPBalance
outer apply (
	select value = isnull (sum (SODD.QAQty), 0)
	from SewingOutput_Detail_Detail SODD
	where	SODD.OrderId = OQG.OrderIDFrom
			and SODD.ComboType = SL.Location
			and SODD.Article = OQG.Article
			and SODD.SizeCode = OQG.SizeCode
) FromSPSewingOutputQty
outer apply (
	select value = isnull (sum (PLD.ShipQty), 0)
	from PackingList_Detail PLD
	inner join PackingList PL on PLD.ID = PL.ID
	where	PL.Status = 'Confirmed'
			and PLD.OrderID = OQG.OrderIDFrom
			and PLD.Article = OQG.Article
			and PLD.SizeCode = OQG.SizeCode
) FromSPPackingQty
outer apply (
	select value = FromSPSewingOutputQty.value - FromSPPackingQty.value
) FromSPAvailableQty
where	ToSPBalance.value > 0
        and ToSPOrders.FtyGroup = @Factory
		-- ToSP
		{0}
		-- FromSP
		{1}
		-- BuyerDelivery
		{2}
order by OQG.OrderIDFrom, ToSPOrders.BuyerDelivery", dicSqlFilte["ToSP"]
                                                        , dicSqlFilte["FromSP"]
                                                        , dicSqlFilte["BuyerDelivery"]);
            #endregion
            this.ShowWaitMessage("Data Loading...");
            #region Set Grid Data
            DataTable dtGridData;
            DualResult result = DBProxy.Current.Select(null, strSqlCmd, listSqlParameter, out dtGridData);
            if (result)
            {
                this.bindingSource.DataSource = dtGridData;
            }
            else
            {
                MyUtility.Msg.WarningBox(result.Description);
                this.bindingSource.DataSource = null;
            }
            #endregion
            this.HideWaitMessage();
        }
    }
}
