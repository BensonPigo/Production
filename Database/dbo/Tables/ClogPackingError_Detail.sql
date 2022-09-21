CREATE TABLE [dbo].ClogPackingError_Detail
(
	ID BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
	ClogPackingErrorID BIGINT NOT NULL DEFAULT 0, 
    PackingReasonIDForTypeEG VARCHAR(5) NOT NULL constraint DF_ClogPackingError_Detail_PackingReasonIDForTypeEG DEFAULT '', 
    PackingReasonIDForTypeEO VARCHAR(5) NOT NULL constraint DF_ClogPackingError_Detail_PackingReasonIDForTypeEO DEFAULT '', 
    PackingReasonIDForTypeET VARCHAR(5) NOT NULL constraint DF_ClogPackingError_Detail_PackingReasonIDForTypeET DEFAULT '', 
    AddName VARCHAR(10) NOT NULL constraint DF_ClogPackingError_Detail_AddName DEFAULT '', 
    AddDate DATETIME NULL, 
    EditName VARCHAR(10) NOT NULL constraint DF_ClogPackingError_Detail_EditName DEFAULT '', 
    EditDate DATETIME NULL
)
