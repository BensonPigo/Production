CREATE TABLE [dbo].[PadPrintReq](
	[ID] [varchar](13)　			CONSTRAINT [DF_PadPrintReq_ID]  DEFAULT ('') NOT NULL,
	[FactoryID] [varchar](8)　		CONSTRAINT [DF_PadPrintReq_FactoryID]  DEFAULT ('') NOT NULL,
	[BrandID] [varchar](8)　		CONSTRAINT [DF_PadPrintReq_BrandID]  DEFAULT ('') NOT NULL,
	[Handle] [varchar](10)　		CONSTRAINT [DF_PadPrintReq_Handle]  DEFAULT ('') NOT NULL,
	[ReqDate] [date] NULL,
	[Status] [varchar](50)　		CONSTRAINT [DF_PadPrintReq_Status]  DEFAULT ('') NOT NULL,
	[ApproveName] [varchar](10)　	CONSTRAINT [DF_PadPrintReq_ApproveName]  DEFAULT ('') NOT NULL,
	[ApproveDate] [datetime] NULL,
	[Remark] [nvarchar](1000)　		CONSTRAINT [DF_PadPrintReq_Remark]  DEFAULT ('') NOT NULL,
	[AddName] [varchar](10)　		CONSTRAINT [DF_PadPrintReq_AddName]  DEFAULT ('') NOT NULL,
	[AddDate] [datetime] NULL,
	[EditName] [varchar](10)　		CONSTRAINT [DF_PadPrintReq_EditName]  DEFAULT ('') NOT NULL,
	[EditDate] [datetime] NULL,
 CONSTRAINT [PK_PadPrintReq_Ukey] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
