

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
	DECLARE @err_msg nvarchar(max);
	
	BEGIN TRY
		IF EXISTS(SELECT * FROM DBO.FIR WITH (NOLOCK) WHERE ID = @FirID AND (PhysicalEncode = 0 or OdorEncode = 0))
		Begin
			-- Do Physical Encode Check Dyelot
			IF EXISTS(SELECT * FROM DBO.FIR WITH (NOLOCK) WHERE ID = @FirID AND PhysicalEncode = 0 and nonPhysical = 0)
			BEGIN
				-- FIR_Physical 未檢驗的 Dyelot
				Set @err_msg = 'Physical Dyelot is not inspected:'+ char(10) +
				Stuff((
					select CONCAT(',',Dyelot)
					from(
						Select distinct a.Dyelot
						from dbo.View_AllReceivingDetail a WITH (NOLOCK)
						inner join dbo.FIR b WITH (NOLOCK) on b.POID = a.PoId and b.SEQ1 =a.Seq1 and b.SEQ2 = a.Seq2 and b.ReceivingID = a.Id
						where b.Id=@FirID
						and not exists(select 1 from dbo.FIR_Physical WITH (NOLOCK) where id = @FirID and Dyelot = a.Dyelot)
					)x
					for xml path('')
				),1,1,'')
				 + char(10) + char(10)
			END

			 -- Do Odor Encode Check Dyelot
			IF EXISTS(SELECT * FROM DBO.FIR WITH (NOLOCK) WHERE ID = @FirID AND OdorEncode = 0 and nonOdor = 0)
			BEGIN
				-- FIR_Odor 未檢驗的 Dyelot
				Set @err_msg = isnull(@err_msg, '') + 'Odor Dyelot is not inspected: '+ char(10) +
				Stuff((
					select CONCAT(',',Dyelot)
					from(
						Select distinct a.Dyelot
						from dbo.View_AllReceivingDetail a WITH (NOLOCK)
						inner join dbo.FIR b WITH (NOLOCK) on b.POID = a.PoId and b.SEQ1 =a.Seq1 and b.SEQ2 = a.Seq2 and b.ReceivingID = a.Id
						where b.Id=@FirID
						and not exists(select 1 from dbo.FIR_Odor WITH (NOLOCK) where id = @FirID and Dyelot = a.Dyelot)
					)x
					for xml path('')
				),1,1,'')
			END
			
			IF @err_msg is not null
			BEGIN
				RAISERROR (@err_msg, -- Message text.
				16, -- Severity.
				1 -- State.
				);
			END

			-- 上方 Physical and Odor check Dyelot end
			IF @err_msg is null
			Begin
				BEGIN TRANSACTION
					IF EXISTS(SELECT 1 FROM DBO.FIR WITH (NOLOCK) WHERE ID = @FirID AND PhysicalEncode = 0)
					BEGIN
						Update Fir
						set
							PhysicalDate = GetDate()
							,PhysicalEncode=1
							,Physical = IIF(EXISTS(SELECT 1 FROM DBO.FIR_Physical WITH (NOLOCK) WHERE ID = @FirID and result = 'Fail'),'Fail','Pass')
							,PhysicalInspector=@Login

							,EditName=@Login
							,EditDate = GetDate()
							,TotalDefectPoint = (select sum(t.TotalPoint) from dbo.FIR_Physical t WITH (NOLOCK) where t.ID =@FirID)
							,TotalInspYds = (select sum(t.ActualYds) from dbo.FIR_Physical t WITH (NOLOCK) where t.ID =@FirID)
						where id =@FirID;
					END

					IF EXISTS(SELECT 1 FROM DBO.FIR WITH (NOLOCK) WHERE ID = @FirID AND OdorEncode = 0)
					BEGIN
						Update Fir
						set
							OdorDate = GetDate()
							,OdorEncode=1
							,Odor = IIF(EXISTS(SELECT 1 FROM DBO.FIR_Odor WITH (NOLOCK) WHERE ID = @FirID and result = 'Fail'),'Fail','Pass')
							,OdorInspector = @Login

							,EditName=@Login
							,EditDate = GetDate()
						where id =@FirID;
					END

				SELECT @Result = DBO.GetFirResult(@FirID);
				IF @Result = ''
					Update dbo.FIR set Result = @Result , status = 'New' where id = @FirID;
				ELSE
					Update dbo.FIR set Result = @Result , status = 'Confirmed' where id = @FirID;

				COMMIT TRANSACTION;
			End
		End
		-- Do Amend
		Else
		BEGIN
			Update Fir
			set
				PhysicalDate = NULL
				,PhysicalEncode=0
				,Physical = ''

				,OdorDate = NULL
				,OdorEncode = 0
				,Odor = ''
			
				,Status='New' 
				,Result = ''
				,TotalDefectPoint = 0
				,TotalInspYds = 0

				,EditName=@Login
				,EditDate = GetDate()
			where id =@FirID;
		END

	END TRY
	BEGIN CATCH
		IF XACT_STATE() <> 0 -- 非0表示有交易
			ROLLBACK TRANSACTION;
		EXECUTE usp_GetErrorInfo;
	END CATCH
    
END
