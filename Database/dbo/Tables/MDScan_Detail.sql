CREATE TABLE [dbo].[MDScan_Detail] (
    [UKey]            BIGINT      IDENTITY (1, 1) NOT NULL,
    [MDScanUKey]      BIGINT      NOT NULL,
    [PackingReasonID] VARCHAR (5) CONSTRAINT [DF_MDScan_Detail_PackingReasonID] DEFAULT ('') NOT NULL,
    [Qty]             INT         CONSTRAINT [DF_MDScan_Detail_Qty] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_MDScan_Detail] PRIMARY KEY CLUSTERED ([UKey] ASC)
);

