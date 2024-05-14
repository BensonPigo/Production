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
    public class SimilarStyle
    {
        /// <inheritdoc/>
        public SimilarStyle()
        {
            DBProxy.Current.DefaultTimeout = 1800;
        }

        /// <inheritdoc/>
        public Base_ViewModel GetSimilarStyleData(DateTime sdate)
        {
            StringBuilder sqlCmd = new StringBuilder();
            #region Where

            string where = $@"and s.OutputDate >= '{sdate.ToString("yyyy-MM-dd")}'";

            #endregion

            #region SQL

            // 基本資料
            sqlCmd.Append($@"
Select distinct
s.outPutDate,
s.FactoryID,
o.StyleID, 
BrandID = case 	when o.BrandID != 'SUBCON-I' then o.BrandID
		        when Order2.BrandID is not null then Order2.BrandID
		        when StyleBrand.BrandID is not null then StyleBrand.BrandID
		        else o.BrandID end,
Remark = isnull(r.Remark,''),
RemarkSimilarStyle = isnull(simerRemark.rRemark,''),
Type = iif((r.Remark != '' or isnull(simerRemark.rRemark,'') != '') and s.outPutDate between DateADD(month, -3, Getdate()) and GetDate(), 'Repeat', 'New Style' )
from Orders o with (nolock)
LEFT JOIN SewingOutput_Detail sd with (nolock) ON sd.OrderId = o.ID
LEFT JOIN SewingOutput s with (nolock) ON sd.ID = s.ID
Outer Apply(
Select max(s1.OutputDate) as OutputDate, Min(s2.SewingLineID) as SewingLineID
from Orders o2 with (nolock)
LEFT JOIN SewingOutput_Detail sd2 with (nolock) on sd2.OrderId = o2.ID
LEFT JOIN SewingOutput s1 with (nolock) ON sd2.ID = s1.ID
Inner Join SewingOutput s2 with (nolock) ON s2.ID = s1.ID and s1.OutputDate = s2.OutputDate
where o2.StyleID = o.StyleID
) MaxSew
Outer apply(
Select Remark = Case When isnull( MaxSew.SewingLineID,'') != '' and isnull(MaxSew.OutputDate,'') != '' 
				then MaxSew.SewingLineID + '(' + CONVERT(NVARCHAR, MaxSew.OutputDate, 111) + ')'
				else '' end
)R
Outer Apply(
Select rRemark = Case when MaxSew.OutputDate Between DATEADD(month,-3, GETDATE()) and GETDATE() then STUFF((
select ',' + simer.StyleID + '→' + MaxSew.SewingLineID + '(' + CONVERT(NVARCHAR, MaxSew.OutputDate, 111) + ')' 
from ( SELECT  simer.StyleID
          FROM (
            SELECT DISTINCT * FROM 
			     (SELECT StyleID = MasterStyleID, BrandID = MasterBrandID
                  FROM  Style_SimilarStyle ss with (nolock)
                  WHERE EXISTS ( SELECT 1  FROM Style s with(nolock) 
								 WHERE  s.Ukey = o.StyleUkey  AND ss.MasterStyleID = s.id  AND s.BrandID = ss.MasterBrandID )
                  UNION 
                  SELECT StyleID = MasterStyleID,  BrandID = MasterBrandID
                  FROM  Style_SimilarStyle ss with (nolock)
                  WHERE EXISTS ( SELECT 1 FROM Style s with(nolock)
                                 WHERE  s.Ukey = o.StyleUkey AND ss.ChildrenStyleID = s.id AND s.BrandID = ss.ChildrenBrandID
                    )
            ) m
        ) AS simer) as simer
for xml path ('')), 1, 1, '' )
else '' end
) as simerRemark
outer apply( select BrandID from orders o1 with (nolock) where o.CustPONo = o1.id) Order2
outer apply( select top 1 BrandID from Style with (nolock) where id = o.StyleID 
    and SeasonID = o.SeasonID and BrandID != 'SUBCON-I') StyleBrand
where 1=1 
{where}
Order by s.outPutDate
");

            #endregion

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = DBProxy.Current.Select("Production", sqlCmd.ToString(), out DataTable[] dataTables),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.DtArr = dataTables;
            return resultReport;
        }
    }
}
