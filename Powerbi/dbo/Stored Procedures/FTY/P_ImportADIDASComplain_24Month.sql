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

	if exists (select 1 from BITableInfo b where b.id = 'P_ImportADIDASComplain_24Month')
	begin
		update b
			set b.TransferDate = getdate()
				, b.IS_Trans = 1
		from BITableInfo b
		where b.id = 'P_ImportADIDASComplain_24Month'
	end
	else 
	begin
		insert into BITableInfo(Id, TransferDate)
		values('P_ImportADIDASComplain_24Month', getdate())
	end


END