

create view [dbo].[View_Order_Artworks]
as 
SELECT   a.[ID]
      ,a.[ArtworkTypeID]
      ,b.[Article]
	  ,b.Qty PoQty
	  ,b.SizeCode
      ,a.[PatternCode]
      ,a.[PatternDesc]
      ,a.[ArtworkID]
      ,a.[ArtworkName]
      ,a.[Qty]
      ,a.[Price]
      ,a.[Cost]
      ,a.[Remark]
      ,a.[Ukey]
      
  FROM [dbo].[Order_Artwork] a ,[dbo].[Order_Qty] b
  where a.Article = '----' and a.id = b.id
union ALL
SELECT   a.[ID]
      ,a.[ArtworkTypeID]
      ,b.[Article]
	  ,b.Qty PoQty
	  ,b.SizeCode
      ,a.[PatternCode]
      ,a.[PatternDesc]
      ,a.[ArtworkID]
      ,a.[ArtworkName]
      ,a.[Qty]
      ,a.[Price]
      ,a.[Cost]
      ,a.[Remark]
      ,a.[Ukey]
      
  FROM [dbo].[Order_Artwork] a ,[dbo].[Order_Qty] b
  where a.Article = b.Article and a.id = b.id




