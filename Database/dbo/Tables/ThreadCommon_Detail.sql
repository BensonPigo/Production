CREATE TABLE [dbo].[ThreadCommon_Detail] (
    [Ukey]             BIGINT       NOT NULL,
    [ThreadCommonUkey] BIGINT       NOT NULL,
    [StartDate]        DATE         NOT NULL,
    [EndDate]          DATE         NULL,
    [AddDate]          DATETIME     NOT NULL,
    [AddName]          VARCHAR (10) NOT NULL,
    [EditDate]         DATETIME     NULL,
    [EditName]         VARCHAR (10) NULL,
    [Type]             VARCHAR (1)  CONSTRAINT [DF_ThreadCommon_Detail_Type] DEFAULT ('') NULL,
    CONSTRAINT [PK_ThreadCommon_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);



