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
    public class QA_R20
    {
        /// <inheritdoc/>
        public QA_R20()
        {
            DBProxy.Current.DefaultTimeout = 1800;
        }

        /// <inheritdoc/>
        public Base_ViewModel GetQA_R20Data(QA_R20_ViewModel model)
        {
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@CDate1", SqlDbType.Date) { Value = (object)model.CDate1 ?? DBNull.Value },
                new SqlParameter("@CDate2", SqlDbType.Date) { Value = (object)model.CDate2 ?? DBNull.Value },

                new SqlParameter("@Factory", SqlDbType.VarChar, 8) { Value = model.Factory },
                new SqlParameter("@Brand", SqlDbType.VarChar, 8) { Value = model.Brand },
                new SqlParameter("@Line", SqlDbType.VarChar, 5) { Value = model.Line },
                new SqlParameter("@Cell", SqlDbType.VarChar, 2) { Value = model.Cell },
                new SqlParameter("@IsPowerBI", SqlDbType.Bit) { Value = model.IsPowerBI },
            };

            #region Where

            string where = string.Empty;

            if (model.IsPowerBI == false)
            {
                if (!MyUtility.Check.Empty(model.CDate1))
                {
                    where += $" and A.CDate >= @CDate1" + Environment.NewLine;
                }

                if (!MyUtility.Check.Empty(model.CDate2))
                {
                    where += $"and A.CDate <= @CDate2" + Environment.NewLine;
                }

                if (!MyUtility.Check.Empty(model.Factory))
                {
                    where += $" and A.FactoryID = @Factory" + Environment.NewLine;
                }

                if (!MyUtility.Check.Empty(model.Brand))
                {
                    where += $" and C.BrandID = @Brand" + Environment.NewLine;
                }

                if (!MyUtility.Check.Empty(model.Line))
                {
                    where += $" and A.SewinglineID = @Line" + Environment.NewLine;
                }

                if (!MyUtility.Check.Empty(model.Cell))
                {
                    where += $" and E.SewingCell = @Cell" + Environment.NewLine;
                }
            }
            else
            {
                where = $" and A.CDate >= @CDate1 and A.CDate <= @CDate2";
            }

            #endregion

            #region SQL

            var sqlCmd = $@"
select
	[Factory] = isnull(A.FACTORYID, ''),
	[CDate] = A.CDATE,
	[OrderID] = isnull(A.ORDERID, ''),
    [Destination] = isnull(ct.Alias, ''),
	[Brand] = isnull(C.BRANDID, ''),
	[Style] = isnull(C.STYLEID, ''),
    [BuyerDelivery] = c.BuyerDelivery ,
	[CDCode] = isnull(C.CDCODEID, ''),
    [CDCodeNew] = isnull(sty.CDCodeNew, ''),
	[ProductType] = isnull(sty.ProductType, ''),
	[FabricType] = isnull(sty.FabricType, ''),
	[Lining] = isnull(sty.Lining, ''),
	[Gender] = isnull(sty.Gender, ''),
	[Construction] = isnull(sty.Construction, ''),
	[Team] = isnull(A.TEAM, ''),
	[Shift] = isnull(A.SHIFT, ''),
	[Line] = isnull(A.SEWINGLINEID, ''),
	[Cell] = isnull(E.SewingCell, ''),
	[InspectQty] = isnull(A.INSPECTQTY, 0),
	[RejectQty] = isnull(A.REJECTQTY, 0),
	[RFT (%)] = isnull(iif(isnull(a.InspectQty,0)=0,0,round((a.InspectQty-a.RejectQty)/a.InspectQty * 100,2)), 0),
	[Over] = isnull(A.Status, ''),
	[QC] = isnull(C.CPUFactor * C.CPU * A.RejectQty, 0),
    [Remark] = isnull(A.Remark, '')
From DBO.Rft A WITH (NOLOCK) 
INNER JOIN DBO.ORDERS C ON C.ID = A.OrderID
INNEr JOIN Country ct WITH (NOLOCK)  ON ct.ID=c.Dest
Outer Apply (
	SELECT SewingCell 
	FROM SewingLine WITH (NOLOCK) 
	WHERE FactoryID = A.FactoryID AND ID = A.SewinglineID
) E
Outer apply (
	SELECT ProductType = r2.Name
		, FabricType = r1.Name
		, Lining
		, Gender
		, Construction = d1.Name
        , s.CDCodeNew
	FROM Style s WITH(NOLOCK)
	left join DropDownList d1 WITH(NOLOCK) on d1.type= 'StyleConstruction' and d1.ID = s.Construction
	left join Reason r1 WITH(NOLOCK) on r1.ReasonTypeID= 'Fabric_Kind' and r1.ID = s.FabricType
	left join Reason r2 WITH(NOLOCK) on r2.ReasonTypeID= 'Style_Apparel_Type' and r2.ID = s.ApparelType
	where s.Ukey = C.StyleUkey
)sty
WHERE 1=1
{where}
Order by [Factory], [CDate], [OrderID]
";

            #endregion

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("Production", sqlCmd, listPar, out DataTable dt),
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
