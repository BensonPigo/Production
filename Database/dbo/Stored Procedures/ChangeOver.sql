﻿
-- =============================================
-- Author:		<Alger Song>
-- Create date: <2017/01/24>
-- Description:	<ChangeOver>
-- =============================================
CREATE PROCEDURE [dbo].[ChangeOver]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

   --刪除不存在Sewing Schedule的資料
	delete from ChgOver 
	where not exists (select 1 
					  from SewingSchedule s, Factory f 
					  where s.APSNo = ChgOver.APSNo and s.FactoryID = f.ID and f.IsSampleRoom = 0);

	--更新現有資料
	update ChgOver set Inline = s.Inline,AlloQty = s.AlloQty,StandardOutput = s.StandardOutput,TotalSewingTime = s.TotalSewingTime
	from SewingSchedule s where s.APSNo = ChgOver.APSNo;
	update ChgOver set StyleID = o.StyleID,CDCodeID = o.CdCodeID, SeasonID = o.SeasonID
	from Orders o where o.ID = ChgOver.OrderID;

	--產生ChgOver資料
	Declare cursor_tmpSewing Cursor for
	select s.FactoryID,s.SewingLineID,s.Inline,s.APSNo,s.ComboType,s.AlloQty,s.TotalSewingTime,s.StandardOutput,
	isnull(o.StyleID,'') as StyleID,isnull(o.SeasonID,'') as SeasonID,o.CdCodeID,s.OrderID,
	LAG(isnull(o.StyleID,'')+s.ComboType,1,'') OVER (Partition by s.FactoryID,s.SewingLineID Order by s.FactoryID,s.SewingLineID,s.Inline) as Compare
	from SewingSchedule s
	left join Orders o on s.OrderID = o.ID
	left join Factory f on s.FactoryID = f.ID
	where s.Inline is not null 
	and s.Offline > DATEADD(MONTH,-1,GETDATE()) 
	and f.IsSampleRoom = 0
	order by s.FactoryID,s.SewingLineID,s.Inline;


	--宣告變數: 記錄程式中的資料
	DECLARE @factoryid VARCHAR(8), --工廠別
			@sewinglineid VARCHAR(2), --Sewing Line ID
			@inline DATETIME, --上線日 
			@apsno INT, --APS系統Sewing Schedule的ID
			@combotype VARCHAR(1), --組合型態
			@alloqty INT, --生產數量
			@ttlsewingtime INT, --總秒數
			@stdoutput INT, --Standard Output 
			@styleid VARCHAR(15), --款式
			@seasonid VARCHAR(10), --季節
			@cdcodeid VARCHAR(6), --CD Code
			@orderid VARCHAR(13), --訂單編號
			@compare VARCHAR(26), --紀錄上一筆的StyleID+ComboType
			@type VARCHAR(1), --New/Repeat
			@chgoverid INT, --紀錄ChgOver.ID
			@chgoverinline DATETIME --紀錄ChgOver.Inline

	--開始run cursor
	OPEN cursor_tmpSewing
	--將第一筆資料填入變數
	FETCH NEXT FROM cursor_tmpSewing INTO @factoryid,@sewinglineid,@inline,@apsno,@combotype,@alloqty,@ttlsewingtime,@stdoutput,@styleid,@seasonid,@cdcodeid,@orderid,@compare
	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF @compare <> ''
			BEGIN
				IF @styleid+@combotype <> @compare --換款式
					BEGIN
						IF EXISTS(select 1 from ChgOver where APSNo = @apsno and FactoryID = @factoryid and SewingLineID = @sewinglineid and OrderID = @orderid and ComboType = @combotype)
							BEGIN
								select @chgoverid = ID,@chgoverinline = Inline
								from ChgOver 
								where FactoryID = @factoryid and SewingLineID = @sewinglineid and StyleID = @styleid and ComboType = @combotype;
								IF @chgoverinline < @inline
									SET @type = 'R'
								ELSE
									BEGIN
										SET @type = 'N'
										update ChgOver set Type = 'R' where ID = @chgoverid
									END
								select @chgoverid = ID from ChgOver where APSNo = @apsno and FactoryID = @factoryid and SewingLineID = @sewinglineid and OrderID = @orderid and ComboType = @combotype
								update ChgOver set Type = @type, Inline = @inline where ID = @chgoverid
							END
						ELSE
							BEGIN
								select @chgoverid = ID,@chgoverinline = Inline
								from ChgOver 
								where FactoryID = @factoryid and SewingLineID = @sewinglineid and StyleID = @styleid and ComboType = @combotype;
								IF @chgoverinline < @inline
									SET @type = 'R'
								ELSE
									BEGIN
										SET @type = 'N'
										update ChgOver set Type = 'R' where ID = @chgoverid
									END
								insert into ChgOver (OrderID,ComboType,FactoryID,StyleID,SeasonID,SewingLineID,CDCodeID,Inline,TotalSewingTime,AlloQty,StandardOutput,Type,Status,AddDate)
								values (@orderid,@combotype,@factoryid,@styleid,@seasonid,@sewinglineid,@cdcodeid,@inline,@ttlsewingtime,@alloqty,@stdoutput,@type,'NEW',GETDATE())
							END
					END
			END
		FETCH NEXT FROM cursor_tmpSewing INTO @factoryid,@sewinglineid,@inline,@apsno,@combotype,@alloqty,@ttlsewingtime,@stdoutput,@styleid,@seasonid,@cdcodeid,@orderid,@compare
	END
	CLOSE cursor_tmpSewing
	DEALLOCATE cursor_tmpSewing

	--刪除連續的StyleID+ComboType
	select *
	from (select ID,StyleID,ComboType,FactoryID,SewingLineID,StyleID+ComboType as CurrentRec,LAG(StyleID+ComboType,1,'') OVER (Partition by FactoryID,SewingLineID Order by FactoryID,SewingLineID,Inline,ID) as LastRec
		  from ChgOver) a
	where a.LastRec <> ''
	and a.CurrentRec = a.LastRec;

	--刪除已不存在ChgOver的ChgOver_Check & ChgOver_Problem
	delete from ChgOver_Check where not exists (select 1 from ChgOver where ChgOver.ID = ChgOver_Check.ID);
	delete from ChgOver_Problem where not exists (select 1 from ChgOver where ChgOver.ID = ChgOver_Problem.ID);


	--填Category欄位值
	update ChgOver set Category = c.Category
	from (select ID, (case when b.ProductionType <> b.LastProdType and b.FabricType <> b.LastFabType then 'A'
						   when b.ProductionType <> b.LastProdType and b.FabricType = b.LastFabType then 'B'
						   when b.ProductionType = b.LastProdType and b.FabricType <> b.LastFabType then 'C'
						   when b.ProductionType = b.LastProdType and b.FabricType = b.LastFabType then 'D'
						   else ''
					  end) as Category
		  from (select ID,ProductionType,FabricType,LAG(ProductionType,1,'') OVER (Partition by a.FactoryID,a.SewingLineID order by a.FactoryID,a.SewingLineID,a.Inline) as LastProdType,
					LAG(FabricType,1,'') OVER (Partition by a.FactoryID,a.SewingLineID order by a.FactoryID,a.SewingLineID,a.Inline) as LastFabType
				from (select co.ID,co.FactoryID,co.SewingLineID,co.StyleID,co.ComboType,co.Inline,
						  isnull(case when co.ComboType = 'T' then cc.TopProductionType when co.ComboType = 'B' then cc.BottomProductionType when co.ComboType = 'I' then cc.InnerProductionType when co.ComboType = 'O' then cc.OuterProductionType else '' end,'') as ProductionType,
						  isnull(case when co.ComboType = 'T' then cc.TopFabricType when co.ComboType = 'B' then cc.BottomFabricType when co.ComboType = 'I' then cc.InnerFabricType when co.ComboType = 'O' then cc.OuterFabricType else '' end,'') as FabricType
					  from ChgOver co
					  left join CDCode_Content cc on co.CDCodeID = cc.ID) a) b
		  where b.LastProdType <> '') c
	where c.ID = ChgOver.ID



END