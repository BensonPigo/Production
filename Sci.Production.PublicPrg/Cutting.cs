using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Sci.Data;
using Sci;
using Ict;
using Ict.Win;

namespace Sci.Production.PublicPrg
{

    public static partial class Prgs
    {
        #region BundleCardCheckSubprocess
        /// <summary>
        /// BundleCardCheckSubprocess(string[] ann, string patterncode,DataTable artTb, out bool lallpart)
        /// </summary>
        /// <param name="ann"></param>
        /// <param name="patterncode"></param>
        /// <param name="artTb"></param>
        /// <param name="lallpartas"></param>
        /// <returns>string</returns>
        public static string BundleCardCheckSubprocess(string[] ann, string patterncode,DataTable artTb, out bool lallpart)
        {
            //artTb 是給前Form 使用同Garment List 的PatternCode 與Subrpocess
            string art = "";
            lallpart = true; //是不是All part
            for (int i = 0; i < ann.Length; i++) //寫入判斷是否存在Subprocess
            {
                string[] ann2 = ann[i].ToString().Split(' '); //剖析Annotation
                if (ann2.Length > 0)
                {
                    #region 有分開字元需剖析
                    for (int j = 0; j < ann2.Length; j++)
                    {
                        if (MyUtility.Check.Seek(ann2[j], "subprocess", "Id"))
                        {
                            lallpart = false;
                            //Artwork 相同的也要顯示, ex: HT+HT
                            //if (art.IndexOf(ann2[j]) == -1)
                            //{
                                DataRow[] existdr = artTb.Select(string.Format("PatternCode ='{0}' and Subprocessid ='{1}'", patterncode, ann2[j]));
                                if (existdr.Length == 0)
                                {
                                    DataRow ndr_art = artTb.NewRow();
                                    ndr_art["PatternCode"] = patterncode;
                                    ndr_art["SubProcessid"] = ann2[j];
                                    artTb.Rows.Add(ndr_art);
                                }
                                if (art == "") art = ann2[j];
                                else art = art.Trim() + "+" + ann2[j];
                            //}
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 無分開字元
                    if (MyUtility.Check.Seek(ann[i], "subprocess", "Id"))
                    {
                        lallpart = false;
                        if (art.IndexOf(ann[i]) == -1)
                        {
                            DataRow[] existdr = artTb.Select(string.Format("PatternCode ='{0}' and Subprocessid ='{1}'", patterncode, ann[i]));
                            if (existdr.Length == 0) //表示無在ArtTable 內
                            {
                                DataRow ndr_art = artTb.NewRow();
                                ndr_art["PatternCode"] = patterncode;
                                ndr_art["SubProcessid"] = ann[i];                             
                                artTb.Rows.Add(ndr_art);
                            }
                            if (art == "") art = ann[i];
                            else art = art.Trim() + "+" + ann[i];
                        }
                    }
                    #endregion
                }
            }
            return art;
        }
        #endregion;

        /// <summary>
        /// 取得Cutting成套的數量
        /// </summary>
        /// <param name="OrderID"></param>
        /// <returns></returns>
        public static List<GarmentList> GetCut(List<string> OrderIDs)
        {

            DataTable tmpDt;
            DualResult result;
            List<GarmentList> GarmentListList = new List<GarmentList>();

            //取得該訂單的組成
            #region 取得該訂單的組成
            string tmpCmd = $@"
SELECT DISTINCT 
    [OrderID]=o.ID
    ,oq.Article
    ,oq.SizeCode
    ,occ.PatternPanel
    ,cons.FabricPanelCode
FROM Orders o WITH (NOLOCK)
INNER JOIN Order_qty oq ON o.ID=oq.ID
INNER JOIN Order_ColorCombo occ ON o.poid = occ.id AND occ.Article = oq.Article
INNER JOIN order_Eachcons cons ON occ.id = cons.id AND cons.FabricCombo = occ.PatternPanel AND cons.CuttingPiece='0'
WHERE occ.FabricCode !='' AND occ.FabricCode IS NOT NULL
AND o.id IN ('{OrderIDs.JoinToString("','")}')
";

            result = DBProxy.Current.Select(null, tmpCmd, out tmpDt);

            foreach (var Key in tmpDt.AsEnumerable().Select(o => new
            {
                OrderID = o["OrderID"].ToString(),
                Article = o["Article"].ToString(),
                SizeCode = o["SizeCode"].ToString()
            }).Distinct()
                    )
            {
                GarmentList obj = new GarmentList()
                {
                    OrderID = Key.OrderID,
                    Article = Key.Article,
                    SizeCode = Key.SizeCode,
                };

                obj.Panels = new List<Panel>();
                var detail = tmpDt.AsEnumerable()
                    .Where(o => o["OrderID"].ToString() == Key.OrderID
                        && o["Article"].ToString() == Key.Article
                        && o["SizeCode"].ToString() == Key.SizeCode)
                    .Select(o => o["PatternPanel"].ToString()).Distinct().ToList();

                // Panel
                foreach (var PatternPanel in detail)
                {
                    Panel panel = new Panel() { PatternPanel = PatternPanel };
                    List<DataRow> FabricPanelCodes = tmpDt.AsEnumerable()
                     .Where(o => o["OrderID"].ToString() == Key.OrderID
                         && o["Article"].ToString() == Key.Article
                         && o["SizeCode"].ToString() == Key.SizeCode
                         && o["PatternPanel"].ToString() == PatternPanel).ToList();
                    panel.FabricPanelCodes = new List<PanelCode>();
                    foreach (var row in FabricPanelCodes)
                    {
                        PanelCode code = new PanelCode() { FabricPanelCode = row["FabricPanelCode"].ToString() };
                        panel.FabricPanelCodes.Add(code);
                    }
                    obj.Panels.Add(panel);
                }

                GarmentListList.Add(obj);
            }
            #endregion

            // 取得所有部位Cutting 數量
            tmpCmd = $@"

SELECT  [EstCutDate]=(SELECT EstCutDate FROM Cutplan WHERE ID = CD.ID)
,WOD.OrderID
,WOD.Article 
,WOD.SizeCode
,wo.FabricCombo
,wo.FabricPanelCode
,[Qty]=SUM(WOD.Qty)
FROM WorkOrder_Distribute WOD
INNER JOIN WorkOrder WO ON WO.Ukey = WOD.WorkOrderUkey
INNER JOIN Cutplan_Detail CD ON CD.WorkorderUkey = WO.Ukey
WHERE WOD.OrderID IN ('{OrderIDs.JoinToString("','")}')
GROUP BY CD.ID
		,WOD.OrderID
		,WOD.Article 
		,WOD.SizeCode
		,wo.FabricCombo
		,wo.FabricPanelCode
--ORDER BY WOD.OrderID,WOD.Article,wo.fabricCombo,wo.FabricPanelCode,WOD.SizeCode
";

            result = DBProxy.Current.Select(null, tmpCmd, out tmpDt);

            foreach (var garment in GarmentListList)
            {
                foreach (var panel in garment.Panels)
                {
                    foreach (var fabricPanelCode in panel.FabricPanelCodes)
                    {
                        var exists = tmpDt.AsEnumerable().Where(o =>
                        o["OrderID"].ToString() == garment.OrderID &&
                        o["Article"].ToString() == garment.Article &&
                        o["SizeCode"].ToString() == garment.SizeCode &&
                        o["FabricCombo"].ToString() == panel.PatternPanel &&
                        o["FabricPanelCode"].ToString() == fabricPanelCode.FabricPanelCode);

                        // 任何一個部位不存在，則記錄下來
                        if (!exists.Any())
                        {
                            garment.IsPanelShortage = true;
                            //garment.EstCutDate = null;
                            //garment.EstCutDate = Convert.ToDateTime(exists.FirstOrDefault()["EstCutDate"]);
                            fabricPanelCode.Qty = 0;
                        }
                        else
                        {
                            garment.IsPanelShortage = false;
                            garment.EstCutDate = Convert.ToDateTime(exists.FirstOrDefault()["EstCutDate"]);
                            fabricPanelCode.Qty = Convert.ToInt32(exists.FirstOrDefault()["Qty"]);
                        }
                    }
                }
            }

            // 移除缺少部位不成套的的
            GarmentListList.RemoveAll(o => o.IsPanelShortage);


            //int CutQty = GarmentListList.Sum(o => o.Panels.Sum(x => x.FabricPanelCodes.Min(y => y.Qty)));

            return GarmentListList;
        }


        /// <summary>
        /// 一件成衣，由哪些部位組成
        /// </summary>
        public class GarmentList
        {
            public DateTime EstCutDate { get; set; }
            // 是否缺部位，因此不成套
            public bool IsPanelShortage { get; set; }
            public string OrderID { get; set; }
            public string Article { get; set; }
            public string SizeCode { get; set; }
            public List<Panel> Panels { get; set; }
        }

        /// <summary>
        /// 大部位名
        /// </summary>
        public class Panel
        {
            /// <summary>
            /// 大部位
            /// </summary>
            public string PatternPanel { get; set; }

            /// <summary>
            /// 該大部位內的小部位
            /// </summary>
            public List<PanelCode> FabricPanelCodes { get; set; }
        }
        public class PanelCode
        {
            public string FabricPanelCode { get; set; }
            public int Qty { get; set; }
        }
    }

}