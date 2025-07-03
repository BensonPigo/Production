CREATE TABLE [dbo].[P_OustandingPO_History](
	[HistoryUkey] [bigint] IDENTITY(1,1) NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[OrderID] [varchar](13) NOT NULL,
	[Seq] [varchar](2) NOT NULL,
	[BIFactoryID] [varchar](8) NOT NULL,
	[BIInsertDate] [datetime] NOT NULL,
 CONSTRAINT [PK_P_OustandingPO_History] PRIMARY KEY CLUSTERED 
(
	[HistoryUkey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�u�t�s��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OustandingPO_History', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�q��s��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OustandingPO_History', @level2type=N'COLUMN',@level2name=N'OrderID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ǽs��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OustandingPO_History', @level2type=N'COLUMN',@level2name=N'Seq'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�O�������u�t����ơAex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OustandingPO_History', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ɶ��W�O�A�����g�Jtable�ɶ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OustandingPO_History', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
