CREATE TABLE [dbo].[VNExportPort] (
    [ID]       VARCHAR (10)  CONSTRAINT [DF_ExportPort_ID] DEFAULT ('') NOT NULL,
    [Name]     NVARCHAR (50) CONSTRAINT [DF_ExportPort_Name] DEFAULT ('') NOT NULL,
    [Junk]     BIT           CONSTRAINT [DF_ExportPort_Junk] DEFAULT ((0)) NULL,
    [AddName]  VARCHAR (10)  CONSTRAINT [DF_ExportPort_AddName] DEFAULT ('') NULL,
    [AddDate]  DATETIME      NULL,
    [EditName] VARCHAR (10)  CONSTRAINT [DF_ExportPort_EditName] DEFAULT ('') NULL,
    [EditDate] DATETIME      NULL,
    CONSTRAINT [PK_ExportPort] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNExportPort', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNExportPort', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNExportPort', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNExportPort', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Junk', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNExportPort', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Name', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNExportPort', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNExportPort', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'港口基本檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNExportPort';

