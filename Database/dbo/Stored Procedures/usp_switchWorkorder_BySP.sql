﻿

CREATE PROCEDURE [dbo].[usp_switchWorkorder_BySP]
	(
	 @WorkType  varChar(1)=2,--By SP = 2
	 @Cuttingid  varChar(13),
	 @mDivisionid varchar(8),
	 @username varchar(10)
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
SET NOCOUNT ON;
update cutting set WorkType =2 where id = @Cuttingid

Declare @POID varchar(13) 
Declare @FactoryID varchar(8) 
Select distinct @POID = POID,@FactoryID=FactoryID From Orders  WITH (NOLOCK) Where Cuttingsp = @Cuttingid and junk=0

select *,Order_EachConsUkey = 0 into #tmp_WorkOrder_Distribute from [WorkOrder_Distribute] where 1=0
alter table #tmp_WorkOrder_Distribute add colorid varchar(6)
alter table #tmp_WorkOrder_Distribute add newKey int
select *,newKey=0 into #tmp_Workorder from WorkOrder where 1=0
select *,newKey=0 into #tmp_WorkOrder_SizeRatio from WorkOrder_SizeRatio where 1=0
select *,newKey=0 into #tmp_WorkOrder_PatternPanel from WorkOrder_PatternPanel where 1=0
--主要資料
Select MixedSizeMarker,	oe.id,	[FactoryID] = @FactoryID,	[MDivisionid] = @MDivisionid,
	[Seq1] = isnull(iif(isnull(s.SEQ2,'')='',s2.seq1,s.SEQ1),''),--若SEQ2 為空就找70大項
	[seq2] = isnull(iif(isnull(s.SEQ2,'')='',s2.seq2,s.SEQ2),''),--若SEQ2 為空就找70大項
	--@orderid	--[Layer]--[Cons] = Layer * @SizeRatioQty * oe.ConsPC--迴圈帶值塞入
	oec.ColorID,	oe.MarkerName,	oe.MarkerLength,	oe.ConsPC,	
	f.Refno,	ob.SCIRefno,	oe.MarkerNo,	oe.MarkerVersion,	[type]=1,--1是by System, 2是手動
	[AddName] = @username,	[AddDate]=GETDATE(),	oe.MarkerDownloadID,	oe.FabricCombo,	oe.FabricCode,	oe.FabricPanelCode,
	[Order_EachConsUkey] = oe.Ukey,
	-----------------------------
	oec.Orderqty,	[ThisMarkerColor_Layer] = oec.Layer,
	[ThisMarkerColor_MaxLayer]=iif(isnull(L.CuttingLayer,0)=0,100,L.CuttingLayer)
	,IDENTITY(int,1,1) as Rowid 
inTo #WorkOrderMix 
From Order_EachCons oe WITH (NOLOCK)
inner join Order_Eachcons_Color oec WITH (NOLOCK) on oec.Order_EachConsUkey = oe.Ukey
inner join Order_BoF ob WITH (NOLOCK) on ob.Id = oe.id and ob.FabricCode = oe.FabricCode
Left Join Fabric f WITH (NOLOCK) on f.SCIRefno = ob.SCIRefno
outer apply(Select c.CuttingLayer  From Construction c WITH (NOLOCK) where c.id = f.ConstructionID)L
outer apply
(
	select top 1 seq1,seq2 
	from PO_Supp_Detail psd with(nolock) 
	where id = @POID and psd.Scirefno = ob.SCIRefno and Junk = 0 AND Colorid = oec.ColorID and psd.seq1 = ob.Seq1
	and psd.OutputSeq1='' and psd.OutputSeq2 = ''
	order by seq2 desc--依據原本的235行-SEQ1SEQ2規則
)s
outer apply
(
	select top 1 seq1,seq2 --=iif(counts2.cts>1,'',seq2)--兩筆以上的70大項就不填小項seq2
	from PO_Supp_Detail psd with(nolock) 
	--outer apply
	--(
	--	select cts = count(1) 
	--	from PO_Supp_Detail pin with(nolock) 
	--	where id=@POID and pin.Scirefno=ob.SCIRefno and Junk = 0 AND Colorid = oec.ColorID and pin.seq1 like '7%' and pin.OutputSeq2 != ''
	--)counts2
	where id = @POID and psd.Scirefno = ob.SCIRefno and Junk = 0 AND Colorid = oec.ColorID and psd.seq1 like '7%' and psd.OutputSeq2 != ''
)s2
outer apply(select top 1 A=0 from Order_EachCons_Article  WITH (NOLOCK) where Order_EachConsUkey=oe.Ukey)hasforArticle--排序用,有ForArticle排前
Where oe.id = @Cuttingid and oe.CuttingPiece = 0--測試用--and colorid = 'BLK'and FabricCombo = 'FA'and MarkerName = 'MAB9'
order by isnull(hasforArticle.A,1),MixedSizeMarker desc,MarkerName
--準備inline和qty分配資料
select Inline = Min(s.Inline),[orderid] = o.id,oq.Article,occ.ColorID,oq.SizeCode,occ.PatternPanel,[Size_orderqty] = oq.qty,IDENTITY(int,1,1) as identRowid
,InlineForOrderby = isnull(Min(s.Inline),'9999/12/31')
into #_tmpdisQty
from Orders o WITH (NOLOCK)
inner join Order_Qty oq WITH (NOLOCK) on oq.id = o.id
inner join Order_ColorCombo occ WITH (NOLOCK) on occ.id = o.POID and occ.FabricCode is not null and occ.FabricCode !='' and occ.Article = oq.Article
inner join Order_EachCons oe WITH (NOLOCK) on oe.id = occ.id and oe.FabricCombo = occ.PatternPanel and oe.CuttingPiece = 0
outer apply
(
	select inline = min(ss.inline)
	from SewingSchedule ss WITH (NOLOCK) 
	inner join SewingSchedule_Detail ssd WITH (NOLOCK) on ss.ID = ssd.ID 
	where ss.OrderID = o.id and ss.MDivisionID = o.MDivisionID and ssd.Article = oq.Article and oq.SizeCode = ssd.SizeCode and occ.Article = ssd.Article
)s
where o.CuttingSP = @Cuttingid and o.MDivisionID = @MDivisionID and o.junk = 0
group by s.Inline,o.id,oq.Article,occ.ColorID,oq.SizeCode,occ.PatternPanel,oq.qty 
order by InlineForOrderby,o.id
--主要資料參數
Declare @MixedSizeMarker varchar(2),@id varchar(13),@SizeCode varchar(8),@FirstSizeCode varchar(8),@colorid varchar(6),
@Seq1 varchar(3),@Seq2 varchar(3),@MarkerName varchar(20),@MarkerLength varchar(15),@ConsPC numeric(6,4),@Refno varchar(20),@SCIRefno varchar(30),
@MarkerNo varchar(10),@MarkerVersion varchar(3),@type int,@AddDate datetime,
@MarkerDownloadID varchar(25),@FabricCombo varchar(2),@FabricCode varchar(3),@FabricPanelCode varchar(2),@Order_EachConsUkey bigint,
@Orderqty int,@ThisMarkerColor_Layer int,@ThisMarkerColor_MaxLayer int,@rowid int,@FirstRatio int,@SizeRatio int, @tmpUkey int = 0, @tmpUkey2 int = 0
--主要資料迴圈
DECLARE CURSOR_WorkOrder CURSOR FOR select * from #WorkOrderMix order by Rowid
OPEN CURSOR_WorkOrder
FETCH NEXT FROM CURSOR_WorkOrder INTO @MixedSizeMarker,@id,@FactoryID,@MDivisionid,@Seq1,@Seq2,@ColorID,@MarkerName,@MarkerLength,@ConsPC,@Refno,@SCIRefno,@MarkerNo,@MarkerVersion,@type,@username,@AddDate,
@MarkerDownloadID,@FabricCombo,@FabricCode,@FabricPanelCode,@Order_EachConsUkey,@Orderqty,@ThisMarkerColor_Layer,@ThisMarkerColor_MaxLayer,@rowid
While @@FETCH_STATUS = 0
Begin	
	select top 1 @FirstSizeCode=SizeCode,@FirstRatio=qty From Order_EachCons_SizeQty WITH(NOLOCK) Where Order_EachConsUkey=@Order_EachConsUkey order by Qty desc
	DECLARE Size CURSOR FOR Select SizeCode,qty	From Order_EachCons_SizeQty WITH(NOLOCK) Where Order_EachConsUkey = @Order_EachConsUkey order by Qty desc	
	OPEN Size
	FETCH NEXT FROM Size INTO @SizeCode,@SizeRatio
	While @@FETCH_STATUS = 0
	Begin
		--參數
		Declare @ThisTotalCutQty int,@ThisLayerCount int,@mQty float,@ForSameID_AccuQty int,@ForSameID_Qty int,@maxLayerQty int,
		@nextQty int = 0,@bQty int,@OldOrderID varchar(13)='',@ThisCutlayer int,@thiscutQty float,@Drowid int,@nextQtyC int,
		@OldArticle  varchar(8),@identRowid int,@Order_Eachcons_ColorUkey int,
		@orderid varchar(13),@Article varchar(8),@Size_orderqty float--要塞的值
		----找到此marker color size實際要切的總數量
		select @ThisTotalCutQty = sum(oeca.Orderqty) 
		from Order_EachCons_Color_Article oeca WITH (NOLOCK)
		inner join Order_EachCons_color oec WITH (NOLOCK) on oec.ukey = oeca.Order_EachCons_ColorUkey
		where oeca.colorid = @colorid and oeca.SizeCode = @sizecode and oec.Order_EachConsUkey = @Order_EachConsUkey
		--inline和qty分配表帶入條件	
		select [SizeRatio]=oes.Qty,	b.identRowid,a.id,b.orderid,b.Article,b.SizeCode,b.Size_orderqty,a.ThisMarkerColor_MaxLayer,Rowid = IDENTITY(int,1,1) 
		into #DistributeSource
		from #WorkOrderMix a
		inner join #_tmpdisQty b on a.ColorID = b.ColorID and a.FabricCombo = b.PatternPanel
		inner join Order_EachCons_SizeQty oes on  oes.id = a.id and oes.Order_EachConsUkey = a.Order_EachConsUkey and b.SizeCode = oes.SizeCode
		outer apply(select top 1 A=0 from Order_EachCons_Article WITH (NOLOCK) where Order_EachConsUkey=a.Order_EachConsUkey)hasforArticle
		outer apply(select Article from Order_EachCons_Article  WITH (NOLOCK) where Order_EachConsUkey=a.Order_EachConsUkey)forArticle
		where (b.Article in (forArticle.Article) or hasforArticle.A is null)--有forArticle已forArticle為主
		and a.MarkerName = @MarkerName and a.ColorID = @colorid and a.FabricCombo = @FabricCombo and b.SizeCode = @sizecode
		order by identRowid
		if (select sum(Size_orderqty) from #DistributeSource) < @ThisTotalCutQty
		begin
			set @ThisTotalCutQty = (select sum(Size_orderqty) from #DistributeSource)
		end
		--迴圈
		DECLARE Distribute CURSOR FOR select * from #DistributeSource order by Rowid
		IF @FirstSizeCode = @sizecode
		begin
		OPEN Distribute
		FETCH NEXT FROM Distribute INTO @SizeRatio,@identRowid,@id,@orderid,@Article,@SizeCode,@Size_orderqty,@ThisMarkerColor_MaxLayer,@Drowid
		While @@FETCH_STATUS = 0
		Begin
			set @maxLayerQty = @ThisMarkerColor_MaxLayer * @SizeRatio
			--與前一筆orderid相同要分在同一筆worker，orderid不同才ukey加1
			if @OldOrderID != @orderid or(@OldOrderID = @orderid and @OldArticle != @Article)
			Begin
				if @nextQty = 0
				Begin
					set @tmpUkey = @tmpUkey + 1			
				End
				set @ForSameID_AccuQty = @Size_orderqty
				set @ForSameID_Qty = @maxLayerQty
			End
			else
			Begin
				if @Size_orderqty + @mQty <= @maxLayerQty
				Begin
					set @ForSameID_AccuQty = @Size_orderqty
				End
				else
				Begin
					set @ForSameID_AccuQty = @Size_orderqty + @mQty
				End
				set @ForSameID_Qty = @maxLayerQty - @mQty
			End
			if @ForSameID_AccuQty <@maxLayerQty
			Begin
				set @mQty = @ForSameID_AccuQty % @maxLayerQty
			end
			else
			Begin
				set @mQty = (@ForSameID_AccuQty-@nextQty) % @maxLayerQty
			end

			if (@nextQty > 0 and @OldOrderID != @orderid)or(@nextQty > 0 and @OldOrderID = @orderid and @OldArticle!=@Article)
			Begin
				select @nextQtyC = min(i)from(select i= @ThisTotalCutQty union all select @nextQty union all select @mQty)i
				insert into #tmp_WorkOrder_Distribute values(0,@id,@orderid,@Article,@SizeCode,@nextQtyC,@Order_EachConsUkey,@colorid,@tmpUkey)
				update #_tmpdisQty set [Size_orderqty] = [Size_orderqty] - @nextQtyC where identRowid = @identRowid--減去使用的
				set @ThisTotalCutQty = @ThisTotalCutQty - @nextQtyC
				if @mQty > 0 and @ForSameID_AccuQty <@maxLayerQty
				Begin
					set @nextQty = @nextQty - @nextQtyC
					set @mQty = @mQty - @nextQtyC
				End

				if @nextQty = 0 and @mQty >0 or(@ForSameID_AccuQty >=@maxLayerQty)
				begin
					set @tmpUkey = @tmpUkey + 1
				end
			End

			
			if @ThisTotalCutQty < @ForSameID_AccuQty
			begin
				set @ForSameID_AccuQty = @ThisTotalCutQty
			end
			set @ThisLayerCount = @ForSameID_AccuQty/@SizeRatio/@ThisMarkerColor_MaxLayer--先判斷此次Layer是否大於MaxLayer
			while @ThisLayerCount > 0
			begin
				if @ThisTotalCutQty < @ForSameID_Qty
				begin
					set @ForSameID_Qty = @ThisTotalCutQty
				end
				insert into #tmp_WorkOrder_Distribute values(0,@id,@orderid,@Article,@SizeCode,@ForSameID_Qty,@Order_EachConsUkey,@colorid,@tmpUkey)
				update #_tmpdisQty set [Size_orderqty] = [Size_orderqty] - @ForSameID_Qty where identRowid = @identRowid--減去使用的
				set @ThisTotalCutQty = @ThisTotalCutQty - @ForSameID_Qty
				set @tmpUkey = @tmpUkey + 1
				set @ThisLayerCount = @ThisLayerCount - 1
			end

			if @ThisTotalCutQty < @mQty
			begin
				set @mQty = @ThisTotalCutQty
			end
			if @mQty > 0
			Begin
				insert into #tmp_WorkOrder_Distribute values(0,@id,@orderid,@Article,@SizeCode,@mQty,@Order_EachConsUkey,@colorid,@tmpUkey)
				update #_tmpdisQty set [Size_orderqty] = [Size_orderqty] - @mQty where identRowid = @identRowid--減去使用的
				set @ThisTotalCutQty = @ThisTotalCutQty - @mQty
				if @OldOrderID != @orderid or(@OldOrderID = @orderid and @OldArticle != @Article)
				Begin
					set @nextQty = ceiling(@mQty / @SizeRatio) * @SizeRatio - @mQty
				End
				else
				Begin
					if @mQty > @SizeRatio
					begin
						set @nextQty = @nextQty + ceiling(@mQty / @SizeRatio) * @SizeRatio - @mQty
					end
				End
			end

			Delete #_tmpdisQty where identRowid = @identRowid and [Size_orderqty] = 0--把使用完的刪除
			set @OldOrderID = @orderid
			set @OldArticle = @Article
			if @ThisTotalCutQty = 0--處理有否EXCESS
			Begin
				if @nextQty > 0
				begin
					insert into #tmp_WorkOrder_Distribute values(0,@id,'EXCESS','',@SizeCode,@nextQty,@Order_EachConsUkey,@colorid,@tmpUkey)			
				end
				break
			end
		FETCH NEXT FROM Distribute INTO @SizeRatio,@identRowid,@id,@orderid,@Article,@SizeCode,@Size_orderqty,@ThisMarkerColor_MaxLayer,@Drowid
		End
		CLOSE Distribute
		DEALLOCATE Distribute
		end
		else
		begin--根據第一個size分配的Layer去分配其它mixsize
			Declare @SQty float,@FLayer float
			DECLARE ukey_Layer CURSOR FOR
			select newkey,FLayer =sum(Qty)/@FirstRatio from #tmp_WorkOrder_Distribute 
			where sizecode = @FirstSizeCode and Order_EachConsUkey= @Order_EachConsUkey and colorid = @colorid group by newkey order by newkey	
			OPEN ukey_Layer
			FETCH NEXT FROM ukey_Layer INTO @tmpUkey2,@FLayer
			While @@FETCH_STATUS = 0
			Begin
				set @thiscutQty = @FLayer * @SizeRatio
				DECLARE Distribute2 CURSOR FOR select * from #DistributeSource order by Rowid
				OPEN Distribute2
				FETCH NEXT FROM Distribute2 INTO @SizeRatio,@identRowid,@id,@orderid,@Article,@SizeCode,@Size_orderqty,@ThisMarkerColor_MaxLayer,@Drowid
				While @@FETCH_STATUS = 0
				Begin
					if @thiscutQty>=@Size_orderqty
					begin
					insert into #tmp_WorkOrder_Distribute values(0,@id,@orderid,@Article,@SizeCode,@Size_orderqty,@Order_EachConsUkey,@colorid,@tmpUkey2)
						update #_tmpdisQty set [Size_orderqty] = [Size_orderqty] - @Size_orderqty where identRowid = @identRowid--減去使用的
						update #DistributeSource set [Size_orderqty] = [Size_orderqty] - @Size_orderqty where rowid = @Drowid--減去使用的
						set @thiscutQty = @thiscutQty - @Size_orderqty
					end
					else
					begin
						insert into #tmp_WorkOrder_Distribute values(0,@id,@orderid,@Article,@SizeCode,@thiscutQty,@Order_EachConsUkey,@colorid,@tmpUkey2)
						update #_tmpdisQty set [Size_orderqty] = [Size_orderqty] - @thiscutQty where identRowid = @identRowid--減去使用的
						update #DistributeSource set [Size_orderqty] = [Size_orderqty] - @thiscutQty where rowid = @Drowid--減去使用的
						set @thiscutQty = 0
					end
					Delete #_tmpdisQty where identRowid = @identRowid and [Size_orderqty] = 0--把使用完的刪除
					Delete #DistributeSource where rowid = @Drowid and [Size_orderqty] = 0--把使用完的刪除
					if @thiscutQty = 0
					begin
						break
					end
				FETCH NEXT FROM Distribute2 INTO @SizeRatio,@identRowid,@id,@orderid,@Article,@SizeCode,@Size_orderqty,@ThisMarkerColor_MaxLayer,@Drowid
				End
				CLOSE Distribute2
				DEALLOCATE Distribute2
				if (select count(1) from #DistributeSource) = 0 and @thiscutQty > 0
				begin				
					insert into #tmp_WorkOrder_Distribute values(0,@id,'EXCESS','',@SizeCode,@thiscutQty,@Order_EachConsUkey,@colorid,@tmpUkey2)		
				end
			FETCH NEXT FROM ukey_Layer INTO @tmpUkey2,@FLayer
			End
			CLOSE ukey_Layer
			DEALLOCATE ukey_Layer
		end
		drop table #DistributeSource
		IF CURSOR_STATUS('global','Distribute')>=-1
		BEGIN
		DEALLOCATE Distribute
		END
	FETCH NEXT FROM Size INTO @SizeCode,@SizeRatio
	End
	CLOSE Size
	DEALLOCATE Size

	--其它Table
	DECLARE @Cons float
	DECLARE insertWorkorder CURSOR FOR
	select newkey,FLayer =sum(Qty)/@FirstRatio from #tmp_WorkOrder_Distribute 
	where sizecode = @FirstSizeCode and Order_EachConsUkey = @Order_EachConsUkey and colorid = @colorid group by newkey order by newkey
	OPEN insertWorkorder
	FETCH NEXT FROM insertWorkorder INTO @tmpUkey2,@FLayer
	While @@FETCH_STATUS = 0
	Begin
		select top 1 @orderid = orderid from #tmp_WorkOrder_Distribute 
		where sizecode = @FirstSizeCode and Order_EachConsUkey = @Order_EachConsUkey and colorid = @colorid and newkey = @tmpUkey2 order by orderid
		set @Cons = @FLayer * @FirstRatio * @ConsPC
		Insert Into #tmp_Workorder(ID,FactoryID,MDivisionid,SEQ1,SEQ2,OrderID,Layer,Colorid,MarkerName,MarkerLength,ConsPC,Cons,Refno,SCIRefno,
		Markerno,MarkerVersion,Type,AddName,AddDate,MarkerDownLoadId,FabricCombo,FabricCode,FabricPanelCode,newKey,Order_eachconsUkey)
		values(@id,@FactoryID,@MDivisionid,@Seq1,@Seq2,@orderid,@FLayer,@ColorID,@MarkerName,@MarkerLength,@ConsPC,@Cons,@Refno,@SCIRefno,
		@MarkerNo,@MarkerVersion,@type,@username,@AddDate,@MarkerDownloadID,@FabricCombo,@FabricCode,@FabricPanelCode,@tmpUkey2,@Order_EachConsUkey)
		--SizeRatio
		DECLARE Size CURSOR FOR Select SizeCode,qty	From Order_EachCons_SizeQty WITH (NOLOCK) Where Order_EachConsUkey = @Order_EachConsUkey order by Qty desc	
		OPEN Size
		FETCH NEXT FROM Size INTO @SizeCode,@SizeRatio
		While @@FETCH_STATUS = 0
		Begin
			insert into #tmp_WorkOrder_SizeRatio values(0,@id,@SizeCode,@SizeRatio,@tmpUkey2)		
		FETCH NEXT FROM Size INTO @SizeCode,@SizeRatio
		End
		CLOSE Size
		DEALLOCATE Size
		--WorkOrder_PatternPanel
		INSERT INTO #tmp_WorkOrder_PatternPanel values(@id,0,@FabricCombo,@FabricPanelCode,@tmpUkey2)
	FETCH NEXT FROM insertWorkorder INTO @tmpUkey2,@FLayer
	End
	CLOSE insertWorkorder
	DEALLOCATE insertWorkorder

FETCH NEXT FROM CURSOR_WorkOrder INTO @MixedSizeMarker,@id,@FactoryID,@MDivisionid,@Seq1,@Seq2,@ColorID,@MarkerName,@MarkerLength,@ConsPC,@Refno,@SCIRefno,@MarkerNo,@MarkerVersion,@type,@username,@AddDate,
@MarkerDownloadID,@FabricCombo,@FabricCode,@FabricPanelCode,@Order_EachConsUkey,@Orderqty,@ThisMarkerColor_Layer,@ThisMarkerColor_MaxLayer,@rowid
End
CLOSE CURSOR_WorkOrder
DEALLOCATE CURSOR_WorkOrder
--select * from #tmp_Workorder order by newkey
--select newkey,orderid,Article,sizecode from #tmp_WorkOrder_Distribute group by newkey,orderid,Article,sizecode having count(1)>1
--select * from #tmp_WorkOrder_Distribute order by newkey
Declare @insertRow int
Declare @insertcount int
Declare @iden bigint
Select @insertcount = Max(newkey) From #tmp_Workorder

DECLARE insertALL CURSOR FOR select newkey from #tmp_Workorder order by newkey
OPEN insertALL
FETCH NEXT FROM insertALL INTO @insertRow
While @@FETCH_STATUS = 0
Begin
	insert into WorkOrder(id,factoryid,MDivisionId,SEQ1,SEQ2,CutRef,OrderID,CutplanID,Cutno,Layer,Colorid,Markername,
					EstCutDate,CutCellid,MarkerLength,ConsPC,Cons,Refno,SCIRefno,MarkerNo,MarkerVersion,Type,Order_EachconsUkey,
					AddName,AddDate,FabricCombo,MarkerDownLoadId,FabricCode,FabricPanelCode)
	Select id,factoryid,MDivisionId,SEQ1,SEQ2,CutRef,OrderID,CutplanID,Cutno,Layer,Colorid,Markername,
	EstCutDate,CutCellid,MarkerLength,ConsPC,Cons,Refno,SCIRefno,MarkerNo,MarkerVersion,Type,Order_EachconsUkey,
	AddName,AddDate,FabricCombo,MarkerDownLoadId,FabricCode,FabricPanelCode 
	From #tmp_Workorder Where newkey = @insertRow
	select @iden = @@IDENTITY 
	--------將撈出的Ident 寫入----------
	update #tmp_WorkOrder_Distribute set WorkOrderUkey = @iden Where newkey = @insertRow
	update #tmp_WorkOrder_PatternPanel set WorkOrderUkey = @iden Where newkey = @insertRow
	update #tmp_WorkOrder_SizeRatio set WorkOrderUkey = @iden Where newkey = @insertRow
	------Insert into 子Table-------------
	insert into WorkOrder_Distribute(WorkOrderUkey,id,Orderid,Article,SizeCode,Qty)
		(Select WorkOrderUkey,id,Orderid,Article,SizeCode,Qty
		From #tmp_WorkOrder_Distribute Where newkey=@insertRow)
	insert into WorkOrder_PatternPanel(WorkOrderUkey,ID,FabricPanelCode,PatternPanel)
		(Select WorkOrderUkey,ID,FabricPanelCode,PatternPanel
		From #tmp_WorkOrder_PatternPanel Where newkey=@insertRow)
	insert into WorkOrder_SizeRatio(WorkOrderUkey,ID,SizeCode,Qty)
		(Select WorkOrderUkey,ID,SizeCode,Qty
		From #tmp_WorkOrder_SizeRatio Where newkey=@insertRow)
		Set @insertRow+=1
FETCH NEXT FROM insertALL INTO @insertRow
End
CLOSE insertALL
DEALLOCATE insertALL
drop table #WorkOrderMix,#_tmpdisQty,#tmp_WorkOrder_Distribute,#tmp_Workorder,#tmp_WorkOrder_SizeRatio,#tmp_WorkOrder_PatternPanel
End