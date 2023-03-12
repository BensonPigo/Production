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
--a.ID	= b.ID
--,a.Version	= b.Version
a.Ukey	= b.Ukey
,a.IEName	= b.IEName
,a.ActFinDate	= b.ActFinDate
,a.GSDStyleCode	= b.GSDStyleCode
,a.GSDStyleTitle	= b.GSDStyleTitle
,a.AddName	= b.AddName
,a.AddDate	= b.AddDate
,a.EditName	= b.EditName
,a.EditDate	= b.EditDate
,a.GSDType	= isnull(b.GSDType, '')

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
,isnull(b.GSDType, '')
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
a.IETMSUkey	= b.IETMSUkey
,a.SEQ	= b.SEQ
,a.Location	= b.Location
,a.OperationID	= b.OperationID
,a.Mold	= b.Mold
,a.Annotation	= b.Annotation
,a.Frequency	= b.Frequency
,a.SMV	= b.SMV
,a.SeamLength	= b.SeamLength
,a.MtlFactorID   =b.MtlFactorID
,a.MtlFactorRate  =b.MtlFactorRate
--,a.UKey	= b.UKey

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
	t.[StyleUkey]	=s.[StyleUkey],
	t.[ProSMV]		=s.[ProSMV]   ,
	t.[ProTMS]		=s.[ProTMS]	  ,
	t.[ProPrice]	=s.[ProPrice]
when not matched by target then
insert ([IETMSUkey],[StyleUkey],[MachineTypeID],[ArtworkTypeID],[Location],[ProSMV],[ProTMS],[ProPrice])
values (s.[IETMSUkey],s.[StyleUkey],s.[MachineTypeID],s.[ArtworkTypeID],s.[Location],s.[ProSMV],s.[ProTMS],s.[ProPrice]);

merge IETMS_Summary_Detail t
using (select * from trade_to_pms.dbo.IETMS_Summary_Detail) s
on t.[IETMSUkey] = s.[IETMSUkey] and t.[ArtworkTypeID]=s.[ArtworkTypeID] and t.[CIPF]=s.[CIPF]
and exists (select 1 from dbo.IETMS where Ukey = s.IETMSUkey)
when matched then update set 
 t.[StyleUkey]		=s.[StyleUkey]
,t.[ProSMV]			=s.[ProSMV]	 
,t.[ProTMS]			=s.[ProTMS]	 
,t.[ProPrice]		=s.[ProPrice] 
when not matched by target then
insert ([IETMSUkey],[StyleUkey],[ArtworkTypeID],[ProSMV],[ProTMS],[ProPrice],[CIPF])
values (s.[IETMSUkey],s.[StyleUkey],s.[ArtworkTypeID],s.[ProSMV],s.[ProTMS],s.[ProPrice],s.[CIPF]);
END





