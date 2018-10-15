USE [Production]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Willy>
-- Create date: <2017/11/13>
-- Description:	< Using in WareHouse_P45 Confirmed >
-- =============================================
CREATE PROCEDURE dbo.usp_RemoveScrapById
	@ID varchar(13)
	
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
		,balance = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0)
		,Adjustqty  = isnull(d.QtyBefore,0) - isnull(d.QtyAfter,0)
		,q = isnull(f.InQty,0)-isnull(f.OutQty,0)+isnull(f.AdjustQty,0) +(isnull(d.QtyAfter,0)-isnull(d.QtyBefore,0))
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

			--Update FtyInventory.AdjustQty
			Update f set f.AdjustQty = f.AdjustQty +(d.QtyAfter- d.QtyBefore) 
			from dbo.Adjust_Detail d WITH (NOLOCK) 
			inner join FtyInventory f WITH (NOLOCK) on d.POID = f.POID and d.Roll = f.Roll  and d.dyelot = f.dyelot and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2
			where 1=1
			and d.Id = @ID
			and f.StockType = 'O'

			--Update MDivisionPoDetail.LObQty
			update m2 set 
			LObQty = b.l
			from(
				select l=sum(a.InQty-a.OutQty+a.AdjustQty),POID,Seq1,Seq2
				from
				(
					select f.InQty,f.OutQty,f.AdjustQty,d.POID,d.Seq1,d.Seq2
					from dbo.Adjust_Detail d WITH (NOLOCK) 
					inner join FtyInventory f WITH (NOLOCK) on d.POID = f.POID and d.Seq1 =f.Seq1 and d.Seq2 = f.Seq2 and d.StockType = f.StockType
					inner join MDivisionPoDetail m WITH (NOLOCK) on m.POID = d.POID and m.Seq1 = d.Seq1 and m.Seq2 = d.Seq2
					where d.Id = @ID
					and f.StockType = 'O'
					group by d.POID,d.Seq1,d.Seq2,f.InQty,f.OutQty,f.AdjustQty
				)a
				group by POID,Seq1,Seq2
			)b
			,MDivisionPoDetail m2
			where m2.POID = b.POID and m2.Seq1 = b.Seq1 and m2.Seq2 = b.Seq2

			--Update Adjust.Status
			update Adjust set Status ='Confirmed' where id = @ID
		END
END Try
BEGIN CATCH
		IF XACT_STATE() <> 0 
			ROLLBACK TRANSACTION;
		EXECUTE usp_GetErrorInfo;
	END CATCH

	DROP TABLE #tmpAll

END
