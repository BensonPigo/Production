CREATE TABLE [dbo].[FtyStdRate_EMB] (
    [Region]        VARCHAR (4)     CONSTRAINT [DF_FtyStdRate_EMB_Region] DEFAULT ('') NOT NULL,
    [SeasonID]      VARCHAR (10)    CONSTRAINT [DF_FtyStdRate_EMB_SeasonID] DEFAULT ('') NOT NULL,
    [StartRange]    DECIMAL (9)     CONSTRAINT [DF_FtyStdRate_EMB_StartRange] DEFAULT ((0)) NOT NULL,
    [EndRange]      DECIMAL (9)     CONSTRAINT [DF_FtyStdRate_EMB_EndRange] DEFAULT ((0)) NOT NULL,
    [BasedStitches] DECIMAL (9)     CONSTRAINT [DF_FtyStdRate_EMB_BasedStitches] DEFAULT ((0)) NOT NULL,
    [BasedPay]      DECIMAL (9, 4)  CONSTRAINT [DF_FtyStdRate_EMB_BasedPay] DEFAULT ((0)) NOT NULL,
    [AddedStitches] DECIMAL (9)     CONSTRAINT [DF_FtyStdRate_EMB_AddedStitches] DEFAULT ((0)) NOT NULL,
    [AddedPay]      DECIMAL (18, 9) CONSTRAINT [DF_FtyStdRate_EMB_AddedPay] DEFAULT ((0)) NOT NULL,
    [ThreadRatio]   DECIMAL (5, 2)  CONSTRAINT [DF_FtyStdRate_EMB_ThreadRatio] DEFAULT ((0)) NOT NULL,
    [Ratio]         DECIMAL (5, 2)  CONSTRAINT [DF_FtyStdRate_EMB_Ratio] DEFAULT ((0)) NOT NULL,
    [AddName]       VARCHAR (10)    CONSTRAINT [DF_FtyStdRate_EMB_AddName] DEFAULT ('') NOT NULL,
    [AddDate]       DATETIME        NULL,
    [EditName]      VARCHAR (10)    CONSTRAINT [DF_FtyStdRate_EMB_EditName] DEFAULT ('') NOT NULL,
    [EditDate]      DATETIME        NULL,
    CONSTRAINT [PK_FtyStdRate_EMB] PRIMARY KEY CLUSTERED ([Region] ASC, [SeasonID] ASC, [StartRange] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_EMB', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_EMB', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_EMB', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_EMB', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'保留利潤(%)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_EMB', @level2type = N'COLUMN', @level2name = N'Ratio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'額外價格', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_EMB', @level2type = N'COLUMN', @level2name = N'AddedPay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'額外針數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_EMB', @level2type = N'COLUMN', @level2name = N'AddedStitches';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'基本價格', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_EMB', @level2type = N'COLUMN', @level2name = N'BasedPay';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'基本針數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_EMB', @level2type = N'COLUMN', @level2name = N'BasedStitches';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'迄止針數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_EMB', @level2type = N'COLUMN', @level2name = N'EndRange';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'起始針數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_EMB', @level2type = N'COLUMN', @level2name = N'StartRange';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'季度', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_EMB', @level2type = N'COLUMN', @level2name = N'SeasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'地區', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_EMB', @level2type = N'COLUMN', @level2name = N'Region';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'COP - 工廠標準成本(Emboridery)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FtyStdRate_EMB';

