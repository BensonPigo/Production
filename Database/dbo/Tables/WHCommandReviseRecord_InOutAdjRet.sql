CREATE TABLE [dbo].[WHCommandReviseRecord_InOutAdjRet] (
    [Ukey]         BIGINT          IDENTITY (1, 1) NOT NULL,
    [FunctionName] VARCHAR (80)    NOT NULL,
    [SendToWMS]    BIT             NOT NULL,
    [ID]           VARCHAR (13)    NOT NULL,
    [POID]         VARCHAR (13)    NOT NULL,
    [SEQ1]         VARCHAR (3)     NOT NULL,
    [SEQ2]         VARCHAR (2)     NOT NULL,
    [Roll]         VARCHAR (8)     NOT NULL,
    [Dyelot]       VARCHAR (8)     NOT NULL,
    [StockType]    VARCHAR (1)     NOT NULL,
    [Type]         VARCHAR (10)    NULL,
    [OriginalQty]  NUMERIC (11, 2) NOT NULL,
    [NewQty]       NUMERIC (11, 2) NULL,
    [Reason]       VARCHAR (50)    NULL,
    [Remark]       NVARCHAR (500)  NULL,
    [AddName]      VARCHAR (10)    NULL,
    [AddDate]      DATETIME        NULL,
    CONSTRAINT [PK_WHCommandReviseRecord_InOutAdjRet] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHCommandReviseRecord_InOutAdjRet', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHCommandReviseRecord_InOutAdjRet', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHCommandReviseRecord_InOutAdjRet', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHCommandReviseRecord_InOutAdjRet', @level2type = N'COLUMN', @level2name = N'Reason';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整後的數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHCommandReviseRecord_InOutAdjRet', @level2type = N'COLUMN', @level2name = N'NewQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整前的數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHCommandReviseRecord_InOutAdjRet', @level2type = N'COLUMN', @level2name = N'OriginalQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新的類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHCommandReviseRecord_InOutAdjRet', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整的單據編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHCommandReviseRecord_InOutAdjRet', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'此資料是否有發送給WMS', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHCommandReviseRecord_InOutAdjRet', @level2type = N'COLUMN', @level2name = N'SendToWMS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'對應的功能名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHCommandReviseRecord_InOutAdjRet', @level2type = N'COLUMN', @level2name = N'FunctionName';

