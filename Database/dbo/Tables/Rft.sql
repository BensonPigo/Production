CREATE TABLE [dbo].[Rft] (
    [OrderID]      VARCHAR (13)  CONSTRAINT [DF_Rft_OrderID] DEFAULT ('') NULL,
    [CDate]        DATE          NULL,
    [SewinglineID] VARCHAR (5)   CONSTRAINT [DF_Rft_SewinglineID] DEFAULT ('') NULL,
    [FactoryID]    VARCHAR (8)   CONSTRAINT [DF_Rft_FactoryID] DEFAULT ('') NULL,
    [InspectQty]   NUMERIC (7)   CONSTRAINT [DF_Rft_InspectQty] DEFAULT ((0)) NULL,
    [RejectQty]    NUMERIC (7)   CONSTRAINT [DF_Rft_RejectQty] DEFAULT ((0)) NULL,
    [DefectQty]    NUMERIC (7)   CONSTRAINT [DF_Rft_DefectQty] DEFAULT ((0)) NULL,
    [Shift]        VARCHAR (1)   CONSTRAINT [DF_Rft_Shift] DEFAULT ('') NULL,
    [Team]         VARCHAR (5)   CONSTRAINT [DF_Rft_Team] DEFAULT ('') NULL,
    [Status]       VARCHAR (15)  CONSTRAINT [DF_Rft_Encode] DEFAULT ('') NULL,
    [Remark]       NVARCHAR (60) CONSTRAINT [DF_Rft_Remark] DEFAULT ('') NULL,
    [ID]           BIGINT        IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [AddName]      VARCHAR (10)  CONSTRAINT [DF_Rft_AddName] DEFAULT ('') NULL,
    [AddDate]      DATETIME      NULL,
    [EditName]     VARCHAR (10)  CONSTRAINT [DF_Rft_EditName] DEFAULT ('') NULL,
    [EditDate]     DATETIME      NULL,
    [MDivisionid]  VARCHAR (8)   CONSTRAINT [DF_Rft_MDivisionid] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Rft] PRIMARY KEY CLUSTERED ([ID] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Right First Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rft';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rft', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'建立日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rft', @level2type = N'COLUMN', @level2name = N'CDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產線代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rft', @level2type = N'COLUMN', @level2name = N'SewinglineID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rft', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rft', @level2type = N'COLUMN', @level2name = N'InspectQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'退回數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rft', @level2type = N'COLUMN', @level2name = N'RejectQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'汙損數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rft', @level2type = N'COLUMN', @level2name = N'DefectQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'班別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rft', @level2type = N'COLUMN', @level2name = N'Shift';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rft', @level2type = N'COLUMN', @level2name = N'Team';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'確認', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rft', @level2type = N'COLUMN', @level2name = 'Status';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rft', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rft', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rft', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rft', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rft', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rft', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
CREATE NONCLUSTERED INDEX [Cdate]
    ON [dbo].[Rft]([CDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IDX_RFT_SewP01]
    ON [dbo].[Rft]([OrderID] ASC, [CDate] DESC, [SewinglineID] ASC, [FactoryID] ASC, [MDivisionid] ASC, [Shift] ASC, [Team] ASC);

