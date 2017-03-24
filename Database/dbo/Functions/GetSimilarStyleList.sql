

CREATE FUNCTION [dbo].[GetSimilarStyleList]
(
	@StyleUKey bigint
)
RETURNS nvarchar(max)
AS
BEGIN
	
	return isnull( 
		(
		--select rtrim(ChildrenStyleID)+'/' 
		--from Style_SimilarStyle as tmp WITH (NOLOCK) 
		--where ChildrenStyleUkey = @StyleUKey 
		--for XML path('')
		select r.MasterStyleID+'/'
		from (
		    SELECT distinct MasterStyleID
			FROM Style_SimilarStyle 
			where MasterStyleUkey in (select  distinct MasterStyleUkey from Style_SimilarStyle where MasterStyleUkey=@StyleUKey or ChildrenStyleUkey=@StyleUKey)
			UNION
			SELECT distinct ChildrenStyleID 
			FROM Style_SimilarStyle 
			where MasterStyleUkey in (select  distinct MasterStyleUkey from Style_SimilarStyle where MasterStyleUkey=@StyleUKey or ChildrenStyleUkey=@StyleUKey)
			)r
		FOR XML PATH('')
		)		
		,'')

END