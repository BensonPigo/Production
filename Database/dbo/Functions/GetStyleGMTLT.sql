

--mCustId,mStyle,mSeason,mFty,mStyleUkey

CREATE Function [dbo].[GetStyleGMTLT]
(
	  @BrandID	    VarChar(8)				--
	 ,@StyleID 	    VarChar(20)				--
	 ,@SeasonId		VarChar(10)				--
	 ,@Facotry		VarChar(8)				--
)
Returns numeric(3, 0)
As
 
Begin
  
   Declare @GMTLT Numeric(3,0)
   Declare @isFty bit

   Set @GMTLT = 0;

	set @isFty = iif(exists(Select GMTLT=@GMTLT from Style_GmtLtFty WITH (NOLOCK) Where StyleUkey = (Select Ukey From Style WITH (NOLOCK) Where id=@Styleid and BrandId = @BrandID and SeasonID=@SeasonID) ),1,0)
   
    if (@isFty=0)
     begin 
		
		 Select @GMTLT=GMTLT From Style WITH (NOLOCK) Where id=@Styleid and BrandId = @BrandID and SeasonID=@SeasonID
	End;

    return @GMTLT

End