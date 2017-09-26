
CREATE Function [dbo].[GetColorMultipleID]
	(
	  @BrandID		VarChar(8)
	 ,@ColorID		VarChar(6)
	)
Returns VarChar(500)
As
Begin
	--Set NoCount On;
	RETURN (
		stuff((select '/' + m.ColorID 
			   from dbo.Color as c
			   LEFT join dbo.Color_multiple as m on m.ID = c.ID 
				  								    and m.BrandID = c.BrandId
			   where c.ID = @ColorID and c.BrandId = @BrandID
			   order by m.Seqno
			   for xml path(''))
			 , 1, 1, '')
	)	
End