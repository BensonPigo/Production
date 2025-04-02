using Ict;
using Sci.Data;
using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <summary>
    /// 此BI報表與 PMS/QA/R32 已脫鉤 待討論
    /// </summary>
    public class P_Import_CFAInspectionRecord_Detail
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_CFAInspectionRecord_Detail(DateTime? sDate)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();

            if (!sDate.HasValue)
            {
                sDate = DateTime.Parse(DateTime.Now.Year.ToString());
            }

            try
            {
                Base_ViewModel resultReport = this.GetDQSDefect_Summary_Data((DateTime)sDate);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                DataTable dataTable = resultReport.Dt;

                // insert into PowerBI
                finalResult = this.UpdateBIData(dataTable);
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

        private Base_ViewModel GetDQSDefect_Summary_Data(DateTime sdate)
        {
            string sqlcmd = $@" 
			declare @sDate varchar(20) = '{sdate.ToString("yyyy/MM/dd")}'

			SELECT 
			c.[ID]
			,c.[AuditDate]
			,c.[FactoryID]
			,c.[MDivisionid]
			,c.[SewingLineID]
			,c.[Team]
			,c.[Shift]
			,c.[Stage]
			,co.[Carton]
			,c.[InspectQty]
			,c.[DefectQty]
			,c.[ClogReceivedPercentage]
			,c.[Result]
			,c.[CFA]
			,c.[Status]
			,c.[Remark]
			,c.[AddName]
			,c.[AddDate]
			,c.[EditName]
			,c.[EditDate]
			,c.[IsCombinePO]
			,c.[FirstInspection]
			,co.OrderID,co.SEQ
			INTO #MainData1
			From Production.dbo.CFAInspectionRecord c WITH(NOLOCK)
			INNER JOIN Production.dbo.CFAInspectionRecord_OrderSEQ co WITH(NOLOCK) ON c.ID = co.ID
			INNER JOIN Production.dbo.Orders O WITH(NOLOCK) ON o.ID = co.OrderID
			WHERE 1=1
			AND c.AuditDate >= @sDate

			SELECT 
			 c.[ID]
			,c.[AuditDate]
			,c.[FactoryID]
			,c.[MDivisionid]
			,c.[SewingLineID]
			,c.[Team]
			,c.[Shift]
			,c.[Stage]
			,co.[Carton]
			,c.[InspectQty]
			,c.[DefectQty]
			,c.[ClogReceivedPercentage]
			,c.[Result]
			,c.[CFA]
			,c.[Status]
			,c.[Remark]
			,c.[AddName]
			,c.[AddDate]
			,c.[EditName]
			,c.[EditDate]
			,c.[IsCombinePO]
			,c.[FirstInspection]
			,co.OrderID
			,co.SEQ
			,[InspectedSP] = cfos.OrderID
			,[InspectedSeq] = cfos.Seq
			,[ReInspection] =  iif(c.ReInspection =1, 1, 0)
			INTO #MainData
			From Production.dbo.CFAInspectionRecord  c
			INNER JOIN Production.dbo.CFAInspectionRecord_OrderSEQ co ON c.ID = co.ID
			outer apply (
				select top 1 OrderID, Seq
				from  Production.dbo.CFAInspectionRecord_OrderSEQ cfos 
				where c.ID = cfos.ID
				ORDER BY Ukey
			) cfos
			WHERE c.ID IN (SELECT ID FROM #MainData1)

			SELECT 
			 c.AuditDate
			,BuyerDelivery = (SELECT BuyerDelivery FROM Production.dbo.Orders WHERE ID = c.OrderID)
			,c.OrderID
			,CustPoNo = (SELECT CustPoNo FROM Production.dbo.Orders WHERE ID = c.OrderID)
			,StyleID = (SELECT StyleID FROM Production.dbo.Orders WHERE ID = c.OrderID)
			,BrandID = (SELECT BrandID FROM Production.dbo.Orders WHERE ID = c.OrderID)
			,Dest = (SELECT Dest FROM Production.dbo.Orders WHERE ID = c.OrderID)
			,Seq = (SELECT Seq FROM Production.dbo.Order_QtyShip WHERE ID = c.OrderID AND Seq = c.SEQ)
			,c.SewingLineID
			,[VasShas]= (SELECT IIF(VasShas=1,'Y','N')  FROM Production.dbo.Orders WHERE ID = c.OrderID)
			,c.ClogReceivedPercentage
			,c.MDivisionid
			,c.FactoryID
			,c.Shift
			,c.Team
			,Qty = (SELECT Qty FROM Production.dbo.Order_QtyShip WHERE ID = c.OrderID AND Seq = c.SEQ)
			,c.Status
			,[Carton] = IIF(c.Carton ='' AND c.Stage = '3rd party','N/A',c.Carton)
			,[CfA] = isnull((select CONCAT(c.CFA, ':', Name) from Production.dbo.Pass1  WITH (NOLOCK) where ID = c.CFA),'')
			,c.stage
			,c.Result
			,c.InspectQty
			,c.DefectQty
			,[SQR] = IIF( c.InspectQty = 0,0 , (c.DefectQty * 1.0 / c.InspectQty) * 100)
			,[DefectDescription] = g.Description
			,[AreaCodeDesc] = cd.CFAAreaID + ' - ' + CfaArea.Description
			,[NoOfDefect] = cd.Qty
			,cd.Remark
			,c.ID
			,c.IsCombinePO
			,[InsCtn]=IIF(c.stage in ('Final' ,'Final Internal') OR c.Stage ='3rd party',
			( 
				SELECT [Val]= COUNT(DISTINCT cr.ID) + 1
				FROM Production.dbo.CFAInspectionRecord cr
				INNER JOIN Production.dbo.CFAInspectionRecord_OrderSEQ crd ON cr.ID = crd.ID
				WHERE crd.OrderID=c.OrderID AND crd.SEQ=c.SEQ
				AND cr.Status = 'Confirmed'
				AND cr.Stage=c.Stage
				AND cr.AuditDate <= c.AuditDate
				AND cr.ID  != c.ID
			)
			,NULL)
			,[Action]= cd.Action
			,[CFAInspectionRecord_Detail_Key]= concat(c.ID,iif(isnull(cd.GarmentDefectCodeID, '') = '', concat(row_Number()over(order by c.ID),''), cd.GarmentDefectCodeID))
			,c.FirstInspection
			,c.[InspectedSP]
			,c.[InspectedSeq] 
			,c.[ReInspection]
			INTO #tmp
			FROm #MainData  c
			LEFT JOIN Production.dbo.CFAInspectionRecord_Detail cd ON c.ID = cd.ID
			LEFT JOIN Production.dbo.GarmentDefectCode g ON g.ID = cd.GarmentDefectCodeID
			LEFT JOIN Production.dbo.CfaArea ON CfaArea.ID = cd.CFAAreaID

			SELECT pd.*
			INTO #PackingList_Detail
			FROM Production.dbo.PackingList_Detail pd
			WHERE EXISTS (SELECT 1 FROM #tmp t WHERE pd.OrderID = t.OrderID AND pd.OrderShipmodeSeq = t.SEQ) 

			SELECT  
			Action
			,AreaCodeDesc
			,AuditDate
			,BrandID
			,BuyerDelivery
			,CFA
			,ClogReceivedPercentage
			,DefectDescription
			,DefectQty
			,Dest
			,FactoryID
			,Carton
			,[Inspected Ctn] = InspectedCtn.Val
			,[Inspected PoQty]=InspectedPoQty.Val
			,Stage
			,SewingLineID
			,MDivisionid
			,NoOfDefect
			,Qty
			,CustPoNo
			,Remark
			,Result
			,InspectQty
			,Seq
			,Shift
			,OrderID
			,SQR
			,Status
			,StyleID
			,Team
			,[TTL CTN] = TtlCtn.Val
			,VasShas
			,FirstInspection  = IIF(FirstInspection = 1, 'Y','')
			,t.[InspectedSP]
			,t.[InspectedSeq] 
			,t.[ReInspection]
			FROM  #tmp t
			OUTER APPLY(
				SELECT [Val] = COUNT(1)
				FROM #PackingList_Detail pd
				WHERE pd.OrderID = t.OrderID AND pd.OrderShipmodeSeq = t.Seq 
				AND pd.CTNQty > 0 AND pd.CTNStartNo != ''
			)TtlCtn
			OUTER APPLY(
				SELECT [Val] = COUNT(DISTINCT pd.CTNStartNo)
				FROM #PackingList_Detail pd
				WHERE pd.OrderID = t.OrderID  AND pd.OrderShipmodeSeq = t.Seq
				AND (',' + t.Carton + ',') like ('%,' + pd.CTNStartNo + ',%')
				AND pd.CTNQty=1
			)InspectedCtn
			OUTER APPLY(
				SELECT [Val] = SUM(pd.ShipQty)
				FROM #PackingList_Detail pd
				WHERE pd.OrderID = t.OrderID  AND pd.OrderShipmodeSeq = t.Seq
				AND (',' + t.Carton + ',') like ('%,' + pd.CTNStartNo + ',%')
			)InspectedPoQty
			Order by id

			DROP TABLE #tmp ,#PackingList_Detail ,#MainData ,#MainData1";
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

        private Base_ViewModel UpdateBIData(DataTable dt)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DualResult result;
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            using (sqlConn)
            {
                string sql = $@" 
				DELETE T FROM P_CFAInspectionRecord_Detail T WHERE EXISTS(SELECT * FROM Production.dbo.factory S WHERE T.FactoryID = S.ID)

				INSERT INTO [dbo].[P_CFAInspectionRecord_Detail]
				(
					 [Action]
					,[AreaCodeDesc]
					,[AuditDate]
					,[BrandID]
					,[BuyerDelivery]
					,[CfaName]
					,[ClogReceivedPercentage]
					,[DefectDesc]
					,[DefectQty]
					,[Destination]
					,[FactoryID]
					,[Carton]
					,[InspectedCtn]
					,[InspectedPoQty]
					,[InspectionStage]
					,[SewingLineID]
					,[Mdivisionid]
					,[NoOfDefect]
					,[OrderQty]
					,[PONO]
					,[Remark]
					,[Result]
					,[SampleLot]
					,[Seq]
					,[Shift]
					,[SPNO]
					,[SQR]
					,[Status]
					,[StyleID]
					,[Team]
					,[TtlCTN]
					,[VasShas]
					,[1st_Inspection]
					,[InspectedSP]
					,[InspectedSeq] 
					,[ReInspection]
				)
				select 
					isnull(Action ,'')
					,isnull(AreaCodeDesc , '')
					,AuditDate
					,isnull(BrandID, '')
					,BuyerDelivery
					,isnull(CFA, '')
					,isnull(ClogReceivedPercentage , 0)
					,isnull(DefectDescription, '')
					,isnull(DefectQty, 0)
					,isnull(Dest ,'')
					,isnull(FactoryID, '')
					,isnull(Carton, '')
					,isnull([Inspected Ctn], 0)
					,isnull([Inspected PoQty], 0)
					,isnull(Stage ,'')
					,isnull(SewingLineID, '')
					,isnull(MDivisionid, '')
					,isnull(NoOfDefect, 0)
					,isnull(Qty, 0)
					,isnull(CustPoNo, '')
					,isnull(Remark, '')
					,isnull(Result, '')
					,isnull(InspectQty, 0)
					,isnull(Seq ,'')
					,isnull(Shift, '')
					,isnull(OrderID, '')
					,isnull(SQR, 0)
					,isnull(Status,'')
					,isnull(StyleID, '')
					,isnull(Team, '')
					,isnull([TTL CTN] , 0)
					,isnull(VasShas ,'')
					,isnull(FirstInspection ,'')
					,isnull([InspectedSP], '')
					,isnull([InspectedSeq],'') 
					,[ReInspection]
					from #Final_P_CFAInspectionRecord_Detail

					update b set b.TransferDate = getdate(), b.IS_Trans = 1
					from BITableInfo b
					where b.id = 'P_CFAInspectionRecord_Detail'";

                result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, temptablename: "#Final_P_CFAInspectionRecord_Detail");
            }

            finalResult.Result = result;

            return finalResult;
        }
    }
}
