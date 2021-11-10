CREATE TABLE [dbo].[SewingSchedule] (
    [ID]              BIGINT         IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [OrderID]         VARCHAR (13)   CONSTRAINT [DF_SewingSchedule_OrderID] DEFAULT ('') NOT NULL,
    [ComboType]       VARCHAR (1)    CONSTRAINT [DF_SewingSchedule_ComboType] DEFAULT ('') NULL,
    [SewingLineID]    VARCHAR (5)    CONSTRAINT [DF_SewingSchedule_SewingLineID] DEFAULT ('') NULL,
    [AlloQty]         INT            CONSTRAINT [DF_SewingSchedule_AlloQty] DEFAULT ((0)) NULL,
    [Inline]          DATETIME       NULL,
    [Offline]         DATETIME       NULL,
    [MDivisionID]     VARCHAR (8)    CONSTRAINT [DF_SewingSchedule_MDivisionID] DEFAULT ('') NULL,
    [FactoryID]       VARCHAR (8)    CONSTRAINT [DF_SewingSchedule_FactoryID] DEFAULT ('') NULL,
    [Sewer]           INT            CONSTRAINT [DF_SewingSchedule_Sewer] DEFAULT ((0)) NULL,
    [TotalSewingTime] INT            CONSTRAINT [DF_SewingSchedule_TotalSewingTime] DEFAULT ((0)) NULL,
    [MaxEff]          NUMERIC (5, 2) CONSTRAINT [DF_SewingSchedule_MaxEff] DEFAULT ((0)) NULL,
    [StandardOutput]  INT            CONSTRAINT [DF_SewingSchedule_StandardOutput] DEFAULT ((0)) NULL,
    [WorkDay]         SMALLINT       CONSTRAINT [DF_SewingSchedule_WorkDay] DEFAULT ((0)) NULL,
    [WorkHour]        NUMERIC (8, 3) CONSTRAINT [DF_SewingSchedule_WorkHour] DEFAULT ((0)) NULL,
    [APSNo]           INT            CONSTRAINT [DF_SewingSchedule_APSNo] DEFAULT ((0)) NULL,
    [OrderFinished]   BIT            CONSTRAINT [DF_SewingSchedule_OrderFinished] DEFAULT ((0)) NULL,
    [AddName]         VARCHAR (10)   CONSTRAINT [DF_SewingSchedule_AddName] DEFAULT ('') NULL,
    [AddDate]         DATETIME       NULL,
    [EditName]        VARCHAR (10)   CONSTRAINT [DF_SewingSchedule_EditName] DEFAULT ('') NULL,
    [EditDate]        DATETIME       NULL,
    [LearnCurveID]    INT            NULL,
    [OriEff]          NUMERIC (5, 2) NULL,
    [SewLineEff]      NUMERIC (5, 2) NULL,
    [LNCSERIALNumber] INT            NULL,
    [SwitchTime]      INT            DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_SewingSchedule] PRIMARY KEY CLUSTERED ([ID] ASC)
);














GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Sewing Schedule', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule', @level2type = N'COLUMN', @level2name = N'OrderID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組合型態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule', @level2type = N'COLUMN', @level2name = N'ComboType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'車縫線', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule', @level2type = N'COLUMN', @level2name = N'SewingLineID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'生產數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule', @level2type = N'COLUMN', @level2name = N'AlloQty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'上線日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule', @level2type = N'COLUMN', @level2name = N'Inline';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'下線日', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule', @level2type = N'COLUMN', @level2name = N'Offline';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'車縫人數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule', @level2type = N'COLUMN', @level2name = N'Sewer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總秒數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule', @level2type = N'COLUMN', @level2name = N'TotalSewingTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Max Efficiency', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule', @level2type = N'COLUMN', @level2name = N'MaxEff';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Standard Output', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule', @level2type = N'COLUMN', @level2name = N'StandardOutput';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'生產天數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule', @level2type = N'COLUMN', @level2name = N'WorkDay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工作時數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule', @level2type = N'COLUMN', @level2name = N'WorkHour';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'APS系統Sewing Schedule的ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule', @level2type = N'COLUMN', @level2name = N'APSNo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Order Shipment Finished', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule', @level2type = N'COLUMN', @level2name = N'OrderFinished';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Manufacturing Division ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SewingSchedule', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
CREATE NONCLUSTERED INDEX [<offline, sysname,>]
    ON [dbo].[SewingSchedule]([OrderID] ASC, [MDivisionID] ASC, [Inline] ASC)
    INCLUDE([Offline]);


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [dbo].[SewingSchedule]([MDivisionID] ASC, [Inline] ASC, [Offline] ASC)
    INCLUDE([OrderID]);


GO
CREATE NONCLUSTERED INDEX [OrderID_ComboType]
    ON [dbo].[SewingSchedule]([OrderID] ASC, [ComboType] ASC);


GO
CREATE NONCLUSTERED INDEX [Index_Offline]
    ON [dbo].[SewingSchedule]([Offline] ASC)
    INCLUDE([OrderID]);


GO
CREATE NONCLUSTERED INDEX [APSNoforP_SewingLineSchedule]
    ON [dbo].[SewingSchedule]([APSNo] ASC)
    INCLUDE([OrderID]);

