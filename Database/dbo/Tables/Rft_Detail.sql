CREATE TABLE [dbo].[Rft_Detail] (
    [ID]                  BIGINT      CONSTRAINT [DF_Rft_Detail_ID] DEFAULT ((0)) NOT NULL,
    [GarmentDefectCodeID] VARCHAR (3) CONSTRAINT [DF_Rft_Detail_GarmentDefectCodeID] DEFAULT ('') NOT NULL,
    [GarmentDefectTypeid] VARCHAR (1) CONSTRAINT [DF_Rft_Detail_GarmentDefectTypeid] DEFAULT ('') NULL,
    [Qty]                 NUMERIC (5) CONSTRAINT [DF_Rft_Detail_Qty] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_Rft_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [GarmentDefectCodeID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Right First Time', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rft_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rft_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rft_Detail', @level2type = N'COLUMN', @level2name = N'GarmentDefectCodeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵分類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rft_Detail', @level2type = N'COLUMN', @level2name = N'GarmentDefectTypeid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Rft_Detail', @level2type = N'COLUMN', @level2name = N'Qty';

