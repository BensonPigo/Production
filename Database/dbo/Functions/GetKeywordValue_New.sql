
CREATE FUNCTION [dbo].[GetKeywordValue_New]
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
			select @FieldValue = 
			case Upper(@KeyWordID)
				when Upper('Buy month') then (Select Orders.Buymonth From dbo.Orders where Orders.ID = @OrderID)
				when Upper('Country Code') then (Select Orders.BrandAreaCode From dbo.Orders where Orders.ID = @OrderID)
				when Upper('Fty code') then (Select Orders.BrandFTYCode From dbo.Orders where Orders.ID = @OrderID)
				when Upper('Orig Buyer deliver') then (Select CONVERT(char(10), OrigBuyerDelivery, 111) From dbo.Orders where Orders.ID = @OrderID)
				when Upper('Plant code') then (Select Orders.BrandAreaCode From dbo.Orders where Orders.ID = @OrderID)
				when Upper('Pono') then (Select Orders.CustPONo From dbo.Orders where Orders.ID = @OrderID)
				when Upper('Program') then (Select Orders.ProgramID From dbo.Orders where Orders.ID = @OrderID)
				when Upper('Season') then (Select Orders.SeasonID From dbo.Orders where Orders.ID = @OrderID)
				when Upper('SpecId1') then (Select Orders.Customize1 From dbo.Orders where Orders.ID = @OrderID)
				when Upper('Style') then (Select Orders.StyleID From dbo.Orders where Orders.ID = @OrderID)
				else ''
				end
		End;

		If Upper(@TableName) = Upper('Style')
		Begin
			select @FieldValue = 
			case Upper(@KeyWordID)
				when Upper('Type') then (
							Select Reason.Name From dbo.Orders 
							Left Join Production.dbo.Style On Style.Ukey = StyleUkey
							Left Join Production.dbo.Reason On Reason.ReasonTypeID = 'Style_Apparel_Type' And Reason.ID = Style.ApparelType
							Where Orders.ID = @OrderID)
				when Upper('Care code') then (
							Select Style.CareCode From dbo.Orders
							Left Join Production.dbo.Style On Style.Ukey = Orders.StyleUkey
							Where Orders.ID = @OrderID)
				when Upper('Content') then (
							Select IIF(IsNull(Style_Article.Contents, '') = '', Style.Contents, Style_Article.Contents)
							From dbo.Orders
							Left Join Production.dbo.Style On Style.Ukey = Orders.StyleUkey
							left join Production.dbo.Style_Article on Style_Article.StyleUkey = Style.Ukey and Style_Article.Article = @Article
							Where Orders.ID = @OrderID)
				when Upper('Gender') then (
							Select iif(Style.Gender = 'M', 'Male', iif(Style.Gender = 'F', 'Female', Style.Gender)) 
							From dbo.Orders
							Left Join Production.dbo.Style On Style.Ukey = Orders.StyleUkey
							Where Orders.ID = @OrderID)
				when Upper('Size page') then (
							Select Style.SizePage From dbo.Orders
							Left Join Production.dbo.Style On Style.Ukey = Orders.StyleUkey
							Where Orders.ID = @OrderID)
				when Upper('Style name') then (
							Select Style.StyleName From dbo.Orders
							Left Join Production.dbo.Style On Style.Ukey = Orders.StyleUkey
							Where Orders.ID = @OrderID)
				when Upper('GMTLT') then (
							Select cast(Style.GMTLT as nvarchar) From dbo.Orders
							Left Join Production.dbo.Style On Style.Ukey = Orders.StyleUkey
							Where Orders.ID = @OrderID)
				when Upper('Fit Type') then (
							Select DropDownList.Name From dbo.Orders
							Left Join Production.dbo.Style On Style.Ukey = Orders.StyleUkey
							Left Join Production.dbo.DropDownList on DropDownList.ID = Style.FitType and DropDownList.Type = 'FitType'
			  				Where Orders.ID = @OrderID)
				when Upper('Gear Line') then (
							Select DropDownList.Name From dbo.Orders
							Left Join Production.dbo.Style On Style.Ukey = Orders.StyleUkey
							Left Join Production.dbo.DropDownList on DropDownList.ID = Style.GearLine and DropDownList.Type = 'GearLine'
							Where Orders.ID = @OrderID)
				when Upper('Age Group') then (
							Select DropDownList.Name From dbo.Orders
							Left Join Production.dbo.Style On Style.Ukey = Orders.StyleUkey
							Left Join Production.dbo.DropDownList on DropDownList.ID = Style.AgeGroup and DropDownList.Type = 'AgeGroup'
							Where Orders.ID = @OrderID)
				when Upper('Brand Gender') then (
							Select DropDownList.Name From dbo.Orders
							Left Join Production.dbo.Style On Style.Ukey = Orders.StyleUkey
							Left Join Production.dbo.DropDownList on DropDownList.ID = Style.BrandGender and DropDownList.Type = 'BrandGender'
							Where Orders.ID = @OrderID)
				else ''
				end
		End;
				
		If Upper(@TableName) = Upper('Brand_Month')
		Begin
			-- Buyer delivery
			Select @FieldValue = Brand_Month.MonthLabel From dbo.Orders
			Left Join Production.dbo.Brand_Month On Brand_Month.ID = Orders.BrandID
				And Brand_Month.Year = Year(Orders.BuyerDelivery)
				And Brand_Month.Month = Month(Orders.BuyerDelivery)
			Where Orders.ID = @OrderID
		End;

		If Upper(@TableName) = Upper('Factory')
		Begin
			-- Factory address
			Select @FieldValue = Factory.AddressCH From dbo.Orders Left Join Production.dbo.Factory On Factory.ID = Orders.FactoryID Where Orders.ID = @OrderID
		End;
		
		If Upper(@TableName) = Upper('FtyCountry')
		Begin
			If Upper(@KeyWordID) = Upper('COO')
			Begin
				Select @FieldValue = FTYCOUNTRY.ALIAS From dbo.Orders 
				Left Join Production.dbo.Factory On Factory.ID = Orders.FactoryID
				Left Join Production.dbo.Country as FtyCountry On FtyCountry.ID = Factory.CountryID
				Where Orders.ID = @OrderID
			END			
			Else If Upper(@KeyWordID) = Upper('COOCode')
			Begin
				Select @FieldValue = Factory.CountryID
				From dbo.Orders 
				Left Join Production.dbo.Factory On Factory.ID = Orders.FactoryID
				Where Orders.ID = @OrderID
			END
		End;

		If Upper(@TableName) = Upper('Factory_BrandDefinition')
		Begin
			If Upper(@KeyWordID) = Upper('SpecialFactoryCode')
			Begin
				Select top 1 @FieldValue = fb.BrandVendorCode
				From dbo.Orders o
				Left Join Production.dbo.Factory_BrandDefinition fb On fb.ID = o.FactoryID
					And fb.BrandID = o.BrandID
					And (Isnull(fb.CDCodeID, '') = '' 
						Or (Isnull(fb.CDCodeID, '') <> '' And fb.CDCodeID = o.CDCodeID))
					And CharIndex(o.BrandFTYCode, fb.BrandFTYCode) > 0
				Where o.ID = @OrderID
				Order By fb.CDCodeID Desc
			End

			If Upper(@KeyWordID) = Upper('V-Code')
			Begin
				Select top 1 @FieldValue = fb.V_Code
				From dbo.Orders o
				Left Join Production.dbo.Factory_BrandDefinition fb On fb.ID = o.FactoryID
					And fb.BrandID = o.BrandID
					And (Isnull(fb.CDCodeID, '') = '' 
						Or (Isnull(fb.CDCodeID, '') <> '' And fb.CDCodeID = o.CDCodeID))
					And CharIndex(o.BrandFTYCode, fb.BrandFTYCode) > 0
				Where o.ID = @OrderID
				Order By fb.CDCodeID Desc
			End
		End

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
			End
			Else If Upper(@KeyWordID) = Upper('Location Apparel Type')
			Begin
				Select @FieldValue = isnull(Reason.Name, '')
				From dbo.Orders 
					Left Join Production.dbo.Style On Style.BrandID = Orders.BrandID
						And Style.ID = Orders.StyleID And Style.SeasonID = Orders.SeasonID
					Left Join Production.dbo.Style_Location on Style.Ukey = Style_Location.StyleUkey
					Left Join Production.dbo.Reason On Reason.ReasonTypeID = 'Style_Apparel_Type' And Reason.ID = Style_Location.ApparelType
				Where Orders.ID = @OrderID and Style_Location.Location = @Location
			End;
		End;

		If Upper(@TableName) = Upper('Season')
		Begin
			If Upper(@KeyWordID) = Upper('SeasonForDisplay')
			Begin
				Select @FieldValue = Season.SeasonForDisplay
				From dbo.Orders
					Left Join Production.dbo.Season On Orders.SeasonID = Season.ID and Orders.BrandID = Season.BrandID
				Where Orders.ID = @OrderID
			END
		End;

		If Upper(@TableName) = Upper('Order_Article')
		Begin
			If Upper(@KeyWordID) = Upper('Certificate Number')
			Begin
				Select @FieldValue = CertificateNumber
				From dbo.Order_Article
				Where Order_Article.ID = @OrderID and Article = @Article
			END			
			Else If Upper(@KeyWordID) = Upper('Security Code')
			Begin
				Select @FieldValue = SecurityCode
				From dbo.Order_Article
				Where Order_Article.ID = @OrderID and Article = @Article
			END
		End;

		If Upper(@TableName) = Upper('Style_Article')
		Begin
			If Upper(@KeyWordID) = Upper('ArticleName')
			Begin
				Select @FieldValue = Style_Article.ArticleName
				From dbo.Orders
				Left Join Production.dbo.Style_Article On Style_Article.StyleUkey = Orders.StyleUkey
				Where Style_Article.Article = @Article
			END
		End;
	End;
	
	return ISNULL(@FieldValue, '')

END