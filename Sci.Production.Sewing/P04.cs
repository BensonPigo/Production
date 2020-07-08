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
    /// P04
    /// </summary>
    public partial class P04 : Win.Tems.QueryForm
    {
        /// <summary>
        /// P04
        /// </summary>
        /// <param name="menuitem">menuitem</param>
        public P04(ToolStripMenuItem menuitem)
            : base(menuitem)
        {
            this.InitializeComponent();
            this.grid.IsEditingReadOnly = false;
        }

        /// <inheritdoc/>
        protected override void OnFormLoaded()
        {
            base.OnFormLoaded();
            #region Grid Setting
            this.Helper.Controls.Grid.Generator(this.grid)
                .Text("ID", header: "SP", iseditingreadonly: true)
                .Text("StyleLocation", header: "*", iseditingreadonly: true)
                .Text("OrderIDFrom", header: "From SP", iseditingreadonly: true)
                .Text("Article", header: "Article", iseditingreadonly: true)
                .Text("SizeCode", header: "Size", iseditingreadonly: true)
                .Numeric("ToSPQty", header: "To SP Qty", iseditingreadonly: true)
                .Numeric("ToSPAllocatedQty", header: "To SP Allocated Qty" + Environment.NewLine + "(From SP# Allocated To SP# Qty)", iseditingreadonly: true)
                .Numeric("ToSPExcess", header: "To SP" + Environment.NewLine + "Excess Qty", iseditingreadonly: true)
                .Numeric("ToSPConfirmPK", header: "To SP" + Environment.NewLine + "Confirm Packing Qty", iseditingreadonly: true)
                .Numeric("ToSPAllSW_Out", header: "To SP All" + Environment.NewLine + "SewingOutput Qty", iseditingreadonly: true)
                .Text("ReduceConfirmPK", header: "Is Must reduce" + Environment.NewLine + "Confirm Packing Qty", iseditingreadonly: true);

            for (int i = 0; i < this.grid.Columns.Count; i++)
            {
                this.grid.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            #endregion
            this.FindNow();
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            this.FindNow();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            DataTable dtSelectData = (DataTable)((BindingSource)this.grid.DataSource).DataSource;
            #region Select Data
            if (dtSelectData.Select("do_flag = 'Y'").Length > 0)
            {
                dtSelectData = dtSelectData.AsEnumerable().Where(row => row["do_flag"].Equals("Y")).CopyToDataTable();
            }
            else
            {
                MyUtility.Msg.InfoBox("No Data can do Remove Garment Order Sewing Output.");
                return;
            }
            #endregion

            this.SetSewingOutput(dtSelectData);

            if (dtSelectData != null)
            {
               var form = new P04_SaveComplete(dtSelectData);
               form.ShowDialog();
               this.FindNow();
            }
        }

        /// <summary>
        /// Get SewingOutput Data
        /// </summary>
        /// <param name="dtSelectData">ID</param>
        private void SetSewingOutput(DataTable dtSelectData)
        {
            DualResult boolResult;
            DataTable[] dtOrdersReceive;

            StringBuilder strSqlCmd = new StringBuilder();
            #region 調整數量
            strSqlCmd.Append(@"
--Update & Delete :  SewingOutput_Detail_Detail_Garment
--create 一個 tmp table 存要更新SewingOutput_Detail_Detail_Garment的分配資料
select ID,SewingOutput_DetailUKey,OrderId,ComboType,Article,SizeCode,OrderIDfrom,NewQAQty = QAQty,Updstatus = '' into #tmpUpd   from SewingOutput_Detail_Detail_Garment where 1 = 0;

declare ComputeNewQaQtyCursor Cursor For
select ID,StyleLocation,Article,SizeCode,OrderIDFrom,ToSPExcess
from #tmp

open	ComputeNewQaQtyCursor;
declare  @orderid      varchar(50)
declare  @StyleLocation varchar(1)
declare  @Article  varchar(50)
declare  @SizeCode varchar(8)
declare  @OrderIDFrom   varchar(13)
declare  @ToSPExcess int

fetch next from ComputeNewQaQtyCursor into @orderid,@StyleLocation,@Article,@SizeCode,@OrderIDFrom,@ToSPExcess
while (@@FETCH_STATUS = 0)
begin
	
	
	--依外層條件抓出SewingOutput_Detail_Detail_Garment再依序扣除對應數量
	declare cur_SDDG cursor for
		select	ID,SewingOutput_DetailUKey,QAQty
			from	SewingOutput_Detail_Detail_Garment
			where	orderid = @orderid and 
					ComboType = @StyleLocation and
					Article = @Article and
					SizeCode = @SizeCode and
					OrderIDfrom = @OrderIDfrom

	
	open cur_SDDG
	declare @ID	varchar(13)
	declare @SewingOutput_DetailUKey bigint
	declare @QAQty int

	fetch	next from cur_SDDG into @ID,@SewingOutput_DetailUKey,@QAQty
	while (@@FETCH_STATUS = 0)
	begin
		set @ToSPExcess =   @ToSPExcess - @QAQty
		--可扣除數量被扣完了，就調出cursor繼續作下一輪
		if(@ToSPExcess <= 0)
		begin
			--@ToSPExcess是負的表示沒被扣完所以用絕對值存入剩餘數量，等於0表是分完所以也跳出
			insert into #tmpUpd(ID,SewingOutput_DetailUKey,OrderId,ComboType,Article,SizeCode,OrderIDFrom,NewQAQty,Updstatus)
				values(@ID,@SewingOutput_DetailUKey,@OrderId,@StyleLocation,@Article,@SizeCode,@OrderIDFrom,ABS(@ToSPExcess),iif(@ToSPExcess=0,'D','U'))
			BREAK
		end
		else
		begin
			----@ToSPExcess大於0 表是數量還未分完繼續作
			insert into #tmpUpd(ID,SewingOutput_DetailUKey,OrderId,ComboType,Article,SizeCode,OrderIDFrom,NewQAQty,Updstatus)
				values(@ID,@SewingOutput_DetailUKey,@OrderId,@StyleLocation,@Article,@SizeCode,@OrderIDFrom,0,'D')
		end

	fetch	next from cur_SDDG into @ID,@SewingOutput_DetailUKey,@QAQty
	end
	close cur_SDDG
	Deallocate cur_SDDG


	fetch next from ComputeNewQaQtyCursor into @ID,@StyleLocation,@Article,@SizeCode,@OrderIDFrom,@ToSPExcess
end;
close ComputeNewQaQtyCursor
Deallocate ComputeNewQaQtyCursor

--用#tmpUpd將分配扣除完的結果 update 回SewingOutput_Detail_Detail_Garment
update soddG
set soddG.QAQty = tmp.NewQAQty
from SewingOutput_Detail_Detail_Garment soddG
inner join #tmpUpd tmp on soddG.id = tmp.id
						  and soddG.SewingOutput_DetailUKey = tmp.SewingOutput_DetailUKey
						  and soddG.OrderId = tmp.OrderId
						  and soddG.ComboType = tmp.ComboType
						  and soddG.Article = tmp.Article
						  and soddG.SizeCode = tmp.SizeCode
						  and soddG.OrderIDFrom = tmp.OrderIDFrom 
						  and tmp.Updstatus = 'U'

--若分配完為0則delete掉
delete soddG
from SewingOutput_Detail_Detail_Garment soddG
inner join #tmpUpd tmp on soddG.id = tmp.id
						  and soddG.SewingOutput_DetailUKey = tmp.SewingOutput_DetailUKey
						  and soddG.OrderId = tmp.OrderId
						  and soddG.ComboType = tmp.ComboType
						  and soddG.Article = tmp.Article
						  and soddG.SizeCode = tmp.SizeCode
						  and soddG.OrderIDFrom = tmp.OrderIDFrom 
						  and tmp.Updstatus = 'D'



--Update & Delete : SewingOutput_Detail_Detail
select distinct ID,SewingOutput_DetailUKey,OrderId,ComboType,Article,SizeCode into #tmp2 from #tmpUpd

update sodd
set sodd.QAQty = NewQaQty.value
from SewingOutput_Detail_Detail sodd
inner join #tmp2 tmpD on			sodd.id = tmpD.ID
									and sodd.SewingOutput_DetailUKey = tmpD.SewingOutput_DetailUKey 
		   							and sodd.orderid = tmpd.orderid
		   							and sodd.combotype = tmpd.combotype
		   							and sodd.article = tmpd.article
		   							and sodd.SizeCode = tmpd.SizeCode
outer apply (
	select value = isnull (sum (soddG.QaQty), 0)
	from SewingOutput_Detail_Detail_Garment soddG
	inner join #tmp2 tmpD on sodd.id = tmpD.ID
										and soddG.SewingOutput_DetailUKey = tmpD.SewingOutput_DetailUKey 
		   								and soddG.OrderId = tmpD.OrderId
		   								and soddG.ComboType = tmpD.ComboType
		   								and soddG.Article = tmpD.Article
		   								and soddG.SizeCode = tmpd.SizeCode
	where   sodd.ID = tmpD.ID
			and sodd.SewingOutput_DetailUKey = tmpD.SewingOutput_DetailUKey
		   	and sodd.OrderId = tmpD.OrderId
		   	and sodd.ComboType = tmpD.ComboType
		   	and sodd.Article = tmpD.Article
		   	and sodd.SizeCode = tmpd.SizeCode
) NewQaQty
where isnull (NewQaQty.value, 0) > 0

delete sodd
from SewingOutput_Detail_Detail sodd
inner join #tmp2 tmpD on			sodd.id = tmpD.ID
									and sodd.SewingOutput_DetailUKey = tmpD.SewingOutput_DetailUKey 
		   							and sodd.orderid = tmpd.orderid
		   							and sodd.combotype = tmpd.combotype
		   							and sodd.article = tmpd.article
		   							and sodd.SizeCode = tmpd.SizeCode
outer apply (
	select value = isnull (sum (soddG.QaQty), 0)
	from SewingOutput_Detail_Detail_Garment soddG
	inner join #tmp2 tmpD on sodd.id = tmpD.ID
										and soddG.SewingOutput_DetailUKey = tmpD.SewingOutput_DetailUKey 
		   								and soddG.OrderId = tmpD.OrderId
		   								and soddG.ComboType = tmpD.ComboType
		   								and soddG.Article = tmpD.Article
		   								and soddG.SizeCode = tmpd.SizeCode
	where   sodd.ID = tmpD.ID
			and sodd.SewingOutput_DetailUKey = tmpD.SewingOutput_DetailUKey
		   	and sodd.OrderId = tmpD.OrderId
		   	and sodd.ComboType = tmpD.ComboType
		   	and sodd.Article = tmpD.Article
		   	and sodd.SizeCode = tmpd.SizeCode
) NewQaQty
where isnull (NewQaQty.value, 0) = 0

---- Update & Delete : SewingOutput_Detail ---------------------------------------------------------------------------------------
select distinct SewingOutput_DetailUKey into #tmp3 from #tmp2

update sod
set sod.QAQty = NewQaQty.value
	, sod.InlineQty = NewQaQty.value
from SewingOutput_Detail sod
inner join #tmp3 tmpD on sod.UKey = tmpD.SewingOutput_DetailUKey
outer apply (
	select value = isnull (sum (sodd.QaQty), 0)
	from SewingOutput_Detail_Detail sodd
	inner join #tmp3 tmpD on sod.UKey = tmpD.SewingOutput_DetailUKey
	where sodd.SewingOutput_DetailUKey = sod.UKey
) NewQaQty
where isnull (NewQaQty.value, 0) > 0

delete sod
from SewingOutput_Detail sod
inner join #tmp3 tmpD on sod.UKey = tmpD.SewingOutput_DetailUKey
outer apply (
	select value = isnull (sum (sodd.QaQty), 0)
	from SewingOutput_Detail_Detail sodd
	inner join #tmp3 tmpD on sod.UKey = tmpD.SewingOutput_DetailUKey
	where sodd.SewingOutput_DetailUKey = sod.UKey
) NewQaQty
where isnull (NewQaQty.value, 0) = 0



--update workHour------------------------------------------
select distinct ID
into #SewingID
from #tmp2


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
group by soddG.ID, soddG.OrderId, soddG.ComboType, soddG.Article, soddG.OrderIDfrom, sod.TMS, soddG.SewingOutput_DetailUKey, AllAllot.value

/*
 * 重新計算子單 WorkHour
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
	declare @ComboType varchar (2);

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

--update SewingOutput.LockDate 讓更新資料傳回台北
update SewingOutput set LockDate = null, ReDailyTransferDate = GETDATE() where ID IN (SELECT ID FROM #SewingID)

DROP TABLE #tmpUpd;
DROP TABLE #tmp2;
DROP TABLE #tmp3;
DROP TABLE #SewingID;
DROP TABLE #updateChild;
DROP TABLE #Child;");
            #endregion

            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                boolResult = MyUtility.Tool.ProcessWithDatatable(dtSelectData, null, strSqlCmd.ToString(), out dtOrdersReceive);
                if (!boolResult)
                {
                    transactionscope.Dispose();
                    MyUtility.Msg.WarningBox(boolResult.ToString());
                }

                #region

                // #region Check 母單 PackingList = Confirmed，必須保證拆單後，母單剩餘數量大於 Packing 數量
                //                strSqlCmd = @"
                //-- Check 母單 PackingList = Confirmed，必須保證拆單後，母單剩餘數量大於 Packing 數量 --
                //-- checkValue => Packing > Sewing = 0 數量不足
                //--                 Packing <= Sewing = 1 數量足夠
                // declare @PackingTtlQty int = 0;
                // declare @SewingTtlQty int = 0;

                // select   @PackingTtlQty = isnull (sum (pld.ShipQty), 0)
                // from PackingList pl
                // inner join PackingList_Detail pld on pl.ID = pld.ID
                // where    pld.OrderID = @FromOrderID
                // and pl.Status = 'Confirmed'

                // select   @SewingTtlQty = isnull (sum (sodd.QAQty), 0)
                // from PackingList pl
                // inner join PackingList_Detail pld on pl.ID = pld.ID
                // inner join SewingOutput_Detail sod on pl.OrderID = sod.OrderId
                // inner join SewingOutput_Detail_Detail sodd on sod.UKey = sodd.SewingOutput_DetailUKey
                // and pld.Article = sodd.Article
                // and pld.SizeCode = sodd.SizeCode
                // where    pl.OrderID = @FromOrderID
                // and pl.Status = 'Confirmed'

                // select checkValue = iif (@SewingTtlQty >= @PackingTtlQty, 1
                // , 0)";
                //                #endregion
                //                if (MyUtility.GetValue.Lookup(strSqlCmd, listSqlPara).Equals("0"))
                //                {
                //                    transactionscope.Dispose();
                //                    MyUtility.Msg.WarningBox(string.Format("From SP {0} Sewing Output Qty can't less then PackingList Qty.", FromOrderID));
                //                    return false;
                //                }

                // #region Check 拆單後的數量，總數不得超過子單 Order_Qty
                //                strSqlCmd = @"
                //-- Check 拆單後的數量，總數不得超過子單 Order_Qty --
                // select   *
                // from Order_Qty oq
                // cross apply (
                // select value = MIN(value)
                // from (
                // select value = sum(sodd.QaQty)
                // from SewingOutput_Detail_Detail sodd
                // where oq.Article = sodd.Article
                // and oq.SizeCode = sodd.SizeCode
                // and oq.ID = sodd.OrderId
                // group by ComboType
                // )x
                // ) SewingQty
                // where    oq.id = @ToOrderID
                // and oq.Qty < SewingQty.value";
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
        }

        private void FindNow()
        {
            #region SQL Parameter
            List<SqlParameter> listSqlParameter = new List<SqlParameter>();
            listSqlParameter.Add(new SqlParameter("@Factory", Sci.Env.User.Factory));
            #endregion
            #region SQL Command
            string strSqlCmd = $@"
select *,
       --若同ID,StyleLocation,Article,SizeCode按數量由小到大累計加總，若加總數超過扣除sewing out總數還大於packing confirm數量才可以作扣除作業
       do_flag = iif((ToSPAllSW_Out - sum_ToSPExcess) >= ToSPConfirmPK,'Y','N')
from (
select	 ID = OQG.ID
		, StyleLocation = SL.Location
		, OrderIDFrom = OQG.OrderIDFrom
		, Article = OQG.Article
		, SizeCode = OQG.SizeCode
		, ToSPQty = ToSPQty.value
		, ToSPAllocatedQty = ToSPAllocatedQty.value
		, ToSPExcess = ToSPExcess.value
		, ToSPConfirmPK = ToSPConfirmPK.value
		, ToSPAllSW_Out = ToSPAllSW_Out.value
        , ReduceConfirmPK = ReduceConfirmPK.value
        --累計加總ToSPExcess(超出數量)
        , sum_ToSPExcess =  sum(ToSPExcess.value) over (partition by OQG.ID,SL.Location,OQG.Article,OQG.SizeCode order by  ToSPExcess.value asc)
From Order_Qty_Garment OQG
Inner join Orders O on OQG.id= O.id
Inner join Factory F on O.FactoryID=F.ID
Inner join Style_Location SL on O.styleukey=SL.Styleukey 
outer apply (
	select value = IIF(OQG.Junk=0,isnull (OQG.Qty, 0),0) 
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
    select value = ToSPAllocatedQty.value - ToSPQty.value
) ToSPExcess
outer apply(
    select value = Isnull(Sum(b.shipqty),0) 
		from PackingList a
		inner join PackingList_Detail b on a.ID=b.ID
		where b.OrderID= OQG.ID
		      and b.Article = OQG.Article 
                and b.SizeCode = OQG.SizeCode
		      and a.Status= 'Confirmed'
) ToSPConfirmPK
outer apply(
    select value = Isnull(Sum(SewingOutput_Detail_Detail.QAQty),0)
        from    SewingOutput_Detail_Detail
        Where   SewingOutput_Detail_Detail.OrderId = OQG.ID
        		and SewingOutput_Detail_Detail.ComboType = SL.Location
        		and SewingOutput_Detail_Detail.Article = OQG.Article
                and SewingOutput_Detail_Detail.SizeCode = OQG.SizeCode 
) ToSPAllSW_Out
outer apply(
    select value = IIF(ToSPAllSW_Out.value - ToSPConfirmPK.value < ToSPExcess.value , 'Y' ,'N')
) ReduceConfirmPK
Where ToSPAllocatedQty.value > ToSPQty.value
 And O.FtyGroup =  @Factory  ) fin_result";
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
