using System;
using System.Data;
using System.Windows.Forms;
using Sci.Data;
using Ict;
using Ict.Win;
using System.Collections.Generic;

namespace Sci.Production.PublicPrg
{
    public static partial class Prgs
    {
        #region Query QA Inspection header function QA 的表頭抓取共用程式

        /// <summary>
        /// 可取出Style,Season,Brand,Cutinline,earliestSciDelivery,MtlLeadTime from Orders,System
        /// </summary>
        /// <param name="poid"></param>
        /// <returns>DataRow</returns>
        public static DualResult QueryQaInspectionHeader(string poid, out DataRow orderDr)
        {
            DataTable queryTb;
            string query = string.Format("Select distinct a.styleid, a.seasonid,a.brandid,a.cutinline,a.category from Orders a WITH (NOLOCK) Where a.poid ='{0}'", poid);
            DualResult dResult = DBProxy.Current.Select(null, query, out queryTb);
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
        /// <param name="poid"></param>
        /// <returns>DataRow</returns>
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

            DateTime? TargetSciDel;
            double mtlLeadT = Convert.ToDouble(MyUtility.GetValue.Lookup("Select MtlLeadTime from System WITH (NOLOCK) ", null));
            if (sciDelv == null)
            {
                return null;
            }

            if (MyUtility.Check.Empty(mtlLeadT))
            {
                TargetSciDel = sciDelv;
            }
            else
            {
                TargetSciDel = ((DateTime)sciDelv).AddDays(Convert.ToDouble(mtlLeadT));
            }

            if (cutinline < TargetSciDel)
            {
                return cutinline;
            }
            else
            {
                return TargetSciDel;
            }
        }
        #endregion;
        #region 判斷Physical OverallResult, Status

