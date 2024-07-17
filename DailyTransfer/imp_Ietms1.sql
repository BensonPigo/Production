-- =============================================
-- Author:		LEO	
-- Create date:20160903
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[imp_Ietms1]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

  -- ietms
--ietms

---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
a.Ukey	= isnull( b.Ukey                     , 0)
,a.IEName	= isnull( b.IEName               , '')
,a.ActFinDate	=  b.ActFinDate
,a.GSDStyleCode	= isnull( b.GSDStyleCode     , '')
,a.GSDStyleTitle	= isnull( b.GSDStyleTitle, '')
,a.AddName	= isnull( b.AddName              , '')
,a.AddDate	=  b.AddDate
,a.EditName	= isnull( b.EditName             , '')
,a.EditDate	= b.EditDate
,a.GSDType	= isnull(b.GSDType, '')
,a.ActFtyIE = ISNULL(b.ActFtyIE,'')
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
,GSDType
,ActFtyIE
)
select
 isnull(ID           , '')
,isnull(Version      , '')
,isnull(Ukey         , 0)
,isnull(IEName       , '')
,ActFinDate
,isnull(GSDStyleCode , '')
,isnull(GSDStyleTitle, '')
,isnull(AddName      , '')
,AddDate
,isnull(EditName     , '')
,EditDate
,isnull(b.GSDType, '')
,ISNULL(b.ActFtyIE,'')
from Trade_To_Pms.dbo.IETMS as b WITH (NOLOCK)
where not exists(select id from Production.dbo.IETMS as a WITH (NOLOCK) where a.id = b.id AND a.Version = b.Version)
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
a.IETMSUkey	= isnull( b.IETMSUkey         , 0)
,a.SEQ	= isnull( b.SEQ                   , '')
,a.Location	= isnull( b.Location          , '')
,a.OperationID	= isnull( b.OperationID   , '')
,a.Mold	= isnull( b.Mold                  , '')
,a.Annotation	= isnull( b.Annotation    , '')
,a.Frequency	= isnull( b.Frequency     , 0)
,a.SMV	= isnull( b.SMV                   , 0)
,a.SeamLength	= isnull( b.SeamLength    , 0)
,a.MtlFactorID   = isnull(b.MtlFactorID   , '')
,a.MtlFactorRate  = isnull(b.MtlFactorRate, 0)
,A.Draft = ISNULL (B.Draft,'')
,A.CodeFrom = ISNULL(B.CodeFrom,'')
,A.Pattern_GL_ArtworkUkey = ISNULL(B.Pattern_GL_ArtworkUkey,'')
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
,MtlFactorID
,MtlFactorRate
,[Draft]
,[CodeFrom]
,[Pattern_GL_ArtworkUkey]
)
select 
 isnull(IETMSUkey    , 0)
,isnull(SEQ          , '')
,isnull(Location     , '')
,isnull(OperationID  , '')
,isnull(Mold         , '')
,isnull(Annotation   , '')
,isnull(Frequency    , 0)
,isnull(SMV          , 0)
,isnull(SeamLength   , 0)
,isnull(UKey         , 0)
,isnull(MtlFactorID  , '')
,isnull(MtlFactorRate, 0)
,[Draft] = ISNULL (B.Draft,'')
,[CodeFrom] = ISNULL(B.CodeFrom,'')
,[Pattern_GL_ArtworkUkey] = ISNULL(B.Pattern_GL_ArtworkUkey,'')
from Trade_To_Pms.dbo.IETMS_Detail as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.IETMS_Detail as a WITH (NOLOCK) where a.UKey = b.UKey)

