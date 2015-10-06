CREATE TABLE [dbo].[MNOrder_ColorCombo] (
    [ID]           VARCHAR (13) CONSTRAINT [DF_MNOrder_ColorCombo_ID] DEFAULT ('') NOT NULL,
    [Article]      VARCHAR (8)  CONSTRAINT [DF_MNOrder_ColorCombo_Article] DEFAULT ('') NOT NULL,
    [ColorID]      VARCHAR (6)  CONSTRAINT [DF_MNOrder_ColorCombo_ColorID] DEFAULT ('') NULL,
    [FabricCode]   VARCHAR (3)  CONSTRAINT [DF_MNOrder_ColorCombo_FabricCode] DEFAULT ('') NULL,
    [PatternPanel] VARCHAR (2)  CONSTRAINT [DF_MNOrder_ColorCombo_PatternPanel] DEFAULT ('') NULL,
    [LectraCode]   VARCHAR (2)  CONSTRAINT [DF_MNOrder_ColorCombo_LectraCode] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_MNOrder_ColorCombo] PRIMARY KEY CLUSTERED ([ID] ASC, [Article] ASC, [LectraCode] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'製造- 配色表 Color Comb. (主料+副料-配色表)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_ColorCombo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_ColorCombo', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色組', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_ColorCombo', @level2type = N'COLUMN', @level2name = N'Article';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_ColorCombo', @level2type = N'COLUMN', @level2name = N'ColorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布種', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_ColorCombo', @level2type = N'COLUMN', @level2name = N'FabricCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'部位別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_ColorCombo', @level2type = N'COLUMN', @level2name = N'PatternPanel';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'布別+部位的代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_ColorCombo', @level2type = N'COLUMN', @level2name = N'LectraCode';

