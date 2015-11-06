CREATE TABLE [dbo].[OperationDesc] (
    [ID]      VARCHAR (50)   CONSTRAINT [DF_OperationDesc_ID] DEFAULT ('') NOT NULL,
    [DescKH]  NVARCHAR (200) CONSTRAINT [DF_OperationDesc_DescKH] DEFAULT ('') NULL,
    [DescVI]  NVARCHAR (200) CONSTRAINT [DF_OperationDesc_DescVI] DEFAULT ('') NULL,
    [DescCHS] NVARCHAR (200) CONSTRAINT [DF_OperationDesc_DescCHS] DEFAULT ('') NULL,
    CONSTRAINT [PK_OperationDesc] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Operation Description翻譯檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OperationDesc';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小工段', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OperationDesc', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Description (柬文)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OperationDesc', @level2type = N'COLUMN', @level2name = N'DescKH';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Description (越文)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OperationDesc', @level2type = N'COLUMN', @level2name = N'DescVI';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Description (簡體中文)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'OperationDesc', @level2type = N'COLUMN', @level2name = N'DescCHS';

