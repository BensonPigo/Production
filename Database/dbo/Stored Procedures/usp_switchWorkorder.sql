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
	from Orders WITH (NOLOCK) where cuttingsp = @Cuttingid
	--撈取最細EachCons_Color_Article
	Select a.ConsPC,a.CuttingWidth,a.FabricCode,a.FabricCombo,a.Id,a.FabricPanelCode,a.MarkerDownloadID,
			a.MarkerLength,a.MarkerName,a.MarkerNo,a.MarkerVersion,a.Ukey,a.Width,
			b.ColorID,b.CutQty as TotalCutQty,b.Layer as TotalLayer,b.YDS,
			c.CutQty,c.SizeCode,c.Layer,c.Article, b.Order_EachConsUkey, b.Ukey as Order_EachCons_ColorUkey
	InTo #Order_EachCons_Color_Article
	From Order_EachCons a  WITH (NOLOCK) , Order_EachCons_Color b  WITH (NOLOCK) , Order_EachCons_Color_Article c  WITH (NOLOCK) 
	Where a.ukey = b.Order_EachConsUkey and b.Ukey = c.Order_EachCons_ColorUkey and a.id = b.id and a.id = c.id and b.id = c.id
			and a.id = @Cuttingid and a.CuttingPiece = 0
	--and b.id = c.id 應該是沒必要寫，已經有and a.id = b.id and a.id = c.id 
	--撈Each Cons
	Select * into #Order_EachCons 
	from Order_EachCons  WITH (NOLOCK)  Where id = @Cuttingid
	--撈出展開Marker OrderEachCons 且非外裁
	Select a.*,b.ColorID,b.CutQty as TotalCutQty,b.Layer as TotalLayer,b.Order_EachConsUkey,b.Orderqty,b.Ukey as Order_EachCons_ColorUkey,b.Variance,b.YDS
	into #marker1
	From #Order_EachCons a,Order_Eachcons_Color b  WITH (NOLOCK)  
	Where a.id = @Cuttingid and a.Ukey = b.Order_EachConsUkey and a.CuttingPiece = 0
	--撈BOF
	Select a.ukey as Order_EachConsUkey,b.SCIRefno,b.Seq1,b.SuppID,b.Ukey as order_BofUkey ,
	c.ConstructionID,c.Width as fabricwidth,c.Refno
	Into #Order_EachCons_BOF 
	From #Order_EachCons a, Order_BoF b  WITH (NOLOCK) 
	Left Join Fabric c  WITH (NOLOCK)  on c.SCIRefno = b.SCIRefno
	Where a.id = b.id and a.FabricCode = b.FabricCode
	--撈EachCon_Color_Article 找出Article對應的層數與最大層數
	Select a.* ,
	iif(isnull((Select iif(isnull(c.CuttingLayer,0)=0,100,c.CuttingLayer)  From Construction c  WITH (NOLOCK)  where c.id = b.ConstructionID),0)=0,100,
	(Select iif(isnull(c.CuttingLayer,0)=0,100,c.CuttingLayer)  From Construction c  WITH (NOLOCK)  where c.id = b.ConstructionID)) as MaxLayer
	Into #Order_EachCons_Color_Layer
	From #marker1 a,#Order_EachCons_BOF b 
	Where a.Ukey = b.Order_EachConsUkey
	---------
	select distinct oeca.ColorID,oeca.Article,oeca.Layer
	into #Articlelayer
	from Order_EachCons_Color_Article oeca  WITH (NOLOCK) 
	where oeca.id = @Cuttingid
	/*
	找CuttingID 的POID
	*/
	Declare @POID varchar(13)
	SET @POID = 'Im default'
	Select distinct @POID = POID
	From Orders  WITH (NOLOCK) 
	Where Cuttingsp = @Cuttingid

	---------組每個SP#的Article,Size,Qty,PatternPanel,inline
	Select distinct e.id,a.article,a.colorid,e.sizecode,a.PatternPanel,e.qty as orderqty, 0 as disqty,f.Inline
	Into #_tmpdisQty
	from Order_ColorCombo a  WITH (NOLOCK) ,Order_EachCons b  WITH (NOLOCK) ,
	(Select d.*,cuttingsp from Order_Qty d  WITH (NOLOCK) ,(Select id,cuttingsp from Orders  WITH (NOLOCK)  where cuttingsp = @Cuttingid) c Where c.id = d.id) e
	left join 
	(Select a.inline,b.Article,b.SizeCode,a.OrderID 
	from SewingSchedule a  WITH (NOLOCK) ,SewingSchedule_Detail b  WITH (NOLOCK) ,
		(Select id from Orders  WITH (NOLOCK)  where cuttingsp = @Cuttingid) c 
		where c.id = a.orderid and a.id = b.id and mDivisionid = @mDivisionid) f 
	on f.OrderID = e.id and f.Article = e.Article and f.SizeCode = e.SizeCode 
	where a.id = @POID and a.FabricCode is not null and a.FabricCode !='' 
	and b.id = @POID and a.id = b.id and b.cuttingpiece='0' and  b.FabricCombo = a.PatternPanel and e.cuttingsp = a.id and e.Article = a.Article
	Order by inline,e.ID

	Select id,article,sizecode,colorid,PatternPanel,orderqty, disqty,Min(INLINE) as inline,IDENTITY(int,1,1) as identRowid
	into #disQty
	From #_tmpdisQty group by id,article,sizecode,PatternPanel,orderqty, disqty,colorid order by inline
	----------------------------------------------------------------------------------
	--New WorkOrder
	Select a.*,0 as newKey InTo #NewWorkorder From Workorder a  WITH (NOLOCK)  Where 1 =0
	Select a.*,0 as newKey InTo #NewWorkOrder_Distribute From WorkOrder_Distribute a  WITH (NOLOCK)  Where 1 = 0
	Select a.*,0 as newKey InTo #NewWorkOrder_SizeRatio From WorkOrder_SizeRatio a  WITH (NOLOCK)  Where 1 = 0
	Select a.*,0 as newKey InTo #NewWorkOrder_PatternPanel From WorkOrder_PatternPanel a  WITH (NOLOCK)  Where 1 = 0
						
	Select a.*,0 as newKey InTo #NewWorkOrder_Distributetmp From WorkOrder_Distribute a  WITH (NOLOCK)  Where 1 = 0
	Select a.*,0 as newKey InTo #NewWorkOrder_SizeRatiotmp From WorkOrder_SizeRatio a  WITH (NOLOCK)  Where 1 = 0
	Select a.*,0 as newKey InTo #NewWorkOrder_PatternPaneltmp From WorkOrder_PatternPanel a  WITH (NOLOCK)  Where 1 = 0
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
	Declare @SCIRefno varchar(30)
	Declare @Refno varchar(20)
	Declare @MarkerNo varchar(10)
	Declare @MarkerVerion varchar(3)
	Declare @FabricCombo varchar(2)
	Declare @PatternPanel varchar(2)
	Declare @MarkerDownLoadid varchar(25)
	Declare @FabricCode varchar(3)
	Declare @FabricPanelCode varchar(2)
	Declare @Suppid varchar(6)
	Declare @TotalLayer int
	Declare @ukey bigint
	Declare @Rowno int
	Declare @Article varchar(8)
	Declare @LongArticle varchar(max)
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
	Declare @WorkOrder_FabricPanelCode varchar(2)
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
	Declare @LongArticleCount int
	---	
	Declare @ACtDisLayer int --實際分配的層數
	Declare @actdistriqtyRowID int
	Declare @actdistriqtyRowCount int
	Declare @actlayer int
	Declare @balancelayer int--記錄尚未分配的layer
	/*
	先用多尺碼MixedSizeMarker = 2
	*/
	Begin
		Select a.*,IDENTITY(int,1,1) as Rowid 
		InTo #WorkOrderMix 
		From #Order_EachCons_Color_Layer a order by MixedSizeMarker desc,MarkerName
		--------------------Factory-------------------------------------
		Select @Factoryid = Factoryid From Orders  WITH (NOLOCK)  Where ID = @Cuttingid
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
				   @FabricPanelCode = FabricPanelCode,
				   @TotalLayer = TotalLayer,
				   @MarkerVerion = MarkerVersion,
				   @ukey = ukey,
				   @Type = '1',
				   @MarkerDownLoadId = MarkerDownloadID,
				   @Order_EachConsUkey = Order_EachConsUkey,
				   @Order_EachCons_ColorUkey = Order_EachCons_ColorUkey

			From #WorkOrderMix
			Where RowID = @WorkOrderMixRowID;

			select distinct Article into #LongArticle from Order_EachCons_Article  WITH (NOLOCK)  where Order_EachConsUkey=@Order_EachConsUkey
			select @LongArticleCount = count(*) from #LongArticle;

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
			Set @Seq2 =''
			Select @Seq2 = isnull(seq2,'') --先找相同SEQ1,SCIRefno
			From PO_Supp_Detail b  WITH (NOLOCK) 
			Where id = @POID AND SEQ1 = @SEQ1 AND Scirefno = @SCIRefno and OutputSeq1='' and OutputSeq2 = '' AND Colorid = @colorid
			and Junk = 0

			--ALGER TEST
			IF @Seq2 IS NULL
			Begin
				SET @Seq2 = ''
			End
			
			if @Seq2 = ''
			Begin
				--若SEQ2 為空就找70大項
				Select *
				into #SEQ2tmp
				From PO_Supp_Detail b  WITH (NOLOCK) 
				Where id = @POID AND Scirefno = @SCIRefno and OutputSeq2 != '' AND Colorid = @colorid and SEQ1 like '7%' and Junk = 0
				SET @Rowno = @@Rowcount
				set @seq1 = ''
				Select top 1 @seq1 = isnull(seq1,'')
				From #SEQ2tmp 
				Where id = @POID AND Scirefno = @SCIRefno and OutputSeq2 != '' AND Colorid = @colorid
				if @Rowno=1 --兩筆以上的70大項就不填小項
				Begin	
					Select top 1 @seq1 = seq1 ,@Seq2 = isnull(SEQ2,'')
					From #SEQ2tmp 
					Where id = @POID AND Scirefno = @SCIRefno and OutputSeq2 != '' AND Colorid = @colorid
				End
				Drop table #SEQ2tmp
			End
			-----------------------------------------------------------------------
			if @WorkType=1 --WorkType by Combination
			Begin			
				--新法-------------------------------------------------------------
				--先組依Article和Maxlayer的分配表#dis_tmpAL
				Begin
					--舊寫法
					--;with A as(
					--	select distinct Order_EachCons_ColorUkey, Article,Layer
					--	from Order_EachCons_Color_Article 
					--	where id = @Cuttingid and ColorID = @Colorid and Order_EachCons_ColorUkey = @Order_EachCons_ColorUkey
					--)
					--select  Article,sum(Layer) Layer,IDENTITY(int,1,1) as Rowid,modlayer =sum(Layer)
					--into #tmpAL 
					--from A group by Article

					--declare @rowid int, @maxrowid int
					--select @rowid=min(Rowid),@maxrowid=MAX(Rowid) 
					--from #tmpAL
				
					--create table #dis_tmpAL (Article varchar(8),Layer int,byWorkorder int)
					----Declare @maxLayer int
					----Declare @modlayer int
					----Declare @Article varchar(8)
					--Declare @byWorkorder int
					--Declare @dislayer int
					--Declare @thisLayer int
					--set @byWorkorder = 1
					--set @dislayer = 0

					--while(@rowid <= @maxrowid)--Article
					--Begin
					--	select @modlayer = modlayer,@Article = article 
					--	from #tmpAL where rowid = @rowid
					--	while(@modlayer>0)
					--	Begin
					--		if(@modlayer > @maxLayer and @dislayer = 0)
					--		Begin				
					--			insert into #dis_tmpAL(Article,Layer,byWorkorder) 
					--			values(@Article,@maxLayer,@byWorkorder)
					--			set @byWorkorder +=1
					--			set @modlayer = @modlayer - @maxLayer
					--			update #tmpAL set modlayer = @modlayer where rowid = @rowid			
					--		END
					--		Else
					--		begin
					--			set @thisLayer = 0
					--			if(@dislayer > 0)
					--			Begin
					--				set @thisLayer = @maxlayer - @dislayer
					--				if(@modlayer <= @thisLayer)
					--				Begin
					--					set @thisLayer = @modlayer
					--				END
					--			End
					--			Else
					--			Begin
					--				set @thisLayer = @modlayer
					--			End

					--			insert into #dis_tmpAL(Article,Layer,byWorkorder) 
					--			values(@Article,@thisLayer,@byWorkorder)
					--			set @dislayer += @thisLayer

					--			if(@dislayer = @maxLayer)
					--			begin
					--				set @dislayer = 0
					--				set @byWorkorder +=1
					--			end

					--			set @modlayer -= @thisLayer
					--			update #tmpAL set modlayer = @modlayer where rowid = @rowid	
					--		END
					--	END
					--	set @rowid += 1
					--END
					--drop table #tmpAL
				

					--新寫法 不依article拆layer
					create table #dis_tmpAL (Article varchar(8),Layer int,byWorkorder int)
					declare @t_layer int = 0
					Declare @byWorkorder int = 1
					--依max layer,total 計算要cut幾次
					while @t_layer <	@TotalLayer
					begin
						set @t_layer	=	@t_layer + @maxLayer
						--最後需cut零頭層數
						if @t_layer > @TotalLayer
						begin
							insert into #dis_tmpAL(Article,Layer,byWorkorder) 
											values('*',@TotalLayer % @maxLayer,@byWorkorder)
						end
						else
						begin

							insert into #dis_tmpAL(Article,Layer,byWorkorder) 
											values('*',@maxLayer,@byWorkorder)
						end
						set @byWorkorder = @byWorkorder +1
					end
					
				End--End--組依Article和Maxlayer的分配表#dis_tmpAL
				Begin
					select *,IDENTITY(int,1,1) as Rowid 
					into #dis_tmpAL_rowid
					from #dis_tmpAL
					order by byWorkorder
					declare @nowRowid int ,@Rowcount int
					select @nowRowid = min(Rowid),@Rowcount = max(Rowid)
					from #dis_tmpAL_rowid

					declare @oldWorkerordernum int
					declare @newWorkerordernum int
					set @oldWorkerordernum = 0
					set @newWorkerordernum = 0

					while(@nowRowid<=@Rowcount)
					Begin
						select @newWorkerordernum = byWorkorder,@Layer = Layer
						from #dis_tmpAL_rowid where Rowid = @nowRowid

						--準備Cons
						SET @SizeRatioQty = 0
						Select @SizeRatioQty = sum(Qty)
						From Order_EachCons_SizeQty  WITH (NOLOCK) 
						Where Order_EachConsUkey = @ukey
						SET @Cons = @Layer * @SizeRatioQty * @ConsPC
						if(@oldWorkerordernum != @newWorkerordernum)
						Begin--byworkorder的Group與前一筆不一樣,則新增一筆
							Insert Into #NewWorkorder(ID,FactoryID,MDivisionid,SEQ1,SEQ2,OrderID,Layer,Colorid,MarkerName,MarkerLength,ConsPC,Cons,Refno,SCIRefno,Markerno,MarkerVersion,Type,AddName,AddDate,MarkerDownLoadId,FabricCombo,FabricCode,FabricPanelCode,newKey,Order_eachconsUkey)
							Values(@Cuttingid,@Factoryid,@mDivisionid,@seq1,@seq2,@Cuttingid,@Layer,@Colorid,@Markername,@MarkerLength,@ConsPC,@Cons,@Refno,@SCIRefno,@MarkerNo,@MarkerVerion,@Type,@username,GETDATE(),@MarkerDownLoadid,@FabricCombo,@FabricCode,@FabricPanelCode,@NewKey,@Order_EachConsUkey)
							SET @NewKey += 1--這邊就先加了,下面同筆insert的要減1才會對應到
							set @oldWorkerordernum = @newWorkerordernum
						End						
						Else					
						Begin--與前一筆一樣則把Layer加上update
							update #NewWorkorder 
							set Layer = Layer + @Layer 
								,Cons = cons + @Cons
							where newKey = (@NewKey-1)
						End



						--準備SizeQty,要用來乘上Layer
						Select a.*,IDENTITY(int,1,1) as Rowid 
						into #distriqty_modlayer 
						From Order_EachCons_SizeQty a  WITH (NOLOCK) ,Order_SizeCode b  WITH (NOLOCK) 
						Where a.id = b.id and a.SizeCode = b.SizeCode and a.Order_EachConsUkey = @ukey 
						order by seq

						Set @distriqtyRowID = 1
						Select @distriqtyRowID = Min(RowID), @distriqtyRowCount = Max(RowID) 
						From #distriqty_modlayer 
						While @distriqtyRowID <= @distriqtyRowCount
						Begin
							Select @Sizecode = sizecode,
								   @CutQty  = @Layer * Qty--分配給此Article的數量
							From #distriqty_modlayer 
							Where Rowid  = @distriqtyRowID

							select id,disqty,Article,orderqty,IDENTITY(int,1,1) as Rowid,Convert(Bigint,identRowid) as identRowid
							into #disorder_modlayer 
							from #disQty 
							Where SizeCode = @sizeCode and Colorid = @colorid and PatternPanel = @FabricCombo--因為不同article要一起計算，所以這邊拿掉 and Article = @Article
							order by Article
							set @disQtyRowID = 1
							Select @disQtyRowID = Min(RowID), @disQtyRowCount = Max(RowID) 
							from #disorder_modlayer
							While @disQtyRowID <= @disQtyRowCount
							Begin
								Select @distributeQty = disQty,
									   @OrderQty = orderqty,
									   @WorkOrder_DisOrderID = ID,
									   @WorkOrder_DisidenRow = identRowid,
									   @Article = Article
								from #disorder_modlayer 
								Where Rowid = @disQtyRowID

								if(@disQtyRowID = 1)
								begin
									Select @Orderid = id
									from #disorder_modlayer
									Where Rowid = @disQtyRowID
								End

								if(@OrderQty > @distributeQty) --若Distribute沒超過OrderQty才可繼續分配
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
										insert into #NewWorkOrder_Distributetmp(ID,OrderID,Article,SizeCode,Qty,NewKey,WorkOrderUkey)
										Values(@Cuttingid, @WorkOrder_DisOrderID,@Article,@SizeCode,@WorkOrder_DisQty,@NewKey-1,0)
									End
								End;

								set @disQtyRowID +=1
							End

							drop table #disorder_modlayer
							SET @distriqtyRowID += 1
							if(@CutQty>0) ---若全分配完還有剩就要給EXCESS
							Begin
								insert into #NewWorkOrder_Distributetmp(ID,OrderID,Article,SizeCode,Qty,NewKey,WorkOrderUkey)
								Values(@Cuttingid, 'EXCESS','',@SizeCode,@CutQty,@NewKey-1,0)		
							End
						END
						
						----------------新增WorkOrder_SizeRatio-------------------------
						Begin						
							Select * ,IDENTITY(int,1,1) as Rowid  
							into #SizeRatio_modlayer 
							From Order_EachCons_SizeQty  WITH (NOLOCK) 
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
								Insert into #NewWorkOrder_SizeRatiotmp(ID,SizeCode,Qty,newKey,WorkOrderUkey)
								Values(@Cuttingid,@SizeCode,@WorkOrder_SizeRatio_Qty,@NewKey-1,0)
								Set @WorkOrder_SizeRatioRowid+= 1
							End;
							
							drop table #SizeRatio_modlayer
						End
						----------------新增WorkOrder_PatternPanel----------------------
						Begin							
							Select * ,IDENTITY(int,1,1) as Rowid 
							into #PatternPanel_modlayer 
							From Order_EachCons_PatternPanel  WITH (NOLOCK) 
							Where Order_EachConsUkey = @ukey
							set @WorkOrder_SizeRatioRowid = 1
							Select @WorkOrder_SizeRatioRowid = Min(Rowid),@WorkOrder_SizeRatioRowCount = Max(RowID) From #PatternPanel_modlayer
							While @WorkOrder_SizeRatioRowid <= @WorkOrder_SizeRatioRowCount
							Begin
								Select @WorkOrder_PatternPanel  = PatternPanel,
									   @WorkOrder_FabricPanelCode = FabricPanelCode
								From #PatternPanel_modlayer 
								Where Rowid = @WorkOrder_SizeRatioRowid

								-- insert WorkOrder_PatternPanel
								Insert into #NewWorkOrder_PatternPaneltmp(ID,PatternPanel,FabricPanelCode,newKey,WorkOrderUkey)
								Values(@Cuttingid,@WorkOrder_PatternPanel,@WorkOrder_FabricPanelCode,@NewKey-1,0)

								set @WorkOrder_SizeRatioRowid += 1
							End;
								
							drop table #PatternPanel_modlayer
						End
						----------------此筆的3個子Table準備結束------------------------
						drop table #distriqty_modlayer
						set @nowRowid += 1
					End
				End
				drop table #dis_tmpAL,#dis_tmpAL_rowid
				--新法結尾---------------------------------------------------------				
			End	
