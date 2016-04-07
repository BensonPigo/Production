-- Batch submitted through debugger: SQLQuery2.sql|7|0|C:\Users\SPIN~1.YAN\AppData\Local\Temp\~vsF77B.sql
-- Batch submitted through debugger: SQLQuery4.sql|7|0|C:\Users\SPIN~1.YAN\AppData\Local\Temp\~vs14CF.sql
-- Batch submitted through debugger: SQLQuery33.sql|7|0|C:\Users\SPIN~1.YAN\AppData\Local\Temp\~vs3FAA.sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================F
CREATE PROCEDURE [dbo].[usp_switchWorkorder]
	-- Add the parameters for the stored procedure here
	(
	 @WorkType  varChar(1),
	 @Cuttingid  varChar(13),
	 @mDivisionid varchar(8),
	 @username varchar(10)
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


	--將Cutting 填入WorkType
	update cutting set WorkType =@WorkType where id = @Cuttingid
    -- Insert statements for procedure here
	-- 撈出所有Cuttingid 的SP#
	--Declare @sptable Table(id varchar(13))
	Select id into #spTable 
	from Orders where cuttingsp = @Cuttingid
	--撈取最細EachCons_Color_Article
	Select a.ConsPC,a.CuttingWidth,a.FabricCode,a.FabricCombo,a.Id,a.LectraCode,a.MarkerDownloadID,
			a.MarkerLength,a.MarkerName,a.MarkerNo,a.MarkerVersion,a.Ukey,a.Width,
			b.ColorID,b.CutQty as TotalCutQty,b.Layer as TotalLayer,b.YDS,
			c.CutQty,c.SizeCode,c.Layer,c.Article,b.Order_EachConsUkey,b.Ukey as Order_EachCons_ColorUkey
	InTo #Order_EachCons_Color_Article
	From Order_EachCons a, Order_EachCons_Color b, Order_EachCons_Color_Article c 
	Where a.ukey = b.Order_EachConsUkey and b.Ukey = c.Order_EachCons_ColorUkey and a.id = b.id and a.id = c.id and b.id = c.id 
			and a.id = @Cuttingid and a.CuttingPiece = 0
	--撈Each Cons
	Select * into #Order_EachCons 
	from Order_EachCons Where id = @Cuttingid
	--撈出展開Marker OrderEachCons 且非外裁
	Select a.*,b.ColorID,b.CutQty as TotalCutQty,b.Layer as TotalLayer,b.Order_EachConsUkey,b.Orderqty,b.Ukey as Order_EachCons_ColorUkey,b.Variance,b.YDS
	into #marker1
	From #Order_EachCons a,Order_Eachcons_Color b 
	Where a.id = @Cuttingid and a.Ukey = b.Order_EachConsUkey and a.CuttingPiece = 0
	--撈BOF
	Select a.ukey as Order_EachConsUkey,b.SCIRefno,b.Seq1,b.SuppID,b.Ukey as order_BofUkey ,
	c.ConstructionID,c.Width as fabricwidth,c.Refno
	Into #Order_EachCons_BOF 
	From #Order_EachCons a, Order_BoF b
	Left Join Fabric c on c.SCIRefno = b.SCIRefno
	Where a.id = b.id and a.FabricCode = b.FabricCode
	--撈EachCon_Color_Article 找出Article對應的層數與最大層數
	Select a.* ,
	iif(isnull((Select iif(isnull(c.CuttingLayer,0)=0,100,c.CuttingLayer)  From Construction c where c.id = b.ConstructionID),0)=0,100,
	(Select iif(isnull(c.CuttingLayer,0)=0,100,c.CuttingLayer)  From Construction c where c.id = b.ConstructionID)) as MaxLayer
	Into #Order_EachCons_Color_ArticleLayer
	From #marker1 a,#Order_EachCons_BOF b 
	Where a.Ukey = b.Order_EachConsUkey


	/*
	找CuttingID 的POID
	*/
	Declare @POID varchar(13)
	SET @POID = 'Im default'
	Select distinct @POID = POID
	From Orders
	Where Cuttingsp = @Cuttingid

	---------組每個SP#的Article,Size,Qty,PatternPanel,inline
	Select distinct e.id,a.article,a.colorid,e.sizecode,a.PatternPanel,e.qty as orderqty, 0 as disqty,f.Inline
	Into #_tmpdisQty
	from Order_ColorCombo a ,Order_EachCons b ,
	(Select d.*,cuttingsp from Order_Qty d,(Select id,cuttingsp from Orders where cuttingsp = @Cuttingid) c 
		Where c.id = d.id) e
	left join 
	(Select a.inline,b.Article,b.SizeCode,a.OrderID 
	from SewingSchedule a ,SewingSchedule_Detail b ,
		(Select id from Orders where cuttingsp = @Cuttingid) c 
		where c.id = a.orderid and a.id = b.id and mDivisionid = @mDivisionid) f on f.OrderID = e.id and f.Article = e.Article and f.SizeCode = e.SizeCode 
	where a.id = @POID and a.FabricCode is not null and a.FabricCode !='' 
	and b.id = @POID and a.id = b.id and b.cuttingpiece='0' and  b.FabricCombo = a.PatternPanel and e.cuttingsp = a.id and e.Article = a.Article
	Order by inline,id
	Select id,article,sizecode,colorid,PatternPanel,orderqty, disqty,Min(INLINE) as inline,IDENTITY(int,1,1) as identRowid
	into #disQty
	From #_tmpdisQty group by id,article,sizecode,PatternPanel,orderqty, disqty,colorid order by inline
	----------------------------------------------------------------------------------
	--New WorkOrder
	Select a.*,0 as newKey InTo #NewWorkorder From Workorder a Where 1 =0
	Select a.*,0 as newKey InTo #NewWorkOrder_Distribute From WorkOrder_Distribute a Where 1 = 0
	Select a.*,0 as newKey InTo #NewWorkOrder_SizeRatio From WorkOrder_SizeRatio a Where 1 = 0
	Select a.*,0 as newKey InTo #NewWorkOrder_PatternPanel From WorkOrder_PatternPanel a Where 1 = 0
	--產生給WorkOrder的變數
	Declare @maxLayer int
	Declare @Layer int
	Declare @WorkOrderMixRowID int
	Declare @WorkOrderMixRowCount int
	Declare @Factoryid varchar(8)
	Declare @ID varchar(13)
	Declare @Seq1 varchar(3)
	Declare @Seq2 varchar(2)
	Declare @OrderID varchar(13)
	Declare @Colorid varchar(6)
	Declare @Markername varchar(5)
	Declare @CutCell varchar(2)
	Declare @MarkerLength  varchar(13)
	Declare @ConsPC numeric(6,4)
	Declare @Cons numeric(9,4)
	Declare @SCIRefno varchar(26)
	Declare @Refno varchar(20)
	Declare @MarkerNo varchar(10)
	Declare @MarkerVerion varchar(3)
	Declare @FabricCombo varchar(2)
	Declare @PatternPanel varchar(2)
	Declare @MarkerDownLoadid varchar(25)
	Declare @FabricCode varchar(3)
	Declare @LectraCode varchar(2)
	Declare @Suppid varchar(6)
	Declare @TotalLayer int
	Declare @ukey bigint
	Declare @Rowno int
	Declare @Article varchar(8)
	Declare @SizeCode varchar(8)
	Declare @distriqtyRowID int
	Declare @distriqtyRowCount int
	Declare @disQtyRowID int
	Declare @disQtyRowCount int
	Declare @CutQty int
	Declare @distributeQty int --分配DiscQty 剩餘數量
	Declare @OrderQty int
	Declare @WorkOrder_DisOrderID varchar(13)
	Declare @WorkOrder_DisQty int
	Declare @NewKey int
	Declare @WorkOrder_DisidenRow int
	Declare @WorkOrder_SizeRatioRowid int
	Declare @WorkOrder_SizeRatioRowCount int
	Declare @WorkOrder_SizeRatio_Qty numeric(6)
	Declare @WorkOrder_PatternPanel varchar(2)
	Declare @WorkOrder_LectraCode varchar(2)
	Declare @SizeRatioQty numeric(6)
	Declare @Type varchar(1)
	Declare @modLayer int --層數的餘數
	Declare @LayerCount int --層數的筆數
	Declare @Layernum int --第幾個層數
	Declare @Layer_Cutqty numeric(5)
	Declare @Order_EachConsUkey bigint
	Declare @Order_EachCons_ColorUkey bigint
	Declare @dislayerRowid int
	Declare @dislayerRowCount int
	Declare @distriqty_byspRowID int
	Declare @disQty_byspRowID int
	Declare @sizeQtyRowid int
	Declare @sizeQtyRowCount int
	Declare @distOrderRowid int
	Declare @distOrderRowCount int
	Declare @distOrderRowid_again int
	Declare @distOrderRowCount_again int
	Declare @disQty_again numeric(6)
	Declare @BalQty numeric(6)
	Declare @OrderLayer int
	Declare @SizeQty numeric(6)
	Declare @linsert bit
	Declare @sizeQtyRowid_again int
	Declare @sizeQtyRowCount_again int 
	---

	/*
	先用多尺碼MixedSizeMarker = 2
	*/
	Begin
		Select a.*,IDENTITY(int,1,1) as Rowid 
		InTo #WorkOrderMix 
		From #Order_EachCons_Color_ArticleLayer a order by MixedSizeMarker desc,MarkerName
		--------------------Factory-------------------------------------
		Select @Factoryid = Factoryid From Orders Where ID = @Cuttingid
		--------------------Loop Start @CuttingCombo--------------------
		Set @WorkOrderMixRowID = 1
		SET @NewKey = 1
		Select @WorkOrderMixRowID = Min(RowID), @WorkOrderMixRowCount = Max(RowID) From #WorkOrderMix
		While @WorkOrderMixRowID <= @WorkOrderMixRowCount
		Begin
			Select @maxLayer = MaxLayer,
				   @ID = id,
				   @Colorid = colorid,
				   @MarkerName = markername,
				   @MarkerLength = MarkerLength,
				   @ConsPc = ConsPc,
				   @Markerno = markerno,
				   @FabricCombo = FabricCombo,
				   @FabricCode = FabricCode,
				   @LectraCode = LectraCode,
				   @TotalLayer = TotalLayer,
				   @MarkerVerion = MarkerVersion,
				   @ukey = ukey,
				   @Type = '1',
				   @MarkerDownLoadId = MarkerDownloadID,
				   @Order_EachConsUkey = Order_EachConsUkey

			From #WorkOrderMix
			Where RowID = @WorkOrderMixRowID;

			SET @SCIRefno = ''

			--找尋SCIRefno 資料
			Select @SCIRefno = SCIRefno,
				   @Refno = Refno,
				   @Seq1 = SEQ1,
				   @Suppid = SuppID
			From #Order_EachCons_BOF
			Where Order_EachConsUkey = @ukey
			-------------------------------------
			------------SEQ1,SEQ2----------------
			Select @Seq2 = isnull(seq2,'') --先找相同SEQ1,SCIRefno
			From PO_Supp_Detail b 
			Where id = @POID AND SEQ1 = @SEQ1 AND Scirefno = @SCIRefno and OutputSeq1='' and OutputSeq2 = ''
			if @Seq2 = ''
			Begin
				--若SEQ2 為空就找70大項
				Select *
				into #SEQ2tmp
				From PO_Supp_Detail b 
				Where id = @POID AND Scirefno = @SCIRefno and OutputSeq1=@SEQ1 and OutputSeq2 != '' AND Colorid = @colorid
				SET @Rowno = @@Rowcount
				if @Rowno=1 --兩筆以上的70大項就不填小項
				Begin	
					Select @Seq2 = isnull(OutputSeq2,'')
					From #SEQ2tmp 
					Where id = @POID AND Scirefno = @SCIRefno and OutputSeq1=@SEQ1 and OutputSeq2 != '' AND Colorid = @colorid
				End
				Drop table #SEQ2tmp
			End
			-----------------------------------------------------------------------
			if @WorkType=1 --WorkType by Combination
			Begin

					SET @modLayer = @TotalLayer % @maxLayer
					SET @LayerCount = Floor(@TotalLayer / @maxLayer) --產生幾筆
					SET @Layernum = 1
					
					While @Layernum<=@LayerCount
					Begin
						Select a.*,IDENTITY(int,1,1) as Rowid 
						into #distriqty_cutlayer 
						From Order_EachCons_SizeQty a,Order_SizeCode b 
						Where a.id = b.id and a.SizeCode = b.SizeCode and a.Order_EachConsUkey = @ukey 
						order by seq

						Set @distriqtyRowID = 1
						Select @distriqtyRowID = Min(RowID), @distriqtyRowCount = Max(RowID) 
						From #distriqty_cutlayer
						While @distriqtyRowID<=@distriqtyRowCount 
						Begin
							--if(@WorkType=1)
							
							Select @Sizecode = sizecode
							From #distriqty_cutlayer
							Where Rowid  = @distriqtyRowID

							select id,Article,disqty,orderqty,IDENTITY(int,1,1) as Rowid,Convert(Bigint,identRowid) as identRowid
							into #disorder_cutlayer
							from #disQty 
							Where SizeCode = @sizeCode and Colorid = @colorid and PatternPanel = @FabricCombo
							set @disQtyRowID = 1
							Select @disQtyRowID = Min(RowID), @disQtyRowCount = Max(RowID) from #disorder_cutlayer
							While @disQtyRowID<=@disQtyRowCount 
							Begin
								if(@disQtyRowID = 1)
								begin
									----------------計算SizeRatio Qty----------------------
									Select @Layer_Cutqty = Qty
									From Order_EachCons_SizeQty
									Where Order_EachConsUkey = @ukey and SizeCode = @sizeCode 
									----------------------------------------							
									SET @CutQty  = @Layer_Cutqty * @maxLayer--分配給此Article的數量
								end 
								Select @distributeQty = disQty, 
									   @OrderQty = orderqty,
									   @WorkOrder_DisOrderID = ID,
									   @WorkOrder_DisidenRow = identRowid,
									   @Article = article
								from #disorder_cutlayer
								Where Rowid = @disQtyRowID
								if(@disQtyRowID = 1) --將第一筆當作Workorder的SP#
								begin
									Select @Orderid = id
									from #disorder_cutlayer
									Where Rowid = @disQtyRowID
								End

								if(@OrderQty > @distributeQty) --若OrderQty沒超過Distribute才可繼續分配
								Begin
							
									if(@CutQty > @OrderQty - @distributeQty) 
									Begin
										SET @WorkOrder_DisQty = @OrderQty - @distributeQty		
									End;
									else
									Begin
										SET @WorkOrder_DisQty = @CutQty
									End
									update #disQty set disqty = disqty + IsNull(@WorkOrder_DisQty,0)
									Where identRowid = @WorkOrder_DisidenRow

									set @CutQty = @CutQty - @WorkOrder_DisQty --剩餘數
									-------------Insert into WorkOrder_Distribute------------------
									if(@WorkOrder_DisQty>0)
									Begin
										insert into #NewWorkOrder_Distribute(ID,OrderID,Article,SizeCode,Qty,NewKey,WorkOrderUkey)
										Values(@Cuttingid, @WorkOrder_DisOrderID,@Article,@SizeCode,@WorkOrder_DisQty,@NewKey,0)		
									End
								End;
								set @disQtyRowID +=1
							End;
							IF(@CutQty>0)
							BEGIN
								insert into #NewWorkOrder_Distribute(ID,OrderID,Article,SizeCode,Qty,NewKey,WorkOrderUkey)
								Values(@Cuttingid, 'Excess','',@SizeCode,@CutQty,@NewKey,0)					
							END
							SET @distriqtyRowID += 1
							drop table #disorder_cutlayer
						End;
						drop table #distriqty_cutlayer
						----------------計算WorkOrder_SizeRatio Qty----------------------
						SET @SizeRatioQty = 0
						Select @SizeRatioQty = sum(Qty)
						From Order_EachCons_SizeQty
						Where Order_EachConsUkey = @ukey
						----------------新增WorkOrder_SizeRatio----------------------
						Select * ,IDENTITY(int,1,1) as Rowid  
						into #SizeRatio_cutlayer
						From Order_EachCons_SizeQty
						Where Order_EachConsUkey = @ukey
						set @WorkOrder_SizeRatioRowid = 1
						Select @WorkOrder_SizeRatioRowid = Min(Rowid),@WorkOrder_SizeRatioRowCount = Max(RowID)
						From #SizeRatio_cutlayer
						While @WorkOrder_SizeRatioRowid <= @WorkOrder_SizeRatioRowCount
						Begin
							Select @SizeCode = sizecode,
								   @WorkOrder_SizeRatio_Qty = Qty
							From #SizeRatio_cutlayer
							Where Rowid = @WorkOrder_SizeRatioRowid
							Insert into #NewWorkOrder_SizeRatio(ID,SizeCode,Qty,newKey,WorkOrderUkey)
							Values(@Cuttingid,@SizeCode,@WorkOrder_SizeRatio_Qty,@NewKey,0)
							Set @WorkOrder_SizeRatioRowid+= 1
						End;
						drop table #SizeRatio_cutlayer
				
						----------------新增WorkOrder_PatternPanel----------------------
						Select * ,IDENTITY(int,1,1) as Rowid 
						into #PatternPanel_cutlayer
						From Order_EachCons_PatternPanel
						Where Order_EachConsUkey = @ukey
						set @WorkOrder_SizeRatioRowid = 1
						Select @WorkOrder_SizeRatioRowid = Min(Rowid),@WorkOrder_SizeRatioRowCount = Max(RowID) 
						From #PatternPanel_cutlayer
						While @WorkOrder_SizeRatioRowid <= @WorkOrder_SizeRatioRowCount
						Begin
							Select @WorkOrder_PatternPanel  = PatternPanel,
								   @WorkOrder_LectraCode = LectraCode
							From #PatternPanel_cutlayer
							Where Rowid = @WorkOrder_SizeRatioRowid
							Insert into #NewWorkOrder_PatternPanel(ID,PatternPanel,LectraCode,newKey)
							Values(@Cuttingid,@WorkOrder_PatternPanel,@WorkOrder_LectraCode,@NewKey)
							set @WorkOrder_SizeRatioRowid += 1
						End;
						drop table #PatternPanel_cutlayer
						-------------------------------------
						SET @Layer = @maxLayer
						SET @Cons = @Layer * @SizeRatioQty * @ConsPC
						Insert Into #NewWorkorder(ID,FactoryID,MDivisionid,SEQ1,SEQ2,OrderID,Layer,Colorid,MarkerName,MarkerLength,ConsPC,Cons,Refno,SCIRefno,Markerno,MarkerVersion,Type,AddName,AddDate,MarkerDownLoadId,FabricCombo,FabricCode,LectraCode,newKey,Order_EachConsUkey)
						Values(@Cuttingid,@Factoryid,@mDivisionid,@seq1,@seq2,@OrderID,@Layer,@Colorid,@Markername,@MarkerLength,@ConsPC,@Cons,@Refno,@SCIRefno,@MarkerNo,@MarkerVerion,@Type,@username,GETDATE(),@MarkerDownLoadid,@FabricCombo,@FabricCode,@LectraCode,@NewKey,@Order_EachConsUkey)
						SET @NewKey += 1
						Set @Layernum +=1
					End
					-----------餘數層數也要做一筆--------------------------------				
					Select a.*,IDENTITY(int,1,1) as Rowid 
					into #distriqty_modlayer 
					From Order_EachCons_SizeQty a,Order_SizeCode b 
					Where a.id = b.id and a.SizeCode = b.SizeCode and a.Order_EachConsUkey = @ukey 
					order by seq

					Set @distriqtyRowID = 1
					Select @distriqtyRowID = Min(RowID), @distriqtyRowCount = Max(RowID) 
					From #distriqty_modlayer 
					While @distriqtyRowID<=@distriqtyRowCount
					Begin

						Select @Sizecode = sizecode,
								@CutQty  = @modLayer * Qty--分配給此Article的數量
						From #distriqty_modlayer 
						Where Rowid  = @distriqtyRowID

						select id,disqty,Article,orderqty,IDENTITY(int,1,1) as Rowid,Convert(Bigint,identRowid) as identRowid
						into #disorder_modlayer 
						from #disQty 
						Where SizeCode = @sizeCode and Colorid = @Colorid and PatternPanel = @FabricCombo
						set @disQtyRowID = 1
						Select @disQtyRowID = Min(RowID), @disQtyRowCount = Max(RowID) from #disorder_modlayer 
						While @disQtyRowID<=@disQtyRowCount
						Begin
							Select @distributeQty = disQty, 
								   @OrderQty = orderqty,
								   @WorkOrder_DisOrderID = ID,
								   @WorkOrder_DisidenRow = identRowid,
								   @Article = article
							from #disorder_modlayer 
							Where Rowid = @disQtyRowID

							if(@disQtyRowID = 1)
							begin
								Select @Orderid = id
								from #disorder_modlayer
								Where Rowid = @disQtyRowID
							End

							if(@OrderQty > @distributeQty) --若OrderQty沒超過Distribute才可繼續分配
							Begin
							
								if(@CutQty >= @OrderQty - @distributeQty) 
								Begin
									SET @WorkOrder_DisQty = @OrderQty - @distributeQty
								End;
								else
								Begin
									SET @WorkOrder_DisQty = @CutQty
								End;
								update #disQty set disqty = disqty + IsNull(@WorkOrder_DisQty,0)
								Where identRowid = @WorkOrder_DisidenRow

								set @CutQty = @CutQty - @WorkOrder_DisQty --剩餘數
								-------------Insert into WorkOrder_Distribute------------------
								if(@WorkOrder_DisQty>0)
								Begin
									insert into #NewWorkOrder_Distribute(ID,OrderID,Article,SizeCode,Qty,NewKey,WorkOrderUkey)
									Values(@Cuttingid, @WorkOrder_DisOrderID,@Article,@SizeCode,@WorkOrder_DisQty,@NewKey,0)
								End
							End;
							set @disQtyRowID +=1
						End;

						IF(@CutQty>0)
						BEGIN
							insert into #NewWorkOrder_Distribute(ID,OrderID,Article,SizeCode,Qty,NewKey,WorkOrderUkey)
							Values(@Cuttingid, 'Excess','',@SizeCode,@CutQty,@NewKey,0)					
						END
						SET @distriqtyRowID += 1
						drop table #disorder_modlayer
					End;
					drop table #distriqty_modlayer
					----------------計算WorkOrder_SizeRatio Qty----------------------
					SET @SizeRatioQty = 0
					Select @SizeRatioQty = sum(Qty)
					From Order_EachCons_SizeQty
					Where Order_EachConsUkey = @ukey
					----------------新增WorkOrder_SizeRatio----------------------
					Select * ,IDENTITY(int,1,1) as Rowid  
					into #SizeRatio_modlayer 
					From Order_EachCons_SizeQty
					Where Order_EachConsUkey = @ukey
					set @WorkOrder_SizeRatioRowid = 1
					Select @WorkOrder_SizeRatioRowid = Min(Rowid),@WorkOrder_SizeRatioRowCount = Max(RowID)
					From #SizeRatio_modlayer 
					While @WorkOrder_SizeRatioRowid <= @WorkOrder_SizeRatioRowCount
					Begin
						Select @SizeCode = sizecode,
							   @WorkOrder_SizeRatio_Qty = Qty
						From #SizeRatio_modlayer 
						Where Rowid = @WorkOrder_SizeRatioRowid
						Insert into #NewWorkOrder_SizeRatio(ID,SizeCode,Qty,newKey,WorkOrderUkey)
						Values(@Cuttingid,@SizeCode,@WorkOrder_SizeRatio_Qty,@NewKey,0)
						Set @WorkOrder_SizeRatioRowid+= 1
					End;
					drop table #SizeRatio_modlayer 
				
					----------------新增WorkOrder_PatternPanel----------------------
					Select * ,IDENTITY(int,1,1) as Rowid 
					into #PatternPanel_modlayer 
					From Order_EachCons_PatternPanel
					Where Order_EachConsUkey = @ukey
					set @WorkOrder_SizeRatioRowid = 1
					Select @WorkOrder_SizeRatioRowid = Min(Rowid),@WorkOrder_SizeRatioRowCount = Max(RowID) From #PatternPanel_modlayer
					While @WorkOrder_SizeRatioRowid <= @WorkOrder_SizeRatioRowCount
					Begin
						Select @WorkOrder_PatternPanel  = PatternPanel,
							   @WorkOrder_LectraCode = LectraCode
						From #PatternPanel_modlayer 
						Where Rowid = @WorkOrder_SizeRatioRowid
						Insert into #NewWorkOrder_PatternPanel(ID,PatternPanel,LectraCode,newKey)
						Values(@Cuttingid,@WorkOrder_PatternPanel,@WorkOrder_LectraCode,@NewKey)
						set @WorkOrder_SizeRatioRowid += 1
					End;
					drop table #PatternPanel_modlayer 
					-------------------------------------
					SET @Layer = @modLayer
					SET @Cons = @Layer * @SizeRatioQty*@ConsPC
					Insert Into #NewWorkorder(ID,FactoryID,MDivisionid,SEQ1,SEQ2,OrderID,Layer,Colorid,MarkerName,MarkerLength,ConsPC,Cons,Refno,SCIRefno,Markerno,MarkerVersion,Type,AddName,AddDate,MarkerDownLoadId,FabricCombo,FabricCode,LectraCode,newKey,Order_eachconsUkey)
					Values(@Cuttingid,@Factoryid,@mDivisionid,@seq1,@seq2,@Orderid,@Layer,@Colorid,@Markername,@MarkerLength,@ConsPC,@Cons,@Refno,@SCIRefno,@MarkerNo,@MarkerVerion,@Type,@username,GETDATE(),@MarkerDownLoadid,@FabricCombo,@FabricCode,@LectraCode,@NewKey,@Order_EachConsUkey)
					SET @NewKey += 1
					----------End 餘數層數也要做一筆-----------------------------			
				End		
			Else --WorkType by SP#
			Begin
				---------排序混碼Size Ratio Qty由大到小，才可以由大的數量先排-------
				--------------------------------------------------------------------
				
				Select *,IDENTITY(int,1,1) as Rowid 
				into #SizeQty
				From Order_EachCons_SizeQty Where Order_EachConsUkey = @ukey 
				order by Qty
				Set @sizeQtyRowid = 1
				Select @sizeQtyRowid = Min(Rowid),@sizeQtyRowCount = Max(Rowid)
				From #SizeQty
				While @sizeQtyRowid<= @sizeQtyRowCount
				Begin
					Select @SizeCode = SizeCode,
						   @SizeQty = Qty
					From #SizeQty
					where rowid = @sizeQtyRowid
					-------取得此部位同Size同顏色inline 較早的Orderid與Qty
					Select id,sizecode,article,colorid,orderqty,disQty,PatternPanel,convert(bigint,identRowid) as identRowid,IDENTITY(int,1,1) as Rowid
					into #distOrder 
					From #disQty
					Where SizeCode = @SizeCode and PatternPanel = @FabricCombo and Colorid = @Colorid and orderQty - disQty >0
					order by inline
					Set @distOrderRowid = 1
					Select @distOrderRowid = Min(Rowid),@distOrderRowCount = Max(Rowid)
					From #distOrder
					While @distOrderRowid <= @distOrderRowCount
					Begin

						Select @BalQty = OrderQty - disQty,
							   @Article = Article,
							   @OrderQty = Orderqty,
							   @OrderID = id,
							   @WorkOrder_DisOrderID = id,
							   @WorkOrder_DisidenRow = identRowid
						From #distOrder
						Where Rowid = @distOrderRowid

						Set @OrderLayer = ceiling(@BalQty / @SizeQty )----無條件進位，要把層數用完
						--------------找出可被分配的TotalLay----------------
						Select @TotalLayer = Layer
						From #Order_EachCons_Color_Article
						Where Article = @Article and Order_EachConsUkey = @Order_EachConsUkey and SizeCode = @SizeCode 
						-------------End找出可被分配的TotalLay-------------------------------------
						IF(@OrderLayer < @TotalLayer)
						BEGIN
							Set @Layer = @OrderLayer
						END
						else
						BEGIN
							Set @Layer = @TotalLayer
						END
						------------------確認筆數-------------------------------
						Set @LayerCount = floor(@Layer / @maxLayer)
						Set @modLayer = @Layer % @maxLayer
						------------------------------------------------
						Set @Layernum = 1
						Set @linsert = 0 --表示尚未新增Distribute
						While @Layernum<=@LayerCount
						Begin
							Set @linsert = 0 --表示尚未新增Distribute
							Set @sizeQtyRowid_again = 1
							Select @sizeQtyRowid_again = Min(Rowid),@sizeQtyRowCount_again = Max(Rowid)
							From #SizeQty
							While @sizeQtyRowid_again<= @sizeQtyRowCount_again ---多尺碼使用
							Begin
								Select @SizeCode = SizeCode,
										@SizeQty = Qty
								From #sizeQty
								where rowid = @sizeQtyRowid_again 

								Set @CutQty = @SizeQty * @maxLayer -----每層的總裁數
								Select id,sizecode,article,colorid,orderqty,disQty,PatternPanel,convert(bigint,identRowid) as identRowid,IDENTITY(int,1,1) as Rowid
								into #distOrder_again 
								From #disQty
								Where SizeCode = @SizeCode and PatternPanel = @FabricCombo and Colorid = @Colorid
								Order by inline
								Set @distOrderRowid_again = 1
								Select @distOrderRowid_again = Min(Rowid),@distOrderRowCount_again = Max(Rowid)
								From #distOrder_again
								While @distOrderRowid_again <= @distOrderRowCount_again 
								Begin
									if(@CutQty<=0)
									BEGIN
										set @distOrderRowid_again += 1
										Continue
									END
									Select @Article = Article,
											@SizeCode = SizeCode,
											@WorkOrder_DisOrderID = id,
											@WorkOrder_DisidenRow = identRowid,
											@OrderQty = OrderQty,
											@disQty_again = disQty
									From #distOrder_again
									Where Rowid = @distOrderRowid_again
									if(@OrderQty-@disQty_again<=0)
									Begin
										set @distOrderRowid_again += 1
										Continue
									End
									if(@CutQty>@OrderQty - @disQty_again)
									Begin
										Set @WorkOrder_DisQty = @OrderQty - @disQty_again
									End
									Else
									Begin
										Set @WorkOrder_DisQty = @CutQty
									End
									---------------------------------------------------------
									
									if(@WorkOrder_DisQty>0)
									begin
										Set @CutQty = @CutQty - @WorkOrder_DisQty
										
										insert into #NewWorkOrder_Distribute(ID,OrderID,Article,SizeCode,Qty,NewKey,WorkOrderUkey)
										Values(@Cuttingid, @WorkOrder_DisOrderID,@Article,@SizeCode,@WorkOrder_DisQty,@NewKey,0)
										update #disQty set disqty = disqty + IsNull(@WorkOrder_DisQty,0) 
										where identRowid = @WorkOrder_DisidenRow
										Set @linsert = 1 ---有新增要改變
									end
									Set @distOrderRowid_again += 1
								End
								Drop table #distOrder_again
								if(@CutQty>0) ---若全分配完還有剩就要給Excess
								Begin
									insert into #NewWorkOrder_Distribute(ID,OrderID,Article,SizeCode,Qty,NewKey,WorkOrderUkey)
									Values(@Cuttingid, 'Excess','',@SizeCode,@CutQty,@NewKey,0)		
									Set @linsert = 1 ---有新增要改變		
								End
								Set @sizeQtyRowid_again+=1
							End
							if(@linsert = 1) ---有新增要增加表頭
							Begin
							----------------計算WorkOrder_SizeRatio Qty----------------------
								SET @SizeRatioQty = 0
								Select @SizeRatioQty = sum(Qty)
								From Order_EachCons_SizeQty
								Where Order_EachConsUkey = @ukey
								----------------新增WorkOrder_SizeRatio----------------------
								Select * ,IDENTITY(int,1,1) as Rowid  
								into #SizeRatio_bysp
								From Order_EachCons_SizeQty
								Where Order_EachConsUkey = @ukey
								set @WorkOrder_SizeRatioRowid = 1
								Select @WorkOrder_SizeRatioRowid = Min(Rowid),@WorkOrder_SizeRatioRowCount = Max(RowID)
								From #SizeRatio_bysp
								While @WorkOrder_SizeRatioRowid <= @WorkOrder_SizeRatioRowCount
								Begin
									Select @SizeCode = sizecode,
										   @WorkOrder_SizeRatio_Qty = Qty
									From #SizeRatio_bysp
									Where Rowid = @WorkOrder_SizeRatioRowid
									Insert into #NewWorkOrder_SizeRatio(ID,SizeCode,Qty,newKey,WorkOrderUkey)
									Values(@Cuttingid,@SizeCode,@WorkOrder_SizeRatio_Qty,@NewKey,0)
									Set @WorkOrder_SizeRatioRowid+= 1
									set @linsert = 0
								End;
								drop table #SizeRatio_bysp
				
								----------------新增WorkOrder_PatternPanel----------------------
								Select * ,IDENTITY(int,1,1) as Rowid 
								into #PatternPanel_bysp
								From Order_EachCons_PatternPanel
								Where Order_EachConsUkey = @ukey
								set @WorkOrder_SizeRatioRowid = 1
								Select @WorkOrder_SizeRatioRowid = Min(Rowid),@WorkOrder_SizeRatioRowCount = Max(RowID) 
								From #PatternPanel_bysp
								While @WorkOrder_SizeRatioRowid <= @WorkOrder_SizeRatioRowCount
								Begin
									Select @WorkOrder_PatternPanel  = PatternPanel,
										   @WorkOrder_LectraCode = LectraCode
									From #PatternPanel_bysp
									Where Rowid = @WorkOrder_SizeRatioRowid
									Insert into #NewWorkOrder_PatternPanel(ID,PatternPanel,LectraCode,newKey)
									Values(@Cuttingid,@WorkOrder_PatternPanel,@WorkOrder_LectraCode,@NewKey)
									set @WorkOrder_SizeRatioRowid += 1
								End;
								drop table #PatternPanel_bysp
								-------------------------------------
								if(@WorkOrder_DisQty>0)
								begin
									SET @Cons = @Layer * @SizeRatioQty * @ConsPC
									--update #Order_EachCons_Color_Article set Layer = Layer - ISNULL(@maxLayer ,0)
									--Where Article = @Article and Order_EachConsUkey = @Order_EachConsUkey and SizeCode = @SizeCode 
									Insert Into #NewWorkorder(ID,FactoryID,MDivisionid,SEQ1,SEQ2,OrderID,Layer,Colorid,MarkerName,MarkerLength,ConsPC,Cons,Refno,SCIRefno,Markerno,MarkerVersion,Type,AddName,AddDate,MarkerDownLoadId,FabricCombo,FabricCode,LectraCode,newKey,Order_eachconsUkey)
									Values(@Cuttingid,@Factoryid,@mDivisionid,@seq1,@seq2,@Orderid,@maxLayer,@Colorid,@Markername,@MarkerLength,@ConsPC,@Cons,@Refno,@SCIRefno,@MarkerNo,@MarkerVerion,@Type,@username,GETDATE(),@MarkerDownLoadid,@FabricCombo,@FabricCode,@LectraCode,@NewKey,@Order_EachConsUkey)
								end
								SET @NewKey += 1
							End
							Set @Layernum+=1
						End
						-------------------剩餘ModLayer-----------------					
						if(@modLayer>0)
						Begin
							Set @sizeQtyRowid_again = 1
							Select @sizeQtyRowid_again = Min(Rowid),@sizeQtyRowCount_again = Max(Rowid)
							From #SizeQty
							While @sizeQtyRowid_again<= @sizeQtyRowCount_again ---多尺碼使用
							Begin
								Select @SizeCode = SizeCode,
										@SizeQty = Qty
								From #sizeQty
								where rowid = @sizeQtyRowid_again 

								Set @linsert = 0 --表示尚未新增Distribute
								set @CutQty = @modLayer * @SizeQty
								Select id,sizecode,article,colorid,orderqty,disQty,PatternPanel,convert(bigint,identRowid) as identRowid,IDENTITY(int,1,1) as Rowid
								into #distOrder_againmod 
								From #disQty
								Where SizeCode = @SizeCode and PatternPanel = @FabricCombo and Colorid = @Colorid
								Set @distOrderRowid_again = 1
								Select @distOrderRowid_again = Min(Rowid),@distOrderRowCount_again = Max(Rowid)
								From #distOrder_againmod
								While @distOrderRowid_again <= @distOrderRowCount_again 
								Begin
									if(@CutQty<=0)
									Begin
										set @distOrderRowid_again += 1
										Continue
									End
									Select @Article = Article,
											@SizeCode = SizeCode,
											@WorkOrder_DisOrderID = id,
											@WorkOrder_DisidenRow = identRowid
									From #distOrder_againmod
									Where Rowid = @distOrderRowid_again

									Select 	@OrderQty = OrderQty,
											@disQty_again = disQty
									From #disQty
									Where identRowid = @WorkOrder_DisidenRow

									if(@OrderQty-@disQty_again <=0)
									Begin
										set @distOrderRowid_again += 1
										Continue
									End
									if(@CutQty>@OrderQty - @disQty_again)
									Begin
										Set @WorkOrder_DisQty = @OrderQty - @disQty_again
									End
									Else
									Begin
										Set @WorkOrder_DisQty = @CutQty
									End
									---------------------------------------------------------
									if(@WorkOrder_DisQty>0)
									begin
										Set @CutQty = @CutQty - @WorkOrder_DisQty
										insert into #NewWorkOrder_Distribute(ID,OrderID,Article,SizeCode,Qty,NewKey,WorkOrderUkey)
										Values(@Cuttingid, @WorkOrder_DisOrderID,@Article,@SizeCode,@WorkOrder_DisQty,@NewKey,0)
										update #disQty set disqty = disqty + IsNull(@WorkOrder_DisQty,0) 
										where identRowid = @WorkOrder_DisidenRow
										Set @linsert = 1 ---有新增要改變
									End
									Set @distOrderRowid_again += 1
								End
								Drop table #distOrder_againmod
								SET @sizeQtyRowid_again += 1
							
								if(@CutQty>0) ---若全分配完還有剩就要給Excess
								Begin
									insert into #NewWorkOrder_Distribute(ID,OrderID,Article,SizeCode,Qty,NewKey,WorkOrderUkey)
									Values(@Cuttingid, 'Excess','',@SizeCode,@CutQty,@NewKey,0)		
									Set @linsert = 1 ---有新增要改變		
								End
							End

							if(@linsert = 1) ---有新增要增加表頭
							Begin
							----------------計算WorkOrder_SizeRatio Qty----------------------
								SET @SizeRatioQty = 0
								Select @SizeRatioQty = sum(Qty)
								From Order_EachCons_SizeQty
								Where Order_EachConsUkey = @ukey
								----------------新增WorkOrder_SizeRatio----------------------
								Select * ,IDENTITY(int,1,1) as Rowid  
								into #SizeRatio_byspmod
								From Order_EachCons_SizeQty
								Where Order_EachConsUkey = @ukey
								set @WorkOrder_SizeRatioRowid = 1
								Select @WorkOrder_SizeRatioRowid = Min(Rowid),@WorkOrder_SizeRatioRowCount = Max(RowID)
								From #SizeRatio_byspmod
								While @WorkOrder_SizeRatioRowid <= @WorkOrder_SizeRatioRowCount
								Begin
									Select @SizeCode = sizecode,
										   @WorkOrder_SizeRatio_Qty = Qty
									From #SizeRatio_byspmod
									Where Rowid = @WorkOrder_SizeRatioRowid
									Insert into #NewWorkOrder_SizeRatio(ID,SizeCode,Qty,newKey,WorkOrderUkey)
									Values(@Cuttingid,@SizeCode,@WorkOrder_SizeRatio_Qty,@NewKey,0)
									Set @WorkOrder_SizeRatioRowid+= 1
								End;
								drop table #SizeRatio_byspmod
				
								----------------新增WorkOrder_PatternPanel----------------------
								Select * ,IDENTITY(int,1,1) as Rowid 
								into #PatternPanel_byspmod
								From Order_EachCons_PatternPanel
								Where Order_EachConsUkey = @ukey
								set @WorkOrder_SizeRatioRowid = 1
								Select @WorkOrder_SizeRatioRowid = Min(Rowid),@WorkOrder_SizeRatioRowCount = Max(RowID) 
								From #PatternPanel_byspmod
								While @WorkOrder_SizeRatioRowid <= @WorkOrder_SizeRatioRowCount
								Begin
									Select @WorkOrder_PatternPanel  = PatternPanel,
										   @WorkOrder_LectraCode = LectraCode
									From #PatternPanel_byspmod
									Where Rowid = @WorkOrder_SizeRatioRowid
									Insert into #NewWorkOrder_PatternPanel(ID,PatternPanel,LectraCode,newKey)
									Values(@Cuttingid,@WorkOrder_PatternPanel,@WorkOrder_LectraCode,@NewKey)
									set @WorkOrder_SizeRatioRowid += 1
								End;
								drop table #PatternPanel_byspmod
								-------------------------------------
								
								SET @Cons = @modLayer * @SizeRatioQty * @ConsPC
								Insert Into #NewWorkorder(ID,FactoryID,MDivisionid,SEQ1,SEQ2,OrderID,Layer,Colorid,MarkerName,MarkerLength,ConsPC,Cons,Refno,SCIRefno,Markerno,MarkerVersion,Type,AddName,AddDate,MarkerDownLoadId,FabricCombo,FabricCode,LectraCode,newKey,Order_eachconsUkey)
								Values(@Cuttingid,@Factoryid,@mDivisionid,@seq1,@seq2,@Orderid,@modLayer,@Colorid,@Markername,@MarkerLength,@ConsPC,@Cons,@Refno,@SCIRefno,@MarkerNo,@MarkerVerion,@Type,@username,GETDATE(),@MarkerDownLoadid,@FabricCombo,@FabricCode,@LectraCode,@NewKey,@Order_EachConsUkey)
								SET @NewKey += 1

								set @linsert = 0
							end						
						End
						Set @distOrderRowid += 1			
					End
					drop table #distOrder
					Set @sizeQtyRowid += 1
				EnD
				Drop table #SizeQty
			End --End WorkType by SP#
		Set @WorkOrderMixRowID += 1
		ENd --End WorkOrder Loop
		drop table #WorkOrderMix
	End;
	
	--Select * from #disQty  order by PatternPanel,Colorid,SizeCode,id 
	/*Select * from #NewWorkOrder ORDER BY Markername
	Select * from #NewWorkOrder_Distribute order by newkey
	Select * from #NewWorkOrder_PatternPanel
	Select * from #NewWorkOrder_SizeRatio
	*/

	Declare @insertRow int
	Declare @insertcount int
	Declare @iden bigint
	Select @insertcount = Max(newkey) 
	From #NewWorkorder

	Set @insertRow = 1
	While @insertRow<=@insertcount
	Begin
		insert into WorkOrder(id,factoryid,MDivisionId,SEQ1,SEQ2,CutRef,OrderID,CutplanID,Cutno,Layer,Colorid,Markername,
						EstCutDate,CutCellid,MarkerLength,ConsPC,Cons,Refno,SCIRefno,MarkerNo,MarkerVersion,Type,Order_EachconsUkey,
						AddName,FabricCombo,MarkerDownLoadId,FabricCode,LectraCode)
						(Select id,factoryid,MDivisionId,SEQ1,SEQ2,CutRef,OrderID,CutplanID,Cutno,Layer,Colorid,Markername,
						EstCutDate,CutCellid,MarkerLength,ConsPC,Cons,Refno,SCIRefno,MarkerNo,MarkerVersion,Type,Order_EachconsUkey,
						AddName,FabricCombo,MarkerDownLoadId,FabricCode,LectraCode 
						From #NewWorkOrder Where newkey = @insertRow)
		select @iden = @@IDENTITY 
		--------將撈出的Ident 寫入----------
		update #NewWorkOrder_Distribute set WorkOrderUkey = @iden Where newkey = @insertRow
		update #NewWorkOrder_PatternPanel set WorkOrderUkey = @iden Where newkey = @insertRow
		update #NewWorkOrder_SizeRatio set WorkOrderUkey = @iden Where newkey = @insertRow
		------Insert into 子Table-------------
		insert into WorkOrder_Distribute(WorkOrderUkey,id,Orderid,Article,SizeCode,Qty)
			(Select WorkOrderUkey,id,Orderid,Article,SizeCode,Qty
			From #NewWorkOrder_Distribute Where newkey=@insertRow)
		insert into WorkOrder_PatternPanel(WorkOrderUkey,ID,LectraCode,PatternPanel)
			(Select WorkOrderUkey,ID,LectraCode,PatternPanel
			From #NewWorkOrder_PatternPanel Where newkey=@insertRow)
		insert into WorkOrder_SizeRatio(WorkOrderUkey,ID,SizeCode,Qty)
			(Select WorkOrderUkey,ID,SizeCode,Qty
			From #NewWorkOrder_SizeRatio Where newkey=@insertRow)
			Set @insertRow+=1
	End
End