	CREATE TABLE [dbo].[P_MtlStatusAnalisis_History](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[SPNo] [varchar](20),
		[FactoryID] [varchar](8),
		[IsProduceFty] bit,
		[Close_Date] DATETIME,
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_MtlStatusAnalisis_History] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�q�渹�X' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis_History', @level2type=N'COLUMN',@level2name=N'SPNo'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�u�t�O' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis_History', @level2type=N'COLUMN',@level2name=N'FactoryID'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�O�_�� Produce Fty' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis_History', @level2type=N'COLUMN',@level2name=N'IsProduceFty'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis_History', @level2type=N'COLUMN',@level2name=N'Close_Date'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�O�������u�t����ơAex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis_History', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
	Go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ɶ��W�O�A�����g�Jtable�ɶ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_MtlStatusAnalisis_History', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
	Go