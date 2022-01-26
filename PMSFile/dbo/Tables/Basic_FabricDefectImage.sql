CREATE TABLE [dbo].[Basic_FabricDefectImage] (
    [Ukey]           BIGINT          IDENTITY (1, 1) NOT NULL,
    [FabricDefectID] VARCHAR (2)     CONSTRAINT [DF_Basic_FabricDefectImage_FabricDefectID] DEFAULT ((0)) NOT NULL,
    [Image]          VARBINARY (MAX) NOT NULL,
    CONSTRAINT [PK_Basic_FabricDefectImage] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'圖片', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Basic_FabricDefectImage', @level2type = N'COLUMN', @level2name = N'Image';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵項目ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Basic_FabricDefectImage', @level2type = N'COLUMN', @level2name = N'FabricDefectID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Basic_FabricDefectImage', @level2type = N'COLUMN', @level2name = N'Ukey';

