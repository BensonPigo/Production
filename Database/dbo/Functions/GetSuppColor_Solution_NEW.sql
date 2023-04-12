
CREATE Function [dbo].[GetSuppColor_Solution_NEW]
(
	  @ColorUkey	BigInt
	 ,@SeasonID		VarChar(10)
	 ,@SuppID		VarChar(6)
	 ,@BrandID		VarChar(8)
	 ,@ProgramID	VarChar(12)
	 ,@StyleID		VarChar(15)
	 ,@RefNo		VarChar(36)
	 ,@MtlTypeId    VarChar(20)
)
Returns Varchar(30)
As
Begin
	--Set NoCount On;
	Declare @SuppColor VarChar(30)
	Declare @CurMonth varchar(10)
	Declare @CurSCIMonth varchar(10)

	Select @CurMonth = Season.Month, @CurSCIMonth = SeasonSCI.Month
	From Season with(nolock)
	Left Join SeasonSCI with(nolock) On Season.SeasonSCIID = SeasonSCI.ID
	Where Season.ID = @SeasonID and Season.BrandID = @BrandID

	--2019/07/02 [IST20190918] Add by Vicky 調整當季無資料時，則抓取最近一季的資料
	Select Top 1 @SuppColor = SuppColor
	From dbo.Color c with(nolock)
	Left Join dbo.Color_SuppColor cs with(nolock) On c.Ukey = cs.ColorUkey
	Left Join dbo.Season with(nolock) On Season.ID = cs.SeasonID and Season.BrandID = c.BrandId
	Left Join dbo.SeasonSCI with(nolock) On Season.SeasonSCIID = SeasonSCI.ID
	Outer apply (
		Select value = iif(cs.SeasonID = @SeasonID, 0, 1)
	) isCurrentSeason
	Where c.Ukey = @ColorUkey
		And (cs.SuppID = @SuppID or exists(select 1 from dbo.Supp where Supp.ID = @SuppID And SuppGroupFabric = cs.SuppGroupFabric))
		And (isnull(cs.MtlTypeId, '') = isnull(@MtlTypeId, '') or isnull(cs.MtlTypeId, '') = '')
		And (isnull(cs.ProgramID, '') = isnull(@ProgramID, '') or isnull(cs.ProgramID, '') = '')
		And (isnull(cs.StyleID, '') =  isnull(@StyleID, '') or isnull(cs.StyleID, '') = '')
		And (isnull(cs.Refno, '') =  isnull(@RefNo, '') or isnull(cs.Refno, '') = '')
		And (Season.Month <= @CurMonth And SeasonSCI.Month <= @CurSCIMonth)
	order by isCurrentSeason.value, SeasonSCI.Month desc, Season.Month desc, isnull(cs.MtlTypeId, '') desc, isnull(cs.ProgramID, '') desc, isnull(cs.StyleID, '') desc, isnull(cs.Refno, '') desc;

	Return @SuppColor;
End