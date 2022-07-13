USE Production
GO

CREATE Function [dbo].GetIsHighRisk
(
	@SuppID varchar(50),
	@Refno varchar(50)
) 

RETURNS bit
AS
BEGIN

	Declare @IsHighRisk as bit = 0
	--Declare @IsRrLr as bit = 0
	--Declare @IsAdiComp as bit = 0
	--Declare @IsQKpi as bit = 0
	Declare @FailCount as int = 0

	IF EXISTS(
		select 1
		from Style_RRLR_Report
		where SuppID = @SuppID AND Refno = @Refno
		AND (RR=1 OR LR=1)	
	)
	BEGIN
		SET @FailCount = @FailCount + 1 
	END

	IF EXISTS(
		select 1
		from ADIDASComplain a
		inner join ADIDASComplain_Detail b on a.ID=b.ID 
		where  a.Junk=0 AND b.Responsibility IN ('T2','T3')
		AND SuppID = @SuppID AND Refno = @Refno
	)
	BEGIN
		SET @FailCount = @FailCount + 1 
	END

	IF EXISTS(
		select 1
		from Quality.dbo.FabricTest
		where CrockingResult='Fail'
		AND SuppID = @SuppID AND Refno = @Refno
	) OR EXISTS(
		select 1
		from Quality.dbo.FabricOven a
		inner join  Quality.dbo.FabricOven_Detail b on a.ReportNo=b.ReportNo
		where a.Status='Confirmed' AND b.Result = 'Fail' AND b.Refno = @Refno
	) 
	BEGIN
		SET @FailCount = @FailCount + 1 
	END

	IF @FailCount >= 2
	BEGIN
		SET @IsHighRisk = 1
	END

	RETURN @IsHighRisk
END


GO