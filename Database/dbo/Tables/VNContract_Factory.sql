CREATE TABLE [dbo].[VNContract_Factory] (
    [VNContractID] VARCHAR (15) NOT NULL,
    [FactoryID]    VARCHAR (8)  NOT NULL,
    CONSTRAINT [PK_VNContract_Factory] PRIMARY KEY CLUSTERED ([VNContractID] ASC, [FactoryID] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'合約對應的工廠代碼', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract_Factory', @level2type = N'COLUMN', @level2name = N'FactoryID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'海關合約ID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'VNContract_Factory', @level2type = N'COLUMN', @level2name = N'VNContractID';

