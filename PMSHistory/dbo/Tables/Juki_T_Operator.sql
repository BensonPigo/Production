CREATE TABLE [dbo].[Juki_T_Operator] (
    [Ukey] BIGINT IDENTITY (0, 1) NOT NULL,
    [FactoryID] VARCHAR (8) CONSTRAINT [DF_Juki_T_Operator_FactoryID] DEFAULT ('') NOT NULL,
    [SewingLineID] VARCHAR (5) CONSTRAINT [DF_Juki_T_Operator_SewingLineID] DEFAULT ('') NOT NULL,
    [EmployeeID] VARCHAR (8) CONSTRAINT [DF_Juki_T_Operator_EmployeeID] DEFAULT ('') NOT NULL,
    [EmployeeName] NVARCHAR (50) CONSTRAINT [DF_Juki_T_Operator_EmployeeName] DEFAULT ('') NOT NULL,
    [RFIDNo] VARCHAR (10) CONSTRAINT [DF_Juki_T_Operator_RFIDNo] DEFAULT ('') NOT NULL,
    [Flag] TINYINT CONSTRAINT [DF_Juki_T_Operator_Flag] DEFAULT ((0)) NOT NULL,
    [AddDate] DATETIME NOT NULL,
    [TransferToJukiExchangeDBDate] DATETIME NULL,
    [TransferToJukiExchangeDBErrorMsg] NVARCHAR (2000) CONSTRAINT [DF_Juki_T_Operator_TransferToJukiExchangeDBErrorMsg] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_Juki_T_Operator] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'車工基本檔交易流水號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Operator', @level2type = N'COLUMN', @level2name = N'Ukey';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Operator', @level2type = N'COLUMN', @level2name = N'FactoryID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'產線號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Operator', @level2type = N'COLUMN', @level2name = N'SewingLineID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'車工編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Operator', @level2type = N'COLUMN', @level2name = N'EmployeeID';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'車工名字', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Operator', @level2type = N'COLUMN', @level2name = N'EmployeeName';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'車工識別卡號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Operator', @level2type = N'COLUMN', @level2name = N'RFIDNo';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'標記', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Operator', @level2type = N'COLUMN', @level2name = N'Flag';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增此筆資料的時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Operator', @level2type = N'COLUMN', @level2name = N'AddDate';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'將該筆數據傳送給Juki-JaNets中間庫的時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Operator', @level2type = N'COLUMN', @level2name = N'TransferToJukiExchangeDBDate';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'傳給Juki-JaNets中間庫的失敗的錯誤訊息', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Juki_T_Operator', @level2type = N'COLUMN', @level2name = N'TransferToJukiExchangeDBErrorMsg';

GO
