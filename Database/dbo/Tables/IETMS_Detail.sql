CREATE TABLE [dbo].[IETMS_Detail] (
    [IETMSUkey]   BIGINT         CONSTRAINT [DF_IETMS_Detail_IETMSUkey] DEFAULT ((0)) NOT NULL,
    [SEQ]         VARCHAR (4)    CONSTRAINT [DF_IETMS_Detail_SEQ] DEFAULT ('') NOT NULL,
    [Location]    VARCHAR (1)    CONSTRAINT [DF_IETMS_Detail_Location] DEFAULT ('') NULL,
    [OperationID] VARCHAR (20)   CONSTRAINT [DF_IETMS_Detail_OperationID] DEFAULT ('') NULL,
    [Mold]        NVARCHAR (65)  CONSTRAINT [DF_IETMS_Detail_Mold] DEFAULT ('') NULL,
    [Annotation]  NVARCHAR (MAX) CONSTRAINT [DF_IETMS_Detail_Annotation] DEFAULT ('') NULL,
    [Frequency]   NUMERIC (7, 2) CONSTRAINT [DF_IETMS_Detail_Frequency] DEFAULT ((0)) NULL,
    [SMV]         NUMERIC (12, 4) CONSTRAINT [DF_IETMS_Detail_SMV] DEFAULT ((0)) NULL,
    [SeamLength]  NUMERIC (12, 2) NULL DEFAULT ((0)),
    [UKey] BIGINT NOT NULL DEFAULT ((0)), 
    [MtlFactorID] VARCHAR(3) NULL, 
    [MtlFactorRate] NUMERIC(8, 2) NULL, 
    CONSTRAINT [PK_IETMS_Detail] PRIMARY KEY CLUSTERED ([UKey])
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