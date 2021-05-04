CREATE TABLE [dbo].[BundleInOut] (
    [BundleNo]              VARCHAR (10) CONSTRAINT [DF_BundleInOut_BundleNo] DEFAULT ('') NOT NULL,
    [SubProcessId]          VARCHAR (15) CONSTRAINT [DF_BundleInOut_SubProcessId] DEFAULT ('') NOT NULL,
    [InComing]              DATETIME     NULL,
    [OutGoing]              DATETIME     NULL,
    [AddDate]               DATETIME     NOT NULL,
    [EditDate]              DATETIME     NULL,
    [SewingLineID]          VARCHAR (2)  NOT NULL DEFAULT (''),
    [LocationID]            VARCHAR (10) DEFAULT ('') NOT NULL,
    [RFIDProcessLocationID] VARCHAR (15) CONSTRAINT [DF_BundleInOut_RFIDProcessLocationID] DEFAULT ('') NOT NULL,
    [PanelNo]               VARCHAR (24) CONSTRAINT [DF_BundleInOut_PanelNo] DEFAULT ('') NOT NULL,
    [CutCellID]             VARCHAR (10) CONSTRAINT [DF_BundleInOut_CutCellID] DEFAULT ('') NOT NULL, 
    CONSTRAINT [PK_BundleInOut] PRIMARY KEY ([SubProcessId], [BundleNo], [RFIDProcessLocationID])
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Out', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut', @level2type = N'COLUMN', @level2name = N'OutGoing';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'In', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut', @level2type = N'COLUMN', @level2name = N'InComing';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sub-process Id', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut', @level2type = N'COLUMN', @level2name = N'SubProcessId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bundle No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut', @level2type = N'COLUMN', @level2name = N'BundleNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Bundle-Subprocess In Out Date', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'BundleInOut';


GO


CREATE NONCLUSTERED INDEX [BundleIO_InComing] ON [dbo].[BundleInOut]
(
	[InComing] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
;
	
GO
CREATE NONCLUSTERED INDEX [BundleIO_OutGoing] ON [dbo].[BundleInOut]
(
	[OutGoing] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
;

GO