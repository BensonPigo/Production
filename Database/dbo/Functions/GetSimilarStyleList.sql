

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
		select r.STY
		from (
		select MasterStyleUkey 
		from Style_SimilarStyle tmpStyles WITH (NOLOCK)
		where (MasterStyleUkey=@StyleUKey or ChildrenStyleUkey=@StyleUKey)
		) tmpStyle
		outer apply(
		     SELECT  ( select distinct B.MasterStyleID+'',
						  (SELECT '/'+C.ChildrenStyleID
						   from Style_SimilarStyle s2 WITH (NOLOCK)
						   OUTER APPLY(SELECT DISTINCT MasterStyleID FROM Style_SimilarStyle WITH (NOLOCK) where MasterStyleUkey=tmpStyle.MasterStyleUkey)B
						   OUTER APPLY(SELECT ChildrenStyleID FROM Style_SimilarStyle WITH (NOLOCK) where MasterStyleUkey=tmpStyle.MasterStyleUkey)C
						   where (s2.MasterStyleUkey=@StyleUKey or s2.ChildrenStyleUkey=@StyleUKey)
						   for xml path(''))
						FROM Style_SimilarStyle WITH (NOLOCK)
						OUTER APPLY(SELECT DISTINCT MasterStyleID FROM Style_SimilarStyle WITH (NOLOCK) where MasterStyleUkey=tmpStyle.MasterStyleUkey)B
						FOR XML PATH(''))AS STY
					) r
		)		
		,'')

END