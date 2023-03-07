CREATE TABLE [dbo].[ThreadIssue_Detail] (
    [ID]               VARCHAR (13) CONSTRAINT [DF_ThreadIssue_Detail_ID] DEFAULT ('') NOT NULL,
    [Refno]            VARCHAR (36) CONSTRAINT [DF_ThreadIssue_Detail_Refno] DEFAULT ('') NOT NULL,
    [ThreadColorID]    VARCHAR (15) CONSTRAINT [DF_ThreadIssue_Detail_ThreadColorID] DEFAULT ('') NOT NULL,
    [NewCone]          NUMERIC (5)  CONSTRAINT [DF_ThreadIssue_Detail_IssueNewCone] DEFAULT ((0)) NULL,
    [UsedCone]         NUMERIC (5)  CONSTRAINT [DF_ThreadIssue_Detail_IssueUsedCone] DEFAULT ((0)) NULL,
    [ThreadLocationID] VARCHAR (10) CONSTRAINT [DF_ThreadIssue_Detail_ThreadLocationID] DEFAULT ('') NOT NULL,
    [Remark] VARCHAR(60) NULL DEFAULT (''), 
    CONSTRAINT [PK_ThreadIssue_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [Refno] ASC, [ThreadColorID] ASC, [ThreadLocationID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Thread Issue Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIssue_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'發料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIssue_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIssue_Detail', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIssue_Detail', @level2type = N'COLUMN', @level2name = N'ThreadColorID';


GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIssue_Detail', @level2type = N'COLUMN', @level2name = N'ThreadLocationID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用過Cone 數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIssue_Detail', @level2type = N'COLUMN', @level2name = N'UsedCone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'完整Cone 數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIssue_Detail', @level2type = N'COLUMN', @level2name = N'NewCone';

