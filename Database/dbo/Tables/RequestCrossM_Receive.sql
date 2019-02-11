CREATE TABLE [dbo].[RequestCrossM_Receive] (
    [Id]          VARCHAR (13)    CONSTRAINT [DF_RequestCrossM_Receive_Id] DEFAULT ('') NOT NULL,
    [Qty]         NUMERIC (10, 2) CONSTRAINT [DF_RequestCrossM_Receive_Qty] DEFAULT ((0)) NULL,
    [MDivisionID] VARCHAR (8)     CONSTRAINT [DF_RequestCrossM_Receive_MDivisionID] DEFAULT ('') NULL,
    [POID]        VARCHAR (13)    CONSTRAINT [DF_RequestCrossM_Receive_POID] DEFAULT ('') NULL,
    [Seq1]        VARCHAR (3)     CONSTRAINT [DF_RequestCrossM_Receive_Seq1] DEFAULT ('') NULL,
    [Seq2]        VARCHAR (2)     CONSTRAINT [DF_RequestCrossM_Receive_Seq2] DEFAULT ('') NULL,
    [Roll]        VARCHAR (8)     CONSTRAINT [DF_RequestCrossM_Receive_Roll] DEFAULT ('') NULL,
    [Dyelot]      VARCHAR (8)     CONSTRAINT [DF_RequestCrossM_Receive_Dyelot] DEFAULT ('') NULL,
    [StockType]   CHAR (1)        CONSTRAINT [DF_RequestCrossM_Receive_StockType] DEFAULT ('') NULL,
    [Ukey]        BIGINT          IDENTITY (1, 1) NOT NULL,
    [Location]    VARCHAR (60)    CONSTRAINT [DF_RequestCrossM_Receive_Location] DEFAULT ('') NULL,
    CONSTRAINT [PK_RequestCrossM_Receive] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);



