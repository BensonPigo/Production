CREATE TABLE [dbo].[ChgOver_Check] (
    [ID]                 BIGINT        CONSTRAINT [DF_ChgOver_Check_ID] DEFAULT ((0)) NOT NULL,
    [DayBe4Inline]       INT           CONSTRAINT [DF_ChgOver_Check_DayBe4Inline] DEFAULT ((0)) NULL,
    [BaseOn]             TINYINT       CONSTRAINT [DF_ChgOver_Check_BaseOn] DEFAULT ((0)) NULL,
    [ChgOverCheckListID] BIGINT        CONSTRAINT [DF_ChgOver_Check_ChgOverCheckListID] DEFAULT ('') NOT NULL,
    [ScheduleDate]       DATE          NULL,
    [ActualDate]         DATE          NULL,
    [Remark]             NVARCHAR (60) CONSTRAINT [DF_ChgOver_Check_Remark] DEFAULT ('') NULL,
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
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'預計完成日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver_Check', @level2type = N'COLUMN', @level2name = N'ScheduleDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際完成日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver_Check', @level2type = N'COLUMN', @level2name = N'ActualDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver_Check', @level2type = N'COLUMN', @level2name = N'Remark';

