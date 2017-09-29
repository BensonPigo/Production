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
                .Numeric("FromSPAccrQty", header: "From SP" + Environment.NewLine + "Accu. Split Qty", iseditingreadonly: true)
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

            #region CheckPoidAvailableQty
            bool result = this.checkPoidAvailableQty(dtSelectData);
            if (result == false)
            {
                return;
            }
            #endregion 

            //每一筆單獨計算
            foreach (DataRow dr in dtSelectData.Rows)
            {
                DualResult boolResult = setSewingOutput(dr["ID"], dr["OrderIDFrom"], dr["StyleLocation"], dr["Article"], dr["SizeCode"], dr["ToSPBalance"], dr["FromSPPackingQty"]);

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

        private bool checkPoidAvailableQty(DataTable dtSelectData)
        {
            #region SQL Parameter
            List<SqlParameter> listSqlParameter = new List<SqlParameter>();;
            listSqlParameter.Add(new SqlParameter("@Factory", Sci.Env.User.Factory));
            #endregion 
            #region SQL Command                       
            string strSqlCmd = @"
select *
from (
	select OrderIDFrom
		   , Article
		   , SizeCode		   
		   , FromSPAvailableQty = Convert (int, sum (isnull (FromSPAvailableQty, 0)))
		   , FromSPPackingQty = Convert (int, sum (isnull (FromSPPackingQty, 0)))
		   , ToSPBalance = Convert (int, sum (isnull (ToSPBalance, 0)))
	from (
        select	ID = OQG.ID
		        , StyleLocation = SL.Location
		        , OrderIDFrom = OQG.OrderIDFrom
		        , Article = OQG.Article
		        , SizeCode = OQG.SizeCode
		        , ToSPBalance = ToSPBalance.value
		        , FromSPPackingQty = FromSPPackingQty.value        
		        , FromSPAvailableQty = FromSPAvailableQty.value
        from Order_Qty_Garment OQG
        inner join Orders ToSPOrders on OQG.ID = ToSPOrders.ID
        inner join Style_Location SL on ToSPOrders.StyleUkey = SL.StyleUkey
        inner join #tmp on OQG.ID = #tmp.ID
				           and SL.Location = #tmp.StyleLocation
				           and OQG.OrderIDFrom = #tmp.OrderIDFrom
				           and OQG.Article = #tmp.Article
				           and OQG.SizeCode = #tmp.SizeCode
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
            select value = isnull (sum(passSodd.QAQty), 0)
	        from SewingOutput_Detail passSod
	        inner join SewingOutput_Detail_Detail passSodd on passSod.UKey = passSodd.SewingOutput_DetailUKey
	        inner join Orders o on passSod.OrderId = o.ID
	        where o.POID = OQG.OrderIDFrom
		          and o.ID != o.POID
		          and SL.Location = passSodd.ComboType
		          and OQG.Article = passSodd.Article
		          and OQG.SizeCode = passSodd.SizeCode
        ) FromSPAccrQty
        outer apply (
	        select value = FromSPSewingOutputQty.value - FromSPPackingQty.value - FromSPAccrQty.value
        ) FromSPAvailableQty
        where	ToSPOrders.FtyGroup = @Factory
    ) newTmp
	group by OrderIDFrom, Article, SizeCode, FromSPAvailableQty, FromSPPackingQty
) groupTmp
where ToSPBalance > FromSPAvailableQty";
            #endregion
            DataTable resultDt;
            DualResult dualResult = MyUtility.Tool.ProcessWithDatatable(dtSelectData, null, strSqlCmd, out resultDt, paramters: listSqlParameter);

            if (dualResult == false)
            {
                MyUtility.Msg.WarningBox(dualResult.Description);
                return false;
            }else if (resultDt == null || resultDt.Rows.Count == 0)
            {
                return true;
            }else
            {
                List<string> errMsg = new List<string>();

                foreach (DataRow dr in resultDt.Rows)
                {
                    errMsg.Add(string.Format(@"
From SP# : {0} color : {1} Size : {2}, SewingOutput {3} qty less than
from SP packing : {4} qty + split qty : {5}", dr["OrderIDFrom"]
                                            , dr["Article"]
                                            , dr["SizeCode"]
                                            , dr["FromSPAvailableQty"]
                                            , dr["FromSPPackingQty"]
                                            , dr["ToSPBalance"]));
                }
                MyUtility.Msg.InfoBox("From SP Sewing Output Qty is not enough!!" + errMsg.JoinToString(Environment.NewLine));
                return false;
            }
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
        private DualResult setSewingOutput(object ToOrderID, object FromOrderID, object ComboType, object Article, object SizeCode, object NeedQty, object FromPackingQty)
        {
            DualResult boolResult;

            List<SqlParameter> listSqlPara = new List<SqlParameter>();
            listSqlPara.Add(new SqlParameter("@ToOrderID", ToOrderID));
            listSqlPara.Add(new SqlParameter("@FromOrderID", FromOrderID));
            listSqlPara.Add(new SqlParameter("@ComboType", ComboType));
            listSqlPara.Add(new SqlParameter("@Article", Article));
            listSqlPara.Add(new SqlParameter("@SizeCode", SizeCode));
            listSqlPara.Add(new SqlParameter("@NeedQty", NeedQty));
            listSqlPara.Add(new SqlParameter("@FromPackingQty", FromPackingQty));

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
into #tmp
from (
	select Reserve.*
		   , ReserveStatus = ReserveStatus.value
		   , ReserveQty = ReserveQty.value
		   , ReserveBeforeQty = ReserveBeforeQty.value
		   , soddQaQtyRunningTotal = sum(ReserveBeforeQty.value) over (order by rowNum)
	from (
		select	rowNum = ROW_NUMBER()over (order by OutPutDate, so.ID)
				, soID = so.ID
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
				, soddQaQty = sodd.QaQty - isnull(passAccuQty.value, 0)
				, ReserveRunningTotal = sum (sodd.QaQty - isnull(passAccuQty.value, 0)) over (order by OutPutDate, so.ID)
				, sodUkey = sod.UKey
		from SewingOutput so
		inner join SewingOutput_Detail sod on so.id = sod.id
		inner join SewingOutput_Detail_Detail sodd on sod.UKey = sodd.SewingOutput_DetailUKey
		-- passAccuQty --
		-- SewingOutput_Detail 計算同母單中，已分配多少數量給子單 --
		outer apply (
			select value = isnull (sum(passSodd.QAQty), 0)
			from SewingOutput_Detail passSod
			inner join SewingOutput_Detail_Detail passSodd on passSod.UKey = passSodd.SewingOutput_DetailUKey
			inner join Orders o on passSod.OrderId = o.ID
			where sod.ID = passSod.ID
				  and o.POID = @FromOrderID
				  and o.ID != o.POID
				  and sodd.ComboType = passSodd.ComboType
				  and sodd.Article = passSodd.Article
				  and sodd.SizeCode = passSodd.SizeCode
		) passAccuQty
		where	sodd.OrderId = @FromOrderID
				and sodd.ComboType = @ComboType
				and sodd.Article = @Article
				and sodd.SizeCode = @SizeCode
	) Reserve
	-- ReserveStatus --
	-- 預留母單的結果 => ReserveD = 不能使用
	--					 ReserveU = 部分可以使用
	--					 ReserveN = 全部可以使用
	outer apply (
		select value = case 
						 when ReserveRunningTotal <= @FromPackingQty then 'ReserveD'
						 when (Reserve.ReserveRunningTotal > @FromPackingQty and Reserve.ReserveRunningTotal - Reserve.soddQaQty < @FromPackingQty) then 'ReserveU'
						 else 'ReserveN'
					   end
	) ReserveStatus
	-- ReserveQty --
	-- 預留母單的數量 --
	outer apply (
		select value = case ReserveStatus.value
						 when 'ReserveN' then 0 
						 when 'ReserveU' then soddQaQty - (ReserveRunningTotal - @FromPackingQty)
					   end
	) ReserveQty 
	-- ReserveBeforeQty --
	-- 預留母單後的數量 --
	outer apply (
		select value = case ReserveStatus.value
						 when 'ReserveN' then soddQaQty 
						 when 'ReserveU' then ReserveRunningTotal - @FromPackingQty
					   end
	) ReserveBeforeQty 
	where ReserveStatus.value != 'ReserveD'
) OutputData
-- SoddStatus -- 
-- 判斷 SewingOutput_Detail_Detail => D = 刪除
--                                    U = 修改
--                                    N = 不會使用到
--									  ReserveU_U = 只能使用部分
outer apply (
	select	value = case 	
						when (OutputData.soddQaQtyRunningTotal <= @NeedQty and OutputData.ReserveStatus = 'ReserveN') then 'D'
						when (OutputData.soddQaQtyRunningTotal <= @NeedQty and OutputData.ReserveStatus = 'ReserveU') then 'ReserveU_U'
						when (OutputData.soddQaQtyRunningTotal > @NeedQty and OutputData.soddQaQtyRunningTotal - OutputData.soddQaQty < @NeedQty) then 'U'
						else 'N'
					end
) SoddStatus
-- SoddNewQaQty --
-- 計算 SewingOutput_Detail_Detail 拆單後剩餘數量 --
outer apply (
	select value = case SoddStatus.value
						when 'U' then soddQaQtyRunningTotal - @NeedQty + OutputData.ReserveQty
						when 'ReserveU_U' then OutputData.ReserveQty
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
						when (SoddStatus.value = 'U' or SoddStatus.value = 'ReserveU_U') then 'U'
						else 'N'
					end
) SodStatus
-- SodNewQaQty --
-- 計算 SewingOutput_Detail 拆單後剩餘數量 --
outer apply (
	select value = case SodStatus.value
						when 'U' then OutputData.sodQAQty - (OutputData.soddQaQty - SoddNewQaQty.value)
						when 'ReserveU_U' then OutputData.ReserveQty
				   end
) SodNewQaQty
-- TakeQty --
-- 計算這一張單拿多少 Qty --
outer apply (
	select value = case SodStatus.value
						when 'U' then OutputData.soddQaQty - SoddNewQaQty.value
						when 'ReserveU_U' then OutputData.soddQaQty - SoddNewQaQty.value - OutputData.ReserveQty
						else OutputData.soddQaQty
				   end
) TakeQty
where SoddStatus.value != 'N'

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
				, t.InlineQty = t.InlineQty + s.TakeQty
				, t.TMS = 0
				, t.HourlyStandardOutput = s.HourlyStandardOutput
				, t.AutoCreate = 1
when not matched by target then
	insert (
		ID			    , OrderID				, ComboType			, Article		, Color
		, TMS		    , HourlyStandardOutput	, WorkHour			, QaQty			, DefectQty
		, InlineQty     , AutoCreate
	) values (
		s.soID		    , s.ToOrderID			, s.ComboType		, s.sodArticle	, s.sodColor
		, 0				, s.HourlyStandardOutput, 0					, s.TakeQty		, 0
		, s.TakeQty		, 1
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
where	pld.OrderID = @FromOrderID
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
        , FromSPAccrQty = FromSPAccrQty.value
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
    select value = isnull (sum(passSodd.QAQty), 0)
	from SewingOutput_Detail passSod
	inner join SewingOutput_Detail_Detail passSodd on passSod.UKey = passSodd.SewingOutput_DetailUKey
	inner join Orders o on passSod.OrderId = o.ID
	where o.POID = OQG.OrderIDFrom
		  and o.ID != o.POID
		  and SL.Location = passSodd.ComboType
		  and OQG.Article = passSodd.Article
		  and OQG.SizeCode = passSodd.SizeCode
) FromSPAccrQty
outer apply (
	select value = FromSPSewingOutputQty.value - FromSPPackingQty.value - FromSPAccrQty.value
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
