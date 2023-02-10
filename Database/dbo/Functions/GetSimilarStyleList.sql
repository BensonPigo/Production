

CREATE FUNCTION GetSimilarStyleList
(
	@StyleUKey bigint
)
RETURNS nvarchar(max)
AS
BEGIN
	Declare @StyleID varchar(15)
	Declare @BrandID varchar(8)

	Select @StyleID = Id, @BrandID = BrandID
	From Style With(nolock)
	Where Ukey = @StyleUkey

	DECLARE  @tmp_Style_SimilarStyle Table(MasterStyleID varchar(8), MasterBrandID varchar(15), ChildrenStyleID varchar(8), ChildrenBrandID varchar(15))

	INSERT INTO @tmp_Style_SimilarStyle
	Select MasterStyleID, MasterBrandID, ChildrenStyleID, ChildrenBrandID
	From Style_SimilarStyle With(nolock)
	Where MasterStyleID = @StyleID and MasterBrandID = @BrandID

	INSERT INTO @tmp_Style_SimilarStyle
	Select MasterStyleID, MasterBrandID, ChildrenStyleID, ChildrenBrandID
	From Style_SimilarStyle With(nolock)
	Where ChildrenStyleID = @StyleID and ChildrenBrandID = @BrandID 

	--2019/07/10 [IST20190986] modify by Anderson 調整為找出所有的Similar Style後排除自己
	return isnull( (select Distinct rtrim(StyleID)+'-'+main.BrandID+'/'
	From (
		Select distinct StyleID = MasterStyleID, BrandID = MasterBrandID
		From @tmp_Style_SimilarStyle
		Union ALL
		Select distinct StyleID = ChildrenStyleID, BrandID = ChildrenBrandID
		From @tmp_Style_SimilarStyle
	) main
	Left join Style s With(nolock) on s.ID = main.StyleID And s.BrandID = main.BrandID
	Where s.Ukey <> @StyleUKey
	for XML path('')),'')

END