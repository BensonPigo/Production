CREATE TABLE [dbo].[CuttingOutputFabricRecord] (
    [Ukey]        BIGINT          IDENTITY (1, 1) NOT NULL,
    [CutRef]      VARCHAR (10)    CONSTRAINT [DF_CuttingOutputFabricRecord_CutRef] DEFAULT ('') NOT NULL,
    [MDivisionId] VARCHAR (8)     CONSTRAINT [DF_CuttingOutputFabricRecord_MDivisionId] DEFAULT ('') NOT NULL,
    [Seq1]        VARCHAR (3)     CONSTRAINT [DF_CuttingOutputFabricRecord_Seq1] DEFAULT ('') NOT NULL,
    [Seq2]        VARCHAR (2)     CONSTRAINT [DF_CuttingOutputFabricRecord_Seq2] DEFAULT ('') NOT NULL,
    [Roll]        VARCHAR (8)     CONSTRAINT [DF_CuttingOutputFabricRecord_Roll] DEFAULT ('') NOT NULL,
    [Dyelot]      VARCHAR (8)     CONSTRAINT [DF_CuttingOutputFabricRecord_Dyelot] DEFAULT ('') NOT NULL,
    [Yardage]     NUMERIC (11, 2) CONSTRAINT [DF_CuttingOutputFabricRecord_Yardage] DEFAULT ((0)) NOT NULL,
    [AddName]     VARCHAR (10)    CONSTRAINT [DF_CuttingOutputFabricRecord_AddName] DEFAULT ('') NULL,
    [AddDate]     DATETIME        NULL,
    [EditName]    VARCHAR (10)    CONSTRAINT [DF_CuttingOutputFabricRecord_EditName] DEFAULT ('') NULL,
    [EditDate]    DATETIME        NULL,
    CONSTRAINT [PK_CuttingOutputFabricRecord] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'碼數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutputFabricRecord', @level2type = N'COLUMN', @level2name = N'Yardage';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutputFabricRecord', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'卷號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutputFabricRecord', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutputFabricRecord', @level2type = N'COLUMN', @level2name = N'Seq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutputFabricRecord', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Work Order Refno#', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutputFabricRecord', @level2type = N'COLUMN', @level2name = N'CutRef';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'記錄各裁次實際使用的SEQ、Roll、Dyelot、Yardage', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CuttingOutputFabricRecord';

