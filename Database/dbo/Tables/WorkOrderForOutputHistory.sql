CREATE TABLE [dbo].[WorkOrderForOutputHistory] (
    [Ukey]       INT          IDENTITY (1, 1) NOT NULL,
    [ID]         VARCHAR (13) CONSTRAINT [DF_WorkOrderForOutputHistory_ID] DEFAULT ('') NOT NULL,
    [SourceFrom] VARCHAR (1)  CONSTRAINT [DF_WorkOrderForOutputHistory_SourceFrom] DEFAULT ('') NOT NULL,
    [GroupID]    VARCHAR (13) CONSTRAINT [DF_WorkOrderForOutputHistory_GroupID] DEFAULT ('') NOT NULL,
    [CutRef]     VARCHAR (10) CONSTRAINT [DF_WorkOrderForOutputHistory_CutRef] DEFAULT ('') NOT NULL,
    [Layer]      INT          CONSTRAINT [DF_WorkOrderForOutputHistory_Layer] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_WorkOrderForOutputHistory] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'層數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutputHistory', @level2type = N'COLUMN', @level2name = N'Layer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁次', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutputHistory', @level2type = N'COLUMN', @level2name = N'CutRef';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'修改前群組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutputHistory', @level2type = N'COLUMN', @level2name = N'GroupID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'1>Cutting_P09. WorkOrder For Output
2>M360_Digital Spreading', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutputHistory', @level2type = N'COLUMN', @level2name = N'SourceFrom';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁剪母單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutputHistory', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'主鍵', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WorkOrderForOutputHistory', @level2type = N'COLUMN', @level2name = N'Ukey';

