
-- =============================================
-- Author:		LEO	
-- Create date:20160903
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[imp_Ietms3]
	
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
a.ID	= b.ID
,a.MainID	= b.MainID
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

from Trade_To_Pms.dbo.SMNotice as b
where not exists(select id from Production.dbo.SMNotice as a where a.id = b.id)

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
a.ID	= b.ID
,a.SEQ	= b.SEQ
,a.MarkerVersion	= b.MarkerVersion
,a.MarkerNo	= b.MarkerNo
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
 
from Trade_To_Pms.dbo.Marker_Send as b
where not exists(select 1 from Production.dbo.Marker_Send as a where a.id = b.id and a.SEQ=b.SEQ and a.MarkerVersion = b.MarkerVersion)

END





