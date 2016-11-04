

-- =============================================
-- Author:		LEO	
-- Create date:20160903
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[imp_Ietms2]
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

-------------------------------------------------------aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
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
--a.ID	= b.ID
a.FromGSD	= b.FromGSD
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

---  20161104 新轉 OperationDesc  from Trade_to_pms

merge Production.dbo.OperationDesc as t
using Trade_to_Pms.dbo.OperationDesc as s 
	on t.id=s.id
	when matched then 
	update set 
	t.DescKH= s.DescKH,
	t.DescVi=s.DescVi,
	t.DescCHS=s.DescCHS
when not matched by target then
	insert(ID,DescKH,DescVi,DescCHS)
	values(s.ID,s.DescKH,s.DescVi,s.DescCHS)
when not matched by source then
	delete ;



----------------------�R���DTABLE�h�����
Delete Production.dbo.MachineType
from Production.dbo.MachineType as a left join Trade_To_Pms.dbo.MachineType as b
on a.id = b.id
where b.id is null
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
--a.ID	= b.ID
a.Description	= b.Description
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
--a.ID	= b.ID
a.Type	= b.Type
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
--aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
--SMNotice
--SMNotice

END





