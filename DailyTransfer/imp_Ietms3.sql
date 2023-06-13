
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
---------------------------UPDATE �DTABLE��ӷ�TABLE ���@��(�DTABLE�h���� �O�_�� ~�ӷ�TABLE�h���ܤ��z�|)
UPDATE a
SET  
a.MainID	= isnull( b.MainID               , '')
,a.Mr	= isnull( b.Mr                       , '')
,a.SMR	= isnull( b.SMR                      , '')
,a.BrandID	= isnull( b.BrandID              , '')
,a.StyleID	= isnull( b.StyleID              , '')
,a.SeasonID	= isnull( b.SeasonID             , '')
,a.StyleUkey	= isnull( b.StyleUkey        , 0)
,a.CountryID	= isnull( b.CountryID        , '')
,a.PatternNo	= isnull( b.PatternNo        , '')
,a.OldStyleID	= isnull( b.OldStyleID       , '')
,a.OldSeasonID	= isnull( b.OldSeasonID      , '')
,a.SizeGroup	= isnull( b.SizeGroup        , '')
,a.SizeCode	= isnull( b.SizeCode             , '')
,a.BuyReady	=  b.BuyReady
,a.Status	= isnull( b.Status               , '')
,a.StatusPattern	= isnull( b.StatusPattern, '')
,a.StatusIE	= isnull( b.StatusIE             , '')
,a.AddName	= isnull( b.AddName              , '')
,a.AddDate	=  b.AddDate
,a.EditName	= isnull( b.EditName             , '')
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
 isnull(ID           , '')
,isnull(MainID       , '')
,isnull(Mr           , '')
,isnull(SMR          , '')
,isnull(BrandID      , '')
,isnull(StyleID      , '')
,isnull(SeasonID     , '')
,isnull(StyleUkey    , 0)
,isnull(CountryID    , '')
,isnull(PatternNo    , '')
,isnull(OldStyleID   , '')
,isnull(OldSeasonID  , '')
,isnull(SizeGroup    , '')
,isnull(SizeCode     , '')
,BuyReady
,isnull(Status       , '')
,isnull(StatusPattern, '')
,isnull(StatusIE     , '')
,isnull(AddName      , '')
,AddDate
,isnull(EditName     , '')
,EditDate

from Trade_To_Pms.dbo.SMNotice as b WITH (NOLOCK)
where not exists(select id from Production.dbo.SMNotice as a WITH (NOLOCK) where a.id = b.id)

--SMNotice_Detail
Merge Production.dbo.smnotice_detail as t
Using (select * from Trade_To_Pms.dbo.smnotice_detail a WITH (NOLOCK) where id in (select id from Trade_To_Pms.dbo.smnotice WITH (NOLOCK)))as s
on t.id = s.id and t.type = s.type
when matched then
	update set 
		t.[UseFor]			= isnull(s.[UseFor]              , '')
		,t.[PhaseID]			= isnull(s.[PhaseID]         , '')
		,t.[RequireDate]		= s.[RequireDate]
		,t.[Apv2SampleTime]	= s.[Apv2SampleTime]
		,t.[Apv2SampleHandle]	= isnull(s.[Apv2SampleHandle], '')
		,t.[ApvName]			= isnull(s.[ApvName]         , '')
		,t.[ApvDate]			= s.[ApvDate]
		,t.[Factory]			= isnull(s.[Factory]         , '')
		,t.[IEConfirmMR]		= isnull(s.[IEConfirmMR]     , '')
		,t.[PendingStatus]	= isnull(s.[PendingStatus]       , 0)
		,t.[BasicPattern]		= isnull(s.[BasicPattern]    , '')
		,t.[Remark1]			= isnull(s.[Remark1]         , '')
		,t.[Remark2]			= isnull(s.[Remark2]         , '')
		,t.[AddName]			= isnull(s.[AddName]         , '')
		,t.[AddDate]			= s.[AddDate]
		,t.[EditName]			= isnull(s.[EditName]        , '')
		,t.[EditDate]			=s.[EditDate]
when not matched by target then 	
insert([ID],[Type],[UseFor],[PhaseID],[RequireDate],[Apv2SampleTime],[Apv2SampleHandle],[ApvName],[ApvDate],[Factory]
,[IEConfirmMR],[PendingStatus],[BasicPattern],[Remark1],[Remark2],[AddName],[AddDate],[EditName],[EditDate])
       VALUES
       (
              isnull(s.[ID],               ''),
              isnull(s.[Type],             ''),
              isnull(s.[UseFor],           ''),
              isnull(s.[PhaseID],          ''),
              s.[RequireDate],
              s.[Apv2SampleTime],
              isnull(s.[Apv2SampleHandle], ''),
              isnull(s.[ApvName],          ''),
              s.[ApvDate] ,
              isnull(s.[Factory],          ''),
              isnull(s.[IEConfirmMR],      ''),
              isnull(s.[PendingStatus],    0),
              isnull(s.[BasicPattern],     ''),
              isnull(s.[Remark1],          ''),
              isnull(s.[Remark2],          ''),
              isnull(s.[AddName],          ''),
              s.[AddDate],
              isnull(s.[EditName],         ''),
              s.[EditDate]
       ) ;


delete t
from Production.dbo.smnotice_detail t
left join Trade_To_Pms.dbo.smnotice_detail s on t.id = s.id and t.type = s.type
WHERE s.id is null 
and exists (select 1 from Trade_To_Pms.dbo.SMNotice a where a.id = t.id)

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

a.MarkerNo	= isnull( b.MarkerNo              , '')
,a.PatternSMID	= isnull( b.PatternSMID       , '')
,a.PatternVersion	= isnull( b.PatternVersion, '')
,a.ToFactory	= isnull( b.ToFactory         , '')
,a.TransLate	= isnull( b.TransLate         , 0)
,a.AddName	= isnull( b.AddName               , '')
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
     isnull(ID            , '')
    ,isnull(SEQ           , '')
    ,isnull(MarkerVersion , '')
    ,isnull(MarkerNo      , '')
    ,isnull(PatternSMID   , '')
    ,isnull(PatternVersion, '')
    ,isnull(ToFactory     , '')
    ,isnull(TransLate     , 0)
    ,isnull(AddName       , '')
    ,AddDate
 
from Trade_To_Pms.dbo.Marker_Send as b WITH (NOLOCK)
where not exists(select 1 from Production.dbo.Marker_Send as a WITH (NOLOCK) where a.id = b.id and a.SEQ=b.SEQ and a.MarkerVersion = b.MarkerVersion)

END





