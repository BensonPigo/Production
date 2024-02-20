CREATE TABLE [dbo].[IETMS_Detail] (
    [IETMSUkey]     BIGINT          CONSTRAINT [DF_IETMS_Detail_IETMSUkey] DEFAULT ((0)) NOT NULL,
    [SEQ]           VARCHAR (4)     CONSTRAINT [DF_IETMS_Detail_SEQ] DEFAULT ('') NOT NULL,
    [Location]      VARCHAR (1)     CONSTRAINT [DF_IETMS_Detail_Location] DEFAULT ('') NOT NULL,
    [OperationID]   VARCHAR (20)    CONSTRAINT [DF_IETMS_Detail_OperationID] DEFAULT ('') NOT NULL,
    [Mold]          NVARCHAR (65)   CONSTRAINT [DF_IETMS_Detail_Mold] DEFAULT ('') NOT NULL,
    [Annotation]    NVARCHAR (MAX)  CONSTRAINT [DF_IETMS_Detail_Annotation] DEFAULT ('') NOT NULL,
    [Frequency]     DECIMAL (7, 2)  CONSTRAINT [DF_IETMS_Detail_Frequency] DEFAULT ((0)) NOT NULL,
    [SMV]           DECIMAL (12, 4) CONSTRAINT [DF_IETMS_Detail_SMV] DEFAULT ((0)) NOT NULL,
    [SeamLength]    DECIMAL (12, 2) CONSTRAINT [DF_IETMS_Detail_SeamLength] DEFAULT ((0)) NOT NULL,
    [UKey]          BIGINT          DEFAULT ((0)) NOT NULL,
    [MtlFactorID]   VARCHAR (3)     CONSTRAINT [DF_IETMS_Detail_MtlFactorID] DEFAULT ('') NOT NULL,
    [MtlFactorRate] DECIMAL (8, 2)  CONSTRAINT [DF_IETMS_Detail_MtlFactorRate] DEFAULT ((0)) NOT NULL,
    [IsPPA]         BIT             CONSTRAINT [DF_IETMS_Detail_IsPPA] DEFAULT ((0)) NOT NULL,
    [PPA]           VARCHAR (2)     CONSTRAINT [DF_IETMS_Detail_PPA] DEFAULT ('') NOT NULL,
    [ProOperationID] VARCHAR(20) CONSTRAINT [DF_IETMS_Detail_ProOperationID] DEFAULT ('') not NULL,
    [CodeFrom] VARCHAR(20) CONSTRAINT [DF_IETMS_Detail_CodeFrom] DEFAULT ('') not NULL,
    [Draft]                     NVARCHAR(100)   CONSTRAINT [DF_IETMS_Detail_Draft]   DEFAULT ('') NOT NULL, 
    [Pattern_GL_ArtworkUkey]    VARCHAR(100)    CONSTRAINT [DF_IETMS_Detail_Pattern_GL_ArtworkUkey]   DEFAULT ('') NOT NULL, 
    CONSTRAINT [PK_IETMS_Detail] PRIMARY KEY CLUSTERED ([UKey] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'IE Operation Detail 明細檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IETMS_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'連結', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IETMS_Detail', @level2type = N'COLUMN', @level2name = N'IETMSUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IETMS_Detail', @level2type = N'COLUMN', @level2name = N'SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IETMS_Detail', @level2type = N'COLUMN', @level2name = N'Location';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'工段', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IETMS_Detail', @level2type = N'COLUMN', @level2name = N'OperationID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'模具', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IETMS_Detail', @level2type = N'COLUMN', @level2name = N'Mold';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'註解', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IETMS_Detail', @level2type = N'COLUMN', @level2name = N'Annotation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'次數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IETMS_Detail', @level2type = N'COLUMN', @level2name = N'Frequency';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'IESMV', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'IETMS_Detail', @level2type = N'COLUMN', @level2name = N'SMV';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'UKey',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_Detail',
    @level2type = N'COLUMN',
    @level2name = N'UKey'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'SeamLength',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_Detail',
    @level2type = N'COLUMN',
    @level2name = N'SeamLength'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'工段前置中文說明',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Draft'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Pattern_GL_Artwork Ukey',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_Detail',
    @level2type = N'COLUMN',
    @level2name = N'Pattern_GL_ArtworkUkey'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Operation Code來源處',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'IETMS_Detail',
    @level2type = N'COLUMN',
    @level2name = N'CodeFrom'