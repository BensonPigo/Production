CREATE TABLE [dbo].[MachineTypeTPE](
	[ID] [varchar](10) NOT NULL,
	[IsDesignatedArea] [bit] NOT NULL,
 CONSTRAINT [PK_MachineType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[MachineTypeTPE] ADD  CONSTRAINT [DF_MachineType_IsDesignatedArea]  DEFAULT ((0)) FOR [IsDesignatedArea]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'機台類別代號' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MachineType', @level2type=N'COLUMN',@level2name=N'ID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'該機器類型要用於指定位置(非sewing line)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'MachineType', @level2type=N'COLUMN',@level2name=N'IsDesignatedArea'
GO