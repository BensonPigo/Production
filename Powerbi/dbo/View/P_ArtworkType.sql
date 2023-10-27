CREATE VIEW [dbo].[P_ArtworkType]
	AS 
	SELECT	[ArtworkTypeNo] =  Seq,
			[ArtworkType] =  ID,
			Classify,
			[ArtworkTypeUnit] =  ArtworkUnit,
			[ArtworkTypeKey] =  iif(ArtworkUnit = '', ID, concat(ID, '-', ArtworkUnit))
	FROM [MainServer].Production.dbo.ArtworkType
