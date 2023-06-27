
CREATE TABLE [dbo].[Style_QTThreadColorCombo_History_Detail] (
    [Style_QTThreadColorCombo_HistoryUkey] BIGINT         NOT NULL,
    [Seq]                                  VARCHAR (2)    NOT NULL,
    [SCIRefNo]                             VARCHAR (30)   CONSTRAINT [DF_Style_QTThreadColorCombo_History_Detail_SCIRefNo] DEFAULT ('') NOT NULL,
    [SuppId]                               VARCHAR (6)    CONSTRAINT [DF_Style_QTThreadColorCombo_History_Detail_SuppId] DEFAULT ('') NOT NULL,
    [Article]                              VARCHAR (8)    NOT NULL,
    [ColorID]                              VARCHAR (6)    CONSTRAINT [DF_Style_QTThreadColorCombo_History_Detail_ColorID] DEFAULT ('') NOT NULL,
    [SuppColor]                            VARCHAR (30)   CONSTRAINT [DF_Style_QTThreadColorCombo_History_Detail_SuppColor] DEFAULT ('') NOT NULL,
    [AddName]                              VARCHAR (10)   CONSTRAINT [DF_Style_QTThreadColorCombo_History_Detail_AddName] DEFAULT ('') NOT NULL,
    [AddDate]                              DATETIME       NULL,
    [EditName]                             VARCHAR (10)   CONSTRAINT [DF_Style_QTThreadColorCombo_History_Detail_EditName] DEFAULT ('') NOT NULL,
    [EditDate]                             DATETIME       NULL,
    [Ukey]                                 BIGINT         NOT NULL,
    [Ratio]                                NUMERIC (5, 2) NOT NULL,
    CONSTRAINT [PK_Style_QTThreadColorCombo_History_Detail] PRIMARY KEY CLUSTERED ([Style_QTThreadColorCombo_HistoryUkey] ASC, [Seq] ASC, [Article] ASC)
);


GO


