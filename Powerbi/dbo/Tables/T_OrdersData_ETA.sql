CREATE TABLE [dbo].[T_OrdersData_ETA](
	[Ukey] [bigint] NOT NULL,
	[SP#] [varchar](13) NULL,
	[SCHD L/ETA (SP)] [date] NULL,
	[SCHD L/ETA] [date] NULL,
	[Request ETA] [date] NULL,
	[L/ETA] [date] NULL,
	[PF ETA (SP)] [date] NULL,
	[KPI L/ETA (SP)] [date] NULL,
	[KPI L/ETA] [date] NULL,
	[Sewing Mtl. ETA (SP)] [date] NULL,
	[Sewing Mtl. ETA (SP excl. Repl.)] [date] NULL,
	[Packing Mtl. ETA (SP)] [date] NULL,
	[Actual Mtl. ETA] [date] NULL,
	[Actual Mtl. ETA (excl. Repl.)] [date] NULL,
	[Mtl. Complete] [varchar](10) NULL,
	[Mtl. Complete (SP)] [varchar](1) NULL,
	[Ori. Shipping Sewing Mtl. ETA (SP excl. Repl.)] [date] NULL,
	[OrderDataKey] [varchar](20) NOT NULL,
 CONSTRAINT [PK_T_OrdersData_ETA] PRIMARY KEY CLUSTERED 
(
	[OrderDataKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]