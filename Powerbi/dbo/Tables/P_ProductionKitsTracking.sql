CREATE TABLE [dbo].[P_ProductionKitsTracking]
(
	[BrandID] VARCHAR(8) NOT NULL DEFAULT (('')), 
    [StyleID] VARCHAR(15) NOT NULL DEFAULT (('')), 
    [SeasonID] VARCHAR(10) NOT NULL DEFAULT (('')), 
    [Article] NVARCHAR(1000) NOT NULL DEFAULT (('')), 
    [Mdivision] VARCHAR(8) NOT NULL DEFAULT (('')), 
    [FactoryID] VARCHAR(8) NOT NULL DEFAULT (('')), 
    [Doc] NVARCHAR(506) NOT NULL DEFAULT (('')), 
    [TWSendDate] DATE NULL, 
    [FtyMRRcvDate] DATE NULL, 
    [FtySendtoQADate] DATE NULL, 
    [QARcvDate] DATE NULL, 
    [UnnecessaryToSend] VARCHAR NOT NULL DEFAULT (('')), 
    [ProvideDate] DATE NULL, 
    [SPNo] VARCHAR(13) NOT NULL DEFAULT (('')), 
    [SCIDelivery] DATE NULL, 
    [BuyerDelivery] DATE NULL, 
    [Pullforward] VARCHAR(1) NOT NULL DEFAULT (('')), 
    [Handle] VARCHAR(61) NOT NULL DEFAULT (('')), 
    [MRHandle] VARCHAR(61) NOT NULL DEFAULT (('')), 
    [SMR] VARCHAR(61) NOT NULL DEFAULT (('')), 
    [POHandle] VARCHAR(61) NOT NULL DEFAULT (('')), 
    [POSMR] VARCHAR(61) NOT NULL DEFAULT (('')), 
    [FtyHandle] VARCHAR(41) NOT NULL DEFAULT (('')), 
    CONSTRAINT [PK_P_ProductionKitsTracking] PRIMARY KEY ([Article],[FactoryID],[Doc],[SPNo])

)