-------------------------------------------------------aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
--Operation
--Operation
merge IETMS_Summary t
using (select * from trade_to_pms.dbo.IETMS_Summary) s
on t.[IETMSUkey] = s.[IETMSUkey] and t.[MachineTypeID]=s.[MachineTypeID] 
and t.[ArtworkTypeID]=s.[ArtworkTypeID]  and t.[Location]=s.[Location]
and exists (select 1 from dbo.IETMS where Ukey = s.IETMSUkey)
when matched then update set 
	t.[StyleUkey]	= isnull(s.[StyleUkey],0),
	t.[ProSMV]		= isnull(s.[ProSMV]   ,0),
	t.[ProTMS]		= isnull(s.[ProTMS]	  ,0),
	t.[ProPrice]	= isnull(s.[ProPrice] ,0)
when not matched by target then
insert ([IETMSUkey],[StyleUkey],[MachineTypeID],[ArtworkTypeID],[Location],[ProSMV],[ProTMS],[ProPrice])
       VALUES
       (
              isnull(s.[IETMSUkey],     0),
              isnull(s.[StyleUkey],     0),
              isnull(s.[MachineTypeID], ''),
              isnull(s.[ArtworkTypeID], ''),
              isnull(s.[Location],      ''),
              isnull(s.[ProSMV],        0),
              isnull(s.[ProTMS],        0),
              isnull(s.[ProPrice],      0)
       );

merge IETMS_Summary_Detail t
using (select * from trade_to_pms.dbo.IETMS_Summary_Detail) s
on t.[IETMSUkey] = s.[IETMSUkey] and t.[ArtworkTypeID]=s.[ArtworkTypeID] and t.[CIPF]=s.[CIPF]
and exists (select 1 from dbo.IETMS where Ukey = s.IETMSUkey)
when matched then update set 
 t.[StyleUkey]		= isnull(s.[StyleUkey],0)
,t.[ProSMV]			= isnull(s.[ProSMV]	  ,0)
,t.[ProTMS]			= isnull(s.[ProTMS]	  ,0)
,t.[ProPrice]		= isnull(s.[ProPrice] ,0)
when not matched by target then
insert ([IETMSUkey],[StyleUkey],[ArtworkTypeID],[ProSMV],[ProTMS],[ProPrice],[CIPF])
       VALUES
       (
              isnull(s.[IETMSUkey],     0),
              isnull(s.[StyleUkey],     0),
              isnull(s.[ArtworkTypeID], ''),
              isnull(s.[ProSMV],        0),
              isnull(s.[ProTMS],        0),
              isnull(s.[ProPrice],      0),
              isnull(s.[CIPF],          '')
       );

----------------------------------IETMS_AT----------------------------------
/*DELETE*/
Delete Production.dbo.IETMS_AT
FROM Production.dbo.IETMS_AT IA
LEFT JOIN Trade_To_Pms.dbo. IETMS_AT TIA WITH(NOLOCK) ON IA.IETMSUkey = TIA.IETMSUkey AND IA.CodeFrom = TIA.CodeFrom
WHERE TIA.IETMSUkey IS NULL AND TIA.CodeFrom IS NULL
/*UPDATE*/
UPDATE IA
SET  
 IA.IETMSUkey = ISNULL(TIA.IETMSUkey,0)
