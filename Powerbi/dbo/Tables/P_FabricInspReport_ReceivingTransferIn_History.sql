	CREATE TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn_History](
		[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
		[DefectCode] [varchar](2) NOT NULL,
		[Dyelot] [varchar](8) NOT NULL,
		[POID] [varchar](13) NOT NULL,
		[ReceivingID] [varchar](13) NOT NULL,
		[Roll] [varchar](8) NOT NULL,
		[SEQ] [varchar](6) NOT NULL,
		[BIFactoryID] [varchar](8) Not NULL,
		[BIInsertDate] [datetime] NOT NULL,	
	 CONSTRAINT [PK_P_FabricInspReport_ReceivingTransferIn_History] PRIMARY KEY CLUSTERED 
	(
		[Ukey] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE [dbo].[P_FabricInspReport_ReceivingTransferIn_History] ADD  CONSTRAINT [DF_P_FabricInspReport_ReceivingTransferIn_History_FactoryID]  DEFAULT ('') FOR [FactoryID]

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'���ʥ��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn_History', @level2type=N'COLUMN',@level2name=N'Buyerdelivery'
	go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'����' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn_History', @level2type=N'COLUMN',@level2name=N'POID'
	go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Receiving �渹' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn_History', @level2type=N'COLUMN',@level2name=N'ReceivingID'
	go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�s��' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn_History', @level2type=N'COLUMN',@level2name=N'SEQ'
	go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�O�������u�t����ơAex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn_History', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
	go
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'�ɶ��W�O�A�����g�Jtable�ɶ�' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricInspReport_ReceivingTransferIn_History', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
	go