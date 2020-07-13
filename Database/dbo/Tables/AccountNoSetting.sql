
CREATE TABLE [dbo].[AccountNoSetting](
	ID [varchar](8) NOT NULL,
	UnselectableShipB03 bit NULL,
	AddDate DATETIME NULL,
	AddName Varchar(10) NOT NULL,
	EditDate DATETIME NULL,
	EditName Varchar(10) NOT NULL,
	CONSTRAINT [PK_AccountNoSetting] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
;

GO

ALTER TABLE [dbo].[AccountNoSetting] 
ADD  CONSTRAINT [DF_AccountNoSetting_UnselectableShipB03]  DEFAULT (0) FOR UnselectableShipB03

GO
ALTER TABLE [dbo].[AccountNoSetting] 
ADD  CONSTRAINT [DF_AccountNoSetting_AddName]  DEFAULT ('') FOR AddName

GO
ALTER TABLE [dbo].[AccountNoSetting] 
ADD  CONSTRAINT [DF_AccountNoSetting_EditName]  DEFAULT ('') FOR EditName

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