
CREATE TABLE [dbo].[AccountNoSetting] (
    [ID]                  VARCHAR (8)  NOT NULL,
    [UnselectableShipB03] BIT          CONSTRAINT [DF_AccountNoSetting_UnselectableShipB03] DEFAULT ((0)) NOT NULL,
    [AddDate]             DATETIME     NULL,
    [AddName]             VARCHAR (10) CONSTRAINT [DF_AccountNoSetting_AddName] DEFAULT ('') NOT NULL,
    [EditDate]            DATETIME     NULL,
    [EditName]            VARCHAR (10) CONSTRAINT [DF_AccountNoSetting_EditName] DEFAULT ('') NOT NULL,
    CONSTRAINT [PK_AccountNoSetting] PRIMARY KEY CLUSTERED ([ID] ASC)
);



GO



GO


GO


GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'會計科目'
, @level0type=N'SCHEMA',@level0name=N'dbo'
, @level1type=N'TABLE',@level1name=N'AccountNoSetting'
, @level2type=N'COLUMN',@level2name=N'ID'

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'排除不可出現在Shipping B03 的選單' 
, @level0type=N'SCHEMA',@level0name=N'dbo'
, @level1type=N'TABLE',@level1name=N'AccountNoSetting'
, @level2type=N'COLUMN',@level2name=N'UnselectableShipB03'
;