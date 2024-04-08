CREATE TABLE [dbo].[P_FabricDispatchRate](
	[EstCutDate] [date] NOT NULL,
	[FactoryID] [varchar](8) NOT NULL,
	[FabricDispatchRate] [numeric](5, 2) NULL,
 CONSTRAINT [PK_P_FabricDispatchRate] PRIMARY KEY CLUSTERED 
(
	[EstCutDate] ASC,
	[FactoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_FabricDispatchRate] ADD  CONSTRAINT [DF_P_FabricDispatchRate_FabricDispatchRate]  DEFAULT ((0)) FOR [FabricDispatchRate]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預計裁剪日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricDispatchRate', @level2type=N'COLUMN',@level2name=N'EstCutDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠代' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricDispatchRate', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dispatch的佔比' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricDispatchRate', @level2type=N'COLUMN',@level2name=N'FabricDispatchRate'
GO
