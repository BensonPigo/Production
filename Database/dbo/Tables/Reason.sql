CREATE TABLE [dbo].[Reason] (
    [ReasonTypeID] VARCHAR (50)   CONSTRAINT [DF_Reason_ReasonTypeID] DEFAULT ('') NOT NULL,
    [ID]           VARCHAR (5)    CONSTRAINT [DF_Reason_ID] DEFAULT ('') NOT NULL,
    [Name]         NVARCHAR (500) CONSTRAINT [DF_Reason_Name] DEFAULT ('') NULL,
    [Remark]       NVARCHAR (500) CONSTRAINT [DF_Reason_Remark] DEFAULT ('') NULL,
    [No]           SMALLINT       CONSTRAINT [DF_Reason_No] DEFAULT ((0)) NULL,
    [ReasonGroup]  VARCHAR (5)    CONSTRAINT [DF_Reason_ReasonGroup] DEFAULT ('') NULL,
    [Junk]         BIT            CONSTRAINT [DF_Reason_Junk] DEFAULT ((0)) NULL,
    [Kpi]          BIT            CONSTRAINT [DF_Reason_Kpi] DEFAULT ((0)) NULL,
    [AccountID]    VARCHAR (8)    CONSTRAINT [DF_Reason_AccountNo] DEFAULT ('') NULL,
    [AddName]      VARCHAR (10)   CONSTRAINT [DF_Reason_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME       NULL,
    [EditName]     VARCHAR (10)   CONSTRAINT [DF_Reason_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME       NULL,
    [FactoryKpi]   BIT            CONSTRAINT [DF_Reason_FactoryKpi] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Reason] PRIMARY KEY CLUSTERED ([ReasonTypeID] ASC, [ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Reason代號對應表', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Reason';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'分類代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Reason', @level2type = N'COLUMN', @level2name = N'ReasonTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Reason', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Reason', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Reason', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'順序編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Reason', @level2type = N'COLUMN', @level2name = N'No';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'分組代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Reason', @level2type = N'COLUMN', @level2name = N'ReasonGroup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Reason', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否扣KPI', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Reason', @level2type = N'COLUMN', @level2name = N'Kpi';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Reason', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Reason', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Reason', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Reason', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否可更新工廠KPI日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Reason', @level2type = N'COLUMN', @level2name = N'FactoryKpi';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'會計科目', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Reason', @level2type = N'COLUMN', @level2name = N'AccountID';

