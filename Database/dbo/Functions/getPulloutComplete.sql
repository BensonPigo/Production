
CREATE FUNCTION [dbo].[getPulloutComplete](@id varchar(13), @pulloutcomplete bit)
RETURNS varchar(3)
BEGIN
	DECLARE @string varchar(3) --要回傳的字串
	IF @pulloutcomplete = 1
		BEGIN
			SET @string = 'OK'
		END
	ELSE
		BEGIN
			Select @string = Count(Distinct pd.ID) 
            from PackingList p with(nolock), PackingList_Detail pd with(nolock)
            where p.PulloutID <> ''
            and p.ID = pd.ID
            and pd.ShipQty > 0
            and pd.OrderID = @id
		END

	RETURN @string
END