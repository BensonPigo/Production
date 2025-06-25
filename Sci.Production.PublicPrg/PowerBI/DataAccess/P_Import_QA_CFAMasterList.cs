using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <summary>
    /// P_Import_QA_CFAMasterList
    /// </summary>
    public class P_Import_QA_CFAMasterList
    {
        /// <summary>
        /// P_QA_CFAMasterList
        /// </summary>
        /// <param name="item">Executed List</param>
        /// <returns>Base_ViewModel</returns>
        public Base_ViewModel P_QA_CFAMasterList(ExecutedList item)
        {
            QA_R31_ViewModel biPar = new QA_R31_ViewModel()
            {
                IsPowerBI = true,
                BIFilterDate = item.SDate,
                CategoryList = new List<string>() { "B", "S", "G" },
            };

            Base_ViewModel finalResult = new QA_R31().GetCFAMasterListReport(biPar);

            if (!finalResult.Result)
            {
                return finalResult;
            }

            DataTable dtUpdateSource = finalResult.DtArr[0];
            DataTable dtDeleteSource = finalResult.DtArr[1];

            if (dtUpdateSource.Rows.Count > 0)
            {
                finalResult.Result = this.UpdateData(dtUpdateSource, item);
            }

            if (!finalResult.Result)
            {
                return finalResult;
            }

            if (dtDeleteSource.Rows.Count > 0)
            {
                finalResult.Result = this.DeleteData(dtDeleteSource);
            }

            finalResult = new Base().UpdateBIData(item);

            return finalResult;
        }

        private DualResult UpdateData(DataTable dtUpdateSource, ExecutedList item)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@BIFactoryID", item.RgCode),
            };

            string sqlUpdate = $@"
alter table #tmp alter column ID varchar(13)
alter table #tmp alter column Seq varchar(2)
CREATE CLUSTERED INDEX IDX_ClusteredIndex ON #tmp(ID, Seq)

update  p set   p.FinalInsp            = t.CFAFinalInspectResult
                ,p.Thirdinsp           = t.CFAIs3rdInspect 
                ,p.ThirdinspResult     = t.CFA3rdInspectResult
                ,p.MDivision           = t.MDivisionID
                ,p.Factory             = t.FactoryID
                ,p.BuyerDelivery       = t.BuyerDelivery
                ,p.Brand               = t.BrandID
                ,p.Catery              = t.Category
                ,p.OrderType           = t.OrderTypeID
                ,p.CustPoNo            = t.CustPoNo
                ,p.Style               = t.StyleID
                ,p.StyleName           = t.StyleName
                ,p.Season              = t.SeasonID
                ,p.Dest                = t.Dest
                ,p.GTNPONO             = t.Customize1
                ,p.CustCD              = t.CustCDID
                ,p.ShipMode            = t.ShipModeID
                ,p.ColorWay            = t.ColorWay
                ,p.SewingLine          = t.SewLine
                ,p.Qty                 = t.Qty
                ,p.StaggeredOutput     = t.StaggeredOutput
                ,p.CMPOutput           = t.CMPoutput
                ,p.CMPOutputPCT        = t.CMPOutputPercent
                ,p.ClogRcvQty          = t.ClogReceivedQty
                ,p.CLOGRcVQtyPCT       = t.ClogReceivedQtyPercent
                ,p.TtlCtn              = t.TtlCtn
                ,p.StaggeredCtn        = t.StaggeredCtn
                ,p.ClogCtn             = t.ClogCtn
                ,p.ClogCtnPCT          = t.ClogCtnPercent
                ,p.LastCtnRcvDate      = t.LastCartonReceivedDate
                ,p.FinalInspDate       = t.CFAFinalInspectDate
                ,p.Last3rdInspDate     = t.CFA3rdInspectDate
                ,p.Remark              = t.CFARemark
                ,p.BIFactoryID         = @BIFactoryID
                ,p.BIInsertDate        = GETDATE()
from P_QA_CFAMasterList p
inner join  #tmp t on t.ID = p.OrderID and t.Seq = p.ShipSeq

