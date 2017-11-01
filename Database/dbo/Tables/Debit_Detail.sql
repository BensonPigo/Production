CREATE TABLE [dbo].[Debit_Detail] (
    [ID]          VARCHAR (13)    CONSTRAINT [DF_Debit_Detail_ID] DEFAULT ('') NOT NULL,
    [OrderID]     VARCHAR (13)    CONSTRAINT [DF_Debit_Detail_OrderID] DEFAULT ('') NULL,
    [ReasonID]    VARCHAR (5)     CONSTRAINT [DF_Debit_Detail_ReasonID] DEFAULT ('') NULL,
    [Description] NVARCHAR (MAX)  CONSTRAINT [DF_Debit_Detail_Description] DEFAULT ('') NULL,
    [Price]       NUMERIC (16, 4)  CONSTRAINT [DF_Debit_Detail_Price] DEFAULT ((0)) NULL,
    [Amount]      NUMERIC (13, 2) CONSTRAINT [DF_Debit_Detail_Amount] DEFAULT ((0)) NULL,
    [UnitID]      VARCHAR (8)     CONSTRAINT [DF_Debit_Detail_UnitID] DEFAULT ('') NULL,
    [SourceID]    VARCHAR (13)    CONSTRAINT [DF_Debit_Detail_SourceID] DEFAULT ('') NULL,
    [Qty]         NUMERIC (11, 2) CONSTRAINT [DF_Debit_Detail_Qty] DEFAULT ((0)) NULL,
    [ReasonNM]    VARCHAR (60)    CONSTRAINT [DF_Debit_Detail_ReasonNM] DEFAULT ('') NULL,
    [TaipeiUkey]  BIGINT          CONSTRAINT [DF_Debit_Detail_TaipeiUkey] DEFAULT ((0)) NOT NULL,
    [Ukey]        BIGINT          IDENTITY (1, 1) NOT NULL,
    CONSTRAINT [PK_Debit_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [TaipeiUkey] ASC, [Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Debit', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Detail', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原因代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Detail', @level2type = N'COLUMN', @level2name = N'ReasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'明細資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Detail', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'價格', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Detail', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'請款金額', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Detail', @level2type = N'COLUMN', @level2name = N'Amount';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Detail', @level2type = N'COLUMN', @level2name = N'UnitID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'相關單據', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Detail', @level2type = N'COLUMN', @level2name = N'SourceID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'扣款原因', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Detail', @level2type = N'COLUMN', @level2name = N'ReasonNM';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'TaipeiUkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Detail', @level2type = N'COLUMN', @level2name = N'TaipeiUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Debit_Detail', @level2type = N'COLUMN', @level2name = N'Ukey';

