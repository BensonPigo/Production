CREATE view [dbo].[View_ShowName_TPE] as
select ID,
	[Name_Extno] = concat( RTRIM(p.name),' Ext.',RTRIM(p.ExtNo))
from dbo.TPEPass1 p
GO


