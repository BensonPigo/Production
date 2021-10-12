CREATE TABLE [dbo].[AIR_DefectImage] (
    [Ukey]        BIGINT          IDENTITY (1, 1) NOT NULL,
    [AIRID]       BIGINT          CONSTRAINT [DF_AIR_DefectImage_AIRID] DEFAULT ((0)) NOT NULL,
    [ReceivingID] VARCHAR (13)    CONSTRAINT [DF_AIR_DefectImage_ReceivingID] DEFAULT ('') NOT NULL,
    [Image]       VARBINARY (MAX) NULL,
    CONSTRAINT [PK_AIR_DefectImage] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'圖片', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_DefectImage', @level2type = N'COLUMN', @level2name = N'Image';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_DefectImage', @level2type = N'COLUMN', @level2name = N'ReceivingID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AIR_DefectImage', @level2type = N'COLUMN', @level2name = N'AIRID';

