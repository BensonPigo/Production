

CREATE PROCEDURE [dbo].[usp_FirEncode] 
	-- Add the parameters for the stored procedure here
	@FirID BIGINT,
	@Login VARCHAR(10)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @Result varchar(15);
	DECLARE @dyelot varchar(8);
	DECLARE @err_msg nvarchar(2000);

	BEGIN TRY
	IF EXISTS(SELECT * FROM DBO.FIR WITH (NOLOCK) WHERE ID = @FirID AND PhysicalEncode = 0) -- Not Encoded
	BEGIN
		IF EXISTS(SELECT * FROM DBO.FIR WITH (NOLOCK) WHERE ID = @FirID AND nonPhysical = 0) -- 需檢驗的要作缸號檢查 (每批來料要每缸都有檢查資料)
		BEGIN
			DECLARE dyelot_cursor CURSOR FOR 
			select r.Dyelot from (
									Select distinct a.Dyelot from dbo.View_AllReceivingDetail a WITH (NOLOCK)
									inner join dbo.FIR b WITH (NOLOCK)
									on b.POID = a.PoId and b.SEQ1 =a.Seq1 and b.SEQ2 = a.Seq2 and b.ReceivingID = a.Id
									where b.Id=@FirID
								 ) r
					left join (select distinct dyelot from dbo.FIR_Physical WITH (NOLOCK) where id = @FirID ) i 
					on i.Dyelot = r.Dyelot
					where i.Dyelot is null

			OPEN dyelot_cursor;
			FETCH NEXT FROM dyelot_cursor INTO @dyelot;
			IF @dyelot is not null
			BEGIN
				WHILE @@FETCH_STATUS = 0
				BEGIN
					SET @err_msg = isnull(@err_msg,'')+'Dyelot: ' +@dyelot + ' is not inspected.'+char(13)+char(10);
					FETCH NEXT FROM dyelot_cursor INTO @dyelot;
				END

				IF @err_msg is not null
				BEGIN
					RAISERROR (@err_msg, -- Message text.
				   16, -- Severity.
				   1 -- State.
				   );
				END
			END
			CLOSE dyelot_cursor;

		END
		BEGIN TRANSACTION

		Update Fir set PhysicalDate = GetDate()
						,PhysicalEncode=1
						,EditName=@Login
						,EditDate = GetDate()
						,Physical = IIF(EXISTS(SELECT * FROM DBO.FIR_Physical WITH (NOLOCK) WHERE ID = @FirID and result = 'Fail'),'Fail','Pass')
						,TotalDefectPoint = (select sum(t.TotalPoint) from dbo.FIR_Physical t WITH (NOLOCK) where t.ID =@FirID)
						,TotalInspYds = (select sum(t.ActualYds) from dbo.FIR_Physical t WITH (NOLOCK) where t.ID =@FirID)
						,Status='Confirmed' 
				where id =@FirID;

		SELECT @Result = DBO.GetFirResult(@FirID);

		IF @Result = ''
			Update dbo.FIR set Result = @Result , status = 'New' where id = @FirID;
		ELSE
			Update dbo.FIR set Result = @Result , status = 'Confirmed' where id = @FirID;

			COMMIT TRANSACTION;
		
	END
	
	ELSE
	BEGIN
		Update Fir set PhysicalDate = NULL
						,PhysicalEncode=0
						,EditName=@Login
						,EditDate = GetDate()
						,Physical = ''
						,TotalDefectPoint = 0
						,TotalInspYds = 0
						,Status='New' 
						,Result = ''
				where id =@FirID;
	END

	END TRY
	BEGIN CATCH
		IF XACT_STATE() <> 0 -- 非0表示有交易
			ROLLBACK TRANSACTION;
		EXECUTE usp_GetErrorInfo;
	END CATCH
    
END