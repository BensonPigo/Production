﻿CREATE TABLE [dbo].[FirstDyelot] (
    [Consignee]      VARCHAR (8)  NOT NULL,
    [Refno]          VARCHAR (20) NOT NULL,
    [SuppID]         VARCHAR (6)   NOT NULL,
    [ColorID]        VARCHAR (6)   NOT NULL,
    [SeasonSCIID]    VARCHAR (8)  NOT NULL,
    [Period]         INT          CONSTRAINT [DF_FirstDyelot_Period] DEFAULT ((0)) NOT NULL,
    [FirstDyelot]    DATE          NULL,
    [TPEFirstDyelot] DATE         NULL,
    [EditName]       VARCHAR (10) NOT NULL DEFAULT (''),
    [EditDate]       DATETIME     NULL,
    CONSTRAINT [PK_FirstDyelot] PRIMARY KEY CLUSTERED ([Consignee] ASC, [Refno] ASC, [SuppID] ASC, [ColorID] ASC, [SeasonSCIID] ASC)
);





