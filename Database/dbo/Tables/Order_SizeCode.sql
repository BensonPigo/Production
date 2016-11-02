﻿CREATE TABLE [dbo].[Order_SizeCode] (
    [Id]        VARCHAR (13) CONSTRAINT [DF_Order_SizeCode_Id] DEFAULT ('') NOT NULL,
    [Seq]       VARCHAR (2)  CONSTRAINT [DF_Order_SizeCode_Seq] DEFAULT ('') NULL,
    [SizeGroup] VARCHAR (1)  CONSTRAINT [DF_Order_SizeCode_SizeGroup] DEFAULT ('') NULL,
    [SizeCode]  VARCHAR (8)  CONSTRAINT [DF_Order_SizeCode_SizeCode] DEFAULT ('') NOT NULL,
    [Ukey]      BIGINT       NULL
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SizeSpec的上方標題', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_SizeCode', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'順序', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_SizeCode', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SizeGroup', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_SizeCode', @level2type = N'COLUMN', @level2name = N'SizeGroup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SizeCode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Order_SizeCode', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
CREATE NONCLUSTERED INDEX [idx_sizeCode]
    ON [dbo].[Order_SizeCode]([Id] ASC, [SizeCode] ASC, [Ukey] ASC);

