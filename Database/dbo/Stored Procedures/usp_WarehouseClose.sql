



CREATE PROCEDURE [dbo].[usp_WarehouseClose] 
	-- Add the parameters for the stored procedure here
	@poid varchar(13) -- 採購母單單號
	,@MDivisionid varchar(8)
	,@loginid varchar(10)
	,@Factoryid varchar(8)
	
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
	DECLARE @OrdersFactoryID varchar(8) ='';

	BEGIN TRY
		BEGIN TRANSACTION;


		IF @poid=''
		BEGIN
			RAISERROR (N'POID can not be empty!', -- Message text.
               16, -- Severity.
               1 -- State.
               );
		END

		IF EXISTS(SELECT * FROM DBO.Orders A WITH (NOLOCK) WHERE A.POID = @poid AND A.Finished =0 and a.Junk = 0)
		BEGIN
			SET @msg = N'Please PPIC Department to close related orders('+@poid+') first!!'
			RAISERROR (@msg, -- Message text.
               16, -- Severity.
               1 -- State.
               );
		END

		IF EXISTS(SELECT * FROM DBO.FtyInventory A WITH (NOLOCK) WHERE A.StockType = 'B' AND A.Lock =1 and a.POID = @poid)
		BEGIN
			SET @msg = N'All materials('+@poid+') in Bulk stock must be not locked before close R''Mtl, Please check!!'
			RAISERROR (@msg, -- Message text.
               16, -- Severity.
               1 -- State.
               );
		END

		IF EXISTS(SELECT * FROM [dbo].[SubTransfer] S WITH (NOLOCK)WHERE S.ID = @poid AND S.Status!='Confirmed')
		BEGIN
			Delete FROM dbo.SubTransfer WHERE ID = @poid  
			Delete FROM dbo.SubTransfer_Detail WHERE ID = @poid  
		END

		-- 新增 報廢單主檔 & 明細檔
		IF EXISTS(SELECT * FROM [dbo].[SubTransfer] S WITH (NOLOCK) WHERE S.ID = @poid AND S.Status='Confirmed')
			update [dbo].[SubTransfer] set [EditName]= @loginid , [EditDate] = GETDATE()		
		ELSE 
		BEGIN
			INSERT INTO [dbo].[SubTransfer]
				   ([Id]				   ,[MDivisionID]				   ,[FactoryID]
				   ,[Type]
				   ,[IssueDate]				   ,[Status]				   ,[Remark]
				   ,[AddName]				   ,[AddDate]				   ,[EditName]
				   ,[EditDate])
			VALUES
					(@poid
					,@MDivisionid
					,@Factoryid
					,'D' -- A2C
					,GETDATE()
					,'Confirmed'
					,'Add by Warehouse Close'
					,@loginid
					,GETDATE()
					,@loginid
					,GETDATE()
					);
		END

		select @OrdersFactoryID=FactoryID from Orders where ID=@poid

		INSERT INTO [dbo].[SubTransfer_Detail]
           ([ID]				,[FromFtyInventoryUkey]           ,[FromMDivisionID]           ,[FromFactoryID]
		   ,[FromPOID]
           ,[FromSeq1]           ,[FromSeq2]						,[FromRoll]					,[FromStockType]
           ,[FromDyelot]           
		   ,[ToMDivisionID]    ,[ToFactoryID]						,[ToPOID]					,[ToSeq1]
           ,[ToSeq2]           ,[ToRoll]							,[ToStockType]				,[ToDyelot]
           ,[Qty])
		 SELECT [POID]
			,Ukey
			,'' [FromMDivisionID] 
			,@OrdersFactoryID [FromFactoryID] 
			,[POID]
			,[Seq1]
			,[Seq2]
			,[Roll]
			,'B' [FromStock]
			,[Dyelot]
			,'' [ToMDivisionID]
			,@OrdersFactoryID [ToFactoryID] 
			,[POID]
			,[Seq1]
			,[Seq2]
			,[Roll]
			,'O'	[ToStock]
			,[Dyelot]
			,(InQty - OutQty + AdjustQty) [Qty]
			FROM DBO.FtyInventory sd WITH (NOLOCK)
			WHERE sd.poid = @poid and stocktype='B' and ISNULL(InQty,0.0) - ISNULL(OutQty,0.0) + ISNULL(AdjustQty,0.0) > 0 and lock=0;

		-- 更新庫存 MDivisionPoDetail & FtyInventory
		Select sd.POID,sd.seq1,sd.Seq2,sum(ISNULL(InQty,0.0) - ISNULL(OutQty,0.0) + ISNULL(AdjustQty,0.0)) ScrapQty into #tmpScrap
		FROM DBO.FtyInventory sd WITH (NOLOCK)
		WHERE sd.POID = @poid and stocktype='B' and ISNULL(InQty,0.0) - ISNULL(OutQty,0.0) + ISNULL(AdjustQty,0.0) > 0 and lock=0
		group by sd.POID,sd.seq1,sd.Seq2
			
		-- 更新 MdivisionPodetail
		--merge dbo.MDivisionPoDetail as target 
		--using #tmpScrap as src
		--on src.MDivisionID = target.MDivisionID and src.poid = target.POID and src.seq1 = target.Seq1 and src.Seq2 = target.seq2
		--when matched then
		--update 
		--set OutQty = isnull(OutQty,0) + isnull(src.ScrapQty, 0);

		update  a
		set a.OutQty = isnull(a.OutQty,0) + isnull(b.ScrapQty, 0)
		   ,a.LObQty = isnull(a.LObQty,0) + isnull(b.ScrapQty, 0)
		from dbo.MDivisionPoDetail a 
		inner join #tmpScrap b on
			a.poid = b.poid and
			a.seq1 = b.seq1 and
			a.seq2 = b.seq2;

		drop table #tmpScrap;
		
		-- Insert Update FtyInventory (Scrap) 要先做Scrap倉再更新Bulk！！
		MERGE INTO dbo.FtyInventory AS t
		USING (SELECT poid,seq1,seq2,roll,dyelot,ISNULL(InQty,0.0) - ISNULL(OutQty,0.0) + ISNULL(AdjustQty,0.0),'O' 
		FROM dbo.FtyInventory sd WITH (NOLOCK)
		WHERE sd.POID = @poid and stocktype='B' and ISNULL(InQty,0.0) - ISNULL(OutQty,0.0) + ISNULL(AdjustQty,0.0) > 0 and lock=0 ) 
		AS s (poid,seq1,seq2,roll,dyelot,qty,stocktype)
		ON ( t.poid = s.poid 
		and t.seq1 = s.seq1 
		and t.seq2 = s.seq2 
		and t.roll = s.roll
		and t.dyelot = s.dyelot
		and t.stocktype = s.stocktype)
		WHEN MATCHED THEN
		UPDATE
		SET inqty = isnull(inqty,0) + s.qty
		WHEN NOT MATCHED THEN
		INSERT
		(poid,seq1,seq2,roll,stocktype,dyelot,inqty,outqty,adjustqty,lock )
		VALUES (s.poid,s.seq1,s.seq2,s.roll,s.stocktype,s.dyelot,s.qty,0,0,0);

		-- 更新 FtyInventory (Bulk)
		update dbo.FtyInventory 
		set OutQty = OutQty + sd.Qty
		FROM dbo.FtyInventory 
		inner join dbo.SubTransfer_Detail sd 
		on sd.id = @poid and  sd.FromFtyInventoryUkey = FtyInventory.Ukey;

		-- 更新Order
		update dbo.Orders set WhseClose = getdate() where POID = @poid;

		---close 需加上Local物料
		INSERT INTO SubTransferLocal
			([ID] ,[MDivisionID],[FactoryID],[IssueDate],[Type],[Status]   ,[Remark],[AddName],[AddDate] )
		VALUES
			(@poid,@MDivisionid ,@Factoryid ,GETDATE()  ,'D'   ,'Confrimed','Add by Warehouse Close',@loginid,GETDATE())

		INSERT INTO [dbo].[SubTransferLocal_Detail]
           ([ID]
           ,[MDivisionID]
           ,[Poid]
           ,[Refno]
           ,[Color]
           ,[FromLocation]
           ,[ToLocation]
           ,[Qty])
		select 
			l.OrderID
			,@MDivisionid
			,l.OrderID
			,l.Refno
			,l.ThreadColorID
			,l.ALocation
			,''
			,l.InQty-l.OutQty+l.AdjustQty
		from LocalInventory l where l.OrderID = @poid AND (l.InQty-l.OutQty+l.AdjustQty)!=0

		Update LocalInventory
		set OutQty= OutQty + (InQty-OutQty+AdjustQty)
			,LobQty= InQty-OutQty+AdjustQty
		where OrderID = @poid

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH
		IF XACT_STATE() <> 0 -- 非0表示有交易
			ROLLBACK TRANSACTION;
		EXECUTE usp_GetErrorInfo;
	END CATCH;
    
END