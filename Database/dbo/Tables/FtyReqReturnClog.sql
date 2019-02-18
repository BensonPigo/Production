CREATE TABLE [dbo].[FtyReqReturnClog](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[RequestID] [Varchar](13) NOT NULL CONSTRAINT [DF_FtyReqReturnClog_RequestID]  DEFAULT (''),
	[RequestDate] DATE NOT NULL ,
	[Reason] [Varchar](5) NOT NULL CONSTRAINT [DF_FtyReqReturnClog_Reason]  DEFAULT (''),
	[MDivisionID] [Varchar](8) NOT NULL CONSTRAINT [DF_FtyReqReturnClog_MDivisionID]  DEFAULT (''),
	[OrderID] [Varchar](13) NOT NULL CONSTRAINT [DF_FtyReqReturnClog_OrderID]  DEFAULT (''),
	[PackingListID] [Varchar](13) NOT NULL CONSTRAINT [DF_FtyReqReturnClog_PackingListID]  DEFAULT (''),
	[CTNStartNo] [Varchar](6) NOT NULL CONSTRAINT [DF_FtyReqReturnClog_CTNStartNo]  DEFAULT (''),
	[AddName] [Varchar](10) NOT NULL CONSTRAINT [DF_FtyReqReturnClog_AddName]  DEFAULT (''),
	[AddDate] [Datetime] NOT NULL ,
 CONSTRAINT [PK_FtyReqReturnClog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) 
GO

	EXEC sys.sp_addextendedproperty @name=N'MS_Description', 
	@value=N'記錄各裁次實際使用的SEQ、Roll、Dyelot、Yardage' 
	,@level0type=N'SCHEMA',@level0name=N'dbo'
	,@level1type=N'TABLE',@level1name=N'FtyReqReturnClog'
	;
GO
	
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', 
	@value=N'箱號' 
	,@level0type=N'SCHEMA',@level0name=N'dbo'
	,@level1type=N'TABLE',@level1name=N'FtyReqReturnClog'
	,@level2type=N'COLUMN',@level2name=N'CTNStartNo'
	;
GO
	EXEC sys.sp_addextendedproperty @name=N'MS_Description', 
	@value=N'PackID' 
	,@level0type=N'SCHEMA',@level0name=N'dbo'
	,@level1type=N'TABLE',@level1name=N'FtyReqReturnClog'
	,@level2type=N'COLUMN',@level2name=N'PackingListID'
	;
	
GO