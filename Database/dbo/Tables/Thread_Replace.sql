CREATE TABLE [dbo].[Thread_Replace] (
    [Ukey]          BIGINT       IDENTITY (1, 1) NOT NULL,
    [BrandID]       VARCHAR (8)  NOT NULL,
    [FromSCIRefno]  VARCHAR (30) NOT NULL,
    [FromSuppColor] VARCHAR (8)  NOT NULL,
    [AddName]       VARCHAR (10) NOT NULL,
    [AddDate]       DATETIME     NULL,
    [EditName]      VARCHAR (10) NULL,
    [EditDate]      DATETIME     NULL,
    CONSTRAINT [PK_Thread_Replace] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

