/* 
=======================================================================================================================================
Author:		Jack
Create date: 2020/01/17
Description: 判斷會計科目 Vat, Import, Export, 姊妹廠代墊（SisFty）, 空運費(IsAPP), 代墊台北(AdvancePaymentTPE) 

統治科目(AccountNo.ID 前四碼) 判斷
 - 姊妹廠代墊(SisFty) 
 - 代墊台北(AdvancePaymentTPE)

Vat = AccountNo.IsShippingVAT
空運費(IsAPP) = AccountNo.IsAPP
代墊台北(AdvancePaymentTPE) = AccountNo.AdvancePaymentTPE
Import, Export, 姊妹廠代墊（SisFty） = DropDownList.Name and DropDownList.Type = 'ExpressType'
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

	--SisFty, AdvancePaymentTPE 判斷統治科目
	if @SplitStringData = LOWER('SisFty') or @SplitStringData = LOWER('AdvancePaymentTPE')
	begin
		set @ID = substring(@ID,1,4)
	end

	declare @rtnVal as bit = 0

	select @rtnVal = 
				case 
					when @SplitStringData = LOWER('vat') then an.IsShippingVAT
					when @SplitStringData = LOWER('AdvancePaymentTPE') then an.AdvancePaymentTPE 
					when @SplitStringData = LOWER('IsAPP') then an.IsAPP
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
