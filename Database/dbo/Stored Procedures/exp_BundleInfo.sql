CREATE PROCEDURE [dbo].[exp_BundleInfo]
as
begin
	/*
 * 訂單類: orders, order_Qty
 * */  
-- orders: 28619
select	o.ID	  ,
		o.BrandID		  ,
		o.ProgramID	  ,
		o.StyleID		  ,
		o.SeasonID	  ,
		o.Category	  ,
		o.Qty			  ,
		o.FactoryID	  ,
		o.BuyerDelivery ,
		o.SciDelivery	  ,
		o.SewInLine	  ,
		o.SewOffLine	  ,
		o.Junk		  ,
		o.POID		  ,
		o.AddName		  ,
		o.AddDate		  ,
		o.EditName	  ,
		o.EditDate	  ,
		o.SewLine		  ,
		o.Finished	  ,
		o.FtyGroup	  ,
		o.MDivisionID 
into #tmp_order
from Orders o with(nolock)
where 1=1
and o.SciDelivery >= DATEADD(MONTH , -2, getdate())

-- order_Qty: 130370
select	oo.ID		  ,
		oo.Article	  ,
		oo.SizeCode	  ,
		oo.Qty		  ,
		oo.OriQty
into #tmp_order_Qty
from #tmp_order tmp with(nolock)
inner join order_Qty oo with(nolock) on tmp.id=oo.ID
where 1=1

--Order_ColorCombo
select	Id				 ,
		Article			 ,
		ColorID			 ,
		FabricCode		 ,
		FabricPanelCode	 ,
		PatternPanel	 ,
		AddName			 ,
		AddDate			 ,
		EditName		 ,
		EditDate		 ,
		FabricType
		into #tmpOrder_ColorCombo
