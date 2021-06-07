﻿CREATE TABLE [dbo].[Receiving_Detail] (
    [Id]           VARCHAR (13)    CONSTRAINT [DF_Receiving_Detail_Id] DEFAULT ('') NOT NULL,
    [MDivisionID]  VARCHAR (8)     CONSTRAINT [DF_Receiving_Detail_MDivisionID] DEFAULT ('') NULL,
    [PoId]         VARCHAR (13)    CONSTRAINT [DF_Receiving_Detail_PoId] DEFAULT ('') NOT NULL,
    [Seq1]         VARCHAR (3)     CONSTRAINT [DF_Receiving_Detail_Seq1] DEFAULT ('') NOT NULL,
    [Seq2]         VARCHAR (2)     CONSTRAINT [DF_Receiving_Detail_Seq2] DEFAULT ('') NOT NULL,
    [Roll]         VARCHAR (8)     CONSTRAINT [DF_Receiving_Detail_Roll] DEFAULT ('') NOT NULL,
    [Dyelot]       VARCHAR (8)     CONSTRAINT [DF_Receiving_Detail_Dyelot] DEFAULT ('') NOT NULL,
    [ShipQty]      NUMERIC (11, 2) CONSTRAINT [DF_Receiving_Detail_ShipQty] DEFAULT ((0)) NULL,
    [ActualQty]    NUMERIC (11, 2) CONSTRAINT [DF_Receiving_Detail_ActualQty] DEFAULT ((0)) NOT NULL,
    [PoUnit]       VARCHAR (8)     CONSTRAINT [DF_Receiving_Detail_PoUnit] DEFAULT ('') NULL,
    [Weight]       NUMERIC (7, 2)  CONSTRAINT [DF_Receiving_Detail_Weight] DEFAULT ((0)) NULL,
    [ActualWeight] NUMERIC (7, 2)  CONSTRAINT [DF_Receiving_Detail_ActualWeight] DEFAULT ((0)) NULL,
    [StockUnit]    VARCHAR (8)     CONSTRAINT [DF_Receiving_Detail_StockUnit] DEFAULT ('') NULL,
    [Price]        NUMERIC (11, 3) CONSTRAINT [DF_Receiving_Detail_Price] DEFAULT ((0)) NULL,
    [Location]     VARCHAR (500)    CONSTRAINT [DF_Receiving_Detail_Location] DEFAULT ('') NULL,
    [Remark]       NVARCHAR (100)  CONSTRAINT [DF_Receiving_Detail_Remark] DEFAULT ('') NULL,
    [StockQty]     NUMERIC (11, 2) CONSTRAINT [DF_Receiving_Detail_StockQty] DEFAULT ((0)) NULL,
    [StockType]    VARCHAR (1)     CONSTRAINT [DF_Receiving_Detail_StockType] DEFAULT ('') NULL,
    [Ukey]         BIGINT          IDENTITY (1, 1) NOT NULL,
    [CompleteTime] DATETIME NULL, 
    [CombineBarcode] VARCHAR NULL, 
    [Unoriginal] BIT NULL, 
    [EncodeSeq] INT NOT NULL DEFAULT ((0)), 
    [SentToWMS] BIT NOT NULL DEFAULT ((0)), 
    [Fabric2LabTime] DATETIME  NULL,
    [Fabric2LabBy] VARCHAR(10) CONSTRAINT [DF_Receiving_Detail_Fabric2LabBy] DEFAULT ('') not NULL,
    [Checker] NVARCHAR(30) NOT NULL DEFAULT (''), 
    CONSTRAINT [PK_Receiving_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);












GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Receiving Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉庫剪布給實驗室的時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'Fabric2LabTime';

GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'更新倉庫剪布給實驗室時間的人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'Fabric2LabBy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'收料單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'PoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'Seq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'出貨數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'ShipQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實收數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'ActualQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'PoUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購重量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'Weight';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實收重量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'ActualWeight';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存單位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'StockUnit';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單價', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'Price';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'儲位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'Location';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'庫存實收數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'StockQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'倉別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'StockType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組織代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Receiving_Detail', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [dbo].[Receiving_Detail]([PoId] ASC, [Seq1] ASC, [Seq2] ASC, [Remark] ASC)
    INCLUDE([Id]);


GO
CREATE NONCLUSTERED INDEX [id_UKEY]
    ON [dbo].[Receiving_Detail]([Id] ASC, [Ukey] ASC);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'用來當作合併布卷條碼判斷值',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Receiving_Detail',
    @level2type = N'COLUMN',
    @level2name = N'CombineBarcode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否是來源值',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Receiving_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Unoriginal'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'使用者匯入 / 新增資料的順序',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Receiving_Detail',
    @level2type = N'COLUMN',
    @level2name = N'EncodeSeq'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'PH 收料時負責秤重 + 剪一小塊布 (ShadeBand) + 搬該物料入庫',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Receiving_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Checker'