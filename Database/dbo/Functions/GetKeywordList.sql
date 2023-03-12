
Create Function [dbo].[GetKeywordList]
	(
	  @Keyword	VarChar(Max)
	)
Returns @List_Keyword table
(
	Seq Int
	, ColumnName VarChar(200)
)
As
Begin
	Declare @splitter2 VarChar(1) = '}';
	Insert Into @List_Keyword
		(Seq, ColumnName)
		Select Row_Number() Over (Order by k.[No])
			 , c.content
		  From Production.dbo.SplitString(@Keyword, '{') as k
		 Outer Apply (Select endidx	= Charindex(@splitter2, k.[Data])) as i
		 Outer Apply (Select content = IIF(IsNull(i.endidx, 0) > 0, Substring(k.[Data], 1, i.endidx - 1), '')) as c
		 inner join Production.dbo.Keyword on Keyword.ID = c.content
		 Where c.content != ''
	Return
End