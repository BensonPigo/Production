CREATE TABLE [dbo].[LineMapping_Detail] (
    [ID]            BIGINT         CONSTRAINT [DF_LineMapping_Detail_ID] DEFAULT ((0)) NOT NULL,
    [No]            VARCHAR (4)    CONSTRAINT [DF_LineMapping_Detail_No] DEFAULT ('') NULL,
    [Annotation]    NVARCHAR (MAX) CONSTRAINT [DF_LineMapping_Detail_Annotation] DEFAULT ('') NULL,
    [GSD]           NUMERIC (12, 2) CONSTRAINT [DF_LineMapping_Detail_GSD] DEFAULT ((0)) NULL,
    [TotalGSD]      NUMERIC (12, 2) CONSTRAINT [DF_LineMapping_Detail_TotalGSD] DEFAULT ((0)) NULL,
    [Cycle]         NUMERIC (12, 2) CONSTRAINT [DF_LineMapping_Detail_Cycle] DEFAULT ((0)) NULL,
    [TotalCycle]    NUMERIC (12, 2) CONSTRAINT [DF_LineMapping_Detail_TotalCycle] DEFAULT ((0)) NULL,
    [MachineTypeID] VARCHAR (10)   CONSTRAINT [DF_LineMapping_Detail_MachineTypeID] DEFAULT ('') NULL,
    [OperationID]   VARCHAR (20)   CONSTRAINT [DF_LineMapping_Detail_OperationID] DEFAULT ('') NULL,
    [MoldID]        NVARCHAR (200) CONSTRAINT [DF_LineMapping_Detail_MoldID] DEFAULT ('') NULL,
    [GroupKey]      INT            CONSTRAINT [DF_LineMapping_Detail_GroupKey] DEFAULT ((0)) NOT NULL,
    [New]           BIT            CONSTRAINT [DF_LineMapping_Detail_New] DEFAULT ((0)) NULL,
    [EmployeeID]    VARCHAR (10)   CONSTRAINT [DF_LineMapping_Detail_EmployeeID] DEFAULT ('') NULL,
    [Attachment]    VARCHAR (100)  NULL,
    [Template]      VARCHAR (100)  NULL,
    [OriNO]         VARCHAR (4)    NULL,
    [Ukey]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [ThreadColor]   NVARCHAR    NULL,
    [IsPPA ] BIT NULL, 
    [ActCycle] NUMERIC(12, 2) NULL,  
    [MasterPlusGroup] VARCHAR(4) NOT NULL DEFAULT (''), 
    [IsHide] BIT NULL , 
    [IEReasonLBRNotHit_DetailUkey] BIGINT NULL, 
    [EstCycleTime]          NUMERIC(7, 2)   CONSTRAINT [DF_LineMapping_Detail_EstCycleTime]    DEFAULT ((0))  NOT NULL, 
    [EstTotalCycleTime]     NUMERIC(7, 2)   CONSTRAINT [DF_LineMapping_Detail_EstTotalCycleTime]    DEFAULT ((0))  NOT NULL, 
    [EstOutputHr]           NUMERIC(7, 2)   CONSTRAINT [DF_LineMapping_Detail_EstOutputHr]    DEFAULT ((0))  NOT NULL, 
    [EstLBR]                NUMERIC(7, 2)   CONSTRAINT [DF_LineMapping_Detail_EstLBR]    DEFAULT ((0))  NOT NULL, 
    [Append] BIT NULL DEFAULT ((0)), 
    CONSTRAINT [PK_LineMapping_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);
















GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Line Mapping Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'No';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Annotation', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'Annotation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GSD秒數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'GSD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'GSD總秒數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'TotalGSD';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cycle秒數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'Cycle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cycle總秒數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'TotalCycle';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機台代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'MachineTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工段', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'OperationID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'模具', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'MoldID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'GroupKey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'手動新增', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'New';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'員工編號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'EmployeeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'LineMapping_Detail', @level2type = N'COLUMN', @level2name = N'ID';

GO
CREATE NONCLUSTERED INDEX [IX_LineMapping_Detail_OrgPK] ON [dbo].[LineMapping_Detail]
(
	[ID] ASC,
	[No] ASC,
	[GroupKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]



GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'是否排除No',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMapping_Detail',
    @level2type = N'COLUMN',
    @level2name = N'IsHide'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'IEReasonLBRNotHit_Detail Ukey',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMapping_Detail',
    @level2type = N'COLUMN',
    @level2name = N'IEReasonLBRNotHit_DetailUkey'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'預估的CycleTime',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMapping_Detail',
    @level2type = N'COLUMN',
    @level2name = N'EstCycleTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'預估站台的TotalCycleTime',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMapping_Detail',
    @level2type = N'COLUMN',
    @level2name = N'EstTotalCycleTime'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'預估每小時產量',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMapping_Detail',
    @level2type = N'COLUMN',
    @level2name = N'EstOutputHr'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'預估的LBR',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMapping_Detail',
    @level2type = N'COLUMN',
    @level2name = N'EstLBR'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'手動插入',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'LineMapping_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Append'