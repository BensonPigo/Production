using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <inheritdoc/>
    public class P_Import_SewingLineScheduleBySP
    {
        /// <inheritdoc/>
        public Base_ViewModel P_SewingLineScheduleBySP(DateTime? sDate, DateTime? eDate)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            PPIC_R01 biModel = new PPIC_R01();
            if (!sDate.HasValue)
            {
                sDate = DateTime.Now.AddDays(-60);
            }

            if (!eDate.HasValue)
            {
                eDate = DateTime.Now.AddDays(120);
            }

            PPIC_R01bySP_ViewModel model = new PPIC_R01bySP_ViewModel()
            {
                MDivisionID = string.Empty,
                FactoryID = string.Empty,
                SewingLineIDFrom = string.Empty,
                SewingLineIDTo = string.Empty,
                SewingDateFrom = sDate,
                SewingDateTo = eDate,
                BuyerDeliveryFrom = null,
                BuyerDeliveryTo = null,
                SciDeliveryFrom = null,
                SciDeliveryTo = null,
                BrandID = string.Empty,
                SubProcess = string.Empty,
                IsPowerBI = true,
            };

            try
            {
                Base_ViewModel resultReport = biModel.GetSewingLineScheduleDataBySP(model);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable dataTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataTable, sDate.Value, eDate.Value);
                if (!finalResult.Result)
                {
                    throw finalResult.Result.GetException();
                }
                else
                {
                    DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
                    TransactionClass.UpatteBIDataTransactionScope(sqlConn, "P_SewingLineScheduleBySP", true);
                }

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
            Data.DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SDate", sDate),
                    new SqlParameter("@EDate", eDate),
                };
                string sql = new Base().SqlBITableHistory("P_SewingLineScheduleBySP", "P_SewingLineScheduleBySP_History", "#tmp", "(convert(date, p.Inline) >= @SDate or (@SDate between convert(date,p.Inline) and convert(date,p.Offline))) and (convert(date, p.Offline) <= @EDate or (@EDate between convert(date,p.Inline) and convert(date,p.Offline)))", needJoin: false) + Environment.NewLine;
                sql += @"	
				delete p
				from P_SewingLineScheduleBySP p
				where	(convert(date, p.Inline) >= @SDate or (@SDate between convert(date,p.Inline) and convert(date,p.Offline))) and
						(convert(date, p.Offline) <= @EDate or (@EDate between convert(date,p.Inline) and convert(date,p.Offline))) and
						not exists(select 1 from #tmp t where p.ID = t.ID)

				update p 
				set 
				p.SewingLineID				= t.SewingLineID
				,p.MDivisionID				= t.MDivisionID
				,p.FactoryID				= t.FactoryID
				,p.SPNo						= t.SPNo
				,p.CustPONo					= t.CustPONo
				,p.Category					= t.Category
				,p.ComboType				= t.ComboType
				,p.SwitchToWorkorder		= t.SwitchToWorkorder
				,p.Colorway					= t.Colorway
				,p.SeasonID					= t.SeasonID
				,p.CDCodeNew				= t.CDCodeNew
				,p.ProductType				= t.ProductType
				,p.MatchFabric				= t.MatchFabric
				,p.FabricType				= t.FabricType
				,p.Lining					= t.Lining
				,p.Gender					= t.Gender
				,p.Construction				= t.Construction
				,p.StyleID					= t.StyleID
				,p.OrderQty					= t.OrderQty
				,p.AlloQty					= t.AlloQty
				,p.CutQty					= t.CutQty
				,p.SewingQty				= t.SewingQty
				,p.ClogQty					= t.ClogQty
				,p.FirstCuttingOutputDate	= t.FirstCuttingOutputDate
				,p.InspectionDate			= t.InspectionDate
				,p.TotalStandardOutput		= t.TotalStandardOutput
				,p.WorkHour					= t.WorkHour
				,p.StandardOutputPerHour	= t.StandardOutputPerHour
				,p.Efficiency				= t.Efficiency
				,p.KPILETA					= t.KPILETA
				,p.PFRemark					= t.PFRemark
				,p.SewETA					= t.SewETA
				,p.ActMTLETA				= t.ActMTLETA
				,p.MTLExport				= t.MTLExport
				,p.CutInLine				= t.CutInLine
				,p.Inline					= t.Inline
				,p.Offline					= t.Offline
				,p.SCIDelivery				= t.SCIDelivery
				,p.BuyerDelivery			= t.BuyerDelivery
				,p.CRDDate					= t.CRDDate
				,p.CPU						= t.CPU
				,p.SewingCPU				= t.SewingCPU
				,p.VASSHAS					= t.VASSHAS
				,p.ShipModeList				= t.ShipModeList
				,p.Destination				= t.Destination
				,p.Artwork					= t.Artwork
				,p.Remarks					= t.Remarks
				,p.TTL_PRINTING_PCS			= t.TTL_PRINTING_PCS
				,p.TTL_PRINTING_PPU_PPU		= t.TTL_PRINTING_PPU_PPU
				,p.SubCon					= t.SubCon			
				,p.BIFactoryID =  (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
				,p.BIInsertDate = GetDate()
				from P_SewingLineScheduleBySP p
				inner join #tmp t on p.ID = t.ID

				insert into P_SewingLineScheduleBySP
				(
					ID
					,SewingLineID
					,MDivisionID
					,FactoryID
					,SPNo
					,CustPONo
					,Category
					,ComboType
					,SwitchToWorkorder
					,Colorway
					,SeasonID
					,CDCodeNew
					,ProductType
					,MatchFabric
					,FabricType
					,Lining
					,Gender
					,Construction
					,StyleID
					,OrderQty
					,AlloQty
					,CutQty
					,SewingQty
					,ClogQty
					,FirstCuttingOutputDate
					,InspectionDate
					,TotalStandardOutput
					,WorkHour
					,StandardOutputPerHour
					,Efficiency
					,KPILETA
					,PFRemark
					,SewETA
					,ActMTLETA
					,MTLExport
					,CutInLine
					,Inline
					,Offline
					,SCIDelivery
					,BuyerDelivery
					,CRDDate
					,CPU
					,SewingCPU
					,VASSHAS
					,ShipModeList
					,Destination
					,Artwork
					,Remarks
					,TTL_PRINTING_PCS
					,TTL_PRINTING_PPU_PPU
					,SubCon
					,BIFactoryID
					,BIInsertDate
				)
				select	 
				t.ID
				,t.SewingLineID
				,t.MDivisionID
				,t.FactoryID
				,t.SPNo
				,t.CustPONo
				,t.Category
				,t.ComboType
				,t.SwitchToWorkorder
				,t.Colorway
				,t.SeasonID
				,t.CDCodeNew
				,t.ProductType
				,t.MatchFabric
				,t.FabricType
				,t.Lining
				,t.Gender
				,t.Construction
				,t.StyleID
				,t.OrderQty
				,t.AlloQty
				,t.CutQty
				,t.SewingQty
				,t.ClogQty
				,t.FirstCuttingOutputDate
				,t.InspectionDate
				,t.TotalStandardOutput
				,t.WorkHour
				,t.StandardOutputPerHour
				,t.Efficiency
				,t.KPILETA
				,t.PFRemark
				,t.SewETA
				,t.ActMTLETA
				,t.MTLExport
				,t.CutInLine
				,t.Inline
				,t.Offline
				,t.SCIDelivery
				,t.BuyerDelivery
				,t.CRDDate
				,t.CPU
				,t.SewingCPU
				,t.VASSHAS
				,t.ShipModeList
				,t.Destination
				,t.Artwork
				,t.Remarks
				,t.TTL_PRINTING_PCS
				,t.TTL_PRINTING_PPU_PPU
				,t.SubCon
				,(select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
				,GetDate()
				from #tmp t
				where not exists(	select 1 from P_SewingLineScheduleBySP p where	p.ID = t.ID)";
                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sql, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
                sqlConn.Close();
                sqlConn.Dispose();
            }

            return finalResult;
        }
    }
}
