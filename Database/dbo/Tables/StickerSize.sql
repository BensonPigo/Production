CREATE TABLE [dbo].[StickerSize] (
    [ID]       BIGINT       IDENTITY (1, 1) NOT NULL,
    [Size]     VARCHAR (20) DEFAULT ('') NOT NULL,
    [Width]    INT          DEFAULT ((0)) NOT NULL,
    [Length]   INT          DEFAULT ((0)) NOT NULL,
    [AddName]  VARCHAR (10) DEFAULT ('') NOT NULL,
    [AddDate]  DATETIME     NULL,
    [EditName] VARCHAR (10) DEFAULT ('') NOT NULL,
    [EditDate] DATETIME     NULL,
    [Junk]     BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_StickerSize] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO