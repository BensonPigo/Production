CREATE TABLE [dbo].[Oven_Detail] (
    [ID]            BIGINT        CONSTRAINT [DF_Oven_Detail_ID] DEFAULT ((0)) NOT NULL,
    [OvenGroup]     VARCHAR (2)   CONSTRAINT [DF_Oven_Detail_OvenGroup] DEFAULT ('') NOT NULL,
    [SEQ1]          VARCHAR (3)   CONSTRAINT [DF_Oven_Detail_SEQ] DEFAULT ('') NOT NULL,
    [SEQ2]          VARCHAR (2)   CONSTRAINT [DF_Oven_Detail_SEQ2] DEFAULT ('') NOT NULL,
    [Roll]          VARCHAR (8)   CONSTRAINT [DF_Oven_Detail_Roll] DEFAULT ('') NOT NULL,
    [Dyelot]        VARCHAR (8)   CONSTRAINT [DF_Oven_Detail_Dyelot] DEFAULT ('') NOT NULL,
    [Result]        VARCHAR (4)   CONSTRAINT [DF_Oven_Detail_Result] DEFAULT ('') NULL,
    [changeScale]   VARCHAR (8)   CONSTRAINT [DF_Oven_Detail_changeScale] DEFAULT ('') NULL,
    [StainingScale] VARCHAR (8)   CONSTRAINT [DF_Oven_Detail_StainingScale] DEFAULT ('') NULL,
    [Remark]        NVARCHAR (60) CONSTRAINT [DF_Oven_Detail_Remark] DEFAULT ('') NULL,
    [AddName]       VARCHAR (10)  CONSTRAINT [DF_Oven_Detail_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME      NULL,
    [EditName]      VARCHAR (10)  CONSTRAINT [DF_Oven_Detail_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME      NULL,
    [ResultChange]  VARCHAR (5)   NULL,
    [ResultStain]   VARCHAR (5)   NULL,
    [SubmitDate]    DATE          NULL,
    [Temperature]   INT           CONSTRAINT [DF_Oven_Detail_Temperature] DEFAULT ((0)) NOT NULL,
    [Time]          INT           CONSTRAINT [DF_Oven_Detail_Time] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Oven_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [OvenGroup] ASC, [SEQ1] ASC, [SEQ2] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fabric Oven Test Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'分組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven_Detail', @level2type = N'COLUMN', @level2name = N'OvenGroup';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven_Detail', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven_Detail', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven_Detail', @level2type = N'COLUMN', @level2name = N'Result';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色差灰階', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven_Detail', @level2type = N'COLUMN', @level2name = N'changeScale';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'染色灰階', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven_Detail', @level2type = N'COLUMN', @level2name = N'StainingScale';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven_Detail', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven_Detail', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven_Detail', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven_Detail', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven_Detail', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven_Detail', @level2type = N'COLUMN', @level2name = N'SEQ1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'烘箱時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven_Detail', @level2type = N'COLUMN', @level2name = N'Time';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'烘箱溫度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Oven_Detail', @level2type = N'COLUMN', @level2name = N'Temperature';

