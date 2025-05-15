using Sci.Data;
using Sci.Production.Prg.PowerBI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.Prg.PowerBI.Logic
{
    /// <summary>
    /// 有調整到需要一併更新至BI
    /// </summary>
    public class IE_R22
    {
        /// <summary>
        /// set the default timeout to 1800 seconds
        /// </summary>
        public IE_R22()
        {
            DBProxy.Current.DefaultTimeout = 1800;
        }

        /// <summary>
        /// Get the summary of the R22 report
        /// </summary>
        /// <param name="model">IE_R22_ViewModel</param>
        /// <returns>Base_ViewModel</returns>
        public Base_ViewModel GetIE_R22_Summary(IE_R22_ViewModel model)
        {
            List<SqlParameter> listPar = this.GetSqlParameters(model);
            string sqlWhere = this.GetWhere(model);
            /*
--     [Factory] 
--     [InlineDate] 
--     [SewingLine] 
--     [OldSP] 
--     [OldStyle] 
--     [OldComboType] 
--     [NewSP] 
--     [NewStyle] 
--     [NewComboType]
            */
            string sql = $@"
--Summary
SELECT  distinct
     [FactoryID] = co.FactoryID,
     [InlineDate] = co.Inline,
     [Ready] = iif ((SELECT COUNT(1) FROM ChgOver_Check WITH(NOLOCK) WHERE [Checked] = 0 AND ID = CO.ID) > 0 ,'','V'),
     [Line] = co.SewingLineID,
     [OldSP] = ISNULL(oldco.OrderID, ''),
     [OldStyle] = ISNULL(oldco.StyleID, ''),
     [OldComboType] = ISNULL(oldco.ComboType, ''),
     [NewSP] = co.OrderID,
     [NewStyle] = co.StyleID,
     [NewComboType] = co.ComboType,
     [StyleType] = iif(co.Type = 'N', 'New', 'Repeat'),
     [Category] = co.Category,
     [FirstSewingOutputDate] = GetOutputDate.OutputDate,
     [BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System]), 
     [BIInsertDate] = GETDATE()
FROM ChgOver co WITH (NOLOCK)
INNER JOIN ChgOver_Check CC WITH (NOLOCK) ON cc.ID = co.ID And cc.No <> 0
LEFT JOIN Style s WITH (NOLOCK) ON s.ID = co.StyleID and co.SeasonID = s.SeasonID
LEFT JOIN Reason r WITH (NOLOCK) ON r.ID = s.ApparelType AND r.ReasonTypeID = 'Style_Apparel_Type' 
LEFT JOIN SewingLine sl WITH (NOLOCK) ON sl.ID = co.SewingLineID AND sl.FactoryID = co.FactoryID
LEFT JOIN ChgOverCheckList ccl WITH(NOLOCK) ON ccl.Category = co.Category AND ccl.StyleType = co.Type and ccl.FactoryID = co.FactoryID
OUTER APPLY
(
    SELECT LTRIM(RTRIM(m.n.value('.[1]', 'varchar(500)'))) AS ResponseDep
    FROM (
            SELECT CAST('<XMLRoot><RowData>' + REPLACE(ResponseDep, ',', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
            FROM ChgOverCheckList_Detail ccld WITH(NOLOCK)
	        WHERE ccl.ID = ccld.ID  and ccld.ChgOverCheckListBaseID = CC.[No]
        ) t
    CROSS APPLY x.nodes('/XMLRoot/RowData') m(n)
) AS ccldx
OUTER APPLY 
(
    SELECT TOP 1 b.OrderID, b.StyleID, b.ComboType
    FROM ChgOver b
    WHERE b.Inline = (
                        SELECT MAX(c.Inline) 
                        FROM ChgOver c WITH(NOLOCK)
                        WHERE c.FactoryID = co.FactoryID 
                          AND c.SewingLineID = co.SewingLineID 
                          AND c.Inline < co.Inline
                       )
    AND b.FactoryID = co.FactoryID
    AND b.SewingLineID = co.SewingLineID
) AS oldco
OUTER APPLY
(
	SELECT val = isnull(DATEDIFF(day,CC.DeadLine,GETDATE()) -(COUNT(1) + dbo.getDateRangeSundayCount(CC.DeadLine,GETDATE())),0)
	FROM Holiday WITH(NOLOCK)
	WHERE HolidayDate BETWEEN CC.Deadline AND GETDATE() AND FactoryID = co.FactoryID
)OverDay_Check_0
OUTER APPLY
(
	SELECT val = isnull(iif((CC.CompletionDate IS NULL) OR (CC.Deadline IS NULL), 0, DATEDIFF(day,CC.DeadLine,CC.CompletionDate) -(COUNT(1) + dbo.getDateRangeSundayCount(CC.DeadLine,CC.CompletionDate))),0)
	FROM Holiday WITH(NOLOCK)
	WHERE HolidayDate BETWEEN CC.Deadline AND CC.CompletionDate AND FactoryID = CO.FactoryID
)OverDay_Check_1
OUTER APPLY
(
    select top(1) s.OutputDate
    from SewingOutput s WITH (NOLOCK) 
    inner join SewingOutput_Detail sd WITH (NOLOCK) on s.ID = sd.ID
    where 
    sd.OrderId = co.OrderId
    and sd.ComboType = co.ComboType
    and s.SewingLineID = co.SewingLineID
    and s.FactoryID = co.FactoryID
)GetOutputDate
WHERE 1 = 1
{sqlWhere}
ORDER BY [InlineDate], [Line], [OldSP], [NewSP] 
";
            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("Production", sql, listPar, out DataTable dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTables;
            return resultReport;
        }

        /// <summary>
        /// Get the detail of the R22 report
        /// </summary>
        /// <param name="model">IE_R22_ViewModel</param>
        /// <returns>Base_ViewModel</returns>
        public Base_ViewModel GetIE_R22_Detail(IE_R22_ViewModel model)
        {
            List<SqlParameter> listPar = this.GetSqlParameters(model);
            string sqlWhere = this.GetWhere(model);
            /*
--   [Factory]
--   [SP]
--   [Style]
--   [Category]
--   [ProductType]
--   [Cell]
--   [InlineDate]
--   [CheckListN
             */
            string sql = $@"
--Detail
SELECT Distinct
    [FactoryID] = co.FactoryID,
    [SP] = co.OrderID,
    [Style] = co.StyleID,
    [ComboType] = co.ComboType,
    [Category] = co.Category,
    [Style Type] =  Case when co.Type = 'R' then 'Repeat' 
                         when co.Type = 'N' then 'New'
				    end,
    [ProductType] = r.Name,
    [Cell] = sl.SewingCell,
    [Line] = co.SewingLineID,
    [DaysLeft] = iif(cc.Checked = 1 ,'-' ,  CONVERT( VARCHAR(10),iif(DaysLefCnt.val < 0 , 0 ,DaysLefCnt.val ))),
    [InlineDate] = co.Inline,
    [OverDays] = iif(cc.[Checked] = 0 , iif(OverDay_Check_0.VAL < 0,0,OverDay_Check_0.VAL) ,iif(OverDay_Check_1.VAL < 0,0,OverDay_Check_1.VAL)),
    [ChgOverCheck] = IIF(cc.Checked = 0, '', 'V'),
    [CompletionDate] = CONVERT(varchar, cc.CompletionDate, 23),
    [ResponseDep] = CC.ResponseDep,
    [CheckListNo] = cc.No,
    [CheckListItem] = colb.CheckList,
    [LateReason] = cc.Remark,
    [Deadline] = CC.Deadline,
    [CompletedinTime] = iif(isnull(cc.CompletedInTime,'') != '', cc.CompletedInTime, iif(isnull(CC.CompletionDate,'') != '', iif(CC.CompletionDate > CC.Deadline, 'Fail', 'Pass'), '')),
    [BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System]), 
    [BIInsertDate] = GETDATE()
FROM ChgOver_Check CC WITH (NOLOCK)
INNER JOIN ChgOver co WITH (NOLOCK) ON CC.ID = co.ID
LEFT JOIN Style s WITH (NOLOCK) ON s.ID = co.StyleID and co.SeasonID = s.SeasonID
LEFT JOIN SewingLine sl WITH (NOLOCK) ON sl.ID = co.SewingLineID AND sl.FactoryID = co.FactoryID
LEFT JOIN Reason r WITH (NOLOCK) ON r.ID = s.ApparelType AND r.ReasonTypeID = 'Style_Apparel_Type'
LEFT JOIN ChgOverCheckList ccl WITH(NOLOCK) ON ccl.Category = co.Category AND ccl.StyleType = co.Type and ccl.FactoryID = co.FactoryID
LEFT JOIN ChgOverCheckListBase colb WITH(NOLOCK) ON colb.NO = CC.[NO]
LEFT JOIN ChgOverCheckList_Detail ccld WITH(NOLOCK) ON ccld.ID = ccl.ID and ccld.ChgOverCheckListBaseID = Colb.ID
OUTER APPLY
(
    SELECT LTRIM(RTRIM(m.n.value('.[1]', 'varchar(500)'))) AS ResponseDep
    FROM (
        SELECT CAST('<XMLRoot><RowData>' + REPLACE(ResponseDep, ',', '</RowData><RowData>') + '</RowData></XMLRoot>' AS XML) AS x
        FROM ChgOverCheckList_Detail ccldx WITH(NOLOCK)
	    WHERE ccl.ID = ccldx.ID  and ccldx.ChgOverCheckListBaseID = CC.[No]
        and ccld.ID = ccldx.ID
        ) t
    CROSS APPLY x.nodes('/XMLRoot/RowData') m(n)
) AS ccldx
OUTER APPLY
(
	SELECT val = isnull(iif((CC.Deadline IS NULL), 0, DATEDIFF(day,GETDATE(),CC.DeadLine) - (COUNT(1) + dbo.getDateRangeSundayCount(GETDATE(),cc.Deadline))),0)
	FROM Holiday WITH(NOLOCK)
	WHERE HolidayDate BETWEEN GETDATE() AND CC.Deadline AND FactoryID = CO.FactoryID
)DaysLefCnt
OUTER APPLY
(
	SELECT val = isnull(iif((CC.Deadline IS NULL), 0, DATEDIFF(day,CC.DeadLine,GETDATE()) -(COUNT(1) + dbo.getDateRangeSundayCount(CC.DeadLine,GETDATE()))),0)
	FROM Holiday WITH(NOLOCK)
	WHERE HolidayDate BETWEEN CC.Deadline AND GETDATE() AND FactoryID = CO.FactoryID
)OverDay_Check_0
OUTER APPLY
(
	SELECT val = isnull(iif((CC.CompletionDate IS NULL) OR (CC.Deadline IS NULL), 0, DATEDIFF(day,CC.DeadLine,CC.CompletionDate) -(COUNT(1) + dbo.getDateRangeSundayCount(CC.DeadLine,CC.CompletionDate))),0)
	FROM Holiday WITH(NOLOCK)
	WHERE HolidayDate BETWEEN CC.Deadline AND CC.CompletionDate AND FactoryID = CO.FactoryID
)OverDay_Check_1
WHERE cc.No <> 0
{sqlWhere}
Order by  [InlineDate], [SP], [Style], [Category], [ProductType], [Cell], [CheckListNo]
";
            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("Production", sql, listPar, out DataTable dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTables;
            return resultReport;
        }

        // logic for the sql parameters
        private List<SqlParameter> GetSqlParameters(IE_R22_ViewModel model)
        {
            return new List<SqlParameter>
                {
                    new SqlParameter("@DeadlineStart", SqlDbType.Date) { Value = (object)model.DeadlineStart ?? DBNull.Value },
                    new SqlParameter("@DeadlineEnd", SqlDbType.Date) { Value = (object)model.DeadlineEnd ?? DBNull.Value },
                    new SqlParameter("@InlineStart", SqlDbType.Date) { Value = (object)model.InlineStart ?? DBNull.Value },
                    new SqlParameter("@InlineEnd", SqlDbType.Date) { Value = (object)model.InlineEnd ?? DBNull.Value },
                    new SqlParameter("@ProductType", SqlDbType.VarChar) { Value = model.ProductType },
                    new SqlParameter("@FactoryID", SqlDbType.VarChar) { Value = model.FactoryID },
                };
        }

        // logic for the where clause
        private string GetWhere(IE_R22_ViewModel model)
        {
            string sqlWhere = string.Empty;
            if (model.IsPowerBI)
            {
                sqlWhere += @"
AND ((co.AddDate >= @DeadlineStart AND co.AddDate < DateAdd(day, 1, @DeadlineEnd))
OR (co.EditDate  >= @DeadlineStart AND co.EditDate  < DateAdd(day, 1, @DeadlineEnd)) 
OR co.Inline >= @DeadlineEnd)";
            }
            else if (!MyUtility.Check.Empty(model.DeadlineStart))
            {
                sqlWhere += " AND cc.Deadline >= @DeadlineStart AND cc.Deadline < DateAdd(day, 1, @DeadlineEnd) \r\n";
            }

            if (!MyUtility.Check.Empty(model.InlineStart))
            {
                sqlWhere += " AND co.Inline >= @InlineStart AND co.Inline < DateAdd(day, 1, @InlineEnd) \r\n";
            }

            if (!MyUtility.Check.Empty(model.OrderID))
            {
                sqlWhere += string.Format($" AND co.OrderID in ( {string.Join(",", model.OrderID.Split(',').Select(s => "'" + s + "'").ToList())} ) \r\n");
            }

            if (!MyUtility.Check.Empty(model.StyleID))
            {
                sqlWhere += string.Format($" AND co.StyleID in ( {string.Join(",", model.StyleID.Split(',').Select(s => "'" + s + "'").ToList())} ) \r\n");
            }

            if (!MyUtility.Check.Empty(model.ProductType))
            {
                sqlWhere += " AND r.Name = @ProductType \r\n";
            }

            if (!MyUtility.Check.Empty(model.Category))
            {
                sqlWhere += string.Format($" AND co.Category in ( {string.Join(",", model.Category.Split(',').Select(s => "'" + s + "'").ToList())} ) \r\n");
            }

            if (!MyUtility.Check.Empty(model.SewingCell))
            {
                sqlWhere += string.Format($" AND sl.SewingCell in ( {string.Join(",", model.SewingCell.Split(',').Select(s => "'" + s + "'").ToList())} ) \r\n");
            }

            if (!MyUtility.Check.Empty(model.ResponseDep))
            {
                sqlWhere += string.Format($" AND ccldx.ResponseDep in ({string.Join(",", model.ResponseDep.Split(',').Select(s => "'" + s + "'").ToList())} ) \r\n");
            }

            if (!MyUtility.Check.Empty(model.FactoryID))
            {
                sqlWhere += " AND co.FactoryID = @FactoryID \r\n";
            }

            if (model.IsOutstanding)
            {
                sqlWhere += " AND (iif(CC.[Checked] = 0 , iif(OverDay_Check_0.VAL < 0,0,OverDay_Check_0.VAL) ,iif(OverDay_Check_1.VAL < 0,0,OverDay_Check_1.VAL))) > 0 \r\n";
            }

            return sqlWhere;
        }
    }
}
