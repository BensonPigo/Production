CREATE TABLE [dbo].[Thread_Replace_Detail_Detail] (
    [Ukey]                      BIGINT       IDENTITY (1, 1) NOT NULL,
    [Thread_Replace_DetailUkey] BIGINT       NOT NULL,
    [SuppID]                    VARCHAR (8)  CONSTRAINT [DF_Thread_Replace_Detail_Detail_SuppID] DEFAULT ('') NOT NULL,
    [ToSCIRefno]                VARCHAR (30) NOT NULL,
    [ToBrandColorID]            VARCHAR (8)  NOT NULL,
    [ToBrandSuppColor]          VARCHAR (8)  NOT NULL,
    [AddName]                   VARCHAR (10) NOT NULL,
    [AddDate]                   DATETIME     NULL,
    [EditName]                  VARCHAR (10) NULL,
    [EditDate]                  DATETIME     NULL,
    CONSTRAINT [PK_Thread_Replace_Detail_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

