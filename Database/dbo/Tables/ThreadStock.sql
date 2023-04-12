CREATE TABLE [dbo].[ThreadStock] (
    [Refno]            VARCHAR (36) CONSTRAINT [DF_ThreadStock_Refno] DEFAULT ('') NOT NULL,
    [ThreadColorID]    VARCHAR (15) CONSTRAINT [DF_ThreadStock_ThreadColorID] DEFAULT ('') NOT NULL,
    [NewCone]          NUMERIC (5)  CONSTRAINT [DF_ThreadStock_NewCone] DEFAULT ((0)) NULL,
    [UsedCone]         NUMERIC (5)  CONSTRAINT [DF_ThreadStock_UsedCone] DEFAULT ((0)) NULL,
    [ThreadLocationID] VARCHAR (10) CONSTRAINT [DF_ThreadStock_ThreadLocationID] DEFAULT ('') NOT NULL,
    [AddName]          VARCHAR (10) CONSTRAINT [DF_ThreadStock_AddName] DEFAULT ('') NULL,
    [AddDate]          DATETIME     NULL,
    [EditName]         VARCHAR (10) CONSTRAINT [DF_ThreadStock_EditName] DEFAULT ('') NULL,
    [EditDate]         DATETIME     NULL,
    CONSTRAINT [PK_ThreadStock] PRIMARY KEY CLUSTERED ([Refno], [ThreadColorID], [ThreadLocationID])
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Thread Stock', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadStock';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadStock', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'顏色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadStock', @level2type = N'COLUMN', @level2name = N'ThreadColorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'完整Cone', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadStock', @level2type = N'COLUMN', @level2name = N'NewCone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'非完整Cone數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadStock', @level2type = N'COLUMN', @level2name = N'UsedCone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'位置', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadStock', @level2type = N'COLUMN', @level2name = N'ThreadLocationID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadStock', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadStock', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadStock', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadStock', @level2type = N'COLUMN', @level2name = N'EditDate';

