CREATE TABLE [dbo].[Employee] (
    [MDivisionID]     VARCHAR (8)   CONSTRAINT [DF_Employee_MDivisionID] DEFAULT ('') NULL,
    [FactoryID]       VARCHAR (8)   CONSTRAINT [DF_Employee_FactoryID] DEFAULT ('') NOT NULL,
    [ID]              VARCHAR (10)  CONSTRAINT [DF_Employee_ID] DEFAULT ('') NOT NULL,
    [Name]            NVARCHAR (30) CONSTRAINT [DF_Employee_Name] DEFAULT ('') NOT NULL,
    [Skill]           NVARCHAR (20) CONSTRAINT [DF_Employee_Skill] DEFAULT ('') NULL,
    [OnBoardDate]     DATE          NULL,
    [ResignationDate] DATE          NULL,
    [SewingLineID]    VARCHAR (5)   CONSTRAINT [DF_Employee_SewingLineID] DEFAULT ('') NULL,
    [AddName]         VARCHAR (10)  CONSTRAINT [DF_Employee_AddName] DEFAULT ('') NULL,
    [AddDate]         DATETIME      NULL,
    [EditName]        VARCHAR (10)  CONSTRAINT [DF_Employee_EditName] DEFAULT ('') NULL,
    [EditDate]        DATETIME      NULL,
    CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED ([FactoryID] ASC, [ID] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'員工基本資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Employee';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Employee', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'員工編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Employee', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'員工姓名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Employee', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'熟悉的做工', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Employee', @level2type = N'COLUMN', @level2name = N'Skill';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'到職日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Employee', @level2type = N'COLUMN', @level2name = N'OnBoardDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'離職日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Employee', @level2type = N'COLUMN', @level2name = N'ResignationDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產線代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Employee', @level2type = N'COLUMN', @level2name = N'SewingLineID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Employee', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Employee', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Employee', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Employee', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Manufacturing Division ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Employee', @level2type = N'COLUMN', @level2name = N'MDivisionID';

