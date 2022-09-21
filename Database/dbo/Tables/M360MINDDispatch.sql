CREATE TABLE [dbo].[M360MINDDispatch] (
    [Ukey]                BIGINT       NOT NULL,
    [RackLocationID]      VARCHAR (20) CONSTRAINT [DF_M360MINDDispatch_RackLocationID] DEFAULT ('') NOT NULL,
    [FactoryID]           VARCHAR (8)  CONSTRAINT [DF_M360MINDDispatch_FactoryID] DEFAULT ('') NOT NULL,
    [RegisterTime]        DATETIME     NULL,
    [RegisterName]        VARCHAR (10) CONSTRAINT [DF_M360MINDDispatch_RegisterName] DEFAULT ('') NOT NULL,
    [DispatchTime]        DATETIME     NULL,
    [DispatchName]        VARCHAR (10) CONSTRAINT [DF_M360MINDDispatch_DispatchName] DEFAULT ('') NOT NULL,
    [FactoryReceivedTime] DATETIME     NULL,
    [FactoryReceivedName] VARCHAR (10) CONSTRAINT [DF_M360MINDDispatch_FactoryReceivedName] DEFAULT ('') NOT NULL,
    [Junk]                BIT          CONSTRAINT [DF_M360MINDDispatch_Junk] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_M360MINDDispatch] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'標註已刪除', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'M360MINDDispatch', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠接收日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'M360MINDDispatch', @level2type = N'COLUMN', @level2name = N'FactoryReceivedTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'準備完成日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'M360MINDDispatch', @level2type = N'COLUMN', @level2name = N'DispatchTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'登記至 Dispatch 清單的日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'M360MINDDispatch', @level2type = N'COLUMN', @level2name = N'RegisterTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'領料工廠', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'M360MINDDispatch', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'M360MINDDispatch', @level2type = N'COLUMN', @level2name = N'RackLocationID';