,IA.CodeFrom = ISNULL(TIA.CodeFrom,'')
,IA.Pattern_GL_ArtworkUkey = ISNULL(TIA.Pattern_GL_ArtworkUkey,'')
,IA.Component = ISNULL (TIA.Component,'')
,IA.PieceOfSeamer = ISNULL(TIA.PieceOfSeamer,0)
,IA.PieceOfGarment = ISNULL(TIA.PieceOfGarment,0)
,IA.IsQuilting = ISNULL(TIA.IsQuilting,0)
,IA.RPM = ISNULL(TIA.RPM,'')
,IA.RPMValue = ISNULL(TIA.RPMValue,0)
,IA.SewingLength = ISNULL(TIA.SewingLength,0)
,IA.SewingLine = ISNULL(TIA.SewingLine,0)
,IA.LaserSpeed = ISNULL(TIA.LaserSpeed,'')
,IA.LaserSpeedValue = ISNULL(TIA.LaserSpeedValue,0)
,IA.LaserLength = ISNULL(TIA.LaserLength,0)
,IA.LaserLine = ISNULL(TIA.LaserLine,0)
,IA.MM2AT = ISNULL(TIA.MM2AT,0)
,IA.[AT] = ISNULL(TIA.[AT],0)
,IA.OperationID = ISNULL(TIA.OperationID,'')
,IA.AddName = ISNULL(TIA.AddName ,'')
,IA.AddDate = TIA.AddDate
,IA.EditName = ISNULL(TIA.EditName,'')
,IA.EditDate = TIA.EditDate
FROM Production.dbo.IETMS_AT IA
INNER JOIN Trade_To_Pms.dbo. IETMS_AT TIA WITH(NOLOCK) ON IA.IETMSUkey = TIA.IETMSUkey AND IA.CodeFrom = TIA.CodeFrom
/*INSERT*/
INSERT INTO Production.dbo.IETMS_AT(
[IETMSUkey]
,[CodeFrom]
,[Pattern_GL_ArtworkUkey]
,[Component]
,[PieceOfSeamer]
,[PieceOfGarment]
,[IsQuilting]
,[RPM]
,[RPMValue]
,[SewingLength]
,[SewingLine]
,[LaserSpeed]
,[LaserSpeedValue]
,[LaserLength]
,[LaserLine]
,[MM2AT]
,[AT]
,[OperationID]
,[AddName] 
,[AddDate] 
,[EditName]
,[EditDate]
)
SELECT 
 [IETMSUkey] = ISNULL(TIA.IETMSUkey,0)
,[CodeFrom] = ISNULL(TIA.CodeFrom,'')
,[Pattern_GL_ArtworkUkey] = ISNULL(TIA.Pattern_GL_ArtworkUkey,'')
,[Component] = ISNULL (TIA.Component,'')
,[PieceOfSeamer] = ISNULL(TIA.PieceOfSeamer,0)
,[PieceOfGarment] = ISNULL(TIA.PieceOfGarment,0)
,[IsQuilting] = ISNULL(TIA.IsQuilting,0)
,[RPM] = ISNULL(TIA.RPM,'')
,[RPMValue] = ISNULL(TIA.RPMValue,0)
,[SewingLength] = ISNULL(TIA.SewingLength,0)
,[SewingLine] = ISNULL(TIA.SewingLine,0)
,[LaserSpeed] = ISNULL(TIA.LaserSpeed,'')
,[LaserSpeedValue] = ISNULL(TIA.LaserSpeedValue,0)
,[LaserLength] = ISNULL(TIA.LaserLength,0)
,[LaserLine] = ISNULL(TIA.LaserLine,0)
,[MM2AT] = ISNULL(TIA.MM2AT,0)
,[AT] = ISNULL(TIA.[AT],0)
,[OperationID] = ISNULL(TIA.OperationID,'')
,[AddName] = ISNULL(TIA.AddName ,'')
,[AddDate] = TIA.AddDate
,[EditName] = ISNULL(TIA.EditName,'')
,[EditDate] = TIA.EditDate
FROM Trade_To_Pms.dbo.IETMS_AT TIA WITH (NOLOCK)
WHERE NOT EXISTS(SELECT 1 FROM Production.dbo.IETMS_AT IA WITH (NOLOCK) WHERE IA.IETMSUkey = TIA.IETMSUkey AND IA.CodeFrom = TIA.CodeFrom)
----------------------------------IETMS_AT_Detail----------------------------------
/*DELETE*/
DELETE Production.dbo.IETMS_AT_Detail 
FROM Production.dbo.IETMS_AT_Detail IAD
LEFT JOIN Trade_To_Pms.dbo. IETMS_AT_Detail TIAD ON IAD.IETMSUkey = TIAD.IETMSUkey AND IAD.CodeFrom = TIAD.CodeFrom AND IAD.IESelectCodeType = TIAD.IESelectCodeType and IAD.IESelectCodeID = TIAD.IESelectCodeID
WHERE TIAD.IETMSUkey IS NULL AND TIAD.CodeFrom IS NULL AND TIAD.IESelectCodeType IS NULL AND TIAD.IESelectCodeID IS NULL
/*UPDATE*/
UPDATE IAD
SET  
 IAD.IETMSUkey = ISNULL(TIAD.IETMSUkey,0)
