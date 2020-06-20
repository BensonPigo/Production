CREATE TABLE [dbo].[Lack] (
    [ID]           VARCHAR (13)  CONSTRAINT [DF_Lack_ID] DEFAULT ('') NOT NULL,
    [IssueDate]    DATE          NOT NULL,
    [FabricType]   VARCHAR (1)   CONSTRAINT [DF_Lack_FabricType] DEFAULT ('') NOT NULL,
    [Type]         VARCHAR (1)   CONSTRAINT [DF_Lack_Type] DEFAULT ('') NOT NULL,
    [Shift]        VARCHAR (1)   CONSTRAINT [DF_Lack_Shift] DEFAULT ('') NOT NULL,
    [MDivisionID]  VARCHAR (8)   CONSTRAINT [DF_Lack_MDivisionID] DEFAULT ('') NOT NULL,
    [FactoryID]    VARCHAR (8)   CONSTRAINT [DF_Lack_FactoryID] DEFAULT ('') NOT NULL,
    [OrderID]      VARCHAR (13)  CONSTRAINT [DF_Lack_OrderID] DEFAULT ('') NOT NULL,
    [POID]         VARCHAR (13)  CONSTRAINT [DF_Lack_POID] DEFAULT ('') NULL,
    [SewingLineID] VARCHAR (2)   CONSTRAINT [DF_Lack_SewingLineID] DEFAULT ('') NOT NULL,
    [Remark]       NVARCHAR (60) CONSTRAINT [DF_Lack_Remark] DEFAULT ('') NULL,
    [ApplyName]    VARCHAR (10)  CONSTRAINT [DF_Lack_ApplyName] DEFAULT ('') NOT NULL,
    [ApvName]      VARCHAR (10)  CONSTRAINT [DF_Lack_ApvName] DEFAULT ('') NULL,
    [ApvDate]      DATETIME      NULL,
    [IssueLackId]  VARCHAR (13)  CONSTRAINT [DF_Lack_IssueLackId] DEFAULT ('') NULL,
    [AddName]      VARCHAR (10)  CONSTRAINT [DF_Lack_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME      NULL,
    [EditName]     VARCHAR (10)  CONSTRAINT [DF_Lack_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME      NULL,
    [IssueLackDT]  DATETIME      NULL,
    [Status]       VARCHAR (15)  CONSTRAINT [DF_Lack_Status] DEFAULT ('') NULL,
    [SubconName] VARCHAR(8) NULL DEFAULT (''), 
    [Dept] VARCHAR(15) NULL CONSTRAINT [DF_Lack_Dept] DEFAULT (''), 
    CONSTRAINT [PK_Lack] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Lacking Replacement', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'申請日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack', @level2type = N'COLUMN', @level2name = N'IssueDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack', @level2type = N'COLUMN', @level2name = N'FabricType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'補料類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'班別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack', @level2type = N'COLUMN', @level2name = N'Shift';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack', @level2type = N'COLUMN', @level2name = N'POID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'申請產線', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack', @level2type = N'COLUMN', @level2name = N'SewingLineID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'申請人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack', @level2type = N'COLUMN', @level2name = N'ApplyName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'審核人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack', @level2type = N'COLUMN', @level2name = N'ApvName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'審核日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack', @level2type = N'COLUMN', @level2name = N'ApvDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發放單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack', @level2type = N'COLUMN', @level2name = N'IssueLackId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發放時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack', @level2type = N'COLUMN', @level2name = N'IssueLackDT';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'狀態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack', @level2type = N'COLUMN', @level2name = N'Status';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部門', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack', @level2type = N'COLUMN', @level2name = N'Dept';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Manufacturing Division ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Lack', @level2type = N'COLUMN', @level2name = N'MDivisionID';

