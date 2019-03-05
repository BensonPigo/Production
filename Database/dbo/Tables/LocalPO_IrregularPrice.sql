CREATE TABLE [dbo].[LocalPO_IrregularPrice] (
    [POID]           VARCHAR (13)    NOT NULL,
    [Category]       VARCHAR (20)    NOT NULL,
    [POPrice]        NUMERIC (16, 4) NULL,
    [StandardPrice]  NUMERIC (16, 4) NULL,
    [SubconReasonID] VARCHAR (5)     NOT NULL,
    [AddDate]        DATETIME        NULL,
    [AddName]        VARCHAR (10)    NULL,
    [EditDate]       DATETIME        NULL,
    [EditName]       VARCHAR (10)    NULL,
    CONSTRAINT [PK_LocalPO_IrregularPrice] PRIMARY KEY CLUSTERED ([POID] ASC, [Category] ASC)
);

