using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <summary>
    /// 此BI報表與 PMS/Sewing/R04已脫鉤 待討論
    /// </summary>
    public class P_Import_SewingDailyOutput
    {
        private DBProxy DBProxy;
        private string columnsName;
        private string nameZ;
        private string nameFinal;
        private string finalColumns;
        private string insertColumns;
        private string updateColumns;
        private string tTLZ;

        /// <inheritdoc/>
        public Base_ViewModel P_SewingDailyOutput(ExecutedList item)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            try
            {
                if (!item.SDate.HasValue)
                {
                    item.SDate = DateTime.Now.AddMonths(-60);
                }

                if (!item.EDate.HasValue)
                {
                    item.EDate = DateTime.Now;
                }

                Base_ViewModel resultReport = this.GetSewingDailyOutput_Data(item);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable dataTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataTable, item);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult = new Base().UpdateBIData(item);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel GetSewingDailyOutput_Data(ExecutedList item)
        {
            #region 動態欄位組合
            string sql_Columns = $@"
			select ID,Seq,ArtworkUnit,ProductionUnit
			into #AT
			from Production.dbo.ArtworkType WITH (NOLOCK)
			where Classify in ('I','A','P') and IsTtlTMS = 0 and Junk = 0

			select ID,Seq
				,ArtworkType_Unit = concat(ID,iif(Unit='QTY','(Price)',iif(Unit = '','','('+Unit+')'))),Unit
				,ArtworkType_CPU = iif(Unit = 'TMS',concat(ID,'(CPU)'),'')
			into #atall
			from(
				Select ID,Seq,Unit = ArtworkUnit from #AT where ArtworkUnit !='' AND ProductionUnit !=''
				UNION
				Select ID,Seq,ProductionUnit from #AT where ArtworkUnit !='' AND ProductionUnit !=''
				UNION
				Select ID,Seq,ArtworkUnit from #AT where ArtworkUnit !='' AND ProductionUnit =''
				UNION
				Select ID,Seq,ProductionUnit from #AT where ArtworkUnit ='' AND ProductionUnit !=''
				UNION
				Select ID,Seq,'' from #AT where ArtworkUnit ='' AND ProductionUnit =''
			)a

			select *
			into #atall2
			from(
				select a.ID,a.Seq,c=1,a.ArtworkType_Unit,a.Unit from #atall a
				UNION
				select a.ID,a.Seq,2,a.ArtworkType_CPU,iif(a.ArtworkType_CPU='','','CPU')from #atall a
				where a.ArtworkType_CPU !=''
			)b


			declare @columnsName nvarchar(max) = stuff((select concat(',[',ArtworkType_Unit,']') from #atall2 for xml path('')),1,1,'')

			SELECT @columnsName

			declare @NameZ nvarchar(max) = (select concat(',[',ArtworkType_Unit,']=isnull([',ArtworkType_Unit,'],0)')from #atall2 for xml path(''))

			SELECT @NameZ

			declare @NameFinal nvarchar(max) = replace((select concat('',col) 
			from 
			(select [col] = Replace(Replace(Replace(Replace(Replace(Replace(concat(iif(ArtworkType_Unit = '', '', ',['+ArtworkType_Unit+']'), iif(ArtworkType_CPU = '', '', ',['+ArtworkType_CPU+']'), iif(ArtworkType_Unit = '', '', ',[TTL_'+ArtworkType_Unit+']'), iif(ArtworkType_CPU = '', '', ',[TTL_'+ArtworkType_CPU+']')), ' (', '_'), ')(', '_'), ' ', '_'), '/', '_'), '(', '_'), ')', '')
			from #atall where id in ('AT (HAND)', 'AT (MACHINE)')
			union all
			select [col] = ',[TTL_AT_CPU] = iif([TTL_AT_HAND_CPU] > [TTL_AT_MACHINE_CPU], [TTL_AT_HAND_CPU], [TTL_AT_MACHINE_CPU])'
			union all
			select [col] = Replace(Replace(Replace(Replace(Replace(Replace(concat(iif(ArtworkType_Unit = '', '', ',['+ArtworkType_Unit+']'), iif(ArtworkType_CPU = '', '', ',['+ArtworkType_CPU+']'), iif(ArtworkType_Unit = '', '', ',[TTL_'+ArtworkType_Unit+']'), iif(ArtworkType_CPU = '', '', ',[TTL_'+ArtworkType_CPU+']')), ' (', '_'), ')(', '_'), ' ', '_'), '/', '_'), '(', '_'), ')', '')
			from #atall where id not in ('AT (HAND)', 'AT (MACHINE)')
			) a for xml path(N'')), '&gt;', '>')

			select @NameFinal

			declare @FinalColumns nvarchar(max) = replace((select concat('',col) 
			from 
			(select [col] = Replace(Replace(Replace(Replace(Replace(Replace(concat(iif(ArtworkType_Unit = '', '', ',['+ArtworkType_Unit+']'), iif(ArtworkType_CPU = '', '', ',['+ArtworkType_CPU+']'), iif(ArtworkType_Unit = '', '', ',[TTL_'+ArtworkType_Unit+']'), iif(ArtworkType_CPU = '', '', ',[TTL_'+ArtworkType_CPU+']')), ' (', '_'), ')(', '_'), ' ', '_'), '/', '_'), '(', '_'), ')', '')
			from #atall where id in ('AT (HAND)', 'AT (MACHINE)')
			union all
			select [col] = ',[TTL_AT_CPU]'
			union all
			select [col] = Replace(Replace(Replace(Replace(Replace(Replace(concat(iif(ArtworkType_Unit = '', '', ',['+ArtworkType_Unit+']'), iif(ArtworkType_CPU = '', '', ',['+ArtworkType_CPU+']'), iif(ArtworkType_Unit = '', '', ',[TTL_'+ArtworkType_Unit+']'), iif(ArtworkType_CPU = '', '', ',[TTL_'+ArtworkType_CPU+']')), ' (', '_'), ')(', '_'), ' ', '_'), '/', '_'), '(', '_'), ')', '')
			from #atall where id not in ('AT (HAND)', 'AT (MACHINE)')
			) a for xml path(N'')), '&gt;', '>')

			select @FinalColumns

			declare @InsertColumns nvarchar(max) = replace((select concat('',col) 
			from 
			(
			Select col = ', ISNULL(s.'+Data + ',0)' From dbo.SplitString(@FinalColumns, ',') where isnull(Data,'') != ''
			) a for xml path(N'')), '&gt;', '>')

			select @InsertColumns

			DECLARE @UpdateColumns NVARCHAR(MAX) = (
				SELECT (
					SELECT ',t.' + Data + ' = ISNULL(s.' + Data + ', 0)'
					FROM dbo.SplitString(@FinalColumns, ',')
					WHERE ISNULL(Data, '') != ''
					FOR XML PATH(''), TYPE
				).value('.', 'NVARCHAR(MAX)')
			);

			select @UpdateColumns

			declare @TTLZ nvarchar(max) = 
			(select concat(',['
			,Replace(Replace(Replace(Replace(Replace(Replace(ArtworkType_Unit, ' (', '_'), ')(', '_'), ' ', '_'), '/', '_'), '(', '_'), ')', '')
			,']=Cast(Round(sum(isnull(Rate*[',ArtworkType_Unit,'],0)) over(partition by t.FactoryID,t.OrderId,t.Team,t.OutputDate,t.SewingLineID,t.LastShift,t.Category,t.ComboType,t.SubconOutFty,t.SubConOutContractNumber,t.Article,t.SizeCode),4) as Numeric(15,4))'
			,iif(ArtworkType_CPU = '', '', concat(',['
			,Replace(Replace(Replace(Replace(Replace(Replace(ArtworkType_CPU, ' (', '_'), ')(', '_'), ' ', '_'), '/', '_'), '(', '_'), ')', '')
			,']=Cast(Round(sum(isnull(Rate*[',ArtworkType_CPU,'],0)) over(partition by t.FactoryID,t.OrderId,t.Team,t.OutputDate,t.SewingLineID,t.LastShift,t.Category,t.ComboType,t.SubconOutFty,t.SubConOutContractNumber,t.Article,t.SizeCode),4) as Numeric(15,4))'))
			,',[TTL_'
			,Replace(Replace(Replace(Replace(Replace(Replace(ArtworkType_Unit, ' (', '_'), ')(', '_'), ' ', '_'), '/', '_'), '(', '_'), ')', '')
			,']=Cast(Round(sum(o.QAQty*Rate*[',ArtworkType_Unit,'])over(partition by t.FactoryID,t.OrderId,t.Team,t.OutputDate,t.SewingLineID,t.LastShift,t.Category,t.ComboType,t.SubconOutFty,t.SubConOutContractNumber,t.Article,t.SizeCode),',iif(Unit='QTY','4','3'),') as Numeric(15,4))'
			,iif(ArtworkType_CPU = '', '', concat(',[TTL_'
			,Replace(Replace(Replace(Replace(Replace(Replace(ArtworkType_CPU, ' (', '_'), ')(', '_'), ' ', '_'), '/', '_'), '(', '_'), ')', '')
			,']=Cast(Round(sum(o.QAQty*Rate*[',ArtworkType_CPU,'])over(partition by t.FactoryID,t.OrderId,t.Team,t.OutputDate,t.SewingLineID,t.LastShift,t.Category,t.ComboType,t.SubconOutFty,t.SubConOutContractNumber,t.Article,t.SizeCode),',iif(Unit='QTY','4','3'),') as Numeric(15,4))'))
			)from #atall for xml path(''))

      
			select @TTLZ
			DROP TABLE #atall2,#AT,#atall";

            Base_ViewModel resultColumns = new Base_ViewModel()
            {
                Result = this.DBProxy.Select("Production", sql_Columns, out DataTable[] dtlist),
            };

            if (!resultColumns.Result)
            {
                return resultColumns;
            }

            this.columnsName = dtlist[0].Rows[0][0].ToString();
            this.nameZ = dtlist[1].Rows[0][0].ToString();
            this.nameFinal = dtlist[2].Rows[0][0].ToString();
            this.finalColumns = dtlist[3].Rows[0][0].ToString();
            this.insertColumns = dtlist[4].Rows[0][0].ToString();
            this.updateColumns = dtlist[5].Rows[0][0].ToString();
            this.tTLZ = dtlist[6].Rows[0][0].ToString();
            #endregion 動態欄位組合

            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@SDate", item.SDate.Value.ToString("yyyy/MM/dd")),
                new SqlParameter("@EDate", item.EDate.Value.ToString("yyyy/MM/dd")),
            };

            string sqlcmd = $@"

            -- 根據條件撈基本資料
            SELECT 
                s.id,
                s.OutputDate,
                s.Category,
                s.Shift,
                s.SewingLineID,
                s.Team,
                s.MDivisionID,
                s.FactoryID,
                sd.OrderId,
                [Article] = ISNULL(sdd.Article, sd.Article),
                [SizeCode] = ISNULL(sdd.SizeCode, ''),
                sd.ComboType,
                [ActManPower] = s.Manpower,
                [WorkHour] = IIF(ISNULL(sd.QAQty, 0) = 0, sd.WorkHour, CAST(sd.WorkHour * (sdd.QAQty * 1.0 / sd.QAQty) AS NUMERIC(6, 4))),
                [QAQty] = ISNULL(sdd.QAQty, 0),
                sd.InlineQty,
                o.LocalOrder,
                o.CustPONo,
                OrderCategory = ISNULL(o.Category, ''),
                OrderType = ISNULL(o.OrderTypeID, ''),
                [IsDevSample] = CASE WHEN ot.IsDevSample = 1 THEN 'Y' ELSE 'N' END,
                [OrderBrandID] = CASE 
                                    WHEN o.BrandID != 'SUBCON-I' THEN o.BrandID
                                    WHEN Order2.BrandID IS NOT NULL THEN Order2.BrandID
                                    WHEN StyleBrand.BrandID IS NOT NULL THEN StyleBrand.BrandID
                                    ELSE o.BrandID 
                                END,
                OrderCdCodeID = ISNULL(o.CdCodeID, ''),
                OrderProgram = ISNULL(o.ProgramID, ''),
                OrderCPU = ISNULL(o.CPU, 0),
                OrderCPUFactor = ISNULL(o.CPUFactor, 0),
                OrderStyle = ISNULL(o.StyleID, ''),
                OrderSeason = ISNULL(o.SeasonID, ''),
                MockupBrandID = ISNULL(mo.BrandID, ''),
                MockupCDCodeID = ISNULL(mo.MockupID, ''),
                MockupProgram = ISNULL(mo.ProgramID, ''),
                MockupCPU = ISNULL(mo.Cpu, 0),
                MockupCPUFactor = ISNULL(mo.CPUFactor, 0),
                MockupStyle = ISNULL(mo.StyleID, ''),
                MockupSeason = ISNULL(mo.SeasonID, ''),
                Rate = ISNULL(Production.dbo.GetOrderLocation_Rate(o.id, sd.ComboType), 100) / 100,
                System.StdTMS,
                [ori_QAQty] = ISNULL(sdd.QAQty, 0),
                [ori_InlineQty] = sd.InlineQty,
                BuyerDelivery = FORMAT(o.BuyerDelivery, 'yyyy/MM/dd'),
                OrderQty = o.Qty,
                s.SubconOutFty,
                s.SubConOutContractNumber,
                o.SubconInSisterFty,
                [SewingReasonDesc] = CAST('' AS NVARCHAR(1000)),
                o.SciDelivery,
                [LockStatus] = CASE 
                                    WHEN s.Status = 'Locked' THEN 'Monthly Lock' 
                                    WHEN s.Status = 'Sent' THEN 'Daily Lock' 
                                    ELSE '' 
                                END,
                [Cancel] = IIF(o.Junk = 1, 'Y', ''),
                [Remark] = CAST('' AS VARCHAR(MAX)),
                [SPFactory] = o.FactoryID,
                [NonRevenue] = IIF(o.NonRevenue = 1, 'Y', 'N'),
                [InlineCategoryID] = s.SewingReasonIDForTypeIC,
                [Inline_Category] = CAST('' AS NVARCHAR(65)),
                [Low_output_Reason] = CAST('' AS NVARCHAR(65)),
                [New_Style_Repeat_style] = CAST('' AS VARCHAR(20)),
                o.StyleUkey,
                ArtworkType = CAST('' AS VARCHAR(100)),
                s.SewingReasonIDForTypeIC,
                s.SewingReasonIDForTypeLO
            INTO #tmpSewingDetail
            FROM 
                Production.dbo.System WITH (NOLOCK),
                Production.dbo.SewingOutput s WITH (NOLOCK) 
                INNER JOIN Production.dbo.SewingOutput_Detail sd WITH (NOLOCK) ON sd.ID = s.ID
                LEFT JOIN Production.dbo.SewingOutput_Detail_Detail sdd WITH (NOLOCK) ON sd.UKey = sdd.SewingOutput_DetailUKey
                LEFT JOIN Production.dbo.Orders o WITH (NOLOCK) ON o.ID = sd.OrderId
                LEFT JOIN Production.dbo.Factory f WITH (NOLOCK) ON o.FactoryID = f.id
                LEFT JOIN Production.dbo.OrderType ot WITH (NOLOCK) ON o.OrderTypeID = ot.ID AND o.BrandID = ot.BrandID
                LEFT JOIN Production.dbo.MockupOrder mo WITH (NOLOCK) ON mo.ID = sd.OrderId
                OUTER APPLY (
                    SELECT BrandID 
                    FROM Production.dbo.Orders o1 WITH (NOLOCK) 
                    WHERE o.CustPONo = o1.id
                ) Order2
                OUTER APPLY (
                    SELECT TOP 1 BrandID 
                    FROM Production.dbo.Style WITH (NOLOCK) 
                    WHERE id = o.StyleID 
                      AND SeasonID = o.SeasonID 
                      AND BrandID != 'SUBCON-I'
                ) StyleBrand
            WHERE 1=1 
                -- 排除non sister的資料o.LocalOrder = 1 and o.SubconInSisterFty = 0
                AND ((o.LocalOrder <> 1 AND o.SubconInType NOT IN (1, 2)) OR (o.LocalOrder = 1 AND o.SubconInType <> 0))
                AND (s.OutputDate BETWEEN @SDate AND @EDate
                    OR s.EditDate BETWEEN @SDate AND DATEADD(DAY, 1, @EDate)
                )
                AND f.Type != 'S';


            -- 更新欄位資訊
            UPDATE s
            SET [SewingReasonDesc] = ISNULL(sr.SewingReasonDesc, ''),
                [Remark] = ISNULL(ssd.SewingOutputRemark, ''),
                [Inline_Category] = 
                    IIF(s.SewingReasonIDForTypeIC = '00005', 
                        (SELECT CONCAT(s.InlineCategoryID, '-' + SR.Description) 
                         FROM Production.dbo.SewingReason sr WITH (NOLOCK)
                         WHERE sr.ID = s.InlineCategoryID 
                         AND sr.Type = 'IC') 
                        , ISNULL(srICReason.Inline_Category, '')),
                [Low_output_Reason] = ISNULL(srLOReason.Low_output_Reason, ''),
                [ArtworkType] = ISNULL(apd.ArtworkType, '')
            FROM #tmpSewingDetail s
            OUTER APPLY (
                SELECT [SewingReasonDesc] = STUFF((
                    SELECT CONCAT('', '', sr.ID + '-' + sr.Description)
                    FROM Production.dbo.SewingReason sr WITH (NOLOCK)
                    INNER JOIN Production.dbo.SewingOutput_Detail sd2 WITH (NOLOCK) ON sd2.SewingReasonID = sr.ID
                    WHERE sr.Type = 'SO' 
                      AND sd2.id = s.id
                      AND sd2.OrderId = s.OrderId
                    FOR XML PATH('')
                ), 1, 1, '')
            ) sr
            OUTER APPLY (
                SELECT [SewingOutputRemark] = STUFF((
                    SELECT CONCAT(',', ssd.Remark)
                    FROM Production.dbo.SewingOutput_Detail ssd WITH (NOLOCK) 
                    WHERE ssd.ID = s.ID
                      AND ssd.OrderId = s.OrderId
                      AND ISNULL(ssd.Remark, '') <> ''
                    FOR XML PATH('')
                ), 1, 1, '')
            ) ssd
            OUTER APPLY (
                SELECT Inline_Category = CONCAT(s.SewingReasonIDForTypeIC, '-' + SR.Description)
                FROM Production.dbo.SewingReason sr WITH (NOLOCK)
                WHERE sr.ID = s.SewingReasonIDForTypeIC
                  AND sr.Type = 'IC'
            ) srICReason
            OUTER APPLY (
                SELECT Low_output_Reason = CONCAT(s.SewingReasonIDForTypeLO, '-' + SR.Description)
                FROM Production.dbo.SewingReason sr WITH (NOLOCK)
                WHERE sr.ID = s.SewingReasonIDForTypeLO 
                  AND sr.Type = 'LO'
            ) srLOReason
            OUTER APPLY (
                SELECT ArtworkType = STUFF((
                    SELECT CONCAT(',', '', ap.ArtworkTypeID)
                    FROM (
                        SELECT DISTINCT ap.ArtworkTypeID
                        FROM Production.dbo.ArtworkAP_Detail apd WITH (NOLOCK)
                        INNER JOIN Production.dbo.ArtworkAP ap WITH (NOLOCK) ON apd.ID = ap.Id
                        WHERE s.OrderID = apd.OrderID
                    ) ap
                    FOR XML PATH('')
                ), 1, 1, '')
            ) apd;

            -- 處理新款重複款
            SELECT
                FactoryID,
                OutputDate,
                SewinglineID,
                Team,
                StyleUkey,
                [NewStyleRepeatStyle] = Production.dbo.IsRepeatStyleBySewingOutput(FactoryID, OutputDate, SewinglineID, Team, StyleUkey)
            INTO #tmpNewStyleRepeatStyle
            FROM (
                SELECT DISTINCT 
                    FactoryID,
                    OutputDate,
                    SewinglineID,
                    Team,
                    StyleUkey
                FROM #tmpSewingDetail 
            ) a;

            UPDATE t 
            SET t.[New_Style_Repeat_style] = tp.NewStyleRepeatStyle
            FROM #tmpSewingDetail t
            INNER JOIN #tmpNewStyleRepeatStyle tp ON tp.FactoryID = t.FactoryID 
                                                AND tp.OutputDate = t.OutputDate 
                                                AND tp.SewinglineID = t.SewinglineID 
                                                AND tp.Team = t.Team
                                                AND tp.StyleUkey = t.StyleUkey;

            -- 分組數據
            SELECT DISTINCT
                ID,
                OutputDate,
                Category,
                Shift,
                SewingLineID,
                Team,
                FactoryID,
                MDivisionID,
                OrderId,
                Article,
                SizeCode,
                ComboType,
                [ActManPower] = s.Manpower,
                WorkHour = SUM(ROUND(WorkHour, 3)) OVER(PARTITION BY id, OrderId, Article, SizeCode, ComboType),
                QAQty = SUM(QAQty) OVER(PARTITION BY id, OrderId, Article, SizeCode, ComboType),
                [InlineQty] = SUM(InlineQty) OVER(PARTITION BY id, OrderId, Article, SizeCode, ComboType),
                LocalOrder, CustPONo, OrderCategory, OrderType, IsDevSample,
                OrderBrandID, OrderCdCodeID, OrderProgram, OrderCPU, OrderCPUFactor, OrderStyle, OrderSeason,
                MockupBrandID, MockupCDCodeID, MockupProgram, MockupCPU, MockupCPUFactor, MockupStyle, MockupSeason,
                Rate, StdTMS,
                ori_QAQty = SUM(ori_QAQty) OVER(PARTITION BY id, OrderId, Article, SizeCode, ComboType),
                ori_InlineQty = SUM(ori_InlineQty) OVER(PARTITION BY id, OrderId, Article, SizeCode, ComboType),
                BuyerDelivery,
                SciDelivery,
                OrderQty,
                SubconOutFty,
                SubConOutContractNumber,
                SubconInSisterFty,
                SewingReasonDesc,
                sty.CDCodeNew,
                [ProductType] = sty.ProductType,
                [FabricType] = sty.FabricType,
                [Lining] = sty.Lining,
                [Gender] = sty.Gender,
                [Construction] = sty.Construction,
                t.LockStatus,
                t.[Cancel],
                t.[Remark],
                t.[SPFactory],
                t.[NonRevenue],
                t.[Inline_Category],
                t.[Low_output_Reason],
                t.[New_Style_Repeat_style],
                t.ArtworkType
            INTO #tmpSewingGroup
            FROM #tmpSewingDetail t
            OUTER APPLY (
                SELECT s.Manpower 
                FROM Production.dbo.SewingOutput s WITH (NOLOCK)
                WHERE s.ID = t.ID
            ) s
            OUTER APPLY (
                SELECT 
                    ProductType = r2.Name,
                    FabricType = r1.Name,
                    Lining,
                    Gender,
                    Construction = d1.Name,
                    s.CDCodeNew
                FROM Production.dbo.Style s WITH(NOLOCK)
                LEFT JOIN Production.dbo.DropDownList d1 WITH(NOLOCK) ON d1.type = 'StyleConstruction' AND d1.ID = s.Construction
                LEFT JOIN Production.dbo.Reason r1 WITH(NOLOCK) ON r1.ReasonTypeID = 'Fabric_Kind' AND r1.ID = s.FabricType
                LEFT JOIN Production.dbo.Reason r2 WITH(NOLOCK) ON r2.ReasonTypeID = 'Style_Apparel_Type' AND r2.ID = s.ApparelType
                WHERE s.ID = t.OrderStyle 
                  AND s.SeasonID = t.OrderSeason 
                  AND s.BrandID = t.OrderBrandID
            ) sty;

            -- 計算輸出日期範圍
            SELECT 
                [MaxOutputDate] = MAX(OutputDate), 
                [MinOutputDate] = MIN(OutputDate), 
                MockupStyle, 
                OrderStyle, 
                SewingLineID, 
                FactoryID 
            INTO #tmpOutputDate
            FROM (
                SELECT DISTINCT 
                    OutputDate, 
                    MockupStyle, 
                    OrderStyle, 
                    SewingLineID, 
                    FactoryID 
                FROM #tmpSewingGroup
            ) a
            GROUP BY 
                MockupStyle, 
                OrderStyle, 
                SewingLineID, 
                FactoryID;

            -- 獲取縫製輸出資料
            SELECT DISTINCT 
                t.FactoryID, 
                t.SewingLineID, 
                t.OrderStyle, 
                t.MockupStyle, 
                s.OutputDate
            INTO #tmpSewingOutput
            FROM #tmpOutputDate t
            INNER JOIN Production.dbo.SewingOutput s WITH (NOLOCK) ON s.SewingLineID = t.SewingLineID 
                                                                 AND s.FactoryID = t.FactoryID 
                                                                 AND s.OutputDate BETWEEN DATEADD(DAY, -240, t.MinOutputDate) AND t.MaxOutputDate
            WHERE EXISTS (
                SELECT 1 
                FROM Production.dbo.SewingOutput_Detail sd WITH (NOLOCK)
                LEFT JOIN Production.dbo.Orders o WITH (NOLOCK) ON o.ID = sd.OrderId
                LEFT JOIN Production.dbo.MockupOrder mo WITH (NOLOCK) ON mo.ID = sd.OrderId
                WHERE s.ID = sd.ID AND (o.StyleID = t.OrderStyle OR mo.StyleID = t.MockupStyle)
            )
            ORDER BY t.FactoryID, t.SewingLineID, t.OrderStyle, t.MockupStyle, s.OutputDate;

            -- 獲取工時資料
            SELECT 
                w.FactoryID, 
                w.SewingLineID, 
                t.OrderStyle, 
                t.MockupStyle, 
                w.Date
            INTO #tmpWorkHour
            FROM Production.dbo.WorkHour w WITH (NOLOCK)
            LEFT JOIN #tmpOutputDate t ON t.SewingLineID = w.SewingLineID 
                                      AND t.FactoryID = w.FactoryID 
                                      AND w.Date BETWEEN t.MinOutputDate AND t.MaxOutputDate
            WHERE 
                w.Holiday = 0 
                AND ISNULL(w.Hours, 0) != 0 
                AND w.Date >= (SELECT DATEADD(DAY, -240, MIN(MinOutputDate)) FROM #tmpOutputDate) 
                AND w.Date <= (SELECT MAX(MaxOutputDate) FROM #tmpOutputDate)
            ORDER BY w.FactoryID, w.SewingLineID, t.OrderStyle, t.MockupStyle, w.Date;

            -- 處理首次篩選資料
            SELECT 
                t.*,
                [LastShift] = IIF(t.Shift <> 'O' AND t.Category <> 'M' AND t.LocalOrder = 1, 'I', t.Shift),
                [FtyType] = f.Type,
                [FtyCountry] = f.CountryID,
                [CumulateDate] = CumulateDate.val,
                [RFT] = IIF(ISNULL(A.InspectQty, 0) = 0, 0, ROUND(((A.InspectQty - A.RejectQty) / A.InspectQty) * 100, 2))
            INTO #tmp1stFilter
            FROM #tmpSewingGroup t
            LEFT JOIN Production.dbo.Factory f WITH (NOLOCK) ON t.FactoryID = f.ID
            OUTER APPLY (
                SELECT val = IIF(COUNT(1) = 0, 1, COUNT(1))
                FROM #tmpSewingOutput s
                WHERE s.FactoryID = t.FactoryID 
                  AND s.MockupStyle = t.MockupStyle 
                  AND s.OrderStyle = t.OrderStyle 
                  AND s.SewingLineID = t.SewingLineID 
                  AND s.OutputDate <= t.OutputDate 
                  AND s.OutputDate > (
                        SELECT CASE 
                                    WHEN MAX(IIF(s1.OutputDate IS NULL, w.Date, NULL)) IS NOT NULL THEN MAX(IIF(s1.OutputDate IS NULL, w.Date, NULL))
                                    WHEN MIN(w.Date) IS NOT NULL THEN DATEADD(DAY, -1, MIN(w.Date))
                                    ELSE t.OutputDate 
                                END
                        FROM #tmpWorkHour w 
                        LEFT JOIN #tmpSewingOutput s1 ON 
                            s1.OutputDate = w.Date 
                            AND s1.FactoryID = w.FactoryID 
                            AND s1.MockupStyle = t.MockupStyle 
                            AND s1.OrderStyle = t.OrderStyle 
                            AND s1.SewingLineID = w.SewingLineID
                        WHERE w.FactoryID = t.FactoryID
                          AND ISNULL(w.MockupStyle, t.MockupStyle) = t.MockupStyle
                          AND ISNULL(w.OrderStyle, t.OrderStyle) = t.OrderStyle
                          AND w.SewingLineID = t.SewingLineID
                          AND w.Date <= t.OutputDate
                    )
            ) CumulateDate
            LEFT JOIN Production.dbo.RFT A WITH (NOLOCK) ON A.OrderID = t.OrderId
                                                        AND A.CDate = t.OutputDate
                                                        AND A.SewinglineID = t.SewinglineID
                                                        AND A.FactoryID = t.FactoryID
                                                        AND A.Shift = t.Shift
                                                        AND A.Team = t.Team 
            WHERE t.OrderCategory IN ('B', 'S'); -- Artwork

            -- 處理最終數據
            SELECT 
                *
            INTO #Final 
            FROM (
                SELECT DISTINCT
                    MDivisionID, t.FactoryID,
                    t.ComboType,
                    FtyType = IIF(FtyType = 'B', 'Bulk', IIF(FtyType = 'S', 'Sample', FtyType)),
                    FtyCountry,
                    t.OutputDate,
                    t.SewingLineID,
                    [Shift] = CASE    
                                  WHEN t.LastShift = 'D' THEN 'Day'
                                  WHEN t.LastShift = 'N' THEN 'Night'
                                  WHEN t.LastShift = 'O' THEN 'Subcon-Out'
                                  WHEN t.LastShift = 'I' AND SubconInSisterFty = 1 THEN 'Subcon-In(Sister)'
                                  ELSE 'Subcon-In(Non Sister)' 
                              END,
                    [LastShift],
                    t.SubconOutFty,
                    t.SubConOutContractNumber,
                    t.Team,
                    t.OrderId,
                    t.Article,
                    t.SizeCode,
                    CustPONo,
                    t.BuyerDelivery,
                    t.SciDelivery,
                    t.OrderQty,
                    Brand = IIF(t.Category = 'M', MockupBrandID, OrderBrandID),
                    Category = IIF(t.OrderCategory = 'M', 'Mockup', 
                               IIF(LocalOrder = 1, 'Local Order', 
                               IIF(t.OrderCategory = 'B', 'Bulk', 
                               IIF(t.OrderCategory = 'S', 'Sample', 
                               IIF(t.OrderCategory = 'G', 'Garment', ''))))),
                    Program = IIF(t.Category = 'M', MockupProgram, OrderProgram),
                    OrderType,
                    IsDevSample,
                    CPURate = IIF(t.Category = 'M', MockupCPUFactor, OrderCPUFactor),
                    Style = IIF(t.Category = 'M', MockupStyle, OrderStyle),
                    Season = IIF(t.Category = 'M', MockupSeason, OrderSeason),
                    CDNo = IIF(t.Category = 'M', MockupCDCodeID, OrderCdCodeID) + '-' + t.ComboType,
                    ActManPower = ActManPower,
                    WorkHour,
                    ManHour = ROUND(ActManPower * WorkHour, 2),
                    TargetCPU = ROUND(ROUND(ActManPower * WorkHour, 2) * 3600 / StdTMS, 2),
                    TMS = IIF(t.Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate) * StdTMS,
                    CPUPrice = IIF(t.Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate),
                    TargetQty = IIF(IIF(t.Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate) > 0,
                                    ROUND(ROUND(ActManPower * WorkHour, 2) * 3600 / StdTMS, 2) / 
                                    IIF(t.Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate), 0),
                    t.QAQty,
                    TotalCPU = ROUND(IIF(t.Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate) * t.QAQty, 3),
                    CPUSewer = IIF(ROUND(ActManPower * WorkHour, 2) > 0,
                                   (IIF(t.Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate) * t.QAQty) / 
                                   ROUND(ActManPower * WorkHour, 2), 0),
                    EFF = ROUND(IIF(ROUND(ActManPower * WorkHour, 2) > 0,
                                    ((IIF(t.Category = 'M', MockupCPU * MockupCPUFactor, OrderCPU * OrderCPUFactor * Rate) * t.QAQty) / 
                                    (ROUND(ActManPower * WorkHour, 2) * 3600 / StdTMS)) * 100, 0), 1),
                    RFT,
                    CumulateDate,
                    DateRange = IIF(CumulateDate >= 10, '>=10', CONVERT(VARCHAR, CumulateDate)),
                    InlineQty, 
                    Diff = t.QAQty - InlineQty,
                    rate,
                    SewingReasonDesc = ISNULL(t.SewingReasonDesc, ''),
                    t.CDCodeNew,
                    t.ProductType,
                    t.FabricType,
                    t.Lining,
                    t.Gender,
                    t.Construction,
                    t.LockStatus,
                    t.Cancel,
                    t.Remark,
                    t.SPFactory,
                    t.NonRevenue,
                    t.Inline_Category,
                    t.Low_output_Reason,
                    t.New_Style_Repeat_Style,
                    [FCategory] = t.Category,
                    t.ArtworkType
                FROM #tmp1stFilter t 
            ) a
            ORDER BY MDivisionID, FactoryID, OutputDate, SewingLineID, Shift, Team, OrderId, Article, SizeCode;

            -- 獲取美術類型
            SELECT 
                ID,
                Seq,
                ArtworkUnit,
                ProductionUnit
            INTO #AT
            FROM Production.dbo.ArtworkType WITH (NOLOCK)
            WHERE Classify IN ('I', 'A', 'P') 
              AND IsTtlTMS = 0 
              AND Junk = 0;

            -- 處理美術類型單位
            SELECT 
                ID,
                Seq,
                ArtworkType_Unit = CONCAT(ID, IIF(Unit = 'QTY', '(Price)', IIF(Unit = '', '', '(' + Unit + ')'))),
                Unit,
                ArtworkType_CPU = IIF(Unit = 'TMS', CONCAT(ID, '(CPU)'), '')
            INTO #atall
            FROM (
                SELECT ID, Seq, Unit = ArtworkUnit FROM #AT WHERE ArtworkUnit != '' AND ProductionUnit != ''
                UNION
                SELECT ID, Seq, Unit = ProductionUnit FROM #AT WHERE ArtworkUnit != '' AND ProductionUnit != ''
                UNION
                SELECT ID, Seq, Unit = ArtworkUnit FROM #AT WHERE ArtworkUnit != '' AND ProductionUnit = ''
                UNION    
                SELECT ID, Seq, Unit = ProductionUnit FROM #AT WHERE ArtworkUnit = '' AND ProductionUnit != ''
                UNION
                SELECT ID, Seq, Unit = '' FROM #AT WHERE ArtworkUnit = '' AND ProductionUnit = ''
            ) a;

            -- 美術類型進一步處理
            SELECT *
            INTO #atall2
            FROM (
                SELECT a.ID, a.Seq, c = 1, a.ArtworkType_Unit, a.Unit FROM #atall a
                UNION
                SELECT a.ID, a.Seq, c = 2, a.ArtworkType_CPU, IIF(a.ArtworkType_CPU = '', '', 'CPU') FROM #atall a
                WHERE a.ArtworkType_CPU != ''
            ) b;

            -- 準備台北資料(須排除這些)
            SELECT ps.ID
            INTO #TPEtmp
            FROM Production.dbo.PO_Supp ps WITH (NOLOCK)
            INNER JOIN Production.dbo.PO_Supp_Detail psd WITH (NOLOCK) ON ps.ID = psd.id AND ps.SEQ1 = psd.Seq1
            INNER JOIN Production.dbo.Fabric fb WITH (NOLOCK) ON psd.SCIRefno = fb.SCIRefno 
            INNER JOIN Production.dbo.MtlType ml WITH (NOLOCK) ON ml.id = fb.MtlTypeID
            WHERE ml.Junk = 0 
              AND psd.Junk = 0 
              AND fb.Junk = 0
              AND ml.isThread = 1 
              AND ps.SuppID <> 'FTY' 
              AND ps.Seq1 NOT LIKE '5%';

            -- 訂單、美術類型和序列號的組合
            SELECT DISTINCT 
                ot.ID,
                ot.ArtworkTypeID,
                ot.Seq,
                ot.Qty,
                ot.Price,
                ot.TMS,
                t.QAQty,
                t.FactoryID,
                t.Team,
                t.OutputDate,
                t.SewingLineID,
                t.SubConOutContractNumber,
                IIF(t.Shift <> 'O' AND t.Category <> 'M' AND t.LocalOrder = 1, 'I', t.Shift) AS LastShift,
                t.Category,
                t.ComboType,
                t.SubconOutFty,
                t.Article,
                t.SizeCode
            INTO #idat
            FROM #tmpSewingGroup t
            INNER JOIN Production.dbo.Order_TmsCost ot WITH (NOLOCK) ON ot.id = t.OrderId
            INNER JOIN Production.dbo.orders o WITH (NOLOCK) ON o.ID = t.OrderId
            INNER JOIN #AT A ON A.ID = ot.ArtworkTypeID
            WHERE ((ot.ArtworkTypeID = 'SP_THREAD' AND NOT EXISTS(SELECT 1 FROM #TPEtmp t WHERE t.ID = o.POID))
                  OR ot.ArtworkTypeID <> 'SP_THREAD');

            -- 透過PIVOT轉置資料
            SELECT 
                orderid,
                SubconOutFty,
                SubConOutContractNumber,
                FactoryID,
                Team,
                OutputDate,
                SewingLineID,
                LastShift,
                Category,
                ComboType,
                qaqty,
                Article,
                SizeCode 
                {this.nameZ}
            INTO #oid_at
            FROM (
                SELECT 
                    orderid = i.ID,
                    a.ArtworkType_Unit,
                    i.qaqty,
                    ptq = IIF(a.Unit = 'QTY', i.Price, 
                          IIF(a.Unit = 'TMS', i.TMS, 
                          IIF(a.Unit = 'CPU', i.Price, i.Qty))),
                    i.FactoryID,
                    i.Team,
                    i.OutputDate,
                    i.SewingLineID,
                    i.LastShift,
                    i.Category,
                    i.ComboType,
                    i.SubconOutFty,
                    i.SubConOutContractNumber,
                    i.Article,
                    i.SizeCode
                FROM #atall2 a 
                LEFT JOIN #idat i ON i.ArtworkTypeID = a.ID AND i.Seq = a.Seq
            ) a
            PIVOT (
                MIN(ptq) FOR ArtworkType_Unit IN ({this.columnsName})
            ) AS pt
            WHERE orderid IS NOT NULL;

            -- 最終結果集
            SELECT 
                MDivisionID, FactoryID, ComboType, FtyType, FtyCountry, OutputDate, SewingLineID, Shift,
                SubconOutFty, SubConOutContractNumber, Team, OrderID, Article, SizeCode, CustPONo, BuyerDelivery,
                OrderQty, Brand, Category, Program, OrderType, IsDevSample, CPURate, Style, Season, CDNo, ActManPower,
                WorkHour, ManHour, TargetCPU, TMS, CPUPrice, TargetQty, QAQTY, TotalCPU, CPUSewer, EFF, RFT, CumulateDate,
                DateRange, InlineQty, Diff, Rate, SewingReasonDesc, SciDelivery, CDCodeNew, ProductType, FabricType,
                Lining, Gender, Construction, LockStatus, Cancel, Remark, SPFactory, NonRevenue, Inline_Category,
                Low_output_Reason, New_Style_Repeat_Style, ArtworkType {this.nameFinal}
            FROM (
                SELECT 
                    t.MDivisionID, t.FactoryID, t.ComboType, t.FtyType, t.FtyCountry, t.OutputDate, t.SewingLineID, t.Shift,
                    t.SubconOutFty, t.SubConOutContractNumber, t.Team, t.OrderID, t.Article, t.SizeCode, t.CustPONo, t.BuyerDelivery,
                    t.OrderQty, t.Brand, t.Category, t.Program, t.OrderType, t.IsDevSample, t.CPURate, t.Style, t.Season, t.CDNo, t.ActManPower,
                    t.WorkHour, t.ManHour, t.TargetCPU, t.TMS, t.CPUPrice, t.TargetQty, t.QAQTY, t.TotalCPU, t.CPUSewer, t.EFF, t.RFT, t.CumulateDate,
                    t.DateRange, t.InlineQty, t.Diff, t.Rate, t.SewingReasonDesc, t.SciDelivery, t.CDCodeNew, t.ProductType, t.FabricType,
                    t.Lining, t.Gender, t.Construction, t.LockStatus, t.Cancel, t.Remark, t.SPFactory, t.NonRevenue, t.Inline_Category,
                    t.Low_output_Reason, t.New_Style_Repeat_Style, t.ArtworkType
                    {this.tTLZ}
                FROM #Final t
                LEFT JOIN #oid_at o ON o.orderid = t.OrderId 
                                    AND o.FactoryID = t.FactoryID
                                    AND o.Team = t.Team 
                                    AND o.OutputDate = t.OutputDate 
                                    AND o.SewingLineID = t.SewingLineID 
                                    AND o.LastShift = t.LastShift 
                                    AND o.Category = t.[FCategory] 
                                    AND o.ComboType = t.ComboType 
                                    AND o.SubconOutFty = t.SubconOutFty 
                                    AND o.SubConOutContractNumber = t.SubConOutContractNumber 
                                    AND o.Article = t.Article 
                                    AND o.SizeCode = t.SizeCode
            ) a
            ";
            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sqlcmd, sqlParameters, out DataTable dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTables;
            return resultReport;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, ExecutedList item)
        {
            Base_ViewModel finalResult;
            Data.DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@BIFactoryID", item.RgCode),
                new SqlParameter("@IsTrans", item.IsTrans),
            };

            using (sqlConn)
            {
                string sql = $@"	
				update t
				set 
				t.MDivisionID =s.MDivisionID
				,t.FactoryID =s.FactoryID
				,t.ComboType =s.ComboType
				,t.Category =s.FtyType
				,t.CountryID =s.FtyCountry
				,t.OutputDate =s.OutputDate
				,t.SewingLineID =s.SewingLineID
				,t.Shift =s.Shift
				,t.SubconOutFty =s.SubconOutFty
				,t.SubConOutContractNumber =s.SubConOutContractNumber
				,t.Team =s.Team
				,t.OrderID =s.OrderID
				,t.Article =s.Article
				,t.SizeCode =s.SizeCode
				,t.CustPONo = s.CustPONo
				,t.BuyerDelivery = s.BuyerDelivery
				,t.OrderQty = s.OrderQty
				,t.BrandID = s.Brand
				,t.OrderCategory = s.Category
				,t.ProgramID = s.Program
				,t.OrderTypeID = s.OrderType
				,t.DevSample = s.IsDevSample
				,t.CPURate = s.CPURate
				,t.StyleID = s.Style
				,t.Season = s.Season
				,t.CdCodeID = s.CDNo
				,t.ActualManpower = s.ActManPower
				,t.NoOfHours = s.WorkHour
				,t.TotalManhours = s.ManHour
				,t.TargetCPU = s.TargetCPU
				,t.TMS = s.TMS
				,t.CPUPrice = s.CPUPrice
				,t.TargetQty = s.TargetQty
				,t.TotalOutputQty = s.QAQTY
				,t.TotalCPU = s.TotalCPU
				,t.CPUSewerHR = s.CPUSewer
				,t.EFF = s.EFF
				,t.RFT = s.RFT
				,t.CumulateOfDays = s.CumulateDate
				,t.DateRange = s.DateRange
				,t.ProdOutput = s.InlineQty
				,t.Diff = s.Diff
				,t.Rate = s.Rate
				,t.SewingReasonDesc = s.SewingReasonDesc
				,t.SciDelivery = s.SciDelivery
				,t.CDCodeNew = s.CDCodeNew
				,t.ProductType = s.ProductType
				,t.FabricType = s.FabricType
				,t.Lining = s.Lining
				,t.Gender = s.Gender
				,t.Construction = s.Construction
				,t.LockStatus = s.LockStatus
				,t.Cancel = s.Cancel
				,t.Remark = s.Remark
				,t.SPFactory = s.SPFactory
				,t.NonRevenue = s.NonRevenue
				,t.Inline_Category = s.Inline_Category
				,t.Low_output_Reason = s.Low_output_Reason
				,t.New_Style_Repeat_Style = s.New_Style_Repeat_Style
				,t.ArtworkType = s.ArtworkType
				,t.[BIFactoryID] = @BIFactoryID
				,t.[BIInsertDate] = GETDATE()
				,t.[BIStatus] = 'New'
				{this.updateColumns}
				from P_SewingDailyOutput t
				inner join #FinalDt s on t.FactoryID=s.FactoryID  
								   AND t.MDivisionID=s.MDivisionID 
								   AND t.SewingLineID=s.SewingLineID 
								   AND t.Team=s.Team 
								   AND t.Shift=s.Shift 
								   AND t.OrderId=s.OrderId 
								   AND t.Article=s.Article 
								   AND t.SizeCode=s.SizeCode 
								   AND t.ComboType=s.ComboType  
								   AND t.OutputDate = s.OutputDate
								   AND t.SubConOutContractNumber = s.SubConOutContractNumber
				;
				insert into P_SewingDailyOutput
				(
					  MDivisionID, FactoryID, ComboType, Category, CountryID, OutputDate, SewingLineID, Shift
					, SubconOutFty, SubConOutContractNumber, Team, OrderID, Article, SizeCode, CustPONo, BuyerDelivery
					, OrderQty, BrandID, OrderCategory, ProgramID, OrderTypeID, DevSample, CPURate, StyleID, Season, CdCodeID, ActualManpower
					, NoOfHours, TotalManhours, TargetCPU, TMS, CPUPrice, TargetQty, TotalOutputQty, TotalCPU, CPUSewerHR, EFF, RFT, CumulateOfDays
					, DateRange, ProdOutput, Diff, Rate, SewingReasonDesc, SciDelivery, CDCodeNew, ProductType, FabricType
					, Lining, Gender, Construction, LockStatus, Cancel, Remark, SPFactory, NonRevenue, Inline_Category
					, Low_output_Reason, New_Style_Repeat_Style,ArtworkType
					{this.finalColumns}
					, [BIFactoryID], [BIInsertDate], [BIStatus]
				)
				select 
				s.MDivisionID,s.FactoryID,s.ComboType,s.FtyType,s.FtyCountry,s.OutputDate,s.SewingLineID,s.Shift
				,s.SubconOutFty,s.SubConOutContractNumber,s.Team,s.OrderID,s.Article,s.SizeCode,s.CustPONo,s.BuyerDelivery
				,s.OrderQty,s.Brand,s.Category,s.Program,s.OrderType,s.IsDevSample,s.CPURate,s.Style,s.Season,s.CDNo,s.ActManPower
				,s.WorkHour,s.ManHour,s.TargetCPU,s.TMS,s.CPUPrice,s.TargetQty,s.QAQTY,s.TotalCPU,s.CPUSewer,s.EFF,s.RFT,s.CumulateDate
				,s.DateRange,s.InlineQty,s.Diff,s.Rate,s.SewingReasonDesc,s.SciDelivery,s.CDCodeNew,s.ProductType,s.FabricType
				,s.Lining,s.Gender,s.Construction,s.LockStatus, s.Cancel, s.Remark, s.SPFactory, s.NonRevenue, s.Inline_Category
				,s.Low_output_Reason, s.New_Style_Repeat_Style, s.ArtworkType
				{this.insertColumns}
				, @BIFactoryID, GETDATE(), 'New'
				from #FinalDt s
				where not exists (select 1 from P_SewingDailyOutput t where t.FactoryID=s.FactoryID  
																	   AND t.MDivisionID=s.MDivisionID 
																	   AND t.SewingLineID=s.SewingLineID 
																	   AND t.Team=s.Team 
																	   AND t.Shift=s.Shift 
																	   AND t.OrderId=s.OrderId 
																	   AND t.Article=s.Article 
																	   AND t.SizeCode=s.SizeCode 
																	   AND t.ComboType=s.ComboType  
																	   AND t.OutputDate = s.OutputDate
																	   AND t.SubConOutContractNumber = s.SubConOutContractNumber)
				;
				INSERT INTO [dbo].[P_SewingDailyOutput_History]
						   ([Ukey],[MDivisionID],[BIFactoryID],[BIInsertDate],[BIStatus])
				SELECT t.Ukey, t.MDivisionID, @BIFactoryID, GETDATE(), 'New'
				from P_SewingDailyOutput t 
				where t.OutputDate in (select outputDate from #FinalDt)
				and exists (select OrderID from #FinalDt f where t.FactoryID=f.FactoryID  AND t.MDivisionID=f.MDivisionID ) 
				and not exists (
				select OrderID from #FinalDt s 
					where t.FactoryID=s.FactoryID  
					AND t.MDivisionID=s.MDivisionID 
					AND t.SewingLineID=s.SewingLineID 
					AND t.Team=s.Team 
					AND t.Shift=s.Shift 
					AND t.OrderID=s.OrderID 
					AND t.Article=s.Article 
					AND t.SizeCode=s.SizeCode 
					AND t.ComboType=s.ComboType 
					AND t.OutputDate = s.OutputDate
					AND t.SubConOutContractNumber = s.SubConOutContractNumber)
				;
				delete t
				from P_SewingDailyOutput t 
				where t.OutputDate in (select outputDate from #FinalDt)
				and exists (select OrderID from #FinalDt f where t.FactoryID=f.FactoryID  AND t.MDivisionID=f.MDivisionID ) 
				and not exists (
				select OrderID from #FinalDt s 
					where t.FactoryID=s.FactoryID  
					AND t.MDivisionID=s.MDivisionID 
					AND t.SewingLineID=s.SewingLineID 
					AND t.Team=s.Team 
					AND t.Shift=s.Shift 
					AND t.OrderID=s.OrderID 
					AND t.Article=s.Article 
					AND t.SizeCode=s.SizeCode 
					AND t.ComboType=s.ComboType 
					AND t.OutputDate = s.OutputDate
					AND t.SubConOutContractNumber = s.SubConOutContractNumber)
				;";

                Dictionary<string, string> columnTypes = new Dictionary<string, string>()
                {
                    { "FactoryID", "varchar(8000)" },
                    { "MDivisionID", "varchar(8000)" },
                    { "SewingLineID", "varchar(8000)" },
                    { "Team", "varchar(8000)" },
                    { "Shift", "varchar(8000)" },
                    { "OrderID", "varchar(8000)" },
                    { "Article", "varchar(8000)" },
                    { "SizeCode", "varchar(8000)" },
                    { "ComboType", "varchar(8000)" },
                    { "SubConOutContractNumber", "varchar(8000)" },
                };
                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable dataTable, temptablename: "#FinalDt", conn: sqlConn, paramters: sqlParameters, columnTypes: columnTypes),
                };
            }

            return finalResult;
        }
    }
}
