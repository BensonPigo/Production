CREATE TABLE [dbo].[MeasurementTranslate] (
    [Ukey]     BIGINT         IDENTITY (1, 1) NOT NULL,
    [DescEN]   VARCHAR (300)  DEFAULT ('') NOT NULL,
    [DescCHS]  NVARCHAR (300) DEFAULT ('') NOT NULL,
    [DescVN]   NVARCHAR (300) DEFAULT ('') NOT NULL,
    [DescKH]   NVARCHAR (300) DEFAULT ('') NOT NULL,
    [AddName]  VARCHAR (10)   DEFAULT ('') NOT NULL,
    [AddDate]  DATETIME       NULL,
    [EditName] VARCHAR (10)   DEFAULT ('') NOT NULL,
    [EditDate] DATETIME       NULL,
    CONSTRAINT [PK_MeasurementTranslate] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Measurement翻譯檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MeasurementTranslate';

