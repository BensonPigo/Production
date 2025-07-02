CREATE TABLE [dbo].[P_CuttingScheduleOutputList_History](
		[HistoryUkey] [bigint] IDENTITY(1,1) NOT NULL,
		[Ukey] [BIGINT] NOT NULL DEFAULT ((0)), 
		[Buyerdelivery] date,
		[FactoryID] [varchar](8) NOT NULL,
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_CuttingScheduleOutputList_History] PRIMARY KEY CLUSTERED 
	(
		[HistoryUkey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[P_CuttingScheduleOutputList_History] ADD  CONSTRAINT [DF_P_CuttingScheduleOutputList_History_FactoryID]  DEFAULT ('') FOR [FactoryID]

	EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Ukey',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CuttingScheduleOutputList_History',
    @level2type = N'COLUMN',
    @level2name = N'Ukey'

	EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'�O�������u�t����ơAex PH1, PH2',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CuttingScheduleOutputList_History',
    @level2type = N'COLUMN',
    @level2name = N'BIFactoryID'

	EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'�ɶ��W�O�A�����g�Jtable�ɶ�',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_CuttingScheduleOutputList_History',
    @level2type = N'COLUMN',
    @level2name = N'BIInsertDate'