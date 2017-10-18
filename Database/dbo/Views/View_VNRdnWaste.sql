CREATE VIEW [dbo].[View_VNNLCodeWaste]
	AS select NLCode,  Waste = Round(RAND()*(WasteUpper-WasteLower)+WasteLower,3,1) 
	from VNNLCodeDesc
