CREATE PROCEDURE [dbo].imp_FactoryEDI
	
AS
BEGIN
	set transaction isolation level read uncommitted
	
	-------------台北正式區處理方式-------------
	--------刪除：台北歷史區存在、工廠正式區存在--------
	--------新增：台北正式區存在、工廠正式區不存在--------
	--------修改：台北正式區存在、工廠正式區存在、台北歷史區不存在--------
	
	-------------台北正式區處理方式--------

	--AccessoryTest
	DELETE FROM Quality.dbo.AccessoryTest WHERE ReportNo IN (SELECT ReportNo FROM Trade_To_Pms.dbo.AccessoryTest_History) 
	Merge Quality.dbo.AccessoryTest as t
	Using (select a.* from Trade_To_Pms.dbo.AccessoryTest a ) as s
		on t.ReportNo=s.ReportNo 
	when matched then 
		update set 
			t.Region = s.Region
		  ,t.POID = s.POID
		  ,t.StyleID = s.StyleID
		  ,t.BrandID = s.BrandID
		  ,t.SeasonID = s.SeasonID
		  ,t.Article = s.Article
		  ,t.Remark = s.Remark
		  ,t.SEQ1 = s.SEQ1
		  ,t.SEQ2 = s.SEQ2
		  ,t.SCIRefno = s.SCIRefno
		  ,t.Refno = s.Refno
		  ,t.SuppID = s.SuppID
		  ,t.Color = s.Color
		  ,t.SizeSpec = s.SizeSpec
		  ,t.Result = s.Result
		  ,t.NonOven = s.NonOven
		  ,t.OvenResult = s.OvenResult
		  ,t.OvenEncode = s.OvenEncode
		  ,t.OvenTestDate = s.OvenTestDate
		  ,t.OvenScale = s.OvenScale
		  ,t.OvenInspector = s.OvenInspector
		  ,t.OvenRemark = s.OvenRemark
		  ,t.NonWash = s.NonWash
		  ,t.WashResult = s.WashResult
		  ,t.WashEncode = s.WashEncode
		  ,t.WashTestDate = s.WashTestDate
		  ,t.WashScale = s.WashScale
		  ,t.WashInspector = s.WashInspector
		  ,t.WashRemark = s.WashRemark
		  ,t.AddName = s.AddName
		  ,t.AddDate = s.AddDate
		  ,t.EditName = s.EditName
		  ,t.EditDate = s.EditDate
		  ,t.MachineWash = s.MachineWash
		  ,t.WashingTemperature = s.WashingTemperature
		  ,t.DryProcess = s.DryProcess
		  ,t.MachineModel = s.MachineModel
		  ,t.WashingCycle = s.WashingCycle
	when not matched by target then
		insert (
			ReportNo,Region,POID,StyleID,BrandID,SeasonID,Article,Remark,SEQ1,SEQ2,SCIRefno,Refno
		  ,SuppID,Color,SizeSpec,Result,NonOven,OvenResult,OvenEncode,OvenTestDate,OvenScale,OvenInspector,OvenRemark
		  ,NonWash,WashResult,WashEncode,WashTestDate,WashScale,WashInspector,WashRemark,AddName,AddDate,EditName,EditDate
		  ,MachineWash,WashingTemperature,DryProcess,MachineModel,WashingCycle
		) values (
			s.ReportNo,s.Region,s.POID,s.StyleID,s.BrandID,s.SeasonID,s.Article,s.Remark,s.SEQ1,s.SEQ2,s.SCIRefno,s.Refno
		  ,s.SuppID,s.Color,s.SizeSpec,s.Result,s.NonOven,s.OvenResult,s.OvenEncode,s.OvenTestDate,s.OvenScale,s.OvenInspector,s.OvenRemark
		  ,s.NonWash,s.WashResult,s.WashEncode,s.WashTestDate,s.WashScale,s.WashInspector,s.WashRemark,s.AddName,s.AddDate,s.EditName,s.EditDate
		  ,s.MachineWash,s.WashingTemperature,s.DryProcess,s.MachineModel,s.WashingCycle
		)
	;
	INSERT INTO Quality.dbo.AccessoryTest_History
		  (HistoryUkey,ReportNo,Region,POID,StyleID,BrandID,SeasonID,Article,Remark,SEQ1,SEQ2,SCIRefno,Refno,SuppID,Color,SizeSpec,Result,NonOven,OvenResult,OvenEncode,OvenTestDate,OvenScale,OvenInspector,OvenRemark,NonWash,WashResult,WashEncode,WashTestDate,WashScale,WashInspector,WashRemark,AddName,AddDate,EditName,EditDate,MachineWash,WashingTemperature,DryProcess,MachineModel,WashingCycle,DeleteDate)
	SELECT HistoryUkey,ReportNo,Region,POID,StyleID,BrandID,SeasonID,Article,Remark,SEQ1,SEQ2,SCIRefno,Refno,SuppID,Color,SizeSpec,Result,NonOven,OvenResult,OvenEncode,OvenTestDate,OvenScale,OvenInspector,OvenRemark,NonWash,WashResult,WashEncode,WashTestDate,WashScale,WashInspector,WashRemark,AddName,AddDate,EditName,EditDate,MachineWash,WashingTemperature,DryProcess,MachineModel,WashingCycle,DeleteDate
	FROM Trade_To_Pms.dbo.AccessoryTest_History 
	WHERE ReportNo NOT IN (SELECT ReportNo FROM Quality.dbo.AccessoryTest_History) 
	
	--PullingTest
	DELETE FROM Quality.dbo.PullingTest WHERE ReportNo IN (SELECT ReportNo FROM Trade_To_Pms.dbo.PullingTest_History) 	
	Merge Quality.dbo.PullingTest as t
	Using (select a.* from Trade_To_Pms.dbo.PullingTest a ) as s
		on t.ReportNo=s.ReportNo 
	when matched then 
		update set 
			t.POID=s.POID
           ,t.StyleID=s.StyleID
           ,t.SeasonID=s.SeasonID
           ,t.BrandID=s.BrandID
           ,t.Article=s.Article
           ,t.SizeCode=s.SizeCode
           ,t.TestDate=s.TestDate
           ,t.Result=s.Result
           ,t.Inspector=s.Inspector
           ,t.TestItem=s.TestItem
           ,t.PullForceUnit=s.PullForceUnit
           ,t.PullForce=s.PullForce
           ,t.Time=s.Time
           ,t.FabricRefno=s.FabricRefno
           ,t.AccRefno=s.AccRefno
           ,t.SnapOperator=s.SnapOperator
           ,t.Remark=s.Remark
           ,t.AddDate=s.AddDate
           ,t.AddName=s.AddName
           ,t.EditDate=s.EditDate
           ,t.EditName=s.EditName
           ,t.Region=s.Region
	when not matched by target then
		insert (
			ReportNo,POID,StyleID,SeasonID,BrandID,Article,SizeCode,TestDate,Result,Inspector,TestItem,PullForceUnit,PullForce,Time
           ,FabricRefno,AccRefno,SnapOperator,Remark,AddDate,AddName,EditDate,EditName,Region
		) values (
			s.ReportNo,s.POID,s.StyleID,s.SeasonID,s.BrandID,s.Article,s.SizeCode,s.TestDate,s.Result,s.Inspector,s.TestItem,s.PullForceUnit,s.PullForce,s.Time
           ,s.FabricRefno,s.AccRefno,s.SnapOperator,s.Remark,s.AddDate,s.AddName,s.EditDate,s.EditName,s.Region
		)
	;
	INSERT INTO Quality.dbo.PullingTest_History( 
			HistoryUkey,ReportNo,POID,StyleID,SeasonID,BrandID,Article,SizeCode,TestDate,Result,Inspector,TestItem,PullForceUnit,PullForce,Time
           ,FabricRefno,AccRefno,SnapOperator,Remark,AddDate,AddName,EditDate,EditName,Region,DeleteDate)
	SELECT  HistoryUkey,ReportNo,POID,StyleID,SeasonID,BrandID,Article,SizeCode,TestDate,Result,Inspector,TestItem,PullForceUnit,PullForce,Time
           ,FabricRefno,AccRefno,SnapOperator,Remark,AddDate,AddName,EditDate,EditName,Region,DeleteDate
	FROM Trade_To_Pms.dbo.PullingTest_History
	WHERE ReportNo NOT IN (SELECT ReportNo FROM Quality.dbo.PullingTest_History) 

	--FabricColorFastness
	DELETE FROM Quality.dbo.FabricColorFastness WHERE ReportNo IN (SELECT ReportNo FROM Trade_To_Pms.dbo.FabricColorFastness_History) 	
	Merge Quality.dbo.FabricColorFastness as t
	Using (select a.* from Trade_To_Pms.dbo.FabricColorFastness a ) as s
		on t.ReportNo=s.ReportNo 
	when matched then 
		update set 
            t.POID=s.POID
           ,t.StyleID=s.StyleID
           ,t.SeasonID=s.SeasonID
           ,t.BrandID=s.BrandID
           ,t.Article=s.Article
           ,t.TestDate=s.TestDate
           ,t.Result=s.Result
           ,t.Inspector=s.Inspector
           ,t.Temperature=s.Temperature
           ,t.Cycle=s.Cycle
           ,t.CycleTime=s.CycleTime
           ,t.Detergent=s.Detergent
           ,t.Machine=s.Machine
           ,t.Drying=s.Drying
           ,t.Remark=s.Remark
           ,t.Status=s.Status
           ,t.AddDate=s.AddDate
           ,t.AddName=s.AddName
           ,t.EditDate=s.EditDate
           ,t.EditName=s.EditName
           ,t.Region=s.Region
	when not matched by target then
		insert (
           ReportNo,POID,StyleID,SeasonID,BrandID,Article,TestDate,Result,Inspector,Temperature,Cycle,CycleTime,Detergent,Machine,Drying,Remark,Status
           ,AddDate,AddName,EditDate,EditName,Region
		) values (
			s.ReportNo,s.POID,s.StyleID,s.SeasonID,s.BrandID,s.Article,s.TestDate,s.Result,s.Inspector,s.Temperature,s.Cycle,s.CycleTime,s.Detergent,s.Machine,s.Drying,s.Remark,s.Status
           ,s.AddDate,s.AddName,s.EditDate,s.EditName,s.Region
		)
	;
	INSERT INTO Quality.dbo.FabricColorFastness_History( 
			HistoryUkey,ReportNo,POID,StyleID,SeasonID,BrandID,Article,TestDate,Result,Inspector,Temperature,Cycle,CycleTime,Detergent,Machine,Drying,Remark,Status
           ,AddDate,AddName,EditDate,EditName,Region,DeleteDate)
	SELECT  HistoryUkey,ReportNo,POID,StyleID,SeasonID,BrandID,Article,TestDate,Result,Inspector,Temperature,Cycle,CycleTime,Detergent,Machine,Drying,Remark,Status
           ,AddDate,AddName,EditDate,EditName,Region,DeleteDate
	FROM Trade_To_Pms.dbo.FabricColorFastness_History
	WHERE ReportNo NOT IN (SELECT ReportNo FROM Quality.dbo.FabricColorFastness_History) 

	--FabricColorFastness_Detail
	DELETE FROM t
	FROM Quality.dbo.FabricColorFastness_Detail t
	WHERE EXISTS (
		SELECT 1 FROM Trade_To_Pms.dbo.FabricColorFastness_Detail_History s
		WHERE t.ReportNo = s.ReportNo AND t.ColorFastnessGroup = s.ColorFastnessGroup AND t.Seq1 = s.Seq1 AND t.Seq2 = s.Seq2
	) 	
	Merge Quality.dbo.FabricColorFastness_Detail as t
	Using (select a.* from Trade_To_Pms.dbo.FabricColorFastness_Detail a ) as s
		on t.ReportNo = s.ReportNo AND t.ColorFastnessGroup = s.ColorFastnessGroup AND t.Seq1 = s.Seq1 AND t.Seq2 = s.Seq2
	when matched then 
		update set 
            t.SubmitDate = s.SubmitDate
           ,t.Roll = s.Roll
           ,t.Dyelot = s.Dyelot
           ,t.Refno = s.Refno
           ,t.SCIRefno = s.SCIRefno
           ,t.Color = s.Color
           ,t.Result = s.Result
           ,t.ChangeScale = s.ChangeScale
           ,t.ResultChange = s.ResultChange
           ,t.StainingScale = s.StainingScale
           ,t.ResultStain = s.ResultStain
           ,t.Remark = s.Remark
           ,t.EditDate = s.EditDate
           ,t.EditName = s.EditName
           ,t.AcetateScale = s.AcetateScale
           ,t.ResultAcetate = s.ResultAcetate
           ,t.CottonScale = s.CottonScale
           ,t.ResultCotton = s.ResultCotton
           ,t.NylonScale = s.NylonScale
           ,t.ResultNylon = s.ResultNylon
           ,t.PolyesterScale = s.PolyesterScale
           ,t.ResultPolyester = s.ResultPolyester
           ,t.AcrylicScale = s.AcrylicScale
           ,t.ResultAcrylic = s.ResultAcrylic
           ,t.WoolScale = s.WoolScale
           ,t.ResultWool = s.ResultWool
           ,t.AddDate = s.AddDate
	when not matched by target then
		insert (
           ReportNo,SubmitDate,ColorFastnessGroup,Seq1,Seq2,Roll,Dyelot,Refno,SCIRefno,Color,Result,ChangeScale,ResultChange,StainingScale
           ,ResultStain,Remark,EditDate,EditName,AcetateScale,ResultAcetate,CottonScale,ResultCotton,NylonScale,ResultNylon,PolyesterScale,ResultPolyester
           ,AcrylicScale,ResultAcrylic,WoolScale,ResultWool,AddDate
		) values (
			s.ReportNo,s.SubmitDate,s.ColorFastnessGroup,s.Seq1,s.Seq2,s.Roll,s.Dyelot,s.Refno,s.SCIRefno,s.Color,s.Result,s.ChangeScale,s.ResultChange
           ,s.StainingScale,s.ResultStain,s.Remark,s.EditDate,s.EditName,s.AcetateScale,s.ResultAcetate,s.CottonScale,s.ResultCotton,s.NylonScale,s.ResultNylon
           ,s.PolyesterScale,s.ResultPolyester,s.AcrylicScale,s.ResultAcrylic,s.WoolScale,s.ResultWool,s.AddDate
		)
	;
	INSERT INTO Quality.dbo.FabricColorFastness_Detail_History( 
			HistoryUkey,ReportNo,SubmitDate,ColorFastnessGroup,Seq1,Seq2,Roll,Dyelot,Refno,SCIRefno,Color,Result,ChangeScale,ResultChange,StainingScale
           ,ResultStain,Remark,EditDate,EditName,AcetateScale,ResultAcetate,CottonScale,ResultCotton,NylonScale,ResultNylon,PolyesterScale,ResultPolyester
           ,AcrylicScale,ResultAcrylic,WoolScale,ResultWool,AddDate,DeleteDate)
	SELECT  HistoryUkey,ReportNo,SubmitDate,ColorFastnessGroup,Seq1,Seq2,Roll,Dyelot,Refno,SCIRefno,Color,Result,ChangeScale,ResultChange,StainingScale
           ,ResultStain,Remark,EditDate,EditName,AcetateScale,ResultAcetate,CottonScale,ResultCotton,NylonScale,ResultNylon,PolyesterScale,ResultPolyester
           ,AcrylicScale,ResultAcrylic,WoolScale,ResultWool,AddDate,DeleteDate
	FROM Trade_To_Pms.dbo.FabricColorFastness_Detail_History t
	WHERE NOT EXISTS (
		SELECT 1 FROM Quality.dbo.FabricColorFastness_Detail_History s
		WHERE t.ReportNo = s.ReportNo AND t.ColorFastnessGroup = s.ColorFastnessGroup AND t.Seq1 = s.Seq1 AND t.Seq2 = s.Seq2
	) 	

	--FabricOven
	DELETE FROM Quality.dbo.FabricOven WHERE ReportNo IN (SELECT ReportNo FROM Trade_To_Pms.dbo.FabricOven_History) 		
	Merge Quality.dbo.FabricOven as t
	Using (select a.* from Trade_To_Pms.dbo.FabricOven a ) as s
		on t.ReportNo=s.ReportNo 
	when matched then 
		update set 
			t.POID = s.POID
           ,t.StyleID = s.StyleID
           ,t.SeasonID = s.SeasonID
           ,t.BrandID = s.BrandID
           ,t.Article = s.Article
           ,t.TestDate = s.TestDate
           ,t.Result = s.Result
           ,t.Inspector = s.Inspector
           ,t.Remark = s.Remark
           ,t.Status = s.Status
           ,t.AddDate = s.AddDate
           ,t.AddName = s.AddName
           ,t.EditDate = s.EditDate
           ,t.EditName = s.EditName
           ,t.Region = s.Region
	when not matched by target then
		insert (
			ReportNo,POID,StyleID,SeasonID,BrandID,Article,TestDate,Result,Inspector,Remark,Status,AddDate,AddName,EditDate,EditName,Region
		) values (
			s.ReportNo,s.POID,s.StyleID,s.SeasonID,s.BrandID,s.Article,s.TestDate,s.Result,s.Inspector,s.Remark,s.Status,s.AddDate,s.AddName,s.EditDate,s.EditName,s.Region
		)
	;
	INSERT INTO Quality.dbo.FabricOven_History( 
			HistoryUkey,ReportNo,POID,StyleID,SeasonID,BrandID,Article,TestDate,Result,Inspector,Remark,Status,AddDate,AddName,EditDate,EditName,Region,DeleteDate)
	SELECT  HistoryUkey,ReportNo,POID,StyleID,SeasonID,BrandID,Article,TestDate,Result,Inspector,Remark,Status,AddDate,AddName,EditDate,EditName,Region,DeleteDate
	FROM Trade_To_Pms.dbo.FabricOven_History t
	WHERE ReportNo NOT IN (SELECT ReportNo FROM Quality.dbo.FabricOven_History) 		

	--FabricOven_Detail
	DELETE FROM t
	FROM Quality.dbo.FabricOven_Detail t
	WHERE EXISTS (
		SELECT 1 FROM Trade_To_Pms.dbo.FabricOven_Detail_History s
		WHERE t.ReportNo = s.ReportNo AND t.OvenGroup = s.OvenGroup AND t.Seq1 = s.Seq1 AND t.Seq2 = s.Seq2
	)
	Merge Quality.dbo.FabricOven_Detail as t
	Using (select a.* from Trade_To_Pms.dbo.FabricOven_Detail a ) as s
		on t.ReportNo=s.ReportNo AND t.OvenGroup=s.OvenGroup AND t.Seq1=s.Seq1 AND t.Seq2=s.Seq2 
	when matched then 
		update set 
			t.SubmitDate = s.SubmitDate
           ,t.Roll = s.Roll
           ,t.Dyelot = s.Dyelot
           ,t.Refno = s.Refno
           ,t.SCIRefno = s.SCIRefno
           ,t.Color = s.Color
           ,t.Result = s.Result
           ,t.ChangeScale = s.ChangeScale
           ,t.ResultChange = s.ResultChange
           ,t.StainingScale = s.StainingScale
           ,t.ResultStain = s.ResultStain
           ,t.Remark = s.Remark
           ,t.Temperature = s.Temperature
           ,t.Time = s.Time
           ,t.EditDate = s.EditDate
           ,t.EditName = s.EditName
           ,t.AddDate = s.AddDate
	when not matched by target then
		insert (
			ReportNo,SubmitDate,OvenGroup,Seq1,Seq2,Roll,Dyelot,Refno,SCIRefno,Color,Result,ChangeScale,ResultChange,StainingScale,ResultStain,Remark
           ,Temperature,Time,EditDate,EditName,AddDate
		) values (
			s.ReportNo,s.SubmitDate,s.OvenGroup,s.Seq1,s.Seq2,s.Roll,s.Dyelot,s.Refno,s.SCIRefno,s.Color,s.Result,s.ChangeScale,s.ResultChange,s.StainingScale,s.ResultStain,s.Remark
           ,s.Temperature,s.Time,s.EditDate,s.EditName,s.AddDate
		)
	;
	INSERT INTO Quality.dbo.FabricOven_Detail_History( 
			HistoryUkey,ReportNo,SubmitDate,OvenGroup,Seq1,Seq2,Roll,Dyelot,Refno,SCIRefno,Color,Result,ChangeScale,ResultChange,StainingScale,ResultStain,Remark
           ,Temperature,Time,EditDate,EditName,AddDate,DeleteDate)
	SELECT  HistoryUkey,ReportNo,SubmitDate,OvenGroup,Seq1,Seq2,Roll,Dyelot,Refno,SCIRefno,Color,Result,ChangeScale,ResultChange,StainingScale,ResultStain,Remark
           ,Temperature,Time,EditDate,EditName,AddDate,DeleteDate
	FROM Trade_To_Pms.dbo.FabricOven_Detail_History t
	WHERE NOT EXISTS (
		SELECT 1 FROM Quality.dbo.FabricOven_Detail_History s
		WHERE t.ReportNo = s.ReportNo AND t.OvenGroup = s.OvenGroup AND t.Seq1 = s.Seq1 AND t.Seq2 = s.Seq2
	)

	--FabricTest
	DELETE FROM Quality.dbo.FabricTest WHERE ReportNo IN (SELECT ReportNo FROM Trade_To_Pms.dbo.FabricTest_History) 			
	Merge Quality.dbo.FabricTest as t
	Using (select a.* from Trade_To_Pms.dbo.FabricTest a ) as s
		on t.ReportNo=s.ReportNo
	when matched then 
		update set 
			 t.POID = s.POID
			,t.StyleID = s.StyleID
			,t.BrandID = s.BrandID
			,t.SeasonID = s.SeasonID
			,t.Article = s.Article
			,t.Remark = s.Remark
			,t.Seq1 = s.Seq1
			,t.Seq2 = s.Seq2
			,t.SCIRefno = s.SCIRefno
			,t.Refno = s.Refno
			,t.Color = s.Color
			,t.SuppID = s.SuppID
			,t.Result = s.Result
			,t.nonCrocking = s.nonCrocking
			,t.CrockingResult = s.CrockingResult
			,t.CrockingTestDate = s.CrockingTestDate
			,t.CrockingInspector = s.CrockingInspector
			,t.CrockingEncode = s.CrockingEncode
			,t.CrockingRemark = s.CrockingRemark
			,t.nonHeat = s.nonHeat
			,t.HeatResult = s.HeatResult
			,t.HeatTestDate = s.HeatTestDate
			,t.HeatInspector = s.HeatInspector
			,t.HeatEncode = s.HeatEncode
			,t.HeatRemark = s.HeatRemark
			,t.nonWash = s.nonWash
			,t.WashResult = s.WashResult
			,t.WashTestDate = s.WashTestDate
			,t.WashInspector = s.WashInspector
			,t.WashEncode = s.WashEncode
			,t.WashRemark = s.WashRemark
			,t.SkewnessOptionID = s.SkewnessOptionID
			,t.AddName = s.AddName
			,t.AddDate = s.AddDate
			,t.EditName = s.EditName
			,t.EditDate = s.EditDate
			,t.Region = s.Region

	when not matched by target then
		insert (
			ReportNo,POID,StyleID,BrandID,SeasonID,Article,Remark,Seq1,Seq2,SCIRefno,Refno,Color,SuppID,Result,nonCrocking,CrockingResult,CrockingTestDate,CrockingInspector
           ,CrockingEncode,CrockingRemark,nonHeat,HeatResult,HeatTestDate,HeatInspector,HeatEncode,HeatRemark,nonWash,WashResult,WashTestDate,WashInspector,WashEncode
           ,WashRemark,SkewnessOptionID,AddName,AddDate,EditName,EditDate,Region
		) values (
			s.ReportNo,s.POID,s.StyleID,s.BrandID,s.SeasonID,s.Article,s.Remark,s.Seq1,s.Seq2,s.SCIRefno,s.Refno,s.Color,s.SuppID,s.Result,s.nonCrocking,s.CrockingResult,s.CrockingTestDate,s.CrockingInspector
           ,s.CrockingEncode,s.CrockingRemark,s.nonHeat,s.HeatResult,s.HeatTestDate,s.HeatInspector,s.HeatEncode,s.HeatRemark,s.nonWash,s.WashResult,s.WashTestDate,s.WashInspector,s.WashEncode
           ,s.WashRemark,s.SkewnessOptionID,s.AddName,s.AddDate,s.EditName,s.EditDate,s.Region)
	;
	INSERT INTO Quality.dbo.FabricTest_History( 
			HistoryUkey,ReportNo,POID,StyleID,BrandID,SeasonID,Article,Remark,Seq1,Seq2,SCIRefno,Refno,Color,SuppID,Result,nonCrocking,CrockingResult,CrockingTestDate,CrockingInspector
           ,CrockingEncode,CrockingRemark,nonHeat,HeatResult,HeatTestDate,HeatInspector,HeatEncode,HeatRemark,nonWash,WashResult,WashTestDate,WashInspector,WashEncode
           ,WashRemark,SkewnessOptionID,AddName,AddDate,EditName,EditDate,Region,DeleteDate)
	SELECT  HistoryUkey,ReportNo,POID,StyleID,BrandID,SeasonID,Article,Remark,Seq1,Seq2,SCIRefno,Refno,Color,SuppID,Result,nonCrocking,CrockingResult,CrockingTestDate,CrockingInspector
           ,CrockingEncode,CrockingRemark,nonHeat,HeatResult,HeatTestDate,HeatInspector,HeatEncode,HeatRemark,nonWash,WashResult,WashTestDate,WashInspector,WashEncode
           ,WashRemark,SkewnessOptionID,AddName,AddDate,EditName,EditDate,Region,DeleteDate
	FROM Trade_To_Pms.dbo.FabricTest_History t
	WHERE ReportNo NOT IN (SELECT ReportNo FROM Quality.dbo.FabricTest_History) 		

	--FabricTest_Crocking
	DELETE FROM t
	FROM Quality.dbo.FabricTest_Crocking t
	WHERE EXISTS (
		SELECT 1 FROM Trade_To_Pms.dbo.FabricTest_Crocking_History s
		WHERE t.ReportNo = s.ReportNo AND t.Roll = s.Roll AND t.Dyelot = s.Dyelot
	)	
	Merge Quality.dbo.FabricTest_Crocking as t
	Using (select a.* from Trade_To_Pms.dbo.FabricTest_Crocking a ) as s
		on t.ReportNo = s.ReportNo AND t.Roll = s.Roll AND t.Dyelot = s.Dyelot
	when matched then 
		update set 
			 t.TestDate = s.TestDate
			,t.Inspector = s.Inspector
			,t.Result = s.Result
			,t.DryScale = s.DryScale
			,t.ResultDry = s.ResultDry
			,t.DryScale_Weft = s.DryScale_Weft
			,t.ResultDry_Weft = s.ResultDry_Weft
			,t.WetScale = s.WetScale
			,t.ResultWet = s.ResultWet
			,t.WetScale_Weft = s.WetScale_Weft
			,t.ResultWet_Weft = s.ResultWet_Weft
			,t.Remark = s.Remark
			,t.EditName = s.EditName
			,t.EditDate = s.EditDate
			,t.AddDate = s.AddDate
	when not matched by target then
		insert (
			ReportNo,Roll,Dyelot,TestDate,Inspector,Result,DryScale,ResultDry,DryScale_Weft,ResultDry_Weft,WetScale,ResultWet,WetScale_Weft,ResultWet_Weft,Remark,EditName,EditDate,AddDate
		) values (
			s.ReportNo,s.Roll,s.Dyelot,s.TestDate,s.Inspector,s.Result,s.DryScale,s.ResultDry,s.DryScale_Weft,s.ResultDry_Weft,s.WetScale,s.ResultWet,s.WetScale_Weft,s.ResultWet_Weft,s.Remark,s.EditName,s.EditDate,s.AddDate
		)
	;
	INSERT INTO Quality.dbo.FabricTest_Crocking_History( 
			HistoryUkey,ReportNo,Roll,Dyelot,TestDate,Inspector,Result,DryScale,ResultDry,DryScale_Weft,ResultDry_Weft,WetScale,ResultWet,WetScale_Weft,ResultWet_Weft,Remark,EditName,EditDate,AddDate,DeleteDate)
	SELECT  HistoryUkey,ReportNo,Roll,Dyelot,TestDate,Inspector,Result,DryScale,ResultDry,DryScale_Weft,ResultDry_Weft,WetScale,ResultWet,WetScale_Weft,ResultWet_Weft,Remark,EditName,EditDate,AddDate,DeleteDate
	FROM Trade_To_Pms.dbo.FabricTest_Crocking_History t
	WHERE NOT EXISTS (
		SELECT 1 FROM Quality.dbo.FabricTest_Crocking_History s
		WHERE t.ReportNo = s.ReportNo AND t.Roll = s.Roll AND t.Dyelot = s.Dyelot
	)	

	--FabricTest_Heat
	DELETE FROM t
	FROM Quality.dbo.FabricTest_Heat t
	WHERE EXISTS (
		SELECT 1 FROM Trade_To_Pms.dbo.FabricTest_Heat_History s
		WHERE t.ReportNo = s.ReportNo AND t.Roll = s.Roll AND t.Dyelot = s.Dyelot
	)	
	Merge Quality.dbo.FabricTest_Heat as t
	Using (select a.* from Trade_To_Pms.dbo.FabricTest_Heat a ) as s
		on t.ReportNo = s.ReportNo AND t.Roll = s.Roll AND t.Dyelot = s.Dyelot
	when matched then 
		update set 
			 t.TestDate = s.TestDate
			,t.Inspector = s.Inspector
			,t.Result = s.Result
			,t.Remark = s.Remark
			,t.HorizontalRate = s.HorizontalRate
			,t.HorizontalOriginal = s.HorizontalOriginal
			,t.HorizontalTest1 = s.HorizontalTest1
			,t.HorizontalTest2 = s.HorizontalTest2
			,t.HorizontalTest3 = s.HorizontalTest3
			,t.VerticalRate = s.VerticalRate
			,t.VerticalOriginal = s.VerticalOriginal
			,t.VerticalTest1 = s.VerticalTest1
			,t.VerticalTest2 = s.VerticalTest2
			,t.VerticalTest3 = s.VerticalTest3
			,t.EditName = s.EditName
			,t.EditDate = s.EditDate
			,t.AddDate = s.AddDate
	when not matched by target then
		insert (
			ReportNo,Roll,Dyelot,TestDate,Inspector,Result,Remark,HorizontalRate,HorizontalOriginal,HorizontalTest1,HorizontalTest2,HorizontalTest3,VerticalRate,VerticalOriginal,VerticalTest1,VerticalTest2
			,VerticalTest3,EditName,EditDate,AddDate
		) values (
			s.ReportNo,s.Roll,s.Dyelot,s.TestDate,s.Inspector,s.Result,s.Remark,s.HorizontalRate,s.HorizontalOriginal,s.HorizontalTest1,s.HorizontalTest2,s.HorizontalTest3,s.VerticalRate,s.VerticalOriginal,s.VerticalTest1,s.VerticalTest2
			,s.VerticalTest3,s.EditName,s.EditDate,s.AddDate
		)
	;
	INSERT INTO Quality.dbo.FabricTest_Heat_History( 
			HistoryUkey,ReportNo,Roll,Dyelot,TestDate,Inspector,Result,Remark,HorizontalRate,HorizontalOriginal,HorizontalTest1,HorizontalTest2,HorizontalTest3,VerticalRate,VerticalOriginal,VerticalTest1,VerticalTest2
			,VerticalTest3,EditName,EditDate,AddDate,DeleteDate)
	SELECT  HistoryUkey,ReportNo,Roll,Dyelot,TestDate,Inspector,Result,Remark,HorizontalRate,HorizontalOriginal,HorizontalTest1,HorizontalTest2,HorizontalTest3,VerticalRate,VerticalOriginal,VerticalTest1,VerticalTest2
			,VerticalTest3,EditName,EditDate,AddDate,DeleteDate
	FROM Trade_To_Pms.dbo.FabricTest_Heat_History t
	WHERE NOT EXISTS (
		SELECT 1 FROM Quality.dbo.FabricTest_Heat_History s
		WHERE t.ReportNo = s.ReportNo AND t.Roll = s.Roll AND t.Dyelot = s.Dyelot
	)	

	--FabricTest_Wash
	DELETE FROM t
	FROM Quality.dbo.FabricTest_Wash t
	WHERE EXISTS (
		SELECT 1 FROM Trade_To_Pms.dbo.FabricTest_Wash_History s
		WHERE t.ReportNo = s.ReportNo AND t.Roll = s.Roll AND t.Dyelot = s.Dyelot
	)	
	Merge Quality.dbo.FabricTest_Wash as t
	Using (select a.* from Trade_To_Pms.dbo.FabricTest_Wash a ) as s
		on t.ReportNo = s.ReportNo AND t.Roll = s.Roll AND t.Dyelot = s.Dyelot
	when matched then 
		update set 
			 t.TestDate = s.TestDate
			,t.Inspector = s.Inspector
			,t.Result = s.Result
			,t.Remark = s.Remark
			,t.HorizontalRate = s.HorizontalRate
			,t.HorizontalOriginal = s.HorizontalOriginal
			,t.HorizontalTest1 = s.HorizontalTest1
			,t.HorizontalTest2 = s.HorizontalTest2
			,t.HorizontalTest3 = s.HorizontalTest3
			,t.VerticalRate = s.VerticalRate
			,t.VerticalOriginal = s.VerticalOriginal
			,t.VerticalTest1 = s.VerticalTest1
			,t.VerticalTest2 = s.VerticalTest2
			,t.VerticalTest3 = s.VerticalTest3
			,t.SkewnessTest1 = s.SkewnessTest1
			,t.SkewnessTest2 = s.SkewnessTest2
			,t.SkewnessTest3 = s.SkewnessTest3
			,t.SkewnessTest4 = s.SkewnessTest4
			,t.SkewnessRate = s.SkewnessRate
			,t.EditName = s.EditName
			,t.EditDate = s.EditDate
			,t.AddDate = s.AddDate

	when not matched by target then
		insert (
			ReportNo,Roll,Dyelot,TestDate,Inspector,Result,Remark,HorizontalRate,HorizontalOriginal,HorizontalTest1,HorizontalTest2,HorizontalTest3
			,VerticalRate,VerticalOriginal,VerticalTest1,VerticalTest2,VerticalTest3,SkewnessTest1,SkewnessTest2,SkewnessTest3,SkewnessTest4,SkewnessRate
			,EditName,EditDate,AddDate
		) values (
			s.ReportNo,s.Roll,s.Dyelot,s.TestDate,s.Inspector,s.Result,s.Remark,s.HorizontalRate,s.HorizontalOriginal,s.HorizontalTest1,s.HorizontalTest2,s.HorizontalTest3
			,s.VerticalRate,s.VerticalOriginal,s.VerticalTest1,s.VerticalTest2,s.VerticalTest3,s.SkewnessTest1,s.SkewnessTest2,s.SkewnessTest3,s.SkewnessTest4,s.SkewnessRate
			,s.EditName,s.EditDate,s.AddDate
		)
	;
	INSERT INTO Quality.dbo.FabricTest_Wash_History( 
			HistoryUkey,ReportNo,Roll,Dyelot,TestDate,Inspector,Result,Remark,HorizontalRate,HorizontalOriginal,HorizontalTest1,HorizontalTest2,HorizontalTest3
			,VerticalRate,VerticalOriginal,VerticalTest1,VerticalTest2,VerticalTest3,SkewnessTest1,SkewnessTest2,SkewnessTest3,SkewnessTest4,SkewnessRate
			,EditName,EditDate,AddDate,DeleteDate)
	SELECT  HistoryUkey,ReportNo,Roll,Dyelot,TestDate,Inspector,Result,Remark,HorizontalRate,HorizontalOriginal,HorizontalTest1,HorizontalTest2,HorizontalTest3
			,VerticalRate,VerticalOriginal,VerticalTest1,VerticalTest2,VerticalTest3,SkewnessTest1,SkewnessTest2,SkewnessTest3,SkewnessTest4,SkewnessRate
			,EditName,EditDate,AddDate,DeleteDate
	FROM Trade_To_Pms.dbo.FabricTest_Wash_History t
	WHERE NOT EXISTS (
		SELECT 1 FROM Quality.dbo.FabricTest_Wash_History s
		WHERE t.ReportNo = s.ReportNo AND t.Roll = s.Roll AND t.Dyelot = s.Dyelot
	)	

	--MockupCrocking
	DELETE FROM Quality.dbo.MockupCrocking WHERE ReportNo IN (SELECT ReportNo FROM Trade_To_Pms.dbo.MockupCrocking_History) 
	Merge Quality.dbo.MockupCrocking as t
	Using (select a.* from Trade_To_Pms.dbo.MockupCrocking a ) as s
		on t.ReportNo = s.ReportNo
	when matched then 
		update set 
			 t.POID = s.POID
			,t.StyleID = s.StyleID
			,t.SeasonID = s.SeasonID
			,t.BrandID = s.BrandID
			,t.Article = s.Article
			,t.ArtworkTypeID = s.ArtworkTypeID
			,t.Remark = s.Remark
			,t.T1Subcon = s.T1Subcon
			,t.TestDate = s.TestDate
			,t.ReceivedDate = s.ReceivedDate
			,t.ReleasedDate = s.ReleasedDate
			,t.Result = s.Result
			,t.Technician = s.Technician
			,t.MR = s.MR
			,t.Type = s.Type
			,t.AddDate = s.AddDate
			,t.AddName = s.AddName
			,t.EditDate = s.EditDate
			,t.EditName = s.EditName
			,t.Region = s.Region
	when not matched by target then
		insert (
			 ReportNo,POID,StyleID,SeasonID,BrandID,Article,ArtworkTypeID,Remark,T1Subcon,TestDate,ReceivedDate,ReleasedDate,Result,Technician,MR,Type,AddDate,AddName,EditDate,EditName,Region
		) values (
			s.ReportNo,s.POID,s.StyleID,s.SeasonID,s.BrandID,s.Article,s.ArtworkTypeID,s.Remark,s.T1Subcon,s.TestDate,s.ReceivedDate,s.ReleasedDate,s.Result,s.Technician,s.MR,s.Type,s.AddDate,s.AddName,s.EditDate,s.EditName,s.Region
		)
	;
	INSERT INTO Quality.dbo.MockupCrocking_History( 
			HistoryUkey,ReportNo,POID,StyleID,SeasonID,BrandID,Article,ArtworkTypeID,Remark,T1Subcon,TestDate,ReceivedDate,ReleasedDate,Result,Technician,MR,Type,AddDate,AddName,EditDate,EditName,Region,DeleteDate)
	SELECT  HistoryUkey,ReportNo,POID,StyleID,SeasonID,BrandID,Article,ArtworkTypeID,Remark,T1Subcon,TestDate,ReceivedDate,ReleasedDate,Result,Technician,MR,Type,AddDate,AddName,EditDate,EditName,Region,DeleteDate
	FROM Trade_To_Pms.dbo.MockupCrocking_History t
	WHERE ReportNo NOT IN (SELECT ReportNo FROM Quality.dbo.MockupCrocking_History) 

	--MockupCrocking_Detail
	DELETE FROM Quality.dbo.MockupCrocking_Detail WHERE Ukey IN (SELECT Ukey FROM Trade_To_Pms.dbo.MockupCrocking_Detail_History) 	
	Merge Quality.dbo.MockupCrocking_Detail as t
	Using (select a.* from Trade_To_Pms.dbo.MockupCrocking_Detail a ) as s
		on t.Ukey = s.Ukey
	when matched then 
		update set 
			 t.Design = s.Design
			,t.ArtworkColor = s.ArtworkColor
			,t.FabricRefNo = s.FabricRefNo
			,t.FabricColor = s.FabricColor
			,t.DryScale = s.DryScale
			,t.WetScale = s.WetScale
			,t.Result = s.Result
			,t.Remark = s.Remark
			,t.EditName = s.EditName
			,t.EditDate = s.EditDate
			,t.AddDate = s.AddDate
	when not matched by target then
		insert (
			 ReportNo,Ukey,Design,ArtworkColor,FabricRefNo,FabricColor,DryScale,WetScale,Result,Remark,EditName,EditDate,AddDate
		) values (
			s.ReportNo,s.Ukey,s.Design,s.ArtworkColor,s.FabricRefNo,s.FabricColor,s.DryScale,s.WetScale,s.Result,s.Remark,s.EditName,s.EditDate,s.AddDate
		)
	;
	INSERT INTO Quality.dbo.MockupCrocking_Detail_History( 
			HistoryUkey,ReportNo,Ukey,Design,ArtworkColor,FabricRefNo,FabricColor,DryScale,WetScale,Result,Remark,EditName,EditDate,AddDate,DeleteDate)
	SELECT  HistoryUkey,ReportNo,Ukey,Design,ArtworkColor,FabricRefNo,FabricColor,DryScale,WetScale,Result,Remark,EditName,EditDate,AddDate,DeleteDate
	FROM Trade_To_Pms.dbo.MockupCrocking_Detail_History t
	WHERE Ukey NOT IN (SELECT Ukey FROM Quality.dbo.MockupCrocking_Detail_History) 	

	--MockupWash
	DELETE FROM Quality.dbo.MockupWash WHERE ReportNo IN (SELECT ReportNo FROM Trade_To_Pms.dbo.MockupWash_History) 
	Merge Quality.dbo.MockupWash as t
	Using (select a.* from Trade_To_Pms.dbo.MockupWash a ) as s
		on t.ReportNo = s.ReportNo
	when matched then 
		update set 
			 t.POID = s.POID
			,t.StyleID = s.StyleID
			,t.SeasonID = s.SeasonID
			,t.BrandID = s.BrandID
			,t.Article = s.Article
			,t.ArtworkTypeID = s.ArtworkTypeID
			,t.Remark = s.Remark
			,t.T1Subcon = s.T1Subcon
			,t.T2Supplier = s.T2Supplier
			,t.TestDate = s.TestDate
			,t.ReceivedDate = s.ReceivedDate
			,t.ReleasedDate = s.ReleasedDate
			,t.Result = s.Result
			,t.Technician = s.Technician
			,t.MR = s.MR
			,t.AddDate = s.AddDate
			,t.AddName = s.AddName
			,t.EditDate = s.EditDate
			,t.EditName = s.EditName
			,t.OtherMethod = s.OtherMethod
			,t.MethodID = s.MethodID
			,t.TestingMethod = s.TestingMethod
			,t.HTPlate = s.HTPlate
			,t.HTFlim = s.HTFlim
			,t.HTTime = s.HTTime
			,t.HTPressure = s.HTPressure
			,t.HTPellOff = s.HTPellOff
			,t.HT2ndPressnoreverse = s.HT2ndPressnoreverse
			,t.HT2ndPressreversed = s.HT2ndPressreversed
			,t.HTCoolingTime = s.HTCoolingTime
			,t.Type = s.Type
			,t.Region = s.Region
	when not matched by target then
		insert (
			 ReportNo,POID,StyleID,SeasonID,BrandID,Article,ArtworkTypeID,Remark,T1Subcon,T2Supplier,TestDate,ReceivedDate,ReleasedDate,Result,Technician,MR,AddDate,AddName,EditDate
			,EditName,OtherMethod,MethodID,TestingMethod,HTPlate,HTFlim,HTTime,HTPressure,HTPellOff,HT2ndPressnoreverse,HT2ndPressreversed,HTCoolingTime,Type,Region
		) values (
			s.ReportNo,s.POID,s.StyleID,s.SeasonID,s.BrandID,s.Article,s.ArtworkTypeID,s.Remark,s.T1Subcon,s.T2Supplier,s.TestDate,s.ReceivedDate,s.ReleasedDate,s.Result,s.Technician,s.MR,s.AddDate,s.AddName,s.EditDate
			,s.EditName,s.OtherMethod,s.MethodID,s.TestingMethod,s.HTPlate,s.HTFlim,s.HTTime,s.HTPressure,s.HTPellOff,s.HT2ndPressnoreverse,s.HT2ndPressreversed,s.HTCoolingTime,s.Type,s.Region
		)
	;
	INSERT INTO Quality.dbo.MockupWash_History( 
			HistoryUkey,ReportNo,POID,StyleID,SeasonID,BrandID,Article,ArtworkTypeID,Remark,T1Subcon,T2Supplier,TestDate,ReceivedDate,ReleasedDate,Result,Technician,MR,AddDate,AddName,EditDate
			,EditName,OtherMethod,MethodID,TestingMethod,HTPlate,HTFlim,HTTime,HTPressure,HTPellOff,HT2ndPressnoreverse,HT2ndPressreversed,HTCoolingTime,Type,Region,DeleteDate)
	SELECT  HistoryUkey,ReportNo,POID,StyleID,SeasonID,BrandID,Article,ArtworkTypeID,Remark,T1Subcon,T2Supplier,TestDate,ReceivedDate,ReleasedDate,Result,Technician,MR,AddDate,AddName,EditDate
			,EditName,OtherMethod,MethodID,TestingMethod,HTPlate,HTFlim,HTTime,HTPressure,HTPellOff,HT2ndPressnoreverse,HT2ndPressreversed,HTCoolingTime,Type,Region,DeleteDate
	FROM Trade_To_Pms.dbo.MockupWash_History t
	WHERE ReportNo NOT IN (SELECT ReportNo FROM Quality.dbo.MockupWash_History) 

	--MockupWash_Detail
	DELETE FROM Quality.dbo.MockupWash_Detail WHERE Ukey IN (SELECT Ukey FROM Trade_To_Pms.dbo.MockupWash_Detail_History) 
	Merge Quality.dbo.MockupWash_Detail as t
	Using (select a.* from Trade_To_Pms.dbo.MockupWash_Detail a ) as s
		on t.Ukey = s.Ukey
	when matched then 
		update set 
			 t.TypeofPrint = s.TypeofPrint
			,t.Design = s.Design
			,t.ArtworkColor = s.ArtworkColor
			,t.AccessoryRefno = s.AccessoryRefno
			,t.FabricRefNo = s.FabricRefNo
			,t.FabricColor = s.FabricColor
			,t.Result = s.Result
			,t.Remark = s.Remark
			,t.EditName = s.EditName
			,t.EditDate = s.EditDate
			,t.AddDate = s.AddDate
	when not matched by target then
		insert (
			 ReportNo,Ukey,TypeofPrint,Design,ArtworkColor,AccessoryRefno,FabricRefNo,FabricColor,Result,Remark,EditName,EditDate,AddDate
		) values (
			s.ReportNo,s.Ukey,s.TypeofPrint,s.Design,s.ArtworkColor,s.AccessoryRefno,s.FabricRefNo,s.FabricColor,s.Result,s.Remark,s.EditName,s.EditDate,s.AddDate
		)
	;
	INSERT INTO Quality.dbo.MockupWash_Detail_History( 
			HistoryUkey,ReportNo,Ukey,TypeofPrint,Design,ArtworkColor,AccessoryRefno,FabricRefNo,FabricColor,Result,Remark,EditName,EditDate,AddDate,DeleteDate)
	SELECT  HistoryUkey,ReportNo,Ukey,TypeofPrint,Design,ArtworkColor,AccessoryRefno,FabricRefNo,FabricColor,Result,Remark,EditName,EditDate,AddDate,DeleteDate
	FROM Trade_To_Pms.dbo.MockupWash_Detail_History t
	WHERE Ukey NOT IN (SELECT Ukey FROM Quality.dbo.MockupWash_Detail_History) 

	--MockupOven
	DELETE FROM Quality.dbo.MockupOven WHERE ReportNo IN (SELECT ReportNo FROM Trade_To_Pms.dbo.MockupOven_History) 
	Merge Quality.dbo.MockupOven as t
	Using (select a.* from Trade_To_Pms.dbo.MockupOven a ) as s
		on t.ReportNo = s.ReportNo
	when matched then 
		update set 
			 t.POID = s.POID
			,t.StyleID = s.StyleID
			,t.SeasonID = s.SeasonID
			,t.BrandID = s.BrandID
			,t.Article = s.Article
			,t.ArtworkTypeID = s.ArtworkTypeID
			,t.Remark = s.Remark
			,t.T1Subcon = s.T1Subcon
			,t.T2Supplier = s.T2Supplier
			,t.TestDate = s.TestDate
			,t.ReceivedDate = s.ReceivedDate
			,t.ReleasedDate = s.ReleasedDate
			,t.Result = s.Result
			,t.Technician = s.Technician
			,t.MR = s.MR
			,t.AddDate = s.AddDate
			,t.AddName = s.AddName
			,t.EditDate = s.EditDate
			,t.EditName = s.EditName
			,t.TestTemperature = s.TestTemperature
			,t.TestTime = s.TestTime
			,t.HTPlate = s.HTPlate
			,t.HTFlim = s.HTFlim
			,t.HTTime = s.HTTime
			,t.HTPressure = s.HTPressure
			,t.HTPellOff = s.HTPellOff
			,t.HT2ndPressnoreverse = s.HT2ndPressnoreverse
			,t.HT2ndPressreversed = s.HT2ndPressreversed
			,t.HTCoolingTime = s.HTCoolingTime
			,t.Type = s.Type
			,t.Region = s.Region
	when not matched by target then
		insert (
			 ReportNo,POID,StyleID,SeasonID,BrandID,Article,ArtworkTypeID,Remark,T1Subcon,T2Supplier,TestDate,ReceivedDate,ReleasedDate,Result,Technician,MR,AddDate,AddName
			,EditDate,EditName,TestTemperature,TestTime,HTPlate,HTFlim,HTTime,HTPressure,HTPellOff,HT2ndPressnoreverse,HT2ndPressreversed,HTCoolingTime,Type,Region
		) values (
			s.ReportNo,s.POID,s.StyleID,s.SeasonID,s.BrandID,s.Article,s.ArtworkTypeID,s.Remark,s.T1Subcon,s.T2Supplier,s.TestDate,s.ReceivedDate,s.ReleasedDate,s.Result,s.Technician,s.MR,s.AddDate,s.AddName
			,s.EditDate,s.EditName,s.TestTemperature,s.TestTime,s.HTPlate,s.HTFlim,s.HTTime,s.HTPressure,s.HTPellOff,s.HT2ndPressnoreverse,s.HT2ndPressreversed,s.HTCoolingTime,s.Type,s.Region
		)
	;
	INSERT INTO Quality.dbo.MockupOven_History( 
			HistoryUkey,ReportNo,POID,StyleID,SeasonID,BrandID,Article,ArtworkTypeID,Remark,T1Subcon,T2Supplier,TestDate,ReceivedDate,ReleasedDate,Result,Technician,MR,AddDate,AddName
			,EditDate,EditName,TestTemperature,TestTime,HTPlate,HTFlim,HTTime,HTPressure,HTPellOff,HT2ndPressnoreverse,HT2ndPressreversed,HTCoolingTime,Type,Region,DeleteDate)
	SELECT  HistoryUkey,ReportNo,POID,StyleID,SeasonID,BrandID,Article,ArtworkTypeID,Remark,T1Subcon,T2Supplier,TestDate,ReceivedDate,ReleasedDate,Result,Technician,MR,AddDate,AddName
			,EditDate,EditName,TestTemperature,TestTime,HTPlate,HTFlim,HTTime,HTPressure,HTPellOff,HT2ndPressnoreverse,HT2ndPressreversed,HTCoolingTime,Type,Region,DeleteDate
	FROM Trade_To_Pms.dbo.MockupOven_History t
	WHERE ReportNo NOT IN (SELECT ReportNo FROM Quality.dbo.MockupOven_History) 

	--MockupOven_Detail
	DELETE FROM Quality.dbo.MockupOven_Detail WHERE Ukey IN (SELECT Ukey FROM Trade_To_Pms.dbo.MockupOven_Detail_History) 
	Merge Quality.dbo.MockupOven_Detail as t
	Using (select a.* from Trade_To_Pms.dbo.MockupOven_Detail a ) as s
		on t.Ukey = s.Ukey
	when matched then 
		update set 
			 t.ReportNo = s.ReportNo
			,t.TypeofPrint = s.TypeofPrint
			,t.Design = s.Design
			,t.ArtworkColor = s.ArtworkColor
			,t.AccessoryRefno = s.AccessoryRefno
			,t.FabricRefNo = s.FabricRefNo
			,t.FabricColor = s.FabricColor
			,t.Result = s.Result
			,t.Remark = s.Remark
			,t.EditName = s.EditName
			,t.EditDate = s.EditDate
			,t.ChangeScale = s.ChangeScale
			,t.ResultChange = s.ResultChange
			,t.StainingScale = s.StainingScale
			,t.ResultStain = s.ResultStain
			,t.AddDate = s.AddDate
	when not matched by target then
		insert (
			 ReportNo,Ukey,TypeofPrint,Design,ArtworkColor,AccessoryRefno,FabricRefNo,FabricColor,Result,Remark,EditName,EditDate,ChangeScale,ResultChange,StainingScale,ResultStain,AddDate
		) values (
			s.ReportNo,s.Ukey,s.TypeofPrint,s.Design,s.ArtworkColor,s.AccessoryRefno,s.FabricRefNo,s.FabricColor,s.Result,s.Remark,s.EditName,s.EditDate,s.ChangeScale,s.ResultChange,s.StainingScale,s.ResultStain,s.AddDate
		)
	;
	INSERT INTO Quality.dbo.MockupOven_Detail_History( 
			HistoryUkey,ReportNo,Ukey,TypeofPrint,Design,ArtworkColor,AccessoryRefno,FabricRefNo,FabricColor,Result,Remark,EditName,EditDate,ChangeScale,ResultChange,StainingScale,ResultStain,AddDate,DeleteDate)
	SELECT  HistoryUkey,ReportNo,Ukey,TypeofPrint,Design,ArtworkColor,AccessoryRefno,FabricRefNo,FabricColor,Result,Remark,EditName,EditDate,ChangeScale,ResultChange,StainingScale,ResultStain,AddDate,DeleteDate
	FROM Trade_To_Pms.dbo.MockupOven_Detail_History t
	WHERE Ukey NOT IN (SELECT Ukey FROM Quality.dbo.MockupOven_Detail_History) 

	--PerspirationFastness
	DELETE FROM Quality.dbo.PerspirationFastness WHERE ReportNo IN (SELECT ReportNo FROM Trade_To_Pms.dbo.PerspirationFastness_History) 
	Merge Quality.dbo.PerspirationFastness as t
	Using (select a.* from Trade_To_Pms.dbo.PerspirationFastness a ) as s
		on t.ReportNo = s.ReportNo
	when matched then 
		update set 
			 t.POID = s.POID
			,t.StyleID = s.StyleID
			,t.SeasonID = s.SeasonID
			,t.BrandID = s.BrandID
			,t.Article = s.Article
			,t.TestDate = s.TestDate
			,t.Result = s.Result
			,t.Status = s.Status
			,t.Inspector = s.Inspector
			,t.Remark = s.Remark
			,t.Temperature = s.Temperature
			,t.Time = s.Time
			,t.MetalContent = s.MetalContent
			,t.Region = s.Region
			,t.AddName = s.AddName
			,t.AddDate = s.AddDate
			,t.EditName = s.EditName
			,t.EditDate = s.EditDate

	when not matched by target then
		insert (
			 ReportNo,POID,StyleID,SeasonID,BrandID,Article,TestDate,Result,Status,Inspector,Remark,Temperature,Time,MetalContent,Region,AddName,AddDate,EditName,EditDate
		) values (
			s.ReportNo,s.POID,s.StyleID,s.SeasonID,s.BrandID,s.Article,s.TestDate,s.Result,s.Status,s.Inspector,s.Remark,s.Temperature,s.Time,s.MetalContent,s.Region,s.AddName,s.AddDate,s.EditName,s.EditDate
		)
	;
	INSERT INTO Quality.dbo.PerspirationFastness_History( 
			HistoryUkey,ReportNo,POID,StyleID,SeasonID,BrandID,Article,TestDate,Result,Status,Inspector,Remark,Temperature,Time,MetalContent,Region,AddName,AddDate,EditName,EditDate,DeleteDate)
	SELECT  HistoryUkey,ReportNo,POID,StyleID,SeasonID,BrandID,Article,TestDate,Result,Status,Inspector,Remark,Temperature,Time,MetalContent,Region,AddName,AddDate,EditName,EditDate,DeleteDate
	FROM Trade_To_Pms.dbo.PerspirationFastness_History t
	WHERE ReportNo NOT IN (SELECT ReportNo FROM Quality.dbo.PerspirationFastness_History) 

	--PerspirationFastness_Detail
	DELETE FROM t
	FROM Quality.dbo.PerspirationFastness_Detail t
	WHERE EXISTS (
		SELECT 1 FROM Trade_To_Pms.dbo.PerspirationFastness_Detail_History s
		WHERE t.ReportNo = s.ReportNo AND t.PerspirationFastnessGroup= s.PerspirationFastnessGroup AND t.Seq1 = s.Seq1 AND t.Seq2 = s.Seq2
	) 
	Merge Quality.dbo.PerspirationFastness_Detail as t
	Using (select a.* from Trade_To_Pms.dbo.PerspirationFastness_Detail a ) as s
		on t.ReportNo = s.ReportNo AND t.PerspirationFastnessGroup = s.PerspirationFastnessGroup AND t.Seq1 = s.Seq1 AND t.Seq2 = s.Seq2
	when matched then 
		update set 
			 t.SubmitDate = s.SubmitDate
			,t.Roll = s.Roll
			,t.Dyelot = s.Dyelot
			,t.Refno = s.Refno
			,t.SCIRefno = s.SCIRefno
			,t.Color = s.Color
			,t.Result = s.Result
			,t.AlkalineChangeScale = s.AlkalineChangeScale
			,t.AlkalineResultChange = s.AlkalineResultChange
			,t.AlkalineAcetateScale = s.AlkalineAcetateScale
			,t.AlkalineResultAcetate = s.AlkalineResultAcetate
			,t.AlkalineCottonScale = s.AlkalineCottonScale
			,t.AlkalineResultCotton = s.AlkalineResultCotton
			,t.AlkalineNylonScale = s.AlkalineNylonScale
			,t.AlkalineResultNylon = s.AlkalineResultNylon
			,t.AlkalinePolyesterScale = s.AlkalinePolyesterScale
			,t.AlkalineResultPolyester = s.AlkalineResultPolyester
			,t.AlkalineAcrylicScale = s.AlkalineAcrylicScale
			,t.AlkalineResultAcrylic = s.AlkalineResultAcrylic
			,t.AlkalineWoolScale = s.AlkalineWoolScale
			,t.AlkalineResultWool = s.AlkalineResultWool
			,t.AcidChangeScale = s.AcidChangeScale
			,t.AcidResultChange = s.AcidResultChange
			,t.AcidAcetateScale = s.AcidAcetateScale
			,t.AcidResultAcetate = s.AcidResultAcetate
			,t.AcidCottonScale = s.AcidCottonScale
			,t.AcidResultCotton = s.AcidResultCotton
			,t.AcidNylonScale = s.AcidNylonScale
			,t.AcidResultNylon = s.AcidResultNylon
			,t.AcidPolyesterScale = s.AcidPolyesterScale
			,t.AcidResultPolyester = s.AcidResultPolyester
			,t.AcidAcrylicScale = s.AcidAcrylicScale
			,t.AcidResultAcrylic = s.AcidResultAcrylic
			,t.AcidWoolScale = s.AcidWoolScale
			,t.AcidResultWool = s.AcidResultWool
			,t.Remark = s.Remark
			,t.EditName = s.EditName
			,t.EditDate = s.EditDate
			,t.AddDate = s.AddDate
	when not matched by target then
		insert (
			 ReportNo,SubmitDate,PerspirationFastnessGroup,SEQ1,SEQ2,Roll,Dyelot,Refno,SCIRefno,Color,Result,AlkalineChangeScale,AlkalineResultChange,AlkalineAcetateScale,AlkalineResultAcetate,AlkalineCottonScale
			 ,AlkalineResultCotton,AlkalineNylonScale,AlkalineResultNylon,AlkalinePolyesterScale,AlkalineResultPolyester,AlkalineAcrylicScale,AlkalineResultAcrylic,AlkalineWoolScale,AlkalineResultWool
			 ,AcidChangeScale,AcidResultChange,AcidAcetateScale,AcidResultAcetate,AcidCottonScale,AcidResultCotton,AcidNylonScale,AcidResultNylon,AcidPolyesterScale,AcidResultPolyester,AcidAcrylicScale
			 ,AcidResultAcrylic,AcidWoolScale,AcidResultWool,Remark,EditName,EditDate,AddDate
		) values (
			s.ReportNo,s.SubmitDate,s.PerspirationFastnessGroup,s.SEQ1,s.SEQ2,s.Roll,s.Dyelot,s.Refno,s.SCIRefno,s.Color,s.Result,s.AlkalineChangeScale,s.AlkalineResultChange,s.AlkalineAcetateScale,s.AlkalineResultAcetate,s.AlkalineCottonScale
			 ,s.AlkalineResultCotton,s.AlkalineNylonScale,s.AlkalineResultNylon,s.AlkalinePolyesterScale,s.AlkalineResultPolyester,s.AlkalineAcrylicScale,s.AlkalineResultAcrylic,s.AlkalineWoolScale,s.AlkalineResultWool
			 ,s.AcidChangeScale,s.AcidResultChange,s.AcidAcetateScale,s.AcidResultAcetate,s.AcidCottonScale,s.AcidResultCotton,s.AcidNylonScale,s.AcidResultNylon,s.AcidPolyesterScale,s.AcidResultPolyester,s.AcidAcrylicScale
			 ,s.AcidResultAcrylic,s.AcidWoolScale,s.AcidResultWool,s.Remark,s.EditName,s.EditDate,s.AddDate
		)
	;
	INSERT INTO Quality.dbo.PerspirationFastness_Detail_History( 
			HistoryUkey,ReportNo,SubmitDate,PerspirationFastnessGroup,SEQ1,SEQ2,Roll,Dyelot,Refno,SCIRefno,Color,Result,AlkalineChangeScale,AlkalineResultChange,AlkalineAcetateScale,AlkalineResultAcetate,AlkalineCottonScale
			 ,AlkalineResultCotton,AlkalineNylonScale,AlkalineResultNylon,AlkalinePolyesterScale,AlkalineResultPolyester,AlkalineAcrylicScale,AlkalineResultAcrylic,AlkalineWoolScale,AlkalineResultWool
			 ,AcidChangeScale,AcidResultChange,AcidAcetateScale,AcidResultAcetate,AcidCottonScale,AcidResultCotton,AcidNylonScale,AcidResultNylon,AcidPolyesterScale,AcidResultPolyester,AcidAcrylicScale
			 ,AcidResultAcrylic,AcidWoolScale,AcidResultWool,Remark,EditName,EditDate,AddDate,DeleteDate)
	SELECT  HistoryUkey,ReportNo,SubmitDate,PerspirationFastnessGroup,SEQ1,SEQ2,Roll,Dyelot,Refno,SCIRefno,Color,Result,AlkalineChangeScale,AlkalineResultChange,AlkalineAcetateScale,AlkalineResultAcetate,AlkalineCottonScale
			 ,AlkalineResultCotton,AlkalineNylonScale,AlkalineResultNylon,AlkalinePolyesterScale,AlkalineResultPolyester,AlkalineAcrylicScale,AlkalineResultAcrylic,AlkalineWoolScale,AlkalineResultWool
			 ,AcidChangeScale,AcidResultChange,AcidAcetateScale,AcidResultAcetate,AcidCottonScale,AcidResultCotton,AcidNylonScale,AcidResultNylon,AcidPolyesterScale,AcidResultPolyester,AcidAcrylicScale
			 ,AcidResultAcrylic,AcidWoolScale,AcidResultWool,Remark,EditName,EditDate,AddDate,DeleteDate
	FROM Trade_To_Pms.dbo.PerspirationFastness_Detail_History t
	WHERE NOT EXISTS (
		SELECT 1 FROM Quality.dbo.PerspirationFastness_Detail_History s
		WHERE t.ReportNo = s.ReportNo AND t.PerspirationFastnessGroup= s.PerspirationFastnessGroup AND t.Seq1 = s.Seq1 AND t.Seq2 = s.Seq2
	) 

	--WaterFastness
	DELETE FROM Quality.dbo.WaterFastness WHERE ReportNo IN (SELECT ReportNo FROM Trade_To_Pms.dbo.WaterFastness_History) 
	Merge Quality.dbo.WaterFastness as t
	Using (select a.* from Trade_To_Pms.dbo.WaterFastness a ) as s
		on t.ReportNo = s.ReportNo
	when matched then 
		update set 
			 t.POID = s.POID
			,t.StyleID = s.StyleID
			,t.SeasonID = s.SeasonID
			,t.BrandID = s.BrandID
			,t.Article = s.Article
			,t.TestDate = s.TestDate
			,t.Result = s.Result
			,t.Inspector = s.Inspector
			,t.Remark = s.Remark
			,t.Temperature = s.Temperature
			,t.Time = s.Time
			,t.Status = s.Status
			,t.AddDate = s.AddDate
			,t.AddName = s.AddName
			,t.EditDate = s.EditDate
			,t.EditName = s.EditName
			,t.Region = s.Region
	when not matched by target then
		insert (
			 ReportNo,POID,StyleID,SeasonID,BrandID,Article,TestDate,Result,Inspector,Remark,Temperature,Time,Status,AddDate,AddName,EditDate,EditName,Region
		) values (
			s.ReportNo,s.POID,s.StyleID,s.SeasonID,s.BrandID,s.Article,s.TestDate,s.Result,s.Inspector,s.Remark,s.Temperature,s.Time,s.Status,s.AddDate,s.AddName,s.EditDate,s.EditName,s.Region
		)
	;
	INSERT INTO Quality.dbo.WaterFastness_History( 
			HistoryUkey,ReportNo,POID,StyleID,SeasonID,BrandID,Article,TestDate,Result,Inspector,Remark,Temperature,Time,Status,AddDate,AddName,EditDate,EditName,Region,DeleteDate)
	SELECT  HistoryUkey,ReportNo,POID,StyleID,SeasonID,BrandID,Article,TestDate,Result,Inspector,Remark,Temperature,Time,Status,AddDate,AddName,EditDate,EditName,Region,DeleteDate
	FROM Trade_To_Pms.dbo.WaterFastness_History t
	WHERE ReportNo NOT IN (SELECT ReportNo FROM Quality.dbo.WaterFastness_History) 

	--WaterFastness_Detail
	DELETE FROM t
	FROM Quality.dbo.WaterFastness_Detail t
	WHERE EXISTS (
		SELECT 1 FROM Trade_To_Pms.dbo.WaterFastness_Detail_History s
		WHERE t.ReportNo = s.ReportNo AND t.WaterFastnessGroup= s.WaterFastnessGroup AND t.Seq1 = s.Seq1 AND t.Seq2 = s.Seq2
	) 
	Merge Quality.dbo.WaterFastness_Detail as t
	Using (select a.* from Trade_To_Pms.dbo.WaterFastness_Detail a ) as s
		on t.ReportNo = s.ReportNo AND t.WaterFastnessGroup = s.WaterFastnessGroup AND t.Seq1 = s.Seq1 AND t.Seq2 = s.Seq2
	when matched then 
		update set 
			 t.SubmitDate = s.SubmitDate
			,t.Roll = s.Roll
			,t.Dyelot = s.Dyelot
			,t.Refno = s.Refno
			,t.SCIRefno = s.SCIRefno
			,t.Color = s.Color
			,t.Result = s.Result
			,t.ChangeScale = s.ChangeScale
			,t.ResultChange = s.ResultChange
			,t.AcetateScale = s.AcetateScale
			,t.ResultAcetate = s.ResultAcetate
			,t.CottonScale = s.CottonScale
			,t.ResultCotton = s.ResultCotton
			,t.NylonScale = s.NylonScale
			,t.ResultNylon = s.ResultNylon
			,t.PolyesterScale = s.PolyesterScale
			,t.ResultPolyester = s.ResultPolyester
			,t.AcrylicScale = s.AcrylicScale
			,t.ResultAcrylic = s.ResultAcrylic
			,t.WoolScale = s.WoolScale
			,t.ResultWool = s.ResultWool
			,t.Remark = s.Remark
			,t.EditDate = s.EditDate
			,t.EditName = s.EditName
			,t.AddDate = s.AddDate
	when not matched by target then
		insert (
			 ReportNo,SubmitDate,WaterFastnessGroup,Seq1,Seq2,Roll,Dyelot,Refno,SCIRefno,Color,Result,ChangeScale,ResultChange,AcetateScale,ResultAcetate,CottonScale,ResultCotton,NylonScale
			 ,ResultNylon,PolyesterScale,ResultPolyester,AcrylicScale,ResultAcrylic,WoolScale,ResultWool,Remark,EditDate,EditName,AddDate
		) values (
			s.ReportNo,s.SubmitDate,s.WaterFastnessGroup,s.Seq1,s.Seq2,s.Roll,s.Dyelot,s.Refno,s.SCIRefno,s.Color,s.Result,s.ChangeScale,s.ResultChange,s.AcetateScale,s.ResultAcetate,s.CottonScale,s.ResultCotton,s.NylonScale
			 ,s.ResultNylon,s.PolyesterScale,s.ResultPolyester,s.AcrylicScale,s.ResultAcrylic,s.WoolScale,s.ResultWool,s.Remark,s.EditDate,s.EditName,s.AddDate
		)
	;
	INSERT INTO Quality.dbo.WaterFastness_Detail_History( 
			HistoryUkey,ReportNo,SubmitDate,WaterFastnessGroup,Seq1,Seq2,Roll,Dyelot,Refno,SCIRefno,Color,Result,ChangeScale,ResultChange,AcetateScale,ResultAcetate,CottonScale,ResultCotton,NylonScale
			 ,ResultNylon,PolyesterScale,ResultPolyester,AcrylicScale,ResultAcrylic,WoolScale,ResultWool,Remark,EditDate,EditName,AddDate,DeleteDate)
	SELECT  HistoryUkey,ReportNo,SubmitDate,WaterFastnessGroup,Seq1,Seq2,Roll,Dyelot,Refno,SCIRefno,Color,Result,ChangeScale,ResultChange,AcetateScale,ResultAcetate,CottonScale,ResultCotton,NylonScale
			 ,ResultNylon,PolyesterScale,ResultPolyester,AcrylicScale,ResultAcrylic,WoolScale,ResultWool,Remark,EditDate,EditName,AddDate,DeleteDate
	FROM Trade_To_Pms.dbo.WaterFastness_Detail_History t
	WHERE NOT EXISTS (
		SELECT 1 FROM Quality.dbo.WaterFastness_Detail_History s
		WHERE t.ReportNo = s.ReportNo AND t.WaterFastnessGroup= s.WaterFastnessGroup AND t.Seq1 = s.Seq1 AND t.Seq2 = s.Seq2
	) 

	--SampleGarmentTest
	DELETE FROM t
	FROM Quality.dbo.SampleGarmentTest t
	WHERE NOT EXISTS (
		SELECT 1 FROM Trade_To_Pms.dbo.SampleGarmentTest_History s
		WHERE t.StyleID = s.StyleID AND t.BrandID= s.BrandID AND t.SeasonID = s.SeasonID AND t.Article = s.Article
	)	
	Merge Quality.dbo.SampleGarmentTest as t
	Using (select a.* from Trade_To_Pms.dbo.SampleGarmentTest a ) as s
		on t.StyleID = s.StyleID AND t.BrandID= s.BrandID AND t.SeasonID = s.SeasonID AND t.Article = s.Article
	when matched then 
		update set 
			 t.ID = s.ID
			,t.Result = s.Result
			,t.ReceivedDate = s.ReceivedDate
			,t.ReleasedDate = s.ReleasedDate
			,t.Deadline = s.Deadline
			,t.Inspdate = s.Inspdate
			,t.Remark = s.Remark
			,t.SeamBreakageResult = s.SeamBreakageResult
			,t.SeamBreakageLastTestDate = s.SeamBreakageLastTestDate
			,t.OdourResult = s.OdourResult
			,t.WashResult = s.WashResult
			,t.AddDate = s.AddDate
			,t.AddName = s.AddName
			,t.EditDate = s.EditDate
			,t.EditName = s.EditName

	when not matched by target then
		insert (
			 ID,StyleID,BrandID,SeasonID,Article,Result,ReceivedDate,ReleasedDate,Deadline,Inspdate,Remark,SeamBreakageResult,SeamBreakageLastTestDate,OdourResult,WashResult,AddDate,AddName,EditDate,EditName
		) values (
			s.ID,s.StyleID,s.BrandID,s.SeasonID,s.Article,s.Result,s.ReceivedDate,s.ReleasedDate,s.Deadline,s.Inspdate,s.Remark,s.SeamBreakageResult,s.SeamBreakageLastTestDate,s.OdourResult,s.WashResult,s.AddDate,s.AddName,s.EditDate,s.EditName
		)
	;
	INSERT INTO Quality.dbo.SampleGarmentTest_History( 
			HistoryUkey,ID,StyleID,BrandID,SeasonID,Article,Result,ReceivedDate,ReleasedDate,Deadline,Inspdate,Remark,SeamBreakageResult,SeamBreakageLastTestDate,OdourResult,WashResult,AddDate,AddName,EditDate,EditName,DeleteDate)
	SELECT  HistoryUkey,ID,StyleID,BrandID,SeasonID,Article,Result,ReceivedDate,ReleasedDate,Deadline,Inspdate,Remark,SeamBreakageResult,SeamBreakageLastTestDate,OdourResult,WashResult,AddDate,AddName,EditDate,EditName,DeleteDate
	FROM Trade_To_Pms.dbo.SampleGarmentTest_History t
	WHERE NOT EXISTS (
		SELECT 1 FROM Quality.dbo.SampleGarmentTest_History s
		WHERE t.StyleID = s.StyleID AND t.BrandID= s.BrandID AND t.SeasonID = s.SeasonID AND t.Article = s.Article
	)	

	--SampleGarmentTest_Detail
	DELETE FROM Quality.dbo.SampleGarmentTest_Detail WHERE ReportNo IN (SELECT ReportNo FROM Trade_To_Pms.dbo.SampleGarmentTest_Detail_History) 
	Merge Quality.dbo.SampleGarmentTest_Detail as t
	Using (select a.* from Trade_To_Pms.dbo.SampleGarmentTest_Detail a ) as s
		on t.ReportNo = s.ReportNo
	when matched then 
		update set 
			 t.ID = s.ID
			,t.No = s.No
			,t.OrderID = s.OrderID
			,t.MtlTypeID = s.MtlTypeID
			,t.InspDate = s.InspDate
			,t.Result = s.Result
			,t.NonSeamBreakageTest = s.NonSeamBreakageTest
			,t.SeamBreakageResult = s.SeamBreakageResult
			,t.OdourResult = s.OdourResult
			,t.WashResult = s.WashResult
			,t.Technician = s.Technician
			,t.Remark = s.Remark
			,t.Sender = s.Sender
			,t.SendDate = s.SendDate
			,t.Receiver = s.Receiver
			,t.ReceivedDate = s.ReceivedDate
			,t.AddDate = s.AddDate
			,t.AddName = s.AddName
			,t.EditDate = s.EditDate
			,t.EditName = s.EditName
			,t.ReportDate = s.ReportDate
			,t.SizeCode = s.SizeCode
			,t.Colour = s.Colour
			,t.LineDry = s.LineDry
			,t.TumbleDry = s.TumbleDry
			,t.HandWash = s.HandWash
			,t.Temperature = s.Temperature
			,t.Machine = s.Machine
			,t.Composition = s.Composition
			,t.Neck = s.Neck
			,t.Status = s.Status
			,t.LOtoFactory = s.LOtoFactory
			,t.Above50NaturalFibres = s.Above50NaturalFibres
			,t.Above50SyntheticFibres = s.Above50SyntheticFibres
			,t.Region = s.Region
	when not matched by target then
		insert (
			 ID,No,OrderID,ReportNo,MtlTypeID,InspDate,Result,NonSeamBreakageTest,SeamBreakageResult,OdourResult,WashResult,Technician,Remark,Sender,SendDate,Receiver,ReceivedDate,AddDate,AddName,EditDate,EditName
			 ,ReportDate,SizeCode,Colour,LineDry,TumbleDry,HandWash,Temperature,Machine,Composition,Neck,Status,LOtoFactory,Above50NaturalFibres,Above50SyntheticFibres,Region
		) values (
			s.ID,s.No,s.OrderID,s.ReportNo,s.MtlTypeID,s.InspDate,s.Result,s.NonSeamBreakageTest,s.SeamBreakageResult,s.OdourResult,s.WashResult,s.Technician,s.Remark,s.Sender,s.SendDate,s.Receiver,s.ReceivedDate,s.AddDate,s.AddName,s.EditDate,s.EditName
			 ,s.ReportDate,s.SizeCode,s.Colour,s.LineDry,s.TumbleDry,s.HandWash,s.Temperature,s.Machine,s.Composition,s.Neck,s.Status,s.LOtoFactory,s.Above50NaturalFibres,s.Above50SyntheticFibres,s.Region
		)
	;
	INSERT INTO Quality.dbo.SampleGarmentTest_Detail_History( 
			HistoryUkey,ID,No,OrderID,ReportNo,MtlTypeID,InspDate,Result,NonSeamBreakageTest,SeamBreakageResult,OdourResult,WashResult,Technician,Remark,Sender,SendDate,Receiver,ReceivedDate,AddDate,AddName,EditDate,EditName
			 ,ReportDate,SizeCode,Colour,LineDry,TumbleDry,HandWash,Temperature,Machine,Composition,Neck,Status,LOtoFactory,Above50NaturalFibres,Above50SyntheticFibres,Region,DeleteDate)
	SELECT  HistoryUkey,ID,No,OrderID,ReportNo,MtlTypeID,InspDate,Result,NonSeamBreakageTest,SeamBreakageResult,OdourResult,WashResult,Technician,Remark,Sender,SendDate,Receiver,ReceivedDate,AddDate,AddName,EditDate,EditName
			 ,ReportDate,SizeCode,Colour,LineDry,TumbleDry,HandWash,Temperature,Machine,Composition,Neck,Status,LOtoFactory,Above50NaturalFibres,Above50SyntheticFibres,Region,DeleteDate
	FROM Trade_To_Pms.dbo.SampleGarmentTest_Detail_History t
	WHERE ReportNo NOT IN (SELECT ReportNo FROM Quality.dbo.SampleGarmentTest_Detail_History) 

	--SampleGarmentTest_Detail_Appear
	DELETE FROM t
	FROM Quality.dbo.SampleGarmentTest_Detail_Appear t
	WHERE EXISTS (
		SELECT 1 FROM Trade_To_Pms.dbo.SampleGarmentTest_Detail_Appear_History s
		WHERE t.ID = s.ID AND t.No= s.No AND t.Seq = s.Seq 
	)
	Merge Quality.dbo.SampleGarmentTest_Detail_Appear as t
	Using (select a.* from Trade_To_Pms.dbo.SampleGarmentTest_Detail_Appear a ) as s
		on t.ID = s.ID AND t.No= s.No AND t.Seq = s.Seq 
	when matched then 
		update set 
			 t.Type = s.Type
			,t.Wash1 = s.Wash1
			,t.Wash2 = s.Wash2
			,t.Wash3 = s.Wash3
			,t.Comment = s.Comment
			,t.AddDate = s.AddDate
			,t.EditDate = s.EditDate
	when not matched by target then
		insert (
			 ID,No,Seq,Type,Wash1,Wash2,Wash3,Comment,AddDate,EditDate
		) values (
			s.ID,s.No,s.Seq,s.Type,s.Wash1,s.Wash2,s.Wash3,s.Comment,s.AddDate,s.EditDate
		)
	;
	INSERT INTO Quality.dbo.SampleGarmentTest_Detail_Appear_History( 
			HistoryUkey,ID,No,Seq,Type,Wash1,Wash2,Wash3,Comment,AddDate,EditDate,DeleteDate)
	SELECT  HistoryUkey,ID,No,Seq,Type,Wash1,Wash2,Wash3,Comment,AddDate,EditDate,DeleteDate
	FROM Trade_To_Pms.dbo.SampleGarmentTest_Detail_Appear_History t
	WHERE EXISTS (
		SELECT 1 FROM Quality.dbo.SampleGarmentTest_Detail_Appear_History s
		WHERE t.ID = s.ID AND t.No= s.No AND t.Seq = s.Seq 
	)

	--SampleGarmentTest_Detail_Shrink
	DELETE FROM t
	FROM Quality.dbo.SampleGarmentTest_Detail_Shrink t
	WHERE EXISTS (
		SELECT 1 FROM Trade_To_Pms.dbo.SampleGarmentTest_Detail_Shrink_History s
		WHERE t.ID = s.ID AND t.No= s.No AND t.Location = s.Location AND t.Type = s.Type
	)
	Merge Quality.dbo.SampleGarmentTest_Detail_Shrink as t
	Using (select a.* from Trade_To_Pms.dbo.SampleGarmentTest_Detail_Shrink a ) as s
		on t.ID = s.ID AND t.No= s.No AND t.Location = s.Location AND t.Type = s.Type
	when matched then 
		update set 
			 t.Seq = s.Seq
			,t.BeforeWash = s.BeforeWash
			,t.SizeSpec = s.SizeSpec
			,t.AfterWash1 = s.AfterWash1
			,t.Shrinkage1 = s.Shrinkage1
			,t.AfterWash2 = s.AfterWash2
			,t.Shrinkage2 = s.Shrinkage2
			,t.AfterWash3 = s.AfterWash3
			,t.Shrinkage3 = s.Shrinkage3
			,t.AddDate = s.AddDate
			,t.EditDate = s.EditDate

	when not matched by target then
		insert (
			 ID,No,Location,Type,Seq,BeforeWash,SizeSpec,AfterWash1,Shrinkage1,AfterWash2,Shrinkage2,AfterWash3,Shrinkage3,AddDate,EditDate
		) values (
			s.ID,s.No,s.Location,s.Type,s.Seq,s.BeforeWash,s.SizeSpec,s.AfterWash1,s.Shrinkage1,s.AfterWash2,s.Shrinkage2,s.AfterWash3,s.Shrinkage3,s.AddDate,s.EditDate
		)
	;
	INSERT INTO Quality.dbo.SampleGarmentTest_Detail_Shrink_History( 
			HistoryUkey,ID,No,Location,Type,Seq,BeforeWash,SizeSpec,AfterWash1,Shrinkage1,AfterWash2,Shrinkage2,AfterWash3,Shrinkage3,AddDate,EditDate,DeleteDate)
	SELECT  HistoryUkey,ID,No,Location,Type,Seq,BeforeWash,SizeSpec,AfterWash1,Shrinkage1,AfterWash2,Shrinkage2,AfterWash3,Shrinkage3,AddDate,EditDate,DeleteDate
	FROM Trade_To_Pms.dbo.SampleGarmentTest_Detail_Shrink_History t
	WHERE NOT EXISTS (
		SELECT 1 FROM Quality.dbo.SampleGarmentTest_Detail_Shrink_History s
		WHERE t.ID = s.ID AND t.No= s.No AND t.Location = s.Location AND t.Type = s.Type
	)

	--SampleGarmentTest_Detail_FGPT
	DELETE FROM t
	FROM Quality.dbo.SampleGarmentTest_Detail_FGPT t
	WHERE EXISTS (
		SELECT 1 FROM Trade_To_Pms.dbo.SampleGarmentTest_Detail_FGPT_History s
		WHERE t.ID = s.ID AND t.No= s.No AND t.Location = s.Location AND t.Seq = s.Seq AND t.Type = s.Type AND t.TestName = s.TestName
	)
	Merge Quality.dbo.SampleGarmentTest_Detail_FGPT as t
	Using (select a.* from Trade_To_Pms.dbo.SampleGarmentTest_Detail_FGPT a ) as s
		on t.ID = s.ID AND t.No= s.No AND t.Location = s.Location AND t.Seq = s.Seq AND t.Type = s.Type AND t.TestName = s.TestName
	when matched then 
		update set 
			 t.TypeSelection_VersionID = s.TypeSelection_VersionID
			,t.TypeSelection_Seq = s.TypeSelection_Seq
			,t.TestDetail = s.TestDetail
			,t.Criteria = s.Criteria
			,t.TestResult = s.TestResult
			,t.TestUnit = s.TestUnit
			,t.IsOriginal = s.IsOriginal
			,t.AddDate = s.AddDate
			,t.EditDate = s.EditDate
	when not matched by target then
		insert (
			 ID,No,Location,Seq,TestName,Type,TypeSelection_VersionID,TypeSelection_Seq,TestDetail,Criteria,TestResult,TestUnit,IsOriginal,AddDate,EditDate
		) values (
			s.ID,s.No,s.Location,s.Seq,s.TestName,s.Type,s.TypeSelection_VersionID,s.TypeSelection_Seq,s.TestDetail,s.Criteria,s.TestResult,s.TestUnit,s.IsOriginal,s.AddDate,s.EditDate
		)
	;
	INSERT INTO Quality.dbo.SampleGarmentTest_Detail_FGPT_History( 
			HistoryUkey,ID,No,Location,Seq,TestName,Type,TypeSelection_VersionID,TypeSelection_Seq,TestDetail,Criteria,TestResult,TestUnit,IsOriginal,AddDate,EditDate,DeleteDate)
	SELECT  HistoryUkey,ID,No,Location,Seq,TestName,Type,TypeSelection_VersionID,TypeSelection_Seq,TestDetail,Criteria,TestResult,TestUnit,IsOriginal,AddDate,EditDate,DeleteDate
	FROM Trade_To_Pms.dbo.SampleGarmentTest_Detail_FGPT_History t
	WHERE NOT EXISTS (
		SELECT 1 FROM Quality.dbo.SampleGarmentTest_Detail_FGPT_History s
		WHERE t.ID = s.ID AND t.No= s.No AND t.Location = s.Location AND t.Seq = s.Seq AND t.Type = s.Type AND t.TestName = s.TestName
	)

	--SampleGarmentTest_Detail_FGWT
	DELETE FROM t
	FROM Quality.dbo.SampleGarmentTest_Detail_FGWT t
	WHERE EXISTS (
		SELECT 1 FROM Trade_To_Pms.dbo.SampleGarmentTest_Detail_FGWT_History s
		WHERE t.ID = s.ID AND t.No= s.No AND t.Location = s.Location AND t.Seq = s.Seq AND t.Type = s.Type
	)
	Merge Quality.dbo.SampleGarmentTest_Detail_FGWT as t
	Using (select a.* from Trade_To_Pms.dbo.SampleGarmentTest_Detail_FGWT a ) as s
		on t.ID = s.ID AND t.No= s.No AND t.Location = s.Location AND t.Seq = s.Seq AND t.Type = s.Type
	when matched then 
		update set
			 t.SystemType = s.SystemType
			,t.TestDetail = s.TestDetail
			,t.BeforeWash = s.BeforeWash
			,t.AfterWash = s.AfterWash
			,t.Shrinkage = s.Shrinkage
			,t.Scale = s.Scale
			,t.Criteria = s.Criteria
			,t.Criteria2 = s.Criteria2
			,t.AddDate = s.AddDate
			,t.EditDate = s.EditDate

	when not matched by target then
		insert (
			 ID,No,Location,Type,SystemType,Seq,TestDetail,BeforeWash,AfterWash,Shrinkage,Scale,Criteria,Criteria2,AddDate,EditDate
		) values (
			s.ID,s.No,s.Location,s.Type,s.SystemType,s.Seq,s.TestDetail,s.BeforeWash,s.AfterWash,s.Shrinkage,s.Scale,s.Criteria,s.Criteria2,s.AddDate,s.EditDate
		)
	;
	INSERT INTO Quality.dbo.SampleGarmentTest_Detail_FGWT_History( 
			HistoryUkey,ID,No,Location,Type,SystemType,Seq,TestDetail,BeforeWash,AfterWash,Shrinkage,Scale,Criteria,Criteria2,AddDate,EditDate,DeleteDate)
	SELECT  HistoryUkey,ID,No,Location,Type,SystemType,Seq,TestDetail,BeforeWash,AfterWash,Shrinkage,Scale,Criteria,Criteria2,AddDate,EditDate,DeleteDate
	FROM Trade_To_Pms.dbo.SampleGarmentTest_Detail_FGWT_History t
	WHERE NOT EXISTS (
		SELECT 1 FROM Quality.dbo.SampleGarmentTest_Detail_FGWT_History s
		WHERE t.ID = s.ID AND t.No= s.No AND t.Location = s.Location AND t.Seq = s.Seq AND t.Type = s.Type
	)

	--SampleGarment_Detail_Spirality
	DELETE FROM t
	FROM Quality.dbo.SampleGarment_Detail_Spirality t
	WHERE EXISTS (
		SELECT 1 FROM Trade_To_Pms.dbo.SampleGarment_Detail_Spirality_History s
		WHERE t.ID = s.ID AND t.No= s.No AND t.Location = s.Location
	)
	Merge Quality.dbo.SampleGarment_Detail_Spirality as t
	Using (select a.* from Trade_To_Pms.dbo.SampleGarment_Detail_Spirality a ) as s
		on t.ID = s.ID AND t.No= s.No AND t.Location = s.Location
	when matched then 
		update set
			 t.MethodA_AAPrime = s.MethodA_AAPrime
			,t.MethodA_APrimeB = s.MethodA_APrimeB
			,t.MethodB_AAPrime = s.MethodB_AAPrime
			,t.MethodB_AB = s.MethodB_AB
			,t.CM = s.CM
			,t.MethodA = s.MethodA
			,t.MethodB = s.MethodB
			,t.AddDate = s.AddDate
			,t.EditDate = s.EditDate
	when not matched by target then
		insert (
			 ID,No,Location,MethodA_AAPrime,MethodA_APrimeB,MethodB_AAPrime,MethodB_AB,CM,MethodA,MethodB,AddDate,EditDate
		) values (
			s.ID,s.No,s.Location,s.MethodA_AAPrime,s.MethodA_APrimeB,s.MethodB_AAPrime,s.MethodB_AB,s.CM,s.MethodA,s.MethodB,s.AddDate,s.EditDate
		)
	;
	INSERT INTO Quality.dbo.SampleGarment_Detail_Spirality_History( 
			HistoryUkey,ID,No,Location,MethodA_AAPrime,MethodA_APrimeB,MethodB_AAPrime,MethodB_AB,CM,MethodA,MethodB,AddDate,EditDate,DeleteDate)
	SELECT  HistoryUkey,ID,No,Location,MethodA_AAPrime,MethodA_APrimeB,MethodB_AAPrime,MethodB_AB,CM,MethodA,MethodB,AddDate,EditDate,DeleteDate
	FROM Trade_To_Pms.dbo.SampleGarment_Detail_Spirality_History t
	WHERE NOT EXISTS (
		SELECT 1 FROM Quality.dbo.SampleGarment_Detail_Spirality_History s
		WHERE t.ID = s.ID AND t.No= s.No AND t.Location = s.Location
	)
	
	


	set transaction isolation level read committed
END

