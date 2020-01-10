CREATE TABLE [dbo].[ChangeMemo_InformDepartment] (
    [ID]          VARCHAR (30)   NOT NULL,
    [MDivisionID] VARCHAR (8)    NOT NULL,
    [Description] NVARCHAR (100) NULL,
    [Junk]        BIT            NULL,
    [AddName]     VARCHAR (10)   NULL,
    [AddDate]     DATETIME       NULL,
    [EditName]    VARCHAR (10)   NULL,
    [EditDate]    DATETIME       NULL,
    CONSTRAINT [PK_ChangeMemo_InformDepartment] PRIMARY KEY CLUSTERED ([ID] ASC, [MDivisionID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'所屬廠區', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChangeMemo_InformDepartment', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'需要被ChanegeMemo通知的部門', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChangeMemo_InformDepartment';

