CREATE TABLE NotificationList (
    Ukey        BIGINT     IDENTITY(1,1)     NOT NULL PRIMARY KEY,
    MenuName    NVARCHAR(50)    NOT NULL,
    ID          VARCHAR(2)      NOT NULL,
    Name        VARCHAR(100)    NOT NULL,
    Description NVARCHAR(100)   NOT NULL,
    Junk        BIT             NOT NULL,
    FormName    VARCHAR(80)     NOT NULL,
    AddName     VARCHAR(10)     NOT NULL,
    AddDate     DATETIME        NULL,
    EditName    VARCHAR(10)     NULL default(''),
    EditDate    DATETIME        NULL
)
go

-- 欄位描述
EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'系統模組',
    @level0type = N'SCHEMA', @level0name = 'dbo',
    @level1type = N'TABLE',  @level1name = 'NotificationList',
    @level2type = N'COLUMN', @level2name = 'MenuName';
    go

EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'規則代號',
    @level0type = N'SCHEMA', @level0name = 'dbo',
    @level1type = N'TABLE',  @level1name = 'NotificationList',
    @level2type = N'COLUMN', @level2name = 'ID';
    go

EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'名稱',
    @level0type = N'SCHEMA', @level0name = 'dbo',
    @level1type = N'TABLE',  @level1name = 'NotificationList',
    @level2type = N'COLUMN', @level2name = 'Name';
    go

EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'詳細敘述',
    @level0type = N'SCHEMA', @level0name = 'dbo',
    @level1type = N'TABLE',  @level1name = 'NotificationList',
    @level2type = N'COLUMN', @level2name = 'Description';
    go

EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'功能名稱',
    @level0type = N'SCHEMA', @level0name = 'dbo',
    @level1type = N'TABLE',  @level1name = 'NotificationList',
    @level2type = N'COLUMN', @level2name = 'FormName';
end
go