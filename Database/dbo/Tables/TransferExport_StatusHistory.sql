CREATE TABLE [dbo].[TransferExport_StatusHistory](
	[ID] [varchar](13) NOT NULL,
	[OldStatus] [varchar](20) NOT NULL,
	[NewStatus] [varchar](20) NOT NULL,
	[OldFtyStatus] [varchar](30) NOT NULL,
	[NewFtyStatus] [varchar](30) NOT NULL,
	[UpdateDate] [datetime] NULL,
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_TransferExport_StatusHistory] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[TransferExport_StatusHistory] ADD  CONSTRAINT [DF_TransferExport_StatusHistory_ID]  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[TransferExport_StatusHistory] ADD  CONSTRAINT [DF_TransferExport_StatusHistory_OldStatus]  DEFAULT ('') FOR [OldStatus]
GO

ALTER TABLE [dbo].[TransferExport_StatusHistory] ADD  CONSTRAINT [DF_TransferExport_StatusHistory_NewStatus]  DEFAULT ('') FOR [NewStatus]
GO

ALTER TABLE [dbo].[TransferExport_StatusHistory] ADD  CONSTRAINT [DF_TransferExport_StatusHistory_OldFtyStatus]  DEFAULT ('') FOR [OldFtyStatus]
GO

ALTER TABLE [dbo].[TransferExport_StatusHistory] ADD  CONSTRAINT [DF_Table_1_NewFtyStatus]  DEFAULT ('') FOR [NewFtyStatus]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'TK ID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferExport_StatusHistory', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'台北 TK 的狀態更新前' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferExport_StatusHistory', @level2type=N'COLUMN',@level2name=N'OldStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'台北 TK 的狀態更新後' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferExport_StatusHistory', @level2type=N'COLUMN',@level2name=N'NewStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠 TK 的狀態更新前' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferExport_StatusHistory', @level2type=N'COLUMN',@level2name=N'OldFtyStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠 TK 的狀態更新後' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferExport_StatusHistory', @level2type=N'COLUMN',@level2name=N'NewFtyStatus'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'更新的日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferExport_StatusHistory', @level2type=N'COLUMN',@level2name=N'UpdateDate'
GO


