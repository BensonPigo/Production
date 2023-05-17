CREATE TABLE [dbo].[BundleInOut] (
    [BundleNo]          VARCHAR (10) NOT NULL,
    [SubProcessID]      VARCHAR (10) NOT NULL,
    [PendingInComing]   DATETIME     NULL,
    [PendingInOutGoing] DATETIME     NULL,
    [FinishedInComing]  DATETIME     NULL,
    [FinishedOutGoing]  DATETIME     NULL,
    [CmdTime]           DATETIME     NOT NULL,
    [SCIUpdated]        BIT          NOT NULL,
    CONSTRAINT [PK_BundleInOut] PRIMARY KEY CLUSTERED ([BundleNo] ASC, [SubProcessID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'飛雁是否已轉制', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut', @level2type = N'COLUMN', @level2name = N'SCIUpdated';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國自傳送此筆資料的時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut', @level2type = N'COLUMN', @level2name = N'CmdTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'離開完成加工區的時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut', @level2type = N'COLUMN', @level2name = N'FinishedOutGoing';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'進入完成加工區的時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut', @level2type = N'COLUMN', @level2name = N'FinishedInComing';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'離開等待加工區的時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut', @level2type = N'COLUMN', @level2name = N'PendingInOutGoing';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'進入等待加工區的時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut', @level2type = N'COLUMN', @level2name = N'PendingInComing';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工段', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut', @level2type = N'COLUMN', @level2name = N'SubProcessID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'綁包號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut', @level2type = N'COLUMN', @level2name = N'BundleNo';

