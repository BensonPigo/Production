CREATE view dbo.View_ShowName as
select ID,
	[Name_Extno] = concat( RTRIM(p.name),' Ext.',RTRIM(p.ExtNo))
from dbo.Pass1 p