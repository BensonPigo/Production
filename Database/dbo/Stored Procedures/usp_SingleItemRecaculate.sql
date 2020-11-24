-- =============================================
-- Author:		MIKE
-- Create date: 2016/05/24
-- Description:	重算單一項目庫存
-- =============================================
CREATE PROCEDURE [dbo].[usp_SingleItemRecaculate]
	@Ukey bigint,
	@Poid varchar(13),
	@Seq1 varchar(3) ,
	@Seq2 varchar(2) 
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @SQLCMD AS NVARCHAR(MAX);
	DECLARE @currentDT AS VARCHAR(25)  -- �O���ɶ��ϥ�

    BEGIN TRY
				BEGIN TRANSACTION

				-- 沒有MDivisionPoDetail資料，先新增一筆。 (才有ukey寫入至 FtyInventory) 
				IF @Ukey is null or @Ukey = ''
				BEGIN
					DECLARE @NewUkey as Table(ukey bigint not null);
					INSERT INTO MDivisionPoDetail (POID,Seq1,Seq2)
					OUTPUT inserted.Ukey INTO @NewUkey
					VALUES (@Poid,@Seq1,@seq2)
					SELECT @Ukey = ukey from @NewUkey;
				END
				
				-- FtyInventory 資料歸零
				SET @SQLCMD = N'UPDATE FtyInventory set inqty = 0,outqty = 0 ,adjustqty = 0
					where POID = @pPoid and Seq1 = @pSeq1 and Seq2 = @pSeq2';

				EXEC sp_executesql @SQLCMD , N'@pPoid varchar(13) ,@pSeq1 varchar(3), @pSeq2 varchar(2)'
				,@pPoid = @Poid ,@pSeq1 = @Seq1, @pSeq2 = @Seq2;

				-- Update Ftyinventory Bulk 倉
				With group6key as(
					SELECT b.poid,b.seq1,b.seq2,b.roll,b.Dyelot,
						sum(AInqty) AInqty,sum(AOutqty) AOutqty,sum(AAdjustQty) AAdjustQty 
						FROM View_TransactionList b WITH (NOLOCK)
						where b.poid = @Poid and b.seq1 = @Seq1 and b.seq2 = @Seq2 AND b.status in ('confirmed','Closed')
						group by b.poid,b.seq1,b.seq2,b.roll,b.Dyelot
				)		
				MERGE  DBO.FTYINVENTORY AS T
				USING group6key AS S
				ON T.POID = S.POID AND T.SEQ1 =  S.SEQ1 AND T.SEQ2 = S.SEQ2 AND T.ROLL = S.ROLL  and T.Dyelot = S.Dyelot  and t.stocktype='B'
				WHEN MATCHED THEN UPDATE set inqty = S.AInqty, outqty = s.AOutqty , adjustQty = AAdjustqty
				WHEN NOT MATCHED BY TARGET THEN 
				INSERT (MDivisionPoDetailUkey,poid,seq1,seq2,roll,stocktype,dyelot,inqty,outqty,adjustqty)
				values (@Ukey,S.POID,S.SEQ1,S.SEQ2,S.ROLL,'B',S.dyelot,S.AInqty,S.AOutqty,S.AAdjustQty)
				;

				-- Update Ftyinventory Inventory 倉
				With group6key as(
					SELECT b.poid,b.seq1,b.seq2,b.roll,b.Dyelot,
						sum(BInqty) BInqty,sum(BOutqty) BOutqty,sum(BAdjustQty) BAdjustQty 
						FROM View_TransactionList b WITH (NOLOCK)
						where b.poid = @Poid and b.seq1 = @Seq1 and b.seq2 = @Seq2 AND b.status in ('confirmed','Closed')
						group by b.poid,b.seq1,b.seq2,b.roll,b.Dyelot
				)
				MERGE  DBO.FTYINVENTORY AS T
				USING group6key AS S
				ON T.POID = S.POID AND T.SEQ1 =  S.SEQ1 AND T.SEQ2 = S.SEQ2 AND T.ROLL = S.ROLL  and T.Dyelot = S.Dyelot  and t.stocktype='I'
				WHEN MATCHED THEN UPDATE set inqty = S.BInqty, outqty = s.BOutqty , adjustQty = BAdjustQty
				WHEN NOT MATCHED BY TARGET THEN 
				INSERT (MDivisionPoDetailUkey,poid,seq1,seq2,roll,stocktype,dyelot,inqty,outqty,adjustqty)
				values (@Ukey,S.POID,S.SEQ1,S.SEQ2,S.ROLL,'I',S.dyelot,S.BInqty,S.BOutqty,S.BAdjustQty)
				;

				-- Update Ftyinventory Scrap 倉
				With group6key as(
					SELECT b.poid,b.seq1,b.seq2,b.roll,b.Dyelot,
						sum(CInqty) CInqty,sum(COutqty) COutqty,sum(CAdjustQty) CAdjustQty 
						FROM View_TransactionList b WITH (NOLOCK)
						where b.poid = @Poid and b.seq1 = @Seq1 and b.seq2 = @Seq2 AND b.status in ('confirmed','Closed')
						group by b.poid,b.seq1,b.seq2,b.roll,b.Dyelot
				)
				MERGE  DBO.FTYINVENTORY AS T
				USING group6key AS S
				ON T.POID = S.POID AND T.SEQ1 =  S.SEQ1 AND T.SEQ2 = S.SEQ2 AND T.ROLL = S.ROLL  and T.Dyelot = S.Dyelot  and t.stocktype='O'
				WHEN MATCHED THEN UPDATE set inqty = S.CInqty, outqty = s.COutqty , adjustQty = CAdjustQty
				WHEN NOT MATCHED BY TARGET THEN 
				INSERT (MDivisionPoDetailUkey,poid,seq1,seq2,roll,stocktype,dyelot,inqty,outqty,adjustqty)
				values (@Ukey,S.POID,S.SEQ1,S.SEQ2,S.ROLL,'O',S.dyelot,S.CInqty,S.COutqty,S.CAdjustQty)
				;

				-- Update MDivisionPoDetail
				 UPDATE MDivisionPoDetail 
				 SET INQTY = ISNULL(t.inqty,0), OutQty =  ISNULL(t.outqty,0) , AdjustQty =  ISNULL(t.adjustqty,0) , LInvQty =  ISNULL(t.BBalance ,0), LObQty =  ISNULL(t.CBalance,0)
				 FROM MDivisionPoDetail WITH (NOLOCK)
				 LEFT JOIN (SELECT a.PoId,a.seq1,a.seq2
						 , sum(inqty) inqty, sum(outqty) outqty, sum(adjustqty) adjustqty
						 , sum(BInqty) - sum(Boutqty) + sum(BAdjustQty) BBalance  
						 , sum(CInqty) - sum(COutqty) + sum(CAdjustQty) CBalance
						 FROM View_TransactionList a WITH (NOLOCK)
						 where a.PoId = @Poid and a.seq1 = @Seq1 and a.seq2 = @Seq2 AND a.status in ('confirmed','Closed')
						 group by a.PoId,a.seq1,a.seq2
						) T
				 on t.PoId = MDivisionPoDetail.POID and t.seq1 =MDivisionPoDetail.seq1 and t.seq2 =  MDivisionPoDetail.Seq2 
				 where ukey = @Ukey;

				COMMIT TRANSACTION;
				select @currentDT = CONVERT(VARCHAR,GETDATE(),13)
				RAISERROR ('%s-%s-%s be caculated on %s', 1,10, @Poid,@Seq1,@Seq2,@currentDT);

			END TRY
			BEGIN CATCH
				IF XACT_STATE() <> 0 -- 非0表示有交易
					ROLLBACK TRANSACTION;
				EXECUTE usp_GetErrorInfo;
			END CATCH;
END