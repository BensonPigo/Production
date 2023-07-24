CREATE TABLE [dbo].[Style_ThreadColorCombo_Detail] (
    [Style_ThreadColorComboUkey] BIGINT       NOT NULL,
    [Seq]                        VARCHAR (2)  NOT NULL,
    [SCIRefNo]                   VARCHAR (30) CONSTRAINT [DF_Style_ThreadColorCombo_Detail_SCIRefNo] DEFAULT ('') NOT NULL,
    [SuppId]                     VARCHAR (6)  CONSTRAINT [DF_Style_ThreadColorCombo_Detail_SuppId] DEFAULT ('') NOT NULL,
    [Article]                    VARCHAR (8)  NOT NULL,
    [ColorID]                    VARCHAR (6)  CONSTRAINT [DF_Style_ThreadColorCombo_Detail_ColorID] DEFAULT ('') NOT NULL,
    [SuppColor]                  VARCHAR (30) CONSTRAINT [DF_Style_ThreadColorCombo_Detail_SuppColor] DEFAULT ('') NOT NULL,
    [AddName]                    VARCHAR (10) CONSTRAINT [DF_Style_ThreadColorCombo_Detail_AddName] DEFAULT ('') NOT NULL,
    [AddDate]                    DATETIME     NULL,
    [EditName]                   VARCHAR (10) CONSTRAINT [DF_Style_ThreadColorCombo_Detail_EditName] DEFAULT ('') NOT NULL,
    [EditDate]                   DATETIME     NULL,
    [Ukey]                       BIGINT       NOT NULL,
    PRIMARY KEY CLUSTERED ([Style_ThreadColorComboUkey] ASC, [Seq] ASC, [Article] ASC)
);

