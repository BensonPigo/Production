CREATE TABLE [dbo].[ThreadTransfer_Detail] (
    [ID]            VARCHAR (13)  CONSTRAINT [DF_ThreadTransfer_Detail_ID] DEFAULT ('') NOT NULL,
    [Refno]         VARCHAR (36)  CONSTRAINT [DF_ThreadTransfer_Detail_Refno] DEFAULT ('') NOT NULL,
    [ThreadColorID] VARCHAR (15)  CONSTRAINT [DF_ThreadTransfer_Detail_ThreadColorID] DEFAULT ('') NOT NULL,
    [LocationFrom]  VARCHAR (10)  CONSTRAINT [DF_ThreadTransfer_Detail_LocationFrom] DEFAULT ('') NOT NULL,
    [LocationTo]    VARCHAR (10)  CONSTRAINT [DF_ThreadTransfer_Detail_LocationTo] DEFAULT ('') NOT NULL,
    [NewCone]       NUMERIC (5)   CONSTRAINT [DF_ThreadTransfer_Detail_NewCone] DEFAULT ((0)) NULL,
    [UsedCone]      NUMERIC (5)   CONSTRAINT [DF_ThreadTransfer_Detail_UsedCone] DEFAULT ((0)) NULL,
    [Remark]        NVARCHAR (60) CONSTRAINT [DF_ThreadTransfer_Detail_Remark] DEFAULT ('') NULL,
    CONSTRAINT [PK_ThreadTransfer_Detail] PRIMARY KEY CLUSTERED ([ID] ASC, [Refno] ASC, [ThreadColorID] ASC, [LocationFrom] ASC, [LocationTo] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Thread Stock Transfer Detail', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadTransfer_Detail';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'線轉庫存單號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadTransfer_Detail', @level2type = N'COLUMN', @level2name = N'ID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'料號', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadTransfer_Detail', @level2type = N'COLUMN', @level2name = N'Refno';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'線色', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadTransfer_Detail', @level2type = N'COLUMN', @level2name = N'ThreadColorID';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'原Location', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadTransfer_Detail', @level2type = N'COLUMN', @level2name = N'LocationFrom';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'轉置Location', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadTransfer_Detail', @level2type = N'COLUMN', @level2name = N'LocationTo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'New Cone 數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadTransfer_Detail', @level2type = N'COLUMN', @level2name = N'NewCone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Used Cone 數量', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadTransfer_Detail', @level2type = N'COLUMN', @level2name = N'UsedCone';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'備註', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ThreadTransfer_Detail', @level2type = N'COLUMN', @level2name = N'Remark';

