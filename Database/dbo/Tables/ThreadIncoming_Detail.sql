CREATE TABLE [dbo].[ThreadIncoming_Detail] (
    [ID]               VARCHAR (13)   CONSTRAINT [DF_ThreadIncoming_Detail_ID] DEFAULT ('') NOT NULL,
    [Refno]            VARCHAR (36)   CONSTRAINT [DF_ThreadIncoming_Detail_Refno] DEFAULT ('') NOT NULL,
    [ThreadColorid]    VARCHAR (15)   CONSTRAINT [DF_ThreadIncoming_Detail_ThreadColorid] DEFAULT ('') NOT NULL,
    [NewCone]          NUMERIC (5)    CONSTRAINT [DF_ThreadIncoming_Detail_NewCone] DEFAULT ((0)) NULL,
    [TotalWeight]      NUMERIC (8, 2) CONSTRAINT [DF_ThreadIncoming_Detail_TotalWeight] DEFAULT ((0)) NULL,
    [PcsUsed]          NUMERIC (5)    CONSTRAINT [DF_ThreadIncoming_Detail_PcsUsed] DEFAULT ((0)) NULL,
    [UsedCone]         NUMERIC (5)    CONSTRAINT [DF_ThreadIncoming_Detail_UsedCone] DEFAULT ((0)) NULL,
    [ThreadLocationid] VARCHAR (10)   CONSTRAINT [DF_ThreadIncoming_Detail_ThreadLocationid] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_ThreadIncoming_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [Refno] ASC, [ThreadColorid] ASC, [ThreadLocationid] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Thread In-coming Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIncoming_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'線入庫單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIncoming_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'線料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIncoming_Detail', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'線顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIncoming_Detail', @level2type = N'COLUMN', @level2name = N'ThreadColorid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'完整Cone數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIncoming_Detail', @level2type = N'COLUMN', @level2name = N'NewCone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總重量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIncoming_Detail', @level2type = N'COLUMN', @level2name = N'TotalWeight';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'非完整Cone數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIncoming_Detail', @level2type = N'COLUMN', @level2name = N'PcsUsed';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'換算完整Cone數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIncoming_Detail', @level2type = N'COLUMN', @level2name = N'UsedCone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadIncoming_Detail', @level2type = N'COLUMN', @level2name = N'ThreadLocationid';

