

-- =============================================
-- Author:		LEO	
-- Create date:20160903
-- Description:	<Description,,>
-- =============================================
Create PROCEDURE [dbo].[imp_Ietms2]
	
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
,a.MachineAllowanceSMV	= b.MachineAllowanceSMV
,a.ManualAllowanceSMV	= b.ManualAllowanceSMV
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
,a.MasterPlusGroup	= b.MasterPlusGroup
,a.Hem = b.Hem
,a.Segment = b.Segment
,a.Tubular = b.Tubular

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
,MachineAllowanceSMV
,ManualAllowanceSMV
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
,MasterPlusGroup
,Hem
,Segment
,Tubular
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
,MachineAllowanceSMV
,ManualAllowanceSMV
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
,MasterPlusGroup
,Hem
,Segment
,Tubular
from Trade_To_Pms.dbo.Operation as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Operation as a WITH (NOLOCK) where a.id = b.id)

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

from Trade_To_Pms.dbo.Mold as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Mold as a WITH (NOLOCK) where a.id = b.id)
---------------------------
update m
	set m.IsAttachment = mt.IsAttachment,
		m.IsTemplate = mt.IsTemplate
from Production.dbo.Mold m
inner join Trade_To_Pms.dbo.MoldTPE mt on m.ID = mt.ID
where (m.IsAttachment <> mt.IsAttachment 
or m.IsTemplate <> mt.IsTemplate)
---------------------------
--aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa
--SMNotice
--SMNotice

--Table:IESelectCode--
merge IESelectCode t
using (select * from trade_to_pms.dbo.IESelectCode WITH (NOLOCK)) s
on t.id = s.id and t.type = s.type
when matched then
	update set 
		 t.name		= s.name		
		,t.AddName	= s.AddName	
		,t.AddDate	= s.AddDate	
		,t.EditName	= s.EditName
		,t.EditDate = s.EditDate
when not matched by target then
insert (type,id,name,AddName,AddDate,EditName,EditDate)
values (s.type,s.id,s.name,s.AddName,s.AddDate,s.EditName,s.EditDate);

END





