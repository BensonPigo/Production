CREATE TABLE [dbo].[Juki_T_SewMachine] (
    [Ukey] BIGINT IDENTITY (0, 1) NOT NULL,
    [FactoryID] VARCHAR (8) CONSTRAINT [DF_Juki_T_SewMachine_FactoryID] DEFAULT ('') NOT NULL,
    [SewingCell] VARCHAR (10) CONSTRAINT [DF_Juki_T_SewMachine_SewingCell] DEFAULT ('') NOT NULL,
    [MachineGroup] VARCHAR (4) CONSTRAINT [DF_Juki_T_SewMachine_MachineGroup] DEFAULT ('') NOT NULL,
    [MachineID] VARCHAR (20) CONSTRAINT [DF_Juki_T_SewMachine_MachineID] DEFAULT ('') NOT NULL,
    [MachineBrandID] VARCHAR (32) CONSTRAINT [DF_Juki_T_SewMachine_MachineBrandID] DEFAULT ('') NOT NULL,
    [ConnType] TINYINT CONSTRAINT [DF_Juki_T_SewMachine_ConnType] DEFAULT ((0)) NOT NULL,
    [ConnImg] TINYINT CONSTRAINT [DF_Juki_T_SewMachine_ConnImg] DEFAULT ((0)) NOT NULL,
    [DetectionType] TINYINT CONSTRAINT [DF_Juki_T_SewMachine_DetectionType] DEFAULT ((0)) NOT NULL,
    [Flag] TINYINT CONSTRAINT [DF_Juki_T_SewMachine_Flag] DEFAULT ((0)) NOT NULL,
    [AddDate] DATETIME NOT NULL,
    [TransferToJukiExchangeDBDate] DATETIME NULL,
    [TransferToJukiExchangeDBErrorMsg] NVARCHAR (2000) CONSTRAINT [DF_Juki_T_SewMachine_TransferToJukiExchangeDBErrorMsg] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Juki_T_SewMachine] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機器基本檔交易流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_SewMachine', @level2type = N'COLUMN', @level2name = N'Ukey';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_SewMachine', @level2type = N'COLUMN', @level2name = N'FactoryID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'車間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_SewMachine', @level2type = N'COLUMN', @level2name = N'SewingCell';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機台群組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_SewMachine', @level2type = N'COLUMN', @level2name = N'MachineGroup';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機台號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_SewMachine', @level2type = N'COLUMN', @level2name = N'MachineID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機台品牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_SewMachine', @level2type = N'COLUMN', @level2name = N'MachineBrandID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Juki連線類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_SewMachine', @level2type = N'COLUMN', @level2name = N'ConnType';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Juki連線圖片', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_SewMachine', @level2type = N'COLUMN', @level2name = N'ConnImg';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Juki檢測類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_SewMachine', @level2type = N'COLUMN', @level2name = N'DetectionType';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'標記', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_SewMachine', @level2type = N'COLUMN', @level2name = N'Flag';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_SewMachine', @level2type = N'COLUMN', @level2name = N'AddDate';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'將該筆數據傳送給Juki-JaNets中間庫的時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_SewMachine', @level2type = N'COLUMN', @level2name = N'TransferToJukiExchangeDBDate';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳給Juki-JaNets中間庫的失敗的錯誤訊息', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_SewMachine', @level2type = N'COLUMN', @level2name = N'TransferToJukiExchangeDBErrorMsg';

GO
