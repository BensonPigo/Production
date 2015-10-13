/*
不要變更資料庫路徑或名稱變數。
任何 sqlcmd 變數都會在組建和部署期間
經過適當取代。
*/
ALTER DATABASE [$(DatabaseName)]
	ADD FILE
	(
		NAME = [Production],
		FILENAME = '$(DefaultDataPath)$(DefaultFilePrefix)_Production.mdf',
		SIZE = 2 GB,
		FILEGROWTH = 1 GB
	)
GO	

ALTER DATABASE [$(DatabaseName)]
ADD LOG FILE
(
	NAME = [Production_log],
	FILENAME = '$(DefaultLogPath)$(DefaultFilePrefix)_Production.ldf',
	SIZE = 2048 MB,
	FILEGROWTH = 256 MB
)
