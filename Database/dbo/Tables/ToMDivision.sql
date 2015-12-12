CREATE TABLE [dbo].[ToMDivision] (
    [ID]              VARCHAR (8)   CONSTRAINT [DF_ToMDivision_ID] DEFAULT ('') NOT NULL,
    [Name]            VARCHAR (10)  CONSTRAINT [DF_ToMDivision_Name] DEFAULT ('') NULL,
    [CountryID]       VARCHAR (2)   CONSTRAINT [DF_ToMDivision_CountryID] DEFAULT ('') NULL,
    [Manager]         VARCHAR (10)  CONSTRAINT [DF_ToMDivision_Manager] DEFAULT ('') NULL,
    [AddName]         VARCHAR (10)  CONSTRAINT [DF_ToMDivision_AddName] DEFAULT ('') NULL,
    [AddDate]         DATETIME      NULL,
    [EditName]        VARCHAR (10)  CONSTRAINT [DF_ToMDivision_EditName] DEFAULT ('') NULL,
    [EditDate]        DATETIME      NULL,
    [APSLoginId]      VARCHAR (15)  CONSTRAINT [DF_ToMDivision_APSLoginId] DEFAULT ('') NULL,
    [APSLoginPwd]     VARCHAR (15)  CONSTRAINT [DF_ToMDivision_APSLoginPwd] DEFAULT ('') NULL,
    [SQLServerName]   VARCHAR (130) CONSTRAINT [DF_ToMDivision_SQLServerName] DEFAULT ('') NULL,
    [APSDatabaseName] VARCHAR (15)  CONSTRAINT [DF_ToMDivision_APSDatabaseName] DEFAULT ('') NULL,
    CONSTRAINT [PK_ToMDivision] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'APS Database Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ToMDivision', @level2type = N'COLUMN', @level2name = N'APSDatabaseName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SQL Server Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ToMDivision', @level2type = N'COLUMN', @level2name = N'SQLServerName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'APS SQL Server PassWord', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ToMDivision', @level2type = N'COLUMN', @level2name = N'APSLoginPwd';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'APS SQL Server Login Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ToMDivision', @level2type = N'COLUMN', @level2name = N'APSLoginId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'½s¿è®É¶¡', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ToMDivision', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'½s¿èªÌ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ToMDivision', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'·s¼W®É¶¡', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ToMDivision', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'·s¼WªÌ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ToMDivision', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ºÞ²zªÌ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ToMDivision', @level2type = N'COLUMN', @level2name = N'Manager';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'°ê§O', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ToMDivision', @level2type = N'COLUMN', @level2name = N'CountryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'¥þ¦W', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ToMDivision', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'¥N¸¹', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ToMDivision', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Manufacturing Division', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ToMDivision';

