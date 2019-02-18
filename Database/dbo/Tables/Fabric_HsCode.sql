CREATE TABLE [dbo].[Fabric_HsCode] (
    [SCIRefno]    VARCHAR (30)   CONSTRAINT [DF_Fabric_HsCode_SCIRefno] DEFAULT ('') NOT NULL,
    [Ukey]        BIGINT         CONSTRAINT [DF_Fabric_HsCode_Ukey] DEFAULT ((0)) NULL,
    [SuppID]      VARCHAR (8)    CONSTRAINT [DF_Fabric_HsCode_SuppID] DEFAULT ('') NOT NULL,
    [Year]        VARCHAR (4)    CONSTRAINT [DF_Fabric_HsCode_Year] DEFAULT ('') NOT NULL,
    [HsCode]      VARCHAR (20)   CONSTRAINT [DF_Fabric_HsCode_HsCode] DEFAULT ('') NOT NULL,
    [ImportDuty]  NUMERIC (6, 2) CONSTRAINT [DF_Fabric_HsCode_ImportDuty] DEFAULT ((0)) NULL,
    [ECFADuty]    NUMERIC (6, 2) CONSTRAINT [DF_Fabric_HsCode_ECFADuty] DEFAULT ((0)) NULL,
    [ASEANDuty]   NUMERIC (6, 2) CONSTRAINT [DF_Fabric_HsCode_ASEANDuty] DEFAULT ((0)) NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_Fabric_HsCode_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   CONSTRAINT [DF_Fabric_HsCode_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME       NULL,
    [OldSys_Ukey] VARCHAR (10)   CONSTRAINT [DF_Fabric_HsCode_OldSys_Ukey] DEFAULT ('') NULL,
    [OldSys_Ver]  VARCHAR (2)    CONSTRAINT [DF_Fabric_HsCode_OldSys_Ver] DEFAULT ('') NULL,
    [HSType] VARCHAR NOT NULL DEFAULT (''), 
    [HSCodeT2] VARCHAR(20) NOT NULL DEFAULT (''), 
    CONSTRAINT [PK_Fabric_HsCode] PRIMARY KEY CLUSTERED ([SCIRefno] ASC, [SuppID] ASC, [Year] ASC ,[HSType] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fabric Tax 主副料HS Code稅額記錄檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_HsCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'飛雁料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_HsCode', @level2type = N'COLUMN', @level2name = N'SCIRefno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'唯一值', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_HsCode', @level2type = N'COLUMN', @level2name = N'Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'廠商代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_HsCode', @level2type = N'COLUMN', @level2name = N'SuppID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'年份', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_HsCode', @level2type = N'COLUMN', @level2name = N'Year';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'中國海關HS編碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_HsCode', @level2type = N'COLUMN', @level2name = N'HsCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'進口關稅.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_HsCode', @level2type = N'COLUMN', @level2name = N'ImportDuty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ECFA的優惠關稅.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_HsCode', @level2type = N'COLUMN', @level2name = N'ECFADuty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'東協的優惠關稅', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_HsCode', @level2type = N'COLUMN', @level2name = N'ASEANDuty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_HsCode', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_HsCode', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_HsCode', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_HsCode', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_HsCode', @level2type = N'COLUMN', @level2name = N'OldSys_Ukey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Fabric_HsCode', @level2type = N'COLUMN', @level2name = N'OldSys_Ver';

