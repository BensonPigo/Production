using Sci.Production.Prg.PowerBI.Logic;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Sci.Production.Prg.PowerBI.DataAccess
{
    /// <summary>
    /// Import Changeover Check List
    /// </summary>
    public class P_Import_ChangeoverCheckList
    {
        /// <summary>
        /// Changeover Check List
        /// </summary>
        /// <param name="sDate">Start Date</param>
        /// <param name="eDate">End Date</param>
        /// <param name="biTableInfoID">BI TableInfo ID</param>
        /// <returns>Base_ViewModel</returns>
        public Base_ViewModel P_ChangeoverCheckList(DateTime? sDate, DateTime? eDate, string biTableInfoID)
        {
            Base_ViewModel finalResult = new Base_ViewModel()
            {
                Result = new Ict.DualResult(false),
            };
            IE_R22 biModel = new IE_R22();
            try
            {
                if (!sDate.HasValue)
                {
                    sDate = DateTime.Parse(DateTime.Now.AddDays(-7).ToString("yyyy/MM/dd"));
                }

                if (!eDate.HasValue)
                {
                    eDate = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
                }

                IE_R22_ViewModel model = new IE_R22_ViewModel()
                {
                    DeadlineStart = sDate,
                    DeadlineEnd = eDate,
                    InlineStart = null,
                    InlineEnd = null,
                    OrderID = string.Empty,
                    StyleID = string.Empty,
                    ProductType = string.Empty,
                    Category = string.Empty,
                    SewingCell = string.Empty,
                    ResponseDep = string.Empty,
                    FactoryID = string.Empty,
                    IsOutstanding = false,
                    IsPowerBI = true,
                };

                #region Summary

                Base_ViewModel resultReport = biModel.GetIE_R22_Summary(model);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                // insert into PowerBI
                resultReport = this.UpdateBIData_Summary(eDate.Value, resultReport.Dt);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                #endregion

                #region Detail

                resultReport = biModel.GetIE_R22_Detail(model);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                resultReport = this.UpdateBIData_Detail(eDate.Value, resultReport.Dt);
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                #endregion

                resultReport = this.UpdateBIData_Delete();
                if (!resultReport.Result)
                {
                    throw resultReport.Result.GetException();
                }

                #region Update BI Table Info
                if (resultReport.Result)
                {
                    finalResult = this.UpdateBIData(biTableInfoID);
                }
                #endregion
            }
            catch (Exception ex)
            {
                finalResult.Result = new Ict.DualResult(false, ex);
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData_Delete()
        {
            Base_ViewModel finalResult;
            Data.DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            string sqlcmd = $@"
select *
into #tmpP_ChangeoverCheckList
from P_ChangeoverCheckList p
where not exists (
	select 1 from [MainServer].[Production].[dbo].[ChgOver] co 
	where p.FactoryID = co.FactoryID 
	and cast(p.InlineDate as date) = cast(co.Inline as date)
	and p.Line = co.SewingLineID 
	and p.NewSP = co.OrderID 
	and p.NewStyle = co.StyleID 
	and p.NewComboType = co.ComboType
)

delete pd
from P_ChangeoverCheckList_Detail pd
where exists (select 1 from #tmpP_ChangeoverCheckList p where pd.FactoryID = p.FactoryID and pd.InlineDate = p.InlineDate and pd.Line = p.Line and pd.SP = p.NewSP and pd.Style = p.NewStyle and pd.ComboType = p.NewComboType)

delete p
from P_ChangeoverCheckList p
inner join #tmpP_ChangeoverCheckList t on p.FactoryID = t.FactoryID and p.InlineDate = t.InlineDate and p.Line = t.Line 
									and p.OldSP = t.OldSP and p.OldStyle = t.OldStyle and p.OldComboType = t.OldComboType
									and p.NewSP = t.NewSP and p.NewStyle = t.NewStyle and p.NewComboType = t.NewComboType
            ";

            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>();
                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ExecuteByConnTransactionScope(sqlConn, sqlcmd, sqlParameters),
                };
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData_Summary(DateTime endDate, DataTable dt)
        {
            Base_ViewModel finalResult;
            Data.DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            string sqlcmd = $@"
update p
	set p.[StyleType] =  t.[StyleType]
	 , p.[Category] = t.[Category]
	 , p.[FirstSewingOutputDate] = t.[FirstSewingOutputDate]
from P_ChangeoverCheckList p
inner join #tmp t on p.[FactoryID] = t.[FactoryID] 
				 AND p.[InlineDate] = t.[InlineDate] 
				 AND p.[Line] = t.[Line]
				 AND p.[OldSP] = t.[OldSP]
				 AND p.[OldStyle] = t.[OldStyle]
				 AND p.[OldComboType] = t.[OldComboType]
				 AND p.[NewSP] = t.[NewSP]
				 AND p.[NewStyle] = t.[NewStyle]
				 AND p.[NewComboType] = t.[NewComboType]



insert into P_ChangeoverCheckList([FactoryID], [InlineDate], [Ready], [Line], [OldSP], [OldStyle], [OldComboType], [NewSP], [NewStyle], [NewComboType], [StyleType], [Category], [FirstSewingOutputDate])
select [FactoryID], [InlineDate], [Ready], [Line], [OldSP], [OldStyle], [OldComboType], [NewSP], [NewStyle], [NewComboType], [StyleType], [Category], [FirstSewingOutputDate]
from #tmp t
where not exists (select 1 from P_ChangeoverCheckList p where  p.[FactoryID] = t.[FactoryID] 
													 AND p.[InlineDate] = t.[InlineDate] 
													 AND p.[Line] = t.[Line]
													 AND p.[OldSP] = t.[OldSP]
													 AND p.[OldStyle] = t.[OldStyle]
													 AND p.[OldComboType] = t.[OldComboType]
													 AND p.[NewSP] = t.[NewSP]
													 AND p.[NewStyle] = t.[NewStyle]
													 AND p.[NewComboType] = t.[NewComboType])




delete p
from P_ChangeoverCheckList p
where not exists (select 1 from #tmp t where p.[FactoryID] = t.[FactoryID] 
													 AND p.[InlineDate] = t.[InlineDate] 
													 AND p.[Line] = t.[Line]
													 AND p.[OldSP] = t.[OldSP]
													 AND p.[OldStyle] = t.[OldStyle]
													 AND p.[OldComboType] = t.[OldComboType]
													 AND p.[NewSP] = t.[NewSP]
													 AND p.[NewStyle] = t.[NewStyle]
													 AND p.[NewComboType] = t.[NewComboType])
and p.[InlineDate] >= @EndDate
            ";

            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@EndDate", endDate),
                };
                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sqlcmd, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData_Detail(DateTime endDate, DataTable dt)
        {
            Base_ViewModel finalResult;
            Data.DBProxy.Current.OpenConnection("PowerBI", out SqlConnection sqlConn);

            string sqlcmd = $@"
update p
	set p.[DaysLeft] =  t.[DaysLeft]
	 , p.[OverDays] = t.[OverDays]
	 , p.[ChgOverCheck] = t.[ChgOverCheck]
	 , p.[CompletionDate] = t.[CompletionDate]
	 , p.[ResponseDep] = t.[ResponseDep]
	 , p.[CheckListItem] = t.[CheckListItem]
	 , p.[LateReason] = t.[LateReason]
from P_ChangeoverCheckList_Detail p
inner join #tmp t on  p.[FactoryID] = t.[FactoryID] 
				 AND p.[SP] = t.[SP] 
				 AND p.[Style] = t.[Style]
				 AND p.[ComboType] = t.[ComboType]
				 AND p.[Category] = t.[Category]
				 AND p.[ProductType] = t.[ProductType]
                 AND p.[Line] = t.[Line]
				 AND p.[InlineDate] = t.[InlineDate]
				 AND p.[CheckListNo] = t.[CheckListNo]



insert into P_ChangeoverCheckList_Detail([FactoryID], [SP], [Style], [ComboType], [Category], [ProductType], [Line], [DaysLeft], [InlineDate], [OverDays], [ChgOverCheck], [CompletionDate], [ResponseDep], [CheckListNo], [CheckListItem], [LateReason])
select [FactoryID], [SP], [Style], [ComboType], [Category], [ProductType], [Line], [DaysLeft], [InlineDate], [OverDays], [ChgOverCheck], [CompletionDate], [ResponseDep], [CheckListNo], [CheckListItem], [LateReason]
from #tmp t
where not exists (select 1 from P_ChangeoverCheckList_Detail p where  p.[FactoryID] = t.[FactoryID] 
														 AND p.[SP] = t.[SP] 
														 AND p.[Style] = t.[Style]
														 AND p.[ComboType] = t.[ComboType]
														 AND p.[Category] = t.[Category]
														 AND p.[ProductType] = t.[ProductType]
                                                         AND p.[Line] = t.[Line]
														 AND p.[InlineDate] = t.[InlineDate]
														 AND p.[CheckListNo] = t.[CheckListNo])




delete p
from P_ChangeoverCheckList_Detail p
where not exists (select 1 from #tmp t  where  p.[FactoryID] = t.[FactoryID] 
										 AND p.[SP] = t.[SP] 
										 AND p.[Style] = t.[Style]
										 AND p.[ComboType] = t.[ComboType]
										 AND p.[Category] = t.[Category]
										 AND p.[ProductType] = t.[ProductType]
										 AND p.[Line] = t.[Line]
										 AND p.[InlineDate] = t.[InlineDate]
										 AND p.[CheckListNo] = t.[CheckListNo])
and p.[InlineDate] >= @EndDate
            ";

            using (sqlConn)
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@EndDate", endDate),
                };
                finalResult = new Base_ViewModel()
                {
                    Result = TransactionClass.ProcessWithDatatableWithTransactionScope(dt, null, sqlcmd: sqlcmd, result: out DataTable dataTable, conn: sqlConn, paramters: sqlParameters),
                };
            }

            return finalResult;
        }

        private Base_ViewModel UpdateBIData(string biTableInfoID)
        {
            string sql = new Base().SqlBITableInfo(biTableInfoID, true);
            return new Base_ViewModel()
            {
                Result = TransactionClass.ExecuteTransactionScope("PowerBI", sql),
            };
        }
    }
}
