-- =============================================
-- Description:	<exp_ChangeKPILETARequest>
-- =============================================
CREATE PROCEDURE [dbo].[exp_ChangeKPILETARequest]

AS
BEGIN

IF OBJECT_ID(N'ChangeKPILETARequest') IS NOT NULL
BEGIN
  DROP TABLE ChangeKPILETARequest
END

SELECT *
INTO ChangeKPILETARequest
FROM Production.dbo.ChangeKPILETARequest
WHERE Status = 'Sent' 
and (
	(AddDate >=	 CONVERT(DATE,DATEADD(day,-30,GETDATE())) and
		AddDate <  CONVERT(DATE,DATEADD(day,+1,GETDATE())))
	or
	(EditDate >= CONVERT(DATE,DATEADD(day,-30,GETDATE())) and
		EditDate <  CONVERT(DATE,DATEADD(day,+1,GETDATE()))
	and EditDate is not null
		)
)



END
GO
