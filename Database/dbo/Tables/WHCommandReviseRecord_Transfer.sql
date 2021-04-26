CREATE TABLE [dbo].[WHCommandReviseRecord_Transfer] (
    [Ukey]          BIGINT          IDENTITY (1, 1) NOT NULL,
    [FunctionName]  VARCHAR (80)    NOT NULL,
    [SendToWMS]     BIT             NOT NULL,
    [ID]            VARCHAR (13)    NOT NULL,
    [FromPOID]      VARCHAR (13)    NOT NULL,
    [FromSEQ1]      VARCHAR (3)     NOT NULL,
    [FromSEQ2]      VARCHAR (2)     NOT NULL,
    [FromRoll]      VARCHAR (8)     NOT NULL,
    [FromDyelot]    VARCHAR (8)     NOT NULL,
    [FromStockType] VARCHAR (1)     NOT NULL,
    [ToPOID]        VARCHAR (13)    NOT NULL,
    [ToSEQ1]        VARCHAR (3)     NOT NULL,
    [ToSEQ2]        VARCHAR (2)     NOT NULL,
    [ToRoll]        VARCHAR (8)     NOT NULL,
    [ToDyelot]      VARCHAR (8)     NOT NULL,
    [ToStockType]   VARCHAR (1)     NOT NULL,
    [Type]          VARCHAR (10)    NULL,
    [OriginalQty]   NUMERIC (11, 2) NOT NULL,
    [NewQty]        NUMERIC (11, 2) NULL,
    [Reason]        VARCHAR (50)    NULL,
    [Remark]        NVARCHAR (500)  NULL,
    [AddName]       VARCHAR (10)    NULL,
    [AddDate]       DATETIME        NULL,
    CONSTRAINT [PK_WHCommandReviseRecord_Transfer] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHCommandReviseRecord_Transfer', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHCommandReviseRecord_Transfer', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHCommandReviseRecord_Transfer', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHCommandReviseRecord_Transfer', @level2type = N'COLUMN', @level2name = N'Reason';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整後的數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHCommandReviseRecord_Transfer', @level2type = N'COLUMN', @level2name = N'NewQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整前的數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHCommandReviseRecord_Transfer', @level2type = N'COLUMN', @level2name = N'OriginalQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新的類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHCommandReviseRecord_Transfer', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'調整的單據編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHCommandReviseRecord_Transfer', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'此資料是否有發送給WMS', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHCommandReviseRecord_Transfer', @level2type = N'COLUMN', @level2name = N'SendToWMS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'對應的功能名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'WHCommandReviseRecord_Transfer', @level2type = N'COLUMN', @level2name = N'FunctionName';

