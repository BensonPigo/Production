CREATE TABLE [dbo].[FIR_Laboratory_Crocking] (
    [ID]        BIGINT         CONSTRAINT [DF_FIR_Laboratory_Crocking_ID] DEFAULT ((0)) NOT NULL,
    [Roll]      VARCHAR (8)    CONSTRAINT [DF_FIR_Laboratory_Crocking_Roll] DEFAULT ('') NOT NULL,
    [Dyelot]    VARCHAR (8)    CONSTRAINT [DF_FIR_Laboratory_Crocking_Dyelot] DEFAULT ('') NOT NULL,
    [DryScale]  VARCHAR (5)    CONSTRAINT [DF_FIR_Laboratory_Crocking_DryScale] DEFAULT ('') NULL,
    [WetScale]  VARCHAR (5)    CONSTRAINT [DF_FIR_Laboratory_Crocking_WetScale] DEFAULT ('') NULL,
    [Inspdate]  DATE           NULL,
    [Inspector] VARCHAR (10)   CONSTRAINT [DF_FIR_Laboratory_Crocking_Inspector] DEFAULT ('') NULL,
    [Result]    VARCHAR (5)    CONSTRAINT [DF_FIR_Laboratory_Crocking_Result] DEFAULT ('') NULL,
    [Remark]    NVARCHAR (100) CONSTRAINT [DF_FIR_Laboratory_Crocking_Remark] DEFAULT ('') NULL,
    [AddName]   VARCHAR (10)   CONSTRAINT [DF_FIR_Laboratory_Crocking_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME       NULL,
    [EditName]  VARCHAR (10)   CONSTRAINT [DF_FIR_Laboratory_Crocking_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME       NULL,
    [ResultDry] VARCHAR (5)    NULL,
    [ResultWet] VARCHAR (5)    NULL,
    CONSTRAINT [PK_FIR_Laboratory_Crocking] PRIMARY KEY CLUSTERED ([ID] ASC, [Roll] ASC, [Dyelot] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Crocking', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Crocking', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Crocking', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Crocking', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Crocking', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Laboratory - Shade bone Inspection', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Crocking';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Crocking', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Crocking', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Crocking', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'乾灰階分數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Crocking', @level2type = N'COLUMN', @level2name = N'DryScale';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'濕灰階分數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Crocking', @level2type = N'COLUMN', @level2name = N'WetScale';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Crocking', @level2type = N'COLUMN', @level2name = N'Inspdate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Crocking', @level2type = N'COLUMN', @level2name = N'Inspector';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Laboratory_Crocking', @level2type = N'COLUMN', @level2name = N'Result';

