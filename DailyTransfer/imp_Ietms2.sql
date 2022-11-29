

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

a.FromGSD	= isnull( b.FromGSD                        , 0)
,a.CalibratedCode	= isnull( b.CalibratedCode         , 0)
,a.DescEN	= isnull( b.DescEN                         , '')
,a.DescCH	= isnull( b.DescCH                         , '')
,a.MachineTypeID	= isnull( b.MachineTypeID          , '')
,a.MtlFactorID	= isnull( b.MtlFactorID                , '')
,a.ISO	= isnull( b.ISO                                , '')
,a.RPM	= isnull( b.RPM                                , 0)
,a.MoldID	= isnull( b.MoldID                         , '')
,a.OperationType	= isnull( b.OperationType          , '')
,a.CostCenter	= isnull( b.CostCenter                 , '')
,a.Section	= isnull( b.Section                        , '')
,a.SMV	= isnull( b.SMV                                , 0)
,a.MachineTMU	= isnull( b.MachineTMU                 , 0)
,a.ManualTMU	= isnull( b.ManualTMU                  , 0)
,a.TotalTMU	= isnull( b.TotalTMU                       , 0)
,a.MachineAllowanceSMV	= isnull( b.MachineAllowanceSMV, 0)
,a.ManualAllowanceSMV	= isnull( b.ManualAllowanceSMV , 0)
,a.StitchCM	= isnull( b.StitchCM                       , 0)
,a.SeamLength	= isnull( b.SeamLength                 , 0)
,a.NeedleThread	= isnull( b.NeedleThread               , '')
,a.BottomThread	= isnull( b.BottomThread               , '')
,a.CoverThread	= isnull( b.CoverThread                , '')
,a.NeedleLength	= isnull( b.NeedleLength               , 0)
,a.BottomLength	= isnull( b.BottomLength               , 0)
,a.CoverLength	= isnull( b.CoverLength                , 0)
,a.Junk	= isnull( b.Junk                               , 0)
,a.AddName	= isnull( b.AddName                        , '')
,a.AddDate	=  b.AddDate
,a.EditName	= isnull( b.EditName                       , '')
,a.EditDate	=  b.EditDate
,a.MasterPlusGroup	= isnull( b.MasterPlusGroup        , '')
,a.Hem = isnull( b.Hem                                 , 0)
,a.Segment = isnull( b.Segment                         , 0)
,a.Tubular = isnull( b.Tubular                         , 0)
,a.DescVN = isnull( b.DescVN                           , '')
,a.DescKH = isnull( b.DescKH                           , '')

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
,DescVN
,DescKH
)
select 
     isnull(ID                 , '')
    ,isnull(FromGSD            , 0)
    ,isnull(CalibratedCode     , 0)
    ,isnull(DescEN             , '')
    ,isnull(DescCH             , '')
    ,isnull(MachineTypeID      , '')
    ,isnull(MtlFactorID        , '')
    ,isnull(ISO                , '')
    ,isnull(RPM                , 0)
    ,isnull(MoldID             , '')
    ,isnull(OperationType      , '')
    ,isnull(CostCenter         , '')
    ,isnull(Section            , '')
    ,isnull(SMV                , 0)
    ,isnull(MachineTMU         , 0)
    ,isnull(ManualTMU          , 0)
    ,isnull(TotalTMU           , 0)
    ,isnull(MachineAllowanceSMV, 0)
    ,isnull(ManualAllowanceSMV , 0)
    ,isnull(StitchCM           , 0)
    ,isnull(SeamLength         , 0)
    ,isnull(NeedleThread       , '')
    ,isnull(BottomThread       , '')
    ,isnull(CoverThread        , '')
    ,isnull(NeedleLength       , 0)
    ,isnull(BottomLength       , 0)
    ,isnull(CoverLength        , 0)
    ,isnull(Junk               , 0)
    ,isnull(AddName            , '')
    ,AddDate
    ,isnull(EditName           , '')
    ,EditDate
    ,isnull(MasterPlusGroup    , '')
    ,isnull(Hem                , 0)
    ,isnull(Segment            , 0)
    ,isnull(Tubular            , 0)
    ,isnull(DescVN             , '')
    ,isnull(DescKH             , '')
from Trade_To_Pms.dbo.Operation as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Operation as a WITH (NOLOCK) where a.id = b.id)

--MACHTYPE
--MachineType


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
a.Type	= isnull( b.Type         , '')
,a.DescCH	= isnull( b.DescCH   , '')
,a.DescEN	= isnull( b.DescEN   , '')
,a.Junk	= isnull( b.Junk         , 0)
,a.AddName	= isnull( b.AddName  , '')
,a.AddDate	=  b.AddDate
,a.EditName	= isnull( b.EditName , '')
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
 isnull(ID      , '')
,isnull(Type    , '')
,isnull(DescCH  , '')
,isnull(DescEN  , '')
,isnull(Junk    , 0)
,isnull(AddName , '')
,AddDate
,isnull(EditName, '')
,EditDate

from Trade_To_Pms.dbo.Mold as b WITH (NOLOCK)
where not exists(select id from Production.dbo.Mold as a WITH (NOLOCK) where a.id = b.id)
---------------------------
update m
	set m.IsAttachment = isnull(mt.IsAttachment, 0),
		m.IsTemplate = isnull(mt.IsTemplate, 0)
