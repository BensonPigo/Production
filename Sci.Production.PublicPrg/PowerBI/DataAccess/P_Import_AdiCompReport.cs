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
    public class P_Import_AdiCompReport
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public Base_ViewModel P_AdiCompReport(ExecutedList item)
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };

            Base_ViewModel finalResult = new Base_ViewModel();
            try
            {
                Base_ViewModel resultReport = this.GetAdiCompReport_Data(item);
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

                finalResult = new Base().UpdateBIData(item);
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData(DataTable dt)
        {
            Base_ViewModel finalResult = new Base_ViewModel();
            DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);
            using (sqlConn)
            {
                string sql = string.Empty;
                sql += $@"  
                Truncate Table P_AdiCompReport";
                sql += $@"  
               insert into P_AdiCompReport  
               (  
                   [Year],  
                   [Month],  
                   [ID],  
                   [SalesID],  
                   [SalesName],  
                   [Article],  
                   [ArticleName],  
                   [ProductionDate],  
                   [DefectMainID],  
                   [DefectSubID],  
                   [FOB],  
                   [Qty],  
                   [ValueinUSD],  
                   [ValueINExRate],  
                   [OrderID],  
                   [RuleNo],  
                   [UKEY],  
                   [BrandID],  
                   [FactoryID],  
                   [SuppID],  
                   [Refno],  
                   [IsEM],  
                   [StyleID],  
                   [ProgramID],  
                   [Supplier],  
                   [SupplierName],  
                   [DefectMain],  
                   [DefectSub],  
                   [Responsibility],  
                   [MDivisionID] ,
                   [BIFactoryID],
                   [BIInsertDate]
               )  
               select  
                   [Year],  
                   [Month],  
                   [ID],  
                   [SalesID],  
                   [SalesName],  
                   [Article],  
                   [ArticleName],  
                   [ProductionDate],  
                   [DefectMainID],  
                   [DefectSubID],  
                   [FOB],  
                   [Qty],  
                   [ValueinUSD],  
                   [ValueINExRate],  
                   [OrderID],  
                   [RuleNo],  
                   [UKEY],  
                   [BrandID],  
                   [FactoryID],  
                   [SuppID],  
                   [Refno],  
                   [IsEM],  
                   [StyleID],  
                   [ProgramID],  
                   [Supplier],  
                   [SupplierName],  
                   [DefectMain],  
                   [DefectSub],  
                   [Responsibility],  
                   [MDivisionID],
                   [BIFactoryID],
                   [BIInsertDate]
               from #tmp t";
                finalResult.Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sql, out DataTable dataTable, conn: sqlConn, temptablename: "#tmp");
            }

            return finalResult;
        }

        private Base_ViewModel GetAdiCompReport_Data(ExecutedList item)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>()
            {
                new SqlParameter("@BIFactoryID", item.RgCode),
            };

            string sqlcmd = $@"
			Select 
			[Year] = Year(a.StartDate)
			, Month = Right('00' + Cast(Month(A.StartDate) as varchar),2)
			, ad.ID
			, ad.SalesID
			, ad.SalesName
			, ad.Article
			, ad.ArticleName
			, ad.ProductionDate
			, ad.DefectMainID
			, ad.DefectSubID
			, FOB = Isnull(ad.FOB, 0)
			, Qty = Isnull(ad.Qty, 0)
			, ValueinUSD = Isnull(ad.ValueinUSD, 0)
			, ValueINExRate = Isnull(ad.ValueINExRate, 0)
			, ad.OrderID
			, ad.RuleNo
			, ad.UKEY
			, ad.BrandID
			, ad.FactoryID
			, ad.SuppID
			, ad.Refno
			, ad.IsEM 
			, StyleID = s.Id
			, s.ProgramID
			, Supplier = iif(ad.SuppID = '', Isnull(po2.SuppID, ''), ad.SuppID)
			, SupplierName = iif(ad.SuppID = '', Isnull(supp.AbbCH, ''), adSupp.AbbCH)
			, DefectMain = concat(d.ID, '-', d.Name)
			, DefectSub = concat(dd.ID, '-', dd.SubName)
			, ad.Responsibility
			, f.MDivisionID
            , [BIFactoryID] = @BIFactoryID
            , [BIInsertDate] = GetDate()
			From Production.dbo.ADIDASComplain a With(Nolock)
			Left join Production.dbo.ADIDASComplain_Detail ad With(Nolock) on a.ID = ad.ID
			Left join Production.dbo.ADIDASComplainDefect d With(Nolock) on d.ID = ad.DefectMainID
			Left join Production.dbo.ADIDASComplainDefect_Detail dd With(Nolock) on dd.ID = d.ID and dd.SubID = ad.DefectSubID
			Left join Production.dbo.Orders o With(Nolock) on ad.OrderID = o.ID
			Left join Production.dbo.Style s With(Nolock) on o.StyleUkey = s.Ukey
			Left join Production.dbo.PO With(Nolock) on o.POID = PO.ID
			Left join Production.dbo.PO_Supp po2 With(Nolock) on po2.ID = po.ID and po2.SEQ1 = '01'
			Left join Production.dbo.Supp With(Nolock) on Supp.ID = po2.SuppID
			Left join Production.dbo.Supp adSupp With(Nolock) on adSupp.ID = ad.SuppID
			Left join Production.dbo.SCIFty f With(Nolock) on f.id = ad.FactoryID
			Order by a.ID
			";

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sqlcmd, sqlParameters, out DataTable dt),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dt;

            return resultReport;
        }
    }
}