insert into P_QA_CFAMasterList( OrderID
                                ,ShipSeq
                                ,FinalInsp      
                                ,Thirdinsp      
                                ,ThirdinspResult
                                ,MDivision      
                                ,Factory        
                                ,BuyerDelivery  
                                ,Brand          
                                ,Catery         
                                ,OrderType      
                                ,CustPoNo       
                                ,Style          
                                ,StyleName      
                                ,Season         
                                ,Dest           
                                ,GTNPONO        
                                ,CustCD         
                                ,ShipMode       
                                ,ColorWay       
                                ,SewingLine     
                                ,Qty            
                                ,StaggeredOutput
                                ,CMPOutput      
                                ,CMPOutputPCT   
                                ,ClogRcvQty     
                                ,CLOGRcVQtyPCT  
                                ,TtlCtn         
                                ,StaggeredCtn   
                                ,ClogCtn        
                                ,ClogCtnPCT     
                                ,LastCtnRcvDate 
                                ,FinalInspDate  
                                ,Last3rdInspDate
                                ,Remark
                                ,BIFactoryID
                                ,BIInsertDate)
select  t.ID
        ,t.Seq
        ,t.CFAFinalInspectResult
        ,t.CFAIs3rdInspect 
        ,t.CFA3rdInspectResult
        ,t.MDivisionID
        ,t.FactoryID
        ,t.BuyerDelivery
        ,t.BrandID
        ,t.Category
        ,t.OrderTypeID
        ,t.CustPoNo
        ,t.StyleID
        ,t.StyleName
        ,t.SeasonID
        ,t.Dest
        ,t.Customize1
        ,t.CustCDID
        ,t.ShipModeID
        ,t.ColorWay
        ,t.SewLine
        ,t.Qty
        ,t.StaggeredOutput
        ,t.CMPoutput
        ,t.CMPOutputPercent
        ,t.ClogReceivedQty
        ,t.ClogReceivedQtyPercent
        ,t.TtlCtn
        ,t.StaggeredCtn
        ,t.ClogCtn
        ,t.ClogCtnPercent
        ,t.LastCartonReceivedDate
        ,t.CFAFinalInspectDate
        ,t.CFA3rdInspectDate
        ,t.CFARemark
        ,@BIFactoryID
        ,GETDATE()
from    #tmp t
where   not exists(select 1 from P_QA_CFAMasterList p with (nolock) where t.ID = p.OrderID and t.Seq = p.ShipSeq)
";
            SqlConnection connBI;
            DBProxy.Current.OpenConnection("PowerBI", out connBI);
            DualResult result = new DualResult(true);

            using (connBI)
            {
                result = MyUtility.Tool.ProcessWithDatatable(dtUpdateSource, null, sqlUpdate, out DataTable dtEmpty, conn: connBI, paramters: sqlParameters);
            }

            return result;
        }

        private DualResult DeleteData(DataTable dtDeleteSource)
        {
            string sqlDelete = $@"
alter table #tmp alter column OrderID varchar(13)
alter table #tmp alter column Seq varchar(2)
CREATE CLUSTERED INDEX IDX_ClusteredIndex ON #tmp(OrderID, Seq)

INSERT INTO P_QA_CFAMasterList_History  
(  
    OrderID,ShipSeq ,   
    BIFactoryID,   
    BIInsertDate  
)   
SELECT   
p.OrderID,p.ShipSeq ,   
p.BIFactoryID,
GETDATE()  
FROM P_QA_CFAMasterList p             
left join #tmp t with (nolock) on t.OrderID = p.OrderID and t.Seq = p.ShipSeq
where	exists(select 1 from #tmp tt where tt.OrderID = p.OrderID)  AND
		t.Seq is null

delete  p
from P_QA_CFAMasterList p
left join #tmp t with (nolock) on t.OrderID = p.OrderID and t.Seq = p.ShipSeq
where	exists(select 1 from #tmp tt where tt.OrderID = p.OrderID)  AND
		t.Seq is null
";

            SqlConnection connBI;
            DBProxy.Current.OpenConnection("PowerBI", out connBI);
            DualResult result = new DualResult(true);

            using (connBI)
            {
                result = MyUtility.Tool.ProcessWithDatatable(dtDeleteSource, null, sqlDelete, out DataTable dtEmpty, conn: connBI);
            }

            return result;
        }
    }
}
