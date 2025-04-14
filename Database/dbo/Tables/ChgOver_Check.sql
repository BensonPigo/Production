CREATE TABLE [dbo].[ChgOver_Check] (
    [ID]                 BIGINT        CONSTRAINT [DF_ChgOver_Check_ID] DEFAULT ((0)) NOT NULL,
    [DayBe4Inline]       INT           CONSTRAINT [DF_ChgOver_Check_DayBe4Inline] DEFAULT ((0)) NULL,
    [BaseOn]             TINYINT       CONSTRAINT [DF_ChgOver_Check_BaseOn] DEFAULT ((0)) NULL,
    [ChgOverCheckListID] BIGINT        CONSTRAINT [DF_ChgOver_Check_ChgOverCheckListID] DEFAULT ('') NOT NULL,
    [ScheduleDate]       DATE          NULL,
    [ActualDate]         DATE          NULL,
    [Remark]             NVARCHAR (60) CONSTRAINT [DF_ChgOver_Check_Remark] DEFAULT ('') NULL,
    [Checked] BIT NOT NULL DEFAULT ((0)), 
    [LeadTime] SMALLINT NOT NULL DEFAULT ((0)), 
    [EditName] VARCHAR(10) NOT NULL DEFAULT (''), 
    [EditDate] DATETIME NULL, 
    [No] INT NOT NULL, 
    [CompletionDate] DATETIME NULL, 
    [Deadline] DATE NULL, 
    [ResponseDep] NVARCHAR(200) NULL DEFAULT (''), 
    [CompletionDate] DATETIME NULL, 
    CONSTRAINT [PK_ChgOver_Check] PRIMARY KEY CLUSTERED ([ID] ASC, [ChgOverCheckListID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'換款工作檢核表(Check List for Style Change over)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver_Check';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver_Check', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'上線日前幾天需完成', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver_Check', @level2type = N'COLUMN', @level2name = N'DayBe4Inline';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'準備日期是依照Change Over或SCI delivery計算', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver_Check', @level2type = N'COLUMN', @level2name = N'BaseOn';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'執行動作', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver_Check', @level2type = N'COLUMN', @level2name = N'ChgOverCheckListID';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際完成日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver_Check', @level2type = N'COLUMN', @level2name = N'ActualDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver_Check', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'完成該項CheckList後勾選',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChgOver_Check',
    @level2type = N'COLUMN',
    @level2name = N'Checked'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'應於InlineDate之前完成準備的天數',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChgOver_Check',
    @level2type = N'COLUMN',
    @level2name = N'LeadTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最後修改人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChgOver_Check',
    @level2type = N'COLUMN',
    @level2name = N'EditName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最後修改時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChgOver_Check',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'CheckList代碼',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChgOver_Check',
    @level2type = N'COLUMN',
    @level2name = N'No'
GO

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'勾選Check的日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChgOver_Check',
    @level2type = N'COLUMN',
    @level2name = N'CompletionDate'
GO

EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'預計完成日',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChgOver_Check',
    @level2type = N'COLUMN',
    @level2name = N'ScheduleDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最晚應勾選Check的日期',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChgOver_Check',
    @level2type = N'COLUMN',
    @level2name = N'Deadline'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'負責部門',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChgOver_Check',
    @level2type = N'COLUMN',
    @level2name = N'ResponseDep'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'完成時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'ChgOver_Check',
    @level2type = N'COLUMN',
    @level2name = N'CompletionDate'