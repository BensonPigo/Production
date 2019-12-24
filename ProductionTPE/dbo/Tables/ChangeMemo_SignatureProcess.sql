CREATE TABLE [dbo].[ChangeMemo_SignatureProcess] (
    [Ukey]                          BIGINT       IDENTITY (1, 1) NOT NULL,
    [ChangeMemoUkey]                BIGINT       NOT NULL,
    [MDivisionID]                   VARCHAR (8)  NOT NULL,
    [ChangeMemo_InformDepartmentID] VARCHAR (30) NOT NULL,
    [ApvName]                       VARCHAR (10) NULL,
    [ApvDate]                       DATETIME     NULL,
    [OnlyInform]                    BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_ChangeMemo_SignatureProcess] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'簽核日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChangeMemo_SignatureProcess', @level2type = N'COLUMN', @level2name = N'ApvDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'簽核人員工號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChangeMemo_SignatureProcess', @level2type = N'COLUMN', @level2name = N'ApvName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'所屬部門', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChangeMemo_SignatureProcess', @level2type = N'COLUMN', @level2name = N'ChangeMemo_InformDepartmentID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'所屬廠區', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChangeMemo_SignatureProcess', @level2type = N'COLUMN', @level2name = N'MDivisionID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'被簽核的Change Memo', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChangeMemo_SignatureProcess', @level2type = N'COLUMN', @level2name = N'ChangeMemoUkey';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'人員簽核紀錄', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ChangeMemo_SignatureProcess';

