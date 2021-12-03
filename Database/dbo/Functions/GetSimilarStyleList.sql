

CREATE FUNCTION GetSimilarStyleList
(
	@StyleUKey bigint
)
RETURNS nvarchar(max)
AS
BEGIN
	Declare @StyleID varchar(15)
	Declare @BrandID varchar(8)

	Select @StyleID = Id
	, @BrandID = BrandID
	From Style
	Where Ukey = @StyleUkey

	--2019/07/10 [IST20190986] modify by Anderson 調整為找出所有的Similar Style後排除自己
	return isnull( (select Distinct rtrim(StyleID)+'-'+main.BrandID+'/'
	From (
		Select StyleID = MasterStyleID, BrandID = MasterBrandID
		From Style_SimilarStyle 
		Where (MasterBrandID = @BrandID and MasterStyleID = @StyleID) Or (ChildrenBrandID = @BrandID and ChildrenStyleID = @StyleID)
		Union 
		Select StyleID = ChildrenStyleID, BrandID = ChildrenBrandID
		From Style_SimilarStyle 
		Where (MasterBrandID = @BrandID and MasterStyleID = @StyleID) Or (ChildrenBrandID = @BrandID and ChildrenStyleID = @StyleID)
	) main
	Left join Style s on s.ID = main.StyleID And s.BrandID = main.BrandID
	Where s.Ukey <> @StyleUKey
	for XML path('')),'')

END