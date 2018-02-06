-- =============================================
-- Description:	<exp_ChangeKPILETARequest>
-- =============================================
CREATE PROCEDURE [dbo].[exp_ChangeKPILETARequest]

AS

IF OBJECT_ID(N'ChangeKPILETARequest') IS NOT NULL
BEGIN
  DROP TABLE ChangeKPILETARequest
END


declare @DateStart date= CONVERT(DATE,DATEADD(day,-30,GETDATE()));
declare @DateEnd date=CONVERT(date, GETDATE());
declare @DateInfoName varchar(30) ='ChangeKPILETARequest';

--�s�W�϶���ƨ�DateInfo name='ChangeKPILETARequest' DateStart=���ɤ���e30�� DateEnd=���ɤ��
If Exists (Select 1 From Pms_To_Trade.dbo.DateInfo Where Name = @DateInfoName )
Begin
	update Pms_To_Trade.dbo.dateInfo
	set DateStart=@DateStart,
	DateEnd=@DateEnd
	Where Name = @DateInfoName 
end;
Else
Begin 
	insert into Pms_To_Trade.dbo.dateInfo(Name,DateStart,DateEnd)
	values (@DateInfoName,@DateStart,@DateEnd);
end;


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