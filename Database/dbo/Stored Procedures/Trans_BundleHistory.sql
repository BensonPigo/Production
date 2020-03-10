
-- =============================================
-- Author:		Jack	
-- Create date: 20191212
-- Description:	Bundle 相關table將舊資料移至歷史區。
-- =============================================
Create PROCEDURE [dbo].[Trans_BundleHistory]
	
AS
BEGIN 
	SET NOCOUNT ON; 

Begin try
	Begin Transaction 

		select distinct o.ID
		into #tmp_orders_ID
		from Orders o
		where o.Finished = 1
		and o.PulloutComplete = 1 
		and not exists(select 1 from Bundle_History where o.ID = OrderID)

		select *
		into #tmp_Bundle
		from Bundle b 
		where exists (select 1 from #tmp_orders_ID where id = b.Orderid)
		AND NOT EXISTS(
			select distinct ad.orderid 
			from ArtworkPO_Detail ad
			inner join ArtworkPO a on a.id = ad.id
			where PoQty > ApQty and OrderID= b.Orderid
			and a.Status='Approved'
		)

		select *
		into #tmp_Bundle_Detail
		from Bundle_Detail bd
		where exists(select 1 from #tmp_Bundle where id = bd.id)

		/*****************************  Bundle_Detail_Art  ************************************/
		Begin
			select *
			into #tmp_Bundle_Detail_Art
			from Bundle_Detail_Art bdr
			where exists (select 1 from #tmp_Bundle where id = bdr.ID)
			
			insert into Bundle_Detail_Art_History([Bundleno], [SubprocessId], [PatternCode], [ID],[PostSewingSubProcess],[NoBundleCardAfterSubprocess])
			select [Bundleno], [SubprocessId], [PatternCode], [ID],[PostSewingSubProcess],[NoBundleCardAfterSubprocess]
			from #tmp_Bundle_Detail_Art

			delete from bdr
			from Bundle_Detail_Art bdr
			where exists (select 1 from #tmp_Bundle_Detail_Art where Ukey = bdr.Ukey)

			drop table #tmp_Bundle_Detail_Art
		end

		/*****************************  Bundle_Detail_Allpart  ************************************/
		Begin
			select *
			into #tmp_Bundle_Detail_Allpart
			from Bundle_Detail_Allpart bdp
			where exists (select 1 from #tmp_Bundle where id = bdp.ID)

			insert into Bundle_Detail_Allpart_History([ID], [Patterncode], [PatternDesc], [parts],  [IsPair], [Location])
			select [ID], [Patterncode], [PatternDesc], [parts],  [IsPair], [Location]
			from #tmp_Bundle_Detail_Allpart

			delete from bdp
			from Bundle_Detail_Allpart bdp
			where exists (select 1 from #tmp_Bundle_Detail_Allpart where Ukey = bdp.Ukey)

			drop table #tmp_Bundle_Detail_Allpart
		end

		/*****************************  BundleInOut  ************************************/
		Begin
			select *
			into #tmp_BundleInOut
			from BundleInOut bt
			where exists (select 1 from #tmp_Bundle_Detail where BundleNo = bt.BundleNo)

			insert into BundleInOut_History([BundleNo], [SubProcessId], [InComing], [OutGoing], [AddDate], [EditDate], [SewingLineID], [LocationID], [RFIDProcessLocationID], [PanelNo], [CutCellID])
			select [BundleNo], [SubProcessId], [InComing], [OutGoing], [AddDate], [EditDate], [SewingLineID], [LocationID], [RFIDProcessLocationID], [PanelNo], [CutCellID]
			from #tmp_BundleInOut

			delete from bio
			from BundleInOut bio
			where exists (select 1 from #tmp_BundleInOut where BundleNo = bio.BundleNo and SubProcessId = bio.SubProcessId and RFIDProcessLocationID = bio.RFIDProcessLocationID)

			drop table #tmp_BundleInOut  
		end

		/*****************************  BundleTransfer  ************************************/
		Begin
			select *
			into #tmp_BundleTransfer
			from BundleTransfer bt
			where exists (select 1 from #tmp_Bundle_Detail where BundleNo = bt.BundleNo)
	 
			insert into BundleTransfer_History([Sid], [RFIDReaderId], [Type], [SubProcessId], [TagId], [BundleNo], [TransferDate], [AddDate], [LocationID], [RFIDProcessLocationID], [PanelNo], [CutCellID], [SewingLineID])
			select [Sid], [RFIDReaderId], [Type], [SubProcessId], [TagId], [BundleNo], [TransferDate], [AddDate], [LocationID], [RFIDProcessLocationID], [PanelNo], [CutCellID], [SewingLineID]
			from #tmp_BundleTransfer

			delete from bt
			from BundleTransfer bt
			where exists (select 1 from #tmp_BundleTransfer where BundleNo = bt.BundleNo)

			drop table #tmp_BundleTransfer 
		end

		/*****************************  Bundle_Detail  ************************************/
		Begin
			insert into Bundle_Detail_History([BundleNo], [Id], [BundleGroup], [Patterncode], [PatternDesc], [SizeCode], [Qty], [Parts], [Farmin], [FarmOut], [PrintDate], [IsPair], [Location])
			select [BundleNo], [Id], [BundleGroup], [Patterncode], [PatternDesc], [SizeCode], [Qty], [Parts], [Farmin], [FarmOut], [PrintDate], [IsPair], [Location]
			from #tmp_Bundle_Detail 

			delete from bd
			from Bundle_Detail bd
			where exists (select 1 from #tmp_Bundle_Detail where BundleNo = bd.BundleNo and Id = bd.Id) 

			drop table #tmp_Bundle_Detail 
		end

		/*****************************  Bundle  ************************************/
		Begin
			insert into Bundle_History([ID], [POID], [MDivisionid], [Sizecode], [Colorid], [Article], [PatternPanel], [Cutno], [Cdate], [Orderid], [Sewinglineid], [Item], [SewingCell], [Ratio], [Startno], [Qty], [PrintDate], [AllPart], [CutRef], [AddName], [AddDate], [EditName], [EditDate], [oldid], [FabricPanelCode], [IsEXCESS])
			select [ID], [POID], [MDivisionid], [Sizecode], [Colorid], [Article], [PatternPanel], [Cutno], [Cdate], [Orderid], [Sewinglineid], [Item], [SewingCell], [Ratio], [Startno], [Qty], [PrintDate], [AllPart], [CutRef], [AddName], [AddDate], [EditName], [EditDate], [oldid], [FabricPanelCode], [IsEXCESS]
			from #tmp_Bundle

			delete from b
			from Bundle b
			where exists (select 1 from #tmp_Bundle where Id = b.Id)

			drop table #tmp_orders_ID, #tmp_Bundle
		end

		/*****************************  End  ************************************/

	Commit Transaction 
END try
Begin Catch
	IF @@TRANCOUNT > 0
	Rollback Transaction

	DECLARE  @ErrorMessage  NVARCHAR(4000),  
			 @ErrorSeverity INT,    
			 @ErrorState    INT;
	SELECT     
		@ErrorMessage  = ERROR_MESSAGE(),    
		@ErrorSeverity = ERROR_SEVERITY(),   
		@ErrorState    = ERROR_STATE();

	RAISERROR (@ErrorMessage, -- Message text.    
				 @ErrorSeverity, -- Severity.    
				 @ErrorState -- State.    
			   ); 
End Catch


END

GO


