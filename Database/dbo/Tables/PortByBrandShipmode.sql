	CREATE TABLE [dbo].PortByBrandShipmode(
		PortID varchar(20) NOT NULL,
		BrandID varchar(80) NOT NULL,
		Remark nvarchar(max) NOT NULL,
		Junk Bit NOT NULL CONSTRAINT [DF_PortByBrandShipmode_Junk] DEFAULT (0),
		AddDate Datetime NULL,
		AddName varchar(10) NOT NULL CONSTRAINT [DF_PortByBrandShipmode_AddName] DEFAULT (''),
		EditDate Datetime NULL,
		EditName varchar(10) NOT NULL CONSTRAINT [DF_PortByBrandShipmode_EditName] DEFAULT (''),
	 CONSTRAINT [PK_PortByBrandShipmode] PRIMARY KEY CLUSTERED 
	(
		PortID ASC,
		BrandID ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
GO