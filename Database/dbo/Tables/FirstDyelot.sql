CREATE TABLE [dbo].[FirstDyelot] (   
    [SuppID]         VARCHAR (6)   NOT NULL,
    [TestDocFactoryGroup]      VARCHAR (8)  NOT NULL,
    [BrandRefno]     VARCHAR (50)   NOT NULL,
    [ColorID]        VARCHAR (6)   NOT NULL,
    [SeasonID]    VARCHAR (8)  NOT NULL,
    [Period]         INT          CONSTRAINT [DF_FirstDyelot_Period] DEFAULT ((0)) NULL,
    [FirstDyelot]    DATE          NULL,
    [EditName]       VARCHAR (10) NOT NULL DEFAULT (''),
    [EditDate]       DATETIME     NULL,
    [AWBno] VARCHAR(30) NOT NULL DEFAULT (''), 
    [AddName] VARCHAR(10) NOT NULL DEFAULT (''), 
    [AddDate] DATETIME NULL, 
    [FTYReceivedReport] DATE NULL, 
    [ReceivedDate] DATE NULL, 
    [ReceivedRemark] VARCHAR(MAX) NOT NULL DEFAULT (''), 
    [DocumentName] VARCHAR(100) NOT NULL DEFAULT (''), 
    [BrandID] VARCHAR(8) NOT NULL DEFAULT (''), 
    [deleteColumn] BIT NOT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_FirstDyelot] PRIMARY KEY ([SuppID], [SeasonID], [TestDocFactoryGroup], [BrandRefno], [ColorID], [DocumentName],[BrandID]) 
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