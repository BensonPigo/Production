	CREATE TABLE [dbo].[P_PPICMASTERLIST_History](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[ColumnName] [varchar](50) not null,
		[OrderID] [varchar](13) NOT NULL,
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_PPICMASTERLIST_History] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

		ALTER TABLE [dbo].[P_PPICMASTERLIST_History] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_History_ColumnName]  DEFAULT ('') FOR [ColumnName]
	ALTER TABLE [dbo].[P_PPICMASTERLIST_History] ADD  CONSTRAINT [DF_P_PPICMASTERLIST_History_OrderID]  DEFAULT ('') FOR [OrderID]


	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�u�t�էO' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST_History', @level2type=N'COLUMN',@level2name=N'SPNO'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�O�������u�t����ơAex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST_History', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
	GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ɶ��W�O�A�����g�Jtable�ɶ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_PPICMASTERLIST_History', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
	GO