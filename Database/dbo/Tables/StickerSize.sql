CREATE TABLE [dbo].[StickerSize] (
        [ID] [bigint] NOT NULL IDENTITY(1,1),
        [Size] [varchar](20) NOT NULL DEFAULT(''),
        [Width] [int] NOT NULL DEFAULT(0),
        [Length] [int] NOT NULL DEFAULT(0),
        [AddName] [varchar](10) NOT NULL DEFAULT(''),
        [AddDate] [datetime] NULL,
        [EditName] [varchar](10) NOT NULL DEFAULT(''),
        [EditDate] [datetime] NULL,
    CONSTRAINT [PK_StickerSize] PRIMARY KEY CLUSTERED ( [ID] ASC)
);


GO