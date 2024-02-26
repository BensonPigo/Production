CREATE TABLE [dbo].[IETMS_AT_Detail]
(
	[IETMSUkey]         BIGINT           CONSTRAINT [DF_IETMS_AT_Detail_IETMSUkey]     DEFAULT ((0))   NOT NULL, 
    [CodeFrom]          VARCHAR(20)      CONSTRAINT [DF_IETMS_AT_Detail_CodeFrom]     DEFAULT ((''))  NOT NULL, 
    [IESelectCodeType]  VARCHAR(5)       CONSTRAINT [DF_IETMS_AT_Detail_IESelectCodeType]     DEFAULT ((''))  NOT NULL, 
    [IESelectCodeID]    VARCHAR(20)      CONSTRAINT [DF_IETMS_AT_Detail_IESelectCodeID]     DEFAULT ((''))  NOT NULL, 
    [Number]            INT              CONSTRAINT [DF_IETMS_AT_Detail_Number]     DEFAULT ((0))   NOT NULL, 
    [Value]             NUMERIC(6, 3)    CONSTRAINT [DF_IETMS_AT_Detail_Value]     DEFAULT ((0))   NOT NULL, 
    [Remark]            NVARCHAR(MAX)    CONSTRAINT [DF_IETMS_AT_Detail_Remark]     DEFAULT ((''))  NOT NULL, 
    [AddName]           VARCHAR(10)      CONSTRAINT [DF_IETMS_AT_Detail_AddName]     DEFAULT ((''))  NOT NULL, 
    [AddDate]           DATETIME         CONSTRAINT [DF_IETMS_AT_Detail_AddDate]                     NULL, 
    [EditName]          VARCHAR(10)      CONSTRAINT [DF_IETMS_AT_Detail_EditName]     DEFAULT ((''))  NOT NULL, 
    [EditDate]          DATETIME         CONSTRAINT [DF_IETMS_AT_Detail_EditDate]                     NULL, 
    CONSTRAINT [PK_IETMS_AT_Detail] PRIMARY KEY ([IETMSUkey],[CodeFrom],[IESelectCodeType],[IESelectCodeID]) 
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'IETMS Ukey',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT_Detail',
    @level2type = N'COLUMN',
    @level2name = N'IETMSUkey'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'CodeFrom',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT_Detail',
    @level2type = N'COLUMN',
    @level2name = N'CodeFrom'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Item',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT_Detail',
    @level2type = N'COLUMN',
    @level2name = N'IESelectCodeType'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Desc',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT_Detail',
    @level2type = N'COLUMN',
    @level2name = N'IESelectCodeID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Number',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Number'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Value',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Value'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Remark'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT_Detail',
    @level2type = N'COLUMN',
    @level2name = N'AddName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT_Detail',
    @level2type = N'COLUMN',
    @level2name = N'AddDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最後修改人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT_Detail',
    @level2type = N'COLUMN',
    @level2name = N'EditName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最後修改時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT_Detail',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'