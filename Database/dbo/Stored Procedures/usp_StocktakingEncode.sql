

CREATE PROCEDURE dbo.usp_StocktakingEncode 
	-- Add the parameters for the stored procedure here
	@StocktakingID varchar(13), -- 盤點單號
	@MDivisionid varchar(8),
	@loginid varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Declare variables used in error checking.
	DECLARE @ErrorVar INT;
	DECLARE @RowCountVar INT;

	BEGIN TRY
		BEGIN TRANSACTION;

		DECLARE @Stocktype as varchar(1);
		DECLARE @count_bulk as INT;
		DECLARE @count_inventory as INT;
		DECLARE	@newid varchar(13) -- bulk adjust id
		DECLARE	@newid2 varchar(13)-- Inventory adjust id

		IF @StocktakingID=''
		BEGIN
			RAISERROR ('Stocktaking ID can not be empty!', -- Message text.
               16, -- Severity.
               1 -- State.
               );
		END

		IF EXISTS(SELECT * FROM DBO.Adjust A WHERE A.StocktakingID = @StocktakingID)
		BEGIN
			RAISERROR ('There is a adjust transaction belong to this stocktaking ID', -- Message text.
               16, -- Severity.
               1 -- State.
               );
		END

		SELECT @Stocktype=S.Stocktype FROM DBO.Stocktaking S WHERE ID=@StocktakingID;
		IF @Stocktype='' OR @Stocktype IS NULL
		BEGIN
			RAISERROR ('Stocktype of stocktaking can not be empty!', -- Message text.
               16, -- Severity.
               1 -- State.
               );
		END

		select @count_bulk = count(1) from dbo.Stocktaking_Detail sd 
		where sd.id = @StocktakingID and sd.QtyBefore != sd.QtyAfter 

		-- 檢查欲產生的調整單明細是否有LOCK或數量不足無法調整的項目
		select sd.UKey,sd.QtyBefore - QtyAfter as qty into #tmpData
		from dbo.Stocktaking_Detail sd 
		where sd.id = @StocktakingID and sd.QtyBefore != sd.QtyAfter;

		

		-- 取Bulk Adjust ID
		IF @Stocktype= 'B'
			EXEC	[dbo].[usp_getID]
					@keyword = @MDivisionid,
					@docno = N'AB',
					@issuedate = NULL,
					@tablename = N'dbo.adjust',
					@newid = @newid OUTPUT;
		ELSE
			EXEC	[dbo].[usp_getID]
					@keyword = @MDivisionid,
					@docno = N'AI',
					@issuedate = NULL,
					@tablename = N'dbo.adjust',
					@newid = @newid OUTPUT;

			

		-- 新增 調整單主檔 & 明細檔
		INSERT INTO [dbo].[Adjust]
					([ID]
					,[MDivisionID]
					,[IssueDate]
					,[Remark]
					,[Status]
					,[AddName]
					,[AddDate]
					,[EditName]
					,[EditDate]
					,[Type]
					,[StocktakingID])
				VALUES
					(@newid
					,@MDivisionid
					,GETDATE()
					,'Add by stocktaking'
					,'Confirmed'
					,@loginid
					,GETDATE()
					,@loginid
					,GETDATE()
					,IIF(@Stocktype='B','A','B')
					,@StocktakingID);

		INSERT INTO DBO.Adjust_Detail
        ([ID]
        ,[FtyInventoryUkey]
        ,[MDivisionID]
        ,[POID]
        ,[Seq1]
        ,[Seq2]
        ,[Roll]
        ,[Dyelot]
        ,[StockType]
        ,[QtyBefore]
        ,[QtyAfter]
        ,[ReasonId]) 
		SELECT @newid
        ,[FtyInventoryUkey]
        ,[MDivisionID]
        ,[POID]
        ,[Seq1]
        ,[Seq2]
        ,[Roll]
        ,[Dyelot]
        ,[StockType]
        ,[QtyBefore]
        ,[QtyAfter],IIF(QTYAFTER>QTYBEFORE,'00010','00011') FROM DBO.STOCKTAKING_DETAIL sd 
		WHERE sd.id = @StocktakingID and sd.QtyBefore != sd.QtyAfter;

		-- 更新庫存 MDivisionPoDetail & FtyInventory
		Select sd.MDivisionID,sd.POID,sd.seq1,sd.Seq2,sum(QtyAfter - QtyBefore) AdjustQty into #tmpAdjust1
		FROM DBO.STOCKTAKING_DETAIL sd 
		WHERE sd.id = @StocktakingID and sd.QtyBefore != sd.QtyAfter 
		group by sd.MDivisionID,sd.POID,sd.seq1,sd.Seq2
			
		IF @Stocktype ='B'
		BEGIN
			-- 更新 MdivisionPodetail
			update dbo.MDivisionPoDetail  SET AdjustQty = AdjustQty +
			(SELECT t.AdjustQty from #tmpAdjust1 t where t.MDivisionID = MDivisionPoDetail.MDivisionID
				and t.poid = MDivisionPoDetail.POID and t.seq1 = MDivisionPoDetail.Seq1 and t.Seq2 = MDivisionPoDetail.seq2)
		END
		ELSE
		BEGIN
			-- 更新 MdivisionPodetail (多update linvQty)
			update dbo.MDivisionPoDetail  SET MDivisionPoDetail.AdjustQty = MDivisionPoDetail.AdjustQty + t.AdjustQty,
			LInvQty = LInvQty + t.AdjustQty
			from dbo.MDivisionPoDetail  inner join #tmpAdjust1 t on t.MDivisionID = MDivisionPoDetail.MDivisionID
			and t.poid = MDivisionPoDetail.POID and t.seq1 = MDivisionPoDetail.Seq1 and t.Seq2 = MDivisionPoDetail.seq2
		END

		drop table #tmpAdjust1;
		
		-- 更新 FtyInventory
		update dbo.FtyInventory set AdjustQty = AdjustQty + sd.QtyAfter - sd.QtyBefore
		FROM dbo.FtyInventory inner join DBO.STOCKTAKING_DETAIL sd 
		on sd.id = @StocktakingID and sd.QtyBefore != sd.QtyAfter and sd.FtyInventoryUkey = FtyInventory.Ukey

		-- 更新StockTaking 狀態
		Update dbo.Stocktaking set status = 'Confirmed',
								   AdjustId = @newid,
								   EditName = @loginid,
								   EditDate = GETDATE() where id= @StocktakingID ;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF XACT_STATE() <> 0 -- 非0表示有交易
			ROLLBACK TRANSACTION;
		EXECUTE usp_GetErrorInfo;
	END CATCH;
    
END