
-- =============================================
-- Author:		LEO	
-- Create date:20160903
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[imp_Ietms3]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;



--SMNotice
--SMNotice
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
--a.ID	= b.ID
a.MainID	= b.MainID
,a.Mr	= b.Mr
,a.SMR	= b.SMR
,a.BrandID	= b.BrandID
,a.StyleID	= b.StyleID
,a.SeasonID	= b.SeasonID
,a.StyleUkey	= b.StyleUkey
,a.CountryID	= b.CountryID
,a.PatternNo	= b.PatternNo
,a.OldStyleID	= b.OldStyleID
,a.OldSeasonID	= b.OldSeasonID
,a.SizeGroup	= b.SizeGroup
,a.SizeCode	= b.SizeCode
,a.BuyReady	= b.BuyReady
,a.Status	= b.Status
,a.StatusPattern	= b.StatusPattern
,a.StatusIE	= b.StatusIE
,a.AddName	= b.AddName
,a.AddDate	= b.AddDate
,a.EditName	= b.EditName
,a.EditDate	= b.EditDate

from Production.dbo.SMNotice as a inner join Trade_To_Pms.dbo.SMNotice as b ON a.id=b.id
-------------------------- INSERT INTO ��
INSERT INTO Production.dbo.SMNotice(
ID
,MainID
,Mr
,SMR
,BrandID
,StyleID
,SeasonID
,StyleUkey
,CountryID
,PatternNo
,OldStyleID
,OldSeasonID
,SizeGroup
,SizeCode
,BuyReady
,Status
,StatusPattern
,StatusIE
,AddName
,AddDate
,EditName
,EditDate

)
select 
ID
,MainID
,Mr
,SMR
,BrandID
,StyleID
,SeasonID
,StyleUkey
,CountryID
,PatternNo
,OldStyleID
,OldSeasonID
,SizeGroup
,SizeCode
,BuyReady
,Status
,StatusPattern
,StatusIE
,AddName
,AddDate
,EditName
,EditDate

from Trade_To_Pms.dbo.SMNotice as b WITH (NOLOCK)
where not exists(select id from Production.dbo.SMNotice as a WITH (NOLOCK) where a.id = b.id)

--SMNotice_Detail
Merge Production.dbo.smnotice_detail as t
Using (select * from Trade_To_Pms.dbo.smnotice_detail a WITH (NOLOCK) where id in (select id from smnotice WITH (NOLOCK)))as s
on t.id = s.id and t.type = s.type
when matched then
	update set 
		t.[UseFor]			=s.[UseFor]
		,t.[PhaseID]			=s.[PhaseID]
		,t.[RequireDate]		=s.[RequireDate]
		,t.[Apv2SampleTime]	=s.[Apv2SampleTime]
		,t.[Apv2SampleHandle]	=s.[Apv2SampleHandle]
		,t.[ApvName]			=s.[ApvName]
		,t.[ApvDate]			=s.[ApvDate]
		,t.[Factory]			=s.[Factory]
		,t.[IEConfirmMR]		=s.[IEConfirmMR]
		,t.[PendingStatus]	=s.[PendingStatus]
		,t.[BasicPattern]		=s.[BasicPattern]
		,t.[Remark1]			=s.[Remark1]
		,t.[Remark2]			=s.[Remark2]
		,t.[AddName]			=s.[AddName]
		,t.[AddDate]			=s.[AddDate]
		,t.[EditName]			=s.[EditName]
		,t.[EditDate]			=s.[EditDate]
when not matched by target then 	
insert([ID],[Type],[UseFor],[PhaseID],[RequireDate],[Apv2SampleTime],[Apv2SampleHandle],[ApvName],[ApvDate],[Factory]
,[IEConfirmMR],[PendingStatus],[BasicPattern],[Remark1],[Remark2],[AddName],[AddDate],[EditName],[EditDate])
values(s.[ID],s.[Type],s.[UseFor],s.[PhaseID],s.[RequireDate],s.[Apv2SampleTime],s.[Apv2SampleHandle],s.[ApvName],s.[ApvDate]
,s.[Factory],s.[IEConfirmMR],s.[PendingStatus],s.[BasicPattern],s.[Remark1],s.[Remark2],s.[AddName],s.[AddDate],s.[EditName],s.[EditDate])	
when not matched by source then delete;

--MarkerSend
--Marker_Send
----------------------�R���DTABLE�h�����
Delete Production.dbo.Marker_Send
from Production.dbo.Marker_Send as a 
INNER JOIN Trade_To_Pms.dbo.SMNotice as t on a.id=t.id
left join Trade_To_Pms.dbo.Marker_Send as b
on a.id = b.id and a.SEQ=b.SEQ and a.MarkerVersion = b.MarkerVersion
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
--a.ID	= b.ID
--,a.SEQ	= b.SEQ
--,a.MarkerVersion	= b.MarkerVersion
a.MarkerNo	= b.MarkerNo
,a.PatternSMID	= b.PatternSMID
,a.PatternVersion	= b.PatternVersion
,a.ToFactory	= b.ToFactory
,a.TransLate	= b.TransLate
,a.AddName	= b.AddName
,a.AddDate	= b.AddDate

from Production.dbo.Marker_Send as a 
inner join Trade_To_Pms.dbo.Marker_Send as b ON a.id=b.id and a.SEQ=b.SEQ and a.MarkerVersion = b.MarkerVersion
-------------------------- INSERT INTO ��
INSERT INTO Production.dbo.Marker_Send(
ID
,SEQ
,MarkerVersion
,MarkerNo
,PatternSMID
,PatternVersion
,ToFactory
,TransLate
,AddName
,AddDate

)
select 
ID
,SEQ
,MarkerVersion
,MarkerNo
,PatternSMID
,PatternVersion
,ToFactory
,TransLate
,AddName
,AddDate
 
from Trade_To_Pms.dbo.Marker_Send as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Marker_Send as a WITH (NOLOCK) where a.id = b.id and a.SEQ=b.SEQ and a.MarkerVersion = b.MarkerVersion)

END





