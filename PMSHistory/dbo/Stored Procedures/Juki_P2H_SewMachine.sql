-- =============================================
/*
將Juki-JaNets System需要的資料, 透過工廠系統觸發的點進行資料傳送每個交易紀錄至PMSHistory 當記錄
先新增PMSHistory資料, 並將新增Ukey 保存起來
再來更新Machine 如果更新失敗,則刪除剛剛新增的PMSHistory資料
*/
-- =============================================
CREATE PROCEDURE  [dbo].[Juki_P2H_SewMachine]
	 
AS
BEGIN
	SET NOCOUNT ON;
    DECLARE @MachineIDs TABLE (MachineID VARCHAR(20));  -- 暫存要更新的Machine

	 -- 暫存 Insert 後的 Identity Key（Ukey）與 MachineID
    DECLARE @InsertedJuki TABLE (
        Ukey bigint,
        MachineID VARCHAR(20)
    );

BEGIN TRY
	
		-- 防呆 JukiExchangeDBActive = 1 才能執行, 否則跳出
		IF NOT EXISTS(SELECT 1 FROM Production.dbo.[System] WHERE JukiExchangeDBActive = 1)
			return ;

		-- 撈取Machine資料
		select 
		 [FactoryID] = isnull(ml.FactoryID,'')
		,[SewingCell] = m.SewingCell
		,[MachineGroup] = CONCAT(m.MasterGroupID,m.MachineGroupID)
		,[MachineID] = m.ID
		,[MachineBrandID] = m.MachineBrandID
		,[ConnType] = m.JukiConnType
		,[ConnImg] = m.JukiConnImg
		,[DetectionType] = m.JukiDetectionType
		,[AddDate] = GETDATE()
		,m.Junk
		,m.JukiSewMachineDataSubmitDate
		into #tmpMachine
		from [ExtendServer].Machine.dbo.Machine m with(nolock)
		left join [ExtendServer].Machine.dbo.MachineLocation ml with(nolock) on ml.ID = m.MachineLocationID and ml.MDivisionID = m.LocationM
		WHERE m.Status in ('Good','Repairing')

		-- 取的同MachineID 最後一筆History紀錄
		select * 
		into #tmp_Juki_T_SewMachine_Last
		from PMSHistory.dbo.Juki_T_SewMachine t with(nolock)
		where exists(
			select 1 from #tmpMachine s
			where s.MachineID = t.MachineID
		)
		and t.Ukey = (
			select MAX(t1.Ukey) 
			from PMSHistory.dbo.Juki_T_SewMachine t1 with(nolock) 
			where t1.MachineID = t.MachineID
		)
		/* ----------------------------------------------------------
		   AddDate 依 Flag 調整（全系列統一規則）

		   ■ 寫入 PMSHistory.dbo.Juki_T_SewMachine 時：
			 - Flag = 3  → AddDate = DATEADD(ms,-6,GETDATE())
			 - Flag = 2  → AddDate = DATEADD(ms,-3,GETDATE())
			 - Flag = 1  → AddDate = GETDATE()

		   ■ 目前判斷 Flag 的邏輯主要依 Ukey 與欄位差異，
			 不用比較 AddDate，因此讀取端無需還原毫秒；
			 若日後需以 AddDate 判斷，請於查詢端加回毫秒
			 （Flag 3 +6ms、Flag 2 +3ms）即可。
		---------------------------------------------------------------- */

		-- Flag = 1.新增 2.修改 3.?除
		-- 計算 Flag 並插入異動記錄
		   SELECT 
                t.FactoryID, t.SewingCell, t.MachineGroup, t.MachineID,
                t.MachineBrandID, t.ConnType, t.ConnImg, t.DetectionType,
                t.AddDate,
                Flag = 
                    CASE
                        WHEN t.Junk = 0 
						AND NOT EXISTS (
                            SELECT 1 FROM PMSHistory.dbo.Juki_T_SewMachine s 
                            WHERE s.MachineID = t.MachineID
                        ) OR
						EXISTS(
							SELECT 1 FROM #tmp_Juki_T_SewMachine_Last s
							where s.MachineID = t.MachineID and s.Flag = 3
						)
						THEN 1 -- "新增" 不需要拿最後一筆比對,因為Machine 不會被刪除只能Junk, 但有可能IT直接修改SQL解除Junk, 就視為新增
                        WHEN t.Junk = 0 AND EXISTS (
                            SELECT 1 FROM #tmp_Juki_T_SewMachine_Last s 
                            WHERE s.MachineID = t.MachineID AND s.Flag != 3
                            AND (
                                s.SewingCell != t.SewingCell OR
                                s.ConnType != t.ConnType OR
                                s.MachineBrandID != t.MachineBrandID OR
                                s.DetectionType != t.DetectionType
                            )
                        ) THEN 2 -- "修改", 要用最後一筆歷史紀錄比對, 這樣才能比對特定欄位不一樣才寫入History
                        WHEN t.Junk = 1 AND EXISTS (
                            SELECT 1 FROM PMSHistory.dbo.Juki_T_SewMachine s 
                            WHERE s.MachineID = t.MachineID AND s.Flag != 3
                        ) THEN 3 -- "刪除", 不需要和最後一筆比對, 因為相同MachineID 只會有一個Junk, 且不能恢復
                        ELSE 0
                    END
			into #tmpInsertJuki
            FROM #tmpMachine t 
			;
			/* 依 Flag 調整 AddDate 毫秒差 */
			UPDATE #tmpInsertJuki
			SET AddDate = CASE 
						   WHEN Flag = 3 THEN DATEADD(ms,-6, AddDate)
						   WHEN Flag = 2 THEN DATEADD(ms,-3, AddDate)
						   ELSE AddDate
						  END
			;

			-- Insert 並抓出新增的 Identity（Ukey）
            INSERT INTO PMSHistory.dbo.Juki_T_SewMachine (
                FactoryID, SewingCell, MachineGroup, MachineID,
                MachineBrandID, ConnType, ConnImg, DetectionType,
                Flag, AddDate
            )
			OUTPUT INSERTED.Ukey, INSERTED.MachineID INTO @InsertedJuki(Ukey, MachineID)
            SELECT 
                FactoryID, SewingCell, MachineGroup, MachineID,
                MachineBrandID, ConnType, ConnImg, DetectionType,
                Flag, AddDate
            FROM #tmpInsertJuki
            WHERE Flag IN (1, 2, 3); --已經篩選,只更新Flag != 0 就好

			  -- 收集要更新的 MachineID
			INSERT INTO @MachineIDs(MachineID)
			SELECT MachineID FROM @InsertedJuki;

		BEGIN TRY
			update m
			set m.JukiSewMachineDataSubmitDate = GETDATE()
			from [ExtendServer].Machine.dbo.Machine m
			inner join @MachineIDs t on m.ID = t.MachineID
			where m.Status in ('Good','Repairing')

		END TRY
        BEGIN CATCH
            -- 更新失敗補償刪除剛剛新增的 PMSHistory 資料
            DELETE FROM PMSHistory.dbo.Juki_T_SewMachine
            WHERE Ukey IN (SELECT Ukey FROM @InsertedJuki);

            DECLARE @ErrMsg NVARCHAR(4000) = 'ExtendServer Machine 更新失敗: ' + ERROR_MESSAGE();
            RAISERROR(@ErrMsg, 16, 1);
        END CATCH
    END TRY
    BEGIN CATCH
        DECLARE @msg NVARCHAR(4000) = 'PMSHistory 更新錯誤: ' + ERROR_MESSAGE();
        RAISERROR(@msg, 16, 1);
    END CATCH
END