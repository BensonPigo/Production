	CREATE TABLE [dbo].[P_DailyAccuCPULoading_History](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[Date] [varchar](5) NOT NULL,
		[FactoryID] [varchar](8) NOT NULL,
		[Month] [varchar](2) NOT NULL,
		[Year] [varchar](4) NOT NULL,
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_DailyAccuCPULoading_History] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[P_DailyAccuCPULoading_History] ADD  CONSTRAINT [DF_P_DailyAccuCPULoading_History_FactoryID]  DEFAULT ('') FOR [FactoryID]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DailyAccuCPULoading_History', @level2type=N'COLUMN',@level2name=N'Date'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�u�t�O' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DailyAccuCPULoading_History', @level2type=N'COLUMN',@level2name=N'FactoryID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DailyAccuCPULoading_History', @level2type=N'COLUMN',@level2name=N'Month'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�~' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DailyAccuCPULoading_History', @level2type=N'COLUMN',@level2name=N'Year'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�O�������u�t����ơAex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DailyAccuCPULoading_History', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ɶ��W�O�A�����g�Jtable�ɶ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_DailyAccuCPULoading_History', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO