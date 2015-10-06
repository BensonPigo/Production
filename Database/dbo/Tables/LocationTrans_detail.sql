CREATE TABLE [dbo].[LocationTrans_detail] (
    [ID]           VARCHAR (13)    CONSTRAINT [DF_LocationTrans_detail_ID] DEFAULT ('') NOT NULL,
    [Poid]         VARCHAR (13)    CONSTRAINT [DF_LocationTrans_detail_Poid] DEFAULT ('') NOT NULL,
    [Seq]          VARCHAR (5)     CONSTRAINT [DF_LocationTrans_detail_Seq] DEFAULT ('') NOT NULL,
    [Roll]         VARCHAR (8)     CONSTRAINT [DF_LocationTrans_detail_Roll] DEFAULT ('') NOT NULL,
    [Dyelot]       VARCHAR (4)     CONSTRAINT [DF_LocationTrans_detail_Dyelot] DEFAULT ('') NOT NULL,
    [FromLocation] VARCHAR (60)    CONSTRAINT [DF_LocationTrans_detail_FromLocation] DEFAULT ('') NULL,
    [ToLocation]   VARCHAR (60)    CONSTRAINT [DF_LocationTrans_detail_ToLocation] DEFAULT ('') NOT NULL,
    [Qty]          NUMERIC (10, 2) CONSTRAINT [DF_LocationTrans_detail_Qty] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_LocationTrans_detail] PRIMARY KEY CLUSTERED ([ID] ASC, [Poid] ASC, [Seq] ASC, [Roll] ASC, [Dyelot] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'儲位變更明細', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans_detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'單據號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans_detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans_detail', @level2type = N'COLUMN', @level2name = N'Poid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans_detail', @level2type = N'COLUMN', @level2name = N'Seq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'卷號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans_detail', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans_detail', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'來源儲位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans_detail', @level2type = N'COLUMN', @level2name = N'FromLocation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'目的儲位', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans_detail', @level2type = N'COLUMN', @level2name = N'ToLocation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'當下庫存數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LocationTrans_detail', @level2type = N'COLUMN', @level2name = N'Qty';

