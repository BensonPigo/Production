-- =============================================
-- Author:		LEO	
-- Create date:20160903
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[imp_Ietms]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

  -- ietms
--ietms

----------------------�R���DTABLE�h�����
Delete Production.dbo.IETMS
from Production.dbo.IETMS as a left join Trade_To_Pms.dbo.IETMS as b
on a.id = b.id AND a.Version = b.Version
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
a.ID	= b.ID
,a.Version	= b.Version
,a.Ukey	= b.Ukey
,a.IEName	= b.IEName
,a.ActFinDate	= b.ActFinDate
,a.GSDStyleCode	= b.GSDStyleCode
,a.GSDStyleTitle	= b.GSDStyleTitle
,a.AddName	= b.AddName
,a.AddDate	= b.AddDate
,a.EditName	= b.EditName
,a.EditDate	= b.EditDate

from Production.dbo.IETMS as a inner join Trade_To_Pms.dbo.IETMS as b ON a.id=b.id AND a.Version = b.Version
-------------------------- INSERT INTO ��
INSERT INTO Production.dbo.IETMS(
ID
,Version
,Ukey
,IEName
,ActFinDate
,GSDStyleCode
,GSDStyleTitle
,AddName
,AddDate
,EditName
,EditDate

)
select 
ID
,Version
,Ukey
,IEName
,ActFinDate
,GSDStyleCode
,GSDStyleTitle
,AddName
,AddDate
,EditName
,EditDate

from Trade_To_Pms.dbo.IETMS as b
where not exists(select id from Production.dbo.IETMS as a where a.id = b.id AND a.Version = b.Version)
--IETMS1
--IETMS_Detail

----------------------�R���DTABLE�h�����
Delete Production.dbo.IETMS_Detail
from Production.dbo.IETMS_Detail as a 
INNER JOIN Trade_To_Pms.dbo.IETMS as t on a.IETMSUkey=t.Ukey
left join Trade_To_Pms.dbo.IETMS_Detail as b
on a.Ukey = b.Ukey
where b.Ukey is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
a.IETMSUkey	= b.IETMSUkey
,a.SEQ	= b.SEQ
,a.Location	= b.Location
,a.OperationID	= b.OperationID
,a.Mold	= b.Mold
,a.Annotation	= b.Annotation
,a.Frequency	= b.Frequency
,a.SMV	= b.SMV
,a.SeamLength	= b.SeamLength
,a.UKey	= b.UKey

from Production.dbo.IETMS_Detail as a 
inner join Trade_To_Pms.dbo.IETMS_Detail as b ON a.Ukey = b.Ukey
-------------------------- INSERT INTO ��
INSERT INTO Production.dbo.IETMS_Detail(
IETMSUkey
,SEQ
,Location
,OperationID
,Mold
,Annotation
,Frequency
,SMV
,SeamLength
,UKey

)
select 
IETMSUkey
,SEQ
,Location
,OperationID
,Mold
,Annotation
,Frequency
,SMV
,SeamLength
,UKey

from Trade_To_Pms.dbo.IETMS_Detail as b
where not exists(select 1 from Production.dbo.IETMS_Detail as a where a.UKey = b.UKey)


--Operation
--Operation
----------------------�R���DTABLE�h�����
Delete Production.dbo.Operation
from Production.dbo.Operation as a left join Trade_To_Pms.dbo.Operation as b
on a.id = b.id
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
a.ID	= b.ID
,a.FromGSD	= b.FromGSD
,a.CalibratedCode	= b.CalibratedCode
,a.DescEN	= b.DescEN
,a.DescCH	= b.DescCH
,a.MachineTypeID	= b.MachineTypeID
,a.MtlFactorID	= b.MtlFactorID
,a.ISO	= b.ISO
,a.RPM	= b.RPM
,a.MoldID	= b.MoldID
,a.OperationType	= b.OperationType
,a.CostCenter	= b.CostCenter
,a.Section	= b.Section
,a.SMV	= b.SMV
,a.MachineTMU	= b.MachineTMU
,a.ManualTMU	= b.ManualTMU
,a.TotalTMU	= b.TotalTMU
,a.MachineSMVWithAllowance	= b.MachineSMVWithAllowance
,a.TotalSMVWithAllowance	= b.TotalSMVWithAllowance
,a.StitchCM	= b.StitchCM
,a.SeamLength	= b.SeamLength
--,a.Picture1	= b.Picture1
--,a.Picture2	= b.Picture2
,a.NeedleThread	= b.NeedleThread
,a.BottomThread	= b.BottomThread
,a.CoverThread	= b.CoverThread
,a.NeedleLength	= b.NeedleLength
,a.BottomLength	= b.BottomLength
,a.CoverLength	= b.CoverLength
,a.Junk	= b.Junk
,a.AddName	= b.AddName
,a.AddDate	= b.AddDate
,a.EditName	= b.EditName
,a.EditDate	= b.EditDate

