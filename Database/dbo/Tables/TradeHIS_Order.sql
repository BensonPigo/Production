CREATE TABLE [dbo].[TradeHIS_Order] (
    [UKEY]         BIGINT         NOT NULL,
    [TableName]    VARCHAR (60)   CONSTRAINT [DF_TradeHIS_Order_TableName] DEFAULT ('') NOT NULL,
    [HisType]      NVARCHAR (60)  CONSTRAINT [DF_TradeHIS_Order_HisType] DEFAULT ('') NOT NULL,
    [SourceID]     VARCHAR (16)   CONSTRAINT [DF_TradeHIS_Order_SourceID] DEFAULT ('') NOT NULL,
    [ReasonTypeID] VARCHAR (50)   CONSTRAINT [DF_TradeHIS_Order_ReasonTypeID] DEFAULT ('') NOT NULL,
    [ReasonID]     VARCHAR (5)    CONSTRAINT [DF_TradeHIS_Order_ReasonID] DEFAULT ('') NOT NULL,
    [OldValue]     NVARCHAR (50)  CONSTRAINT [DF_TradeHIS_Order_OldValue] DEFAULT ('') NOT NULL,
    [NewValue]     NVARCHAR (50)  CONSTRAINT [DF_TradeHIS_Order_NewValue] DEFAULT ('') NOT NULL,
    [Remark]       NVARCHAR (MAX) CONSTRAINT [DF_TradeHIS_Order_Remark] DEFAULT ('') NOT NULL,
    [AddName]      VARCHAR (10)   CONSTRAINT [DF_TradeHIS_Order_AddName] DEFAULT ('') NOT NULL,
    [AddDate]      DATETIME       NULL,
    CONSTRAINT [PK_TradeHIS_Order] PRIMARY KEY CLUSTERED ([UKEY] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UKEY', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TradeHIS_Order', @level2type = N'COLUMN', @level2name = N'UKEY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'歷史記錄Table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TradeHIS_Order';

