using Ict;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sci.Data;
using Sci.Production.Automation;
using Sci.Production.CallPmsAPI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static PmsWebApiUtility20.WebApiTool;
using static Sci.AuthenticationAPI.AuthenticationAD;

namespace Sci.Production.Cutting
{
    /// <summary>
    /// P02, P09共用
    /// </summary>
    public partial class CuttingWorkOrder
    {
        /// <summary>
        /// 取得下一筆CutRef流水號
        /// </summary>
        /// <param name="tableName">tableName</param>
        /// <returns>string</returns>
        public static string GetNextCutRef(string tableName)
        {
            var postBody = new
            {
                TableName = tableName,
                ColumnName = "CutRef",
            };

            string maxCutRef = string.Empty;
            WebApiBaseResult webApiBaseResult = PmsWebApiUtility45.WebApiTool.WebApiPost(PackingA2BWebAPI.GetCurrentWebAPIUrl(), "api/ColumnValue/GetNextValue", postBody, 75);
            if (webApiBaseResult.isSuccess)
            {
                maxCutRef = webApiBaseResult.responseContent.Replace("\"", string.Empty);
            }

            return maxCutRef;
        }

        /// <inheritdoc/>
        public static void AutoCut(IList<DataRow> detailDatas, string tableMiddleName)
        {
            int maxcutno;
            string colName = tableMiddleName == "ForPlanning" ? "CutplanID" : "CutNo";
            DataTable wk = detailDatas.CopyToDataTable();
            foreach (DataRow dr in detailDatas)
            {
                if (MyUtility.Check.Empty(dr[colName]) && !MyUtility.Check.Empty(dr["estcutdate"]))
                {
                    string temp = wk.Compute($"Max({colName})", string.Format("FabricCombo ='{0}'", dr["FabricCombo"])).ToString();
                    if (MyUtility.Check.Empty(temp))
                    {
                        maxcutno = 1;
                    }
                    else
                    {
                        int maxno = Convert.ToInt32(wk.Compute($"Max({colName})", string.Format("FabricCombo ='{0}'", dr["FabricCombo"])));
                        maxcutno = maxno + 1;
                    }

                    dr["colName"] = maxcutno;
                }
            }
        }

