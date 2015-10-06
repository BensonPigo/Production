CREATE TABLE [dbo].[AirPP_History] (
    [ID]       VARCHAR (13)   CONSTRAINT [DF_AirPP_History_ID] DEFAULT ('') NOT NULL,
    [HisType]  VARCHAR (50)   CONSTRAINT [DF_AirPP_History_ColumnName] DEFAULT ('') NOT NULL,
    [OldValue] VARCHAR (20)   CONSTRAINT [DF_AirPP_History_OldValue] DEFAULT ('') NULL,
    [NewValue] VARCHAR (20)   CONSTRAINT [DF_AirPP_History_NewValue] DEFAULT ('') NULL,
    [ReasonID] VARCHAR (5)    CONSTRAINT [DF_AirPP_History_ReasonID] DEFAULT ('') NOT NULL,
    [Remark]   NVARCHAR (MAX) CONSTRAINT [DF_AirPP_History_Remark] DEFAULT ('') NULL,
    [AddName]  VARCHAR (10)   CONSTRAINT [DF_AirPP_History_EditName] DEFAULT ('') NOT NULL,
    [AddDate]  DATETIME       NOT NULL,
    [Ukey]     BIGINT         IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_AirPP_History] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Air Pre-Paid History', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP_History';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AirPP單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP_History', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'欄位名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP_History', @level2type = N'COLUMN', @level2name = N'HisType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'舊值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP_History', @level2type = N'COLUMN', @level2name = N'OldValue';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP_History', @level2type = N'COLUMN', @level2name = N'NewValue';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原因代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP_History', @level2type = N'COLUMN', @level2name = N'ReasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP_History', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改人', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP_History', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP_History', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'AirPP_History', @level2type = N'COLUMN', @level2name = N'Ukey';

