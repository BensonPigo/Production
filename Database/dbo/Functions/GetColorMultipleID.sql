
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
		stuff((select concat('/', c2.Name)
			   from dbo.Color as c
			   LEFT join dbo.Color_multiple as m on m.ID = c.ID 
				  								    and m.BrandID = c.BrandId
               left join color c2 on c2.id = m.ColorID and c2.BrandID = m.BrandID
			   where c.ID = @ColorID and c.BrandId = @BrandID
			   order by m.Seqno
			   for xml path(''))
			 , 1, 1, '')
	)	
End