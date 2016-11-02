CREATE TABLE [dbo].[Cfa_Detail] (
    [ID]                  VARCHAR (13)   CONSTRAINT [DF_Cfa_Detail_ID] DEFAULT ('') NOT NULL,
    [GarmentDefectCodeID] VARCHAR (3)    CONSTRAINT [DF_Cfa_Detail_GarmentDefectCodeID] DEFAULT ('') NOT NULL,
    [GarmentDefectTypeid] VARCHAR (1)    CONSTRAINT [DF_Cfa_Detail_GarmentDefectTypeid] DEFAULT ('') NULL,
    [Qty]                 NUMERIC (5)    CONSTRAINT [DF_Cfa_Detail_Qty] DEFAULT ((0)) NULL,
    [Action]              VARCHAR (254)  CONSTRAINT [DF_Cfa_Detail_Action] DEFAULT ('') NULL,
    [Remark]              NVARCHAR (254) CONSTRAINT [DF_Cfa_Detail_Remark] DEFAULT ('') NULL,
    [CFAAreaID]           VARCHAR (3)    NULL,
    CONSTRAINT [PK_Cfa_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [GarmentDefectCodeID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'CFA Inline Record', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa_Detail', @level2type = N'COLUMN', @level2name = N'GarmentDefectCodeID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵分類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa_Detail', @level2type = N'COLUMN', @level2name = N'GarmentDefectTypeid';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa_Detail', @level2type = N'COLUMN', @level2name = N'Qty';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'解決方案', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa_Detail', @level2type = N'COLUMN', @level2name = N'Action';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Cfa_Detail', @level2type = N'COLUMN', @level2name = N'Remark';

