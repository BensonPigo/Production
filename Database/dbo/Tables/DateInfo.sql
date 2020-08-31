CREATE TABLE [dbo].[DateInfo] (
    [Name]      VARCHAR (30)   CONSTRAINT [DF_DateInfo_Name] DEFAULT ('') NOT NULL,
    [DateStart] DATE           NULL,
    [DateEnd]   DATE           NULL,
    [Remark]    NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_DateInfo] PRIMARY KEY CLUSTERED ([Name] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DateInfo', @level2type = N'COLUMN', @level2name = N'DateEnd';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DateInfo', @level2type = N'COLUMN', @level2name = N'DateStart';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DateInfo', @level2type = N'COLUMN', @level2name = N'Name';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'每日資料交換用的空殼Table', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'DateInfo';

