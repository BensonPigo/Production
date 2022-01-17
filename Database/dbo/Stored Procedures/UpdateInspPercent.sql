

--�N�ھڶǤJ�ѼơA��s PO.FIRInspPercent �� PO.AIRInspPercent
CREATE PROCEDURE [dbo].[UpdateInspPercent] 
(
	@Type varchar(20) = '' --FIR , AIR , FIRLab, LabColorFastness
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
			where P.ID=@POID
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
			where P.ID=@POID
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
		
		IF @Type='LabColorFastness'
		BEGIN
			UPDATE p
			SET 
			LabColorFastnessPercent= (   select 
				cnt= isnull(convert(varchar,round(convert(float,sum(case when status='Confirmed' then 1 else 0 end))/convert(float,count(*)),4)*100),0)
				from ColorFastness 
				where POID=@POID )
			FROM [PO] p
			where P.ID=@POID
		END

		IF @Type='LabWaterFastness'
		BEGIN
			UPDATE p
			SET 
			LabWaterFastnessPercent= (   select 
				cnt= isnull(convert(varchar,round(convert(float,sum(case when status='Confirmed' then 1 else 0 end))/convert(float,count(*)),4)*100),0)
				from WaterFastness 
				where POID=@POID )
			FROM [PO] p
			where P.ID=@POID
		END

		IF @Type='LabOven'
		BEGIN
			UPDATE p
			SET 
			LabOvenPercent= (   select 
				cnt= isnull(convert(varchar,round(convert(float,sum(case when status='Confirmed' then 1 else 0 end))
				/convert(float,count(*)),4)*100),0)
			from oven  
			where POID=@POID )
			FROM [PO] p
			where P.ID=@POID
		END

			IF @Type='AIRLab'
		BEGIN
			UPDATE p
			SET 
			AIRLabInspPercent= (   select 
				cnt= isnull(convert(varchar,round(convert(float,sum(case when Result<>'' OR( NonOven='True' and NonWash='True') then 1					else 0 end))/convert(float,count(*)),4)*100),0)
			from AIR_Laboratory 
			where POID=@POID )
			FROM [PO] p
			where P.ID=@POID
		END


	END TRY
	BEGIN CATCH
		EXEC usp_GetErrorInfo;
	END CATCH

END