        /// <inheritdoc/>
        public static DualResult AutoRef(string id, string keyword, string tableMiddleName)
        {
            string colName = tableMiddleName == "ForPlanning" ? "CutplanID" : "CutNo";
            string where = tableMiddleName == "ForPlanning" ? string.Empty : "and (CutCellid is not null and CutCellid !='' )";
            string cmdWhere = tableMiddleName == "ForPlanning" ? "And (cutplanid is null or cutplanid = '')" : "And CutNo is not null";
            #region 變更先將同d,Cutref, FabricPanelCode, CutNo, MarkerName, estcutdate 且有cutref,Cuno無cutplanid 的cutref值找出來Group by→cutref 會相同
            string cmdsql = $@"
SELECT isnull(w.Cutref,'') as cutref, isnull(w.FabricCombo,'') as FabricCombo, w.{colName},
isnull(w.MarkerName,'') as MarkerName, w.estcutdate, ws.SizeRatio
FROM Workorder{tableMiddleName} w WITH (NOLOCK) 
Outer APPLY(
select Stuff((
SELECT
 ',' + b.SizeCode + ':' + Cast(b.Qty as varchar)
from 
    ( 
	   select SizeCode,Qty from WorkOrder{tableMiddleName}_SizeRatio ws where ws.WorkOrder{tableMiddleName}Ukey = w.Ukey and w.ID = ws.ID
	   --Order by ws.SizeCode
    ) b
    FOR XML PATH('')
    ), 1, 1, '') as SizeRatio
) ws
WHERE (w.cutref is not null and w.cutref !='') and w.id = '{id}' and w.mDivisionid = '{keyword}'
{cmdWhere}
GROUP BY w.Cutref, w.FabricCombo, w.{colName}, w.MarkerName, w.estcutdate, ws.SizeRatio
";
            DualResult cutrefresult = DBProxy.Current.Select(null, cmdsql, out DataTable cutreftb);
            if (!cutrefresult)
            {
                return cutrefresult;
            }
            #endregion

            // 找出空的cutref
            cmdsql = $@"
Select * 
From workorder{tableMiddleName} w WITH (NOLOCK) 
Outer APPLY(
select Stuff((
SELECT
 ',' + b.SizeCode + ':' + Cast(b.Qty as varchar)
from 
    ( 
	   select SizeCode,Qty from WorkOrder{tableMiddleName}_SizeRatio ws where ws.WorkOrder{tableMiddleName}Ukey = w.Ukey and w.ID = ws.ID
	   --Order by ws.SizeCode
    ) b
    FOR XML PATH('')
    ), 1, 1, '') as SizeRatio
) ws
Where (w.{colName} is not null ) and (w.cutref is null or w.cutref ='') 
and (w.estcutdate is not null and w.estcutdate !='' )
{where}
and w.id = '{id}' and w.mDivisionid = '{keyword}'
order by w.FabricCombo,w.{colName}
";
            cutrefresult = DBProxy.Current.Select(null, cmdsql, out DataTable workordertmp);
            if (!cutrefresult)
            {
                return cutrefresult;
            }

            string updatecutref = $@"
Create table #tmpWorkorder{tableMiddleName}
	(
		Ukey bigint
	)
DECLARE @chk tinyint
SET @chk = 0
Begin Transaction [Trans_Name] -- Trans_Name 
";

            // 寫入空的Cutref
            foreach (DataRow dr in workordertmp.Rows)
            {
                DataRow[] findrow = cutreftb.Select(string.Format(@"MarkerName = '{0}' and FabricCombo = '{1}' and {2} = {3} and estcutdate = '{4}' and SizeRatio = '{5}' ", dr["MarkerName"], dr["FabricCombo"], colName, dr["Cutno"], dr["estcutdate"], dr["SizeRatio"]));
                string newcutref = string.Empty;

                // P09 若有找到同馬克同部位同Cutno同裁剪日就寫入同cutref，WorkOrderForOutput_SizeRatio內容物也要一模一樣才寫入相同的CutRef
                // P02 一列資料一個 CutRef, 不得重覆
                if (findrow.Length != 0 && tableMiddleName != "ForPlanning")
                {
                    newcutref = findrow[0]["cutref"].ToString();
                }
                else
                {
                    string maxref = string.Empty;
                    maxref = GetNextCutRef($"Workorder{tableMiddleName}");
                    DataRow newdr = cutreftb.NewRow();
                    newdr["MarkerName"] = dr["MarkerName"] ?? string.Empty;
                    newdr["FabricCombo"] = dr["FabricCombo"] ?? string.Empty;
                    if (tableMiddleName == "ForPlanning")
                    {
                        newdr["CutPlanID"] = dr["CutPlanID"];
                    }
                    else
                    {
                        newdr["Cutno"] = dr["Cutno"];
                    }

                    newdr["estcutdate"] = dr["estcutdate"] ?? string.Empty;
                    newdr["cutref"] = maxref;
                    newdr["SizeRatio"] = dr["SizeRatio"];
                    cutreftb.Rows.Add(newdr);
                    newcutref = maxref;
                }

                updatecutref += string.Format($@"
    if (select COUNT(1) from Workorder{tableMiddleName} WITH (NOLOCK) where cutref = '{newcutref}' and id != '{id}')>0
	begin
		RAISERROR ('Duplicate Cutref. Please redo Auto Ref#',12, 1) 
		Rollback Transaction [Trans_Name] -- 復原所有操作所造成的變更
	end
    Update Workorder{tableMiddleName} set cutref = '{newcutref}' 
    output	INSERTED.Ukey
	into #tmpWorkorder{tableMiddleName}
    where ukey = '{dr["ukey"]}';");
            }

            updatecutref += $@"
    IF @@Error <> 0 BEGIN SET @chk = 1 END
IF @chk <> 0 BEGIN -- 若是新增資料發生錯誤
    Rollback Transaction [Trans_Name] -- 復原所有操作所造成的變更
END
ELSE BEGIN
    select w.* 
    from #tmpWorkorder{tableMiddleName} tw
    inner join WorkOrder{tableMiddleName} w with (nolock) on tw.Ukey = w.Ukey

    Commit Transaction [Trans_Name] -- 提交所有操作所造成的變更
END";

            DualResult upResult;
            DataTable dtWorkorder = new DataTable();
            TransactionScope transactionscope = new TransactionScope();
            using (transactionscope)
            {
                if (!(upResult = DBProxy.Current.Select(null, updatecutref, out dtWorkorder)))
                {
                    if (upResult.ToString().Contains("Duplicate Cutref. Please redo Auto Ref#"))
                    {
                        transactionscope.Dispose();
                        MyUtility.Msg.WarningBox("Duplicate Cutref. Please redo Auto Ref#");
                    }
                    else
                    {
                        transactionscope.Dispose();
                        return upResult;
                    }
                }
                else
                {
                    transactionscope.Complete();
                    if (dtWorkorder.Rows.Count > 0)
                    {
                        Task.Run(() => new Guozi_AGV().SentWorkOrderToAGV(dtWorkorder));
                    }
                }
            }

            return upResult;
        }
    }
}