        /// <summary>
        /// 判斷並回寫Physical OverallResult, Status string[0]=Result, string[1]=status
        /// </summary>
        /// <param name ="ID"></param>
        /// <returns></returns>
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
        /// <param name ="ID"></param>
        /// <returns></returns>
        public static string[] GetOverallResult_Lab(object fir_id)
        {
            DataRow maindr;
            MyUtility.Check.Seek(string.Format("Select * from FIR_Laboratory WITH (NOLOCK) Where id={0}", fir_id), out maindr);
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
        public class cellResult : DataGridViewGeneratorTextColumnSettings
        {
            public static DataGridViewGeneratorTextColumnSettings GetGridCell()
            {
                cellResult Result = new cellResult();
                Result.CellMouseDoubleClick += (s, e) =>
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
                return Result;
            }
        }

        /// <summary>
        /// GarmentTest_Detail_FGWT 和 SampleGarmentTest_Detail的欄位物件
        /// </summary>
        public class FGWT
        {
            public string Location { get; set; }

            public string Type { get; set; }

            public string Scale { get; set; }

            public string TestDetail { get; set; }

            public double Criteria { get; set; }
        }

        /// <summary>
        /// 取得預設FGWT
        /// </summary>
        /// <param name="isTop">是否TOP</param>
        /// <param name="isBottom">>是否Bottom</param>
        /// <param name="isTop_Bottom">>是否TOP & Bottom</param>
        /// <param name="isAll">>是否All</param>
        /// <returns>預設清單</returns>
        public static List<FGWT> GetDefaultFGWT(bool isTop, bool isBottom, bool isTop_Bottom, string mtlTypeID, bool isAll = true)
        {
            List<FGWT> defaultFGWTList = new List<FGWT>();

            // 若只有B則寫入Bottom的項目+ALL的項目，若只有T則寫入TOP的項目+ALL的項目，若有B和T則寫入Top+ Bottom的項目+ALL的項目
            // 每種Type要寫入哪一種標準，請見ISP20201331的補充說明

            // Criteria欄位視作百分比，knits <= 5% woven <= 2 %
            double percent_five_two = 0;

            switch (mtlTypeID)
            {
                case "KNIT":
                    percent_five_two = 5;
                    break;
                case "WOVEN":
                    percent_five_two = 2;
                    break;
                default:
                    break;
            }

            // Criteria欄位視作百分比，knits <= 3% woven <= 2 %
            double percent_three_two = 0;

            switch (mtlTypeID)
            {
                case "KNIT":
                    percent_three_two = 3;
                    break;
                case "WOVEN":
                    percent_three_two = 2;
                    break;
                default:
                    break;
            }

            // Criteria欄位視作公分
            double cm = 5;

            /*除了以上兩種情況，都以Scale欄位作為標準，因此不寫入Criteria欄位視作百分比欄位*/

            List<FGWT> top = new List<FGWT>()
            {
                new FGWT() { Location = "Top", TestDetail = "%", Type = "dimensional change: jacket-like garment a) length of necktape", Criteria = percent_five_two },
                new FGWT() { Location = "Top", TestDetail = "%", Type = "dimensional change: jacket-like garment b) length armhole to bottom hem", Criteria = percent_five_two },
                new FGWT() { Location = "Top", TestDetail = "%", Type = "dimensional change: jacket-like garment c) length of front", Criteria = percent_five_two },
                new FGWT() { Location = "Top", TestDetail = "%", Type = "dimensional change: jacket-like garment d) length of centre back", Criteria = percent_five_two },
                new FGWT() { Location = "Top", TestDetail = "%", Type = "dimensional change: jacket-like garment e) length of underarm", Criteria = percent_five_two },
                new FGWT() { Location = "Top", TestDetail = "%", Type = "dimensional change: jacket-like garment f) width across back", Criteria = percent_five_two },
                new FGWT() { Location = "Top", TestDetail = "%", Type = "dimensional change: jacket-like garment g) width below centre back neck (average)", Criteria = percent_five_two },
                new FGWT() { Location = "Top", TestDetail = "%", Type = "dimensional change: jacket-like garment h) width of sleeve", Criteria = percent_five_two },
                new FGWT() { Location = "Top", TestDetail = "%", Type = "dimensional change: jacket-like garment i) width of sleeve botttom/cuff bottom", Criteria = percent_five_two },
                new FGWT() { Location = "Top", TestDetail = "%", Type = "dimensional change: lining of jacket-like garment a) length of necktape", Criteria = percent_five_two },
                new FGWT() { Location = "Top", TestDetail = "%", Type = "dimensional change: lining of jacket-like garment b) length armhole to bottom hem", Criteria = percent_five_two },
                new FGWT() { Location = "Top", TestDetail = "%", Type = "dimensional change: lining of jacket-like garment c) length of front", Criteria = percent_five_two },
                new FGWT() { Location = "Top", TestDetail = "%", Type = "dimensional change: lining of jacket-like garment d) length of centre back", Criteria = percent_five_two },
                new FGWT() { Location = "Top", TestDetail = "%", Type = "dimensional change: lining of jacket-like garment e) length of underarm", Criteria = percent_five_two },
                new FGWT() { Location = "Top", TestDetail = "%", Type = "dimensional change: lining of jacket-like garment f) width across back", Criteria = percent_five_two },
                new FGWT() { Location = "Top", TestDetail = "%", Type = "dimensional change: lining of jacket-like garment g) width below centre back neck(average)", Criteria = percent_five_two },
                new FGWT() { Location = "Top", TestDetail = "%", Type = "dimensional change: lining of jacket-like garment h) width of sleeve", Criteria = percent_five_two },
                new FGWT() { Location = "Top", TestDetail = "%", Type = "dimensional change: lining of jacket-like garment i) width of sleeve botttom/ cuff bottom", Criteria = percent_five_two },
            };

            List<FGWT> bottom = new List<FGWT>()
            {
                new FGWT() { Location = "Bottom", TestDetail = "%", Type = "dimensional change: trouser-like garment a) length of front leg", Criteria = percent_three_two },
                new FGWT() { Location = "Bottom", TestDetail = "%", Type = "dimensional change: trouser-like garment b) length of back leg", Criteria = percent_three_two },
                new FGWT() { Location = "Bottom", TestDetail = "%", Type = "dimensional change: trouser-like garment c) length of inside leg", Criteria = percent_three_two },
                new FGWT() { Location = "Bottom", TestDetail = "%", Type = "dimensional change: trouser-like garment d) width at waist", Criteria = percent_three_two },
                new FGWT() { Location = "Bottom", TestDetail = "%", Type = "dimensional change: trouser-like garment e) width at bottom of leg", Criteria = percent_three_two },
                new FGWT() { Location = "Bottom", TestDetail = "%", Type = "dimensional change: trouser-like garment f) width of leg halfway", Criteria = percent_three_two },
                new FGWT() { Location = "Bottom", TestDetail = "%", Type = "dimensional change: trouser-like garment g) width of top of leg", Criteria = percent_three_two },
                new FGWT() { Location = "Bottom", TestDetail = "%", Type = "dimensional change: lining of trouser-like garment a) length of front leg", Criteria = percent_three_two },
                new FGWT() { Location = "Bottom", TestDetail = "%", Type = "dimensional change: lining of trouser-like garment b) length of back leg", Criteria = percent_three_two },
                new FGWT() { Location = "Bottom", TestDetail = "%", Type = "dimensional change: lining of trouser-like garment c) length of inside leg", Criteria = percent_three_two },
                new FGWT() { Location = "Bottom", TestDetail = "%", Type = "dimensional change: lining of trouser-like garment d) width at waist", Criteria = percent_three_two},
                new FGWT() { Location = "Bottom", TestDetail = "%", Type = "dimensional change: lining of trouser-like garment e) width at bottom of leg", Criteria = percent_three_two },
                new FGWT() { Location = "Bottom", TestDetail = "%", Type = "dimensional change: lining of trouser-like garment f) width of leg halfway", Criteria = percent_three_two },
                new FGWT() { Location = "Bottom", TestDetail = "%", Type = "dimensional change: lining of trouser-like garment g) width of top of leg", Criteria = percent_three_two },
                new FGWT() { Location = "Bottom", TestDetail = "%", Type = "dimensional change: skirt a) length from waist to bottom hem", Criteria = percent_three_two },
                new FGWT() { Location = "Bottom", TestDetail = "%", Type = "dimensional change: skirt b) width at waistband", Criteria = percent_three_two },
                new FGWT() { Location = "Bottom", TestDetail = "%", Type = "dimensional change: skirt c) width below top/bottom edge of waistband (average)" , Criteria = percent_three_two},
                new FGWT() { Location = "Bottom", TestDetail = "%", Type = "dimensional change: lining of skirt a) length from waist to bottom hem", Criteria = percent_three_two },
                new FGWT() { Location = "Bottom", TestDetail = "%", Type = "dimensional change: lining of skirt b) width at waistband" , Criteria = percent_three_two},
                new FGWT() { Location = "Bottom", TestDetail = "%", Type = "dimensional change: lining of skirt c) width below top/bottom edge of waistband (average)", Criteria = percent_three_two },
            };

            List<FGWT> top_bottom = new List<FGWT>()
            {
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: full body wear a) length of neckktape", Criteria = percent_five_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: full body wear b) length armhole to bottom hem", Criteria = percent_five_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: full body wear c) length of centre front", Criteria = percent_five_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: full body wear d) length of centre back", Criteria = percent_five_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: full body wear e) length of underarm", Criteria = percent_five_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: full body wear f) width across back", Criteria = percent_five_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: full body wear g) width below centre back neck (average)", Criteria = percent_five_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: full body wear h) width of sleeve", Criteria = percent_five_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: full body wear i) width of sleeve botttom/cuff bottom", Criteria = percent_five_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: full body wear j) length of front leg", Criteria = percent_three_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: full body wear k) length of back leg", Criteria = percent_three_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: full body wear l) length of inside leg", Criteria = percent_three_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: full body wear m) width at waist", Criteria = percent_three_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: full body wear n) width at bottom of leg", Criteria = percent_three_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: full body wear o) width of leg halfway", Criteria = percent_three_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: full body wear p) width of top of leg", Criteria = percent_three_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: lining of full body wear a) length of neckktape", Criteria = percent_five_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: lining of full body wear b) length armhole to bottom hem", Criteria = percent_five_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: lining of full body wear c) length of centre front", Criteria = percent_five_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: lining of full body wear d) length of centre back", Criteria = percent_five_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: lining of full body wear e) length of underarm", Criteria = percent_five_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: lining of full body wear f) width across back", Criteria = percent_five_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: lining of full body wear g) width below centre back neck (average)", Criteria = percent_five_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: lining of full body wear h) width of sleeve", Criteria = percent_five_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: lining of full body wear i) width of sleeve botttom/cuff bottom", Criteria = percent_five_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: lining of full body wear j) length of front leg", Criteria = percent_three_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: lining of full body wear k) length of back leg", Criteria = percent_three_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: lining of full body wear l) length of inside leg", Criteria = percent_three_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: lining of full body wear m) width at waist", Criteria = percent_three_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: lining of full body wear n) width at bottom of leg", Criteria = percent_three_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: lining of full body wear o) width of leg halfway", Criteria = percent_three_two },
                new FGWT() { Location = "Top+Bottom", TestDetail = "%", Type = "dimensional change: lining of full body wear p) width of top of leg", Criteria = percent_three_two },
            };

            // All項目最後會寫入空白
            List<FGWT> all = new List<FGWT>()
            {
                new FGWT() { Location = string.Empty, TestDetail = "%", Type = "dimensional change: flat made-up textile articles a) overall length" },
                new FGWT() { Location = string.Empty, TestDetail = "%", Type = "dimensional change: flat made-up textile articles b) overall width" },
                new FGWT() { Location = string.Empty, TestDetail = "%", Type = "spirality: Garment - in percentage (average)", Criteria = percent_three_two },
                new FGWT() { Location = string.Empty, TestDetail = "cm", Type = "spirality: Garment - hem opening in cm", Criteria = cm },
                new FGWT() { Location = string.Empty, TestDetail = "grade", Type = "appearance after laundering: Garment - colour change", Scale = string.Empty },
                new FGWT() { Location = string.Empty, TestDetail = "grade", Type = "appearance after laundering: Garment - staining", Scale = string.Empty },
                new FGWT() { Location = string.Empty, TestDetail = "pass/fail",Type = "appearance after laundering: Garment - physical changes", Scale = string.Empty },
            };

            if (isTop)
            {
                defaultFGWTList.AddRange(top);
            }

            if (isBottom)
            {
                defaultFGWTList.AddRange(bottom);
            }

            if (isTop_Bottom)
            {
                defaultFGWTList.AddRange(top_bottom);
            }

            if (isAll)
            {
                defaultFGWTList.AddRange(all);
            }

            return defaultFGWTList;
        }
    }
}