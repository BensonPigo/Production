CREATE TABLE [dbo].[SpreadingSchedule_Detail] (
    [SpreadingScheduleUkey] BIGINT      NOT NULL,
    [CutRef]                VARCHAR (6) CONSTRAINT [DF_SpreadingSchedule_Detail_CutRef] DEFAULT ('') NOT NULL,
    [SpreadingSchdlSeq]     NUMERIC (3) NULL,
    [IsAGVArrived]          BIT         CONSTRAINT [DF_SpreadingSchedule_Detail_IsAGVArrived] DEFAULT ((0)) NOT NULL,
    [IsSuspend]             BIT         CONSTRAINT [DF_SpreadingSchedule_Detail_IsSuspend] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SpreadingSchedule_Detail] PRIMARY KEY CLUSTERED ([SpreadingScheduleUkey] ASC, [CutRef] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'該seq暫停', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpreadingSchedule_Detail', @level2type = N'COLUMN', @level2name = N'IsSuspend';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'AGV是否已送達', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpreadingSchedule_Detail', @level2type = N'COLUMN', @level2name = N'IsAGVArrived';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Spreading拉布順序', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpreadingSchedule_Detail', @level2type = N'COLUMN', @level2name = N'SpreadingSchdlSeq';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁次', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpreadingSchedule_Detail', @level2type = N'COLUMN', @level2name = N'CutRef';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpreadingSchedule_Detail', @level2type = N'COLUMN', @level2name = N'SpreadingScheduleUkey';

