using Ict;
using Sci.Data;
using Sci.Production.Prg;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sci.Production.Cutting
{
    /// <summary>
    /// P02
    /// </summary>
    public partial class P02
    {
        private int markerSerNo = 1;
        private List<long> listWorkOrderUkey;

        private DualResult ImportKHMarkerExcel()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel files (*.xlsx;*.xls)|*.xlsx;*.xls";

            DialogResult dialogResult = openFileDialog.ShowDialog();

            if (dialogResult != DialogResult.OK)
            {
                return new DualResult(true);
            }

            this.ShowWaitMessage("Loading...");
            string filename = openFileDialog.FileName;

            Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();

            excel.Workbooks.Open(MyUtility.Convert.GetString(filename));

            int sheetCnt = excel.ActiveWorkbook.Worksheets.Count;

            SqlConnection sqlConn;
            DualResult result = DBProxy._OpenConnection(null, out sqlConn);

            if (!result)
            {
                return result;
            }

            bool isNoMatchSP = true;

            using (TransactionScope transactionScope = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromMinutes(5)))
            using (sqlConn)
            {
                try
                {
                    for (int i = 1; i <= sheetCnt; i++)
                    {
                        Excel.Worksheet worksheet = excel.ActiveWorkbook.Worksheets[i];
                        this.markerSerNo = 1;

                        // 檢查SP
                        string poID = worksheet.GetCellValue(2, 2);

                        if (poID != this.CurrentMaintain["ID"].ToString())
                        {
                            Marshal.ReleaseComObject(worksheet);
                            continue;
                        }

                        isNoMatchSP = false;

                        // inset WorkOrder, WorkOrder_SizeRatio
                        result = this.LoadDetailPart(worksheet, poID, sqlConn);

                        if (!result)
                        {
                            this.HideWaitMessage();
                            Marshal.ReleaseComObject(worksheet);
                            return result;
                        }

                        // inset WorkOrder_Distribute and update WorkOrder.OrderID
                        this.InsertWorkOrder_Distribute(this.listWorkOrderUkey, sqlConn);
                        if (!result)
                        {
                            this.HideWaitMessage();
                            Marshal.ReleaseComObject(worksheet);
                            return result;
                        }

                        Marshal.ReleaseComObject(worksheet);
                    }

                    transactionScope.Complete();
                    transactionScope.Dispose();
                }
                catch (Exception ex)
                {
                    this.HideWaitMessage();
                    transactionScope.Dispose();
                    return new DualResult(false, ex);
                }
            }

            this.HideWaitMessage();

            if (isNoMatchSP)
            {
                MyUtility.Msg.InfoBox($"Excel not contain Cutting SP<{this.CurrentMaintain["ID"]}>");
            }

            return new DualResult(true);
        }

        private DualResult LoadDetailPart(Microsoft.Office.Interop.Excel.Worksheet worksheet, string poID, SqlConnection sqlConnection)
        {
            this.listWorkOrderUkey = new List<long>();
            string keyWord_FabPanelCode = "布种:";
            DataTable dtWorkOrder;

            DBProxy.Current.Select(null, "select * from WorkOrder where 1 = 0", out dtWorkOrder);

            int curRowIndex = 1;
            int emptyRowCount = 0;
            while (true)
            {
                // 如果讀取超過20行都沒有"布种:"，就當作此sheet已經沒資料
                if (emptyRowCount > 20)
                {
                    break;
                }

                // 定位布种欄位
                if (worksheet.GetCellValue(1, curRowIndex) != keyWord_FabPanelCode)
                {
                    curRowIndex++;
                    emptyRowCount++;
                    continue;
                }

                emptyRowCount = 0;

                DataRow drWorkOrder = dtWorkOrder.NewRow();
                drWorkOrder["FabricPanelCode"] = worksheet.GetCellValue(2, curRowIndex);

                if (MyUtility.Check.Empty(drWorkOrder["FabricPanelCode"]))
                {
                    curRowIndex += 16;
                    continue;
                }

                string[] colorInfo = worksheet.GetCellValue(2, curRowIndex + 2).Split('-');

                drWorkOrder["ID"] = this.CurrentMaintain["ID"];
                drWorkOrder["FactoryID"] = this.CurrentMaintain["FactoryID"];
                drWorkOrder["MDivisionId"] = this.CurrentMaintain["MDivisionid"];
                drWorkOrder["Colorid"] = colorInfo[0];
                drWorkOrder["Tone"] = colorInfo.Length == 1 ? string.Empty : colorInfo[1];

                string sqlGetWorkOrderInfo = $@"
select top 1	oc.PatternPanel,
                oc.FabricPanelCode,
				oc.FabricCode,
				ob.SCIRefno,
				ob.Refno,
				psd.SEQ1,
				psd.SEQ2,
				[CuttingLayer] = iif(isnull(c.CuttingLayer, 100) = 0, 100, isnull(c.CuttingLayer, 100))
from Order_ColorCombo oc with (nolock)
inner join Order_BOF ob with (nolock) on ob.Id = oc.id and ob.FabricCode = oc.FabricCode
inner join PO_Supp_Detail psd with (nolock) on psd.ID = oc.Id and psd.SCIRefno = ob.SCIRefno and psd.ColorID = oc.ColorID
inner join Fabric f with(nolock) ON ob.SCIRefno=f.SCIRefno
left join Construction c on c.Id = f.ConstructionID and c.Junk = 0
where oc.ID = '{poID}' and oc.FabricPanelCode = '{drWorkOrder["FabricPanelCode"]}' and oc.ColorID = '{drWorkOrder["Colorid"]}'
";
                DataRow drWorkOrderInfo;
                bool existsColor = MyUtility.Check.Seek(sqlGetWorkOrderInfo, out drWorkOrderInfo);

                if (!existsColor)
                {
                    return new DualResult(false, $"<Color> {drWorkOrder["Colorid"]} not exists");
                }

                drWorkOrder["SEQ1"] = drWorkOrderInfo["SEQ1"];
                drWorkOrder["SEQ2"] = drWorkOrderInfo["SEQ2"];
                drWorkOrder["Refno"] = drWorkOrderInfo["Refno"];
                drWorkOrder["SCIRefno"] = drWorkOrderInfo["SCIRefno"];
                drWorkOrder["FabricCode"] = drWorkOrderInfo["FabricCode"];
                drWorkOrder["FabricPanelCode"] = drWorkOrderInfo["FabricPanelCode"];
                drWorkOrder["FabricCombo"] = drWorkOrderInfo["PatternPanel"];

                // 取得SizeRatio Range
                Excel.Range rangeSizeRatio = worksheet.GetRange(1, curRowIndex, 21, curRowIndex + 13);
                this.LoadSizeRatio(rangeSizeRatio, drWorkOrder, MyUtility.Convert.GetInt(drWorkOrderInfo["CuttingLayer"]), sqlConnection);

                curRowIndex += 16;
            }

            return new DualResult(true);
        }

        private DualResult LoadSizeRatio(Excel.Range rangeSizeRatio, DataRow drWorkOrder, int cuttingLayer, SqlConnection sqlConnection)
        {
            // 讀每一個MkName
            for (int idxMarker = 7; idxMarker < 15; idxMarker++)
            {
                int idxSize = 3;
                int totalLayer = 0;
                int garmentCnt = 0;
                decimal conPcs = 0;
                decimal layerYDS = 0;
                Dictionary<string, int> dicSizeRatio = new Dictionary<string, int>();

                while (true)
                {
                    string size = rangeSizeRatio.GetCellValue(idxSize, 2);
                    if (size.ToUpper() == "TOTAL")
                    {
                        totalLayer = MyUtility.Convert.GetInt(rangeSizeRatio.GetCellValue(idxSize, idxMarker));
                        layerYDS = MyUtility.Convert.GetDecimal(rangeSizeRatio.GetCellValue(idxSize + 2, idxMarker));
                        break;
                    }

                    int sizeQty = MyUtility.Convert.GetInt(rangeSizeRatio.GetCellValue(idxSize, idxMarker));
                    if (sizeQty == 0)
                    {
                        idxSize++;
                        continue;
                    }

                    dicSizeRatio.Add(size, sizeQty);
                    idxSize++;
                }

                if (dicSizeRatio.Count == 0 || totalLayer == 0)
                {
                    continue;
                }

                garmentCnt = dicSizeRatio.Sum(s => s.Value);
                List<SqlParameter> listPar = new List<SqlParameter>() { new SqlParameter("@ydsPcs", layerYDS / garmentCnt) };
                conPcs = MyUtility.Convert.GetDecimal(MyUtility.GetValue.Lookup("select dbo.GetUnitQty('M', 'CONE', dbo.GetUnitQty('YDS', 'M', @ydsPcs))", listPar));

                // insert WorkOrder
                List<long> newWorkOrderUkey = this.InsertWorkOrder(drWorkOrder, totalLayer, cuttingLayer, conPcs, garmentCnt, sqlConnection);
                this.listWorkOrderUkey.AddRange(newWorkOrderUkey);
                foreach (long workOrderUkey in newWorkOrderUkey)
                {
                    foreach (KeyValuePair<string, int> itemSizeRatio in dicSizeRatio)
                    {
                        string sqlInsertWorkOrder_SizeRatio = $@"
insert into WorkOrder_SizeRatio(WorkOrderUkey, ID, SizeCode, Qty)
            values('{workOrderUkey}', '{this.CurrentMaintain["ID"]}', '{itemSizeRatio.Key}', '{itemSizeRatio.Value}')
";
                        DualResult result = DBProxy.Current.ExecuteByConn(sqlConnection, sqlInsertWorkOrder_SizeRatio);
                        if (!result)
                        {
                            return result;
                        }
                    }
                }
            }

            return new DualResult(true);
        }

        private List<long> InsertWorkOrder(DataRow drWorkOrder, int totalLayer, int cuttingLayer, decimal conPcs, int garmentCnt, SqlConnection sqlConnection)
        {
            List<long> listWorkOrderUkey = new List<long>();
            string markername = "MK_" + this.markerSerNo.ToString().PadLeft(3, '0');
            while (true)
            {
                int layer = totalLayer > cuttingLayer ? cuttingLayer : totalLayer;
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@conPcs", conPcs),
                    new SqlParameter("@Cons", garmentCnt * conPcs * layer),
                };
                string sqlInsertWorkOrder = $@"
insert into WorkOrder(
ID
,FactoryID
,MDivisionId
,SEQ1
,SEQ2
,Layer
,Colorid
,Markername
,MarkerLength
,ConsPC
,Cons
,Refno
,SCIRefno
,MarkerNo
,MarkerVersion
,Type
,AddName
,AddDate
,FabricCombo
,FabricCode
,FabricPanelCode
,Order_EachconsUkey
,OldFabricUkey
,OldFabricVer
,ActCuttingPerimeter
,StraightLength
,CurvedLength
,SpreadingNoID
,Shift
,Tone
)
values
(
'{this.CurrentMaintain["ID"]}'
,'{this.CurrentMaintain["FactoryID"]}'
,'{this.CurrentMaintain["MDivisionid"]}'
,'{drWorkOrder["SEQ1"]}'
,'{drWorkOrder["SEQ2"]}'
,'{layer}'
,'{drWorkOrder["Colorid"]}'
,'{markername}'
,'' --MarkerLength
,@conPcs --ConsPC
,@Cons --Cons
,'{drWorkOrder["Refno"]}'
,'{drWorkOrder["SCIRefno"]}'
,''
,-1
,'1'
,'{Env.User.UserID}'
,getdate()
,'{drWorkOrder["FabricCombo"]}'
,'{drWorkOrder["FabricCode"]}'
,'{drWorkOrder["FabricPanelCode"]}'
,-1
,''
,''
,''
,''
,''
,''
,''
,'{drWorkOrder["Tone"]}'
)

select [WorkOrderUkey] = @@IDENTITY
";
                DualResult result = DBProxy.Current.SelectByConn(sqlConnection, sqlInsertWorkOrder, out DataTable dtResult);

                if (!result)
                {
                    throw result.GetException();
                }

                long workOrderUkey = MyUtility.Convert.GetLong(dtResult.Rows[0][0]);

                listWorkOrderUkey.Add(workOrderUkey);

                if (totalLayer < cuttingLayer)
                {
                    break;
                }

                totalLayer -= cuttingLayer;
            }

            if (listWorkOrderUkey.Count > 0)
            {
                this.markerSerNo++;
            }

            return listWorkOrderUkey;
        }

        private DualResult InsertWorkOrder_Distribute(List<long> listWorkOrderUkey, SqlConnection sqlConnection)
        {
            string whereWorkOrderUkey = listWorkOrderUkey.Select(s => s.ToString()).JoinToString(",");
            string sqlInsertWorkOrder_Distribute = $@"
select w.Ukey, w.Colorid, w.FabricCombo, ws.SizeCode, [CutQty] = isnull(ws.Qty * w.Layer, 0)
into #tmpCutting
from WorkOrder w with (nolock)
inner join WorkOrder_SizeRatio ws with (nolock) on ws.WorkOrderUkey = w.Ukey
where w.Ukey in ({whereWorkOrderUkey})
order by ukey

select * from #tmpCutting


select oq.ID, oq.Article, oq.SizeCode, [Qty] = isnull(oq.Qty, 0), oc.ColorID, oc.PatternPanel, o.SewInLine
from Orders o with (nolock)
inner join Order_Qty oq with (nolock) on oq.ID = o.ID
inner join Order_ColorCombo oc with (nolock) on oc.Id = o.POID and oc.Article = oq.Article and oc.FabricType = 'F' and oc.ColorID = 'PELT'
where   o.POID = '{this.CurrentMaintain["ID"]}'
order by o.SewInLine, oq.ID


";

            DataTable[] dtResults;
            DBProxy.Current.SelectByConn(sqlConnection, sqlInsertWorkOrder_Distribute, out dtResults);
            DataTable dtWorkOrderDistribute = dtResults[0];
            DataTable dtOrderQty = dtResults[1];
            string sqlInsertWorkOrderDistribute = string.Empty;
            DualResult result = new DualResult(true);
            foreach (DataRow itemDistribute in dtWorkOrderDistribute.Rows)
            {
                if ((int)itemDistribute["CutQty"] == 0)
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
                    if ((int)itemDistribute["CutQty"] >= (int)drDistributeOrderQty["Qty"])
                    {
                        distributrQty = (int)drDistributeOrderQty["Qty"];
                    }
                    else
                    {
                        distributrQty = (int)itemDistribute["CutQty"];
                    }

                    // 表示workOrder size數已經分配完
                    if (distributrQty == 0)
                    {
                        break;
                    }

                    itemDistribute["CutQty"] = (int)itemDistribute["CutQty"] - distributrQty;
                    drDistributeOrderQty["Qty"] = (int)drDistributeOrderQty["Qty"] - distributrQty;

                    sqlInsertWorkOrderDistribute += $@"
insert into WorkOrder_Distribute(WorkOrderUkey, ID, OrderID, Article, SizeCode, Qty)
values({itemDistribute["Ukey"]}, '{this.CurrentMaintain["ID"]}', '{drDistributeOrderQty["ID"]}', '{drDistributeOrderQty["Article"]}', '{itemDistribute["SizeCode"]}', '{distributrQty}')
";
                }

                // 如果還有未分配就insert EXCESS
                if ((int)itemDistribute["CutQty"] > 0)
                {
                    sqlInsertWorkOrderDistribute += $@"
insert into WorkOrder_Distribute(WorkOrderUkey, ID, OrderID, Article, SizeCode, Qty)
values({itemDistribute["Ukey"]}, '{this.CurrentMaintain["ID"]}', 'EXCESS', '', '{itemDistribute["SizeCode"]}', '{itemDistribute["CutQty"]}')
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
