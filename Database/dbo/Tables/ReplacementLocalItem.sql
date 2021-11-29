
CREATE TABLE [dbo].[ReplacementLocalItem] (
    [ID]           VARCHAR (13)  CONSTRAINT [DF_ReplacementLocalItem_ID] DEFAULT ('') NOT NULL,
    [IssueDate]    DATE          NOT NULL,
    [FabricType]   VARCHAR (1)   CONSTRAINT [DF_ReplacementLocalItem_FabricType] DEFAULT ('') NOT NULL,
    [Type]         VARCHAR (1)   CONSTRAINT [DF_ReplacementLocalItem_Type] DEFAULT ('') NOT NULL,
    [Shift]        VARCHAR (1)   CONSTRAINT [DF_ReplacementLocalItemShift] DEFAULT ('') NOT NULL,
    [MDivisionID]  VARCHAR (8)   CONSTRAINT [DF_ReplacementLocalItem_MDivisionID] DEFAULT ('') NOT NULL,
    [FactoryID]    VARCHAR (8)   CONSTRAINT [DF_ReplacementLocalItem_FactoryID] DEFAULT ('') NOT NULL,
    [OrderID]      VARCHAR (13)  CONSTRAINT [DF_ReplacementLocalItem_OrderID] DEFAULT ('') NOT NULL,
    [POID]         VARCHAR (13)  CONSTRAINT [DF_ReplacementLocalItem_POID] DEFAULT ('') NULL,
    [SewingLineID] VARCHAR (5)   CONSTRAINT [DF_ReplacementLocalItem_SewingLineID] DEFAULT ('') NOT NULL,
    [Remark]       NVARCHAR (60) CONSTRAINT [DF_ReplacementLocalItem_Remark] DEFAULT ('') NULL,
    [ApplyName]    VARCHAR (10)  CONSTRAINT [DF_ReplacementLocalItem_ApplyName] DEFAULT ('') NOT NULL,
    [ApvName]      VARCHAR (10)  CONSTRAINT [DF_ReplacementLocalItem_ApvName] DEFAULT ('') NULL,
    [ApvDate]      DATETIME      NULL,
    [AddName]      VARCHAR (10)  CONSTRAINT [DF_ReplacementLocalItem_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME      NULL,
    [EditName]     VARCHAR (10)  CONSTRAINT [DF_ReplacementLocalItem_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME      NULL,
    [Status]       VARCHAR (15)  CONSTRAINT [DF_ReplacementLocalItem_Status] DEFAULT ('') NULL,
    [SubconName]   VARCHAR (8)   DEFAULT ('') NULL,
    [Dept]         VARCHAR (15)  CONSTRAINT [DF_ReplacementLocalItem_Dept] DEFAULT ('') NULL,
    CONSTRAINT [PK_ReplacementLocalItem] PRIMARY KEY CLUSTERED ([ID] ASC)
);





GO



GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO
