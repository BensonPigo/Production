-- =============================================
-- Author:		Mike
-- Create date: 2016/05/23
-- Description:	重計庫存
--				(1) 傳入交易單號，則重計該交易下有影響到的所有PO的所有項目。
--				(2) 傳入POID，則重計該PO下的所有項目。
--				(3) 傳入POID-SEQ1-SEQ2，則重計該項目。
-- =============================================
CREATE PROCEDURE [dbo].[usp_StockRecaculate]
	@MDivisionid varchar(8),
	@TransID varchar(13) = null,
	@Poid varchar(13) = null,
	@Seq1 varchar(3) = null,
	@Seq2 varchar(2) = null
AS	 
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
	
	SET NOCOUNT ON;

	DECLARE @SQLCMD AS NVARCHAR(MAX);
	DECLARE @tUkey as bigint;
	DECLARE @tMDivisionid as VARCHAR(8);	-- Cursor 暫存使用
	DECLARE @tPoid AS VARCHAR(13);	-- Cursor 暫存使用
	DECLARE @tSeq1 AS VARCHAR(3);	-- Cursor 暫存使用
	DECLARE @tSeq2 AS VARCHAR(2);	-- Cursor 暫存使用
	DECLARE @currentDT AS VARCHAR(25)  -- 記錄時間使用

	IF @MDivisionid = '' OR @MDivisionid IS NULL
	BEGIN
		RAISERROR (N'@MDivisionid can not be empty!', -- Message text.
            16, -- Severity.
            1 -- State.
            );
	END


	IF (@Seq1 = '' OR @Seq1 IS NULL) AND (@Seq1 IS NOT NULL OR @Seq2 != '')
	BEGIN
		RAISERROR (N'@Seq1 can not be empty!', -- Message text.
            16, -- Severity.
            1 -- State.
            );
	END

	IF (@Seq1 IS NOT NULL AND @SEQ1 !='')
	BEGIN
		SELECT @tUkey = M.Ukey FROM DBO.MDivisionPoDetail M 
		WHERE M.MDivisionID = @MDivisionid AND M.POID = @Poid AND M.SEQ1 = @Seq1 AND M.Seq2 = @Seq2;

		EXEC usp_SingleItemRecaculate @Ukey = @tUkey, @MDivisionid = @MDivisionid,@Poid = @Poid,@Seq1 = @Seq1,@Seq2 = @Seq2;
	END
	ELSE
	BEGIN
	-- 先抓出要重算庫存的目標群
	select * into #tmp_table from dbo.View_TransactionList where 1=2 ;

	select @currentDT = CONVERT(VARCHAR,GETDATE(),13)
	RAISERROR ('%s on %s', 1,1, 'Beginning to Insert #tmp_table',@currentDT);

	SET @SQLCMD = N'Insert into #tmp_table SELECT *
	From DBO.View_TransactionList WHERE MDivisionid = @pMDivisionid ';

	IF @TransID is not null
	SET @SQLCMD = @SQLCMD + ' AND ID = @pTransID';
		
	IF @Poid is not null
	SET @SQLCMD = @SQLCMD + ' AND POID = @pPoid';
		
	IF @Seq1 is not null
	SET @SQLCMD = @SQLCMD + ' AND SEQ1 = @pSeq1';
		
	IF @Seq2 is not null
	SET @SQLCMD = @SQLCMD + ' AND SEQ2 = @pSeq2';
	
	select @currentDT = CONVERT(VARCHAR,GETDATE(),13)
	RAISERROR ('%s on %s', 1,1, @SQLCMD,@currentDT);

	EXEC sp_executesql @SQLCMD , N'@pMDivisionid varchar(8), @pTransID varchar(13), @pPoid varchar(13), @pSeq1 varchar(3) ,@pSeq2 varchar(2) '
	,@pMDivisionid = @MDivisionid, @pTransID = @TransID, @pPoid = @Poid, @pSeq1 = @Seq1 , @pSeq2 = @Seq2 ;

	--- 依目標群先抓出POID的資料
	DECLARE PO_CURSOR CURSOR FOR  
	select distinct a.poid  from #tmp_table a;

	OPEN PO_CURSOR ;

	FETCH NEXT FROM PO_CURSOR INTO @tPoid;

	WHILE @@FETCH_STATUS = 0 
	BEGIN 
		-- PO下的項目先歸零
		SET @SQLCMD = N'UPDATE MDivisionPoDetail set inqty = 0,outqty = 0 ,adjustqty = 0, linvqty = 0 , lobqty = 0 
		where MDivisionid = @pMDivisionid and POID = @pPoid';
		
		select @currentDT = CONVERT(VARCHAR,GETDATE(),13)
		RAISERROR ('%s on %s', 1,1, @SQLCMD,@currentDT);

		EXEC sp_executesql @SQLCMD , N'@pMDivisionid varchar(8), @pPoid varchar(13)',@pMDivisionid = @MDivisionid, @pPoid = @tPoid ;

		--- 依PO抓出MDivisionPoDetail的資料
		DECLARE MYCURSOR CURSOR FOR  
		select distinct B.Ukey,B.MDivisionid,B.seq1,B.seq2  from dbo.MDivisionPoDetail B 
		WHERE B.MDivisionID = @MDivisionid AND B.POID = @tPoid;

		OPEN MYCURSOR ;

		FETCH NEXT FROM MYCURSOR INTO @tUkey,@tMDivisionid,@tSeq1,@tSeq2;

		WHILE @@FETCH_STATUS = 0 
		BEGIN 
			BEGIN TRY
				EXEC usp_SingleItemRecaculate @Ukey = @tUkey, @MDivisionid = @tMDivisionid,@Poid = @tPoid,@Seq1 = @tSeq1,@Seq2 = @tSeq2;
				select @currentDT = CONVERT(VARCHAR,GETDATE(),13)
				RAISERROR ('%s-%s-%s-%s be caculated on %s', 1,10, @tMDivisionid, @tPoid,@tSeq1,@tSeq2,@currentDT);

				-- 抓下一筆
				FETCH NEXT FROM MYCURSOR INTO @tUkey,@tMDivisionid,@tSeq1,@tSeq2;

			END TRY
			BEGIN CATCH
				EXECUTE usp_GetErrorInfo;
			END CATCH;
		END 
		CLOSE MYCURSOR 
		DEALLOCATE MYCURSOR
		FETCH NEXT FROM PO_CURSOR INTO @tPoid;
	END
	CLOSE PO_CURSOR 
	DEALLOCATE PO_CURSOR
	END
END