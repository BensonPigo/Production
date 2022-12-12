
Create Function [dbo].[MarkerLengthSampleTOTrade]
(
	  @MarkerLength		varChar(20),
	  @MatchFabric		varchar(5)
)
Returns VarChar(20)
As
Begin

	Declare @newMarkerLength varchar(20);
	Declare @yd varchar(20);
	Declare @addstring varchar(20);
	Declare @length int ;
	Declare @ydno int;

	set @length = CharIndex('Yd',@MarkerLength);
	if(@length <= 0)
	begin
		set @newMarkerLength = '00Y';
	end
	else
	begin
		set @yd　= substring(@MarkerLength,0,@length);
		set @newMarkerLength = iif(len(@yd) =1,'0'+@yd+'Y',@yd+'Y'); -- 不足2碼補0
	end

	set @length = CharIndex('"',@MarkerLength);
	if(@length <= 0)
	begin
		set @newMarkerLength += '00-';
	end
	else
	begin
		set @ydno = CHARINDEX('YD',@MarkerLength);
		if(@ydno >= 0)
		begin
			set @ydno += 2;
		end
		else 
		begin
			set @ydno = 0;
		end

		Declare @inch varchar(20) = substring( @MarkerLength, @ydno,@length - @ydno)
		set @newMarkerLength += iif(len(@inch) =1,'0'+@inch+'-',@inch+'-'); -- 不足2碼補0
	end

	--當為"Body Mapping"時，[Marker Length]不必顯示+1
	if(@MatchFabric = 1)
	begin
		set @addstring = '"';
	end
	else
	begin
		set @addstring = '+1"';
	end
	set @length = CHARINDEX('/',@MarkerLength);

	if(@length <=0)
	begin
		set @newMarkerLength += '0/0'+@addstring;
	end
	else
	begin
		set @newMarkerLength += SUBSTRING(@MarkerLength,@length-1,3) +@addstring;
	end

	-- Return the result of the function
	Return @newMarkerLength

End