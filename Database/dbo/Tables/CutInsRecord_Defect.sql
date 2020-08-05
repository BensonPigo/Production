CREATE TABLE [dbo].[CutInsRecord_Defect] (
    [Ukey]             BIGINT       IDENTITY (1, 1) NOT NULL,
    [CutInsRecordUkey] BIGINT       NOT NULL,
    [DefectCode]       VARCHAR (50) NULL,
    [DefectQty]        INT          NULL,
    CONSTRAINT [PK_CutInsRecord_Defect] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

