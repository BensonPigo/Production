

CREATE FUNCTION [dbo].[GetSimilarStyleList]
(
	@StyleUKey bigint
)
RETURNS nvarchar(max)
AS
BEGIN
	
	return isnull( (select rtrim(ChildrenStyleID)+'/' from Style_SimilarStyle as tmp where MasterStyleUkey = @StyleUKey for XML path('')),'')

END