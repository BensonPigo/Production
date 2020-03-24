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
	from Orders WITH (NOLOCK) where cuttingsp = @Cuttingid and junk=0
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
	(Select iif(isnull(c.CuttingLayer,0)=0,100,c.CuttingLayer)  From Construction c  WITH (NOLOCK)  where c.id = b.ConstructionID)) as MaxLayer,SCIRefno
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
	Where Cuttingsp = @Cuttingid and junk=0

	---------組每個SP#的Article,Size,Qty,PatternPanel,inline
	Select distinct e.id,a.article,a.colorid,e.sizecode,a.PatternPanel,e.qty as orderqty, 0 as disqty,f.Inline,sr.SCIRefno,a.FabricPanelCode
	Into #_tmpdisQty
	from Order_ColorCombo a  WITH (NOLOCK) 
	inner join Order_EachCons b  WITH (NOLOCK) on a.id = b.id and b.cuttingpiece='0' and  b.FabricCombo = a.PatternPanel
	inner join (Select d.*,cuttingsp from Order_Qty d  WITH (NOLOCK) ,(Select id,cuttingsp from Orders  WITH (NOLOCK)  where cuttingsp = @Cuttingid and junk=0) c Where c.id = d.id) e
	on e.cuttingsp = a.id and e.Article = a.Article
	left join 
	(Select a.inline,b.Article,b.SizeCode,a.OrderID 
	from SewingSchedule a  WITH (NOLOCK) ,SewingSchedule_Detail b  WITH (NOLOCK) ,
		(Select id from Orders  WITH (NOLOCK)  where cuttingsp = @Cuttingid and junk=0) c 
		where c.id = a.orderid and a.id = b.id and mDivisionid = @mDivisionid) f 
	on f.OrderID = e.id and f.Article = e.Article and f.SizeCode = e.SizeCode 
	outer apply(
		select ob.scirefno
		from Order_BoF ob WITH (NOLOCK) 
		where ob.Id = b.id and ob.FabricCode = a.FabricCode
	)sr
	where a.id = @POID and a.FabricCode is not null and a.FabricCode !='' 
	and b.id = @POID    
	Order by inline,e.ID
	
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
	Declare @Markername varchar(20)
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
	Declare @ActCuttingPerimeter nvarchar(15)
	Declare @StraightLength varchar(15)
	Declare @CurvedLength varchar(15)
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
		From #Order_EachCons_Color_Layer a 
		outer apply(select top 1 A=0 from Order_EachCons_Article WITH (NOLOCK) where Order_EachConsUkey=a.Order_EachConsUkey)hasforArticle
		outer apply(select ct=count(1) from Order_EachCons_Article WITH (NOLOCK) where Order_EachConsUkey=a.Order_EachConsUkey)hasforArticleCt
		order by isnull(hasforArticle.A,1),hasforArticleCt.ct,MixedSizeMarker desc,MarkerName
		--------------------------------
		select b.id,b.article,b.sizecode,b.colorid,PatternPanel,b.orderqty, disqty,Min(INLINE) as inline,IDENTITY(int,1,1) as identRowid,a.SCIRefno,a.FabricPanelCode
		into #disQty
		from #WorkOrderMix a
		inner join #_tmpdisQty b on a.ColorID = b.ColorID and a.FabricCombo = b.PatternPanel and a.SCIRefno = b.SCIRefno and a.FabricPanelCode = b.FabricPanelCode
		inner join Order_EachCons_SizeQty oes on  oes.id = a.id and oes.Order_EachConsUkey = a.Order_EachConsUkey and b.SizeCode = oes.SizeCode
		outer apply(select top 1 A=0 from Order_EachCons_Article WITH (NOLOCK) where Order_EachConsUkey=a.Order_EachConsUkey)hasforArticle
		outer apply(select Article from Order_EachCons_Article  WITH (NOLOCK) where Order_EachConsUkey=a.Order_EachConsUkey)forArticle
		where (b.Article in (forArticle.Article) or hasforArticle.A is null)
		--and a.MarkerName = @MarkerName and a.ColorID = @colorid and a.FabricCombo = @FabricCombo and b.SizeCode = @sizecode  and a.SCIRefno = @SCIRefno
		group by b.id,b.article,b.sizecode,b.colorid,PatternPanel,b.orderqty, disqty,a.SCIRefno,a.FabricPanelCode
		order by inline
		--------------------Factory-------------------------------------
		Select @Factoryid = FtyGroup From Orders  WITH (NOLOCK)  Where ID = @Cuttingid
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
				   @Order_EachCons_ColorUkey = Order_EachCons_ColorUkey,
				   @ActCuttingPerimeter = ActCuttingPerimeter,
				   @StraightLength = StraightLength,
				   @CurvedLength = CurvedLength
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
				Select top 1 @seq1 = seq1 ,@Seq2 = SEQ2
				From PO_Supp_Detail b  WITH (NOLOCK) 
				Where id = @POID AND Scirefno = @SCIRefno and OutputSeq2 != '' AND Colorid = @colorid and SEQ1 like '7%' and Junk = 0
			End
			--先組依Article和Maxlayer的分配表#dis_tmpAL
			Begin
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
				declare @oldWorkerordernum int
				declare @newWorkerordernum int
				set @oldWorkerordernum = 0
				set @newWorkerordernum = 0

				--準備Cons
				SET @SizeRatioQty = 0
				Select @SizeRatioQty = sum(Qty)
				From Order_EachCons_SizeQty  WITH (NOLOCK) 
				Where Order_EachConsUkey = @ukey
					
				DECLARE cur_dis_tmpAL CURSOR FOR 
					Select byWorkorder,Layer from #dis_tmpAL;
				OPEN cur_dis_tmpAL
				FETCH NEXT FROM cur_dis_tmpAL INTO @newWorkerordernum,@Layer
				while @@FETCH_STATUS = 0
				Begin
					SET @Cons = @Layer * @SizeRatioQty * @ConsPC
					if(@oldWorkerordernum != @newWorkerordernum)
					Begin--byworkorder的Group與前一筆不一樣,則新增一筆
						Insert Into #NewWorkorder(ID,FactoryID,MDivisionid,SEQ1,SEQ2,OrderID,Layer,Colorid,MarkerName,MarkerLength,ConsPC,Cons,Refno,SCIRefno,Markerno,MarkerVersion,Type,AddName,AddDate,MarkerDownLoadId,FabricCombo,FabricCode,FabricPanelCode,newKey,Order_eachconsUkey,ActCuttingPerimeter,StraightLength,CurvedLength,[Shift])
						Values(@Cuttingid,@Factoryid,@mDivisionid,@seq1,@seq2,@Cuttingid,@Layer,@Colorid,@Markername,@MarkerLength,@ConsPC,@Cons,@Refno,@SCIRefno,@MarkerNo,@MarkerVerion,@Type,@username,GETDATE(),@MarkerDownLoadid,@FabricCombo,@FabricCode,@FabricPanelCode,@NewKey,@Order_EachConsUkey,@ActCuttingPerimeter,@StraightLength,@CurvedLength,'')
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
					DECLARE cur_distriqty_modlayer CURSOR FOR 
					Select A.sizecode,A.Qty * @Layer
					From Order_EachCons_SizeQty a  WITH (NOLOCK) ,Order_SizeCode b  WITH (NOLOCK)   
					Where a.Order_EachConsUkey = @ukey  and a.id = b.id and a.SizeCode = b.SizeCode 
					order by seq

					OPEN cur_distriqty_modlayer
					FETCH NEXT FROM cur_distriqty_modlayer INTO @Sizecode,@CutQty
					While @@FETCH_STATUS = 0
					Begin
						DECLARE cur_disQty  CURSOR FOR 
						Select disqty,orderqty,Article,identRowid,ID
						from #disQty 
						Where SizeCode = @sizeCode and Colorid = @colorid and PatternPanel = @FabricCombo and SCIRefno = @SCIRefno
						and FabricPanelCode=@FabricPanelCode
						--因為不同article要一起計算，拿掉 
						and (Article in(select Article from #LongArticle) or @LongArticleCount=0 )
						order by identRowid
							
						OPEN cur_disQty
						FETCH NEXT FROM cur_disQty INTO @distributeQty,@OrderQty,@Article,@WorkOrder_DisidenRow,@WorkOrder_DisOrderID
						While @@FETCH_STATUS = 0
						Begin
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
						FETCH NEXT FROM cur_disQty INTO @distributeQty,@OrderQty,@Article,@WorkOrder_DisidenRow,@WorkOrder_DisOrderID
						End
						CLOSE cur_disQty
						DEALLOCATE cur_disQty 

						if(@CutQty>0) ---若全分配完還有剩就要給EXCESS
						Begin
							insert into #NewWorkOrder_Distributetmp(ID,OrderID,Article,SizeCode,Qty,NewKey,WorkOrderUkey)
							Values(@Cuttingid, 'EXCESS','',@SizeCode,@CutQty,@NewKey-1,0)		
						End
					FETCH NEXT FROM cur_distriqty_modlayer INTO @Sizecode,@CutQty
					END
					CLOSE cur_distriqty_modlayer
					DEALLOCATE cur_distriqty_modlayer 
					----------------新增WorkOrder_SizeRatio-------------------------
					Begin						
							
						DECLARE cur_Order_EachCons_SizeQty CURSOR FOR 
						Select sizecode,Qty
						From Order_EachCons_SizeQty  WITH (NOLOCK) 
						Where Order_EachConsUkey = @ukey

						OPEN cur_Order_EachCons_SizeQty
						FETCH NEXT FROM cur_Order_EachCons_SizeQty INTO @SizeCode,@WorkOrder_SizeRatio_Qty
						While @@FETCH_STATUS = 0
						Begin
							Insert into #NewWorkOrder_SizeRatiotmp(ID,SizeCode,Qty,newKey,WorkOrderUkey)
							Values(@Cuttingid,@SizeCode,@WorkOrder_SizeRatio_Qty,@NewKey-1,0)
						FETCH NEXT FROM cur_Order_EachCons_SizeQty INTO @SizeCode,@WorkOrder_SizeRatio_Qty
						End;
						CLOSE cur_Order_EachCons_SizeQty
						DEALLOCATE cur_Order_EachCons_SizeQty 
					End
					----------------新增WorkOrder_PatternPanel----------------------
					Begin
						DECLARE  cur_Order_EachCons_PatternPanel CURSOR FOR
						Select PatternPanel,FabricPanelCode
						From Order_EachCons_PatternPanel  WITH (NOLOCK) 
						Where Order_EachConsUkey = @ukey

						OPEN cur_Order_EachCons_PatternPanel
						FETCH NEXT FROM cur_Order_EachCons_PatternPanel INTO @WorkOrder_PatternPanel,@WorkOrder_FabricPanelCode
						While @@FETCH_STATUS = 0
						Begin
							-- insert WorkOrder_PatternPanel
							Insert into #NewWorkOrder_PatternPaneltmp(ID,PatternPanel,FabricPanelCode,newKey,WorkOrderUkey)
							Values(@Cuttingid,@WorkOrder_PatternPanel,@WorkOrder_FabricPanelCode,@NewKey-1,0)
						FETCH NEXT FROM cur_Order_EachCons_PatternPanel INTO @WorkOrder_PatternPanel,@WorkOrder_FabricPanelCode	
						End;
						CLOSE cur_Order_EachCons_PatternPanel
						DEALLOCATE cur_Order_EachCons_PatternPanel 	
					End
					----------------此筆的3個子Table準備結束------------------------
				FETCH NEXT FROM cur_dis_tmpAL INTO @newWorkerordernum,@Layer
				End
				CLOSE cur_dis_tmpAL
				DEALLOCATE cur_dis_tmpAL 
			End
			drop table #dis_tmpAL
		Set @WorkOrderMixRowID += 1
		Drop table #LongArticle
		ENd --End WorkOrder Loop
		drop table #WorkOrderMix
	End;

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
						AddName,AddDate,FabricCombo,MarkerDownLoadId,FabricCode,FabricPanelCode,ActCuttingPerimeter,StraightLength,CurvedLength,[Shift])
						(Select id,factoryid,MDivisionId,SEQ1,SEQ2,CutRef,OrderID,CutplanID,Cutno,Layer,Colorid,Markername,
						EstCutDate,CutCellid,MarkerLength,ConsPC,Cons,Refno,SCIRefno,MarkerNo,MarkerVersion,Type,Order_EachconsUkey,
						AddName,AddDate,FabricCombo,MarkerDownLoadId,FabricCode,FabricPanelCode ,ActCuttingPerimeter,StraightLength,CurvedLength,''
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