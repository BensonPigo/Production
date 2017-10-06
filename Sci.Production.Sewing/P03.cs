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

            DataTable[] dtOrdersReceive = setSewingOutput(dtSelectData);

            if (dtOrdersReceive != null)
            {
                var form = new P03_SaveComplete(dtOrdersReceive[1], dtOrdersReceive[0]);
                form.ShowDialog();
                this.findNow();
            }
        }

        /// <summary>
        /// Get SewingOutput Data
        /// </summary>
        /// <param name="dtSelectData">ID</param>
        private DataTable[] setSewingOutput(DataTable dtSelectData)
        {
            DualResult boolResult;
            DataTable[] dtOrdersReceive;

            StringBuilder strSqlCmd = new StringBuilder();
            #region 確認 Packing 數量 
            strSqlCmd.Append(@"
---- Output 數量超過 Garment ---------------------------------------------------------------------------------------
select OrderID = OQG.ID
	   , POID = OQG.OrderIDFrom
	   , ComboType = SL.Location
	   , OQG.Article
	   , OQG.SizeCode
	   , ToSPQty = case OQG.Junk
					when 1 then 0
					else OQG.Qty
				   end
	   , OverQty = OverQty.value
	   , BalanceQty = AccuSoddQty.value - OverQty.value
into #OverGarment
from Order_Qty_Garment OQG 
inner join Orders ToSPOrders on OQG.ID = ToSPOrders.ID
inner join Style_Location SL on ToSPOrders.StyleUkey = SL.StyleUkey
outer apply (
	select value = isnull (sum (isnull (sodd.QAQty, 0)), 0)
	from SewingOutput_Detail_Detail sodd 
	where oqg.ID = sodd.OrderId
		  and SL.Location = sodd.ComboType
		  and OQG.Article = sodd.Article
		  and OQG.SizeCode = sodd.SizeCode
) AccuSoddQty
outer apply (
	select value = case OQG.Junk
						when 1 then AccuSoddQty.value
						else AccuSoddQty.value - OQG.Qty
				   end
) OverQty
where OverQty.value > 0
	  and exists (select 1
				  from #tmp 
				  where OQG.OrderIDFrom = #tmp.OrderIDFrom)

/*
select * from #OverGarment
drop table #OverGarment
*/
---- Packing 數量超過準備移除的數量 ---------------------------------------------------------------------------------------
select #OverGarment.*
	   , PackingLockQty = PackingQty.value - #OverGarment.BalanceQty
into #PackingNotEnough
from #OverGarment
outer apply (
	select value = isnull (sum (isnull (pld.ShipQty, 0)), 0)
	from PackingList pl
	inner join PackingList_Detail pld on pl.ID = pld.ID
	where pld.OrderID = #OverGarment.OrderID
		  and pl.Status = 'Confirmed'	
) PackingQty
where PackingQty.value > #OverGarment.BalanceQty

/*
select * from #PackingNotEnough
drop table #PackingNotEnough
*/
---- tmp 移除 : Packing 數量足夠的 Output ---------------------------------------------------------------------------------------
select	ID
		, SewingOutput_DetailUKey
		, OrderId
		, ComboType
		, Article
		, SizeCode
		, QAQty
		, DeleteRunningTotal
		, tmpStatus = tmpStatus.value
		, newQaQty = case tmpStatus.value
						when 'D' then 0
						when 'U' then DeleteRunningTotal - OverQty
					 end
into #tmpDeleteData
from (
	select so.ID
		   , sodd.SewingOutput_DetailUKey
		   , sodd.OrderId
		   , sodd.ComboType
		   , sodd.Article
		   , sodd.SizeCode
		   , sodd.QAQty
		   , DeleteRunningTotal = sum (sodd.QAQty) over (partition by sodd.OrderId, sodd.ComboType, sodd.Article, sodd.SizeCode
														 order by so.OutputDate desc)
		   , DeleteTmp.OverQty
	from SewingOutput so
	inner join SewingOutput_Detail_Detail sodd on so.ID = sodd.ID
	inner join Order_Qty_Garment OQG on sodd.OrderId = OQG.ID
										and sodd.Article = OQG.Article
										and sodd.SizeCode = OQG.SizeCode
	inner join (
		select *
		from #OverGarment
		where not exists (select 1 
						  from #PackingNotEnough
						  where #OverGarment.OrderID = #PackingNotEnough.OrderID)
	) DeleteTmp on sodd.OrderId = DeleteTmp.OrderID
				   and OQG.OrderIDFrom = DeleteTmp.POID
				   and sodd.ComboType = DeleteTmp.ComboType
				   and sodd.Article = DeleteTmp.Article
				   and sodd.SizeCode = DeleteTmp.SizeCode
) DeleteData
outer apply (
	select value = case 
					  when DeleteRunningTotal > OverQty and DeleteRunningTotal - QAQty >= OverQty then 'N'
					  when DeleteRunningTotal > OverQty then 'U'
					  when DeleteRunningTotal <= OverQty then 'D'
				   end
) tmpStatus

/*
select * from #tmpDeleteData
drop table #tmpDeleteData
*/
---- Update & Delete : SewingOutput_Detail_Detail ---------------------------------------------------------------------------------------
update sodd
set sodd.QAQty = tmpD.newQaQty
from SewingOutput_Detail_Detail sodd
inner join #tmpDeleteData tmpD on sodd.SewingOutput_DetailUKey = tmpD.SewingOutput_DetailUKey
								  and sodd.OrderId = tmpD.OrderId
								  and sodd.ComboType = tmpD.ComboType
								  and sodd.Article = tmpD.Article
								  and sodd.SizeCode = tmpd.SizeCode
where tmpD.tmpStatus in ('U')

delete sodd
from SewingOutput_Detail_Detail sodd
inner join #tmpDeleteData tmpD on sodd.SewingOutput_DetailUKey = tmpD.SewingOutput_DetailUKey
								  and sodd.OrderId = tmpD.OrderId
								  and sodd.ComboType = tmpD.ComboType
								  and sodd.Article = tmpD.Article
								  and sodd.SizeCode = tmpd.SizeCode
where tmpD.tmpStatus in ('D')

---- Update & Delete : SewingOutput_Detail ---------------------------------------------------------------------------------------
update sod
set sod.QAQty = NewQaQty.value
	, sod.InlineQty = NewQaQty.value
from SewingOutput_Detail sod
inner join #tmpDeleteData tmpD on sod.UKey = tmpD.SewingOutput_DetailUKey
								  and sod.OrderId = tmpD.OrderId
								  and sod.ComboType = tmpD.ComboType
								  and sod.Article = tmpD.Article
outer apply (
	select value = isnull (sum (sodd.QaQty), 0)
	from SewingOutput_Detail_Detail sodd
	where sodd.SewingOutput_DetailUKey = sod.UKey
) NewQaQty
where tmpD.tmpStatus in ('D', 'U')
	  and NewQaQty.value != 0

delete sod
from SewingOutput_Detail sod
inner join #tmpDeleteData tmpD on sod.UKey = tmpD.SewingOutput_DetailUKey
								  and sod.OrderId = tmpD.OrderId
								  and sod.ComboType = tmpD.ComboType
								  and sod.Article = tmpD.Article
outer apply (
	select value = isnull (sum (sodd.QaQty), 0)
	from SewingOutput_Detail_Detail sodd
	where sodd.SewingOutput_DetailUKey = sod.UKey
) NewQaQty
where tmpD.tmpStatus in ('D', 'U')
	  and NewQaQty.value = 0

---- out Packing 數量超過準備移除的數量 ---------------------------------------------------------------------------------------
select *
from #PackingNotEnough
drop table #OverGarment, #PackingNotEnough, #tmpDeleteData;");
            #endregion 
            #region Sewing P03 拆單
            strSqlCmd.Append(@"
---- 需求 ---------------------------------------------------------------------------------------
select	ID = OQG.ID
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
        , FromSPAccrQty = FromSPAccrQty.value
		, FromSPAvailableQty = FromSPAvailableQty.value
into #SelectData
from Order_Qty_Garment OQG
inner join Orders ToSPOrders on OQG.ID = ToSPOrders.ID
inner join Style_Location SL on ToSPOrders.StyleUkey = SL.StyleUkey
inner join #tmp on OQG.ID = #tmp.ID
                   and OQG.OrderIDFrom = #tmp.OrderIDFrom
                   and SL.Location = #tmp.StyleLocation
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
	select value = FromSPSewingOutputQty.value - FromSPAccrQty.value
) FromSPAvailableQty

/*
select * from #SelectData
drop table #SelectData
*/
---- 累計可分配數 -------------------------------------------------------------------------------
select *
	   , FrontAvailableQtyRunningTotal = AvailableQtyRunningTotal - AvailableQty
into #PoidAvailableReserveQty
from (
	select PoidSodd.ID
		   , PoidSo.OutputDate
		   , PoidSo.SewingLineID
		   , GroupByPoid.POID
		   , PoidSodd.ComboType
		   , PoidSodd.Article
		   , PoidSod.Color
		   , PoidSodd.SizeCode
		   , AvailableQty = AvailableQty.value
		   , AvailableQtyRunningTotal = sum (AvailableQty.value) over (partition by PoidSodd.OrderID, PoidSodd.ComboType, PoidSodd.Article, PoidSodd.SizeCode
	   																   order by PoidSo.OutputDate, PoidSo.ID)
	from (
		select distinct POID = #SelectData.OrderIDFrom
			   , ComboType = #SelectData.StyleLocation
			   , #SelectData.Article
			   , #SelectData.SizeCode
		from #SelectData
	) GroupByPoid
	inner join SewingOutput_Detail_Detail PoidSodd on GroupByPoid.POID = PoidSodd.OrderId
												  and GroupByPoid.ComboType = PoidSodd.ComboType
												  and GroupByPoid.Article = PoidSodd.Article
												  and GroupByPoid.SizeCode = PoidSodd.SizeCode
	inner join SewingOutput_Detail PoidSod on PoidSodd.SewingOutput_DetailUKey = PoidSod.UKey
	inner join SewingOutput PoidSo on PoidSodd.ID = PoidSo.ID
	outer apply (
		select value = PoidSodd.QaQty - isnull (sum (isnull (ReserveSodd.QAQty, 0)), 0)
		from Order_Qty_Garment ReserveOrders 
		inner join SewingOutput_Detail_Detail ReserveSodd on ReserveOrders.ID = ReserveSodd.OrderId
															 and ReserveOrders.Article = ReserveSodd.Article
															 and ReserveOrders.SizeCode = ReserveSodd.SizeCode
		where ReserveOrders.OrderIDFrom = PoidSodd.OrderId
			  and ReserveOrders.ID != ReserveOrders.OrderIDFrom			  
			  and PoidSodd.ID = ReserveSodd.ID
			  and PoidSodd.ComboType = ReserveSodd.ComboType
			  and PoidSodd.Article = ReserveSodd.Article
			  and PoidSodd.SizeCode = ReserveSodd.SizeCode
	) AvailableQty
	where AvailableQty.value > 0
) PoidAvailableReserveQty

/*
select * from #PoidAvailableReserveQty
drop table #PoidAvailableReserveQty
*/
---- 累計需求數 ---------------------------------------------------------------------------------
select OrdersAccuNeedQty.ID
	   , OrdersAccuNeedQty.POID
	   , OrdersAccuNeedQty.ComboType
	   , OrdersAccuNeedQty.Article
	   , OrdersAccuNeedQty.SizeCode
	   , OrdersAccuNeedQty.NeedQty
	   , OrdersAccuNeedQty.NeedQtyRunningTotal
	   , FrontNeedQtyRunningTotal = NeedQtyRunningTotal - NeedQty
	   , AccuNeedStatus = AccuNeedStatus.value
	   , CanReceiveQty = case AccuNeedStatus.value
							when 'O' then NeedQty
							when 'S' then PoidQty - (NeedQtyRunningTotal - NeedQty)
							when 'N' then 0
						 end
into #OrdersAccuNeedQty
from (
	select #SelectData.ID
		   , POID = #SelectData.OrderIDFrom
		   , ComboType = #SelectData.StyleLocation
		   , #SelectData.Article
		   , #SelectData.SizeCode
		   , NeedQty = #SelectData.ToSPBalance
		   , NeedQtyRunningTotal = sum (#SelectData.ToSPBalance) over (partition by #SelectData.OrderIDFrom, #SelectData.StyleLocation, #SelectData.Article, #SelectData.SizeCode
																       order by #SelectData.OrderIDFrom, #SelectData.ToSPBuyerDeliver, #SelectData.ID)
		   , PoidQty = PoidQty.value	
	from #SelectData
	outer apply (
		select value = sum (isnull (PoidAvailable.AvailableQty, 0))
		from #PoidAvailableReserveQty PoidAvailable
		where #SelectData.OrderIDFrom = PoidAvailable.POID
			  and #SelectData.StyleLocation = PoidAvailable.ComboType
			  and #SelectData.Article = PoidAvailable.Article
			  and #SelectData.SizeCode = PoidAvailable.SizeCode
	) PoidQty
) OrdersAccuNeedQty
-- AccuNeedStatus --
---- 母單有【足夠】數量可分配 O : PoidQty >= NeedQtyRunningTotal
---- 母單有【些許】數量可分配 S : PoidQty < NeedQtyRunningTotal && PoidQty > (NeedQtyRunningTotal - NeedQty)
---- 母單無數量可以分配		  N : PoidQty < NeedQtyRunningTotal && PoidQty <= (NeedQtyRunningTotal - NeedQty)
outer apply (
	select value = case 
						when PoidQty >= NeedQtyRunningTotal then 'O'
						when PoidQty < NeedQtyRunningTotal and PoidQty > (NeedQtyRunningTotal - NeedQty) then 'S'
						when PoidQty < NeedQtyRunningTotal and PoidQty <= (NeedQtyRunningTotal - NeedQty) then 'N'
					end
) AccuNeedStatus

/*
select * from #OrdersAccuNeedQty
drop table #OrdersAccuNeedQty
*/
---- 累計分配 -----------------------------------------------------------------------------------
select TakeQty.SewingOutputID
	   , TakeQty.SewingLineID
	   , AccuNeed.ID
	   , AccuNeed.POID
	   , AccuNeed.ComboType
	   , AccuNeed.Article
	   , TakeQty.Color
	   , AccuNeed.SizeCode
	   , TakeQty = TakeQty.value
into #OrdersReceiveTmp
from #OrdersAccuNeedQty AccuNeed
-- TakeQty --
outer apply (
	select SewingOutputID = AvailableReserve.ID
		   , AvailableReserve.SewingLineID
		   , AvailableReserve.Color
		   , TakeStatus = TakeStatus.value
		   , value = case TakeStatus.value
						when 'AG' then AvailableReserve.AvailableQty
						when 'SSG' then AvailableReserve.AvailableQty - (AvailableReserve.AvailableQtyRunningTotal - AccuNeed.NeedQtyRunningTotal)
						when 'AMSG' then AvailableReserve.AvailableQtyRunningTotal - AccuNeed.FrontNeedQtyRunningTotal
						when 'SMSG' then AccuNeed.NeedQty
					 end
	from #PoidAvailableReserveQty AvailableReserve
	--  'N' 不取數量 
	----  1. 母單數量已分配完畢			 : AccuNeed.FrontNeedQtyRunningTotal >= AvailableReserve.AvailableQtyRunningTotal
	----  2. 上一個母單已分配足夠的數量  : AccuNeed.NeedQtyRunningTotal <= AvailableReserve.FrontAvailableQtyRunningTotal
	--  'AG' 取全部數量
	----  1.a. 前面的母單數量可以滿足上一個子單     : AccuNeed.FrontNeedQtyRunningTotal <= AvailableReserve.FrontAvailableQtyRunningTotal
	----  1.b. 目前母單所有數量可被目前子單分配完畢 : AccuNeed.NeedQtyRunningTotal >= AvailableReserve.AvailableQtyRunningTotal
	--  【SSG, AMSG, SMSG】取部分數量
	---- 'SSG'  【沒有】其他子單【先】取走該母單部分數量，只取【部分】數量			   : AccuNeed.FrontNeedQtyRunningTotal <= AvailableReserve.FrontAvailableQtyRunningTotal && AccuNeed.NeedQtyRunningTotal < AvailableReserve.AvailableQtyRunningTotal
	---- 'AMSG' 【有】其他子單【先】取走該母單部分數量，且剩餘數量可以直接【完全】分配 : AccuNeed.NeedQty >= AvailableReserve.AvailableQtyRunningTotal - AccuNeed.FrontNeedQtyRunningTotal
	---- 'SMSG' 【有】其他子單【先】取走該母單部分數量，但只取【部分】數量
	outer apply (
		select value = case 
							when AccuNeed.FrontNeedQtyRunningTotal >= AvailableReserve.AvailableQtyRunningTotal then 'N'
							when AccuNeed.NeedQtyRunningTotal <= AvailableReserve.FrontAvailableQtyRunningTotal then 'N'
							when AccuNeed.FrontNeedQtyRunningTotal <= AvailableReserve.FrontAvailableQtyRunningTotal
								 and AccuNeed.NeedQtyRunningTotal >= AvailableReserve.AvailableQtyRunningTotal then 'AG'
						    when AccuNeed.FrontNeedQtyRunningTotal <= AvailableReserve.FrontAvailableQtyRunningTotal
								 and AccuNeed.NeedQtyRunningTotal < AvailableReserve.AvailableQtyRunningTotal then 'SSG'
							when AccuNeed.NeedQty >= AvailableReserve.AvailableQtyRunningTotal - AccuNeed.FrontNeedQtyRunningTotal then 'AMSG'
							else 'SMSG'
					   end
	) TakeStatus
	where AccuNeed.POID = AvailableReserve.POID
		  and AccuNeed.ComboType = AvailableReserve.ComboType
		  and AccuNeed.Article = AvailableReserve.Article
		  and AccuNeed.SizeCode = AvailableReserve.SizeCode
		  and TakeStatus.value != 'N'
) TakeQty
where AccuNeed.AccuNeedStatus != 'N'

---- 子單 Merge SewingOutput_Detail -------------------------------------------------------------
Merge SewingOutput_Detail as t
using (
	select	tmp.SewingOutputID
			, tmp.ID
			, tmp.ComboType
			, Article
			, tmp.Color	
			, takeQty = sum(takeQty)
	from #OrdersReceiveTmp tmp
	left join Orders o on tmp.ID = o.ID
	left join Style_Location sl on o.StyleUkey = sl.StyleUkey
								   and tmp.ComboType = sl.Location
	left join SewingSchedule ss on tmp.ID = ss.OrderID
								   and tmp.ComboType = ss.ComboType
								   and tmp.SewingLineID = ss.SewingLineID
	group by tmp.SewingOutputID, tmp.ID, tmp.ComboType, Article, tmp.Color			 
) as s on t.ID = s.SewingOutputID
		  and t.OrderID = s.ID
		  and t.ComboType = s.ComboType
		  and t.Article = s.Article
		  and t.Color = s.Color
when matched then 
	update set	t.QaQty = t.QaQty + s.TakeQty
				, t.InlineQty = t.InlineQty + s.TakeQty
when not matched by target then
	insert (
		ID					, OrderID				, ComboType			, Article		, Color
		, TMS				, HourlyStandardOutput	, WorkHour			, QaQty			, DefectQty
		, InlineQty			, AutoCreate
	) values (
		s.SewingOutputID	, s.ID					, s.ComboType		, s.Article		, s.Color
		, 0					, 0						, 0					, s.TakeQty		, 0
		, s.TakeQty			, 1
	);

---- 子單 Merge SewingOutput_Detail_Detail ------------------------------------------------------
Merge SewingOutput_Detail_Detail as t
using (
	select	tmp.*
			, sod.Ukey
	from #OrdersReceiveTmp tmp
	inner join SewingOutput_Detail sod on sod.ID = tmp.SewingOutputID
										  and sod.OrderID = tmp.ID
										  and sod.ComboType = tmp.ComboType
										  and sod.Article = tmp.Article
										  and sod.Color = tmp.Color
) as s on t.SewingOutput_DetailUkey = s.Ukey
		  and t.ID = s.SewingOutputID
		  and t.OrderID = s.ID
		  and t.ComboType = s.ComboType
		  and t.Article = s.Article
		  and t.SizeCode = s.SizeCode
when matched then
	update set	t.QaQty = t.QaQty + s.TakeQty
when not matched by target then 
	insert (
		ID					, SewingOutput_DetailUkey	, OrderID	, ComboType		, Article
		, SizeCode			, QaQty
	) values (
		s.SewingOutputID	, s.Ukey					, s.ID		, s.ComboType	, s.Article
		, s.SizeCode		, s.TakeQty
	);

---- 成功分配清單 ------------------------------------------------------
select OrderID = ID
	   , POID
	   , ComboType
	   , Article
	   , SizeCode
	   , NeedQty
	   , CanReceiveQty
from #OrdersAccuNeedQty
where AccuNeedStatus in ('O', 'S')

drop table #tmp, #PoidAvailableReserveQty, #OrdersAccuNeedQty, #OrdersReceiveTmp");
            #endregion

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {

                boolResult = MyUtility.Tool.ProcessWithDatatable(dtSelectData, null, strSqlCmd.ToString(), out dtOrdersReceive);
                if (!boolResult)
                {
                    transactionscope.Dispose();
                    MyUtility.Msg.WarningBox(boolResult.ToString());
                    return null;
                }

                #region 
                //                #region Check 母單 PackingList = Confirmed，必須保證拆單後，母單剩餘數量大於 Packing 數量
                //                strSqlCmd = @"
                //-- Check 母單 PackingList = Confirmed，必須保證拆單後，母單剩餘數量大於 Packing 數量 --
                //-- checkValue => Packing > Sewing = 0 數量不足
                //--				 Packing <= Sewing = 1 數量足夠
                //declare @PackingTtlQty int = 0;
                //declare @SewingTtlQty int = 0;

                //select	@PackingTtlQty = isnull (sum (pld.ShipQty), 0)
                //from PackingList pl
                //inner join PackingList_Detail pld on pl.ID = pld.ID							  
                //where	pld.OrderID = @FromOrderID
                //		and pl.Status = 'Confirmed'		

                //select	@SewingTtlQty = isnull (sum (sodd.QAQty), 0)
                //from PackingList pl
                //inner join PackingList_Detail pld on pl.ID = pld.ID
                //inner join SewingOutput_Detail sod on pl.OrderID = sod.OrderId
                //inner join SewingOutput_Detail_Detail sodd on sod.UKey = sodd.SewingOutput_DetailUKey
                //											  and pld.Article = sodd.Article
                //											  and pld.SizeCode = sodd.SizeCode											  
                //where	pl.OrderID = @FromOrderID
                //		and pl.Status = 'Confirmed'

                //select checkValue = iif (@SewingTtlQty >= @PackingTtlQty, 1
                //													    , 0)";
                //                #endregion
                //                if (MyUtility.GetValue.Lookup(strSqlCmd, listSqlPara).Equals("0"))
                //                {
                //                    transactionscope.Dispose();
                //                    MyUtility.Msg.WarningBox(string.Format("From SP {0} Sewing Output Qty can't less then PackingList Qty.", FromOrderID));
                //                    return false;
                //                }

                //                #region Check 拆單後的數量，總數不得超過子單 Order_Qty
                //                strSqlCmd = @"
                //-- Check 拆單後的數量，總數不得超過子單 Order_Qty --
                //select	*
                //from Order_Qty oq
                //cross apply (
                //	select value = MIN(value)
                //	from (
                //		select value = sum(sodd.QaQty)
                //		from SewingOutput_Detail_Detail sodd 
                //		where oq.Article = sodd.Article
                //			  and oq.SizeCode = sodd.SizeCode
                //			  and oq.ID = sodd.OrderId		  
                //		group by ComboType
                //	)x
                //) SewingQty
                //where	oq.id = @ToOrderID
                //		and oq.Qty < SewingQty.value";
                //                #endregion                
                //                if (MyUtility.Check.Seek(strSqlCmd, listSqlPara))
                //                {
                //                    transactionscope.Dispose();
                //                    MyUtility.Msg.WarningBox(string.Format("SP {0}, ComboType {1}, Article {2}, SizeCode {3} Sewing Output Qty can't more then Order_Qty.", ToOrderID, ComboType, Article, SizeCode));
                //                    return false;
                //                }
                #endregion

                transactionscope.Complete();
                transactionscope.Dispose();
            }
            
            return dtOrdersReceive;
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
select	sel = 0
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
	select value = FromSPSewingOutputQty.value - FromSPAccrQty.value
) FromSPAvailableQty
where	ToSPBalance.value > 0
        and (OQG.junk = 0 or OQG.junk is null)
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
