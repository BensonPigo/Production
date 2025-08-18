CREATE TABLE [dbo].[P_SubProInsReport] (
    [Ukey]                 BIGINT          IDENTITY (1, 1) NOT NULL,
    [FactoryID]            VARCHAR (8000)  CONSTRAINT [PK_P_SubProInsReport_FactoryID_New] DEFAULT ('') NOT NULL,
    [SubProLocationID]     VARCHAR (8000)  CONSTRAINT [PK_P_SubProInsReport_SubProLocationID_New] DEFAULT ('') NOT NULL,
    [InspectionDate]       DATE            NULL,
    [SewInLine]            DATE            NULL,
    [SewinglineID]         VARCHAR (8000)  CONSTRAINT [PK_P_SubProInsReport_SewinglineID_New] DEFAULT ('') NOT NULL,
    [Shift]                VARCHAR (8000)  CONSTRAINT [PK_P_SubProInsReport_Shift_New] DEFAULT ('') NOT NULL,
    [RFT]                  NUMERIC (38, 2) CONSTRAINT [PK_P_SubProInsReport_RFT_New] DEFAULT ((0)) NOT NULL,
    [SubProcessID]         VARCHAR (8000)  CONSTRAINT [PK_P_SubProInsReport_SubProcessID_New] DEFAULT ('') NOT NULL,
    [BundleNo]             VARCHAR (8000)  CONSTRAINT [PK_P_SubProInsReport_BundleNo_New] DEFAULT ('') NOT NULL,
    [Artwork]              VARCHAR (8000)  CONSTRAINT [PK_P_SubProInsReport_Artwork_New] DEFAULT ('') NULL,
    [OrderID]              VARCHAR (8000)  CONSTRAINT [PK_P_SubProInsReport_OrderID_New] DEFAULT ('') NOT NULL,
    [Alias]                VARCHAR (8000)  CONSTRAINT [PK_P_SubProInsReport_Alias_New] DEFAULT ('') NOT NULL,
    [BuyerDelivery]        DATE            NULL,
    [BundleGroup]          NUMERIC (38)    CONSTRAINT [PK_P_SubProInsReport_BundleGroup_New] DEFAULT ((0)) NOT NULL,
    [SeasonID]             VARCHAR (8000)  CONSTRAINT [PK_P_SubProInsReport_SeasonID_New] DEFAULT ('') NOT NULL,
    [StyleID]              VARCHAR (8000)  CONSTRAINT [PK_P_SubProInsReport_StyleID_New] DEFAULT ('') NOT NULL,
    [ColorID]              VARCHAR (8000)  CONSTRAINT [PK_P_SubProInsReport_ColorID_New] DEFAULT ('') NOT NULL,
    [SizeCode]             VARCHAR (8000)  CONSTRAINT [PK_P_SubProInsReport_SizeCode_New] DEFAULT ('') NOT NULL,
    [PatternDesc]          NVARCHAR (1000) CONSTRAINT [PK_P_SubProInsReport_PatternDesc_New] DEFAULT ('') NOT NULL,
    [Item]                 VARCHAR (8000)  CONSTRAINT [PK_P_SubProInsReport_Item_New] DEFAULT ('') NOT NULL,
    [Qty]                  NUMERIC (38)    CONSTRAINT [PK_P_SubProInsReport_Qty_New] DEFAULT ((0)) NOT NULL,
    [RejectQty]            INT             CONSTRAINT [PK_P_SubProInsReport_RejectQty_New] DEFAULT ((0)) NOT NULL,
    [Machine]              VARCHAR (8000)  CONSTRAINT [PK_P_SubProInsReport_Machine_New] DEFAULT ('') NOT NULL,
    [Serial]               VARCHAR (8000)  CONSTRAINT [PK_P_SubProInsReport_Serial_New] DEFAULT ('') NOT NULL,
    [Junk]                 BIT             CONSTRAINT [PK_P_SubProInsReport_Junk_New] DEFAULT ((0)) NOT NULL,
    [Description]          NVARCHAR (1000) CONSTRAINT [PK_P_SubProInsReport_Description_New] DEFAULT ('') NOT NULL,
    [DefectCode]           VARCHAR (8000)  CONSTRAINT [PK_P_SubProInsReport_DefectCode_New] DEFAULT ('') NOT NULL,
    [DefectQty]            INT             CONSTRAINT [PK_P_SubProInsReport_DefectQty_New] DEFAULT ((0)) NOT NULL,
    [Inspector]            VARCHAR (8000)  CONSTRAINT [PK_P_SubProInsReport_Inspector_New] DEFAULT ('') NOT NULL,
    [Remark]               NVARCHAR (1000) CONSTRAINT [DF_P_SubProInsReport_Remark_New] DEFAULT ('') NOT NULL,
    [AddDate]              DATETIME        NULL,
    [RepairedDatetime]     DATETIME        NULL,
    [RepairedTime]         INT             CONSTRAINT [PK_P_SubProInsReport_RepairedTime_New] DEFAULT ((0)) NOT NULL,
    [ResolveTime]          INT             CONSTRAINT [PK_P_SubProInsReport_ResolveTime_New] DEFAULT ((0)) NOT NULL,
    [SubProResponseTeamID] VARCHAR (8000)  CONSTRAINT [PK_P_SubProInsReport_SubProResponseTeamID_New] DEFAULT ('') NOT NULL,
    [CustomColumn1]        VARCHAR (8000)  CONSTRAINT [PK_P_SubProInsReport_CustomColumn1_New] DEFAULT ('') NOT NULL,
    [MDivisionID]          VARCHAR (8000)  CONSTRAINT [DF_P_SubProInsReport_MDivisionID_New] DEFAULT ('') NOT NULL,
    [OperatorID]           NVARCHAR (1000) CONSTRAINT [DF_P_SubProInsReport_OpreatorID_New] DEFAULT ('') NOT NULL,
    [OperatorName]         NVARCHAR (1000) CONSTRAINT [DF_P_SubProInsReport_OperatorName_New] DEFAULT ('') NOT NULL,
    [BIFactoryID]          VARCHAR (8000)  CONSTRAINT [DF_P_SubProInsReport_BIFactoryID_New] DEFAULT ('') NOT NULL,
    [BIInsertDate]         DATETIME        NULL,
    [BIStatus]             VARCHAR (8000)  CONSTRAINT [DF_P_SubProInsReport_BIStatus_New] DEFAULT (N'New') NULL,
    CONSTRAINT [PK_P_SubProInsReport] PRIMARY KEY CLUSTERED ([Ukey] ASC)
);



GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�[�u�q�t�Ц�m', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReport', @level2type = N'COLUMN', @level2name = N'SubProLocationID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'���_���', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReport', @level2type = N'COLUMN', @level2name = N'SewInLine';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�Ĥ@������N�q�L�����\�v', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReport', @level2type = N'COLUMN', @level2name = N'RFT';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�[�u�q', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReport', @level2type = N'COLUMN', @level2name = N'SubProcessID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�u��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReport', @level2type = N'COLUMN', @level2name = N'Artwork';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'��a', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReport', @level2type = N'COLUMN', @level2name = N'Alias';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'���컡��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReport', @level2type = N'COLUMN', @level2name = N'PatternDesc';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'��A���ءAT-shirt�BShorts�K', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReport', @level2type = N'COLUMN', @level2name = N'Item';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�j�]�ƶq', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReport', @level2type = N'COLUMN', @level2name = N'Qty';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'���q�L�ƶq', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReport', @level2type = N'COLUMN', @level2name = N'RejectQty';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�[�u�q����', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReport', @level2type = N'COLUMN', @level2name = N'Machine';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�[�u�q�������A', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReport', @level2type = N'COLUMN', @level2name = N'Junk';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�[�u�q��������', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReport', @level2type = N'COLUMN', @level2name = N'Description';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�岫�ƶq', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReport', @level2type = N'COLUMN', @level2name = N'DefectQty';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�ާ@��', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReport', @level2type = N'COLUMN', @level2name = N'Inspector';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�״_������', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReport', @level2type = N'COLUMN', @level2name = N'RepairedDatetime';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�����j�]�����״_�`�ɪ�', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReport', @level2type = N'COLUMN', @level2name = N'RepairedTime';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�`�״_�ɶ�', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReport', @level2type = N'COLUMN', @level2name = N'ResolveTime';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�[�u�q�t�d���', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReport', @level2type = N'COLUMN', @level2name = N'SubProResponseTeamID';


GO

EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'�ۭq���', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReport', @level2type = N'COLUMN', @level2name = N'CustomColumn1';


GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Operator Name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'OperatorName'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'記錄哪間工廠的資料，ex PH1, PH2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'BIFactoryID'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'時間戳記，紀錄寫入table時間' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'P_SubProInsReport', @level2type=N'COLUMN',@level2name=N'BIInsertDate'
GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'是否傳回台北', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'P_SubProInsReport', @level2type = N'COLUMN', @level2name = N'BIStatus';

