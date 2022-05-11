CREATE TABLE [dbo].[Basic_AccessoryDefectImage] (
    [Ukey]              BIGINT          IDENTITY (1, 1) NOT NULL,
    [AccessoryDefectID] VARCHAR (10)    CONSTRAINT [DF_Basic_AccessoryDefectImage_AccessoryDefectID] DEFAULT ('') NOT NULL,
    [Image]             VARBINARY (MAX) NULL,
    CONSTRAINT [PK_Basic_AccessoryDefectImage] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'圖片', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Basic_AccessoryDefectImage', @level2type = N'COLUMN', @level2name = N'Image';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵項目ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Basic_AccessoryDefectImage', @level2type = N'COLUMN', @level2name = N'AccessoryDefectID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Basic_AccessoryDefectImage', @level2type = N'COLUMN', @level2name = N'Ukey';

