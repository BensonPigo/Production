CREATE TABLE [dbo].[TimeStudy] (
    [StyleID]         VARCHAR (15) CONSTRAINT [DF_TimeStudy_StyleID] DEFAULT ('') NOT NULL,
    [SeasonID]        VARCHAR (10) CONSTRAINT [DF_TimeStudy_SeasonID] DEFAULT ('') NOT NULL,
    [ComboType]       VARCHAR (1)  CONSTRAINT [DF_TimeStudy_ComboType] DEFAULT ('') NOT NULL,
    [BrandID]         VARCHAR (8)  CONSTRAINT [DF_TimeStudy_BrandID] DEFAULT ('') NOT NULL,
    [Version]         VARCHAR (2)  CONSTRAINT [DF_TimeStudy_Version] DEFAULT ('') NULL,
    [Phase]           VARCHAR (10) CONSTRAINT [DF_TimeStudy_Phase] DEFAULT ('') NULL,
    [TotalSewingTime] INT          CONSTRAINT [DF_TimeStudy_TotalSewingTime] DEFAULT ((0)) NULL,
    [NumberSewer]     TINYINT      CONSTRAINT [DF_TimeStudy_NumberSewer] DEFAULT ((0)) NULL,
    [ID]              BIGINT       IDENTITY (1, 1) NOT NULL,
    [AddName]         VARCHAR (10) CONSTRAINT [DF_TimeStudy_AddName] DEFAULT ('') NULL,
    [AddDate]         DATETIME     NULL,
    [EditName]        VARCHAR (10) CONSTRAINT [DF_TimeStudy_EditName] DEFAULT ('') NULL,
    [EditDate]        DATETIME     NULL,
    [OldKey]          VARCHAR (13) CONSTRAINT [DF_TimeStudy_OldKey] DEFAULT ('') NULL,
    [IETMSID]         VARCHAR (10) CONSTRAINT [DF_TimeStudy_IETMSID] DEFAULT ('') NULL,
    [IETMSVersion]    VARCHAR (3)  CONSTRAINT [DF_TimeStudy_IETMSVersion] DEFAULT ('') NULL,
    [Status]          VARCHAR(15)  CONSTRAINT [DF_TimeStudy_Status] DEFAULT ('') NOT NULL , 
    CONSTRAINT [PK_TimeStudy] PRIMARY KEY CLUSTERED ([StyleID] ASC, [SeasonID] ASC, [ComboType] ASC, [BrandID] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Factory GSD', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'款式號碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy', @level2type = N'COLUMN', @level2name = N'StyleID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'季別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy', @level2type = N'COLUMN', @level2name = N'SeasonID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'組合型態', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy', @level2type = N'COLUMN', @level2name = N'ComboType';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Brand', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy', @level2type = N'COLUMN', @level2name = N'BrandID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'版本', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy', @level2type = N'COLUMN', @level2name = N'Version';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'階段', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy', @level2type = N'COLUMN', @level2name = N'Phase';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'總秒數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy', @level2type = N'COLUMN', @level2name = N'TotalSewingTime';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'所需人力數', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy', @level2type = N'COLUMN', @level2name = N'NumberSewer';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Time Study UKey', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'TimeStudy', @level2type = N'COLUMN', @level2name = N'OldKey';


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'狀態',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'TimeStudy',
    @level2type = N'COLUMN',
    @level2name = N'Status'