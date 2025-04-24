using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using Sci.Data;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <inheritdoc/>
    public class Warehouse_R40
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Warehouse_R40()
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 10800,
            };
        }

        /// <inheritdoc/>
        public Base_ViewModel GetWarehouse_R40Data(Warehouse_R40_ViewModel model)
        {
            #region Set SQL Command & SQLParameter
            string whereReceiving = string.Empty;
			string whereTransferIn = string.Empty;
			List<SqlParameter> listPar = new List<SqlParameter>();
			Base_ViewModel resultReport = new Base_ViewModel();
			if (!MyUtility.Check.Empty(model.SP1))
			{
				whereReceiving += " and rd.POID >= @Sp1 ";
				whereTransferIn += " and rd.POID >= @Sp1 ";
				listPar.Add(new SqlParameter("@Sp1", model.SP1));
			}

			if (!MyUtility.Check.Empty(model.SP2))
			{
				whereReceiving += " and rd.POID <= @Sp2 ";
				whereTransferIn += " and rd.POID <= @Sp2 ";
				listPar.Add(new SqlParameter("@Sp2", model.SP2));
			}

			if (model.ArriveDateStart.HasValue)
			{
				whereReceiving += " and r.WhseArrival >= @arriveDateStart ";
				whereTransferIn += " and r.IssueDate >= @arriveDateStart ";
				listPar.Add(new SqlParameter("@arriveDateStart", model.ArriveDateStart));
			}

			if (model.ArriveDateEnd.HasValue)
			{
				whereReceiving += " and r.WhseArrival <= @arriveDateEnd ";
				whereTransferIn += " and r.IssueDate <= @arriveDateEnd ";
				listPar.Add(new SqlParameter("@arriveDateEnd", model.ArriveDateEnd));
			}

			if (!MyUtility.Check.Empty(model.WKID1))
			{
				whereReceiving += " and r.ExportID >= @wkStart ";
				whereTransferIn += " and 1 = 0 ";
				listPar.Add(new SqlParameter("@wkStart", model.WKID1));
			}

			if (!MyUtility.Check.Empty(model.WKID2))
			{
				whereReceiving += " and r.ExportID <= @wkEnd ";
				whereTransferIn += " and 1 = 0 ";
				listPar.Add(new SqlParameter("@wkEnd", model.WKID2));
			}

			string whereReceivingAct = string.Empty;
			string whereCutShadeband = string.Empty;
			string whereFabricLab = string.Empty;
			string whereChecker = string.Empty;
			string whereMind = string.Empty;
			string selPowerBI = string.Empty;
			string colPowerBI = string.Empty;
			string isQRCodeCreatedByPMS = "iif (dbo.IsQRCodeCreatedByPMS(rd.MINDQRCode) = 1, 'Create from PMS', '')";

			if (model.Status == "AlreadyUpdated")
			{
				whereReceivingAct += " and ActualWeight != 0 ";
				whereCutShadeband += " and CutShadebandTime is not null";
				whereFabricLab += " and Fabric2LabTime is not null";
				whereChecker += " and ISNULL(Checker,'') <> ''";
			}

			if (model.Status == "NotYetUpdate")
			{
				whereReceivingAct += " and ActualWeight = 0 ";
				whereCutShadeband += " and CutShadebandTime is null";
				whereFabricLab += " and Fabric2LabTime is null";
				whereChecker += " and ISNULL(Checker,'') = ''";
			}

			if (model.Status.EqualString("AlreadyScanned") || model.Status == "AlreadyUpdated")
			{
				whereMind += " and MINDCheckAddDate is not null";
			}

			if (model.Status.EqualString("NotYetScanned") || model.Status == "NotYetUpdate")
			{
				whereMind += " and MINDCheckAddDate is null";
			}

			if (model.IsPowerBI)
			{
				selPowerBI = @", o.FtyGroup
	, r.AddDate
	, r.EditDate
	, [BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
	, [BIInsertDate] = GETDATE()
";
				colPowerBI = @",FtyGroup = isnull(FtyGroup, '')
	,CutShadebandTime --Cut Shadeband
	,CutBy = isnull(CutBy, '') --Cut Shadeband
	,Fabric2LabTime --Fabric to Lab
	,Fabric2LabBy = isnull(Fabric2LabBy, '') --Fabric to Lab
	,Checker = isnull(Checker, '') --Checker
	,rd.AddDate
	,rd.EditDate
	,rdStockType = isnull(rdStockType, '')
	, [BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
	, [BIInsertDate] = GETDATE()
";
				if (model.AddEditDateStart.HasValue)
				{
					whereReceiving += " and (r.AddDate >= @AddEditDateStart or r.EditDate >= @AddEditDateStart)";
					whereTransferIn += " and (r.AddDate >= @AddEditDateStart or r.EditDate >= @AddEditDateStart)";
					listPar.Add(new SqlParameter("@AddEditDateStart", model.AddEditDateStart));
				}

				if (model.AddEditDateEnd.HasValue)
				{
					whereReceiving += " and (r.AddDate <= @AddEditDateEnd or r.EditDate <= @AddEditDateEnd)";
					whereTransferIn += " and (r.AddDate <= @AddEditDateEnd or r.EditDate <= @AddEditDateEnd)";
					listPar.Add(new SqlParameter("@AddEditDateEnd", model.AddEditDateEnd));
				}

				isQRCodeCreatedByPMS = "iif (dbo.IsQRCodeCreatedByPMS(rd.MINDQRCode) = 1, 'Y', '')";
			}

			string strSql = $@"
select * into #tmpResult
 from (
		select
			[ReceivingID] = r.ID
		    ,r.ExportID
			,[ArriveDate] = r.WhseArrival
		    ,rd.PoId
			,[Seq] = rd.Seq1 + ' ' + rd.Seq2
			,o.BrandID
			,o.StyleID
			,psd.refno
			,fb.WeaveTypeID
			,[Color] = iif(fb.MtlTypeID = 'EMB THREAD' OR fb.MtlTypeID = 'SP THREAD' OR fb.MtlTypeID = 'THREAD' 
						, IIF(psd.SuppColor = '', dbo.GetColorMultipleID (o.BrandID, isnull(psdsC.SpecValue, '')) , psd.SuppColor)
						, isnull(psdsC.SpecValue, ''))
			,rd.Roll
		    ,rd.Dyelot
			,rd.StockQty
			,StockType = isnull (ddl.Name, rd.StockType)
			,rd.Location
			,rd.Weight
			,rd.ActualWeight
			,[CutShadebandTime]=cutTime.CutTime
		    ,cutTime.CutBy
			,rd.Fabric2LabBy
			,rd.Fabric2LabTime
            ,rd.Checker
		from  Receiving r with (nolock)
		inner join Receiving_Detail rd with (nolock) on r.ID = rd.ID
		inner join Orders o with (nolock) on o.ID = rd.POID 
		inner join PO_Supp_Detail psd with (nolock) on rd.PoId = psd.ID and rd.Seq1 = psd.SEQ1 and rd.Seq2 = psd.SEQ2
        inner join PO_Supp ps with (nolock) on ps.ID = psd.id and ps.SEQ1 = psd.SEQ1
		inner join Fabric fb with (nolock) on psd.SCIRefno = fb.SCIRefno
        left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
        left join DropDownList ddl WITH (NOLOCK) on ddl.Type = 'Pms_StockType'
                                                    and REPLACE(ddl.ID,'''','') = rd.StockType
		OUTER APPLY(
		    SELECT  fs.CutTime,fs.CutBy
		    FROM FIR f with (nolock)
		    INNER JOIN FIR_Shadebone fs with (nolock) on f.id = fs.ID 	
		    WHERE  r.id = f.ReceivingID and rd.PoId = F.POID and rd.Seq1 = F.SEQ1 and rd.Seq2 = F.SEQ2 AND rd.Roll = fs.Roll and rd.Dyelot = fs.Dyelot
		) cutTime
        where   psd.FabricType ='F' {whereReceiving}
		union all
		SELECT 
			[ReceivingID] = r.ID
		    ,[ExportID] = ''
			,[ArriveDate] = r.IssueDate
		    ,rd.PoId
			,[Seq] = rd.Seq1 + ' ' + rd.Seq2
			,o.BrandID
			,o.StyleID
			,psd.refno
			,fb.WeaveTypeID
			,[Color] = iif(fb.MtlTypeID = 'EMB THREAD' OR fb.MtlTypeID = 'SP THREAD' OR fb.MtlTypeID = 'THREAD' 
						, IIF(psd.SuppColor = '', dbo.GetColorMultipleID (o.BrandID, isnull(psdsC.SpecValue, '')) , psd.SuppColor)
						, isnull(psdsC.SpecValue, ''))
			,rd.Roll
		    ,rd.Dyelot
			,[StockQty] = rd.Qty
			,StockType = isnull (ddl.Name, rd.StockType)
			,rd.Location
			,rd.Weight
			,rd.ActualWeight
			,[CutShadebandTime]=cutTime.CutTime
		    ,cutTime.CutBy
			,rd.Fabric2LabBy
			,rd.Fabric2LabTime
            ,rd.Checker
		FROM TransferIn r with (nolock)
		INNER JOIN TransferIn_Detail rd with (nolock) ON r.ID = rd.ID
		INNER JOIN Orders o with (nolock) ON o.ID = rd.POID
		INNER JOIN PO_Supp_Detail psd with (nolock) on rd.PoId = psd.ID and rd.Seq1 = psd.SEQ1 and rd.Seq2 = psd.SEQ2
        inner join PO_Supp ps with (nolock) on ps.ID = psd.id and ps.SEQ1 = psd.SEQ1
		INNER JOIN Fabric fb with (nolock) on psd.SCIRefno = fb.SCIRefno
        left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
        left join DropDownList ddl WITH (NOLOCK) on ddl.Type = 'Pms_StockType'
                                                    and REPLACE(ddl.ID,'''','') = rd.StockType
		OUTER APPLY(
		    SELECT  fs.CutTime,fs.CutBy
		    FROM FIR f with (nolock)
		    INNER JOIN FIR_Shadebone fs with (nolock) on f.id = fs.ID 	
		    WHERE  r.id = f.ReceivingID and rd.PoId = F.POID and rd.Seq1 = F.SEQ1 and rd.Seq2 = F.SEQ2 AND rd.Roll = fs.Roll and rd.Dyelot = fs.Dyelot
		)cutTime
        where   psd.FabricType ='F' {whereTransferIn}
	) a

select  ReceivingID
		,ExportID
		,ArriveDate
		,PoId
		,Seq
		,#tmpResult.BrandID
		,StyleID
		,refno
		,WeaveTypeID
		,Color
		,ColorName = c.Name
		,Roll
		,Dyelot
		,StockQty
		,StockType
		,Location
		,Weight
		,ActualWeight
from #tmpResult
left join Color c on c.ID = #tmpResult.Color and c.BrandID = #tmpResult.BrandID
where 1 = 1 {whereReceivingAct}

select  ReceivingID
		,ExportID
		,ArriveDate
		,PoId
		,Seq
		,#tmpResult.BrandID
		,StyleID
		,refno
		,WeaveTypeID
		,Color
		,ColorName = c.Name
		,Roll
		,Dyelot
		,StockQty
		,StockType
		,Location
		,Weight
		,CutShadebandTime
		,CutBy
from #tmpResult
left join Color c on c.ID = #tmpResult.Color and c.BrandID = #tmpResult.BrandID
where 1 = 1 {whereCutShadeband}

select  ReceivingID
		,ExportID
		,ArriveDate
		,PoId
		,Seq
		,#tmpResult.BrandID
		,StyleID
		,refno
		,WeaveTypeID
		,Color
		,ColorName = c.Name
		,Roll
		,Dyelot
		,StockQty
		,StockType
		,Location
		,Weight
		,Fabric2LabTime
		,Fabric2LabBy
from #tmpResult
left join Color c on c.ID = #tmpResult.Color and c.BrandID = #tmpResult.BrandID
where 1 = 1 {whereFabricLab}

select  ReceivingID
		,ExportID
		,ArriveDate
		,PoId
		,Seq
		,#tmpResult.BrandID
		,StyleID
		,refno
		,WeaveTypeID
		,Color
		,ColorName = c.Name
		,Roll
		,Dyelot
		,StockQty
		,StockType
		,Location
		,Weight
		,Checker
from #tmpResult
left join Color c on c.ID = #tmpResult.Color and c.BrandID = #tmpResult.BrandID
where 1 = 1 {whereChecker}

drop table #tmpResult
";

			if (model.UpdateInfo == "*" || model.UpdateInfo == "4" || model.IsPowerBI)
			{
				string sqlmind = $@"
select
	[ReceivingID] = r.ID
	,r.ExportID
	,Packages = isnull(e.Packages,0)
	,[ArriveDate] = r.WhseArrival
	,rd.PoId
    ,rd.seq1
    ,rd.seq2
	,[Seq] = rd.Seq1 + ' ' + rd.Seq2
	,o.BrandID
	,o.StyleID
	,psd.refno
	,fb.WeaveTypeID
	,[Color] = iif(fb.MtlTypeID = 'EMB THREAD' OR fb.MtlTypeID = 'SP THREAD' OR fb.MtlTypeID = 'THREAD' 
				, IIF(psd.SuppColor = '', dbo.GetColorMultipleID (o.BrandID, isnull(psdsC.SpecValue, '')) , psd.SuppColor)
				, isnull(psdsC.SpecValue, ''))
	,rd.Roll
	,rd.Dyelot
	,rd.StockQty
	,StockType = isnull (ddl.Name, rd.StockType)
	,Location= isnull(dbo.Getlocation(fi.Ukey),'')
	,rd.Weight
	,rd.ActualWeight
	,[CutShadebandTime]=cutTime.CutTime
	,CutBy = isnull(cutTime.CutBy,'')
	,rd.Fabric2LabBy
	,rd.Fabric2LabTime
    ,rd.Checker
    ,IsQRCodeCreatedByPMS = {isQRCodeCreatedByPMS}
    ,rd.MINDChecker
    ,rd.QRCode_PrintDate
    ,rd.MINDCheckAddDate
    ,rd.MINDCheckEditDate
    ,AbbEN = (select Supp.AbbEN from Supp with (nolock) where Supp.id =ps.SuppID)
    ,rdStockType = rd.StockType
    ,ForInspection = iif(rd.ForInspection = 1, 'Y', '')
    ,rd.ForInspectionTime
    ,OneYardForWashing = iif(rd.OneYardForWashing = 1, 'Y', '')
    ,Hold = iif(rd.Hold = 1, 'Y', '')
    ,rd.Remark
	{selPowerBI}
into #tmpMind
from  Receiving r with (nolock)
inner join Receiving_Detail rd with (nolock) on r.ID = rd.ID
inner join Orders o with (nolock) on o.ID = rd.POID 
inner join PO_Supp_Detail psd with (nolock) on rd.PoId = psd.ID and rd.Seq1 = psd.SEQ1 and rd.Seq2 = psd.SEQ2
inner join PO_Supp ps with (nolock) on ps.ID = psd.id and ps.SEQ1 = psd.SEQ1
inner join Fabric fb with (nolock) on psd.SCIRefno = fb.SCIRefno
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join DropDownList ddl WITH (NOLOCK) on ddl.Type = 'Pms_StockType'
                                            and REPLACE(ddl.ID,'''','') = rd.StockType
left join FtyInventory fi with (nolock) on  fi.POID = rd.POID and 
                                            fi.Seq1 = rd.Seq1 and 
                                            fi.Seq2 = rd.Seq2 and 
                                            fi.Roll = rd.Roll and
                                            fi.Dyelot = rd.Dyelot and
                                            fi.StockType = rd.StockType
OUTER APPLY(
	SELECT  fs.CutTime,fs.CutBy
	FROM FIR f with (nolock)
	INNER JOIN FIR_Shadebone fs with (nolock) on f.id = fs.ID 	
	WHERE  r.id = f.ReceivingID and rd.PoId = F.POID and rd.Seq1 = F.SEQ1 and rd.Seq2 = F.SEQ2 AND rd.Roll = fs.Roll and rd.Dyelot = fs.Dyelot
) cutTime
outer apply (
	select [Packages] = sum(e.Packages)
	from Export e with (nolock) 
	where EXISTS (
		select distinct e2.BLNO
		from Export e2 with (nolock) 
		where r.ExportId = e2.ID AND e2.BLNO =  e.Blno
	)
)e

where   psd.FabricType ='F' and r.Type = 'A'
{whereReceiving}

union 
select
	[ReceivingID] = r.ID
	,[ExportID] = ''
	,Packages = isnull(r.Packages,0)
	,[ArriveDate] = r.IssueDate
	,rd.PoId
    ,rd.seq1
    ,rd.seq2
	,[Seq] = rd.Seq1 + ' ' + rd.Seq2
	,o.BrandID
	,o.StyleID
	,psd.refno
	,fb.WeaveTypeID
	,[Color] = iif(fb.MtlTypeID = 'EMB THREAD' OR fb.MtlTypeID = 'SP THREAD' OR fb.MtlTypeID = 'THREAD' 
				, IIF(psd.SuppColor = '', dbo.GetColorMultipleID (o.BrandID, isnull(psdsC.SpecValue, '')) , psd.SuppColor)
				, isnull(psdsC.SpecValue, ''))
	,rd.Roll
	,rd.Dyelot
	,[StockQty] = rd.Qty
	,StockType = isnull (ddl.Name, rd.StockType)
	,Location= isnull(dbo.Getlocation(fi.Ukey),'')
	,rd.Weight
	,rd.ActualWeight
	,[CutShadebandTime]=cutTime.CutTime
	,CutBy = isnull(cutTime.CutBy,'')
	,rd.Fabric2LabBy
	,rd.Fabric2LabTime
    ,rd.Checker
    ,IsQRCodeCreatedByPMS = {isQRCodeCreatedByPMS}
    ,rd.MINDChecker
    ,rd.QRCode_PrintDate
    ,rd.MINDCheckAddDate
    ,rd.MINDCheckEditDate
    ,AbbEN = (select Supp.AbbEN from Supp with (nolock) where Supp.id =ps.SuppID)
    ,rdStockType = rd.StockType
    ,ForInspection = iif(rd.ForInspection = 1, 'Y', '')
    ,rd.ForInspectionTime
    ,OneYardForWashing = iif(rd.OneYardForWashing = 1, 'Y', '')
    ,Hold = iif(rd.Hold = 1, 'Y', '')
    ,rd.Remark
	{selPowerBI}
from  TransferIn r with (nolock)
inner join TransferIn_Detail rd with (nolock) on r.ID = rd.ID
inner join Orders o with (nolock) on o.ID = rd.POID 
inner join PO_Supp_Detail psd with (nolock) on rd.PoId = psd.ID and rd.Seq1 = psd.SEQ1 and rd.Seq2 = psd.SEQ2
inner join PO_Supp ps with (nolock) on ps.ID = psd.id and ps.SEQ1 = psd.SEQ1
inner join Fabric fb with (nolock) on psd.SCIRefno = fb.SCIRefno
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
left join DropDownList ddl WITH (NOLOCK) on ddl.Type = 'Pms_StockType'
                                            and REPLACE(ddl.ID,'''','') = rd.StockType
left join FtyInventory fi with (nolock) on  fi.POID = rd.POID and 
                                            fi.Seq1 = rd.Seq1 and 
                                            fi.Seq2 = rd.Seq2 and 
                                            fi.Roll = rd.Roll and
                                            fi.Dyelot = rd.Dyelot and
                                            fi.StockType = rd.StockType
OUTER APPLY(
	SELECT  fs.CutTime,fs.CutBy
	FROM FIR f with (nolock)
	INNER JOIN FIR_Shadebone fs with (nolock) on f.id = fs.ID 	
	WHERE  r.id = f.ReceivingID and rd.PoId = F.POID and rd.Seq1 = F.SEQ1 and rd.Seq2 = F.SEQ2 AND rd.Roll = fs.Roll and rd.Dyelot = fs.Dyelot
) cutTime
where   psd.FabricType ='F' 
{whereTransferIn}

select  ReceivingID = isnull(ReceivingID, '')
		,ExportID = isnull(ExportID, '')
		,Packages = isnull(Packages, '') 
		,ArriveDate
		,PoId = isnull(PoId, '')
		,Seq = isnull(Seq, '')
		,BrandID = isnull(rd.BrandID, '')
		,StyleID = isnull(StyleID, '')
		,refno = isnull(refno, '')
		,WeaveTypeID = isnull(WeaveTypeID, '')
		,Color = isnull(Color, '')
		,ColorName = isnull(c.Name, '')
		,Roll = isnull(Roll, '')
		,Dyelot = isnull(Dyelot, '')
		,StockQty = isnull(StockQty, 0)
		,StockType = isnull(StockType, '')
		,Location = isnull(Location, '')
		,Weight = isnull(Weight, 0)
        ,ActualWeight = isnull(ActualWeight, 0)
        ,IsQRCodeCreatedByPMS = isnull(IsQRCodeCreatedByPMS, '')
        ,LastP26RemarkData = isnull(LastP26RemarkData,'')
        ,MINDChecker = isnull(MINDChecker, '')
        ,QRCode_PrintDate
        ,MINDCheckAddDate
        ,MINDCheckEditDate
        ,AbbEN = isnull(AbbEN, '')
        ,ForInspection = isnull(ForInspection, '')
        ,ForInspectionTime
        ,OneYardForWashing = isnull(OneYardForWashing, '')
        ,Hold = isnull(Hold, '')
        ,Remark = isnull(Remark, '')
		{colPowerBI}
from #tmpMind rd
left join Color c with (nolock) on rd.Color = c.ID and c.BrandID = rd.BrandID
OUTER APPLY(
    select top 1 LastP26RemarkData =  isnull(lt.Remark,'')
	FROM LocationTrans lt with (nolock)
	INNER JOIN LocationTrans_detail ltd with (nolock) ON lt.ID=ltd.ID
    where lt.Status='Confirmed'
    and ltd.poid = rd.poid and ltd.seq1 = rd.seq1 and ltd.seq2 = rd.seq2  AND ltd.Roll = rd.Roll and ltd.Dyelot = rd.Dyelot and ltd.StockType = rd.rdStockType
    order by EditDate desc
)p26
where 1 = 1 {whereMind}
drop table #tmpMind
";

				resultReport.Result = this.DBProxy.Select("Production", sqlmind, listPar, out DataTable dataTable);
                if (!resultReport.Result)
                {
                    return resultReport;
                }

				resultReport.Dt = dataTable;
			}
			#endregion

			if (model.UpdateInfo != "4" && !model.IsPowerBI)
			{
				resultReport.Result = this.DBProxy.Select("Production", strSql, listPar, out DataTable[] dataTables);
				if (!resultReport.Result)
                {
					return resultReport;
				}

				resultReport.DtArr = dataTables;
			}

            return resultReport;
        }
    }
}
