
CREATE PROCEDURE [dbo].[usp_WarehouseClose] 
	-- Add the parameters for the stored procedure here
	@poid varchar(13) -- 採購母單單號
	,@MDivisionid varchar(8)
	,@Factoryid varchar(8)
	,@loginid varchar(10)
	,@NewID varchar(13)
    ,@FirstClose bit = 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	-- Declare variables used in error checking.
	DECLARE @ErrorVar INT;
	DECLARE @RowCountVar INT;
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

		IF @NewID=''
		BEGIN
			RAISERROR (N'ID can not be empty!', -- Message text.
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
        
        --Avoid using the WH.P01 dual-open program to use the close function
		IF @FirstClose = 1 AND EXISTS (SELECT 1 FROM Orders o WITH (NOLOCK) WHERE o.POID = @poid AND o.WhseClose IS NOT NULL)
		BEGIN
			SET @msg = N'Materials('+@poid+') has been closed'
			RAISERROR (@msg, -- Message text.
               16, -- Severity.
               1 -- State.
               );
		END

		IF EXISTS(SELECT * FROM [dbo].[SubTransfer] S WITH (NOLOCK)WHERE S.ID = @NewID AND S.Status!='Confirmed')
		BEGIN
			Delete FROM dbo.SubTransfer WHERE ID = @NewID  
			Delete FROM dbo.SubTransfer_Detail WHERE ID = @NewID  
		END
        
		-- 新增 報廢單主檔 & 明細檔
		IF EXISTS(SELECT * FROM [dbo].[SubTransfer] S WITH (NOLOCK) WHERE S.ID = @NewID AND S.Status='Confirmed')
			update [dbo].[SubTransfer] set [EditName]= @loginid , [EditDate] = GETDATE() WHERE ID = @NewID  
		ELSE 
		BEGIN
			INSERT INTO [dbo].[SubTransfer]
				   ([Id]				   ,[MDivisionID]				   ,[FactoryID]
				   ,[Type]
				   ,[IssueDate]				   ,[Status]				   ,[Remark]
				   ,[AddName]				   ,[AddDate]				   ,[EditName]
				   ,[EditDate], [POID])
			VALUES
					(@NewID
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
					,@poid
					);
		END
		
		--寫入 [SubTransfer_Detail] 從 Bulk 資訊填入 Scrap
		SELECT
			[ID]=@NewID
			,[FromFtyInventoryUkey]=Ukey
			,[FromMDivisionID]=''
			,[FromFactoryID] = (select FactoryID from Orders with (nolock) where ID = @poid)
			,[FromPOID]=[POID]
			,[FromSeq1]=[Seq1]
			,[FromSeq2]=[Seq2]
			,[FromRoll]=[Roll]
			,[FromStockType]='B'
			,[FromDyelot]=[Dyelot]
			,[ToMDivisionID]=''
			,[ToFactoryID] = (select FactoryID from Orders with (nolock) where ID = @poid)
			,[ToPOID]=[POID]
			,[ToSeq1]=[Seq1]
			,[ToSeq2]=[Seq2]
			,[ToRoll]=[Roll]
			,[ToStockType]='O'
			,[ToDyelot]=[Dyelot]
			,[Qty]=ISNULL(InQty,0.0) - ISNULL(OutQty,0.0) + ISNULL(AdjustQty,0.0) - ISNULL(ReturnQty,0.0)
			,[ToLocation] = isnull(stuff((
                select concat(',', d.MtlLocationID)
                from FtyInventory_Detail d WITH (NOLOCK)
                inner join MtlLocation m WITH (NOLOCK) on m.ID = d.MtlLocationID	
                where d.ukey = sd.Ukey 
                and d.MtlLocationID <> ''
                and m.StockType='O'
                AND m.Junk=0
			    for xml path(''))
			, 1, 1, ''), '')
		into #insert_SubTransfer_Detail
		FROM DBO.FtyInventory sd WITH (NOLOCK)
		WHERE sd.poid = @poid
		and stocktype='B'
		and ISNULL(InQty,0.0) - ISNULL(OutQty,0.0) + ISNULL(AdjustQty,0.0) - ISNULL(ReturnQty,0.0) > 0 
		and lock = 0

		INSERT INTO [dbo].[SubTransfer_Detail]
			([ID]             ,[FromFtyInventoryUkey]
			,[FromMDivisionID],[FromFactoryID]
			,[FromPOID]       ,[FromSeq1]          ,[FromSeq2]    ,[FromRoll]    ,[FromStockType]    ,[FromDyelot]           
			,[ToMDivisionID]  ,[ToFactoryID]						
			,[ToPOID]         ,[ToSeq1]            ,[ToSeq2]      ,[ToRoll]      ,[ToStockType]      ,[ToDyelot]
			,[Qty]            ,[ToLocation])
		select * 
		from #insert_SubTransfer_Detail
		
		-- 更新庫存 MDivisionPoDetail & FtyInventory

		-- Insert Update FtyInventory (Scrap) 要先做Scrap倉再更新Bulk！！
        -- 更新/寫入 C 倉
		MERGE INTO dbo.FtyInventory AS t
		USING (
		    SELECT poid,seq1,seq2,roll,dyelot,qty=ISNULL(InQty,0.0) - ISNULL(OutQty,0.0) + ISNULL(AdjustQty,0.0) - ISNULL(ReturnQty,0.0),stocktype='O' ,sd.Ukey
		    FROM dbo.FtyInventory sd WITH (NOLOCK)
		    WHERE sd.POID = @poid and stocktype='B' and ISNULL(InQty,0.0) - ISNULL(OutQty,0.0) + ISNULL(AdjustQty,0.0) - ISNULL(ReturnQty,0.0) > 0 and lock=0         
        ) s
		    ON  t.poid = s.poid 
		    and t.seq1 = s.seq1 
		    and t.seq2 = s.seq2 
		    and t.roll = s.roll
		    and t.dyelot = s.dyelot
		    and t.stocktype = s.stocktype
		WHEN MATCHED THEN
		    UPDATE
		    SET inqty = isnull(inqty, 0) + s.qty
		WHEN NOT MATCHED THEN
		    INSERT (poid,seq1,seq2,roll,stocktype,dyelot,inqty,outqty,adjustqty,lock )
		    VALUES (s.poid,s.seq1,s.seq2,s.roll,s.stocktype,s.dyelot,s.qty,0,0,0)
		;

		--寫入Scrap的FtyInventory時一併寫入Location資訊進FtyInventory_Detail        
		Insert into FtyInventory_Detail(Ukey, MtlLocationID)
		select f.Ukey, ToLocation = ToLocation.data
		from FtyInventory f
		inner join dbo.SubTransfer_Detail sd WITH (NOLOCK) on sd.ToPOID = f.POID and -- 上方新寫入的 SubTransfer_Detail
															  sd.ToSeq1 = f.Seq1 and
															  sd.ToSeq2 = f.Seq2 and
															  sd.ToRoll = f.Roll and
															  sd.ToDyelot = f.Dyelot and
															  sd.ToStockType = f.StockType
        outer apply(select data from [dbo].[SplitString](sd.ToLocation ,','))ToLocation --SubTransfer_Detail.ToLocation 值先拆解
		where f.StockType = 'O' 
        and sd.id = @NewID 
		and sd.ToLocation <> ''
		and not exists(select 1 from FtyInventory_Detail fd WITH (NOLOCK) where f.Ukey = fd.Ukey and ToLocation.data = fd.MtlLocationID)
		and exists(select 1 from MtlLocation m WITH (NOLOCK) where m.StockType = 'O' and m.id = ToLocation.data and m.Junk = 0) -- Location需存在MtlLocation

		-- FtyInventory, FtyInventory_Detail先更新完, 再更新 MDivisionPoDetail
		select
			f.POID, f.seq1, f.seq2,
			ScrapQty = ISNULL(InQty,0.0) - ISNULL(OutQty,0.0) + ISNULL(AdjustQty,0.0) - ISNULL(ReturnQty,0.0)
		into #afterInsertFtyInventory
		from FtyInventory f with(nolock)
		where f.poid = @poid and f.StockType = 'O'
        
        select f.*,        
			CLocation = stuff((
				select distinct concat(',',fd.MtlLocationID)
				from FtyInventory f2
				inner join FtyInventory_Detail fd WITH (NOLOCK) on f2.Ukey = fd.Ukey
				where f2.poid = f.poid and f2.seq1 = f.Seq1 and f2.seq2 = f.seq2
				and f2.StockType = 'O'
				and exists(select 1 from MtlLocation m WITH (NOLOCK) where m.StockType = 'O' and m.id = fd.MtlLocationID and m.Junk = 0)
				for xml path('')
			),1,1,'')            
		into #tmpScrap
        from(
		    select f.POID, f.seq1, f.seq2, ScrapQty = sum(ScrapQty)
		    from #afterInsertFtyInventory f
		    group by POID, seq1, seq2
        ) f

		update  a set
			a.OutQty = isnull(a.OutQty,0) + isnull(b.ScrapQty, 0)
		   ,a.LObQty = isnull(a.LObQty,0) + isnull(b.ScrapQty, 0)
		   ,a.CLocation = b.CLocation
		from dbo.MDivisionPoDetail a WITH (NOLOCK)
		inner join #tmpScrap b on a.poid = b.POID and a.seq1 = b.seq1 and a.seq2 = b.seq2;

		drop table #tmpScrap
        
		-- 更新 FtyInventory (Bulk)
		update dbo.FtyInventory 
		set OutQty = OutQty + sd.Qty
		FROM dbo.FtyInventory WITH (NOLOCK)
		inner join dbo.SubTransfer_Detail sd on sd.id = @NewID and sd.FromFtyInventoryUkey = FtyInventory.Ukey;
        
        if @FirstClose = 1
        Begin
		    -- 更新Order
		    update dbo.Orders set WhseClose = getdate() where POID = @poid;
        
		    ---close 需加上Local物料
		    IF EXISTS(SELECT 1 FROM [dbo].[SubTransferLocal]  WITH (NOLOCK)WHERE ID = @poid AND Status!='Confirmed')
		    BEGIN
			    Delete FROM dbo.SubTransferLocal WHERE ID = @poid  
			    Delete FROM dbo.SubTransferLocal_Detail WHERE ID = @poid  
		    END

		    IF EXISTS (SELECT 1 FROM SubTransferLocal WITH (NOLOCK) WHERE ID=@poid AND Status='Confirmed' )
		    BEGIN
			    UPDATE 	SubTransferLocal
			    SET [MDivisionID]=@MDivisionid,
			    [FactoryID]=@Factoryid,
			    [IssueDate]=GETDATE() ,			
			    [Status]='Confirmed',
			    EditName= @loginid,
			    EditDate=GETDATE()
			    WHERE ID=@poid 
		    END
		    ELSE
		    BEGIN
			    INSERT INTO SubTransferLocal
				    ([ID] ,[MDivisionID],[FactoryID],[IssueDate],[Type],[Status]   ,[Remark],[AddName],[AddDate] )
			    VALUES
				    (@poid,@MDivisionid ,@Factoryid ,GETDATE()  ,'D'  ,'Confirmed','Add by Warehouse Close',@loginid,GETDATE())
		    END

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
			     isnull(l.OrderID,'')
			    ,isnull(@MDivisionid,'')
			    ,isnull(l.OrderID,'')
			    ,isnull(l.Refno,'')
			    ,isnull(l.ThreadColorID,'')
			    ,isnull(l.ALocation,'')
			    ,''
			    ,l.InQty-l.OutQty+l.AdjustQty
		    from LocalInventory l where l.OrderID = @poid AND (l.InQty-l.OutQty+l.AdjustQty)!=0

		    Update LocalInventory
		    set OutQty= OutQty + (InQty-OutQty+AdjustQty)
			    ,LobQty= InQty-OutQty+AdjustQty
		    where OrderID = @poid
        End

		COMMIT TRANSACTION;

	END TRY
	BEGIN CATCH
		IF XACT_STATE() <> 0 -- 非0表示有交易
			ROLLBACK TRANSACTION;
		EXECUTE usp_GetErrorInfo;
	END CATCH;
    
END