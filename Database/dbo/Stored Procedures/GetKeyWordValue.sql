	              

Create Procedure [dbo].[GetKeyWordValue]
	(
	  @OrderID		VarChar(13)
	 ,@KeyWordID	VarChar(30)
	 ,@FieldValue	VarChar(100) Output
	)
As
Begin
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
	Set NoCount On;
	
	Declare @FieldName VarChar(40);
	/*
	Select @FieldName = KeywordCommon.Fieldname
	  From dbo.KeywordCommon
	 Where KeywordCommon.ID = @KeyWordID;
	*/
	Select @FieldName = Keyword.FieldName
	  From dbo.Keyword
	 Where Keyword.ID = @KeyWordID;
	
	Create Table #FieldCursor
		(FieldValue Varchar(100));

	Declare @SqlCmd NVarchar(Max);

	Set @SqlCmd = N'Insert Into #FieldCursor'+
				   '	Select ' + IIF(IsNull(@FieldName, '') = '', Char(39) + '{' + @KeyWordID + '}' + Char(39), @FieldName) +
				   '	  From dbo.Orders' +
				   '	  Left Join dbo.Style' +
				   '		On	   Style.BrandID = Orders.BrandID' +
				   '		   And Style.ID = Orders.StyleID' +
				   '		   And Style.SeasonID = Orders.SeasonID' +
				   '	  Left Join Brand' +
				   '		On Brand.ID = Orders.BrandID' +
				   '	  Left Join dbo.Factory' +
				   '		On Factory.ID = Orders.FactoryID' +
				   '	  Left Join dbo.Country as FtyCountry' +
				   '		On FtyCountry.ID = Factory.CountryID' +
				   '	 Where Orders.ID = ''' + @OrderID + '''';
	
	Exec Sp_ExecuteSql @SqlCmd;

	Select @FieldValue = FieldValue 
	  From #FieldCursor;
	
	If Upper(@FieldName) = 'Style.Gender'
	Begin
		If @FieldValue = 'M'
		Begin
			Set @FieldValue = 'Male';
		End;
		If @FieldValue = 'F'
		Begin
			Set @FieldValue = 'Female';
		End;
	End;

	Drop Table #FieldCursor;
End
