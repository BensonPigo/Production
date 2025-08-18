CREATE TABLE [dbo].[P_FabricDispatchRate] (
    [EstCutDate]         DATE            NOT NULL,
    [FactoryID]          VARCHAR (8000)  NOT NULL,
    [FabricDispatchRate] NUMERIC (38, 2) CONSTRAINT [DF_P_FabricDispatchRate_FabricDispatchRate_New] DEFAULT ((0)) NULL,
    [BIFactoryID]        VARCHAR (8000)  CONSTRAINT [DF_P_FabricDispatchRate_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]       DATETIME        NULL,
    [BIStatus]           VARCHAR (8000)  CONSTRAINT [DF_P_FabricDispatchRate_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_FabricDispatchRate] PRIMARY KEY CLUSTERED ([EstCutDate] ASC, [FactoryID] ASC)
);



GO


GO


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'預計裁剪日' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricDispatchRate', @level2type=N'COLUMN',@level2name=N'EstCutDate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'廠代' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricDispatchRate', @level2type=N'COLUMN',@level2name=N'FactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dispatch的佔比' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricDispatchRate', @level2type=N'COLUMN',@level2name=N'FabricDispatchRate'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricDispatchRate', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_FabricDispatchRate', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_FabricDispatchRate', @level2type = N'COLUMN', @level2name = N'BIStatus';

