using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <summary>
    /// 有調整到需要一併更新至BI
    /// </summary>
    public class QA_R51
    {
        /// <inheritdoc/>
        public QA_R51()
        {
             DBProxy.Current.DefaultTimeout = 1800;
        }

        /// <inheritdoc/>
        public Base_ViewModel Get_QA_R51(QA_R51_ViewModel model)
        {
            var srWhere = this.GetSRWhere(model);

            var itemWhere = this.GetWhere(model);

            var itemCol_Join = this.GetCol_Join(model.FormatType, model.IsBI);

            string sqlcmd = $@"
            select 
            *
            into #SubProInsRecord
            from(
	            select  
	            SR.*,
	            [Inspector] = CONCAT(a.ID, ':', a.Name)
	            ,RowNo = ROW_NUMBER() over(partition by BundleNo,SubProcessID order by sr.Adddate desc)
	            from SubProInsRecord SR
                LEFT join [ExtendServer].ManufacturingExecution.dbo.Pass1 a WITH (NOLOCK) on a.ID = SR.AddName
                where 1 = 1
                {srWhere}
            )aa
            where RowNo = 1


            select
            SR.FactoryID,
            Fac.MDivisionID,
            SR.SubProLocationID,
	        SR.InspectionDate,
            O.SewInLine,
	        B.Sewinglineid,
            SR.Shift,
	        [RFT] = isnull(RftValue.val,0),
	        SR.SubProcessID,
	        SR.BundleNo,
            [Artwork] = Artwork.val,
	        B.OrderID,
            Country.Alias,
            s.AbbEN,
            PSD.Refno,
            o.BuyerDelivery,
	        BD.BundleGroup,
            o.SeasonID,
	        O.styleID,
	        B.Colorid,
	        BD.SizeCode,
            BD.PatternDesc,
            B.Item,
	        BD.Qty,
	        SR.RejectQty,
	        SR.Machine,
	        {itemCol_Join.Item1}
	        Inspector = SR.Inspector,
	        SR.Remark,
            AddDate2 = SR.AddDate,
            SR.RepairedDatetime,
	        RepairedTime = iif(RepairedDatetime is null, null, ttlSecond),
            {itemCol_Join.Item2}
	        ResolveTime = iif(isnull(ttlSecond_RD, 0) = 0, null, ttlSecond_RD),
	        SubProResponseTeamID
            ,CustomColumn1
            into #tmp
            from #SubProInsRecord SR WITH (NOLOCK)
            Left join Bundle_Detail BD WITH (NOLOCK) on SR.BundleNo=BD.BundleNo
            Left join Bundle B WITH (NOLOCK) on BD.ID=B.ID
            Left join Orders O WITH (NOLOCK) on B.OrderID=O.ID
            left join Country on Country.ID = o.Dest
            Left JOIN WorkOrderForOutput WO ON WO.CutRef=B.CutRef and b.CutRef <> '' and wo.ID = b.POID and wo.OrderID =b.Orderid
            Left JOIN PO_Supp_Detail PSD WITH (NOLOCK) ON PSD.ID=WO.ID AND PSD.SEQ1 = WO.SEQ1 AND PSD.SEQ2=WO.SEQ2
            Left JOIN PO_SUPP PS WITH (NOLOCK) ON PS.ID= PSD.ID AND PS.SEQ1=PSD.SEQ1
            Left JOIN Supp S WITH (NOLOCK) ON S.ID=PS.SuppID
            outer apply(SELECT val =  Stuff((select distinct concat( '+',SubprocessId)   
                                                from Bundle_Detail_Art bda with (nolock) 
                                                where bda.Bundleno = BD.Bundleno
                                                FOR XML PATH('')),1,1,'') ) Artwork
            LEFT JOIN Factory fac WITH(NOLOCK) on fac.ID = SR.FactoryID and fac.Junk =0
            {itemCol_Join.Item3}
            outer apply(select ttlSecond = DATEDIFF(Second, SR.AddDate, RepairedDatetime)) ttlSecond
            outer apply
            (
                SELECT 
                VAL = IIF(isnull((R.InspectQty+R.RejectQty),0) = 0, 0, round((R.InspectQty / (R.InspectQty+R.RejectQty))*100,2))
                FROM SewingOutput_Detail sod 
                inner join SewingOutput so with(nolock) on so.id = sod.id
                inner join Rft r on r.OrderID = sod.OrderId AND
					                r.SewinglineID = so.SewingLineID AND
					                r.Team = so.Team AND
					                r.Shift = so.Shift AND
					                r.CDate = so.OutputDate
                WHERE sod.OrderId = O.ID and so.SewinglineID = B.SewinglineID and so.FactoryID=SR.FactoryID 
	            and so.Shift= iif(SR.Shift = 'Day','D','N') 
	            and r.CDate = O.SewInLine
            )RftValue
            Where 1=1
            {itemWhere.Item1}

            UNION

            select
            SR.FactoryID,
            Fac.MDivisionID,
            SR.SubProLocationID,
	        SR.InspectionDate,
            O.SewInLine,
            BR.Sewinglineid,
            SR.Shift,
	        [RFT] = isnull(RftValue.val,0),
	        SR.SubProcessID,
	        SR.BundleNo,
            [Artwork] = Artwork.val,
	        BR.OrderID,
            Country.Alias,
            s.AbbEN,
            PSD.Refno,
            o.BuyerDelivery,
	        BRD.BundleGroup,
            o.SeasonID,
	        O.styleID,
	        BR.Colorid,
	        BRD.SizeCode,
            BRD.PatternDesc,
            BR.Item,
	        BRD.Qty,
	        SR.RejectQty,
	        SR.Machine,
	        {itemCol_Join.Item1}
	        Inspector = SR.Inspector,
	        SR.Remark,
            AddDate2 = SR.AddDate,
            SR.RepairedDatetime,
	        iif(RepairedDatetime is null, null, ttlSecond),
            {itemCol_Join.Item2}
	        iif(isnull(ttlSecond_RD, 0) = 0, null, ttlSecond_RD),
	        SubProResponseTeamID
            ,CustomColumn1--自定義欄位, 在最後一個若有變動,則輸出Excel部分也要一起改
            from #SubProInsRecord SR WITH (NOLOCK)
            Left join BundleReplacement_Detail BRD WITH (NOLOCK) on SR.BundleNo=BRD.BundleNo
            Left join BundleReplacement BR WITH (NOLOCK) on BRD.ID=BR.ID
            Left join Orders O WITH (NOLOCK) on BR.OrderID=O.ID
            left join Country on Country.ID = o.Dest
            Left join Bundle_Detail BD WITH (NOLOCK) on SR.BundleNo=BD.BundleNo
            Left JOIN Bundle B WITH (NOLOCK) ON BD.ID=B.ID
            Left JOIN WorkOrderForOutput WO ON WO.CutRef=B.CutRef and b.CutRef <> '' and wo.ID = b.POID and wo.OrderID =b.Orderid
            Left JOIN PO_Supp_Detail PSD WITH (NOLOCK) ON PSD.ID=WO.ID AND PSD.SEQ1 = WO.SEQ1 AND PSD.SEQ2=WO.SEQ2
            Left JOIN PO_SUPP PS WITH (NOLOCK) ON PS.ID= PSD.ID AND PS.SEQ1=PSD.SEQ1
            Left JOIN Supp S WITH (NOLOCK) ON S.ID=PS.SuppID
            outer apply(SELECT val =  Stuff((select distinct concat( '+',SubprocessId)   
                                                from Bundle_Detail_Art bda with (nolock) 
                                                where bda.Bundleno = SR.BundleNo
                                                FOR XML PATH('')),1,1,'') ) Artwork
            LEFT JOIN Factory fac WITH(NOLOCK) on fac.ID = SR.FactoryID and fac.Junk =0
            {itemCol_Join.Item3}
            outer apply(select ttlSecond = DATEDIFF(Second, SR.AddDate, RepairedDatetime)) ttlSecond
            outer apply
            (
                SELECT 
                VAL = IIF(isnull((R.InspectQty+R.RejectQty),0) = 0, 0, round((R.InspectQty / (R.InspectQty+R.RejectQty))*100,2))
                FROM SewingOutput_Detail sod 
                inner join SewingOutput so with(nolock) on so.id = sod.id
                inner join Rft r on r.OrderID = sod.OrderId AND
					                r.SewinglineID = so.SewingLineID AND
					                r.Team = so.Team AND
					                r.Shift = so.Shift AND
					                r.CDate = so.OutputDate
                WHERE sod.OrderId = O.ID and so.SewinglineID = B.SewinglineID and so.FactoryID=SR.FactoryID 
	            and so.Shift= iif(SR.Shift = 'Day','D','N') 
	            and r.CDate = O.SewInLine
            )RftValue
            Where 1=1
            {itemWhere.Item2}

            select *, BundleNoCT = COUNT(1) over(partition by t.BundleNo)
            into #tmp2
            from #tmp t

            select*
            into #tmp3
            from #tmp2 t
            where BundleNoCT = 1--綁包 / 補料都沒有,在第一段union會合併成一筆
            or(BundleNoCT > 1 and isnull(t.Orderid, '') <> '')--綁包 / 補料其中一個有

            -- SubProInsRecord可能會有多筆相同BundleNo 和 SubProcessID, 所以只取AddDate最後一筆資料
            -- by ISP20230577
            select * from #tmp3

            drop table #tmp,#tmp2,#tmp3

            ";

            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@StartInspectionDate", SqlDbType.Date) { Value = (object)model.StartInspectionDate.Value ?? DBNull.Value },
                new SqlParameter("@EndInspectionDate", SqlDbType.Date) { Value = (object)model.EndInspectionDate.Value ?? DBNull.Value },
                new SqlParameter("@SP", SqlDbType.VarChar, 15) { Value = (object)model.SP ?? DBNull.Value },
                new SqlParameter("@M", SqlDbType.VarChar, 5) { Value = (object)model.M ?? DBNull.Value },
                new SqlParameter("@Style", SqlDbType.VarChar, 20) { Value = (object)model.Style ?? DBNull.Value },
                new SqlParameter("@Factory", SqlDbType.VarChar,5) { Value = (object)model.Factory ?? DBNull.Value },
                new SqlParameter("@Shift", SqlDbType.VarChar, 5) { Value = (object)model.Shift ?? DBNull.Value },
            };

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("Production", sqlcmd, listPar, out DataTable[] dataTables),
            };
            resultReport.DtArr = dataTables;
            return resultReport;
        }

        /// <inheritdoc/>
        public Tuple<string, string, string> GetCol_Join(string type, bool isBI = false)
        {
            string formatCol1 = string.Empty;
            string formatCol2 = string.Empty;
            string formatJoin = string.Empty;
            string sqlDefValue = $@"
            outer apply(select ttlSecond_RD = sum(DATEDIFF(Second, StartResolveDate, EndResolveDate)) from SubProInsRecord_ResponseTeam where EndResolveDate is not null and SubProInsRecordUkey = SR.Ukey)ttlSecond_RD
            outer apply
            (
	            select SubProResponseTeamID = STUFF((
		            select CONCAT(',', SubProResponseTeamID)
		            from SubProInsRecord_ResponseTeam
		            where SubProInsRecordUkey = SR.Ukey
		            order by SubProResponseTeamID
		            for xml path('')
	            ),1,1,'')
            )SubProResponseTeamID
            ";

            switch (type)
            {
                case "Summary":
                    formatJoin = @"
                    outer apply 
                    (
                        select [val] = sum(SRD.DefectQty)
                        from SubProInsRecord_Defect SRD WITH(NOLOCK)
		                where SR.Ukey=SRD.SubProInsRecordUkey 
                    )DefectQty" + sqlDefValue;
                    formatCol1 = "DefectQty.val,";
                    formatCol2 = string.Empty;
                    return Tuple.Create(formatCol1, formatCol2, formatJoin);
                case "DefectType":
                    string strJunk = isBI ? "m.Junk," : "[Junk] = iif(m.Junk = 1, 'Y', 'N'),";
                    formatJoin = @"
                    left join SubProMachine m on SR.Machine = m.ID and SR.FactoryID = m.FactoryID and SR.SubProcessID = m.SubProcessID
                    left join SubProInsRecord_Defect SRD on SR.Ukey = SRD.SubProInsRecordUkey
                    outer apply 
                    (
                        select OperatorID = STUFF((
		                    select CONCAT(',', SubProOperatorEmployeeID)
		                    from SubProInsRecord_Operator with (nolock)
		                    where SubProInsRecordUkey = SR.Ukey
		                    order by SubProOperatorEmployeeID
		                    for xml path('')
	                    ),1,1,'')
                    ) SOE
					outer apply 
                    (
                        select OperatorName = STUFF((
		                    select CONCAT(',', spo.FirstName, '-', spo.LastName)
							from SubProInsRecord_Operator spiro with (nolock)
							inner join SubProOperator spo with (nolock) on spiro.SubProOperatorEmployeeID = spo.EmployeeID
		                    where spiro.SubProInsRecordUkey = SR.Ukey
		                    order by spiro.SubProOperatorEmployeeID
		                    for xml path('')
	                    ),1,1,'')
                    ) spo" + sqlDefValue;
                    formatCol1 = $@"
                    m.Serial,
                    {strJunk}
                    m.Description,
                    SRD.DefectCode,
                    SRD.DefectQty,
                    SOE.OperatorID,
                    spo.OperatorName,";
                    formatCol2 = string.Empty;
                    return Tuple.Create(formatCol1, formatCol2, formatJoin);
                case "ResponseTeam":
                    formatJoin = @"
                    left join SubProInsRecord_Defect SRD on SR.Ukey = SRD.SubProInsRecordUkey
                    left join SubProInsRecord_ResponseTeam SRR on SRR.SubProInsRecordUkey = SR.Ukey
                    outer apply(select ttlSecond_RD = DATEDIFF(Second, StartResolveDate, EndResolveDate))ttlSecond_RD";
                    formatCol1 = @"  
                    SRD.DefectCode,
                    SRD.DefectQty,";
                    formatCol2 = $@"
                    SRR.StartResolveDate,
                    SRR.EndResolveDate,";
                    return Tuple.Create(formatCol1, formatCol2, formatJoin);
                case "Operator":
                    formatJoin = @" left join SubProInsRecord_Defect SRD on SR.Ukey = SRD.SubProInsRecordUkey
                                left join SubProInsRecord_Operator sro with (nolock) on sro.SubProInsRecordUkey = SR.Ukey
                                left join SubProOperator spo with (nolock) on spo.EmployeeID = sro.SubProOperatorEmployeeID
                            " + sqlDefValue;
                    formatCol1 = @"  SRD.DefectCode,
                                SRD.DefectQty,
                                sro.SubProOperatorEmployeeID,
                                [OperatorName] = iif(isnull(sro.SubProOperatorEmployeeID, '') = '', '', spo.FirstName + ' ' + spo.LastName),";
                    formatCol2 = string.Empty;
                    return Tuple.Create(formatCol1, formatCol2, formatJoin);
                case "":
                    formatCol1 = string.Empty;
                    formatCol2 = string.Empty;
                    formatJoin = string.Empty;
                    return Tuple.Create(formatCol1, formatCol2, formatJoin);
                default:
                    throw new ArgumentException($"Unknown FormatType: {type}");
            }
        }

        /// <inheritdoc/>
        public string GetSRWhere(QA_R51_ViewModel model)
        {
            var srWhere = string.Empty;

            if (!model.SubProcess.Empty())
            {
                srWhere = $@" and SR.SubProcessID in ({model.SubProcess})";
            }

            if (!model.StartInspectionDate.Value.Empty())
            {
                srWhere += $@" and SR.InspectionDate between @StartInspectionDate and @EndInspectionDate";
            }

            if (!model.Factory.Empty())
            {
                srWhere += $@" and SR.FactoryID = @Factory";
            }

            if (!model.Shift.Empty())
            {
                srWhere += $@" and SR.Shift = @Shift";
            }

            return srWhere;
        }

        /// <inheritdoc/>
        public Tuple<string, string> GetWhere(QA_R51_ViewModel model)
        {
            string sqlWhere1 = string.Empty;
            string sqlWhere2 = string.Empty;

            if (!model.SP.Empty())
            {
                sqlWhere1 += $@" and B.OrderID = @SP";
                sqlWhere2 += $@" and BR.OrderID = @SP";
            }

            if (!model.Style.Empty())
            {
                sqlWhere1 += $@" and O.StyleID= @Style";
                sqlWhere2 += $@" and O.StyleID= @Style";
            }

            if (!model.M.Empty())
            {
                sqlWhere1 += $@" and Fac.MDivisionID= @M";
                sqlWhere2 += $@" and Fac.MDivisionID= @M";
            }

            return Tuple.Create(sqlWhere1, sqlWhere2);
        }
    }
}