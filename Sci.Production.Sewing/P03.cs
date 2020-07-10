using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Sci.Data;
using Ict;
using System.Transactions;

namespace Sci.Production.Sewing
{
    /// <summary>
    /// P03
    /// </summary>
    public partial class P03 : Win.Tems.QueryForm
    {
        /// <summary>
        /// P03
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P03(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            #region Set Default Data
            this.dateRangeBuyerDelivery.Value1 = DateTime.Today;
            this.dateRangeBuyerDelivery.Value2 = DateTime.Today.AddMonths(1);
            #endregion
            this.grid.IsEditingReadOnly = false;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region Grid Setting
            this.Helper.Controls.Grid.Generator(this.grid)
                .CheckBox("sel", header: string.Empty, trueValue: 1, falseValue: 0, iseditable: true)
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
            this.FindNow();
        }

        private void ButtonNewSearch_Click(object sender, EventArgs e)
        {
            this.textBoxToSpNum.Text = string.Empty;
            this.textBoxFromSpNum.Text = string.Empty;
            this.dateRangeBuyerDelivery.Value1 = null;
            this.dateRangeBuyerDelivery.Value2 = null;
        }

        private void ButtonFindNow_Click(object sender, EventArgs e)
        {
            this.FindNow();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            DataTable dtSelectData = (DataTable)((BindingSource)this.grid.DataSource).DataSource;
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

            DataTable[] dtOrdersReceive = this.SetSewingOutput(dtSelectData);

            if (dtOrdersReceive != null)
            {
                var form = new P03_SaveComplete(dtOrdersReceive[0]);
                form.ShowDialog();
                this.FindNow();
            }
        }

