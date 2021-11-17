CREATE TABLE [dbo].[TransferLocation] (
    [ID]             BIGINT         NOT NULL,
    [SCICtnNo]       VARCHAR (15)   NOT NULL,
    [CustCTN]        VARCHAR (30)   NOT NULL,
    [GW]             NUMERIC (7, 3) NULL,
    [ClogLocationId] VARCHAR (10)   NULL,
    [Pallet]         VARCHAR (50)   NULL,
    [Time]           DATETIME       NOT NULL,
    [SCIUpdate]      BIT            DEFAULT ((0)) NOT NULL,
    [Type]           VARCHAR (15)   DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_TransferLocation] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fty2Clog : 入MiniLoad倉後 & 退回的箱子再次入倉
CFA2Clog : Clog 入庫 - 從 CFA 收回紙箱
UpdLocation : Clog 儲位調整', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferLocation', @level2type = N'COLUMN', @level2name = N'Type';

