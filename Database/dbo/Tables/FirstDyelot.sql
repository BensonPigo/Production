CREATE TABLE [dbo].[FirstDyelot] (
    [TestDocFactoryGroup]      VARCHAR (8)  NOT NULL,
    [Refno]          VARCHAR (36) NOT NULL,
    [SuppID]         VARCHAR (6)   NOT NULL,
    [ColorID]        VARCHAR (6)   NOT NULL,
    [SeasonSCIID]    VARCHAR (8)  NOT NULL,
    [Period]         INT          CONSTRAINT [DF_FirstDyelot_Period] DEFAULT ((0)) NULL,
    [FirstDyelot]    DATE          NULL,
    [TPEFirstDyelot] DATE         NULL,
    [EditName]       VARCHAR (10) NOT NULL DEFAULT (''),
    [EditDate]       DATETIME     NULL,
    [BrandRefno] VARCHAR(20) NOT NULL DEFAULT (''), 
    [AWBno] VARCHAR(30) NOT NULL DEFAULT (''), 
    [AddName] VARCHAR(10) NOT NULL DEFAULT (''), 
    [AddDate] DATETIME NULL, 
    [FTYReceivedReport] DATE NULL, 
    [ReceivedDate] DATE NULL, 
    [ReceivedRemark] VARCHAR(MAX) NOT NULL DEFAULT (''), 
    [DocumentName] VARCHAR(100) NOT NULL DEFAULT (''), 
    [BrandID] VARCHAR(8) NOT NULL DEFAULT (''), 
    CONSTRAINT [PK_FirstDyelot] PRIMARY KEY CLUSTERED ([TestDocFactoryGroup] ASC, [Refno] ASC, [SuppID] ASC, [ColorID] ASC, [SeasonSCIID] ASC)
);





