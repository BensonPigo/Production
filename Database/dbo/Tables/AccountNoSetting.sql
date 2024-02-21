
CREATE TABLE [dbo].[AccountNoSetting] (
    [ID]                  VARCHAR (8)  NOT NULL,
    [UnselectableShipB03] BIT          CONSTRAINT [DF_AccountNoSetting_UnselectableShipB03] DEFAULT ((1)) NOT NULL,
    [AddDate]             DATETIME     NULL,
    [AddName]             VARCHAR (10) CONSTRAINT [DF_AccountNoSetting_AddName] DEFAULT ('') NOT NULL,
    [EditDate]            DATETIME     NULL,
    [EditName]            VARCHAR (10) CONSTRAINT [DF_AccountNoSetting_EditName] DEFAULT ('') NOT NULL,
    [NeedShareExpense] BIT  CONSTRAINT [DF_AccountNoSetting_NeedShareExpense] NOT NULL DEFAULT ((1)), 
    [Remark] NVARCHAR(200)  CONSTRAINT [DF_AccountNoSetting_Remark] NOT NULL DEFAULT (1), 
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
EXEC sp_addextendedproperty @name=N'MS_Description', @value=N'排除不可出現在Shipping B03的選單。0 = unchecked = 可出現;1 = checked = 不可出現' 
, @level0type=N'SCHEMA',@level0name=N'dbo'
, @level1type=N'TABLE',@level1name=N'AccountNoSetting'
, @level2type=N'COLUMN',@level2name=N'UnselectableShipB03'
;
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'勾選表示要做運費分攤。1=checked;2=unchecked',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AccountNoSetting',
    @level2type = N'COLUMN',
    @level2name = N'NeedShareExpense'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'備註',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'AccountNoSetting',
    @level2type = N'COLUMN',
    @level2name = N'Remark'