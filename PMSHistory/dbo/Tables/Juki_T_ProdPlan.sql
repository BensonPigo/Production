CREATE TABLE [dbo].[Juki_T_ProdPlan] (
    [Ukey] BIGINT IDENTITY (0, 1) NOT NULL,
    [LMID] BIGINT CONSTRAINT [DF_Juki_T_ProdPlan_LMID] DEFAULT ((0)) NOT NULL,
    [FactoryID] VARCHAR (8) CONSTRAINT [DF_Juki_T_ProdPlan_FactoryID] DEFAULT ('') NOT NULL,
    [SewingLineID] VARCHAR (5) CONSTRAINT [DF_Juki_T_ProdPlan_SewingLineID] DEFAULT ('') NOT NULL,
    [SampleGroup] VARCHAR (2) CONSTRAINT [DF_Juki_T_ProdPlan_SampleGroup] DEFAULT ('') NOT NULL,
    [StyleID] VARCHAR (15) CONSTRAINT [DF_Juki_T_ProdPlan_StyleID] DEFAULT ('') NOT NULL,
    [SeasonID] VARCHAR (10) CONSTRAINT [DF_Juki_T_ProdPlan_SeasonID] DEFAULT ('') NOT NULL,
    [ComboType] VARCHAR (1) CONSTRAINT [DF_Juki_T_ProdPlan_ComboType] DEFAULT ('') NOT NULL,
    [Flag] INT NULL,
    [TableName] VARCHAR (30) CONSTRAINT [DF_Juki_T_ProdPlan_TableName] DEFAULT ('') NOT NULL,
    [AddDate] DATETIME NOT NULL,
    [TransferToJukiExchangeDBDate] DATETIME NULL,
    [TransferToJukiExchangeDBErrorMsg] NVARCHAR (2000) CONSTRAINT [DF_Juki_T_ProdPlan_TransferToJukiExchangeDBErrorMsg] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Juki_T_ProdPlan] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'計劃交易流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_ProdPlan', @level2type = N'COLUMN', @level2name = N'Ukey';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'LineMapping主鍵', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_ProdPlan', @level2type = N'COLUMN', @level2name = N'LMID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_ProdPlan', @level2type = N'COLUMN', @level2name = N'FactoryID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產線號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_ProdPlan', @level2type = N'COLUMN', @level2name = N'SewingLineID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌簡碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_ProdPlan', @level2type = N'COLUMN', @level2name = N'SampleGroup';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_ProdPlan', @level2type = N'COLUMN', @level2name = N'StyleID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款號季別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_ProdPlan', @level2type = N'COLUMN', @level2name = N'SeasonID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_ProdPlan', @level2type = N'COLUMN', @level2name = N'ComboType';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'標記', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_ProdPlan', @level2type = N'COLUMN', @level2name = N'Flag';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'LMID表名紀錄', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_ProdPlan', @level2type = N'COLUMN', @level2name = N'TableName';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增此筆資料的時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_ProdPlan', @level2type = N'COLUMN', @level2name = N'AddDate';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'將該筆數據傳送給Juki-JaNets中間庫的時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_ProdPlan', @level2type = N'COLUMN', @level2name = N'TransferToJukiExchangeDBDate';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳給Juki-JaNets中間庫的失敗的錯誤訊息', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_ProdPlan', @level2type = N'COLUMN', @level2name = N'TransferToJukiExchangeDBErrorMsg';

GO
