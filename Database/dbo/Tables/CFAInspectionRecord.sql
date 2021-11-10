

CREATE TABLE [dbo].[CFAInspectionRecord] (
    [ID]                     VARCHAR (13)   NOT NULL,
    [AuditDate]              DATE           NULL,
    [FactoryID]              VARCHAR (8)    DEFAULT ('') NOT NULL,
    [MDivisionid]            VARCHAR (8)    DEFAULT ('') NOT NULL,
    [SewingLineID]           VARCHAR (100)  DEFAULT ('') NOT NULL,
    [Team]                   VARCHAR (1)    DEFAULT ('') NOT NULL,
    [Shift]                  VARCHAR (1)    DEFAULT ('') NOT NULL,
    [Stage]                  VARCHAR (10)   DEFAULT ('') NOT NULL,
    [InspectQty]             NUMERIC (7)    DEFAULT ((0)) NOT NULL,
    [DefectQty]              NUMERIC (7)    DEFAULT ((0)) NOT NULL,
    [ClogReceivedPercentage] NUMERIC (3)    DEFAULT ((0)) NOT NULL,
    [Result]                 VARCHAR (16)   DEFAULT ('') NOT NULL,
    [CFA]                    VARCHAR (10)   DEFAULT ('') NOT NULL,
    [Status]                 VARCHAR (15)   DEFAULT ('') NOT NULL,
    [Remark]                 NVARCHAR (500) DEFAULT ('') NOT NULL,
    [AddName]                VARCHAR (10)   DEFAULT ('') NOT NULL,
    [AddDate]                DATETIME       NULL,
    [EditName]               VARCHAR (10)   DEFAULT ('') NOT NULL,
    [EditDate]               DATETIME       NULL,
    [IsCombinePO]            BIT            CONSTRAINT [DF_CFAInspectionRecord_IsCombinePO] DEFAULT ((0)) NOT NULL,
    [FirstInspection]        BIT            CONSTRAINT [DF_CFAInspectionRecord_FirstInspection] DEFAULT ((0)) NOT NULL,
    [IsImportFromMES]        BIT            DEFAULT ((0)) NULL,
    CONSTRAINT [PK_CFAInspectionRecord] PRIMARY KEY CLUSTERED ([ID] ASC)
);

 
GO
;



GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO



EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�ӳ�کҦ����Ƚc�O�_�O�Ĥ@������ Stagger', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CFAInspectionRecord', @level2type = N'COLUMN', @level2name = N'FirstInspection';
GO
EXECUTE sp_addextendedproperty @name = N'IsImportFromMES', @value = N'是否為從MES轉入的資料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CFAInspectionRecord', @level2type = N'COLUMN', @level2name = N'IsImportFromMES';

