CREATE TABLE [dbo].[CutInsRecord_RollDyelot] (
    [Ukey]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [CutInsRecordUkey] BIGINT         NOT NULL,
    [TicketYds]        NUMERIC (8, 2) CONSTRAINT [DF_CutInsRecord_RollDyelot_TicketYds] DEFAULT ((0)) NOT NULL,
    [Roll]             VARCHAR (8)    CONSTRAINT [DF_CutInsRecord_RollDyelot_Roll] DEFAULT ('') NOT NULL,
    [Dyelot]           VARCHAR (8)    CONSTRAINT [DF_CutInsRecord_RollDyelot_Dyelot] DEFAULT ('') NOT NULL,
    [CutPartName]      VARCHAR (20)   CONSTRAINT [DF_CutInsRecord_RollDyelot_CutPartName] DEFAULT ('') NOT NULL,
    [InspRatio]        INT            CONSTRAINT [DF_CutInsRecord_RollDyelot_InspRatio] DEFAULT ((0)) NOT NULL,
    [TMB]              VARCHAR (1)    CONSTRAINT [DF_CutInsRecord_RollDyelot_TMB] DEFAULT ('') NOT NULL,
    [DeviationValue]   VARCHAR (2)    CONSTRAINT [DF_CutInsRecord_RollDyelot_DeviationValue] DEFAULT ('') NOT NULL,
    [DefectCode]       VARCHAR (50)   CONSTRAINT [DF_CutInsRecord_RollDyelot_DefectCode] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_CutInsRecord_RollDyelot] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'捲號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CutInsRecord_RollDyelot', @level2type = N'COLUMN', @level2name = N'Roll';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'缸號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CutInsRecord_RollDyelot', @level2type = N'COLUMN', @level2name = N'Dyelot';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'針對裁次的最高層數
    取裁完後的裁片最上面一層、中間一層、最下面一層
    確認這 3 層的裁片與馬克公版是否有差異
    只要這 3 層都與公版一致 就代表這次裁剪沒有問題
    ( 像是手裁若不小心裁歪，會導致越下層的裁片與公版越差越多 )', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CutInsRecord_RollDyelot', @level2type = N'COLUMN', @level2name = N'TMB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'檢驗層數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CutInsRecord_RollDyelot', @level2type = N'COLUMN', @level2name = N'InspRatio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁片剪下來後與馬克公版確認誤差', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CutInsRecord_RollDyelot', @level2type = N'COLUMN', @level2name = N'DeviationValue';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CutInsRecord_RollDyelot', @level2type = N'COLUMN', @level2name = N'DefectCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'裁片部位名', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CutInsRecord_RollDyelot', @level2type = N'COLUMN', @level2name = N'CutPartName';

