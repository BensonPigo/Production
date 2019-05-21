CREATE TABLE [dbo].[CriticalActivity] (
    [OrderID]        VARCHAR (13) NOT NULL,
    [DropDownListID] VARCHAR (50) NOT NULL,
    [TargetDate]     DATE         NULL,
    [EditName]       VARCHAR (10) NULL,
    [EditDate]       DATETIME     NULL,
    CONSTRAINT [PK_CriticalActivity] PRIMARY KEY CLUSTERED ([OrderID] ASC, [DropDownListID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CriticalActivity', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CriticalActivity', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目標日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CriticalActivity', @level2type = N'COLUMN', @level2name = N'TargetDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目標欄位ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CriticalActivity', @level2type = N'COLUMN', @level2name = N'DropDownListID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CriticalActivity', @level2type = N'COLUMN', @level2name = N'OrderID';

