CREATE TABLE [dbo].[FIR_Continuity] (
    [ID]        BIGINT         CONSTRAINT [DF_FIR_Continuity_ID] DEFAULT ((0)) NOT NULL,
    [Roll]      VARCHAR (8)    CONSTRAINT [DF_FIR_Continuity_Roll] DEFAULT ('') NOT NULL,
    [Dyelot]    VARCHAR (8)    CONSTRAINT [DF_FIR_Continuity_Dyelot] DEFAULT ('') NOT NULL,
    [Scale]     VARCHAR (5)    CONSTRAINT [DF_FIR_Continuity_Scale] DEFAULT ('') NULL,
    [Inspdate]  DATE           NULL,
    [Inspector] VARCHAR (10)   CONSTRAINT [DF_FIR_Continuity_Inspector] DEFAULT ('') NULL,
    [Result]    VARCHAR (5)    CONSTRAINT [DF_FIR_Continuity_Result] DEFAULT ('') NULL,
    [Remark]    NVARCHAR (100) CONSTRAINT [DF_FIR_Continuity_Remark] DEFAULT ('') NULL,
    [AddName]   VARCHAR (10)   CONSTRAINT [DF_FIR_Continuity_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME       NULL,
    [EditName]  VARCHAR (10)   CONSTRAINT [DF_FIR_Continuity_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME       NULL,
    [TicketYds] NUMERIC (8, 2) DEFAULT ((0)) NULL,
    CONSTRAINT [PK_FIR_Continuity] PRIMARY KEY CLUSTERED ([ID] ASC, [Roll] ASC, [Dyelot] ASC)
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fabric Inspection - Continuity Inspection', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Continuity';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Continuity', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Continuity', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Continuity', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'灰階分數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Continuity', @level2type = N'COLUMN', @level2name = N'Scale';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Continuity', @level2type = N'COLUMN', @level2name = N'Inspdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Continuity', @level2type = N'COLUMN', @level2name = N'Inspector';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Continuity', @level2type = N'COLUMN', @level2name = N'Result';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Continuity', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Continuity', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Continuity', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Continuity', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Continuity', @level2type = N'COLUMN', @level2name = N'EditDate';

