CREATE TABLE [dbo].[FinishingProcess] (
    [DM300]    TINYINT      NOT NULL,
    [DM200]    INT          CONSTRAINT [DF_FinishingProcess_DM200] DEFAULT ((0)) NULL,
    [DM201]    INT          CONSTRAINT [DF_FinishingProcess_DM201] DEFAULT ((0)) NULL,
    [DM202]    INT          CONSTRAINT [DF_FinishingProcess_DM202] DEFAULT ((0)) NULL,
    [DM205]    INT          CONSTRAINT [DF_FinishingProcess_DM205] DEFAULT ((0)) NULL,
    [DM203]    INT          CONSTRAINT [DF_FinishingProcess_DM203] DEFAULT ((0)) NULL,
    [DM204]    INT          CONSTRAINT [DF_FinishingProcess_DM204] DEFAULT ((0)) NULL,
    [DM206]    INT          CONSTRAINT [DF_FinishingProcess_DM206] DEFAULT ((0)) NULL,
    [DM207]    INT          CONSTRAINT [DF_FinishingProcess_DM207] DEFAULT ((0)) NULL,
    [DM208]    INT          CONSTRAINT [DF_FinishingProcess_DM208] DEFAULT ((0)) NULL,
    [DM209]    INT          CONSTRAINT [DF_FinishingProcess_DM209] DEFAULT ((0)) NULL,
    [DM210]    INT          CONSTRAINT [DF_FinishingProcess_DM210] DEFAULT ((0)) NULL,
    [DM212]    INT          CONSTRAINT [DF_FinishingProcess_DM212] DEFAULT ((0)) NULL,
    [DM214]    INT          CONSTRAINT [DF_FinishingProcess_DM214] DEFAULT ((0)) NULL,
    [DM215]    INT          CONSTRAINT [DF_FinishingProcess_DM215] DEFAULT ((0)) NULL,
    [DM216]    INT          CONSTRAINT [DF_FinishingProcess_DM216] DEFAULT ((0)) NULL,
    [DM219]    INT          CONSTRAINT [DF_FinishingProcess_DM219] DEFAULT ((0)) NULL,
    [AddName]  VARCHAR (10) NULL,
    [AddDate]  DATETIME     NULL,
    [EditName] VARCHAR (10) NULL,
    [EditDate] DATETIME     NULL,
    [Junk]     BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_FinishingProcess] PRIMARY KEY CLUSTERED ([DM300] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'溫度允許偏差', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FinishingProcess', @level2type = N'COLUMN', @level2name = N'DM219';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'落物檢測時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FinishingProcess', @level2type = N'COLUMN', @level2name = N'DM216';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'清掃時間間隔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FinishingProcess', @level2type = N'COLUMN', @level2name = N'DM215';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'抽濕閥時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FinishingProcess', @level2type = N'COLUMN', @level2name = N'DM214';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳動電機2速度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FinishingProcess', @level2type = N'COLUMN', @level2name = N'DM212';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳動電機1速度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FinishingProcess', @level2type = N'COLUMN', @level2name = N'DM210';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'風機四組風量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FinishingProcess', @level2type = N'COLUMN', @level2name = N'DM209';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'風機三組風量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FinishingProcess', @level2type = N'COLUMN', @level2name = N'DM208';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'風機二組風量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FinishingProcess', @level2type = N'COLUMN', @level2name = N'DM207';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'風機一組風量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FinishingProcess', @level2type = N'COLUMN', @level2name = N'DM206';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'給濕等級', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FinishingProcess', @level2type = N'COLUMN', @level2name = N'DM204';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'溫度下限', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FinishingProcess', @level2type = N'COLUMN', @level2name = N'DM203';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'箱體溫度4', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FinishingProcess', @level2type = N'COLUMN', @level2name = N'DM205';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'箱體溫度3', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FinishingProcess', @level2type = N'COLUMN', @level2name = N'DM202';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'箱體溫度2', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FinishingProcess', @level2type = N'COLUMN', @level2name = N'DM201';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'箱體溫度1', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FinishingProcess', @level2type = N'COLUMN', @level2name = N'DM200';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FinishingProcess', @level2type = N'COLUMN', @level2name = N'DM300';

