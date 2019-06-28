CREATE TABLE [dbo].[F_F001] (
    [ALLSeq]           NUMERIC (18, 2) NOT NULL,
    [MdivisionSeq]     INT             NOT NULL,
    [Mdivision]        NVARCHAR (3)    NULL,
    [YearMonth]        NVARCHAR (6)    NOT NULL,
    [ProcessSeq]       INT             NOT NULL,
    [Process]          NVARCHAR (200)  NULL,
    [ProcessDetailSeq] INT             NOT NULL,
    [ProcessDetail]    NVARCHAR (200)  NULL,
    [Amount]           NUMERIC (24, 8) NULL,
    [Sub Revenue]      NUMERIC (24, 8) NULL,
    [Sub Expense]      NUMERIC (24, 8) NULL,
    [SubProfit/Loss]   NUMERIC (24, 8) NULL,
    [年月]               DATETIME        NULL,
    CONSTRAINT [PK_ALL] PRIMARY KEY CLUSTERED ([ALLSeq] ASC)
);

