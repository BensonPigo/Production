
CREATE TABLE [dbo].[CFAInspectionRecord_Detail](
	[ID] [varchar](13) NOT NULL,
	[GarmentDefectCodeID] [varchar](3) NOT NULL,
	[GarmentDefectTypeID] [varchar](1) NOT NULL,
	[Qty] [numeric](5, 0) NOT NULL,
	[Action] [nvarchar](300) NOT NULL,
	[Remark] [nvarchar](500) NOT NULL,
	[CFAAreaID] [varchar](3) NOT NULL,
	 CONSTRAINT [PK_CFAInspectionRecord_Detail] PRIMARY KEY CLUSTERED 
	(
		[ID] ASC,
		[GarmentDefectCodeID] ASC
	)
) 
;

ALTER TABLE [dbo].[CFAInspectionRecord_Detail] ADD  DEFAULT ('') FOR [GarmentDefectTypeID]
GO

ALTER TABLE [dbo].[CFAInspectionRecord_Detail] ADD  DEFAULT ((0)) FOR [Qty]
GO

ALTER TABLE [dbo].[CFAInspectionRecord_Detail] ADD  DEFAULT ('') FOR [Action]
GO

ALTER TABLE [dbo].[CFAInspectionRecord_Detail] ADD  DEFAULT ('') FOR [Remark]
GO

ALTER TABLE [dbo].[CFAInspectionRecord_Detail] ADD  DEFAULT ('') FOR [CFAAreaID]
GO


