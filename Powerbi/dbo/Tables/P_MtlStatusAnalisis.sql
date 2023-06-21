CREATE TABLE [dbo].[P_MtlStatusAnalisis]
(
    [WK]                    VARCHAR(13)     CONSTRAINT [DF_P_MtlStatusAnalisis_WK]                  DEFAULT ((''))  NOT NULL, 
    [LoadingCountry]        VARCHAR(2)      CONSTRAINT [DF_P_MtlStatusAnalisis_LoadingCountry]      DEFAULT ((''))  NOT NULL,
    [LoadingPort]           VARCHAR(20)     CONSTRAINT [DF_P_MtlStatusAnalisis_LoadingPort]         DEFAULT ((''))  NOT NULL, 
    [Shipmode]              VARCHAR(10)     CONSTRAINT [DF_P_MtlStatusAnalisis_Shipmode]            DEFAULT ((''))  NOT NULL, 
    [Close_Date]            DATE                                                                                    NULL,      
    [ETD]                   DATE                                                                                    NULL, 
    [ETA]                   DATE                                                                                    NULL, 
    [Arrive_WH_Date]        DATE                                                                                    NULL, 
    [KPI_LETA]              DATE                                                                                    NULL, 
    [Prod_LT]               INT             CONSTRAINT [DF_P_MtlStatusAnalisis_Prod_LT]             DEFAULT ((0))   NOT NULL, 
    [WK_Factory]            VARCHAR(8)      CONSTRAINT [DF_P_MtlStatusAnalisis_WK_Factory]          DEFAULT ((''))  NOT NULL,
    [FactoryID]             VARCHAR(8)      CONSTRAINT [DF_P_MtlStatusAnalisis_FactoryID]           DEFAULT ((''))  NOT NULL, 
    [SPNo]                  VARCHAR(13)     CONSTRAINT [DF_P_MtlStatusAnalisis_SPNo]                DEFAULT ((''))  NOT NULL,
    [SEQ]                   VARCHAR(MAX)    CONSTRAINT [DF_P_MtlStatusAnalisis_SEQ]                 DEFAULT ((''))  NOT NULL, 
    [Category]              NVARCHAR(50)    CONSTRAINT [DF_P_MtlStatusAnalisis_Category]            DEFAULT ((''))  NOT NULL, 
    [PF_ETA]                DATE                                                                                    NULL, 
    [SewinLine]             DATE                                                                                    NULL, 
    [BuyerDelivery]         DATE                                                                                    NULL, 
    [SCIDelivery]           DATE                                                                                    NULL, 
    [PO_SMR]                NVARCHAR(69)    CONSTRAINT [DF_P_MtlStatusAnalisis_PO_SMR]              DEFAULT ((''))  NOT NULL,
    [PO_Handle]             NVARCHAR(69)    CONSTRAINT [DF_P_MtlStatusAnalisis_PO_Handle]           DEFAULT ((''))  NOT NULL,
    [SMR]                   NVARCHAR(69)    CONSTRAINT [DF_P_MtlStatusAnalisis_SMR]                 DEFAULT ((''))  NOT NULL,
    [MR]                    NVARCHAR(69)    CONSTRAINT [DF_P_MtlStatusAnalisis_MR]                  DEFAULT ((''))  NOT NULL,
    [PC_Handle]             NVARCHAR(69)    CONSTRAINT [DF_P_MtlStatusAnalisis_PC_Handle]           DEFAULT ((''))  NOT NULL, 
    [Style]                 VARCHAR(15)     CONSTRAINT [DF_P_MtlStatusAnalisis_Style]               DEFAULT ((''))  NOT NULL,
    [SP_List]               VARCHAR(MAX)    CONSTRAINT [DF_P_MtlStatusAnalisis_SP_List]             DEFAULT ((''))  NOT NULL, 
    [PO_Qty]                NUMERIC(10)     CONSTRAINT [DF_P_MtlStatusAnalisis_PO_Qty]              DEFAULT ((0))   NULL, 
    [Project]               VARCHAR(5)      CONSTRAINT [DF_P_MtlStatusAnalisis_Project]             DEFAULT ((''))  NOT NULL, 
    [Early_Ship_Reason]     VARCHAR(MAX)    CONSTRAINT [DF_P_MtlStatusAnalisis_Early_Ship_Reason]   DEFAULT ((''))  NOT NULL, 
    [WK_Handle]             NVARCHAR(69)    CONSTRAINT [DF_P_MtlStatusAnalisis_WK_Handle]           DEFAULT ((''))  NOT NULL, 
    [MTL_Confirm]           VARCHAR(1)      CONSTRAINT [DF_P_MtlStatusAnalisis_MTL_Confirm]         DEFAULT ((''))  NOT NULL, 
    [Duty]                  NVARCHAR(600)   CONSTRAINT [DF_P_MtlStatusAnalisis_Duty]                DEFAULT ((''))  NOT NULL, 
    [PF_Remark]             VARCHAR(MAX)    CONSTRAINT [DF_P_MtlStatusAnalisis_PF_Remark]           DEFAULT ((''))  NOT NULL, 
    [Type]                  VARCHAR(10)     CONSTRAINT [DF_P_MtlStatusAnalisis_Type]                DEFAULT ((''))  NOT NULL, 
    [Ukey]                  BIGINT NULL
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'WK',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'WK'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'LoadingCountry',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'LoadingCountry'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'LoadingPort',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'LoadingPort'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Shipmode',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'Shipmode'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Close_Date',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'Close_Date'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ETD',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'ETD'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'ETA',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'ETA'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Arrive_WH_Date',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'Arrive_WH_Date'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'KPI_LETA',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'KPI_LETA'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Prod_LT',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'Prod_LT'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'WK_Factory',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'WK_Factory'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'FactoryID',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'FactoryID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'SPNo',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'SPNo'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'SEQ',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'SEQ'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Category',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'Category'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'PF_ETA',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'PF_ETA'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Ukey',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'Ukey'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Type',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'Type'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'PF_Remark',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'PF_Remark'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Duty',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'Duty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'MTL_Confirm',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'MTL_Confirm'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'WK_Handle',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'WK_Handle'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Early_Ship_Reason',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'Early_Ship_Reason'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Project',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'Project'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'PO_Qty',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'PO_Qty'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'SP_List',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'SP_List'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Style',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'Style'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'PC_Handle',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'PC_Handle'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'MR',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'MR'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'SMR',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'SMR'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'PO_Handle',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'PO_Handle'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'PO_SMR',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'PO_SMR'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'SCIDelivery',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'SCIDelivery'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'BuyerDelivery',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'BuyerDelivery'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'SewinLine',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'P_MtlStatusAnalisis',
    @level2type = N'COLUMN',
    @level2name = N'SewinLine'