using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_ImportScheduleList
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_ImportScheduleList(DateTime? sDate, DateTime? eDate)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!sDate.HasValue)
            {
                sDate = DateTime.Now.AddDays(-90);
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Now;
            }

            try
            {
                Base_ViewModel resultReport = this.GetImportScheduleList_Data((DateTime)sDate, (DateTime)eDate);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable dataTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataTable, (DateTime)sDate, (DateTime)eDate);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }
                else
                {
                    DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
                    TransactionClass.UpatteBIDataTransactionScope(sqlConn, "P_ImportScheduleList", false);
                }

                finalResult.Result = new Ict.DualResult(true);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData(DataTable dt, DateTime sdate, DateTime edate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DualResult result;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            List<SqlParameter> lisSqlParameter = new List<SqlParameter>();
            lisSqlParameter.Add(new SqlParameter("@StartDate", sdate));
            lisSqlParameter.Add(new SqlParameter("@EndDate", edate));

            using (sqlConn)
            {
                string sql = new Base().SqlBITableHistory("P_ImportScheduleList", "P_ImportScheduleList_History", "#tmpP_ImportScheduleList", "((AddDate >= @StartDate AND AddDate <= @EndDate) OR (EditDate >= @StartDate AND EditDate <= @EndDate))", needJoin: false) + Environment.NewLine;
                sql += $@" 
                DELETE p
				FROM P_ImportScheduleList p
				WHERE ((AddDate >= @StartDate AND AddDate <= @EndDate)
					OR (EditDate >= @StartDate AND EditDate <= @EndDate))
				AND NOT EXISTS (SELECT 1 FROM #tmpP_ImportScheduleList WHERE ExportDetailUkey = p.ExportDetailUkey)

				UPDATE P
				SET 
					[WK]                   = t.[WK]
				,[ETA]                  = t.[ETA]
				,[MDivisionID]          = t.[MDivisionID]
				,[FactoryID]            = t.[FactoryID]
				,[Consignee]            = t.[Consignee]
				,[ShipModeID]           = t.[ShipModeID]
				,[CYCFS]                = t.[CYCFS]
				,[Blno]                 = t.[Blno]
				,[Packages]             = t.[Packages]
				,[Vessel]               = t.[Vessel]
				,[ProdFactory]          = t.[ProdFactory]
				,[OrderTypeID]          = t.[OrderTypeID]
				,[ProjectID]            = t.[ProjectID]
				,[Category]             = t.[Category]
				,[BrandID]              = t.[BrandID]
				,[SeasonID]             = t.[SeasonID]
				,[StyleID]              = t.[StyleID]
				,[StyleName]            = t.[StyleName]
				,[PoID]                 = t.[PoID]
				,[Seq]                  = t.[Seq]
				,[Refno]                = t.[Refno]
				,[Color]                = t.[Color]
				,[ColorName]            = t.[ColorName]
				,[Description]          = t.[Description]
				,[MtlType]              = t.[MtlType]
				,[SubMtlType]           = t.[SubMtlType]
				,[WeaveType]            = t.[WeaveType]
				,[SuppID]               = t.[SuppID]
				,[SuppName]             = t.[SuppName]
				,[UnitID]               = t.[UnitID]
				,[SizeSpec]             = t.[SizeSpec]
				,[ShipQty]              = t.[ShipQty]
				,[FOC]                  = t.[FOC]
				,[NetKg]                = t.[NetKg]
				,[WeightKg]             = t.[WeightKg]
				,[ArriveQty]            = t.[ArriveQty]
				,[ArriveQtyStockUnit]   = t.[ArriveQtyStockUnit]
				,[FirstBulkSewInLine]   = t.[FirstBulkSewInLine]
				,[FirstCutDate]         = t.[FirstCutDate]
				,[ReceiveQty]           = t.[ReceiveQty]
				,[TotalRollsCalculated] = t.[TotalRollsCalculated]
				,[StockUnit]            = t.[StockUnit]
				,[MCHandle]             = t.[MCHandle]
				,[ContainerType]        = t.[ContainerType]
				,[ContainerNo]          = t.[ContainerNo]
				,[PortArrival]          = t.[PortArrival]
				,[WhseArrival]          = t.[WhseArrival]
				,[KPILETA]              = t.[KPILETA]
				,[PFETA]                = t.[PFETA]
				,[EarliestSCIDelivery]  = t.[EarliestSCIDelivery]
				,[EarlyDays]            = t.[EarlyDays]
				,[EarliestPFETA]        = t.[EarliestPFETA]
				,[MRMail]               = t.[MRMail]
				,[SMRMail]              = t.[SMRMail]
				,[EditName]             = t.[EditName]
				,[AddDate]              = t.[AddDate]
				,[EditDate]             = t.[EditDate]
				,[FabricCombo]			= t.[FabricCombo]
				,[BIFactoryID]			= t.[BIFactoryID]
				,[BIInsertDate]			= t.[BIInsertDate]
				FROM P_ImportScheduleList P
				INNER JOIN #tmpP_ImportScheduleList t ON t.ExportDetailUkey = P.ExportDetailUkey

				INSERT P_ImportScheduleList([WK], [ExportDetailUkey], [ETA], [MDivisionID], [FactoryID], [Consignee], [ShipModeID], [CYCFS], [Blno], [Packages], [Vessel], [ProdFactory], [OrderTypeID], [ProjectID], [Category], [BrandID], [SeasonID], [StyleID], [StyleName], [PoID], [Seq], [Refno], [Color], [ColorName], [Description], [MtlType], [SubMtlType], [WeaveType], [SuppID], [SuppName], [UnitID], [SizeSpec], [ShipQty], [FOC], [NetKg], [WeightKg], [ArriveQty], [ArriveQtyStockUnit], [FirstBulkSewInLine], [FirstCutDate], [ReceiveQty], [TotalRollsCalculated], [StockUnit], [MCHandle], [ContainerType], [ContainerNo], [PortArrival], [WhseArrival], [KPILETA], [PFETA], [EarliestSCIDelivery], [EarlyDays], [EarliestPFETA], [MRMail], [SMRMail], [EditName], [AddDate], [EditDate], [FabricCombo],[BIFactoryID],[BIInsertDate])
				SELECT [WK], [ExportDetailUkey], [ETA], [MDivisionID], [FactoryID], [Consignee], [ShipModeID], [CYCFS], [Blno], [Packages], [Vessel], [ProdFactory], [OrderTypeID], [ProjectID], [Category], [BrandID], [SeasonID], [StyleID], [StyleName], [PoID], [Seq], [Refno], [Color], [ColorName], [Description], [MtlType], [SubMtlType], [WeaveType], [SuppID], [SuppName], [UnitID], [SizeSpec], [ShipQty], [FOC], [NetKg], [WeightKg], [ArriveQty], [ArriveQtyStockUnit], [FirstBulkSewInLine], [FirstCutDate], [ReceiveQty], [TotalRollsCalculated], [StockUnit], [MCHandle], [ContainerType], [ContainerNo], [PortArrival], [WhseArrival], [KPILETA], [PFETA], [EarliestSCIDelivery], [EarlyDays], [EarliestPFETA], [MRMail], [SMRMail], [EditName], [AddDate], [EditDate], [FabricCombo],[BIFactoryID],[BIInsertDate]					   
				FROM #tmpP_ImportScheduleList t
				WHERE NOT EXISTS(SELECT 1 FROM P_ImportScheduleList WHERE ExportDetailUkey = t.ExportDetailUkey)

                ";

                result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, paramters: lisSqlParameter, temptablename: "#tmpP_ImportScheduleList");
                sqlConn.Close();
                sqlConn.Dispose();
            }

            finalResult.Result = result;

            return finalResult;
        }

        private Base_ViewModel GetImportScheduleList_Data(DateTime sdate, DateTime edate)
        {
            string sqlcmd = $@"
            declare @Date_S date = '{sdate.ToString("yyyy/MM/dd")}'
            declare @Date_E date ='{edate.ToString("yyyy/MM/dd")}'

			select 
			* 
			, [BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
            , [BIInsertDate] = GetDate()
			from Production.dbo.Warehouse_Report_R25
			(1
			,@Date_S
			,@Date_E
			,NULL--@WK1
			,NULL--@WK2
			,NULL--@POID1
			,NULL--@POID2
			,NULL--@SuppID
			,NULL--@FabricType
			,NULL--@WhseArrival1
			,NULL--@WhseArrival2
			,NULL--@Eta1
			,NULL--@Eta2
			,NULL--@BrandID
			,NULL--@MDivisionID
			,NULL--@FactoryID
			,NULL--@KPILETA1
			,NULL--@KPILETA2
			,0--@RecLessArv
			)

			";

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sqlcmd, out DataTable dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTables;

            return resultReport;
        }
    }
}
