CREATE TABLE [dbo].[MonitorBoardParameter] (
    [MorningShiftStart] TIME (7)      NULL,
    [MorningShiftEnd]   TIME (7)      NULL,
    [NightShiftStart]   TIME (7)      NULL,
    [NightShiftEnd]     TIME (7)      NULL,
    [IntervalPerPage]   INT           NULL,
    [InteralAll]        INT           NULL,
    [FactoryID]         NVARCHAR (8)  NULL,
    [SubProcessID]      NVARCHAR (20) NULL,
    [WIPRange]          INT           NULL
);

