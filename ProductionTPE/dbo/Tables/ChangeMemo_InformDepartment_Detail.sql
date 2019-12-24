CREATE TABLE [dbo].[ChangeMemo_InformDepartment_Detail] (
    [Ukey]        BIGINT       IDENTITY (1, 1) NOT NULL,
    [MDivisionID] VARCHAR (8)  NOT NULL,
    [ID]          VARCHAR (30) NOT NULL,
    [Type]        VARCHAR (10) NOT NULL,
    [Pass1FtyID]  VARCHAR (10) NOT NULL,
    CONSTRAINT [PK_ChangeMemo_InformDepartment_Detail] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'員工工號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChangeMemo_InformDepartment_Detail', @level2type = N'COLUMN', @level2name = N'Pass1FtyID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'所屬部門', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChangeMemo_InformDepartment_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'所屬廠區', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChangeMemo_InformDepartment_Detail', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'各部門負責簽核Change Memo的人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChangeMemo_InformDepartment_Detail';

