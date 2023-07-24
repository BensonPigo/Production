
	CREATE TABLE [dbo].[Style_ThreadColorCombo_History_Detail] (
    [Style_ThreadColorCombo_HistoryUkey] BIGINT         NOT NULL,
    [Seq]                                VARCHAR (2)    NOT NULL,
    [SCIRefNo]                           VARCHAR (30)   CONSTRAINT [DF_Style_ThreadColorCombo_History_Detail_SCIRefNo] DEFAULT ('') NOT NULL,
    [SuppId]                             VARCHAR (6)    CONSTRAINT [DF_Style_ThreadColorCombo_History_Detail_SuppId] DEFAULT ('') NOT NULL,
    [Article]                            VARCHAR (8)    NOT NULL,
    [ColorID]                            VARCHAR (6)    CONSTRAINT [DF_Style_ThreadColorCombo_History_Detail_ColorID] DEFAULT ('') NOT NULL,
    [SuppColor]                          VARCHAR (30)   CONSTRAINT [DF_Style_ThreadColorCombo_History_Detail_SuppColor] DEFAULT ('') NOT NULL,
    [AddName]                            VARCHAR (10)   CONSTRAINT [DF_Style_ThreadColorCombo_History_Detail_AddName] DEFAULT ('') NOT NULL,
    [AddDate]                            DATETIME       NULL,
    [EditName]                           VARCHAR (10)   CONSTRAINT [DF_Style_ThreadColorCombo_History_Detail_EditName] DEFAULT ('') NOT NULL,
    [EditDate]                           DATETIME       NULL,
    [Ukey]                               BIGINT         NOT NULL,
    [UseRatio]                           DECIMAL (5, 2) CONSTRAINT [DF_Style_ThreadColorCombo_History_Detail_UseRatio] DEFAULT ((0)) NOT NULL,
    [Allowance]                          DECIMAL (4, 2) CONSTRAINT [DF_Style_ThreadColorCombo_History_Detail_Allowance] DEFAULT ((0)) NOT NULL,
    [AllowanceTubular]                   DECIMAL (4, 2) CONSTRAINT [DF_Style_ThreadColorCombo_History_Detail_AllowanceTubular] DEFAULT ((0)) NOT NULL,
    [UseRatioHem]                        NUMERIC (5, 2) CONSTRAINT [DF_Style_ThreadColorCombo_History_Detail_UseRatioHem] DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Style_ThreadColorCombo_HistoryUkey] ASC, [Seq] ASC, [Article] ASC)
);


