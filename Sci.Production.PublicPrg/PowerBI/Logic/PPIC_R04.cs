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
    public class PPIC_R04
    {
        private DBProxy DBProxy;

        /// <inheritdoc/>
        public PPIC_R04()
        {
            this.DBProxy = new DBProxy()
            {
                DefaultTimeout = 1800,
            };
        }

        /// <inheritdoc/>
        public Base_ViewModel GetPPIC_R04Data(PPIC_R04_ViewModel model)
        {
            string tmpsql = string.Empty;
            List<SqlParameter> listPar = new List<SqlParameter>
            {
                new SqlParameter("@ReportType", SqlDbType.VarChar, 20) { Value = model.ReportType },
                new SqlParameter("@ApvDate1", SqlDbType.Date) { Value = (object)model.ApvDate1 ?? DBNull.Value },
                new SqlParameter("@ApvDate2", SqlDbType.Date) { Value = (object)model.ApvDate2 ?? DBNull.Value },
                new SqlParameter("@MDivisionID", SqlDbType.VarChar, 10) { Value = model.MDivisionID },
                new SqlParameter("@FactoryID", SqlDbType.VarChar, 10) { Value = model.FactoryID },
                new SqlParameter("@LeadTime", SqlDbType.Int) { Value = model.LeadTime },
                new SqlParameter("@BIEditDate", SqlDbType.Date) { Value = (object)model.BIEditDate ?? DBNull.Value },
                new SqlParameter("@IsPowerBI", SqlDbType.Bit) { Value = model.IsPowerBI },
            };

            if (model.IsPowerBI)
            {
                tmpsql = @",[SP] = isnull(l.OrderID,'')
                           ,[ReplacementID] = isnull(l.ID,'')";
            }

            StringBuilder sqlCmd = new StringBuilder();
            sqlCmd.Append($@"
select distinct MDivisionID = isnull(l.MDivisionID,'')
    ,[FactoryID] = isnull(l.FactoryID,'')
    ,[ID] = isnull(l.ID,'')
    ,[SewingLineID] = isnull(l.SewingLineID,'')
	,[SewingCell] = isnull(s.SewingCell,'')
    ,[Department] = isnull(l.Dept,'')
	,[StyleID] = isnull(o.StyleID,'')
    ,[StyleName] = isnull(sty.StyleName,'')
	,[OrderID] = isnull(l.OrderID,'')
	,[Seq] = concat(isnull(ld.Seq1,''),' ',isnull(ld.Seq2,''))
	,[ColorName] = isnull(c.Name,'')
	,[Refno] = isnull(psd.Refno,'')
	,l.ApvDate
	,[RejectQty] = isnull(ld.RejectQty,0)
	,[RequestQty] = isnull(ld.RequestQty,0)
	,[IssueQty] = isnull(ld.IssueQty,0)
	,[FinishedDate] = IIF(l.Status= 'Received',l.EditDate,null)
	,[Type] = IIF(l.Type='R','Replacement','Lacking')
	,[Description] = isnull(IIF(l.FabricType = 'F',pr.Description,pr1.Description),PPICReasonID)
	,[OnTime] = IIF(l.Status = 'Received',IIF(DATEDIFF(ss,l.ApvDate,l.EditDate) <= @LeadTime,'Y','N'),'N')
	,[Remark] = isnull(l.Remark,'')
    ,[Process] = isnull(ld.Process,'')
    ,[FabricType] = isnull(l.FabricType,'')
	,[DetailRemark] = isnull(ld.Remark,'')
    ,[MaterialType] = CASE 
					  WHEN psd.FabricType = 'F' THEN 'Fabric-' + ISNULL(f.MtlTypeID, '')
					  WHEN psd.FabricType = 'A' THEN 'Accessory-' + ISNULL(f.MtlTypeID, '')
					  WHEN psd.FabricType = 'O' THEN 'Other-' + ISNULL(f.MtlTypeID, '')
					  ELSE isnull(f.MtlTypeID,'')
					  END
    ,[SewingQty] = isnull(SewingQty.val,0)
    ,[BIFactoryID] = (select top 1 IIF(RgCode = 'PHI', 'PH1', RgCode) from Production.dbo.[System])
    ,[BIInsertDate] = GETDATE()
    {(model.IsPowerBI ? tmpsql : string.Empty)}
from Lack l WITH (NOLOCK) 
inner join Lack_Detail ld WITH (NOLOCK) on l.ID = ld.ID
left join SewingLine s WITH (NOLOCK) on s.ID = l.SewingLineID AND S.FactoryID=L.FactoryID
left join Orders o WITH (NOLOCK) on o.ID = l.OrderID
left join Style sty WITH (NOLOCK) on sty.Ukey = o.StyleUkey
left join PO_Supp_Detail psd WITH (NOLOCK) on psd.ID = l.POID and psd.SEQ1 = ld.Seq1 and psd.SEQ2 = ld.Seq2
left join PO_Supp_Detail_Spec psdsC WITH (NOLOCK) on psdsC.ID = psd.id and psdsC.seq1 = psd.seq1 and psdsC.seq2 = psd.seq2 and psdsC.SpecColumnID = 'Color'
LEFT JOIN Fabric f WITH (NOLOCK) ON psd.SCIRefNo = f.SCIRefNo
left join Color c WITH (NOLOCK) on c.BrandId = o.BrandID and c.ID = isnull(psdsC.SpecValue ,'')
left join PPICReason pr WITH (NOLOCK) on pr.Type = 'FL' and (pr.ID = ld.PPICReasonID or pr.ID = concat('FR','0',ld.PPICReasonID))
left join PPICReason pr1 WITH (NOLOCK) on pr1.Type = 'AL' and (pr1.ID = ld.PPICReasonID or pr1.ID = concat('AR','0',ld.PPICReasonID))
OUTER APPLY
(
	SELECT 
    val = SUM(minSewQty.val)
	FROM
	(
		SELECT 
			oq.Article,
			oq.SizeCode,
			sl.Location AS ComboType,
			val = MIN(ISNULL(sdd.QAQty, 0))
		FROM Orders oop WITH (NOLOCK) 
		INNER JOIN Order_Location sl WITH (NOLOCK) ON sl.OrderId =oop.ID
		INNER JOIN Order_Qty oq WITH (NOLOCK) ON oq.ID = oop.ID
		LEFT JOIN SewingOutput_Detail_Detail sdd WITH (NOLOCK) 
			ON sdd.OrderId = oop.ID 
			AND sdd.Article = oq.Article 
			AND sdd.SizeCode = oq.SizeCode 
			AND sdd.ComboType = sl.Location
		WHERE oop.ID = o.ID
		GROUP BY oq.Article, oq.SizeCode, sl.Location
	) minSewQty
)SewingQty 
WHERE 1=1
");

            #region Where

            if (model.IsPowerBI == true)
            {
                sqlCmd.Append(@"
and l.EditDate >= @BIEditDate
" + Environment.NewLine);
            }
            else
            {
                if (!MyUtility.Check.Empty(model.ReportType))
                {
                    sqlCmd.Append($"AND l.FabricType = @ReportType " + Environment.NewLine);
                }

                if (!MyUtility.Check.Empty(model.ApvDate1))
                {
                    sqlCmd.Append($" and convert(date,l.ApvDate) >= '{Convert.ToDateTime(model.ApvDate1).ToString("yyyy/MM/dd")}'");
                }

                if (!MyUtility.Check.Empty(model.ApvDate2))
                {
                    sqlCmd.Append($" and convert(date,l.ApvDate) <= '{Convert.ToDateTime(model.ApvDate2).ToString("yyyy/MM/dd")}'");
                }

                if (!MyUtility.Check.Empty(model.MDivisionID))
                {
                    sqlCmd.Append(" and l.MDivisionID = @MDivisionID");
                }

                if (!MyUtility.Check.Empty(model.FactoryID))
                {
                    sqlCmd.Append($" and l.FactoryID = @FactoryID");
                }
            }

            #endregion

            Base_ViewModel resultReport = new Base_ViewModel
            {
                Result = this.DBProxy.Select("Production", sqlCmd.ToString(), listPar, out DataTable dataTable),
            };

            if (!resultReport.Result)
            {
                return resultReport;
            }

            resultReport.Dt = dataTable;
            return resultReport;
        }
    }
}
