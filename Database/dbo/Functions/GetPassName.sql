CREATE Function [dbo].[GetPassName] (
	  @UserID 		VarChar(10) 
)
Returns @rtnTable Table (
	  ID		  varchar(10)
	 ,NameOnly	  nvarchar(30)
	 ,NameAndExt  nvarchar(50)
	 ,IdAndNameAndExt  nvarchar(100)
	 ,IdAndName  nvarchar(100)
)
As
Begin 

	if isnull(@UserID, '') = ''
	begin
		return;
	end 
	 
	if exists (select 1 from Pass1 where ID = @UserID)
	begin
		insert into @rtnTable(ID, NameOnly, NameAndExt, IdAndNameAndExt, IdAndName)
		Select ID
			 , Name as NameOnly
			 , (RTrim(Name) + ' #' + ExtNo) as NameAndExt
			 , (RTrim(ID) + '-' + RTrim(Name) + ' #' + ExtNo) as IdAndNameAndExt
			 , (RTrim(ID) + '-' + RTrim(Name)) as IdAndName
		From Pass1 p
		where p.ID = @UserID
	end
	else
	begin
		insert into @rtnTable(ID, NameOnly, NameAndExt, IdAndNameAndExt, IdAndName)
		Select ID
			 , Name as NameOnly
			 , (RTrim(Name) + ' #' + ExtNo) as NameAndExt
			 , (RTrim(ID) + '-' + RTrim(Name) + ' #' + ExtNo) as IdAndNameAndExt
			 , (RTrim(ID) + '-' + RTrim(Name)) as IdAndName
		From TPEPass1 p
		where p.ID = @UserID
	end

	Return;
End