from Production.dbo.Mold m
inner join Trade_To_Pms.dbo.MoldTPE mt on m.ID = mt.ID
where (m.IsAttachment <> mt.IsAttachment 
or m.IsTemplate <> mt.IsTemplate)
---------------------------

--Table:IESelectCode--
merge IESelectCode t
using (select * from trade_to_pms.dbo.IESelectCode WITH (NOLOCK)) s
on t.id = s.id and t.type = s.type
when matched then
	update set 
		 t.name		= isnull( s.name	, '')
		,t.AddName	= isnull( s.AddName	, '')
		,t.AddDate	=  s.AddDate
		,t.EditName	= isnull( s.EditName, '')
		,t.EditDate = s.EditDate
when not matched by target then
insert (type,id,name,AddName,AddDate,EditName,EditDate)
values (s.type,s.id,s.name,s.AddName,s.AddDate,s.EditName,s.EditDate);


------Operation_His------
Merge Production.dbo.Operation_His as t
Using (select a.* from Trade_To_Pms.dbo.Operation_His a ) as s
on t.Ukey=s.Ukey 
when matched then 
   update SET t.Type = isnull( s.Type       , '')
      ,t.TypeName = isnull( s.TypeName      , '')
      ,t.OperationID = isnull( s.OperationID, '')
      ,t.OldValue = isnull( s.OldValue      , '')
      ,t.NewValue = isnull( s.NewValue      , '')
      ,t.Remark = isnull( s.Remark          , '')
      ,t.EditDate =  s.EditDate
      ,t.EditName = isnull( s.EditName      , '')
when not matched by target then
	INSERT (Type
           ,TypeName
           ,OperationID
           ,OldValue
           ,NewValue
           ,Remark
           ,EditDate
           ,EditName
		   ,Ukey)
		VALUES  (
            isnull(s.Type       , '')
           ,isnull(s.TypeName   , '')
           ,isnull(s.OperationID, '')
           ,isnull(s.OldValue   , '')
           ,isnull(s.NewValue   , '')
           ,isnull(s.Remark     , '')
           ,s.EditDate
           ,isnull(s.EditName   , '')
		   ,isnull(s.Ukey       , 0)
           )
when not matched by source then 
	delete;
	
------Thread_Quilting------
Merge Production.dbo.Thread_Quilting as t
Using (select a.* from Trade_To_Pms.dbo.Thread_Quilting a ) as s
on t.Shape=s.Shape 
when matched then 
   update SET  t.Picture1 = isnull( s.Picture1, '')
			  ,t.Picture2 = isnull( s.Picture2, '')
			  ,t.Junk = isnull( s.Junk        , 0)
			  ,t.AddName = isnull( s.AddName  , '')
			  ,t.AddDate =  s.AddDate
			  ,t.EditName = isnull( s.EditName, '')
			  ,t.EditDate = s.EditDate
when not matched by target then
	INSERT (Shape
           ,Picture1
           ,Picture2
           ,Junk
           ,AddName
           ,AddDate
           ,EditName
           ,EditDate)
		VALUES  (
            isnull(s.Shape   , '')
           ,isnull(s.Picture1, '')
           ,isnull(s.Picture2, '')
           ,isnull(s.Junk    , 0)
           ,isnull(s.AddName , '')
           ,s.AddDate
           ,isnull(s.EditName, '')
           ,s.EditDate)
when not matched by source then 
	delete;

------Thread_Quilting_Size------
Merge Production.dbo.Thread_Quilting_Size as t
Using (select a.* from Trade_To_Pms.dbo.Thread_Quilting_Size a ) as s
on t.Ukey=s.Ukey 
when matched then 
   update SET  t.Shape = isnull( s.Shape                  , '')
			  ,t.HSize = isnull( s.HSize                  , 0)
			  ,t.VSize = isnull( s.VSize                  , 0)
			  ,t.ASize = isnull( s.ASize                  , 0)
			  ,t.NeedleDistance = isnull( s.NeedleDistance, 0)
when not matched by target then
	INSERT (Shape
           ,HSize
           ,VSize
           ,ASize
           ,NeedleDistance
		   ,Ukey)
		VALUES  (
            isnull(s.Shape         , '')
           ,isnull(s.HSize         , 0)
           ,isnull(s.VSize         , 0)
           ,isnull(s.ASize         , 0)
           ,isnull(s.NeedleDistance, 0)
		   ,isnull(s.Ukey          , 0)
           )
when not matched by source then 
	delete;

------Thread_Quilting_Size_Location------
Merge Production.dbo.Thread_Quilting_Size_Location as t
Using (select a.* from Trade_To_Pms.dbo.Thread_Quilting_Size_Location a ) as s
on t.Thread_Quilting_SizeUkey=s.Thread_Quilting_SizeUkey AND t.Seq=s.Seq 
when matched then 
   update SET  t.Shape = isnull( s.Shape      , '')
			  ,t.Location = isnull( s.Location, '')
			  ,t.Ratio = isnull( s.Ratio      , 0)
when not matched by target then
	INSERT (Shape
           ,Thread_Quilting_SizeUkey
           ,Seq
           ,Location
           ,Ratio)
		VALUES  (
            isnull(s.Shape                   , '')
           ,isnull(s.Thread_Quilting_SizeUkey, 0)
           ,isnull(s.Seq                     , '')
           ,isnull(s.Location                , '')
           ,isnull(s.Ratio                   , 0)
           )
when not matched by source then 
	delete;

END



