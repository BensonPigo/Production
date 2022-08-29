CREATE Function [dbo].[GetBomTypeValue]
(
    @Order_BoaUkey bigint
	, @BomType varchar(50)
	, @Location varchar(1)
	, @SizeCode VarChar(8)
)
Returns varchar(50)
As
Begin
    declare @Keyword varchar(max);
	declare @KeywordID varchar(30);
	declare @BomTypeValue varchar(50);
	declare @StyleUkey bigint;
	declare @OrderID varchar(13);
	declare @OrderComboID varchar(13);
	declare @SizeItem varchar(3);

	select @Keyword = Keyword from dbo.Order_BOA with (nolock) where Ukey = @Order_BoaUkey;

	select top 1 @KeywordID = Upper(ky.ID)
	from dbo.GetKeywordList(@Keyword) list
	inner join Production.dbo.Keyword ky with (nolock) on list.ColumnName = ky.ID
	inner join Production.dbo.Keyword_BomType kb with (nolock) on kb.ID = ky.ID
	where kb.BomType = @BomType;

	select @StyleUkey = o.StyleUkey
		, @OrderID = o.ID
		, @OrderComboID = OrderComboID
		, @SizeItem = case Upper(@KeywordID)
						when Upper('Customer size') then boa.CustomerSizeRelation
						when Upper('Dec lable size') then boa.DecLabelSizeRelation
						else '' end
	from dbo.Order_BOA boa with (nolock)
	inner join dbo.Orders o with (nolock) on boa.Id = o.ID
	where boa.Ukey = @Order_BoaUkey;

	IF @KeywordID in (Upper('Customer size'), Upper('Dec lable size'))
	Begin
		If Exists (Select 1 From Production.dbo.Order_SizeSpec_OrderCombo with (nolock) Where ID = @OrderID And OrderComboID = @OrderComboID And SizeItem = @SizeItem)
		Begin
			Select @BomTypeValue = IsNull(SizeSpec, '')
				From dbo.Order_SizeSpec_OrderCombo with (nolock)
				Where ID = @OrderID
				And OrderComboID = @OrderComboID
				And SizeItem = @SizeItem
				And SizeCode = @SizeCode;
		End;
		Else
		Begin
			Select @BomTypeValue = IsNull(SizeSpec, '')
				From dbo.Order_SizeSpec with (nolock)
				Where ID = @OrderID
				And SizeItem = @SizeItem
				And SizeCode = @SizeCode;
		End;
	End
	Else
	Begin
	select @BomTypeValue = 
	case
		when @KeywordID = Upper('COO')
			then (select f.CountryID from dbo.Orders o with (nolock) left join Factory f on f.ID = o.FactoryID where o.ID = @OrderID)
		when @KeywordID = Upper('Gender')
			then (select Gender from Production.dbo.Style with (nolock) where Ukey = @StyleUkey)
		when @KeywordID = Upper('Brand Gender')
			then (select BrandGender from Production.dbo.Style with (nolock) where Ukey = @StyleUkey)
		when @KeywordID in (Upper('Fty code'), Upper('Country Code'), Upper('Factory address'), Upper('SpecialFactoryCode'))
			then (select BrandFTYCode from dbo.Orders with (nolock) where ID = @OrderID)
		when @KeywordID = Upper('Location Apparel Type')
			then (select isnull(Reason.Name, '') from Production.dbo.Style_Location sl with (nolock)
					left join Reason on Reason.ReasonTypeID = 'Style_Apparel_Type' and Reason.ID = sl.ApparelType
					where sl.StyleUkey = @StyleUkey and sl.Location = @Location
						and (not exists(select 1 from dbo.Order_BOA_Location ol with (nolock) where ol.Order_BOAUkey = @Order_BoaUkey)
							or exists(select 1 from dbo.Order_BOA_Location ol with (nolock) where ol.Order_BOAUkey = @Order_BoaUkey and ol.Location = @Location)))
		when @KeywordID = Upper('Location')
			then (select sl.Location from Production.dbo.Style_Location sl with (nolock)
					where sl.StyleUkey = @StyleUkey and sl.Location = @Location
						and (not exists(select 1 from dbo.Order_BOA_Location ol with (nolock) where ol.Order_BOAUkey = @Order_BoaUkey)
							or exists(select 1 from dbo.Order_BOA_Location ol with (nolock) where ol.Order_BOAUkey = @Order_BoaUkey and ol.Location = @Location)))
		when @KeywordID = Upper('Season')
			then (select SeasonID from dbo.Orders with (nolock) where ID = @OrderID)
		when @KeywordID = Upper('SeasonForDisplay')
			then (select Season.SeasonForDisplay from dbo.Orders o with (nolock)
					left join Season with (nolock) on o.SeasonID = Season.ID and o.BrandID = Season.BrandID
					where o.ID = @OrderID)
		when @KeywordID = Upper('Care code')
			then (select CareCode from Production.dbo.Style with (nolock) where Ukey = @StyleUkey)

		-- BomType為Style時統一帶出StyleID
		when @BomType = 'Style'
			then (select StyleID from dbo.Orders with (nolock) where ID = @OrderID)
		else '' end;
	End

	Return isnull(@BomTypeValue, '')
End