CREATE TABLE [dbo].[MDivision] (
    [ID]              VARCHAR (8)   CONSTRAINT [DF_MDivision_ID] DEFAULT ('') NOT NULL,
    [Name]            VARCHAR (10)  CONSTRAINT [DF_MDivision_Name] DEFAULT ('') NULL,
    [CountryID]       VARCHAR (2)   CONSTRAINT [DF_MDivision_CountryID] DEFAULT ('') NULL,
    [Manager]         VARCHAR (10)  CONSTRAINT [DF_MDivision_Manager] DEFAULT ('') NULL,
    [AddName]         VARCHAR (10)  CONSTRAINT [DF_MDivision_AddName] DEFAULT ('') NULL,
    [AddDate]         DATETIME      NULL,
    [EditName]        VARCHAR (10)  CONSTRAINT [DF_MDivision_EditName] DEFAULT ('') NULL,
    [EditDate]        DATETIME      NULL,
    [APSLoginId]      VARCHAR (15)  CONSTRAINT [DF_MDivision_APSLoginId] DEFAULT ('') NULL,
    [APSLoginPwd]     VARCHAR (15)  CONSTRAINT [DF_MDivision_APSLoginPwd] DEFAULT ('') NULL,
    [SQLServerName]   VARCHAR (130) CONSTRAINT [DF_MDivision_SQLServerName] DEFAULT ('') NULL,
    [APSDatabaseName] VARCHAR (15)  CONSTRAINT [DF_MDivision_APSDatabaseName] DEFAULT ('') NULL,
    [KeyWord]         VARCHAR (3)   CONSTRAINT [DF_MDivision_KeyWord] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_MDivision] PRIMARY KEY CLUSTERED ([ID] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDivision', @level2type = N'COLUMN', @level2name = N'EditDate';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDivision', @level2type = N'COLUMN', @level2name = N'EditName';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDivision', @level2type = N'COLUMN', @level2name = N'AddDate';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDivision', @level2type = N'COLUMN', @level2name = N'AddName';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'管理者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDivision', @level2type = N'COLUMN', @level2name = N'Manager';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDivision', @level2type = N'COLUMN', @level2name = N'CountryID';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'全名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDivision', @level2type = N'COLUMN', @level2name = N'Name';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDivision', @level2type = N'COLUMN', @level2name = N'ID';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Manufacturing Division', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDivision';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SQL Server Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDivision', @level2type = N'COLUMN', @level2name = N'SQLServerName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'APS SQL Server PassWord', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDivision', @level2type = N'COLUMN', @level2name = N'APSLoginPwd';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'APS SQL Server Login Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDivision', @level2type = N'COLUMN', @level2name = N'APSLoginId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'APS Database Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDivision', @level2type = N'COLUMN', @level2name = N'APSDatabaseName';

