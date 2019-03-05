
--將根據傳入參數，更新 PO.FIRInspPercent 或 PO.AIRInspPercent
CREATE PROCEDURE [dbo].[UpdateInspPercent] 
(
	@Type varchar(10) = '' --FIR , AIR , FIRLab
	,@POID varchar(20) = ''
)
AS
BEGIN

	SET NOCOUNT ON;
	BEGIN TRY
		IF @Type='FIR'
		BEGIN
			UPDATE p
			SET 
			FIRInspPercent= (   select TOP 1
								 cnt= isnull(convert(varchar,round(convert(float,sum(case when Result<>'' or (nonphysical=1 and nonweight=1 and nonshadebond=1 and noncontinuity=1 and nonOdor = 1) then 1 else 0 end))/convert(float,count(*)),4)*100),0)
								 from FIR 
								 where POID=@POID )
			FROM [PO] p
		END

		IF @Type='AIR'
		BEGIN
			UPDATE p
			SET 
			AIRInspPercent=( 
								select 
								cnt= isnull(convert(varchar,round(convert(float,sum(case when Result<>'' AND STATUS='Confirmed' then 1 else 0 end))/convert(float,count(*)),4)*100),0)
								from AIR 
								where POID=@POID) 
			FROM [PO] p
		END

		IF @Type = 'FIRLab'
		BEGIN
			update PO
			set PO.FIRLabInspPercent = 
			(select cnt= isnull(
				convert(varchar,
				round(convert(float,sum(case when b.Result <>'' or (nonCrocking=1 and nonWash=1 and nonHeat=1) then 1 else 0 end))
				/convert(float,count(*)),4)*100),0)
			 from fir a
			 left join FIR_Laboratory b on a.ID=b.ID
			 left join Receiving c on a.ReceivingID=c.Id 
			 where a.POID = po.ID
			 )
			from PO where PO.ID=@POID
		END

	END TRY
	BEGIN CATCH
		EXEC usp_GetErrorInfo;
	END CATCH

END