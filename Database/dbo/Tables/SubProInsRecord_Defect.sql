CREATE TABLE [dbo].[SubProInsRecord_Defect] (
    [Ukey]                BIGINT       IDENTITY (1, 1) NOT NULL,
    [SubProInsRecordUkey] BIGINT       NOT NULL,
    [DefectCode]          VARCHAR (50) NULL,
    [DefectQty]           INT          NULL,
    CONSTRAINT [PK_SubProInsRecord_Defect] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);

