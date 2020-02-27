CREATE TABLE [dbo].[SewingOutputTransfer] (
    [ID]         VARCHAR (13)   CONSTRAINT [DF_SewingOutputTransfer_ID] DEFAULT ('') NOT NULL,
    [CreateDate] DATE           NOT NULL,
    [FactoryID]  VARCHAR (13)   CONSTRAINT [DF_SewingOutputTransfer_FactoryID] DEFAULT ('') NOT NULL,
    [Status]     VARCHAR (15)   CONSTRAINT [DF_SewingOutputTransfer_Status] DEFAULT ('') NOT NULL,
    [Remark]     NVARCHAR (250) CONSTRAINT [DF_SewingOutputTransfer_Remark] DEFAULT ('') NOT NULL,
    [AddName]    VARCHAR (10)   CONSTRAINT [DF_SewingOutputTransfer_AddName] DEFAULT ('') NOT NULL,
    [AddDate]    DATETIME       NULL,
    [EditName]   VARCHAR (10)   CONSTRAINT [DF_SewingOutputTransfer_EditName] DEFAULT ('') NOT NULL,
    [EditDate]   DATETIME       NULL,
    CONSTRAINT [PK_SewingOutputTransfer] PRIMARY KEY CLUSTERED ([ID] ASC)
);

