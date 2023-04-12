CREATE TABLE [dbo].[MDMachineBasic] (
    [MachineID]   VARCHAR (30)   CONSTRAINT [DF_MDMachineBasic_MachineID] DEFAULT ('') NOT NULL,
    [Operator]    VARCHAR (10)   CONSTRAINT [DF_MDMachineBasic_Operator] DEFAULT ('') NOT NULL,
    [Junk]        BIT            CONSTRAINT [DF_MDMachineBasic_Junk] DEFAULT ((0)) NOT NULL,
    [Description] NVARCHAR (500) CONSTRAINT [DF_MDMachineBasic_Description] DEFAULT ('') NOT NULL,
    [AddDate]     DATETIME       NULL,
    [AddName]     VARCHAR (10)   CONSTRAINT [DF_MDMachineBasic_AddName] DEFAULT ('') NOT NULL,
    [EditDate]    DATETIME       NULL,
    [Editname]    VARCHAR (10)   CONSTRAINT [DF_MDMachineBasic_Editname] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_MDMachineBasic] PRIMARY KEY CLUSTERED ([MachineID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDMachineBasic', @level2type = N'COLUMN', @level2name = N'Editname';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'編輯日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDMachineBasic', @level2type = N'COLUMN', @level2name = N'EditDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新建人員', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDMachineBasic', @level2type = N'COLUMN', @level2name = N'AddName';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'新建日期', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDMachineBasic', @level2type = N'COLUMN', @level2name = N'AddDate';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'描述', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDMachineBasic', @level2type = N'COLUMN', @level2name = N'Description';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'使用者', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDMachineBasic', @level2type = N'COLUMN', @level2name = N'Operator';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'機器ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'MDMachineBasic', @level2type = N'COLUMN', @level2name = N'MachineID';

