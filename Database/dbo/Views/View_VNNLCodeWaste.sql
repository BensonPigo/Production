

CREATE VIEW [dbo].[View_VNNLCodeWaste]
    AS 

	SELECT NLCode
	,Waste  = ROUND(((WasteUpper - WasteLower) * RAND() + WasteLower), 3)
	,ContractNo = ID
	FROM VNContract_Detail

union all	
		
	select NLCode
	,Waste = Round(RAND()*(WasteUpper-WasteLower)+WasteLower,3,1) 
	,ContractNo=''
    from VNNLCodeDesc
