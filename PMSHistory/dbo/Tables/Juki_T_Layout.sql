CREATE TABLE [dbo].[Juki_T_Layout] (
    [Ukey] BIGINT IDENTITY (0, 1) NOT NULL,
    [LMID] BIGINT CONSTRAINT [DF_Juki_T_Layout_LMID] DEFAULT ((0)) NOT NULL,
    [FactoryID] VARCHAR (8) CONSTRAINT [DF_Juki_T_Layout_FactoryID] DEFAULT ('') NOT NULL,
    [SewingLineID] VARCHAR (5) CONSTRAINT [DF_Juki_T_Layout_SewingLineID] DEFAULT ('') NOT NULL,
    [SampleGroup] VARCHAR (2) CONSTRAINT [DF_Juki_T_Layout_SampleGroup] DEFAULT ('') NOT NULL,
    [StyleID] VARCHAR (15) CONSTRAINT [DF_Juki_T_Layout_StyleID] DEFAULT ('') NOT NULL,
    [SeasonID] VARCHAR (10) CONSTRAINT [DF_Juki_T_Layout_SeasonID] DEFAULT ('') NOT NULL,
    [ComboType] VARCHAR (1) CONSTRAINT [DF_Juki_T_Layout_ComboType] DEFAULT ('') NOT NULL,
    [MachineSeq] TINYINT NULL,
    [MachineID] VARCHAR (16) CONSTRAINT [DF_Juki_T_Layout_MachineID] DEFAULT ('') NOT NULL,
    [ProcessSeq] TINYINT CONSTRAINT [DF_Juki_T_Layout_ProcessSeq] DEFAULT ((0)) NOT NULL,
    [ProcessCode] VARCHAR (500) CONSTRAINT [DF_Juki_T_Layout_ProcessCode] DEFAULT ('') NOT NULL,
    [SewerDiffPercentage] NUMERIC (3, 2) CONSTRAINT [DF_Juki_T_Layout_SewerDiffPercentage] DEFAULT ((0)) NOT NULL,
    [Flag] INT NULL,
    [TableName] VARCHAR (30) CONSTRAINT [DF_Juki_T_Layout_TableName] DEFAULT ('') NOT NULL,
    [AddDate] DATETIME NOT NULL,
    [TransferToJukiExchangeDBDate] DATETIME NULL,
    [TransferToJukiExchangeDBErrorMsg] NVARCHAR (2000) CONSTRAINT [DF_Juki_T_Layout_TransferToJukiExchangeDBErrorMsg] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Juki_T_Layout] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'LM交易流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Layout', @level2type = N'COLUMN', @level2name = N'Ukey';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'LineMapping主鍵', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Layout', @level2type = N'COLUMN', @level2name = N'LMID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Layout', @level2type = N'COLUMN', @level2name = N'FactoryID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產線號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Layout', @level2type = N'COLUMN', @level2name = N'SewingLineID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'品牌簡碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Layout', @level2type = N'COLUMN', @level2name = N'SampleGroup';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Layout', @level2type = N'COLUMN', @level2name = N'StyleID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款號季別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Layout', @level2type = N'COLUMN', @level2name = N'SeasonID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'類別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Layout', @level2type = N'COLUMN', @level2name = N'ComboType';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機器序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Layout', @level2type = N'COLUMN', @level2name = N'MachineSeq';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機台號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Layout', @level2type = N'COLUMN', @level2name = N'MachineID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作工編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Layout', @level2type = N'COLUMN', @level2name = N'ProcessSeq';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作工編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Layout', @level2type = N'COLUMN', @level2name = N'ProcessCode';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作工分配比例', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Layout', @level2type = N'COLUMN', @level2name = N'SewerDiffPercentage';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'標記', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Layout', @level2type = N'COLUMN', @level2name = N'Flag';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'LMID表名紀錄', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Layout', @level2type = N'COLUMN', @level2name = N'TableName';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增此筆資料的時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Layout', @level2type = N'COLUMN', @level2name = N'AddDate';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'將該筆數據傳送給Juki-JaNets中間庫的時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Layout', @level2type = N'COLUMN', @level2name = N'TransferToJukiExchangeDBDate';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳給Juki-JaNets中間庫的失敗的錯誤訊息', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Layout', @level2type = N'COLUMN', @level2name = N'TransferToJukiExchangeDBErrorMsg';

GO
