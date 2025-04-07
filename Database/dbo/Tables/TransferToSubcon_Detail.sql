	CREATE TABLE [dbo].[TransferToSubcon_Detail] (
    [ID]        VARCHAR (13)    CONSTRAINT [DF_TransferToSubcon_Detail_ID] DEFAULT ('') NOT NULL,
    [POID]      VARCHAR (13)    CONSTRAINT [DF_TransferToSubcon_Detail_POID] DEFAULT ('') NOT NULL,
    [Seq1]      VARCHAR (3)     CONSTRAINT [DF_TransferToSubcon_Detail_Seq1] DEFAULT ('') NOT NULL,
    [Seq2]      VARCHAR (2)     CONSTRAINT [DF_TransferToSubcon_Detail_Seq2] DEFAULT ('') NOT NULL,
    [Roll]      VARCHAR (8)     CONSTRAINT [DF_TransferToSubcon_Detail_Roll] DEFAULT ('') NOT NULL,
    [Dyelot]    VARCHAR (8)     CONSTRAINT [DF_TransferToSubcon_Detail_Dyelot] DEFAULT ('') NOT NULL,
    [StockType] VARCHAR (1)     CONSTRAINT [DF_TransferToSubcon_Detail_StockType] DEFAULT ('') NOT NULL,
    [Qty]       NUMERIC (11, 2) CONSTRAINT [DF_TransferToSubcon_Detail_Qty] DEFAULT ((0)) NOT NULL,
    [Ukey]      BIGINT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [RecvKG]    NUMERIC (7, 2)  NULL,
    CONSTRAINT [PK_TransferToSubcon_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);




	GO
	EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'外發單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon_Detail', @level2type=N'COLUMN',@level2name=N'ID'
	GO

	EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'採購單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon_Detail', @level2type=N'COLUMN',@level2name=N'POID'
	GO

	EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料大項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon_Detail', @level2type=N'COLUMN',@level2name=N'Seq1'
	GO

	EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'物料小項' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon_Detail', @level2type=N'COLUMN',@level2name=N'Seq2'
	GO

	EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'捲號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon_Detail', @level2type=N'COLUMN',@level2name=N'Roll'
	GO

	EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'缸號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon_Detail', @level2type=N'COLUMN',@level2name=N'Dyelot'
	GO

	EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'倉庫代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon_Detail', @level2type=N'COLUMN',@level2name=N'StockType'
	GO

	EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'轉出當下的庫存數量' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon_Detail', @level2type=N'COLUMN',@level2name=N'Qty'
	GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'記錄轉出當下
布卷重量 
( 資料來源 收料單 )', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TransferToSubcon_Detail', @level2type = N'COLUMN', @level2name = N'RecvKG';

