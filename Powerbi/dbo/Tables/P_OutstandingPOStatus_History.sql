	CREATE TABLE [dbo].[P_OutstandingPOStatus_History](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[Buyerdelivery] date,
		[FTYGroup] [varchar](3) NOT NULL,
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_OutstandingPOStatus_History] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[P_OutstandingPOStatus_History] ADD  CONSTRAINT [DF_P_OutstandingPOStatus_History_FactoryID]  DEFAULT ('') FOR [FTYGroup]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ʥ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OutstandingPOStatus_History', @level2type=N'COLUMN',@level2name=N'Buyerdelivery'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�u�t�էO' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OutstandingPOStatus_History', @level2type=N'COLUMN',@level2name=N'FTYGroup'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�O�������u�t����ơAex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OutstandingPOStatus_History', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ɶ��W�O�A�����g�Jtable�ɶ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_OutstandingPOStatus_History', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
	Go