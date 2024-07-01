CREATE TABLE [dbo].[WorkOrderForOutputDelete] (
    [Ukey]    INT          IDENTITY (1, 1) NOT NULL,
    [ID]      VARCHAR (13) CONSTRAINT [DF_WorkOrderForOutputDelete_ID] DEFAULT ('') NOT NULL,
    [GroupID] VARCHAR (13) CONSTRAINT [DF_WorkOrderForOutputDelete_GroupID] DEFAULT ('') NOT NULL,
    [CutRef]  VARCHAR (10) CONSTRAINT [DF_WorkOrderForOutputDelete_CutRef] DEFAULT ('') NOT NULL,
    [Layer]   INT          CONSTRAINT [DF_WorkOrderForOutputDelete_Layer] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_WorkOrderForOutputDelete] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'層數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutputDelete', @level2type = N'COLUMN', @level2name = N'Layer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁次', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutputDelete', @level2type = N'COLUMN', @level2name = N'CutRef';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'刪除前群組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutputDelete', @level2type = N'COLUMN', @level2name = N'GroupID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪母單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutputDelete', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'主鍵', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutputDelete', @level2type = N'COLUMN', @level2name = N'Ukey';

