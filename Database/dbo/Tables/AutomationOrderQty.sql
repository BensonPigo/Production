CREATE TABLE [dbo].[AutomationOrderQty] (
    [ID]       VARCHAR (13) CONSTRAINT [DF_AutomationOrderQty_ID] DEFAULT ('') NOT NULL,
    [Article]  VARCHAR (8)  CONSTRAINT [DF_AutomationOrderQty_Article] DEFAULT ('') NOT NULL,
    [SizeCode] VARCHAR (8)  CONSTRAINT [DF_AutomationOrderQty_SizeCode] DEFAULT ('') NOT NULL
    CONSTRAINT [PK_AutomationOrderQty] PRIMARY KEY CLUSTERED ([ID] ASC, [Article] ASC, [SizeCode] ASC)
);


GO