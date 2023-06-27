CREATE TABLE [dbo].[IEReasonLBRnotHit_1st] (
    [Type]      VARCHAR (50)   NOT NULL,
    [TypeGroup] VARCHAR (50)   NOT NULL,
    [Code]      VARCHAR (20)   NOT NULL,
    [Ukey]      BIGINT         NOT NULL,
    [Name]      NVARCHAR (200) CONSTRAINT [DF_IEReasonLBRnotHit_1st_Name] DEFAULT ('') NOT NULL,
    [Remark]    NVARCHAR (200) CONSTRAINT [DF_IEReasonLBRnotHit_1st_Remark] DEFAULT ('') NOT NULL,
    [Junk]      BIT            NOT NULL,
    [AddDate]   DATETIME       NULL,
    [AddName]   VARCHAR (10)   CONSTRAINT [DF_IEReasonLBRnotHit_1st_AddName] DEFAULT ('') NOT NULL,
    [EditDate]  DATETIME       NULL,
    [EditName]  VARCHAR (10)   CONSTRAINT [DF_IEReasonLBRnotHit_1st_EditName] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_IEReasonLBRnotHit_1st] PRIMARY KEY CLUSTERED ([Type] ASC, [TypeGroup] ASC, [Code] ASC)
);


GO