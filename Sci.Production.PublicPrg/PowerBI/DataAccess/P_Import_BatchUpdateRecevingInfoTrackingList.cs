﻿using System;
using System.Collections.Generic;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System.Data;
using System.Data.SqlClient;
using Sci.Data;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_BatchUpdateRecevingInfoTrackingList
    {
        /// <inheritdoc/>
        public Base_ViewModel P_BatchUpdateRecevingInfoTrackingList(DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            Warehouse_R40 biModel = new Warehouse_R40();
            DateTime? firstDate = DateTime.Parse("2021/12/01");

            try
            {
                Warehouse_R40_ViewModel warehouse_R40 = new Warehouse_R40_ViewModel()
                {
                    SP1 = string.Empty,
                    SP2 = string.Empty,
                    WKID1 = string.Empty,
                    WKID2 = string.Empty,
                    UpdateInfo = string.Empty,
                    Status = string.Empty,
                    IsPowerBI = true,
                };

                if (!sDate.HasValue)
                {
                    warehouse_R40.ArriveDateStart = firstDate;
                    warehouse_R40.AddEditDateEnd = null;
                    sDate = firstDate;
                    eDate = DateTime.Now.AddYears(100);
                }
                else
                {
                    warehouse_R40.AddEditDateStart = sDate;
                    if (eDate.HasValue)
                    {
                        warehouse_R40.AddEditDateEnd = eDate;
                    }
                }

                Base_ViewModel resultReport = biModel.GetWarehouse_R40Data(warehouse_R40);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable detailTable = resultReport.Dt;
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                // insert into PowerBI
                finalResult = this.UpdateBIData(detailTable, sDate.Value, eDate.Value);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }

                finalResult.Result = new Ict.DualResult(true);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, DateTime sDate, DateTime eDate)
        {
            Base_ViewModel finalResult;
            DBProxy.Current.DefaultTimeout = 10800;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SDate", sDate),
                    new SqlParameter("@EDate", eDate),
                };
                string sql = @"	
UPDATE t
SET 
	t.ReceivingID = s.ReceivingID
, t.ExportID = s.ExportID
, t.FtyGroup = s.FtyGroup
, t.Packages = s.Packages
, t.ArriveDate = s.ArriveDate
, t.Poid = s.Poid
, t.Seq = s.Seq
, t.BrandID = s.BrandID
, t.refno = s.refno
, t.WeaveTypeID = s.WeaveTypeID
, t.Color = s.Color
, t.Roll = s.Roll
, t.Dyelot = s.Dyelot
, t.StockQty = s.StockQty
, t.StockType = s.rdStockType
, t.Location = s.Location
, t.Weight = s.Weight
, t.ActualWeight = s.ActualWeight
, t.CutShadebandTime = s.CutShadebandTime
, t.CutBy = s.CutBy
, t.Fabric2LabTime = s.Fabric2LabTime
, t.Fabric2LabBy  = s.Fabric2LabBy
, t.Checker  = s.Checker
, t.IsQRCodeCreatedByPMS  = s.IsQRCodeCreatedByPMS
, t.LastP26RemarkData = s.LastP26RemarkData
, t.MINDChecker = s.MINDChecker
, t.QRCode_PrintDate  = s.QRCode_PrintDate
, t.MINDCheckAddDate = s.MINDCheckAddDate
, t.MINDCheckEditDate = s.MINDCheckEditDate
, t.SuppAbbEN = s.AbbEN
, t.ForInspection  = s.ForInspection
, t.ForInspectionTime  = s.ForInspectionTime
, t.OneYardForWashing = s.OneYardForWashing
, t.Hold = s.Hold
, t.Remark = s.Remark
, t.AddDate = s.AddDate
, t.EditDate = s.EditDate
from P_BatchUpdateRecevingInfoTrackingList t 
inner join #tmp s on t.ReceivingID = s.ReceivingID
AND t.Poid = s.Poid 
AND t.Seq = s.Seq 
AND t.StockType = s.rdStockType
AND t.Roll = s.Roll
AND t.Dyelot = s.Dyelot


insert into P_BatchUpdateRecevingInfoTrackingList (
    ReceivingID,ExportID,FtyGroup,Packages,ArriveDate,Poid,Seq,BrandID,refno,WeaveTypeID,Color,Roll,Dyelot,StockQty,StockType
,Location,Weight,ActualWeight,CutShadebandTime,CutBy,Fabric2LabTime,Fabric2LabBy,Checker,IsQRCodeCreatedByPMS,LastP26RemarkData
,MINDChecker,QRCode_PrintDate,MINDCheckAddDate,MINDCheckEditDate,SuppAbbEN,ForInspection,ForInspectionTime,OneYardForWashing
,Hold,Remark,AddDate,EditDate
)
select 	s.ReceivingID,s.ExportID,s.FtyGroup,s.Packages,s.ArriveDate,s.Poid,s.Seq,s.BrandID,s.refno,s.WeaveTypeID,s.Color,s.Roll
,s.Dyelot,s.StockQty,StockType = s.rdStockType,s.Location,s.Weight,s.ActualWeight,s.CutShadebandTime,s.CutBy,s.Fabric2LabTime,s.Fabric2LabBy
,s.Checker,s.IsQRCodeCreatedByPMS,s.LastP26RemarkData,s.MINDChecker,s.QRCode_PrintDate,s.MINDCheckAddDate,s.MINDCheckEditDate
,s.AbbEN,s.ForInspection,s.ForInspectionTime,s.OneYardForWashing,s.Hold,s.Remark,s.AddDate,s.EditDate
from #tmp s
where not exists (
    select 1 from P_BatchUpdateRecevingInfoTrackingList t 
    where t.ReceivingID = s.ReceivingID 
    AND t.Poid = s.Poid 
	AND t.Seq = s.Seq 
	AND t.StockType = s.rdStockType
	AND t.Roll = s.Roll
	AND t.Dyelot = s.Dyelot
)

delete t 
from dbo.P_BatchUpdateRecevingInfoTrackingList t
where not exists (
    select 1 from #tmp s 
    where t.ReceivingID = s.ReceivingID 
    AND t.Poid = s.Poid 
	AND t.Seq = s.Seq 
	AND t.StockType = s.rdStockType 
	AND t.Roll = s.Roll
	AND t.Dyelot = s.Dyelot
)
and ((t.AddDate >= @SDate or t.EditDate >= @SDate) and (t.AddDate <= @EDate or t.EditDate <= @EDate))

update b
	set b.TransferDate = getdate()
		, b.IS_Trans = 1
from BITableInfo b
where b.id = 'P_BatchUpdateRecevingInfoTrackingList'
";
                finalResult = new Base_ViewModel()
                {
                    Result = MyUtility.Tool.ProcessWithDatatable(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }
    }
}
