CREATE TABLE [dbo].[Juki_T_Mechanic] (
    [Ukey] BIGINT IDENTITY (0, 1) NOT NULL,
    [EmployeeID] VARCHAR (8) NOT NULL,
    [EmployeeName] NVARCHAR (50) CONSTRAINT [DF_Juki_T_Mechanic_EmployeeName] DEFAULT ('') NOT NULL,
    [RFIDNo] VARCHAR (10) CONSTRAINT [DF_Juki_T_Mechanic_RFIDNo] DEFAULT ('') NOT NULL,
    [Flag] TINYINT CONSTRAINT [DF_Juki_T_Mechanic_Flag] DEFAULT ((0)) NOT NULL,
    [AddDate] DATETIME NOT NULL,
    [TransferToJukiExchangeDBDate] DATETIME NULL,
    [TransferToJukiExchangeDBErrorMsg] NVARCHAR (2000) CONSTRAINT [DF_Juki_T_Mechanic_TransferToJukiExchangeDBErrorMsg] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Juki_T_Mechanic] PRIMARY KEY CLUSTERED ([Ukey] ASC, [EmployeeID] ASC)
);

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機修基本檔交易流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Mechanic', @level2type = N'COLUMN', @level2name = N'Ukey';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機修編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Mechanic', @level2type = N'COLUMN', @level2name = N'EmployeeID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機修名字', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Mechanic', @level2type = N'COLUMN', @level2name = N'EmployeeName';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機修識別卡號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Mechanic', @level2type = N'COLUMN', @level2name = N'RFIDNo';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'標記', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Mechanic', @level2type = N'COLUMN', @level2name = N'Flag';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Mechanic', @level2type = N'COLUMN', @level2name = N'AddDate';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'將該筆數據傳送給Juki-JaNets中間庫的時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Mechanic', @level2type = N'COLUMN', @level2name = N'TransferToJukiExchangeDBDate';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳給Juki-JaNets中間庫的失敗的錯誤訊息', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Mechanic', @level2type = N'COLUMN', @level2name = N'TransferToJukiExchangeDBErrorMsg';

GO
