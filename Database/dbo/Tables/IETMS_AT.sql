CREATE TABLE [dbo].[IETMS_AT]
(
	[IETMSUkey]                 bigint          CONSTRAINT [DF_IETMS_AT_IETMSUkey]   DEFAULT ((0))   NOT NULL, 
    [CodeFrom]                  VARCHAR(20)     CONSTRAINT [DF_IETMS_AT_CodeFrom]   DEFAULT ((''))  NOT NULL, 
    [Pattern_GL_ArtworkUkey]    VARCHAR(100)    CONSTRAINT [DF_IETMS_AT_Pattern_GL_ArtworkUkey]   DEFAULT ((''))  NOT NULL, 
    [Component]                 VARCHAR(100)    CONSTRAINT [DF_IETMS_AT_Component]   DEFAULT ((''))  NOT NULL, 
    [PieceOfSeamer]             INT             CONSTRAINT [DF_IETMS_AT_PieceOfSeamer]   DEFAULT ((0))   NOT NULL, 
    [PieceOfGarment]            INT             CONSTRAINT [DF_IETMS_AT_PieceOfGarment]   DEFAULT ((0))   NOT NULL, 
    [IsQuilting]                BIT             CONSTRAINT [DF_IETMS_AT_IsQuilting]   DEFAULT ((0))   NOT NULL, 
    [RPM]                       VARCHAR(20)     CONSTRAINT [DF_IETMS_AT_RPM]   DEFAULT ((''))  NOT NULL, 
    [RPMValue]                  NUMERIC(6, 3)   CONSTRAINT [DF_IETMS_AT_RPMValue]   DEFAULT ((0))   NOT NULL, 
    [SewingLength]              NUMERIC(7, 2)   CONSTRAINT [DF_IETMS_AT_SewingLength]   DEFAULT ((0))   NOT NULL, 
    [SewingLine]                INT             CONSTRAINT [DF_IETMS_AT_SewingLine]   DEFAULT ((0))   NOT NULL, 
    [LaserSpeed]                VARCHAR(20)     CONSTRAINT [DF_IETMS_AT_LaserSpeed]   DEFAULT ((''))  NOT NULL, 
    [LaserSpeedValue]           NUMERIC(6, 3)   CONSTRAINT [DF_IETMS_AT_LaserSpeedValue]   DEFAULT ((0))   NOT NULL, 
    [LaserLength]               NUMERIC(7, 2)   CONSTRAINT [DF_IETMS_AT_LaserLength]   DEFAULT ((0))   NOT NULL, 
    [LaserLine]                 INT             CONSTRAINT [DF_IETMS_AT_LaserLine]   DEFAULT ((0))   NOT NULL, 
    [MM2AT]                     NUMERIC(7, 4)   CONSTRAINT [DF_IETMS_AT_MM2AT]   DEFAULT ((0))   NOT NULL, 
    [AT]                        NUMERIC(7, 4)   CONSTRAINT [DF_IETMS_AT_AT]   DEFAULT ((0))   NOT NULL, 
    [OperationID]               VARCHAR(200)    CONSTRAINT [DF_IETMS_AT_OperationID]   DEFAULT ((''))  NOT NULL, 
    [PieceOfSeamerEdited]       INT             CONSTRAINT [DF_IETMS_AT_PieceOfSeamerEdited] DEFAULT ((0))   NOT NULL, 
    [RPMEdited] VARCHAR(20) CONSTRAINT [DF_IESelectCode_RPMEdited] DEFAULT ('') NOT NULL,
    [LaserSpeedEdited] VARCHAR(20) CONSTRAINT [DF_IESelectCode_LaserSpeedEdited] DEFAULT ('') NOT NULL,
    [AddName]                   VARCHAR(10)     CONSTRAINT [DF_IETMS_AT_AddName]   DEFAULT ((''))  NOT NULL, 
    [AddDate]                   DATETIME        CONSTRAINT [DF_IETMS_AT_AddDate]                   NULL, 
    [EditName]                  VARCHAR(10)     CONSTRAINT [DF_IETMS_AT_EditName]   DEFAULT ((''))  NOT NULL, 
    [EditDate]                  DATETIME        CONSTRAINT [DF_IETMS_AT_EditDate]                   NULL, 
    CONSTRAINT [PK_IETMS_AT] PRIMARY KEY ([IETMSUkey],[CodeFrom])  
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'IETMS Ukey',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'IETMSUkey'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'AT的CodeFrom',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'CodeFrom'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Pattern_GL_Artwork.Ukey(多筆逗號串起)',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'Pattern_GL_ArtworkUkey'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'IESelectCode.Type = ''00004''',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'Component'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Piece Of Seamer',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'PieceOfSeamer'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Piece Of Garment',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'PieceOfGarment'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Is Quilting',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'IsQuilting'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'IESelectCode.Type = ''00016''',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'RPM'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'RPM Value',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'RPMValue'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Sewing Length',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'SewingLength'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Sewing Line',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'SewingLine'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'IESelectCode.Type = ''00017''',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'LaserSpeed'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Laser Speed Value',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'LaserSpeedValue'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Laser Length',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'LaserLength'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Laser Line',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'LaserLine'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'MM2 AT',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'MM2AT'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'AT',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'AT'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工段',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'OperationID'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'AddName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'新增時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'AddDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最後修改人員',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'EditName'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'最後修改時間',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'EditDate'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Piece Of Seamer',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'PieceOfSeamerEdited'
    GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'IESelectCode.Type = ''00016''',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'RPMEdited'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'IESelectCode.Type = ''00017''',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_AT',
    @level2type = N'COLUMN',
    @level2name = N'LaserSpeedEdited'