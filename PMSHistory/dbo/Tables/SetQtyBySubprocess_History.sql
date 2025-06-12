CREATE TABLE [dbo].[SetQtyBySubprocess_History] (
    [OrderID]          VARCHAR (13) CONSTRAINT [DF_SetQtyBySubprocess_History_OrderID] DEFAULT ('') NOT NULL,
    [Article]          VARCHAR (8)  CONSTRAINT [DF_SetQtyBySubprocess_History_Article] DEFAULT ('') NOT NULL,
    [SizeCode]         VARCHAR (8)  CONSTRAINT [DF_SetQtyBySubprocess_History_SizeCode] DEFAULT ('') NOT NULL,
    [PatternPanel]     VARCHAR (2)  CONSTRAINT [DF_SetQtyBySubprocess_History_PatternPanel] DEFAULT ('') NOT NULL,
    [InQtyBySet]       INT          NOT NULL,
    [OutQtyBySet]      INT          NOT NULL,
    [FinishedQtyBySet] INT          NOT NULL,
    [SubprocessID]     VARCHAR (15) CONSTRAINT [DF_SetQtyBySubprocess_History_SubprocessID] DEFAULT ('') NOT NULL,
    [TransferTime]     DATETIME     NOT NULL,
    [AddDate]          DATETIME     NULL,
    CONSTRAINT [PK_SetQtyBySubprocess_History] PRIMARY KEY CLUSTERED ([OrderID] ASC, [Article] ASC, [SizeCode] ASC, [PatternPanel] ASC, [SubprocessID] ASC, [TransferTime] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉入時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SetQtyBySubprocess_History', @level2type = N'COLUMN', @level2name = N'TransferTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'加工段', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SetQtyBySubprocess_History', @level2type = N'COLUMN', @level2name = N'SubprocessID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SetQtyBySubprocess_History', @level2type = N'COLUMN', @level2name = N'PatternPanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'尺寸', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SetQtyBySubprocess_History', @level2type = N'COLUMN', @level2name = N'SizeCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SetQtyBySubprocess_History', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SetQtyBySubprocess_History', @level2type = N'COLUMN', @level2name = N'OrderID';

