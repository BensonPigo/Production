
CREATE FUNCTION [dbo].[GetKeywordValue]
(
	  @OrderID		VarChar(13)
	 ,@KeyWordID	VarChar(30)
	 ,@Article		VarChar(8)	= ''
	 ,@SizeCode		VarChar(8)	= ''
	 ,@Location		VarChar(1)	= ''
)
RETURNS nvarchar(max)
AS
BEGIN
	declare @FieldValue nvarchar(max) = ''	
	Declare @TableName VarChar(50);
	Declare @FieldName VarChar(100);

	--Set FieldName
	Select @FieldName = Keyword.FieldName
	  From Production.dbo.Keyword
	 Where Keyword.ID = @KeyWordID;
	
	--FieldName is Empty
	If IsNull(@FieldName, '') = ''
	Begin
		Set @FieldValue = Char(39) + '{' + @KeyWordID + '}' + Char(39);
	End;
	Else
	Begin
		Set @TableName = SubString(@FieldName, 1, CharIndex('.', @FieldName) - 1);

		If Upper(@TableName) = Upper('Orders')
		Begin			
			if(@KeyWordID = 'Buy month') Select @FieldValue = Orders.Buymonth From dbo.Orders where Orders.ID = @OrderID
			if(@KeyWordID = 'Country Code') Select @FieldValue = Orders.BrandAreaCode From dbo.Orders where Orders.ID = @OrderID
			if(@KeyWordID = 'Fty code') Select @FieldValue = Orders.BrandFTYCode From dbo.Orders where Orders.ID = @OrderID
			if(@KeyWordID = 'Orig Buyer deliver') Select @FieldValue = Orders.OrigBuyerDelivery From dbo.Orders where Orders.ID = @OrderID
			if(@KeyWordID = 'Plant code') Select @FieldValue = Orders.BrandAreaCode From dbo.Orders where Orders.ID = @OrderID
			if(@KeyWordID = 'Pono') Select @FieldValue = Orders.CustPONo From dbo.Orders where Orders.ID = @OrderID
			if(@KeyWordID = 'Program') Select @FieldValue = Orders.ProgramID From dbo.Orders where Orders.ID = @OrderID
			if(@KeyWordID = 'Season') Select @FieldValue = Orders.SeasonID From dbo.Orders where Orders.ID = @OrderID
			if(@KeyWordID = 'SpecId1') Select @FieldValue = Orders.Customize1 From dbo.Orders where Orders.ID = @OrderID
			if(@KeyWordID = 'Style') Select @FieldValue = Orders.StyleID From dbo.Orders where Orders.ID = @OrderID
						
		End;

		If Upper(@TableName) = Upper('Style')
		Begin
			If Upper(@KeyWordID) = Upper('Type')
			Begin
				Select @FieldValue = Reason.Name From dbo.Orders 
					Left Join Production.dbo.Style On Style.Ukey = StyleUkey
					Left Join Production.dbo.Reason On Reason.ReasonTypeID = 'Style_Apparel_Type'
						And Reason.ID = Style.ApparelType
				Where Orders.ID = @OrderID

			End;
			Else
			Begin
				if(@KeyWordID = 'Care code') Select @FieldValue = Style.CareCode From dbo.Orders Left Join Production.dbo.Style On Style.BrandID = Orders.BrandID And Style.ID = Orders.StyleID And Style.SeasonID = Orders.SeasonID Where Orders.ID = @OrderID
				----------------------------------------------------------------------------------
				--2017.09.11 modify by Ben, 當Style Article的Contents為空時，抓取Style的Contents
				--if(@KeyWordID = 'Content') Select @FieldValue = isnull(Style_Article.Contents, Style.Contents) From dbo.Orders Left Join dbo.Style On Style.BrandID = Orders.BrandID And Style.ID = Orders.StyleID And Style.SeasonID = Orders.SeasonID left join Style_Article on Style_Article.StyleUkey = Style.Ukey and Style_Article.Article = @Article Where Orders.ID = @OrderID
				if(@KeyWordID = 'Content') Select @FieldValue = IIF(IsNull(Style_Article.Contents, '') = '', Style.Contents, Style_Article.Contents) From dbo.Orders Left Join Production.dbo.Style On Style.BrandID = Orders.BrandID And Style.ID = Orders.StyleID And Style.SeasonID = Orders.SeasonID left join Production.dbo.Style_Article on Style_Article.StyleUkey = Style.Ukey and Style_Article.Article = @Article Where Orders.ID = @OrderID
				----------------------------------------------------------------------------------
				if(@KeyWordID = 'Gender') Select @FieldValue = iif(Style.Gender = 'M','Male',iif(Style.Gender = 'F','Female',Style.Gender)) 
																				From dbo.Orders Left Join Production.dbo.Style On Style.BrandID = Orders.BrandID And Style.ID = Orders.StyleID And Style.SeasonID = Orders.SeasonID Where Orders.ID = @OrderID
				if(@KeyWordID = 'Size page') Select @FieldValue = Style.SizePage From dbo.Orders Left Join Production.dbo.Style On Style.BrandID = Orders.BrandID And Style.ID = Orders.StyleID And Style.SeasonID = Orders.SeasonID Where Orders.ID = @OrderID
				if(@KeyWordID = 'Style name') Select @FieldValue = Style.Description From dbo.Orders Left Join Production.dbo.Style On Style.BrandID = Orders.BrandID And Style.ID = Orders.StyleID And Style.SeasonID = Orders.SeasonID Where Orders.ID = @OrderID
				if(@KeyWordID = 'GMTLT') Select @FieldValue = Style.GMTLT From dbo.Orders Left Join Production.dbo.Style On Style.BrandID = Orders.BrandID And Style.ID = Orders.StyleID And Style.SeasonID = Orders.SeasonID Where Orders.ID = @OrderID
				if(@KeyWordID = 'Fit Type') Select @FieldValue = DropDownList.Name  From dbo.Orders Left Join Production.dbo.Style On Style.BrandID = Orders.BrandID And Style.ID = Orders.StyleID And Style.SeasonID = Orders.SeasonID Left Join Production.dbo.DropDownList on DropDownList.ID = Style.FitType and DropDownList.Type = 'FitType'
			  	  Where Orders.ID = @OrderID
				if(@KeyWordID = 'Gear Line') Select @FieldValue = DropDownList.Name  From dbo.Orders Left Join Production.dbo.Style On Style.BrandID = Orders.BrandID And Style.ID = Orders.StyleID And Style.SeasonID = Orders.SeasonID Left Join Production.dbo.DropDownList on DropDownList.ID = Style.GearLine and DropDownList.Type = 'GearLine'
				  Where Orders.ID = @OrderID
			End;
		End;
				
		If Upper(@TableName) = Upper('Brand_Month')
		Begin
			Select @FieldValue = Brand_Month.MonthLabel From dbo.Orders
				Left Join Production.dbo.Brand_Month On Brand_Month.ID = Orders.BrandID
					And Brand_Month.Year = Year(Orders.BuyerDelivery)
					And Brand_Month.Month = Month(Orders.BuyerDelivery)
				Where Orders.ID = @OrderID

		End;

		If Upper(@TableName) = Upper('Factory')
		Begin			
			Select @FieldValue = Factory.AddressCH From dbo.Orders Left Join Production.dbo.Factory On Factory.ID = Orders.FactoryID Where Orders.ID = @OrderID

		End;
		
		If Upper(@TableName) = Upper('FtyCountry')
		Begin
			Select @FieldValue = FTYCOUNTRY.ALIAS From dbo.Orders 
				Left Join Production.dbo.Factory On Factory.ID = Orders.FactoryID
				Left Join Production.dbo.Country as FtyCountry On FtyCountry.ID = Factory.CountryID
			Where Orders.ID = @OrderID

		End;

		If Upper(@TableName) = Upper('HealthLabelSupp_FtyExpiration')
		Begin
			Select @FieldValue = HealthLabelSupp_FtyExpiration.Registry From dbo.Orders
				Left Join Production.dbo.CustCD On CustCD.BrandID = Orders.BrandID And CustCD.ID = Orders.CustCDID
				Left Join Production.dbo.HealthLabelSupp_FtyExpiration On HealthLabelSupp_FtyExpiration.ID = CustCd.HealthID
					And HealthLabelSupp_FtyExpiration.FactoryID = Orders.FactoryID
			Where Orders.ID = @OrderID
		End;

		If Upper(@TableName) = Upper('Style_Location')
		Begin
			If Upper(@KeyWordID) = Upper('Location')
			Begin
				Select @FieldValue = DropDownList.Name 
				From dbo.Orders 
					Left Join Production.dbo.Style On Style.BrandID = Orders.BrandID
						And Style.ID = Orders.StyleID And Style.SeasonID = Orders.SeasonID
					Left Join Production.dbo.Style_Location on Style.Ukey = Style_Location.StyleUkey
					Left Join Production.dbo.DropDownList on DropDownList.ID = Style_Location.Location and DropDownList.Type = 'Location'
				Where Orders.ID = @OrderID and Style_Location.Location = @Location
			End;
		End;
	End;
	
	return ISNULL(@FieldValue, '')

END