CREATE TABLE [dbo].[GarmentTest_Detail_Shrinkage] (
    [ID]           BIGINT          NOT NULL,
    [No]           INT             NOT NULL,
    [Location]     VARCHAR (1)     NOT NULL,
    [Type]         VARCHAR (150)   NOT NULL,
    [BeforeWash]   NUMERIC (11, 2) NULL,
    [SizeSpec]     NUMERIC (11, 2) NULL,
    [AfterWash1]   NUMERIC (11, 2) NULL,
    [Shrinkage1]   NUMERIC (11, 2) NULL,
    [AfterWash2]   NUMERIC (11, 2) NULL,
    [Shrinkage2]   NUMERIC (11, 2) NULL,
    [AfterWash3]   NUMERIC (11, 2) NULL,
    [Shrinkage3]   NUMERIC (11, 2) NULL,
    [Seq]          NUMERIC (6)     NULL,
    [BrandID]      VARCHAR (8)     CONSTRAINT [DF__GarmentTe__Brand__06B6917A] DEFAULT ('') NULL,
    [LocationGoup] VARCHAR (4)     CONSTRAINT [DF__GarmentTe__Locat__07AAB5B3] DEFAULT ('') NULL,
    CONSTRAINT [PK_GarmentTest_Detail_Shrinkage] PRIMARY KEY CLUSTERED ([ID] ASC, [No] ASC, [Location] ASC, [Type] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'判斷款式是否為套裝(T+B)/其他種類的套裝/PCS', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest_Detail_Shrinkage', @level2type = N'COLUMN', @level2name = N'LocationGoup';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Shrinkage 測量點清單是否有指定品牌', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'GarmentTest_Detail_Shrinkage', @level2type = N'COLUMN', @level2name = N'BrandID';