------------------------------------------------------------------------------------------------------------------------------------
			Else --WorkType by SP#
			Begin
				---------排序混碼Size Ratio Qty由大到小，才可以由大的數量先排-------
				--------------------------------------------------------------------
				
				Select *,IDENTITY(int,1,1) as Rowid 
				into #SizeQty
				From Order_EachCons_SizeQty  WITH (NOLOCK)  Where Order_EachConsUkey = @ukey 
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
					Where SizeCode = @SizeCode and PatternPanel = @FabricCombo and Colorid = @Colorid and (Article in (select Article from #LongArticle) or @LongArticleCount=0) and orderQty - disQty >0
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
								Where SizeCode = @SizeCode and PatternPanel = @FabricCombo and Colorid = @Colorid and (Article in (select Article from #LongArticle) or @LongArticleCount=0) and orderQty - disQty >0
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
										--更新DisQty暫存table中的Disqty (已分配數量)欄位
										update #disQty set disqty = disqty + IsNull(@WorkOrder_DisQty,0) 
										where identRowid = @WorkOrder_DisidenRow

										--更新DistOrder暫存table中的Disqty (已分配數量)欄位，否則會有重覆問題
										update #distOrder set disqty = disqty + IsNull(@WorkOrder_DisQty,0) 
										where identRowid = @WorkOrder_DisidenRow

										Set @linsert = 1 ---有新增要改變
									end
									Set @distOrderRowid_again += 1
								End
								Drop table #distOrder_again
								if(@CutQty>0 ) ---若全分配完還有剩就要給EXCESS
								Begin
									insert into #NewWorkOrder_Distribute(ID,OrderID,Article,SizeCode,Qty,NewKey,WorkOrderUkey)
									Values(@Cuttingid, 'EXCESS','',@SizeCode,@CutQty,@NewKey,0)		
									Set @linsert = 1 ---有新增要改變		
								End
								Set @sizeQtyRowid_again+=1
							End
							if(@linsert = 1) ---有新增要增加表頭
							Begin
							----------------計算WorkOrder_SizeRatio Qty----------------------
								SET @SizeRatioQty = 0
								Select @SizeRatioQty = sum(Qty)
								From Order_EachCons_SizeQty  WITH (NOLOCK) 
								Where Order_EachConsUkey = @ukey
								----------------新增WorkOrder_SizeRatio----------------------
								Select * ,IDENTITY(int,1,1) as Rowid  
								into #SizeRatio_bysp
								From Order_EachCons_SizeQty  WITH (NOLOCK) 
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
								From Order_EachCons_PatternPanel  WITH (NOLOCK) 
								Where Order_EachConsUkey = @ukey
								set @WorkOrder_SizeRatioRowid = 1
								Select @WorkOrder_SizeRatioRowid = Min(Rowid),@WorkOrder_SizeRatioRowCount = Max(RowID) 
								From #PatternPanel_bysp
								While @WorkOrder_SizeRatioRowid <= @WorkOrder_SizeRatioRowCount
								Begin
									Select @WorkOrder_PatternPanel  = PatternPanel,
										   @WorkOrder_FabricPanelCode = FabricPanelCode
									From #PatternPanel_bysp
									Where Rowid = @WorkOrder_SizeRatioRowid

									--insert NewWorkOrder_PatternPanel
									Insert into #NewWorkOrder_PatternPanel(ID,PatternPanel,FabricPanelCode,newKey,WorkOrderUkey)
									Values(@Cuttingid,@WorkOrder_PatternPanel,@WorkOrder_FabricPanelCode,@NewKey,0)

									set @WorkOrder_SizeRatioRowid += 1
								End;
								drop table #PatternPanel_bysp
								-------------------------------------
								if(@WorkOrder_DisQty>0)
								begin
									SET @Cons = @maxLayer * @SizeRatioQty * @ConsPC
									--update #Order_EachCons_Color_Article set Layer = Layer - ISNULL(@maxLayer ,0)
									--Where Article = @Article and Order_EachConsUkey = @Order_EachConsUkey and SizeCode = @SizeCode 
									Insert Into #NewWorkorder(ID,FactoryID,MDivisionid,SEQ1,SEQ2,OrderID,Layer,Colorid,MarkerName,MarkerLength,ConsPC,Cons,Refno,SCIRefno,Markerno,MarkerVersion,Type,AddName,AddDate,MarkerDownLoadId,FabricCombo,FabricCode,FabricPanelCode,newKey,Order_eachconsUkey)
									Values(@Cuttingid,@Factoryid,@mDivisionid,@seq1,@seq2,@Orderid,@maxLayer,@Colorid,@Markername,@MarkerLength,@ConsPC,@Cons,@Refno,@SCIRefno,@MarkerNo,@MarkerVerion,@Type,@username,GETDATE(),@MarkerDownLoadid,@FabricCombo,@FabricCode,@FabricPanelCode,@NewKey,@Order_EachConsUkey)
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
								Where SizeCode = @SizeCode and PatternPanel = @FabricCombo and Colorid = @Colorid and (Article in (select Article from #LongArticle) or @LongArticleCount=0) and orderQty - disQty >0

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

										--更新DisQty暫存table中的Disqty (已分配數量)欄位
										update #disQty set disqty = disqty + IsNull(@WorkOrder_DisQty,0) 
										where identRowid = @WorkOrder_DisidenRow

										--更新DistOrder暫存table中的Disqty (已分配數量)欄位，否則會有
										update #distOrder set disqty = disqty + IsNull(@WorkOrder_DisQty,0) 
										where identRowid = @WorkOrder_DisidenRow

										Set @linsert = 1 ---有新增要改變
									End
									Set @distOrderRowid_again += 1
								End
								Drop table #distOrder_againmod
								SET @sizeQtyRowid_again += 1
							
								if(@CutQty>0 ) ---若全分配完還有剩就要給EXCESS
								Begin
									insert into #NewWorkOrder_Distribute(ID,OrderID,Article,SizeCode,Qty,NewKey,WorkOrderUkey)
									Values(@Cuttingid, 'EXCESS','',@SizeCode,@CutQty,@NewKey,0)		
									Set @linsert = 1 ---有新增要改變		
								End
							End

							if(@linsert = 1) ---有新增要增加表頭
							Begin
							----------------計算WorkOrder_SizeRatio Qty----------------------
								SET @SizeRatioQty = 0
								Select @SizeRatioQty = sum(Qty)
								From Order_EachCons_SizeQty  WITH (NOLOCK) 
								Where Order_EachConsUkey = @ukey
								----------------新增WorkOrder_SizeRatio----------------------
								Select * ,IDENTITY(int,1,1) as Rowid  
								into #SizeRatio_byspmod
								From Order_EachCons_SizeQty  WITH (NOLOCK) 
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
								From Order_EachCons_PatternPanel  WITH (NOLOCK) 
								Where Order_EachConsUkey = @ukey
								set @WorkOrder_SizeRatioRowid = 1
								Select @WorkOrder_SizeRatioRowid = Min(Rowid),@WorkOrder_SizeRatioRowCount = Max(RowID) 
								From #PatternPanel_byspmod
								While @WorkOrder_SizeRatioRowid <= @WorkOrder_SizeRatioRowCount
								Begin
									Select @WorkOrder_PatternPanel  = PatternPanel,
										   @WorkOrder_FabricPanelCode = FabricPanelCode
									From #PatternPanel_byspmod
									Where Rowid = @WorkOrder_SizeRatioRowid

									--insert NewWorkOrder_PatternPanel
									Insert into #NewWorkOrder_PatternPanel(ID,PatternPanel,FabricPanelCode,newKey,WorkOrderUkey)
									Values(@Cuttingid,@WorkOrder_PatternPanel,@WorkOrder_FabricPanelCode,@NewKey,0)

									set @WorkOrder_SizeRatioRowid += 1
								End;
								drop table #PatternPanel_byspmod
								-------------------------------------
								
								SET @Cons = @modLayer * @SizeRatioQty * @ConsPC
								Insert Into #NewWorkorder(ID,FactoryID,MDivisionid,SEQ1,SEQ2,OrderID,Layer,Colorid,MarkerName,MarkerLength,ConsPC,Cons,Refno,SCIRefno,Markerno,MarkerVersion,Type,AddName,AddDate,MarkerDownLoadId,FabricCombo,FabricCode,FabricPanelCode,newKey,Order_eachconsUkey)
								Values(@Cuttingid,@Factoryid,@mDivisionid,@seq1,@seq2,@Orderid,@modLayer,@Colorid,@Markername,@MarkerLength,@ConsPC,@Cons,@Refno,@SCIRefno,@MarkerNo,@MarkerVerion,@Type,@username,GETDATE(),@MarkerDownLoadid,@FabricCombo,@FabricCode,@FabricPanelCode,@NewKey,@Order_EachConsUkey)
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
------------------------------------------------------------------------------------------------------------------------------------
		Set @WorkOrderMixRowID += 1

		Drop table #LongArticle

		ENd --End WorkOrder Loop
		drop table #WorkOrderMix
	End;
	
	--Select * from #disQty  order by PatternPanel,Colorid,SizeCode,id 
	/*Select * from #NewWorkOrder ORDER BY Markername
	Select * from #NewWorkOrder_Distribute order by newkey
	Select * from #NewWorkOrder_PatternPanel
	Select * from #NewWorkOrder_SizeRatio
	*/

	if(@WorkType=1)
	Begin
		Insert into #NewWorkOrder_Distribute(ID,OrderID,Article,SizeCode,Qty,NewKey,WorkOrderUkey)							
		select ID,OrderID,Article,SizeCode,sum(Qty),NewKey,WorkOrderUkey
		from #NewWorkOrder_Distributetmp
		group by ID,OrderID,Article,SizeCode,NewKey,WorkOrderUkey

		Insert into #NewWorkOrder_SizeRatio(ID,SizeCode,Qty,newKey,WorkOrderUkey)
		select distinct ID,SizeCode,Qty,newKey,WorkOrderUkey
		from #NewWorkOrder_SizeRatiotmp

		Insert into #NewWorkOrder_PatternPanel(ID,PatternPanel,FabricPanelCode,newKey,WorkOrderUkey)							
		select distinct ID,PatternPanel,FabricPanelCode,newKey,WorkOrderUkey
		from #NewWorkOrder_PatternPaneltmp
	End

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
						AddName,AddDate,FabricCombo,MarkerDownLoadId,FabricCode,FabricPanelCode)
						(Select id,factoryid,MDivisionId,SEQ1,SEQ2,CutRef,OrderID,CutplanID,Cutno,Layer,Colorid,Markername,
						EstCutDate,CutCellid,MarkerLength,ConsPC,Cons,Refno,SCIRefno,MarkerNo,MarkerVersion,Type,Order_EachconsUkey,
						AddName,AddDate,FabricCombo,MarkerDownLoadId,FabricCode,FabricPanelCode 
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
		insert into WorkOrder_PatternPanel(WorkOrderUkey,ID,FabricPanelCode,PatternPanel)
			(Select WorkOrderUkey,ID,FabricPanelCode,PatternPanel
			From #NewWorkOrder_PatternPanel Where newkey=@insertRow)
		insert into WorkOrder_SizeRatio(WorkOrderUkey,ID,SizeCode,Qty)
			(Select WorkOrderUkey,ID,SizeCode,Qty
			From #NewWorkOrder_SizeRatio Where newkey=@insertRow)
			Set @insertRow+=1
	End
End
