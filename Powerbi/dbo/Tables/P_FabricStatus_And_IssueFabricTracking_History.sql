	CREATE TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking_History](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[RefNo] [varchar](36) NOT NULL,
		[ReplacementID] [varchar](13) NOT NULL,
		[Seq] [varchar](6) NOT NULL,
		[SP] [varchar](13) NOT NULL,
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_FabricStatus_And_IssueFabricTracking_History] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking_History] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_History_RefNO]  DEFAULT ('') FOR [RefNo]
	ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking_History] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_History_ReplacementID]  DEFAULT ('') FOR [ReplacementID]
	ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking_History] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_History_Seq]  DEFAULT ('') FOR [Seq]
	ALTER TABLE [dbo].[P_FabricStatus_And_IssueFabricTracking_History] ADD  CONSTRAINT [DF_P_FabricStatus_And_IssueFabricTracking_History_SP]  DEFAULT ('') FOR [SP]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�Ƹ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricStatus_And_IssueFabricTracking_History', @level2type=N'COLUMN',@level2name=N'RefNo'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Replacement ���X' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricStatus_And_IssueFabricTracking_History', @level2type=N'COLUMN',@level2name=N'ReplacementID'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�s��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricStatus_And_IssueFabricTracking_History', @level2type=N'COLUMN',@level2name=N'Seq'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�q��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricStatus_And_IssueFabricTracking_History', @level2type=N'COLUMN',@level2name=N'SP'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�O�������u�t����ơAex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricStatus_And_IssueFabricTracking_History', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ɶ��W�O�A�����g�Jtable�ɶ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricStatus_And_IssueFabricTracking_History', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
	Go