        /// <summary>
        /// SetSewingOutput
        /// </summary>
        /// <param name="dtSelectData">dtSelectData</param>
        /// <returns>DataTable[]</returns>
        private DataTable[] SetSewingOutput(DataTable dtSelectData)
        {
            DualResult boolResult;
            DataTable[] dtOrdersReceive;

            StringBuilder strSqlCmd = new StringBuilder();

            #region Sewing P03 拆單
            strSqlCmd.Append(@"
--檢查OrderId在Order_Location是否有資料，沒資料就補
DECLARE CUR_SewingOutput_Detail CURSOR FOR 
     Select distinct id from #tmp 

declare @orderid1 varchar(13) 
OPEN CUR_SewingOutput_Detail   
FETCH NEXT FROM CUR_SewingOutput_Detail INTO @orderid1 
WHILE @@FETCH_STATUS = 0 
BEGIN
  exec dbo.Ins_OrderLocation @orderid1
FETCH NEXT FROM CUR_SewingOutput_Detail INTO @orderid1
END
CLOSE CUR_SewingOutput_Detail
DEALLOCATE CUR_SewingOutput_Detail

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
	select value = isnull (sum (SODDG.QAQty), 0)
	from SewingOutput_Detail_Detail_Garment SODDG
	where	SODDG.OrderId = OQG.ID
			and SODDG.ComboType = SL.Location
			and SODDG.Article = OQG.Article
			and SODDG.SizeCode = OQG.SizeCode
			and SODDG.OrderIDfrom = OQG.OrderIDFrom
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
    select value = isnull (sum(passSoddG.QAQty), 0)
	from SewingOutput_Detail_Detail_Garment passSoddG
	where #tmp.OrderIDFrom = passSoddG.OrderIDfrom
		  and #tmp.StyleLocation = passSoddG.ComboType
		  and #tmp.Article = passSoddG.Article
		  and #tmp.SizeCode = passSoddG.SizeCode
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
		select value = PoidSodd.QaQty - isnull (sum (isnull (ReserveSoddG.QAQty, 0)), 0)
		from SewingOutput_Detail_Detail_Garment ReserveSoddG
		where PoidSodd.ID = ReserveSoddG.ID
			  and GroupByPoid.POID = ReserveSoddG.OrderIDfrom
			  and PoidSodd.ComboType = ReserveSoddG.ComboType
			  and PoidSodd.Article = ReserveSoddG.Article
			  and PoidSodd.SizeCode = ReserveSoddG.SizeCode
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
       , OrdersAccuNeedQty.OrderIDFrom
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
           , #SelectData.OrderIDFrom	
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
where NeedQty > 0

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
       ,AccuNeed.OrderIDFrom
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
			, tmp.Article
			, tmp.Color	
			, takeQty = sum(takeQty)
            , TMS = TMS.value
	from #OrdersReceiveTmp tmp
    /*outer apply (
	    select top 1 value = StdTMS
	    from System WITH (NOLOCK)
    ) StdTms
    outer apply (
	    select top 1 value = Round (o.CPU 
							        * o.CPUFactor 
							        * ([dbo].[GetStyleLocation_Rate](o.StyleUkey ,sl.Location) / 100) 
							        * StdTms.value
							       , 0)
	    from Orders o WITH (NOLOCK) 
	    inner join Style_Location SL on o.StyleUkey = SL.StyleUkey
	    where   o.ID = tmp.POID
			    and SL.Location = tmp.ComboType
    ) TMS*/
    --TMS改取母單
    outer apply (
	    select  value = sod.TMS
	    from SewingOutput_Detail sod WITH (NOLOCK) 
	    where   sod.ID = tmp.SewingOutputID 
            and sod.OrderId = tmp.OrderIDFrom 
            and sod.ComboType = tmp.ComboType
		    and sod.Article = tmp.Article
		    and sod.Color = tmp.Color
    ) TMS
	group by tmp.SewingOutputID, tmp.ID, tmp.ComboType, tmp.Article, tmp.Color, TMS.value 
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
		, s.TMS				, 0						, 0					, s.TakeQty		, 0
		, s.TakeQty			, 1
	);

---- 子單 Merge SewingOutput_Detail_Detail ------------------------------------------------------
Merge SewingOutput_Detail_Detail as t
using (
	select	tmp.SewingOutputID
			, tmp.ID
			, tmp.ComboType
			, tmp.Article
			, tmp.Color	
            , tmp.SizeCode
			, sod.Ukey
			, takeQty = sum(takeQty)
	from #OrdersReceiveTmp tmp
	inner join SewingOutput_Detail sod on sod.ID = tmp.SewingOutputID
										  and sod.OrderID = tmp.ID
										  and sod.ComboType = tmp.ComboType
										  and sod.Article = tmp.Article
										  and sod.Color = tmp.Color
    group by tmp.SewingOutputID, tmp.ID, tmp.ComboType, tmp.Article, tmp.Color, tmp.SizeCode, sod.Ukey
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

---- 子單 Merge SewingOutput_Detail_Detail_Garment ----------------------------------------------
Merge SewingOutput_Detail_Detail_Garment as t
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
		  and t.OrderIDFrom = s.POID
when matched then
	update set	t.QaQty = t.QaQty + s.TakeQty
when not matched by target then 
	insert (
		ID					, SewingOutput_DetailUkey	, OrderID	, ComboType		, Article
		, SizeCode			, OrderIDFrom 				, QaQty
	) values (
		s.SewingOutputID	, s.Ukey					, s.ID		, s.ComboType	, s.Article
		, s.SizeCode		, s.POID 			        , s.TakeQty
	);

---- 重新計算 SewingOutputID 子單有新增 or 修改資料的 WorkHour ---------
/*
 * 所有須更新的 SewingOutputID
 */
select distinct ID = SewingOutputID
into #SewingID
from #OrdersReceiveTmp

/*
 * 紀錄所有須更新的子單
 */
select soddG.ID
	   , soddG.OrderId
	   , soddG.ComboType
	   , soddG.Article
	   , soddG.OrderIDfrom
	   , sod.TMS
	   , QAQty = sum(soddG.QaQty)
	   , soddG.SewingOutput_DetailUKey
       , AllAllot = AllAllot.value
into #Child
from SewingOutput_Detail_Detail_Garment soddG
inner join SewingOutput_Detail sod on soddG.SewingOutput_DetailUKey = sod.UKey
inner join #SewingID on soddG.ID = #SewingID.ID
outer apply (
	select value = sum (mSod.QAQty)
	from SewingOutput_Detail mSod
	where sod.ID = mSod.ID
		  and soddG.OrderIDfrom = mSod.OrderId
		  and soddG.ComboType = mSod.ComboType
		  and soddG.Article = mSod.Article
) motherTTL
outer apply (
	select value = sum (cSoddG.QAQty)
	from SewingOutput_Detail_Detail_Garment cSoddG
	where sod.ID = cSoddG.ID
		  and soddG.OrderIDfrom = cSoddG.OrderIDfrom
		  and soddG.ComboType = cSoddG.ComboType
		  and soddG.Article = cSoddG.Article
) childTTL
outer apply (
	select value = iif (motherTTL.value = childTTL.value, 1, 0)
) AllAllot
group by soddG.ID, soddG.OrderId, soddG.ComboType, soddG.Article, soddG.OrderIDfrom
         , sod.TMS, soddG.SewingOutput_DetailUKey, AllAllot.value

/*
 * tmp 重新計算子單 WorkHour
 */
select distinct #Child.ID
	   , #Child.OrderId
	   , #Child.ComboType
	   , #Child.Article
	   , #Child.SewingOutput_DetailUKey
	   , workHour = Convert (numeric(11, 3), 0)
into #updateChild
from #Child

/*
 * 迴圈更新每一個 SewingOutputID
 */
declare SewingOutputCursor Cursor For
select ID
from #SewingID

Open SewingOutputCursor
declare @SewingID varchar(50);

Fetch Next From SewingOutputCursor Into @SewingID
while (@@FETCH_STATUS = 0)
begin
	/*
	 * 根據 Sewing ID 取得所有母單的 OrderID, ComboType, Article
	 */
	declare ComputeCursor Cursor For
	select OrderID
		   , ComboType
		   , Article
	from SewingOutput_Detail
	where ID = @SewingID

	Open ComputeCursor
	declare @OrderID varchar (50);
	declare @ComboType varchar (2);
	declare @Article varchar (50);

	Fetch Next From ComputeCursor Into @OrderID, @ComboType, @Article
	while (@@FETCH_STATUS = 0)
	begin
		update upd
			set upd.WorkHour = upd.WorkHour
						       + case setWorkHour.AllAllot
                                    -- 母單數量已全部分配
                                    -- 子單 WorkHour 加總 = 母單 WorkHour
								    when 1 then 
									    case
									        when setWorkHour.rowNum = setWorkHour.rowCounts then
										        iif (setWorkHour.AccuWorkHour - setWorkHour.newWorkHour > setWorkHour.soWorkHour, 0
																														        , setWorkHour.soWorkHour - (setWorkHour.AccuWorkHour - setWorkHour.newWorkHour))
									        else
										        iif (setWorkHour.AccuWorkHour > setWorkHour.soWorkHour, iif (setWorkHour.AccuWorkHour - setWorkHour.newWorkHour >  setWorkHour.soWorkHour, 0
											 																																	         , setWorkHour.soWorkHour - (setWorkHour.AccuWorkHour - setWorkHour.newWorkHour))
																							          , setWorkHour.newWorkHour)
									    end
                                    -- 母單數量尚未分配完畢
								    else
									    case 
										    when setWorkHour.AccuWorkHour > setWorkHour.soWorkHour then
											    iif (setWorkHour.soWorkHour < (setWorkHour.AccuWorkHour - setWorkHour.newWorkHour), 0
																															      , setWorkHour.soWorkHour - (setWorkHour.AccuWorkHour - setWorkHour.newWorkHour))
										    else
											    setWorkHour.newWorkHour
									    end
						         end
		from #updateChild upd
		inner join (
			select	#Child.SewingOutput_DetailUKey
					, soWorkHour = sod.WorkHour
					, newWorkHour = ComputeWorkHour.value
					, AccuWorkHour = sum(ComputeWorkHour.value) over (partition by #Child.OrderIDFrom order by #Child.OrderID)
					, rowNum = row_number() over (partition by #Child.OrderIDFrom order by #Child.OrderID)
					, rowCounts = count(1) over (partition by #Child.OrderIDFrom)
				    , #Child.AllAllot
			from SewingOutput_Detail sod
			inner join #Child on sod.ID = #Child.ID
								 and sod.OrderId = #Child.OrderIDfrom
								 and sod.ComboType = #Child.ComboType
								 and sod.Article = #Child.Article
			outer apply (
				select value = isnull(sod.QaQty * sod.TMS, 0)
			) TotalQaQty
			outer apply (
				select value = Round(1.0 * #Child.QaQty * #Child.TMS / TotalQaQty.value * sod.WorkHour, 3)
			) ComputeWorkHour
			where sod.ID = @SewingID
				  and sod.OrderId = @OrderID
				  and sod.ComboType = @ComboType
				  and sod.Article = @Article
				  and sod.AutoCreate = 0
		) setWorkHour on upd.SewingOutput_DetailUKey = setWorkHour.SewingOutput_DetailUKey

		Fetch Next From ComputeCursor Into @OrderID, @ComboType, @Article
	end

	close ComputeCursor
	Deallocate ComputeCursor

	Fetch Next From SewingOutputCursor Into @SewingID
end

close SewingOutputCursor
Deallocate SewingOutputCursor

/*
 * Update
 */
update sod
set sod.WorkHour = upd.workHour
from SewingOutput_Detail sod
inner join #updateChild upd on sod.UKey = upd.SewingOutput_DetailUKey

--update SewingOutput.ReDailyTransferDate 讓更新資料傳回台北
update SewingOutput set ReDailyTransferDate = GETDATE() where ID IN (SELECT SewingOutputID FROM #OrdersReceiveTmp)

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


drop table #tmp, #PoidAvailableReserveQty, #OrdersAccuNeedQty, #OrdersReceiveTmp, #SelectData;");
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

                transactionscope.Complete();
                transactionscope.Dispose();
            }

            return dtOrdersReceive;
        }

        private void FindNow()
        {
            #region SQL Parameter
            List<SqlParameter> listSqlParameter = new List<SqlParameter>();
            listSqlParameter.Add(new SqlParameter("@ToSP", this.textBoxToSpNum.Text));
            listSqlParameter.Add(new SqlParameter("@FromSP", this.textBoxFromSpNum.Text));
            listSqlParameter.Add(new SqlParameter("@StartDate", this.dateRangeBuyerDelivery.Value1.Empty() ? string.Empty : ((DateTime)this.dateRangeBuyerDelivery.Value1).ToString("yyyy/MM/dd")));
            listSqlParameter.Add(new SqlParameter("@EndDate", this.dateRangeBuyerDelivery.Value2.Empty() ? string.Empty : ((DateTime)this.dateRangeBuyerDelivery.Value2).ToString("yyyy/MM/dd")));
            listSqlParameter.Add(new SqlParameter("@Factory", Env.User.Factory));
            #endregion
            #region SQL Filte
            #region BuyerDelivery Filte
            string strBuyerDeliveryFilte = string.Empty;
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
            dicSqlFilte.Add("ToSP", this.textBoxToSpNum.Text.Empty() ? string.Empty : "and OQG.ID = @ToSP");
            dicSqlFilte.Add("FromSP", this.textBoxFromSpNum.Text.Empty() ? string.Empty : "and OQG.OrderIDFrom = @FromSP");
            dicSqlFilte.Add("BuyerDelivery", strBuyerDeliveryFilte);
            #endregion
            #region SQL Command
            string strSqlCmd = $@"
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
inner join factory f on ToSPOrders.FactoryID = f.id 
		   				and f.IsProduceFty = 1 --判斷是否為生產工廠
outer apply (
	select value = isnull (OQG.Qty, 0)
) ToSPQty
outer apply (
	select value = isnull (sum (SODDG.QAQty), 0)
	from SewingOutput_Detail_Detail_Garment SODDG
	where	SODDG.OrderId = OQG.ID
			and SODDG.ComboType = SL.Location
			and SODDG.Article = OQG.Article
			and SODDG.SizeCode = OQG.SizeCode
			and SODDG.OrderIDFrom = OQG.OrderIDFrom
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
	select value = isnull (sum (SODDG.QAQty), 0)
	from SewingOutput_Detail_Detail_Garment SODDG
	where	SODDG.ComboType = SL.Location
			and SODDG.Article = OQG.Article
			and SODDG.SizeCode = OQG.SizeCode
			and SODDG.OrderIDFrom = OQG.OrderIDFrom
) FromSPAccrQty
outer apply (
	select value = FromSPSewingOutputQty.value - FromSPAccrQty.value
) FromSPAvailableQty
where	ToSPBalance.value > 0
        and (OQG.junk = 0 or OQG.junk is null)
        and ToSPOrders.FtyGroup = @Factory
		-- ToSP
		{dicSqlFilte["ToSP"]}
        -- FromSP
        {dicSqlFilte["FromSP"]}
        --BuyerDelivery
        {dicSqlFilte["BuyerDelivery"]}
order by OQG.OrderIDFrom, ToSPOrders.BuyerDelivery";
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
                MyUtility.Msg.WarningBox(result.ToString());
                this.bindingSource.DataSource = null;
            }
            #endregion
            this.HideWaitMessage();
        }
    }
}
