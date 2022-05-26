CREATE TABLE [dbo].[CFAInspectionRecord_OrderSEQ](
	[Ukey] [bigint] IDENTITY(1,1) NOT NULL,
	[ID] [varchar](13) NOT NULL,
	[OrderID] [varchar](13) NOT NULL,
	[SEQ] [varchar](2) NOT NULL,
	[Carton] [varchar](max) NULL,
 CONSTRAINT [PK_CFAInspectionRecord_OrderSEQ] PRIMARY KEY CLUSTERED 
(
	[Ukey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[CFAInspectionRecord_OrderSEQ] ADD  CONSTRAINT [DF_CFAInspectionRecord_OrderSEQ_ID]  DEFAULT ('') FOR [ID]
GO

ALTER TABLE [dbo].[CFAInspectionRecord_OrderSEQ] ADD  CONSTRAINT [DF_CFAInspectionRecord_OrderSEQ_OrderID]  DEFAULT ('') FOR [OrderID]
GO

ALTER TABLE [dbo].[CFAInspectionRecord_OrderSEQ] ADD  CONSTRAINT [DF_CFAInspectionRecord_OrderSEQ_Seq]  DEFAULT ('') FOR [SEQ]
GO

ALTER TABLE [dbo].[CFAInspectionRecord_OrderSEQ] ADD  CONSTRAINT [DF_CFAInspectionRecord_OrderSEQ_Carton]  DEFAULT ('') FOR [Carton]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'此次檢驗的紙箱箱號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'CFAInspectionRecord_OrderSEQ', @level2type=N'COLUMN',@level2name=N'Carton'
GO