CREATE TABLE [dbo].[BundleTrack_detail] (
    [Id]          VARCHAR (13) CONSTRAINT [DF_BundleTrack_detail_Id] DEFAULT ('') NOT NULL,
    [BundleNo]    VARCHAR (10) CONSTRAINT [DF_BundleTrack_detail_BundleNo] DEFAULT ('') NOT NULL,
    [orderid]     VARCHAR (13) CONSTRAINT [DF_BundleTrack_detail_orderid] DEFAULT ('') NULL,
    [ReceiveDate] DATETIME     NULL,
    [ReceiveName] VARCHAR (10) CONSTRAINT [DF_BundleTrack_detail_ReceiveName] DEFAULT ('') NULL,
    [IsImport]    BIT          CONSTRAINT [DF_BundleTrack_detail_IsImport] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_BundleTrack_detail] PRIMARY KEY CLUSTERED ([Id] ASC, [BundleNo] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Real Time bundle track detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTrack_detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTrack_detail', @level2type = N'COLUMN', @level2name = N'Id';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTrack_detail', @level2type = N'COLUMN', @level2name = N'BundleNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTrack_detail', @level2type = N'COLUMN', @level2name = N'orderid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTrack_detail', @level2type = N'COLUMN', @level2name = N'ReceiveDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTrack_detail', @level2type = N'COLUMN', @level2name = N'ReceiveName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleTrack_detail', @level2type = N'COLUMN', @level2name = N'IsImport';

