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
END





