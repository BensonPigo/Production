CREATE TABLE [dbo].[PPASchedule_Detail] (
    [ID]         BIGINT         NOT NULL,
    [OrderID]    VARCHAR (13)   NOT NULL,
    [OutputDate] DATE           NOT NULL,
    [WeekOfYear] INT            NULL,
    [Qty]        INT            NULL,
    [Remark]     NVARCHAR (100) NULL,
    CONSTRAINT [PK_PPASchedule_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [OrderID] ASC, [OutputDate] ASC)
);



