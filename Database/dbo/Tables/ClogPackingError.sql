CREATE TABLE dbo.ClogPackingError
(
	ID BIGINT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    PackingErrorDate DATE NULL, 
    MDivisionID VARCHAR(8) NOT NULL constraint DF_ClogPackingError_MDivisionID DEFAULT '', 
    OrderID VARCHAR(13) NOT NULL constraint DF_ClogPackingError_OrderID  DEFAULT '', 
    PackingListID VARCHAR(13) NOT NULL constraint DF_ClogPackingError_PackingListID  DEFAULT '', 
    CTNStartNo VARCHAR(6) NOT NULL constraint DF_ClogPackingError_CTNStartNo  DEFAULT '', 
    PackingErrorID VARCHAR(8) NOT NULL, 
    SCICtnNo VARCHAR(16) NOT NULL constraint DF_ClogPackingError_SCICtnNo  DEFAULT '', 
    ErrQty SMALLINT NOT NULL constraint DF_ClogPackingError_ErrQty  DEFAULT 0, 
    CFMDate DATE NULL, 
    AddName VARCHAR(10) NOT NULL constraint DF_ClogPackingError_AddName  DEFAULT '', 
    AddDate DATETIME NULL, 
    EditName VARCHAR(10) NOT NULL constraint DF_ClogPackingError_EditName  DEFAULT '', 
    EditDate DATETIME NULL
)
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'接收日期', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'ClogPackingError'
, @level2type = N'COLUMN', @level2name = N'PackingErrorDate';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'箱號', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'ClogPackingError'
, @level2type = N'COLUMN', @level2name = N'CTNStartNo';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'包裝異常的原因', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'ClogPackingError'
, @level2type = N'COLUMN', @level2name = N'PackingErrorID';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'PackingListID + CTNStartNo', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'ClogPackingError'
, @level2type = N'COLUMN', @level2name = N'SCICtnNo';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'包裝異常的數量', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'ClogPackingError'
, @level2type = N'COLUMN', @level2name = N'ErrQty';
GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'返修完成日期', @level0type = N'SCHEMA', @level0name = N'dbo'
, @level1type = N'TABLE', @level1name = N'ClogPackingError'
, @level2type = N'COLUMN', @level2name = N'CFMDate';
GO
