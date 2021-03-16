CREATE TABLE [dbo].[WHWorkingCalendar] (
    [MDivision] VARCHAR (8)  NOT NULL,
    [StartDate] DATETIME     NOT NULL,
    [BeginTime] VARCHAR (8)  NOT NULL,
    [EndTime]   VARCHAR (8)  NOT NULL,
    [AddDate]   DATETIME     NULL,
    [AddName]   VARCHAR (10) CONSTRAINT [DF_WHWorkingCalendar_AddName] DEFAULT ('') NOT NULL,
    [EditDate]  DATETIME     NULL,
    [EditName]  VARCHAR (10) CONSTRAINT [DF_WHWorkingCalendar_EditName] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_WHWorkingCalendar] PRIMARY KEY CLUSTERED ([MDivision] ASC, [StartDate] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHWorkingCalendar', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHWorkingCalendar', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHWorkingCalendar', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHWorkingCalendar', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'下班時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHWorkingCalendar', @level2type = N'COLUMN', @level2name = N'EndTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'上班時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHWorkingCalendar', @level2type = N'COLUMN', @level2name = N'BeginTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'班表起始日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHWorkingCalendar', @level2type = N'COLUMN', @level2name = N'StartDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'M', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHWorkingCalendar', @level2type = N'COLUMN', @level2name = N'MDivision';

