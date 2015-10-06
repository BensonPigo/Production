CREATE TABLE [dbo].[ChgOver_Problem] (
    [ID]         BIGINT         CONSTRAINT [DF_ChgOver_Problem_ID] DEFAULT ((0)) NOT NULL,
    [IEReasonID] VARCHAR (5)    CONSTRAINT [DF_ChgOver_Problem_IEReasonID] DEFAULT ('') NOT NULL,
    [ShiftA]     NVARCHAR (MAX) CONSTRAINT [DF_ChgOver_Problem_ShiftA] DEFAULT ('') NULL,
    [ShiftB]     NVARCHAR (MAX) CONSTRAINT [DF_ChgOver_Problem_ShiftB] DEFAULT ('') NULL,
    [AddName]    VARCHAR (10)   CONSTRAINT [DF_ChgOver_Problem_AddName] DEFAULT ('') NULL,
    [AddDate]    DATETIME       NULL,
    [EditName]   VARCHAR (10)   CONSTRAINT [DF_ChgOver_Problem_EditName] DEFAULT ('') NULL,
    [EditDate]   DATETIME       NULL,
    CONSTRAINT [PK_ChgOver_Problem] PRIMARY KEY CLUSTERED ([ID] ASC, [IEReasonID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'換款工作問題集', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver_Problem';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver_Problem', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Problem Encountered', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver_Problem', @level2type = N'COLUMN', @level2name = N'IEReasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shift A', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver_Problem', @level2type = N'COLUMN', @level2name = N'ShiftA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shift B', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver_Problem', @level2type = N'COLUMN', @level2name = N'ShiftB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver_Problem', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver_Problem', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver_Problem', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChgOver_Problem', @level2type = N'COLUMN', @level2name = N'EditDate';

