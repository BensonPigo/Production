CREATE TABLE [dbo].[SpreadingInspection_InsCutRef_Inspection_Roll] (
    [Ukey]                                       BIGINT       IDENTITY (1, 1) NOT NULL,
    [SpreadingInspectionInsCutRefInspectionUkey] BIGINT       DEFAULT ((0)) NOT NULL,
    [POID]                                       VARCHAR (13) DEFAULT ('') NOT NULL,
    [Seq1]                                       VARCHAR (3)  DEFAULT ('') NOT NULL,
    [Seq2]                                       VARCHAR (2)  DEFAULT ('') NOT NULL,
    [Roll]                                       VARCHAR (8)  DEFAULT ('') NOT NULL,
    [Deylot]                                     VARCHAR (8)  DEFAULT ('') NOT NULL,
    PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpreadingInspection_InsCutRef_Inspection_Roll', @level2type = N'COLUMN', @level2name = N'Deylot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpreadingInspection_InsCutRef_Inspection_Roll', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購小項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpreadingInspection_InsCutRef_Inspection_Roll', @level2type = N'COLUMN', @level2name = N'Seq2';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購大項', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpreadingInspection_InsCutRef_Inspection_Roll', @level2type = N'COLUMN', @level2name = N'Seq1';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'採購母單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpreadingInspection_InsCutRef_Inspection_Roll', @level2type = N'COLUMN', @level2name = N'POID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Spreading Inline - 該次檢驗 Fabric Defect 的 Pkey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'SpreadingInspection_InsCutRef_Inspection_Roll', @level2type = N'COLUMN', @level2name = N'SpreadingInspectionInsCutRefInspectionUkey';

