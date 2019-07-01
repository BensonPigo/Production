CREATE TABLE [dbo].[F_ReportList] (
    [ID]          VARCHAR (4)    NULL,
    [System]      VARCHAR (15)   NULL,
    [ReportName]  NVARCHAR (50)  NULL,
    [Description] NVARCHAR (120) NULL
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'F_ReportList', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'報告名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'F_ReportList', @level2type = N'COLUMN', @level2name = N'ReportName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'系統別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'F_ReportList', @level2type = N'COLUMN', @level2name = N'System';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'報表代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'F_ReportList', @level2type = N'COLUMN', @level2name = N'ID';

