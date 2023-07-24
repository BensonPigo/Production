CREATE TABLE [dbo].[FirstDyelot] (
    [SuppID]              VARCHAR (6)   CONSTRAINT [DF_FirstDyelot_SuppID] DEFAULT ('') NOT NULL,
    [TestDocFactoryGroup] VARCHAR (8)   CONSTRAINT [DF_FirstDyelot_TestDocFactoryGroup] DEFAULT ('') NOT NULL,
    [BrandRefno]          VARCHAR (50)  CONSTRAINT [DF_FirstDyelot_BrandRefno] DEFAULT ('') NOT NULL,
    [ColorID]             VARCHAR (6)   CONSTRAINT [DF_FirstDyelot_ColorID] DEFAULT ('') NOT NULL,
    [SeasonID]            VARCHAR (8)   CONSTRAINT [DF_FirstDyelot_SeasonID] DEFAULT ('') NOT NULL,
    [Period]              INT           CONSTRAINT [DF_FirstDyelot_Period] DEFAULT ((0)) NOT NULL,
    [FirstDyelot]         DATE          NULL,
    [AWBno]               VARCHAR (30)  CONSTRAINT [DF_FirstDyelot_AWBno] DEFAULT ('') NULL,
    [AddName]             VARCHAR (10)  CONSTRAINT [DF_FirstDyelot_AddName] DEFAULT ('') NULL,
    [AddDate]             DATETIME      NULL,
    [EditName]            VARCHAR (10)  CONSTRAINT [DF_FirstDyelot_EditName] DEFAULT ('') NULL,
    [EditDate]            DATETIME      NULL,
    [FTYReceivedReport]   DATE          NULL,
    [ReceivedDate]        DATE          NULL,
    [ReceivedRemark]      VARCHAR (MAX) CONSTRAINT [DF_FirstDyelot_ReceivedRemark] DEFAULT ('') NULL,
    [DocumentName]        VARCHAR (100) CONSTRAINT [DF_FirstDyelot_DocumentName] DEFAULT ('') NOT NULL,
    [BrandID]             VARCHAR (8)   CONSTRAINT [DF_FirstDyelot_BrandID] DEFAULT ('') NOT NULL,
    [deleteColumn]        BIT           DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_FirstDyelot] PRIMARY KEY CLUSTERED ([SuppID] ASC, [TestDocFactoryGroup] ASC, [BrandRefno] ASC, [ColorID] ASC, [SeasonID] ASC, [DocumentName] ASC, [BrandID] ASC)
);








GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'刪除資料用備份',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'FirstDyelot',
    @level2type = N'COLUMN',
    @level2name = N'deleteColumn'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'頭缸卡工廠群組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FirstDyelot', @level2type = N'COLUMN', @level2name = N'TestDocFactoryGroup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'WK No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FirstDyelot', @level2type = N'COLUMN', @level2name = N'SuppID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'季節ID
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FirstDyelot', @level2type = N'COLUMN', @level2name = N'SeasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠接收頭缸卡的備註
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FirstDyelot', @level2type = N'COLUMN', @level2name = N'ReceivedRemark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'台北接收頭缸卡的日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FirstDyelot', @level2type = N'COLUMN', @level2name = N'ReceivedDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠接收頭缸卡的日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FirstDyelot', @level2type = N'COLUMN', @level2name = N'FTYReceivedReport';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'頭缸卡日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FirstDyelot', @level2type = N'COLUMN', @level2name = N'FirstDyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人名
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FirstDyelot', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FirstDyelot', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'測試報告文件名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FirstDyelot', @level2type = N'COLUMN', @level2name = N'DocumentName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色ID
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FirstDyelot', @level2type = N'COLUMN', @level2name = N'ColorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌料號
', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FirstDyelot', @level2type = N'COLUMN', @level2name = N'BrandRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FirstDyelot', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AWBno', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FirstDyelot', @level2type = N'COLUMN', @level2name = N'AWBno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新建日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FirstDyelot', @level2type = N'COLUMN', @level2name = N'AddDate';

