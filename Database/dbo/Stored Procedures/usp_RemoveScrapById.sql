-- =============================================
-- Author:		<Willy>
-- Create date: <2017/11/13>
-- Description:	< Using in WareHouse_P45 Confirmed >
-- =============================================
CREATE PROCEDURE dbo.usp_RemoveScrapById
	@ID varchar(13),
	@UserID varchar(10)
AS
BEGIN	
	SET NOCOUNT ON;

--¨ÌSP#+SEQ#+Roll#+ StockType = 'O' ÀË¬d®w¦s¬O§_¨¬°÷
Select  d.POID
		,seq = concat(d.Seq1,'-',d.Seq2)
		,d.Seq1
		,d.Seq2
		,d.Roll
		,d.Dyelot
		,balance = isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0)
		,Adjustqty  = isnull(d.QtyBefore,0) - isnull(d.QtyAfter,0)
		,q = isnull(f.InQty,0) - isnull(f.OutQty,0) + isnull(f.AdjustQty,0) - isnull(f.ReturnQty,0) + (isnull(d.QtyAfter,0) - isnull(d.QtyBefore,0))
into #tmpAll
from dbo.Adjust_Detail d WITH (NOLOCK) 
inner join FtyInventory f WITH (NOLOCK) on d.POID = f.POID and d.Roll = f.Roll and d.dyelot = f.dyelot and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2
where 1=1
and d.Id = @ID
and f.StockType = 'O'

Declare @sql NVARCHAR(MAX)

--q < 0 Balacne Qty is not enough!!
BEGIN Try
	IF Exists(select * from #tmpAll where q<0)
		BEGIN			
			SET @sql=N'select * from #tmpAll where q < 0'
			EXEC sp_executesql @sql
		END 
	ELSE
		BEGIN
			declare @POID varchar(13)
					, @seq1 varchar(3)
					, @seq2 varchar(3)
					, @Roll varchar(8)
					, @Dyelot varchar(8)
					, @StockType varchar(1)
					, @AdjustQty numeric(11, 2)


			DECLARE _cursor CURSOR FOR
			select ad.POID, ad.Seq1, ad.Seq2, ad.Roll, ad.Dyelot, ad.StockType, [AdjustQty] = (ad.QtyAfter- ad.QtyBefore) 
			from Adjust_Detail ad
			where ad.id	 = @ID

			OPEN _cursor
			FETCH NEXT FROM _cursor INTO @POID, @seq1, @seq2, @Roll, @Dyelot, @StockType, @AdjustQty
			WHILE @@FETCH_STATUS = 0
			BEGIN
				--Update FtyInventory.AdjustQty
				update f
					set [AdjustQty] = f.AdjustQty + @AdjustQty
				from FtyInventory f
				where f.POID = @POID
				and f.Seq1 = @seq1
				and f.Seq2 = @seq2
				and f.Roll = @Roll
				and f.Dyelot = @Dyelot
				and f.StockType = 'O'

				--Update MDivisionPoDetail.LObQty
				update m
					set [LObQty] = m.LObQty + @AdjustQty  
				from MDivisionPoDetail m
				where m.POID = @POID
				and m.Seq1 = @seq1
				and m.Seq2 = @seq2

				FETCH NEXT FROM _cursor INTO @POID, @seq1, @seq2, @Roll, @Dyelot, @StockType, @AdjustQty
			END
			CLOSE _cursor
			DEALLOCATE _cursor

			--Update Adjust.Status
			update Adjust set Status ='Confirmed', EditName = @UserID, EditDate = Getdate() where id = @ID
		END
END Try
BEGIN CATCH
		IF XACT_STATE() <> 0 
			ROLLBACK TRANSACTION;
		EXECUTE usp_GetErrorInfo;
	END CATCH

	DROP TABLE #tmpAll

END