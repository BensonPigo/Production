CREATE TABLE [dbo].[MachineType_ThreadRatio] (
    [ID]               VARCHAR (10)   CONSTRAINT [DF_MachineType_ThreadRatio_ID] DEFAULT ('') NOT NULL,
    [SEQ]              VARCHAR (2)    CONSTRAINT [DF_MachineType_ThreadRatio_SEQ] DEFAULT ('') NOT NULL,
    [ThreadLocation]   VARCHAR (20)   CONSTRAINT [DF_MachineType_ThreadRatio_ThreadLocation] DEFAULT ('') NULL,
    [UseRatio]         NUMERIC (5, 2) CONSTRAINT [DF_MachineType_ThreadRatio_UseRatioNumeric] DEFAULT ((0)) NULL,
    [Allowance]        NUMERIC (4, 2) NULL,
    [AllowanceTubular] NUMERIC (4, 2) DEFAULT ((0)) NULL,
    CONSTRAINT [PK_MachineType_ThreadRatio] PRIMARY KEY CLUSTERED ([ID] ASC, [SEQ] ASC)
);










GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機台類型', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType_ThreadRatio', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'SEQ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType_ThreadRatio', @level2type = N'COLUMN', @level2name = N'SEQ';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'線在機台的位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType_ThreadRatio', @level2type = N'COLUMN', @level2name = N'ThreadLocation';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'用線量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType_ThreadRatio', @level2type = N'COLUMN', @level2name = N'UseRatio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Material Type的ThreadRatio明細檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType_ThreadRatio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'有使用圓筒輔助機台的耗損量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MachineType_ThreadRatio', @level2type = N'COLUMN', @level2name = N'AllowanceTubular';

