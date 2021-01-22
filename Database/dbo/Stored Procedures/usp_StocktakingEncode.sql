

CREATE PROCEDURE [dbo].[usp_StocktakingEncode] 
	-- Add the parameters for the stored procedure here
	@StocktakingID varchar(13), -- 盤點單號
	@MDivisionid varchar(8),
	@Factoryid varchar(8),
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
		DECLARE	@newid varchar(13) -- adjust id
		DECLARE @poid varchar(13), @seq1 varchar(3), @seq2 varchar(2), @roll varchar(8), @dyelot varchar(4);
		DECLARE @err_msg nvarchar(2000);


		IF @StocktakingID=''
		BEGIN
			RAISERROR (N'Stocktaking ID can not be empty!', -- Message text.
               16, -- Severity.
               1 -- State.
               );
		END

		IF EXISTS(SELECT * FROM DBO.Adjust A WITH (NOLOCK) WHERE A.StocktakingID = @StocktakingID)
		BEGIN
			RAISERROR (N'There is a adjust transaction belong to this stocktaking ID', -- Message text.
               16, -- Severity.
               1 -- State.
               );
		END

		SELECT @Stocktype=S.Stocktype FROM DBO.Stocktaking S WITH (NOLOCK) WHERE ID=@StocktakingID;
		IF @Stocktype='' OR @Stocktype IS NULL
		BEGIN
			RAISERROR (N'Stocktype of stocktaking can not be empty!', -- Message text.
               16, -- Severity.
               1 -- State.
               );
		END

		-- 檢查欲產生的調整單明細是否有LOCK;無法調整的項目
		DECLARE CheckLock_cursor CURSOR FOR
		select f.POID,f.seq1,f.seq2,f.Roll,f.Dyelot --into #tmpCheckLock
		from dbo.Stocktaking_Detail sd WITH (NOLOCK)
		inner join dbo.FtyInventory f WITH (NOLOCK) on f.Ukey = sd.FtyInventoryUkey
		where sd.id = @StocktakingID and sd.QtyBefore != sd.QtyAfter and f.Lock = 1;
		OPEN CheckLock_cursor;
			FETCH NEXT FROM CheckLock_cursor INTO @poid,@seq1,@seq2,@roll,@dyelot;
		IF @poid is not null
		BEGIN
			WHILE @@FETCH_STATUS = 0
			BEGIN
				SET @err_msg = isnull(@err_msg,Char(13)+Char(10)) + @poid+'-'+@seq1+'-'+@seq2+'-'+@roll+'-'+@dyelot + ' is locked.'+char(13)+char(10);
				FETCH NEXT FROM CheckLock_cursor INTO @poid,@seq1,@seq2,@roll,@dyelot;
			END

			IF @err_msg is not null
			BEGIN
				RAISERROR (@err_msg, -- Message text.
               16, -- Severity.
               1 -- State.
               );
			END
		END
		CLOSE CheckLock_cursor;

		-- 檢查欲產生的調整單明細是否數量不足;無法調整的項目
		DECLARE CheckBalanceQty_cursor CURSOR FOR
			select f.POID,f.seq1,f.seq2,f.Roll,f.Dyelot --into #tmpCheckLock
			from dbo.Stocktaking_Detail sd WITH (NOLOCK)
			inner join dbo.FtyInventory f WITH (NOLOCK) on f.Ukey = sd.FtyInventoryUkey
			where sd.id = @StocktakingID and sd.QtyBefore != sd.QtyAfter 
			and f.InQty - f.OutQty + f.AdjustQty - f.ReturnQty + (sd.QtyAfter - sd.QtyBefore) < 0;
		OPEN CheckBalanceQty_cursor;
			FETCH NEXT FROM CheckBalanceQty_cursor INTO @poid,@seq1,@seq2,@roll,@dyelot;
		IF @poid is not null
		BEGIN
			WHILE @@FETCH_STATUS = 0
			BEGIN
				SET @err_msg = isnull(@err_msg,Char(13)+Char(10)) + @poid+'-'+@seq1+'-'+@seq2+'-'+@roll+'-'+@dyelot + ' ,Balance is not enough for adjust.'+char(13)+char(10);
				FETCH NEXT FROM CheckBalanceQty_cursor INTO @poid,@seq1,@seq2,@roll,@dyelot;
			END

			IF @err_msg is not null
			BEGIN
				RAISERROR (@err_msg, -- Message text.
               16, -- Severity.
               1 -- State.
               );
			END
		END
		CLOSE CheckBalanceQty_cursor;
		

		-- 取Bulk Adjust ID
		IF @Stocktype= 'B'
			EXEC	[dbo].[usp_getID]
					@keyword = @Factoryid,
					@docno = N'AB',
					@issuedate = NULL,
					@tablename = N'dbo.adjust',
					@newid = @newid OUTPUT;
		ELSE
			EXEC	[dbo].[usp_getID]
					@keyword = @Factoryid,
					@docno = N'AI',
					@issuedate = NULL,
					@tablename = N'dbo.adjust',
					@newid = @newid OUTPUT;

			

		-- 新增 調整單主檔 & 明細檔
		INSERT INTO [dbo].[Adjust]
					([ID]
					,[MDivisionID]
					,[FactoryID]
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
					,@FactoryID
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
        ,[POID]
        ,[Seq1]
        ,[Seq2]
        ,[Roll]
        ,[Dyelot]
        ,[StockType]
        ,[QtyBefore]
        ,[QtyAfter],IIF(QTYAFTER>QTYBEFORE,'00010','00011') FROM DBO.STOCKTAKING_DETAIL sd WITH (NOLOCK)
		WHERE sd.id = @StocktakingID and sd.QtyBefore != sd.QtyAfter;

		-- 更新庫存 MDivisionPoDetail & FtyInventory
		Select sd.POID,sd.seq1,sd.Seq2,sum(QtyAfter - QtyBefore) AdjustQty into #tmpAdjust1
		FROM DBO.STOCKTAKING_DETAIL sd WITH (NOLOCK)
		WHERE sd.id = @StocktakingID and sd.QtyBefore != sd.QtyAfter 
		group by sd.POID,sd.seq1,sd.Seq2
			
		IF @Stocktype ='B'
		BEGIN
			-- 更新 MdivisionPodetail
			--update dbo.MDivisionPoDetail  SET AdjustQty = AdjustQty +
			--(SELECT t.AdjustQty from #tmpAdjust1 t where t.MDivisionID = MDivisionPoDetail.MDivisionID
			--	and t.poid = MDivisionPoDetail.POID and t.seq1 = MDivisionPoDetail.Seq1 and t.Seq2 = MDivisionPoDetail.seq2)
			update a
			set a.AdjustQty = isnull(a.AdjustQty, 0) + ISNULL(b.AdjustQty, 0)
			from dbo.MDivisionPoDetail a
			inner join #tmpAdjust1 b on a.POID = b.POID and a.Seq1 = b.Seq1 and a.Seq2 = b.Seq2
		END
		ELSE
		BEGIN
			-- 更新 MdivisionPodetail (多update linvQty)
			update dbo.MDivisionPoDetail  SET MDivisionPoDetail.AdjustQty = MDivisionPoDetail.AdjustQty + t.AdjustQty,
			LInvQty = LInvQty + t.AdjustQty
			from dbo.MDivisionPoDetail  inner join #tmpAdjust1 t on t.poid = MDivisionPoDetail.POID 
			and t.seq1 = MDivisionPoDetail.Seq1 and t.Seq2 = MDivisionPoDetail.seq2
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