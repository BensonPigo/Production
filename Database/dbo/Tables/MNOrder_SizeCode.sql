CREATE TABLE [dbo].[MNOrder_SizeCode] (
    [Id]        VARCHAR (13) CONSTRAINT [DF_MNOrder_SizeCode_Id] DEFAULT ('') NOT NULL,
    [Seq]       VARCHAR (2)  CONSTRAINT [DF_MNOrder_SizeCode_Seq] DEFAULT ('') NULL,
    [SizeGroup] VARCHAR (1)  CONSTRAINT [DF_MNOrder_SizeCode_SizeGroup] DEFAULT ('') NULL,
    [SizeCode]  VARCHAR (8)  CONSTRAINT [DF_MNOrder_SizeCode_SizeCode] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_MNOrder_SizeCode] PRIMARY KEY CLUSTERED ([Id] ASC, [SizeCode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SizeCode', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_SizeCode', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SizeGroup', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_SizeCode', @level2type = N'COLUMN', @level2name = N'SizeGroup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'順序', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_SizeCode', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_SizeCode', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SizeSpec的上方標題', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_SizeCode';

