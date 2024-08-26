using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <inheritdoc/>
    public class QA_R01
    {
        /// <inheritdoc/>
        public QA_R01()
        {
            DBProxy.Current.DefaultTimeout = 3600;
        }

        /// <inheritdoc/>
        public Base_ViewModel Get_QA_R01(QA_R01_ViewModel model)
        {
            string biWhere = $@"
            AND (f.AddDate >= @StartBIDate AND f.AddDate <= @EndBIDate)
			OR (f.EditDate >= @StartBIDate AND f.EditDate <= @EndBIDate)";
            var itemWhere = this.GetWhere(model);
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@StartInstPhysicalInspDate", SqlDbType.DateTime) { Value = (object)model.StartInstPhysicalInspDate ?? DBNull.Value },
                new SqlParameter("@EndInstPhysicalInspDate", SqlDbType.DateTime) { Value = (object)model.EndInstPhysicalInspDate ?? DBNull.Value },

                new SqlParameter("@StartArriveWHDate", SqlDbType.DateTime) { Value = (object)model.StartArriveWHDate ?? DBNull.Value },
                new SqlParameter("@EndArriveWHDate", SqlDbType.DateTime) { Value = (object)model.EndArriveWHDate ?? DBNull.Value },

                new SqlParameter("@StartSciDelivery", SqlDbType.VarChar, 10) { Value = (object)model.StartSciDelivery ?? DBNull.Value },
                new SqlParameter("@EndSciDelivery", SqlDbType.Date) { Value = (object)model.EndSciDelivery ?? DBNull.Value },

                new SqlParameter("@StartSewingInLineDate", SqlDbType.VarChar, 10) { Value = (object)model.StartSewingInLineDate ?? DBNull.Value },
                new SqlParameter("@EndSewingInLineDate", SqlDbType.VarChar, 10) { Value = (object)model.EndSewingInLineDate ?? DBNull.Value },

                new SqlParameter("@StartEstCuttingDate", SqlDbType.VarChar, 10) { Value = (object)model.StartEstCuttingDate ?? DBNull.Value },
                new SqlParameter("@EndEstCuttingDate", SqlDbType.VarChar, 10) { Value = (object)model.EndEstCuttingDate ?? DBNull.Value },

                new SqlParameter("@StartWK", SqlDbType.VarChar, 10) { Value = (object)model.StartWK ?? DBNull.Value },
                new SqlParameter("@EndWK", SqlDbType.VarChar, 10) { Value = (object)model.EndWK ?? DBNull.Value },

                new SqlParameter("@StartSP", SqlDbType.VarChar, 15) { Value = (object)model.StartSP ?? DBNull.Value },
                new SqlParameter("@EndSP", SqlDbType.VarChar, 15) { Value = (object)model.EndSP ?? DBNull.Value },

                new SqlParameter("@Season", SqlDbType.VarChar, 15) { Value = (object)model.Season ?? DBNull.Value },

                new SqlParameter("@Brand", SqlDbType.VarChar, 15) { Value = (object)model.Brand ?? DBNull.Value },

                new SqlParameter("@Refno", SqlDbType.VarChar, 36) { Value = (object)model.Refno ?? DBNull.Value },

                new SqlParameter("@Supplier", SqlDbType.VarChar, 15) { Value = (object)model.Supplier ?? DBNull.Value },

                new SqlParameter("@OverallResultStatus", SqlDbType.VarChar, 15) { Value = (object)model.OverallResultStatus ?? DBNull.Value },

                new SqlParameter("@StartBIDate", SqlDbType.Date) { Value = model.StartBIDate.HasValue ? (object)model.StartBIDate.Value : DBNull.Value },
                new SqlParameter("@EndBIDate", SqlDbType.Date) { Value = model.EndBIDate.HasValue ? (object)model.EndBIDate.Value : DBNull.Value },
            };

            string sqlcmd = $@"select *  from OPENQUERY([MainServer], 'exec Production.[dbo].[GetFabricInspLabSummaryReport] ''{model.StartBIDate.Value.ToString("yyyy/MM/dd")}'', ''{model.EndBIDate.Value.ToString("yyyy/MM/dd")}'' ')";

            // 合併SQL如下，因效能太慢先暫緩整理
            string sqlcmdTmp = $@"
            SET ARITHABORT ON

            select 
            [BalanceQty]=sum(fit.inqty - fit.outqty + fit.adjustqty - fit.ReturnQty) 
            ,rd.poid
            ,rd.seq1
            ,rd.seq2
            ,RD.ID
            INTO #balanceTmp
            from dbo.View_AllReceivingDetail rd WITH (NOLOCK)
            inner join FIR f WITH (NOLOCK) on f.POID=rd.poid AND  f.ReceivingID = rd.id AND f.seq1 = rd.seq1 and f.seq2 = rd.Seq2
            inner join FtyInventory fit WITH (NOLOCK) on fit.poid = rd.PoId and fit.seq1 = rd.seq1 and fit.seq2 = rd.Seq2 AND fit.StockType=rd.StockType and fit.Roll=rd.Roll and fit.Dyelot=rd.Dyelot
            where 1=1 
            {(model.IsBI ? biWhere : itemWhere.Item2 + itemWhere.Item1.Replace("SP.", "f.").Replace("P.", "f."))}
            GROUP BY rd.poid,rd.seq1,rd.seq2,RD.ID

            select  
            F.POID
            ,(F.SEQ1+'-'+F.SEQ2)SEQ
            {(model.IsBI ? ",F.ReceivingID" : string.Empty)}
            ,O.factoryid
            ,O.BrandID
            ,O.StyleID
            ,O.SeasonID
            ,t.ExportId
            ,t.InvNo
            ,t.WhseArrival
            ,[StockQty1] = t.StockQty
            ,[InvStock] = t.InvStock
            ,[BulkStock] = t.BulkStock
            ,[BalanceQty] = IIF(BalanceQty.BalanceQty=0,NULL,BalanceQty.BalanceQty)
            ,[TotalRollsCalculated] = t.TotalRollsCalculated
            ,mp.ALocation
            ,LT.BulkLocationDate
            ,mp.BLocation	
            ,ILT.InvLocationDate
            ,[MinSciDelivery] = sci.MinSciDelivery
            ,[MinBuyerDelivery] = sci.MinBuyerDelivery
            ,F.Refno
            ,C.Description
            ,[ColorID] = ps.SpecValue
            ,[ColorName] = color.Name
            ,[SupplierCode] = SP.SuppID
            ,[SupplierName] = s.AbbEN
            ,C.WeaveTypeID
            {(model.IsBI ? string.Empty : ",[InspectionGroup] = (Select InspectionGroup from Fabric where SCIRefno = p.SCIRefno)")}
            ,[NAPhysical] = IIF(F.Nonphysical = 1,'Y',' ')
            ,F.Result
            ,[CutShadebandQtyByRoll] = Qty.Roll
            ,[CutShadeband] = Shadeband.sCount
            ,F.Physical
            ,[PhysicalInspector] = (select name from Pass1 where id = f.PhysicalInspector)
            ,F.PhysicalDate
            {(model.IsBI ? string.Empty :
            $@"
            ,TotalYardage = TotalYardage.Val
            ,TotalYardageArrDate = {(model.StartArriveWHDate.Empty() && model.EndArriveWHDate.Empty() ? "NULL" : "TotalYardageArrDate.Val - ActTotalYdsArrDate.ActualYds")}")}
            ,ftp.ActualYds
            ,[InspectionRate] = ROUND(iif(t.StockQty = 0,0,CAST (ftp.ActualYds/t.StockQty AS FLOAT)) ,3)
            {(model.IsBI ? string.Empty : $@"
            ,TotalLotNumber
            ,InspectedLotNumber")}
            ,ftp.TotalPoint
            ,F.CustInspNumber
            ,F.Weight
            ,[WeightInspector] = (select name from Pass1 where id = f.WeightInspector)
            ,F.WeightDate
            ,F.ShadeBond
            ,[ShadeboneInspector] = (select name from Pass1 where id = f.ShadeboneInspector)
            ,F.ShadeBondDate
            ,[ShadeBandPass] =  [Shade_Band_Pass].cnt
            ,[ShadeBandFail] =  [Shade_Band_Fail].cnt
            ,F.Continuity
            ,[ContinuityInspector] = (select name from Pass1 where id = f.ContinuityInspector)
            ,F.ContinuityDate
            ,F.Odor
            ,[OdorInspector] = (select name from Pass1 where id = f.OdorInspector)
            ,F.OdorDate
            ,F.Moisture
            ,F.MoistureDate
            ,[CrockingShrinkageOverAllResult] = L.Result
            ,[NACrocking] = IIF(L.nonCrocking=1,'Y',' ')
            ,LC.Crocking
            ,fl.CrockingInspector
            ,LC.CrockingDate
            ,[NAHeatShrinkage] = IIF(L.nonHeat=1,'Y',' ')
            ,LH.Heat
            ,fl.HeatInspector
            ,LH.HeatDate
            ,[NAWashShrinkage] = IIF(L.nonWash=1,'Y',' ' )
            ,LW.Wash
            ,fl.WashInspector
            ,LW.WashDate
            ,[OvenTestResult] = V.Result
            ,[OvenTestInspector] = v2.Name
            ,[ColorFastnessResult] = CFD.Result
            ,[ColorFastnessInspector] = cfd2.Name
            ,[LocalMR] = ps1.LocalMR
            ,[Category] = ddl.Name
            ,[CuttingDate] = o.CutInLine
            ,[OrderType] = O.OrderTypeID
            ,[TotalYardsBC] = isnull(fptbc.TicketYds, 0)
            ,[TotalPointBC] = isnull(fptbc.TotalPoint, 0)
            ,[TotalPointA] = isnull(fpta.TotalPoint, 0)
            {(model.IsBI ? $@"
            ,F.AddDate
            ,F.EditDate
            ,t.StockType" :
            $@"
            ,[C_Grade_TOP3Defects] = isnull(CGradT3.Value,'')
            ,[A_Grade_TOP3Defects] = isnull(AGradT3.Value,'')
            ,[CutTime] = Qty.CutTime")}
            into #tmpFinal
            from dbo.FIR F WITH (NOLOCK) 
            cross apply
            (
	            select rd.WhseArrival,rd.InvNo,rd.ExportId,rd.Id,rd.PoId,RD.seq1,RD.seq2
	            ,[StockQty] = sum(RD.StockQty)
	            ,[InvStock] = iif(rd.StockType = 'I', sum(RD.StockQty), 0)
	            ,[BulkStock] = iif(rd.StockType = 'B', sum(RD.StockQty), 0)
                ,{(model.IsBI ? "[StockType] = rd.StockType" : string.Empty)}
                ,TotalRollsCalculated = count(1)
	            from dbo.View_AllReceivingDetail rd WITH (NOLOCK) 
	            where rd.PoId = F.POID and rd.Seq1 = F.SEQ1 and rd.Seq2 = F.SEQ2 AND rd.Id=F.ReceivingID
                {(model.IsBI ? biWhere : itemWhere.Item2.Replace("where", "AND"))}
                group by rd.WhseArrival,rd.InvNo,rd.ExportId,rd.Id,rd.PoId,RD.seq1,RD.seq2,rd.StockType
            ) t
            inner join 
            (
                select distinct poid,O.factoryid,O.BrandID,O.StyleID,O.SeasonID,O.Category,id ,CutInLine, o.OrderTypeID
                from dbo.Orders o WITH (NOLOCK)  
                {(model.IsBI ? "where O.Category in ('B','S','M','T','A')" : "Where 1=1" + itemWhere.Item3.Replace("where", "AND"))}
            ) O on O.id = F.POID
            left join DropDownList ddl with(nolock) on o.Category = ddl.ID and ddl.Type = 'Category'
            inner join dbo.PO_Supp SP WITH (NOLOCK) on SP.id = F.POID and SP.SEQ1 = F.SEQ1
            inner join dbo.PO_Supp_Detail P WITH (NOLOCK) on P.ID = F.POID and P.SEQ1 = F.SEQ1 and P.SEQ2 = F.SEQ2
            left join dbo.PO_Supp_Detail_Spec ps WITH (NOLOCK) on P.ID = ps.id and P.SEQ1 = ps.SEQ1 and P.SEQ2 = ps.SEQ2 and ps.SpecColumnID='Color'
            inner join supp s WITH (NOLOCK) on s.id = SP.SuppID 
            LEFT JOIN #balanceTmp BalanceQty ON BalanceQty.poid = f.POID and BalanceQty.seq1 = f.seq1 and BalanceQty.seq2 =f.seq2 AND BalanceQty.ID = f.ReceivingID
            left join MDivisionPoDetail mp on mp.POID=f.POID and mp.Seq1=f.SEQ1 and mp.Seq2=f.SEQ2
            OUter APPLY
            (  
                SELECT MinSciDelivery,MinBuyerDelivery FROM  DBO.GetSCI(F.Poid,O.Category)
            )sci
            OUTER APPLY
            (
	            SELECT * FROM  Fabric C WITH (NOLOCK) WHERE C.SCIRefno = F.SCIRefno
            )C
            OUTER APPLY
            (
	            SELECT * FROM  FIR_Laboratory L WITH (NOLOCK) WHERE 1=1		
	            AND L.ID = F.ID AND L.SEQ1 = F.SEQ1 AND L.SEQ2 = F.SEQ2
            )L
            OUTER APPLY
            (
	            SELECT * FROM  FIR_Laboratory L WITH (NOLOCK) WHERE 1=1
	            AND L.CrockingEncode=1 
	            AND L.ID = F.ID AND L.SEQ1 = F.SEQ1 AND L.SEQ2 = F.SEQ2
            )LC
            OUTER APPLY
            (
	            SELECT * FROM  FIR_Laboratory L WITH (NOLOCK) WHERE 1=1 
	            AND L.HeatEncode=1 
	            AND L.ID = F.ID AND L.SEQ1 = F.SEQ1 AND L.SEQ2 = F.SEQ2
            )LH
            OUTER APPLY
            (
	            SELECT * FROM  FIR_Laboratory L WITH (NOLOCK) WHERE 1=1
	            AND L.WashEncode=1 
	            AND L.ID = F.ID AND L.SEQ1 = F.SEQ1 AND L.SEQ2 = F.SEQ2
	            )LW
            OUTER APPLY
            (
                select Result = Stuff
                ((
		            select concat(',',Result)
		            from
                    (
			            select distinct od.Result 
			            from dbo.Oven ov WITH (NOLOCK) 
			            inner join dbo.Oven_Detail od WITH (NOLOCK) on od.ID = ov.ID
			            left join pass1 WITH (NOLOCK) on pass1.id = ov.Inspector
			            where ov.POID=F.POID and od.SEQ1=F.Seq1 and seq2=F.Seq2 
			            and ov.Status='Confirmed'
		            ) s
		            for xml path ('')
	            ) , 1, 1, '')
            )V
            OUTER APPLY
            (
                select [name ]= Stuff
                ((
		            select concat(',',name)
		            from 
                    (
			            select distinct pass1.name 
			            from dbo.Oven ov WITH (NOLOCK) 
			            inner join dbo.Oven_Detail od WITH (NOLOCK) on od.ID = ov.ID
			            left join pass1 WITH (NOLOCK) on pass1.id = ov.Inspector
			            where ov.POID=F.POID and od.SEQ1=F.Seq1 and seq2=F.Seq2 
			            and ov.Status='Confirmed'
		            ) s
		            for xml path ('')
	            ) , 1, 1, '')
            )V2
            OUTER APPLY
            (
                select Result = Stuff
                ((
		            select concat(',',Result)
		            from 
                    (
			            select distinct cd.Result
			            from dbo.ColorFastness CF WITH (NOLOCK) 
			            inner join dbo.ColorFastness_Detail cd WITH (NOLOCK) on cd.ID = CF.ID
			            left join pass1 WITH (NOLOCK) on pass1.id = cf.Inspector
			            where CF.Status = 'Confirmed' 
			            and CF.POID=F.POID and cd.SEQ1=F.Seq1 and cd.seq2=F.Seq2
		            ) s
		            for xml path ('')
	            ) , 1, 1, '')
            )CFD
            OUTER APPLY
            (
                select Name = Stuff
                ((
		            select concat(',',Name)
		            from 
                    (
			            select distinct Pass1.Name
			            from dbo.ColorFastness CF WITH (NOLOCK) 
			            inner join dbo.ColorFastness_Detail cd WITH (NOLOCK) on cd.ID = CF.ID
			            left join pass1 WITH (NOLOCK) on pass1.id = cf.Inspector
			            where CF.Status = 'Confirmed' 
			            and CF.POID=F.POID and cd.SEQ1=F.Seq1 and cd.seq2=F.Seq2
		            ) s
		            for xml path ('')
	            ) , 1, 1, '')
            )CFD2
            Outer apply
            (
	            select (A.id+' - '+ A.name + ' #'+A.extno) LocalMR 
                from orders od 
                inner join pass1 a on a.id=od.LocalMR 
                where od.id=o.POID
            ) ps1
            outer apply(select TotalPoint = Sum(fp.TotalPoint) ,ActualYds = Sum(fp.ActualYds) from FIR_Physical fp WITH (NOLOCK) where fp.id=f.id) ftp
            {(
            model.IsBI ? string.Empty :
            $@"
            outer apply
            (
	            select Val = Sum(ISNULL(fi.InQty,0))
	            from FtyInventory fi
	            inner join Receiving_Detail rd WITH (NOLOCK) on rd.PoId = fi.POID and rd.Seq1 = fi.Seq1 and rd.Seq2 = fi.Seq2 AND fi.StockType=rd.StockType and rd.Roll = fi.Roll and rd.Dyelot = fi.Dyelot
	            where fi.POID = f.POID AND fi.Seq1 = f.Seq1 AND fi.Seq2 = f.Seq2 AND rd.Id=f.ReceivingID AND rd.ForInspection=1
            ) TotalYardage
            outer apply
            (
                {itemWhere.Item4}
            ) TotalYardageArrDate
            outer apply
            (
                {itemWhere.Item5}
            ) ActTotalYdsArrDate")}
            outer apply(select TicketYds = Sum(fp.TicketYds), TotalPoint = Sum(fp.TotalPoint) from FIR_Physical fp WITH (NOLOCK) where fp.id = f.id and (fp.Grade = 'B' or fp.Grade = 'C')) fptbc
            outer apply(select TotalPoint = Sum(fp.TotalPoint) from FIR_Physical fp WITH (NOLOCK) where fp.id = f.id and fp.Grade = 'A') fpta
            outer apply
            (
                select  CrockingInspector = (select name from Pass1 where id = CrockingInspector)
	            ,HeatInspector = (select name from Pass1 where id = HeatInspector)
	            ,WashInspector = (select name from Pass1 where id = WashInspector)
	            from FIR_Laboratory WITH (NOLOCK) where Id=f.ID
            )FL
            outer apply
            (
	            SELECT Min(a.EditDate) BulkLocationDate
	            FROM LocationTrans a WITH (NOLOCK) 
	            inner join LocationTrans_detail as b WITH (NOLOCK) on a.ID = b.ID 
	            WHERE a.status = 'Confirmed' and b.stocktype='B'
	            AND b.Poid=f.POID and b.Seq1=f.SEQ1 and b.Seq2=f.SEQ2
            )LT
            outer apply
            (
	            SELECT Min(a.EditDate) InvLocationDate
	            FROM LocationTrans a WITH (NOLOCK) 
	            inner join LocationTrans_detail as b WITH (NOLOCK) on a.ID = b.ID 
	            WHERE a.status = 'Confirmed' and b.stocktype='I'
	            AND b.Poid=f.POID and b.Seq1=f.SEQ1 and b.Seq2=f.SEQ2
            )ILT
            outer apply
            (
	            select Roll = count(Roll+Dyelot){(model.IsBI ? string.Empty : ",CutTime=max(fs.CutTime)")} from FIR_Shadebone fs WITH (NOLOCK) where fs.ID =F.ID and fs.CutTime is not null
            )
            Qty 
            outer apply
            (
	            select [sCount] = iif(isnull(s.Roll,0) = 0 , 0, cast(Qty.Roll as float) / cast(s.Roll as float)) 
	            from
	            (
		            select Roll = count(Roll+Dyelot) 
		            from FIR_Shadebone fs WITH (NOLOCK)
		            where fs.ID = f.ID 
	            ) s
            ) Shadeband
            outer apply
            (
	            select cnt = count(1) 
	            from FIR_Shadebone t WITH (NOLOCK)
	            where UPPER(Result) = UPPER('Pass')
	            and t.ID = f.ID
            ) [Shade_Band_Pass]
            outer apply
            (
	            select cnt = count(1) 
	            from FIR_Shadebone t WITH (NOLOCK)
	            where UPPER(Result) = UPPER('Fail')
	            and t.ID = f.ID
            ) [Shade_Band_Fail]
            OUTER APPLY
            (
                SELECT Name 
                FROM Color c WITH (NOLOCK)
                where c.BrandId = O.BrandID 
                and c.ID = ps.SpecValue
            )color
            {(model.IsBI ? string.Empty : $@"
            OUTER APPLY(
                SELECT TotalLotNumber = COUNT(1)
                FROM(
                    SELECT DISTINCT rd.Dyelot
                    FROM View_AllReceivingDetail rd WITH (NOLOCK)
                    WHERE rd.Id = F.ReceivingID 
                    AND rd.PoId = F.POID
                    AND rd.Seq1 = F.SEQ1
                    AND rd.Seq2 = F.SEQ2 
                )TotalLotNumber
            )TotalLotNumber
            OUTER APPLY(
                SELECT InspectedLotNumber = COUNT(1)
                FROM(
                    SELECT DISTINCT rd.Dyelot
                    FROM View_AllReceivingDetail rd WITH (NOLOCK)
	                INNER JOIN FIR f2 WITH (NOLOCK) on f2.ReceivingID = rd.Id and f2.POID = rd.PoId and f2.SEQ1 = rd.Seq1 and f2.SEQ2 = rd.Seq2
	                INNER JOIN FIR_Physical fp WITH (NOLOCK) on f.id = fp.ID and fp.Roll = rd.Roll and fp.Dyelot = fp.Dyelot
                    WHERE rd.Id = F.ReceivingID 
                    AND rd.PoId = F.POID
                    AND rd.Seq1 = F.SEQ1
                    AND rd.Seq2 = F.SEQ2 
                )InspectedLotNumber
            )InspectedLotNumber
            OUTER APPLY(
	            select [Value]= Stuff((
		            select concat(',',defect)
		            from (
			            select TOP 3 defect = concat(fd.DescriptionEN,'(',count(1),')'),count(1) as Qty
			            from FIR
			            INNER JOIN FIR_Physical fp WITH (NOLOCK) ON FIR.ID= FP.ID
			            inner join FIR_Physical_Defect_Realtime fpd WITH (NOLOCK) on fp.DetailUkey = fpd.FIR_PhysicalDetailUKey
			            inner join FabricDefect fd WITH (NOLOCK) on fd.ID=fpd.FabricdefectID
			            where 1=1
			            and fp.Grade='C'
			            AND FIR.ID = F.ID 
			            group by fd.DescriptionEN
			            order by Qty desc, fd.DescriptionEN asc
		            ) s
		            for xml path ('')
	            ) , 1, 1, '')
            )CGradT3
            OUTER APPLY(
	            select [Value]= Stuff((
		            select concat(',',defect)
		            from (
			            select TOP 3 defect = concat(fd.DescriptionEN,'(',count(1),')'),count(1) as Qty
			            from FIR
			            INNER JOIN FIR_Physical fp WITH (NOLOCK) ON FIR.ID= FP.ID
			            inner join FIR_Physical_Defect_Realtime fpd WITH (NOLOCK) on fp.DetailUkey = fpd.FIR_PhysicalDetailUKey
			            inner join FabricDefect fd WITH (NOLOCK) on fd.ID=fpd.FabricdefectID
			            where 1=1
			            and fp.Grade='A'
			            AND FIR.ID=F.ID
			            group by fd.DescriptionEN
			            order by Qty desc, fd.DescriptionEN asc
		            ) s
		            for xml path ('')
	            ) , 1, 1, '')
            )AGradT3
            Where 1=1
            {itemWhere.Item1} 
            ORDER BY POID,SEQ
            ")}
            OPTION (OPTIMIZE FOR UNKNOWN)
            
            SELECT
            [Category] = ISNULL([Category], '')
            ,[POID] = ISNULL([POID], '')
            ,[SEQ] = ISNULL([SEQ], '')
            ,[FactoryID] = ISNULL([FactoryID], '')
            ,[BrandID] = ISNULL([BrandID], '')
            ,[StyleID] = ISNULL([StyleID], '')
            ,[SeasonID] = ISNULL([SeasonID], '')
            ,[Wkno] = ISNULL([ExportId], '')
            ,[InvNo] = ISNULL([InvNo], '')
            ,[CuttingDate]
            ,[ArriveWHDate] = [WhseArrival]
            ,[ArriveQty] = ISNULL([StockQty1], 0)
            ,[Inventory] = ISNULL([InvStock], 0)
            ,[Bulk] = ISNULL([BulkStock], 0)
            ,[BalanceQty] = ISNULL([BalanceQty], 0)
            ,[TtlRollsCalculated] = ISNULL([TotalRollsCalculated], 0)
            ,[BulkLocation] = ISNULL([ALocation], '')
            ,[FirstUpdateBulkLocationDate] = [BulkLocationDate]
            ,[InventoryLocation] = ISNULL([BLocation], '')
            ,[FirstUpdateStocksLocationDate] = [InvLocationDate]
            ,[EarliestSCIDelivery] = [MinSciDelivery]
            ,[BuyerDelivery] = [MinBuyerDelivery]
            ,[Refno] = ISNULL([Refno], '')
            ,[Description] = ISNULL([Description], '')
            ,[Color] = ISNULL([ColorID], '')
            ,[ColorName] = ISNULL([ColorName], '')
            ,[SupplierCode] = ISNULL([SupplierCode], '')
            ,[SupplierName] = ISNULL([SupplierName], '')
            ,[WeaveType] = ISNULL([WeaveTypeID], '')
            {(model.IsBI ? string.Empty : ",[InspectionGroup] = ISNULL(tf.[InspectionGroup], '')")}
            ,[NAPhysical] = ISNULL([NAPhysical], '')
            ,[InspectionOverallResult] = ISNULL([Result], '')
            ,[PhysicalInspResult] = ISNULL(Physical, '')
            ,[TtlYrdsUnderBCGrade] = ISNULL([TotalYardsBC], 0)
            ,[TtlPointsUnderBCGrade] = ISNULL([TotalPointBC], 0)
            {(model.IsBI ? string.Empty : ",[C_Grade_TOP3Defects] = ISNULL(tf.[C_Grade_TOP3Defects], '')")}
            ,[TtlPointsUnderAGrade] = ISNULL([TotalPointA], 0)
            {(model.IsBI ? string.Empty : ",[A_Grade_TOP3Defects] = ISNULL(tf.[A_Grade_TOP3Defects], '')")}
            ,[PhysicalInspector] = ISNULL([PhysicalInspector], '')
            ,[PhysicalInspDate] = [PhysicalDate]
            {(model.IsBI ? string.Empty : ",tf.TotalYardage  ,tf.TotalYardageArrDate")}
            ,[ActTtlYdsInspection] = ISNULL([ActualYds], 0)
            {(model.IsBI ? ",[InspectionPCT] = ISNULL(CAST([InspectionRate] * 100 AS NUMERIC(6,1)), 0)" : ",[InspectionRate] = isnull([InspectionRate] , 0)  ,[TotalLotNumber] = ISNULL([TotalLotNumber],0)  ,[InspectedLotNumber] = ISNULL([InspectedLotNumber], 0)")}
            ,[PhysicalInspDefectPoint] = ISNULL([TotalPoint], 0)
            ,[CustInspNumber] = ISNULL([CustInspNumber], '')
            ,[WeightTestResult] = ISNULL([Weight], '')
            ,[WeightTestInspector] = ISNULL([WeightInspector], '')
            ,[WeightTestDate] = [WeightDate]
            ,[CutShadebandQtyByRoll] = ISNULL([CutShadebandQtyByRoll], 0)
            {(model.IsBI ? ",[CutShadebandPCT] = ISNULL(CAST([CutShadeband] * 100 AS NUMERIC(5,2)), 0)" : ",[CutShadeband] = ISNULL(tf.[CutShadeband],0)  ,[CutTime] = CutTime")}
            ,[ShadeBondResult] = ISNULL([ShadeBond], '')
            ,[ShadeBondInspector] = ISNULL([ShadeboneInspector], '')
            ,[ShadeBondDate] = [ShadeBondDate]
            ,[NoOfRollShadebandPass] = ISNULL([ShadeBandPass], 0)
            ,[NoOfRollShadebandFail] = ISNULL([ShadeBandFail], 0)
            ,[ContinuityResult] = ISNULL([Continuity], '')
            ,[ContinuityInspector] = ISNULL([ContinuityInspector], '')
            ,[ContinuityDate]
            ,[OdorResult] = ISNULL([Odor], '')
            ,[OdorInspector] = ISNULL([OdorInspector], '')
            ,[OdorDate]
            ,[MoistureResult] = ISNULL([Moisture], '')
            ,[MoistureDate]
            ,[CrockingShrinkageOverAllResult] = ISNULL([CrockingShrinkageOverAllResult], '')
            ,[NACrocking] = ISNULL([NACrocking], '')
            ,[CrockingResult] = ISNULL([Crocking], '')
            ,[CrockingInspector] = ISNULL([CrockingInspector], '')
            ,[CrockingTestDate] = [CrockingDate]
            ,[NAHeatShrinkage] = ISNULL([NAHeatShrinkage], '')
            ,[HeatShrinkageTestResult] = ISNULL([Heat], '')
            ,[HeatShrinkageInspector] = ISNULL([HeatInspector], '')
            ,[HeatShrinkageTestDate] = [HeatDate]
            ,[NAWashShrinkage] = ISNULL([NAWashShrinkage], '')
            ,[WashShrinkageTestResult] = ISNULL([Wash], '')
            ,[WashShrinkageInspector] = ISNULL([WashInspector], '')
            ,[WashShrinkageTestDate] = [WashDate]    
            ,[OvenTestResult] = ISNULL([OvenTestResult], '')
            ,[OvenTestInspector] = ISNULL([OvenTestInspector], '')
            ,[ColorFastnessResult] = ISNULL([ColorFastnessResult], '')
            ,[ColorFastnessInspector] = ISNULL([ColorFastnessInspector], '')
            ,[LocalMR] = ISNULL([LocalMR], '')
            ,[OrderType] = ISNULL([OrderType], '')
            {(model.IsBI ? ",[ReceivingID] = ISNULL([ReceivingID], '')  ,[AddDate] ,[EditDate] ,[StockType] = ISNULL([StockType], '')" : string.Empty)}
            from #tmpFinal tf
            ORDER BY POID,SEQ
           
            ";

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("Production", sqlcmd, listPar, out DataTable[] dataTables),
            };
            resultReport.DtArr = dataTables;
            return resultReport;
        }

        /// <inheritdoc/>
        public Tuple<string, string, string, string, string> GetWhere(QA_R01_ViewModel model)
        {
            string sqlWhere1 = string.Empty;  // 原sqlWhere
            string sqlWhere2 = string.Empty;  // 原rWheres
            string sqlWhere3 = string.Empty;  // 原oWheres
            string sqlcmd1 = string.Empty;
            string sqlcmd2 = string.Empty;

            if (model.StartArriveWHDate.Empty() && model.EndArriveWHDate.Empty())
            {
                sqlcmd1 = $@" select Val = 0.0 ";
                sqlcmd2 = $@" select Val = 0.0 ";
            }
            else
            {
                sqlcmd1 = $@"
                select Val = Sum(ISNULL(fi.InQty,0))
                from FtyInventory fi
                inner join Receiving_Detail rd on rd.PoId = fi.POID and rd.Seq1 = fi.Seq1 and rd.Seq2 = fi.Seq2 AND fi.StockType=rd.StockType and rd.Roll = fi.Roll and rd.Dyelot = fi.Dyelot
                inner join Receiving r on r.Id=rd.Id
                where fi.POID = f.POID AND fi.Seq1 = f.Seq1 AND fi.Seq2 = f.Seq2 AND rd.Id=f.ReceivingID AND rd.ForInspection=1 ";

                sqlcmd2 = $@"
                select ActualYds = Sum(fp.ActualYds) 
                from FIR_Physical fp 
                where fp.ID = f.ID and EXISTS
                (
	                select 1
	                from  Receiving r  
	                where r.Id=f.ReceivingID
	                WhseArrival_1
                    WhseArrival_2
                )";
            }

            if (!model.StartInstPhysicalInspDate.Empty())
            {
                sqlWhere1 += " AND F.PhysicalDate >= @StartInstPhysicalInspDate";
            }

            if (!model.EndInstPhysicalInspDate.Empty())
            {
                sqlWhere1 += " AND F.PhysicalDate <= @EndInstPhysicalInspDate";
            }

            if (!model.StartArriveWHDate.Empty())
            {
                sqlWhere2 += " AND rd.WhseArrival >= @StartArriveWHDate";
                sqlcmd1 += " AND r.WhseArrival >= @StartArriveWHDate";
                sqlcmd2 = sqlcmd2.Replace($@"WhseArrival_1", "AND r.WhseArrival >= @StartArriveWHDate");
            }
            else
            {
                sqlcmd2.Replace("WhseArrival_1", string.Empty);
            }

            if (!model.EndArriveWHDate.Empty())
            {
                sqlWhere2 += " AND rd.WhseArrival <= @EndArriveWHDate";
                sqlcmd1 += " AND r.WhseArrival <= @EndArriveWHDate";
                sqlcmd2 = sqlcmd2.Replace($@"WhseArrival_2", "AND r.WhseArrival <= @EndArriveWHDate");
            }
            else
            {
                sqlcmd2.Replace("WhseArrival_2", string.Empty);
            }

            if (!model.StartSciDelivery.Empty())
            {
                sqlWhere3 += " AND O.SciDelivery >= @StartSciDelivery";
            }

            if (!model.EndSciDelivery.Empty())
            {
                sqlWhere3 += " AND O.SciDelivery <= @EndSciDelivery";
            }

            if (!model.StartSewingInLineDate.Empty())
            {
                sqlWhere3 += " AND O.SewInLine >= @StartSewingInLineDate";
            }

            if (!model.EndSewingInLineDate.Empty())
            {
                sqlWhere3 += " AND O.SewInLine <= @EndSewingInLineDate";
            }

            if (!model.StartEstCuttingDate.Empty())
            {
                sqlWhere3 += " AND O.CutInLine >= @StartEstCuttingDate";
            }

            if (!model.EndEstCuttingDate.Empty())
            {
                sqlWhere3 += " AND O.CutInLine <= @EndEstCuttingDate";
            }

            if (!model.StartSP.Empty())
            {
                sqlWhere3 += " AND O.Id between @StartSP and @EndSP";
            }

            if (!model.StartWK.Empty())
            {
                sqlWhere2 += " AND rd.ExportId between @StartWK and @EndWK";
            }

            if (!model.Season.Empty())
            {
                sqlWhere3 += " AND O.SeasonID = @Season";
            }

            if (!model.Brand.Empty())
            {
                sqlWhere3 += " AND O.BrandID = @Brand";
            }

            if (!model.Refno.Empty())
            {
                sqlWhere1 += " AND P.Refno = @Refno";
            }

            if (!model.Category.Empty())
            {
                sqlWhere3 += $" AND O.Category in ({model.Category})";
            }

            if (!model.Supplier.Empty())
            {
                sqlWhere1 += " AND SP.SuppId = @Supplier";
            }

            if (model.OverallResultStatus == "Pass")
            {
                sqlWhere1 += " AND f.result = 'Pass'";
            }

            if (model.OverallResultStatus == "Fail")
            {
                sqlWhere1 += " AND f.result = 'Fail'";
            }

            if (model.OverallResultStatus == "Empty Result")
            {
                sqlWhere1 += " AND f.result  = '' ";
            }

            return Tuple.Create(sqlWhere1, sqlWhere2, sqlWhere3, sqlcmd1, sqlcmd2);
        }
    }
}
