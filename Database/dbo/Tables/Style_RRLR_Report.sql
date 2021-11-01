CREATE TABLE [dbo].[Style_RRLRReport] (
    [StyleUkey]      BIGINT         NOT NULL,
    [SuppID]         VARCHAR (6)    NOT NULL,
    [Refno]          VARCHAR (20)   NOT NULL,
    [Material]       VARCHAR (50)   CONSTRAINT [DF_Style_RRLRReport_Material] DEFAULT ('') NOT NULL,
    [ColorID]        VARCHAR (6)    NOT NULL,
    [LabDipStatus]   VARCHAR (50)   CONSTRAINT [DF_Style_RRLRReport_LabDipStatus] DEFAULT ('') NOT NULL,
    [RR]             BIT            CONSTRAINT [DF_Style_RRLRReport_RR] DEFAULT ((0)) NOT NULL,
    [RRRemark]       NVARCHAR (MAX) CONSTRAINT [DF_Style_RRLRReport_RRRemark] DEFAULT ('') NOT NULL,
    [LifecycleState] VARCHAR (50)   CONSTRAINT [DF_Style_RRLRReport_LifecycleState] DEFAULT ('') NOT NULL,
    [LR]             BIT            CONSTRAINT [DF_Style_RRLRReport_LR] DEFAULT ((0)) NOT NULL,
    [AddName]        VARCHAR (10)   CONSTRAINT [DF_Style_RRLRReport_AddName] DEFAULT ('') NOT NULL,
    [AddDate]        DATETIME       NULL,
    CONSTRAINT [PK_Style_RRLRReport] PRIMARY KEY CLUSTERED ([StyleUkey] ASC, [SuppID] ASC, [Refno] ASC, [ColorID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_RRLRReport', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_RRLRReport', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Limited release', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_RRLRReport', @level2type = N'COLUMN', @level2name = N'LR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Restricted release', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_RRLRReport', @level2type = N'COLUMN', @level2name = N'RR';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_RRLRReport', @level2type = N'COLUMN', @level2name = N'ColorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料屬於 Fabirc / Trim', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_RRLRReport', @level2type = N'COLUMN', @level2name = N'Material';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'物料', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_RRLRReport', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'供應商', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Style_RRLRReport', @level2type = N'COLUMN', @level2name = N'SuppID';

