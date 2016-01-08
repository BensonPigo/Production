CREATE TABLE [dbo].[ThreadAdjust] (
    [ID]                VARCHAR (13)   CONSTRAINT [DF_ThreadAdjust_ID] DEFAULT ('') NOT NULL,
    [CDate]             DATE           NULL,
    [Status]            VARCHAR (15)   NULL,
    [mDivisionid]       VARCHAR (8)    CONSTRAINT [DF_ThreadAdjust_mDivisionid] DEFAULT ('') NOT NULL,
    [ThreadInventoryid] VARCHAR (13)   NULL,
    [AddName]           VARCHAR (10)   NULL,
    [AddDate]           DATETIME       NULL,
    [EditName]          VARCHAR (10)   NULL,
    [EditDate]          DATETIME       NULL,
    [Remark]            NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_ThreadAdjust] PRIMARY KEY CLUSTERED ([ID] ASC)
);



