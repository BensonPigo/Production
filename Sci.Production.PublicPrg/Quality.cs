using Ict;
using Ict.Win;
using Sci.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
            string allResult = string.Empty;
            string status = "New";

            #region 新改的邏輯

            // 判斷Result是Pass的唯一狀況
            if (
                (maindr["Physical"].ToString() == "Pass" || MyUtility.Convert.GetBool(maindr["Nonphysical"])) &&
                (maindr["Weight"].ToString() == "Pass" || MyUtility.Convert.GetBool(maindr["NonWeight"])) &&
                (maindr["ShadeBond"].ToString() == "Pass" || MyUtility.Convert.GetBool(maindr["NonShadeBond"])) &&
                (maindr["Continuity"].ToString() == "Pass" || MyUtility.Convert.GetBool(maindr["NonContinuity"])) &&
                (maindr["Odor"].ToString() == "Pass" || MyUtility.Convert.GetBool(maindr["NonOdor"])))
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
                (MyUtility.Check.Empty(maindr["Odor"]) && !MyUtility.Convert.GetBool(maindr["NonOdor"])))
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

            DataTable dtResult;
            DualResult result = DBProxy.Current.Select(null, sqlGetDefaultFGWT, out dtResult);

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
        /// <param name="isLining">isLining</param>
        /// <param name="location">location</param>
        /// <returns>List<FGPT></returns>
        public static List<FGPT> GetDefaultFGPT(bool isTop, bool isBottom, bool isTop_Bottom, bool isRugbyFootBall, bool isLining, string location)
        {
            List<FGPT> defaultFGPTList = new List<FGPT>();

            int liningCriteria = 130;
            int lowerFullBodywearCriteria = 250;
            int lowerFullBodywearCriteriaPHXAP0450 = 140;
            int upperbodywearCriteriaPHXAP0450 = 140;
            int liningCriteriaPHXAP0450 = 80;

            /*除了以上兩種情況，都以Scale欄位作為標準，因此不寫入Criteria欄位視作百分比欄位*/

            List<FGPT> upperOnly = new List<FGPT>()
            {
                new FGPT() { Location = "Top", TestDetail = "mm", TestUnit = "mm", TestName = "PHX-AP0413", Type = "seam slippage: Garment - weft - upper bodywear 150N", Criteria = 4 },
                new FGPT() { Location = "Top", TestDetail = "mm", TestUnit = "mm", TestName = "PHX-AP0413", Type = "seam slippage: Garment - warp - upper bodywear 150N", Criteria = 4 },
                new FGPT() { Location = "Top", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No fabric breakage: Garment - weft - upper bodywear 150N", Criteria = 150 },
                new FGPT() { Location = "Top", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No fabric breakage: Garment - warp - upper bodywear 150N", Criteria = 150 },
                new FGPT() { Location = "Top", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No seam breakage: Garment - weft - upper bodywear 150N", Criteria = 150 },
                new FGPT() { Location = "Top", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No seam breakage: Garment - warp - upper bodywear 150N", Criteria = 150 },
                new FGPT() { Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - upper body wear(Shoulder)", Criteria = upperbodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - upper body wear(Sleeve)", Criteria = upperbodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - upper body wear(Side seam)", Criteria = upperbodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - upper body wear(Front seam)", Criteria = upperbodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - upper body wear(Waistband)", Criteria = upperbodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - upper body wear", Criteria = upperbodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - length direction - upper body wear", Criteria = upperbodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - upper body wear", Criteria = upperbodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - upper bodywear", Criteria = upperbodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - length direction - upper bodywear", Criteria = upperbodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - upper bodywear", Criteria = upperbodywearCriteriaPHXAP0450 },
            };

            List<FGPT> lowerOnly = new List<FGPT>()
            {
                new FGPT() { Location = "Bottom", TestDetail = "mm", TestUnit = "mm", TestName = "PHX-AP0413", Type = "seam slippage: Garment - weft - lower body wear/ full body wear 160N", Criteria = 4 },
                new FGPT() { Location = "Bottom", TestDetail = "mm", TestUnit = "mm", TestName = "PHX-AP0413", Type = "seam slippage: Garment - warp - lower body wear/ full body wear 160N", Criteria = 4 },
                new FGPT() { Location = "Bottom", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No fabric breakage: Garment - weft - lower body wear/ full body wear 160N", Criteria = 160 },
                new FGPT() { Location = "Bottom", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No fabric breakage: Garment - warp - lower body wear/ full body wear 160N", Criteria = 160 },
                new FGPT() { Location = "Bottom", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No seam breakage: Garment - weft - lower body wear/ full body wear 160N", Criteria = 160 },
                new FGPT() { Location = "Bottom", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No seam breakage: Garment - warp - lower body wear/ full body wear 160N", Criteria = 160 },
                new FGPT() { Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450",  Type = "seam breakage: Garment - length direction - lower body wear/ full body wear(Front rise)", Criteria = lowerFullBodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450",  Type = "seam breakage: Garment - length direction - lower body wear/ full body wear(Back rise)", Criteria = lowerFullBodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450",  Type = "seam breakage: Garment - length direction - lower body wear/ full body wear(inseam)", Criteria = lowerFullBodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450",  Type = "seam breakage: Garment - length direction - lower body wear/ full body wear(sideseam)", Criteria = lowerFullBodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450",  Type = "seam breakage: Garment - length direction - lower body wear/ full body wear(seam at front)", Criteria = lowerFullBodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - lower body wear/ full body wear", Criteria = lowerFullBodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - length direction - lower body wear/ full body wear", Criteria = lowerFullBodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - lower body wear/ full body wear", Criteria = lowerFullBodywearCriteriaPHXAP0450 },
            };

            List<FGPT> fullBody = new List<FGPT>()
            {
                new FGPT() { Location = "Full", TestDetail = "mm", TestUnit = "mm", TestName = "PHX-AP0413", Type = "seam slippage: Garment - weft - lower body wear/ full body wear 160N", Criteria = 4 },
                new FGPT() { Location = "Full", TestDetail = "mm", TestUnit = "mm", TestName = "PHX-AP0413", Type = "seam slippage: Garment - warp - lower body wear/ full body wear 160N", Criteria = 4 },
                new FGPT() { Location = "Full", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No fabric breakage: Garment - weft - lower body wear/ full body wear 160N", Criteria = lowerFullBodywearCriteria },
                new FGPT() { Location = "Full", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No fabric breakage: Garment - warp - lower body wear/ full body wear 160N", Criteria = 160 },
                new FGPT() { Location = "Full", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No seam breakage: Garment - weft - lower body wear/ full body wear 160N", Criteria = 160 },
                new FGPT() { Location = "Full", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No seam breakage: Garment - warp - lower body wear/ full body wear 160N", Criteria = 160 },
                new FGPT() { Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - lower body wear/ full body wear", Criteria = lowerFullBodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear(Front rise)", Criteria = lowerFullBodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear(Back rise)", Criteria = lowerFullBodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear(inseam)", Criteria = lowerFullBodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear(sideseam)", Criteria = lowerFullBodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lower body wear/ full body wear(seam at front)", Criteria = lowerFullBodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - length direction - lower body wear/ full body wear", Criteria = lowerFullBodywearCriteriaPHXAP0450 },
                new FGPT() { Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - lower body wear/ full body wear", Criteria = lowerFullBodywearCriteriaPHXAP0450 },
            };

            List<FGPT> lining_Upper = new List<FGPT>()
            {
                new FGPT() { Location = "Top", TestDetail = "mm", TestUnit = "mm", TestName = "PHX-AP0413", Type = "seam slippage: Garment - weft - lining of upper bodywear 130N", Criteria = 4 },
                new FGPT() { Location = "Top", TestDetail = "mm", TestUnit = "mm", TestName = "PHX-AP0413", Type = "seam slippage: Garment - warp - lining of upper bodywear 130N", Criteria = 4 },
                new FGPT() { Location = "Top", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No fabric breakage: Garment - weft - lining of upper bodywear 130N", Criteria = 130 },
                new FGPT() { Location = "Top", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No fabric breakage: Garment - warp - lining of upper bodywear 130N", Criteria = 130 },
                new FGPT() { Location = "Top", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No seam breakage: Garment - weft - lining of upper bodywear 130N", Criteria = 130 },
                new FGPT() { Location = "Top", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No seam breakage: Garment - warp - lining of upper bodywear 130N", Criteria = 130 },
                new FGPT() { Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lining upper body wear", Criteria = liningCriteriaPHXAP0450 },
                new FGPT() { Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lining of upper bodywear", Criteria = liningCriteriaPHXAP0450 },
                new FGPT() { Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - lining upper body wear", Criteria = liningCriteriaPHXAP0450 },
                new FGPT() { Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - lining of upper bodywear", Criteria = liningCriteriaPHXAP0450 },
                new FGPT() { Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - length direction - lining upper body wear", Criteria = liningCriteriaPHXAP0450 },
                new FGPT() { Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - length direction - lining of upper bodywear", Criteria = liningCriteriaPHXAP0450 },
                new FGPT() { Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - lining upper body wear", Criteria = liningCriteriaPHXAP0450 },
                new FGPT() { Location = "Top", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - lining of upper bodywear", Criteria = liningCriteriaPHXAP0450 },
            };

            List<FGPT> lining_Lower = new List<FGPT>()
            {
                new FGPT() { Location = "Bottom", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No fabric breakage: Garment - weft - lining of lower body wear/ full body wear 130N", Criteria = 130 },
                new FGPT() { Location = "Bottom", TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No fabric breakage: Garment - warp - lining of lower body wear/ full body wear 130N", Criteria = 130 },
                new FGPT() { Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0413", Type = "No seam breakage: Garment - warp - lining of lower body wear/ full body wear 130N", Criteria = 130 },
                new FGPT() { Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lining of lower body wear/ full body wear", Criteria = liningCriteriaPHXAP0450 },
                new FGPT() { Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lining of lower body wear/ full body", Criteria = liningCriteriaPHXAP0450 },
                new FGPT() { Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - lining of lower body wear/ full body wear", Criteria = liningCriteriaPHXAP0450 },
                new FGPT() { Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - lining of lower body wear/ full body", Criteria = liningCriteriaPHXAP0450 },
                new FGPT() { Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - length direction - lining of lower body wear/ full body wear", Criteria = liningCriteriaPHXAP0450 },
                new FGPT() { Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - length direction - lining of lower body wear/ full body", Criteria = liningCriteriaPHXAP0450 },
                new FGPT() { Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - lining of lower body wear/ full body wear", Criteria = liningCriteriaPHXAP0450 },
                new FGPT() { Location = "Bottom", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - lining of lower body wear/ full body", Criteria = liningCriteriaPHXAP0450 },
            };

            List<FGPT> lining_Full = new List<FGPT>()
            {
                new FGPT() { Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0413", Type = "No fabric breakage: Garment - weft - lining of lower body wear/ full body wear 130N", Criteria = liningCriteria },
                new FGPT() { Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0413", Type = "No fabric breakage: Garment - warp - lining of lower body wear/ full body wear 130N", Criteria = liningCriteria },
                new FGPT() { Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0413", Type = "No seam breakage: Garment - warp - lining of lower body wear/ full body wear 130N", Criteria = 130 },
                new FGPT() { Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lining of lower body wear/ full body wear", Criteria = liningCriteriaPHXAP0450 },
                new FGPT() { Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - length direction - lining of lower body wear/ full body", Criteria = liningCriteriaPHXAP0450 },
                new FGPT() { Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - lining of lower body wear/ full body wear", Criteria = liningCriteriaPHXAP0450 },
                new FGPT() { Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage: Garment - width direction - lining of lower body wear/ full body", Criteria = liningCriteriaPHXAP0450 },
                new FGPT() { Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - length direction - lining of lower body wear/ full body wear", Criteria = liningCriteriaPHXAP0450 },
                new FGPT() { Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - length direction - lining of lower body wear/ full body", Criteria = liningCriteriaPHXAP0450 },
                new FGPT() { Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - lining of lower body wear/ full body wear", Criteria = liningCriteriaPHXAP0450 },
                new FGPT() { Location = "Full", TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0450", Type = "seam breakage after wash (only for welded/bonded seams): Garment - width direction - lining of lower body wear/ full body", Criteria = liningCriteriaPHXAP0450 },
            };

            List<FGPT> rugby_FootBall = new List<FGPT>()
            {
                new FGPT() { TestDetail = "mm", TestUnit = "mm", TestName = "PHX-AP0413", Type = "seam slippage: Garment - weft - Rugby/Football 160N", Criteria = 4 },
                new FGPT() { TestDetail = "mm", TestUnit = "mm", TestName = "PHX-AP0413", Type = "seam slippage: Garment - warp - Rugby/Football 160N", Criteria = 4 },
                new FGPT() { TestDetail = "N", TestUnit = "N", TestName = "PHX-AP0413", Type = "No fabric breakage: Garment - weft - Rugby/Football 160N", Criteria = 160 },
                new FGPT() { TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No fabric breakage: Garment - warp - Rugby/Football 160N", Criteria = 160 },
                new FGPT() { TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No seam breakage: Garment - weft -Rugby/Football 160N", Criteria = 160 },
                new FGPT() { TestDetail = "pass/fail", TestUnit = "N", TestName = "PHX-AP0413", Type = "No seam breakage: Garment - warp - Rugby/Football 160N", Criteria = 160 },
            };

            if (isLining)
            {
                if (isTop)
                {
                    defaultFGPTList.AddRange(lining_Upper);
                    defaultFGPTList.AddRange(upperOnly);
                }

                if (isBottom)
                {
                    defaultFGPTList.AddRange(lining_Lower);
                    defaultFGPTList.AddRange(lowerOnly);
                }

                if (isTop_Bottom)
                {
                    defaultFGPTList.AddRange(lining_Full);
                    defaultFGPTList.AddRange(fullBody);
                }
            }
            else
            {
                if (isTop)
                {
                    defaultFGPTList.AddRange(upperOnly);
                }

                if (isBottom)
                {
                    defaultFGPTList.AddRange(lowerOnly);
                }

                if (isTop_Bottom)
                {
                    defaultFGPTList.AddRange(fullBody);
                }
            }

            if (isRugbyFootBall)
            {
                foreach (var fGPT in rugby_FootBall)
                {
                    fGPT.Location = location;
                }

                defaultFGPTList.AddRange(rugby_FootBall);
            }

            defaultFGPTList.Add(new FGPT()
            {
                Location = string.Empty,
                Type = "odour: Garment",
                TestDetail = "pass/fail",
                TestUnit = "Pass/Fail",
                TestName = "PHX-AP0451",
            });

            return defaultFGPTList.OrderBy(o => o.Type).ToList();
        }
    }
}