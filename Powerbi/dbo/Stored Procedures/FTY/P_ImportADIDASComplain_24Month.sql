CREATE PROCEDURE [dbo].[P_ImportADIDASComplain_24Month]
AS
BEGIN
	DECLARE @sqlcmd NVARCHAR(MAX) = '';

	set @sqlcmd = 
	N'
	Delete FROM dbo.P_ADIDASComplain_24Month

	Insert into dbo.P_ADIDASComplain_24Month
	Select 
	[Year]
	,[Month]
	,[BrandFtyCode]
	,[FactoryName]
	,[KPILO]
	,[PV_RAW_24Month]
	,[SV_RAW_24Month]
	,[WHC]
	,[Defective_Return]
	,isnull([AddName],'''''''')
	,[AddDate]
	,[EditName]
	,[EditDate]
	FROM MainServer.Production.dbo.ADIDASComplain_24Month
	'
	Exec(@sqlcmd)
END