CREATE TABLE [dbo].[FirstDyelot] (
    [Consignee]      VARCHAR (8)  NOT NULL,
    [Refno]          VARCHAR (20) NOT NULL,
    [SuppID]         VARCHAR (6)  CONSTRAINT [DF_FirstDyelot_SuppID] DEFAULT ('') NOT NULL,
    [ColorID]        VARCHAR (6)  CONSTRAINT [DF_FirstDyelot_ColorID] DEFAULT ('') NOT NULL,
    [SeasonSCIID]    VARCHAR (8)  NOT NULL,
    [Period]         INT          CONSTRAINT [DF_FirstDyelot_Period] DEFAULT ((0)) NOT NULL,
    [FirstDyelot]    DATE         CONSTRAINT [DF_FirstDyelot_FirstDyelot] DEFAULT ('') NOT NULL,
    [TPEFirstDyelot] DATE         NULL,
    [EditName]       VARCHAR (10) NULL,
    [EditDate]       DATETIME     NOT NULL,
    CONSTRAINT [PK_FirstDyelot] PRIMARY KEY CLUSTERED ([Consignee] ASC, [Refno] ASC, [SuppID] ASC, [ColorID] ASC, [SeasonSCIID] ASC)
);





