CREATE TABLE [dbo].[ClogGarmentDispose] (
    [ID]           VARCHAR (13)   NOT NULL,
    [DisposeDate]  DATE           NULL,
    [MDivisionID]  VARCHAR (8)    CONSTRAINT [DF_ClogGarmentDispose_MDivisionID] DEFAULT ('') NOT NULL,
    [Status]       VARCHAR (15)   CONSTRAINT [DF_ClogGarmentDispose_Status] DEFAULT ('') NOT NULL,
    [Remark]       NVARCHAR (100) CONSTRAINT [DF_ClogGarmentDispose_Remark] DEFAULT ('') NOT NULL,
    [AddName]      VARCHAR (10)   CONSTRAINT [DF_ClogGarmentDispose_AddName] DEFAULT ('') NOT NULL,
    [AddDate]      DATETIME       NULL,
    [EditName]     VARCHAR (10)   CONSTRAINT [DF_ClogGarmentDispose_EditName] DEFAULT ('') NOT NULL,
    [EditDate]     DATETIME       NULL,
    [ClogReasonID] VARCHAR (5)    CONSTRAINT [DF_ClogGarmentDispose_ClogReasonID] DEFAULT ('') NULL,
    CONSTRAINT [PK_ClogGarmentDispose] PRIMARY KEY CLUSTERED ([ID] ASC)
);


