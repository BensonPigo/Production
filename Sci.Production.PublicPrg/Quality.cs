using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Sci.Production.PublicPrg
{
    /// <summary>
    /// Prgs
    /// </summary>
    public static partial class Prgs
    {
        #region Query QA Inspection header function QA 的表頭抓取共用程式

        /// <summary>
        /// 可取出Style,Season,Brand,Cutinline,earliestSciDelivery,MtlLeadTime from Orders,System
        /// </summary>
        /// <param name="poid">poid</param>
        /// <param name="orderDr">orderDr</param>
        /// <returns>DataRow</returns>
        public static DualResult QueryQaInspectionHeader(string poid, out DataRow orderDr)
        {
            string query = string.Format("Select distinct a.styleid, a.seasonid,a.brandid,a.cutinline,a.category from Orders a WITH (NOLOCK) Where a.poid ='{0}'", poid);
            DualResult dResult = DBProxy.Current.Select(null, query, out DataTable queryTb);
            if (dResult && queryTb.Rows.Count > 0)
            {
                orderDr = queryTb.Rows[0];
            }
            else
            {
                orderDr = null;
            }

            return dResult;
        }
        #endregion;
        #region 找QA 的Target Lead Time (SCIDelivery -System.MtlLeadTime)

        /// <summary>
        /// 找QA 的Target Lead Time (比較Cutinline跟SciDelv-system.MtlLeadTime找出比較小的日期)
        /// </summary>
        /// <param name="cu">cu</param>
        /// <param name="del">del</param>
        /// <returns>DateTime</returns>
        public static DateTime? GetTargetLeadTime(object cu, object del)
        {
            DateTime? cutinline, sciDelv;

            if (cu == DBNull.Value || cu.Empty())
            {
                cutinline = null;
            }
            else
            {
                cutinline = Convert.ToDateTime(cu);
            }

            if (del == DBNull.Value)
            {
                sciDelv = null;
            }
            else
            {
                sciDelv = Convert.ToDateTime(del);
            }

            DateTime? targetSciDel;
            double mtlLeadT = Convert.ToDouble(MyUtility.GetValue.Lookup("Select MtlLeadTime from System WITH (NOLOCK) ", null));
            if (sciDelv == null)
            {
                return null;
            }

            if (MyUtility.Check.Empty(mtlLeadT))
            {
                targetSciDel = sciDelv;
            }
            else
            {
                targetSciDel = ((DateTime)sciDelv).AddDays(Convert.ToDouble(mtlLeadT));
            }

            if (cutinline < targetSciDel)
            {
                return cutinline;
            }
            else
            {
                return targetSciDel;
            }
        }
        #endregion;
        #region 判斷Physical OverallResult, Status

        /// <summary>
        /// 判斷並回寫Physical OverallResult, Status string[0]=Result, string[1]=status
        /// </summary>
        /// <param name ="maindr">maindr</param>
        /// <returns>string[]</returns>
        public static string[] GetOverallResult_Status(DataRow maindr)
        {
            string status = "New";

            string allResult;
            #region 新改的邏輯

            // 判斷Result是Pass的唯一狀況
            if (
                (maindr["Physical"].ToString() == "Pass" || MyUtility.Convert.GetBool(maindr["Nonphysical"])) &&
                (maindr["Weight"].ToString() == "Pass" || MyUtility.Convert.GetBool(maindr["NonWeight"])) &&
                (maindr["ShadeBond"].ToString() == "Pass" || MyUtility.Convert.GetBool(maindr["NonShadeBond"])) &&
                (maindr["Continuity"].ToString() == "Pass" || MyUtility.Convert.GetBool(maindr["NonContinuity"])) &&
                (maindr["Odor"].ToString() == "Pass" || MyUtility.Convert.GetBool(maindr["NonOdor"])) &&
                (maindr["Moisture"].ToString() == "Pass" || MyUtility.Convert.GetBool(maindr["NonMoisture"])))
            {
                allResult = "Pass";
                status = "Confirmed";
            }

            // 判斷Result 是空值
            else if (
                (MyUtility.Check.Empty(maindr["Physical"]) && !MyUtility.Convert.GetBool(maindr["Nonphysical"])) ||
                (MyUtility.Check.Empty(maindr["Weight"]) && !MyUtility.Convert.GetBool(maindr["NonWeight"])) ||
                (MyUtility.Check.Empty(maindr["ShadeBond"]) && !MyUtility.Convert.GetBool(maindr["NonShadeBond"])) ||
                (MyUtility.Check.Empty(maindr["Continuity"]) && !MyUtility.Convert.GetBool(maindr["NonContinuity"])) ||
                (MyUtility.Check.Empty(maindr["Odor"]) && !MyUtility.Convert.GetBool(maindr["NonOdor"])) ||
                (MyUtility.Check.Empty(maindr["Moisture"]) && !MyUtility.Convert.GetBool(maindr["NonMoisture"])))
            {
                allResult = string.Empty;
            }
            else
            {
                allResult = "Fail";
            }
            #endregion

            string[] re_str = { allResult, status };
            return re_str;
        }
        #endregion
        #region 判斷FIR_Laboratory OverallResult

        /// <summary>
        /// 判斷並回寫FIR_Laboratory OverallResult, string[0]=Result
        /// </summary>
        /// <param name ="fir_id">fir_id</param>
        /// <returns>string[]</returns>
        public static string[] GetOverallResult_Lab(object fir_id)
        {
            MyUtility.Check.Seek(string.Format("Select * from FIR_Laboratory WITH (NOLOCK) Where id={0}", fir_id), out DataRow maindr);
            string allResult = string.Empty;

            // 當(FIR_Laboratory.Crocking 有值或FIR_Laboratory.nonCrocking=T) 且(FIR_Laboratory.Wash有值或FIR_Laboratory.nonWash=T)且(FIR_Laboratory.Heat或FIR_Laboratory.nonHeat=T) 才回寫FIR_Laboratory.Result，只要其中一個FIR_Laboratory.Crocking, FIR_Laboratory.Wash, FIR_Laboratory.Heat 的值為’F’，Fir.Result 就回寫’F’
            if ((!MyUtility.Check.Empty(maindr["Crocking"]) || MyUtility.Convert.GetBool(maindr["nonCrocking"]))
                && (!MyUtility.Check.Empty(maindr["Wash"]) || MyUtility.Convert.GetBool(maindr["nonWash"]))
                && (!MyUtility.Check.Empty(maindr["Heat"]) || MyUtility.Convert.GetBool(maindr["nonHeat"])))
            {
                if (maindr["Crocking"].ToString() == "Fail" ||
                    maindr["Wash"].ToString() == "Fail" ||
                    maindr["Heat"].ToString() == "Fail")
                {
                    allResult = "Fail";
                }
                else
                {
                    allResult = "Pass";
                }
            }

            string[] re_str = { allResult };
            return re_str;
        }
        #endregion

        /// <summary>
        /// defect 形式: 空白, A1, AA4
        /// 最後一碼數字最大為 4 所以只會有 1 碼
        /// default type = 0 排除數字, 取 defectID
        /// type = 1 排除字母取 point
        /// </summary>
        /// <inheritdoc/>
        public static string SplitDefectNum(string defectcode, int type = 0)
        {
            if (type == 0)
            {
                return Regex.Replace(defectcode, @"[\d]", string.Empty);
            }
            else
            {
                return Regex.Replace(defectcode, @"[A-Za-z]", string.Empty);
            }
        }

        /// <summary>
        /// Double Click後將Result替換成相反結果(Pass<=>Fail)
        /// </summary>
        /// <returns></returns>
        public class CellResult : DataGridViewGeneratorTextColumnSettings
        {
            /// <summary>
            /// GetGridCell
            /// </summary>
            /// <returns>CellResult</returns>
            public static DataGridViewGeneratorTextColumnSettings GetGridCell()
            {
                CellResult result = new CellResult();
                result.CellMouseDoubleClick += (s, e) =>
                {
                    if (e.RowIndex == -1)
                    {
                        return;
                    }

                    DataGridView grid = ((DataGridViewColumn)s).DataGridView;
                    if (!((Win.Forms.Base)grid.FindForm()).EditMode)
                    {
                        return;
                    }

                    DataRow dr = grid.GetDataRow(e.RowIndex);
                    if (dr["Result"].ToString().ToUpper() == "PASS")
                    {
                        dr["Result"] = "Fail";
                    }
                    else
                    {
                        dr["Result"] = "Pass";
                    }
                };
                return result;
            }
        }

        /// <summary>
        /// GarmentTest_Detail_FGWT 和 SampleGarmentTest_Detail_FGWT的欄位物件
        /// </summary>
        public class FGWT
        {
            /// <summary>
            /// Seq
            /// </summary>
            public int Seq { get; set; }

            /// <summary>
            /// Location
            /// </summary>
            public string Location { get; set; }

            /// <summary>
            /// Type
            /// </summary>
            public string Type { get; set; }

            /// <summary>
            /// SystemType
            /// </summary>
            public string SystemType { get; set; }

            /// <summary>
            /// Scale
            /// </summary>
            public string Scale { get; set; }

            /// <summary>
            /// TestDetail
            /// </summary>
            public string TestDetail { get; set; }

            /// <summary>
            /// Criteria_Min
            /// </summary>
            public double Criteria { get; set; }

            /// <summary>
            /// Criteria_Max
            /// </summary>
            public double Criteria2 { get; set; }
        }

        /// <summary>
        /// GarmentTest_Detail_FGPT 和 SampleGarmentTest_Detail_FGPT的欄位物件
        /// </summary>
        public class FGPT
        {
            /// <summary>
            /// Location
            /// </summary>
            public string Location { get; set; }

            /// <summary>
            /// TypeSelectionVersionID
            /// </summary>
            public int TypeSelectionVersionID { get; set; }

            /// <summary>
            /// TypeSelection_Seq
            /// </summary>
            public int TypeSelection_Seq { get; set; }

            /// <summary>
            /// Type
            /// </summary>
            public string Type { get; set; }

            /// <summary>
            /// Scale
            /// </summary>
            public string Scale { get; set; }

            /// <summary>
            /// TestDetail
            /// </summary>
            public string TestDetail { get; set; }

            /// <summary>
            /// Criteria
            /// </summary>
            public double Criteria { get; set; }

            /// <summary>
            /// TestUnit
            /// </summary>
            public string TestUnit { get; set; }

            /// <summary>
            /// TestName
            /// </summary>
            public string TestName { get; set; }

            /// <summary>
            /// Seq
            /// </summary>
            public int Seq { get; set; }
        }

        /// <summary>
        /// 取得預設FGWT
        /// </summary>
        /// <param name="isTop">是否TOP</param>
        /// <param name="isBottom">>是否Bottom</param>
        /// <param name="isTop_Bottom">>是否TOP & Bottom</param>
        /// <param name="mtlTypeID">mtlTypeID</param>
        /// <param name="washType">washType</param>
        /// <param name="fibresType">fibresType</param>
        /// <param name="isAll">>是否All</param>
        /// <returns>預設清單</returns>
        public static List<FGWT> GetDefaultFGWT(bool isTop, bool isBottom, bool isTop_Bottom, string mtlTypeID, string washType, string fibresType, bool isAll = true)
        {
            string sqlWhere = string.Empty;

            if (!MyUtility.Check.Empty(mtlTypeID))
            {
                sqlWhere += $" and MtlTypeID = '{mtlTypeID}' ";
            }

            if (mtlTypeID == "KNIT")
            {
                if (!MyUtility.Check.Empty(washType))
                {
                    string washing = string.Empty;

                    switch (washType)
                    {
                        case "Hand":
                            washing = "HandWash";
                            break;
                        case "Line":
                            washing = "LineDry";
                            break;
                        case "Tumnle":
                            washing = "TumbleDry";
                            break;
                        default:
                            break;
                    }

                    sqlWhere += $" and Washing = '{washing}' ";
                }

                if (!MyUtility.Check.Empty(fibresType))
                {
                    string fabricComposition = string.Empty;

                    switch (fibresType)
                    {
                        case "Natural":
                            fabricComposition = "Above50NaturaFibres";
                            break;
                        case "Synthetic":
                            fabricComposition = "Above50SyntheticFibres";
                            break;
                        default:
                            break;
                    }

                    sqlWhere += $" and FabricComposition = '{fabricComposition}' ";
                }
            }

            if (isAll || isBottom || isTop || isTop_Bottom)
            {
                List<string> listLocation = new List<string>();

                if (isAll)
                {
                    listLocation.Add("''");
                }

                if (isBottom)
                {
                    listLocation.Add("'B'");
                }

                if (isTop)
                {
                    listLocation.Add("'T'");
                }

                if (isTop_Bottom)
                {
                    listLocation.Add("'S'");
                }

                sqlWhere += $" and Location in ({listLocation.JoinToString(",")}) ";
            }

            string sqlGetDefaultFGWT = $@"
select  Seq,
        [Location] = case when Location = 'T' then 'Top'
                          when Location = 'B' then 'Bottom'
                          when Location = 'S' then 'Top+Bottom'
                          else '' end,
        ReportType,
        SystemType,
        Scale,
        TestDetail,
        Criteria,
        Criteria2
from    Adidas_FGWT with (nolock)
where 1 = 1 {sqlWhere}
";

            DualResult result = DBProxy.Current.Select(null, sqlGetDefaultFGWT, out DataTable dtResult);

            if (!result)
            {
                MyUtility.Msg.ErrorBox(result.GetException().ToString());
                return new List<FGWT>();
            }

            List<FGWT> defaultFGWTList = dtResult.AsEnumerable().Select(s => new FGWT
            {
                Seq = MyUtility.Convert.GetInt(s["Seq"]),
                Location = s["Location"].ToString(),
                Type = s["ReportType"].ToString(),
                SystemType = s["SystemType"].ToString(),
                Scale = s["Scale"] == DBNull.Value ? null : s["Scale"].ToString(),
                TestDetail = s["TestDetail"].ToString(),
                Criteria = MyUtility.Convert.GetDouble(s["Criteria"]),
                Criteria2 = MyUtility.Convert.GetDouble(s["Criteria2"]),
            }).ToList();

            return defaultFGWTList;
        }

        /// <summary>
        /// 取得預設FGPT
        /// </summary>
        /// <param name="isTop">isTop</param>
        /// <param name="isBottom">isBottom</param>
        /// <param name="isTop_Bottom">isTop_Bottom</param>
        /// <param name="isRugbyFootBall">isRugbyFootBall</param>
        /// <param name="location">location</param>
        /// <returns>List<FGPT></returns>
        public static List<FGPT> GetDefaultFGPT(bool isTop, bool isBottom, bool isTop_Bottom, bool isRugbyFootBall, string location)
        {
            List<FGPT> defaultFGPTList = new List<FGPT>();

            List<FGPT> upperOnly = new List<FGPT>()
            {
                new FGPT() { Seq = 3, Location = "Top", TestDetail = "mm", TestUnit = "mm", TestName = "PHX-AP0413", Type = "seam slippage: Garment - weft - upper bodywear 150N", Criteria = 4 },
                new FGPT() { Seq = 6, Location = "Top", TestDetail = "mm", TestUnit = "mm", TestName = "PHX-AP0413", Type = "seam slippage: Garment - warp - upper bodywear 150N", Criteria = 4 },
                new FGPT() { Seq = 9, Location = "Top", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No fabric breakage: Garment - weft - upper bodywear 150N", Criteria = 150 },
                new FGPT() { Seq = 12, Location = "Top", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No fabric breakage: Garment - warp - upper bodywear 150N", Criteria = 150 },
                new FGPT() { Seq = 15, Location = "Top", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No seam breakage: Garment - weft - upper bodywear 150N", Criteria = 150 },
                new FGPT() { Seq = 18, Location = "Top", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No seam breakage: Garment - warp - upper bodywear 150N", Criteria = 150 },
                new FGPT() { Seq = 1, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - Upper body wear/ full body wear (Side seam - Method B  ≥180N )", Criteria = 180 },
                new FGPT() { Seq = 2, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - Upper body wear/ full body wear (Armhole seam - Method B ≥180N)", Criteria = 180 },
                new FGPT() { Seq = 3, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - Upper body wear/ full body wear (Under arm seam or sleeve seam - Method B ≥180N )", Criteria = 180 },
                new FGPT() { Seq = 4, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - Upper body wear/ full body wear (Shoulder seam - Method B ≥180N )", Criteria = 180 },
                new FGPT() { Seq = 5, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - Upper body wear/ full body wear (Waistband seam  - Method B ≥180N )", Criteria = 180 },
                new FGPT() { Seq = 6, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - Upper body wear/ full body wear (Hood seam - Method B ≥180N )", Criteria = 180 },
                new FGPT() { Seq = 7, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - Upper body wear/ full body wear ({0}- Method B ≥180N ) Other Joining seam  selection", Criteria = 180, TypeSelectionVersionID = 1, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 8, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - Upper body wear/ full body wear ({0}- Method B ≥180N ) Other Joining seam  selection", Criteria = 180, TypeSelectionVersionID = 1, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 9, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - Upperr body wear/ full body wear ({0}- Method B ≥180N ) Other Joining seam  selection", Criteria = 180, TypeSelectionVersionID = 1, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 19, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - Upper  body wear/ full body wear (Side seam - Method A ≥70N)", Criteria = 70 },
                new FGPT() { Seq = 20, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - Upper body wear/ full body wear (Armhole seam - Method A ≥70N )", Criteria = 70 },
                new FGPT() { Seq = 21, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - Upperr body wear/ full body wear (Under arm seam or sleeve seam - Method A ≥70N )", Criteria = 70 },
                new FGPT() { Seq = 22, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - Upper body wear/ full body wear (Shoulder seam - Method A ≥70N)", Criteria = 70 },
                new FGPT() { Seq = 23, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - Upper body wear/ full body wear (Neck seam - Method A ≥70N )", Criteria = 70 },
                new FGPT() { Seq = 24, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - Upper body wear/ full body wear (Waistband seam - Method A ≥70N )", Criteria = 70 },
                new FGPT() { Seq = 25, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - Upper body wear/ full body wear (Hood seam - Method A ≥70N )", Criteria = 70 },
                new FGPT() { Seq = 26, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - Upper body wear/ full body wear ({0}- Method A ≥70N ) Other Joining seam  selection", Criteria = 70, TypeSelectionVersionID = 1, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 27, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - Upper body wear/ full body wear ({0}- Method A ≥70N ) Other Joining seam  selection", Criteria = 70, TypeSelectionVersionID = 1, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 28, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - Upper body wear/ full body wear ({0}- Method A ≥70N ) Other Joining seam  selection", Criteria = 70, TypeSelectionVersionID = 1, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 29, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - Upper body wear/ full body wear (Side seam - Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 30, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - Upper body wear/ full body wear (Armhole seam - Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 31, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - Upper body wear/ full body wear (Under arm seam or sleeve seam - Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 32, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - Upper body wear/ full body wear (Shoulder seam - Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 33, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - Upper body wear/ full body wear (Waistband seam  - Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 34, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - Upper body wear/ full body wear (Hood seam - Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 35, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - Upper body wear/ full body wear ({0}- Method B ≥140N ) Other Joining seam  selection", Criteria = 140, TypeSelectionVersionID = 1, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 36, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - Upper body wear/ full body wear ({0}- Method B ≥140N ) Other Joining seam  selection", Criteria = 140, TypeSelectionVersionID = 1, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 37, Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - Upperr body wear/ full body wear ({0}- Method B ≥140N ) Other Joining seam  selection", Criteria = 140, TypeSelectionVersionID = 1, TypeSelection_Seq = 1 },
            };

            List<FGPT> lowerOnly = new List<FGPT>()
            {
                new FGPT() { Seq = 1, Location = "Bottom", TestDetail = "mm", TestUnit = "mm", TestName = "PHX-AP0413", Type = "seam slippage: Garment - weft - lower body wear/ full body wear 150N", Criteria = 4 },
                new FGPT() { Seq = 4, Location = "Bottom", TestDetail = "mm", TestUnit = "mm", TestName = "PHX-AP0413", Type = "seam slippage: Garment - warp - lower body wear/ full body wear 150N", Criteria = 4 },
                new FGPT() { Seq = 7, Location = "Bottom", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No fabric breakage: Garment - weft - lower body wear/ full body wear 150N", Criteria = 150 },
                new FGPT() { Seq = 10, Location = "Bottom", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No fabric breakage: Garment - warp - lower body wear/ full body wear 150N", Criteria = 150 },
                new FGPT() { Seq = 13, Location = "Bottom", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No seam breakage: Garment - weft - lower body wear/ full body wear 150N", Criteria = 150 },
                new FGPT() { Seq = 16, Location = "Bottom", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No seam breakage: Garment - warp - lower body wear/ full body wear 150N", Criteria = 150 },
                new FGPT() { Seq = 10, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - length direction - lower body wear/ full body wear (Back rise- Method B ≥180N )", Criteria = 180 },
                new FGPT() { Seq = 11, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - length direction - lower body wear/ full body wear (Crotch- Method B ≥180N)", Criteria = 180 },
                new FGPT() { Seq = 12, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - lower body wear/ full body wear (Front rise- Method B ≥180N )", Criteria = 180 },
                new FGPT() { Seq = 13, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - lower body wear/ full body wear (Inseam- Method B ≥180N )", Criteria = 180 },
                new FGPT() { Seq = 14, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - lower body wear/ full body wear (Sideseam- Method B ≥180N )", Criteria = 180 },
                new FGPT() { Seq = 15, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - lower body wear/ full body wear (Waistband- Method B ≥180N )", Criteria = 180 },
                new FGPT() { Seq = 16, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - lower body wear/ full body wear ({0}- Method B ≥180N ) Other Joining seam  selection", Criteria = 180, TypeSelectionVersionID = 2, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 17, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - lower body wear/ full body wear ({0}- Method B ≥180N ) Other Joining seam  selection", Criteria = 180, TypeSelectionVersionID = 2, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 18, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - lower body wear/ full body wear ({0}- Method B ≥180N ) Other Joining seam  selection", Criteria = 180, TypeSelectionVersionID = 2, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 38, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear (Back rise- Method A ≥70N  )", Criteria = 70 },
                new FGPT() { Seq = 39, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear (Front rise- Method A ≥70N  )", Criteria = 70 },
                new FGPT() { Seq = 40, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear (Inseam- Method A ≥70N  )", Criteria = 70 },
                new FGPT() { Seq = 41, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear (Waistband- Method A ≥70N  )", Criteria = 70 },
                new FGPT() { Seq = 42, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear (Sideseam- Method A ≥70N  )", Criteria = 70 },
                new FGPT() { Seq = 43, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear ({0}- Method A  ≥70N ) Other Joining seam  selection", Criteria = 70, TypeSelectionVersionID = 2, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 44, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear ({0}- Method A ≥70N ) Other Joining seam  selection", Criteria = 70, TypeSelectionVersionID = 2, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 45, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear ({0}- Method A  ≥70N  ) Other Joining seam  selection", Criteria = 70, TypeSelectionVersionID = 2, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 46, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - lower body wear/ full body wear (Front rise- Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 47, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear (Back rise- Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 48, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear (Crotch- Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 49, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - lower body wear/ full body wear (Inseam- Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 50, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - lower body wear/ full body wear (Sideseam- Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 51, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - lower body wear/ full body wear (Waistband- Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 52, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - lower body wear/ full body wear ({0}- Method B ≥140N ) Other Joining seam  selection", Criteria = 140, TypeSelectionVersionID = 2, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 53, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - lower body wear/ full body wear ({0}- Method B ≥140N ) Other Joining seam  selection", Criteria = 140, TypeSelectionVersionID = 2, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 54, Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - lower body wear/ full body wear ({0}- Method B ≥140N ) Other Joining seam  selection", Criteria = 140, TypeSelectionVersionID = 2, TypeSelection_Seq = 1 },
            };

            List<FGPT> fullBody = new List<FGPT>()
            {
                new FGPT() { Seq = 1, Location = "Full", TestDetail = "mm", TestUnit = "mm", TestName = "PHX-AP0413", Type = "seam slippage: Garment - weft - lower body wear/ full body wear 150N", Criteria = 4 },
                new FGPT() { Seq = 4, Location = "Full", TestDetail = "mm", TestUnit = "mm", TestName = "PHX-AP0413", Type = "seam slippage: Garment - warp - lower body wear/ full body wear 150N", Criteria = 4 },
                new FGPT() { Seq = 7, Location = "Full", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No fabric breakage: Garment - weft - lower body wear/ full body wear 150N", Criteria = 150 },
                new FGPT() { Seq = 10, Location = "Full", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No fabric breakage: Garment - warp - lower body wear/ full body wear 150N", Criteria = 150 },
                new FGPT() { Seq = 13, Location = "Full", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No seam breakage: Garment - weft - lower body wear/ full body wear 150N", Criteria = 150 },
                new FGPT() { Seq = 16, Location = "Full", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No seam breakage: Garment - warp - lower body wear/ full body wear 150N", Criteria = 150 },
                new FGPT() { Seq = 1, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - Upper body wear/ full body wear (Side seam - Method B  ≥180N )", Criteria = 180 },
                new FGPT() { Seq = 2, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - Upper body wear/ full body wear (Armhole seam - Method B ≥180N)", Criteria = 180 },
                new FGPT() { Seq = 3, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - Upper body wear/ full body wear (Under arm seam or sleeve seam - Method B ≥180N )", Criteria = 180 },
                new FGPT() { Seq = 4, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - Upper body wear/ full body wear (Shoulder seam - Method B ≥180N )", Criteria = 180 },
                new FGPT() { Seq = 5, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - Upper body wear/ full body wear (Waistband seam  - Method B ≥180N )", Criteria = 180 },
                new FGPT() { Seq = 6, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - Upper body wear/ full body wear (Hood seam - Method B ≥180N )", Criteria = 180 },
                new FGPT() { Seq = 7, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - Upper body wear/ full body wear ({0}- Method B ≥180N ) Other Joining seam  selection", Criteria = 180, TypeSelectionVersionID = 1, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 8, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - Upper body wear/ full body wear ({0}- Method B ≥180N ) Other Joining seam  selection", Criteria = 180, TypeSelectionVersionID = 1, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 9, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - Upperr body wear/ full body wear ({0}- Method B ≥180N ) Other Joining seam  selection", Criteria = 180, TypeSelectionVersionID = 1, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 10, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - length direction - lower body wear/ full body wear (Back rise- Method B ≥180N )", Criteria = 180 },
                new FGPT() { Seq = 11, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - length direction - lower body wear/ full body wear (Crotch- Method B ≥180N)", Criteria = 180 },
                new FGPT() { Seq = 12, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - lower body wear/ full body wear (Front rise- Method B ≥180N )", Criteria = 180 },
                new FGPT() { Seq = 13, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - lower body wear/ full body wear (Inseam- Method B ≥180N )", Criteria = 180 },
                new FGPT() { Seq = 14, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - lower body wear/ full body wear (Sideseam- Method B ≥180N )", Criteria = 180 },
                new FGPT() { Seq = 15, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - lower body wear/ full body wear (Waistband- Method B ≥180N )", Criteria = 180 },
                new FGPT() { Seq = 16, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - lower body wear/ full body wear ({0}- Method B ≥180N ) Other Joining seam  selection", Criteria = 180, TypeSelectionVersionID = 2, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 17, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - lower body wear/ full body wear ({0}- Method B ≥180N ) Other Joining seam  selection", Criteria = 180, TypeSelectionVersionID = 2, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 18, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - lower body wear/ full body wear ({0}- Method B ≥180N ) Other Joining seam  selection", Criteria = 180, TypeSelectionVersionID = 2, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 19, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - Upper  body wear/ full body wear (Side seam - Method A ≥70N)", Criteria = 70 },
                new FGPT() { Seq = 20, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - Upper body wear/ full body wear (Armhole seam - Method A ≥70N )", Criteria = 70 },
                new FGPT() { Seq = 21, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - Upperr body wear/ full body wear (Under arm seam or sleeve seam - Method A ≥70N )", Criteria = 70 },
                new FGPT() { Seq = 22, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - Upper body wear/ full body wear (Shoulder seam - Method A ≥70N)", Criteria = 70 },
                new FGPT() { Seq = 23, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - Upper body wear/ full body wear (Neck seam - Method A ≥70N )", Criteria = 70 },
                new FGPT() { Seq = 24, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - Upper body wear/ full body wear (Waistband seam - Method A ≥70N )", Criteria = 70 },
                new FGPT() { Seq = 25, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - Upper body wear/ full body wear (Hood seam - Method A ≥70N )", Criteria = 70 },
                new FGPT() { Seq = 26, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - Upper body wear/ full body wear ({0}- Method A ≥70N ) Other Joining seam  selection", Criteria = 70, TypeSelectionVersionID = 1, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 27, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - Upper body wear/ full body wear ({0}- Method A ≥70N ) Other Joining seam  selection", Criteria = 70, TypeSelectionVersionID = 1, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 28, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - Upper body wear/ full body wear ({0}- Method A ≥70N ) Other Joining seam  selection", Criteria = 70, TypeSelectionVersionID = 1, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 29, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - Upper body wear/ full body wear (Side seam - Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 30, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - Upper body wear/ full body wear (Armhole seam - Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 31, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - Upper body wear/ full body wear (Under arm seam or sleeve seam - Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 32, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - Upper body wear/ full body wear (Shoulder seam - Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 33, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - Upper body wear/ full body wear (Waistband seam  - Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 34, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - Upper body wear/ full body wear (Hood seam - Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 35, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - Upper body wear/ full body wear ({0}- Method B ≥140N ) Other Joining seam  selection", Criteria = 140, TypeSelectionVersionID = 1, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 36, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - Upper body wear/ full body wear ({0}- Method B ≥140N ) Other Joining seam  selection", Criteria = 140, TypeSelectionVersionID = 1, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 37, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - Upperr body wear/ full body wear ({0}- Method B ≥140N ) Other Joining seam  selection", Criteria = 140, TypeSelectionVersionID = 1, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 38, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear (Back rise- Method A ≥70N  )", Criteria = 70 },
                new FGPT() { Seq = 39, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear (Front rise- Method A ≥70N  )", Criteria = 70 },
                new FGPT() { Seq = 40, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear (Inseam- Method A ≥70N  )", Criteria = 70 },
                new FGPT() { Seq = 41, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear (Waistband- Method A ≥70N  )", Criteria = 70 },
                new FGPT() { Seq = 42, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear (Sideseam- Method A ≥70N  )", Criteria = 70 },
                new FGPT() { Seq = 43, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear ({0}- Method A  ≥70N ) Other Joining seam  selection", Criteria = 70, TypeSelectionVersionID = 2, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 44, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear ({0}- Method A ≥70N ) Other Joining seam  selection", Criteria = 70, TypeSelectionVersionID = 2, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 45, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear ({0}- Method A  ≥70N  ) Other Joining seam  selection", Criteria = 70, TypeSelectionVersionID = 2, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 46, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - lower body wear/ full body wear (Front rise- Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 47, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear (Back rise- Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 48, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear (Crotch- Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 49, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - lower body wear/ full body wear (Inseam- Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 50, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - lower body wear/ full body wear (Sideseam- Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 51, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - lower body wear/ full body wear (Waistband- Method B ≥140N )", Criteria = 140 },
                new FGPT() { Seq = 52, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - lower body wear/ full body wear ({0}- Method B ≥140N ) Other Joining seam  selection", Criteria = 140, TypeSelectionVersionID = 2, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 53, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - lower body wear/ full body wear ({0}- Method B ≥140N ) Other Joining seam  selection", Criteria = 140, TypeSelectionVersionID = 2, TypeSelection_Seq = 1 },
                new FGPT() { Seq = 54, Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - lower body wear/ full body wear ({0}- Method B ≥140N ) Other Joining seam  selection", Criteria = 140, TypeSelectionVersionID = 2, TypeSelection_Seq = 1 },
            };

            List<FGPT> rugby_FootBall = new List<FGPT>()
            {
                new FGPT() { Seq = 2, Location = "Football Style", TestDetail = "mm", TestUnit = "mm", TestName = "PHX-AP0413", Type = "seam slippage: Garment - weft - Rugby/Football 160N", Criteria = 4 },
                new FGPT() { Seq = 5, Location = "Football Style", TestDetail = "mm", TestUnit = "mm", TestName = "PHX-AP0413", Type = "seam slippage: Garment - warp - Rugby/Football160N", Criteria = 4 },
                new FGPT() { Seq = 8, Location = "Football Style", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No fabric breakage: Garment - weft - Rugby/Football 160N", Criteria = 160 },
                new FGPT() { Seq = 11, Location = "Football Style", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No fabric breakage: Garment - warp - Rugby/Football 160N", Criteria = 160 },
                new FGPT() { Seq = 14, Location = "Football Style", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No seam breakage: Garment - weft -Rugby/Football 160N", Criteria = 160 },
                new FGPT() { Seq = 17, Location = "Football Style", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No seam breakage: Garment - warp - Rugby/Football 160N", Criteria = 160 },
            };

            switch (location)
            {
                case "T":
                    defaultFGPTList.AddRange(upperOnly);
                    break;
                case "B":
                    defaultFGPTList.AddRange(lowerOnly);
                    break;
                case "S":
                    defaultFGPTList.AddRange(fullBody);
                    break;
            }

            if (isRugbyFootBall)
            {
                foreach (var fGPT in rugby_FootBall)
                {
                    fGPT.Location = location;
                }

                defaultFGPTList.AddRange(rugby_FootBall);
            }

            defaultFGPTList.Add(new FGPT() { Seq = 1, Location = string.Empty, Type = "odour: Garment", TestDetail = "pass/fail", TestUnit = "pass/Fail", TestName = "PHX-AP0451", });

            return defaultFGPTList.OrderBy(o => o.Type).ToList();
        }
    }
}