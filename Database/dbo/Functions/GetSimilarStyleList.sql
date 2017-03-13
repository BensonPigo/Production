

CREATE FUNCTION [dbo].[GetSimilarStyleList]
(
	@StyleUKey bigint
)
RETURNS nvarchar(max)
AS
BEGIN
	
	return isnull( (select rtrim(ChildrenStyleID)+'/' from Style_SimilarStyle as tmp WITH (NOLOCK) where ChildrenStyleUkey = @StyleUKey for XML path('')),'')

END