from Production.dbo.Operation as a inner join Trade_To_Pms.dbo.Operation as b ON a.id=b.id
-------------------------- INSERT INTO ��
INSERT INTO Production.dbo.Operation(
ID
,FromGSD
,CalibratedCode
,DescEN
,DescCH
,MachineTypeID
,MtlFactorID
,ISO
,RPM
,MoldID
,OperationType
,CostCenter
,Section
,SMV
,MachineTMU
,ManualTMU
,TotalTMU
,MachineSMVWithAllowance
,TotalSMVWithAllowance
,StitchCM
,SeamLength
--,Picture1
--,Picture2
,NeedleThread
,BottomThread
,CoverThread
,NeedleLength
,BottomLength
,CoverLength
,Junk
,AddName
,AddDate
,EditName
,EditDate

)
select 
ID
,FromGSD
,CalibratedCode
,DescEN
,DescCH
,MachineTypeID
,MtlFactorID
,ISO
,RPM
,MoldID
,OperationType
,CostCenter
,Section
,SMV
,MachineTMU
,ManualTMU
,TotalTMU
,MachineSMVWithAllowance
,TotalSMVWithAllowance
,StitchCM
,SeamLength
--,Picture1
--,Picture2
,NeedleThread
,BottomThread
,CoverThread
,NeedleLength
,BottomLength
,CoverLength
,Junk
,AddName
,AddDate
,EditName
,EditDate

from Trade_To_Pms.dbo.Operation as b
where not exists(select id from Production.dbo.Operation as a where a.id = b.id)

--MACHTYPE
--MachineType
----------------------�R���DTABLE�h�����
Delete Production.dbo.MachineType
from Production.dbo.MachineType as a left join Trade_To_Pms.dbo.MachineType as b
on a.id = b.id
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
a.ID	= b.ID
,a.Description	= b.Description
,a.DescCH	= b.DescCH
,a.ISO	= b.ISO
,a.ArtworkTypeID	= b.ArtworkTypeID
,a.ArtworkTypeDetail	= b.ArtworkTypeDetail
,a.Mold	= b.Mold
,a.RPM	= b.RPM
,a.Stitches	= b.Stitches
,a.Picture1	= b.Picture1
,a.Picture2	= b.Picture2
,a.MachineAllow	= b.MachineAllow
,a.ManAllow	= b.ManAllow
,a.MachineGroupID	= b.MachineGroupID
,a.Junk	= b.Junk
,a.AddName	= b.AddName
,a.AddDate	= b.AddDate
,a.EditName	= b.EditName
,a.EditDate	= b.EditDate

from Production.dbo.MachineType as a inner join Trade_To_Pms.dbo.MachineType as b ON a.id=b.id
-------------------------- INSERT INTO ��
INSERT INTO Production.dbo.MachineType(
ID
,Description
,DescCH
,ISO
,ArtworkTypeID
,ArtworkTypeDetail
,Mold
,RPM
,Stitches
,Picture1
,Picture2
,MachineAllow
,ManAllow
,MachineGroupID
,Junk
,AddName
,AddDate
,EditName
,EditDate

)
select 
ID
,Description
,DescCH
,ISO
,ArtworkTypeID
,ArtworkTypeDetail
,Mold
,RPM
,Stitches
,Picture1
,Picture2
,MachineAllow
,ManAllow
,MachineGroupID
,Junk
,AddName
,AddDate
,EditName
,EditDate

from Trade_To_Pms.dbo.MachineType as b
where not exists(select id from Production.dbo.MachineType as a where a.id = b.id)

--ATTACH
--MOLD
----------------------�R���DTABLE�h�����
Delete Production.dbo.Mold
from Production.dbo.Mold as a left join Trade_To_Pms.dbo.Mold as b
on a.id = b.id
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
a.ID	= b.ID
,a.Type	= b.Type
,a.DescCH	= b.DescCH
,a.DescEN	= b.DescEN
,a.Junk	= b.Junk
,a.AddName	= b.AddName
,a.AddDate	= b.AddDate
,a.EditName	= b.EditName
,a.EditDate	= b.EditDate

from Production.dbo.Mold as a inner join Trade_To_Pms.dbo.Mold as b ON a.id=b.id
-------------------------- INSERT INTO ��
INSERT INTO Production.dbo.Mold(
ID
,Type
,DescCH
,DescEN
,Junk
,AddName
,AddDate
,EditName
,EditDate

)
select 
ID
,Type
,DescCH
,DescEN
,Junk
,AddName
,AddDate
,EditName
,EditDate

from Trade_To_Pms.dbo.Mold as b
where not exists(select id from Production.dbo.Mold as a where a.id = b.id)
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




