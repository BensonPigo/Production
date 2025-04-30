-- =============================================
-- Description:	將Juki-JaNets System需要的資料, 透過工廠系統觸發的點進行資料傳送每個交易紀錄至PMSHistory 當記錄
-- =============================================
CREATE PROCEDURE  [dbo].[Juki_T_SewMachine]
	 
AS
BEGIN
	SET NOCOUNT ON;

BEGIN TRY
BEGIN TRANSACTION; -- 開始交易
	
	-- 防呆 JukiExchangeDBActive = 1 才能執行, 否則跳出
	IF EXISTS(SELECT 1 FROM Production.dbo.[System] WHERE JukiExchangeDBActive = 1)
	Begin
		-- 撈取Machine資料
		select [FactoryID] = isnull(ml.FactoryID,'')
		,[SewingCell] = m.SewingCell
		,[MachineGroup] = CONCAT(m.MasterGroupID,m.MachineGroupID)
		,[MachineID] = m.ID
		,[MachineBrandID] = m.MachineBrandID
		,[ConnType] = m.JukiConnType
		,[ConnImg] = m.JukiConnImg
		,[DetectionType] = m.JukiDetectionType
		,[AddDate] = GETDATE()
		,m.Junk
		into #tmpMachine
		from [ExtendServer].Machine.dbo.Machine m with(nolock)
		left join [ExtendServer].Machine.dbo.MachineLocation ml with(nolock) on ml.ID = m.MachineLocationID and ml.MDivisionID = m.LocationM
		WHERE m.Status in ('Good','Repairing')

		-- Flag = 1.新增 2.修改 3.删除
		 -- 計算 Flag 並插入異動記錄
            WITH MachineWithFlag AS (
                SELECT 
                    t.FactoryID, t.SewingCell, t.MachineGroup, t.MachineID,
                    t.MachineBrandID, t.ConnType, t.ConnImg, t.DetectionType,
                    t.AddDate,
                    Flag = 
                        CASE
                            WHEN t.Junk = 0 AND NOT EXISTS (
                                SELECT 1 FROM PMSHistory.dbo.Juki_T_SewMachine s 
                                WHERE s.MachineID = t.MachineID AND s.Flag != 3
                            ) THEN 1 -- 新增
                            WHEN t.Junk = 0 AND EXISTS (
                                SELECT 1 FROM PMSHistory.dbo.Juki_T_SewMachine s 
                                WHERE s.MachineID = t.MachineID AND s.Flag != 3
                                AND (
                                    s.SewingCell != t.SewingCell OR
                                    s.MachineGroup != t.MachineGroup OR
                                    s.MachineBrandID != t.MachineBrandID OR
                                    s.DetectionType != t.DetectionType
                                )
                            ) THEN 2 -- 修改
                            WHEN t.Junk = 1 AND EXISTS (
                                SELECT 1 FROM PMSHistory.dbo.Juki_T_SewMachine s 
                                WHERE s.MachineID = t.MachineID AND s.Flag != 3
                            ) THEN 3 -- 刪除
                            ELSE 0
                        END
                FROM #tmpMachine t
            )
            INSERT INTO PMSHistory.dbo.Juki_T_SewMachine (
                FactoryID, SewingCell, MachineGroup, MachineID,
                MachineBrandID, ConnType, ConnImg, DetectionType,
                Flag, AddDate
            )
            SELECT 
                FactoryID, SewingCell, MachineGroup, MachineID,
                MachineBrandID, ConnType, ConnImg, DetectionType,
                Flag, AddDate
            FROM MachineWithFlag
            WHERE Flag IN (1, 2, 3); --CTE已經篩選,只更新Flag != 0 就好
		
	end

   COMMIT TRANSACTION; 
    END TRY
    BEGIN CATCH
        -- 發生錯誤就rollback
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        -- 可以選擇把錯誤往外丟或 Log 起來
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END