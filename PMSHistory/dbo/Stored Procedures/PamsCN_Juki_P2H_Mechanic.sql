CREATE Procedure [dbo].[Juki_P2H_Mechanic]

AS
BEGIN
	SET NOCOUNT ON;

BEGIN TRY
BEGIN TRANSACTION; -- 開始交易
	-- 防呆 JukiExchangeDBActive = 1 才能執行, 否則跳出
	IF EXISTS(SELECT 1 FROM Production.dbo.[System] WHERE JukiExchangeDBActive = 1)
	BEGIN
		SELECT e.ID, [Name] = PMSHistory.dbo.GetFirstName(e.[Name]), e.JukiRFIDNo 
		INTO #tmp_Employee
		FROM PamsCN.dbo.Employee e
		WHERE e.WorkTypeID = 2

		SELECT j.*
		INTO #tmp_Juki_T_Mechanic_Last
		FROM PMSHistory.dbo.Juki_T_Mechanic j
		WHERE EXISTS (SELECT 1 FROM #tmp_Employee t WHERE j.EmployeeID = t.ID)
		AND j.Ukey = (SELECT MAX(j2.Ukey) FROM PMSHistory.dbo.Juki_T_Mechanic j2 WHERE j.EmployeeID = j2.EmployeeID)

		-- 新增 (沒資料)
		SELECT e.ID, e.[Name], e.JukiRFIDNo
			, [Flag] = 1
			, [AddDate] = GETDATE()
		INTO #tmp_Employee_toJuki
		FROM #tmp_Employee e
		WHERE NOT EXISTS (SELECT 1 FROM #tmp_Juki_T_Mechanic_Last lj WHERE e.ID = lj.EmployeeID)		
		UNION ALL
		-- 新增 (最後一筆為刪除 3)
		-- 修改 (最後一筆為新增 1 或 修改 2)
		SELECT e.ID, e.[Name], e.JukiRFIDNo
			, [Flag] = CASE WHEN lj.Flag =3 THEN 1
					   ELSE 2
					   END
			, [AddDate] = GETDATE()
		FROM #tmp_Employee e
		INNER JOIN #tmp_Juki_T_Mechanic_Last lj ON e.ID = lj.EmployeeID
		WHERE e.[Name] <> lj.EmployeeName
		OR e.JukiRFIDNo <> lj.RFIDNo
		UNION ALL
		-- 刪除 (不存在原表的 且 最後一筆不為3)
		SELECT j.EmployeeID, j.EmployeeName, j.RFIDNo
			, [Flag] = 3
			, [AddDate] = GETDATE()
		FROM PMSHistory.dbo.Juki_T_Mechanic j
		WHERE NOT EXISTS (SELECT 1 FROM #tmp_Employee e WHERE j.EmployeeID = e.ID)
		AND j.Ukey = (SELECT MAX(j2.Ukey) FROM PMSHistory.dbo.Juki_T_Mechanic j2 WHERE j.EmployeeID = j2.EmployeeID)
		AND j.Flag != 3
		;
		/* ----------------------------------------------------------
		   Juki_T_Mechanic.AddDate 依 Flag 調整（毫秒位移規則）
		   ‣ 寫入 PMSHistory.dbo.Juki_T_Mechanic 前：
			   - Flag = 3 → AddDate = DATEADD(ms,-6,GETDATE())  (刪除)
			   - Flag = 2 → AddDate = DATEADD(ms,-3,GETDATE())  (修改)
			   - Flag = 1 → AddDate = GETDATE()                 (新增)
		   ‣ 目前本流程並未以 AddDate 做歷史比對，因此讀取端無需還原毫秒。
		---------------------------------------------------------------- */
		/* 依 Flag 調整 AddDate 毫秒差 */
        UPDATE #tmp_Employee_toJuki
        SET    AddDate = CASE
                            WHEN Flag = 3 THEN DATEADD(ms,-6, AddDate)
                            WHEN Flag = 2 THEN DATEADD(ms,-3, AddDate)
                            ELSE AddDate
                            END;
		;
		INSERT INTO PMSHistory.dbo.Juki_T_Mechanic([EmployeeID], [EmployeeName], [RFIDNo], [Flag], [AddDate])
		SELECT e.ID, e.[Name], e.JukiRFIDNo, e.Flag, e.AddDate
		FROM #tmp_Employee_toJuki e

		UPDATE e
			SET e.JukiMechanicDataSubmitDate = GETDATE()
		FROM PamsCN.dbo.Employee e
		WHERE EXISTS (SELECT 1 FROM #tmp_Employee_toJuki j WHERE e.ID = j.ID)

	END
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
GO