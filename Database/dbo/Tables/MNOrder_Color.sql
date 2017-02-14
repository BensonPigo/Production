CREATE TABLE [dbo].[MNOrder_Color] (
    [ID]                VARCHAR (13)  CONSTRAINT [DF_MNOrder_Color_ID] DEFAULT ('') NOT NULL,
    [ColorID]           VARCHAR (6)   CONSTRAINT [DF_MNOrder_Color_ColorID] DEFAULT ('') NOT NULL,
    [ColorName]         NVARCHAR (90) CONSTRAINT [DF_MNOrder_Color_ColorName] DEFAULT ('') NULL,
    [Seqno]             VARCHAR (2)   CONSTRAINT [DF_MNOrder_Color_Seqno] DEFAULT ('') NOT NULL,
    [ColorMultiple]     VARCHAR (6)   CONSTRAINT [DF_MNOrder_Color_ColorMultiple] DEFAULT ('') NULL,
    [ColorMultipleName] NVARCHAR (90) CONSTRAINT [DF_MNOrder_Color_ColorMultipleName] DEFAULT ('') NULL,
    CONSTRAINT [PK_MNOrder_Color] PRIMARY KEY CLUSTERED ([ID], [ColorID], [Seqno])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'製造- 顏色清單', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_Color';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'訂單單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_Color', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_Color', @level2type = N'COLUMN', @level2name = N'ColorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_Color', @level2type = N'COLUMN', @level2name = N'ColorName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'序號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_Color', @level2type = N'COLUMN', @level2name = N'Seqno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_Color', @level2type = N'COLUMN', @level2name = N'ColorMultiple';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色名稱', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MNOrder_Color', @level2type = N'COLUMN', @level2name = N'ColorMultipleName';

