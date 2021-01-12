CREATE TABLE [dbo].[FabricDefect] (
    [ID]            VARCHAR (2)   CONSTRAINT [DF_FabricDefect_ID] DEFAULT ('') NOT NULL,
    [Type]          VARCHAR (20)  CONSTRAINT [DF_FabricDefect_Type] DEFAULT ('') NULL,
    [DescriptionEN] VARCHAR (60)  CONSTRAINT [DF_FabricDefect_DescriptionEN] DEFAULT ('') NULL,
    [Junk]          BIT           CONSTRAINT [DF_FabricDefect_Junk] DEFAULT ((0)) NULL,
    [AddName]       VARCHAR (10)  CONSTRAINT [DF_FabricDefect_AddName] DEFAULT ('') NULL,
    [AddDate]       DATETIME      NULL,
    [EditName]      VARCHAR (10)  CONSTRAINT [DF_FabricDefect_EditName] DEFAULT ('') NULL,
    [EditDate]      DATETIME      NULL,
    [LocalDesc]     NVARCHAR (60) CONSTRAINT [DF_FabricDefect_LocalDesc] DEFAULT ('') NULL,
    CONSTRAINT [PK_FabricDefect] PRIMARY KEY CLUSTERED ([ID] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Fabric Defect 基本檔', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FabricDefect';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FabricDefect', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'瑕疵種類', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FabricDefect', @level2type = N'COLUMN', @level2name = N'Type';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'英文描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FabricDefect', @level2type = N'COLUMN', @level2name = N'DescriptionEN';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'取消', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FabricDefect', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FabricDefect', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FabricDefect', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FabricDefect', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FabricDefect', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'當地語言描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FabricDefect', @level2type = N'COLUMN', @level2name = N'LocalDesc';

