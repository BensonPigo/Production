CREATE TABLE [dbo].[TransferToSubcon]
(
	[ID] [varchar](13) NOT NULL,
	[MDivisionID] [varchar](8)	CONSTRAINT [DF_TransferToSubcon_MDivisionID]	DEFAULT ('') NOT NULL,
	[FactoryID] [varchar](8)	CONSTRAINT [DF_TransferToSubcon_FactoryID]		DEFAULT ('') NOT NULL,
	[TransferOutDate] [date] NULL,
	[Subcon] [varchar](20)		CONSTRAINT [DF_TransferToSubcon_Subcon]			DEFAULT ('') NOT NULL,
	[Status] [varchar](15)		CONSTRAINT [DF_TransferToSubcon_Status]			DEFAULT ('') NOT NULL,
	[Remark] [nvarchar](100)	CONSTRAINT [DF_TransferToSubcon_Remark]			DEFAULT ('') NOT NULL,
	[AddDate] [datetime] NULL,
	[AddName] [varchar](10)		CONSTRAINT [DF_TransferToSubcon_AddName]		DEFAULT ('') NOT NULL,
	[EditDate] [datetime] NULL,
	[EditName] [varchar](10)	CONSTRAINT [DF_TransferToSubcon_EditName]		DEFAULT ('') NOT NULL,
	CONSTRAINT [PK_TransferToSubcon] PRIMARY KEY CLUSTERED ([ID] ASC)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'外發單號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'M' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon', @level2type=N'COLUMN',@level2name=N'MDivisionID'
GO

EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'工廠代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'外發日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon', @level2type=N'COLUMN',@level2name=N'TransferOutDate'
GO

EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'外發加工段' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon', @level2type=N'COLUMN',@level2name=N'Subcon'
GO

EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'單據狀態' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon', @level2type=N'COLUMN',@level2name=N'Status'
GO

EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'備註' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon', @level2type=N'COLUMN',@level2name=N'Remark'
GO

EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon', @level2type=N'COLUMN',@level2name=N'AddDate'
GO

EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'新增人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon', @level2type=N'COLUMN',@level2name=N'AddName'
GO

EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon', @level2type=N'COLUMN',@level2name=N'EditDate'
GO

EXECUTE sys.sp_addextendedproperty @name=N'MS_Description', @value=N'編輯人員' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TransferToSubcon', @level2type=N'COLUMN',@level2name=N'EditName'
GO
