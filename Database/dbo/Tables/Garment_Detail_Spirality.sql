CREATE TABLE [dbo].[Garment_Detail_Spirality] (
    [ID]              BIGINT          NOT NULL,
    [No]              INT             NOT NULL,
    [Location]        VARCHAR (1)     NOT NULL,
    [MethodA_AAPrime] NUMERIC (11, 2) CONSTRAINT [DF_Garment_Detail_Spirality_AAPrime] DEFAULT ((0)) NOT NULL,
    [MethodA_APrimeB] NUMERIC (11, 2) CONSTRAINT [DF_Garment_Detail_Spirality_APrimeB] DEFAULT ((0)) NOT NULL,
    [MethodB_AAPrime] NUMERIC (11, 2) NOT NULL,
    [MethodB_AB]      NUMERIC (11, 2) CONSTRAINT [DF_Garment_Detail_Spirality_AB] DEFAULT ((0)) NOT NULL,
    [CM]              NUMERIC (11, 2) CONSTRAINT [DF_Garment_Detail_Spirality_CM] DEFAULT ((0)) NOT NULL,
    [MethodA]         NUMERIC (11, 2) CONSTRAINT [DF_Garment_Detail_Spirality_MethodA] DEFAULT ((0)) NOT NULL,
    [MethodB]         NUMERIC (11, 2) CONSTRAINT [DF_Garment_Detail_Spirality_MethodB] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Garment_Detail_Spirality] PRIMARY KEY CLUSTERED ([ID] ASC, [No] ASC, [Location] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Side Seam (%) side-panel (Method B)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Garment_Detail_Spirality', @level2type = N'COLUMN', @level2name = N'MethodB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Body A (%) within-panel (Method A)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Garment_Detail_Spirality', @level2type = N'COLUMN', @level2name = N'MethodA';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Movement of side seam/outseam in CM', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Garment_Detail_Spirality', @level2type = N'COLUMN', @level2name = N'CM';


GO



GO



GO



GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位 (Top, Bottom)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Garment_Detail_Spirality', @level2type = N'COLUMN', @level2name = N'Location';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Garment_Detail.No', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Garment_Detail_Spirality', @level2type = N'COLUMN', @level2name = N'No';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Garment_Detail.ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Garment_Detail_Spirality', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MethodB參數 AB', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Garment_Detail_Spirality', @level2type = N'COLUMN', @level2name = N'MethodB_AB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MethodB參數 AA’', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Garment_Detail_Spirality', @level2type = N'COLUMN', @level2name = N'MethodB_AAPrime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MethodA參數 A’B', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Garment_Detail_Spirality', @level2type = N'COLUMN', @level2name = N'MethodA_APrimeB';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'MethodA參數 AA’', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Garment_Detail_Spirality', @level2type = N'COLUMN', @level2name = N'MethodA_AAPrime';

