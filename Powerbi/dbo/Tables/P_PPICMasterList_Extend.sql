CREATE TABLE [dbo].[P_PPICMasterList_Extend](
	[OrderID] [varchar](13) NOT NULL,
	[ColumnName] [varchar](50) NOT NULL,
	[ColumnValue] numeric(38, 6) NOT NULL,
 CONSTRAINT [PK_P_PPICMasterList_Extend] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC,
	[ColumnName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[P_PPICMasterList_Extend] ADD  CONSTRAINT [DF_P_PPICMasterList_Extend_ColumnValue]  DEFAULT (0) FOR [ColumnValue]
GO