from Order_ColorCombo with (nolock)
where ID in (select ID from #tmp_order)


---------------------------------------------------------------------------------------------
/*
 * 綁包類: bundle, bundle_detail, Bundle_Detail_Allpart, Bundle_Detail_Art, Bundle_Detail_Order
 * */ 
-- Bundle_Detail_Order: 459157
select	b.Ukey		,
		b.ID		,
		b.BundleNo	,
		b.OrderID	,
		b.Qty
into #tmp_Bundle_Detail_Order
from Bundle_Detail_Order b with(nolock)
inner join #tmp_order tmp with(nolock) on b.OrderID =tmp.id

-- bundle_detail: 347391
select	b.BundleNo		 ,
		b.Id			 ,
		b.BundleGroup	 ,
		b.Patterncode	 ,
		b.PatternDesc	 ,
		b.SizeCode		 ,
		b.Qty			 ,
		b.Parts			 ,
		b.Farmin		 ,
		b.FarmOut		 ,
		b.PrintDate		 ,
		b.IsPair		 ,
		b.Location		 ,
		b.RFUID			 ,
		b.Tone
into #tmp_bundle_detail
from bundle_detail b with(nolock)
where exists (select 1 from #tmp_Bundle_Detail_Order tmp with(nolock) where b.BundleNo = tmp.bundleno and b.id=tmp.id)

-- bundle: 62447
select	b.ID			 ,
		b.POID			 ,
		b.MDivisionid	 ,
		b.Sizecode		 ,
		b.Colorid		 ,
		b.Article		 ,
		b.PatternPanel	 ,
		b.Cutno			 ,
		b.Cdate			 ,
		b.Orderid		 ,
		b.Sewinglineid	 ,
		b.Item			 ,
		b.SewingCell	 ,
		b.Ratio			 ,
		b.Startno		 ,
		b.Qty			 ,
		b.PrintDate		 ,
		b.AllPart		 ,
		b.CutRef		 ,
		b.AddName		 ,
		b.AddDate		 ,
		b.EditName		 ,
		b.EditDate		 ,
		b.oldid			 ,
		b.FabricPanelCode,
		b.IsEXCESS
into #tmp_bundle
from bundle b with(nolock)
where exists (select 1 from #tmp_bundle_detail tmp where b.id=tmp.id)

-- Bundle_Detail_Allpart: 354182
 select bda.ID			,
		bda.Patterncode	,
		bda.PatternDesc	,
		bda.parts		,
		bda.Ukey		,
		bda.IsPair		,
		bda.Location
 into #tmp_Bundle_Detail_Allpart
 from Bundle_Detail_Allpart bda with (nolock)
 where bda.id in (select id from #tmp_bundle)

-- Bundle_Detail_Art: 401445
 select bda.Bundleno			  ,
		bda.SubprocessId		  ,
		bda.PatternCode			  ,
		bda.ID					  ,
		bda.Ukey				  ,
		bda.PostSewingSubProcess  ,
		bda.NoBundleCardAfterSubprocess
 into #tmp_Bundle_Detail_Art
 from Bundle_Detail_Art bda with (nolock)
 where exists (select 1 from #tmp_bundle_detail tmp where  tmp.id=bda.ID and tmp.bundleno=bda.Bundleno )

---------------------------------------------------------------------------------------------
/*-----
 * 基本檔 subprocess, machine
 * */ 
-- subprocess: 36
select	Id			  ,
		ArtworkTypeId ,
		IsRFIDProcess ,
		Junk,
		InOutRule
 into #tmp_SubProcess
 from SubProcess with(nolock) 

-- Machine: 9346
select	ID				  ,
		MachineBrandID	  ,
		RFIDCardNo		  ,
		Status			  
 into #tmp_Machine
 from [ExtendServer].machine.dbo.Machine with(nolock)

---------------------------------------------------------------------------------------------
/*-----
 * 用量主、副料號 Order_BOF, Order_BoA
 * */ 
-- bof: 113053
select	ob.Id			,
		ob.FabricCode	,
		ob.Refno		,
		ob.SCIRefno	,
		ob.SuppID,
		ob.Ukey
 into #tmp_bof
 from Order_BOF ob with(nolock)
 where id in (select poid from #tmp_order)

 -- boA: 536388
select	ob.Id		  ,
		ob.Ukey	  ,
		ob.Refno	  ,
		ob.SCIRefno  ,
		ob.SuppID
 into #tmp_boa
 from Order_BOA ob with(nolock)
 where id in (select poid from #tmp_order)

 SET XACT_ABORT ON
 --BEGIN TRANSACTION
	delete [RFID_Middle].[PMS_TO_RFID].dbo.Orders
	
	insert into [RFID_Middle].[PMS_TO_RFID].dbo.Orders(ID	  ,
												BrandID		  ,
												ProgramID	  ,
												StyleID		  ,
												SeasonID	  ,
												Category	  ,
												Qty			  ,
												FactoryID	  ,
												BuyerDelivery ,
												SciDelivery	  ,
												SewInLine	  ,
												SewOffLine	  ,
												Junk		  ,
												POID		  ,
												AddName		  ,
												AddDate		  ,
												EditName	  ,
												EditDate	  ,
												SewLine		  ,
												Finished	  ,
												FtyGroup	  ,
												MDivisionID )
			select	ID	  ,
					BrandID		  ,
					ProgramID	  ,
					StyleID		  ,
					SeasonID	  ,
					Category	  ,
					Qty			  ,
					FactoryID	  ,
					BuyerDelivery ,
					SciDelivery	  ,
					SewInLine	  ,
					SewOffLine	  ,
					Junk		  ,
					POID		  ,
					AddName		  ,
					AddDate		  ,
					EditName	  ,
					EditDate	  ,
					SewLine		  ,
					Finished	  ,
					FtyGroup	  ,
					MDivisionID
			from #tmp_order

	delete [RFID_Middle].[PMS_TO_RFID].dbo.Bundle
	insert into [RFID_Middle].[PMS_TO_RFID].dbo.Bundle(ID			 ,
												POID			 ,
												MDivisionid	 ,
												Sizecode		 ,
												Colorid		 ,
												Article		 ,
												PatternPanel	 ,
												Cutno			 ,
												Cdate			 ,
												Orderid		 ,
												Sewinglineid	 ,
												Item			 ,
												SewingCell	 ,
												Ratio			 ,
												Startno		 ,
												Qty			 ,
												PrintDate		 ,
												AllPart		 ,
												CutRef		 ,
												AddName		 ,
												AddDate		 ,
												EditName		 ,
												EditDate		 ,
												oldid			 ,
												FabricPanelCode,
												IsEXCESS)
				select	ID			 ,
						POID			 ,
						MDivisionid	 ,
						Sizecode		 ,
						Colorid		 ,
						Article		 ,
						PatternPanel	 ,
						Cutno			 ,
						Cdate			 ,
						Orderid		 ,
						Sewinglineid	 ,
						Item			 ,
						SewingCell	 ,
						Ratio			 ,
						Startno		 ,
						Qty			 ,
						PrintDate		 ,
						AllPart		 ,
						CutRef		 ,
						AddName		 ,
						AddDate		 ,
						EditName		 ,
						EditDate		 ,
						oldid			 ,
						FabricPanelCode,
						IsEXCESS
				from #tmp_bundle

	delete [RFID_Middle].[PMS_TO_RFID].dbo.Bundle_Detail
	insert into [RFID_Middle].[PMS_TO_RFID].dbo.Bundle_Detail(	BundleNo		 ,
														Id			 ,
														BundleGroup	 ,
														Patterncode	 ,
														PatternDesc	 ,
														SizeCode		 ,
														Qty			 ,
														Parts			 ,
														Farmin		 ,
														FarmOut		 ,
														PrintDate		 ,
														IsPair		 ,
														Location		 ,
														RFUID			 ,
														Tone)
				select	BundleNo		 ,
						Id			 ,
						BundleGroup	 ,
						Patterncode	 ,
						PatternDesc	 ,
						SizeCode		 ,
						Qty			 ,
						Parts			 ,
						Farmin		 ,
						FarmOut		 ,
						PrintDate		 ,
						IsPair		 ,
						Location		 ,
						RFUID			 ,
						Tone
				from	#tmp_bundle_detail

	delete [RFID_Middle].[PMS_TO_RFID].dbo.Bundle_Detail_Allpart
	insert into [RFID_Middle].[PMS_TO_RFID].dbo.Bundle_Detail_Allpart(	ID			,
																Patterncode	,
																PatternDesc	,
																parts		,
																Ukey		,
																IsPair		,
																Location)
				select	ID			,
						Patterncode	,
						PatternDesc	,
						parts		,
						Ukey		,
						IsPair		,
						Location
				from #tmp_Bundle_Detail_Allpart

	delete [RFID_Middle].[PMS_TO_RFID].dbo.Bundle_Detail_Art
	insert into [RFID_Middle].[PMS_TO_RFID].dbo.Bundle_Detail_Art(	Bundleno			  ,
															SubprocessId		  ,
															PatternCode			  ,
															ID					  ,
															Ukey				  ,
															PostSewingSubProcess  ,
															NoBundleCardAfterSubprocess)
				select	Bundleno			  ,
						SubprocessId		  ,
						PatternCode			  ,
						ID					  ,
						Ukey				  ,
						PostSewingSubProcess  ,
						NoBundleCardAfterSubprocess
				from #tmp_Bundle_Detail_Art

	delete [RFID_Middle].[PMS_TO_RFID].dbo.Bundle_Detail_Order
	insert into [RFID_Middle].[PMS_TO_RFID].dbo.Bundle_Detail_Order(	Ukey		,
																ID		,
																BundleNo	,
																OrderID	,
																Qty)
				select	Ukey		,
						ID		,
						BundleNo	,
						OrderID	,
						Qty
				from #tmp_Bundle_Detail_Order

	delete [RFID_Middle].[PMS_TO_RFID].dbo.Machine
	insert into [RFID_Middle].[PMS_TO_RFID].dbo.Machine(ID				  ,
												 MachineBrandID	  ,
												 RFIDCardNo		  ,
												 Status)
				select	ID				  ,
						MachineBrandID	  ,
						RFIDCardNo		  ,
						Status
				from #tmp_Machine

	delete [RFID_Middle].[PMS_TO_RFID].dbo.Order_BOA
	insert into	[RFID_Middle].[PMS_TO_RFID].dbo.Order_BOA(	Id		  ,
													Ukey	  ,
													Refno	  ,
													SCIRefno  ,
													SuppID)
				select	Id		  ,
						Ukey	  ,
						Refno	  ,
						SCIRefno  ,
						SuppID
				from #tmp_boa

	delete [RFID_Middle].[PMS_TO_RFID].dbo.Order_BOF
	insert into [RFID_Middle].[PMS_TO_RFID].dbo.Order_BOF(	Id			,
													Ukey	  ,
													FabricCode	,
													Refno		,
													SCIRefno	,
													SuppID)
				select	Id			,
						Ukey	  ,
						FabricCode	,
						Refno		,
						SCIRefno	,
						SuppID
				from #tmp_bof

	delete [RFID_Middle].[PMS_TO_RFID].dbo.Order_Qty
	insert into [RFID_Middle].[PMS_TO_RFID].dbo.Order_Qty(	ID		  ,
													Article	  ,
													SizeCode	  ,
													Qty		  ,
													OriQty)
				select	ID		  ,
						Article	  ,
						SizeCode	  ,
						Qty		  ,
						OriQty
				from #tmp_order_Qty
	
	delete [RFID_Middle].[PMS_TO_RFID].dbo.Order_ColorCombo

	insert into [RFID_Middle].[PMS_TO_RFID].dbo.Order_ColorCombo(Id				   ,
																 Article		   ,
																 ColorID		   ,
																 FabricCode		   ,
																 FabricPanelCode   ,
																 PatternPanel	   ,
																 AddName		   ,
																 AddDate		   ,
																 EditName		   ,
																 EditDate		   ,
																 FabricType
																 )
				select	Id				   ,
						Article		   ,
						ColorID		   ,
						FabricCode		   ,
						FabricPanelCode   ,
						PatternPanel	   ,
						AddName		   ,
						AddDate		   ,
						EditName		   ,
						EditDate		   ,
						FabricType
				from #tmpOrder_ColorCombo

	delete [RFID_Middle].[PMS_TO_RFID].dbo.[Order_EachCons]
	INSERT INTO [RFID_Middle].[PMS_TO_RFID].dbo.[Order_EachCons]
			   ([Id]
			   ,[Ukey]
			   ,[Seq]
			   ,[MarkerName]
			   ,[FabricCombo]
			   ,[MarkerLength]
			   ,[FabricPanelCode]
			   ,[ConsPC]
			   ,[CuttingPiece]
			   ,[ActCuttingPerimeter]
			   ,[StraightLength]
			   ,[FabricCode]
			   ,[CurvedLength]
			   ,[Efficiency]
			   ,[Article]
			   ,[Remark]
			   ,[MixedSizeMarker]
			   ,[MarkerNo]
			   ,[MarkerUpdate]
			   ,[MarkerUpdateName]
			   ,[AllSize]
			   ,[PhaseID]
			   ,[SMNoticeID]
			   ,[MarkerVersion]
			   ,[Direction]
			   ,[CuttingWidth]
			   ,[Width]
			   ,[TYPE]
			   ,[AddName]
			   ,[AddDate]
			   ,[EditName]
			   ,[EditDate]
			   ,[isQT]
			   ,[MarkerDownloadID])
	select
				oe.[Id]
			   ,oe.[Ukey]
			   ,oe.[Seq]
			   ,oe.[MarkerName]
			   ,oe.[FabricCombo]
			   ,oe.[MarkerLength]
			   ,oe.[FabricPanelCode]
			   ,oe.[ConsPC]
			   ,oe.[CuttingPiece]
			   ,oe.[ActCuttingPerimeter]
			   ,oe.[StraightLength]
			   ,oe.[FabricCode]
			   ,oe.[CurvedLength]
			   ,oe.[Efficiency]
			   ,oe.[Article]
			   ,oe.[Remark]
			   ,oe.[MixedSizeMarker]
			   ,oe.[MarkerNo]
			   ,oe.[MarkerUpdate]
			   ,oe.[MarkerUpdateName]
			   ,oe.[AllSize]
			   ,oe.[PhaseID]
			   ,oe.[SMNoticeID]
			   ,oe.[MarkerVersion]
			   ,oe.[Direction]
			   ,oe.[CuttingWidth]
			   ,oe.[Width]
			   ,oe.[TYPE]
			   ,oe.[AddName]
			   ,oe.[AddDate]
			   ,oe.[EditName]
			   ,oe.[EditDate]
			   ,oe.[isQT]
			   ,oe.[MarkerDownloadID]
	from [Order_EachCons] oe
	inner join #tmp_order t on t.ID = oe.id
	
	delete [RFID_Middle].[PMS_TO_RFID].dbo.[Order_EachCons_Color]	
	INSERT INTO [RFID_Middle].[PMS_TO_RFID].dbo.[Order_EachCons_Color]	
			   ([Id]
			   ,[Order_EachConsUkey]
			   ,[Ukey]
			   ,[ColorID]
			   ,[CutQty]
			   ,[Layer]
			   ,[Orderqty]
			   ,[SizeList]
			   ,[Variance]
			   ,[YDS])
	select
				oec.[Id]
			   ,oec.[Order_EachConsUkey]
			   ,oec.[Ukey]
			   ,oec.[ColorID]
			   ,oec.[CutQty]
			   ,oec.[Layer]
			   ,oec.[Orderqty]
			   ,oec.[SizeList]
			   ,oec.[Variance]
			   ,oec.[YDS]
	from [Order_EachCons_Color]	oec
	inner join #tmp_order t on t.ID = oec.id

	delete [RFID_Middle].[PMS_TO_RFID].dbo.SubProcess
	insert into [RFID_Middle].[PMS_TO_RFID].dbo.SubProcess(Id			  ,
													ArtworkTypeId ,
													IsRFIDProcess ,
													Junk,
													InOutRule)
				select	Id			  ,
						ArtworkTypeId ,
						IsRFIDProcess ,
						Junk,
						InOutRule
				from #tmp_SubProcess

				
------[WorkOrder]
	select w.[Ukey]
	into #tmpWorkOrderUkey
	from #tmp_order t
	inner join WorkOrder w on w.ID = t.ID

	delete [RFID_Middle].[PMS_TO_RFID].dbo.[WorkOrder]
	INSERT INTO [RFID_Middle].[PMS_TO_RFID].dbo.[WorkOrder]
			   ([Ukey]
			   ,[CuttingID]
			   ,[FactoryID]
			   ,[MDivisionId]
			   ,[CutRef]
			   ,[Cutno]
			   ,[Markername]
			   ,[FabricCombo]
			   ,[FabricPanelCode]
			   ,[Article]
			   ,[Colorid]
			   ,[Layers]
			   ,[CutQty]
			   ,[OrderID]
			   ,[SEQ1]
			   ,[SEQ2]
			   ,[Fabeta]
			   ,[WKETA]
			   ,[EstCutDate]
			   ,[SpreadingNoID]
			   ,[CutCellid]
			   ,[Shift]
			   ,[CutplanID]
			   ,[ActCutDate]
			   ,[EditDate]
			   ,[EditName]
			   ,[AddDate]
			   ,[AddName]
			   ,[MarkerNo]
			   ,[MarkerVersion]
			   ,[MarkerDownLoadId]
			   ,[EachconsMarkerNo]
			   ,[EachconsMarkerDownloadID]
			   ,[EachconsMarkerVersion]
			   ,[ActCuttingPerimeterNew]
			   ,[StraightLengthNew]
			   ,[CurvedLengthNew])
	select
		w.[Ukey]
		,w.ID
		,w.[FactoryID]
		,w.[MDivisionId]
		,w.[CutRef]
		,w.[Cutno]
		,w.[Markername]
		,w.[FabricCombo]
		,w.[FabricPanelCode]
		,article.article
		,w.[Colorid]
		,w.Layer
		,CutQty.CutQty
		,w.[OrderID]
		,w.[SEQ1]
		,w.[SEQ2]
		,fabeta.fabeta
		,w.[WKETA]
		,w.[EstCutDate]
		,w.[SpreadingNoID]
		,w.[CutCellid]
		,w.[Shift]
		,w.[CutplanID]
		,actcutdate.actcutdate
		,w.[EditDate]
		,w.[EditName]
		,w.[AddDate]
		,w.[AddName]
		,w.[MarkerNo]
		,w.[MarkerVersion]
		,w.[MarkerDownLoadId]
		,[EachconsMarkerNo] = oe.MarkerNo
		,[EachconsMarkerDownloadID] = oe.[MarkerVersion]
		,[EachconsMarkerVersion]  = oe.[MarkerVersion]
		,w.ActCuttingPerimeter
		,w.[StraightLength]
		,w.[CurvedLength]
	from #tmp_order t
	inner join WorkOrder w on w.ID = t.ID
	left join Order_EachCons oe on oe.Ukey = w.Order_EachconsUkey
	outer apply
	(
		select article = stuff(
		(
			Select distinct concat('/' ,Article)
			From WorkOrder_Distribute b WITH (NOLOCK) 
			Where b.workorderukey = w.Ukey and b.article!=''
			For XML path('')
		),1,1,'')
	) as article
	outer apply
	(
		select CutQty = stuff(
		(
			Select concat(', ', c.sizecode, '/ ', c.qty * w.layer)
			From WorkOrder_SizeRatio c WITH (NOLOCK) 
			Where  c.WorkOrderUkey =w.Ukey 
			For XML path('')
		),1,1,'')
	) as CutQty
	outer apply
	(
		Select fabeta = iif(e.Complete=1, e.FinalETA, iif(e.Eta is not null, e.eta, iif(e.shipeta is not null, e.shipeta,e.finaletd)))
		From PO_Supp_Detail e WITH (NOLOCK) 
		Where e.id = (Select distinct poid from orders WITH (NOLOCK) where orders.cuttingsp = w.ID) and e.seq1 = w.seq1 and e.seq2 = w.seq2
	) as fabeta
	outer apply
	(
		Select actcutdate = iif(sum(cut_b.Layer) = w.Layer, Max(cut.cdate),null)
		From cuttingoutput cut WITH (NOLOCK) 
		inner join cuttingoutput_detail cut_b WITH (NOLOCK) on cut.id = cut_b.id
		Where cut_b.workorderukey = w.Ukey and cut.Status != 'New' 
	)  as actcutdate
		
	delete [RFID_Middle].[PMS_TO_RFID].dbo.[WorkOrder_Distribute]
	INSERT INTO [RFID_Middle].[PMS_TO_RFID].dbo.[WorkOrder_Distribute]
			   ([WorkOrderUkey]
			   ,[CuttingID]
			   ,[OrderID]
			   ,[Article]
			   ,[SizeCode]
			   ,[Qty])
	select
		[WorkOrderUkey]
		,ID
		,[OrderID]
		,[Article]
		,[SizeCode]
		,[Qty]
	from [WorkOrder_Distribute] wd
	inner join #tmpWorkOrderUkey t on t.Ukey = wd.WorkOrderUkey
	
	delete [RFID_Middle].[PMS_TO_RFID].dbo.[WorkOrder_SizeRatio]
	INSERT INTO [RFID_Middle].[PMS_TO_RFID].dbo.[WorkOrder_SizeRatio]
			   ([WorkOrderUkey]
			   ,[CuttingID]
			   ,[SizeCode]
			   ,[Qty])
	select
		[WorkOrderUkey]
		,ID
		,[SizeCode]
		,[Qty]
	from [WorkOrder_SizeRatio] wd
	inner join #tmpWorkOrderUkey t on t.Ukey = wd.WorkOrderUkey


 --commit


--------------------------------------------------------------------------------------------- 
drop table #tmp_order, #tmp_order_Qty
 , #tmp_Bundle_Detail_Order, #tmp_bundle_detail, #tmp_bundle, #tmp_Bundle_Detail_Allpart, #tmp_Bundle_Detail_Art
 , #tmp_SubProcess, #tmp_Machine
 , #tmp_bof, #tmp_boa
 , #tmpOrder_ColorCombo
 , #tmpWorkOrderUkey
end