,IAD.CodeFrom = ISNULL(TIAD.CodeFrom,'')
,IAD.IESelectCodeType = ISNULL(TIAD.IESelectCodeType,'')
,IAD.IESelectCodeID = ISNULL(TIAD.IESelectCodeID,'')
,IAD.Number = ISNULL(TIAD.Number,0)
,IAD.[Value] = ISNULL(TIAD.[Value],0)
,IAD.Remark = ISNULL(TIAD.Remark,'')
,IAD.AddName = ISNULL(TIAD.AddName,'')
,IAD.AddDate = TIAD.AddDate
,IAD.EditName = ISNULL(TIAD.EditName,'')
,IAD.EditDate = TIAD.EditDate
FROM Production.dbo.IETMS_AT_Detail IAD
INNER JOIN Trade_To_Pms.dbo. IETMS_AT_Detail TIAD WITH(NOLOCK) ON IAD.IETMSUkey = TIAD.IETMSUkey AND IAD.CodeFrom = TIAD.CodeFrom AND IAD.IESelectCodeType = TIAD.IESelectCodeType and IAD.IESelectCodeID = TIAD.IESelectCodeID
/*INSERT*/
INSERT INTO Production.dbo.IETMS_AT_Detail(
 [IETMSUkey]
,[CodeFrom]
,[IESelectCodeType]
,[IESelectCodeID]
,[Number]
,[Value]
,[Remark]
,[AddName]
,[AddDate]
,[EditName]
,[EditDate]
)
SELECT 
 [IETMSUkey] = ISNULL(TIAD.IETMSUkey,0)
,[CodeFrom] = ISNULL(TIAD.CodeFrom,'')
,[IESelectCodeType] = ISNULL(TIAD.IESelectCodeType,'')
,[IESelectCodeID] = ISNULL(TIAD.IESelectCodeID,'')
,[Number] = ISNULL(TIAD.Number,0)
,[Value] = ISNULL(TIAD.[Value],0)
,[Remark] = ISNULL(TIAD.Remark,'')
,[AddName] = ISNULL(TIAD.AddName,'')
,[AddDate] = TIAD.AddDate
,[EditName] = ISNULL(TIAD.EditName,'')
,[EditDate] = TIAD.EditDate
FROM Trade_To_Pms.dbo.IETMS_AT_Detail TIAD WITH (NOLOCK)
WHERE NOT EXISTS(SELECT 1 FROM Production.dbo.IETMS_AT_Detail IAD WITH (NOLOCK) WHERE IAD.IETMSUkey = TIAD.IETMSUkey AND IAD.CodeFrom = TIAD.CodeFrom AND IAD.IESelectCodeType = TIAD.IESelectCodeType and IAD.IESelectCodeID = TIAD.IESelectCodeID)
----------------------------------Pattern_GL_Artwork----------------------------------
/*DELETE*/
DELETE Production.dbo.Pattern_GL_Artwork
FROM Production.dbo.Pattern_GL_Artwork PGA
LEFT JOIN Trade_To_Pms.dbo.Pattern_GL_Artwork TPGA ON PGA.UKEY = TPGA. UKEY
WHERE TPGA. UKEY IS NULL
/*UPDATE*/
UPDATE PGA
SET  
 PGA.ID = ISNULL(TPGA.ID,'')
