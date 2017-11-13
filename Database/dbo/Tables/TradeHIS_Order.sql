CREATE TABLE [dbo].[TradeHIS_Order] (
    [UKEY]         BIGINT         NOT NULL,
    [TableName]    VARCHAR (60)   NULL,
    [HisType]      NVARCHAR (60)  NULL,
    [SourceID]     VARCHAR (13)   NULL,
    [ReasonTypeID] VARCHAR (50)   NULL,
    [ReasonID]     VARCHAR (5)    NULL,
    [OldValue]     NVARCHAR (50)  NULL,
    [NewValue]     NVARCHAR (50)  NULL,
    [Remark]       NVARCHAR (MAX) NULL,
    [AddName]      VARCHAR (10)   NULL,
    [AddDate]      DATETIME       NULL,
    CONSTRAINT [PK_TradeHIS_Order] PRIMARY KEY CLUSTERED ([UKEY] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'UKEY', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TradeHIS_Order', @level2type = N'COLUMN', @level2name = N'UKEY';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'歷史記錄Table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TradeHIS_Order';

