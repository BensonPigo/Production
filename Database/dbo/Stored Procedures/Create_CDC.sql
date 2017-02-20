
CREATE PROCEDURE [dbo].[Create_CDC] 
(
	@TableName varchar(50) = '', --CDC Table
	@KeepTime int = 0	--保留的時間
)
AS
BEGIN
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
--資料庫啟用CDC
exec sys.sp_cdc_enable_db


--要啟用CDC的Table
exec sys.sp_cdc_enable_table
	@source_schema = 'dbo',
	@source_name = @TableName,
	@role_name = null,
	@supports_net_changes = '1'
	
--設定CDC記錄保留天數
exec sys.sp_cdc_change_job @job_type='cleanup', @retention=@KeepTime

--@retention 指的是保留幾分鐘
--21600 = 15days * 24 hrs * 60 minutes

--檢查CDC是否有被啟用，1:啟用 0:沒啟用
select name,is_cdc_enabled from sys.databases where name = 'Production'
--確認ShareExpense已啟用CDC，1:啟用 0:沒啟用
select name,is_tracked_by_cdc from sys.tables where name = @TableName


END