,PGA.[Version] = ISNULL(TPGA.[Version],'')
,PGA.UKEY = ISNULL(TPGA.UKEY,0)
,PGA.SEQ = ISNULL(TPGA.SEQ,'')
,PGA.ArtworkTypeID = ISNULL(TPGA.ArtworkTypeID,'')
,PGA.PatternUkey = ISNULL(TPGA.PatternUkey,0)
,PGA.Annotation = ISNULL(TPGA.Annotation,'')
FROM Production.dbo.Pattern_GL_Artwork PGA
INNER JOIN Trade_To_Pms.dbo.Pattern_GL_Artwork TPGA ON PGA.UKEY = TPGA. UKEY
/*INSERT*/
INSERT INTO Production.dbo.Pattern_GL_Artwork(
[ID]
,[Version]
,[UKEY]
,[SEQ]
,[ArtworkTypeID]
,[PatternUkey]
,[Annotation]
)
SELECT 
 [ID] = ISNULL(TPGA.ID,'')
,[Version] = ISNULL(TPGA.[Version],'')
,[UKEY] = ISNULL(TPGA.UKEY,0)
,[SEQ] = ISNULL(TPGA.SEQ,'')
,[ArtworkTypeID] = ISNULL(TPGA.ArtworkTypeID,'')
,[PatternUkey] = ISNULL(TPGA.PatternUkey,0)
,[Annotation] = ISNULL(TPGA.Annotation,'')
FROM Trade_To_Pms.dbo.Pattern_GL_Artwork TPGA WITH (NOLOCK)
WHERE NOT EXISTS(SELECT 1 FROM Production.dbo.Pattern_GL_Artwork PGA WITH (NOLOCK) WHERE PGA.UKEY = TPGA. UKEY)
----------------------------------------------------------------------------

----------------------------------SewingMachineAttachment----------------------------------

/*UPDATE*/
UPDATE t
SET  t.Picture1 = ISNULL(s.Picture1,'')
	,t.Picture2 = ISNULL(s.Picture2,'')
FROM Production.dbo.SewingMachineAttachment t
INNER JOIN Trade_To_Pms.dbo.SewingMachineAttachment s WITH(NOLOCK) ON t.ID = s.ID


----------------------------------ChgOverCheckListBase----------------------------------
/*DELETE*/
Delete Production.dbo.ChgOverCheckListBase
FROM Production.dbo.ChgOverCheckListBase PCCB
LEFT JOIN Trade_To_Pms.dbo.ChgOverCheckListBase TCCB WITH(NOLOCK) ON TCCB.ID = PCCB.ID
WHERE TCCB.ID IS NULL

/*UPDATE*/
UPDATE PCCB
SET  
 PCCB.[NO] = ISNULL(TCCB.[No],0)
,PCCB.CheckList = ISNULL(TCCB.CheckList,'')
,PCCB. Junk = TCCB.Junk
,PCCB.AddName  = isnull(TCCB.AddName,'')
,PCCB.AddDate = TCCB.AddDate
,PCCB.EditName = ISNULL(TCCB.EditName,'')
,PCCB.EditDate = TCCB.EditDate
FROM Production.dbo.ChgOverCheckListBase PCCB
INNER JOIN Trade_To_Pms.dbo.ChgOverCheckListBase TCCB WITH(NOLOCK) ON TCCB.ID = PCCB.ID

/*INSERT*/
INSERT INTO Production.dbo.ChgOverCheckListBase(
[ID]
,[No]
,[CheckList]
,[Junk]
,[AddName] 
,[AddDate] 
,[EditName]
,[EditDate]
)
SELECT 
[ID] = ISNULL(TCCB.[ID],0)
,[NO] = ISNULL(TCCB.[No],0)
,[CheckList] = ISNULL(TCCB.CheckList,'')
,[Junk] = ISNULL(TCCB.Junk,0)
,[AddName] = ISNULL(TCCB.AddName ,'')
,[AddDate] = TCCB.AddDate
,[EditName] = ISNULL(TCCB.EditName,'')
,[EditDate] = TCCB.EditDate
FROM Trade_To_Pms.dbo.ChgOverCheckListBase TCCB WITH (NOLOCK)
WHERE NOT EXISTS(SELECT 1 FROM Production.dbo.ChgOverCheckListBase PCCB WITH (NOLOCK) WHERE TCCB.ID = PCCB.ID)



END


