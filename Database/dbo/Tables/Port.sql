CREATE TABLE [dbo].[Port] (
    [ID]        VARCHAR (20)   CONSTRAINT [DF_Port_ID] DEFAULT ('') NOT NULL,
    [CountryID] VARCHAR (2)    CONSTRAINT [DF_Port_CountryID] DEFAULT ('') NULL,
    [Remark]    NVARCHAR (MAX) CONSTRAINT [DF_Port_Remark] DEFAULT ('') NULL,
    [AddName]   VARCHAR (10)   CONSTRAINT [DF_Port_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME       NULL,
    [EditName]  VARCHAR (10)   CONSTRAINT [DF_Port_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME       NULL,
    [Junk]      BIT            CONSTRAINT [DF_Port_Junk] DEFAULT ((0)) NULL,
    [Name]  [varchar](50) NOT NULL CONSTRAINT [DF_Port_Name] DEFAULT (''),
    [AirPort]      BIT            CONSTRAINT [DF_Port_AirPort] DEFAULT ((0)) NOT NULL,
    [SeaPort]      BIT            CONSTRAINT [DF_Port_SeaPort] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Port] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Port', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Port';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'港口', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Port', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Port', @level2type = N'COLUMN', @level2name = N'CountryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Port', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Port', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Port', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Port', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Port', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cancel', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Port', @level2type = N'COLUMN', @level2name = N'Junk';

