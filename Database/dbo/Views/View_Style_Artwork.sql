CREATE VIEW [dbo].[View_Style_Artwork]
	AS 
SELECT 	sArtwork.[ArtworkTypeID]
		,sArticle.[Article]
		,sArtwork.[PatternCode]
		,sArtwork.[PatternDesc]
		,sArtwork.[ArtworkID]
		,sArtwork.[ArtworkName]
		,sArtwork.[Qty]
		,sArtwork.[Cost]
		,sArtwork.Remark
		,sArtwork.[StyleUkey]
		,sArtwork.TMS
		, [StyleArtworkUkey] = sArtwork.[Ukey]
FROM [dbo].[Style_Artwork] sArtwork
inner join Style_Article sArticle on sArtwork.StyleUkey = sArticle.StyleUkey
where sArtwork.Article = '----'
union ALL
SELECT 	sArtwork.[ArtworkTypeID]
		,sArticle.[Article]
		,sArtwork.[PatternCode]
		,sArtwork.[PatternDesc]
		,sArtwork.[ArtworkID]
		,sArtwork.[ArtworkName]
		,sArtwork.[Qty]
		,sArtwork.[Cost]
		,sArtwork.Remark
		,sArtwork.[StyleUkey]
		,sArtwork.TMS
		, [StyleArtworkUkey] = sArtwork.[Ukey]
FROM [dbo].[Style_Artwork] sArtwork
inner join Style_Article sArticle on sArtwork.StyleUkey = sArticle.StyleUkey
									 and sArtwork.Article = sArticle.Article
