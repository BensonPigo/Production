CREATE TABLE [dbo].[Factory_TMS] (
    [ID]            VARCHAR (8)  CONSTRAINT [DF_Factory_TMS_ID] DEFAULT ('') NOT NULL,
    [Year]          VARCHAR (4)  CONSTRAINT [DF_Factory_TMS_Year] DEFAULT ('') NOT NULL,
    [ArtworkTypeID] VARCHAR (20) CONSTRAINT [DF_Factory_TMS_ArtworkTypeID] DEFAULT ('') NOT NULL,
    [Month]         VARCHAR (2)  CONSTRAINT [DF_Factory_TMS_Month] DEFAULT ('') NOT NULL,
    [TMS]           NUMERIC(8)          CONSTRAINT [DF_Factory_TMS_TMS] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Factory_TMS] PRIMARY KEY CLUSTERED ([ID] ASC, [Year] ASC, [ArtworkTypeID] ASC, [Month] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠標準產能明細表', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_TMS';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工廠代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_TMS', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'年度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_TMS', @level2type = N'COLUMN', @level2name = N'Year';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作工代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_TMS', @level2type = N'COLUMN', @level2name = N'ArtworkTypeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'月份', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_TMS', @level2type = N'COLUMN', @level2name = N'Month';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'作工的TMS/Min', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Factory_TMS', @level2type = N'COLUMN', @level2name = N'TMS';

