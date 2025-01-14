CREATE TABLE [dbo].[SpreadingNo] (
    [ID]          VARCHAR (5)   NOT NULL,
    [MDivisionid] VARCHAR (8)   NULL,
    [Description] NVARCHAR (60) NULL,
    [Junk]        BIT           CONSTRAINT [DF_SpreadingNo_Junk] DEFAULT ((0)) NULL,
    [CutCellID]   VARCHAR (2)   NULL,
    [AddName]     VARCHAR (10)  NULL,
    [AddDate]     DATETIME      NULL,
    [EditName]    VARCHAR (10)  NULL,
    [EditDate]    DATETIME      NULL,
    [FactoryID]   VARCHAR (8)   CONSTRAINT [DF_SpreadingNo_FactoryID] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_SpreadingNo] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpreadingNo', @level2type = N'COLUMN', @level2name = N'FactoryID';

