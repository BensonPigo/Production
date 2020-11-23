CREATE TABLE [dbo].[PulloutPort] (
    [ID]                VARCHAR (20)   CONSTRAINT [DF_PulloutPort_ID] DEFAULT ('') NOT NULL,
    [Name]              VARCHAR (50)   CONSTRAINT [DF_PulloutPort_Name] DEFAULT ('') NOT NULL,
    [InternationalCode] VARCHAR (10)   CONSTRAINT [DF_PulloutPort_InternationalCode] DEFAULT ('') NOT NULL,
    [CountryID]         VARCHAR (2)    CONSTRAINT [DF_PulloutPort_CountryID] DEFAULT ('') NOT NULL,
    [AirPort]           BIT            CONSTRAINT [DF_PulloutPort_AirPort] DEFAULT ((0)) NOT NULL,
    [SeaPort]           BIT            CONSTRAINT [DF_PulloutPort_SeaPort] DEFAULT ((0)) NOT NULL,
    [Remark]            NVARCHAR (MAX) CONSTRAINT [DF_PulloutPort_Remark] DEFAULT ('') NULL,
    [AddName]           VARCHAR (10)   CONSTRAINT [DF_PulloutPort_AddName] DEFAULT ('') NULL,
    [AddDate]           DATETIME       NULL,
    [EditName]          VARCHAR (10)   CONSTRAINT [DF_PulloutPort_EditName] DEFAULT ('') NULL,
    [EditDate]          DATETIME       NULL,
    [Junk]              BIT            CONSTRAINT [DF_PulloutPort_Junk] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_PulloutPort] PRIMARY KEY CLUSTERED ([ID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Cancel', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PulloutPort', @level2type = N'COLUMN', @level2name = N'Junk';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PulloutPort', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'最後修改人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PulloutPort', @level2type = N'COLUMN', @level2name = N'EditName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增時間', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PulloutPort', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新增人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PulloutPort', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PulloutPort', @level2type = N'COLUMN', @level2name = N'Remark';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國別', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PulloutPort', @level2type = N'COLUMN', @level2name = N'CountryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'國際代號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PulloutPort', @level2type = N'COLUMN', @level2name = N'InternationalCode';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'港口', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PulloutPort', @level2type = N'COLUMN', @level2name = N'ID';

