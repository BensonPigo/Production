


CREATE PROCEDURE [dbo].[usp_WarehouseClose] 
	-- Add the parameters for the stored procedure here
	@poid varchar(13) -- 採購母單單號
	,@MDivisionid varchar(8),
	@loginid varchar(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Declare variables used in error checking.
	DECLARE @ErrorVar INT;
	DECLARE @RowCountVar INT;
	DECLARE	@newid varchar(13) -- A2C id
	DECLARE @msg nvarchar(500) ='';

	BEGIN TRY
		BEGIN TRANSACTION;


		IF @poid=''
		BEGIN
			RAISERROR (N'POID can not be empty!', -- Message text.
               16, -- Severity.
               1 -- State.
               );
		END

		--IF EXISTS(SELECT * FROM DBO.Orders a WHERE A.POID = @poid AND a.WhseClose is not null and a.Junk = 0)
		--BEGIN
		--	RAISERROR (N'Already closed!!', -- Message text.
  --             16, -- Severity.
  --             1 -- State.
  --             );
		--END

		IF EXISTS(SELECT * FROM DBO.Orders A WHERE A.POID = @poid AND A.Finished =0 and a.Junk = 0)
		BEGIN
			SET @msg = N'Please PPIC Department to close related orders('+@poid+') first!!'
			RAISERROR (@msg, -- Message text.
               16, -- Severity.
               1 -- State.
               );
		END

		IF EXISTS(SELECT * FROM DBO.FtyInventory A WHERE A.StockType = 'B' AND A.Lock =1 and a.POID = @poid)
		BEGIN
			SET @msg = N'All materials('+@poid+') in Bulk stock must be not locked before close R''Mtl, Please check!!'
			RAISERROR (@msg, -- Message text.
               16, -- Severity.
               1 -- State.
               );
		END

		-- 新增 報廢單主檔 & 明細檔
		INSERT INTO [dbo].[SubTransfer]
				   ([Id]				   ,[MDivisionID]				   ,[Type]
				   ,[IssueDate]				   ,[Status]				   ,[Remark]
				   ,[AddName]				   ,[AddDate]				   ,[EditName]
				   ,[EditDate])
			VALUES
					(@poid
					,@MDivisionid
					,'D' -- A2C
					,GETDATE()
					,'Confirmed'
					,'Add by Warehouse Close'
					,@loginid
					,GETDATE()
					,@loginid
					,GETDATE()
					);

		INSERT INTO [dbo].[SubTransfer_Detail]
           ([ID]				,[FromFtyInventoryUkey]           ,[FromMDivisionID]           ,[FromPOID]
           ,[FromSeq1]           ,[FromSeq2]						,[FromRoll]					,[FromStockType]
           ,[FromDyelot]           
		   ,[ToMDivisionID]    ,[ToPOID]							,[ToSeq1]
           ,[ToSeq2]           ,[ToRoll]							,[ToStockType]				,[ToDyelot]
           ,[Qty])
		 SELECT [POID]
			,Ukey
			,[MDivisionID]
			,[POID]
			,[Seq1]
			,[Seq2]
			,[Roll]
			,'B' [FromStock]
			,[Dyelot]
			,[MDivisionID]
			,[POID]
			,[Seq1]
			,[Seq2]
			,[Roll]
			,'O'	[ToStock]
			,[Dyelot]
			,(InQty - OutQty + AdjustQty) [Qty]
			FROM DBO.FtyInventory sd 
			WHERE sd.poid = @poid and stocktype='B' and ISNULL(InQty,0.0) - ISNULL(OutQty,0.0) + ISNULL(AdjustQty,0.0) > 0 and lock=0;

		-- 更新庫存 MDivisionPoDetail & FtyInventory
		Select sd.MDivisionID,sd.POID,sd.seq1,sd.Seq2,sum(ISNULL(InQty,0.0) - ISNULL(OutQty,0.0) + ISNULL(AdjustQty,0.0)) ScrapQty into #tmpScrap
		FROM DBO.FtyInventory sd 
		WHERE sd.mdivisionid=@mdivisionid and sd.POID = @poid and stocktype='B' and ISNULL(InQty,0.0) - ISNULL(OutQty,0.0) + ISNULL(AdjustQty,0.0) > 0 and lock=0
		group by sd.MDivisionID,sd.POID,sd.seq1,sd.Seq2
			
		-- 更新 MdivisionPodetail
		update dbo.MDivisionPoDetail  
		SET OutQty = OutQty +
		(SELECT t.ScrapQty from #tmpScrap t where t.MDivisionID = MDivisionPoDetail.MDivisionID
			and t.poid = MDivisionPoDetail.POID and t.seq1 = MDivisionPoDetail.Seq1 and t.Seq2 = MDivisionPoDetail.seq2);

		drop table #tmpScrap;
		
		-- Insert Update FtyInventory (Scrap) 要先做Scrap倉再更新Bulk！！
		MERGE INTO dbo.FtyInventory AS t
		USING (SELECT mdivisionid,poid,seq1,seq2,roll,dyelot,ISNULL(InQty,0.0) - ISNULL(OutQty,0.0) + ISNULL(AdjustQty,0.0),'O' 
		FROM dbo.FtyInventory sd 
		WHERE sd.mdivisionid=@mdivisionid and sd.POID = @poid and stocktype='B' and ISNULL(InQty,0.0) - ISNULL(OutQty,0.0) + ISNULL(AdjustQty,0.0) > 0 and lock=0 ) 
		AS s (mdivisionid,poid,seq1,seq2,roll,dyelot,qty,stocktype)
		ON (t.MDivisionID = s.mdivisionid 
		and t.poid = s.poid 
		and t.seq1 = s.seq1 
		and t.seq2 = s.seq2 
		and t.roll = s.roll
		and t.dyelot = s.dyelot
		and t.stocktype = s.stocktype)
		WHEN MATCHED THEN
		UPDATE
		SET outqty = isnull(outqty,0) + s.qty
		WHEN NOT MATCHED THEN
		INSERT
		(mdivisionid, poid,seq1,seq2,roll,stocktype,dyelot,inqty,outqty,adjustqty,lock )
		VALUES (s.mdivisionid,s.poid,s.seq1,s.seq2,s.roll,s.stocktype,s.dyelot,s.qty,0,0,0);

		-- 更新 FtyInventory (Bulk)
		update dbo.FtyInventory 
		set OutQty = OutQty + sd.Qty
		FROM dbo.FtyInventory 
		inner join dbo.SubTransfer_Detail sd 
		on sd.id = @poid and  sd.FromFtyInventoryUkey = FtyInventory.Ukey;

		-- 更新Order
		update dbo.Orders set WhseClose = getdate() where POID = @poid;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF XACT_STATE() <> 0 -- 非0表示有交易
			ROLLBACK TRANSACTION;
		EXECUTE usp_GetErrorInfo;
	END CATCH;
    
END