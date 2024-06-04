using Ict;
using Sci.Data;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Sci.Production.Cutting
{
    /// <summary>
    /// P02, P09共用
    /// </summary>
    public partial class CuttingWorkOrder
    {
        /// <summary>
        /// 分配 Distribute
        /// </summary>
        /// <param name="id">Cutting.ID</param>
        /// <param name="listWorkOrderUkey">listWorkOrderUkey</param>
        /// <param name="tableMiddleName">傳入字串 P02: ForPlanning  P09:ForOutput </param>
        /// <param name="sqlConnection">sqlConnection</param>
        /// <returns>DualResult</returns>
        private DualResult InsertWorkOrder_Distribute(string id, List<long> listWorkOrderUkey, string tableMiddleName, SqlConnection sqlConnection)
        {
            string whereWorkOrderUkey = listWorkOrderUkey.Select(s => s.ToString()).JoinToString(",");
            string sqlInsertWorkOrder_Distribute = $@"
select w.Ukey, w.Colorid, w.FabricCombo, ws.SizeCode, [CutQty] = isnull(ws.Qty * w.Layer, 0)
into #tmpCutting
from WorkOrder{tableMiddleName} w with (nolock)
inner join WorkOrder{tableMiddleName}_SizeRatio ws with (nolock) on ws.WorkOrder{tableMiddleName}Ukey = w.Ukey
where w.Ukey in ({whereWorkOrderUkey})
order by ukey

select * from #tmpCutting


select oq.ID, oq.Article, oq.SizeCode, [Qty] = Cast(isnull(oq.Qty, 0) as int), oc.ColorID, oc.PatternPanel, o.SewInLine
from Orders o with (nolock)
inner join Order_Qty oq with (nolock) on oq.ID = o.ID
inner join Order_ColorCombo oc with (nolock) on oc.Id = o.POID and oc.Article = oq.Article
where   o.POID = '{id}'
order by o.SewInLine, oq.ID


";
            DualResult result = DBProxy.Current.SelectByConn(sqlConnection, sqlInsertWorkOrder_Distribute, out DataTable[] dtResults);
            if (!result)
            {
                return result;
            }

            DataTable dtWorkOrderDistribute = dtResults[0];
            DataTable dtOrderQty = dtResults[1];
            string sqlInsertWorkOrderDistribute = string.Empty;

            foreach (DataRow itemDistribute in dtWorkOrderDistribute.Rows)
            {
                if (MyUtility.Convert.GetInt(itemDistribute["CutQty"]) == 0)
                {
                    continue;
                }

                string filterDistribute = $@"
PatternPanel = '{itemDistribute["FabricCombo"]}' and
SizeCode = '{itemDistribute["SizeCode"]}' and
ColorID = '{itemDistribute["Colorid"]}' and
Qty > 0
";
                DataRow[] arrayDistributeOrderQty = dtOrderQty.Select(filterDistribute, "SewInLine,ID");

                foreach (DataRow drDistributeOrderQty in arrayDistributeOrderQty)
                {
                    int distributrQty = 0;
                    if (MyUtility.Convert.GetInt(itemDistribute["CutQty"]) >= MyUtility.Convert.GetInt(drDistributeOrderQty["Qty"]))
                    {
                        distributrQty = MyUtility.Convert.GetInt(drDistributeOrderQty["Qty"]);
                    }
                    else
                    {
                        distributrQty = MyUtility.Convert.GetInt(itemDistribute["CutQty"]);
                    }

                    // 表示workOrder size數已經分配完
                    if (distributrQty == 0)
                    {
                        break;
                    }

                    itemDistribute["CutQty"] = MyUtility.Convert.GetInt(itemDistribute["CutQty"]) - distributrQty;
                    drDistributeOrderQty["Qty"] = MyUtility.Convert.GetInt(drDistributeOrderQty["Qty"]) - distributrQty;

                    sqlInsertWorkOrderDistribute += $@"
insert into WorkOrder{tableMiddleName}_Distribute(WorkOrder{tableMiddleName}Ukey, ID, OrderID, Article, SizeCode, Qty)
values({itemDistribute["Ukey"]}, '{id}', '{drDistributeOrderQty["ID"]}', '{drDistributeOrderQty["Article"]}', '{itemDistribute["SizeCode"]}', '{distributrQty}')
";
                }

                // 如果還有未分配就insert EXCESS
                if (MyUtility.Convert.GetInt(itemDistribute["CutQty"]) > 0)
                {
                    sqlInsertWorkOrderDistribute += $@"
insert into WorkOrder{tableMiddleName}_Distribute(WorkOrder{tableMiddleName}Ukey, ID, OrderID, Article, SizeCode, Qty)
values({itemDistribute["Ukey"]}, '{id}', 'EXCESS', '', '{itemDistribute["SizeCode"]}', '{itemDistribute["CutQty"]}')
";
                }
            }

            if (!MyUtility.Check.Empty(sqlInsertWorkOrderDistribute))
            {
                result = DBProxy.Current.ExecuteByConn(sqlConnection, sqlInsertWorkOrderDistribute);
            }

            return result;
        }
    }
}
