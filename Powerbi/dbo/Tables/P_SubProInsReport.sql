CREATE TABLE [dbo].[P_SubProInsReport]
(
	[Ukey] BIGINT NOT NULL IDENTITY, 
    [FactoryID] VARCHAR(8) NOT NULL CONSTRAINT [PK_P_SubProInsReport_FactoryID] default (''), 
    [SubProLocationID] VARCHAR(20) NOT NULL CONSTRAINT [PK_P_SubProInsReport_SubProLocationID] default (''), 
    [InspectionDate] DATE NULL, 
    [SewInLine] DATE NULL, 
    [SewinglineID] VARCHAR(5) NOT NULL CONSTRAINT [PK_P_SubProInsReport_SewinglineID] default (''), 
    [Shift] VARCHAR(5) NOT NULL CONSTRAINT [PK_P_SubProInsReport_Shift] default (''), 
    [RFT] NUMERIC(5, 2) NOT NULL CONSTRAINT [PK_P_SubProInsReport_RFT] default (0), 
    [SubProcessID] VARCHAR(15) NOT NULL CONSTRAINT [PK_P_SubProInsReport_SubProcessID] default (''), 
    [BundleNo] VARCHAR(10) NOT NULL CONSTRAINT [PK_P_SubProInsReport_BundleNo] default (''), 
    [Artwork] VARCHAR(20) NOT NULL CONSTRAINT [PK_P_SubProInsReport_Artwork] default (''), 
    [OrderID] VARCHAR(13) NOT NULL CONSTRAINT [PK_P_SubProInsReport_OrderID] default (''), 
    [Alias] VARCHAR(30) NOT NULL CONSTRAINT [PK_P_SubProInsReport_Alias] default (''), 
    [BuyerDelivery] DATE NULL, 
    [BundleGroup] NUMERIC(5) NOT NULL CONSTRAINT [PK_P_SubProInsReport_BundleGroup] default (0), 
    [SeasonID] VARCHAR(10) NOT NULL CONSTRAINT [PK_P_SubProInsReport_SeasonID] default (''), 
    [StyleID] VARCHAR(15) NOT NULL CONSTRAINT [PK_P_SubProInsReport_StyleID] default (''), 
    [ColorID] VARCHAR(6) NOT NULL CONSTRAINT [PK_P_SubProInsReport_ColorID] default (''), 
    [SizeCode] VARCHAR(8) NOT NULL CONSTRAINT [PK_P_SubProInsReport_SizeCode] default (''), 
    [PatternDesc] NVARCHAR(100) NOT NULL CONSTRAINT [PK_P_SubProInsReport_PatternDesc] default (''), 
    [Item] VARCHAR(20) NOT NULL CONSTRAINT [PK_P_SubProInsReport_Item] default (''), 
    [Qty] NUMERIC(5) NOT NULL CONSTRAINT [PK_P_SubProInsReport_Qty] default (0), 
    [RejectQty] INT NOT NULL CONSTRAINT [PK_P_SubProInsReport_RejectQty] default (0), 
    [Machine] VARCHAR(50) NOT NULL CONSTRAINT [PK_P_SubProInsReport_Machine] default (''), 
    [Serial] VARCHAR(50) NOT NULL CONSTRAINT [PK_P_SubProInsReport_Serial] default (''), 
    [Junk] BIT NOT NULL CONSTRAINT [PK_P_SubProInsReport_Junk] default (0), 
    [Description] NVARCHAR(500) NOT NULL CONSTRAINT [PK_P_SubProInsReport_Description] default (''), 
    [DefectCode] VARCHAR(50) NOT NULL CONSTRAINT [PK_P_SubProInsReport_DefectCode] default (''), 
    [DefectQty] INT NOT NULL CONSTRAINT [PK_P_SubProInsReport_DefectQty] default (0), 
    [Inspector] VARCHAR(50) NOT NULL CONSTRAINT [PK_P_SubProInsReport_Inspector] default (''), 
    [Remark] NVARCHAR(300) NOT NULL CONSTRAINT [PK_P_SubProInsReport_Remark] default (''), 
    [AddDate] DATETIME NULL, 
    [RepairedDatetime] DATETIME NULL, 
    [RepairedTime] INT NOT NULL CONSTRAINT [PK_P_SubProInsReport_RepairedTime] default (0), 
    [ResolveTime] INT NOT NULL CONSTRAINT [PK_P_SubProInsReport_ResolveTime] default (0), 
    [SubProResponseTeamID] VARCHAR(1000) NOT NULL CONSTRAINT [PK_P_SubProInsReport_SubProResponseTeamID] default (''), 
    [CustomColumn1] VARCHAR(300) NOT NULL CONSTRAINT [PK_P_SubProInsReport_CustomColumn1] default (''), 
    --台北BI不用PK
    CONSTRAINT [PK_P_SubProInsReport] PRIMARY KEY ([Ukey])
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'加工段廠房位置',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReport',
    @level2type = N'COLUMN',
    @level2name = N'SubProLocationID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'車縫日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReport',
    @level2type = N'COLUMN',
    @level2name = N'SewInLine'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'第一次檢驗就通過的成功率',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReport',
    @level2type = N'COLUMN',
    @level2name = N'RFT'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'加工段',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReport',
    @level2type = N'COLUMN',
    @level2name = N'SubProcessID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工藝',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReport',
    @level2type = N'COLUMN',
    @level2name = N'Artwork'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'國家',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReport',
    @level2type = N'COLUMN',
    @level2name = N'Alias'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'部位說明',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReport',
    @level2type = N'COLUMN',
    @level2name = N'PatternDesc'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'衣服項目，T-shirt、Shorts…',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReport',
    @level2type = N'COLUMN',
    @level2name = N'Item'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'綁包數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReport',
    @level2type = N'COLUMN',
    @level2name = N'Qty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'不通過數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReport',
    @level2type = N'COLUMN',
    @level2name = N'RejectQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'加工段機器',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReport',
    @level2type = N'COLUMN',
    @level2name = N'Machine'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'加工段機器狀態',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReport',
    @level2type = N'COLUMN',
    @level2name = N'Junk'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'加工段機器說明',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReport',
    @level2type = N'COLUMN',
    @level2name = N'Description'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'瑕疵數量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReport',
    @level2type = N'COLUMN',
    @level2name = N'DefectQty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'操作員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReport',
    @level2type = N'COLUMN',
    @level2name = N'Inspector'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'修復完成日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReport',
    @level2type = N'COLUMN',
    @level2name = N'RepairedDatetime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'檢驗到綁包完成修復總時長',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReport',
    @level2type = N'COLUMN',
    @level2name = N'RepairedTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'總修復時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReport',
    @level2type = N'COLUMN',
    @level2name = N'ResolveTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'加工段負責單位',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReport',
    @level2type = N'COLUMN',
    @level2name = N'SubProResponseTeamID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'自訂欄位',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_SubProInsReport',
    @level2type = N'COLUMN',
    @level2name = N'CustomColumn1'