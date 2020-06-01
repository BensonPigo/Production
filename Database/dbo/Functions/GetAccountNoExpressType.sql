/* 
=======================================================================================================================================
Author:		Jack
Create date: 2020/01/17
Description: 判斷會計科目 Vat, Import, Export, 姊妹廠代墊（SisFty）, 空運費(IsAPP), 代墊台北(AdvancePaymentTPE) 

Vat = AccountNo.IsShippingVAT
空運費(IsAPP) = AccountNo.IsAPP
代墊台北(AdvancePaymentTPE) = AccountNo.AdvancePaymentTPE
Import, Export, 姊妹廠代墊（SisFty） = DropDownList.Name and DropDownList.Type = 'ExpressType'

規則 : 先判斷統制科目，沒有設定在判斷子科目
=======================================================================================================================================
*/
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

	-- 統制科目
	declare @controlAccount varchar(4) = substring(@ID,1,4) 
	declare @rtnVal as bit = 0

	-- 先判斷統制科目
	select @rtnVal = 
				case 
					when @SplitStringData = LOWER('vat') then an.IsShippingVAT
					when @SplitStringData = LOWER('AdvancePaymentTPE') then an.AdvancePaymentTPE 
					when @SplitStringData = LOWER('IsAPP') then an.IsAPP
				else splitString.value  
				end
	from FinanceEN.dbo.AccountNo an
	left join FinanceEN.dbo.DropDownList ddl on an.ExpressTypeID = ddl.ID and ddl.Type = 'ExpressType'
	outer apply (
		select value = cast(isnull( 
			(select 1 
			where exists (
				select Data
				from FinanceEN.dbo.SplitString(ddl.Name, '+')
				where LOWER(Data) = @SplitStringData))
		,0) as bit)
	) splitString
	where an.ID = @controlAccount

	-- 沒有設定在判斷子科目
	if @rtnVal = 0
	begin
		select @rtnVal = 
				case 
					when @SplitStringData = LOWER('vat') then an.IsShippingVAT
					when @SplitStringData = LOWER('AdvancePaymentTPE') then an.AdvancePaymentTPE 
					when @SplitStringData = LOWER('IsAPP') then an.IsAPP
				else splitString.value  
				end
		from FinanceEN.dbo.AccountNo an
		left join FinanceEN.dbo.DropDownList ddl on an.ExpressTypeID = ddl.ID and ddl.Type = 'ExpressType'
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
	end

	return @rtnVal
END
