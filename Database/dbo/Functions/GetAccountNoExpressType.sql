-- =============================================
-- Author:		Jack
-- Create date: 2020/01/17
-- Description:	判斷會計科目 Vat, Import, Export, 姊妹廠代墊（SisFty）
-- =============================================

CREATE FUNCTION [dbo].[GetAccountNoExpressType]
(
    @ID as varchar(8) = '',
    @SplitStringData varchar(20) = ''
)
RETURNS bit 
AS
BEGIN
	if isnull(@ID,'') = ''
	begin
		return 0
	end 

	if  isnull(@SplitStringData,'') = ''
	begin
		return 0
	end 

	set @SplitStringData = LOWER(@SplitStringData)

	declare @rtnVal as bit = 0

	select @rtnVal = case when @SplitStringData = 'vat' then an.IsShippingVAT
				else splitString.value  
				end
	from FinanceEN.dbo.AccountNo an
	inner join FinanceEN.dbo.DropDownList ddl on an.ExpressTypeID = ddl.ID and ddl.Type = 'ExpressType'
	outer apply (
		select value = cast(isnull( 
			(select 1 
			where exists (
				select Data
				from FinanceEN.dbo.SplitString(ddl.Name, '+')
				where LOWER(Data) = @SplitStringData))
		,0) as bit)
	) splitString
	where an.ID = @ID

	RETURN @rtnVal
END
