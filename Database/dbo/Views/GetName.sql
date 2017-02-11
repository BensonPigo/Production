
/*
If Object_Id ( 'GetName', 'V' ) Is Not Null
   Drop View GetName;
*/
Create View [dbo].[GetName] As
Select ID
     , Name as NameOnly
	 , (RTrim(Name) + ' #' + ExtNo) as NameAndExt
	 , (RTrim(ID) + '-' + RTrim(Name) + ' #' + ExtNo) as IdAndNameAndExt
	 , (RTrim(ID) + '-' + RTrim(Name)) as IdAndName
  From Dbo.Pass1