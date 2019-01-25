CREATE TABLE [dbo].[FIR_Odor] (
    [ID]        BIGINT         CONSTRAINT [DF_FIR_Odor_ID] DEFAULT ('') NOT NULL,
    [Roll]      VARCHAR (8)    CONSTRAINT [DF_FIR_Odor_Roll] DEFAULT ('') NOT NULL,
    [Dyelot]    VARCHAR (8)    CONSTRAINT [DF_FIR_Odor_Dyelot] DEFAULT ('') NOT NULL,
    [Inspdate]  DATE           CONSTRAINT [DF_FIR_Odor_Inspdate] DEFAULT ('') NULL,
    [Inspector] VARCHAR (10)   CONSTRAINT [DF_FIR_Odor_Inspector] DEFAULT ('') NULL,
    [Result]    VARCHAR (5)    CONSTRAINT [DF_FIR_Odor_Result] DEFAULT ('') NULL,
    [Remark]    NVARCHAR (100) CONSTRAINT [DF_FIR_Odor_Remark] DEFAULT ('') NULL,
    [AddName]   VARCHAR (10)   CONSTRAINT [DF_FIR_Odor_AddName] DEFAULT ('') NULL,
    [AddDate]   DATETIME       NULL,
    [EditName]  VARCHAR (10)   CONSTRAINT [DF_FIR_Odor_EditName] DEFAULT ('') NULL,
    [EditDate]  DATETIME       NULL,
    CONSTRAINT [PK_FIR_Odor] PRIMARY KEY CLUSTERED ([ID] ASC, [Roll] ASC, [Dyelot] ASC)
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'����', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Odor', @level2type = N'COLUMN', @level2name = N'Roll';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'���絲�G', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Odor', @level2type = N'COLUMN', @level2name = N'Result';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�Ƶ�', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Odor', @level2type = N'COLUMN', @level2name = N'Remark';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'����H��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Odor', @level2type = N'COLUMN', @level2name = N'Inspector';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'������', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Odor', @level2type = N'COLUMN', @level2name = N'Inspdate';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Odor', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�̫�s��H��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Odor', @level2type = N'COLUMN', @level2name = N'EditName';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�̫�s��ɶ�', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Odor', @level2type = N'COLUMN', @level2name = N'EditDate';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'����', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Odor', @level2type = N'COLUMN', @level2name = N'Dyelot';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�s�W�H��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Odor', @level2type = N'COLUMN', @level2name = N'AddName';




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�s�W�ɶ�', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'FIR_Odor', @level2type = N'COLUMN', @level2name = N'AddDate';



