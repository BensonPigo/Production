CREATE TABLE [dbo].[FirstDyelot] (
    [SCIRefno]       VARCHAR (30) NOT NULL,
    [SuppID]         VARCHAR (6)  CONSTRAINT [DF_FirstDyelot_SuppID] DEFAULT ('') NOT NULL,
    [ColorID]        VARCHAR (6)  CONSTRAINT [DF_FirstDyelot_ColorID] DEFAULT ('') NOT NULL,
    [FirstDyelot]    DATE         CONSTRAINT [DF_FirstDyelot_FirstDyelot] DEFAULT ('') NOT NULL,
    [TPEFirstDyelot] DATE         NULL,
    [EditName]       VARCHAR (10) NULL,
    [EditDate]       DATETIME     NOT NULL,
    CONSTRAINT [PK_FirstDyelot] PRIMARY KEY CLUSTERED ([SCIRefno] ASC)
);

