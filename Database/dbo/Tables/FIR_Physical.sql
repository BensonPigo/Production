CREATE TABLE [dbo].[FIR_Physical] (
    [ID]               BIGINT         CONSTRAINT [DF_FIR_Physical_ID] DEFAULT ((0)) NOT NULL,
    [Roll]             VARCHAR (8)    CONSTRAINT [DF_FIR_Physical_Roll] DEFAULT ('') NULL,
    [Dyelot]           VARCHAR (8)    CONSTRAINT [DF_FIR_Physical_Dyelot] DEFAULT ('') NULL,
    [TicketYds]        NUMERIC (8, 2) CONSTRAINT [DF_FIR_Physical_TicketYds] DEFAULT ((0)) NOT NULL,
    [ActualYds]        NUMERIC (8, 2) CONSTRAINT [DF_FIR_Physical_ActualYds] DEFAULT ((0)) NOT NULL,
    [FullWidth]        NUMERIC (5, 2) CONSTRAINT [DF_FIR_Physical_FullWidth] DEFAULT ((0)) NOT NULL,
    [ActualWidth]      NUMERIC (5, 2) CONSTRAINT [DF_FIR_Physical_ActualWidth] DEFAULT ((0)) NOT NULL,
    [TotalPoint]       NUMERIC (6)    CONSTRAINT [DF_FIR_Physical_TotalPoint] DEFAULT ((0)) NULL,
    [PointRate]        NUMERIC (6, 2) CONSTRAINT [DF_FIR_Physical_PointRate] DEFAULT ((0)) NULL,
    [Grade]            VARCHAR (1)    CONSTRAINT [DF_FIR_Physical_Grade] DEFAULT ('') NULL,
    [Result]           VARCHAR (5)    CONSTRAINT [DF_FIR_Physical_Result] DEFAULT ('') NULL,
    [Remark]           NVARCHAR (60)  CONSTRAINT [DF_FIR_Physical_Remark] DEFAULT ('') NULL,
    [InspDate]         DATE           NULL,
    [Inspector]        VARCHAR (10)   CONSTRAINT [DF_FIR_Physical_Inspector] DEFAULT ('') NULL,
    [DetailUkey]       BIGINT         IDENTITY (1, 1) NOT NULL,
    [AddName]          VARCHAR (10)   CONSTRAINT [DF_FIR_Physical_AddName] DEFAULT ('') NULL,
    [AddDate]          DATETIME       NULL,
    [EditName]         VARCHAR (10)   CONSTRAINT [DF_FIR_Physical_EditName] DEFAULT ('') NULL,
    [EditDate]         DATETIME       NULL,
    [Moisture]         BIT            CONSTRAINT [DF_FIR_Physical_Moisture] DEFAULT ((0)) NULL,
    [QCTime]           INT            CONSTRAINT [DF_FIR_Physical_QCTime] DEFAULT ((0)) NOT NULL,
    [QCStopQty]        TINYINT        CONSTRAINT [DF_FIR_Physical_QCStopQty] DEFAULT ((0)) NOT NULL,
    [IsQMS]            BIT            DEFAULT ((0)) NULL,
    [Issue_DetailUkey] BIGINT         DEFAULT ((0)) NOT NULL,
    [TransactionID]    VARCHAR (30)   NULL,
    CONSTRAINT [PK_FIR_Physical] PRIMARY KEY CLUSTERED ([DetailUkey] ASC)
);


















GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fabric Inspection Report-Physical Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'帳面登記碼長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical', @level2type = N'COLUMN', @level2name = N'TicketYds';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際檢驗碼長', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical', @level2type = N'COLUMN', @level2name = N'ActualYds';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'全幅寬', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical', @level2type = N'COLUMN', @level2name = N'FullWidth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'實際幅寬', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical', @level2type = N'COLUMN', @level2name = N'ActualWidth';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總瑕疵點數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical', @level2type = N'COLUMN', @level2name = N'TotalPoint';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵比率', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical', @level2type = N'COLUMN', @level2name = N'PointRate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'等級', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical', @level2type = N'COLUMN', @level2name = N'Grade';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'結果', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical', @level2type = N'COLUMN', @level2name = N'Result';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical', @level2type = N'COLUMN', @level2name = N'InspDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical', @level2type = N'COLUMN', @level2name = N'Inspector';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Detail Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical', @level2type = N'COLUMN', @level2name = N'DetailUkey';


GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
CREATE NONCLUSTERED INDEX [<Name of Missing Index, sysname,>]
    ON [dbo].[FIR_Physical]([ID] ASC);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'QC驗布時間(秒)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical', @level2type = N'COLUMN', @level2name = N'QCTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否為QMS操作', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical', @level2type = N'COLUMN', @level2name = N'IsQMS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Issue_Detail Ukey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Physical', @level2type = N'COLUMN', @level2name = N'Issue_DetailUkey';

