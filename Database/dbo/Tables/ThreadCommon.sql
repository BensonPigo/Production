CREATE TABLE [dbo].[ThreadCommon] (
    [Ukey]     BIGINT       NOT NULL,
    [BrandID]  VARCHAR (8)  NOT NULL,
    [Refno]    VARCHAR (36) NOT NULL,
    [ColorId]  VARCHAR (6)  NOT NULL,
    [AddDate]  DATETIME     NOT NULL,
    [AddName]  VARCHAR (10) NOT NULL,
    [EditDate] DATETIME     NULL,
    [EditName] VARCHAR (10) NULL,
    CONSTRAINT [PK_ThreadCommon] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);



